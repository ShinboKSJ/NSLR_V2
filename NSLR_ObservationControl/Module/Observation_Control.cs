using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Markup;
using static NSLR_ObservationControl.CSU_Observation;
using static NSLR_ObservationControl.Module.Observation_TMS;
using static NSLR_ObservationControl.ControlCommand;
using NSLR_ObservationControl.Subsystem;
using System.Net;
using System.Xml;
using System.Collections.Concurrent;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static NSLR_ObservationControl.Subsystem.RGG_SAT_Controller;

namespace NSLR_ObservationControl.Module
{
    public partial class Observation_Control : UserControl
    {
        static StringBuilder consoleOutput = new StringBuilder(); // Console 출력값을 저장할 StringBuilder 객체
                                                                  //MainForm mainForm = new MainForm();
        public Socket clientSocket;
        public NetworkStream stream;

        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        const int SERVER_MODE = 0;
        private int Op_mode = 0;
        const string MSG_NO_CONNECT = "Waiting for a Client.......";
        const string MSG_CONNECT = " +++ Connected  +++";
        //  const string OPPONENT_IP = "192.168.0.31";
        const int PORT = 1111;

        Tcp cTcpServer;
        CSU_GroundCalibration csu_groundcalibration;

        int count = 0;
        int countSet = 100000000;

        Dictionary<int, int> data = new Dictionary<int, int>();

        TaskControl taskControl = TaskControl.instance;

        private LAS_SAT_Controller lasSatControl;
        private LAS_DEB_Controller lasDebControl;
        private RGG_SAT_Controller satRGGcontrol;
        private RGG_DEB_Controller debRGGcontrol;
        private DOM_Controller_v40 domeController;


        public Observation_Control()
        {
            InitializeComponent();

            domeController = DOM_Controller_v40.instance;
            satRGGcontrol = RGG_SAT_Controller.instance;
            debRGGcontrol = RGG_DEB_Controller.instance;
            lasSatControl = LAS_SAT_Controller.instance;
            lasDebControl = LAS_DEB_Controller.instance;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                lasSatControl.laserStart_SatTrack();
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
                lasDebControl.laserStart_DebTrack();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            {
                lasSatControl.laserStop_SatTrack();
                satRGGcontrol.SetStandbyMode();

            }
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                lasDebControl.laserStop_DebTrack();
                debRGGcontrol.SetStandbyMode();

            }
            var parentControl = this.Parent as CSU_Observation;
            if (parentControl != null)
            {
                parentControl.StopProgressLoop();
            }
            observationEnd();
            
            SaveDataToFile();

