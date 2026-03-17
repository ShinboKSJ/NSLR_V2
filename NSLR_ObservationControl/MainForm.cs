using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Imaging;
using NSLR_ObservationControl.Module;
using log4net;
using log4net.Config;
using NSLR_ObservationControl.Subsystem;
using NSLR_ObservationControl.Util;
using mv.impact.acquire.helper;
using mv.impact.acquire;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.IO;
using NSLR_ObservationControl.Network;
using System.Threading;
using System.Windows.Automation.Peers;
using OpenTK.Platform.Windows;
using NSLR_ObservationControl.OAS;
using static NSLR_ObservationControl.CSU_StarCalibration;
using static NSLR_ObservationControl.CSU_Observation;
using static log4net.Appender.RollingFileAppender;
using System.Diagnostics;
using System.Xml.Linq;
using Npgsql;
using static NSLR_ObservationControl.Subsystem.ASICameraDll2;
namespace NSLR_ObservationControl
{

    public partial class MainForm : Form
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateConfiguration();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateConstants();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateGroundSite(double[] loc, StringBuilder siteName, double waveLength);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateTimeSystem();
        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateTLE();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetTLEData(IntPtr tleClass, StringBuilder line1, StringBuilder line2, double[] epochTime);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ConvertMJD2Greg(IntPtr timeSystem, double mjd, double[] greg);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ConvertGreg2MJD(IntPtr timeSystem, double[] greg);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetSiteLocation(IntPtr obsSite, double[] loc);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GenerateCommandFile_TLE(IntPtr tleClass, double[] siteLoc, double startMJD, double finalMJD, double stepSize, StringBuilder fileName, int opt);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GenerateCommand_TLE(IntPtr tleClass, double[] siteLoc, double targetMJD, int opt, double[] command);

        //////////////////////////////////////////////////////////////////////////////////////////////////////////

        private System.Windows.Forms.OpenFileDialog obsFileDialog;
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LoadObservation(IntPtr obsClass, StringBuilder _fileName);

        TaskControl taskControl = TaskControl.instance;
          
        private AID_Controller     aidControl;
        private DOM_Controller_v40 domControl;
        private LAS_SAT_Controller lasSATControl;
        private LAS_DEB_Controller lasDEBControl;
        private RGG_SAT_Controller rggSATControl;
        private RGG_DEB_Controller rggDEBControl;

        private UserControl userControlSelected;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        Module.groundResult CircularForm = new Module.groundResult();
        public UserControlManager _userManager;


        // MWIR 임시용
        public static MainForm mainForm;

        public MWIR mwir = new MWIR();

        public Device pDev;

        public static UdpClient ServerTMS;

        public static TcpClient OperatingsystemTCP;
        public static TcpClient OpticalsystemTCP;

        public Module.Observation_TMS TrackingMountSystem;
        public tcspk tcspk_, tcspk_satel;
#if LOCALTEST
        private Server OCUtcpserver;
        private const int OCU_PORT = 5101;
        //  private const string OCU_IP = "127.0.0.1";
        private const string OCU_IP = "192.168.0.31";
        private Server OPStcpserver;
        private const int OPS_PORT = 5300;
        private const string OPS_IP = "127.0.0.1";

#else

        private Server OPStcpserver;
        private const int OPS_PORT = 5300;
        //private const string OPS_IP = "192.168.10.11";  // 회사 테스트용
        private const string OPS_IP = "192.168.10.71";
        //private const string OPS_IP = "192.168.0.168";
        private Server OCUtcpserver;
        private const int OCU_PORT = 5101;
        private const string OCU_IP = "192.168.10.10";
#endif
        public bool OpticalsystemConn, Operatingsystemconn;

        // 현재 사용자 정보
        public string[] login_information = new string[6];
        // 위성 AZEL 받기위한 Class 객체
        Test_SatelliteAZEL test_satelliteAZEL;

        System.Windows.Forms.Timer WaveLength_update_timer = new System.Windows.Forms.Timer();
        
