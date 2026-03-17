using Emgu.CV.Features2D;
using mv.impact.acquire;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Configuration;

//using System.Windows.Controls;
using System.Windows.Forms;
using static NSLR_ObservationControl.ControlCommand;
using static System.Windows.Forms.AxHost;
using static NSLR_ObservationControl.CSU_StarCalibration;
using static NSLR_ObservationControl.CSU_Observation;
using NSLR_ObservationControl.Subsystem;
using SGPdotNET.Observation;
using static NSLR_ObservationControl.Module.StarCalibration_Control;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Linq.Expressions;

namespace NSLR_ObservationControl.Module
{
    public partial class GroundTargetMount
    {
        private UdpClient ServerTMS;

        private bool flag;
        private static long frequency;
        private long lastTime;
        private IPEndPoint TMSEndPoint;

        private Thread thread1;
        private bool isRunning1 = true;
        private Thread thread2;
        private bool isRunning2;

        public const double TrackingReadyTime = 3000;
        public static ushort operatingMode;
        public static ushort controlCommand;
        public static ushort TMSoperatingMode;  // 상태정보 public static 추가
        public static ushort TMScontrolCommand; // 상태정보 public static 추가
        public static double azUserOffset { get; set; }
        public static double elUserOffset { get; set; }
        public static double TimeBiasValue { get; set; } = 0;
        double TMSPacketTime;
        public static double TMSAZPositon;  // 상태정보 public static 추가
        public static double TMSELPosition; // 상태정보 public static 추가
        double TMSUsePacketTime;
        public static double[] elevationData = new double[8];   // 사용자 옵셋 public static 추가
        public static double[] azimuthData = new double[8];     // 사용자 옵셋 public static 추가
        public byte[] MSG_MSG_ID_MOUNT;
        byte[] TMSCBIT;
        byte[] TMSPBIT;
        byte[] TMSIBIT;

        byte[] som = new byte[4];
        byte[] eom = new byte[4];
        byte[] msgId = new byte[4];
        uint seqNumber;

        public static bool TMSConn = false;

        private DOM_Controller_v40 domControler;
        public delegate void UdpDataReceivedHandler(double az, double el);
        public event UdpDataReceivedHandler UdpDataReceived;

        public void SimulateUdpReceive(double az, double el)
        {
            UdpDataReceived?.Invoke(az, el);
        }

        private void OnUdpDataReceived(double az, double el)
        {
            UdpDataReceived?.Invoke(az, el);
        }
        public struct Homing
        {
            double az;
            double el;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool QueryPerformanceFrequency(out long frequency);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool QueryPerformanceCounter(out long count);
        [DllImport("winmm.dll")]
        private static extern uint timeBeginPeriod(uint uPeriod);
        [DllImport("winmm.dll")]
        private static extern uint timeEndPeriod(uint uPeriod);

        private GroundCalibration_Info groundCalibration_Info;
        public GroundTargetMount()
        {
            InitializeThreads();
            TMS_Value_INIT();

            try
            {
#if LOCALTEST
                TMSEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 7501);
#else
                TMSEndPoint = new IPEndPoint(IPAddress.Parse("192.168.10.21"), 7500);  
#endif
                InitializeData();
                MSG_MSG_ID_MOUNT = MSG_ID_MOUNT_PBIT_CMD;
            }
            catch (Exception ex)
            {
                MessageBox.Show("TMS 디자인 예외");
            }

            domControler = DOM_Controller_v40.instance;

            if (!QueryPerformanceFrequency(out frequency))
            {
                throw new Exception("QueryPerformanceFrequency failed.");
            }

            long currentTime;
            if (!QueryPerformanceCounter(out currentTime))
            {
                throw new Exception("QueryPerformanceCounter failed.");
            }