            domeController.doStop();
            await DisposeResourcesAsync();
            observationEndandDataReset();
            ResetAll();
            taskControl.SetMutex("None");
        }
        public void ResetAll()
        {
            try
            {
                lock (lockObject)
                {
                    dataBuffer?.Clear();
                }

                lock (bufferLock)
                {
                }

                histogram?.Clear();

                count = 0;

                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => ClearCharts()));
                }
                else
                {
                    ClearCharts();
                }

                CleanupConnections();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reset error: {ex.Message}");
            }
        }

        private void ClearCharts()
        {
            try
            {
                var parentControl = this.Parent;
                if (parentControl == null) return;

                var chart1 = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
                var chart2 = parentControl.Controls.Find("chart2", true).FirstOrDefault() as Chart;

                if (chart1 != null)
                {
                    chart1.Series[0]?.Points.Clear();
                    chart1.ChartAreas[0]?.RecalculateAxesScale();
                    chart1.Update();
                }

                if (chart2 != null)
                {
                    chart2.Series[0]?.Points.Clear();
                    chart2.ChartAreas[0]?.RecalculateAxesScale();
                    chart2.Update();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chart clear error: {ex.Message}");
            }
        }
       

        private void observationEnd()
        {
            controlCommand = TrackingMount.ServoDisable;
            operatingMode = TrackingMount.InitEndState;
            TargetInfo.TargetFlag = false;
            TargetInfo.TargetStartFlag = true;
            TargetStep = TargetInfo.Step.None;
        }
        private void observationEndandDataReset()
        {
            CSU_Observation.AddAzOffSet = 0;
            CSU_Observation.AddElOffSet = 0;
            ObservationStart.Enabled = true;
            ObservationLazerStart.Enabled = true;
            while (!processedStartTimes.IsEmpty)
            {
                processedStartTimes.TryDequeue(out _);
            }
            startTimes.Clear();
            endTimes.Clear();
            chartTof.Clear();
            satRGGcontrol.RGG_TOF.Clear();
            satRGGcontrol.RGG_UTCns.Clear();
            debRGGcontrol.RGG_TOF.Clear();
            debRGGcontrol.RGG_UTCns.Clear();
            count = 0;
            //ObservationData.Clear();
            if ( stream == null )
            {
                Console.WriteLine("ET connection closed");
                button1.Enabled = true;
            label1.Text = "통신 연결 필요";
                return;
            }
        
        }
        private void SaveDataToFile()
        {
            try
            {
                string directoryPath = Path.Combine(Application.StartupPath, "ET_Logging");
                string directoryPath2 = Path.Combine(Application.StartupPath, "RGG_Logging");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                if (!Directory.Exists(directoryPath2))
                {
                    Directory.CreateDirectory(directoryPath2);
                }
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                string baseFileName = !string.IsNullOrEmpty(ObservingSatellite_Information.observingSatellite_name)
                    ? ObservingSatellite_Information.observingSatellite_name
                    : "default";

                string fileName = $"{baseFileName}_{timestamp}.txt";
                string filePath = Path.Combine(directoryPath, fileName);

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("[Start Times]");
                    foreach (var start in startTimes)
                    {
                        writer.WriteLine(start.ToString("F15"));
                    }

                    writer.WriteLine("\n[End Times]");
                    foreach (var end in endTimes)
                    {
                        writer.WriteLine(end.ToString("F15"));
                    }
                    writer.WriteLine("\n[Az]");
                    foreach (var Set in AzElSets)
                    {
                        writer.WriteLine(Set.Az.ToString("F6"));
                    }
                    writer.WriteLine("\n[El]");
                    foreach (var Set in AzElSets)
                    {
                        writer.WriteLine(Set.El.ToString("F6"));
                }
                }
                string filePath2 = Path.Combine(directoryPath2, fileName);
                using (StreamWriter writer2 = new StreamWriter(filePath2))
                {
                    if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                    {
                    writer2.WriteLine("[UTCns]");
                    foreach (var UTCns in satRGGcontrol.RGG_UTCns)
                    {
                        writer2.WriteLine(UTCns);
                    }

                    writer2.WriteLine("\n[TOF]");
                    foreach (var tof in satRGGcontrol.RGG_TOF)
                    {
                        writer2.WriteLine(tof);
                    }
                }
                    else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
                    {
                        writer2.WriteLine("[UTCns]");
                        foreach (var UTCns in debRGGcontrol.RGG_UTCns)
                        {
                            writer2.WriteLine(UTCns);
                        }

                        writer2.WriteLine("\n[TOF]");
                        foreach (var tof in debRGGcontrol.RGG_TOF)
                        {
                            writer2.WriteLine(tof);
                        }
                    }
              
                }
                Console.WriteLine($"데이터 저장 완료! (파일 위치: {filePath})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"파일 저장 오류: {ex.Message}");
            }
        }
        private async Task DisposeResourcesAsync()
        {
            try
            {
                if (cancellationTokenSource != null)
                {
                    try
                    {
                        cancellationTokenSource.Cancel();
                        await Task.Delay(100); 
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"cancellation cancel error: {ex.Message}");
                    }
                }

                if (stream != null)
                {
                    try
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"stream close error: {ex.Message}");
                    }
                    stream = null;
                }

                if (clientSocket != null)
                {
                    try
                    {
                        if (clientSocket.Connected)
                        {
                            clientSocket.Shutdown(SocketShutdown.Both);
                        }
                        clientSocket.Close();
                        clientSocket.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"socket close error: {ex.Message}");
                    }
                    clientSocket = null;
                }

                if (cancellationTokenSource != null)
                {
                    try
                    {
                        cancellationTokenSource.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"cancellation dispose error: {ex.Message}");
                    }
                        cancellationTokenSource = null;
                    }
                }
            catch (Exception ex)
            {
                Console.WriteLine($"DisposeResourcesAsync Error: {ex.Message}");
            }
        }

        private async void button_observation_start(object sender, EventArgs e)
        {
            try
            {
                var parentControl = this.Parent as CSU_Observation;
                if (parentControl != null)
                {
                    parentControl.StopProgressLoop();
                }

                if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                {
                    if (TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.RANGE)
                    {
                        lasSatControl.laserReady_SatTrack();
                    }
                }
                else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
                {
                    if (TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.RANGE)
                    {
                        lasDebControl.laserReady_DebTrack();
                    }
                }
                observationStart();
                var PGparm = ObservingSatellite_Information.GetData();
                parentControl?.StartProgressUpdateLoop(PGparm.StartTime, PGparm.EndTime);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
            }
        }
        private void observationStart()
        {
            TargetInfo.TargetFlag = true;
            TargetInfo.TargetStartFlag = true;
            TargetStep = TargetInfo.Step.One;
            operatingMode = TrackingMount.ReadyState;
           
        }
        private async Task<bool> CreateStreamAsync(string ip, int port)
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                await clientSocket.ConnectAsync(ip, port);
                if (!clientSocket.Connected)
                {
                    Console.WriteLine("Failed to connect to ET server");
                    return false;
                }
                stream = new NetworkStream(clientSocket);
                ParamInit();
                Console.WriteLine("Connected to ET server");
                return true;
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Failed to connect to ET server: {ex.Message}");
                Console.WriteLine("Retry");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
        private void ParamInit()
        {
            var parentControl = this.Parent;
            var chartControl = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
            var chart1 = (Chart)chartControl;
            var chartControl2 = parentControl.Controls.Find("chart2", true).FirstOrDefault() as Chart;
            var chart2 = (Chart)chartControl2;
            foreach (Series series in chart1.Series)
            {
                chart1.Series[0].Points.Clear();
            }
            foreach (Series series in chart2.Series)
            {
                chart2.Series[0].Points.Clear();
            }
            count = 0;
            displayCount = 0;
            data.Clear();
        }
        private double minValue = double.MaxValue;
        private double maxValue = double.MinValue;
        private Task processingTask;
        int highValue = 0;
        int lowValue = 0;
        int bufferSize = 64;
        private ConcurrentQueue<byte[]> dataQueue = new ConcurrentQueue<byte[]>();
        List<(double x, double y)> dataBuffer = new List<(double x, double y)>();
        List<double> startTimes = new List<double>();
        List<double> endTimes = new List<double>();
        List<double> tof = new List<double>();
        List<double> chartTof = new List<double>();
        private object lockObject = new object();
        double TOF = 0;
        double RggWidth = 0;
        private async void button2_Click_2(object sender, EventArgs e)
        {
            try
            {
                if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                {
                    lasSatControl.laserStart_SatTrack();
                    satRGGcontrol.SetRangeMode();
                }
                else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
                {
                    lasDebControl.laserStart_DebTrack();
                    debRGGcontrol.SetRangeMode();
                }

                temp_count = 0;
                cancellationTokenSource = new CancellationTokenSource();
                cancellationToken = cancellationTokenSource.Token;

                _observationData = new List<(int Id, double StartTime, double TofMs)>();
                processingTask = Task.Run(ProcessDataAsync, cancellationToken);
                await ReadDataAsync();
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"Stream DisposedException : {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error : {ex.Message}");
                return;
            }
        }
        private async Task ReadDataAsync()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (stream == null || !stream.CanRead || !stream.CanWrite)
                {
                    Console.WriteLine("Stream is not available for reading.");
                    break;
                }
                try
                {
                    byte[] buffer = new byte[bufferSize];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    if (bytesRead == 0) break;
                    Console.WriteLine("ET READING");
                    dataQueue.Enqueue(buffer.Take(bytesRead).ToArray());
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Stream has been disposed.");
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error in ReadDataAsync: {ex.Message}");
                }
            }
        }
        private int startTimeIndex = 0;
        private int endTimeIndex = 0;
        private List<ValueTuple<double, double>> combinedTimes = new List<ValueTuple<double, double>>();
        private HashSet<UInt64> processedData = new HashSet<UInt64>();
        private ConcurrentQueue<double> processedStartTimes = new ConcurrentQueue<double>();
        private ConcurrentQueue<double> processedEndTimes = new ConcurrentQueue<double>();
        private ConcurrentQueue<(double endTime, double tof)> processedEndTimepairs = new ConcurrentQueue<(double, double)>();
        private ConcurrentQueue<(double Az, double El)> AzElSets = new ConcurrentQueue<(double, double)>();
        private Queue<double> pendingStartTimes = new Queue<double>();
        private double lastEndTime = -1;
        int temp_count;
        private async Task ProcessDataAsync()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!dataQueue.TryDequeue(out var buffer))
                {
                    await Task.Delay(1);
                    continue;
                }
                try
                {
                    int previousStartCount = startTimes.Count;
                    int previousEndCount = endTimes.Count;
                    for (int i = 0; i < buffer.Length; i += 8)
                    {
                        if (i + 7 >= buffer.Length) break;

                        byte[] irBytes = buffer.Skip(i).Take(4).ToArray();
                        byte[] timeBytes = buffer.Skip(i + 4).Take(4).ToArray();

                        if (!BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(irBytes);
                            Array.Reverse(timeBytes);
                        }

                        int ir = BitConverter.ToInt32(irBytes, 0);
                        int time = BitConverter.ToInt32(timeBytes, 0);

                        //double adjustedTime = (Math.Abs(ir) * 327680000.0 + time) * 1e-12;
                        double adjustedTime = Math.Abs(ir) * 327680.0e-9 + time * 1e-12;
                        temp_count++;
                        if (ir < 0)
                        {
                            startTimes.Add(adjustedTime);
                            processedStartTimes.Enqueue(adjustedTime);
                        }
                        else
                        {
                            endTimes.Add(adjustedTime);
                            double expectedTof = GetToFForCurrentTime();
                            tof.Add(expectedTof);
                            processedEndTimepairs.Enqueue((adjustedTime, expectedTof));
                            lock (Observation_TMS.LockObj)
                            {
                                AzElSets.Enqueue((Observation_TMS.TMSAZPositon, Observation_TMS.TMSELPosition));
                            }
                            CombineStartAndEndTimes();
                            //CombineWithRggTimestamps();
                        }
                        if (temp_count % 100 == 0)
                        {

                            await UpdateUIAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in ProcessDataAsync: {ex.Message}");
                }
            }
        }
        double Tof;
        double threshold;
        //double Cal0 = 279140e-12;

        private const int expectedCount = 100000;
        private List<(int Id, double StartTime, double TofMs)> _observationData = new List<(int Id, double StartTime, double TofMs)>(expectedCount);
        public List<(int Id, double StartTime, double TofMs)> ObservationData
        {
            get { return _observationData; }
            set { _observationData = value; }
        }
        /*
        private void CombineStartAndEndTimes()
        {
            double allowedTolerance = 4e-7;

            while (processedEndTimepairs.TryDequeue(out var pair))
            {

                foreach (var startTime in processedStartTimes)
                {
                    double actualTof = pair.endTime - startTime;

                    if (Math.Abs(actualTof - pair.tof - Cal0) <= allowedTolerance)
                    {
                        Int64 tofPs = (Int64)(actualTof * 1e12);
                        dataBuffer.Add(tofPs - (Int64)(pair.tof * 1e12) - (Int64)(Cal0 * 1e12));
                        chartTof.Add(pair.tof);
                        count++;
                        double tofMs = (actualTof * 1e3) - (pair.tof * 1e3) - (Cal0 * 1e3);
                        _observationData.Add((
                              0,
                              startTime,
                              tofMs
                         ));
                    }
                    else if (startTime > pair.endTime - pair.tof - Cal0 - allowedTolerance)
                    {
                        threshold = pair.endTime - pair.tof - Cal0 - allowedTolerance - 1e-1;
                        CleanupProcessedStartTimes(threshold);
                        break;
                    }
                }
            }
        }*/
        private const int BinSize = 10;

        private int GetBinValue(double ocValue)
        {
            return (int)(Math.Round(ocValue / BinSize) * BinSize);
        }
        private void CombineStartAndEndTimes()
        {
            // allowedTolerance: 매칭 윈도우
            //  - 하드웨어 타임스탬프(이벤트 타이머)와 DateTime.UtcNow 기반 TOF 예측 간 클럭 오프셋 포함
            //  - 60Hz 간격 = 16.67ms → 오인매칭 방지를 위해 절반(~8ms) 이하여야 안전
            //  - GEO(TOF ~0.24s, TLE 기반)는 클럭 표류/예측 오차가 수백 μs ~ 수 ms 범위 가능
            //  - 1ms (1e-3)로 설정: 인접 펄스(16.67ms)와 오인매칭 없으면서 클럭 오프셋 허용
            double allowedTolerance = 1e-3;

            while (processedEndTimepairs.TryDequeue(out var pair))
            {
                double expectedStartLowerBound = pair.endTime - pair.tof - GlobalVariables.SystemDelay - allowedTolerance ;
                double expectedStartUpperBound = pair.endTime - pair.tof - GlobalVariables.SystemDelay + allowedTolerance ;

                while (processedStartTimes.TryPeek(out var startTime))
                {
                    double actualTof = pair.endTime - startTime;

                    if (startTime >= expectedStartLowerBound && startTime <= expectedStartUpperBound)
                    {
                        processedStartTimes.TryDequeue(out _);

                        double O_C = (actualTof - pair.tof - GlobalVariables.SystemDelay ) * 1e3;
                        double tofMs = actualTof;

                        int binnedOC = GetBinValue(O_C);

                        lock (bufferLock)
                        {
                            dataBuffer.Add((O_C, pair.endTime));
                        }
                        if (data.ContainsKey(binnedOC))
                        {
                            data[binnedOC]++;
                        }
                        else
                        {
                            data[binnedOC] = 1;
                        }
                        chartTof.Add(O_C);
                        count++;

                        _observationData.Add((
                            1, // 1 = Identified (페어링 성공)
                            startTime,
                            tofMs
                        ));

                        Console.WriteLine($"[INFO] 페어링 성공: O-C Value ={O_C:F6}ms, start={startTime:F9}, end={pair.endTime:F9}");
                        Console.WriteLine($"[INFO] 페어링 성공: tofMs={tofMs:F9}");
                        break;
                    }
                    else if (startTime > expectedStartUpperBound)
                    {
                        // 이 start는 현재 end pair보다 최신 — 다음 end pair를 위해 큐에 보존
                        // (GEO 60Hz: ~14개 이상의 펄스가 동시 비행 중이므로 반드시 보존 필요)
                        break;
                    }
                    else
                    {
                        // startTime < expectedStartLowerBound: 너무 오래된 start — 폐기
                        processedStartTimes.TryDequeue(out _);
                        Console.WriteLine($"[DEBUG] 페어링 실패(오래된 start 폐기): start={startTime:F9}, end={pair.endTime:F9}, TOF={actualTof:F9}");
                    }
                }
            }
        }
        private void CleanupProcessedStartTimes(double threshold)
        {
            int removedCount = 0;
            while (processedStartTimes.TryPeek(out double startTime))
            {
                if (startTime < threshold)
                {
                    processedStartTimes.TryDequeue(out _);
                    removedCount++;
                }
                else
                {
                    break;
                }
            }
            if (removedCount > 0)
                Console.WriteLine($"Cleanup removed {removedCount} old startTimes");
        }
        private void CombineWithRggTimestamps()
        {
            double allowedTolerance = 4e-7;

            while (processedEndTimepairs.TryDequeue(out var pair))
            {
                foreach (var rgg in satRGGcontrol.RGG_Timestamps.ToArray()) 
                {
                    double actualTof = pair.endTime - rgg.UtcSeconds;

                    if (Math.Abs(rgg.TofSeconds - GlobalVariables.SystemDelay) <= allowedTolerance)
                    {
                        Int64 tofPs = (Int64)(actualTof * 1e12);
                      //  dataBuffer.Add(tofPs - (long)(rgg.TofSeconds * 1e12));
                        chartTof.Add(rgg.TofSeconds);
                        count++;
                    }
                    else if (rgg.UtcSeconds > pair.endTime - rgg.TofSeconds - GlobalVariables.SystemDelay - allowedTolerance)
                    {
                        double threshold = pair.endTime - rgg.TofSeconds - GlobalVariables.SystemDelay - allowedTolerance - 1e-1;
                        CleanupOldRggTimestamps(threshold);
                        break;
                }
            }
        }
        }

        private void CleanupOldRggTimestamps(double threshold)
        {
            int removedCount = 0;

            while (satRGGcontrol.RGG_Timestamps.TryPeek(out var rgg) &&
                   rgg.UtcSeconds < threshold)
            {
                if (satRGGcontrol.RGG_Timestamps.TryDequeue(out _))
                {
                    removedCount++;
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine($"Cleanup removed {removedCount} old RGG_Timestamps");
        }
        private async Task UpdateUIAsync()
        {
            await Task.Run(() =>
            {
                this.BeginInvoke((MethodInvoker)delegate
                {
                    lock (lockObject)
                    {
                        ProcessDataBuffer(dataBuffer);
                    }

                    if (count == countSet)
                    {
                        CleanupConnections();
                    }
                });
            });
        }
        int displayCount = 0;

        private readonly object bufferLock = new object();
        public void ProcessDataBuffer(List<(double,double)> dataBuffer)
        {
            var parentControl = this.Parent;
            var chartControl = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
            var chart1 = (Chart)chartControl;

            var chartControl2 = parentControl.Controls.Find("chart2", true).FirstOrDefault() as Chart;
            var chart2 = (Chart)chartControl2;
            List<(double,double)> bufferCopy;
            lock (bufferLock)
            {
                bufferCopy = dataBuffer.ToList();
                dataBuffer.Clear();
            }

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => ProcessChart(bufferCopy)));
            }
            else
            {
                ProcessChart(bufferCopy);
            }
        }
        Dictionary<double, int> histogram = new Dictionary<double, int>();
        private void ProcessChart(List<(double, double)> bufferCopy)
        {
            var chart1 = this.Parent.Controls.Find("chart1", true).FirstOrDefault() as Chart;
            var chart2 = this.Parent.Controls.Find("chart2", true).FirstOrDefault() as Chart;
            if (chart1 == null || chart2 == null) return;

            double binWidth = 0.0001;

            foreach (var dataPoint in bufferCopy)
            {
                double yVal = dataPoint.Item1; 
                double xVal = dataPoint.Item2;

                chart1.Series[0].Points.AddXY(xVal, yVal);

                double binKey = Math.Round(yVal / binWidth) * binWidth;
                if (!histogram.ContainsKey(binKey))
                    histogram[binKey] = 0;
                histogram[binKey]++;
            }

            chart2.Series[0].Points.Clear();
            foreach (var kvp in histogram.OrderBy(k => k.Key))
            {
                chart2.Series[0].Points.AddXY(kvp.Key, kvp.Value);
            }

            if (chart1.ChartAreas.Count > 0 && chart2.ChartAreas.Count > 0)
            {
                chart2.ChartAreas[0].AxisX.Minimum = chart1.ChartAreas[0].AxisY.Minimum;
                chart2.ChartAreas[0].AxisX.Maximum = chart1.ChartAreas[0].AxisY.Maximum;
            }

            chart1.Update();
            chart2.Update();
        }
        private void CleanupConnections()
        {
            if (stream != null)
            {
                stream.Dispose();
                stream = null;
            }

            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                clientSocket = null;
            }
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
            count = 0;

            Console.WriteLine("종료");
        }
        private readonly object lockObj = new object();
        private ObservationData observationData;
 
        public double GetToFForCurrentTime()
        {
            long currentUtcMs = (long)DateTime.UtcNow.TimeOfDay.TotalMilliseconds + (long)Observation_TMS.TimeBiasValue;

            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            {
                return InterpolateToF(currentUtcMs, RGG_SAT_Controller.interpolated_UtcMs, RGG_SAT_Controller.interpolated_ToF);
            }
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                return InterpolateToF(currentUtcMs, RGG_DEB_Controller.interpolated_UtcMs, RGG_DEB_Controller.interpolated_ToF);
            }
            return 0.0;
        }
        private double InterpolateToF(long targetUtcMs, List<long> utcMsList, List<double> tofList)
        {
            if (utcMsList.Count < 8 || tofList.Count < 8)
                throw new InvalidOperationException("7차 라그랑주 보간 데이터 부족");

            if (utcMsList.Count != tofList.Count)
                throw new InvalidOperationException("UtcMs와 ToF 리스트의 길이 불일치");

            int n = utcMsList.Count;

            int closestIndex = utcMsList.FindIndex(t => t >= targetUtcMs);
            if (closestIndex == -1)
                closestIndex = n - 1;

            int startIndex = closestIndex - 4;
            int endIndex = startIndex + 7;

            if (startIndex < 0)
            {
                startIndex = 0;
                endIndex = 7;
            }
            else if (endIndex >= n)
            {
                endIndex = n - 1;
                startIndex = endIndex - 7;
            }

            var selectedUtcMs = utcMsList.GetRange(startIndex, 8).Select(x => (double)x).ToList();
            var selectedToF = tofList.GetRange(startIndex, 8);

            double result = 0.0;

            for (int i = 0; i < 8; i++)
            {
                double term = selectedToF[i];
                double xi = selectedUtcMs[i];

                for (int j = 0; j < 8; j++)
                {
                    if (i == j) continue;

                    double xj = selectedUtcMs[j];
                    term *= (targetUtcMs - xj) / (xi - xj);
                }

                result += term;
            }

            return result;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                lasSatControl.laserStop_SatTrack();
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
                lasDebControl.laserStop_DebTrack();
        }

        /*private async void button_observation_ready(object sender, EventArgs e)
        {
            try
            {
                string ip = DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT ? "192.168.10.61" : "192.168.10.51";
                int port = 33228;
                ObservationStart.Enabled = false;
                ObservationLazerStart.Enabled = false;
                button1.Enabled = false;
                label1.Text = "연결 시도중";
                bool isConnected = await CreateStreamAsync(ip, port);
                if (!isConnected || stream == null || !stream.CanWrite)
                {
                    Console.WriteLine("ET connection failed or stream unavailable");
                    return;
                }

                byte[] buffer = new byte[20];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                Console.WriteLine("Received (after peer1): " + BitConverter.ToString(buffer, 0, bytesRead));
                Console.WriteLine("Connected to server");

                byte[] peer1 = new byte[]
                {
                      0x09, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                      0x02, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00,
                      0xe8, 0x03, 0x00, 0x00
                };
                await stream.WriteAsync(peer1, 0, peer1.Length);
                Console.WriteLine("Sent peer1 (setting)");

                buffer = new byte[8];
                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                byte[] peer2 = new byte[]
                {
                      0xe7, 0x03, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00
                };
                await stream.WriteAsync(peer2, 0, peer2.Length);
                Console.WriteLine("Sent peer2 (run)");
                byte[] feedback = new byte[8];
                var delayTask = Task.Delay(15000);
                var readTask = stream.ReadAsync(feedback, 0, 8);
                var completedTask = await Task.WhenAny(readTask, delayTask);
                if (completedTask == readTask)
                {
                    int feedbackRead = await readTask;

                    if (feedbackRead == 8)
                    {
                        int errorCode = BitConverter.ToInt32(feedback, 4);
                        if (errorCode == -10)
                        {
                            label1.Text = "연결실패";
                            ObservationStart.Enabled = false;
                            ObservationLazerStart.Enabled = false;
                            button1.Enabled = true;
                            await DisposeResourcesAsync();
                            await Task.Delay(1000);
                        }
                    }
                    else
                    {
                      *//*  buffer = new byte[8];
                        byte[] peer3 = new byte[] { 0xde, 0x00, 0x00, 0x00 };
                        bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        var readTask1 = stream.ReadAsync(feedback, 0, 8);
                        Console.WriteLine("Received (after peer3): " + BitConverter.ToString(buffer, 0, bytesRead));*//*
                        button1.Enabled = false;
                        Console.WriteLine("정상 피드백 수신. 준비 완료.");
                        label1.Text = "연결완료";
                        ObservationStart.Enabled = true;
                        ObservationLazerStart.Enabled = true;
                    }
                }
                else
                {
                    button1.Enabled = false;
                    Console.WriteLine("정상 피드백 수신. 준비 완료.");
                    label1.Text = "연결완료";
                    ObservationStart.Enabled = true;
                    ObservationLazerStart.Enabled = true;
                    }
                }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
            }

        }*/
        private async void button_observation_ready(object sender, EventArgs e)
        {
            try
            {
                // string ip = DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT ? "192.168.10.61" : "192.168.10.51";
                string ip = "127.0.0.1";
                int port = 33228;
                ObservationStart.Enabled = false;
                ObservationLazerStart.Enabled = false;
                button1.Enabled = false;
                label1.Text = "연결 시도중";
                bool isConnected = await CreateStreamAsync(ip, port);
                if (!isConnected || stream == null || !stream.CanWrite)
                {
                    Console.WriteLine("ET connection failed or stream unavailable");
                    return;
                }

                /*                byte[] buffer = new byte[20];
                                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                                Console.WriteLine("Received (after peer1): " + BitConverter.ToString(buffer, 0, bytesRead));
                                Console.WriteLine("Connected to server");*/

                byte[] peer1 = new byte[]
                {
                      0x09, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                      0x02, 0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00,
                      0xe8, 0x03, 0x00, 0x00
                };
                await stream.WriteAsync(peer1, 0, peer1.Length);
                Console.WriteLine("Sent peer1 (setting)");

                /*     buffer = new byte[8];
                     bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);*/
                byte[] peer2 = new byte[]
                {
                      0xe7, 0x03, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00
                };
                await stream.WriteAsync(peer2, 0, peer2.Length);
                Console.WriteLine("Sent peer2 (run)");
                /*      byte[] feedback = new byte[8];
                      var delayTask = Task.Delay(15000);
                      var readTask = stream.ReadAsync(feedback, 0, 8);
                      var completedTask = await Task.WhenAny(readTask, delayTask);*/
                //   if (completedTask == readTask)
                if (true)
                {
                    button1.Enabled = false;
                    Console.WriteLine("정상 피드백 수신. 준비 완료.");
                    label1.Text = "연결완료";
                    ObservationStart.Enabled = true;
                    ObservationLazerStart.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                    Console.WriteLine("정상 피드백 수신. 준비 완료.");
                    label1.Text = "연결완료";
                    ObservationStart.Enabled = true;
                    ObservationLazerStart.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
            }

        }

        private CancellationTokenSource scanCancelTokenSource;
        private async void mountAutoScanning(scanInfo scanInfo)
        {
            scanCancelTokenSource?.Cancel();
            // 로컬 변수에 캡처하여 finally가 항상 자신의 CTS를 dispose하도록 보장
            var cts = new CancellationTokenSource();
            scanCancelTokenSource = cts;
            CancellationToken token = cts.Token;

            int range = scanInfo.range;
            // GenerateSpiralScanPoints 반환값은 이미 deg 단위이므로 추가 변환 불필요
            List<(double x, double y)> spiralPoints = GenerateSpiralScanPoints(range, scanInfo);

            try
            {
                foreach (var point in spiralPoints)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("스캔 취소");
                        break;
                    }

                    // point.x/y는 GenerateSpiralScanPoints에서 arcsec→deg 변환 완료된 값
                    AddAzOffSet = point.x;
                    AddElOffSet = point.y;

                    Console.WriteLine($"Az : {AddAzOffSet}, El : {AddElOffSet}");

                    await Task.Delay((int)(scanInfo.stayTime * 1000), token);
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("스캔 취소");
            }
            finally
            {
                cts.Dispose();
                // 다른 스캔이 이미 시작되지 않은 경우에만 null로 초기화
                if (scanCancelTokenSource == cts)
                    scanCancelTokenSource = null;
            }
        }

        private List<(double x, double y)> GenerateSpiralScanPoints(int range, scanInfo scanInfo)
        {
            double arcsecToDeg = scanInfo.tickOffset / 3600.0;
            // (2*range+1)² : ±range 격자 전체 포인트 수
            int maxSteps = (2 * range + 1) * (2 * range + 1);
            List<(double x, double y)> spiralPoints = new List<(double x, double y)>();

            int x = 0, y = 0;
            spiralPoints.Add((x, y)); 

            int dx = 1, dy = 0;
            int steps = 1;

            while (spiralPoints.Count < maxSteps)
            {
                for (int dir = 0; dir < 2; dir++) 
                {
                    for (int i = 0; i < steps; i++)
                    {
                        x += dx;
                        y += dy;

                        if (Math.Abs(x) <= range && Math.Abs(y) <= range)
                        {
                            spiralPoints.Add((x, y));
                        }
                    }
                    int temp = dx;
                    dx = -dy;
                    dy = temp;
                }
                steps++; 
            }
            return spiralPoints.Select(p => (p.x * arcsecToDeg, p.y * arcsecToDeg)).ToList();
        }



        private double ArcsecToDegree(double arcsec)
        {
            return arcsec / 3600.0;
        }

        private void AutoScan_Click(object sender, EventArgs e)
        {
            /*       using (ScanOption optionForm = new ScanOption())
            {
                if (optionForm.ShowDialog() == DialogResult.OK)
                {
                    scanInfo scan = new scanInfo
                    {
                        range = optionForm.RangeValue,
                        tickOffset = optionForm.TickOffsetValue,
                        stayTime = optionForm.StayTimeValue
                    };

                    mountAutoScanning(scan);
                }
                   }*/
            ScanOption optionForm = new ScanOption();

            optionForm.ScanConfirmed += (s, scan) =>
            {
                mountAutoScanning(scan);
            };

            optionForm.Show();
        }
        public class scanInfo
        {
            public int range;
            public double tickOffset;
            public double stayTime;
            }

        private void btnStopScan_Click(object sender, EventArgs e)
        {
            scanCancelTokenSource?.Cancel();
        }
    }
}