        public MainForm()   // 개발용(No Login) > 추후 삭제
        {
            //display.DisplaySettings();

            InitializeComponent();
            mainForm = this;

            NSLR_ObservationControl.Module.SystemSettings.SLRValuesChanged += UpdateSLRLabels;
            NSLR_ObservationControl.Module.SystemSettings.DLTValuesChanged += UpdateDLTLabels;

            _userManager = new UserControlManager(this);

            aidControl = AID_Controller.instance;
            domControl = DOM_Controller_v40.instance;
            lasSATControl = LAS_SAT_Controller.instance;
            lasDEBControl = LAS_DEB_Controller.instance;
            rggSATControl = RGG_SAT_Controller.instance;
            rggDEBControl = RGG_DEB_Controller.instance;
            
            //서브시스템 초기화
            //SubSystemManager.Initialize();
            aidControl.Initialize();
            domControl.Initialize();
            lasSATControl.Initialize();
            lasDEBControl.Initialize();
            rggSATControl.Initialize();
            rggDEBControl.Initialize();

            aidControl.Airplane_Detected += PauseIndicator_Set;

            // TCP & UDP
#if LOCALTEST
            //  ServerTMS = new UdpClient(5501);
#else
            ServerTMS = new UdpClient(7500);
#endif
            OPStcpserver = new Server(OPS_PORT);
            OPStcpserver.ClientConnected += HandleClientConnected;
            OPStcpserver.ClientDisconnected += HandleClientDisconnected;
            OPStcpserver.Start();

            OCUtcpserver = new Server(OCU_PORT);
            OCUtcpserver.ClientConnected += HandleClientConnected;
            OCUtcpserver.ClientDisconnected += HandleClientDisconnected;
            OCUtcpserver.Start();

            connData = CollectData();

            // 위성 AZEL 받기위한 Class 생성 (궤도라이브러리 적용)
            test_satelliteAZEL = new Test_SatelliteAZEL();
            tcspk_ = new tcspk();
            tcspk_satel = new tcspk();
            
            DutyModeNow.CurrentSystemObject = DutyModeNow.SystemObject.SLR;

            WaveLength_update_timer.Interval = 1000;
            WaveLength_update_timer.Tick += (s, e) =>
            {
                label_WaveLengthE.Text = $"파장변환기E: {lasSATControl.WaveLengthConverterE.ToString(),4} mJ";
            };
            WaveLength_update_timer.Start();
        }



        public MainForm(string[] information)   // 해당 생성자 사용
        {
            InitializeComponent();
            mainForm = this;

            // Login Data 전달 (LoginForm > MainForm)
            for (int i = 0; i < login_information.Length; i++)
            {
                login_information[i] = information[i];
            }
            _userManager = new UserControlManager(this);

            log.Info("관측제어 PC MainForm ()");

            // TCP & UDP
            //    ServerTMS = new UdpClient(7500);

            OPStcpserver = new Server(OPS_PORT);
            OPStcpserver.ClientConnected += HandleClientConnected;
            OPStcpserver.ClientDisconnected += HandleClientDisconnected;
            OPStcpserver.Start();

            OCUtcpserver = new Server(OCU_PORT);
            OCUtcpserver.ClientConnected += HandleClientConnected;
            OCUtcpserver.ClientDisconnected += HandleClientDisconnected;
            OCUtcpserver.Start();

            connData = CollectData();
            tcspk_ = new tcspk();
            tcspk_satel = new tcspk();
        }

        private void Form_Load(object sender, System.EventArgs e)
        {
            /*
            CSU_ObserveSchedule ObserveScheduler = new CSU_ObserveSchedule();

            Screen currentScreen = Screen.FromControl(this);
            Screen nextScreen = Screen.AllScreens[(Array.IndexOf(Screen.AllScreens, currentScreen) + 1) % Screen.AllScreens.Length];
            ObserveScheduler.StartPosition = FormStartPosition.Manual;
            ObserveScheduler.Location = nextScreen.WorkingArea.Location;
            CenterToParent();
            ObserveScheduler.Show();
            */
            /*
            // UI 서브모니터 전시0807
            Screen[] scr = Screen.AllScreens;

            if (scr.Length > 1)
            {
                this.Location = scr[1].Bounds.Location;
            }
            */

            //test_satelliteAZEL.Test_Example();
            /*
            MessageBox.Show("test start!");

            List<Tuple<DateTime, double, double, double, double>> predictData = new List<Tuple<DateTime, double, double, double, double>>();

            predictData.Add(new Tuple<DateTime, double, double, double, double>(new DateTime(2025, 10, 16, 4, 30, 0),
                                        -34312050.919000, 24508161.912000, 342613.038000, 1.0));
            predictData.Add(new Tuple<DateTime, double, double, double, double>(new DateTime(2025, 10, 16, 4, 35, 0),
                                        -34311453.703000, 24508188.466000, 319618.214000, 1.0));
            predictData.Add(new Tuple<DateTime, double, double, double, double>(new DateTime(2025, 10, 16, 4, 40, 0),
                                        -34310857.960000, 24508190.571000, 296470.398000, 1.0));
            predictData.Add(new Tuple<DateTime, double, double, double, double>(new DateTime(2025, 10, 16, 4, 45, 0),
                                        -34310264.013000, 24508167.742000, 273180.658000, 1.0));
            predictData.Add(new Tuple<DateTime, double, double, double, double>(new DateTime(2025, 10, 16, 4, 50, 0),
                                        -34309672.206000, 24508119.501000, 249760.128000, 1.0));
            predictData.Add(new Tuple<DateTime, double, double, double, double>(new DateTime(2025, 10, 16, 4, 55, 0),
                                        -34309082.901000, 24508045.392000, 226220.009000, 1.0));

            List<Tuple<DateTime, double, double, double, double>> result = CSU_ObserveSchedule2.CPF_Interpolation(predictData, 300, 12);

            MessageBox.Show("test end!");
            */
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            buttonSAT.PerformClick();
            /*
            // 1) 위치 강제 제어 준비
            this.WindowState = FormWindowState.Normal;               // 최대화/최소화 상태면 좌표가 잘 안 먹음
            this.StartPosition = FormStartPosition.Manual;           // 수동 배치로 전환

            // 2) 보조 모니터 선택(Primary=false). 없으면 기본 모니터 유지
            var target = Screen.AllScreens.FirstOrDefault(s => !s.Primary);
            if (target == null) return;

            // 3) 작업영역(작업표시줄 제외) 기준 중앙 배치
            var wa = target.WorkingArea;

            // 폼이 너무 크면 작업영역에 맞춰 축소
            int w = Math.Min(this.Width, wa.Width);
            int h = Math.Min(this.Height, wa.Height);
            if (w != this.Width || h != this.Height)
                this.Size = new Size(w, h);

            // 중앙 좌표 계산 (보조 모니터가 왼쪽/위쪽에 있어도 음수 좌표 지원됨)
            int x = wa.Left + (wa.Width - this.Width) / 2;
            int y = wa.Top + (wa.Height - this.Height) / 2;

            // 4) 실제 이동
            this.Location = new Point(x, y);

            // 5) 필요하면 보조 모니터에서 바로 최대화
            this.WindowState = FormWindowState.Maximized;
            */
        }

