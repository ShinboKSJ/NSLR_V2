using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NSLR_ObservationControl.Module
{
    /// <summary>
    /// 60Hz 레이저 신호 실시간 Range Combine 처리
    /// 데이터 처리와 UI 업데이트를 완전히 분리하여 실시간성 보장
    /// </summary>
    public class RealTimeRangeCombiner : IDisposable
    {
        #region 상수 정의
        private const int LASER_FREQUENCY_HZ = 60;
        private const double FRAME_INTERVAL_SEC = 1.0 / LASER_FREQUENCY_HZ; // ~16.67ms
        private const double TIME_TOLERANCE_SEC = 5e-5; // 50μs 허용 오차
        private const double CLEANUP_MARGIN_SEC = 5e-1; // 500ms 정리 마진
        private const int RING_BUFFER_SIZE = 8192; // 2의 거듭제곱 (비트 마스킹 최적화)
        private const int UI_UPDATE_INTERVAL_MS = 50; // UI는 20Hz로 업데이트 (60Hz 대신)
        private const int BIN_SIZE_PICOSEC = 10; // 히스토그램 빈 크기
        #endregion

        #region 링버퍼 구조체 (Lock-Free 구현)
        /// <summary>
        /// 고성능 링버퍼 - Lock-Free 알고리즘 사용
        /// </summary>
        private class LockFreeRingBuffer<T>
        {
            private readonly T[] _buffer;
            private readonly int _mask;
            private long _writeIndex = 0;
            private long _readIndex = 0;

            public LockFreeRingBuffer(int size)
            {
                if ((size & (size - 1)) != 0)
                    throw new ArgumentException("Size must be a power of 2");
                
                _buffer = new T[size];
                _mask = size - 1;
            }

            public bool TryEnqueue(T item)
            {
                long currentWrite = Interlocked.Read(ref _writeIndex);
                long currentRead = Interlocked.Read(ref _readIndex);
                
                // 버퍼 가득 찬 경우
                if (currentWrite - currentRead >= _buffer.Length)
                    return false;

                int index = (int)(currentWrite & _mask);
                _buffer[index] = item;
                Interlocked.Increment(ref _writeIndex);
                return true;
            }

            public bool TryDequeue(out T item)
            {
                long currentWrite = Interlocked.Read(ref _writeIndex);
                long currentRead = Interlocked.Read(ref _readIndex);

                if (currentRead >= currentWrite)
                {
                    item = default(T);
                    return false;
                }

                int index = (int)(currentRead & _mask);
                item = _buffer[index];
                Interlocked.Increment(ref _readIndex);
                return true;
            }

            public int Count
            {
                get
                {
                    long write = Interlocked.Read(ref _writeIndex);
                    long read = Interlocked.Read(ref _readIndex);
                    return (int)(write - read);
                }
            }
        }
        #endregion

        #region Lookup Table 클래스
        /// <summary>
        /// RGG에서 전송받은 시간별 예측 ToF Lookup Table
        /// UTC time of day ms 기반 빠른 검색 지원
        /// </summary>
        public class ToFLookupTable
        {
            private readonly SortedList<long, double> _lookupData; // UTC ms -> ToF (초)
            private readonly ReaderWriterLockSlim _lock;
            private long _minUtcMs = long.MaxValue;
            private long _maxUtcMs = long.MinValue;
            
            public ToFLookupTable()
            {
                _lookupData = new SortedList<long, double>();
                _lock = new ReaderWriterLockSlim();
            }

            /// <summary>
            /// Lookup table 데이터 설정 (RGG에서 전송받은 데이터)
            /// </summary>
            /// <param name="utcMsList">UTC time of day ms 리스트</param>
            /// <param name="tofSecondsList">예측 ToF (초) 리스트</param>
            public void SetLookupData(List<long> utcMsList, List<double> tofSecondsList)
            {
                if (utcMsList == null || tofSecondsList == null)
                    throw new ArgumentNullException();
                
                if (utcMsList.Count != tofSecondsList.Count)
                    throw new ArgumentException("UTC and ToF lists must have same length");

                _lock.EnterWriteLock();
                try
                {
                    _lookupData.Clear();
                    
                    for (int i = 0; i < utcMsList.Count; i++)
                    {
                        long utcMs = utcMsList[i];
                        double tofSec = tofSecondsList[i];
                        
                        _lookupData[utcMs] = tofSec;
                        
                        if (utcMs < _minUtcMs) _minUtcMs = utcMs;
                        if (utcMs > _maxUtcMs) _maxUtcMs = utcMs;
                    }
                    
                    Console.WriteLine($"[LookupTable] Loaded {_lookupData.Count} entries, " +
                                    $"Range: {_minUtcMs} ~ {_maxUtcMs} ms");
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }

            /// <summary>
            /// 특정 UTC 시간의 예측 ToF 조회 (선형 보간)
            /// </summary>
            public bool TryGetToF(long utcMs, out double tofSeconds)
            {
                _lock.EnterReadLock();
                try
                {
                    if (_lookupData.Count == 0)
                    {
                        tofSeconds = 0;
                        return false;
                    }

                    // 범위 체크
                    if (utcMs < _minUtcMs || utcMs > _maxUtcMs)
                    {
                        // 범위 밖이면 가장 가까운 값 반환
                        if (utcMs < _minUtcMs)
                            tofSeconds = _lookupData.Values[0];
                        else
                            tofSeconds = _lookupData.Values[_lookupData.Count - 1];
                        
                        return true; // 경고는 하되 값은 반환
                    }

                    // 정확히 일치하는 경우
                    if (_lookupData.TryGetValue(utcMs, out tofSeconds))
                        return true;

                    // 선형 보간
                    int index = _lookupData.Keys.ToList().BinarySearch(utcMs);
                    if (index < 0)
                    {
                        index = ~index; // BinarySearch는 음수로 삽입 위치 반환
                        
                        if (index == 0)
                        {
                            tofSeconds = _lookupData.Values[0];
                        }
                        else if (index >= _lookupData.Count)
                        {
                            tofSeconds = _lookupData.Values[_lookupData.Count - 1];
                        }
                        else
                        {
                            // 선형 보간
                            long utc1 = _lookupData.Keys[index - 1];
                            long utc2 = _lookupData.Keys[index];
                            double tof1 = _lookupData.Values[index - 1];
                            double tof2 = _lookupData.Values[index];
                            
                            double ratio = (double)(utcMs - utc1) / (utc2 - utc1);
                            tofSeconds = tof1 + ratio * (tof2 - tof1);
                        }
                    }
                    
                    return true;
                }
                finally
                {
                    _lock.ExitReadLock();
                }
            }

            /// <summary>
            /// Event Timer 시간(초)을 UTC ms로 변환하여 ToF 조회
            /// </summary>
            public bool TryGetToFFromEventTime(double eventTimeSeconds, out double tofSeconds)
            {
                // Event Timer 시간(초)을 UTC time of day ms로 변환
                // Event Timer는 하루 시작부터의 초 단위 시간을 사용한다고 가정
                long utcMs = (long)(eventTimeSeconds * 1000.0);
                return TryGetToF(utcMs, out tofSeconds);
            }

            public int Count => _lookupData.Count;
            
            public void Clear()
            {
                _lock.EnterWriteLock();
                try
                {
                    _lookupData.Clear();
                    _minUtcMs = long.MaxValue;
                    _maxUtcMs = long.MinValue;
                }
                finally
                {
                    _lock.ExitWriteLock();
                }
            }
        }
        #endregion

        #region 데이터 구조체
        /// <summary>
        /// 타임스탬프가 포함된 이벤트 타이머 데이터
        /// </summary>
        private struct TimestampedEvent
        {
            public double Timestamp;      // 이벤트 발생 시간 (초) - UTC time of day
            public bool IsStartEvent;     // true: Start, false: End
            public long SystemTickCount;  // 시스템 수신 시각 (동기화용)

            public TimestampedEvent(double timestamp, bool isStart)
            {
                Timestamp = timestamp;
                IsStartEvent = isStart;
                SystemTickCount = Stopwatch.GetTimestamp();
            }
        }

        /// <summary>
        /// 페어링된 레이저 측정 데이터
        /// </summary>
        public struct LaserMeasurement
        {
            public double StartTime;      // 레이저 발사 시간 (초)
            public double EndTime;        // 수신 시간 (초)
            public double ToF;            // Time of Flight (초)
            public double OminusC;        // O-C 값 (밀리초)
            public double AzimuthDeg;     // 방위각
            public double ElevationDeg;   // 고도각
            public long SequenceId;       // 시퀀스 번호
        }
        #endregion

        #region 필드
        // 링버퍼 (Lock-Free)
        private readonly LockFreeRingBuffer<TimestampedEvent> _eventBuffer;
        
        // 처리 대기 큐 (Start/End 분리)
        private readonly ConcurrentQueue<TimestampedEvent> _startQueue;
        private readonly ConcurrentQueue<(TimestampedEvent endEvent, double expectedToF)> _endQueue;
        
        // 결과 데이터
        private readonly ConcurrentQueue<LaserMeasurement> _measurementQueue;
        private readonly ConcurrentDictionary<int, int> _histogram; // O-C 히스토그램
        
        // 스레드 제어
        private Thread _processingThread;
        private CancellationTokenSource _cts;
        private readonly ManualResetEventSlim _dataAvailableEvent;
        
        // 통계
        private long _totalReceived = 0;
        private long _totalPaired = 0;
        private long _totalDropped = 0;
        private long _lookupMisses = 0; // Lookup table에서 ToF를 찾지 못한 횟수
        private volatile double _processingLatencyMs = 0;
        
        // RGG Lookup Table
        private readonly ToFLookupTable _lookupTable;
        
        // 호환성을 위한 ToF 계산 함수 (옵션)
        private Func<double, double> _fallbackTofCalculator;
        
        // UI 업데이트 콜백
        public event Action<List<LaserMeasurement>> OnMeasurementBatch;
        public event Action<Dictionary<int, int>> OnHistogramUpdate;
        public event Action<ProcessingStatistics> OnStatisticsUpdate;
        #endregion

        #region 통계 구조체
        public struct ProcessingStatistics
        {
            public long TotalReceived;
            public long TotalPaired;
            public long TotalDropped;
            public long LookupMisses;
            public double PairingRate;
            public double ProcessingLatencyMs;
            public int BufferUtilization;
            public int QueueDepth;
            public int LookupTableSize;
        }
        #endregion

        #region 생성자 및 초기화
        /// <summary>
        /// 기본 생성자 (Lookup Table 모드)
        /// </summary>
        public RealTimeRangeCombiner()
        {
            _eventBuffer = new LockFreeRingBuffer<TimestampedEvent>(RING_BUFFER_SIZE);
            _startQueue = new ConcurrentQueue<TimestampedEvent>();
            _endQueue = new ConcurrentQueue<(TimestampedEvent, double)>();
            _measurementQueue = new ConcurrentQueue<LaserMeasurement>();
            _histogram = new ConcurrentDictionary<int, int>();
            _dataAvailableEvent = new ManualResetEventSlim(false);
            _lookupTable = new ToFLookupTable();
        }

        /// <summary>
        /// 호환성을 위한 생성자 (Fallback 함수 제공)
        /// </summary>
        public RealTimeRangeCombiner(Func<double, double> fallbackTofCalculator) : this()
        {
            _fallbackTofCalculator = fallbackTofCalculator;
        }

        /// <summary>
        /// RGG Lookup Table 설정 (운영 시작 시 호출)
        /// </summary>
        /// <param name="utcMsList">UTC time of day ms 리스트</param>
        /// <param name="tofSecondsList">예측 ToF (초) 리스트</param>
        public void SetLookupTable(List<long> utcMsList, List<double> tofSecondsList)
        {
            _lookupTable.SetLookupData(utcMsList, tofSecondsList);
            Console.WriteLine($"[RangeCombiner] Lookup table configured with {_lookupTable.Count} entries");
        }

        /// <summary>
        /// Lookup Table 접근자 (외부에서 직접 설정 가능)
        /// </summary>
        public ToFLookupTable LookupTable => _lookupTable;

        /// <summary>
        /// 처리 시작
        /// </summary>
        public void Start()
        {
            if (_processingThread != null && _processingThread.IsAlive)
                return;

            _cts = new CancellationTokenSource();
            _processingThread = new Thread(ProcessingLoop)
            {
                Name = "RangeCombiner",
                Priority = ThreadPriority.Highest, // 실시간 처리를 위해 최고 우선순위
                IsBackground = true
            };
            _processingThread.Start();

            // UI 업데이트 스레드 시작
            Task.Run(() => UIUpdateLoop(_cts.Token));
        }

        /// <summary>
        /// 처리 중지
        /// </summary>
        public void Stop()
        {
            _cts?.Cancel();
            _dataAvailableEvent.Set();
            _processingThread?.Join(1000);
        }
        #endregion

        #region 데이터 입력
        /// <summary>
        /// 이벤트 타이머 데이터 입력 (네트워크 수신 스레드에서 호출)
        /// </summary>
        /// <param name="irValue">IR 값 (음수: Start, 양수: End)</param>
        /// <param name="timeValue">Time 값</param>
        public void EnqueueEvent(int irValue, int timeValue)
        {
            // 시간 계산 (나노초 -> 초)
            double timestamp = Math.Abs(irValue) * 327680.0e-9 + timeValue * 1e-12;
            bool isStart = irValue < 0;

            var evt = new TimestampedEvent(timestamp, isStart);

            // 링버퍼에 추가 (실패시 드롭)
            if (!_eventBuffer.TryEnqueue(evt))
            {
                Interlocked.Increment(ref _totalDropped);
                Console.WriteLine($"[WARNING] Event buffer full! Data dropped. Total dropped: {_totalDropped}");
            }
            else
            {
                Interlocked.Increment(ref _totalReceived);
                _dataAvailableEvent.Set(); // 처리 스레드 깨우기
            }
        }

        /// <summary>
        /// 원시 바이트 배열 입력 (기존 코드와 호환)
        /// </summary>
        public void EnqueueRawData(byte[] buffer, int length)
        {
            for (int i = 0; i < length; i += 8)
            {
                if (i + 7 >= length) break;

                byte[] irBytes = new byte[4];
                byte[] timeBytes = new byte[4];
                Array.Copy(buffer, i, irBytes, 0, 4);
                Array.Copy(buffer, i + 4, timeBytes, 0, 4);

                if (!BitConverter.IsLittleEndian)
                {
                    Array.Reverse(irBytes);
                    Array.Reverse(timeBytes);
                }

                int ir = BitConverter.ToInt32(irBytes, 0);
                int time = BitConverter.ToInt32(timeBytes, 0);

                EnqueueEvent(ir, time);
            }
        }
        #endregion

        #region 핵심 처리 루프
        /// <summary>
        /// 메인 처리 루프 (별도 고우선순위 스레드)
        /// </summary>
        private void ProcessingLoop()
        {
            var sw = Stopwatch.StartNew();
            
            while (!_cts.Token.IsCancellationRequested)
            {
                // 데이터가 있을 때까지 대기 (CPU 사용률 최소화)
                if (_eventBuffer.Count == 0)
                {
                    _dataAvailableEvent.Wait(10, _cts.Token);
                    _dataAvailableEvent.Reset();
                    continue;
                }

                sw.Restart();

                // 링버퍼에서 이벤트 추출
                while (_eventBuffer.TryDequeue(out var evt))
                {
                    if (evt.IsStartEvent)
                    {
                        _startQueue.Enqueue(evt);
                    }
                    else
                    {
                        // Lookup Table에서 예측 ToF 조회
                        double expectedToF;
                        if (!_lookupTable.TryGetToFFromEventTime(evt.Timestamp, out expectedToF))
                        {
                            // Lookup table에서 찾지 못한 경우
                            Interlocked.Increment(ref _lookupMisses);
                            
                            // Fallback 함수 사용 (있는 경우)
                            if (_fallbackTofCalculator != null)
                            {
                                expectedToF = _fallbackTofCalculator(evt.Timestamp);
                            }
                            else
                            {
                                // Fallback도 없으면 스킵
                                Console.WriteLine($"[WARNING] No ToF found for time {evt.Timestamp:F6}s, event dropped");
                                Interlocked.Increment(ref _totalDropped);
                                continue;
                            }
                        }
                        
                        _endQueue.Enqueue((evt, expectedToF));
                    }
                }

                // Start-End 페어링 수행
                PerformPairing();

                // 처리 지연 시간 기록
                _processingLatencyMs = sw.Elapsed.TotalMilliseconds;
            }
        }

        /// <summary>
        /// Start와 End 이벤트 페어링
        /// </summary>
        private void PerformPairing()
        {
            while (_endQueue.TryDequeue(out var endPair))
            {
                var endEvent = endPair.Item1;
                var expectedToF = endPair.Item2;

                // 예상 Start 시간 범위 계산
                double expectedStartLower = endEvent.Timestamp - expectedToF - GlobalVariables.SystemDelay - TIME_TOLERANCE_SEC;
                double expectedStartUpper = endEvent.Timestamp - expectedToF - GlobalVariables.SystemDelay + TIME_TOLERANCE_SEC;

                bool paired = false;

                // Start 큐에서 매칭되는 이벤트 찾기
                var tempStarts = new List<TimestampedEvent>();
                
                while (_startQueue.TryDequeue(out var startEvent))
                {
                    // 시간 범위 내에 있는 경우
                    if (startEvent.Timestamp >= expectedStartLower && startEvent.Timestamp <= expectedStartUpper)
                    {
                        // 페어링 성공
                        CreateMeasurement(startEvent, endEvent, expectedToF);
                        paired = true;

                        // 사용하지 않은 Start 이벤트들 다시 큐에 추가
                        foreach (var temp in tempStarts)
                            _startQueue.Enqueue(temp);
                        
                        break;
                    }
                    else if (startEvent.Timestamp < expectedStartLower)
                    {
                        // 너무 오래된 Start - 버림
                        Interlocked.Increment(ref _totalDropped);
                    }
                    else if (startEvent.Timestamp > expectedStartUpper + CLEANUP_MARGIN_SEC)
                    {
                        // 미래의 Start - 큐에 다시 추가
                        tempStarts.Add(startEvent);
                    }
                    else
                    {
                        // 범위 밖이지만 아직 유효할 수 있음
                        tempStarts.Add(startEvent);
                    }
                }

                if (!paired)
                {
                    // 페어링 실패
                    Interlocked.Increment(ref _totalDropped);
                    
                    // 임시 보관했던 Start 이벤트들 복원
                    foreach (var temp in tempStarts)
                        _startQueue.Enqueue(temp);
                }
            }

            // 오래된 Start 이벤트 정리 (메모리 누수 방지)
            CleanupOldStartEvents();
        }

        /// <summary>
        /// 측정 데이터 생성
        /// </summary>
        private void CreateMeasurement(TimestampedEvent startEvent, TimestampedEvent endEvent, double expectedToF)
        {
            double actualToF = endEvent.Timestamp - startEvent.Timestamp;
            double oMinusC = (actualToF - expectedToF - GlobalVariables.SystemDelay) * 1e3; // ms로 변환

            var measurement = new LaserMeasurement
            {
                StartTime = startEvent.Timestamp,
                EndTime = endEvent.Timestamp,
                ToF = actualToF,
                OminusC = oMinusC,
                AzimuthDeg = Observation_TMS.TMSAZPositon,
                ElevationDeg = Observation_TMS.TMSELPosition,
                SequenceId = Interlocked.Increment(ref _totalPaired)
            };

            _measurementQueue.Enqueue(measurement);

            // 히스토그램 업데이트 (빈 단위로)
            int binKey = (int)(Math.Round(oMinusC / BIN_SIZE_PICOSEC) * BIN_SIZE_PICOSEC);
            _histogram.AddOrUpdate(binKey, 1, (key, oldValue) => oldValue + 1);
        }

        /// <summary>
        /// 오래된 Start 이벤트 정리
        /// </summary>
        private void CleanupOldStartEvents()
        {
            double currentTime = _endQueue.Count > 0 ? 
                _endQueue.ToArray().Last().Item1.Timestamp : 
                DateTime.UtcNow.TimeOfDay.TotalSeconds;
            
            double threshold = currentTime - CLEANUP_MARGIN_SEC;
            var tempList = new List<TimestampedEvent>();

            while (_startQueue.TryDequeue(out var evt))
            {
                if (evt.Timestamp >= threshold)
                {
                    tempList.Add(evt);
                }
                else
                {
                    Interlocked.Increment(ref _totalDropped);
                }
            }

            // 유효한 이벤트들 다시 큐에 추가
            foreach (var evt in tempList)
                _startQueue.Enqueue(evt);
        }
        #endregion

        #region UI 업데이트 루프
        /// <summary>
        /// UI 업데이트 루프 (별도 저우선순위 스레드, 20Hz)
        /// </summary>
        private async Task UIUpdateLoop(CancellationToken ct)
        {
            var sw = Stopwatch.StartNew();
            
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    var measurements = new List<LaserMeasurement>();
                    
                    // 측정 데이터 배치 수집 (최대 100개)
                    for (int i = 0; i < 100 && _measurementQueue.TryDequeue(out var m); i++)
                    {
                        measurements.Add(m);
                    }

                    if (measurements.Count > 0)
                    {
                        OnMeasurementBatch?.Invoke(measurements);
                    }

                    // 히스토그램 업데이트
                    if (_histogram.Count > 0)
                    {
                        OnHistogramUpdate?.Invoke(new Dictionary<int, int>(_histogram));
                    }

                    // 통계 업데이트
                    var stats = GetStatistics();
                    OnStatisticsUpdate?.Invoke(stats);

                    // 다음 업데이트까지 대기 (20Hz = 50ms)
                    await Task.Delay(UI_UPDATE_INTERVAL_MS, ct);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] UI update error: {ex.Message}");
                }
            }
        }
        #endregion

        #region 통계 및 모니터링
        /// <summary>
        /// 현재 처리 통계 반환
        /// </summary>
        public ProcessingStatistics GetStatistics()
        {
            long received = Interlocked.Read(ref _totalReceived);
            long paired = Interlocked.Read(ref _totalPaired);
            long dropped = Interlocked.Read(ref _totalDropped);
            long lookupMisses = Interlocked.Read(ref _lookupMisses);

            return new ProcessingStatistics
            {
                TotalReceived = received,
                TotalPaired = paired,
                TotalDropped = dropped,
                LookupMisses = lookupMisses,
                PairingRate = received > 0 ? (paired * 100.0 / received) : 0,
                ProcessingLatencyMs = _processingLatencyMs,
                BufferUtilization = (_eventBuffer.Count * 100) / RING_BUFFER_SIZE,
                QueueDepth = _startQueue.Count + _endQueue.Count,
                LookupTableSize = _lookupTable.Count
            };
        }

        /// <summary>
        /// 통계 초기화
        /// </summary>
        public void ResetStatistics()
        {
            Interlocked.Exchange(ref _totalReceived, 0);
            Interlocked.Exchange(ref _totalPaired, 0);
            Interlocked.Exchange(ref _totalDropped, 0);
            Interlocked.Exchange(ref _lookupMisses, 0);
            _histogram.Clear();
        }

        /// <summary>
        /// 측정 데이터 가져오기 (CRD 파일 생성용)
        /// </summary>
        public List<LaserMeasurement> GetAllMeasurements()
        {
            var result = new List<LaserMeasurement>();
            while (_measurementQueue.TryDequeue(out var m))
            {
                result.Add(m);
            }
            return result;
        }
        #endregion

        #region IDisposable 구현
        public void Dispose()
        {
            Stop();
            _cts?.Dispose();
            _dataAvailableEvent?.Dispose();
        }
        #endregion
    }
}
