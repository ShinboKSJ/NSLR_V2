using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NSLR_ObservationControl.Module;
using static NSLR_ObservationControl.Module.Observation_TMS;

namespace NSLR_ObservationControl.Module
{
    /// <summary>
    /// RealTimeRangeCombiner 사용 예제
    /// 기존 Observation_Control.cs 코드를 이것으로 교체하면 됩니다
    /// </summary>
    public partial class Observation_Control_Example : UserControl
    {
        private RealTimeRangeCombiner _rangeCombiner;
        private System.Threading.CancellationTokenSource _readCts;
        
        // 차트 참조
        private Chart _chart1; // O-C 시계열
        private Chart _chart2; // 히스토그램

        /// <summary>
        /// Range Combiner 초기화 + RGG Lookup Table 설정
        /// </summary>
        public void InitializeRangeCombiner()
        {
            // Range Combiner 생성 (Lookup Table 모드)
            _rangeCombiner = new RealTimeRangeCombiner();

            // RGG에서 Lookup Table 데이터 가져오기
            LoadLookupTableFromRGG();

            // 이벤트 핸들러 등록
            _rangeCombiner.OnMeasurementBatch += OnMeasurementBatchReceived;
            _rangeCombiner.OnHistogramUpdate += OnHistogramUpdated;
            _rangeCombiner.OnStatisticsUpdate += OnStatisticsUpdated;

            // 처리 시작
            _rangeCombiner.Start();

            Console.WriteLine("[INFO] RealTimeRangeCombiner initialized with Lookup Table mode");
        }

        /// <summary>
        /// RGG에서 Lookup Table 로드
        /// RGG 운영 시작 시 전송받은 데이터 사용
        /// </summary>
        private void LoadLookupTableFromRGG()
        {
            List<long> utcMsList;
            List<double> tofSecondsList;

            // 시스템 모드에 따라 RGG 컨트롤러에서 데이터 가져오기
            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            {
                utcMsList = Subsystem.RGG_SAT_Controller.interpolated_UtcMs;
                tofSecondsList = Subsystem.RGG_SAT_Controller.interpolated_ToF;
            }
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                utcMsList = Subsystem.RGG_DEB_Controller.interpolated_UtcMs;
                tofSecondsList = Subsystem.RGG_DEB_Controller.interpolated_ToF;
            }
            else
            {
                Console.WriteLine("[WARNING] Unknown system object, lookup table not loaded");
                return;
            }

            // Lookup Table 설정
            if (utcMsList != null && tofSecondsList != null && utcMsList.Count > 0)
            {
                _rangeCombiner.SetLookupTable(utcMsList, tofSecondsList);
                
                Console.WriteLine($"[INFO] Loaded {utcMsList.Count} lookup entries");
                Console.WriteLine($"[INFO] Time range: {utcMsList[0]} ~ {utcMsList[utcMsList.Count - 1]} ms");
                Console.WriteLine($"[INFO] ToF range: {tofSecondsList.Min():F6} ~ {tofSecondsList.Max():F6} sec");
            }
            else
            {
                Console.WriteLine("[ERROR] Lookup table data is empty or null!");
                MessageBox.Show("RGG Lookup Table 데이터가 없습니다.\n먼저 RGG에 예측 궤도 데이터를 전송하세요.",
                    "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// RGG Lookup Table 갱신 (운영 중 궤도 업데이트 시)
        /// </summary>
        public void UpdateLookupTable()
        {
            LoadLookupTableFromRGG();
            Console.WriteLine("[INFO] Lookup table updated");
        }

        /// <summary>
        /// 측정 데이터 배치 수신 핸들러 (UI 스레드에서 호출됨, 20Hz)
        /// </summary>
        private void OnMeasurementBatchReceived(List<RealTimeRangeCombiner.LaserMeasurement> measurements)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => OnMeasurementBatchReceived(measurements)));
                return;
            }