        public static DateTime GetBuildDate(Assembly assembly)
        {
            const int peHeaderOffset = 60;
            const int linkerTimestampOffset = 8;
            byte[] bytes = new byte[2048];

            using (FileStream stream = new FileStream(assembly.Location, FileMode.Open, FileAccess.Read))
            {
                stream.Read(bytes, 0, bytes.Length);
            }

            int headerPos = BitConverter.ToInt32(bytes, peHeaderOffset);
            int secondsSince1970 = BitConverter.ToInt32(bytes, headerPos + linkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return dt.AddSeconds(secondsSince1970).ToLocalTime();
        }

        public void LoadObject_Star()
        {
            controlC.observation_TMS1.ExternalLibraryObject = tcspk_;
            tcspk_.StarExcute();
        }
        public void LoadObject_Satel()
        {
            controlA.observation_TMS2.ExternalLibraryObject = tcspk_satel;
            tcspk_satel.StarExcute();
        }

        OPSUserData opticalsystemData;
        ConnUserData connData;
        TMSUserData tmsData;
        
        AIDUserData aidData;
        DOMUserData domData;
        LASUserData lasData;
        RGGUserData rggData;

        private void SendtoOS()
        {
            if (OperatingsystemTCP != null && OperatingsystemTCP.Connected)
            {

                try
                {
                    foreach (Control control in panel_Main.Controls)
                    {
                        if (control is ICollectData && control.Visible && control is ICollectData2)
                        {
                            opticalsystemData = ((ICollectData)control).CollectDataFromOpticalSystem();
                            tmsData = ((ICollectData2)control).CollectDataFromTrackingMountSystem();
                        }
                        else if (control is ICollectData2 && control.Visible)
                        {
                            tmsData = ((ICollectData2)control).CollectDataFromTrackingMountSystem();
                        }
                        else if (control is ICollectData && control.Visible)
                        {
                            opticalsystemData = ((ICollectData)control).CollectDataFromOpticalSystem();
                        }
                    }
                    connData = CollectData();


                    if(DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                    {
                        lasData = lasSATControl.userData; 
                        rggData = rggSATControl.userData; 
                    }
                    else
                    {
                        lasData = lasDEBControl.userData;
                        rggData = rggDEBControl.userData;
                    }

                    aidData = aidControl.userData;
                    domData = domControl.userData; 

                    var combinedData = new
                    {
                        OpticalSystem = opticalsystemData,
                        Connection = connData,
                        TrackingMountSystem = tmsData,
                        AidSystem = aidData,
                        DomSystem = domData,
                        LasSystem = lasData,
                        RggSystem = rggData,
                    };
                    //log.Warn($">> AIDUserData ID:{combinedData.AidSystem.ID} OP:{combinedData.AidSystem.AIDopMode} Radar:{combinedData.AidSystem.RadarSyncState} ADS-B:{combinedData.AidSystem.ADSbSyncState} BIT {combinedData.AidSystem.BITResult} ") ;
                    //log.Warn($">> DomUserData ID:{combinedData.DomSystem.ID} Shuuter {combinedData.DomSystem.Shutter} Rain:{combinedData.DomSystem.Rain} Home:{combinedData.DomSystem.Home} Pos:{combinedData.DomSystem.Position}");
                    //log.Warn($">> LASUserData ID:{combinedData.LasSystem.ID} OP:{combinedData.LasSystem.LASopMode} {combinedData.LasSystem.LaserFireState}");
                    //log.Warn($">> RGGUserData :{combinedData.RggSystem.ID}{combinedData.RggSystem.GatePulseSO} {combinedData.RggSystem.GatePulseWidth}");

                    string jsonData = JsonConvert.SerializeObject(combinedData);
                    byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonData);

                    NetworkStream stream = OperatingsystemTCP.GetStream();
                    if (stream != null)
                    {
                        stream.Write(jsonBytes, 0, jsonBytes.Length);
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine("IOException occurred: " + ex.Message);
                }
                catch (ObjectDisposedException ex)
                {
                    Console.WriteLine("NetworkStream was closed: " + ex.Message);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("An unexpected exception occurred: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("TCP client is not connected or is null.");
            }
        }


        public interface ICollectData
        {
            OPSUserData CollectDataFromOpticalSystem();
        }
        public interface ICollectData2
        {
            TMSUserData CollectDataFromTrackingMountSystem();
        }
        public ConnUserData CollectData()
        {
            //var conn = SubSystemManager.IsConnected();
            //foreach (var keyValue in conn){  Console.WriteLine($"{keyValue.Key}: {(keyValue.Value ? true : false)}); }
            ConnUserData userData = new ConnUserData
            {
                OpticalsystemConn = OpticalsystemConn,
                Operatingsystemconn = Operatingsystemconn,
                TMSConn = Observation_TMS.TMSConn,
                AIDconn = aidControl.IsConnected(),//conn.GetConnection("AID_Controller")
                CDSconn = domControl.IsConnected(),      //conn.GetConnection("DOM_Controller_v40"),
                LAS1conn = lasSATControl.IsConnected(),  //conn.GetConnection("LAS_SAT_Controller"),
                LAS2conn = lasDEBControl.IsConnected(),  //conn.GetConnection("LAS_DEB_Controller"),
                OEU1conn = rggSATControl.IsConnected(),  //conn.GetConnection("RGG_SAT_Controller"),
                OEU2conn = rggDEBControl.IsConnected(),  //conn.GetConnection("RGG_DEB_Controller"),
            };
            
            return userData;
        }
        private Thread sendOPSPacket, sendOSPacket;
        private Thread receiveDataThread;
        private void HandleClientConnected(object sender, TcpClientEventArgs e)
        {
            if (e.Client.Client.RemoteEndPoint is System.Net.IPEndPoint endPoint)
            {
                if (endPoint.Address.ToString() == OPS_IP)
                {
                    OpticalsystemTCP = e.Client;
                    OpticalsystemConn = true;
                    // ChangeLabelColor(OpticalsystemConn);

                    Thread receiveDataThread = new Thread(() =>
                    {
                        while (OpticalsystemConn)
                        {
                            foreach (Control control in panel_Main.Controls)
                            {
                                try
                                {
                                    if (_userManager._newControl.Name == "CSU_Observation")
                                    {

                                        controlA.opticalSystem1.ReceiveDataFromClient();
                                        Thread.Sleep(200);
                                    }
                                    else if (_userManager._newControl.Name == "CSU_GroundCalibration")
                                    {
                                        starStep = StarCalInfo.Step.None;
                                        controlB.opticalSystem1.ReceiveDataFromClient();
                                        Thread.Sleep(200);
                                    }
                                    else if (_userManager._newControl.Name == "CSU_StarCalibration")
                                    {

                                        controlC.opticalSystem1.ReceiveDataFromClient();
                                        Thread.Sleep(200);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    });
                    receiveDataThread.Start();
                    Thread sendOPSPacket = new Thread(() =>
                    {

                        while (OpticalsystemConn)
                        {
                            foreach (Control control in panel_Main.Controls)
                            {

                                try
                                {
                                    if (_userManager._newControl.Name == "CSU_Observation")
                                    {
                                        controlA.opticalSystem1.OPS_Period20ms(ControlCommand.OPSCONTROL);
                                        Thread.Sleep(1000);
                                    }
                                    else if (_userManager._newControl.Name == "CSU_GroundCalibration")
                                    {
                                        controlB.opticalSystem1.OPS_Period20ms(ControlCommand.OPSCONTROL);
                                        Thread.Sleep(1000);
                                    }
                                    else if (_userManager._newControl.Name == "CSU_StarCalibration")
                                    {
                                        controlC.opticalSystem1.OPS_Period20ms(ControlCommand.OPSCONTROL);
                                        Thread.Sleep(1000);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                            }
                        }
                    });
                    sendOPSPacket.Start();
                }
                else if (endPoint.Address.ToString() == OCU_IP)
                {
                    OperatingsystemTCP = e.Client;
                    Operatingsystemconn = true;
                    Thread sendOSPacket = new Thread(() =>
                    {
                        while (Operatingsystemconn)
                        {
                            SendtoOS();
                            Thread.Sleep(3000);
                        }
                    });
                    sendOSPacket.Start();
                }
            }
        }
        private void HandleClientDisconnected(object sender, TcpClientEventArgs e)
        {
            if (OpticalsystemTCP == e.Client)
            {
                OpticalsystemTCP = null;
                OpticalsystemConn = false;

                if (sendOPSPacket != null && sendOPSPacket.IsAlive)
                {
                    sendOPSPacket.Join();
                }

                if (receiveDataThread != null && receiveDataThread.IsAlive)
                {
                    receiveDataThread.Join();
                }
            }
            else if (OperatingsystemTCP == e.Client)
            {
                OperatingsystemTCP = null;
                Operatingsystemconn = false;
                if (sendOSPacket != null && sendOSPacket.IsAlive)
                {
                    sendOSPacket.Join();
                }
            }
        }


        public CSU_ObserveSchedule2 csu_observeSchedule2;


        private void displaySchedule_button_Click(object sender, EventArgs e)   // 스케줄러 전시 Click 시
        {
            // CPF 다운로드 중 여부 Check
            if (CPFDownload_status == true)
            {
                
                MessageBox.Show("CPF 다운로드 중..." + "\r\n" + "잠시 후 시도하세요.");
                return;
            }

            // 최신 CPF 다운로드 여부 Check
            DirectoryInfo cpf_directory = new DirectoryInfo(Application.StartupPath + "\\" + CPF_FolderName);
            if (cpf_directory.Exists == true)
            {
                DateTime lastWrite_dateTime = Directory.GetLastWriteTime(cpf_directory.FullName);

                if (lastWrite_dateTime.Year != DateTime.Now.Year || lastWrite_dateTime.Month != DateTime.Now.Month || lastWrite_dateTime.Day != DateTime.Now.Day)
                {
                    MessageBox.Show("최신 CPF 다운로드 하세요.");
                    return;
                }
            }
            else
            {
                MessageBox.Show("CPF 다운로드 하세요.");
                return;
            }

            // 스케줄러 전시
            if (csu_observeSchedule2 == null)
            {
                csu_observeSchedule2 = new CSU_ObserveSchedule2();
                csu_observeSchedule2.SatelliteDataUpdated += controlA.OnSatelliteDataUpdated;
                csu_observeSchedule2.Show();
            }

        }

        private void downloadCPF_button_Click(object sender, EventArgs e)   // CPF Download Click 시
        {
            if (csu_observeSchedule2 != null)
            {
                MessageBox.Show("스케줄러 전시 상태입니다." + "\r\n" + "CPF Download 불가!");
                return;
            }

            Excute_CPFDownloader();
        }

        bool CPFDownload_status = false;
        string CPF_FolderName = "CPF";
        private void Excute_CPFDownloader()
        {
            CPFDownload_status = true;  // 시작
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////// 기본적으로 CPFDownloader 관련 실행파일(.exe)과 참조파일(.dll, .xml 등)은 프로젝트 내부에 보관 ////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // 실행파일 위치 내 CPF 다운로드 폴더 유무 확인 + 폴더 내 기존파일 삭제
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.StartupPath + "\\" + CPF_FolderName);

            if (directoryInfo.Exists == false)
            {
                directoryInfo.Create();
            }
            else
            {
                foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                {
                    fileInfo.Delete();
                }
            }

            // Config.xml 파일 내 LocalPath 수정
            string configFile_path = Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\CPFDownloaderSetup\\Config.xml";

            if (File.Exists(configFile_path) == false)
            {
                MessageBox.Show("Config.xml 파일 없음.");
                CPFDownload_status = false; // 종료(비정상)
                return;
            }

            try
            {
                XDocument xDocument = XDocument.Load(configFile_path);
                XElement localPathElement = xDocument.Element("DataCenterConfig")?.Element("LocalPath");

                if (localPathElement == null)
                {
                    MessageBox.Show("LocalPath(Config.xml) 설정 중 오류발생.");
                    CPFDownload_status = false; // 종료(비정상)
                    return;
                }

                localPathElement.Value = directoryInfo.FullName;
                xDocument.Save(configFile_path);
            }
            catch (Exception ex)
            {
                MessageBox.Show("LocalPath(Config.xml) 설정 중 오류발생.");
                CPFDownload_status = false; // 종료(비정상)
                return;
            }

            // CPFDownloader 실행파일(.exe) 실행
            Process externalProcess = new Process();
            externalProcess.StartInfo.FileName = Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\CPFDownloaderSetup\\CPFDownloader.exe";
            externalProcess.StartInfo.WorkingDirectory = Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + "\\" + "CPFDownloaderSetup";
            externalProcess.EnableRaisingEvents = true;
            externalProcess.Exited += new EventHandler(ProcessExited);
            externalProcess.Start();

        }

        private void ProcessExited(object sender, EventArgs e)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.StartupPath + "\\" + CPF_FolderName);

            if (directoryInfo.Exists == true)
            {
                int file_count = Directory.GetFiles(directoryInfo.FullName, "*.*").Length;
                MessageBox.Show("CPF 다운로드 완료!" + "\r\n" + "(파일갯수 : " + file_count + ")");
            }

            CPFDownload_status = false; // 종료(정상)
        }


        #region UI Control
        private void button_FormClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_FormNormalMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void button_FormMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        bool isMove;
        Point fPt;
        private void panel_Title_MouseDown(object sender, MouseEventArgs e)
        {
            isMove = true;
            fPt = new Point(e.X, e.Y);
        }

        private void panel_Title_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove && (e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (fPt.X - e.X), this.Top - (fPt.Y - e.Y));
            }
        }

        private void panel_Title_MouseUp(object sender, MouseEventArgs e)
        {
            isMove = false;
        }
        #endregion

        #region del Event Uc_StateIndicator
        private void Uc_StateIndicator(object sender, string strState)
        {
            label_Mode.Text = sender.ToString();
            label_State.Text = strState;
        }

        //private void ReceiveMessage(string message)
        //{         
        //    label_ADS.Text = message;
        //}

        #endregion

        private UserControl GetActiveUserControl(Panel panel)
        {
            foreach (Control control in panel.Controls)
            {
                if (control is UserControl && control.Visible)
                {
                    return (UserControl)control;
                }
            }
            return null;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            // If it was the user, we want to make sure he didn't do it by accident
            DialogResult r = MessageBox.Show("Are you sure you want this?", "Application is shutting down."
                                             , MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Do save works
            if (r != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }
            //display.RestoreDisplaySetting();
            log.Info("(운용자) 프로그램을 종료합니다.");


            // 임시용

            // MWIR
            mwir.MWIR_DisplayStop();

            mwir.MWIR_DevClose();

            mwir.Dispose();

            // EMCCD
            controlA.caMs1.emccd.Disconnect();
            controlC.caMs1.emccd.Disconnect();

            //controlA.caMs1.emccd.OnLinkDisconnected(controlA.caMs1.emccd.mDevice);
            //controlC.caMs1.emccd.OnLinkDisconnected(controlC.caMs1.emccd.mDevice);

            // 공통
            controlA.Dispose();
            controlC.Dispose();


            System.Windows.Forms.Application.ExitThread();
            Environment.Exit(0);


        }

        // Main 화면 Button Click Event (동일 Event로 받고 코드 상에서 분기 처리)
        private void button_Click(object sender, EventArgs e)
        {
            Control ctrl = (Control)sender;
            //MessageBox.Show($"{ctrl.Name}");

            switch (ctrl.Name)
            {
                case "btn_Observation": //SatelliteRanging 
                    break;
            }
        }


      /*  private void pictureBox_Laser_Click(object sender, EventArgs e)
        {
            domControl.doShutter();

           // Animate(pictureBox_Laser, !IsAnimating(pictureBox_Laser));
        }*/
        private void pictureBox_Laser_Click(object sender, EventArgs e)
        {
            var result = domControl.doShutter();

            if (result.Success)
            {
                if (result.TargetState == "OPENING")
                {
                    pictureBox_Laser.Image = Properties.Resources.DomeOpened; // OPEN 이미지
                    log.Info("[UI] Shutter image changed to OPENING");
                }
                else if (result.TargetState == "CLOSING")
                {
                    pictureBox_Laser.Image = Properties.Resources.DomeClosed; // CLOSE 이미지
                    log.Info("[UI] Shutter image changed to CLOSING");
                }
            }
            else
            {
                log.Warn($"[UI] Shutter command failed. Current state: {result.CurrentState}");
            }
        }
        private static bool IsAnimating(PictureBox box)
        {
            var fi = box.GetType().GetField("currentlyAnimating",
                BindingFlags.NonPublic | BindingFlags.Instance);
            //MessageBox.Show($"{fi.GetValue(box)}");
            return (bool)fi.GetValue(box);
        }
        private void Animate(PictureBox box, bool enable)
        {
            var anim = box.GetType().GetMethod("Animate",
                BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(bool) }, null);
            anim.Invoke(box, new object[] { enable });
        }

        CSU_Observation controlA = new CSU_Observation();
        private void btn_Raning_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Ranging.Checked == true)
            {
                if (taskControl.task_mutex)
                {
                    MessageBox.Show($"{taskControl.task_name} 임무 수행 중입니다. 먼저 임무 완료를 해주세요. ");
                    return;
                }
TaskModeNow.CurrentSystemObject = TaskModeNow.SystemObject.RANGE;

                controlC.observation_TMS1.Dispose();
                controlA.observation_TMS2.Dispose();
                controlB.GroundTargetMount.Dispose();
                starStep = StarCalInfo.Step.None;
                StarCalInfo.StarCalFlag = false;
                mwir.MWIR_DisplayStop();
                controlA.caMs1.emccd.Disconnect();
                controlC.caMs1.emccd.Disconnect();
                controlA.caMs1.starCAM.starcamDispose();
                controlC.caMs1.starCAM.starcamDispose();
                //  if (controlA.caMs1.starCAM.pASICameraInfo.CameraID != null) ASIStopVideoCapture(0);
                controlC.observation_TMS1.UdpClientChk();
                controlB.GroundTargetMount.UdpClientChk();
                _userManager.SwitchUserControl(controlA);
                LoadObject_Satel();
                controlA.observation_TMS2.State();
                //   controlA.reloadLibrary();
                //var controlA = new CSU_Observation();
                Uc_StateIndicator("관측 모드", "준비 상태");
                log.Info("[Menu] 관측 모드");
            }

        }

        CSU_GroundCalibration controlB = new CSU_GroundCalibration();
        GroundTargetMount GroundTargetMount;
        private void btn_GroundCalibration_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_GroundCalibration.Checked == true)
            {
                if (taskControl.task_mutex)
                {
                    MessageBox.Show($"{taskControl.task_name} 임무 수행 중입니다. 먼저 임무 완료를 해주세요. ");
                    return;
                }
                TaskModeNow.CurrentSystemObject = TaskModeNow.SystemObject.G_CAL;

                controlA.observation_TMS2.Dispose();
                controlC.observation_TMS1.Dispose();
                controlB.GroundTargetMount.Dispose();
                starStep = StarCalInfo.Step.None;
                StarCalInfo.StarCalFlag = false;
                TargetStep = TargetInfo.Step.None;
                TargetInfo.TargetFlag = false;
                //var controlA = new CSU_GroundCalibration();
                controlA.observation_TMS2.UdpClientChk();
                controlC.observation_TMS1.UdpClientChk();
                _userManager.SwitchUserControl(controlB);
                controlB.GroundTargetMount.State();
                Uc_StateIndicator("지상보정 모드", "준비 상태");
                log.Info("[Menu] 지상보정 모드");
            }

        }

        CSU_StarCalibration controlC = new CSU_StarCalibration();
        private void btn_StarCalibration_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_StarCalibration.Checked == true)
            {
                if (taskControl.task_mutex)
                {
                    MessageBox.Show($"{taskControl.task_name} 임무 수행 중입니다. 먼저 임무 완료를 해주세요. ");
                    return;
                }
                TaskModeNow.CurrentSystemObject = TaskModeNow.SystemObject.S_CAL;

                controlA.observation_TMS2.Dispose();
                controlC.observation_TMS1.Dispose();
                controlB.GroundTargetMount.Dispose();
                TargetStep = TargetInfo.Step.None;
                TargetInfo.TargetFlag = false;
                mwir.MWIR_DisplayStop();
                controlA.caMs1.emccd.Disconnect();
                controlC.caMs1.emccd.Disconnect();
               controlA.caMs1.starCAM.starcamDispose();
                controlC.caMs1.starCAM.starcamDispose();
                //     if (controlC.caMs1.starCAM.pASICameraInfo.CameraID != null) ASIStopVideoCapture(0);
                controlA.observation_TMS2.UdpClientChk();
                controlB.GroundTargetMount.UdpClientChk();
                _userManager.SwitchUserControl(controlC);
                LoadObject_Star();
                controlC.observation_TMS1.State();
                //controlC.reloadLibrary();
                //var controlA = new CSU_StarCalibration();
                Uc_StateIndicator("별보정 모드", "준비 상태");
                log.Info("[Menu] 별보정 모드");
            }

        }

        RecordManagement controlD = new RecordManagement();
        private void radioButton_RecordManagement_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_RecordManagement.Checked == true)
            {
                if (taskControl.task_mutex)
                {
                    MessageBox.Show($"{taskControl.task_name} 임무 수행 중입니다. 먼저 임무 완료를 해주세요. ");
                    return;
                }

                //var controlA = new RecordManagement();
                _userManager.SwitchUserControl(controlD);
                Uc_StateIndicator("데이터 관리", "정상");
                log.Info("[Menu] 데이터 관리");
            }


        }

        UserManagement controlE = new UserManagement();
        private void radioButton_UserManagemnet_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_UserManagemnet.Checked == true)
            {
                if (taskControl.task_mutex)
                {
                    MessageBox.Show($"{taskControl.task_name} 임무 수행 중입니다. 먼저 임무완료를 해주세요. ");
                    return;
                }

                if (login_information[0] == null)   // 개발용(No Login) > 추후 삭제
                {
                    //MessageBox.Show("ID/PW 불일치 (개발용)");
                    return;
                }

                //var controlA = new UserManagement();
                _userManager.SwitchUserControl(controlE);
                Uc_StateIndicator("사용자 관리", "정상");
                log.Info("[Menu] 사용자관리");
            }

        }

        ConfigSetting controlF = new ConfigSetting();
        private void radioButton_ConfigSetting_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ConfigSetting.Checked == true)
            {
                if (taskControl.task_mutex)
                {
                    MessageBox.Show($"{taskControl.task_name} 임무 수행 중입니다. 먼저 임무완료를 해주세요. ");
                    return;
                }

                //var controlA = new ConfigSetting();
                _userManager.SwitchUserControl(controlF);
                Uc_StateIndicator("시스템 설정", "정상");
                log.Info("[Menu] 시스템 설정");
            }

        }

        SystemDiagnostics controlG = new SystemDiagnostics();
        private void radioButton_SystemDiagnostic_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_SystemDiagnostic.Checked == true)
            {
                if (taskControl.task_mutex)
                {
                    MessageBox.Show($"{taskControl.task_name}  수행 중입니다. 먼저 임무완료를 해주세요. ");
                    return;
                }

                //var controlA = new SystemDiagnostics();
                controlA.observation_TMS2.Dispose();
                controlC.observation_TMS1.Dispose();
                starStep = StarCalInfo.Step.None;
                StarCalInfo.StarCalFlag = false;
                TargetStep = TargetInfo.Step.None;
                TargetInfo.TargetFlag = false;
                controlA.observation_TMS2.UdpClientChk();
                controlC.observation_TMS1.UdpClientChk();
                _userManager.SwitchUserControl(controlG);
                _userManager.SwitchUserControl(controlG);
                Uc_StateIndicator("시스템 진단", "정상");
                log.Info("[Menu] 시스템 진단");
            }

        }

        private bool bEmergencyStop = false;
        private void btn_EmergencyStop_Click(object sender, EventArgs e)
        {
            foreach (UserControl control in panel_Main.Controls)
            {
                if (control.Name == "CSU_Observation")
                {
                    Uc_StateIndicator("관측 모드", "안전 상태");
                }
                else if (control.Name == "CSU_GroundCalibration")
                {
                    controlB.groundCalibration_Info1.EmergencyStop();
                    Uc_StateIndicator("지상보정 모드", "안전 상태");
                }
                else if (control.Name == "CSU_StarCalibration")
                {
                    Uc_StateIndicator("별보정 모드", "안전 상태");
                }
            }

            bEmergencyStop = !bEmergencyStop;

            if (bEmergencyStop)
            {
                EmergencyStop.BackColor = Color.Red;
                EmergencyStop.ForeColor = Color.White;
                EmergencyStop.Text = "긴급정지";
            }
            else
            {
                EmergencyStop.BackColor = Color.Lime;
                EmergencyStop.ForeColor = Color.Black;
                EmergencyStop.Text = "긴급정지 해제";
            }
        }


        private void PauseIndicator_Set(object sender, PauseEventArgs e)
        {
            if (e.IsPaused.Contains("PAUSE"))
            {
                PauseIndicator.BackColor = Color.Red;
                PauseIndicator.ForeColor = Color.White;
                PauseIndicator.Text = "일시정지";
                //log.Info("[AID] 일시정지 on");
            }
            else if (e.IsPaused.Contains("RESUME"))
            {
                PauseIndicator.BackColor = Color.Lime;
                PauseIndicator.ForeColor = Color.Black;
                PauseIndicator.Text = "일시정지 해제";
                //log.Info("[AID] 일시정지 off");
            }
        }


        private void label_Mode_MouseHover(object sender, EventArgs e)
        {
            Console.WriteLine($"[MainForm] rggSAT connected {rggSATControl.IsConnected()}]");
        }

        private void time2_MouseHover(object sender, EventArgs e)
        {
            DateTime buildDate = GetBuildDate(Assembly.GetExecutingAssembly());
            string directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            MessageBox.Show($"{buildDate} {directory}");
        }

        private void PauseIndicator_Click(object sender, EventArgs e)
        {

        }

        private void buttonSatellite_Click(object sender, EventArgs e)
        {
            DutyModeNow.CurrentSystemObject = DutyModeNow.SystemObject.SLR;
            //MessageBox.Show("인공위성 관측 Setting! ");
            OpMsg.Text = "인공위성 모드 선택";
            if(e.ToString().Contains("MouseEvent"))
                log.Info("(운용자) 인공위성 모드로 변경하였습니다.}");
            else
                log.Info("인공위성 모드입니다.");
            buttonSAT.Enabled = false;
            buttonSAT.ForeColor = Color.Black;
            buttonSAT.BackColor = Color.PaleGreen;

            buttonDEB.ForeColor = Color.PaleGreen;
            buttonDEB.BackColor = Color.Black;
            buttonDEB.Enabled = true;
            var (az,el) = NSLR_ObservationControl.Module.SystemSettings.GetSLRValues();
            controlB.groundCalibration_Info1.label2.Text = az.ToString("F5");
            controlB.groundCalibration_Info1.label16.Text = el.ToString("F5");
        }

        private void buttonDebris_Click(object sender, EventArgs e)
        {
            DutyModeNow.CurrentSystemObject = DutyModeNow.SystemObject.DLT;
            //MessageBox.Show("우주물체 관측 Setting! ");
            OpMsg.Text = "우주물체 모드 선택";
            if (e.ToString().Contains("MouseEvent"))
            log.Info("(운용자) 우주물체 모드로 변경하였습니다.");
            else
                log.Info("우주물체 모드입니다.");
            buttonDEB.Enabled = false;
            buttonDEB.ForeColor = Color.Black;
            buttonDEB.BackColor = Color.PaleGreen;

            buttonSAT.ForeColor = Color.PaleGreen;
            buttonSAT.BackColor = Color.Black;
            buttonSAT.Enabled = true;
            var (az, el) = NSLR_ObservationControl.Module.SystemSettings.GetDLTValues();
            controlB.groundCalibration_Info1.label2.Text = az.ToString("F5");
            controlB.groundCalibration_Info1.label16.Text = el.ToString("F5");
        }


        //EMCCD emccd = new EMCCD();
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //emccd.OnLinkDisconnected(emccd.mDevice);
        }
        private void UpdateSLRLabels()
        {
            var (az, el) = NSLR_ObservationControl.Module.SystemSettings.GetSLRValues();

            controlB.groundCalibration_Info1.label2.Text = az.ToString("F5");
            controlB.groundCalibration_Info1.label16.Text = el.ToString("F5");
        }

        private void UpdateDLTLabels()
        {
            var (az, el) = NSLR_ObservationControl.Module.SystemSettings.GetDLTValues();

            controlB.groundCalibration_Info1.label2.Text = az.ToString("F5");
            controlB.groundCalibration_Info1.label16.Text = el.ToString("F5");
        }
    }
}