            lastTime = currentTime;
        }
        public void InitializeData()
        {
#if MOUNTTEST
            azimuthData[0] = 0.001;
            azimuthData[1] = 0.002;
            azimuthData[2] = 0.003;
            azimuthData[3] = 0.004;
            azimuthData[4] = 0.005;
            azimuthData[5] = 0.006;
            azimuthData[6] = 0.007;
            azimuthData[7] = 0.008;
            elevationData[0] = 20.001;
            elevationData[1] = 20.002;
            elevationData[2] = 20.003;
            elevationData[3] = 20.004;
            elevationData[4] = 20.005;
            elevationData[5] = 20.006;
            elevationData[6] = 20.007;
            elevationData[7] = 20.008;
#else
            azimuthData[0] = 0.0;
            azimuthData[1] = 0.0;
            azimuthData[2] = 0.0;
            azimuthData[3] = 0.0;
            azimuthData[4] = 0.0;
            azimuthData[5] = 0.0;
            azimuthData[6] = 0.0;
            azimuthData[7] = 0.0;
            elevationData[0] = 0.0;
            elevationData[1] = 0.0;
            elevationData[2] = 0.0;
            elevationData[3] = 0.0;
            elevationData[4] = 0.0;
            elevationData[5] = 0.0;
            elevationData[6] = 0.0;
            elevationData[7] = 0.0;
#endif
        }
        public void UdpClientChk()
        {
            try
            {
                if (ServerTMS != null)
                {
                    //ServerTMS.Client.Close();
                    ServerTMS.Close();
                }
            }
            catch { }
        }
        public void State()
        {
            if (ServerTMS != null)
            {
                ServerTMS.Close();
                ServerTMS = null;
            }

            if (ServerTMS == null)
            {
                ServerTMS = new UdpClient(5501);
                InitializeData();
                TMS_Value_INIT();
                ServerTMS.Client.SendBufferSize = 65536;
                ServerTMS.Client.ReceiveBufferSize = 65536;
            }
            timeBeginPeriod(1);
            flag = false;

            try
            {
                if (flag == true)
                {
                    flag = false;
                    DataUpdating(MSG_ID_MOUNT_PBIT_CMD);
                }
                else if (isRunning1 == false)
                {
                    DataUpdating(MSG_ID_MOUNT_PBIT_CMD);
                }
                StartReceiveLoop();
            }
            catch
            {
            }
        }
        private void ExecuteWithHighPrecisionTiming(int periodMs, Action action)
        {
            long frequency;
            if (!QueryPerformanceFrequency(out frequency))
            {
                Console.WriteLine("QueryPerformanceFrequency failed.");
                return;
            }

            long ticksPerMs = frequency / 1000;
            long periodTicks = ticksPerMs * periodMs;

            long startCounter;
            QueryPerformanceCounter(out startCounter);

            while (isRunning2)
            {
                long currentCounter;
                QueryPerformanceCounter(out currentCounter);

                long elapsedTicks = currentCounter - startCounter;

                if (elapsedTicks >= periodTicks)
                {
                    startCounter += periodTicks;
                    action();

                    if (currentCounter - startCounter > periodTicks)
                    {
                        startCounter = currentCounter;
                    }
                }
            }
        }


        public void Dispose()
        {
            if (thread1 != null && thread1.IsAlive && isRunning1)
            { StopThread1(); }
            if (isRunning2)
            { StopThread2(); }
            timeEndPeriod(1);
        }


        private void InitializeThreads()
        {
            isRunning1 = false;
            isRunning2 = false;
        }
        public void TMS_Value_INIT()
        {
            TMSoperatingMode = 0;
            TMScontrolCommand = 0;
            TMSPacketTime = 0;
            TMSAZPositon = 0;
            TMSELPosition = 0;
            TMSUsePacketTime = 0;
            TMSCBIT = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            TMSPBIT = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            TMSIBIT = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            controlCommand = TrackingMount.ServoDisable;
            operatingMode = TrackingMount.InitEndState;
            seqNumber = 0;

        }

        public TMSUserData CollectData()
        {
            TMSUserData userData = new TMSUserData
            {
                TMSoperatingMode = TMSoperatingMode,
                TMScontrolCommand = TMScontrolCommand,
                TMSPacketTime = TMSPacketTime,
                TMSAZPosition = TMSAZPositon,
                TMSELPosition = TMSELPosition,
                TMSUsePacketTime = TMSUsePacketTime,
                TMSCBIT = TMSCBIT,
                TMSPBIT = TMSPBIT,
                TMSIBIT = TMSIBIT
            };
            return userData;
        }

