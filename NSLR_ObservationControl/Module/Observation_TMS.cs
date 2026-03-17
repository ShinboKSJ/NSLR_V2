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
    public partial class Observation_TMS : UserControl
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
        public static readonly object LockObj = new object();

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
        private LAS_SAT_Controller lasSATcontrol = LAS_SAT_Controller.instance;
        private LAS_DEB_Controller lasDEBcontrol = LAS_DEB_Controller.instance;
        private RGG_SAT_Controller satRGGcontrol = RGG_SAT_Controller.instance;
        private RGG_DEB_Controller debRGGcontrol = RGG_DEB_Controller.instance;
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


        public Observation_TMS()
        {
            InitializeComponent();
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

        public tcspk _tcspk;
        public tcspk ExternalLibraryObject
        {
            get { return _tcspk; }
            set { _tcspk = value; }
        }


        private Test_SatelliteAZEL _satelliteAZEL;
        public Test_SatelliteAZEL _satel_ExternalLibrary
        {
            get { return _satelliteAZEL; }
            set { _satelliteAZEL = value; }
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
        public void TMSPause()
        {
            controlCommand = TrackingMount.ServoDisable;
            flag = false;
        }
        private void Observation_TMS_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {


            }
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
        /*        public UserData CollectData()
                {
                    UserData userData = new UserData
                    {
                        TMSoperatingMode = TMSoperatingMode,
                        TMScontrolCommand = TMScontrolCommand,
                        TMSPacketTime = TMSPacketTime,
                        TMSAZPositon = TMSAZPositon,
                        TMSELPosition = TMSELPosition,
                        TMSCBIT = TMSCBIT,
                        TMSPBIT = TMSPBIT,
                        TMSIBIT = TMSIBIT,

                    };
                    return userData;
                }*/
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
                                msgId = reader.ReadBytes(4).Reverse().ToArray();

                                byte[] seqNumberBytes = reader.ReadBytes(sizeof(int));
                                Array.Reverse(seqNumberBytes);
                                int seqNumber = BitConverter.ToInt32(seqNumberBytes, 0);

                                int payloadDataLength = BitConverter.ToInt32(reader.ReadBytes(sizeof(int)).ToArray(), 0);
                                byte[] payloadData = reader.ReadBytes(payloadDataLength);

                                eom = reader.ReadBytes(4).Reverse().ToArray();

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

                    TMSAZPositon = BitConverter.ToDouble(reader.ReadBytes(sizeof(double)).ToArray(), 0);         // double
                    TMSELPosition = BitConverter.ToDouble(reader.ReadBytes(sizeof(double)).ToArray(), 0);        // double
                    TMSUsePacketTime = BitConverter.ToDouble(reader.ReadBytes(sizeof(double)).ToArray(), 0);       // double
                    TMSCBIT = reader.ReadBytes(4);                                                               // byte[4] ( ICD : 2바이트 )
                                                                                                                 //실제 비트는 10~27로 매핑
                    uint cbits = BitConverter.ToUInt32(TMSCBIT, 0);
                    (bool state, string desc)[] bitStatus =
         {
    ((cbits & (1u << 10)) != 0, "bit00 : AZ 위치제어오차 초과"),
    ((cbits & (1u << 11)) != 0, "bit01 : EL 위치제어오차 초과"),
    ((cbits & (1u << 12)) != 0, "bit02 : AZ 비정상 step 명령 인가"),
    ((cbits & (1u << 13)) != 0, "bit03 : EL 비정상 step 명령 인가"),
    ((cbits & (1u << 14)) != 0, "bit04 : AZ 구동 속도 초과"),
    ((cbits & (1u << 15)) != 0, "bit05 : EL 구동 속도 초과"),
    ((cbits & (1u << 16)) != 0, "bit06 : AZ 구동 전류 초과"),
    ((cbits & (1u << 17)) != 0, "bit07 : EL 구동 전류 초과"),
    ((cbits & (1u << 18)) != 0, "bit08 : AZ 전기적 구동 범위 초과"),
    ((cbits & (1u << 19)) != 0, "bit09 : EL 전기적 구동 범위 초과"),
    ((cbits & (1u << 20)) != 0, "bit10 : AZ CW Limit sensor 고장"),
    ((cbits & (1u << 21)) != 0, "bit11 : AZ CCW Limit sensor 고장"),
    ((cbits & (1u << 22)) != 0, "bit12 : EL UP Limit sensor 고장"),
    ((cbits & (1u << 23)) != 0, "bit13 : EL DOWN Limit sensor 고장"),
    ((cbits & (1u << 24)) != 0, "bit14 : AZ CW Limit sensor 감지"),
    ((cbits & (1u << 25)) != 0, "bit15 : AZ CCW Limit sensor 감지"),
    ((cbits & (1u << 26)) != 0, "bit16 : EL UP Limit sensor 감지"),
    ((cbits & (1u << 27)) != 0, "bit17 : EL DOWN Limit sensor 감지"),
};

                    foreach (var (state, desc) in bitStatus)
                    {
                        if (state)
                            Console.WriteLine($"[CBIT] {desc}");
                    }
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
                    if (StarCalInfo.StarCalFlag)
                    {
                        if (starStep == StarCalInfo.Step.One)
                        {
                            currentUTC = DateTime.UtcNow;
                            currentUTCDate = currentUTC.Date;
                            currentUTCMillis = (currentUTC - currentUTCDate).TotalMilliseconds;
                            domePeriodStarPointingTime = currentUTCMillis;
                            button27.BackColor = Color.Green;
                            button28.BackColor = Color.Green;
                            button29.BackColor = Color.Green;
                            button30.BackColor = Color.Green;
                            CSU_StarCalibration.AddAzOffSet = 0;
                            CSU_StarCalibration.AddElOffSet = 0;

                            if (StarCalInfo.StarStartFlag == true)
                            {
                                var position = new double[2];
                                for (int i = 0; i < pointingTimestamp.Length; i++)
                                {
                                    pointingTimestamp[i] = currentUTCMillis + TrackingReadyTime;
                                }
                                StarCalInfo.StarStartFlag = false;

                                position = _tcspk.ObtainTargetPointed(currentUTCDate, pointingTimestamp[0]);
                                /*         if (position[0] == -1)
                                         {
                                             controlCommand = TrackingMount.ServoDisable;

                                             starStep = StarCalInfo.Step.None;
                                         }*/
                                var SbCheck = CheckStability(position[0], position[1]);
                                if (SbCheck == false)
                                {
                                    //MessageBox.Show("Over Range");
                                    controlCommand = TrackingMount.ServoDisable;
                                }

                                else
                                {
                                    controlCommand = TrackingMount.PointingMode;
                                    azUserOffset = position[0] + sync.Azsync;
                                    elUserOffset = position[1] + sync.Elsync;
                                    UpdateLabel(label4, azUserOffset.ToString("F5"));
                                    UpdateLabel(label5, elUserOffset.ToString("F5"));
                                    domControler.doPositionMove(azUserOffset, elUserOffset); //JcK
                                }

                            }
                            for (int i = 0; i < Timestamp.Length; i++)
                            {
                                Timestamp[i] = (uint)currentUTCMillis;

                            }
                            packetTimestamp = Timestamp[0];
                            if (currentUTCMillis >= pointingTimestamp[0] && TMSCBIT[1] == 0x00)
                            {
                                starStep = StarCalInfo.Step.Two;
                                TrackingReadyFlag = true;
                            }
                        }
                        else if (starStep == StarCalInfo.Step.Two)
                        {

                            var positionData = new double[2][];
                            for (int i = 0; i < positionData.Length; i++)
                            {
                                positionData[i] = new double[8];
                            }
                            currentUTC = DateTime.UtcNow;
                            currentUTCDate = currentUTC.Date;
                            currentUTCMillis = (currentUTC - currentUTCDate).TotalMilliseconds;
                            packetTimestamp = (uint)currentUTCMillis;
                            double[] offsets = { -70, -50, -30, -10, 10, 30, 50, 70 };

                            positionData = _tcspk.ObtainTargetTracked(currentUTCDate, packetTimestamp);
                            var SbCheck = CheckStability(positionData[0][0] + sync.Azsync, positionData[1][0] + sync.Elsync);
                            if (SbCheck == false)
                            {
                                StarEndTracking();
                                MessageBox.Show("Over Range");
                            }
                            else
                            {
                                for (int i = 0; i < 8; i++)
                                {
                                    uint timestamp = (uint)(Convert.ToInt32(Math.Floor(currentUTCMillis)) + offsets[i]);
                                    Timestamp[i] = timestamp;
                                    azimuthData[i] = positionData[0][i] + sync.Azsync;
                                    elevationData[i] = positionData[1][i] + sync.Elsync;
                                    UpdateLabel(label6, azimuthData[i].ToString("F5"));
                                    UpdateLabel(label7, elevationData[i].ToString("F5"));
                                    double Azoffset = CSU_StarCalibration.AddAzOffSet;
                                    if (Math.Abs(Azoffset) < 1e-10)
                                    {
                                        Azoffset = 0.0;
                                    }
                                    UpdateLabel(label8, $"Az offset - {Azoffset:F5} deg");
                                    double Eloffset = CSU_StarCalibration.AddElOffSet;
                                    if (Math.Abs(Eloffset) < 1e-10)
                                    {
                                        Eloffset = 0.0;
                                    }
                                    UpdateLabel(label9, $"El offset - {Eloffset:F5} deg");
                                    if (domePeriodStarPointingTime + domePointingInterval < packetTimestamp)
                                    {
                                        domePeriodStarPointingTime = packetTimestamp;
                                        domControler.doPositionMove(azimuthData[0] < 0 ? azimuthData[0] + 360 : azimuthData[0], elevationData[i]);
                                    }
                                }

                            }

                            if (TrackingReadyFlag)
                            {
                                operatingMode = TrackingMount.OperatingState;
                                controlCommand = TrackingMount.TrackingMode;
                                TrackingReadyFlag = false;
                                azUserOffset = 0;
                                elUserOffset = 0;
                            }
                            azUserOffset = CSU_StarCalibration.AddAzOffSet;
                            elUserOffset = CSU_StarCalibration.AddElOffSet;
                            /*   if (debuggingRunning)
                               {
                                   SaveParsedData(packetTimestamp, azimuthData, elevationData);
                               }*/
                        }
                        else if (starStep == StarCalInfo.Step.Three)
                        {

                        }
                        else if (starStep == StarCalInfo.Step.None)
                        {
                            Console.WriteLine("pointing mode Az 범위 벗어남");
                        }
                        UpdateLabel(label10, TMSAZPositon.ToString("F5"));
                        UpdateLabel(label11, TMSELPosition.ToString("F5"));
                        writer.Write(operatingMode);
                        writer.Write(controlCommand);
                        writer.Write(packetTimestamp);
                        for (int i = 0; i < 8; i++)
                        {
                            writer.Write(Timestamp[i]);
                        }
                        for (int i = 0; i < 8; i++)
                        {
                            ConvertToDMS(azimuthData[i]);
                            writer.Write(azimuthData[i]);
                        }
                        for (int i = 0; i < 8; i++)
                        {
                            ConvertToDMS(elevationData[i]);
                            writer.Write(elevationData[i]);
                        }
                        writer.Write(azUserOffset);
                        writer.Write(elUserOffset);
                    }
                    else if (TargetInfo.TargetFlag)
                    {
                        if (TargetStep == TargetInfo.Step.One)
                        {
                            currentUTC = DateTime.UtcNow;
                            currentUTCDate = currentUTC.Date;
                            currentUTCMillis = (currentUTC - currentUTCDate).TotalMilliseconds;
                            TimeBiasValue = 0;
                            satRGGcontrol.RggTimeBias = 0;
                            debRGGcontrol.RggTimeBias = 0;
                            CSU_Observation.AddAzOffSet = 0;
                            CSU_Observation.AddElOffSet = 0;
                            button27.BackColor = Color.Green;
                            button28.BackColor = Color.Green;
                            button29.BackColor = Color.Green;
                            button30.BackColor = Color.Green;
                            UpdateLabel(CurrentTimeBiasValue, TimeBiasValue);
                            if (timestamps == null)
                            {
                                MessageBox.Show("timestamps 데이터가 없습니다. 데이터를 확인하고 다시 시도해주세요.", "데이터 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                EndTracking();
                                TargetInfo.TargetStartFlag = false;
                            }
                            else if (TargetInfo.TargetStartFlag == true)
                            {
                                var position = new double[2];
                                for (int i = 0; i < pointingTimestamp.Length; i++)
                                {
                                    pointingTimestamp[i] = currentUTCMillis + TrackingReadyTime;
                                }
                                TargetInfo.TargetStartFlag = false;

                                (position[0], position[1]) = CurrentPointingPosition(currentUTCMillis);

                                var SbCheck = CheckStability(position[0], position[1]);
                                //    var SbCheck = true;
                                if (SbCheck == false)
                                {
                                    //MessageBox.Show("Over Range");
                                }
#if MOUNTTEST
                                else
                                {
                                    azUserOffset = 0;
                                    elUserOffset = 20.1;
                                    UpdateLabel(label4, azUserOffset);
                                    UpdateLabel(label5, elUserOffset);
                                }
#else
                                else
                                {
                                    controlCommand = TrackingMount.PointingMode;
                                    azUserOffset = position[0] + sync.Azsync;
                                    elUserOffset = position[1] + sync.Elsync;
                                    UpdateLabel(label4, azUserOffset.ToString("F5"));
                                    UpdateLabel(label5, elUserOffset.ToString("F5"));
                                    domControler.doPositionMove(azUserOffset, elUserOffset); //JcK 
                                                                                             //domControler.doTracking(azUserOffset); //JcK (StarCalInfo.StarCalFlag)
                                }
#endif
                            }
                            for (int i = 0; i < Timestamp.Length; i++)
                            {
                                Timestamp[i] = (uint)currentUTCMillis;

                            }
                            packetTimestamp = Timestamp[0];
                            if (currentUTCMillis >= pointingTimestamp[0] && TMSCBIT[1] == 0x00)
                            {
                                TargetStep = TargetInfo.Step.Two;
                                TrackingReadyFlag = true;

                            }
                        }

                        else if (TargetStep == TargetInfo.Step.Two)
                        {
                            if (positionData == null)
                            {
                                positionData = new double[2][];
                                for (int i = 0; i < positionData.Length; i++)
                                {
                                    positionData[i] = new double[8];
                                }
                            }
                            for (int i = 0; i < positionData[0].Length; i++)
                            {
                                positionData[0][i] = 0.0;
                                positionData[1][i] = 0.0;
                            }
                            currentUTC = DateTime.UtcNow;
                            currentUTCDate = currentUTC.Date;
                            packetTimestamp = (uint)Math.Floor((currentUTC - currentUTCDate).TotalMilliseconds);

                            (Timestamp, positionData) = CurrentTrackingPosition(packetTimestamp + Constants.DataLeadTime);

#if MOUNTTEST
                            addMillis = new double[] { -70, -50, -30, -10, 10, 30, 50, 70 };
                            for (int i = 0; i < 8; i++)
                            {

                                double totalMilliseconds = (futureUTC.AddMilliseconds(addMillis[i]) - currentUTCDate).TotalMilliseconds;
                                Timestamp[i] = (uint)Math.Floor(totalMilliseconds);
                                azimuthData[i] += 0.001; 
                                elevationData[i] += 0.001;

                            }
#else

                            //var SbCheck = CheckStability(positionData[0][0] + CSU_Observation.AddAzOffSet, positionData[1][0] + CSU_Observation.AddElOffSet);
                            var SbCheck = CheckStability(positionData);
                            // var SbCheck = true;
                            if (SbCheck == false)
                            {
                                //  MessageBox.Show("Over Range");
                                EndTracking();
                            }
                            else
                            {
                                for (int i = 0; i < 8; i++)
                                {

                                    azimuthData[i] = positionData[0][i];
                                    if (ObservingSatellite_Information.observinSatellite_keyhole && isInKeyhole)
                                    {
                                        if (positionData[1][i] + CSU_Observation.AddElOffSet >= 85)
                                        {
                                            elevationData[i] = 85.0 - CSU_Observation.AddElOffSet;
                                        }
                                        else
                                        {
                                    elevationData[i] = positionData[1][i];
                                }
                                    }
                                    else
                                    {
                                        elevationData[i] = positionData[1][i];
                                    }
                                }


                                DateTime timestampDateTime = DateTimeOffset.FromUnixTimeMilliseconds((long)Timestamp[3]).UtcDateTime;
                                if (ObservingSatellite_Information.observingSatellite_endTime < timestampDateTime)
                                {
                                    EndTracking();
                                }
                            }
#endif
                            if (TrackingReadyFlag)
                            {
                                operatingMode = TrackingMount.OperatingState;
                                controlCommand = TrackingMount.TrackingMode;
                                TrackingReadyFlag = false;
                                azUserOffset = 0;
                                elUserOffset = 0;
                            }
                            azUserOffset = CSU_Observation.AddAzOffSet + sync.Azsync;
                            elUserOffset = CSU_Observation.AddElOffSet + sync.Elsync;
                        }
                        else if (TargetStep == TargetInfo.Step.Three)
                        {

                        }
                        else if (TargetStep == TargetInfo.Step.None)
                        {

                        }

                        UpdateLabel(CurrentTimeBiasValue, TimeBiasValue);
                        UpdateLabel(label6, azimuthData[0].ToString("F5"));
                        UpdateLabel(label7, elevationData[0].ToString("F5"));
                        double Azoffset = CSU_Observation.AddAzOffSet;
                        if (Math.Abs(Azoffset) < 1e-10)
                        {
                            Azoffset = 0.0;
                        }
                        UpdateLabel(label8, $"Az offset - {Azoffset:F5} deg");
                        double Eloffset = CSU_Observation.AddElOffSet;
                        if (Math.Abs(Eloffset) < 1e-10)
                        {
                            Eloffset = 0.0;
                        }
                        UpdateLabel(label9, $"El offset - {Eloffset:F5} deg");
                        UpdateLabel(label10, TMSAZPositon.ToString("F5"));
                        UpdateLabel(label11, TMSELPosition.ToString("F5"));
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

                    else
                    {
                        DateTime currentTimeUtc = DateTime.UtcNow;
                        DateTime currentDateUtc = currentTimeUtc.Date;

                        double currentTimeMillis = (currentTimeUtc - currentDateUtc).TotalMilliseconds;

                        packetTimestamp = (uint)currentTimeMillis;


                        for (int i = 0; i < 8; i++)
                        {
                            currentTimeUtc = DateTime.UtcNow;
                            long currentTime = (long)(currentTimeUtc - currentDateUtc).TotalMilliseconds;
                            azimuthTimestamps[i] = (uint)currentTime;
                        }


                        writer.Write(operatingMode);
                        writer.Write(controlCommand);
                        writer.Write(packetTimestamp);
                        double[] offsets = { -70, -50, -30, -10, 10, 30, 50, 70 };

                        for (int i = 0; i < 8; i++)
                        {
                            uint timestamp = (uint)(Convert.ToInt32(Math.Floor(currentUTCMillis)) + offsets[i]);
                            azimuthTimestamps[i] = timestamp;

                        }
                        for (int i = 0; i < 8; i++)
                        {
                            writer.Write(azimuthTimestamps[i]);
                        }
                        if (controlCommand == 0x0200)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                azimuthData[i] = azimuthData[i] + 0.00000001;
                                // azimuthData[i] = azimuthData[i]+azUserOffset;
                              //  Console.WriteLine(azimuthData[i]);
                                writer.Write(azimuthData[i]);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                writer.Write(azimuthData[i]);
                            }
                        }
                        for (int i = 0; i < 8; i++)
                        {
                            writer.Write(elevationData[i]);
                        }
                        writer.Write(azUserOffset);
                        writer.Write(elUserOffset);

                        UpdateLabel(CurrentTimeBiasValue, TimeBiasValue);
                    }

                }
                /// operatingMode++; 
                // controlCommand = (controlCommand == (ushort)0x0100) ? (ushort)0x0200 : (controlCommand == (ushort)0x0200) ? (ushort)0x0301 : (controlCommand == (ushort)0x0301) ? (ushort)0x0302 : (controlCommand == (ushort)0x0302) ? (ushort)0x0400 : (ushort)0x0100;
                //  if (operatingMode == (ushort)6) operatingMode = (ushort)1;
                return payloadData;
            }
            /*            else if (mode == MSG_ID_MOUNT_PBIT_CMD)
                        {
                            byte[] payloadData = { 0x00, 0x00, 0x00, 0x00 };
                            return payloadData;
                        }
                        else if (mode == MSG_ID_MOUNT_IBIT_CMD)
                        {
                            byte[] payloadData = { 0x00, 0x00, 0x00, 0x00 };
                            return payloadData;
                        }*/
            return MOUNT_OP_CHECK;

        }
        public void EndTracking()
        {
            domControler.doStop();
            TargetInfo.TargetFlag = false;
            TargetInfo.TargetStartFlag = true;
            TargetStep = TargetInfo.Step.None;
            controlCommand = TrackingMount.ServoDisable;
            TrackingReadyFlag = false;

            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            {
                lasSATcontrol.laserStop_SatTrack();
            }
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                lasDEBcontrol.laserStop_DebTrack();
            }
        }
        public void StarEndTracking()
        {
            domControler.doStop();
            StarCalInfo.StarCalFlag = false;
            StarCalInfo.StarStartFlag = true;
            starStep = StarCalInfo.Step.None;
            controlCommand = TrackingMount.ServoDisable;
            TrackingReadyFlag = false;
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
        public void ConvertToDMS1(double degrees, System.Windows.Forms.Label label)
        {
            int d = (int)degrees < 0 ? 360 + (int)degrees : (int)degrees;

            double fractionalPart = degrees - d < 0 ? Math.Abs(degrees - d + 360) : Math.Abs(degrees - d);
            double minutesFull = fractionalPart * 60;
            int m = (int)minutesFull;

            double secondsFull = (minutesFull - m) * 60;
            double s = secondsFull;
            //  Console.WriteLine($"{d}° {m}' {s:0.##}");
            UpdateLabel(label, $"{d}° {m}' {s:0.##}");
        }
        private void CheckStability(double[] az, double[] el)
        {
            for (var i = 0; i < 8; i++)
            {
                if (az[i] > 330 || az[i] < -330)   // 기존 범위 -330~330 이지만 안전상의 문제로 임시로 -270~270까지
                                                   // if (az[i] > 300 || az[i] < -300)
                {
                    controlCommand = TrackingMount.ServoDisable;
                    // MessageBox.Show($"Warning!!! Az 값 허용범위 초과 - {az[i]}");
                    button29.BackColor = Color.Red;
                    starStep = StarCalInfo.Step.None;
                }
                if (el[i] > 85 || el[i] < 20)
                {
                    controlCommand = TrackingMount.ServoDisable;
                    //MessageBox.Show($"Warning!!! El 값 허용범위 초과 - {el[i]}");
                    button30.BackColor = Color.Red;
                    starStep = StarCalInfo.Step.None;
                }
            }
        }
        private bool CheckStability(double az, double el)
        {
            if (az > 330 || az < -330)
            //if (az > 320 || az < -320)
            {
                controlCommand = TrackingMount.ServoDisable;
                // MessageBox.Show($"Warning!!! Az 값 허용범위 초과 - {az}");
                button27.BackColor = Color.Red;
                starStep = StarCalInfo.Step.None;

                return false;
            }
            if (el > 85 || el < 20)
            {
                controlCommand = TrackingMount.ServoDisable;
                // MessageBox.Show($"Warning!!! El 값 허용범위 초과 - {el}");
                button28.BackColor = Color.Red;
                starStep = StarCalInfo.Step.None;

                return false;
            }
            return true;
        }
        private bool isInKeyhole = false;
        private bool CheckStability(double[][] position)
        {
            bool hasKeyholeViolation = false;

            for (var i = 0; i < 8; i++)
            {
                if (position[0][i] + CSU_Observation.AddAzOffSet > 330 ||
                    position[0][i] + CSU_Observation.AddAzOffSet < -330)
                {
                    controlCommand = TrackingMount.ServoDisable;
                    button27.BackColor = Color.Red;
                    return false;
                }
                if (position[1][i] + CSU_Observation.AddElOffSet >= 85 ||
                    position[1][i] + CSU_Observation.AddElOffSet < 20)
                {
                    if (ObservingSatellite_Information.observinSatellite_keyhole)
                    {
                        if (position[1][i] + CSU_Observation.AddElOffSet >= 85)
                        {
                            hasKeyholeViolation = true;
                        }
                        else
                        {
                            controlCommand = TrackingMount.ServoDisable;
                            button28.BackColor = Color.Red;
                    return false;
                }
                    }
                    else
                {
                    controlCommand = TrackingMount.ServoDisable;
                    button28.BackColor = Color.Red;
                    return false;
                }
            }
            }
            if (ObservingSatellite_Information.observinSatellite_keyhole)
            {
                isInKeyhole = hasKeyholeViolation;
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
        private void OperatingMode(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (Convert.ToInt32(btn.Tag))
            {
                case 1: // 초기/종료 상태
                    operatingMode = (ushort)0x01;
                    break;
                case 2: // 준비상태
                    operatingMode = (ushort)0x02;
                    break;
                case 3: // 운용상태
                    operatingMode = (ushort)0x03;
                    break;
                case 4: // 점검상태
                    operatingMode = (ushort)0x04;
                    break;
                case 5: // 안전상태
                    operatingMode = (ushort)0x05;
                    break;
            }
        }

        private void ControlCommand(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (Convert.ToInt32(btn.Tag))
            {
                case 1: // Pointing Mode
                    controlCommand = (ushort)0x0100;
                    break;
                case 2: // Tracking Mode
                    controlCommand = (ushort)0x0200;
                    break;
                case 3: // Servo Enable
                    controlCommand = (ushort)0x0301;
                    break;
                case 4: // Servo Disable
                    controlCommand = (ushort)0x0302;
                    break;
                case 5: // Servo Homing
                    controlCommand = (ushort)0x0400;
                    break;
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
            BeginInvoke(new Action(() =>
            {
                textBox1.Text = azimuthData[0].ToString();
            }));
        }

        private void AzAngle_Down(object sender, EventArgs e)
        {
            for (var i = 0; i < 8; i++)
            {
                azimuthData[i] -= 0.001;
            }
            BeginInvoke(new Action(() =>
            {
                textBox1.Text = azimuthData[0].ToString();
            }));
        }
        double count2 = 0.001;
        /* private void ElAngle_up(object sender, EventArgs e)
         {
             for (var i = 0; i < 8; i++)
             {
                 elevationData[i] = elevationData[i] + (i + 1) * count2;
             }
             BeginInvoke(new Action(() =>
             {
                 textBox2.Text = elevationData[0].ToString();
             }));
         }

         private void ElAngle_Down(object sender, EventArgs e)
         {
             for (var i = 0; i < 8; i++)
             {
                 elevationData[i] -= 0.001;
             }
             BeginInvoke(new Action(() =>
             {
                 textBox2.Text = elevationData[0].ToString();
             }));
         }
 */
        private void PBIT_check(object sender, EventArgs e)
        {
            byte[] seqNumber = { 0x00, 0x00, 0x00, 0x00 };
            if (TMSEndPoint != null)
            {
                byte[] payloadLengthBytes = { 0x00, 0x00, 0x00, 0x00 };
                byte[] commandData = new byte[MOUNT_SOM.Length + MSG_ID_MOUNT_PBIT_CMD.Length + seqNumber.Length + payloadLengthBytes.Length + MOUNT_EOM.Length];

                Array.Copy(MOUNT_SOM.Reverse().ToArray(), 0, commandData, 0, MOUNT_SOM.Length);
                Array.Copy(MSG_ID_MOUNT_PBIT_CMD.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length, MSG_ID_MOUNT_PBIT_CMD.Length);
                Array.Copy(seqNumber.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length + MSG_ID_MOUNT_PBIT_CMD.Length, seqNumber.Length);
                Array.Reverse(payloadLengthBytes);
                Array.Copy(payloadLengthBytes, 0, commandData, MOUNT_SOM.Length + MSG_ID_MOUNT_PBIT_CMD.Length + seqNumber.Length, payloadLengthBytes.Length);
                Array.Copy(MOUNT_EOM.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length + MSG_ID_MOUNT_PBIT_CMD.Length + seqNumber.Length + payloadLengthBytes.Length, MOUNT_EOM.Length);

                ServerTMS.Send(commandData, commandData.Length, TMSEndPoint);
            }
        }

        private void IBIT_check(object sender, EventArgs e)
        {
            byte[] seqNumber = { 0x00, 0x00, 0x00, 0x00 };
            if (TMSEndPoint != null)
            {
                byte[] payloadLengthBytes = { 0x00, 0x00, 0x00, 0x00 };
                byte[] commandData = new byte[MOUNT_SOM.Length + MSG_ID_MOUNT_IBIT_CMD.Length + seqNumber.Length + payloadLengthBytes.Length + MOUNT_EOM.Length];

                Array.Copy(MOUNT_SOM.Reverse().ToArray(), 0, commandData, 0, MOUNT_SOM.Length);
                Array.Copy(MSG_ID_MOUNT_IBIT_CMD.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length, MSG_ID_MOUNT_IBIT_CMD.Length);
                Array.Copy(seqNumber.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length + MSG_ID_MOUNT_IBIT_CMD.Length, seqNumber.Length);
                Array.Reverse(payloadLengthBytes);
                Array.Copy(payloadLengthBytes, 0, commandData, MOUNT_SOM.Length + MSG_ID_MOUNT_IBIT_CMD.Length + seqNumber.Length, payloadLengthBytes.Length);
                Array.Copy(MOUNT_EOM.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length + MSG_ID_MOUNT_IBIT_CMD.Length + seqNumber.Length + payloadLengthBytes.Length, MOUNT_EOM.Length);

                ServerTMS.Send(commandData, commandData.Length, TMSEndPoint);
            }
        }

        private void Offset_Up(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (Convert.ToInt32(btn.Tag))
            {
                case 1: // 
                    azUserOffset += UserOffsetTick;
                    BeginInvoke(new Action(() =>
                    {
                        textBox4.Text = azUserOffset.ToString();
                    }));
                    break;
                case 2: // 
                    elUserOffset += UserOffsetTick;
                    BeginInvoke(new Action(() =>
                    {
                        textBox3.Text = elUserOffset.ToString();
                    }));
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
                    BeginInvoke(new Action(() =>
                    {
                        textBox4.Text = azUserOffset.ToString();
                    }));
                    break;
                case 2: // 
                    elUserOffset -= UserOffsetTick;
                    BeginInvoke(new Action(() =>
                    {
                        textBox3.Text = elUserOffset.ToString();
                    }));
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
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<byte[]>(DataUpdating), msg);
                return;
            }

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

                if (debuggingRunning)
                {
                    SaveParsedData(packetTimestamp, azimuthData, elevationData);
                }

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

            /*                byte[] PayloadData = GetRandomData(MSG_MSG_ID_MOUNT);
                            byte[] commandData = new byte[MOUNT_SOM.Length + MSG_MSG_ID_MOUNT.Length + seqNumber.Length + sizeof(int) + PayloadData.Length + MOUNT_EOM.Length];

                            Array.Copy(MOUNT_SOM.Reverse().ToArray(), 0, commandData, 0, MOUNT_SOM.Length);
                            Array.Copy(MSG_MSG_ID_MOUNT.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length, MSG_MSG_ID_MOUNT.Length);
                            Array.Copy(seqNumber.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length + MSG_MSG_ID_MOUNT.Length, seqNumber.Length);

                            byte[] payloadLengthBytes = BitConverter.GetBytes(PayloadData.Length);
                            Array.Reverse(payloadLengthBytes);
                            Array.Copy(payloadLengthBytes, 0, commandData, MOUNT_SOM.Length + MSG_MSG_ID_MOUNT.Length + seqNumber.Length, sizeof(int));

                            Array.Copy(PayloadData, 0, commandData, MOUNT_SOM.Length + MSG_MSG_ID_MOUNT.Length + seqNumber.Length + sizeof(int), PayloadData.Length);
                            Array.Copy(MOUNT_EOM.Reverse().ToArray(), 0, commandData, MOUNT_SOM.Length + MSG_MSG_ID_MOUNT.Length + seqNumber.Length + sizeof(int) + PayloadData.Length, MOUNT_EOM.Length);

                            MainForm.ServerTMS.Send(commandData, commandData.Length, TMSEndPoint);*/
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
        private void button24_Click(object sender, EventArgs e)
        {
            string ControlSensitive = textBox5.Text;
            if (!string.IsNullOrEmpty(ControlSensitive) &&
                double.TryParse(ControlSensitive, out double Sensitive) &&
                Sensitive >= 0.000001 && Sensitive <= 1)
            {
                UserOffsetTick = Sensitive;
                offset.Text = $"Offset 민감도 - {UserOffsetTick}";
            }
            else
            {
                MessageBox.Show("0.000001 ~ 1 사이의 값을 입력하세요.");
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            string AzOffset = textBox4.Text;
            string ElOffset = textBox3.Text;
            if (!string.IsNullOrEmpty(AzOffset) &&
                double.TryParse(AzOffset, out double Sensitive) &&
                Sensitive >= -330.0 && Sensitive <= 330 && !string.IsNullOrEmpty(ElOffset) &&
                double.TryParse(ElOffset, out double Sensitive2) &&
                Sensitive2 >= -1.0 && Sensitive2 <= 90 && TMSCBIT[1] == 0x00)
            {
                Console.WriteLine($"[TMS] > DOME Az set {AzOffset}");
                azUserOffset = Sensitive;
                elUserOffset = Sensitive2;
                domControler.doPositionMove(azUserOffset, elUserOffset);

                //MessageBox.Show("조건에 부합하는 값입니다.");
            }
            else
            {
                MessageBox.Show("범위를 벗어난 값입니다. 확인 후 다시 입력해주세요");
            }
        }
        /*        public double[] StarCalStart(double tai)
       {
           double[] TargetPosition = new double[2];
           TargetPosition = tcspk.ObtainTargetPointed(tai);
           return TargetPosition;
       }
       public double[][] TrackingData(double tai)
       {
           double[][] TargetPosition = new double[2][];
           for (int i = 0; i < TargetPosition.Length; i++)
           {
               TargetPosition[i] = new double[8];
           }
           TargetPosition = tcspk.ObtainTargetTracked(tai);
           return TargetPosition;
       }*/
        private void UpdateLabel(System.Windows.Forms.Label label, double newText1)
        {
            if (label != null)
            {
                if (label.InvokeRequired)
                {
                    label.Invoke(new Action(() => UpdateLabelContent(label, newText1)));
                }
                else
                {
                    UpdateLabelContent(label, newText1);
                }
            }
        }

        private void UpdateLabelContent(System.Windows.Forms.Label label, double newText1)
        {
            label.Text = newText1.ToString();
            label.Invalidate();
            label.Update();
        }



        private void UpdateLabel(System.Windows.Forms.Label label, string newText1)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() => label.Text = newText1.ToString()));
            }
            else
            {
                label.Text = newText1.ToString();
                label.Update();
            }
        }

        private bool debuggingRunning = false;

        private void button31_Click(object sender, EventArgs e)
        {
            debuggingRunning = true;

        }
        private void SaveParsedData(double packetTime, double[] azimuth, double[] elevation)
        {
            using (StreamWriter debugWriter = new StreamWriter("parsed_data.txt", true))
            {
                debugWriter.Write("packetTime: ");
                debugWriter.Write($"{packetTime:F5} ");
                debugWriter.WriteLine();

                debugWriter.Write("AzimuthData: ");
                for (int i = 0; i < azimuth.Length; i++)
                {
                    debugWriter.Write($"{azimuth[i]:F5} ");
                }
                debugWriter.WriteLine();

                debugWriter.Write("ElevationData: ");
                for (int i = 0; i < elevation.Length; i++)
                {
                    debugWriter.Write($"{elevation[i]:F5} ");
                }
                debugWriter.WriteLine();

                debugWriter.WriteLine("------ End of Data ------");
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            debuggingRunning = false;
        }

        private void button33_Click(object sender, EventArgs e)
        {
            Form graphForm = new Form
            {
                Text = "Graph Viewer",
                Width = 1300,
                Height = 900
            };

            debuggingPlot plot = new debuggingPlot
            {
                Dock = DockStyle.Fill
            };

            graphForm.Controls.Add(plot);
            graphForm.ShowDialog();
        }
        public static double[][] AzElData;
        public static double[] timestamps;
        public static double[][] AzElData_Dome;
        public static double[] timestamps_dome;
        int satellitevector;
        private void button25_Click(object sender, EventArgs e)
        {
            var satelliteData = ObservingSatellite_Information.GetData();
            if (satelliteData.AvailableFlag)
            {
                AzElData = new double[2][];
                for (var j = 0; j < AzElData.Length; j++)
                {
                    AzElData[j] = new double[satelliteData.RaDec[j].Length];
                }
                int totalSteps = (int)((satelliteData.EndTime - satelliteData.StartTime).TotalMilliseconds / (satelliteData.Interval * 1000)) + 1;
                timestamps = new double[totalSteps];
                DateTime currentTime = satelliteData.StartTime;
                TimeSpan intervalSpan = TimeSpan.FromMilliseconds(satelliteData.Interval * 1000);

                for (int i = 0; i < totalSteps; i++)
                {
                    TimeSpan elapsedTime = currentTime - satelliteData.StartTime.Date;
                    timestamps[i] = elapsedTime.TotalMilliseconds;

                    currentTime += intervalSpan;
                }
                _tcspk.StarExcute(ProjectModeSelect);
                //_tcspk.StarExcute();
                AzElData = _tcspk.FetchAndProcessTrackingData(satelliteData);
                double firstAz = AzElData[0][0];
                double secondAz = AzElData[0][1];
                double lastAz = AzElData[0][AzElData[0].Length - 1];
                satellitevector = _tcspk.StarPositionSignDecide(firstAz, secondAz, lastAz);
                for (var i = 0; i < AzElData[0].Length; i++)
                {
                    AzElData[0][i] = _tcspk.AzConvert_satel(AzElData[0][i], satellitevector);
                }
            }



            if (satelliteData.AvailableFlag)
            {
                AzElData_Dome = new double[2][];
                for (var j = 0; j < AzElData.Length; j++)
                {
                    AzElData_Dome[j] = new double[satelliteData.RaDec[j].Length];
                }
                int totalSteps = (int)((satelliteData.EndTime - satelliteData.StartTime).TotalMilliseconds / (satelliteData.Interval * 5000)) + 1;
                timestamps_dome = new double[totalSteps];
                DateTime currentTime = satelliteData.StartTime;
                TimeSpan intervalSpan = TimeSpan.FromMilliseconds(satelliteData.Interval * 5 * 1000);

                for (int i = 0; i < totalSteps; i++)
                {
                    TimeSpan elapsedTime = currentTime - satelliteData.StartTime.Date;
                    timestamps_dome[i] = elapsedTime.TotalMilliseconds;

                    currentTime += intervalSpan;
                }
                //_tcspk.StarExcute();
                _tcspk.StarExcute(ProjectModeSelect);
                AzElData_Dome = _tcspk.FetchAndProcessTrackingDomeData(satelliteData);
            }

        }
        private (double, double) CurrentPointingPosition(double currentUTCMillis)
        {
            double[] position = new double[2];
            if (timestamps == null || AzElData == null || AzElData[0] == null || AzElData[1] == null)
                return (position[0], position[1]);

            for (var i = 0; i < timestamps.Length; i++)
            {
                if (currentUTCMillis < timestamps[i])
                {
                    position[0] = AzElData[0][i];
                    position[1] = AzElData[1][i];
                    currentUTCMillis = timestamps[i];
                    return (position[0], position[1]);
                }
            }
            return (position[0], position[1]);
        }
        /* private (uint[], double[][]) CurrentTrackingPosition(double currentUTCMillis)
        {
            double[][] position = new double[2][];
            for (int i = 0; i < position.Length; i++)
            {
                position[i] = new double[8];
            }

            uint[] Timestamp = new uint[8];
            for (var i = 0; i < timestamps.Length-4; i++)
            {
                if (currentUTCMillis < timestamps[i])
                {
                    for (int j = -3; j <= 4; j++)
                    {
                        int index = i + j;

                        if (index >= 0 && index < timestamps.Length)
                        {
                            position[0][j + 3] = AzElData[0][index];
                            position[1][j + 3] = AzElData[1][index];
                            Timestamp[j + 3] = (uint)timestamps[index];
                        }
                    }

                    return (Timestamp, position);
                }
            }
            DateTime timestampDateTime = DateTimeOffset.FromUnixTimeMilliseconds((long)Timestamp[7]).UtcDateTime;
            if (ObservingSatellite_Information.observingSatellite_endTime < timestampDateTime)
            {
                Timestamp[7] = (uint)timestamps[timestamps.Length - 1];
                Timestamp[6] = (uint)timestamps[timestamps.Length - 2];
                Timestamp[5] = (uint)timestamps[timestamps.Length - 3];
                Timestamp[4] = (uint)timestamps[timestamps.Length - 4];
                Timestamp[3] = (uint)timestamps[timestamps.Length - 5];
                Timestamp[2] = (uint)timestamps[timestamps.Length - 6];
                Timestamp[1] = (uint)timestamps[timestamps.Length - 7];
                Timestamp[0] = (uint)timestamps[timestamps.Length - 8];
                
                 EndTracking();
             }
             return (Timestamp, position);
         }*/
        private (uint[], double[][]) CurrentTrackingPosition(double currentUTCMillis)
        {
            double[][] position = new double[2][];
            for (int i = 0; i < position.Length; i++)
            {
                position[i] = new double[8];
            }

            uint[] Timestamp = new uint[8];

            double adjustedUTCMillis = currentUTCMillis + TimeBiasValue;

            for (var i = 0; i < timestamps.Length - 4; i++)
            {
                if (currentUTCMillis < timestamps[i])
                {
                    int adjustedIndex = i;

                    for (int k = 0; k < timestamps.Length; k++)
                    {
                        if (adjustedUTCMillis < timestamps[k])
                        {
                            adjustedIndex = k;
                            break;
                        }
                    }

                    for (int j = -3; j <= 4; j++)
                    {
                        int index = i + j;
                        int adjustedPosIndex = adjustedIndex + j;

                        if (index >= 0 && index < timestamps.Length)
                        {
                            Timestamp[j + 3] = (uint)timestamps[index];
                        }
                        if (adjustedPosIndex >= 0 && adjustedPosIndex < timestamps.Length)
                        {
                            position[0][j + 3] = AzElData[0][adjustedPosIndex];
                            position[1][j + 3] = AzElData[1][adjustedPosIndex];
                        }
                    }

                    return (Timestamp, position);
                }
            }

            DateTime timestampDateTime = DateTimeOffset.FromUnixTimeMilliseconds((long)Timestamp[7]).UtcDateTime;
            if (ObservingSatellite_Information.observingSatellite_endTime < timestampDateTime)
            {
                for (int k = 0; k < 8; k++)
                {
                    Timestamp[k] = (uint)timestamps[timestamps.Length - (8 - k)];
                }
                EndTracking();
            }
            return (Timestamp, position);
        }
        private void PreviousTimeBias(object sender, EventArgs e)
        {
            string ControlSensitive = textBox1.Text;
            if (!string.IsNullOrEmpty(ControlSensitive) &&
                double.TryParse(ControlSensitive, out double Sensitive) &&
                Sensitive >= 10 && Sensitive <= 10000)
            {
                TimeBiasTick = Sensitive;
                TimeBiasValue -= TimeBiasTick;
                if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                {
                    if (double.TryParse(satRGGcontrol.rggTimeBias, out double biasValue))
                    {
                        biasValue -= TimeBiasTick;
                        satRGGcontrol.rggTimeBias = biasValue.ToString();
                    }

            }
                else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
                {
                    debRGGcontrol.RggTimeBias -= TimeBiasTick;
                }
            }
            else
            {
                MessageBox.Show("10 ~ 10000 사이의 값을 입력하세요.");
            }
        }

        private void LaterTimeBias(object sender, EventArgs e)
        {
            string ControlSensitive = textBox1.Text;
            if (!string.IsNullOrEmpty(ControlSensitive) &&
                double.TryParse(ControlSensitive, out double Sensitive) &&
                Sensitive >= 10 && Sensitive <= 10000)
            {
                TimeBiasTick = Sensitive;
                TimeBiasValue += TimeBiasTick;
                if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                {
                    if (double.TryParse(satRGGcontrol.rggTimeBias, out double biasValue))
                    {
                        biasValue += TimeBiasTick;
                        satRGGcontrol.rggTimeBias = biasValue.ToString();
                    }
            }
                else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
                {
                   // debRGGcontrol.rggTimeBias += TimeBiasTick;
                }
                
            }
            else
            {
                MessageBox.Show("10 ~ 10000 사이의 값을 입력하세요.");
            }
        }
        int ProjectModeSelect = 1;
        private void button22_Click(object sender, EventArgs e)
        {
            ProjectModeSelect = 1;
            button_1.Enabled = false;
            button_1.ForeColor = Color.Black;
            button_1.BackColor = Color.Lime;

            button_2.ForeColor = Color.White;
            button_2.BackColor = Color.Gray;
            button_2.Enabled = true;
            _tcspk.StarExcute(ProjectModeSelect);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            ProjectModeSelect = 2;
            button_1.Enabled = true;
            button_1.ForeColor = Color.White;
            button_1.BackColor = Color.Gray;

            button_2.ForeColor = Color.Black;
            button_2.BackColor = Color.Lime;
            button_2.Enabled = false;
            _tcspk.StarExcute(ProjectModeSelect);
        }

    }
}