            try
            {
                if (_chart1 == null)
                    _chart1 = this.Parent?.Controls.Find("chart1", true).FirstOrDefault() as Chart;

                if (_chart1 == null) return;

                // 시계열 차트에 추가
                foreach (var m in measurements)
                {
                    _chart1.Series[0].Points.AddXY(m.EndTime, m.OminusC);
                    
                    // 데이터 포인트가 너무 많아지면 오래된 것 제거
                    if (_chart1.Series[0].Points.Count > 10000)
                    {
                        _chart1.Series[0].Points.RemoveAt(0);
                    }
                }

                // 차트 업데이트
                _chart1.ChartAreas[0].RecalculateAxesScale();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Chart update error: {ex.Message}");
            }
        }

        /// <summary>
        /// 히스토그램 업데이트 핸들러 (20Hz)
        /// </summary>
        private void OnHistogramUpdated(Dictionary<int, int> histogram)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => OnHistogramUpdated(histogram)));
                return;
            }

            try
            {
                if (_chart2 == null)
                    _chart2 = this.Parent?.Controls.Find("chart2", true).FirstOrDefault() as Chart;

                if (_chart2 == null) return;

                // 히스토그램 차트 업데이트
                _chart2.Series[0].Points.Clear();
                
                foreach (var kvp in histogram.OrderBy(k => k.Key))
                {
                    _chart2.Series[0].Points.AddXY(kvp.Key, kvp.Value);
                }

                // X축 범위 동기화
                if (_chart1 != null && _chart1.ChartAreas.Count > 0 && _chart2.ChartAreas.Count > 0)
                {
                    _chart2.ChartAreas[0].AxisX.Minimum = _chart1.ChartAreas[0].AxisY.Minimum;
                    _chart2.ChartAreas[0].AxisX.Maximum = _chart1.ChartAreas[0].AxisY.Maximum;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Histogram update error: {ex.Message}");
            }
        }

        /// <summary>
        /// 통계 업데이트 핸들러 (20Hz)
        /// </summary>
        private void OnStatisticsUpdated(RealTimeRangeCombiner.ProcessingStatistics stats)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => OnStatisticsUpdated(stats)));
                return;
            }

            // 콘솔 또는 UI 레이블에 통계 표시
            Console.WriteLine($"[STATS] Received:{stats.TotalReceived}, " +
                            $"Paired:{stats.TotalPaired}, " +
                            $"Dropped:{stats.TotalDropped}, " +
                            $"LookupMisses:{stats.LookupMisses}, " +
                            $"Rate:{stats.PairingRate:F2}%, " +
                            $"Latency:{stats.ProcessingLatencyMs:F3}ms, " +
                            $"Buffer:{stats.BufferUtilization}%, " +
                            $"Queue:{stats.QueueDepth}, " +
                            $"LookupSize:{stats.LookupTableSize}");
            
            // Lookup Miss 경고
            if (stats.LookupMisses > 0 && stats.TotalReceived > 0)
            {
                double missRate = (stats.LookupMisses * 100.0) / stats.TotalReceived;
                if (missRate > 10.0) // 10% 이상 miss
                {
                    Console.WriteLine($"[WARNING] High lookup miss rate: {missRate:F2}%");
                }
            }
        }

        /// <summary>
        /// 관측 시작 버튼 (기존 button2_Click_2 대체)
        /// </summary>
        private async void StartObservation_Click(object sender, EventArgs e)
        {
            try
            {
                // 레이저 및 RGG 시작
                if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                {
                    Subsystem.LAS_SAT_Controller.instance.laserStart_SatTrack();
                    Subsystem.RGG_SAT_Controller.instance.SetRangeMode();
                }
                else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
                {
                    Subsystem.LAS_DEB_Controller.instance.laserStart_DebTrack();
                    Subsystem.RGG_DEB_Controller.instance.SetRangeMode();
                }

                // Range Combiner 초기화 및 시작
                if (_rangeCombiner == null)
                    InitializeRangeCombiner();
                else
                    _rangeCombiner.ResetStatistics();

                // 네트워크 읽기 시작
                _readCts = new System.Threading.CancellationTokenSource();
                await ReadEventTimerDataAsync(_readCts.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"관측 시작 실패: {ex.Message}", "오류", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 이벤트 타이머 데이터 읽기 (비동기)
        /// </summary>
        private async System.Threading.Tasks.Task ReadEventTimerDataAsync(System.Threading.CancellationToken ct)
        {
            const int BUFFER_SIZE = 64; // 8바이트 * 8개
            byte[] buffer = new byte[BUFFER_SIZE];

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    if (stream == null || !stream.CanRead)
                    {
                        Console.WriteLine("[WARNING] Stream not available");
                        await System.Threading.Tasks.Task.Delay(100, ct);
                        continue;
                    }

                    // 네트워크에서 데이터 읽기
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, ct);
                    
                    if (bytesRead == 0)
                    {
                        Console.WriteLine("[WARNING] No data read from stream");
                        break;
                    }

                    // Range Combiner에 데이터 전달 (복사본)
                    byte[] dataCopy = new byte[bytesRead];
                    Array.Copy(buffer, dataCopy, bytesRead);
                    _rangeCombiner.EnqueueRawData(dataCopy, bytesRead);
                }
                catch (System.Threading.Tasks.TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Read error: {ex.Message}");
                    await System.Threading.Tasks.Task.Delay(100, ct);
                }
            }
        }

        /// <summary>
        /// 관측 중지 버튼
        /// </summary>
        private void StopObservation_Click(object sender, EventArgs e)
        {
            try
            {
                // 네트워크 읽기 중지
                _readCts?.Cancel();

                // 레이저 중지
                if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                {
                    Subsystem.LAS_SAT_Controller.instance.laserStop_SatTrack();
                    Subsystem.RGG_SAT_Controller.instance.SetStandbyMode();
                }
                else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
                {
                    Subsystem.LAS_DEB_Controller.instance.laserStop_DebTrack();
                    Subsystem.RGG_DEB_Controller.instance.SetStandbyMode();
                }

                // Range Combiner 중지 (하지만 데이터는 유지)
                _rangeCombiner?.Stop();

                // 측정 데이터 저장
                SaveMeasurementData();

                Console.WriteLine("[INFO] Observation stopped");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Stop observation error: {ex.Message}");
            }
        }

        /// <summary>
        /// 측정 데이터 저장 (CRD 파일 등)
        /// </summary>
        private void SaveMeasurementData()
        {
            if (_rangeCombiner == null) return;

            try
            {
                var measurements = _rangeCombiner.GetAllMeasurements();
                
                if (measurements.Count == 0)
                {
                    Console.WriteLine("[INFO] No measurements to save");
                    return;
                }

                // CRD 파일 또는 다른 형식으로 저장
                string filename = $"Range_{DateTime.Now:yyyyMMdd_HHmmss}.dat";
                
                using (var writer = new System.IO.StreamWriter(filename))
                {
                    writer.WriteLine("# Laser Range Measurement Data");
                    writer.WriteLine($"# Total Measurements: {measurements.Count}");
                    writer.WriteLine($"# Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    writer.WriteLine("# Format: SequenceId, StartTime(s), EndTime(s), ToF(s), O-C(ms), Az(deg), El(deg)");
                    writer.WriteLine("#");

                    foreach (var m in measurements)
                    {
                        writer.WriteLine($"{m.SequenceId},{m.StartTime:F12},{m.EndTime:F12}," +
                                       $"{m.ToF:F12},{m.OminusC:F6},{m.AzimuthDeg:F6},{m.ElevationDeg:F6}");
                    }
                }

                Console.WriteLine($"[INFO] Saved {measurements.Count} measurements to {filename}");
                MessageBox.Show($"측정 데이터 저장 완료\n파일: {filename}\n개수: {measurements.Count}", 
                    "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Save error: {ex.Message}");
                MessageBox.Show($"데이터 저장 실패: {ex.Message}", "오류", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 폼 종료 시 정리
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _readCts?.Cancel();
                _rangeCombiner?.Dispose();
            }
            base.Dispose(disposing);
        }

        // 기존 코드에서 필요한 필드들
        private System.Net.Sockets.NetworkStream stream;
        
        // InterpolateToF 함수 (기존 코드에 있다고 가정)
        private double InterpolateToF(long utcMs, List<long> utcList, List<double> tofList)
        {
            if (utcList == null || tofList == null || utcList.Count == 0)
                return 0;

            // 선형 보간
            for (int i = 0; i < utcList.Count - 1; i++)
            {
                if (utcMs >= utcList[i] && utcMs <= utcList[i + 1])
                {
                    double ratio = (double)(utcMs - utcList[i]) / (utcList[i + 1] - utcList[i]);
                    return tofList[i] + ratio * (tofList[i + 1] - tofList[i]);
                }
            }

            // 범위 밖이면 가장 가까운 값 반환
            if (utcMs < utcList[0])
                return tofList[0];
            else
                return tofList[tofList.Count - 1];
        }
    }
}