        #region 수신부
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        public void StartReceiveLoop()
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            Task.Run(() => ReceiveLoop(cancellationToken));
        }

        public void StopReceiveLoop()
        {
            cancellationTokenSource.Cancel();
        }
        private async Task ReceiveLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    if (ServerTMS != null && ServerTMS.Client != null)
                    {
                        try
                        {
                            UdpReceiveResult result = await ServerTMS.ReceiveAsync();

                            byte[] data = result.Buffer;
                            IPEndPoint remoteEndPoint = result.RemoteEndPoint;

                            using (MemoryStream stream = new MemoryStream(data))
                            using (BinaryReader reader = new BinaryReader(stream))
                            {
                                som = reader.ReadBytes(4).Reverse().ToArray();

                               // Console.WriteLine(BitConverter.ToString(som));
                                msgId = reader.ReadBytes(4).Reverse().ToArray();

                               // Console.WriteLine(BitConverter.ToString(msgId));
                                byte[] seqNumberBytes = reader.ReadBytes(sizeof(int));
                                Array.Reverse(seqNumberBytes);
                                int seqNumber = BitConverter.ToInt32(seqNumberBytes, 0);

                               // Console.WriteLine($"{seqNumber}");
                                int payloadDataLength = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)).ToArray(), 0);
                                //Console.WriteLine($"{payloadDataLength}");
                                byte[] payloadData = reader.ReadBytes(payloadDataLength);

