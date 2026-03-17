using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace NSLR_ObservationControl
{
    public class Serial_PORT
    {
        public const string LAB_TEST = "COM13";  //LabTest용

        //public const string RGG_SAT = "COM6";  // 회사 테스트용
        public const string RGG_SAT  = "COM3";  // Satellite(인공위성용) 25.01.15 거창 [#3]
        //public const string RGG_DEB = "COM7";  // 회사 테스트용
        public const string RGG_DEB  = "COM5";  // Debris(우주물체용)    25.01.20 거창 [#5]
    }

    public class InterPC_PORT
    {
        public const int LAS = 55100;
        public const int RGG = 55200;
        public const int DOM = 55400;
        public const int AID = 55500;
        public const int LOG = 55555;
    }
    public class SubSystem_PORT
    {
        public const int TMS = 5000;
        public const int LAS_SAT = 5100;
        public const int LAS_DEB = 5110;
        public const int EVT_SAT = 5200;
        public const int EVT_DEB = 5210;
        public const int OPT = 5300;
        public const int DOM = 5400;
        public const int AID = 5500;
    }
    public static class GlobalVariables
    {
        
#if LOCALTEST
        private static double systemDelay = 0;
#else
        private static double systemDelay = 279140e-12;
#endif
        // private static double opticalPath  = 145990e-12;
        public static double SystemDelay
        {
            get { return systemDelay; }
            set { systemDelay = value; }
        }
        /*public static double OpticalPath
        {
            get { return opticalPath; }
        }*/
    }
    public static class MountModelingCoff
    {

    }
    public class NSLR_IP
    {
        public const string LAB_TEST = "192.168.0.10";

        public const string OCU_OPC = "192.168.10.10";
        public const string OCU_OBC = "192.168.10.11";

        public const string TMS = "192.168.10.21";
        public const string LAS_SAT = "192.168.10.31";
        public const string LAS_DEB = "192.168.10.41";

        public const string EVT_SAT = "192.168.10.51";
        public const string EVT_DEB = "192.168.10.61";
        public const string OPT = "192.168.10.71";
        //public const string DOM = "192.168.10.11";  // 회사 테스트용
        public const string DOM = "192.168.10.81";
        public const string AID = "192.168.10.91";
    }


    public class ObservationData
    {
        public string SatelliteName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Interval_second { get; set; }
        public long[] UtcMilliseconds { get; set; }
        public double[][] LLA_data { get; set; }
    }

    /// <summary>
    /// RGG 모니터링을 위한 상태정보 메시지 형식
    /// </summary>
    public class MONITOR_RGG_DATA
    {
        public string ID { get; set; }
        public string RGGctrl { get; set; }
        public string GatePulseWidth { get; set; }
        public string GatePulseStartOffset { get; set; }
        public string AvoidSetPositionStartOffset { get; set; }
        public string AvoidWidth { get; set; }
        public string LookupTableSize { get; set; }
        public string UtcNow { get; set; }
        public string TofNow { get; set; }
    }
    /// <summary>
    ///  RGG 모니터링을 위한 BIT 메시지 형식
    /// </summary>
    public class MONITOR_RGG_BIT
    {
        public string ID { get; set; }
        public string BitResult { get; set; }
    }
    //////////////////////////////////////////////////////////////////////////////////////////// 
    /// <summary>
    /// LAS 모니터링을 위한 상태정보 메시지 형식
    /// </summary>
    public class MONITOR_LAS_DATA
    {
        public string ID { get; set; }
        public string LASopMode { get; set; } //레이저부 운용모드 
        public string LaserMode { get; set; } //레이저 모드 상태
        public string LaserFireState { get; set; } //레이저 조사 상태
        public string LASopEnd { get; set; } //레이저 운용 상태 
        public byte CbitResult1 { get; set; } //CBIT 결과_전원상태
        public byte CbitResult2 { get; set; } //CBIT 결과_동작상태
    }

    /// <summary>
    ///  RGG 모니터링을 위한 BIT 메시지 형식
    /// </summary>
    public class MONITOR_LAS_BIT
    {
        public string ID { get; set; } //PBIT or IBIT
        public byte BitResult1 { get; set; }
        public byte BitResult2 { get; set; }
    }
    ////////////////////////////////////////////////////////////////////////////////////////////    
    /// <summary>
    ///  RGG 모니터링을 위한 BIT 메시지 형식
    /// </summary>
    public class MONITOR_DOM_DATA
    {
       public string ID { get; set; }
        public string ShutterState { get; set; } //셔터상태     1:담힘  2:여는중  3:담힘  4:닫는중  5:IDLE 
        public string RainState { get; set; }  //강우상태  0:평상시  1:강우시
        public string DomeHomeState { get; set; }  //돔 홈상태  0:찾음  1:안찾음  2:Homming 
        public string DomeDrivingState { get; set; }  //돔 구동상태  1:포지션  2:추적  3:정지 
        public string DomePosition { get; set; }  //180.00
        public string BIT { get; set; }  //0:CBIT  1:PBIT   2:IBIT
        public string BitResult { get; set; }
    }
    public class MONITOR_DOM_BIT_RESULT
    {
        public string BIT {  set; get; }
        public string BitResult { get; set; }
        /*
        // Value  :  0  이상 /  1 정상 
        public string Encoder { get; set; } //[0] 엔코더상태 : 돔 회전(CW/CCW) OpenLoop으로 2도 구동 후 엔코더 펄스값 확인
        public string Motor1 { get; set; }  //[1] 구동모터1 : 돔 회전(CW/CCW) CloseLoop으로 2도 구동 후 모터 펄스값 확인
        public string Moter2 { get; set; }  //[2] 구동모터2  
        public string Moter3 { get; set; }  //[3] 구동모터3  
        public string Moter4 { get; set; }  //[4] 구동모터4
        public string Shutter_Moter { get; set; }  //[5] 셔터모터 : 5초간 동작하여  Limit Switch에 도달하느냐를 확인
        public string RainSensor { get; set; } //[6] 강우센서 
        */
    }



    ////////////////////////////////////////////////////////////////////////////////////////////    
    /// <summary>
    ///  항공기탐지 레이더를 위한 BIT 메시지 형식
    /// </summary>
    public class MONITOR_AID_DATA
    {
        public string ID { get; set; }
        public string AIDopMode { get; set; }  //운용모드   1:초기/종료   2:준비  3:운용  4:점검
        public string RadarSyncState { get; set; }  //KASI Radar 연결상태  1:연결됨 2:연결안됨
        public string ADSbSyncState { get; set; }  //ADS-B 연결상태  1:연결됨  2:연결안됨
        public string CBITresult { get; set; } // CBIT 수행 결과
        // [B0] 항공기탐지 운용모드 점검 0:정상, 1:고장
        // [B1] KASI Radar 연결상태 점검 0:연결, 1:연결X
        // [B2] ADS-B 연결     상태 점검 0:정상, 1:연결X
        // [B3] 항공기탐지 신호     점검 0:탐지x,1:탐지됨
    }
    /// <summary>
    ///  RGG 모니터링을 위한 BIT 메시지 형식
    /// </summary>
    public class MONITOR_AID_BIT
    {
        public string ID { get; set; } //PBIT or IBIT
        public string BitResult { get; set; }        
    }


    public class IPC_MSG
    {
        public const string CONNECTION_RGG_SAT = "SAT.RGG_CONNECTED";
        public const string DISCONNECTION_RGG_SAT = "SAT.RGG_DISCONNECTED";

        public const string CONNECTION_RGG_DEB = "DEB.RGG_CONNECTED";
        public const string DISCONNECTION_RGG_DEB = "DEB.RGG_DISCONNECTED";

        public const string CONNECTION_LAS_SAT = "SAT.LAS_CONNECTED";
        public const string DISCONNECTION_LAS_SAT = "SAT.LAS_DISCONNECTED";

        public const string CONNECTION_LAS_DEB = "DEB.LAS_CONNECTED";
        public const string DISCONNECTION_LAS_DEB = "DEB.LAS_DISCONNECTED";

        public const string CONNECTION_DOM = "DOM_CONNECTED";
        public const string DISCONNECTION_DOM = "DOM_DISCONNECTED";

        public const string CONNECTION_AID = "AID_CONNECTED";
        public const string DISCONNECTION_AID = "AID_DISCONNECTED";
    }

    public class ConnectedEventArgs : EventArgs
    {
        public string IsConnected { get; set; }
        public ConnectedEventArgs(string isConnected) { IsConnected = isConnected; }
    }

    public class PauseEventArgs : EventArgs
    {
        public string IsPaused { get; set; }
        public PauseEventArgs(string isPaused) { IsPaused = isPaused; }
    }


    public class RGG_MODE
    {
        public const string Standby  = "0001";
        public const string LUT_Send  = "0002";
        public const string LUT_Init  = "0003";
        public const string GroundCAL = "0004";
        public const string Ranging =   "0005";
    }

    // ChartData를 구조체 처럼 가지고 있을 Class
    public class ChartData
    {
        DataTable _ChartMain;
        SeriesChartType _ChartType = SeriesChartType.Area;

        public DataTable ChartMain { get => _ChartMain; set => _ChartMain = value; }
        public SeriesChartType ChartType { get => _ChartType; set => _ChartType = value; }
    }



    public delegate void delegate_StateIndicator(object sender, string strState);
    //public delegate void MessageHandler(string what, string messagge);


    public static class Global
    {
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateConfiguration();

        public static IntPtr config;
        public static IntPtr solarSystem;
        public static IntPtr OASCONST;

        public static IntPtr dynModel;

        public static IntPtr orbit;
        public static IntPtr epochTime;
        public static IntPtr sat;

        public static IntPtr laserSite;

        public static IntPtr timeSys;
        public static IntPtr coordSys;

        public static IntPtr obsClass;
        public static IntPtr meaModel;

        public static IntPtr estClass;
        public static IntPtr paramClass;

        public static IntPtr residual;
    }
    public static class StarCalInfo 
    {
        public enum Step
        {
            None,
            One,
            Two,
            Three,
        };
        public static bool StarCalFlag { get; set; }
        public static bool StarStartFlag { get; set; }
    }
    public static class GroundCalInfo
    {
        public enum Step
        {
            None,
            One,
            Two,
            Three,
        };
        public static bool GroundCalFlag { get; set; }
    }
    public class TrackingMount 
    {
        public const ushort PointingMode = (ushort)0x0100;
        public const ushort TrackingMode = (ushort)0x0200;
        public const ushort ServoEnable = (ushort)0x0301;
        public const ushort ServoDisable = (ushort)0x0302;
        public const ushort ServoHoming = (ushort)0x0400;

        public const ushort InitEndState = (ushort)0x0001;
        public const ushort ReadyState = (ushort)0x0002;
        public const ushort OperatingState = (ushort)0x0003;
        public const ushort InspectionState = (ushort)0x0004;
        public const ushort SafetyState = (ushort)0x0005;

    }
    public class ObservationSiteInfo 
    {
        public const double LATITUDE = 35.5901;
        public const double LONGITUDE = 127.9192;
        public const double ALTITUDE = 923.302744;

    }
    public class StarPositionSignMode
    {
        public const int Notyet = 0;
        public const int Positive = 1;
        public const int Negative = 2;
        public const int Changed = 3;
    }
    public class SetellitePositionSignMode
    {
        public const int Notyet = 0;
        public const int Positive = 1;
        public const int Negative = 2;
        public const int Changed = 3;
    }
    public static class TargetInfo
    {
        public enum Step
        {
            None,
            One,
            Two,
            Three,
        };
        public static bool TargetFlag { get; set; }
        public static bool TargetStartFlag { get; set; }
    }
    public static class Constants
    {
        public const uint DataLeadTime = 2000;
    }


    public static class DutyModeNow //운영제어부의 Main 임무인 위성관측 / 우주물체관측 구분
    {
        public enum SystemObject
        {
            [Description("Satellite Laser Ranging")]
            SLR,
            [Description("Debris Laser Tracking")]
            DLT,
        }
        private static SystemObject _currentSystemObject = SystemObject.SLR;

        public static SystemObject CurrentSystemObject
        {
            get { return _currentSystemObject; }
            set { _currentSystemObject = value; }
        }
    }

    public static class TaskModeNow //운영제어부의 Sub 과업인 Range / GroundCalibration 구분 
    {
        public enum SystemObject
        {
            [Description("Ranging")]
            RANGE,
            [Description("Ground Calibration")]
            G_CAL,
            [Description("Star Calibration")]
            S_CAL,
        }
        private static SystemObject _currentSystemObject = SystemObject.RANGE;

        public static SystemObject CurrentSystemObject
        {
            get { return _currentSystemObject; }
            set { _currentSystemObject = value; }
        }
    }


    public class TaskControl
    {
        private static TaskControl _instance;

        public bool task_mutex { get; set; }
        public string task_name { get; set; }

        //public const string NONE = "None";
        //public const string OBSV = "Observation";
        //public const string SCAL = "Star_Calibration";
        //public const string GCAL = "Ground_Calibration";
        //public const string CONF = "Config_Setting";
        //public const string UMNG = "User_Management";
        //public const string RECD = "Record_Management";
        //public const string SYSD = "System_Diagnosis";
        //public enum NAMEofTASK
        //{
        //    None = 0,
        //    Observation,
        //    Star_Calibration,
        //    Ground_Calibration,
        //    Config_Setting,
        //    User_Management,
        //    Record_Management,
        //    System_Diagnosis
        //};
        //NAMEofTASK taskNAME = NAMEofTASK.None;
        public static TaskControl instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TaskControl();
                }
                return _instance;
            }
        }


        public void SetMutex(string taskName)
        {
            if (taskName.Equals("None"))
            {
                task_mutex = false;
            }
            else
            {
                task_mutex = true;
            }
            task_name = taskName;
        }
    }



}