                                eom = reader.ReadBytes(4).Reverse().ToArray();
                                //Console.WriteLine(BitConverter.ToString(eom));
                                if (ByteArrayEquals(som, MOUNT_SOM) && ByteArrayEquals(eom, MOUNT_EOM))
                                {
                                    if (!flag && ByteArrayEquals(msgId, MSG_ID_MOUNT_PBIT_RSP))
                                    {
                                        ProcessPayloadData(payloadData);
                                        DataUpdating(MSG_ID_MOUNT_OP_CMD);
                                        flag = true;
                                        TMSConn = true;
                                    }
                                    else
                                    {
                                        ProcessPayloadData(payloadData);
                                    }
                                }
                            }
                        }
                        catch (SocketException ex)
                        {
                            Console.WriteLine($"SocketException: {ex.Message}");
                            break;
                        }
                    }

                    await Task.Delay(10, token);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("TMS doesnt Received.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in ReceiveLoop: {ex.Message}");
            }
            finally
            {
                if (ServerTMS != null)
                {
                    ServerTMS.Close();
                    // ServerTMS = null;
                }
            }
        }
        private void ProcessPayloadData(byte[] payloadData)
        {
            using (MemoryStream stream = new MemoryStream(payloadData))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                if (ByteArrayEquals(msgId, MSG_ID_MOUNT_OP_RSP))
                {
                    TMSoperatingMode = BitConverter.ToUInt16(reader.ReadBytes(sizeof(ushort)).ToArray(), 0);     // ushort
                    TMScontrolCommand = BitConverter.ToUInt16(reader.ReadBytes(sizeof(ushort)).ToArray(), 0);    // ushort
                                                                                                                 //   TMSPacketTime = BitConverter.ToDouble(reader.ReadBytes(sizeof(double)).ToArray(), 0);        // double -> uint 로 변경
                    TMSPacketTime = BitConverter.ToUInt32(reader.ReadBytes(sizeof(UInt32)).ToArray(), 0);        // double
                    lock (Observation_TMS.LockObj)
                    {
                    TMSAZPositon = BitConverter.ToDouble(reader.ReadBytes(sizeof(double)).ToArray(), 0);         // double
                    TMSELPosition = BitConverter.ToDouble(reader.ReadBytes(sizeof(double)).ToArray(), 0);        // double
                    }
                    TMSUsePacketTime = BitConverter.ToDouble(reader.ReadBytes(sizeof(double)).ToArray(), 0);       // double
                    TMSCBIT = reader.ReadBytes(4);                                                               // byte[4] ( ICD : 2바이트 )
                    OnUdpDataReceived(TMSAZPositon, TMSELPosition);
                }
                else if (ByteArrayEquals(msgId, MSG_ID_MOUNT_PBIT_RSP))
                {
                    TMSPBIT = reader.ReadBytes(4);  // byte[4] ( ICD : 2바이트 )
                }
                else if (ByteArrayEquals(msgId, MSG_ID_MOUNT_IBIT_RSP))
                {
                    TMSIBIT = reader.ReadBytes(4); // byte[4] ( ICD : 2바이트 )
                }
            }
        }
        private bool ByteArrayEquals(byte[] arr1, byte[] arr2)
        {
            if (arr1.Length != arr2.Length)
                return false;

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i] != arr2[i])
                    return false;
            }

            return true;
        }
        #endregion 수신부
        #region 송신부
        double debugcount = 0;
        uint[] azimuthTimestamps = new uint[8];
        double[] pointingTimestamp = new double[8];
        double[] currenttime = new double[8];
        DateTime currentUTC;
        DateTime currentUTCDate;
        public static double currentUTCMillis;
        uint packetTimestamp;
        public static uint[] Timestamp = new uint[8];
        DateTime baseTime;
        bool TrackingReadyFlag = false;
        private double[][] positionData;
        private object[][] satelliteInfo;
        public static double domePeriodStarPointingTime;
        private double domePointingInterval = 60000;

        private byte[] GetRandomData(byte[] mode)
        {
            if (mode == MSG_ID_MOUNT_OP_CMD)
            {
                int dataSize = 184;

                byte[] payloadData = new byte[dataSize];
                using (MemoryStream stream = new MemoryStream(payloadData))
                using (BinaryWriter writer = new BinaryWriter(stream))
                {

                    currentUTC = DateTime.UtcNow;
                    currentUTCDate = currentUTC.Date;
                    currentUTCMillis = (currentUTC - currentUTCDate).TotalMilliseconds;
                    domePeriodStarPointingTime = currentUTCMillis;

                    for (int i = 0; i < pointingTimestamp.Length; i++)
                    {
                        pointingTimestamp[i] = currentUTCMillis + TrackingReadyTime;
                    }

                    
                    for (int i = 0; i < Timestamp.Length; i++)
                    {
                        Timestamp[i] = (uint)currentUTCMillis;

                    }
                    packetTimestamp = Timestamp[0];
                    if (currentUTCMillis >= pointingTimestamp[0] && TMSCBIT[1] == 0x00)
                    {
                        controlCommand = TrackingMount.ServoDisable;
                    }

                    writer.Write(operatingMode);
                    writer.Write(controlCommand);
                    writer.Write(packetTimestamp);
                    for (int i = 0; i < 8; i++)
                    {
                        writer.Write(Timestamp[i]);
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        writer.Write(azimuthData[i]);
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        writer.Write(elevationData[i]);
                    }
                    writer.Write(azUserOffset);
                    writer.Write(elUserOffset);
                }
                return payloadData;
            }
            return MOUNT_OP_CHECK;
        }
        
        public byte GetTMSCBIT()
        {
            return TMSCBIT[1];
        }

        public static void ConvertToDMS(double degrees)
        {
            int d = (int)degrees;

            double fractionalPart = Math.Abs(degrees - d);
            double minutesFull = fractionalPart * 60;
            int m = (int)minutesFull;

            double secondsFull = (minutesFull - m) * 60;
            double s = secondsFull;
            //  Console.WriteLine($"{d}° {m}' {s:0.##}");
        }

        private bool CheckStability(double az, double el)
        {
            if (az > 330 || az < -330)
            //if (az > 320 || az < -320)
            {
                controlCommand = TrackingMount.ServoDisable;
                // MessageBox.Show($"Warning!!! Az 값 허용범위 초과 - {az}");
                starStep = StarCalInfo.Step.None;

                return false;
            }
            if (el > 85 || el < 20)
            {
                controlCommand = TrackingMount.ServoDisable;
                // MessageBox.Show($"Warning!!! El 값 허용범위 초과 - {el}");
                starStep = StarCalInfo.Step.None;

                return false;
            }
            return true;
        }

        #endregion
        #region 버튼 이벤트 처리
        private void MountMode(object sender, EventArgs e)
        {
            Control button = sender as Button;
            if (button != null)
            {
                switch (Convert.ToInt16(button.Tag))
                {
                    case 1:
                        DataUpdating(MSG_ID_MOUNT_OP_CMD);
                        break;
                }
            }
        }
    
        double count = 0.001;
        private void AzAngle_up(object sender, EventArgs e)
        {

            /*          for (var i = 0; i < 8; i++)
                      {
                          azimuthData[i]+=0.001;
                      }*/
            for (var i = 0; i < 8; i++)
            {
                azimuthData[i] = azimuthData[i] + (i + 1) * count;
            }
        }

        private void AzAngle_Down(object sender, EventArgs e)
        {
            for (var i = 0; i < 8; i++)
            {
                azimuthData[i] -= 0.001;
            }
        }
        double count2 = 0.001;

        private void Offset_Up(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (Convert.ToInt32(btn.Tag))
            {
                case 1: // 
                    azUserOffset += UserOffsetTick;
                    break;
                case 2: // 
                    elUserOffset += UserOffsetTick;
                    break;
            }
        }
        public static double UserOffsetTick { get; set; } = 1;
        public static double TimeBiasTick { get; set; } = 10;
        private void Offset_Down(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (Convert.ToInt32(btn.Tag))
            {
                case 1: // 
                    azUserOffset -= UserOffsetTick;
                    break;
                case 2: // 
                    elUserOffset -= UserOffsetTick;
                    break;
            }
        }
        #endregion
        #region 로직 처리
        private void StartThread1()
        {
            if (!isRunning1)
            {
                thread1 = new Thread(Thread1Work);
                thread1.IsBackground = true;
                thread1.Start();
                isRunning1 = true;
            }
        }

        public void Thread1Work()
        {
            while (true)
            {
                PBITCHECK();
                Thread.Sleep(1000);
            }
        }

        private void StopThread1()
        {
            if (isRunning1)
            {
                thread1.Abort();
                isRunning1 = false;
                flag = false;
            }
        }

        private void StopThread2()
        {
            isRunning2 = false;
            flag = false;
        }
        private void DataUpdating(byte[] msg)
        {

            if (msg == MSG_ID_MOUNT_OP_CMD)
            {
                StopThread1();
                StartThread2();
            }
            else if (msg == MSG_ID_MOUNT_PBIT_CMD && flag == false)
            {
                StartThread1();
            }
        }
        private long lastExecutionTime = 0;
        private int executionCount = 0;
        private DateTime lastReportTime = DateTime.Now;
        private void StartThread2()
        {
            if (!isRunning2)
            {
                isRunning2 = true;
                flag = true;

                Task.Run(() =>
                {
                    var timer = new System.Threading.Timer((e) =>
                    {
                        Console.WriteLine($"Executions in last 1 second: {executionCount}");
                        executionCount = 0;
                    }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(200));
                });
                Task.Run(() =>
                {
                    Thread.CurrentThread.Priority = ThreadPriority.Highest;
                    ExecuteWithHighPrecisionTiming(20, () =>
                    {
#if STOPWATCH
                        stopwatch.Reset();
                        stopwatch.Start();
#endif
                        if (!isRunning2) return;

                        PeriodData();

#if STOPWATCH
                        stopwatch.Stop();
#endif
                        long currentTime = Stopwatch.GetTimestamp();
                        if (lastExecutionTime > 0)
                        {
                            double elapsedMs = (currentTime - lastExecutionTime) * 1000.0 / Stopwatch.Frequency;
#if STOPWATCH
                            Console.WriteLine($"Actual interval: {elapsedMs:F4} ms");
#endif
                        }
                        lastExecutionTime = currentTime;

                        executionCount++;
                    });
                });
            }
        }

        public void Thread2Work()
        {
            while (true)
            {
                PeriodData();
            }
        }
        private Stopwatch stopwatch = new Stopwatch();
        private async void PeriodData()
        {
            byte[] seqNumberbytes = BitConverter.GetBytes(seqNumber);
            try
            {
                byte[] PayloadData = GetRandomData(MSG_ID_MOUNT_OP_CMD);
                byte[] commandData = new byte[MOUNT_SOM.Length + MSG_ID_MOUNT_OP_CMD.Length + seqNumberbytes.Length + sizeof(int) + PayloadData.Length + MOUNT_EOM.Length];
                Array.Copy(MOUNT_SOM.Reverse().ToArray(), 0, commandData, 0, MOUNT_SOM.Length);
                Array.Copy(MSG_ID_MOUNT_OP_CMD.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length, MSG_ID_MOUNT_OP_CMD.Length);
                Array.Copy(seqNumberbytes.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length + MSG_ID_MOUNT_OP_CMD.Length, seqNumberbytes.Length);

                byte[] payloadLengthBytes = BitConverter.GetBytes(PayloadData.Length);
                Array.Reverse(payloadLengthBytes);
                Array.Copy(payloadLengthBytes, 0, commandData, MOUNT_SOM.Length + MSG_ID_MOUNT_OP_CMD.Length + seqNumberbytes.Length, sizeof(int));

                Array.Copy(PayloadData, 0, commandData, MOUNT_SOM.Length + MSG_ID_MOUNT_OP_CMD.Length + seqNumberbytes.Length + sizeof(int), PayloadData.Length);
                Array.Copy(MOUNT_EOM.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length + MSG_ID_MOUNT_OP_CMD.Length + seqNumberbytes.Length + sizeof(int) + PayloadData.Length, MOUNT_EOM.Length);

    

                if (ServerTMS != null && TMSEndPoint != null) await ServerTMS.SendAsync(commandData, commandData.Length, TMSEndPoint);
                seqNumber++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        Semaphore Semaphore = new Semaphore(0, 2);
        private async void PBITCHECK()
        {
            if (Semaphore.WaitOne(0)) { return; }
            byte[] seqNumber = { 0x00, 0x00, 0x00, 0x00 };

            byte[] payloadLengthBytes = { 0x00, 0x00, 0x00, 0x00 };
            byte[] commandData = new byte[MOUNT_SOM.Length + MSG_ID_MOUNT_PBIT_CMD.Length + seqNumber.Length + payloadLengthBytes.Length + MOUNT_EOM.Length];
            try
            {
                if (ServerTMS != null && TMSEndPoint != null)
                {
                    Array.Copy(MOUNT_SOM.Reverse().ToArray(), 0, commandData, 0, MOUNT_SOM.Length);
                    Array.Copy(MSG_ID_MOUNT_PBIT_CMD.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length, MSG_ID_MOUNT_PBIT_CMD.Length);
                    Array.Copy(seqNumber.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length + MSG_ID_MOUNT_PBIT_CMD.Length, seqNumber.Length);
                    Array.Reverse(payloadLengthBytes);
                    Array.Copy(payloadLengthBytes, 0, commandData, MOUNT_SOM.Length + MSG_ID_MOUNT_PBIT_CMD.Length + seqNumber.Length, payloadLengthBytes.Length);
                    Array.Copy(MOUNT_EOM.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length + MSG_ID_MOUNT_PBIT_CMD.Length + seqNumber.Length + payloadLengthBytes.Length, MOUNT_EOM.Length);


                    await ServerTMS.SendAsync(commandData, commandData.Length, TMSEndPoint);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                Semaphore.Release();
            }
        }
        #endregion
        double[][] AzElData;
        double[] timestamps;
        public static double[][] AzElData_Dome;
        public static double[] timestamps_dome;
        public void groundTargetPoiting(double Az, double El)
        {
            operatingMode = TrackingMount.ReadyState;
            controlCommand = TrackingMount.PointingMode;
            azUserOffset = Az;
            elUserOffset = El;
        }
    }
}
