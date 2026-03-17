using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.VisualStyles;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using Braincase.GanttChart;
using Emgu.CV.Dnn;
using Emgu.CV.Stitching;
using Npgsql;
using NSLR_ObservationControl.Module;
using NSLR_ObservationControl.OrbitData;
using NSLR_ObservationControl.Subsystem;

/*
using Emgu.CV;
using Emgu.CV.Geodetic;
*/
using OpenGlobe.Core;
using PvDotNet;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.Propagation;
using SGPdotNET.TLE;
using SGPdotNET.Util;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static log4net.Appender.RollingFileAppender;
using static NSLR_ObservationControl.CSU_ObserveSchedule;
using static NSLR_ObservationControl.tcspk;
using static NSLR_ObservationControl.Module.Observation_TMS;
using Microsoft.Extensions.Logging.Abstractions;
using OpenTK.Graphics.OpenGL;
namespace NSLR_ObservationControl
{
    public partial class CSU_ObserveSchedule2 : Form
    {
        /////////////////////////////// 스케줄러 DllImport ////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateConfiguration();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateSolarSystem();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateConstants();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateOrbitDynamics();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateSatellite();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateGroundSite(double[] loc, StringBuilder siteName, double waveLength);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateMeasurementModel();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateObservation();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateEstimator();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateParameters(IntPtr satClass);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateTimeSystem();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateCoordinateSystem();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateResidual();

        ///////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateTLE();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetTLEData(IntPtr tleClass, StringBuilder line1, StringBuilder line2, double[] epochTime);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ConvertGreg2MJD(IntPtr timeSystem, double[] greg);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ConvertMJD2Greg(IntPtr timeSystem, double mjd, double[] greg);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GenerateCommandFile_TLE(IntPtr tleClass, double[] siteLoc, double startMJD, double finalMJD, double stepSize, StringBuilder fileName, int opt);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetSiteLocation(IntPtr obsSite, double[] loc);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GenerateCommand_TLE(IntPtr tleClass, double[] siteLoc, double targetMJD, int opt, double[] command);
        ///////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenerateBodyLocationFile(StringBuilder _bodyName, double[] geodeticGS, double startMJD, double finalMJD, double stepSize, StringBuilder _fileName, int opt);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenerateBodyEventFile(StringBuilder _bodyName, double[] geodeticGS, double startMJD, double finalMJD, double cutoffAngle, StringBuilder _fileName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenerateLLA_TLE(IntPtr tleClass, double targetMJD, double[] command);

        ///////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateCPF();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ReadCPF(StringBuilder cpfFileName, IntPtr cpfData);
        /*
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double LoadCPFInfo(IntPtr cpfData, StringBuilder targetName);
        */
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenerateCommand_CPF(IntPtr cpfData, double[] siteLoc, double targetMJD, int opt, double[] command);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenerateCommandFile_CPF(IntPtr cpfData, double[] siteLoc, double startMJD, double finalMJD, double stepSize, StringBuilder _fileName, int opt);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern void GenerateLLA_CPF(IntPtr cpfData, double targetMJD, double[] command);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GenerateLLAFile_CPF(IntPtr cpfData, double startMJD, double finalMJD, double stepSize, StringBuilder fileName);
        ///////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ComputeLightTime(IntPtr meaModel, IntPtr laserSite, double[] MJDtags, double[] targetEphemeris, int nTime, double _TXtimeTag);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ComputeLightTime_array(IntPtr laserSite, double[] MJDtags, double[] targetEphemeris, int nTime, double[] _TXtimeTag, int nPoints, bool tropoFlag, double[] _env, double[] _tof);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LoadCPFInfo(IntPtr cpfData, StringBuilder targetName, ref double epochMJD);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LoadCPFEphemeris(IntPtr cpfData, StringBuilder _outCoord, double[] _elapsedSecs, double[] _ephemeris);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ReadEphemerisInfo(StringBuilder _ephemFileName, double[] startTime, double[] finalTime);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double LoadEphemeris(StringBuilder _ephemFileName, double[] mjdTags, double[] ephemeris);
        ///////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////// 궤도결정 DllImport ////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PropagateTLE(IntPtr tleClass, double[] elapsedSecs, int N, double[] ephemeris, int optCoord);
        ///////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LoadObservation(IntPtr obsClass, StringBuilder _fileName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetObservatoryInfo(IntPtr obsClass, int n, StringBuilder siteName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetNObsData(IntPtr obsClass, int n);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetObservationTime(IntPtr obsClass, int n, double[] _timeSet);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetOrbit(IntPtr _sat, double[] _stateVec, StringBuilder _type, StringBuilder _coordSys, StringBuilder epochType, StringBuilder _epochSys, double[] epochTime);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpdateParameterSatellite(IntPtr paramClass, IntPtr satClass);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetObservatory(IntPtr estClass, int code, StringBuilder name);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetObservable(IntPtr estClass, bool[] idxVec);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetParameterEpochTime(IntPtr paramClass, double[] dateVec);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetEstimatorTime(IntPtr estClass, StringBuilder type, double[] dateVec);
        ///////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetOrbitalState(IntPtr _sat, double[] stateVec, StringBuilder type);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LeastSquaresInitializer(IntPtr estClass, IntPtr paramClass, IntPtr obsClass, IntPtr estObsClass, IntPtr dynModel, double[] aprioriSolveforVec, double[] aprioriCov);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double LeastSquaresIterator(IntPtr estClass, IntPtr satClass, IntPtr paramClass, IntPtr obsClass, IntPtr dynModel, IntPtr meaModel, double[] aprioriSolveforVec, double[] aprioriCov, IntPtr resStr, double[] deltaX);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetResidualDataInfo(IntPtr resStr, int[] info);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetResidualRMS(IntPtr resStr, double[] _rms);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ApplyEstimation(IntPtr estClass, IntPtr satClass, IntPtr paramClass, double[] aprioriCov, double[] delx);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ConvergenceTest(IntPtr estClass, double costFnc, double oldCostFnc, int iter, ref bool diFlag);
        ///////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetEpochTime(IntPtr sat, double[] epochDate);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenerateEphemeris(IntPtr dynModel, IntPtr sat, double startMJD, double finalMJD, double stepSize, StringBuilder _fileName);
        ///////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GenerateLLAFile_Ephemeris(StringBuilder _ephemFileName, double startMJD, double finalMJD, double stepSize, StringBuilder fileName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenerateCommandFile_Ephemeris(StringBuilder _ephemFileName, double[] siteLoc, double startMJD, double finalMJD, double stepSize, StringBuilder _fileName, int opt);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenerateLLA_Ephemeris(StringBuilder _ephemFileName, double targetMJD, double[] command);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenerateCommand_Ephemeris(StringBuilder _ephemFileName, double[] siteLoc, double targetMJD, int opt, double[] command);
        ///////////////////////////////////////////////////////////////////////////////////


        ProjectManager _mManager = null;    // GanttChart 매니저
        ProjectManager _mManager_cpf = null;    // GanttChart 매니저(CPF)
        ProjectManager _mManager_ephemeris = null;  // GanttChart 매니저(Ephemeris)
        Progress_State progress_state;  // 진행상황 Form
        string recentTLE_directory = "./TLE";
        string recentTLE_file = "recent_tle.txt";
        string outputs_directory = "./Outputs";
        public static Dictionary<string, TLE> TLE_Information = new Dictionary<string, TLE>();


        public IntPtr tleClass; // tle class
        public IntPtr cpfClass; // cpf class
        public IntPtr sub_tleClass; // sub tle class
        public IntPtr sub_cpfClass; // sub cpf class
        public IntPtr observingSatellite_tleClass;  // 관측위성정보 전송용 tle class
        public IntPtr observingSatellite_cpfClass;  // 관측위성정보 전송용 cpf class

        ToolTip toolTip;    // Filtering Value 조건 설명

        RGG_SAT_Controller rggSATcontrol;
        RGG_DEB_Controller rggDEBcontrol;

        public delegate void SatelliteDataUpdatedHandler(DateTime startTime, DateTime endTime, string satelliteName, int noradId);
        public event SatelliteDataUpdatedHandler SatelliteDataUpdated;

       

        public CSU_ObserveSchedule2()
        {
            InitializeComponent();

            _mManager = new ProjectManager();
            _mManager_cpf = new ProjectManager();
            _mManager_ephemeris = new ProjectManager();
 
            rggSATcontrol = RGG_SAT_Controller.instance;
            rggDEBcontrol = RGG_DEB_Controller.instance;

        }

        private void CSU_ObserveSchedule2_Load(object sender, EventArgs e)
        {
            // UI 서브모니터 전시
            Screen[] scr = Screen.AllScreens;

            if (scr.Length > 1)
            {
                this.Location = scr[1].Bounds.Location;
            }


            // taskGridView 설정
            taskGridView.RowHeadersVisible = false;
            taskGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            taskGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            taskGridView.AllowUserToResizeColumns = false;
            taskGridView.AllowUserToResizeRows = false;
            taskGridView.EnableHeadersVisualStyles = false;
            taskGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            taskGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                taskGridView.ColumnHeadersDefaultCellStyle.BackColor;
            taskGridView.MultiSelect = false;
            taskGridView.ReadOnly = true;

            taskGridView.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 14,
                FontStyle.Bold);
            taskGridView.DefaultCellStyle.Font = new Font("맑은 고딕", 12, FontStyle.Regular);
            taskGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.CellClick += TaskGridView_CellClick;

            // CPFGridView 설정
            CPFGridView.RowHeadersVisible = false;
            CPFGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            CPFGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            CPFGridView.AllowUserToResizeColumns = false;
            CPFGridView.AllowUserToResizeRows = false;
            CPFGridView.EnableHeadersVisualStyles = false;
            CPFGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            CPFGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                CPFGridView.ColumnHeadersDefaultCellStyle.BackColor;
            CPFGridView.MultiSelect = false;
            CPFGridView.ReadOnly = true;

            CPFGridView.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 14,
                FontStyle.Bold);
            CPFGridView.DefaultCellStyle.Font = new Font("맑은 고딕", 12, FontStyle.Regular);
            CPFGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            CPFGridView.RowTemplate.Height = 30;
            CPFGridView.CellClick += CPFGridView_CellClick;

            // reference_dataGridView
            reference_dataGridView.RowHeadersVisible = false;
            reference_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            reference_dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            reference_dataGridView.AllowUserToResizeColumns = false;
            reference_dataGridView.AllowUserToResizeRows = false;
            reference_dataGridView.EnableHeadersVisualStyles = false;
            reference_dataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            reference_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                reference_dataGridView.ColumnHeadersDefaultCellStyle.BackColor;
            reference_dataGridView.MultiSelect = false;
            reference_dataGridView.ReadOnly = true;

            reference_dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 14,
                FontStyle.Bold);
            reference_dataGridView.DefaultCellStyle.Font = new Font("맑은 고딕", 12, FontStyle.Regular);
            reference_dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            startReference_txtbox.ReadOnly = true;
            endReference_txtbox.ReadOnly = true;

            // 궤도결정 Panel 내 DataGridView Setting
            Initialize_OrbitDetermination_DataGridView();

            // Filtering Value 설정 (시작 시간, 종료 시간, 고도, 고각, RCS, 타입)
            DateTime dateTimePicker_tempStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            //DateTime dataTimePicker_tempEnd = dateTimePicker_tempStart.AddHours(1);
            DateTime dataTimePicker_tempEnd = dateTimePicker_tempStart.AddMinutes(15);

            startDate_dateTimePicker.Value = dateTimePicker_tempStart;
            startTime_dateTimePicker.Value = dateTimePicker_tempStart;
            endDate_dateTimePicker.Value = dataTimePicker_tempEnd;
            endTime_dateTimePicker.Value = dataTimePicker_tempEnd;

            

            // 고도 Default Set
            altitude_startValue_txt.Text = "0";
            //altitude_endValue_txt.Text = "100000";
            altitude_endValue_txt.Text = "38000";

            // 고각 Default Set
            //elevation_startValue_txt.Text = "-90";
            elevation_startValue_txt.Text = "20";
            //elevation_endValue_txt.Text = "90";
            elevation_endValue_txt.Text = "85";

            // RCS 고정
            RCS_startValue_txt.Text = "0";
            RCS_endValue_txt.Text = "1000";
            RCS_startValue_txt.ReadOnly = true;
            RCS_endValue_txt.ReadOnly = true;

            string[] object_types = { "ALL", "PAYLOAD", "DEBRIS", "NONE"/*Unknown*/ };
            object_type_comboBox.Items.AddRange(object_types);
            object_type_comboBox.SelectedItem = "ALL";

            // 입력조건 설명창
            toolTip = new ToolTip();
            toolTip.AutomaticDelay = 0;
            toolTip.InitialDelay = 0;
            toolTip.ReshowDelay = 0;
            toolTip.ToolTipIcon = ToolTipIcon.Info;
            toolTip.ToolTipTitle = "참고";
            toolTip.SetToolTip(altitude_startValue_txt, "고도 입력 범위 : 0 이상" + "\r\n" + "※ 최소 고도는 최대 고도 보다 작은 값이어야함.");
            toolTip.SetToolTip(altitude_endValue_txt, "고도 입력 범위 : 0 이상" + "\r\n" + "※ 최대 고도는 최소 고도 보다 큰 값이어야함.");
            toolTip.SetToolTip(elevation_startValue_txt, "고각 입력 범위 : -90 이상, 90 이하" + "\r\n" + "※ 최소 고각는 최대 고각 보다 작은 값이어야함.");
            toolTip.SetToolTip(elevation_endValue_txt, "고각 입력 범위 : -90 이상, 90 이하" + "\r\n" + "※ 최대 고각는 최소 고각 보다 큰 값이어야함.");
            toolTip.SetToolTip(RCS_startValue_txt, "RCS 입력 범위 : 0 이상" + "\r\n" + "※ 최소 RCS는 최대 RCS 보다 작은 값이어야함.");
            toolTip.SetToolTip(RCS_endValue_txt, "RCS 입력 범위 : 0 이상" + "\r\n" + "※ 최대 RCS는 최소 RCS 보다 큰 값이어야함.");
            toolTip.SetToolTip(object_type_comboBox, "- ALL 선택 시 : 모든 위성 타입으로 검색" + "\r\n" + "- NONE 선택 시 : 위성 타입은 Unknown");

            // 검색 조건 오류(범위 or 형식)
            if (Set_FilteringSatelliteValue() == false)
            {
                return;
            }
            
            progress_state = new Progress_State();
            Thread init_thread = new Thread(() =>
            {
                // Progress_State 전시
                this.Invoke(new System.Action(() =>
                {
                    progress_state.Set_ProgressBarMaximum(1, 1);
                    progress_state.Set_ProgressBarValue(1, 0);
                    progress_state.Set_StateText(1, "환경 구성 중...");
                }));

                // 궤도 라이브러리 객체 Create
                Global.config = CreateConfiguration();
                Global.solarSystem = CreateSolarSystem();   // 태양위치데이터 기능사용 >> 주석 해제 * 문제사항(Delay 발생)
                Global.OASCONST = CreateConstants();

                Global.dynModel = CreateOrbitDynamics();
                Global.sat = CreateSatellite();

                //double[] siteLoc = { 126.16659925, 36.70035547, 0.234866 };
                double[] siteLoc = { 127.9192, 35.5901, 0.92330 }; // 거창 관측소
                StringBuilder siteName = new StringBuilder("Geochang");
                double waveLength = 532;
                Global.laserSite = CreateGroundSite(siteLoc, siteName, waveLength);

                Global.meaModel = CreateMeasurementModel();
                Global.obsClass = CreateObservation();

                Global.estClass = CreateEstimator();
                Global.paramClass = CreateParameters(Global.sat);

                Global.timeSys = CreateTimeSystem();
                Global.coordSys = CreateCoordinateSystem();

                Global.residual = CreateResidual();

                /////////// 궤도결정 테스트 (임시) ////////////
                //Orbital_Determination orbital_determination = new Orbital_Determination();
                ///////////////////////////////////////////////

                // TLE/CPF Class Create
                tleClass = CreateTLE();
                cpfClass = CreateCPF();
                sub_tleClass = CreateTLE();
                sub_cpfClass = CreateCPF();
                observingSatellite_tleClass = CreateTLE();
                observingSatellite_cpfClass = CreateCPF();

                // Celestrak TLE 최신본 다운로드
                URL_TLE_Reader();
                // FTP CPF 최신본 다운로드
                //DownloadFTP_CPF(); >> 운영제어부 시작 전 천문연 실행파일로 생성

                // Progress_State 전시
                this.Invoke(new System.Action(() =>
                {
                    progress_state.Set_ProgressBarValue(1, 1);
                    progress_state.Set_StateText(1, "TLE && CPF 다운로드 완료");
                }));

                // 스케줄러 생성
                RaDec_To_CreateTask(true, true, false);
            });
            init_thread.Start();

            progress_state.ShowDialog();
            init_thread.Join();


            // Sky View Init
            ChartInit();
            

            // 궤도결정 테스트용
            /*
            // 궤도 라이브러리 객체 Create
            Global.config = CreateConfiguration();
            Global.solarSystem = CreateSolarSystem();   // 태양위치데이터 기능사용 >> 주석 해제 * 문제사항(Delay 발생)
            Global.OASCONST = CreateConstants();

            Global.dynModel = CreateOrbitDynamics();
            Global.sat = CreateSatellite();

            //double[] siteLoc = { 126.16659925, 36.70035547, 0.234866 };
            double[] siteLoc = { 127.9192, 35.5901, 0.92330 }; // 거창 관측소
            StringBuilder siteName = new StringBuilder("Geochang");
            double waveLength = 532;
            Global.laserSite = CreateGroundSite(siteLoc, siteName, waveLength);

            Global.meaModel = CreateMeasurementModel();
            Global.obsClass = CreateObservation();

            Global.estClass = CreateEstimator();
            Global.paramClass = CreateParameters(Global.sat);

            Global.timeSys = CreateTimeSystem();
            Global.coordSys = CreateCoordinateSystem();

            Global.residual = CreateResidual();

            tleClass = CreateTLE();
            cpfClass = CreateCPF();
            observingSatellite_tleClass = CreateTLE();
            observingSatellite_cpfClass = CreateCPF();

            URL_TLE_Reader();
            Altitide_Elevation_Reader();
            */
            
        }


        public void CelesTrack_SATCAT_Reader() // CelesTrack SATCAT Data 불러오기
        {
            int progress_count = 0;

            bool read_flag = false;

            string strURL;
            HttpWebRequest webRequest;
            HttpWebResponse webResponse;
            try
            {
                FileInfo fileInfo = new FileInfo(outputs_directory + "/" + "SATCAT_Data3.txt");

                using (StreamWriter writer = fileInfo.CreateText())
                {
                    foreach (KeyValuePair<string, TLE> tle_information in TLE_Information)
                    {
                        if (read_flag == true)
                        {
                            strURL = "https://celestrak.org/satcat/records.php?NAME=" + tle_information.Key + "&FORMAT=CSV";
                            webRequest = (HttpWebRequest)WebRequest.Create(strURL);
                            webRequest.Method = "GET";
                            webResponse = (HttpWebResponse)webRequest.GetResponse();

                            using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                            {

                                while (!reader.EndOfStream)
                                {
                                    string line = reader.ReadLine();
                                    string[] line_word = line.Split(',');
                                    if (line_word[0] == tle_information.Key)
                                    {
                                        TLE_Information[tle_information.Key].SATCAT_Data.Object_Type = line_word[3];

                                        if (line_word[13] == "")
                                        {
                                            TLE_Information[tle_information.Key].SATCAT_Data.RCS = 0.0;
                                        }
                                        else
                                        {
                                            TLE_Information[tle_information.Key].SATCAT_Data.RCS = double.Parse(line_word[13]);
                                        }

                                        writer.WriteLine(tle_information.Key + "\t" + TLE_Information[tle_information.Key].SATCAT_Data.Object_Type + "\t" + TLE_Information[tle_information.Key].SATCAT_Data.RCS.ToString());
                                        break;
                                    }

                                }


                            }
                        }
                        else if (tle_information.Key == "STARLINK-32248")
                        {
                            read_flag = true;
                        }

                        //if (progress_count > 3) { break; }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + "\r\n" + progress_count.ToString());
            }



            this.Invoke(new System.Action(() =>
            {
                // Progress_State 종료
                progress_state.Close();

                // Gantt Chart 생성 및 전시
                MessageBox.Show("완료");
            }));



        }

        public void Altitide_Elevation_Reader() // 고도 및 고각 정보 저장
        {

            foreach (KeyValuePair<string, TLE> tle_information in TLE_Information)
            {
                StringBuilder line1 = new StringBuilder(tle_information.Value.Line1);
                StringBuilder line2 = new StringBuilder(tle_information.Value.Line2);
                double[] epochTime = new double[6];
                double noradID = GetTLEData(tleClass, line1, line2, epochTime);

                string str_time = (schedule_startTime.ToUniversalTime()).ToString("yyyy-MM-dd-HH-mm-ss.fff");
                string[] split_time = str_time.Split('-');
                double[] double_time = new double[split_time.Length];
                for (int i = 0; i < double_time.Length; i++)
                {
                    double_time[i] = double.Parse(split_time[i]);
                }
                double current_MJD = ConvertGreg2MJD(Global.timeSys, double_time);

                double[] siteloc = new double[3];
                GetSiteLocation(Global.laserSite, siteloc);

                double[] LLA_result = new double[3];    // 경도, 위도, 고도
                GenerateLLA_TLE(tleClass, current_MJD, LLA_result);

                double[] AZEL_result = new double[2];   // 방위각, 고각
                GenerateCommand_TLE(tleClass, siteloc, current_MJD, 0, AZEL_result);

                // 결과값 저장
                TLE_Information[tle_information.Key].Norad = (int)noradID;
                TLE_Information[tle_information.Key].Altitude = LLA_result[2];
                TLE_Information[tle_information.Key].Elevation = AZEL_result[1];    // (임시) StartTime에 대한 고각
            }

        }

        public void SATCAT_Data_Reader() // Object Type 및 RCS 정보 저장 >> 파일로 저장(임시) 데이터베이스 사용? 여부 판단 
        {
            FileInfo fileInfo = new FileInfo(outputs_directory + "/" + "SATCAT_Data.txt");

            if (fileInfo.Exists)
            {
                // SATCAT 정보 Read
                using (var SATCAT_reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                {
                    while (!SATCAT_reader.EndOfStream)
                    {
                        string line = SATCAT_reader.ReadLine();
                        string[] line_word = line.Split('\t');

                        // 해당 위성이 TLE_Information 에 포함되어 있을 경우
                        if (TLE_Information.ContainsKey(line_word[0]) == true)
                        {
                            // Object Type
                            if (line_word[1].Contains("PAY")) { TLE_Information[line_word[0]].SATCAT_Data.Object_Type = "PAYLOAD"; }
                            else { TLE_Information[line_word[0]].SATCAT_Data.Object_Type = "NONE"; }

                            // RCS
                            if (String.IsNullOrEmpty(line_word[2])) { TLE_Information[line_word[0]].SATCAT_Data.RCS = 0.0; }
                            else { TLE_Information[line_word[0]].SATCAT_Data.RCS = double.Parse(line_word[2]); }

                        }
                    }
                }

            }


        }

        // n2yo tle 정보
        string[,] n2yo_tle =
        {
            { "Cosmos 1833 (ID:17589)", "1 17589U 87027A   24346.86889417 -.00000201  00000-0 -81784-4 0  9991", "2 17589  70.9145 197.7976 0024084 172.9404 187.2061 14.12991578946347" },
            { "MOS 1-B Rocket (ID:20491)", "1 20491U 90013D   24346.77722382 -.00000816  00000-0 -14626-2 0  9994", "2 20491  99.0161   4.6212 0468102  14.4967 346.9060 13.03694977657040" },
            { "USA 215 (ID:37162)", "1 37162U 10046A   24301.20358907 0.00000000  00000-0  00000-0 0    07", "2 37162 122.9901 240.5357 0005797 125.1274 234.8726 13.41445942    00" },
            { "SL-8 R/B (ID:6271)", "1 06271U 72087J   24346.81629936 -.00000054  00000-0 -30899-3 0  9996", "2 06271  74.0420   6.8393 0079871 180.9198 352.5643 12.34551636348438" },
            { "USA 281 (ID:43145)", "1 43145U 18005A   24343.22421568 0.00000000  00000-0  00000-0 0    00", "2 43145 106.0292 181.2568 0001964 152.7994 207.2006 13.47980625    09" },
            { "USA 234 (ID:38109)", "1 38109U 12014A   24310.81705936 0.00000000  00000-0  00000-0 0    01", "2 38109 122.9967  89.9792 0007563 181.7757 178.2242 13.41449638    09" },

            { "DELTA 2 R/B(1) (ID:22176)", "1 22176U 92066B   24346.81975852  .00000076  00000-0 -63774-5 0  9991", "2 22176  25.0926   8.2985 0964828 250.8506 281.9630 10.85565464276973" }

        };

        public void URL_TLE_Reader()    // TLE 파일 최신본 업로드 및 TLE 정보 업데이트
        {


            // 프로그램 시작 시 TLE 파일 최신여부 Check (서버 액세스 거부 방지용)
            if (!Directory.Exists(recentTLE_directory))
            {
                MessageBox.Show("TLE 디렉토리 없음...");
                return;
            }

            // Progress_State 전시 >> 디버깅으로 인한 임시 주석

            this.Invoke(new System.Action(() =>
            {
                progress_state.Set_StateText(1, "TLE && CPF 다운로드 중...");
            }));
            

            // n2yo TLE 정보 저장
            /*
            for (int i = 0; i < n2yo_tle.Length / 3; i++)
            {
                TLE_Information.Add(n2yo_tle[i, 0], new TLE(n2yo_tle[i, 1], n2yo_tle[i, 2]));
            }
            */

            FileInfo fileInfo = new FileInfo(recentTLE_directory + "/" + recentTLE_file);
            
            if (fileInfo.Exists)
            {
                if (((fileInfo.LastWriteTime).Year == DateTime.Now.Year) && ((fileInfo.LastWriteTime).Month == DateTime.Now.Month) && ((fileInfo.LastWriteTime).Day == DateTime.Now.Day))
                {
                    // TLE 정보만 업데이트
                    using (var tle_reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                    {
                        while (!tle_reader.EndOfStream)
                        {
                            string name = tle_reader.ReadLine().Trim();
                            string line1 = tle_reader.ReadLine();
                            string line2 = tle_reader.ReadLine();

                            if (name == null || line1 == null || line2 == null)
                                break;

                            if (name.StartsWith("STARLINK", StringComparison.OrdinalIgnoreCase) ||
                                name.StartsWith("ONEWEB", StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            if (!TLE_Information.ContainsKey(name))
                            {
                                TLE_Information.Add(name, new TLE(line1, line2));
                            }
                        }
                    }

                    //Altitide_Elevation_Reader();    // 고도 및 고각 정보 저장
                    //SATCAT_Data_Reader();   // Object Type 및 RCS 정보 저장
                    return;
                }
                else
                {
                    fileInfo.Delete();
                }
            }


            // string strURL;
            // HttpWebRequest webRequest;
            // HttpWebResponse webResponse;
            string[] urls = new string[]
            {
                "https://celestrak.org/NORAD/elements/gp.php?GROUP=active&FORMAT=tle",
                "https://celestrak.org/NORAD/elements/gp.php?GROUP=visual&FORMAT=tle"
            };

            foreach (string strURL in urls)
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(strURL);
                webRequest.Method = "GET";
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        using (StreamWriter writer = fileInfo.AppendText())
                        {
                            while (!reader.EndOfStream)
                            {
                                string name = reader.ReadLine()?.Trim();
                                string line1 = reader.ReadLine();
                                string line2 = reader.ReadLine();

                                if (name == null || line1 == null || line2 == null)
                                    break;

                                writer.WriteLine(name);
                                writer.WriteLine(line1);
                                writer.WriteLine(line2);

                                // STARLINK / ONEWEB 필터
                                if (name.StartsWith("STARLINK", StringComparison.OrdinalIgnoreCase) ||
                                    name.StartsWith("ONEWEB", StringComparison.OrdinalIgnoreCase))
                                {
                                    continue;
                                }

                                if (!TLE_Information.ContainsKey(name))
                                {
                                    TLE_Information.Add(name, new TLE(line1, line2));
                                }
                            }

                        }
                    }
                }

            }

            //Altitide_Elevation_Reader();    // 고도 및 고각 정보 저장
            //SATCAT_Data_Reader();   // Object 및 RCS 정보 저장
        }



        public double[][] Sun_Moon_AZEL_Reader(DateTime startTime, DateTime endTime, int duration, string planet) // 태양 위치데이터(AZ/EL) 반환 (StartTime ~ EndTime)
        {
            // Data 파일 생성
            double[] start_time = new double[6];
            double[] end_time = new double[6];

            DateTime date_time = startTime.AddSeconds(-1 * 60); // StartTime 설정 시, StartTime + 60초 부터 출력 >> StartTime - 60초 설정
            start_time[0] = Convert.ToDouble(date_time.Year);
            start_time[1] = Convert.ToDouble(date_time.Month);
            start_time[2] = Convert.ToDouble(date_time.Day);
            start_time[3] = Convert.ToDouble(date_time.Hour);
            start_time[4] = Convert.ToDouble(date_time.Minute);
            start_time[5] = 0.0;
            double start_MJD = ConvertGreg2MJD(Global.timeSys, start_time);

            date_time = endTime;
            end_time[0] = Convert.ToDouble(date_time.Year);
            end_time[1] = Convert.ToDouble(date_time.Month);
            end_time[2] = Convert.ToDouble(date_time.Day);
            end_time[3] = Convert.ToDouble(date_time.Hour);
            end_time[4] = Convert.ToDouble(date_time.Minute);
            end_time[5] = 0.0;
            double end_MJD = ConvertGreg2MJD(Global.timeSys, end_time);

            double stepSize = 60.0;

            StringBuilder fileName = new StringBuilder();
            if (planet == "Sun")
            {
                fileName = new StringBuilder("Sun_AZEL_Data.txt");
            }
            else if (planet == "Moon")
            {
                fileName = new StringBuilder("Moon_AZEL_Data.txt");
            }
            
            double[] geodeticGS = new double[3];
            GetSiteLocation(Global.laserSite, geodeticGS);

            // Sun or Moon
            StringBuilder bodyName = new StringBuilder(planet);

            GenerateBodyLocationFile(bodyName, geodeticGS, start_MJD, end_MJD, stepSize, fileName, 0);

            // Data 불러오기
            double[][] sunAzEl = new double[2][];
            for (int i = 0; i < sunAzEl.Length; i++) { sunAzEl[i] = new double[duration]; }

            FileInfo fileInfo = new FileInfo(outputs_directory + "/" + fileName.ToString());

            if (fileInfo.Exists)
            {
                int receive_count = 0;

                using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                {
                    int count = 0;
                    while (!reader.EndOfStream)
                    {
                        count++;
                        string line = reader.ReadLine();
                        string[] line_words = line.Split('\t');

                        if (count >= 12)
                        {
                            if (line_words[0].Contains("END Ephemeris"))
                            {
                                // 끝
                            }
                            else if ((Math.Round(double.Parse(line_words[0]), 0) % 60.0) == 0)
                            {
                                if (receive_count < duration)
                                {
                                    // 형변환 실패 시, out 매개변수에 0 할당
                                    double.TryParse(line_words[1], out sunAzEl[0][receive_count]);
                                    double.TryParse(line_words[2], out sunAzEl[1][receive_count]);

                                    receive_count++;
                                }
                            }
                        }
                    }

                }

            }

            return sunAzEl;
        }
        public double[][] Satellite_AZEL_Reader(string satellite, DateTime current_UTC, int duration) // 현재시간 기준 24시간, 위성 위치데이터(AZ/EL) 반환
        {
            double[][] satAzEl = new double[2][];

            for (int i = 0; i < satAzEl.Length; i++) { satAzEl[i] = new double[duration]; }

            // TLE 추출
            StringBuilder line1 = new StringBuilder(TLE_Information[satellite].Line1);
            StringBuilder line2 = new StringBuilder(TLE_Information[satellite].Line2);

            // TLE 정보 입력 및 위성에 대한 Epoch Time 산출
            double[] epochTime = new double[6];
            GetTLEData(tleClass, line1, line2, epochTime);

            // Site Location 설정
            double[] siteloc = new double[3];
            GetSiteLocation(Global.laserSite, siteloc);

            // Az/El 값 산출
            double timeMJD;
            double[] result = new double[2];
            for (int i = 0; i < satAzEl[0].Length; i++)
            {
                string str_time = (current_UTC.AddMinutes(i)).ToString("yyyy-MM-dd-HH-mm-ss.fff");
                string[] split_time = str_time.Split('-');
                double[] double_time = new double[split_time.Length];
                for (int j = 0; j < double_time.Length; j++)
                {
                    double_time[j] = double.Parse(split_time[j]);
                }

                timeMJD = ConvertGreg2MJD(Global.timeSys, double_time);
                GenerateCommand_TLE(tleClass, siteloc, timeMJD, 0, result);

                satAzEl[0][i] = result[0];    // Azimuth
                satAzEl[1][i] = result[1];    // Elevation
            }

            return satAzEl;
        }


        public double Filtering_SunPosition(double sun_azimuth, double sun_elevation, double sat_azimuth, double sat_elevation) // 태양-위성 구면좌표계상 거리 산출
        {
            // Degree To Radian
            double radian_sunAz = sun_azimuth * (Math.PI / 180.0);
            double radian_sunEl = sun_elevation * (Math.PI / 180.0);
            double radian_satAz = sat_azimuth * (Math.PI / 180.0);
            double radian_satEl = sat_elevation * (Math.PI / 180.0);

            // 구면 좌표계상 두 지점 사이의 거리
            double distance = Math.Acos((Math.Cos(90.0 - radian_sunEl) * Math.Cos(90.0 - radian_satEl)) + (Math.Sin(90.0 - radian_sunEl) * Math.Sin(90.0 - radian_satEl) * Math.Cos(radian_sunAz - radian_satAz)));

            // Radian To Degree
            double result = distance * (180.0 / Math.PI);

            return result;
        }

        public DateTime[] Get_Sunrise_Sunset(DateTime standard_UTC)   // 일출, 일몰 시간(UTC) 반환
        {
            double[] current_UTC = new double[6];
            double[] addDay_UTC = new double[6];

            DateTime date_time = standard_UTC;
            current_UTC[0] = Convert.ToDouble(date_time.Year);
            current_UTC[1] = Convert.ToDouble(date_time.Month);
            current_UTC[2] = Convert.ToDouble(date_time.Day);
            current_UTC[3] = Convert.ToDouble(date_time.Hour);
            current_UTC[4] = 0.0;
            current_UTC[5] = 0.0;
            double current_MJD = ConvertGreg2MJD(Global.timeSys, current_UTC);

            date_time = date_time.AddDays(1);
            addDay_UTC[0] = Convert.ToDouble(date_time.Year);
            addDay_UTC[1] = Convert.ToDouble(date_time.Month);
            addDay_UTC[2] = Convert.ToDouble(date_time.Day);
            addDay_UTC[3] = Convert.ToDouble(date_time.Hour);
            addDay_UTC[4] = 0.0;
            addDay_UTC[5] = 0.0;
            double addDay_MJD = ConvertGreg2MJD(Global.timeSys, addDay_UTC);

            StringBuilder bodyName = new StringBuilder("Sun");
            double[] geodeticGS = new double[3];
            GetSiteLocation(Global.laserSite, geodeticGS);

            double cutoffAngle = 0.0;   // temp
            StringBuilder fileName = new StringBuilder("Sunrise_Sunset_Time.txt");

            GenerateBodyEventFile(bodyName, geodeticGS, current_MJD, addDay_MJD, cutoffAngle, fileName);    // 파일 생성


            DateTime[] riseSet_time = new DateTime[2];

            FileInfo fileInfo = new FileInfo(outputs_directory + "/" + fileName.ToString());

            if (fileInfo.Exists)
            {
                using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] line_words = line.Split(' ');

                        if (line_words[0].Contains("Rise") && line_words[1].Contains("Time"))
                        {
                            // 일출시간
                            string[] split_date = line_words[2].Split('/');
                            string[] split_time = line_words[3].Split(':');
                            string[] split_second = split_time[2].Split('.');

                            riseSet_time[0] = new DateTime(int.Parse(split_date[0]), int.Parse(split_date[1]), int.Parse(split_date[2]),
                                int.Parse(split_time[0]), int.Parse(split_time[1]), int.Parse(split_second[0]), DateTimeKind.Utc);

                        }
                        else if (line_words[0].Contains("Set") && line_words[1].Contains("Time"))
                        {
                            // 일몰시간
                            string[] split_date = line_words[2].Split('/');
                            string[] split_time = line_words[3].Split(':');
                            string[] split_second = split_time[2].Split('.');

                            riseSet_time[1] = new DateTime(int.Parse(split_date[0]), int.Parse(split_date[1]), int.Parse(split_date[2]),
                                int.Parse(split_time[0]), int.Parse(split_time[1]), int.Parse(split_second[0]), DateTimeKind.Utc);

                            break;
                        }
                    }

                }

            }

            return riseSet_time;
        }

        

        DateTime schedule_startTime;
        DateTime schedule_endTime;
        int schedule_duration;

        bool[] check_sunAvoidance;
        double[][] sunAZEL;
        double[][] moonAZEL;

        int total_page = 0;
        int current_page = 0;

        List<MyTask> myTasks = new List<MyTask>();
        List<TaskGridView_Info> taskGridView_infos = new List<TaskGridView_Info>();
        DateTime[] sunRiseSet_time;
        System.Drawing.Brush[] total_brush = new System.Drawing.Brush[3]; // 1. Red / 2. Black / 3. Yellow
        public void RaDec_To_CreateTask(bool filtering_flag, bool load_tle, bool load_cpf)   // 각 위성(RaDec 기반)에 대한 Task 생성
        {
            // 날짜 + 시간 Setting
            schedule_startTime = new DateTime(startDate_dateTimePicker.Value.Year, startDate_dateTimePicker.Value.Month, startDate_dateTimePicker.Value.Day,
                startTime_dateTimePicker.Value.Hour, startTime_dateTimePicker.Value.Minute, 0);
            schedule_endTime = new DateTime(endDate_dateTimePicker.Value.Year, endDate_dateTimePicker.Value.Month, endDate_dateTimePicker.Value.Day,
                endTime_dateTimePicker.Value.Hour, endTime_dateTimePicker.Value.Minute, 0);

            // Schedule Total Minute 산출
            TimeSpan diff_dateTime = schedule_endTime - schedule_startTime;
            schedule_duration = (int)diff_dateTime.TotalMinutes;

            this.Invoke(new System.Action(() =>
            {
                period_label.Text = String.Format("{0:D4}년 {1:D2}월 {2:D2}일 {3:D2}시 {4:D2}분 ~ " +
                    "{5:D4}년 {6:D2}월 {7:D2}일 {8:D2}시 {9:D2}분",
                    schedule_startTime.Year, schedule_startTime.Month, schedule_startTime.Day, schedule_startTime.Hour, schedule_startTime.Minute,
                    schedule_endTime.Year, schedule_endTime.Month, schedule_endTime.Day, schedule_endTime.Hour, schedule_endTime.Minute);
            }));

            // 일출, 일몰 시간    * 문제사항(Delay 발생)
            DateTime standard_dateTime = new DateTime(schedule_startTime.Year, schedule_startTime.Month, schedule_startTime.Day, 0, 0, 0);
            sunRiseSet_time = Get_Sunrise_Sunset(standard_dateTime.ToUniversalTime());
            //sunRiseSet_time = new DateTime[2] { DateTime.UtcNow, DateTime.UtcNow };    // 임시용(Get_Sunrise_Sunset 오류 발생 시 해당부분 사용)

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            // 태양회피 전구간 적용
            check_sunAvoidance = new bool[schedule_duration];
            //for (int i = 0; i < check_sunAvoidance.Length; i++) { check_sunAvoidance[i] = true; }

            // 낮 : 태양회피 / 밤 : 달회피
            for (int i = 0; i < check_sunAvoidance.Length; i++)
            {
                DateTime check_dateTime = schedule_startTime.ToUniversalTime().AddMinutes(i);

                if (DateTime.Compare(check_dateTime, sunRiseSet_time[0]) == -1)
                {
                    // (CheckTime < RiseTime) : 달 회피
                    check_sunAvoidance[i] = false;
                }
                else if (DateTime.Compare(check_dateTime, sunRiseSet_time[0]) == 0 ||
                    (DateTime.Compare(check_dateTime, sunRiseSet_time[0]) == 1 && DateTime.Compare(check_dateTime, sunRiseSet_time[1]) == -1))
                {
                    // (RiseTime = CheckTime) or (RiseTime < CheckTime < SetTime) : 태양 회피
                    check_sunAvoidance[i] = true;
                }
                else if (DateTime.Compare(check_dateTime, sunRiseSet_time[1]) == 0 || DateTime.Compare(check_dateTime, sunRiseSet_time[1]) == 1)
                {
                    // (SetTime = CheckTime) or (SetTime < CheckTime) : 달회피
                    check_sunAvoidance[i] = false;
                }

            }

            // 태양 위치데이터(Az/El) 저장
            sunAZEL = Sun_Moon_AZEL_Reader(schedule_startTime.ToUniversalTime(), schedule_endTime.ToUniversalTime(), schedule_duration, "Sun");

            // 달 위치데이터(Az/El) 저장
            moonAZEL = Sun_Moon_AZEL_Reader(schedule_startTime.ToUniversalTime(), schedule_endTime.ToUniversalTime(), schedule_duration, "Moon");

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            //////////////////////////////////// CPF 목록 생성 + 고도 추출 ////////////////////////////////////////
            Thread cpf_thread = new Thread(() =>
            {
                Display_CPFList(check_sunAvoidance, sunAZEL, moonAZEL);
            });

            if (load_cpf == false)
            {
                // Progress_State 전시
                this.Invoke(new System.Action(() =>
                {
                    progress_state.Set_ProgressBarMaximum(3, 0);
                    progress_state.Set_ProgressBarValue(3, 0);
                    progress_state.Set_StateText(3, "CPF 제외");
                }));
            }
            else
            {
                cpf_thread.Start();
            }
            
            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            if (load_tle == false)
            {
                // Progress_State 전시
                this.Invoke(new System.Action(() =>
                {
                    progress_state.Set_ProgressBarMaximum(2, 0);
                    progress_state.Set_ProgressBarValue(2, 0);
                    progress_state.Set_StateText(2, "TLE 제외");
                }));
            }
            else
            {
                // Progress_State 전시
                this.Invoke(new System.Action(() =>
                {
                    progress_state.Set_ProgressBarMaximum(2, TLE_Information.Count);
                    progress_state.Set_ProgressBarValue(2, 0);
                    progress_state.Set_StateText(2, "TLE 목록 생성 중...");
                }));
                int progress_count = 0;


                // 고도 및 고각 정보 저장
                Altitide_Elevation_Reader();
                // Object Type 및 RCS 정보 저장
                SATCAT_Data_Reader();

                total_brush[0] = System.Drawing.Brushes.Red;
                total_brush[1] = System.Drawing.Brushes.Black;
                total_brush[2] = System.Drawing.Brushes.Yellow;

                foreach (KeyValuePair<string, TLE> tle_information in TLE_Information)
                {
                    if (filtering_flag == false || (filtering_flag == true &&
                            tle_information.Value.Altitude >= Filtering_SatelliteValue.Minimum_Altitude && tle_information.Value.Altitude <= Filtering_SatelliteValue.Maximum_Altitude &&
                            /*tle_information.Value.Elevation >= Filtering_SatelliteValue.Minimum_Elevation && tle_information.Value.Elevation <= Filtering_SatelliteValue.Maximum_Elevation &&*/
                            tle_information.Value.SATCAT_Data.RCS >= Filtering_SatelliteValue.Minimum_RCS && tle_information.Value.SATCAT_Data.RCS <= Filtering_SatelliteValue.Maximum_RCS &&
                            (tle_information.Value.SATCAT_Data.Object_Type == Filtering_SatelliteValue.Object_Type || Filtering_SatelliteValue.Object_Type == "ALL")))
                    {
                        // Task 생성
                        MyTask myTask = new MyTask(_mManager) { Name = tle_information.Key };

                        // Split 갯수 및 색상 설정
                        int division_count = 0;
                        int[] temp_duration = new int[schedule_duration];
                        System.Drawing.Brush[] temp_brush = new System.Drawing.Brush[schedule_duration];


                        double[][] satAZEL = Satellite_AZEL_Reader(myTask.Name, schedule_startTime.ToUniversalTime(), schedule_duration);
                        bool elevation_check = false;

                        // 최대고각
                        double max_elevation = 0.0;

                        for (int i = 0; i < schedule_duration; i++)
                        {
                            // 고각이 범위안에 드는 위성인지 Checking
                            if (satAZEL[1][i] >= Filtering_SatelliteValue.Minimum_Elevation && satAZEL[1][i] <= Filtering_SatelliteValue.Maximum_Elevation)
                            {
                                elevation_check = true;
                            }

                            // 최대고각 Set
                            if (i > 0)
                            {
                                if (satAZEL[1][i] > max_elevation) { max_elevation = satAZEL[1][i]; }
                            }
                            else { max_elevation = satAZEL[1][i]; }

                            if (satAZEL[1][i] > 85.0 || satAZEL[1][i] < 20.0)
                            {
                                // 관측 불가 : 고각 제한
                                if (i > 0)
                                {
                                    if (temp_brush[division_count] == total_brush[1])
                                    {
                                        temp_duration[division_count]++;
                                    }
                                    else
                                    {
                                        division_count++;
                                        temp_duration[division_count] = 1;
                                        temp_brush[division_count] = total_brush[1];
                                    }
                                }
                                else if (i == 0)
                                {
                                    temp_duration[division_count] = 1;
                                    temp_brush[division_count] = total_brush[1];
                                }
                            }
                            else if ((check_sunAvoidance[i] == true && Filtering_SunPosition(sunAZEL[0][i], sunAZEL[1][i], satAZEL[0][i], satAZEL[1][i]) < 30.0) ||
                                (check_sunAvoidance[i] == false && Filtering_SunPosition(moonAZEL[0][i], moonAZEL[1][i], satAZEL[0][i], satAZEL[1][i]) < 30.0))
                            {
                                // 관측 불가 : 태양/달 회피
                                if (i > 0)
                                {
                                    if (temp_brush[division_count] == total_brush[0])
                                    {
                                        temp_duration[division_count]++;
                                    }
                                    else
                                    {
                                        division_count++;
                                        temp_duration[division_count] = 1;
                                        temp_brush[division_count] = total_brush[0];
                                    }
                                }
                                else if (i == 0)
                                {
                                    temp_duration[division_count] = 1;
                                    temp_brush[division_count] = total_brush[0];
                                }
                            }
                            else
                            {
                                // 관측 가능
                                if (i > 0)
                                {
                                    if (temp_brush[division_count] == total_brush[2])
                                    {
                                        temp_duration[division_count]++;
                                    }
                                    else
                                    {
                                        division_count++;
                                        temp_duration[division_count] = 1;
                                        temp_brush[division_count] = total_brush[2];
                                    }
                                }
                                else if (i == 0)
                                {
                                    temp_duration[division_count] = 1;
                                    temp_brush[division_count] = total_brush[2];
                                }
                            }

                        }

                        if (elevation_check == true)
                        {
                            // Task Duration/Brush Setting
                            int[] task_duration = new int[division_count + 1];
                            System.Drawing.Brush[] task_brush = new System.Drawing.Brush[division_count + 1];
                            Array.Copy(temp_duration, task_duration, task_duration.Length);
                            Array.Copy(temp_brush, task_brush, task_brush.Length);
                            Split_InterfaceFunction(_mManager, myTask, task_duration, task_brush);

                            // Task 추가
                            myTasks.Add(myTask);
                            _mManager.Add(myTask);
                            _mManager.SetDuration(myTask, TimeSpan.FromMinutes(schedule_duration));
                            if (myTasks.Count > 1000) { _mManager.Delete(myTask); } // 스케줄 Page 분할

                            // DateGridView 정보 생성
                            TaskGridView_Info taskGridView_info = new TaskGridView_Info(tle_information.Key);
                            taskGridView_info.NoradID = tle_information.Value.Norad;
                            taskGridView_info.Altitude = Math.Round(tle_information.Value.Altitude, 5);
                            taskGridView_info.MaxElevation = Math.Round(max_elevation, 5)/*Math.Round(tle_information.Value.Elevation, 5)*/;
                            taskGridView_info.RCS = Math.Round(tle_information.Value.SATCAT_Data.RCS, 5);
                            taskGridView_info.Type = tle_information.Value.SATCAT_Data.Object_Type;
                            taskGridView_infos.Add(taskGridView_info);
                        }
                        else
                        {
                            // 필터링고각 요건충족 X
                        }

                    }

                    // Progress_State 전시
                    this.Invoke(new System.Action(() =>
                    {
                        progress_count++;
                        progress_state.Set_ProgressBarValue(2, progress_count);
                    }));
                }

                // Progress_State 전시
                this.Invoke(new System.Action(() =>
                {
                    progress_state.Set_StateText(2, "TLE 목록 생성 완료");
                }));


            }



            // CPF 목록생성 여부 Checking
            if (load_cpf == true)
            {
                cpf_thread.Join();
            }
            
            Thread.Sleep(2000);

            // CPFGridView UI 초기화 문제로 인한 위치 변경 //

            this.Invoke(new System.Action(() =>
            {
                // Progress_State 종료
                progress_state.Close();

                // Gantt Chart 생성 및 전시
                Display_GanttChart();

                // 위성 추적(관측) 스케줄러 Init
                Initialize_TrackingChart();

                // Ephimeris 스케줄러 Init
                Initialize_EphimerisGanttChart();

            }));
            
        }

        private void addTLE_button_Click(object sender, EventArgs e)    // TLE 추가
        {
            string satellite_name = "";
            string tle_line1 = "";
            string tle_line2 = "";


            // AddTLE_Form 정보 불러오기
            using (var addTLE_form = new AddTLE_Form())
            {
                DialogResult dialogResult = addTLE_form.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    
                    satellite_name = addTLE_form.satellite_name;
                    tle_line1 = addTLE_form.line1;
                    tle_line2 = addTLE_form.line2;
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    return;
                }
            }


            // 앞/뒤 공백 제거 (위성 정보 유효성 검사 → 추후 판단)
            satellite_name = satellite_name.Trim();
            tle_line1 = tle_line1.Trim();
            tle_line2 = tle_line2.Trim();


            // 위성명 중복여부 Check
            if (TLE_Information.ContainsKey(satellite_name))
            {
                MessageBox.Show("해당 위성은 이미 존재합니다. (중복 : 위성명)");
                return;
            }


            // Norad ID 중복여부 Check + TLE 유효성 Check
            int norad = 0; 
            try
            {
                double[] epoch = new double[6];
                norad = (int)GetTLEData(tleClass, new StringBuilder(tle_line1), new StringBuilder(tle_line2), epoch);
            }
            catch (Exception ex)
            {
                MessageBox.Show("유효하지 않은 TLE 입니다.");
                return;
            }
            
            foreach (KeyValuePair<string, TLE> tle_information in TLE_Information)
            {
                if (tle_information.Value.Norad == norad)
                {
                    MessageBox.Show("해당 위성은 이미 존재합니다. (중복 : Norad ID)");
                    return;
                }
            }


            // 위성 리스트 추가
            TLE_Information.Add(satellite_name, new TLE(tle_line1, tle_line2));


            // taskGridView 및 CPFGridView 선택 해제
            taskGridView.ClearSelection();
            CPFGridView.ClearSelection();


            // Manager/Gantt Chart 및 DataGridView 초기화
            _mManager = new ProjectManager();
            _mManager.Start = schedule_startTime;
            _mChart.Init(_mManager);
            _mChart.Invalidate();

            if (taskGridView.DataSource != null)
            {
                taskGridView.Columns.Clear();
                taskGridView.DataSource = null;
            }


            // 위성/우주물체 정보 초기화 + Thread 정지
            Initialize_Satellite_Information();


            // GroundTrack 및 SkyView 초기화
            GroundTrack.Image = null;
            GroundTrack.Update();
            ChartPointClear();


            // Reference Data 관련요소 초기화
            Initialize_Reference();


            // 고도 및 고각 정보 저장
            StringBuilder line1 = new StringBuilder(TLE_Information[satellite_name].Line1);
            StringBuilder line2 = new StringBuilder(TLE_Information[satellite_name].Line2);
            double[] epochTime = new double[6];
            double noradID = GetTLEData(tleClass, line1, line2, epochTime);

            string str_time = (schedule_startTime.ToUniversalTime()).ToString("yyyy-MM-dd-HH-mm-ss.fff");
            string[] split_time = str_time.Split('-');
            double[] double_time = new double[split_time.Length];
            for (int i = 0; i < double_time.Length; i++)
            {
                double_time[i] = double.Parse(split_time[i]);
            }
            double current_MJD = ConvertGreg2MJD(Global.timeSys, double_time);

            double[] siteloc = new double[3];
            GetSiteLocation(Global.laserSite, siteloc);

            double[] LLA_result = new double[3];    // 경도, 위도, 고도
            GenerateLLA_TLE(tleClass, current_MJD, LLA_result);

            double[] AZEL_result = new double[2];   // 방위각, 고각
            GenerateCommand_TLE(tleClass, siteloc, current_MJD, 0, AZEL_result);

            TLE_Information[satellite_name].Norad = (int)noradID;
            TLE_Information[satellite_name].Altitude = LLA_result[2];
            TLE_Information[satellite_name].Elevation = AZEL_result[1];    // (임시) StartTime에 대한 고각


            // 스케줄러 정보 생성
            if (TLE_Information[satellite_name].Altitude >= Filtering_SatelliteValue.Minimum_Altitude && TLE_Information[satellite_name].Altitude <= Filtering_SatelliteValue.Maximum_Altitude &&
                    TLE_Information[satellite_name].SATCAT_Data.RCS >= Filtering_SatelliteValue.Minimum_RCS && TLE_Information[satellite_name].SATCAT_Data.RCS <= Filtering_SatelliteValue.Maximum_RCS &&
                    (TLE_Information[satellite_name].SATCAT_Data.Object_Type == Filtering_SatelliteValue.Object_Type || Filtering_SatelliteValue.Object_Type == "ALL"))
            {
                // Task 생성
                MyTask myTask = new MyTask(_mManager) { Name = satellite_name };

                // Split 갯수 및 색상 설정
                int division_count = 0;
                int[] temp_duration = new int[schedule_duration];
                System.Drawing.Brush[] temp_brush = new System.Drawing.Brush[schedule_duration];

                double[][] satAZEL = Satellite_AZEL_Reader(myTask.Name, schedule_startTime.ToUniversalTime(), schedule_duration);
                bool elevation_check = false;

                // 최대고각
                double max_elevation = 0.0;

                for (int i = 0; i < schedule_duration; i++)
                {
                    // 고각이 범위안에 드는 위성인지 Checking
                    if (satAZEL[1][i] >= Filtering_SatelliteValue.Minimum_Elevation && satAZEL[1][i] <= Filtering_SatelliteValue.Maximum_Elevation)
                    {
                        elevation_check = true;
                    }

                    // 최대고각 Set
                    if (i > 0)
                    {
                        if (satAZEL[1][i] > max_elevation) { max_elevation = satAZEL[1][i]; }
                    }
                    else { max_elevation = satAZEL[1][i]; }

                    if (satAZEL[1][i] > 85.0 || satAZEL[1][i] < 20.0)
                    {
                        // 관측 불가 : 고각 제한
                        if (i > 0)
                        {
                            if (temp_brush[division_count] == total_brush[1])
                            {
                                temp_duration[division_count]++;
                            }
                            else
                            {
                                division_count++;
                                temp_duration[division_count] = 1;
                                temp_brush[division_count] = total_brush[1];
                            }
                        }
                        else if (i == 0)
                        {
                            temp_duration[division_count] = 1;
                            temp_brush[division_count] = total_brush[1];
                        }
                    }
                    else if ((check_sunAvoidance[i] == true && Filtering_SunPosition(sunAZEL[0][i], sunAZEL[1][i], satAZEL[0][i], satAZEL[1][i]) < 30.0) ||
                        (check_sunAvoidance[i] == false && Filtering_SunPosition(moonAZEL[0][i], moonAZEL[1][i], satAZEL[0][i], satAZEL[1][i]) < 30.0))
                    {
                        // 관측 불가 : 태양/달 회피
                        if (i > 0)
                        {
                            if (temp_brush[division_count] == total_brush[0])
                            {
                                temp_duration[division_count]++;
                            }
                            else
                            {
                                division_count++;
                                temp_duration[division_count] = 1;
                                temp_brush[division_count] = total_brush[0];
                            }
                        }
                        else if (i == 0)
                        {
                            temp_duration[division_count] = 1;
                            temp_brush[division_count] = total_brush[0];
                        }
                    }
                    else
                    {
                        // 관측 가능
                        if (i > 0)
                        {
                            if (temp_brush[division_count] == total_brush[2])
                            {
                                temp_duration[division_count]++;
                            }
                            else
                            {
                                division_count++;
                                temp_duration[division_count] = 1;
                                temp_brush[division_count] = total_brush[2];
                            }
                        }
                        else if (i == 0)
                        {
                            temp_duration[division_count] = 1;
                            temp_brush[division_count] = total_brush[2];
                        }
                    }

                }

                // 기존 위성정보 불러오기 & 저장
                for (int i = 0; i < myTasks.Count; i++)
                {
                    _mManager.Add(myTasks[i]);
                    _mManager.SetDuration(myTasks[i], TimeSpan.FromMinutes(schedule_duration));
                    Split_InterfaceFunction(_mManager, myTasks[i], myTasks[i].PartDuration, myTasks[i].PartColor);
                    if (myTasks.Count > 1000) { _mManager.Delete(myTasks[i]); } // 스케줄 Page 분할
                }

                if (elevation_check == true)
                {
                    // Task Duration/Brush Setting
                    int[] task_duration = new int[division_count + 1];
                    System.Drawing.Brush[] task_brush = new System.Drawing.Brush[division_count + 1];
                    Array.Copy(temp_duration, task_duration, task_duration.Length);
                    Array.Copy(temp_brush, task_brush, task_brush.Length);
                    Split_InterfaceFunction(_mManager, myTask, task_duration, task_brush);

                    // Task 추가
                    myTasks.Add(myTask);
                    _mManager.Add(myTask);
                    _mManager.SetDuration(myTask, TimeSpan.FromMinutes(schedule_duration));
                    if (myTasks.Count > 1000) { _mManager.Delete(myTask); } // 스케줄 Page 분할

                    // DateGridView 정보 생성
                    TaskGridView_Info taskGridView_info = new TaskGridView_Info(satellite_name);
                    taskGridView_info.NoradID = TLE_Information[satellite_name].Norad;
                    taskGridView_info.Altitude = Math.Round(TLE_Information[satellite_name].Altitude, 5);
                    taskGridView_info.MaxElevation = Math.Round(max_elevation, 5)/*Math.Round(tle_information.Value.Elevation, 5)*/;
                    taskGridView_info.RCS = Math.Round(TLE_Information[satellite_name].SATCAT_Data.RCS, 5);
                    taskGridView_info.Type = TLE_Information[satellite_name].SATCAT_Data.Object_Type;
                    taskGridView_infos.Add(taskGridView_info);
                }
                else
                {
                    // 필터링고각 요건충족 X
                }

            }


            // Gantt Chart 생성 및 DataGridView 전시
            _mManager.Start = schedule_startTime;
            _mChart.Init(_mManager);
            _mChart.Invalidate();

            _mChart.CreateTaskDelegate = delegate () { return new MyTask(_mManager); };
            _mChart.AllowTaskDragDrop = false;

            _mChart.TimeResolution = TimeResolution.Minute;

            // DateGridView 정보 전시
            if (taskGridView_infos.Count <= 1000)
            {
                taskGridView.DataSource = taskGridView_infos;
            }
            else
            {
                taskGridView.DataSource = taskGridView_infos.GetRange(0, 1000);  // 스케줄 Page 분할
            }

            taskGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.ClearSelection();

            // 페이지 전시
            if (taskGridView_infos.Count != 0 && taskGridView_infos.Count % 1000 == 0) { total_page = taskGridView_infos.Count / 1000; }
            else { total_page = (taskGridView_infos.Count / 1000) + 1; }
            current_page = 1;

            currentPage_txtbox.Text = String.Format("{0} / {1}", current_page, total_page);

        }

        private void beforePage_btn_Click(object sender, EventArgs e)   // 이전 페이지
        {
            if (current_page > 1)
            {
                // 페이지 전시
                current_page--;
                currentPage_txtbox.Text = String.Format("{0} / {1}", current_page, total_page);

                // DateGridView 정보 전시
                if (taskGridView.DataSource != null)
                {
                    taskGridView.Columns.Clear();
                    taskGridView.DataSource = null;
                }

                taskGridView.DataSource = taskGridView_infos.GetRange((current_page - 1) * 1000, 1000);
                taskGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                taskGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                taskGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                taskGridView.ClearSelection();

                // GanttChart 전시
                if ((current_page + 1 == total_page) && (taskGridView_infos.Count % 1000 != 0))
                {
                    for (int i = current_page * 1000; i < taskGridView_infos.Count; i++)
                    {
                        _mManager.Delete(myTasks[i]);
                    }
                }
                else
                {
                    for (int i = current_page * 1000; i < (current_page + 1) * 1000; i++)
                    {
                        _mManager.Delete(myTasks[i]);
                    }
                }
                
                for (int i = (current_page - 1) * 1000; i < current_page * 1000; i++)
                {
                    _mManager.Add(myTasks[i]);
                    Split_InterfaceFunction(_mManager, myTasks[i], myTasks[i].PartDuration, myTasks[i].PartColor);
                }

                _mChart.Init(_mManager);
                _mChart.Invalidate();

                // 위성/우주물체 정보 초기화
                Initialize_Satellite_Information();

                // GroundTrack 및 SkyView 초기화
                GroundTrack.Image = null;
                GroundTrack.Update();
                ChartPointClear();

                // Reference Data 관련요소 초기화
                Initialize_Reference();
            }

            

            
        }

        private void searchSatellite_btn_Click(object sender, EventArgs e)  // 검색(위성 선택) Click 시
        {
            if (String.IsNullOrEmpty(searchSatellite_txt.Text))
            {
                MessageBox.Show("위성 이름을 입력하세요.");
                return;
            }

            // 검색 textbox 와 Norad ID 비교용
            int tempNorad_int = 0;
            bool tempNorad_bool = int.TryParse(searchSatellite_txt.Text, out tempNorad_int);

            int i;
            for (i = 0; i < myTasks.Count; i++)
            {
                if ( (myTasks[i].Name == searchSatellite_txt.Text) ||
                    (tempNorad_bool == true && TLE_Information[myTasks[i].Name].Norad == tempNorad_int) )
                {
                    // DataGridView 및 Gantt Chart 페이지 변경
                    int searching_page = (i / 1000) + 1;
                    if (searching_page != current_page)
                    {
                        // DateGridView 정보 전시
                        if (taskGridView.DataSource != null)
                        {
                            taskGridView.Columns.Clear();
                            taskGridView.DataSource = null;
                        }

                        if (searching_page == total_page && (taskGridView_infos.Count % 1000 != 0))
                        {
                            taskGridView.DataSource = taskGridView_infos.GetRange((searching_page - 1) * 1000, taskGridView_infos.Count % 1000);
                        }
                        else
                        {
                            taskGridView.DataSource = taskGridView_infos.GetRange((searching_page - 1) * 1000, 1000);
                        }
                        taskGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        taskGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        taskGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        taskGridView.ClearSelection();

                        // GanttChart 전시
                        if ((current_page == total_page) && (taskGridView_infos.Count % 1000 != 0))
                        {
                            for (int j = (current_page - 1) * 1000; j < taskGridView_infos.Count; j++) { _mManager.Delete(myTasks[j]); }
                        }
                        else
                        {
                            for (int j = (current_page - 1) * 1000; j < current_page * 1000; j++) { _mManager.Delete(myTasks[j]); }
                        }

                        if ((searching_page == total_page) && (taskGridView_infos.Count % 1000 != 0))
                        {
                            for (int j = (searching_page - 1) * 1000; j < taskGridView_infos.Count; j++)
                            {
                                _mManager.Add(myTasks[j]);
                                Split_InterfaceFunction(_mManager, myTasks[j], myTasks[j].PartDuration, myTasks[j].PartColor);
                            }
                        }
                        else
                        {
                            for (int j = (searching_page - 1) * 1000; j < searching_page * 1000; j++)
                            {
                                _mManager.Add(myTasks[j]);
                                Split_InterfaceFunction(_mManager, myTasks[j], myTasks[j].PartDuration, myTasks[j].PartColor);
                            }
                        }

                        _mChart.Init(_mManager);
                        _mChart.Invalidate();

                        // 페이지 전시
                        current_page = searching_page;
                        currentPage_txtbox.Text = String.Format("{0} / {1}", current_page, total_page);
                    }

                    // DataGridView 에서 해당 위성 선택
                    taskGridView.Rows[i % 1000].Cells[0].Selected = true; // 수정

                    // CPFGridView 선택 해제
                    CPFGridView.ClearSelection();

                    // GanttChart 에서 해당 위성 선택
                    tabControl1.SelectedTab = tlePage;
                    _mChart.ScrollTo(myTasks[i]);

                    // Ground Track 및 Sky View 전시
                    TaskGridView_CellClick_Event(i);

                    // 초기화(Reference Data용) + selectedSatellite_label(Reference Data용) Set
                    Initialize_Reference();
                    selectedSatellite_label.Text = myTasks[i].Name;

                    break;
                }
            }

            if (i == myTasks.Count)
            {
                MessageBox.Show("해당 위성을 찾지 못했습니다.");
            }

        }

        private void nextPage_btn_Click(object sender, EventArgs e) // 다음 페이지
        {
            if ( ((myTasks.Count % 1000) == 0 && (current_page < myTasks.Count / 1000)) || 
                ((myTasks.Count % 1000) != 0) && (current_page < (myTasks.Count / 1000) + 1))
            {
                // 페이지 전시
                current_page++;
                currentPage_txtbox.Text = String.Format("{0} / {1}", current_page, total_page);

                // DateGridView 정보 전시
                if (taskGridView.DataSource != null)
                {
                    taskGridView.Columns.Clear();
                    taskGridView.DataSource = null;
                }

                if (current_page == total_page && (taskGridView_infos.Count % 1000 != 0))
                {
                    taskGridView.DataSource = taskGridView_infos.GetRange((current_page - 1) * 1000, taskGridView_infos.Count % 1000);
                }
                else
                {
                    taskGridView.DataSource = taskGridView_infos.GetRange((current_page - 1) * 1000, 1000);
                }
                taskGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                taskGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                taskGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                taskGridView.ClearSelection();

                // Gantt Chart 전시
                for (int i = (current_page - 2) * 1000; i < (current_page - 1) * 1000; i++)
                {
                    _mManager.Delete(myTasks[i]);
                }
                
                if ((current_page == total_page) && (taskGridView_infos.Count % 1000 != 0))
                {
                    for (int i = (current_page - 1) * 1000; i < taskGridView_infos.Count; i++)
                    {
                        _mManager.Add(myTasks[i]);
                        Split_InterfaceFunction(_mManager, myTasks[i], myTasks[i].PartDuration, myTasks[i].PartColor);
                    }
                }
                else
                {
                    for (int i = (current_page - 1) * 1000; i < current_page * 1000; i++)
                    {
                        _mManager.Add(myTasks[i]);
                        Split_InterfaceFunction(_mManager, myTasks[i], myTasks[i].PartDuration, myTasks[i].PartColor);
                    }
                }

                _mChart.Init(_mManager);
                _mChart.Invalidate();

                // 위성/우주물체 정보 초기화
                Initialize_Satellite_Information();

                // GroundTrack 및 SkyView 초기화
                GroundTrack.Image = null;
                GroundTrack.Update();
                ChartPointClear();

                // Reference Data 관련요소 초기화
                Initialize_Reference();
            }


        }

        public void Split_InterfaceFunction(ProjectManager projectManager, MyTask task, int[] division_duration, System.Drawing.Brush[] division_brush)
        {
            // 하위 Task 생성
            Braincase.GanttChart.Task[] tasks = new Braincase.GanttChart.Task[division_duration.Length];
            for (int i = 0; i < division_duration.Length; i++)
            {
                tasks[i] = new Braincase.GanttChart.Task();
            }

            // Task에 적용할 division_duration 할당
            task.PartDuration = new int[division_duration.Length];
            Array.Copy(division_duration, task.PartDuration, division_duration.Length);

            // Task에 적용할 division_brush 할당
            task.PartColor = new System.Drawing.Brush[division_brush.Length];
            Array.Copy(division_brush, task.PartColor, division_brush.Length);

            // Split 작업 수행
            projectManager.Modified_Split(task/*상위 Task*/, tasks/*하위 Task*/);
        }

        public void Display_GanttChart()    // 각 위성의 스케줄링을 간트차트로 전시
        {
            // Gantt Chart 전시
            _mManager.Start = schedule_startTime;
            _mChart.Init(_mManager);
            _mChart.Invalidate();

            _mChart.CreateTaskDelegate = delegate () { return new MyTask(_mManager); };
            _mChart.AllowTaskDragDrop = false;

            _mChart.TimeResolution = TimeResolution.Minute;

            // DateGridView 정보 전시
            if (taskGridView_infos.Count <= 1000)
            {
                taskGridView.DataSource = taskGridView_infos;
            }
            else
            {
                taskGridView.DataSource = taskGridView_infos.GetRange(0, 1000);  // 스케줄 Page 분할
            }

            taskGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.ClearSelection();

            // 페이지 전시
            if (taskGridView_infos.Count != 0 && taskGridView_infos.Count % 1000 == 0) { total_page = taskGridView_infos.Count / 1000; }
            else { total_page = (taskGridView_infos.Count / 1000) + 1; }
            current_page = 1;

            currentPage_txtbox.Text = String.Format("{0} / {1}", current_page, total_page);
        }

        


        private void TaskGridView_CellClick(object sender, DataGridViewCellEventArgs e) // 위성 선택 시 >> Ground Track / Sky View 전시
        {

            // 해당 스케줄링 차트 위치로 이동
            if (e.RowIndex >= 0 && e.RowIndex < taskGridView.Rows.Count)
            {
                int task_index = ((current_page - 1) * 1000) + e.RowIndex;

                // CPFGridView 선택 해제
                CPFGridView.ClearSelection();
                ephemerisData_dataGridView.ClearSelection();

                // GanttChart 에서 해당위성 선택
                tabControl1.SelectedTab = tlePage;
                _mChart.ScrollTo(myTasks[task_index]);

                // Ground Track 및 Sky View 전시
                TaskGridView_CellClick_Event(task_index);

                // 초기화(Reference Data용) + selectedSatellite_label(Reference Data용) Set
                Initialize_Reference();
                selectedSatellite_label.Text = myTasks[task_index].Name;

            }


        }

        Point[] satellite_point1;
        Point[] satellite_point2;

        List<Point[]> total_points = new List<Point[]>();
        List<System.Drawing.Color> total_color = new List<System.Drawing.Color>();



        public void TaskGridView_CellClick_Event(int dataGridView_rowIndex)
        {

            // 위성/우주물체 정보 초기화
            Initialize_Satellite_Information();

            // GroundTrack 및 SkyView 초기화
            GroundTrack.Image = null;
            GroundTrack.Update();
            ChartPointClear();


            double[][] azelDatas = new double[2][];

            for (int i = 0; i < azelDatas.Length; i++) { azelDatas[i] = new double[schedule_duration]; }

            // TLE 추출
            StringBuilder line1 = new StringBuilder(TLE_Information[myTasks[dataGridView_rowIndex].Name].Line1);
            StringBuilder line2 = new StringBuilder(TLE_Information[myTasks[dataGridView_rowIndex].Name].Line2);

            // TLE 정보 입력 맟 위성에 대한 Epoch Time 산출
            double[] epochTime = new double[6];
            GetTLEData(tleClass, line1, line2, epochTime);


            // Site Location 설정
            double[] siteloc = new double[3];
            GetSiteLocation(Global.laserSite, siteloc);

            // 위/경도 데이터 산출
            double timeMJD;
            double[] result = new double[3];
            System.Drawing.Color current_color = System.Drawing.Color.White;
            int graphics_count = 0;
            int stackData_count = 0; int subData_count = 0;

            // points/colors List 초기화
            total_points.Clear();
            total_color.Clear();

            // local > UTC
            DateTime schedule_startTime_UTC = schedule_startTime.ToUniversalTime();

            for (int i = 0; i < azelDatas[0].Length; i++)
            {

                string str_time = (schedule_startTime_UTC.AddMinutes(i)).ToString("yyyy-MM-dd-HH-mm-ss.fff");
                string[] split_time = str_time.Split('-');
                double[] double_time = new double[split_time.Length];
                for (int j = 0; j < double_time.Length; j++)
                {
                    double_time[j] = double.Parse(split_time[j]);
                }

                timeMJD = ConvertGreg2MJD(Global.timeSys, double_time);
                GenerateLLA_TLE(tleClass, timeMJD, result);

                azelDatas[0][i] = result[0];    // 경도
                azelDatas[1][i] = result[1];    // 위도
                stackData_count++;

                // 확보가능 위치 데이터는 1440 / 궤도 전시 알고리즘 구상
                if (i > 0)
                {
                    // 위도 90도 or -90도 지점에서 경도 데이터가 크게 변동하는 시점 고려 ////////////////////
                    if (i > 1)
                    {
                        if ((azelDatas[1][i - 2] < azelDatas[1][i - 1] && azelDatas[1][i - 1] > azelDatas[1][i] && Math.Abs(azelDatas[0][i - 1] - azelDatas[0][i]) > 20) ||
                            (azelDatas[1][i - 2] > azelDatas[1][i - 1] && azelDatas[1][i-1] < azelDatas[1][i] && Math.Abs(azelDatas[0][i - 1] - azelDatas[0][i]) > 20))
                        {
                            //MessageBox.Show((i-1).ToString());
                            /*
                            if (myTasks[dataGridView_rowIndex].PartColor[graphics_count] == total_brush[0]) { current_color = System.Drawing.Color.Red; }
                            else if (myTasks[dataGridView_rowIndex].PartColor[graphics_count] == total_brush[1]) { current_color = System.Drawing.Color.White; }
                            else if (myTasks[dataGridView_rowIndex].PartColor[graphics_count] == total_brush[2]) { current_color = System.Drawing.Color.Yellow; }

                            // Ground Track 전시 데이터 Set
                            if ((stackData_count - subData_count - 1) > 1)
                            {
                                /////////////////////// 레이아웃 변환 시 공백 채우기용 /////////////////////
                                if (subData_count == 0)
                                {
                                    // 끝 부분만
                                    Point[] points = new Point[stackData_count - subData_count - 1 + 1];

                                    for (int j = 0; j < points.Length - 1; j++)
                                    {
                                        points[j] = MercatorProjection(azelDatas[0][(i + 1) - points.Length + j], azelDatas[1][(i + 1) - points.Length + j]);
                                    }

                                    if (azelDatas[1][i - 1] < 90.0 && azelDatas[1][i - 1] > 0.0)
                                    {
                                        points[points.Length - 1] = MercatorProjection(azelDatas[1][i - 1], 89.9);
                                    }
                                    else // (azelDatas[0][i - 1] > -90 && azelDatas[0][i - 1] < 0)
                                    {
                                        points[points.Length - 1] = MercatorProjection(azelDatas[1][i - 1], -89.9);
                                    }
                                    total_points.Add(points);
                                }
                                else
                                {
                                    // 시작 + 끝 부분
                                    Point[] points = new Point[1 + stackData_count - subData_count - 1 + 1];

                                    for (int j = 1; j < points.Length - 1; j++)
                                    {
                                        points[j] = MercatorProjection(azelDatas[0][(i + 1 + 1) - points.Length + (j - 1)], azelDatas[1][(i + 1 + 1) - points.Length + (j - 1)]);
                                    }

                                    if (azelDatas[1][(i + 1 + 1) - points.Length] < 90.0 && azelDatas[1][(i + 1 + 1) - points.Length] > 0.0)
                                    {
                                        points[0] = MercatorProjection(azelDatas[0][(i + 1 + 1) - points.Length], 89.9);
                                    }
                                    else
                                    {
                                        points[0] = MercatorProjection(azelDatas[0][(i + 1 + 1) - points.Length], -89.9);
                                    }
                                    if (azelDatas[1][i - 1] < 90.0 && azelDatas[1][i - 1] > 0.0)
                                    {
                                        points[points.Length - 1] = MercatorProjection(azelDatas[0][i - 1], 89.9);
                                    }
                                    else
                                    {
                                        points[points.Length - 1] = MercatorProjection(azelDatas[0][i - 1], -89.9);
                                    }
                                    total_points.Add(points);
                                }

                                System.Drawing.Color color = current_color;
                                total_color.Add(color);

                            }

                            // subData 설정
                            subData_count = stackData_count - 1;
                            */
                        }

                    }
                    ////////////////////////////////////////////////////////////////////////////////////////
                    if (((azelDatas[0][i] < 220.0 && azelDatas[0][i] >= 180.0) && (azelDatas[0][i - 1] > 140.0 && azelDatas[0][i - 1] < 180.0)) ||
                            ((azelDatas[0][i] > 140.0 && azelDatas[0][i] < 180.0) && (azelDatas[0][i - 1] >= 180.0 && azelDatas[0][i - 1] < 220.0)))
                    {
                        if (myTasks[dataGridView_rowIndex].PartColor[graphics_count] == total_brush[0]) { current_color = System.Drawing.Color.Red; }
                        else if (myTasks[dataGridView_rowIndex].PartColor[graphics_count] == total_brush[1]) { current_color = System.Drawing.Color.White/*Color.Black*/; }
                        else if (myTasks[dataGridView_rowIndex].PartColor[graphics_count] == total_brush[2]) { current_color = System.Drawing.Color.Yellow; }

                        // Ground Track 전시 데이터 Set
                        if ((stackData_count - subData_count - 1) > 1)
                        {
                            /////////////////////// 레이아웃 변환 시 공백 채우기용 /////////////////////
                            if (subData_count == 0)
                            {
                                // 끝 부분만
                                Point[] points = new Point[stackData_count - subData_count - 1 + 1];

                                for (int j = 0; j < points.Length - 1; j++)
                                {
                                    points[j] = MercatorProjection(azelDatas[0][(i + 1) - points.Length + j], azelDatas[1][(i + 1) - points.Length + j]);
                                }

                                if (azelDatas[0][i - 1] < 180)
                                {
                                    points[points.Length - 1] = MercatorProjection(179.9, azelDatas[1][i - 1]);
                                }
                                else // (azelDatas[0][i - 1] >= 180)
                                {
                                    points[points.Length - 1] = MercatorProjection(180.0, azelDatas[1][i - 1]);
                                }
                                total_points.Add(points);
                            }
                            else
                            {
                                // 시작 + 끝 부분
                                Point[] points = new Point[1 + stackData_count - subData_count - 1 + 1];

                                for (int j = 1; j < points.Length - 1; j++)
                                {
                                    points[j] = MercatorProjection(azelDatas[0][(i + 1 + 1) - points.Length + (j - 1)], azelDatas[1][(i + 1 + 1) - points.Length + (j - 1)]);
                                }

                                if (azelDatas[0][(i + 1 + 1) - points.Length] < 180)
                                {
                                    points[0] = MercatorProjection(179.9, azelDatas[1][(i + 1 + 1) - points.Length]);
                                }
                                else
                                {
                                    points[0] = MercatorProjection(180.0, azelDatas[1][(i + 1 + 1) - points.Length]);
                                }
                                if (azelDatas[0][i - 1] < 180)
                                {
                                    points[points.Length - 1] = MercatorProjection(179.9, azelDatas[1][i - 1]);
                                }
                                else
                                {
                                    points[points.Length - 1] = MercatorProjection(180.0, azelDatas[1][i - 1]);
                                }
                                total_points.Add(points);
                            }

                            System.Drawing.Color color = current_color;
                            total_color.Add(color);

                        }

                        // subData 설정
                        subData_count = stackData_count - 1;
                    }
                }

                // 궤도 색상 구분 구현
                if (/*((i + 1) % 60 == 0) && */(i != 0))
                {
                    // 색상 변경여부 Checking
                    if ((stackData_count/* / 60*/) == myTasks[dataGridView_rowIndex].PartDuration[graphics_count])
                    {
                        if (myTasks[dataGridView_rowIndex].PartColor[graphics_count] == total_brush[0]) { current_color = System.Drawing.Color.Red; }
                        else if (myTasks[dataGridView_rowIndex].PartColor[graphics_count] == total_brush[1]) { current_color = System.Drawing.Color.White/*Color.Black*/; }
                        else if (myTasks[dataGridView_rowIndex].PartColor[graphics_count] == total_brush[2]) { current_color = System.Drawing.Color.Yellow; }

                        // Ground Track 전시 데이터 Set
                        if ((stackData_count - subData_count) > 1)
                        {
                            //////////////// 그래프 사이 간격 채우기용 ///////////////////
                            str_time = (schedule_startTime_UTC.AddMinutes(i + 1)).ToString("yyyy-MM-dd-HH-mm-ss.fff");
                            split_time = str_time.Split('-');
                            for (int j = 0; j < double_time.Length; j++)
                            {
                                double_time[j] = double.Parse(split_time[j]);
                            }

                            timeMJD = ConvertGreg2MJD(Global.timeSys, double_time);
                            GenerateLLA_TLE(tleClass, timeMJD, result);

                            /////////////////////// 레이아웃 변환 시 공백 채우기용 /////////////////////
                            if (subData_count == 0)
                            {


                                if (((result[0] < 220.0 && result[0] >= 180.0) && (azelDatas[0][i] > 140.0 && azelDatas[0][i] < 180.0)) ||
                                    ((result[0] > 140.0 && result[0] < 180.0) && (azelDatas[0][i] >= 180.0 && azelDatas[0][i] < 220.0)))
                                {
                                    Point[] points = new Point[stackData_count - subData_count];

                                    for (int j = 0; j < points.Length; j++)
                                    {
                                        points[j] = MercatorProjection(azelDatas[0][(i + 1) - points.Length + j], azelDatas[1][(i + 1) - points.Length + j]);
                                    }
                                    total_points.Add(points);
                                }
                                else
                                {
                                    Point[] points = new Point[stackData_count - subData_count + 1];
                                    for (int j = 0; j < points.Length - 1; j++)
                                    {
                                        points[j] = MercatorProjection(azelDatas[0][(i + 1 + 1) - points.Length + j], azelDatas[1][(i + 1 + 1) - points.Length + j]);
                                    }

                                    points[points.Length - 1] = MercatorProjection(result[0], result[1]);
                                    total_points.Add(points);
                                }
                            }
                            else
                            {
                                // 시작 부분 추가
                                if (((result[0] < 220.0 && result[0] >= 180.0) && (azelDatas[0][i] > 140.0 && azelDatas[0][i] < 180.0)) ||
                                    ((result[0] > 140.0 && result[0] < 180.0) && (azelDatas[0][i] >= 180.0 && azelDatas[0][i] < 220.0)))
                                {
                                    Point[] points = new Point[1 + stackData_count - subData_count];

                                    for (int j = 1; j < points.Length; j++)
                                    {
                                        points[j] = MercatorProjection(azelDatas[0][(i + 1 + 1) - points.Length + (j - 1)], azelDatas[1][(i + 1 + 1) - points.Length + (j - 1)]);
                                    }

                                    if (azelDatas[0][(i + 1 + 1) - points.Length] < 180)
                                    {
                                        points[0] = MercatorProjection(179.9, azelDatas[1][(i + 1 + 1) - points.Length]);
                                    }
                                    else
                                    {
                                        points[0] = MercatorProjection(180.0, azelDatas[1][(i + 1 + 1) - points.Length]);
                                    }
                                    total_points.Add(points);
                                }
                                else
                                {
                                    Point[] points = new Point[1 + stackData_count - subData_count + 1];

                                    for (int j = 1; j < points.Length - 1; j++)
                                    {
                                        points[j] = MercatorProjection(azelDatas[0][(i + 1 + 1 + 1) - points.Length + (j - 1)], azelDatas[1][(i + 1 + 1 + 1) - points.Length + (j - 1)]);
                                    }

                                    if (azelDatas[0][(i + 1 + 1 + 1) - points.Length] < 180)
                                    {
                                        points[0] = MercatorProjection(179.9, azelDatas[1][(i + 1 + 1 + 1) - points.Length]);
                                    }
                                    else
                                    {
                                        points[0] = MercatorProjection(180.0, azelDatas[1][(i + 1 + 1 + 1) - points.Length]);
                                    }
                                    points[points.Length - 1] = MercatorProjection(result[0], result[1]);
                                    total_points.Add(points);
                                }

                            }

                            System.Drawing.Color color = current_color;
                            total_color.Add(color);

                        }

                        // graphics 카운팅 + stackData 갯수 초기화
                        graphics_count++;
                        stackData_count = 0;
                        if (subData_count != 0) { subData_count = 0; }

                    }
                }


            }

            // Ground Track 전시
            for (int i = 0; i < total_points.Count; i++)
            {
                Graphics graphics = GroundTrack.CreateGraphics();
                System.Drawing.Pen pen = new System.Drawing.Pen(total_color[i], 4f);
                graphics.DrawCurve(pen, total_points[i]);
            }

            // SkyView 전시

            int sky_count = 0;
            for (int i = 0; i < myTasks[dataGridView_rowIndex].PartDuration.Length; i++)
            {
                double[] sky_longitude = new double[myTasks[dataGridView_rowIndex].PartDuration[i]];
                double[] sky_latitude = new double[myTasks[dataGridView_rowIndex].PartDuration[i]];
                Array.Copy(azelDatas[0], sky_count, sky_longitude, 0, sky_longitude.Length);
                Array.Copy(azelDatas[1], sky_count, sky_latitude, 0, sky_latitude.Length);

                string skyView_color = "";
                if (myTasks[dataGridView_rowIndex].PartColor[i] == total_brush[0]) { skyView_color = "red"; }
                else if (myTasks[dataGridView_rowIndex].PartColor[i] == total_brush[1]) { skyView_color = "black"; }
                else if (myTasks[dataGridView_rowIndex].PartColor[i] == total_brush[2]) { skyView_color = "yellow"; }

                bool startOrbit = false; bool endOrbit = false;

                DrawDataOnPolarGragh(sky_longitude, sky_latitude, skyView_color, startOrbit, endOrbit);
                sky_count = sky_count + myTasks[dataGridView_rowIndex].PartDuration[i];

                if (i == 0)
                {
                    chart1.Series["white"].Points.AddXY(azelDatas[0][0], azelDatas[1][0]);
                    chart1.Series["white"].Points[0].Label = "Start";
                }
                if (i == myTasks[dataGridView_rowIndex].PartDuration.Length - 1)
                {
                    chart1.Series["white"].Points.AddXY(azelDatas[0][schedule_duration - 1], azelDatas[1][schedule_duration - 1]);
                    chart1.Series["white"].Points[1].Label = "End";
                }



            }

            string avoid_text = "-";
            int count = 0;
            for(int i = 0; i < myTasks[dataGridView_rowIndex].PartColor.Length; i++)
            {
                if (myTasks[dataGridView_rowIndex].PartColor[i] == total_brush[0])
                {
                    // 태양/달 회피구간 있음
                    avoid_text = schedule_startTime.AddMinutes(count).ToString("HH:mm") + " ~ " + schedule_startTime.AddMinutes(count + myTasks[dataGridView_rowIndex].PartDuration[i]).ToString("HH:mm");
                    break;
                }
                count = count + myTasks[dataGridView_rowIndex].PartDuration[i];
            }

            // 위성/우주물체 정보 전시
            Display_Satellite_Information(myTasks[dataGridView_rowIndex].Name, avoid_text);
            select_TLE_CPF = "tle";
        }


        public static double Altitude_RGG = 0.0;    // RGG 참고용 고도정보 저장
        Thread display_satInformation;

        public void Display_Satellite_Information(string satellite, string data9_text) // 위성/우주물체 정보 전시
        {

            // 1초 간격 데이터 업데이트
            if (display_satInformation != null)
            {
                if (display_satInformation.IsAlive == true)
                {
                    display_satInformation.Abort();
                }
                display_satInformation = null;
            }

            display_satInformation = new Thread(() =>
            {
                StringBuilder line1 = new StringBuilder(TLE_Information[satellite].Line1);
                StringBuilder line2 = new StringBuilder(TLE_Information[satellite].Line2);
                double[] epochTime = new double[6];
                GetTLEData(tleClass, line1, line2, epochTime);

                double[] siteloc = new double[3];
                GetSiteLocation(Global.laserSite, siteloc);

                this.Invoke(new System.Action(() =>
                {
                    information_data1.Text = satellite;
                    information_data2.Text = "NSLR-OAS";
                    information_data6.Text = (sunRiseSet_time[0].ToLocalTime()).ToString("yyyy-MM-dd HH:mm");
                    information_data7.Text = (sunRiseSet_time[1].ToLocalTime()).ToString("yyyy-MM-dd HH:mm");
                    //information_data8.Text = "-";
                    information_data9.Text = data9_text;
                    information_data10.Text = String.Format("{0:0.00000}", taskGridView_infos[(current_page - 1) * 1000 + taskGridView.SelectedRows[0].Index].MaxElevation) + "°";
                }));

                int thread_count = 0;
                while (true)
                {
                    DateTime UTCNow_time = DateTime.UtcNow;
                    string str_time = UTCNow_time.ToString("yyyy-MM-dd-HH-mm-ss.ffffff");
                    string[] split_time = str_time.Split('-');
                    double[] double_time = new double[split_time.Length];
                    for (int i = 0; i < double_time.Length; i++)
                    {
                        double_time[i] = double.Parse(split_time[i]);
                    }
                    double current_MJD = ConvertGreg2MJD(Global.timeSys, double_time);

                    double[] LLA_result = new double[3];    // 경도, 위도, 고도
                    GenerateLLA_TLE(tleClass, current_MJD, LLA_result);

                    double[] AZEL_result = new double[2];   // 방위각, 고각
                    GenerateCommand_TLE(tleClass, siteloc, current_MJD, 0, AZEL_result);

                    Altitude_RGG = LLA_result[2];   // RGG 참고용 고도정보 저장
                    this.Invoke(new System.Action(() =>
                    {
                        information_data3.Text = String.Format("{0:0.00}", LLA_result[2]) + " km";
                        information_data4.Text = String.Format("{0:0.00000}", AZEL_result[0]) + "°";
                        information_data5.Text = String.Format("{0:0.00000}", AZEL_result[1]) + "°";
                        //information_data10.Text = String.Format("{0:0.00000}", AZEL_result[1]) + "°";
                    }));

                    // Grount Track 실시간위성 표시 (5초 주기)
                    if (thread_count % 5 == 0)
                    {

                        this.Invoke(new System.Action(() =>
                        {
                            // Ground Track 전시
                            GroundTrack.Image = null;
                            GroundTrack.Update();

                            for (int i = 0; i < total_points.Count; i++)
                            {
                                Graphics graphic = GroundTrack.CreateGraphics();
                                System.Drawing.Pen pen = new System.Drawing.Pen(total_color[i], 4f);
                                graphic.DrawCurve(pen, total_points[i]);
                            }

                            Graphics position_graphic = GroundTrack.CreateGraphics();
                            Bitmap satelliteIcon = new Bitmap(NSLR_ObservationControl.Properties.Resources.satOrbit1);
                            Point LLA_point = MercatorProjection(LLA_result[0], LLA_result[1]);
                            position_graphic.DrawImage(satelliteIcon, LLA_point.X - 25, LLA_point.Y - 25, 50, 50);

                        }));


                    }


                    thread_count++;
                    Thread.Sleep(1000);
                }

            });
            display_satInformation.Start();

        }

        public void Initialize_Satellite_Information()  // 위성/우주물체 정보 초기화
        {
            // 기존 display_satInformation 쓰레드 정지
            if (display_satInformation != null)
            {
                if (display_satInformation.IsAlive == true)
                {
                    display_satInformation.Abort();
                }
                display_satInformation = null;
            }

            information_data1.Text = "-";
            information_data2.Text = "-";
            information_data3.Text = "-";
            information_data4.Text = "-";
            information_data5.Text = "-";
            information_data6.Text = "-";
            information_data7.Text = "-";
            information_data8.Text = "-";
            information_data9.Text = "-";
            information_data10.Text = "-";
        }

        public void Initialize_CPF_Satellite_Information()
        {
            information_data1.Text = "-";
            information_data2.Text = "-";
            information_data3.Text = "-";
            information_data4.Text = "-";
            information_data5.Text = "-";
            information_data6.Text = "-";
            information_data7.Text = "-";
            information_data8.Text = "-";
            information_data9.Text = "-";
            information_data10.Text = "-";
        }

        private Point MercatorProjection(double longitude, double latitude) // GroundTrack : 궤도 그리기 함수
        {
            const double longitude2map = 360;
            const double latitude2map = 180;

            double x;
            if (longitude < 180)
            {
                x = (180 + longitude) * (GroundTrack.Width / longitude2map);
            }
            else
            {
                x = (-180 + longitude) * (GroundTrack.Width / longitude2map);
            }

            double y = GroundTrack.Height - ((latitude + 90) * (GroundTrack.Height / latitude2map));

            return new Point((int)x, (int)y);
        }

        public double[][] Drawing_Orbit(string satellite)
        {
            double[][] orbit_result = new double[2][];

            if (TLE_Information.ContainsKey(satellite) == false)
            {
                MessageBox.Show("해당 위성에 대한 TLE 정보가 없습니다.");
                return orbit_result;
            }

            for (int i = 0; i < orbit_result.Length; i++) { orbit_result[i] = new double[/*1440*/1440]; };

            // 해당 위성 TLE Searching
            StringBuilder line1 = new StringBuilder(TLE_Information[satellite].Line1);
            StringBuilder line2 = new StringBuilder(TLE_Information[satellite].Line2);

            double[] epoch_time = new double[6];
            GetTLEData(tleClass, line1, line2, epoch_time);

            //MessageBox.Show(epoch_time[0].ToString() + " / " + epoch_time[1].ToString() + " / " + epoch_time[2].ToString() + " / " + epoch_time[3] + " / " + epoch_time[4].ToString() + " / " + epoch_time[5].ToString());

            double[] siteloc = new double[3];
            GetSiteLocation(Global.laserSite, siteloc);

            double timeMJD;
            double[] result = new double[2];
            epoch_time[5] = 0.000;
            DateTime dateTime = new DateTime((int)epoch_time[0], (int)epoch_time[1], (int)epoch_time[2], (int)epoch_time[3], (int)epoch_time[4], (int)epoch_time[5]);
            for (int i = 0; i < orbit_result[0].Length; i++)
            {
                string str_time = (dateTime.AddMinutes(i)).ToString("yyyy-MM-dd-HH-mm-ss");

                string[] split_time = str_time.Split('-');

                double[] current_time = new double[6];
                for (int j = 0; j < current_time.Length; j++)
                {
                    current_time[j] = double.Parse(split_time[j]);
                }
                /*
                MessageBox.Show(current_time[0].ToString() + "/" + current_time[1].ToString() + "/" + current_time[2].ToString() + "/" + current_time[3].ToString() + 
                    "/" + current_time[4].ToString() + "/" + current_time[5].ToString());
                */
                timeMJD = ConvertGreg2MJD(Global.timeSys, current_time);
                GenerateCommand_TLE(tleClass, siteloc, timeMJD, 0, result);

                orbit_result[0][i] = result[0];
                orbit_result[1][i] = result[1];
            }

            return orbit_result;

        }


        private void ChartInit()
        {
            chart1.Series["yellow"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            chart1.Series["yellow"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            chart1.Series["yellow"].MarkerSize = 8;
            chart1.Series["yellow"].Color = System.Drawing.Color.Yellow;
            //chart1.Series["Series1"].LabelBackColor = Color.Green;
            chart1.Series["yellow"]["PolarDrawingStyle"] = "Marker";
            chart1.Series["yellow"]["AreaDrawingStyle"] = "Circle";

            chart1.Series["red"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            chart1.Series["red"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            chart1.Series["red"].MarkerSize = 8;
            chart1.Series["red"].Color = System.Drawing.Color.Red;
            //chart1.Series["Series1"].LabelBackColor = Color.Green;
            chart1.Series["red"]["PolarDrawingStyle"] = "Marker";
            chart1.Series["red"]["AreaDrawingStyle"] = "Circle";

            chart1.Series["black"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            chart1.Series["black"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            chart1.Series["black"].MarkerSize = 8;
            chart1.Series["black"].Color = System.Drawing.Color.Black;
            //chart1.Series["Series1"].LabelBackColor = Color.Green;
            chart1.Series["black"]["PolarDrawingStyle"] = "Marker";
            chart1.Series["black"]["AreaDrawingStyle"] = "Circle";

            // Chart 초기 전시용, Start/End 표기용
            chart1.Series["white"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            chart1.Series["white"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            chart1.Series["white"].MarkerSize = 0;
            chart1.Series["white"].Color = System.Drawing.Color.White;
            //chart1.Series["Series1"].LabelBackColor = Color.Green;
            chart1.Series["white"]["PolarDrawingStyle"] = "Marker";
            chart1.Series["white"]["AreaDrawingStyle"] = "Circle";

            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 360;
            chart1.ChartAreas[0].AxisX.Interval = 30;
            chart1.ChartAreas[0].AxisY.Minimum = -90;
            chart1.ChartAreas[0].AxisY.Maximum = 90;
            chart1.ChartAreas[0].AxisY.Interval = 30;
            //chart1.ChartAreas[0].AxisY.Crossing = 180;


            //chart1.Series["yellow"].Points.AddXY(0, 0);
            /*
            chart1.Series["yellow"].Points.AddXY(10, 10);
            chart1.Series["red"].Points.AddXY(20, 20);
            chart1.Series["red"].Points.AddXY(30, 30);
            chart1.Series["black"].Points.AddXY(40, 40);
            chart1.Series["black"].Points.AddXY(50, 50);
            */

            // 최초 차트 전시용
            chart1.Series["white"].Points.AddXY(0, -90);


        }

        public void DrawDataOnPolarGragh(double[] Ra, double[] Dec, string color, bool startOrbit, bool endOrbit)
        {
            for (int i = 0; i < Ra.Length; i++)
            {
                chart1.Series[color].Points.AddXY(Ra[i], Dec[i]);
            }

            if (startOrbit == true)
            {
                chart1.Series[color].Points[0].Label = "Start";
            }
            if (endOrbit == true)
            {
                chart1.Series[color].Points[Ra.Length - 1].Label = "End";
            }


        }

        public void ChartPointClear()
        {
            chart1.Series["yellow"].Points.Clear();
            chart1.Series["red"].Points.Clear();
            chart1.Series["black"].Points.Clear();
            chart1.Series["white"].Points.Clear();
        }



        public bool Set_FilteringSatelliteValue()   // Searching 을 위한 기준값 Setting
        {
            // 고도
            if (double.TryParse(altitude_startValue_txt.Text, out Filtering_SatelliteValue.Minimum_Altitude))
            {
                if (Filtering_SatelliteValue.Minimum_Altitude < 0.0)
                {
                    // 범위 예외
                    MessageBox.Show("범위 오류 : 최소 고도");
                    return false;
                }
            }
            else
            {
                // 형식 예외
                MessageBox.Show("형식 오류 : 최소 고도");
                return false;
            }

            if (double.TryParse(altitude_endValue_txt.Text, out Filtering_SatelliteValue.Maximum_Altitude))
            {
                if (Filtering_SatelliteValue.Maximum_Altitude < 0.0 || Filtering_SatelliteValue.Maximum_Altitude < Filtering_SatelliteValue.Minimum_Altitude)
                {
                    MessageBox.Show("범위 오류 : 최대 고도");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("형식 오류 : 최대 고도");
                return false;
            }

            // 시작시간 & 종료시간
            DateTime filtering_startTime = new DateTime(startDate_dateTimePicker.Value.Year, startDate_dateTimePicker.Value.Month, startDate_dateTimePicker.Value.Day,
                startTime_dateTimePicker.Value.Hour, startTime_dateTimePicker.Value.Minute, 0);
            DateTime filtering_endTime = new DateTime(endDate_dateTimePicker.Value.Year, endDate_dateTimePicker.Value.Month, endDate_dateTimePicker.Value.Day,
                endTime_dateTimePicker.Value.Hour, endTime_dateTimePicker.Value.Minute, 0);
            if (DateTime.Compare(filtering_startTime, filtering_endTime) != -1)
            {
                MessageBox.Show("형식 오류 : 시간 범위");
                return false;
            }

            // 고각
            if (double.TryParse(elevation_startValue_txt.Text, out Filtering_SatelliteValue.Minimum_Elevation))
            {
                if (Filtering_SatelliteValue.Minimum_Elevation < -90.0 || Filtering_SatelliteValue.Minimum_Elevation > 90.0)
                {
                    MessageBox.Show("범위 오류 : 최소 고각");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("형식 오류 : 최소 고각");
                return false;
            }

            if (double.TryParse(elevation_endValue_txt.Text, out Filtering_SatelliteValue.Maximum_Elevation))
            {
                if (Filtering_SatelliteValue.Maximum_Elevation < -90.0 || Filtering_SatelliteValue.Maximum_Elevation > 90.0 ||
                    Filtering_SatelliteValue.Maximum_Elevation < Filtering_SatelliteValue.Minimum_Elevation)
                {
                    MessageBox.Show("범위 오류 : 최대 고각");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("형식 오류 : 최대 고각");
                return false;
            }


            // RCS
            if (double.TryParse(RCS_startValue_txt.Text, out Filtering_SatelliteValue.Minimum_RCS))
            {
                if (Filtering_SatelliteValue.Minimum_RCS < 0.0)
                {
                    MessageBox.Show("범위 오류 : 최소 RCS");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("형식 오류 : 최소 RCS");
                return false;
            }

            if (double.TryParse(RCS_endValue_txt.Text, out Filtering_SatelliteValue.Maximum_RCS))
            {
                if (Filtering_SatelliteValue.Maximum_RCS < 0.0 || Filtering_SatelliteValue.Maximum_RCS < Filtering_SatelliteValue.Minimum_RCS)
                {
                    MessageBox.Show("범위 오류 : 최대 RCS");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("형식 오류 : 최대 RCS");
                return false;
            }

            // 타입
            Filtering_SatelliteValue.Object_Type = object_type_comboBox.SelectedItem.ToString();



            return true;
        }

        private void Search_btn_Click(object sender, EventArgs e)   // 검색(조건 필터링) Click 시
        {
            // 검색 조건 오류(범위 or 형식)
            if (Set_FilteringSatelliteValue() == false)
            {
                return;
            }

            // TLE/CPF 선택
            bool check_tle = TLE_checkBox.Checked;
            bool check_cpf = CPF_checkBox.Checked;
            if (check_tle == false && check_cpf == false)
            {
                MessageBox.Show("경고 : 궤도정보(TLE/CPF)를 선택하세요.");
                return;
            }

            // TLE 목록 초기화
            _mManager = new ProjectManager();
            _mManager.Start = schedule_startTime;
            _mChart.Init(_mManager);
            _mChart.Invalidate();

            myTasks.Clear();
            taskGridView_infos.Clear();

            if (taskGridView.DataSource != null)
            {
                taskGridView.Columns.Clear();
                taskGridView.DataSource = null;
            }

            // CPF 목록 초기화
            _mManager_cpf = new ProjectManager();
            _mManager_cpf.Start = schedule_startTime;
            _mChart_cpf.Init(_mManager_cpf);
            _mChart_cpf.Invalidate();

            myTasks_cpf.Clear();
            cpfGridView_infos.Clear();

            if (CPFGridView.DataSource != null)
            {
                CPFGridView.Columns.Clear();
                CPFGridView.DataSource = null;
            }

            list_cpfFiles.Clear();

            // Ephemeris 목록 초기화
            _mManager_ephemeris = new ProjectManager();
            _mManager_ephemeris.Start = schedule_startTime;
            _mChart_ephemeris.Init(_mManager_ephemeris);
            _mChart_ephemeris.Invalidate();

            myTasks_ephemeris.Clear();
            ephemerisData_infos.Clear();

            if (ephemerisData_dataGridView != null)
            {
                ephemerisData_dataGridView.Columns.Clear();
                ephemerisData_dataGridView.DataSource = null;
            }

            ephemeris_tuples.Clear();

            // 위성/우주물체 정보 초기화
            Initialize_Satellite_Information();

            // GroundTrack 및 SkyView 초기화
            GroundTrack.Image = null;
            GroundTrack.Update();
            ChartPointClear();

            // Reference Data 관련요소 초기화 + 관측 스케줄 관련요소 초기화
            Initialize_Reference();
            Initialize_ObservingSchedule();

            Thread thread = new Thread(() =>
            {
                // 스케줄러 생성
                RaDec_To_CreateTask(true, check_tle, check_cpf);
            });
            thread.Start();

            progress_state.ShowDialog();
            thread.Join();
        }

        private void searchingKeyhole_btn_Click(object sender, EventArgs e) // Keyhole 검색
        {
            if (information_data1.Text == "-")
            {
                MessageBox.Show("위성을 선택하세요.");
                return;
            }

            bool exist_keyhole = false;
            DateTime keyhole_startTime = new DateTime();
            DateTime keyhole_endTime = new DateTime();

            TimeSpan diff = schedule_endTime - schedule_startTime;
            int totalSeconds = (int)diff.TotalSeconds;

            double[][] keyhole_azel = new double[2][];
            for (int i = 0;  i < keyhole_azel.Length; i++) { keyhole_azel[i] = new double[totalSeconds]; }

            if (select_TLE_CPF == "tle")
            {
                StringBuilder line1 = new StringBuilder(TLE_Information[information_data1.Text].Line1);
                StringBuilder line2 = new StringBuilder(TLE_Information[information_data1.Text].Line2);
                double[] epochTime = new double[6];
                GetTLEData(sub_tleClass, line1, line2, epochTime);

                double[] siteloc = new double[3];
                GetSiteLocation(Global.laserSite, siteloc);

                double timeMJD;
                double[] result = new double[2];
                for (int i = 0; i < totalSeconds; i++)
                {
                    string str_time = ((schedule_startTime.ToUniversalTime()).AddSeconds(i)).ToString("yyyy-MM-dd-HH-mm-ss.fff");
                    string[] split_time = str_time.Split('-');
                    double[] double_time = new double[split_time.Length];
                    for (int j = 0; j < double_time.Length; j++) { double_time[j] = double.Parse(split_time[j]); }

                    timeMJD = ConvertGreg2MJD(Global.timeSys, double_time);
                    GenerateCommand_TLE(sub_tleClass, siteloc, timeMJD, 0, result);

                    keyhole_azel[0][i] = result[0];    // Azimuth
                    keyhole_azel[1][i] = result[1];    // Elevation

                    if (exist_keyhole == false && keyhole_azel[1][i] > 85.0)
                    {
                        exist_keyhole = true;
                        keyhole_startTime = schedule_startTime.AddSeconds(i);
                    }
                    else if (exist_keyhole == true && keyhole_azel[1][i] < 85.0)
                    {
                        keyhole_endTime = schedule_startTime.AddSeconds(i - 1);
                        break;
                    }
                    else if (exist_keyhole == true && i == totalSeconds - 1)
                    {
                        keyhole_endTime = schedule_startTime.AddSeconds(i);
                        break;
                    }
                }

                if (exist_keyhole == true)
                {
                    information_data8.Text = keyhole_startTime.ToString("HH:mm:ss") + " ~ " + keyhole_endTime.ToString("HH:mm:ss");
                }
                else { information_data8.Text = "없음"; }

            }
            else if (select_TLE_CPF == "cpf")
            {
                //MessageBox.Show("CPF Keyhole 구현중...");
                
                StringBuilder cpfFile = new StringBuilder(list_cpfFiles[CPFGridView.SelectedRows[0].Index]);
                ReadCPF(cpfFile, sub_cpfClass);
                StringBuilder satellite_name = new StringBuilder(); // 위성이름 Get용 Box
                double epoch_MJD = new double();
                LoadCPFInfo(sub_cpfClass, satellite_name, ref epoch_MJD);
                
                double[] siteloc = new double[3];
                GetSiteLocation(Global.laserSite, siteloc);

                // Set Start Time
                string[] start_split = (schedule_startTime.ToUniversalTime().ToString("yyyy-MM-dd-HH-mm-ss.fff")).Split('-');
                double[] start_double = new double[start_split.Length];
                for (int i = 0; i < start_double.Length; i++) { start_double[i] = double.Parse(start_split[i]); }
                double start_MJD = ConvertGreg2MJD(Global.timeSys, start_double);
                // Set End Time
                string[] end_split = (schedule_endTime.ToUniversalTime().ToString("yyyy-MM-dd-HH-mm-ss.fff")).Split('-');
                double[] end_double = new double[end_split.Length];
                for (int i = 0; i < end_double.Length; i++) { end_double[i] = double.Parse(end_split[i]); }
                double end_MJD = ConvertGreg2MJD(Global.timeSys, end_double);
                // Set Step Size
                double stepSize = 1.0;
                // Set File Name
                StringBuilder fileName = new StringBuilder("CPF_KeyholeData" + ".txt");

                // 데이터 파일 생성
                GenerateCommandFile_CPF(sub_cpfClass, siteloc, start_MJD, end_MJD, stepSize, fileName, 0);

                // 데이터 파일 Read
                try
                {
                    FileInfo fileInfo = new FileInfo("./Outputs/" + fileName.ToString());
                    if (fileInfo.Exists)
                    {
                        int receive_count = 0;

                        using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                        {
                            int count = 0;
                            while (!reader.EndOfStream)
                            {
                                count++;
                                string line = reader.ReadLine();
                                string[] line_words = line.Split('\t');

                                if (count >= 12)
                                {
                                    if (line_words[0].Contains("END Ephemeris"))
                                    {
                                        // 끝
                                        if (receive_count != totalSeconds)
                                        {
                                            MessageBox.Show("오류(CPF) : Keyhole 탐색 실패");
                                            return;
                                        }
                                    }
                                    else if (true)
                                    {

                                        if (receive_count < totalSeconds)
                                        {
                                            // 형변환 실패 시, 오류 출력
                                            keyhole_azel[0][receive_count] = double.Parse(line_words[1]);
                                            keyhole_azel[1][receive_count] = double.Parse(line_words[2]);
                                            receive_count++;
                                        }
                                    }
                                }
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류(CPF) : Keyhole 탐색 실패");
                    return;
                }

                // Keyhole Searching
                for (int i = 0; i < keyhole_azel[0].Length; i++)
                {
                    if (exist_keyhole == false && keyhole_azel[1][i] > 85.0)
                    {
                        exist_keyhole = true;
                        keyhole_startTime = schedule_startTime.AddSeconds(i);
                    }
                    else if (exist_keyhole == true && keyhole_azel[1][i] < 85.0)
                    {
                        keyhole_endTime = schedule_startTime.AddSeconds(i - 1);
                        break;
                    }
                    else if (exist_keyhole == true && i == keyhole_azel[0].Length - 1)
                    {
                        keyhole_endTime = schedule_startTime.AddSeconds(i + 1);
                        break;
                    }
                }

                if (exist_keyhole == true)
                {
                    information_data8.Text = keyhole_startTime.ToString("HH:mm:ss") + " ~ " + keyhole_endTime.ToString("HH:mm:ss");
                }
                else { information_data8.Text = "없음"; }

            }
            else if(select_TLE_CPF == "ephemeris")
            {
                string ephemeris_fullPath = ephemeris_tuples[ephemerisData_dataGridView.SelectedRows[0].Index].Item2;
                DateTime ephemeris_startUTC = ephemeris_tuples[ephemerisData_dataGridView.SelectedRows[0].Index].Item3;
                DateTime ephemeris_endUTC = ephemeris_tuples[ephemerisData_dataGridView.SelectedRows[0].Index].Item4;

                // Ephemeris AzEl 추출
                double[][] ephemeris_keyhole_azel = EphemerisFileToTrackingData(ephemeris_fullPath, ephemeris_startUTC, ephemeris_endUTC, 1.0, 0);

                // Keyhole Searching
                for(int i = 0; i < ephemeris_keyhole_azel[0].Length; i++)
                {
                    if (exist_keyhole == false && ephemeris_keyhole_azel[1][i] > 85.0)
                    {
                        exist_keyhole = true;
                        keyhole_startTime = ephemeris_startUTC.AddSeconds(i).ToLocalTime();
                    }
                    else if (exist_keyhole == true && ephemeris_keyhole_azel[1][i] < 85.0)
                    {
                        keyhole_endTime = ephemeris_startUTC.AddSeconds(i - 1).ToLocalTime();
                        break;
                    }
                    else if (exist_keyhole == true && i == ephemeris_keyhole_azel[0].Length - 1)
                    {
                        keyhole_endTime = ephemeris_startUTC.AddSeconds(i + 1).ToLocalTime();
                        break;
                    }
                }

                if (exist_keyhole == true)
                {
                    information_data8.Text = keyhole_startTime.ToString("HH:mm:ss") + " ~ " + keyhole_endTime.ToString("HH:mm:ss");
                }
                else { information_data8.Text = "없음"; }
            }

        }

        Thread sorting_thread;

        private void sortNoradID_btn_Click(object sender, EventArgs e)  // 정렬 : Norad ID
        {
            // Check
            if (sorting_thread != null)
            {
                if (sorting_thread.IsAlive == true) { return; }
                sorting_thread = null;
            }

            // Start
            sortNoradID_btn.Text = "정렬 중...";
            sortNoradID_btn.Enabled = false;
            sortNoradID_btn.Update();

            // Sort
            SortingTLE_NoradID();

            // End
            sortNoradID_btn.Text = "Norad ID";
            sortNoradID_btn.Enabled = true;
            sortNoradID_btn.Update();
        }

        private void sortMaxElevation_btn_Click(object sender, EventArgs e) // 정렬 : Max Elevation
        {
            // Check
            if (sorting_thread != null)
            {
                if (sorting_thread.IsAlive == true) { return; }
                sorting_thread = null;
            }

            // Start
            sortMaxElevation_btn.Text = "정렬 중...";
            sortMaxElevation_btn.Enabled = false;
            sortMaxElevation_btn.Update();

            // Sort
            SortingTLE_MaxElevation();

            // End
            sortMaxElevation_btn.Text = "Max Elevation";
            sortMaxElevation_btn.Enabled = true;
            sortMaxElevation_btn.Update();
        }

        public void SortingTLE_NoradID()
        {
            // TaskGridView 초기화
            if (taskGridView.DataSource != null)
            {
                taskGridView.Columns.Clear();
                taskGridView.DataSource = null;
            }

            // _mManager 및 GanttChart 초기화
            if ((current_page == total_page) && (taskGridView_infos.Count % 1000 != 0))
            {
                for (int i = (current_page - 1) * 1000; i < taskGridView_infos.Count; i++) { _mManager.Delete(myTasks[i]); }
            }
            else
            {
                for (int i = (current_page - 1) * 1000; i < current_page * 1000; i++) { _mManager.Delete(myTasks[i]); }
            }

            // 정렬 수행
            sorting_thread = new Thread(() =>
            {
                for (int i = 0; i < taskGridView_infos.Count - 1; i++)
                {
                    for (int j = i + 1; j < taskGridView_infos.Count; j++)
                    {
                        if (taskGridView_infos[i].NoradID > taskGridView_infos[j].NoradID)
                        {
                            // taskGridView_infos 변경
                            TaskGridView_Info temp_taskGridView_info = taskGridView_infos[i];
                            taskGridView_infos[i] = taskGridView_infos[j];
                            taskGridView_infos[j] = temp_taskGridView_info;

                            // myTasks 변경
                            MyTask temp_myTask = myTasks[i];
                            myTasks[i] = myTasks[j];
                            myTasks[j] = temp_myTask;

                            // _mManager 및 GanttChart 변경
                            //_mManager.Move(myTasks[j], j - i);
                            //_mManager.Move(myTasks[i], i - j + 1);

                        }
                    }
                }
            });
            sorting_thread.Start();
            sorting_thread.Join();

            // DateGridView 정보 전시
            if (taskGridView_infos.Count <= 1000)
            {
                taskGridView.DataSource = taskGridView_infos;
            }
            else
            {
                taskGridView.DataSource = taskGridView_infos.GetRange(0, 1000);  // 스케줄 Page 분할
            }
            taskGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.ClearSelection();

            current_page = 1;
            currentPage_txtbox.Text = String.Format("{0} / {1}", current_page, total_page);

            // Gantt Chart 전시
            if ((current_page == total_page) && (taskGridView_infos.Count % 1000 != 0))
            {
                for (int i = (current_page - 1) * 1000; i < taskGridView_infos.Count; i++)
                {
                    _mManager.Add(myTasks[i]);
                    Split_InterfaceFunction(_mManager, myTasks[i], myTasks[i].PartDuration, myTasks[i].PartColor);
                }
            }
            else
            {
                for (int i = (current_page - 1) * 1000; i < current_page * 1000; i++)
                {
                    _mManager.Add(myTasks[i]);
                    Split_InterfaceFunction(_mManager, myTasks[i], myTasks[i].PartDuration, myTasks[i].PartColor);
                }
            }

            _mChart.Init(_mManager);
            _mChart.Invalidate();

            // 위성/우주물체 정보 초기화
            Initialize_Satellite_Information();

            // GroundTrack 및 SkyView 초기화
            GroundTrack.Image = null;
            GroundTrack.Update();
            ChartPointClear();

            // Reference Data 관련요소 초기화
            Initialize_Reference();
        }

        public void SortingTLE_MaxElevation()
        {
            // TaskGridView 초기화
            if (taskGridView.DataSource != null)
            {
                taskGridView.Columns.Clear();
                taskGridView.DataSource = null;
            }

            // _mManager 및 GanttChart 초기화
            if ((current_page == total_page) && (taskGridView_infos.Count % 1000 != 0))
            {
                for (int i = (current_page - 1) * 1000; i < taskGridView_infos.Count; i++) { _mManager.Delete(myTasks[i]); }
            }
            else
            {
                for (int i = (current_page - 1) * 1000; i < current_page * 1000; i++) { _mManager.Delete(myTasks[i]); }
            }

            // 정렬 수행
            sorting_thread = new Thread(() =>
            {
                for (int i = 0; i < taskGridView_infos.Count - 1; i++)
                {
                    for (int j = i + 1; j < taskGridView_infos.Count; j++)
                    {
                        if (taskGridView_infos[i].MaxElevation < taskGridView_infos[j].MaxElevation)
                        {
                            // taskGridView_infos 변경
                            TaskGridView_Info temp_taskGridView_info = taskGridView_infos[i];
                            taskGridView_infos[i] = taskGridView_infos[j];
                            taskGridView_infos[j] = temp_taskGridView_info;

                            // myTasks 변경
                            MyTask temp_myTask = myTasks[i];
                            myTasks[i] = myTasks[j];
                            myTasks[j] = temp_myTask;

                            // _mManager 및 GanttChart 변경
                            //_mManager.Move(myTasks[j], j - i);
                            //_mManager.Move(myTasks[i], i - j + 1);

                        }
                    }
                }
            });
            sorting_thread.Start();
            sorting_thread.Join();

            // DateGridView 정보 전시
            if (taskGridView_infos.Count <= 1000)
            {
                taskGridView.DataSource = taskGridView_infos;
            }
            else
            {
                taskGridView.DataSource = taskGridView_infos.GetRange(0, 1000);  // 스케줄 Page 분할
            }
            taskGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.ClearSelection();

            current_page = 1;
            currentPage_txtbox.Text = String.Format("{0} / {1}", current_page, total_page);

            // Gantt Chart 전시
            if ((current_page == total_page) && (taskGridView_infos.Count % 1000 != 0))
            {
                for (int i = (current_page - 1) * 1000; i < taskGridView_infos.Count; i++)
                {
                    _mManager.Add(myTasks[i]);
                    Split_InterfaceFunction(_mManager, myTasks[i], myTasks[i].PartDuration, myTasks[i].PartColor);
                }
            }
            else
            {
                for (int i = (current_page - 1) * 1000; i < current_page * 1000; i++)
                {
                    _mManager.Add(myTasks[i]);
                    Split_InterfaceFunction(_mManager, myTasks[i], myTasks[i].PartDuration, myTasks[i].PartColor);
                }
            }

            _mChart.Init(_mManager);
            _mChart.Invalidate();

            // 위성/우주물체 정보 초기화
            Initialize_Satellite_Information();

            // GroundTrack 및 SkyView 초기화
            GroundTrack.Image = null;
            GroundTrack.Update();
            ChartPointClear();

            // Reference Data 관련요소 초기화
            Initialize_Reference();
        }

        public void altitude_sort() // 정렬 도구(고도)
        {
            for (int i = 0; i < taskGridView_infos.Count - 1; i++)
            {
                for (int j = i + 1; j < taskGridView_infos.Count; j++)
                {
                    if (taskGridView_infos[i].Altitude < taskGridView_infos[j].Altitude)
                    {
                        // taskGridView_infos 변경
                        TaskGridView_Info temp_taskGridView_info = taskGridView_infos[i];
                        taskGridView_infos[i] = taskGridView_infos[j];
                        taskGridView_infos[j] = temp_taskGridView_info;

                        // myTasks 변경
                        MyTask temp_myTask = myTasks[i];
                        myTasks[i] = myTasks[j];
                        myTasks[j] = temp_myTask;

                        // _mManager 및 GanttChart 변경
                        _mManager.Move(myTasks[j], j - i);
                        _mManager.Move(myTasks[i], i - j + 1);

                    }
                }

            }

            // DateGridView 전시
            if (taskGridView.DataSource != null)
            {
                taskGridView.Columns.Clear();
                taskGridView.DataSource = null;
            }
            taskGridView.DataSource = taskGridView_infos;
            taskGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.ClearSelection();

            // GanttChart 전시
            _mChart.Init(_mManager);
            _mChart.Invalidate();

            // 위성/우주물체 정보 초기화
            Initialize_Satellite_Information();

            // GroundTrack 및 SkyView 초기화
            GroundTrack.Image = null;
            GroundTrack.Update();
            ChartPointClear();
        }


        public void elevation_sort()    // 정렬 도구(고각)
        {
            for (int i = 0; i < taskGridView_infos.Count - 1; i++)
            {
                for (int j = i + 1; j < taskGridView_infos.Count; j++)
                {
                    if (taskGridView_infos[i].MaxElevation < taskGridView_infos[j].MaxElevation)
                    {
                        // taskGridView_infos 변경
                        TaskGridView_Info temp_taskGridView_info = taskGridView_infos[i];
                        taskGridView_infos[i] = taskGridView_infos[j];
                        taskGridView_infos[j] = temp_taskGridView_info;

                        // myTasks 변경
                        MyTask temp_myTask = myTasks[i];
                        myTasks[i] = myTasks[j];
                        myTasks[j] = temp_myTask;

                        // _mManager 및 GanttChart 변경
                        _mManager.Move(myTasks[j], j - i);
                        _mManager.Move(myTasks[i], i - j + 1);

                    }
                }

            }

            // DateGridView 전시
            if (taskGridView.DataSource != null)
            {
                taskGridView.Columns.Clear();
                taskGridView.DataSource = null;
            }
            taskGridView.DataSource = taskGridView_infos;
            taskGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.ClearSelection();

            // GanttChart 전시
            _mChart.Init(_mManager);
            _mChart.Invalidate();

            // 위성/우주물체 정보 초기화
            Initialize_Satellite_Information();

            // GroundTrack 및 SkyView 초기화
            GroundTrack.Image = null;
            GroundTrack.Update();
            ChartPointClear();
        }


        public void RCS_sort()  // 정렬 도구(RCS)
        {
            for (int i = 0; i < taskGridView_infos.Count - 1; i++)
            {
                for (int j = i + 1; j < taskGridView_infos.Count; j++)
                {
                    if (taskGridView_infos[i].RCS < taskGridView_infos[j].RCS)
                    {
                        // taskGridView_infos 변경
                        TaskGridView_Info temp_taskGridView_info = taskGridView_infos[i];
                        taskGridView_infos[i] = taskGridView_infos[j];
                        taskGridView_infos[j] = temp_taskGridView_info;

                        // myTasks 변경
                        MyTask temp_myTask = myTasks[i];
                        myTasks[i] = myTasks[j];
                        myTasks[j] = temp_myTask;

                        // _mManager 및 GanttChart 변경
                        _mManager.Move(myTasks[j], j - i);
                        _mManager.Move(myTasks[i], i - j + 1);

                    }
                }

            }

            // DateGridView 전시
            if (taskGridView.DataSource != null)
            {
                taskGridView.Columns.Clear();
                taskGridView.DataSource = null;
            }
            taskGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            taskGridView.ClearSelection();

            // GanttChart 전시
            _mChart.Init(_mManager);
            _mChart.Invalidate();

            // 위성/우주물체 정보 초기화
            Initialize_Satellite_Information();

            // GroundTrack 및 SkyView 초기화
            GroundTrack.Image = null;
            GroundTrack.Update();
            ChartPointClear();
        }





        private void CSU_ObserveSchedule2_FormClosing(object sender, FormClosingEventArgs e)    // CSU_ObserveSchedule2 Form 종료 시
        {
            // 관측제어부 위성 데이터 전송 Thread 종료
            if (collect_thread != null)
            {
                if (collect_thread.IsAlive == true)
                {
                    collect_thread.Abort();
                }
                collect_thread = null;
            }

            // 위성/우주물체 정보 전시 + 실시간 위성 위치 업데이트 Thread 종료
            if (display_satInformation != null)
            {
                if (display_satInformation.IsAlive == true)
                {
                    display_satInformation.Abort();
                }
                display_satInformation = null;
            }


        }
        ////////////////////////////////////////////// 위성 위치 데이터 Interface (관측제어부용) ////////////////////////////////////////////////////////

        


        List<string[]> cpfFile_information = new List<string[]>();  // 천문연 서버 모든 파일정보 저장
        public void DownloadFTP_CPF()   // 천문연 FTP 서버 접속 및 CPF File 다운로드
        {

            // CPF 디렉토리 생성
            string CPF_directory = "./CPF";
            DirectoryInfo directoryInfo = new DirectoryInfo(CPF_directory);

            if (directoryInfo.Exists == false)
            {
                directoryInfo.Create();
            }
            else
            {
                // 모든 파일 삭제
                foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                {
                    fileInfo.Delete();
                }
            }

            // 서버접속 정보 Setting
            string ftpPath = "ftp://121.176.96.20:10051";
            string user = "hanwha";
            string pwd = "tlsqhgksghk1!";

            // CPF File Information 확보
            if (cpfFile_information.Count > 0) { cpfFile_information.Clear(); }

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(user, pwd);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    char[] delimiterChars = { ' ' };
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        // [0]:생성날짜, [1]:생성시간, [2]:파일크기, [3]:파일이름
                        string[] split_str = line.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                        cpfFile_information.Add(split_str);
                    }

                }
            }


            // CPF File List 다운로드
            foreach (string[] file_information in cpfFile_information)
            {

                // 라이브러리 문제 및 파괴 위성으로 제외시키는 위성 Filtering
                // hy2b, hy2c, hy2d, lares2, paz 제외 (라이브러리 연동문제 > Delay 너무 심함)
                // apollo11, apollo14, apollo15, luna17, luna21 제외 (파괴/부식 위성)
                if (file_information[3] == "hy2b.cpf" || file_information[3] == "hy2c.cpf" || file_information[3] == "hy2d.cpf" ||
                    file_information[3] == "lares2.cpf" || file_information[3] == "paz.cpf" ||
                    file_information[3] == "apollo11.cpf" || file_information[3] == "apollo14.cpf" || file_information[3] == "apollo15.cpf" ||
                    file_information[3] == "luna17.cpf" || file_information[3] == "luna21.cpf")
                {
                    continue;
                }

                

                // 다운로드 실행
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpPath + "/" + file_information[3]);
                req.Method = WebRequestMethods.Ftp.DownloadFile;
                req.Credentials = new NetworkCredential(user, pwd);

                using (FtpWebResponse response = (FtpWebResponse)req.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (FileStream fileStream = new FileStream(CPF_directory + "/" + file_information[3], FileMode.Create))
                        {
                            responseStream.CopyTo(fileStream);
                        }
                    }
                }

            }


        }

        string[] total_cpfFiles;
        List<string> list_cpfFiles = new List<string>();
        List<MyTask> myTasks_cpf = new List<MyTask>();
        List<CPFGridView_Info> cpfGridView_infos = new List<CPFGridView_Info>();

        Brush[] total_brush_cpf = new Brush[3]; // 1. Red / 2. Black / 3. Yellow
        public void Display_CPFList(bool[] avoidance_type, double[][] sun_azel, double[][] moon_azel)
        {

            total_brush_cpf[0] = System.Drawing.Brushes.Red;
            total_brush_cpf[1] = System.Drawing.Brushes.Black;
            total_brush_cpf[2] = System.Drawing.Brushes.Yellow;

            // Location 설정
            double[] location = new double[3];
            GetSiteLocation(Global.laserSite, location);

            // CPF File 불러오기
            DirectoryInfo cpf_directory = new DirectoryInfo(Application.StartupPath + "\\CPF");

            if (cpf_directory.Exists == false)
            {
                cpf_directory.Create();
            }

            string[] extensions = { ".opa" };   // CPF 확장자 종류 : ".sha", ".hts", ".sgf", ".opa", ".esa", ".dgf", ".gal", ".ner", ".gfz", ".cne", ".hds", "qss", ".eum", ".cod"
            total_cpfFiles = Directory.GetFiles(cpf_directory.FullName, "*.*", SearchOption.TopDirectoryOnly)
                .Where(f => !extensions.Contains(Path.GetExtension(f),StringComparer.OrdinalIgnoreCase)).ToArray();
            

            // Progress_State 전시
            this.Invoke(new System.Action(() =>
            {
                progress_state.Set_ProgressBarMaximum(3, total_cpfFiles.Length);
                progress_state.Set_ProgressBarValue(3, 0);
                progress_state.Set_StateText(3, "CPF 목록 생성 중...");
            }));
            int progress_count = 0;

            foreach (var cpfFile in total_cpfFiles)
            {
                // 파일 이름
                // 확장자 제외 버전
                //string[] file_words = Path.GetFileName(cpfFile).Split('.');
                //string file_name = file_words[0];
                // 확장자 추가 버전 (파일 Searching 용)
                string file_name = Path.GetFileName(cpfFile);

                (double[][] cpf_AzEl, double cpf_altitude) = CPF_AZEL_Reader(cpfFile, schedule_startTime.ToUniversalTime(), schedule_endTime.ToUniversalTime(), schedule_duration);

                if (cpf_altitude >= Filtering_SatelliteValue.Minimum_Altitude && cpf_altitude <= Filtering_SatelliteValue.Maximum_Altitude) // 고도 Filtering
                {
                    // Task 생성
                    MyTask myTask_cpf = new MyTask(_mManager_cpf) { Name = file_name };

                    // Split 갯수 및 색상 설정
                    int division_count = 0;
                    int[] temp_duration = new int[schedule_duration];
                    Brush[] temp_brush = new Brush[schedule_duration];

                    bool elevation_check = false;

                    // 최대고각
                    double max_elevation = 0.0;

                    for (int i = 0; i < schedule_duration; i++)
                    {
                        // 고각이 범위안에 드는 위성인지 Checking
                        if (cpf_AzEl[1][i] >= Filtering_SatelliteValue.Minimum_Elevation && cpf_AzEl[1][i] <= Filtering_SatelliteValue.Maximum_Elevation)
                        {
                            elevation_check = true;
                        }

                        // 최대고각 Set
                        if (i > 0)
                        {
                            if (cpf_AzEl[1][i] > max_elevation) { max_elevation = cpf_AzEl[1][i]; }
                        }
                        else { max_elevation = cpf_AzEl[1][i]; }

                        if (cpf_AzEl[1][i] > 85.0 || cpf_AzEl[1][i] < 20.0)
                        {
                            // 관측 불가 : 고각 제한
                            if (i > 0)
                            {
                                if (temp_brush[division_count] == total_brush_cpf[1])
                                {
                                    temp_duration[division_count]++;
                                }
                                else
                                {
                                    division_count++;
                                    temp_duration[division_count] = 1;
                                    temp_brush[division_count] = total_brush_cpf[1];
                                }
                            }
                            else if (i == 0)
                            {
                                temp_duration[division_count] = 1;
                                temp_brush[division_count] = total_brush_cpf[1];
                            }
                        }
                        else if ((avoidance_type[i] == true && Filtering_SunPosition(sun_azel[0][i], sun_azel[1][i], cpf_AzEl[0][i], cpf_AzEl[1][i]) < 30.0) ||
                            (avoidance_type[i] == false && Filtering_SunPosition(moon_azel[0][i], moon_azel[1][i], cpf_AzEl[0][i], cpf_AzEl[1][i]) < 30.0))
                        {
                            // 관측 불가 : 태양/달 회피
                            if (i > 0)
                            {
                                if (temp_brush[division_count] == total_brush_cpf[0])
                                {
                                    temp_duration[division_count]++;
                                }
                                else
                                {
                                    division_count++;
                                    temp_duration[division_count] = 1;
                                    temp_brush[division_count] = total_brush_cpf[0];
                                }
                            }
                            else if (i == 0)
                            {
                                temp_duration[division_count] = 1;
                                temp_brush[division_count] = total_brush_cpf[0];
                            }
                        }
                        else
                        {
                            // 관측 가능
                            if (i > 0)
                            {
                                if (temp_brush[division_count] == total_brush_cpf[2])
                                {
                                    temp_duration[division_count]++;
                                }
                                else
                                {
                                    division_count++;
                                    temp_duration[division_count] = 1;
                                    temp_brush[division_count] = total_brush_cpf[2];
                                }
                            }
                            else if (i == 0)
                            {
                                temp_duration[division_count] = 1;
                                temp_brush[division_count] = total_brush_cpf[2];
                            }
                        }

                    }

                    if (elevation_check == true)
                    {
                        // CPF List 추가
                        list_cpfFiles.Add(cpfFile);

                        // Task Duration/Brush Setting
                        int[] task_duration = new int[division_count + 1];
                        Brush[] task_brush = new Brush[division_count + 1];
                        Array.Copy(temp_duration, task_duration, task_duration.Length);
                        Array.Copy(temp_brush, task_brush, task_brush.Length);
                        Split_InterfaceFunction(_mManager_cpf, myTask_cpf, task_duration, task_brush);

                        // Task 추가
                        myTasks_cpf.Add(myTask_cpf);
                        _mManager_cpf.Add(myTask_cpf);
                        _mManager_cpf.SetDuration(myTask_cpf, TimeSpan.FromMinutes(schedule_duration));

                        // DateGridView 정보 생성
                        CPFGridView_Info CPF_Info = new CPFGridView_Info(file_name, cpf_altitude.ToString() + " km", max_elevation.ToString());
                        cpfGridView_infos.Add(CPF_Info);


                    }
                    else
                    {
                        // 필터링고각 요건충족 X
                    }

                }

                // Progress_State 전시
                this.Invoke(new System.Action(() =>
                {
                    progress_count++;
                    progress_state.Set_ProgressBarValue(3, progress_count);
                }));

            }

            // Progress_State 전시
            this.Invoke(new System.Action(() =>
            {
                progress_state.Set_StateText(3, "CPF 목록 생성 완료");
            }));

            this.Invoke(new System.Action(() =>
            {
                // Gantt Chart(CPF) 생성 및 전시
                _mManager_cpf.Start = schedule_startTime;
                _mChart_cpf.Init(_mManager_cpf);
                _mChart_cpf.Invalidate();

                _mChart_cpf.CreateTaskDelegate = delegate () { return new MyTask(_mManager_cpf); };
                _mChart_cpf.AllowTaskDragDrop = false;

                _mChart_cpf.TimeResolution = TimeResolution.Minute;

                CPFGridView.DataSource = cpfGridView_infos;
                CPFGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                CPFGridView.ClearSelection();
            }));
        }

        public (double[][], double) CPF_AZEL_Reader(string file, DateTime start_UTC, DateTime end_UTC, int total_duration)
        {

            double[][] AzEl_data = new double[2][];
            for (int i = 0; i < AzEl_data.Length; i++) { AzEl_data[i] = new double[total_duration]; }
            double altitude = 0.0;

            StringBuilder cpfFile = new StringBuilder(file);
            ReadCPF(cpfFile, cpfClass);
            StringBuilder satellite_name = new StringBuilder(); // 위성이름 Get용 Box
            double epoch_MJD = new double();
            LoadCPFInfo(cpfClass, satellite_name, ref epoch_MJD);

            double[] siteloc = new double[3];
            GetSiteLocation(Global.laserSite, siteloc);

            // Set Start Time
            string[] start_split = (start_UTC.ToString("yyyy-MM-dd-HH-mm-ss.fff")).Split('-');
            double[] start_double = new double[start_split.Length];
            for (int i = 0; i < start_double.Length; i++) { start_double[i] = double.Parse(start_split[i]); }
            double start_MJD = ConvertGreg2MJD(Global.timeSys, start_double);
            // Set End Time
            string[] end_split = (end_UTC.ToString("yyyy-MM-dd-HH-mm-ss.fff")).Split('-');
            double[] end_double = new double[end_split.Length];
            for (int i = 0; i < end_double.Length; i++) { end_double[i] = double.Parse(end_split[i]); }
            double end_MJD = ConvertGreg2MJD(Global.timeSys, end_double);
            // Set Step Size
            double stepSize = 60.0;
            // Set File Name
            StringBuilder fileName = new StringBuilder("CPF_ChartData" + ".txt");

            // 데이터 파일 생성
            GenerateCommandFile_CPF(cpfClass, siteloc, start_MJD, end_MJD, stepSize, fileName, 0);

            // LLA - 고도 추출
            double[] LLA_command = new double[3];
            GenerateLLA_CPF(cpfClass, start_MJD, LLA_command);
            altitude = LLA_command[2];

            // 데이터 파일 Read
            try
            {
                FileInfo fileInfo = new FileInfo("./Outputs/" + fileName.ToString());
                if (fileInfo.Exists)
                {
                    int receive_count = 0;

                    using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                    {
                        int count = 0;
                        while (!reader.EndOfStream)
                        {
                            count++;
                            string line = reader.ReadLine();
                            string[] line_words = line.Split('\t');

                            if (count >= 12)
                            {
                                if (line_words[0].Contains("END Ephemeris"))
                                {
                                    // 끝
                                    if (receive_count != schedule_duration)
                                    {
                                        MessageBox.Show("오류 : CPF 데이터 생성 실패");
                                        return (AzEl_data, altitude);
                                    }
                                }
                                else if (true/*double.Parse(line_words[0]) % 60.0 == 0*/)
                                {

                                    if (receive_count < schedule_duration)
                                    {
                                        // 형변환 실패 시, 오류 출력
                                        AzEl_data[0][receive_count] = double.Parse(line_words[1]);
                                        AzEl_data[1][receive_count] = double.Parse(line_words[2]);
                                        receive_count++;
                                    }
                                }
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류 : CPF 데이터 생성 실패");
                return (AzEl_data, altitude);
            }

            return (AzEl_data, altitude);
        }

        Thread cpfGridView_cellClick_thread;
        int cpfGridView_index = -1; // CPFGridView 선택 취소 기능추가
        private void CPFGridView_CellClick(object sender, DataGridViewCellEventArgs e) // CPF 위성 선택 시 >> Ground Track / Sky View 전시
        {
            if (e.RowIndex < 0 || e.RowIndex >= CPFGridView.Rows.Count || e.ColumnIndex < 0)
                return;
            int rowIndex = CPFGridView.SelectedRows[0].Index;
            if (rowIndex < 0)
                return;
            if (cpfGridView_cellClick_thread != null && cpfGridView_cellClick_thread.IsAlive)
            {
                try
                {
                    if (cpfGridView_index >= 0 && cpfGridView_index < CPFGridView.Rows.Count)
                        CPFGridView.Rows[cpfGridView_index].Selected = true;
                }
                catch { /* 인덱스 범위 벗어나도 무시 */ }

                MessageBox.Show("CPF 정보 불러오는 중...\r\n잠시 후 시도하세요.");
                return;
            }

            cpfGridView_index = e.RowIndex;

            try
            {
                taskGridView.ClearSelection();
                ephemerisData_dataGridView.ClearSelection();

                tabControl1.SelectedTab = cpfPage;
                _mChart_cpf.ScrollTo(myTasks_cpf[e.RowIndex]);
                _mChart_cpf.Update();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] CPFGridView_CellClick chart update error: {ex.Message}");
            }

            cpfGridView_cellClick_thread = new Thread(() =>
            {
                try
                {
                    CPFGridView_CellClick_Event(e.RowIndex);

                    this.Invoke(new Action(() =>
                    {
                        try
                        {
                            Initialize_Reference();
                            selectedSatellite_label.Text = myTasks_cpf[e.RowIndex].Name;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[DEBUG] UI update error: {ex.Message}");
                        }
                    }));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEBUG] CPFGridView_CellClick thread error: {ex.Message}");
                }
            });
            cpfGridView_cellClick_thread.Start();
        }

        List<Point[]> total_points_cpf = new List<Point[]>();
        List<Color> total_color_cpf = new List<Color>();
        public void CPFGridView_CellClick_Event(int dataGridView_rowIndex)
        {
            if (select_TLE_CPF == "cpf")
            {
                cpfDisplay_stop = true;
                if (display_satInformation != null && display_satInformation.IsAlive)
                {
                    display_satInformation.Join();
                }

                cpfDisplay_stop = false;

                this.Invoke(new System.Action(() =>
                {
                    // 위성/우주물체 정보 초기화 + Thread 정지
                    Initialize_CPF_Satellite_Information();

                    // GroundTrack 및 SkyView 초기화
                    GroundTrack.Image = null;
                    GroundTrack.Update();
                    ChartPointClear();
                }));
            }
            else
            {
                this.Invoke(new System.Action(() =>
                {
                    // 위성/우주물체 정보 초기화 + Thread 정지
                    Initialize_Satellite_Information();

                    // GroundTrack 및 SkyView 초기화
                    GroundTrack.Image = null;
                    GroundTrack.Update();
                    ChartPointClear();
                }));
            }
            

            double[][] LLA_data = new double[2][];
            for (int i = 0; i < LLA_data.Length; i++) { LLA_data[i] = new double[schedule_duration]; }
            
            StringBuilder cpfFile = new StringBuilder(list_cpfFiles[dataGridView_rowIndex]);
            ReadCPF(cpfFile, cpfClass);
            StringBuilder satellite_name = new StringBuilder(); // 위성이름 Get용 Box
            double epoch_MJD = new double();
            LoadCPFInfo(cpfClass, satellite_name, ref epoch_MJD);

            // Set Start Time
            string[] start_split = ((schedule_startTime.ToUniversalTime()).ToString("yyyy-MM-dd-HH-mm-ss.fff")).Split('-');
            double[] start_double = new double[start_split.Length];
            for (int i = 0; i < start_double.Length; i++) { start_double[i] = double.Parse(start_split[i]); }
            double start_MJD = ConvertGreg2MJD(Global.timeSys, start_double);
            // Set End Time
            string[] end_split = ((schedule_endTime.ToUniversalTime()).ToString("yyyy-MM-dd-HH-mm-ss.fff")).Split('-');
            double[] end_double = new double[end_split.Length];
            for (int i = 0; i < end_double.Length; i++) { end_double[i] = double.Parse(end_split[i]); }
            double end_MJD = ConvertGreg2MJD(Global.timeSys, end_double);
            // Set Step Size
            double stepSize = 60.0;
            // Set File Name
            StringBuilder fileName = new StringBuilder("CPF_DrawingData" + ".txt");

            // Create Data File
            GenerateLLAFile_CPF(cpfClass, start_MJD, end_MJD, stepSize, fileName);



            // 데이터 파일 Read
            try
            {
                FileInfo fileInfo = new FileInfo("./Outputs/" + fileName.ToString());
                if (fileInfo.Exists)
                {
                    int receive_count = 0;

                    using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                    {
                        int count = 0;
                        while (!reader.EndOfStream)
                        {
                            count++;
                            string line = reader.ReadLine();
                            string[] line_words = line.Split('\t');

                            if (count >= 12)
                            {
                                if (line_words[0].Contains("END Ephemeris"))
                                {
                                    // 끝
                                    if (receive_count != schedule_duration)
                                    {
                                        MessageBox.Show("오류 : CPF 데이터 생성 실패");
                                        return;
                                    }
                                }
                                else if (true/*double.Parse(line_words[0]) % 60.0 == 0*/)
                                {

                                    if (receive_count < schedule_duration)
                                    {
                                        // 형변환 실패 시, 오류 출력
                                        LLA_data[0][receive_count] = double.Parse(line_words[1]);
                                        LLA_data[1][receive_count] = double.Parse(line_words[2]);
                                        receive_count++;
                                    }
                                }
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류 : CPF 데이터 생성 실패");
                return;
            }



            Color current_color = Color.White;
            int graphics_count = 0;
            int stackData_count = 0; int subData_count = 0;

            // points/colors List 초기화
            total_points_cpf.Clear();
            total_color_cpf.Clear();

            for (int i = 0; i < schedule_duration; i++)
            {
                stackData_count++;

                // 확보가능 위치 데이터는 1440 / 궤도 전시 알고리즘 구상
                if (i > 0)
                {
                    if (((LLA_data[0][i] < 220.0 && LLA_data[0][i] >= 180.0) && (LLA_data[0][i - 1] > 140.0 && LLA_data[0][i - 1] < 180.0)) ||
                            ((LLA_data[0][i] > 140.0 && LLA_data[0][i] < 180.0) && (LLA_data[0][i - 1] >= 180.0 && LLA_data[0][i - 1] < 220.0)))
                    {
                        if (myTasks_cpf[dataGridView_rowIndex].PartColor[graphics_count] == total_brush_cpf[0]) { current_color = Color.Red; }
                        else if (myTasks_cpf[dataGridView_rowIndex].PartColor[graphics_count] == total_brush_cpf[1]) { current_color = Color.White/*Color.Black*/; }
                        else if (myTasks_cpf[dataGridView_rowIndex].PartColor[graphics_count] == total_brush_cpf[2]) { current_color = Color.Yellow; }

                        // Ground Track 전시 데이터 Set
                        if ((stackData_count - subData_count - 1) > 1)
                        {
                            /////////////////////// 레이아웃 변환 시 공백 채우기용 /////////////////////
                            if (subData_count == 0)
                            {
                                // 끝 부분만
                                Point[] points = new Point[stackData_count - subData_count - 1 + 1];

                                for (int j = 0; j < points.Length - 1; j++)
                                {
                                    points[j] = MercatorProjection(LLA_data[0][(i + 1) - points.Length + j], LLA_data[1][(i + 1) - points.Length + j]);
                                }

                                if (LLA_data[0][i - 1] < 180)
                                {
                                    points[points.Length - 1] = MercatorProjection(179.9, LLA_data[1][i - 1]);
                                }
                                else // (azelDatas[0][i - 1] >= 180)
                                {
                                    points[points.Length - 1] = MercatorProjection(180.0, LLA_data[1][i - 1]);
                                }
                                total_points_cpf.Add(points);
                            }
                            else
                            {
                                // 시작 + 끝 부분
                                Point[] points = new Point[1 + stackData_count - subData_count - 1 + 1];

                                for (int j = 1; j < points.Length - 1; j++)
                                {
                                    points[j] = MercatorProjection(LLA_data[0][(i + 1 + 1) - points.Length + (j - 1)], LLA_data[1][(i + 1 + 1) - points.Length + (j - 1)]);
                                }

                                if (LLA_data[0][(i + 1 + 1) - points.Length] < 180)
                                {
                                    points[0] = MercatorProjection(179.9, LLA_data[1][(i + 1 + 1) - points.Length]);
                                }
                                else
                                {
                                    points[0] = MercatorProjection(180.0, LLA_data[1][(i + 1 + 1) - points.Length]);
                                }
                                if (LLA_data[0][i - 1] < 180)
                                {
                                    points[points.Length - 1] = MercatorProjection(179.9, LLA_data[1][i - 1]);
                                }
                                else
                                {
                                    points[points.Length - 1] = MercatorProjection(180.0, LLA_data[1][i - 1]);
                                }
                                total_points_cpf.Add(points);
                            }

                            Color color = current_color;
                            total_color_cpf.Add(color);

                        }

                        // subData 설정
                        subData_count = stackData_count - 1;
                    }
                }

                // 궤도 색상 구분 구현
                if (/*((i + 1) % 60 == 0) && */(i != 0))
                {
                    // 색상 변경여부 Checking
                    if ((stackData_count/* / 60*/) == myTasks_cpf[dataGridView_rowIndex].PartDuration[graphics_count])
                    {
                        if (myTasks_cpf[dataGridView_rowIndex].PartColor[graphics_count] == total_brush_cpf[0]) { current_color = System.Drawing.Color.Red; }
                        else if (myTasks_cpf[dataGridView_rowIndex].PartColor[graphics_count] == total_brush_cpf[1]) { current_color = System.Drawing.Color.White/*Color.Black*/; }
                        else if (myTasks_cpf[dataGridView_rowIndex].PartColor[graphics_count] == total_brush_cpf[2]) { current_color = System.Drawing.Color.Yellow; }

                        // Ground Track 전시 데이터 Set
                        if ((stackData_count - subData_count) > 1)
                        {
                            //////////////// 그래프 사이 간격 채우기용 ///////////////////


                            /////////////////////// 레이아웃 변환 시 공백 채우기용 /////////////////////
                            if (subData_count == 0)
                            {


                                if ((i + 1 == schedule_duration) ||
                                    ((LLA_data[0][i + 1] < 220.0 && LLA_data[0][i + 1] >= 180.0) && (LLA_data[0][i] > 140.0 && LLA_data[0][i] < 180.0)) ||
                                    ((LLA_data[0][i + 1] > 140.0 && LLA_data[0][i + 1] < 180.0) && (LLA_data[0][i] >= 180.0 && LLA_data[0][i] < 220.0)))
                                {
                                    Point[] points = new Point[stackData_count - subData_count];

                                    for (int j = 0; j < points.Length; j++)
                                    {
                                        points[j] = MercatorProjection(LLA_data[0][(i + 1) - points.Length + j], LLA_data[1][(i + 1) - points.Length + j]);
                                    }
                                    total_points_cpf.Add(points);
                                }
                                else
                                {
                                    Point[] points = new Point[stackData_count - subData_count + 1];
                                    for (int j = 0; j < points.Length - 1; j++)
                                    {
                                        points[j] = MercatorProjection(LLA_data[0][(i + 1 + 1) - points.Length + j], LLA_data[1][(i + 1 + 1) - points.Length + j]);
                                    }

                                    points[points.Length - 1] = MercatorProjection(LLA_data[0][i + 1], LLA_data[1][i + 1]);
                                    total_points_cpf.Add(points);
                                }
                            }
                            else
                            {
                                // 시작 부분 추가
                                if ((i + 1 == schedule_duration) ||
                                    ((LLA_data[0][i + 1] < 220.0 && LLA_data[0][i + 1] >= 180.0) && (LLA_data[0][i] > 140.0 && LLA_data[0][i] < 180.0)) ||
                                    ((LLA_data[0][i + 1] > 140.0 && LLA_data[0][i + 1] < 180.0) && (LLA_data[0][i] >= 180.0 && LLA_data[0][i] < 220.0)))
                                {
                                    Point[] points = new Point[1 + stackData_count - subData_count];

                                    for (int j = 1; j < points.Length; j++)
                                    {
                                        points[j] = MercatorProjection(LLA_data[0][(i + 1 + 1) - points.Length + (j - 1)], LLA_data[1][(i + 1 + 1) - points.Length + (j - 1)]);
                                    }

                                    if (LLA_data[0][(i + 1 + 1) - points.Length] < 180)
                                    {
                                        points[0] = MercatorProjection(179.9, LLA_data[1][(i + 1 + 1) - points.Length]);
                                    }
                                    else
                                    {
                                        points[0] = MercatorProjection(180.0, LLA_data[1][(i + 1 + 1) - points.Length]);
                                    }
                                    total_points_cpf.Add(points);
                                }
                                else
                                {
                                    Point[] points = new Point[1 + stackData_count - subData_count + 1];

                                    for (int j = 1; j < points.Length - 1; j++)
                                    {
                                        points[j] = MercatorProjection(LLA_data[0][(i + 1 + 1 + 1) - points.Length + (j - 1)], LLA_data[1][(i + 1 + 1 + 1) - points.Length + (j - 1)]);
                                    }

                                    if (LLA_data[0][(i + 1 + 1 + 1) - points.Length] < 180)
                                    {
                                        points[0] = MercatorProjection(179.9, LLA_data[1][(i + 1 + 1 + 1) - points.Length]);
                                    }
                                    else
                                    {
                                        points[0] = MercatorProjection(180.0, LLA_data[1][(i + 1 + 1 + 1) - points.Length]);
                                    }
                                    points[points.Length - 1] = MercatorProjection(LLA_data[0][i + 1], LLA_data[1][i + 1]);
                                    total_points_cpf.Add(points);
                                }

                            }

                            Color color = current_color;
                            total_color_cpf.Add(color);

                        }

                        // graphics 카운팅 + stackData 갯수 초기화
                        graphics_count++;
                        stackData_count = 0;
                        if (subData_count != 0) { subData_count = 0; }

                    }
                }



            }

            this.Invoke(new System.Action(() =>
            {
                // Ground Track 전시
                for (int i = 0; i < total_points_cpf.Count; i++)
                {
                    Graphics graphics = GroundTrack.CreateGraphics();
                    Pen pen = new Pen(total_color_cpf[i], 4f);
                    graphics.DrawCurve(pen, total_points_cpf[i]);
                }

                // SkyView 전시
                int sky_count = 0;
                for (int i = 0; i < myTasks_cpf[dataGridView_rowIndex].PartDuration.Length; i++)
                {
                    double[] sky_longitude = new double[myTasks_cpf[dataGridView_rowIndex].PartDuration[i]];
                    double[] sky_latitude = new double[myTasks_cpf[dataGridView_rowIndex].PartDuration[i]];
                    Array.Copy(LLA_data[0], sky_count, sky_longitude, 0, sky_longitude.Length);
                    Array.Copy(LLA_data[1], sky_count, sky_latitude, 0, sky_latitude.Length);

                    string skyView_color = "";
                    if (myTasks_cpf[dataGridView_rowIndex].PartColor[i] == total_brush_cpf[0]) { skyView_color = "red"; }
                    else if (myTasks_cpf[dataGridView_rowIndex].PartColor[i] == total_brush_cpf[1]) { skyView_color = "black"; }
                    else if (myTasks_cpf[dataGridView_rowIndex].PartColor[i] == total_brush_cpf[2]) { skyView_color = "yellow"; }

                    bool startOrbit = false; bool endOrbit = false;

                    DrawDataOnPolarGragh(sky_longitude, sky_latitude, skyView_color, startOrbit, endOrbit);
                    sky_count = sky_count + myTasks_cpf[dataGridView_rowIndex].PartDuration[i];

                    if (i == 0)
                    {
                        chart1.Series["white"].Points.AddXY(LLA_data[0][0], LLA_data[1][0]);
                        chart1.Series["white"].Points[0].Label = "Start";
                    }
                    if (i == myTasks_cpf[dataGridView_rowIndex].PartDuration.Length - 1)
                    {
                        chart1.Series["white"].Points.AddXY(LLA_data[0][schedule_duration - 1], LLA_data[1][schedule_duration - 1]);
                        chart1.Series["white"].Points[1].Label = "End";
                    }

                }

            }));

            string avoid_text = "-";
            int avoid_count = 0;
            for (int i = 0; i < myTasks_cpf[dataGridView_rowIndex].PartColor.Length; i++)
            {
                if (myTasks_cpf[dataGridView_rowIndex].PartColor[i] == total_brush_cpf[0])
                {
                    // 태양/달 회피구간 있음
                    avoid_text = schedule_startTime.AddMinutes(avoid_count).ToString("HH:mm") + " ~ " + schedule_startTime.AddMinutes(avoid_count + myTasks_cpf[dataGridView_rowIndex].PartDuration[i]).ToString("HH:mm");
                    break;
                }
                avoid_count = avoid_count + myTasks_cpf[dataGridView_rowIndex].PartDuration[i];
            }

            // 위성/우주물체 정보 전시
            Display_CPFSatellite_Information(list_cpfFiles[dataGridView_rowIndex], dataGridView_rowIndex, avoid_text);
            select_TLE_CPF = "cpf";
        }

        bool cpfDisplay_stop = false;
        public void Display_CPFSatellite_Information(string cpfFile, int index, string data9_text)
        {
            // 1초 간격 데이터 업데이트
            if (display_satInformation != null)
            {
                if (display_satInformation.IsAlive == true)
                {
                    display_satInformation.Abort();
                }
                display_satInformation = null;
            }

            display_satInformation = new Thread(() =>
            {
                // 위성 이름
                //string[] file_words = Path.GetFileName(cpfFile).Split('.');
                //string file_name = file_words[0];
                string file_name = Path.GetFileName(cpfFile);

                // CPF 각 위성병 고도 추출
                StringBuilder file = new StringBuilder(cpfFile);
                ReadCPF(file, cpfClass);
                StringBuilder targetName = new StringBuilder();
                double epochMJD = new double();
                LoadCPFInfo(cpfClass, targetName, ref epochMJD);

                // Site Location 설정
                double[] location = new double[3];
                GetSiteLocation(Global.laserSite, location);

                // 정보전시 (1)
                this.Invoke(new System.Action(() =>
                {
                    information_data1.Text = file_name;
                    information_data2.Text = "NSLR-OAS";
                    information_data6.Text = (sunRiseSet_time[0].ToLocalTime()).ToString("yyyy-MM-dd HH:mm");
                    information_data7.Text = (sunRiseSet_time[1].ToLocalTime()).ToString("yyyy-MM-dd HH:mm");
                    //information_data8.Text = "-";
                    information_data9.Text = data9_text;
                    information_data10.Text = String.Format("{0:0.00000}", cpfGridView_infos[index].MaxElevation) + "°"; ;
                }));

                // 결과 Box
                double[] LLA_command = new double[3];
                double[] AzEl_command = new double[2];

                int thread_count = 0;
                while (true)
                {
                    DateTime dateTime = DateTime.UtcNow;
                    string[] splitData = (dateTime.ToString("yyyy-MM-dd-HH-mm-ss.fff")).Split('-');
                    double[] doubleData = new double[splitData.Length];
                    for (int i = 0; i < doubleData.Length; i++) { doubleData[i] = double.Parse(splitData[i]); }
                    double targetMJD = ConvertGreg2MJD(Global.timeSys, doubleData);

                    GenerateLLA_CPF(cpfClass, targetMJD, LLA_command);  // LLA
                    GenerateCommand_CPF(cpfClass, location, targetMJD, 0, AzEl_command);    // Az/El

                    // 정보 전시 (2)
                    this.Invoke(new System.Action(() =>
                    {
                        information_data3.Text = String.Format("{0:0.00}", LLA_command[2]) + " km";
                        information_data4.Text = String.Format("{0:0.00000}", AzEl_command[0]) + "°";
                        information_data5.Text = String.Format("{0:0.00000}", AzEl_command[1]) + "°";
                        //information_data10.Text = String.Format("{0:0.00000}", AzEl_command[1]) + "°";
                    }));

                    // Grount Track 실시간위성 표시 (5회 주기)
                    if (thread_count % 5 == 0)
                    {

                        this.Invoke(new System.Action(() =>
                        {
                            // Ground Track 전시
                            GroundTrack.Image = null;
                            GroundTrack.Update();

                            for (int i = 0; i < total_points_cpf.Count; i++)
                            {
                                Graphics graphic = GroundTrack.CreateGraphics();
                                Pen pen = new Pen(total_color_cpf[i], 4f);
                                graphic.DrawCurve(pen, total_points_cpf[i]);
                            }

                            Graphics position_graphic = GroundTrack.CreateGraphics();
                            Bitmap satelliteIcon = new Bitmap(NSLR_ObservationControl.Properties.Resources.satOrbit1);
                            Point LLA_point = MercatorProjection(LLA_command[0], LLA_command[1]);
                            position_graphic.DrawImage(satelliteIcon, LLA_point.X - 25, LLA_point.Y - 25, 50, 50);

                        }));


                    }
                    thread_count++;

                    if (cpfDisplay_stop == true)
                    {
                        // Display Thread 중지요청
                        break;
                    }
                }

                
            });
            display_satInformation.Start();

        }

        int sendCPF_index = -1;
        public async System.Threading.Tasks.Task GetRaDec_ObservingSatelliteCPF(DateTime local_start, DateTime local_end)
        {
            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            rggSATcontrol.Set_LUT_Clear(); //LUT을 전송하기 전에 RGG의 LUT를 지우기
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
                rggDEBcontrol.Set_LUT_Clear(); //LUT을 전송하기 전에 RGG의 LUT를 지우기

            int row_index = CPFGridView.SelectedRows[0].Index;
            sendCPF_index = row_index;  // 관측데이터 생성 중, 운용자가 CPF 위성 바꾸는 Case 고려
            if (row_index >= 0 && row_index < CPFGridView.Rows.Count)
            {
                ObservingSatellite_Information.available_flag = false;
/*                this.Invoke(new System.Action(() =>
                {
                    CPF_label.Text = "CPF LIST (상태 : 데이터 수집 중)";
                }));*/

                string CPF_satellite = CPFGridView.SelectedRows[0].Cells[0].Value.ToString();
                double[][] RaDec_data = new double[2][];
                double[][] Keyhole_data = new double[2][];
                //double temp_observingminutes = 10;
                double temp_observingminutes = (local_end.ToUniversalTime() - local_start.ToUniversalTime()).TotalMinutes;
                double interval_seconds = 0.02;
                int data_size = (int)(temp_observingminutes) * 60 * (int)(1 / interval_seconds);

                for (int i = 0; i < RaDec_data.Length; i++) { RaDec_data[i] = new double[data_size]; }
                for (int i = 0; i < Keyhole_data.Length; i++) { Keyhole_data[i] = new double[data_size]; }
                // LLA ///////////////////////////////////////
                double[][] LLA_data = new double[3][];
                //double temp_observingminutes_LLA = 10;
                double temp_observingminutes_LLA = (local_end.ToUniversalTime() - local_start.ToUniversalTime()).TotalMinutes;
                double interval_seconds_LLA = 0.05;
                int data_size_LLA = (int)(temp_observingminutes_LLA) * 60 * (int)(1 / interval_seconds_LLA);

                for (int i = 0; i < LLA_data.Length; i++) { LLA_data[i] = new double[data_size_LLA]; }
                //////////////////////////////////////////////
                StringBuilder cpfFile = new StringBuilder(list_cpfFiles[row_index]);
                ReadCPF(cpfFile, observingSatellite_cpfClass);
                StringBuilder satellite_name = new StringBuilder(); // 위성이름 Get용 Box
                double epoch_MJD = new double();
                LoadCPFInfo(observingSatellite_cpfClass, satellite_name, ref epoch_MJD);

                double[] siteloc = new double[3];
                GetSiteLocation(Global.laserSite, siteloc);

                // Set Start Time
                //DateTime start_time = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0, DateTimeKind.Utc);
                DateTime start_time = local_start.ToUniversalTime();
                string[] start_split = (start_time.ToString("yyyy-MM-dd-HH-mm-ss.ff")).Split('-');
                double[] start_double = new double[start_split.Length];
                for (int i = 0; i < start_double.Length; i++) { start_double[i] = double.Parse(start_split[i]); }
                double start_MJD = ConvertGreg2MJD(Global.timeSys, start_double);
                // Set End Time
                //DateTime end_time = start_time.AddMinutes(temp_observingminutes);
                DateTime end_time = local_end.ToUniversalTime();
                string[] end_split = (end_time.ToString("yyyy-MM-dd-HH-mm-ss.ff")).Split('-');
                double[] end_double = new double[end_split.Length];
                for (int i = 0; i < end_double.Length; i++) { end_double[i] = double.Parse(end_split[i]); }
                double end_MJD = ConvertGreg2MJD(Global.timeSys, end_double);
                // Set Step Size
                double stepSize = 0.02;
                // LLA ///////////////////////////////////////
                double stepSize_LLA = 0.05;
                //////////////////////////////////////////////

                // Set File Name
                StringBuilder fileName = new StringBuilder("CPF_TempObservingData" + ".txt");
                StringBuilder fileName2 = new StringBuilder("CPF_KeyholeCheckingData" + ".txt");
                // Create Data File
                GenerateCommandFile_CPF(observingSatellite_cpfClass, siteloc, start_MJD, end_MJD, stepSize, fileName, 2);
                GenerateCommandFile_CPF(observingSatellite_cpfClass, siteloc, start_MJD, end_MJD, stepSize, fileName2, 0);


                // Position Data read
                try
                {
                    FileInfo fileInfo = new FileInfo("./Outputs/" + fileName.ToString());
                    if (fileInfo.Exists)
                    {
                        int receive_count = 0;

                        using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                        {
                            int count = 0;
                            while (!reader.EndOfStream)
                            {
                                count++;
                                string line = reader.ReadLine();
                                string[] line_words = line.Split('\t');

                                if (count >= 12)
                                {
                                    if (line_words[0].Contains("END Ephemeris"))
                                    {
                                        // 끝
                                        if (receive_count != data_size)
                                        {
                                            MessageBox.Show("오류 : CPF 데이터 생성 실패");
                                            this.Invoke(new System.Action(() =>
                                            {
                                                CPF_label.Text = "CPF LIST (상태 : 데이터 수집 오류발생)";
                                            }));
                                        }
                                    }
                                    else if (true/*(Math.Round(double.Parse(line_words[0]), 2) % 0.02) == 0*/)
                                    {

                                        if (receive_count < data_size)
                                        {
                                            // 형변환 실패 시, 오류 출력
                                            RaDec_data[0][receive_count] = double.Parse(line_words[1]);
                                            RaDec_data[1][receive_count] = double.Parse(line_words[2]);

                                            receive_count++;
                                        }
                                    }
                                }
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류 : CPF 데이터 생성 실패");
                    this.Invoke(new System.Action(() =>
                    {
                        CPF_label.Text = "CPF LIST (상태 : 데이터 수집 오류발생)";
                    }));
                }

                try
                {
                    FileInfo fileInfo = new FileInfo("./Outputs/" + fileName2.ToString());
                    if (fileInfo.Exists)
                    {
                        int receive_count = 0;

                        using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                        {
                            int count = 0;
                            while (!reader.EndOfStream)
                            {
                                count++;
                                string line = reader.ReadLine();
                                string[] line_words = line.Split('\t');

                                if (count >= 12)
                                {
                                    if (line_words[0].Contains("END Ephemeris"))
                                    {
                                        // 끝
                                        if (receive_count != data_size)
                                        {
                                            MessageBox.Show("오류 : CPF 데이터 생성 실패");
                                            this.Invoke(new System.Action(() =>
                                            {
                                                CPF_label.Text = "CPF LIST (상태 : 데이터 수집 오류발생)";
                                            }));
                                        }
                                    }
                                    else if (true/*(Math.Round(double.Parse(line_words[0]), 2) % 0.02) == 0*/)
                                    {

                                        if (receive_count < data_size)
                                        {
                                            // 형변환 실패 시, 오류 출력
                                            Keyhole_data[0][receive_count] = double.Parse(line_words[1]);
                                            Keyhole_data[1][receive_count] = double.Parse(line_words[2]);

                                            receive_count++;
                                        }
                                    }
                                }
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류 : CPF 데이터 생성 실패");
                    this.Invoke(new System.Action(() =>
                    {
                        CPF_label.Text = "CPF LIST (상태 : 데이터 수집 오류발생)";
                    }));
                }

                // Set File Name
                StringBuilder fileName_LLA = new StringBuilder("CPF_TempObservingData_LLA" + ".txt");
                // Create Data File
                GenerateLLAFile_CPF(observingSatellite_cpfClass, start_MJD, end_MJD, stepSize_LLA, fileName_LLA);

                // Position Data read
                try
                {
                    FileInfo fileInfo = new FileInfo("./Outputs/" + fileName_LLA.ToString());
                    if (fileInfo.Exists)
                    {
                        int receive_count = 0;

                        using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                        {
                            int count = 0;
                            while (!reader.EndOfStream)
                            {
                                count++;
                                string line = reader.ReadLine();
                                string[] line_words = line.Split('\t');

                                if (count >= 12)
                                {
                                    if (line_words[0].Contains("END Ephemeris"))
                                    {
                                        // 끝
                                        if (receive_count != data_size_LLA)
                                        {
                                            MessageBox.Show("오류 : CPF 데이터 생성 실패");
                                            this.Invoke(new System.Action(() =>
                                            {
                                                CPF_label.Text = "CPF LIST (상태 : 데이터 수집 오류발생)";
                                            }));
                                        }
                                    }
                                    else if (true/*(Math.Round(double.Parse(line_words[0]), 2) % 0.02) == 0*/)
                                    {

                                        if (receive_count < data_size_LLA)
                                        {
                                            // 형변환 실패 시, 오류 출력
                                            LLA_data[0][receive_count] = double.Parse(line_words[1]);
                                            LLA_data[1][receive_count] = double.Parse(line_words[2]);
                                            LLA_data[2][receive_count] = double.Parse(line_words[3]);

                                            receive_count++;
                                        }
                                    }
                                }
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("오류 : " + ex.ToString());
                    this.Invoke(new System.Action(() =>
                    {
                        CPF_label.Text = "CPF LIST (상태 : 데이터 수집 오류발생)";
                    }));
                }

                // Check Keyhole
                bool keyhole_flag = false;
                for (int i = 0; i < Keyhole_data[0].Length; i++)
                {
                    if (Keyhole_data[1][i] >= 85.0)
                    {
                        keyhole_flag = true;
                        break;
                    }
                }


                ObservingSatellite_Information.ObservingSatellite_SettingData(CPF_satellite, start_time, end_time.AddSeconds(-1 * interval_seconds), interval_seconds);
                ObservingSatellite_Information.ObservingSatellite_SettingRaDec(RaDec_data);
                // LLA ///////////////////////////////////////////////////
                ObservingSatellite_Information.ObservingSatellite_SettingLLA(end_time.AddSeconds(-1 * interval_seconds_LLA), LLA_data);
                ObservingSatellite_Information.ObservingSatellite_SettingKeyhole(keyhole_flag);
                //////////////////////////////////////////////////////////

                //Jason RGG_Controller에 CPF 궤도 데이터 전달 ////////////////////////
                ObservationData observationData = new ObservationData
                {
                    SatelliteName = CPF_satellite,
                    StartTime = start_time,
                    EndTime = end_time.AddSeconds(-1 * interval_seconds_LLA),
                    Interval_second = interval_seconds_LLA,
                    LLA_data = LLA_data
                };
                currSatelCPFFile = observationData.SatelliteName;

                if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                    await rggSATcontrol.SetObservationData("RGG_CPF", observationData);
                else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
                    await rggDEBcontrol.SetObservationData("RGG_CPF", observationData);

                /////////////////////////////////////////////////////////////
                ObservingSatellite_Information.available_flag = true;
                CalculateAzElDataAndAssign();
   /*             this.Invoke(new System.Action(() =>
                {
                    CPF_label.Text = "CPF LIST (상태 : 데이터 수집 완료)";
                }));*/
                SatelliteDataUpdated?.Invoke(observationData.StartTime, observationData.EndTime, observationData.SatelliteName, 0 );
            }
            else
            {
                MessageBox.Show("CPF LIST 위성을 선택하세요.");
            }



        }

        public static string currSatelCPFFile { get; set; }
        tcspk _tcspk = new tcspk();
        private void CalculateAzElDataAndAssign()
        {
            var satelliteData = ObservingSatellite_Information.GetData();
            if (!satelliteData.AvailableFlag) return;
            var azel = new double[2][];
            for (int j = 0; j < 2; j++)
                azel[j] = new double[satelliteData.RaDec[j].Length];

            int totalSteps = (int)((satelliteData.EndTime - satelliteData.StartTime).TotalMilliseconds / (satelliteData.Interval * 1000)) + 1;
            timestamps = new double[totalSteps];
            DateTime currentTime = satelliteData.StartTime;
            TimeSpan interval = TimeSpan.FromMilliseconds(satelliteData.Interval * 1000);

            for (int i = 0; i < totalSteps; i++)
            {
                timestamps[i] = (currentTime - satelliteData.StartTime.Date).TotalMilliseconds;
                currentTime += interval;
            }
            var ProjectModeSelect = 1;
            _tcspk.StarExcute(ProjectModeSelect);
            azel = _tcspk.FetchAndProcessTrackingData(satelliteData);

            double firstAz = azel[0][0];
            double secondAz = azel[0][1];
            double lastAz = azel[0][azel[0].Length - 1];
            int direction = _tcspk.StarPositionSignDecide(firstAz, secondAz, lastAz);

            for (int i = 0; i < azel[0].Length; i++)
                azel[0][i] = _tcspk.AzConvert_satel(azel[0][i], direction);
            AzElData = azel;
            var azelDome = new double[2][];
            for (int j = 0; j < 2; j++)
                azelDome[j] = new double[satelliteData.RaDec[j].Length];

            int domeSteps = (int)((satelliteData.EndTime - satelliteData.StartTime).TotalMilliseconds / (satelliteData.Interval * 5000)) + 1;
            timestamps_dome = new double[domeSteps];
            currentTime = satelliteData.StartTime;
            interval = TimeSpan.FromMilliseconds(satelliteData.Interval * 5 * 1000);

            for (int i = 0; i < domeSteps; i++)
            {
                timestamps_dome[i] = (currentTime - satelliteData.StartTime.Date).TotalMilliseconds;
                currentTime += interval;
        }

            _tcspk.StarExcute(ProjectModeSelect);
            azelDome = _tcspk.FetchAndProcessTrackingDomeData(satelliteData);
            AzElData_Dome = azelDome;
        }


        Thread collect_thread;
        private async void sendData_btn_Click(object sender, EventArgs e) // 데이터 전송(TLE) Click 시
        {
            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR || DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                if (TaskModeNow.CurrentSystemObject != TaskModeNow.SystemObject.RANGE)
                {
                    MessageBox.Show("Range 모드로 전환 후 재시도해주세요.");
                    return;
                }
            }
            // 예외 처리
            if (taskGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("위성을 선택하세요,");
                return;
            }

            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR || DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                if (TaskModeNow.CurrentSystemObject != TaskModeNow.SystemObject.RANGE)
                {
                    MessageBox.Show("Range 모드로 전환 후 재시도해주세요.");
                    return;
                }
            }
            // 이전 관측 위성 정보 저장 중 일 경우 >> 임시
            if (collect_thread != null && collect_thread.IsAlive)
            {
                MessageBox.Show("에러 : 현재 위성 정보수집 중 입니다." + "\r\n" + "잠시 후 시도하세요,");
                return;
            }

            int task_index = ((current_page - 1) * 1000) + taskGridView.SelectedRows[0].Index;

            // Start ~ End 신호 임의변경 기능 추가 ///////////////////////////////////////////
            DateTime local_start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tempStart_dateTimePicker.Value.Hour, tempStart_dateTimePicker.Value.Minute, 0);
            DateTime local_end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tempEnd_dateTimePicker.Value.Hour, tempEnd_dateTimePicker.Value.Minute, 0);
            if (DateTime.Compare(local_start, local_end) != -1)
            {
                MessageBox.Show("시간 Range 오류!");
                return;
            }
            TLE_label.Text = "TLE LIST (상태 : 데이터 수집 중)";
            TLE_label.ForeColor = Color.OrangeRed;
            if (DateTime.Compare(local_start, schedule_startTime) == -1 || DateTime.Compare(local_end, schedule_endTime) == 1)
            {
                if (MessageBox.Show("관측 Range(Start ~ End)가 스케줄러 Range(Start ~ End)를 벗어납니다.\r\n전송하겠습니까?", "경고", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            /////////////////////////////////////////////////////////////////////////////////

            // 관측 위성 정보 저장 >> 임시
            /*            collect_thread = new Thread(async () =>
                        {
                            GetRaDec_ObservingSatellite(myTasks[task_index].Name, local_start, local_end);

                            //Test_0122("BSAT-2A", new DateTime(2025, 1, 22, 6, 36, 0, DateTimeKind.Utc));
                            //Test_0122("THAICOM 4", new DateTime(2025, 1, 22, 6, 39, 0, DateTimeKind.Utc));
                            //Test_0122("JCSAT-5A", new DateTime(2025, 1, 22, 6, 41, 0, DateTimeKind.Utc));
                            //Test_0122("NAVSTAR 68 (USA 242)", new DateTime(2025, 1, 22, 6, 45, 0, DateTimeKind.Utc));
                        });
                        collect_thread.Start();*/

            try
            {
                await System.Threading.Tasks.Task.Run(async () =>
                {
                    await GetRaDec_ObservingSatellite(myTasks[task_index].Name, local_start, local_end);
                });

                TLE_label.Text = "TLE LIST (상태 : 데이터 수집 완료)";
                TLE_label.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                TLE_label.Text = "TLE LIST (상태 : 오류 발생)";
                TLE_label.ForeColor = Color.Red;
                MessageBox.Show($"데이터 수집 중 오류 발생: {ex.Message}");
            }

        }

        private async void sendData2_btn_Click(object sender, EventArgs e)
        {
            // 예외 처리
            if (CPFGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("위성을 선택하세요,");
                return;
            }

            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR || DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                if (TaskModeNow.CurrentSystemObject != TaskModeNow.SystemObject.RANGE)
                {
                    MessageBox.Show("Range 모드로 전환 후 재시도해주세요.");
                    return;
                }
            }
            // 이전 관측 위성 정보 저장 중 일 경우 >> 임시
            if (collect_thread != null && collect_thread.IsAlive)
            {
                MessageBox.Show("에러 : 현재 위성 정보수집 중 입니다." + "\r\n" + "잠시 후 시도하세요,");
                return;
            }
            CPF_label.Text = "CPF LIST (상태 : 데이터 수집 중)";
            CPF_label.ForeColor = Color.OrangeRed;
            // Start ~ End 신호 임의변경 기능 추가 ///////////////////////////////////////////
            DateTime local_start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tempStart_cpf_dateTimePicker.Value.Hour, tempStart_cpf_dateTimePicker.Value.Minute, 0);
            DateTime local_end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tempEnd_cpf_dateTimePicker.Value.Hour, tempEnd_cpf_dateTimePicker.Value.Minute, 0);
            if (DateTime.Compare(local_start, local_end) != -1)
            {
                MessageBox.Show("시간 Rage 오류!");
                return;
            }
            /////////////////////////////////////////////////////////////////////////////////

            // 관측 위성 정보 저장 >> 임시
            /*      collect_thread = new Thread(() =>
                  {
                      GetRaDec_ObservingSatelliteCPF(local_start, local_end);
                  });
                  collect_thread.Start();*/
            try
            {
                await System.Threading.Tasks.Task.Run(async () =>
                {
                    await GetRaDec_ObservingSatelliteCPF(local_start, local_end);
                });

                CPF_label.Text = "CPF LIST (상태 : 데이터 수집 완료)";
                CPF_label.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                CPF_label.Text = "CPF LIST (상태 : 오류 발생)";
                CPF_label.ForeColor = Color.Red;
                MessageBox.Show($"데이터 수집 중 오류 발생:\r\n{ex.Message}");
            }

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public void ISSData_ButtonFunction()
        {
            // ISS 데이터 확인용
            CreateFile_AzEl("SPACE STATION");
        }
        public void ISS_ButtonFunction()
        {
            // 이전 관측 위성 정보 저장 중 일 경우 >> 임시
            if (collect_thread != null && collect_thread.IsAlive)
            {
                MessageBox.Show("에러 : 현재 위성 정보수집 중 입니다." + "\r\n" + "잠시 후 시도하세요,");
                return;
            }

            // 관측 위성 정보 저장 >> 임시
            collect_thread = new Thread(() =>
            {
                GetRaDec_ISS();


            });
            collect_thread.Start();
        }


        public void GetRaDec_ISS()
        {
            ObservingSatellite_Information.available_flag = false;
            this.Invoke(new System.Action(() =>
            {
                tracking_label.Text = "SPACE STATION" + " " + " (상태 : 데이터 수집 중)";
            }));

            double[][] RaDec_data = new double[2][];
            int temp_observingHours = 1;
            double interval_seconds = 0.02;
            int data_size = temp_observingHours * 60 * 60 * (int)(1 / interval_seconds);

            for (int i = 0; i < RaDec_data.Length; i++) { RaDec_data[i] = new double[data_size]; }

            // 주어진 TLE 정보
            string ISS_line1 = "1 25544U 98067A   24346.98672894  .00019985  00000-0  35072-3 0  9994";
            string ISS_line2 = "2 25544  51.6393 154.4947 0007200 331.0970 170.8608 15.50504106486123";

            // TLE 객체 생성
            Tle tle = new Tle(ISS_line1, ISS_line2);
            Sgp4 sgp4 = new Sgp4(tle);

            // 관측자의 ECI 좌표 계산 (예: 샌프란시스코 위치)
            double observerLat = 35.5901;   // 위도 (예: 샌프란시스코)
            double observerLon = 127.9192; // 경도 (예: 샌프란시스코)
            double observerAlt = 0.92330;     // 고도 (예: 30m)

            DateTime startTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0, DateTimeKind.Utc);
            DateTime endTime = (startTime.AddHours(temp_observingHours)).AddSeconds(-1 * interval_seconds);


            FileInfo fileInfo = new FileInfo("./" + "RaDec_TestData.txt");
            using (StreamWriter writer = fileInfo.CreateText())
            {
                for (int i = 0; i < data_size; i++)
                {
                    // 주어진 UTC 시간
                    DateTime utcDateTime = startTime.AddSeconds(interval_seconds * i);

                    // 위성 위치 계산
                    var eci = sgp4.FindPosition(utcDateTime);

                    // ECI 좌표 추출 (km 단위)
                    double x = eci.Position.X;
                    double y = eci.Position.Y;
                    double z = eci.Position.Z;

                    var observerEci = CalculateObserverECI(observerLat, observerLon, observerAlt, utcDateTime);

                    // 위성의 Topocentric MJ2000Eq 좌표 계산
                    double topoX = x - observerEci.X;
                    double topoY = y - observerEci.Y;
                    double topoZ = z - observerEci.Z;

                    // Topocentric RA/Dec 계산
                    double r = Math.Sqrt(topoX * topoX + topoY * topoY + topoZ * topoZ);
                    double ra = Math.Atan2(topoY, topoX);  // RA in radians
                    double dec = Math.Asin(topoZ / r);     // Dec in radians

                    // 라디안에서 도 단위로 변환
                    ra = ra * (180.0 / Math.PI); // Right Ascension in degrees
                    dec = dec * (180.0 / Math.PI); // Declination in degrees

                    // RA를 0-360도 범위로 조정
                    if (ra < 0)
                        ra += 360;

                    // 결과값 입력
                    RaDec_data[0][i] = ra;
                    RaDec_data[1][i] = dec;

                    if (i % 3000 == 0)
                    {
                        writer.WriteLine(utcDateTime.ToString("yyyy-MM-dd-HH-mm-ss.ff") + " / " + RaDec_data[0][i].ToString() + " " + RaDec_data[1][i].ToString());
                    }
                }


            }

            ObservingSatellite_Information.ObservingSatellite_SettingData("SPACE STATION", startTime, endTime, interval_seconds);
            ObservingSatellite_Information.ObservingSatellite_SettingRaDec(RaDec_data);


            ObservingSatellite_Information.available_flag = true;
            this.Invoke(new System.Action(() =>
            {
                tracking_label.Text = "SPACE STATION" + " " + "(상태 : 데이터 수집 완료)";
            }));

        }

        // 관측자의 ECI 좌표 계산
        static (double X, double Y, double Z) CalculateObserverECI(double lat, double lon, double alt, DateTime utcDateTime)
        {
            // 지구 반지름과 편평률
            double earthRadius = 6378.137; // Equatorial radius in km
            double flattening = 1.0 / 298.257223563;

            // 위도, 경도를 라디안으로 변환
            lat = lat * (Math.PI / 180.0);
            lon = lon * (Math.PI / 180.0);

            // 지구 반지름 계산
            double cosLat = Math.Cos(lat);
            double sinLat = Math.Sin(lat);
            double f = 1.0 - flattening;
            double c = 1.0 / Math.Sqrt(cosLat * cosLat + f * f * sinLat * sinLat);
            double s = f * f * c;

            // ECEF 좌표
            double x = (earthRadius * c + alt) * cosLat * Math.Cos(lon);
            double y = (earthRadius * c + alt) * cosLat * Math.Sin(lon);
            double z = (earthRadius * s + alt) * sinLat;

            // UTC → GMST 변환
            double jd = JulianDate(utcDateTime);
            double gmst = GreenwichMeanSiderealTime(jd);

            // ECEF → ECI 변환
            double xEci = x * Math.Cos(gmst) - y * Math.Sin(gmst);
            double yEci = x * Math.Sin(gmst) + y * Math.Cos(gmst);
            double zEci = z;

            return (xEci, yEci, zEci);
        }

        // 율리우스 날짜 계산
        static double JulianDate(DateTime dateTime)
        {
            int year = dateTime.Year;
            int month = dateTime.Month;
            if (month <= 2)
            {
                year -= 1;
                month += 12;
            }
            int A = year / 100;
            int B = 2 - A + (A / 4);
            double jd = Math.Floor(365.25 * (year + 4716)) +
                        Math.Floor(30.6001 * (month + 1)) +
                        dateTime.Day + B - 1524.5 +
                        (dateTime.Hour + dateTime.Minute / 60.0 + dateTime.Second / 3600.0) / 24.0;
            return jd;
        }

        // 그리니치 평균 항성시(GMST) 계산
        static double GreenwichMeanSiderealTime(double jd)
        {
            double T = (jd - 2451545.0) / 36525.0;
            double gmst = 280.46061837 + 360.98564736629 * (jd - 2451545.0) +
                          0.000387933 * T * T - T * T * T / 38710000.0;
            gmst = (gmst % 360.0) * (Math.PI / 180.0); // Degrees to radians
            if (gmst < 0)
                gmst += 2 * Math.PI; // Ensure GMST is in 0 to 2π range
            return gmst;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Test_0122(string satellite_name, DateTime startUTC)
        {
            // Az/El
            double[][] AZEL_data = new double[2][];
            double interval_seconds = 0.02;
            int data_size = 10 * 60 * (int)(1 / interval_seconds);
            for (int i = 0; i < AZEL_data.Length; i++) { AZEL_data[i] = new double[data_size]; }

            StringBuilder line1 = new StringBuilder(TLE_Information[satellite_name].Line1);
            StringBuilder line2 = new StringBuilder(TLE_Information[satellite_name].Line2);

            double[] epochTime = new double[6];
            GetTLEData(observingSatellite_tleClass, line1, line2, epochTime);

            double[] siteloc = new double[3];
            GetSiteLocation(Global.laserSite, siteloc);

            double currentMJD;
            double[] result = new double[2];

            FileInfo fileInfo = new FileInfo("./" + satellite_name + "_AzEl.txt");
            using (StreamWriter writer = fileInfo.CreateText())
            {
                for (int i = 0; i < data_size; i++)
                {
                    string[] split_time = (startUTC.AddSeconds(interval_seconds * i).ToString("yyyy-MM-dd-HH-mm-ss.ff")).Split('-');
                    double[] double_time = new double[split_time.Length];
                    for (int j = 0; j < double_time.Length; j++) { double_time[j] = double.Parse(split_time[j]); }

                    currentMJD = ConvertGreg2MJD(Global.timeSys, double_time);
                    GenerateCommand_TLE(observingSatellite_tleClass, siteloc, currentMJD, 0, result);

                    AZEL_data[0][i] = result[0];
                    AZEL_data[1][i] = result[1];

                    writer.WriteLine(startUTC.AddSeconds(interval_seconds * i).ToString("yyyy-MM-dd-HH-mm-ss.ff") + " / " +
                        AZEL_data[0][i].ToString() + " " + AZEL_data[1][i].ToString());
                }

            }
        }

        public async System.Threading.Tasks.Task GetRaDec_ObservingSatellite(string satellite_name, DateTime local_start, DateTime local_end)  // NSLR-OAS(한화시스템) 라이브러리 버전
        {
            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            rggSATcontrol.Set_LUT_Clear();  //LUT을 전송하기 전에 RGG의 LUT를 지우기
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
                rggDEBcontrol.Set_LUT_Clear(); //LUT을 전송하기 전에 RGG의 LUT를 지우기


            ObservingSatellite_Information.available_flag = false;
/*            this.Invoke(new System.Action(() =>
            {
                TLE_label.Text = "TLE LIST (상태 : 데이터 수집 중)";
            }));*/

            //double temp_observingminutes = 10;
            double temp_observingminutes = (local_end.ToUniversalTime() - local_start.ToUniversalTime()).TotalMinutes;

            // Ra/Dec
            double[][] RaDec_data = new double[2][];
            double[][] Keyhole_data = new double[2][];
            double interval_seconds = 0.02;
            int data_size = (int)(temp_observingminutes) * 60 * (int)(1 / interval_seconds);
            for (int i = 0; i < RaDec_data.Length; i++) { RaDec_data[i] = new double[data_size]; }
            for (int i = 0; i < Keyhole_data.Length; i++) { Keyhole_data[i] = new double[data_size]; }

            // LLA ///////////////////////////////////////////////////
            double[][] LLA_data = new double[3][];
            double LLA_interval_seconds = 0.05;
            int LLA_data_size = (int)(temp_observingminutes) * 60 * (int)(1 / LLA_interval_seconds);
            for (int i = 0; i < LLA_data.Length; i++) { LLA_data[i] = new double[LLA_data_size]; }
            //////////////////////////////////////////////////////////

            StringBuilder line1 = new StringBuilder(TLE_Information[satellite_name].Line1);
            StringBuilder line2 = new StringBuilder(TLE_Information[satellite_name].Line2);

            double[] epochTime = new double[6];
            var nor = GetTLEData(observingSatellite_tleClass, line1, line2, epochTime);

            double[] siteloc = new double[3];
            GetSiteLocation(Global.laserSite, siteloc);

            //DateTime startTime = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0, DateTimeKind.Utc);
            DateTime startTime = local_start.ToUniversalTime();
            //DateTime endTime = (startTime.AddMinutes(temp_observingminutes)).AddSeconds(-1 * interval_seconds);
            DateTime endTime = (local_end.ToUniversalTime()).AddSeconds(-1 * interval_seconds);
            double currentMJD;
            double[] result = new double[2];
            double[] result2 = new double[2];
            // LLA ///////////////////////////////////////////////////
            DateTime LLA_endTime = (local_end.ToUniversalTime()).AddSeconds(-1 * LLA_interval_seconds);
            double[] LLA_result = new double[3];
            //////////////////////////////////////////////////////////


            FileInfo fileInfo = new FileInfo("./" + "RaDec_TestData.txt");
            using (StreamWriter writer = fileInfo.CreateText())
            {
                for (int i = 0; i < data_size; i++)
                {
                    string[] split_time = (startTime.AddSeconds(interval_seconds * i).ToString("yyyy-MM-dd-HH-mm-ss.ff")).Split('-');
                    double[] double_time = new double[6];
                    for (int j = 0; j < double_time.Length; j++)
                    {
                        double_time[j] = double.Parse(split_time[j]);
                    }

                    currentMJD = ConvertGreg2MJD(Global.timeSys, double_time);
                    GenerateCommand_TLE(observingSatellite_tleClass, siteloc, currentMJD, 2, result);  // RaDec (Topo)
                    GenerateCommand_TLE(observingSatellite_tleClass, siteloc, currentMJD, 0, result2);  // keyhole

                    RaDec_data[0][i] = result[0];
                    RaDec_data[1][i] = result[1];
                    Keyhole_data[0][i] = result2[0];
                    Keyhole_data[1][i] = result2[1];

                    if (i % 50 == 0)
                    {
                        writer.WriteLine(startTime.AddSeconds(interval_seconds * i).ToString("yyyy-MM-dd-HH-mm-ss.ff") + " / " + RaDec_data[0][i].ToString() + " " + RaDec_data[1][i].ToString());
                    }
                }

            }

            // Check Keyhole
            bool keyhole_flag = false;
            for (int i = 0; i < Keyhole_data[0].Length; i++)
            {
                if (Keyhole_data[1][i] >= 85.0)
                {
                    keyhole_flag = true;
                    break;
                }
            }

            // LLA ///////////////////////////////////////////////////
            FileInfo LLA_fileInfo = new FileInfo("./" + "LLA_TestData.txt");
            using (StreamWriter LLA_writer = LLA_fileInfo.CreateText())
            {
                for (int i = 0; i < LLA_data_size; i++)
                {
                    string[] split_time = (startTime.AddSeconds(LLA_interval_seconds * i).ToString("yyyy-MM-dd-HH-mm-ss.ff")).Split('-');
                    double[] double_time = new double[6];
                    for (int j = 0; j < double_time.Length; j++)
                    {
                        double_time[j] = double.Parse(split_time[j]);
                    }

                    currentMJD = ConvertGreg2MJD(Global.timeSys, double_time);
                    GenerateLLA_TLE(observingSatellite_tleClass, currentMJD, LLA_result);  // LLA

                    LLA_data[0][i] = LLA_result[0];
                    LLA_data[1][i] = LLA_result[1];
                    LLA_data[2][i] = LLA_result[2];

                    if (i % 20 == 0)
                    {
                        LLA_writer.WriteLine(startTime.AddSeconds(LLA_interval_seconds * i).ToString("yyyy-MM-dd-HH-mm-ss.ff") + " / " + 
                            LLA_data[0][i].ToString() + " " + LLA_data[1][i].ToString() + " " + LLA_data[2][i].ToString());
                    }
                }

            }
            //////////////////////////////////////////////////////////

            ObservingSatellite_Information.ObservingSatellite_SettingData(satellite_name, startTime, endTime, interval_seconds);
            ObservingSatellite_Information.ObservingSatellite_SettingRaDec(RaDec_data);
            // LLA ///////////////////////////////////////////////////
            ObservingSatellite_Information.ObservingSatellite_SettingLLA(LLA_endTime, LLA_data);
            ObservingSatellite_Information.ObservingSatellite_SettingKeyhole(keyhole_flag);
            //////////////////////////////////////////////////////////




            //Jason RGG_Controller에 TLE 궤도 데이터 전달 ////////////////////////
            ObservationData observationData = new ObservationData
            {
                SatelliteName = satellite_name,
                StartTime = startTime,
                EndTime = LLA_endTime,
                Interval_second = LLA_interval_seconds,
                LLA_data = LLA_data
            };

            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                await rggSATcontrol.SetObservationData("RGG_TLE", observationData);
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
                await rggDEBcontrol.SetObservationData("RGG_TLE", observationData);

            /////////////////////////////////////////////////////////////
            ObservingSatellite_Information.available_flag = true;
            CalculateAzElDataAndAssign();
 /*           this.Invoke(new System.Action(() =>
            {
                TLE_label.Text = "TLE LIST (상태 : 데이터 수집 완료)";
            }));*/
            var epoch_time = new double[6];
            SatelliteDataUpdated?.Invoke(observationData.StartTime, observationData.EndTime, satellite_name, (int)nor);
            Console.WriteLine($"[DEBUG] nor value = {nor}");
        }

        public List<double> Compute_Light_Travel_time(string satellite, DateTime UTC_start, DateTime UTC_end , int interval)
        {
            List<double> satellite_tof = new List<double>();

            DateTime UTC_current = DateTime.UtcNow;
            (bool tropoFlag, double[] env) = Get_WMSInformation(UTC_current);

            if (tropoFlag == false)
            {
                MessageBox.Show("대기모델 미반영" + "\r\n" + "사유 : 해당 시간대 날씨정보를 찾을 수 없습니다.");
            }

            TimeSpan observation_diff = UTC_end - UTC_start;
            int totalSeconds = (int)observation_diff.TotalSeconds;

            string[] start_str = UTC_start.ToString("yyyy-MM-dd-HH-mm-ss.fff").Split('-');
            double[] start_time = new double[6];
            for (int i = 0; i < start_time.Length; i++) { start_time[i] = double.Parse(start_str[i]); }
            double start_MJD = ConvertGreg2MJD(Global.timeSys, start_time);

            IntPtr LTT_tleClass = CreateTLE();
            StringBuilder line1 = new StringBuilder(TLE_Information[satellite].Line1);  // TLE Line1
            StringBuilder line2 = new StringBuilder(TLE_Information[satellite].Line2);  // TLE Line2
            double[] epoch_time = new double[6];
            GetTLEData(LTT_tleClass, line1, line2, epoch_time);
            double epoch_MJD = ConvertGreg2MJD(Global.timeSys, epoch_time);

            double tgap = (double)interval / 1000.0;
            double tspan = (double)totalSeconds;
            int nPoints = (int)(tspan / tgap);

            double timeOffset = (start_MJD - epoch_MJD) * 86400.0;
            double[] TXtimeTag = new double[nPoints];
            for (int i = 0; i < nPoints; i++)
            {
                TXtimeTag[i] = epoch_MJD + (timeOffset / 86400.0) + (tgap * i / 86400.0);
            }
            
            int nTime;
            double ephemerisTspan = (start_MJD - epoch_MJD) * 86400.0 + totalSeconds;
            nTime = (int)(ephemerisTspan / 30.0) + 1;
            ephemerisTspan = (double)nTime * 30.0;

            double[] elapsedSecs = new double[nTime];
            for (int n = 0; n < nTime; n++)
            {
                elapsedSecs[n] = n * ephemerisTspan / (double)nTime;
            }
            double[] targetEphemeris = new double[nTime * 6];
            PropagateTLE(LTT_tleClass, elapsedSecs, nTime, targetEphemeris, 0);

            double[] MJDtags = new double[nTime];
            for (int n = 0; n < nTime; n++)
            {
                MJDtags[n] = epoch_MJD + elapsedSecs[n] / 86400.0;
            }

            double[] tof = new double[nPoints];
            ComputeLightTime_array(Global.laserSite, MJDtags, targetEphemeris, nTime, TXtimeTag, nPoints, tropoFlag, env, tof);

            satellite_tof = tof.ToList();

            return satellite_tof;
        }

        public List<double> Compute_Light_Travel_time_CPF(string satellite, DateTime UTC_start, DateTime UTC_end, int interval)
        {
            List<double> satellite_tof_cpf = new List<double>();

            DateTime UTC_current = DateTime.UtcNow;
            (bool tropoFlag, double[] env) = Get_WMSInformation(UTC_current);

            if (tropoFlag == false)
            {
                MessageBox.Show("대기모델 미반영" + "\r\n" + "사유 : 해당 시간대 날씨정보를 찾을 수 없습니다.");
            }

            TimeSpan observation_diff = UTC_end - UTC_start;
            int totalSeconds = (int)observation_diff.TotalSeconds;

            string[] start_str = UTC_start.ToString("yyyy-MM-dd-HH-mm-ss.fff").Split('-');
            double[] start_time = new double[6];
            for (int i = 0; i < start_time.Length; i++) { start_time[i] = double.Parse(start_str[i]); }
            double start_MJD = ConvertGreg2MJD(Global.timeSys, start_time);

            IntPtr LTT_cptClass = CreateCPF();
            StringBuilder LTT_cpfFile = new StringBuilder(list_cpfFiles[CPFGridView.SelectedRows[0].Index]);
            ReadCPF(LTT_cpfFile, LTT_cptClass);
            StringBuilder targetName = new StringBuilder();
            double epochMJD = new double();
            int nTime = LoadCPFInfo(LTT_cptClass, targetName, ref epochMJD);
            StringBuilder _outCoord = new StringBuilder("ECI");
            double[] MJDtags = new double[nTime];
            double[] targetEphemeris = new double[nTime * 6];
            LoadCPFEphemeris(LTT_cptClass, _outCoord, MJDtags, targetEphemeris);

            double tgap = (double)interval / 1000.0;
            double tspan = (double)totalSeconds;
            int nPoints = (int)(tspan / tgap);

            double timeOffset = (start_MJD - epochMJD) * 86400.0;
            double[] TXtimeTag = new double[nPoints];
            for (int i = 0; i < nPoints; i++)
            {
                TXtimeTag[i] = epochMJD + (timeOffset / 86400.0) + (tgap * i / 86400.0);
            }

            double[] tof = new double[nPoints];
            ComputeLightTime_array(Global.laserSite, MJDtags, targetEphemeris, nTime, TXtimeTag, nPoints, tropoFlag, env, tof);

            satellite_tof_cpf = tof.ToList();

            return satellite_tof_cpf;
        }

        public List<double> Compute_Light_Travel_time_Ephemeris(string satellite, DateTime UTC_start, DateTime UTC_end, int interval)
        {
            List<double> satellite_tof_ephemeris = new List<double>();

            DateTime UTC_current = DateTime.UtcNow;
            (bool tropoFlag, double[] env) = Get_WMSInformation(UTC_current);   // 미래시간일 경우, 날씨정보 없음 > 현재시간으로 Searching

            if (tropoFlag == false)
            {
                MessageBox.Show("대기모델 미반영" + "\r\n" + "(사유 : 해당 시간대 날씨정보를 찾을 수 없습니다.)");
            }

            TimeSpan observation_diff = UTC_end - UTC_start;
            int totalSeconds = (int)observation_diff.TotalSeconds;

            string[] start_str = UTC_start.ToString("yyyy-MM-dd-HH-mm-ss.fff").Split('-');
            double[] start_time = new double[6];
            for (int i = 0; i < start_time.Length; i++) { start_time[i] = double.Parse(start_str[i]); }
            double start_MJD = ConvertGreg2MJD(Global.timeSys, start_time);

            StringBuilder ephemFile = new StringBuilder(ephemeris_tuples[ephemerisData_dataGridView.SelectedRows[0].Index].Item2);
            double[] startTime = new double[6];
            double[] finalTime = new double[6];
            int nTime = ReadEphemerisInfo(ephemFile, startTime, finalTime);
            double[] MJDtags = new double[nTime];
            double[] targetEphemeris = new double[nTime * 6];
            double epoch_MJD = LoadEphemeris(ephemFile, MJDtags, targetEphemeris);

            double tgap = (double)interval / 1000.0;
            double tspan = (double) totalSeconds;
            int nPoints = (int)(tspan / tgap);
             
            double timeOffset = (start_MJD - epoch_MJD) * 86400.0;
            double[] TXtimeTag = new double[nPoints];
            for (int i = 0; i < nPoints; i++)
            {
                TXtimeTag[i] = epoch_MJD + (timeOffset / 86400.0) + (tgap * i / 86400.0);
            }

            double[] tof = new double[nPoints];
            ComputeLightTime_array(Global.laserSite, MJDtags, targetEphemeris, nTime, TXtimeTag, nPoints, tropoFlag, env, tof);

            satellite_tof_ephemeris = tof.ToList();

            return satellite_tof_ephemeris;
        }

        //string opc_postgresql_connectStr = String.Format("Server={0};Port={1};Database=postgres;User Id={2};password={3}", "192.168.10.10", "55432", "postgres", "1234");

        public (bool, double[]) Get_WMSInformation(DateTime current_UTC)    // OPC DataBase 내 기상 데이터 검색 (Tof 대기모델 적용)
        {
            bool exist_data = false;
            double[] wms_data = new double[3];

            DateTime current_local = (new DateTime(current_UTC.Year, current_UTC.Month, current_UTC.Day, current_UTC.Hour,
                current_UTC.Minute, 0, DateTimeKind.Utc)).ToLocalTime();

            //  WMS 기상정보 불러오기
            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        // WMS 기상정보 Searching
                        command.CommandText = "SELECT * FROM nslr_wmsinformation;";
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Start Time
                                if (reader["date"].ToString() == current_local.ToString("yyyy-MM-dd") && reader["time"].ToString() == current_local.ToString("HH:mm:ss"))
                                {
                                    wms_data = new double[] { 1e2 * double.Parse(reader["pressure"].ToString()), double.Parse(reader["humidity"].ToString()),
                                        double.Parse(reader["temperature"].ToString())  
                                        };
                                    exist_data = true;
                                    break;
                                }
                            }
                        }

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Postgresql : 기상데이터 검색 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }

            }

            return (exist_data, wms_data);
        }

        public void CreateFile_AzEl(String satellite_name)
        {
            double[][] azel_data = new double[2][];
            int data_size = 6/*3*/ * 60;

            for (int i = 0; i < azel_data.Length; i++)
            {
                azel_data[i] = new double[data_size];
            }

            // ISS 용
            Satellite satellite = new Satellite(satellite_name,
                "1 25544U 98067A   24346.98672894  .00019985  00000-0  35072-3 0  9994",
                "2 25544  51.6393 154.4947 0007200 331.0970 170.8608 15.50504106486123");

            GeodeticCoordinate location = new GeodeticCoordinate(Angle.FromDegrees(35.5901), Angle.FromDegrees(127.9192), 0.92330);
            GroundStation groundStation = new GroundStation(location);
            double[] result = new double[2];
            DateTime standard_UTC = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 12/*DateTime.UtcNow.Day*/, 22/*DateTime.UtcNow.Hour*/, 26/*DateTime.UtcNow.Minute*/, 0, DateTimeKind.Utc);

            for (int i = 0; i < data_size; i++)
            {
                var observation = groundStation.Observe(satellite, standard_UTC.AddSeconds(i)/*standard_UTC.AddMinutes(i)*/);

                result[0] = observation.Azimuth.Degrees;
                result[1] = observation.Elevation.Degrees;

                azel_data[0][i] = result[0];
                azel_data[1][i] = result[1];
            }

            /*
            Checking_OvservingData checking_ovservingData = new Checking_OvservingData(standard_UTC, azel_data);
            checking_ovservingData.ShowDialog();
            */
        }

        public void SGP4_GetRaDec_ObservingSatellite(string satellite_name) // SGP4 버전, 테스트용
        {
            if (TLE_Information.ContainsKey(satellite_name) == false)
            {
                MessageBox.Show("등록되지 않은 위성입니다. 다시 압룍허새요!");
                return;
            }

            ObservingSatellite_Information.available_flag = false;
            this.Invoke(new System.Action(() =>
            {
                TLE_label.Text = "TLE LIST (상태 : 데이터 수집 중)";
            }));


            // start/end Time 및 interval은 스케줄러에 따라 수정
            double[][] RaDec_data = new double[2][];
            int observe_Hours = 1;
            double interval_seconds = 0.02;
            int data_size = observe_Hours * 60 * 60 * (int)(1 / interval_seconds);

            for (int i = 0; i < RaDec_data.Length; i++)
            {
                RaDec_data[i] = new double[data_size];
            }


            Satellite satellite = new Satellite(satellite_name, TLE_Information[satellite_name].Line1, TLE_Information[satellite_name].Line2);
            DateTime current_UTC = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0, DateTimeKind.Utc);
            DateTime start_UTC = current_UTC;
            DateTime end_UTC = (current_UTC.AddHours(observe_Hours)).AddSeconds(-1 * interval_seconds);
            double[] radec_result = new double[2];

            /*
            // SGP Az/El Data 출력CPF_label
            GeodeticCoordinate location = new GeodeticCoordinate(Angle.FromDegrees(35.59), Angle.FromDegrees(127.92), 0.0);
            GroundStation groundStation = new GroundStation(location);
            var observation = groundStation.Observe(satellite, DateTime.UtcNow);    // 설정한 groundStation 적용인가? 다른 용도가 있는가? 
            double[] azel_data = new double[2];
            azel_data[0] = observation.Azimuth.Degrees;
            azel_data[1] = observation.Elevation.Degrees;
            */


            FileInfo fileInfo = new FileInfo("./" + "RaDec_TestData.txt");
            using (StreamWriter writer = fileInfo.CreateText())
            {
                for (int i = 0; i < data_size; i++)
                {
                    if (i != 0) { current_UTC = current_UTC.AddSeconds(interval_seconds); }

                    EciCoordinate eciCoordinate = satellite.Predict(current_UTC);

                    /*
                    // SGP Ra/Dec Data 출력
                    radec_result[0] = Math.Atan2(eciCoordinate.Position.Y, eciCoordinate.Position.X);
                    if (radec_result[0] < 0) { radec_result[0] += 2 * Math.PI; }
                    radec_result[1] = Math.Atan2(eciCoordinate.Position.Z, Math.Sqrt(eciCoordinate.Position.X * eciCoordinate.Position.X + eciCoordinate.Position.Y * eciCoordinate.Position.Y));

                    RaDec_data[0][i] = radec_result[0] * (180 / Math.PI);
                    RaDec_data[1][i] = radec_result[1] * (180 / Math.PI);
                    */

                    // ChatGPT 계산
                    double r = Math.Sqrt(eciCoordinate.Position.X * eciCoordinate.Position.X + eciCoordinate.Position.Y * eciCoordinate.Position.Y +
                        eciCoordinate.Position.Z * eciCoordinate.Position.Z);

                    radec_result[1] = Math.Asin(eciCoordinate.Position.Z / r) * (180.0 / Math.PI);

                    radec_result[0] = Math.Atan2(eciCoordinate.Position.Y, eciCoordinate.Position.X) * (180.0 / Math.PI);

                    if (radec_result[0] < 0)
                        radec_result[0] += 360.0;

                    RaDec_data[0][i] = radec_result[0];
                    RaDec_data[1][i] = radec_result[1];


                    if (i % 3000 == 0)
                    {
                        writer.WriteLine(current_UTC.ToString("yyyy-MM-dd HH:mm:ss.ff") + " / " + RaDec_data[0][i].ToString() + " " + RaDec_data[1][i].ToString());
                    }

                }


            }



            // ObservingSatellite_Information(현재 관측 중인 위성정보) Update
            ObservingSatellite_Information.ObservingSatellite_SettingData(satellite_name, start_UTC, end_UTC, interval_seconds);
            ObservingSatellite_Information.ObservingSatellite_SettingRaDec(RaDec_data);


            ObservingSatellite_Information.available_flag = true;
            this.Invoke(new System.Action(() =>
            {
                TLE_label.Text = "TLE LIST (상태 : 데이터 수집 완료)";
                //MessageBox.Show(start_UTC.ToString("yyyy-MM-dd HH:mm:ss.ff") + " / " + current_UTC.ToString("yyyy-MM-dd HH:mm:ss.fff") + " / " + RaDec_data[0][0].ToString() + " / " + RaDec_data[1][0].ToString());
            }));


            /*
            // SGP LLA Data 출력
            double[] LLA_data = new double[3];
            LLA_data[0] = eciCoordinate.ToGeodetic().Longitude.Degrees;
            LLA_data[1] = eciCoordinate.ToGeodetic().Latitude.Degrees;
            LLA_data[2] = eciCoordinate.ToGeodetic().Altitude;
            */

        }


        /////////////////////////////////////////////////////// 위성 추적 스케줄러 Part /////////////////////////////////////////////////////////////////

        string select_TLE_CPF = "";
        List<Reference_Data> reference_list = new List<Reference_Data>();
        private void createReference_btn_Click(object sender, EventArgs e)  // Reference Data 생성(초단위)
        {
            // 작업 진행 중 일때 (예외처리)
            if (observingSchedule_thread != null && observingSchedule_thread.IsAlive)
            {
                MessageBox.Show("작업 진행 중 입니다. 잠시 후 시도하세요.");
                return;
            }

            if (selectedSatellite_label.Text == "-")
            {
                MessageBox.Show("위성을 선택하세요.");
            }
            else
            {
                createReference_btn.Text = "생성 중...";
                createReference_btn.Enabled = false;
                createReference_btn.Update();

                observingSchedule_thread = new Thread(() =>
                {
                    double[][] reference_data = new double[2][];
                    for (int i = 0; i < reference_data.Length; i++) { reference_data[i] = new double[schedule_duration * 60 + 1]; }

                    if (select_TLE_CPF == "tle")
                    {
                        StringBuilder line1 = new StringBuilder(TLE_Information[selectedSatellite_label.Text].Line1);
                        StringBuilder line2 = new StringBuilder(TLE_Information[selectedSatellite_label.Text].Line2);
                        double[] epochTime = new double[6];
                        GetTLEData(observingSatellite_tleClass, line1, line2, epochTime);

                        int opt = 0;
                        double[] siteLoc = new double[3];
                        GetSiteLocation(Global.laserSite, siteLoc);

                        double targetMJD;
                        double[] result = new double[2];
                        for (int i = 0; i < reference_data[0].Length; i++)
                        {
                            string current_str = ((schedule_startTime.AddSeconds(i)).ToUniversalTime()).ToString("yyyy-MM-dd-HH-mm-ss.fff");
                            string[] current_split = current_str.Split('-');
                            double[] current_double = new double[current_split.Length];
                            for (int j = 0; j < current_double.Length; j++) { current_double[j] = double.Parse(current_split[j]); }
                            targetMJD = ConvertGreg2MJD(Global.timeSys, current_double);

                            GenerateCommand_TLE(observingSatellite_tleClass, siteLoc, targetMJD, opt, result);

                            reference_data[0][i] = result[0];
                            reference_data[1][i] = result[1];
                        }
                    }
                    else if (select_TLE_CPF == "cpf")
                    {

                        StringBuilder cpfFile = new StringBuilder(list_cpfFiles[CPFGridView.SelectedRows[0].Index]);
                        ReadCPF(cpfFile, observingSatellite_cpfClass);
                        StringBuilder satellite_name = new StringBuilder(); // 위성이름 Get용 Box
                        double epoch_MJD = new double();
                        LoadCPFInfo(observingSatellite_cpfClass, satellite_name, ref epoch_MJD);

                        double[] siteloc = new double[3];
                        GetSiteLocation(Global.laserSite, siteloc);

                        // Set Start Time
                        string[] start_split = ((schedule_startTime.ToUniversalTime()).ToString("yyyy-MM-dd-HH-mm-ss.fff")).Split('-');
                        double[] start_double = new double[start_split.Length];
                        for (int i = 0; i < start_double.Length; i++) { start_double[i] = double.Parse(start_split[i]); }
                        double start_MJD = ConvertGreg2MJD(Global.timeSys, start_double);
                        // Set End Time
                        string[] end_split = (((schedule_endTime.AddSeconds(1)).ToUniversalTime()).ToString("yyyy-MM-dd-HH-mm-ss.fff")).Split('-');
                        double[] end_double = new double[end_split.Length];
                        for (int i = 0; i < end_double.Length; i++) { end_double[i] = double.Parse(end_split[i]); }
                        double end_MJD = ConvertGreg2MJD(Global.timeSys, end_double);
                        // Set Step Size
                        double stepSize = 1.0;
                        // Set File Name
                        StringBuilder fileName = new StringBuilder("CPF_referenceData" + ".txt");

                        // 데이터 파일 생성
                        GenerateCommandFile_CPF(observingSatellite_cpfClass, siteloc, start_MJD, end_MJD, stepSize, fileName, 0);


                        // 데이터 파일 Read
                        try
                        {
                            FileInfo fileInfo = new FileInfo("./Outputs/" + fileName.ToString());
                            if (fileInfo.Exists)
                            {
                                int receive_count = 0;

                                using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                                {
                                    int count = 0;
                                    while (!reader.EndOfStream)
                                    {
                                        count++;
                                        string line = reader.ReadLine();
                                        string[] line_words = line.Split('\t');

                                        if (count >= 12)
                                        {
                                            if (line_words[0].Contains("END Ephemeris"))
                                            {
                                                // 끝
                                                if (receive_count != reference_data[0].Length)
                                                {
                                                    MessageBox.Show("오류 : CPF 데이터 생성 실패");
                                                    return;
                                                }
                                            }
                                            else if (true/*double.Parse(line_words[0]) % 1.0 == 0*/)
                                            {

                                                if (receive_count < reference_data[0].Length)
                                                {
                                                    // 형변환 실패 시, 오류 출력
                                                    reference_data[0][receive_count] = double.Parse(line_words[1]);
                                                    reference_data[1][receive_count] = double.Parse(line_words[2]);
                                                    receive_count++;
                                                }
                                            }
                                        }
                                    }

                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("오류 : CPF 데이터 생성 실패");
                            return;
                        }
                    }
                    else if (select_TLE_CPF == "ephemeris")
                    {
                        int current_index = ephemerisData_dataGridView.SelectedRows[0].Index;
                        reference_data = EphemerisFileToTrackingData(ephemeris_tuples[current_index].Item2, ephemeris_tuples[current_index].Item3,
                            ephemeris_tuples[current_index].Item4, 1.0, 0);
                    }
                    


                    // Reference DataGridView 생성
                    if (reference_list.Count != 0) { reference_list.Clear(); }

                    if (select_TLE_CPF == "tle" || select_TLE_CPF == "cpf")
                    {
                        for (int i = 0; i < reference_data[0].Length; i++)
                        {
                            Reference_Data reference_item = new Reference_Data((schedule_startTime.AddSeconds(i)).ToString("yyyy-MM-dd HH:mm:ss"),
                                reference_data[0][i], reference_data[1][i]);

                            reference_list.Add(reference_item);
                        }
                    }
                    else if (select_TLE_CPF == "ephemeris")
                    {
                        int current_index = ephemerisData_dataGridView.SelectedRows[0].Index;

                        for (int i = 0; i < reference_data[0].Length; i++)
                        {
                            Reference_Data reference_item = new Reference_Data((ephemeris_tuples[current_index].Item3.AddSeconds(i).ToLocalTime()).ToString("yyyy-MM-dd HH:mm:ss"),
                                reference_data[0][i], reference_data[1][i]);

                            reference_list.Add(reference_item);
                        }
                    }

                    this.Invoke(new System.Action(() =>
                    {
                        if (reference_dataGridView.DataSource != null)
                        {
                            reference_dataGridView.Columns.Clear();
                            reference_dataGridView.DataSource = null;
                        }
                        reference_dataGridView.DataSource = reference_list;
                        reference_dataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        reference_dataGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        reference_dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        reference_dataGridView.ClearSelection();

                        createReference_btn.Text = "Reference Data\r\n생성 (초단위)";
                        createReference_btn.Enabled = true;
                        createReference_btn.Update();
                    }));
                });
                observingSchedule_thread.Start();

            }


        }

        int reference_startIndex = -1;
        private void startReference_btn_Click(object sender, EventArgs e)   // Start Time 설정
        {
            // 작업 진행 중 일때 (예외처리)
            if (observingSchedule_thread != null && observingSchedule_thread.IsAlive)
            {
                MessageBox.Show("작업 진행 중 입니다. 잠시 후 시도하세요.");
                return;
            }

            if (reference_list.Count == 0)
            {
                MessageBox.Show("Reference Data를 생성하세요.");
                return;
            }

            if (reference_dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("설정할 Start Time을 선택하세요.");
                return;
            }

            startReference_txtbox.Text = reference_dataGridView.SelectedRows[0].Cells[0].Value.ToString();
            reference_startIndex = reference_dataGridView.SelectedRows[0].Index;
            reference_dataGridView.ClearSelection();
        }

        int reference_endIndex = -1;
        private void endReference_btn_Click(object sender, EventArgs e) // End Time 설정
        {
            // 작업 진행 중 일때 (예외처리)
            if (observingSchedule_thread != null && observingSchedule_thread.IsAlive)
            {
                MessageBox.Show("작업 진행 중 입니다. 잠시 후 시도하세요.");
                return;
            }

            if (reference_list.Count == 0)
            {
                MessageBox.Show("Reference Data를 생성하세요.");
                return;
            }

            if (String.IsNullOrEmpty(startReference_txtbox.Text))
            {
                MessageBox.Show("Start Time을 설정하세요.");
                return;
            }

            if (reference_dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("설정할 End Time을 선택하세요.");
                return;
            }

            if (reference_startIndex >= reference_dataGridView.SelectedRows[0].Index)
            {
                MessageBox.Show("설정할 수 없는 End Time 입니다.");
                return;
            }

            endReference_txtbox.Text = reference_dataGridView.SelectedRows[0].Cells[0].Value.ToString();
            reference_endIndex = reference_dataGridView.SelectedRows[0].Index;
            reference_dataGridView.ClearSelection();
        }

        public void Initialize_Reference()  // Reference Data 관련요소 초기화
        {
            // Thread 초기화
            if (observingSchedule_thread != null)
            {
                if (observingSchedule_thread.IsAlive) { observingSchedule_thread.Abort(); }
                observingSchedule_thread = null;
            }

            // 선택된 위성 label 초기화
            selectedSatellite_label.Text = "-";

            // Reference List 초기화
            if (reference_list.Count != 0) { reference_list.Clear(); }

            // Reference DataGridView 초기화
            if (reference_dataGridView.DataSource != null)
            {
                reference_dataGridView.Columns.Clear();
                reference_dataGridView.DataSource = null;
            }

            // Reference Data 생성 버튼 초기화
            createReference_btn.Text = "Reference Data\r\n생성 (초단위)";
            createReference_btn.Enabled = true;
            createReference_btn.Update();

            // Reference Start/End Time 초기화
            reference_startIndex = -1;
            reference_endIndex = -1;
            startReference_txtbox.Clear();
            endReference_txtbox.Clear();

            // ProgressBar 초기화
            addObservingSchedule_progressBar.Value = 0;

            // ProgressState 초기화
            progressState_label.Text = "State Message";
        }

        public void Initialize_ObservingSchedule()  // 관측 스케줄 관련요소 초기화
        {
            // Gantt Chart 초기화
            trackingManager = new ProjectManager();

            trackingManager.Start = schedule_startTime;
            trackingChart.Init(trackingManager);
            trackingChart.Invalidate();

            mySchedules.Clear();

            // 관측위성 객체(ObservingSchedule_Information) 초기화
            observingSchedule_informations.Clear();

            // ComboBox 초기화
            observingSchedule_comboBox.Items.Clear();

        }

        Thread observingSchedule_thread;
        List<MyTask> mySchedules = new List<MyTask>();
        public static List<ObservingSchedule_Information> observingSchedule_informations = new List<ObservingSchedule_Information>();
        private void addObservingSchedule_btn_Click(object sender, EventArgs e) // 관측 스케줄 추가
        {
            // 작업 진행 중 일때 (예외처리)
            if (observingSchedule_thread != null && observingSchedule_thread.IsAlive)
            {
                MessageBox.Show("작업 진행 중 입니다. 잠시 후 시도하세요.");
                return;
            }

            if (selectedSatellite_label.Text == "-")
            {
                MessageBox.Show("위성을 선택하세요.");
                return;
            }
            if (reference_list.Count == 0)
            {
                MessageBox.Show("Reference Data를 생성하세요.");
                return;
            }
            if (String.IsNullOrEmpty(startReference_txtbox.Text))
            {
                MessageBox.Show("Start Time을 설정하세요.");
                return;
            }
            if (String.IsNullOrEmpty(endReference_txtbox.Text))
            {
                MessageBox.Show("End Time을 선택하세요.");
                return;
            }

            // 이름 / startTime / endTime setting
            string name_str = selectedSatellite_label.Text;
            string start_str = startReference_txtbox.Text;
            string end_str = endReference_txtbox.Text;
            DateTime start_dateTime = new DateTime(int.Parse(start_str.Substring(0, 4)), int.Parse(start_str.Substring(5, 2)), int.Parse(start_str.Substring(8, 2)),
                int.Parse(start_str.Substring(11, 2)), int.Parse(start_str.Substring(14, 2)), int.Parse(start_str.Substring(17, 2)));
            DateTime end_dateTime = new DateTime(int.Parse(end_str.Substring(0, 4)), int.Parse(end_str.Substring(5, 2)), int.Parse(end_str.Substring(8, 2)),
                int.Parse(end_str.Substring(11, 2)), int.Parse(end_str.Substring(14, 2)), int.Parse(end_str.Substring(17, 2)));

            // 위성 추적 스케줄러 > 중복 Check
            for (int i = 0; i < observingSchedule_informations.Count; i++)
            {
                // (이전 위성 <> 다음 위성 사이의 최소 공백시간 : 1분)
                if ((DateTime.Compare(end_dateTime.ToUniversalTime(), observingSchedule_informations[i].observingSchedule_startTime.AddSeconds(-60)) == -1 || DateTime.Compare(end_dateTime.ToUniversalTime(), observingSchedule_informations[i].observingSchedule_startTime.AddSeconds(-60)) == 0) ||
                    (DateTime.Compare(observingSchedule_informations[i].observingSchedule_endTime.AddSeconds(60.02), start_dateTime.ToUniversalTime()) == 0 || DateTime.Compare(observingSchedule_informations[i].observingSchedule_endTime.AddSeconds(60.02), start_dateTime.ToUniversalTime()) == -1))
                {
                    // 스케줄 추가 가능
                }
                else
                {
                    // 스케줄 추가 불가능
                    MessageBox.Show("스케줄 추가 불가 : 시간 중복");
                    return;
                }
            }

            observingSchedule_thread = new Thread(() =>
            {
                // ProgressBar 전시 (1)
                this.Invoke(new System.Action(() =>
                {
                    addObservingSchedule_progressBar.Value = 0;
                    progressState_label.Text = "관측 데이터 생성 중...";
                }));

                // 마운트용 RaDec 추출
                double[][] RaDec_data = new double[2][];
                double interval_seconds = 0.02;
                int data_size = (int)((end_dateTime - start_dateTime).TotalSeconds) * (int)(1 / interval_seconds);
                for (int i = 0; i < RaDec_data.Length; i++) { RaDec_data[i] = new double[data_size]; }

                // RGG용 Tof 추출
                List<long> interpolated_UtcMs = new List<long>();
                List<double> interpolated_ToF = new List<double>();

                DateTime start_UTC = start_dateTime.ToUniversalTime();
                DateTime end_UTC = (end_dateTime.AddSeconds(-1 * interval_seconds)).ToUniversalTime();

                if (select_TLE_CPF == "tle")
                {
                    StringBuilder line1 = new StringBuilder(TLE_Information[name_str].Line1);
                    StringBuilder line2 = new StringBuilder(TLE_Information[name_str].Line2);

                    double[] epochTime = new double[6];
                    GetTLEData(observingSatellite_tleClass, line1, line2, epochTime);

                    double[] siteloc = new double[3];
                    GetSiteLocation(Global.laserSite, siteloc);

                    
                    double currentMJD;
                    double[] result = new double[2];

                    // ProgressBar 전시 (2)
                    this.Invoke(new System.Action(() =>
                    {
                        addObservingSchedule_progressBar.Minimum = 0;
                        addObservingSchedule_progressBar.Maximum = data_size;
                        addObservingSchedule_progressBar.Step = 1;
                        addObservingSchedule_progressBar.Value = 0;
                    }));

                    for (int i = 0; i < data_size; i++)
                    {
                        string[] split_time = ((start_UTC.AddSeconds(interval_seconds * i)).ToString("yyyy-MM-dd-HH-mm-ss.fff")).Split('-');
                        double[] double_time = new double[6];
                        for (int j = 0; j < double_time.Length; j++) { double_time[j] = double.Parse(split_time[j]); }

                        currentMJD = ConvertGreg2MJD(Global.timeSys, double_time);
                        GenerateCommand_TLE(observingSatellite_tleClass, siteloc, currentMJD, 2, result);  // RaDec (Topo)

                        RaDec_data[0][i] = result[0];
                        RaDec_data[1][i] = result[1];

                        // ProgressBar 전시 (3)
                        if ((i + 1) % (int)(1 / interval_seconds) == 0)
                        {
                            this.Invoke(new System.Action(() => { addObservingSchedule_progressBar.Value = i + 1; }));
                        }
                    }

                    ////////////////////////////////////////////////////////////////////////
                    // TLE 기반 Tof 산출
                    double tof_interval = 0.1; // 100 ms

                    interpolated_ToF = Compute_Light_Travel_time(name_str, start_dateTime.ToUniversalTime(), end_dateTime.ToUniversalTime(), (int)(tof_interval * 1000));

                    for (int i = 0; i < interpolated_ToF.Count; i++)
                    {
                        DateTime interpolated_dateTime = start_UTC.AddSeconds(tof_interval * i);
                        long utcMs = (long)(interpolated_dateTime.TimeOfDay.TotalMilliseconds);
                        interpolated_UtcMs.Add(utcMs);
                    }
                    ////////////////////////////////////////////////////////////////////////
                }
                else if(select_TLE_CPF == "cpf")
                {
                    StringBuilder cpfFile = new StringBuilder(list_cpfFiles[CPFGridView.SelectedRows[0].Index]);
                    ReadCPF(cpfFile, observingSatellite_cpfClass);
                    StringBuilder satellite_name = new StringBuilder(); // 위성이름 Get용 Box
                    double epoch_MJD = new double();
                    LoadCPFInfo(observingSatellite_cpfClass, satellite_name, ref epoch_MJD);

                    double[] siteloc = new double[3];
                    GetSiteLocation(Global.laserSite, siteloc);

                    // Set Start Time
                    string[] start_split = ((start_dateTime.ToUniversalTime()).ToString("yyyy-MM-dd-HH-mm-ss.fff")).Split('-');
                    double[] start_double = new double[start_split.Length];
                    for (int i = 0; i < start_double.Length; i++) { start_double[i] = double.Parse(start_split[i]); }
                    double start_MJD = ConvertGreg2MJD(Global.timeSys, start_double);
                    // Set End Time
                    string[] end_split = ((end_dateTime.ToUniversalTime()).ToString("yyyy-MM-dd-HH-mm-ss.fff")).Split('-');
                    double[] end_double = new double[end_split.Length];
                    for (int i = 0; i < end_double.Length; i++) { end_double[i] = double.Parse(end_split[i]); }
                    double end_MJD = ConvertGreg2MJD(Global.timeSys, end_double);
                    // Set Step Size
                    double stepSize = 0.02;
                    // Set File Name
                    StringBuilder fileName = new StringBuilder("CPF_ObservingData" + ".txt");

                    Thread timer_thread = new Thread(() =>
                    {
                        int timer_count = 0;
                        this.Invoke(new System.Action(() =>
                        {
                            addObservingSchedule_progressBar.Minimum = 0;
                            addObservingSchedule_progressBar.Maximum = (int)((end_dateTime - start_dateTime).TotalMinutes);
                            addObservingSchedule_progressBar.Step = 1;
                            addObservingSchedule_progressBar.Value = timer_count;
                        }));

                        while (true)
                        {
                            Thread.Sleep(1000);

                            timer_count++;
                            this.Invoke(new System.Action(() =>
                            {
                                addObservingSchedule_progressBar.Value = timer_count;
                            }));

                            if (timer_count == (int)((end_dateTime - start_dateTime).TotalMinutes)) { break; }
                        }
                        
                    });
                    timer_thread.Start();
                    // 데이터 파일 생성
                    GenerateCommandFile_CPF(observingSatellite_cpfClass, siteloc, start_MJD, end_MJD, stepSize, fileName, 2);
                    
                    if (timer_thread != null && timer_thread.IsAlive)
                    {
                        timer_thread.Abort();
                        this.Invoke(new System.Action(() =>
                        {
                            addObservingSchedule_progressBar.Value = (int)((end_dateTime - start_dateTime).TotalMinutes);
                        }));
                    }

                    // 데이터 파일 Read
                    try
                    {
                        FileInfo fileInfo = new FileInfo("./Outputs/" + fileName.ToString());
                        if (fileInfo.Exists)
                        {
                            int receive_count = 0;

                            using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                            {
                                int count = 0;
                                while (!reader.EndOfStream)
                                {
                                    count++;
                                    string line = reader.ReadLine();
                                    string[] line_words = line.Split('\t');

                                    if (count >= 12)
                                    {
                                        if (line_words[0].Contains("END Ephemeris"))
                                        {
                                            // 끝
                                            if (receive_count != data_size)
                                            {
                                                MessageBox.Show("오류 : CPF 데이터 생성 실패");
                                                return;
                                            }
                                        }
                                        else if (true/*double.Parse(line_words[0]) % 1.0 == 0*/)
                                        {

                                            if (receive_count < data_size)
                                            {
                                                // 형변환 실패 시, 오류 출력
                                                RaDec_data[0][receive_count] = double.Parse(line_words[1]);
                                                RaDec_data[1][receive_count] = double.Parse(line_words[2]);
                                                receive_count++;
                                            }
                                        }
                                    }
                                }

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("오류 : CPF 데이터 생성 실패");
                        return;
                    }

                    ////////////////////////////////////////////////////////////////////////
                    // CPF 기반 Tof 산출
                    double tof_interval = 0.1; // 100 ms

                    interpolated_ToF = Compute_Light_Travel_time_CPF(name_str, start_dateTime.ToUniversalTime(), end_dateTime.ToUniversalTime(), (int)(tof_interval * 1000));

                    for (int i = 0; i < interpolated_ToF.Count; i++)
                    {
                        DateTime interpolated_dateTime = start_UTC.AddSeconds(tof_interval * i);
                        long utcMs = (long)(interpolated_dateTime.TimeOfDay.TotalMilliseconds);
                        interpolated_UtcMs.Add(utcMs);
                    }
                    ////////////////////////////////////////////////////////////////////////
                }
                else if (select_TLE_CPF == "ephemeris") // Tracking Data로 추출해야하지만, 진행율 전시를 위해 Pointing Data로 추출
                {
                    double[] siteloc = new double[3];
                    GetSiteLocation(Global.laserSite, siteloc);

                    // ProgressBar 전시 (2)
                    this.Invoke(new System.Action(() =>
                    {
                        addObservingSchedule_progressBar.Minimum = 0;
                        addObservingSchedule_progressBar.Maximum = data_size;
                        addObservingSchedule_progressBar.Step = 1;
                        addObservingSchedule_progressBar.Value = 0;
                    }));

                    double target_mjd;
                    double[] result = new double[2];

                    for (int i = 0; i < data_size; i++)
                    {
                        DateTime target_UTC = start_UTC.AddSeconds(interval_seconds * i);
                        result = EphemerisFileToPointingData(ephemeris_tuples[ephemerisData_dataGridView.SelectedRows[0].Index].Item2, target_UTC, 2);

                        RaDec_data[0][i] = result[0];
                        RaDec_data[1][i] = result[1];

                        // ProgressBar 전시 (3)
                        if ((i + 1) % (int)(1 / interval_seconds) == 0)
                        {
                            this.Invoke(new System.Action(() => { addObservingSchedule_progressBar.Value = i + 1; }));
                        }
                    }

                    ////////////////////////////////////////////////////////////////////////
                    // Ephemeris 기반 Tof 산출
                    double tof_interval = 0.1; // 100ms

                    interpolated_ToF = Compute_Light_Travel_time_Ephemeris(name_str, start_dateTime.ToUniversalTime(), end_dateTime.ToUniversalTime(), (int)(tof_interval * 1000));

                    for (int i = 0; i < interpolated_ToF.Count; i++)
                    {
                        DateTime interpolated_dateTime = start_UTC.AddSeconds(tof_interval * i);
                        long utcMs = (long)(interpolated_dateTime.TimeOfDay.TotalMilliseconds);
                        interpolated_UtcMs.Add(utcMs);
                    }
                    ////////////////////////////////////////////////////////////////////////
                }



                ObservingSchedule_Information observingSchedule_information = new ObservingSchedule_Information();
                observingSchedule_information.ObservingSchedule_SettingData(name_str, start_UTC, end_UTC, interval_seconds);
                observingSchedule_information.ObservingSchedule_SettingRaDec(RaDec_data);
                observingSchedule_information.ObservingSchedule_SettingTof(interpolated_UtcMs, interpolated_ToF);   // Ephemeris File 기반 Tof 생성은 구현 예정

                // LDAP용 Reference 데이터 (궤도정보 종류 + 궤도정보 파일 + 위성번호)
                string orbit_type = select_TLE_CPF;
                string orbit_source = "";
                int norad_id = 0;
                if (orbit_type == "tle")
                {
                    orbit_source = name_str;

                    // TLE 기반 norad id 확보
                    norad_id = TLE_Information[name_str].Norad;
                }
                else if (orbit_type == "cpf")
                {
                    orbit_source = list_cpfFiles[CPFGridView.SelectedRows[0].Index];

                    // CPF 기반 norad id 확보
                    FileInfo fileInfo = new FileInfo(orbit_source);
                    if (fileInfo.Exists)
                    {
                        using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                        {
                            while (!reader.EndOfStream)
                            {
                                string line = reader.ReadLine();
                                string[] line_words = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                                if (line_words[0] == "h2" || line_words[0] == "H2")
                                {
                                    norad_id = int.Parse(line_words[3]);
                                    break;
                                }
                            }

                        }

                    }

                }
                else if (orbit_type == "ephemeris")
                {
                    orbit_source = ephemeris_tuples[ephemerisData_dataGridView.SelectedRows[0].Index].Item2;

                    // Ephemeris 기반 norad id 확보
                    string ephemeris_name = ephemeris_tuples[ephemerisData_dataGridView.SelectedRows[0].Index].Item1;
                    norad_id = TLE_Information[ephemeris_name].Norad;

                }

                observingSchedule_information.ObservingSchedule_SettingOrbit(orbit_type, orbit_source, norad_id);

                observingSchedule_information.available_flag = true;
                observingSchedule_informations.Add(observingSchedule_information);

                // ObservingSchedule_Information List 정렬
                observingSchedule_informations.Sort((schedule1, schedule2) => schedule1.observingSchedule_startTime.CompareTo(schedule2.observingSchedule_startTime));

                ///////////////////////////////////////////////////////////////////

                this.Invoke(new System.Action(() =>
                {
                    // Gantt Chart 추가
                    TimeSpan startSpan = start_dateTime - schedule_startTime;
                    TimeSpan durationSpan = end_dateTime - start_dateTime;

                    MyTask mySchedule = new MyTask(trackingManager) { Name = name_str };
                    
                    mySchedules.Add(mySchedule);
                    trackingManager.Add(mySchedule);
                    trackingManager.SetStart(mySchedule, TimeSpan.FromMinutes(startSpan.TotalMinutes));
                    trackingManager.SetDuration(mySchedule, TimeSpan.FromMinutes(durationSpan.TotalMinutes));

                    //trackingChart.Init(trackingManager);
                    //trackingChart.Invalidate();

                    //observingSchedule_comboBox.Items.Add(mySchedule.Name);

                    // mySchedules + trackingManager/trackingChart+ ComboBox 정렬 ////////////
                    mySchedules.Sort((mySchedule1, mySchedule2) => mySchedule1.Start.CompareTo(mySchedule2.Start));

                    Initialize_TrackingChart();
                    for(int i = 0; i < mySchedules.Count; i++)
                    {
                        trackingManager.Add(mySchedules[i]);
                    }
                    trackingChart.Init(trackingManager);
                    trackingChart.Invalidate();

                    observingSchedule_comboBox.Items.Clear();
                    for (int i = 0; i < mySchedules.Count; i++) { observingSchedule_comboBox.Items.Add(mySchedules[i].Name); }
                    ///////////////////////////////////////////////////////////////////////////
                    
                    progressState_label.Text = "관측 데이터 생성 완료";
                }));
                
            });

            observingSchedule_thread.Start();

        }

        Thread lookup_thread;
        private void CompletedSchedule_btn_Click(object sender, EventArgs e)    // 위성 추적 스케줄러 적용 (LookUp Table 전송)
        {
            // 1. RGG_SAT_Controller 및 RGG_DEB_Controller 접근
            // 2. Start2SendLUT 함수 호출 부터 시작
            // 고려사항 : 스케줄링된 모든 위성에 대한 Tof를 보내줘야함, 어떻게 보낼지는 담당자와 협의 후 진행

            if (lookup_thread != null)
            {
                if (lookup_thread.IsAlive)
                {
                    MessageBox.Show("RGG LookUp Table 전송 중입니다." + "\r\n" + "잠시 후 시도하세요!");
                    return;
                }
                lookup_thread = null;
            }

            if (observingSchedule_informations.Count == 0)
            {
                MessageBox.Show("관측 스케줄을 추가하세요.");
                return;
            }

            lookup_thread = new Thread(() =>
            {
                Send_LookUpTable();
            });
            lookup_thread.Start();

        }

        public async System.Threading.Tasks.Task Send_LookUpTable()
        {
            // 전송 시작 표시
            this.Invoke(new System.Action(() =>
            {
                CompletedSchedule_btn.Text = "LoopUp Table 전송 중...";
                CompletedSchedule_btn.Enabled = false;
                CompletedSchedule_btn.Update();
            }));

            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            {
                rggSATcontrol.Set_LUT_Clear();  // LUT을 전송하기 전, RGG의 LUT를 지우기
                await rggSATcontrol.Start2SendLUT_Schedule();
            }
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                rggDEBcontrol.Set_LUT_Clear();  // LUT을 전송하기 전, RGG의 LUT를 지우기
                await rggDEBcontrol.Start2SendLUT_Schedule();
            }

            // 전송 종료 표시
            this.Invoke(new System.Action(() =>
            {
                CompletedSchedule_btn.Text = "스케줄러 적용" + "\r\n" + "(LookUp Table 전송)";
                CompletedSchedule_btn.Enabled = true;
                CompletedSchedule_btn.Update();
            }));
        }

        private void checkingRaDec_btn_Click(object sender, EventArgs e) // Ra/Dec Check
        {
            // 작업 진행 중 일때 (예외처리)
            if (observingSchedule_thread != null && observingSchedule_thread.IsAlive)
            {
                MessageBox.Show("작업 진행 중 입니다. 잠시 후 시도하세요.");
                return;
            }

            if (observingSchedule_comboBox.SelectedIndex != -1)
            {
                // 데이터 전시
                Checking_OvservingData checking_ovservingData = new Checking_OvservingData(
                    (observingSchedule_informations[observingSchedule_comboBox.SelectedIndex].observingSchedule_startTime).ToLocalTime(),
                    observingSchedule_informations[observingSchedule_comboBox.SelectedIndex].observingSchedule_interval,
                    observingSchedule_informations[observingSchedule_comboBox.SelectedIndex].observingSchedule_RaDec);
                checking_ovservingData.ShowDialog();
            }
            else
            {
                MessageBox.Show("관측 스케줄을 선택하세요!");
            }

            
        }

        private void deleteObservingSchedule_btn_Click(object sender, EventArgs e) // 관측 스케줄 삭제
        {
            // 작업 진행 중 일때 (예외처리)
            if (observingSchedule_thread != null && observingSchedule_thread.IsAlive)
            {
                MessageBox.Show("작업 진행 중 입니다. 잠시 후 시도하세요.");
                return;
            }

            if (observingSchedule_comboBox.SelectedIndex != -1)
            {
                if (MessageBox.Show(observingSchedule_comboBox.SelectedItem.ToString() + " 관측 스케줄을 삭제하겠습니까?", "삭 제", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // Gantt Chart 삭제
                    trackingManager.Delete(mySchedules[observingSchedule_comboBox.SelectedIndex]);
                    trackingChart.Init(trackingManager);
                    trackingChart.Invalidate();

                    mySchedules.RemoveAt(observingSchedule_comboBox.SelectedIndex);

                    // 관측위성 객체(RaDec 포함) 삭제
                    observingSchedule_informations.RemoveAt(observingSchedule_comboBox.SelectedIndex);

                    // ComboBox 삭제
                    observingSchedule_comboBox.Items.RemoveAt(observingSchedule_comboBox.SelectedIndex);
                }
                
            }
            else
            {
                MessageBox.Show("관측 스케줄을 선택하세요!");
            }

        }

        private void SendingLDAP_btn_Click(object sender, EventArgs e)  // 관측 데이터 저장 (LDAP)
        {
            // 작업 진행 중 일때 (예외처리)
            if (observingSchedule_thread != null && observingSchedule_thread.IsAlive)
            {
                MessageBox.Show("작업 진행 중 입니다. 잠시 후 시도하세요.");
                return;
            }

            // ObservingSchedule Information 데이터 체크
            foreach (var schedule in observingSchedule_informations)
            {
                if (schedule.orbit_type == "" || schedule.orbit_source == "" || schedule.norad_id == 0)
                {
                    MessageBox.Show("관측 데이터 저장 불가" + "\r\n" + "(데이터 유효성 검사 : Fail)");
                }
            }

            SendingLDAP_btn.Text = "저장 중...";
            SendingLDAP_btn.Enabled = false;
            SendingLDAP_btn.Update();

            observingSchedule_thread = new Thread(() =>
            {
                // Create Schedule Table
                using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
                {
                    try
                    {
                        connect.Open();
                        using (NpgsqlCommand command = new NpgsqlCommand())
                        {
                            command.Connection = connect;


                            // Schedule Table Check, 없을 시 생성
                            bool exist_scheduleTable = false;

                            command.CommandText = "SELECT * FROM pg_catalog.pg_tables WHERE schemaname = 'public';";

                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader["tablename"].ToString() == "nslr_scheduleinformation") { exist_scheduleTable = true; }
                                }

                            }

                            if (exist_scheduleTable == false)
                            {
                                command.CommandText = "CREATE TABLE nslr_scheduleinformation (              " +
                                                      "         number                  integer,            " +
                                                      "         orbit_type              varchar(50),        " +
                                                      "         orbit_source            varchar(200),        " +
                                                      "         satellite               varchar(50),        " +
                                                      "         norad_id                integer,            " +
                                                      "         start_time              varchar(50),        " +
                                                      "         end_time                varchar(50),        " +
                                                      "         daily_table             varchar(200),       " +
                                                      "         tracking_table          varchar(200),       " +
                                                      "         status_table            varchar(200)        " +
                                                      ");                                                   ";
                                command.ExecuteNonQuery();
                            }

                            // Daily + Tracking 테이블 삭제
                            command.CommandText = "SELECT * FROM nslr_scheduleinformation;";

                            string drop_command = "";
                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string daily_table = reader["daily_table"].ToString();
                                    string tracking_table = reader["tracking_table"].ToString();
                                    string status_table = reader["status_table"].ToString();

                                    drop_command += "DROP TABLE " + daily_table + ";";
                                    drop_command += "DROP TABLE " + tracking_table + ";";
                                    drop_command += "DROP TABLE " + status_table + ";";
                                }
                            }

                            if (drop_command != "")
                            {
                                command.CommandText = String.Format(drop_command);
                                command.ExecuteNonQuery();
                            }

                            // Information 항목 삭제
                            command.CommandText = String.Format("DELETE FROM nslr_scheduleinformation;");
                            command.ExecuteNonQuery();


                            int num = 0;
                            foreach (var schedule in observingSchedule_informations)
                            {
                                num++;
                                string daily_table = "nslr_scheduledaily_" + num.ToString();
                                string tracking_table = "nslr_scheduletracking_" + num.ToString();
                                string status_table = "nslr_schedulestatus_" + num.ToString();

                                // Schedule Information 입력
                                command.CommandText = String.Format("INSERT INTO nslr_scheduleinformation VALUES ({0}, '{1}', '{2}', '{3}', {4}, " +
                                    "'{5}', '{6}', " +
                                    "'{7}', '{8}', '{9}');",
                                    num, schedule.orbit_type, schedule.orbit_source, schedule.observingSchedule_name, schedule.norad_id,
                                    schedule.observingSchedule_startTime.ToString("yyyy-MM-dd HH:mm:ss"), (schedule.observingSchedule_endTime.AddSeconds(schedule.observingSchedule_interval)).ToString("yyyy-MM-dd HH:mm:ss"),
                                    daily_table, tracking_table, status_table);
                                command.ExecuteNonQuery();

                                // Schedule Daily 테이블 생성 + 데이터 입력 ////////////////////////////////////////
                                command.CommandText = "CREATE TABLE " + daily_table + " (                   " +
                                                      "         norad_id                integer,            " +
                                                      "         height                  double precision,   " +
                                                      "         datetime                varchar(50),        " +
                                                      "         longitude               double precision,   " +
                                                      "         latitude                double precision,   " +
                                                      "         distance                double precision    " +
                                                      ");                                                   ";
                                command.ExecuteNonQuery();

                                DateTime utcNow = DateTime.UtcNow;

                                // LLA 및 Distance(LDAP용) 생성
                                double[][] daily_data = Generate_LDAP_LLA_Distance(schedule.orbit_type, schedule.orbit_source, utcNow, utcNow.AddHours(24), 3600.0);

                                for (int i = 0; i < daily_data[0].Length; i++)
                                {
                                    command.CommandText = String.Format("INSERT INTO " + daily_table + " VALUES ({0}, {1}, '{2}', {3}, {4}, {5});",
                                        schedule.norad_id, daily_data[2][i], (utcNow.AddHours(i)).ToString("yyyy-MM-dd HH:mm:ss"),
                                        daily_data[0][i], daily_data[1][i], daily_data[3][i]);

                                    command.ExecuteNonQuery();
                                }


                                // Schedule Tracking 테이블 생성 + 데이터 입력 /////////////////////////////////////
                                command.CommandText = "CREATE TABLE " + tracking_table + " (                " +
                                                      "         norad_id               integer,             " +
                                                      "         height                 double precision,    " +
                                                      "         datetime               varchar(50),         " +
                                                      "         ra                     double precision,    " +
                                                      "         dec                    double precision,    " +
                                                      "         distance               double precision     " +
                                                      ");                                                   ";
                                command.ExecuteNonQuery();


                                // RaDec + LLA(LDAP용) 생성
                                double[][] tracking_data1 = Generate_LDAP_RaDec(schedule.orbit_type, schedule.orbit_source,
                                    schedule.observingSchedule_startTime, schedule.observingSchedule_endTime.AddSeconds(schedule.observingSchedule_interval), 0.02);
                                double[][] tracking_data2 = Generate_LDAP_LLA_Distance(schedule.orbit_type, schedule.orbit_source,
                                    schedule.observingSchedule_startTime, schedule.observingSchedule_endTime.AddSeconds(schedule.observingSchedule_interval), 0.02);

                                for (int i = 0; i < tracking_data1[0].Length; i++)
                                {
                                    command.CommandText = String.Format("INSERT INTO " + tracking_table + " VALUES ({0}, {1}, '{2}', {3}, {4}, {5});",
                                        schedule.norad_id, tracking_data2[2][i],
                                        (schedule.observingSchedule_startTime.AddSeconds(i * schedule.observingSchedule_interval)).ToString("yyyy-MM-dd HH:mm:ss.ff"),
                                        tracking_data1[0][i], tracking_data1[1][i], tracking_data2[3][i]);

                                    command.ExecuteNonQuery();
                                }

                                // Schedule Status 테이블 생성 + 데이터 입력 //////////////////////////////////////
                                command.CommandText = "CREATE TABLE " + status_table + " (                  " +
                                                      "         norad_id               integer,             " +
                                                      "         datetime               varchar(50),         " +
                                                      "         longitude              double precision,    " +
                                                      "         latitude               double precision     " +
                                                      ");                                                   ";
                                command.ExecuteNonQuery();

                                int status_interval = (int)(1.0 / schedule.observingSchedule_interval);
                                int status_length = tracking_data2[0].Length / status_interval;

                                for (int i = 0; i < status_length; i++)
                                {
                                    command.CommandText = String.Format("INSERT INTO " + status_table + " VALUES ({0}, '{1}', {2}, {3});",
                                        schedule.norad_id, schedule.observingSchedule_startTime.AddSeconds(i).ToString("yyyy-MM-dd HH:mm:ss.ff"), 
                                        tracking_data2[0][i * status_interval], tracking_data2[1][i * status_interval]);

                                    command.ExecuteNonQuery();
                                }

                            }

                            connect.Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Schedule Table Create 중 오류발생!");
                    }
                    finally
                    {
                        connect.Close();
                    }
                }


                this.Invoke(new System.Action(() =>
                {
                    SendingLDAP_btn.Text = "관측 데이터 저장 (LDAP)";
                    SendingLDAP_btn.Enabled = true;
                    SendingLDAP_btn.Update();
                }));

            });

            observingSchedule_thread.Start();

        }

        public double[][] Generate_LDAP_LLA_Distance(string orbit_type, string orbit_source, DateTime start, DateTime end, double interval)
        {
            // Input : 궤도종류, 참조정보, 시작시간, 종료시간, 시간간격 / Output : 경도, 위도, 고도, 거리

            TimeSpan diff = end - start;
            double total_seconds = diff.TotalSeconds;
            int size = (int)(total_seconds / interval);

            double[][] result_data = new double[4][];
            for (int i = 0; i < result_data.Length; i++) { result_data[i] = new double[size]; }

            if (orbit_type == "tle")    // TLE 기반
            {
                IntPtr ldap_tleClass = CreateTLE();
                StringBuilder line1 = new StringBuilder(TLE_Information[orbit_source].Line1);
                StringBuilder line2 = new StringBuilder(TLE_Information[orbit_source].Line2);
                double[] epoch_time = new double[6];
                GetTLEData(ldap_tleClass, line1, line2, epoch_time);

                double currentMJD;
                double[] result = new double[3];

                for (int i = 0; i < size; i++)
                {
                    string[] split_time = (start.AddSeconds(i * interval)).ToString("yyyy-MM-dd-HH-mm-ss.ff").Split('-');
                    double[] double_time = new double[6];
                    for (int j = 0; j < double_time.Length; j++) { double_time[j] = double.Parse(split_time[j]); }

                    currentMJD = ConvertGreg2MJD(Global.timeSys, double_time);
                    GenerateLLA_TLE(ldap_tleClass, currentMJD, result);

                    result_data[0][i] = result[0];  // 경도
                    result_data[1][i] = result[1];  // 위도
                    result_data[2][i] = result[2];  // 고도
                    result_data[3][i] = CalculateDistance(result[0], result[1], result[2]); // 거리
                }

            }
            else if (orbit_type == "cpf")   // CPF 기반
            {
                IntPtr ldap_cpfClass = CreateCPF();
                StringBuilder cpfFile = new StringBuilder(orbit_source);
                ReadCPF(cpfFile, ldap_cpfClass);
                StringBuilder satellite_name = new StringBuilder();
                double epoch_MJD = new double();
                LoadCPFInfo(ldap_cpfClass, satellite_name, ref epoch_MJD);

                string[] start_split = (start.ToString("yyyy-MM-dd-HH-mm-ss.ff")).Split('-');
                double[] start_double = new double[start_split.Length];
                for (int i = 0; i < start_double.Length; i++) { start_double[i] = double.Parse(start_split[i]); }
                double start_MJD = ConvertGreg2MJD(Global.timeSys, start_double);

                string[] end_split = (end.ToString("yyyy-MM-dd-HH-mm-ss.ff")).Split('-');
                double[] end_double = new double[end_split.Length];
                for (int i = 0; i < end_double.Length; i++) { end_double[i] = double.Parse(end_split[i]); }
                double end_MJD = ConvertGreg2MJD(Global.timeSys, end_double);

                double stepSize = interval;
                StringBuilder fileName = new StringBuilder("CPF_LDAPData" + ".txt");

                // 데이터 파일 생성
                GenerateLLAFile_CPF(ldap_cpfClass, start_MJD, end_MJD, stepSize, fileName);

                // 데이터 파일 읽기
                FileInfo fileInfo = new FileInfo("./Outputs/" + fileName.ToString());
                if (fileInfo.Exists)
                {
                    int receive_count = 0;

                    using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                    {
                        int count = 0;
                        while (!reader.EndOfStream)
                        {
                            count++;
                            string line = reader.ReadLine();
                            string[] line_words = line.Split('\t');

                            if (count >= 12)
                            {
                                if (line_words[0].Contains("END Ephemeris"))
                                {
                                    // 끝
                                    if (receive_count != size)
                                    {
                                        MessageBox.Show("오류 : CPF 데이터 생성 실패");
                                    }
                                }
                                else if (receive_count < size)
                                {
                                    result_data[0][receive_count] = double.Parse(line_words[1]);
                                    result_data[1][receive_count] = double.Parse(line_words[2]);
                                    result_data[2][receive_count] = double.Parse(line_words[3]);
                                    result_data[3][receive_count] = CalculateDistance(double.Parse(line_words[1]), double.Parse(line_words[2]), double.Parse(line_words[3]));
                                    receive_count++;
                                }
                            }
                        }

                    }

                }

            }
            else if (orbit_type == "ephemeris") // Ephemeris 기반
            {
                StringBuilder ephemerisFile = new StringBuilder(orbit_source);

                string[] start_split = (start.ToString("yyyy-MM-dd-HH-mm-ss.ff")).Split('-');
                double[] start_double = new double[start_split.Length];
                for (int i = 0; i < start_double.Length; i++) { start_double[i] = double.Parse(start_split[i]); }
                double start_MJD = ConvertGreg2MJD(Global.timeSys, start_double);

                string[] end_split = (end.ToString("yyyy-MM-dd-HH-mm-ss.ff")).Split('-');
                double[] end_double = new double[end_split.Length];
                for (int i = 0; i < end_double.Length; i++) { end_double[i] = double.Parse(end_split[i]); }
                double end_MJD = ConvertGreg2MJD(Global.timeSys, end_double);

                double stepSize = interval;
                StringBuilder fileName = new StringBuilder("Ephemeris_LDAPData" + ".txt");

                // 데이터 파일 생성
                GenerateLLAFile_Ephemeris(ephemerisFile, start_MJD, end_MJD, stepSize, fileName);

                // 데이터 파일 읽기
                FileInfo fileInfo = new FileInfo("./Outputs/" + fileName.ToString());
                if (fileInfo.Exists)
                {
                    int receive_count = 0;

                    using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                    {
                        int count = 0;
                        while (!reader.EndOfStream)
                        {
                            count++;
                            string line = reader.ReadLine();
                            string[] line_words = line.Split('\t');

                            if (count >= 12)
                            {
                                if (line_words[0].Contains("END Ephemeris"))
                                {
                                    // 끝
                                    if (receive_count != size)
                                    {
                                        MessageBox.Show("오류 : Ephemeris 데이터 생성 실패");
                                    }
                                }
                                else if (receive_count < size)
                                {
                                    result_data[0][receive_count] = double.Parse(line_words[1]);
                                    result_data[1][receive_count] = double.Parse(line_words[2]);
                                    result_data[2][receive_count] = double.Parse(line_words[3]);
                                    result_data[3][receive_count] = CalculateDistance(double.Parse(line_words[1]), double.Parse(line_words[2]), double.Parse(line_words[3]));
                                    receive_count++;
                                }
                            }
                        }

                    }

                }

            }

            return result_data;
            
        }

        public double[][] Generate_LDAP_RaDec(string orbit_type, string orbit_source, DateTime start, DateTime end, double interval)
        {
            // Input : 궤도종류, 참조정보, 시작시간, 종료시간, 시간간격 / Output : RaDec

            TimeSpan diff = end - start;
            double total_seconds = diff.TotalSeconds;
            int size = (int)(total_seconds / interval);

            double[] siteloc = new double[3] { LDAP_longitude, LDAP_latitude, LDAP_altitude };
            int opt = 2;

            double[][] result_data = new double[2][];
            for (int i = 0; i < result_data.Length; i++) { result_data[i] = new double[size]; }

            if (orbit_type == "tle")    // TLE 기반
            {
                IntPtr ldap_tleClass = CreateTLE();
                StringBuilder line1 = new StringBuilder(TLE_Information[orbit_source].Line1);
                StringBuilder line2 = new StringBuilder(TLE_Information[orbit_source].Line2);
                double[] epoch_time = new double[6];
                GetTLEData(ldap_tleClass, line1, line2, epoch_time);

                double currentMJD;
                double[] result = new double[2];

                for (int i = 0; i < size; i++)
                {
                    string[] split_time = (start.AddSeconds(i * interval)).ToString("yyyy-MM-dd-HH-mm-ss.ff").Split('-');
                    double[] double_time = new double[6];
                    for (int j = 0; j < double_time.Length; j++) { double_time[j] = double.Parse(split_time[j]); }

                    currentMJD = ConvertGreg2MJD(Global.timeSys, double_time);
                    GenerateCommand_TLE(ldap_tleClass, siteloc, currentMJD, opt, result);

                    result_data[0][i] = result[0];  // Ra
                    result_data[1][i] = result[1];  // Dec
                }

            }
            else if (orbit_type == "cpf")   // CPF 기반
            {
                IntPtr ldap_cpfClass = CreateCPF();
                StringBuilder cpfFile = new StringBuilder(orbit_source);
                ReadCPF(cpfFile, ldap_cpfClass);
                StringBuilder satellite_name = new StringBuilder();
                double epoch_MJD = new double();
                LoadCPFInfo(ldap_cpfClass, satellite_name, ref epoch_MJD);

                string[] start_split = (start.ToString("yyyy-MM-dd-HH-mm-ss.ff")).Split('-');
                double[] start_double = new double[start_split.Length];
                for (int i = 0; i < start_double.Length; i++) { start_double[i] = double.Parse(start_split[i]); }
                double start_MJD = ConvertGreg2MJD(Global.timeSys, start_double);

                string[] end_split = (end.ToString("yyyy-MM-dd-HH-mm-ss.ff")).Split('-');
                double[] end_double = new double[end_split.Length];
                for (int i = 0; i < end_double.Length; i++) { end_double[i] = double.Parse(end_split[i]); }
                double end_MJD = ConvertGreg2MJD(Global.timeSys, end_double);

                double stepSize = interval;
                StringBuilder fileName = new StringBuilder("CPF_LDAPData" + ".txt");

                // 데이터 파일 생성
                GenerateCommandFile_CPF(ldap_cpfClass, siteloc, start_MJD, end_MJD, stepSize, fileName, opt);

                // 데이터 파일 읽기
                FileInfo fileInfo = new FileInfo("./Outputs/" + fileName.ToString());
                if (fileInfo.Exists)
                {
                    int receive_count = 0;

                    using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                    {
                        int count = 0;
                        while (!reader.EndOfStream)
                        {
                            count++;
                            string line = reader.ReadLine();
                            string[] line_words = line.Split('\t');

                            if (count >= 12)
                            {
                                if (line_words[0].Contains("END Ephemeris"))
                                {
                                    // 끝
                                    if (receive_count != size)
                                    {
                                        MessageBox.Show("오류 : CPF 데이터 생성 실패");
                                    }
                                }
                                else if (receive_count < size)
                                {
                                    result_data[0][receive_count] = double.Parse(line_words[1]);
                                    result_data[1][receive_count] = double.Parse(line_words[2]);
                                    receive_count++;
                                }
                            }
                        }

                    }

                }

            }
            else if (orbit_type == "ephemeris") // Ephemeris 기반
            {
                StringBuilder ephemerisFile = new StringBuilder(orbit_source);

                string[] start_split = (start.ToString("yyyy-MM-dd-HH-mm-ss.ff")).Split('-');
                double[] start_double = new double[start_split.Length];
                for (int i = 0; i < start_double.Length; i++) { start_double[i] = double.Parse(start_split[i]); }
                double start_MJD = ConvertGreg2MJD(Global.timeSys, start_double);

                string[] end_split = (end.ToString("yyyy-MM-dd-HH-mm-ss.ff")).Split('-');
                double[] end_double = new double[end_split.Length];
                for (int i = 0; i < end_double.Length; i++) { end_double[i] = double.Parse(end_split[i]); }
                double end_MJD = ConvertGreg2MJD(Global.timeSys, end_double);

                double stepSize = interval;
                StringBuilder fileName = new StringBuilder("Ephemeris_LDAPData" + ".txt");

                // 데이터 파일 생성
                GenerateCommandFile_Ephemeris(ephemerisFile, siteloc, start_MJD, end_MJD, stepSize, fileName, opt);

                // 데이터 파일 읽기
                FileInfo fileInfo = new FileInfo("./Outputs/" + fileName.ToString());
                if (fileInfo.Exists)
                {
                    int receive_count = 0;

                    using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                    {
                        int count = 0;
                        while (!reader.EndOfStream)
                        {
                            count++;
                            string line = reader.ReadLine();
                            string[] line_words = line.Split('\t');

                            if (count >= 12)
                            {
                                if (line_words[0].Contains("END Ephemeris"))
                                {
                                    // 끝
                                    if (receive_count != size)
                                    {
                                        MessageBox.Show("오류 : Ephemeris 데이터 생성 실패");
                                    }
                                }
                                else if (receive_count < size)
                                {
                                    result_data[0][receive_count] = double.Parse(line_words[1]);
                                    result_data[1][receive_count] = double.Parse(line_words[2]);
                                    receive_count++;
                                }
                            }
                        }

                    }

                }

            }


            return result_data;
        }

        const double LDAP_longitude = 127.920050;
        const double LDAP_latitude = 35.590027;
        const double LDAP_altitude = 0.896;
        const double earth_radius = 6371.0;

        private double CalculateDistance(double satLon, double satLat, double satAlt)  // 거리산출 함수 (1)
        {
            // 1. 좌표 변환
            double[] satelliteCartesian = EquatorialToCartesian(satLon, satLat, satAlt);
            double[] groundStationCartesian = GeodeticToCartesian(LDAP_longitude, LDAP_latitude, LDAP_altitude);

            // 2. 거리 계산
            double distance = CalculateDistance(satelliteCartesian, groundStationCartesian);
            return distance;
        }

        private double[] EquatorialToCartesian(double Long, double Lat, double Alt) // 거리산출 함수 (2)
        {
            var Sa = Alt;
            double satelliteRadius = earth_radius + Sa;
            double LongRadian;
            double LatRadian;
            LongRadian = Long * Math.PI / 180;
            LatRadian = Lat * Math.PI / 180;
            double x = satelliteRadius * Math.Cos(LatRadian) * Math.Cos(LongRadian);
            double y = satelliteRadius * Math.Cos(LatRadian) * Math.Sin(LongRadian);
            double z = satelliteRadius * Math.Sin(LatRadian);
            return new double[] { x, y, z };
        }

        private double[] GeodeticToCartesian(double lon, double lat, double alt)    // 거리산출 함수 (3)
        {
            var Sa = earth_radius + alt;
            double x = Sa * Math.Cos(lat * Math.PI / 180) * Math.Cos(lon * Math.PI / 180);
            double y = Sa * Math.Cos(lat * Math.PI / 180) * Math.Sin(lon * Math.PI / 180);
            double z = Sa * Math.Sin(lat * Math.PI / 180);
            return new double[] { x, y, z };
        }

        private double CalculateDistance(double[] point1, double[] point2)  // 거리산출 함수 (4)
        {
            double dx = point2[0] - point1[0];
            double dy = point2[1] - point1[1];
            double dz = point2[2] - point1[2];
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }


        ProjectManager trackingManager = null;
        public void Initialize_TrackingChart()  // Tracking Chart 초기화
        {
            trackingManager = new ProjectManager();

            trackingManager.Start = schedule_startTime;
            trackingChart.Init(trackingManager);
            trackingChart.Invalidate();

            trackingChart.CreateTaskDelegate = delegate () { return new MyTask(trackingManager); };
            trackingChart.AllowTaskDragDrop = false;
            trackingChart.TimeResolution = TimeResolution.Minute;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// << 해당시점부터 궤도결정 코드

        string dataPC_postgresql_connectStr = String.Format("Server={0};Port={1};Database=postgres;User Id={2};password={3}", "192.168.10.13", "5432", "postgres", "1234");
        //string postgresql_connectStr = String.Format("Server=localhost;Port=55432;Database=postgres;User Id={0};password={1}", "postgres", "1234");
        string externalCRD_directory = "./Resources/ExternalCRD";
        string nslrCRD_directory = "./Resources/NSLR_CRD";
        string nslrEphemeris_directory = "./Outputs/Ephemeris";

        private void orbitPage1_btn_Click(object sender, EventArgs e)   // - TLE / CPF - Page 이동
        {
            orbit_tabControl.SelectedIndex = 0;
        }

        private void orbitPage2_btn_Click(object sender, EventArgs e)   // - 궤도결정 - Page 이동
        {
            orbit_tabControl.SelectedIndex = 1;

            // 궤도결정 Table Check
            Check_DatabaseTable();

            // 레이저 관측 데이터 List 업데이트
            Update_ObservationData();

            // CRD File List 업데이트
            Update_CRDFile();

            // Ephemeris File List 업데이트
            Update_EphemerisFile();
        }

        List<ObservationData_Info> ObservationData_List = new List<ObservationData_Info>();
        List<CRDFile_info> CRDFile_List = new List<CRDFile_info>();
        List<EphimerisFile_Info> EphimerisFile_List = new List<EphimerisFile_Info>();
        List<EphemerisData_Info> EphimerisData_List = new List<EphemerisData_Info>();

        public void Initialize_OrbitDetermination_DataGridView()    // 궤도결정 관련 DataGridView 초기화
        {
            // ObservationData_DataGridView 설정
            observationData_dataGridView.RowHeadersVisible = false;
            observationData_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            observationData_dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            observationData_dataGridView.AllowUserToResizeColumns = false;
            observationData_dataGridView.AllowUserToResizeRows = false;
            observationData_dataGridView.EnableHeadersVisualStyles = false;
            observationData_dataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            observationData_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = observationData_dataGridView.ColumnHeadersDefaultCellStyle.BackColor;
            observationData_dataGridView.MultiSelect = false;
            observationData_dataGridView.ReadOnly = true;

            observationData_dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 14, FontStyle.Bold);
            observationData_dataGridView.DefaultCellStyle.Font = new Font("맑은 고딕", 12, FontStyle.Regular);
            observationData_dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            observationData_dataGridView.RowTemplate.Height = 30;
            
            // CRDFile_DataGridView 설정
            CRDFile_dataGridView.RowHeadersVisible = false;
            CRDFile_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            CRDFile_dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            CRDFile_dataGridView.AllowUserToResizeColumns = false;
            CRDFile_dataGridView.AllowUserToResizeRows = false;
            CRDFile_dataGridView.EnableHeadersVisualStyles = false;
            CRDFile_dataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            CRDFile_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = CRDFile_dataGridView.ColumnHeadersDefaultCellStyle.BackColor;
            CRDFile_dataGridView.MultiSelect = false;
            CRDFile_dataGridView.ReadOnly = true;

            CRDFile_dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 14, FontStyle.Bold);
            CRDFile_dataGridView.DefaultCellStyle.Font = new Font("맑은 고딕", 12, FontStyle.Regular);
            CRDFile_dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            CRDFile_dataGridView.RowTemplate.Height = 30;

            // EphimerisFile_DataGridView 설정
            ephimerisFile_dataGridView.RowHeadersVisible = false;
            ephimerisFile_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ephimerisFile_dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ephimerisFile_dataGridView.AllowUserToResizeColumns = false;
            ephimerisFile_dataGridView.AllowUserToResizeRows = false;
            ephimerisFile_dataGridView.EnableHeadersVisualStyles = false;
            ephimerisFile_dataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            ephimerisFile_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = ephimerisFile_dataGridView.ColumnHeadersDefaultCellStyle.BackColor;
            ephimerisFile_dataGridView.MultiSelect = false;
            ephimerisFile_dataGridView.ReadOnly = true;

            ephimerisFile_dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 14, FontStyle.Bold);
            ephimerisFile_dataGridView.DefaultCellStyle.Font = new Font("맑은 고딕", 12, FontStyle.Regular);
            ephimerisFile_dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ephimerisFile_dataGridView.RowTemplate.Height = 30;

            // EphimerisData_DataGridView 설정
            ephemerisData_dataGridView.RowHeadersVisible = false;
            ephemerisData_dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ephemerisData_dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ephemerisData_dataGridView.AllowUserToResizeColumns = false;
            ephemerisData_dataGridView.AllowUserToResizeRows = false;
            ephemerisData_dataGridView.EnableHeadersVisualStyles = false;
            ephemerisData_dataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            ephemerisData_dataGridView.ColumnHeadersDefaultCellStyle.SelectionBackColor = ephemerisData_dataGridView.ColumnHeadersDefaultCellStyle.BackColor;
            ephemerisData_dataGridView.MultiSelect = false;
            ephemerisData_dataGridView.ReadOnly = true;

            ephemerisData_dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 14, FontStyle.Bold);
            ephemerisData_dataGridView.DefaultCellStyle.Font = new Font("맑은 고딕", 12, FontStyle.Regular);
            ephemerisData_dataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ephemerisData_dataGridView.RowTemplate.Height = 30;
            ephemerisData_dataGridView.CellClick += EphimerisGridView_CellClick;



            // ComboBox Setting
            string[] type_item = { "Full-Rate" , "Normal-Point" };
            CRDtype_comboBox.Items.AddRange(type_item);
            CRDtype_comboBox.SelectedIndex = 0;

            string[] version_item = { "Version 1" , "Version 2" };
            CRDversion_comboBox.Items.AddRange(version_item);
            CRDversion_comboBox.SelectedIndex = 0;
        }

        private void CRDtype_comboBox_SelectedIndexChanged(object sender, EventArgs e)  // CRD Type(Full-Rate/Normal-Point) 선택 이벤트
        {
            if (CRDtype_comboBox.SelectedItem.ToString() == "Full-Rate")
            {
                CRDversion_comboBox.Items.Clear();

                string version_item = "Version 1";
                CRDversion_comboBox.Items.Add(version_item);
                CRDversion_comboBox.SelectedIndex = 0;
            }
            else if (CRDtype_comboBox.SelectedItem.ToString() == "Normal-Point")
            {
                CRDversion_comboBox.Items.Clear();

                string[] version_item = { "Version 1", "Version 2" };
                CRDversion_comboBox.Items.AddRange(version_item);
                CRDversion_comboBox.SelectedIndex = 0;
            }
        }

        ProjectManager ephimerisManager = null;
        public void Initialize_EphimerisGanttChart()    // Ephimeris Chart 초기화
        {
            ephimerisManager = new ProjectManager();

            // Gantt Chart 전시
            ephimerisManager.Start = schedule_startTime;
            _mChart_ephemeris.Init(ephimerisManager);
            _mChart_ephemeris.Invalidate();

            _mChart_ephemeris.CreateTaskDelegate = delegate () { return new MyTask(ephimerisManager); };
            _mChart_ephemeris.AllowTaskDragDrop = false;
            _mChart_ephemeris.TimeResolution = TimeResolution.Minute;
        }

        public void Check_DatabaseTable()   // 데이터베이스 궤도결정 관련 Table 존재여부 확인
        {

            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        // Observation Data & CRD File & Ephemeris File Table 존재여부 Check
                        bool exist_observationData = false;
                        bool exist_crdFile = false;
                        bool exist_ephemerisFile = false;

                        command.CommandText = "SELECT * FROM pg_catalog.pg_tables WHERE schemaname = 'public';";

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["tablename"].ToString() == "nslr_observationdata") { exist_observationData = true; }
                                else if (reader["tablename"].ToString() == "nslr_crdfile") { exist_crdFile = true; }
                                else if (reader["tablename"].ToString() == "nslr_ephemerisfile") { exist_ephemerisFile = true; }
                            }

                        }

                        // Observation Data Table 없을 시 생성
                        if (exist_observationData == false)
                        {
                            command.CommandText = "CREATE TABLE nslr_observationdata (           " +
                                                  "         satellite           varchar(50),     " +
                                                  "         norad_id            integer,         " +
                                                  "         start_time          varchar(50),     " +
                                                  "         end_time            varchar(50),     " +
                                                  "         station_name        varchar(50),     " +
                                                  "         station_identifier  integer,         " +
                                                  "         orbit_information   varchar(200),    " +
                                                  "         table_name          varchar(80)      " +
                                                  ");                                            ";
                            command.ExecuteNonQuery();
                        }


                        // CRD File Table 없을 시 생성
                        if (exist_crdFile == false)
                        {
                            command.CommandText = "CREATE TABLE nslr_crdfile (                   " +
                                                  "         satellite           varchar(50),     " +
                                                  "         start_time          varchar(50),     " +
                                                  "         end_time            varchar(50),     " +
                                                  "         file_type           varchar(50),     " +
                                                  "         orbit_information   varchar(200),    " +
                                                  "         file_name           varchar(80)      " +
                                                  ");                                            ";
                            command.ExecuteNonQuery();
                        }


                        // Ephemeris File Table 없을 시 생성
                        if (exist_ephemerisFile == false)
                        {
                            command.CommandText = "CREATE TABLE nslr_ephemerisfile (                 " +
                                                  "         satellite               varchar(50),     " +
                                                  "         observation_time        varchar(50),     " +
                                                  "         period_start            varchar(50),     " +
                                                  "         period_end              varchar(50),     " +
                                                  "         crd_type                varchar(50),     " +
                                                  "         applied_model           varchar(50),     " +
                                                  "         file_name               varchar(150)     " +
                                                  ");                                                ";
                            command.ExecuteNonQuery();
                        }

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("궤도결정 Table Check 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }
            }


        }


        OpenFileDialog openFileDialog = new OpenFileDialog();

        private void ObservationDataGenerator_btn_Click(object sender, EventArgs e) // External CRD → 관측 데이터　(임시 : Full-Rate 전용)
        {
            DirectoryInfo externalCRD_directoryInfo = new DirectoryInfo(externalCRD_directory);

            if (externalCRD_directoryInfo.Exists == false)
            {
                externalCRD_directoryInfo.Create();
            }

            openFileDialog.InitialDirectory = externalCRD_directoryInfo.FullName;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string externalCRD_filePath = openFileDialog.FileName;
                string file_type = "";
                
                // External CRD(Full-Rate)는 Version 1만 추출 가능하도록 구현
                // Version 2는 NSLR 관측 데이터 형식과 맞지않음.
                if (externalCRD_filePath.Contains(".frd") == true) { file_type = "frd"; }
                else
                {
                    MessageBox.Show("적용할 수 없는 파일 형식입니다!");
                    return;
                }

                ExternalCRDToObservationData(externalCRD_filePath, file_type);  // 관측 데이터 추출 + DB 저장
            }

        }

        private void ConvertCRD_btn_Click(object sender, EventArgs e)   // External CRD → NSLR CRD　(임시 : Normal-Point 전용)
        {
            DirectoryInfo externalCRD_directoryInfo = new DirectoryInfo(externalCRD_directory);

            if (externalCRD_directoryInfo.Exists == false)
            {
                externalCRD_directoryInfo.Create();
            }

            openFileDialog.InitialDirectory = externalCRD_directoryInfo.FullName;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string externalCRD_filePath = openFileDialog.FileName;
                string file_type = "";

                if (externalCRD_filePath.Contains(".npt") == true) { file_type = "npt"; }
                else if (externalCRD_filePath.Contains(".np2") == true) { file_type = "npt2"; }
                else
                {
                    MessageBox.Show("적용할 수 없는 파일 형식입니다!");
                    return;
                }

                ExternalCRDToObservationData(externalCRD_filePath, file_type);  // 관측 데이터 추출 + CRD 생성 (관측 데이터 DB 저장 X)
            }

        }


        public void ExternalCRDToObservationData(string filePath, string fileType)  // External CRD 기반 관측 데이터 추출 (Full-Rate / Nomal-Point)
        {
            string station_name = "";
            int station_identifier = 0;
            string satellite_name = "";
            int satellite_noradID = 0;
            DateTime observation_start = new DateTime();
            DateTime observation_end = new DateTime();

            // .frd 전용 : millisecond / tof / az / el
            List<Tuple<double, double, double, double>> observationData_frd = new List<Tuple<double, double, double, double>>();

            // .npt/.np2 전용 : millisecond / tof / 초단위범위 / np포함된 데이터 갯수 / np RMS / 왜도 / 첨도 / 최빈값 (+ return-rate 추가여부 판단)
            List<double[]> observationData_npt = new List<double[]>();

            using (var streamReader = new StreamReader(filePath, Encoding.UTF8))
            {
                // .frd 전용
                double millisecond = 0.0; double tof = 0.0; double azimuth = 0.0; double elevation = 0.0;

                bool start_flag = false;
                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();
                    string[] line_words = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                    if ((line_words[0] == "H1" || line_words[0] == "h1") && start_flag == true) { break; }

                    switch (line_words[0])
                    {
                        case "H1":
                        case "h1":
                            // CRD Version(2), CRD 파일생성일자(년/월/일/시)(4) → 관측 데이터 X
                            start_flag = true;
                            break;
                        case "H2":
                        case "h2":
                            // Station 이름(1), Crustal Dynamics Project Pad Identifier(1) → 관측 데이터 O
                            // Crustal Dynamics Project 2-Digit Info(2), Station Epoch(1) → 관측 데이터 X
                            station_name = line_words[1];
                            station_identifier = Convert.ToInt32(line_words[2]);
                            break;
                        case "H3":
                        case "h3":
                            // 위성 이름(1) → 관측 데이터 O
                            // ILRS 위성 식별자(1), SIC(위성식별코드)(1) → 관측 데이터 X
                            // NORAD ID(1) → 관측 데이터 O
                            // 우주선 Eppch(transponder only)(1), Target Type(1) → 관측 데이터 X
                            satellite_name = line_words[1];
                            satellite_noradID = Convert.ToInt32(line_words[4]);

                            // 외부 CRD에 명시된 위성이름과 TLE_Information 상 위성이름dl 다른 경우
                            // 위성이름으로 TLE_Information 상 매칭되는 위성 탐색 불가능
                            // 이에 따라 Norad 데이터로 TLE_Information 탑색 후, 매칭되는 위성의 이름으로 Set
                            bool exist_information = false;
                            foreach (KeyValuePair<string, TLE> tle_information in TLE_Information)
                            {
                                if (tle_information.Value.Norad == satellite_noradID)
                                {
                                    satellite_name = tle_information.Key;
                                    exist_information = true;
                                    break;
                                }
                            }

                            if (exist_information == false)
                            {
                                MessageBox.Show("External CRD 사용불가 : 해당되는 위성정보가 없습니다.");
                                return;
                            }
                            break;
                        case "H4":
                        case "h4":
                            // 파일 형식(1) → 관측 데이터 X
                            // 시작~종료(년/월/일/시/분/초)(12) → 관측 데이터 O
                            // 기타 항목(8) → 관측 데이터 X
                            observation_start = new DateTime(int.Parse(line_words[2]), int.Parse(line_words[3]), int.Parse(line_words[4]), int.Parse(line_words[5]), int.Parse(line_words[6]), int.Parse(line_words[7]), DateTimeKind.Utc);
                            observation_end = new DateTime(int.Parse(line_words[8]), int.Parse(line_words[9]), int.Parse(line_words[10]), int.Parse(line_words[11]), int.Parse(line_words[12]), int.Parse(line_words[13]), DateTimeKind.Utc);
                            break;
                        case "C0":
                        case "c0":
                            // 세부 정보 유형(1), 투과 파장(1), 시스템 구성 ID(1~4) → 관측 데이터 X
                            break;
                        case "C1":
                        case "c1":
                            // 레이저 Configuration → 관측 데이터 X
                            break;
                        case "C2":
                        case "c2":
                            // 검출기 Configuration  → 관측 데이터 X
                            break;
                        case "C3":
                        case "c3":
                            // 타이밍 시스템 Configuration  → 관측 데이터 X
                            break;
                        case "C4":
                        case "c4":
                            // 트랜스폰더 Configuration → 관측 데이터 X
                            break;
                        case "10":
                            if (fileType == "frd")
                            {
                                millisecond = Convert.ToDouble(line_words[1]);
                                tof = Convert.ToDouble(line_words[2].IndexOf(".") == 0 ? line_words[2].Insert(0, "0") : line_words[2]);
                                azimuth = 0.0;
                                elevation = 0.0;
                            }
                            break;
                        case "11":
                            if (fileType == "npt" || fileType == "npt2")
                            {
                                observationData_npt.Add(new double[8] {
                                Convert.ToDouble(line_words[1]), Convert.ToDouble(line_words[2]), Convert.ToDouble(line_words[5]), Convert.ToDouble(line_words[6]),
                                Convert.ToDouble(line_words[7]), Convert.ToDouble(line_words[8]), Convert.ToDouble(line_words[9]), Convert.ToDouble(line_words[10])
                                });
                            }
                            break;
                        case "30":
                            if (fileType == "frd")
                            {
                                if (millisecond == Convert.ToDouble(line_words[1]))
                                {
                                    azimuth = Convert.ToDouble(line_words[2]);
                                    elevation = Convert.ToDouble(line_words[3]);

                                    observationData_frd.Add(Tuple.Create(millisecond, tof, azimuth, elevation));
                                }
                            }
                            break;

                    }

                }

            }

            if (fileType == "frd")
            {
                // 관측 데이터 생성 및 DB 저장
                ObservationDataGenerator("tle", satellite_name, observation_start, observation_end, station_name, station_identifier, observationData_frd);
            }
            else if (fileType == "npt" || fileType == "npt2")
            {
                // TLE Ephemeris 정보
                double[] targetEphemeris = new double[6];

                // TLE Propagator
                StringBuilder line1 = new StringBuilder(TLE_Information[satellite_name].Line1);
                StringBuilder line2 = new StringBuilder(TLE_Information[satellite_name].Line2);
                double[] epoch_time = new double[6];
                int noradID = (int)GetTLEData(sub_tleClass, line1, line2, epoch_time);

                double[] observation_time = { (double)observation_start.Year, (double)observation_start.Month, (double)observation_start.Day,
                    (double)observation_start.Hour, (double)observation_start.Minute, (double)observation_start.Second };

                double epoch_MJD = ConvertGreg2MJD(Global.timeSys, epoch_time);
                double observation_MJD = ConvertGreg2MJD(Global.timeSys, observation_time);

                int nTime = 1;
                double[] elapsedSecs = new double[nTime];
                for (int n = 0; n < nTime; n++)
                {
                    elapsedSecs[n] = (observation_MJD - epoch_MJD) * 86400.0;
                }
                targetEphemeris = new double[nTime * 6];
                int optCoord = 0;   // 0 : J2000, 1 : ECEF
                PropagateTLE(sub_tleClass, elapsedSecs, nTime, targetEphemeris, optCoord);

                // NSLR CRD(.npt or .np2) 생성
                NormalPointToCRD(satellite_name, satellite_noradID, observation_start, observation_end, station_name, station_identifier, 
                    targetEphemeris, observationData_npt, fileType);
            }


        }

        public void ObservationDataGenerator(string TLE_CPF_Info, string name, DateTime start, DateTime end, string station_name, int station_identifier,
            List<Tuple<double, double, double, double>> data)  // 관측 데이터 DB 저장 (Full-Rate 전용) + 해당 함수로 NSLR 관측데이터 입력가능
        {
            // TLE/CPF Propagator + NoradID 저장
            int noradID = 0;
            double[] targetEphemeris = new double[6];

            if (TLE_CPF_Info == "tle")
            {
                // TLE Propagator
                StringBuilder line1 = new StringBuilder(TLE_Information[name].Line1);
                StringBuilder line2 = new StringBuilder(TLE_Information[name].Line2);
                double[] epoch_time = new double[6];
                noradID = (int)GetTLEData(sub_tleClass, line1, line2, epoch_time);

                double[] observation_time = { (double)start.Year, (double)start.Month, (double)start.Day,
                (double)start.Hour, (double)start.Minute, (double)start.Second };

                double epoch_MJD = ConvertGreg2MJD(Global.timeSys, epoch_time);
                double observation_MJD = ConvertGreg2MJD(Global.timeSys, observation_time);

                int nTime = 1;
                double[] elapsedSecs = new double[nTime];
                for (int n = 0; n < nTime; n++)
                {
                    elapsedSecs[n] = (observation_MJD - epoch_MJD) * 86400.0;
                }
                targetEphemeris = new double[nTime * 6];
                int optCoord = 0;   // 0 : J2000, 1 : ECEF
                PropagateTLE(sub_tleClass, elapsedSecs, nTime, targetEphemeris, optCoord);
            }
            else if (TLE_CPF_Info == "cpf")
            {
                // CPF 파일 Parameter로 받아야함, 해당부분에서 Searching 불가 >> TLE_CPF == "cpf" 일 경우, 위성이름으로 CPF 파일 Searching

                // satellite name 정보, CPF는 파일 정보로 사용 / Norad ID 및 ephemeris 정보 추출
                string filePath = "./CPF/" + name;
                using (var streamReader = new StreamReader(filePath, Encoding.UTF8))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        string[] line_words = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                        if (line_words[0] == "H2" || line_words[0] == "h2")
                        {
                            noradID = int.Parse(line_words[3]);
                            break;
                        }
                    }

                }

                bool exist_TLE = false;
                string satName_TLE = "";
                foreach (KeyValuePair<string, TLE> tle_information in TLE_Information)
                {
                    if (tle_information.Value.Norad == noradID)
                    {
                        exist_TLE = true;
                        satName_TLE = tle_information.Key;
                        break;
                    }
                }

                if (exist_TLE == false)
                {
                    MessageBox.Show("해당 CPF에 대한 초기궤도를 찾을 수 없습니다.");
                    return;
                }

                // TLE Propagator
                StringBuilder line1 = new StringBuilder(TLE_Information[satName_TLE].Line1);
                StringBuilder line2 = new StringBuilder(TLE_Information[satName_TLE].Line2);
                double[] epoch_time = new double[6];
                GetTLEData(sub_tleClass, line1, line2, epoch_time);

                double[] observation_time = { (double)start.Year, (double)start.Month, (double)start.Day,
                (double)start.Hour, (double)start.Minute, (double)start.Second };

                double epoch_MJD = ConvertGreg2MJD(Global.timeSys, epoch_time);
                double observation_MJD = ConvertGreg2MJD(Global.timeSys, observation_time);

                int nTime = 1;
                double[] elapsedSecs = new double[nTime];
                for (int n = 0; n < nTime; n++)
                {
                    elapsedSecs[n] = (observation_MJD - epoch_MJD) * 86400.0;
                }
                targetEphemeris = new double[nTime * 6];
                int optCoord = 0;   // 0 : J2000, 1 : ECEF
                PropagateTLE(sub_tleClass, elapsedSecs, nTime, targetEphemeris, optCoord);

            }

            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        // 관측 데이터 List Table 존재여부 Check
                        command.CommandText = "SELECT * FROM pg_catalog.pg_tables WHERE schemaname = 'public';";
                        bool exist_table = false;
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["tablename"].ToString() == "nslr_observationdata")
                                {
                                    exist_table = true;
                                    break;
                                }
                            }
                        }

                        // 관측 데이터 List Table 없을 시 생성
                        if (exist_table == false)
                        {
                            command.CommandText = "CREATE TABLE nslr_observationdata (           " +
                                                  "         satellite           varchar(50),     " +
                                                  "         norad_id            integer,         " +
                                                  "         start_time          varchar(50),     " +
                                                  "         end_time            varchar(50),     " +
                                                  "         station_name        varchar(50),     " +
                                                  "         station_identifier  integer,         " +
                                                  "         orbit_information   varchar(200),    " +
                                                  "         table_name          varchar(80)      " +
                                                  ");                                            ";
                            command.ExecuteNonQuery();
                        }

                        // 동일 관측 데이터 생성 방지
                        command.CommandText = "SELECT * FROM nslr_observationdata WHERE " + "satellite = '" + name +
                            "' AND start_time = '" + start.ToString("yyyy-MM-dd HH:mm:ss") + "' AND end_time = '" + end.ToString("yyyy-MM-dd HH:mm:ss") + "';";
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MessageBox.Show("해당 관측데이터는 이미 있습니다!");
                                return;
                            }
                        }

                        // 관측 데이터 List 입력
                        string table_name = ("observationresult" + "_" + noradID.ToString() + "_" + start.ToString("yyyyMMddHHmmss")).ToLower();
                        string orbit_information = targetEphemeris[0] + "/" + targetEphemeris[1] + "/" + targetEphemeris[2] + "/" + targetEphemeris[3] + "/" +
                            targetEphemeris[4] + "/" + targetEphemeris[5];

                        command.CommandText = String.Format("INSERT INTO nslr_observationdata VALUES ('{0}', {1}, '{2}', '{3}', '{4}', {5}, '{6}', '{7}');",
                            name, noradID, start.ToString("yyyy-MM-dd HH:mm:ss"), end.ToString("yyyy-MM-dd HH:mm:ss"), station_name, station_identifier,
                            orbit_information, table_name);
                        command.ExecuteNonQuery();

                        // 관측 데이터 Table 생성
                        command.CommandText = "CREATE TABLE " + table_name + " (               " +
                                              "         Millisecond      double precision,     " +
                                              "         Tof              double precision,     " +
                                              "         Azimuth          double precision,     " +
                                              "         Elevation        double precision      " +
                                              ");                                              ";
                        command.ExecuteNonQuery();

                        // 관측 데이터 Table 입력
                        for (int i = 0; i < data.Count; i++)
                        {
                            command.CommandText = String.Format("INSERT INTO " + table_name + " VALUES ({0}, {1}, {2}, {3});",
                            data[i].Item1, data[i].Item2, data[i].Item3, data[i].Item4);

                            command.ExecuteNonQuery();
                        }

                        connect.Close();
                    }

                    MessageBox.Show("관측데이터(" + name + ") 생성 완료!");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("관측 데이터 생성 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }
            }

            // 레이저 관측 데이터 List 업데이트
            Update_ObservationData();
        }


        public void NormalPointToCRD(string name, int noradID, DateTime start_UTC, DateTime end_UTC, string station_name, int station_identifier,
            double[] targetEphemeris, List<double[]> datas/*Size : 8*/, string type)   // Normal-Point → CRD (Normal-Point 전용)
        {


            DirectoryInfo nslrCRD_directoryInfo = new DirectoryInfo(nslrCRD_directory);
            if (nslrCRD_directoryInfo.Exists == false) { nslrCRD_directoryInfo.Create(); }

            string nslrCRD_fileName = "nslrCRD_" + (string.Concat(name.Where(x => !char.IsWhiteSpace(x)))).ToLower() + "_" + start_UTC.ToString("yyyyMMddHHmmss");
            if (type == "npt") { nslrCRD_fileName = nslrCRD_fileName + "_npt" + ".npt"; }
            else if (type == "npt2") { nslrCRD_fileName = nslrCRD_fileName + "_npt2" + ".np2"; }
            FileInfo nslrCRD_fileInfo = new FileInfo(nslrCRD_directory + "/" + nslrCRD_fileName);

            // 동일 CRD File 생성 방지
            if (nslrCRD_fileInfo.Exists == true)
            {
                MessageBox.Show("해당 CRD는 이미 있습니다!");
                return;
            }

            // CRD 생성
            if (type == "npt")
            {
                using (StreamWriter writer = nslrCRD_fileInfo.CreateText())
                {
                    // H1, H2, H3, H4, C0
                    writer.WriteLine("h1" + " CRD  1 " + DateTime.UtcNow.Year.ToString("D4") + " " + DateTime.UtcNow.Month.ToString("D2") + " " + DateTime.UtcNow.Day.ToString("D2") + " " + DateTime.UtcNow.Hour.ToString("D2"));
                    writer.WriteLine("h2" + " " + station_name + "       " + station_identifier.ToString("D4") + " " + (0).ToString("D2") + " " + (0).ToString("D2") + "  " + (4).ToString("D1"));
                    writer.WriteLine("h3" + " " + (string.Concat(name.Where(x => !char.IsWhiteSpace(x)))).ToLower() + "    " + (0).ToString("D7") + " " + (0).ToString("D4") + "    " + (noradID).ToString("D6") + " " + (0).ToString("D1") + " " + (1).ToString("D1"));
                    writer.WriteLine("h4" + "  " + (1).ToString("D1") + " " + start_UTC.ToString("yyyy MM dd HH mm ss") + " " + end_UTC.ToString("yyyy MM dd HH mm ss") + "  " +
                        (0).ToString("D1") + " " + (0).ToString("D1") + " " + (0).ToString("D1") + " " + (0).ToString("D1") + " " + (1).ToString("D1") + " " + (0).ToString("D1") + " " + (2).ToString("D1") + " " + (0).ToString("D1"));
                    writer.WriteLine("c0" + " " + (0).ToString("D1") + " " + (532.000).ToString("F3") + " " + "std");

                    // D11
                    foreach (double[] data in datas)
                    {
                        writer.WriteLine("11" + " " + data[0].ToString("F12") + " " + data[1].ToString("F12") + " " + "std" + " " + (2).ToString("D1") + " " + data[2].ToString("F1") + " " + ((int)data[3]).ToString() +
                            " " + data[4].ToString("F1") + " " + data[5].ToString("F3") + " " + data[6].ToString("F3") + " " + data[7].ToString("F1") + " " + (-1.0).ToString("F1") + " " + (0).ToString("D1"));
                    }

                    // H8, H9
                    writer.WriteLine("h8");
                    writer.WriteLine("H9");
                }
            }
            else if (type == "npt2")
            {
                using (StreamWriter writer = nslrCRD_fileInfo.CreateText())
                {
                    // H1, H2, H3, H4, C0
                    writer.WriteLine("H1" + " CRD  2 " + DateTime.UtcNow.Year.ToString("D4") + " " + DateTime.UtcNow.Month.ToString("D2") + " " + DateTime.UtcNow.Day.ToString("D2") + " " + DateTime.UtcNow.Hour.ToString("D2"));
                    writer.WriteLine("H2" + " " + station_name + " " + station_identifier.ToString("D4") + " " + (0).ToString("D2") + " " + (0).ToString("D2") + " " + (4).ToString("D1") + " " + "na");
                    writer.WriteLine("H3" + " " + (string.Concat(name.Where(x => !char.IsWhiteSpace(x)))).ToLower() + " " + (0).ToString("D7") + " " + (0).ToString("D4") + " " + (noradID).ToString("D6") + " " + (0).ToString("D1") + " " + (0).ToString("D1") + " " + "na");
                    writer.WriteLine("H4" + " " + (1).ToString("D1") + " " + start_UTC.ToString("yyyy MM dd HH mm ss") + " " + end_UTC.ToString("yyyy MM dd HH mm ss") + " " +
                        (0).ToString("D1") + " " + (0).ToString("D1") + " " + (0).ToString("D1") + " " + (0).ToString("D1") + " " + (1).ToString("D1") + " " + (0).ToString("D1") + " " + (2).ToString("D1") + " " + (0).ToString("D1"));
                    writer.WriteLine("C0" + " " + (0).ToString("D1") + " " + (532.000).ToString("F3") + " " + "std");

                    // D11
                    foreach (double[] data in datas)
                    {
                        writer.WriteLine("11" + " " + data[0].ToString("F12") + " " + data[1].ToString("F12") + "  " + "std" + " " + (2).ToString("D1") + " " + data[2].ToString("F1") + " " + ((int)data[3]).ToString() +
                            " " + data[4].ToString("F1") + " " + data[5].ToString("F3") + " " + data[6].ToString("F3") + " " + data[7].ToString("F1") + " " + "na" + " " + (0).ToString("D1") + " " + "na");
                    }

                    // H8, H9
                    writer.WriteLine("H8");
                    writer.WriteLine("H9");
                }
            }


            // 생성된 CRD 정보, Database 저장
            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        // CRD File 정보 Table 존재여부 Check
                        command.CommandText = "SELECT * FROM pg_catalog.pg_tables WHERE schemaname = 'public';";
                        bool exist_table = false;
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["tablename"].ToString() == "nslr_crdfile")
                                {
                                    exist_table = true;
                                    break;
                                }
                            }
                        }

                        // CRD File 정보 Table 없을 시 생성
                        if (exist_table == false)
                        {
                            command.CommandText = "CREATE TABLE nslr_crdfile (                   " +
                                                  "         satellite           varchar(50),     " +
                                                  "         start_time          varchar(50),     " +
                                                  "         end_time            varchar(50),     " +
                                                  "         file_type           varchar(50),     " +
                                                  "         orbit_information   varchar(200),    " +
                                                  "         file_name           varchar(80)      " +
                                                  ");                                            ";
                            command.ExecuteNonQuery();
                        }

                        // 관측 데이터 List 입력
                        string orbit_information = targetEphemeris[0] + "/" + targetEphemeris[1] + "/" + targetEphemeris[2] + "/" + targetEphemeris[3] + "/" +
                            targetEphemeris[4] + "/" + targetEphemeris[5];

                        command.CommandText = String.Format("INSERT INTO nslr_crdfile VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                            name, start_UTC.ToString("yyyy-MM-dd HH:mm:ss"), end_UTC.ToString("yyyy-MM-dd HH:mm:ss"), "npt2", orbit_information, nslrCRD_fileName);
                        command.ExecuteNonQuery();

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("CRD 정보 저장 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }
            }

            MessageBox.Show("CRD(" + name + ") 생성 완료!");

            // CRD File List 업데이트
            Update_CRDFile();
        }


        public void ObservationDataToCRD(string crd_type)  // DB 관측 데이터 → CRD (Full-Rate 전용)
        {
            int row_index = observationData_dataGridView.SelectedRows[0].Index;
            if (!(row_index >= 0 && row_index < observationData_dataGridView.Rows.Count))
            {
                MessageBox.Show("관측 데이터를 선택하세요!");
                return;
            }

            string item1 = observationData_dataGridView.SelectedRows[0].Cells[0].Value.ToString();  // 위성명
            string item2 = observationData_dataGridView.SelectedRows[0].Cells[1].Value.ToString();  // 관측시간(Start)
            string item3 = observationData_dataGridView.SelectedRows[0].Cells[2].Value.ToString();  // 관측시간(End)

            int norad_id = 0;
            string station_name = "";
            int station_identifier = 0;
            string orbit_information = "";
            string table_name = "";
            List<Tuple<string, string, string, string>> observation_datas = new List<Tuple<string, string, string, string>>();

            // postgresql 접속
            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        command.CommandText = "SELECT * FROM nslr_observationdata WHERE " + "satellite = '" + item1 + "' AND start_time = '" + item2 + "' AND end_time = '" + item3 + "';";
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                norad_id = Convert.ToInt32(reader["norad_id"].ToString());
                                station_name = reader["station_name"].ToString();
                                station_identifier = Convert.ToInt32(reader["station_identifier"].ToString());
                                orbit_information = reader["orbit_information"].ToString();
                                table_name = reader["table_name"].ToString();
                            }
                        }

                        command.CommandText = "SELECT * FROM " + table_name + ";";
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                observation_datas.Add(new Tuple<string, string, string, string>(reader["millisecond"].ToString(), reader["tof"].ToString(), reader["azimuth"].ToString(), reader["elevation"].ToString()));
                            }
                        }

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("관측 데이터 읽는 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }
            }

            // CRD File 생성
            DirectoryInfo nslrCRD_directoryInfo = new DirectoryInfo(nslrCRD_directory);
            if (nslrCRD_directoryInfo.Exists == false) { nslrCRD_directoryInfo.Create(); }

            string[] dateTime_array = item2.Split(' ');
            string[] date_array = dateTime_array[0].Split('-');
            string[] time_array = dateTime_array[1].Split(':');
            DateTime dateTime = new DateTime(int.Parse(date_array[0]), int.Parse(date_array[1]), int.Parse(date_array[2]), int.Parse(time_array[0]),
                int.Parse(time_array[1]), int.Parse(time_array[2]), DateTimeKind.Utc);

            string[] dateTime_array2 = item3.Split(' ');
            string[] date_array2 = dateTime_array2[0].Split('-');
            string[] time_array2 = dateTime_array2[1].Split(':');

            string nslrCRD_fileName = "nslrCRD_" + (string.Concat(item1.Where(x => !char.IsWhiteSpace(x)))).ToLower() + "_" + dateTime.ToString("yyyyMMddHHmmss" + "_frd" + ".frd");
            FileInfo nslrCRD_fileInfo = new FileInfo(nslrCRD_directory + "/" + nslrCRD_fileName);

            // 동일 CRD File 생성 방지
            if (nslrCRD_fileInfo.Exists == true)
            {
                MessageBox.Show("해당 CRD는 이미 있습니다!");
                return;
            }

            using (StreamWriter writer = nslrCRD_fileInfo.CreateText())
            {
                // H1, H2, H3, H4, C0
                writer.WriteLine("H1" + " crd  1 " + DateTime.UtcNow.Year.ToString("D4") + " " + DateTime.UtcNow.Month.ToString("D2") + " " + DateTime.UtcNow.Day.ToString("D2") + " " + DateTime.UtcNow.Hour.ToString("D2"));
                writer.WriteLine("H2" + "       " + station_name + " " + station_identifier.ToString("D4") + " " + (0).ToString("D2") + " " + (1).ToString("D2") + " " + (4).ToString("D1"));
                writer.WriteLine("H3" + " " + (string.Concat(item1.Where(x => !char.IsWhiteSpace(x)))).ToLower() + "     " + (0).ToString("D7") + " " + (0).ToString("D4") + "   " + (norad_id).ToString("D6") + " " + (0).ToString("D1") + " " + (1).ToString("D1"));
                writer.WriteLine("H4" + "  " + (0).ToString("D1") + " " + date_array[0] + " " + date_array[1] + " " + date_array[2] + " " + time_array[0] + " " + time_array[1] + " " + time_array[2] + " " +
                    date_array2[0] + " " + date_array2[1] + " " + date_array2[2] + " " + time_array2[0] + " " + time_array2[1] + " " + time_array2[2] + "  " +
                    (0).ToString("D1") + " " + (0).ToString("D1") + " " + (0).ToString("D1") + " " + (0).ToString("D1") + " " + (0).ToString("D1") + " " + (0).ToString("D1") + " " + (2).ToString("D1") + " " + (0).ToString("D1"));
                writer.WriteLine("C0" + " " + (0).ToString("D1") + "    " + (532.000).ToString("F3") + " " + "STD");

                // D20
                writer.WriteLine("20" + "  " + (Convert.ToDouble(observation_datas[0].Item1)).ToString("F12") + " " + (950.00).ToString("F2") + "  " + (273.00).ToString("F2") + "  " + (30).ToString("D2") + " " + (0).ToString("D1"));

                // D10, D30
                foreach (Tuple<string, string, string, string> observation_data in observation_datas)
                {
                    writer.WriteLine("10" + " " + (Convert.ToDouble(observation_data.Item1)).ToString("F12") + "     " + (Convert.ToDouble(observation_data.Item2)).ToString("F12") + " " +
                        "STD" + "  " + (2).ToString("D1") + " " + (2).ToString("D1") + " " + (2).ToString("D1") + " " + (0).ToString("D1") + "     " + (0).ToString("D1"));
                    writer.WriteLine("30" + " " + (Convert.ToDouble(observation_data.Item1)).ToString("F12") + "     " + (Convert.ToDouble(observation_data.Item3)).ToString("F12") + "     " + (Convert.ToDouble(observation_data.Item4)).ToString("F12") + " " +
                        (0).ToString("D1") + " " + (2).ToString("D1") + " " + (0).ToString("D1"));
                }

                // H8, H9
                writer.WriteLine("H8");
                writer.WriteLine("H9");
            }

            //  셍성된 CRD 정보, Database 저장
            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        // CRD File 정보 Table 존재여부 Check
                        command.CommandText = "SELECT * FROM pg_catalog.pg_tables WHERE schemaname = 'public';";
                        bool exist_table = false;
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["tablename"].ToString() == "nslr_crdfile")
                                {
                                    exist_table = true;
                                    break;
                                }
                            }
                        }

                        // CRD File 정보 Table 없을 시 생성
                        if (exist_table == false)
                        {
                            command.CommandText = "CREATE TABLE nslr_crdfile (                   " +
                                                  "         satellite           varchar(50),     " +
                                                  "         start_time          varchar(50),     " +
                                                  "         end_time            varchar(50),     " +
                                                  "         file_type           varchar(50),     " +
                                                  "         orbit_information   varchar(200),    " +
                                                  "         file_name           varchar(80)      " +
                                                  ");                                            ";
                            command.ExecuteNonQuery();
                        }

                        // 관측 데이터 List 입력
                        command.CommandText = String.Format("INSERT INTO nslr_crdfile VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                            item1, item2, item3, crd_type, orbit_information, nslrCRD_fileName);
                        command.ExecuteNonQuery();

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("CRD 정보 저장 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }
            }

            MessageBox.Show("CRD(" + item1 + ") 생성 완료!");

            // CRD File List 업데이트
            Update_CRDFile();
        }


        private const double Bin_Size = 60.0;    // 단위 : 초
        public void ObservationDataToNormalPoint(string crd_type)   // 관측 데이터(DB) → Normal-Point (Normal-Point 전용)
        {
            int row_index = observationData_dataGridView.SelectedRows[0].Index;
            if (row_index < 0 || row_index >= observationData_dataGridView.Rows.Count)
            {
                MessageBox.Show("관측 데이터를 선택하세요!");
                return;
            }

            string item1 = observationData_dataGridView.SelectedRows[0].Cells[0].Value.ToString();  // 위성명
            string item2 = observationData_dataGridView.SelectedRows[0].Cells[1].Value.ToString();  // 관측시간(Start)
            string item3 = observationData_dataGridView.SelectedRows[0].Cells[2].Value.ToString();  // 관측시간(End)

            int norad_id = 0;
            string station_name = "";
            int station_identifier = 0;
            string orbit_information = "";
            string table_name = "";
            List<Tuple<double, double, double, double>> observation_datas = new List<Tuple<double, double, double, double>>();

            // postgresql 접속 후 관측 데이터 불러오기
            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        command.CommandText = "SELECT * FROM nslr_observationdata WHERE " + "satellite = '" + item1 + "' AND start_time = '" + item2 + "' AND end_time = '" + item3 + "';";
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                norad_id = Convert.ToInt32(reader["norad_id"].ToString());
                                station_name = reader["station_name"].ToString();
                                station_identifier = Convert.ToInt32(reader["station_identifier"].ToString());
                                orbit_information = reader["orbit_information"].ToString();
                                table_name = reader["table_name"].ToString();
                            }
                        }

                        command.CommandText = "SELECT * FROM " + table_name + ";";
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                double millisecond = Convert.ToDouble(reader["millisecond"].ToString());
                                double tof = Convert.ToDouble(reader["tof"].ToString());
                                double azimuth = Convert.ToDouble(reader["azimuth"].ToString());
                                double elevation = Convert.ToDouble(reader["elevation"].ToString());

                                observation_datas.Add(new Tuple<double, double, double, double>(millisecond, tof, azimuth, elevation));
                            }
                        }

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("관측 데이터 읽는 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }
            }

            // Full-Rate → Normal-Point
            List<double[]> normalPoint_datas = new List<double[]>();

            double firstTime = observation_datas.First().Item1;
            double lastTime = observation_datas.Last().Item1;

            for (double binStart = firstTime; binStart <= lastTime; binStart += Bin_Size)
            {
                double binEnd = binStart + Bin_Size;

                var bin_data = observation_datas.Where(t => t.Item1 >= binStart && t.Item1 < binEnd).ToList();

                if (bin_data.Count == 0) { continue; }

                // Seconds Of Day 평균치
                double average_secondsOfDay = bin_data.Average(t => t.Item1);

                // Time Of Flight 평균치
                double average_tof = bin_data.Average(t => t.Item2);

                // bin Tof의 RMS
                double rms = Math.Sqrt(bin_data.Sum(t => Math.Pow(t.Item2 - average_tof, 2)) / bin_data.Count);

                // === 추가 통계량: 왜도, 첨도, 최빈값 === >> 추후 필요 시 구현 (산출방식 수정 필요함)
                /*
                var tofList = bin_data.Select(t => t.Item2).ToList();
                int n = tofList.Count;
                double stdDev = Math.Sqrt(tofList.Sum(x => Math.Pow(x - average_tof, 2)) / n);

                double skewness = 0;
                double kurtosis = 0;
                if (stdDev > 0)
                {
                    skewness = tofList.Sum(x => Math.Pow((x - average_tof) / stdDev, 3)) / n;
                    kurtosis = tofList.Sum(x => Math.Pow((x - average_tof) / stdDev, 4)) / n - 3; // excess kurtosis
                }
                */

                normalPoint_datas.Add(new double[8] {average_secondsOfDay, average_tof, Bin_Size, bin_data.Count, rms, 0.0, 0.0, 0.0});
            }

            // Normal-Point → CRD
            string[] split_str = item2.Split(' ');
            string[] split_date = split_str[0].Split('-');
            string[] split_time = split_str[1].Split(':');
            DateTime start_UTC = new DateTime(int.Parse(split_date[0]), int.Parse(split_date[1]), int.Parse(split_date[2]), int.Parse(split_time[0]),
                int.Parse(split_time[1]), int.Parse(split_time[2]));
            DateTime end_UTC = new DateTime(int.Parse(split_date[0]), int.Parse(split_date[1]), int.Parse(split_date[2]), int.Parse(split_time[0]),
                int.Parse(split_time[1]), int.Parse(split_time[2]));

            string[] targetEphemeris_str = orbit_information.Split('/');
            double[] targetEphemeris = new double[targetEphemeris_str.Length];
            for (int i = 0; i < targetEphemeris.Length; i++) { targetEphemeris[i] = Convert.ToDouble(targetEphemeris_str[i]); }

            if (crd_type == "npt")
            {
                NormalPointToCRD(item1, norad_id, start_UTC, end_UTC, "NSLR", 9999, targetEphemeris, normalPoint_datas, crd_type);
            }
            else if (crd_type == "npt2")
            {
                NormalPointToCRD(item1, norad_id, start_UTC, end_UTC, "NSLR", 9999, targetEphemeris, normalPoint_datas, crd_type);
            }
            
        }

        

        
        
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        


        public void Update_ObservationData()    // 관측 데이터 List 업데이트
        {
            if (observationData_dataGridView.DataSource != null)
            {
                observationData_dataGridView.Columns.Clear();
                observationData_dataGridView.DataSource = null;
                ObservationData_List.Clear();
            }

            // DataBase 접속 후 데이터 List 읽기
            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        // 관측 데이터 목록 불러오기
                        command.CommandText = "SELECT * FROM nslr_observationdata;";

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string satellite = reader["satellite"].ToString();

                                string[] dateTime_array = (reader["start_time"].ToString()).Split(' ');
                                string[] date_array = dateTime_array[0].Split('-');
                                string[] time_array = dateTime_array[1].Split(':');

                                DateTime start_UTC = new DateTime(int.Parse(date_array[0]), int.Parse(date_array[1]), int.Parse(date_array[2]),
                                    int.Parse(time_array[0]), int.Parse(time_array[1]), int.Parse(time_array[2]), DateTimeKind.Utc);

                                dateTime_array = (reader["end_time"].ToString()).Split(' ');
                                date_array = dateTime_array[0].Split('-');
                                time_array = dateTime_array[1].Split(':');

                                DateTime end_UTC = new DateTime(int.Parse(date_array[0]), int.Parse(date_array[1]), int.Parse(date_array[2]),
                                    int.Parse(time_array[0]), int.Parse(time_array[1]), int.Parse(time_array[2]), DateTimeKind.Utc);

                                string available = "";
                                if (DateTime.Compare(start_UTC.AddDays(1), DateTime.UtcNow) == 1)   // 관측데이터 유효시간 : (관측시작시간) 부터 (관측시작시간 + 1일) 까지 
                                {
                                    available = "사용 가능";
                                }
                                else
                                {
                                    available = "사용 불가";
                                }


                                ObservationData_List.Add(new ObservationData_Info(satellite, reader["start_time"].ToString(), reader["end_time"].ToString(), available));
                                
                            }
                        }

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("관측 데이터 읽는 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }
            }

            if (ObservationData_List.Count > 0)
            {
                observationData_dataGridView.DataSource = ObservationData_List;
                observationData_dataGridView.Columns["Satellite"].HeaderText = "위성명";
                observationData_dataGridView.Columns["StartTime"].HeaderText = "관측시간(Start)";
                observationData_dataGridView.Columns["EndTime"].HeaderText = "관측시간(End)";
                observationData_dataGridView.Columns["Available"].HeaderText = "Available";

                observationData_dataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                observationData_dataGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                observationData_dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                observationData_dataGridView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                observationData_dataGridView.ClearSelection();
            }

        }


        public void Update_CRDFile()    // CRD File List 업데이트
        {
            if (CRDFile_dataGridView.DataSource != null)
            {
                CRDFile_dataGridView.Columns.Clear();
                CRDFile_dataGridView.DataSource = null;
                CRDFile_List.Clear();
            }

            // DataBase 접속 후 데이터 List 읽기
            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        // CRD File 목록 불러오기
                        command.CommandText = "SELECT * FROM nslr_crdfile;";

                        string delete_query = null;
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 해당 CRD File 실제 존재여부 Check, 없을 시 해당 목록 DataBase에서 삭제
                                FileInfo CRD_file = new FileInfo(nslrCRD_directory + "/" + reader["file_name"].ToString());
                                if (!CRD_file.Exists)
                                {
                                    delete_query += "DELETE FROM nslr_crdfile WHERE file_name = '" + reader["file_name"].ToString() + "';";
                                    continue;
                                }

                                // 목록 추가
                                string satellite = reader["satellite"].ToString();
                                string observation_time = reader["start_time"].ToString();

                                string file_type = reader["file_type"].ToString();
                                string data_type = "";
                                string version = "";
                                if (file_type.Contains("frd")) { data_type = "Full-Rate"; }
                                else if (file_type.Contains("npt")) { data_type = "Normal-Point"; }

                                if (file_type.Contains("2")) { version = "Version2"; }
                                else { version = "Version1"; }

                                string[] dateTime_array = observation_time.Split(' ');
                                string[] date_array = dateTime_array[0].Split('-');
                                string[] time_array = dateTime_array[1].Split(':');
                                DateTime observationTime_UTC = new DateTime(int.Parse(date_array[0]), int.Parse(date_array[1]), int.Parse(date_array[2]),
                                    int.Parse(time_array[0]), int.Parse(time_array[1]), int.Parse(time_array[2]), DateTimeKind.Utc);

                                string available = "";
                                if (DateTime.Compare(observationTime_UTC.AddDays(1), DateTime.UtcNow) == 1)   // CRD File 유효기간 : (관측시작시간) 부터 (관측시작시간 + 1일) 까지 
                                {
                                    available = "사용 가능";
                                }
                                else
                                {
                                    available = "사용 불가";
                                }

                                CRDFile_List.Add(new CRDFile_info(satellite, observation_time, data_type, version, available));
                            }
                        }

                        if (delete_query != null)
                        {
                            command.CommandText = delete_query;
                            command.ExecuteNonQuery();
                        }

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("CRD File 읽는 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }
            }

            if (CRDFile_List.Count > 0)
            {
                CRDFile_dataGridView.DataSource = CRDFile_List;
                CRDFile_dataGridView.Columns["Satellite"].HeaderText = "위성명";
                CRDFile_dataGridView.Columns["Observation_Time"].HeaderText = "관측 일자";
                CRDFile_dataGridView.Columns["Data_Type"].HeaderText = "Data Type";
                CRDFile_dataGridView.Columns["Version"].HeaderText = "Version";
                CRDFile_dataGridView.Columns["Available"].HeaderText = "Available";

                CRDFile_dataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                CRDFile_dataGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                CRDFile_dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                CRDFile_dataGridView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                CRDFile_dataGridView.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                CRDFile_dataGridView.ClearSelection();
            }

        }

        public void Update_EphemerisFile()  // Ephemeris File List 업데이트
        {
            if (ephimerisFile_dataGridView.DataSource != null)
            {
                ephimerisFile_dataGridView.Columns.Clear();
                ephimerisFile_dataGridView.DataSource = null;
                EphimerisFile_List.Clear();
            }

            // DataBase 접속 후 Ephemeris File List 읽기
            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        // Ephemeris File 목록 불러오기
                        command.CommandText = "SELECT * FROM nslr_ephemerisfile;";

                        string delete_query = null;
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // 해당 Ephemeris File 실제 존재여부 Check, 없을 시 해당 목록 DataBase에서 삭제
                                FileInfo ephemeris_file = new FileInfo(nslrEphemeris_directory + "/" + reader["file_name"].ToString());
                                if (!ephemeris_file.Exists)
                                {
                                    delete_query += "DELETE FROM nslr_ephemerisfile WHERE file_name = '" + reader["file_name"].ToString() + "';";
                                    continue;
                                }

                                // 목록 추가
                                string satellite = reader["satellite"].ToString();
                                string period_start = reader["period_start"].ToString();
                                string period_end = reader["period_end"].ToString();

                                string[] dateTime_start = period_start.Split(' ');
                                string[] date_start = dateTime_start[0].Split('-');
                                string[] time_start = dateTime_start[1].Split(':');
                                DateTime start_UTC = new DateTime(int.Parse(date_start[0]), int.Parse(date_start[1]), int.Parse(date_start[2]),
                                    int.Parse(time_start[0]), int.Parse(time_start[1]), int.Parse(time_start[2]), DateTimeKind.Utc);

                                string[] dateTime_end = period_end.Split(' ');
                                string[] date_end = dateTime_end[0].Split('-');
                                string[] time_end = dateTime_end[1].Split(':');
                                DateTime end_UTC = new DateTime(int.Parse(date_end[0]), int.Parse(date_end[1]), int.Parse(date_end[2]),
                                    int.Parse(time_end[0]), int.Parse(time_end[1]), int.Parse(time_end[2]), DateTimeKind.Utc);

                                string available = "";
                                if (DateTime.Compare(start_UTC, DateTime.UtcNow) == 1 || DateTime.Compare(DateTime.UtcNow, end_UTC) == 1)   // Ephemeris File 유효기간 : (Period Start) 부터 (Period End) 까지 
                                {
                                    available = "사용 불가";
                                }
                                else
                                {
                                    available = "사용 가능";
                                }

                                EphimerisFile_List.Add(new EphimerisFile_Info(satellite, period_start, period_end, available));
                            }
                        }

                        if (delete_query != null)
                        {
                            command.CommandText = delete_query;
                            command.ExecuteNonQuery();
                        }

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ephemeris File 읽는 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }
            }


            if (EphimerisFile_List.Count > 0)
            {
                ephimerisFile_dataGridView.DataSource = EphimerisFile_List;
                ephimerisFile_dataGridView.Columns["Satellite"].HeaderText = "위성명";
                ephimerisFile_dataGridView.Columns["Available_StartTime"].HeaderText = "유효기간(Start)";
                ephimerisFile_dataGridView.Columns["Available_EndTime"].HeaderText = "유효기간(End)";
                ephimerisFile_dataGridView.Columns["Available"].HeaderText = "Available";

                ephimerisFile_dataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                ephimerisFile_dataGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                ephimerisFile_dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                ephimerisFile_dataGridView.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                ephimerisFile_dataGridView.ClearSelection();
            }
        }

        private void deleteData1_btn_Click(object sender, EventArgs e)  // Observation Data 삭제
        {
            if (observationData_dataGridView.SelectedRows.Count <= 0)
            {
                MessageBox.Show("삭제할 관측데이터를 선택하세요.");
                return;
            }

            // 선택된 행 정보 저장
            string[] cell_data = new string[3];

            for (int i = 0; i < cell_data.Length; i++)
            {
                cell_data[i] = observationData_dataGridView.SelectedRows[0].Cells[i].Value.ToString();
            }

            // Postgres 접속 후 매칭 데이터 불러오기
            string selectedTable_name = "";

            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        command.CommandText = "SELECT * FROM nslr_observationdata WHERE " + "satellite = '" + cell_data[0] + "' AND start_time = '" + cell_data[1] +
                            "' AND end_time = '" + cell_data[2] + "';";
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                selectedTable_name = reader["table_name"].ToString();
                            }
                        }

                        // 선택된 관측데이터 정보 삭제
                        command.CommandText = String.Format("DELETE FROM nslr_observationdata WHERE " + "satellite = '" + cell_data[0] + "' AND start_time = '" + cell_data[1] +
                            "' AND end_time = '" + cell_data[2] + "' AND table_name = '" + selectedTable_name + "';");
                        command.ExecuteNonQuery();

                        // 선택된 관측데이터 Table 삭제
                        command.CommandText = String.Format("DROP TABLE " + selectedTable_name + ";");
                        command.ExecuteNonQuery();

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("CRD 삭제 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }

            }

            // 관측 데이터 List 업데이트
            Update_ObservationData();

            MessageBox.Show("관측 데이터 삭제완료!");
        }

        private void deleteData2_btn_Click(object sender, EventArgs e)  // CRD File 삭제
        {

            if (CRDFile_dataGridView.SelectedRows.Count <= 0)
            {
                MessageBox.Show("삭제할 CRD를 선택하세요.");
                return;
            }

            // 선택된 행 정보 저장
            string[] cell_data = new string[4];
            string file_type = "";

            for (int i = 0; i < cell_data.Length; i++)
            {
                cell_data[i] = CRDFile_dataGridView.SelectedRows[0].Cells[i].Value.ToString();
            }

            if (cell_data[2] == "Full-Rate")
            {
                if (cell_data[3] == "Version1") { file_type = "frd"; }
                else if (cell_data[3] == "Version2") { file_type = "frd2"; }
            }
            else if (cell_data[2] == "Normal-Point")
            {
                if (cell_data[3] == "Version1") { file_type = "npt"; }
                else if (cell_data[3] == "Version2") { file_type = "npt2"; }
            }

            // Postgres 접속 후 매칭 데이터 불러오기
            string selectedFile_name = "";

            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        command.CommandText = "SELECT * FROM nslr_crdfile WHERE " + "satellite = '" + cell_data[0] + "' AND start_time = '" + cell_data[1] + 
                            "' AND file_type = '" + file_type + "';";
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                selectedFile_name = reader["file_name"].ToString();
                            }
                        }

                        // 선택된 CRD 정보 삭제
                        command.CommandText = String.Format("DELETE FROM nslr_crdfile WHERE " + "satellite = '" + cell_data[0] + "' AND start_time = '" + cell_data[1] +
                            "' AND file_type = '" + file_type + "' AND file_name = '" + selectedFile_name + "';");
                        command.ExecuteNonQuery();

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("CRD 삭제 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }

            }

            // 선택된 로컬 CRD 파일 삭제
            DirectoryInfo nslrCRD_directoryInfo = new DirectoryInfo(nslrCRD_directory);
            if (nslrCRD_directoryInfo.Exists == false) { nslrCRD_directoryInfo.Create(); }

            FileInfo nslrCRD_fileInfo = new FileInfo(nslrCRD_directory + "/" + selectedFile_name);

            if (nslrCRD_fileInfo.Exists == true) { nslrCRD_fileInfo.Delete(); }

            // CRD List 업데이트
            Update_CRDFile();

            MessageBox.Show("CRD 삭제완료!");
        }

        private void deleteData3_btn_Click(object sender, EventArgs e)  // Ephemeris File 삭제
        {
            if (ephimerisFile_dataGridView.SelectedRows.Count <= 0)
            {
                MessageBox.Show("삭제할 Ephemeris를 선택하세요.");
                return;
            }

            // 선택된 행 정보 저장
            string[] cell_data = new string[3];
            
            for (int i = 0; i < cell_data.Length; i++)
            {
                cell_data[i] = ephimerisFile_dataGridView.SelectedRows[0].Cells[i].Value.ToString();
            }

            // Postgres 접속 후 매칭 데이터 불러오기
            string selectedFile_name = "";

            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        command.CommandText = "SELECT * FROM nslr_ephemerisfile WHERE " + "satellite = '" + cell_data[0] + "' AND period_start = '" + cell_data[1] +
                            "' AND period_end = '" + cell_data[2] + "';";
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                selectedFile_name = reader["file_name"].ToString();
                            }
                        }

                        // 선택된 CRD 정보 삭제
                        command.CommandText = String.Format("DELETE FROM nslr_ephemerisfile WHERE " + "satellite = '" + cell_data[0] + "' AND period_start = '" + cell_data[1] +
                            "' AND period_end = '" + cell_data[2] + "' AND file_name = '" + selectedFile_name + "';");
                        command.ExecuteNonQuery();

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ephemeris 삭제 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }

            }

            // 선택된 로컬 CRD 파일 삭제
            DirectoryInfo nslrEphemeris_directoryInfo = new DirectoryInfo(nslrEphemeris_directory);
            if (nslrEphemeris_directoryInfo.Exists == false) { nslrEphemeris_directoryInfo.Create(); }

            FileInfo nslrEphemeris_fileInfo = new FileInfo(nslrEphemeris_directory + "/" + selectedFile_name);

            if (nslrEphemeris_fileInfo.Exists == true) { nslrEphemeris_fileInfo.Delete(); }

            // Ephemeris List 업데이트
            Update_EphemerisFile();

            MessageBox.Show("Ephemeris 삭제완료!");
        }


        private void CRDGenerator_btn_Click(object sender, EventArgs e) // CRD Generator Click 시
        {
            if (observationData_dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("관측 데이터를 선택하세요.");
                return;
            }

            if (CRDtype_comboBox.SelectedItem.ToString() == "Full-Rate")
            {
                if (CRDversion_comboBox.SelectedIndex == 0) { ObservationDataToCRD("frd"); }

            }
            else if (CRDtype_comboBox.SelectedItem.ToString() == "Normal-Point")
            {
                if (CRDversion_comboBox.SelectedIndex == 0) { ObservationDataToNormalPoint("npt"); }
                else if (CRDversion_comboBox.SelectedIndex == 1) { ObservationDataToNormalPoint("npt2"); }
            }

        }

        private void EphimerisGenerator_btn_Click(object sender, EventArgs e)   // 궤도결정 + Ephimeris Generator Click 시
        {
            if (CRDFile_dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("CRD File을 선택하세요.");
                return;
            }

            if (LS_checkBox.Checked == true)
            {
                Ready_LeastSquare();
            }
            else if (EKF_checkBox.Checked == true)
            {
                MessageBox.Show("구현 안됨.");
            }
            else
            {
                MessageBox.Show("궤도결정 Type을 선택하세요.");
                return;
            }

        }

        Thread orbitDetermination_thread;

        public void Ready_LeastSquare()
        {
            if (orbitDetermination_thread != null)
            {
                if (orbitDetermination_thread.IsAlive == true)
                {
                    MessageBox.Show("이미 궤도결정 수행 중입니다.");
                    return;
                }
                orbitDetermination_thread = null;
            }

            // DateTimePicker 에서 Time Range Check
            DateTime period_start = new DateTime(periodDateStart_dateTimePicker.Value.Year, periodDateStart_dateTimePicker.Value.Month, periodDateStart_dateTimePicker.Value.Day, 
                periodTimeStart_dateTimePicker.Value.Hour, periodTimeStart_dateTimePicker.Value.Minute, 0, DateTimeKind.Utc);
            DateTime period_end = new DateTime(periodDateEnd_dateTimePicker.Value.Year, periodDateEnd_dateTimePicker.Value.Month, periodDateEnd_dateTimePicker.Value.Day, 
                periodTimeEnd_dateTimePicker.Value.Hour, periodTimeEnd_dateTimePicker.Value.Minute, 0, DateTimeKind.Utc);
            string dateTime_str_ = CRDFile_dataGridView.SelectedRows[0].Cells[1].Value.ToString();
            string[] dateTime_array_ = dateTime_str_.Split(' ');
            string[] date_array_ = dateTime_array_[0].Split('-');
            string[] time_array_ = dateTime_array_[1].Split(':');
            DateTime range_start = new DateTime(int.Parse(date_array_[0]), int.Parse(date_array_[1]), int.Parse(date_array_[2]),
                int.Parse(time_array_[0]), int.Parse(time_array_[1]), 0, DateTimeKind.Utc);
            DateTime range_end = range_start.AddHours(24);

            if (DateTime.Compare(range_start, period_start) == 1 || DateTime.Compare(period_end, range_end) == 1)
            {
                MessageBox.Show("Ephemeris Period를 다시 설정하세요." + "\r\n" + "* 생성가능범위 : (관측일자) ~ (관측일자 + 1일)");
                return;
            }

            // 선탹된 DataGridView Row 정보 Get
            string search_sat = CRDFile_dataGridView.SelectedRows[0].Cells[0].Value.ToString();
            string search_time = CRDFile_dataGridView.SelectedRows[0].Cells[1].Value.ToString();

            string search_fileType = "";
            string search_type = CRDFile_dataGridView.SelectedRows[0].Cells[2].Value.ToString();
            string search_version = CRDFile_dataGridView.SelectedRows[0].Cells[3].Value.ToString();
            if (search_type == "Full-Rate")
            {
                if (search_version == "Version1") { search_fileType = "frd"; }
                else if (search_version == "Version2") { search_fileType = "frd2"; }
            }
            else if (search_type == "Normal-Point")
            {
                if (search_version == "Version1") { search_fileType = "npt"; }
                else if (search_version == "Version2") { search_fileType = "npt2"; }
            }

            string orbit_information = "";
            string crdFile_name = "";

            // DataBase 접속 후 Ephemeris 중복 Check & CRD 정보 읽기
            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        // Ephemeris File 목록 상, 중복 Check
                        command.CommandText = "SELECT * FROM nslr_ephemerisfile WHERE satellite = '" + search_sat + "' AND observation_time = '" + search_time +
                            "' AND period_start = '" + period_start.ToString("yyyy-MM-dd HH:mm:ss") + "' AND period_end = '" + period_end.ToString("yyyy-MM-dd HH:mm:ss") +
                            "' AND crd_type = '" + search_fileType + "' AND applied_model = '" + "ls" + "';";

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MessageBox.Show("이미 중복되는 Ephemeris File이 존재합니다.");
                                connect.Close();
                                return;
                            }
                        }

                        // CRD File 목록 불러오기
                        command.CommandText = "SELECT * FROM nslr_crdfile WHERE satellite = '" + search_sat + "' AND start_time = '" + search_time + "' AND file_type = '" + 
                            search_fileType + "';";

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            
                            while (reader.Read())
                            {
                                orbit_information = reader["orbit_information"].ToString();
                                crdFile_name = reader["file_name"].ToString();
                            }
                        }

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("CRD 정보 읽는 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }
            }

            // Epoch Time 산출
            string[] dateTime_array = search_time.Split(' ');
            string[] date_array = dateTime_array[0].Split('-');
            string[] time_array = dateTime_array[1].Split(':');
            double[] observation_time = { Convert.ToDouble(date_array[0]), Convert.ToDouble(date_array[1]), Convert.ToDouble(date_array[2]), 
                Convert.ToDouble(time_array[0]), Convert.ToDouble(time_array[1]), Convert.ToDouble(time_array[2]) };

            // Ephemeris 산출
            string[] ephemeris_vector_str = orbit_information.Split('/');
            double[] ephemeris_vector = new double[6];
            for (int i = 0; i < ephemeris_vector.Length; i++) { ephemeris_vector[i] = Convert.ToDouble(ephemeris_vector_str[i]); }


            // 궤도결정(Least Square) 수행 + Ephemeris File 생성
            orbitDetermination_thread = new Thread(() =>
            {
                // Start
                this.Invoke(new System.Action(() =>
                {
                    EphimerisGenerator_btn.Text = "Generating...";
                    EphimerisGenerator_btn.Enabled = false;
                    EphimerisGenerator_btn.Update();
                }));

                // 궤도결정(Least Square) 수행
                Run_LeastSquare1(nslrCRD_directory + "/" + crdFile_name, observation_time, ephemeris_vector);

                // Ephemeris File 생성
                Generator_EphemerisFile(search_sat, search_time, period_start, period_end, search_fileType, "ls");

                // End
                this.Invoke(new System.Action(() =>
                {
                    // Ephemeris File List 업데이트
                    Update_EphemerisFile();

                    EphimerisGenerator_btn.Text = "Generator";
                    EphimerisGenerator_btn.Enabled = true;
                    EphimerisGenerator_btn.Update();
                }));
            });
            orbitDetermination_thread.Start();
        }

        public void Run_LeastSquare1(string CRD_Path, double[] observation_time, double[] state_vector)  // 가중최소자승법 기반 궤도결정 (1)
        {
            // CRD File 입력
            StringBuilder CRDFile = new StringBuilder(CRD_Path);
            LoadObservation(Global.obsClass, CRDFile);

            // 관측소 이름
            StringBuilder siteName = new StringBuilder();
            int siteCode = GetObservatoryInfo(Global.obsClass, 0, siteName);
            
            // 데이터 갯수
            int nObs = GetNObsData(Global.obsClass, 0);

            // Start/End 시간
            double[] obsTimeSet = new double[2];
            GetObservationTime(Global.obsClass, 0, obsTimeSet);
            double[] startDate = new double[6];
            double[] finalTime = new double[6];
            ConvertMJD2Greg(Global.timeSys, obsTimeSet[0], startDate);
            ConvertMJD2Greg(Global.timeSys, obsTimeSet[1]/*obsTimeSet[0] + 20.0 / 86400*/, finalTime);
            
            // 초기궤도정보 입력
            double[] stateVec = state_vector;
            double[] epochDate = observation_time;
            StringBuilder type = new StringBuilder("Cartesian");
            StringBuilder coord = new StringBuilder("EarthMJ2000Eq");
            StringBuilder epochType = new StringBuilder("Gregorian");
            StringBuilder epochSys = new StringBuilder("UTC");
            SetOrbit(Global.sat, stateVec, type, coord, epochType, epochSys, epochDate);
            UpdateParameterSatellite(Global.paramClass, Global.sat);

            // 관측소 정보 + 관측 시간 입력
            SetObservatory(Global.estClass, siteCode, siteName);
            bool[] obsIdx = { true, true };
            SetObservable(Global.estClass, obsIdx);
            SetParameterEpochTime(Global.paramClass, epochDate);
            epochType = new StringBuilder("start");
            SetEstimatorTime(Global.estClass, epochType, startDate);
            epochType = new StringBuilder("end");
            SetEstimatorTime(Global.estClass, epochType, finalTime);

            // 가중최소자승법
            int flag = Run_LeastSquare2();
        }

        public int Run_LeastSquare2() // 가중최소자승법 기반 궤도결정 (2) : 8.13 Ver.
        {
            // 해당 시점에서 State 전시
            this.Invoke(new System.Action(() => { state_label.Text = "State  :  LeastSquaresInitializer..."; }));

            // initialize
            int nSolveforParams = 6;
            double[] aprioriSolveforVec = new double[nSolveforParams];
            double[] aprioriCov = new double[nSolveforParams];
            StringBuilder type = new StringBuilder();
            GetOrbitalState(Global.sat, aprioriSolveforVec, type);

            IntPtr estObsClass = CreateObservation();
            int iterMax = LeastSquaresInitializer(Global.estClass, Global.paramClass, Global.obsClass, estObsClass, Global.dynModel, aprioriSolveforVec, aprioriCov);

            double[,] totalSolveforVec = new double[iterMax + 1, nSolveforParams];  // 8.13 Ver.
            for (int i = 0; i < nSolveforParams; i++)
            {
                totalSolveforVec[0, i] = aprioriSolveforVec[i];
            }

            // 해당 시점에서 State 전시 (0)
            this.Invoke(new System.Action(() => { state_label.Text = "State  :  LeastSquaresIterator...(0)"; }));

            // initial guess
            double[] solveforVec = new double[nSolveforParams];
            double[] deltaX = new double[nSolveforParams];
            double oldCostFnc = LeastSquaresIterator(Global.estClass, Global.sat, Global.paramClass, estObsClass, Global.dynModel, Global.meaModel,
                aprioriSolveforVec, aprioriCov, Global.residual, deltaX);

            int[] info = new int[2];
            GetResidualDataInfo(Global.residual, info);
            double[] rms = new double[info[1]];
            GetResidualRMS(Global.residual, rms);

            double[] zeros = { 0, 0, 0, 0, 0, 0 };

            // Main iteration
            int estFlag = 9999;
            double costFnc;
            bool divFlag = false;
            bool apply;
            double[] oldrms = new double[info[1]];
            double[] oldDeltaX = new double[nSolveforParams];

            StringBuilder coord = new StringBuilder("EarthMJ2000Eq");
            for (int iter = 1; iter <= iterMax; iter++)
            {
                apply = ApplyEstimation(Global.estClass, Global.sat, Global.paramClass, aprioriCov, deltaX);

                if (!apply)
                {
                    estFlag = 2;
                    break;
                }

                // 해당 시점에서 State 전시 (iter)
                this.Invoke(new System.Action(() => { state_label.Text = "State  :  LeastSquaresIterator...(" + iter.ToString() + ")"; }));

                oldDeltaX = deltaX;
                GetOrbitalState(Global.sat, solveforVec, type);
                for (int i = 0; i < nSolveforParams; i++)
                {
                    totalSolveforVec[iter, i] = solveforVec[i];
                }

                costFnc = LeastSquaresIterator(Global.estClass, Global.sat, Global.paramClass, estObsClass, Global.dynModel, Global.meaModel,
                    aprioriSolveforVec, aprioriCov, Global.residual, deltaX);

                GetResidualRMS(Global.residual, rms);

                // Converge test
                estFlag = ConvergenceTest(Global.estClass, costFnc, oldCostFnc, iter, ref divFlag);

                if (estFlag == 10)          // converging
                {
                    oldCostFnc = costFnc;
                    oldrms = rms;
                    continue;
                }
                else if (estFlag == 1)      // converged
                    break;
                else if (estFlag == 0)      // diverged
                    break;

            }

            return estFlag;
        }

        public void Generator_EphemerisFile(string satellite, string observation_time, DateTime period_start, DateTime period_end, string crd_type, string model)   // Ephemeris File 생성
        {
            // 해당 시점에서 State 전시 (Generating Ephemeris)
            this.Invoke(new System.Action(() => { state_label.Text = "State  :  Creating Ephemeris File..."; }));

            // Ephemeris File 생성
            double[] epochDate = new double[6];
            double mjd = GetEpochTime(Global.sat, epochDate);

            double[] startTime = { (double)period_start.Year, (double)period_start.Month, (double)period_start.Day, (double)period_start.Hour, (double)period_start.Minute, (double)period_start.Second };
            double[] endTime = { (double)period_end.Year, (double)period_end.Month, (double)period_end.Day, (double)period_end.Hour, (double)period_end.Minute, (double)period_end.Second };

            double startMJD = ConvertGreg2MJD(Global.timeSys, startTime);
            double finalMJD = ConvertGreg2MJD(Global.timeSys, endTime);

            double stepSize = 60.0;

            string[] dateTime_array = observation_time.Split(' ');
            string[] date_array = dateTime_array[0].Split('-');
            string[] time_array = dateTime_array[1].Split(':');
            DateTime dateTime = new DateTime(int.Parse(date_array[0]), int.Parse(date_array[1]), int.Parse(date_array[2]), int.Parse(time_array[0]), int.Parse(time_array[1]), int.Parse(time_array[2]), DateTimeKind.Utc);

            string fileName_str = "nslrEphemeris" + "_" + (string.Concat(satellite.Where(x => !char.IsWhiteSpace(x)))).ToLower() + "_" + 
                dateTime.ToString("yyyyMMddHHmmss") + "_" + period_start.ToString("yyyyMMddHHmmss") + "_" + period_end.ToString("yyyyMMddHHmmss") + "_" + crd_type + "_" + model + ".txt";
            StringBuilder fileName = new StringBuilder(fileName_str);

            GenerateEphemeris(Global.dynModel, Global.sat, startMJD, finalMJD, stepSize, fileName);


            // Database : Ephemeris File Table 추가
            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        // Ephemeris File 정보 Table 존재여부 Check
                        command.CommandText = "SELECT * FROM pg_catalog.pg_tables WHERE schemaname = 'public';";
                        bool exist_table = false;
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["tablename"].ToString() == "nslr_ephemerisfile")
                                {
                                    exist_table = true;
                                    break;
                                }
                            }
                        }

                        // Ephemeris File 정보 Table 없을 시 생성
                        if (exist_table == false)
                        {
                            command.CommandText = "CREATE TABLE nslr_ephemerisfile (                 " +
                                                  "         satellite               varchar(50),     " +
                                                  "         observation_time        varchar(50),     " +
                                                  "         period_start            varchar(50),     " +
                                                  "         period_end              varchar(50),     " +
                                                  "         crd_type                varchar(50),     " +
                                                  "         applied_model           varchar(50),     " +
                                                  "         file_name               varchar(150)      " +
                                                  ");                                                ";
                            command.ExecuteNonQuery();
                        }

                        // 관측 데이터 List 입력
                        command.CommandText = String.Format("INSERT INTO nslr_ephemerisfile VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');",
                            satellite, observation_time, period_start.ToString("yyyy-MM-dd HH:mm:ss"), period_end.ToString("yyyy-MM-dd HH:mm:ss"), crd_type, model, fileName_str);
                        command.ExecuteNonQuery();

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ephemeris File 정보 추가 중 오류발생!");
                }
                finally
                {
                    connect.Close();
                }
            }

            // 해당 시점에서 State 전시 (Generator Ephemeris)
            this.Invoke(new System.Action(() => { state_label.Text = "State  :  Complete LeastSquares \r\n && Ephemeris File"; }));

            
        }

        private void scheduleUpdate_btn_Click(object sender, EventArgs e)
        {
            UpdateSchedule();
        }

        List<Tuple<string, string, DateTime, DateTime>> ephemeris_tuples = new List<Tuple<string, string, DateTime, DateTime>>();
        List<MyTask> myTasks_ephemeris = new List<MyTask>();
        List<EphemerisData_Info> ephemerisData_infos = new List<EphemerisData_Info>();

        Brush[] total_brush_ephemeris = new Brush[3];
        public void UpdateSchedule()
        {
            // Ephemeris_GanttChart + EphemerisData_DataGridView 초기화
            _mManager_ephemeris = new ProjectManager();
            _mManager_ephemeris.Start = schedule_startTime;
            _mChart_ephemeris.Init(_mManager_ephemeris);
            _mChart_ephemeris.Invalidate();

            ephemeris_tuples.Clear();
            myTasks_ephemeris.Clear();
            ephemerisData_infos.Clear();

            if (ephemerisData_dataGridView.DataSource != null)
            {
                ephemerisData_dataGridView.Columns.Clear();
                ephemerisData_dataGridView.DataSource = null;
            }

            total_brush_ephemeris[0] = Brushes.Red;
            total_brush_ephemeris[1] = Brushes.Black;
            total_brush_ephemeris[2] = Brushes.Yellow;

            List<string[]> ephemeris_table = new List<string[]>();

            // Database : Ephemeris File List 불러오기
            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        command.CommandText = "SELECT * FROM nslr_ephemerisfile;";
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ephemeris_table.Add(new string[] { reader["satellite"].ToString(), reader["observation_time"].ToString(), reader["period_start"].ToString(),
                            reader["period_end"].ToString(), reader["crd_type"].ToString(), reader["applied_model"].ToString(), reader["file_name"].ToString() });
                            }
                        }

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ephemeris File 정보 추가 중 오류발생!");
                    connect.Close();
                    return;
                }
            }

            for (int i = 0; i < ephemeris_table.Count; i++)
            {
                string file_fullPath = Path.GetFullPath("./Outputs/Ephemeris/" + ephemeris_table[i][6]);

                string[] startStr = ephemeris_table[i][2].Split(' ');
                string[] startDate = startStr[0].Split('-');
                string[] startTime = startStr[1].Split(':');
                DateTime period_start = new DateTime(int.Parse(startDate[0]), int.Parse(startDate[1]), int.Parse(startDate[2]), int.Parse(startTime[0]),
                    int.Parse(startTime[1]), int.Parse(startTime[2]), DateTimeKind.Utc);

                string[] endStr = ephemeris_table[i][3].Split(' ');
                string[] endDate = endStr[0].Split('-');
                string[] endTime = endStr[1].Split(':');
                DateTime period_end = new DateTime(int.Parse(endDate[0]), int.Parse(endDate[1]), int.Parse(endDate[2]), int.Parse(endTime[0]),
                    int.Parse(endTime[1]), int.Parse(endTime[2]), DateTimeKind.Utc);

                DateTime display_startUTC = new DateTime();
                DateTime display_endUTC = new DateTime();
                int display_duration = 0;
                
                if (DateTime.Compare(period_start, schedule_startTime.ToUniversalTime()) == -1 && 
                    (DateTime.Compare(schedule_startTime.ToUniversalTime(), period_end) == -1 && 
                    (DateTime.Compare(period_end, schedule_endTime.ToUniversalTime()) == -1 || DateTime.Compare(period_end, schedule_endTime.ToUniversalTime()) == 0)))
                {
                    // 시작 : schedule_startTime
                    // 끝 : period_end
                    display_startUTC = schedule_startTime.ToUniversalTime();
                    display_endUTC = period_end;
                    display_duration = (int)((display_endUTC - display_startUTC).TotalMinutes);
                }
                else if (((DateTime.Compare(schedule_startTime.ToUniversalTime(), period_start) == 0 || DateTime.Compare(schedule_startTime.ToUniversalTime(), period_start) == -1) && 
                    DateTime.Compare(period_start, schedule_endTime.ToUniversalTime()) == -1) && 
                    DateTime.Compare(schedule_endTime.ToUniversalTime(), period_end) == -1)
                {
                    // 시작 : period_start
                    // 끝 : schedule_endTime
                    display_startUTC = period_start;
                    display_endUTC = schedule_endTime.ToUniversalTime();
                    display_duration = (int)((display_endUTC - display_startUTC).TotalMinutes);
                }
                else if ((DateTime.Compare(schedule_startTime.ToUniversalTime(), period_start) == 0 || DateTime.Compare(schedule_startTime.ToUniversalTime(), period_start) == -1) && 
                    (DateTime.Compare(period_end, schedule_endTime.ToUniversalTime()) == -1 || DateTime.Compare(period_end, schedule_endTime.ToUniversalTime()) == 0))
                {
                    // 시작 : period_start
                    // 끝 : period_end
                    display_startUTC = period_start;
                    display_endUTC = period_end;
                    display_duration = (int)((display_endUTC - display_startUTC).TotalMinutes);
                }
                else if (DateTime.Compare(period_start, schedule_startTime.ToUniversalTime()) == -1 && // 추가
                    DateTime.Compare(schedule_endTime.ToUniversalTime(), period_end) == -1)
                {
                    // 시작 : schedule_start
                    // 끝 : schedule_end
                    display_startUTC = schedule_startTime.ToUniversalTime();
                    display_endUTC = schedule_endTime.ToUniversalTime();
                    display_duration = (int)((display_endUTC - display_startUTC).TotalMinutes);
                }
                else
                {
                    // 범위에 없음
                    continue;
                }

                // Task 생성
                MyTask myTask = new MyTask(_mManager_ephemeris) { Name = ephemeris_table[i][0] };

                // Split 갯수 및 색상 설정
                int division_count = 0;
                int[] temp_duration = new int[display_duration];
                System.Drawing.Brush[] temp_brush = new System.Drawing.Brush[display_duration];
                
                // 시작시간 Matching 변수(태양회피 등)
                int ephemeris_sunAvoidance = (int)((display_startUTC - schedule_startTime.ToUniversalTime()).TotalMinutes);

                // 스케줄러 Filtering 구간 내 추출 가능한 데이터(Ephemeris 고려)만 확보
                double[][] tracking_data = EphemerisFileToTrackingData(file_fullPath, display_startUTC, display_endUTC, 60.0, 0);

                // 최대고각
                double max_elevation = 0.0;

                for (int j = 0; j < display_duration; j++)
                {
                    // 최대고각 Set
                    if (j > 0)
                    {
                        if (tracking_data[1][j] > max_elevation) { max_elevation = tracking_data[1][j]; }
                    }
                    else { max_elevation = tracking_data[1][j]; }

                    if (tracking_data[1][j] > 85.0 || tracking_data[1][j] < 20.0)
                    {
                        // 관측 불가 : 고각 제한
                        if (j > 0)
                        {
                            if (temp_brush[division_count] == total_brush_ephemeris[1])
                            {
                                temp_duration[division_count]++;
                            }
                            else
                            {
                                division_count++;
                                temp_duration[division_count] = 1;
                                temp_brush[division_count] = total_brush_ephemeris[1];
                            }
                        }
                        else if (j == 0)
                        {
                            temp_duration[division_count] = 1;
                            temp_brush[division_count] = total_brush_ephemeris[1];
                        }
                    }
                    else if ((check_sunAvoidance[ephemeris_sunAvoidance + j] == true && Filtering_SunPosition(sunAZEL[0][ephemeris_sunAvoidance + j], sunAZEL[1][ephemeris_sunAvoidance + j], 
                        tracking_data[0][j], tracking_data[1][j]) < 30.0) ||
                        (check_sunAvoidance[ephemeris_sunAvoidance + j] == false && Filtering_SunPosition(moonAZEL[0][ephemeris_sunAvoidance + j], moonAZEL[1][ephemeris_sunAvoidance + j], 
                        tracking_data[0][j], tracking_data[1][j]) < 30.0))
                    {
                        // 관측 불가 : 태양/달 회피
                        if (j > 0)
                        {
                            if (temp_brush[division_count] == total_brush_ephemeris[0])
                            {
                                temp_duration[division_count]++;
                            }
                            else
                            {
                                division_count++;
                                temp_duration[division_count] = 1;
                                temp_brush[division_count] = total_brush_ephemeris[0];
                            }
                        }
                        else if (j == 0)
                        {
                            temp_duration[division_count] = 1;
                            temp_brush[division_count] = total_brush_ephemeris[0];
                        }
                    }
                    else
                    {
                        // 관측 가능
                        if (j > 0)
                        {
                            if (temp_brush[division_count] == total_brush_ephemeris[2])
                            {
                                temp_duration[division_count]++;
                            }
                            else
                            {
                                division_count++;
                                temp_duration[division_count] = 1;
                                temp_brush[division_count] = total_brush_ephemeris[2];
                            }
                        }
                        else if (j == 0)
                        {
                            temp_duration[division_count] = 1;
                            temp_brush[division_count] = total_brush_ephemeris[2];
                        }
                    }
                }

                // Task Duration/Brush Setting
                int[] task_duration = new int[division_count + 1];
                Brush[] task_brush = new Brush[division_count + 1];
                Array.Copy(temp_duration, task_duration, task_duration.Length);
                Array.Copy(temp_brush, task_brush, task_brush.Length);
                Split_InterfaceFunction(_mManager_ephemeris, myTask, task_duration, task_brush);

                // Task 추가
                myTasks_ephemeris.Add(myTask);
                _mManager_ephemeris.Add(myTask);
                _mManager_ephemeris.SetStart(myTask, TimeSpan.FromMinutes(ephemeris_sunAvoidance));
                _mManager_ephemeris.SetDuration(myTask, TimeSpan.FromMinutes(display_duration));

                // DataGridView 정보 생성 + 고도 추출
                double[] target_value = { (double)display_startUTC.Year, (double)display_startUTC.Month, (double)display_startUTC.Day,
                        (double)display_startUTC.Hour, (double)display_startUTC.Minute, (double)display_startUTC.Second};
                double target_MJD = ConvertGreg2MJD(Global.timeSys, target_value);

                StringBuilder ephemerisFile = new StringBuilder(file_fullPath);

                double[] command = new double[3];   // command[2] : 고도

                GenerateLLA_Ephemeris(ephemerisFile, target_MJD, command);


                EphemerisData_Info ephemerisData_info = new EphemerisData_Info(ephemeris_table[i][0], command[2].ToString() + " km", max_elevation.ToString());
                ephemerisData_infos.Add(ephemerisData_info);

                // DataGridView 내 각 위성 정보 보관
                ephemeris_tuples.Add(Tuple.Create(ephemeris_table[i][0], file_fullPath, display_startUTC, display_endUTC));
            }

            // GanttChart 전시
            _mManager_ephemeris.Start = schedule_startTime;
            _mChart_ephemeris.Init(_mManager_ephemeris);
            _mChart_ephemeris.Invalidate();

            _mChart_ephemeris.CreateTaskDelegate = delegate () { return new MyTask(_mManager_ephemeris); };
            _mChart_ephemeris.AllowTaskDragDrop = false;

            _mChart_ephemeris.TimeResolution = TimeResolution.Minute;

            // DataGridView 전시
            if (ephemerisData_infos.Count > 0)
            {
                ephemerisData_dataGridView.DataSource = ephemerisData_infos;
                ephemerisData_dataGridView.Columns["Satellite"].HeaderText = "위성명";
                ephemerisData_dataGridView.Columns["Altitude"].HeaderText = "고도(Altitude)";
                ephemerisData_dataGridView.Columns["MaxElevation"].HeaderText = "최대고각(MaxElevation)";

                ephemerisData_dataGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                ephemerisData_dataGridView.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                ephemerisData_dataGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                ephemerisData_dataGridView.ClearSelection();
            }

        }

        private void EphimerisGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < ephemerisData_dataGridView.Rows.Count)
            {
                // taskGridView 및 CPFGridView 선택 해제
                taskGridView.ClearSelection();
                CPFGridView.ClearSelection();

                // GanttChart 내 해당위성 선택
                _mChart_ephemeris.ScrollTo(myTasks_ephemeris[e.RowIndex]);

                // Ground Track 및 Sky View 전시
                EphimerisGridView_CellClick_Event(e.RowIndex);

                // Reference Data 관련요소 초기화 및 Set
                Initialize_Reference();
                selectedSatellite_label.Text = "Ephemeris : " + myTasks_ephemeris[e.RowIndex].Name;
            }

        }

        List<Point[]> total_points_ephemeris = new List<Point[]>();
        List<Color> total_color_ephemeris = new List<Color>();
        public void EphimerisGridView_CellClick_Event(int dataGridView_rowIndex)
        {
            // 위성/우주물체 정보 초기화
            Initialize_Satellite_Information();

            // GroundTrack 및 SkyView 초기화
            GroundTrack.Image = null;
            GroundTrack.Update();
            ChartPointClear();

            double[][] LLA_datas = EphemerisFileToTrackingData(ephemeris_tuples[dataGridView_rowIndex].Item2, ephemeris_tuples[dataGridView_rowIndex].Item3,
                ephemeris_tuples[dataGridView_rowIndex].Item4, 60.0, 3);

            Color current_color = Color.White;
            int graphics_count = 0;
            int stackData_count = 0; int subData_count = 0;

            // points/colors List 초기화
            total_points_ephemeris.Clear();
            total_color_ephemeris.Clear();

            for (int i = 0; i < LLA_datas[0].Length; i++)
            {
                stackData_count++;

                if (i > 0)
                {
                    if (((LLA_datas[0][i] < 220.0 && LLA_datas[0][i] >= 180.0) && (LLA_datas[0][i - 1] > 140.0 && LLA_datas[0][i - 1] < 180.0)) ||
                            ((LLA_datas[0][i] > 140.0 && LLA_datas[0][i] < 180.0) && (LLA_datas[0][i - 1] >= 180.0 && LLA_datas[0][i - 1] < 220.0)))
                    {
                        if (myTasks_ephemeris[dataGridView_rowIndex].PartColor[graphics_count] == total_brush_ephemeris[0]) { current_color = Color.Red; }
                        else if (myTasks_ephemeris[dataGridView_rowIndex].PartColor[graphics_count] == total_brush_ephemeris[1]) { current_color = Color.White/*Color.Black*/; }
                        else if (myTasks_ephemeris[dataGridView_rowIndex].PartColor[graphics_count] == total_brush_ephemeris[2]) { current_color = Color.Yellow; }

                        // Ground Track 전시 데이터 Set
                        if ((stackData_count - subData_count - 1) > 1)
                        {
                            /////////////////////// 레이아웃 변환 시 공백 채우기용 /////////////////////
                            if (subData_count == 0)
                            {
                                // 끝 부분만
                                Point[] points = new Point[stackData_count - subData_count - 1 + 1];

                                for (int j = 0; j < points.Length - 1; j++)
                                {
                                    points[j] = MercatorProjection(LLA_datas[0][(i + 1) - points.Length + j], LLA_datas[1][(i + 1) - points.Length + j]);
                                }

                                if (LLA_datas[0][i - 1] < 180)
                                {
                                    points[points.Length - 1] = MercatorProjection(179.9, LLA_datas[1][i - 1]);
                                }
                                else // (azelDatas[0][i - 1] >= 180)
                                {
                                    points[points.Length - 1] = MercatorProjection(180.0, LLA_datas[1][i - 1]);
                                }
                                total_points_ephemeris.Add(points);
                            }
                            else
                            {
                                // 시작 + 끝 부분
                                Point[] points = new Point[1 + stackData_count - subData_count - 1 + 1];

                                for (int j = 1; j < points.Length - 1; j++)
                                {
                                    points[j] = MercatorProjection(LLA_datas[0][(i + 1 + 1) - points.Length + (j - 1)], LLA_datas[1][(i + 1 + 1) - points.Length + (j - 1)]);
                                }

                                if (LLA_datas[0][(i + 1 + 1) - points.Length] < 180)
                                {
                                    points[0] = MercatorProjection(179.9, LLA_datas[1][(i + 1 + 1) - points.Length]);
                                }
                                else
                                {
                                    points[0] = MercatorProjection(180.0, LLA_datas[1][(i + 1 + 1) - points.Length]);
                                }
                                if (LLA_datas[0][i - 1] < 180)
                                {
                                    points[points.Length - 1] = MercatorProjection(179.9, LLA_datas[1][i - 1]);
                                }
                                else
                                {
                                    points[points.Length - 1] = MercatorProjection(180.0, LLA_datas[1][i - 1]);
                                }
                                total_points_ephemeris.Add(points);
                            }

                            Color color = current_color;
                            total_color_ephemeris.Add(color);

                        }

                        // subData 설정
                        subData_count = stackData_count - 1;
                    }
                }

                // 궤도 색상 구분 구현
                if (/*((i + 1) % 60 == 0) && */(i != 0))
                {
                    // 색상 변경여부 Checking
                    if ((stackData_count/* / 60*/) == myTasks_ephemeris[dataGridView_rowIndex].PartDuration[graphics_count])
                    {
                        if (myTasks_ephemeris[dataGridView_rowIndex].PartColor[graphics_count] == total_brush_ephemeris[0]) { current_color = System.Drawing.Color.Red; }
                        else if (myTasks_ephemeris[dataGridView_rowIndex].PartColor[graphics_count] == total_brush_ephemeris[1]) { current_color = System.Drawing.Color.White/*Color.Black*/; }
                        else if (myTasks_ephemeris[dataGridView_rowIndex].PartColor[graphics_count] == total_brush_ephemeris[2]) { current_color = System.Drawing.Color.Yellow; }

                        // Ground Track 전시 데이터 Set
                        if ((stackData_count - subData_count) > 1)
                        {
                            //////////////// 그래프 사이 간격 채우기용 ///////////////////


                            /////////////////////// 레이아웃 변환 시 공백 채우기용 /////////////////////
                            if (subData_count == 0)
                            {


                                if ((i + 1 == LLA_datas[0].Length) ||
                                    ((LLA_datas[0][i + 1] < 220.0 && LLA_datas[0][i + 1] >= 180.0) && (LLA_datas[0][i] > 140.0 && LLA_datas[0][i] < 180.0)) ||
                                    ((LLA_datas[0][i + 1] > 140.0 && LLA_datas[0][i + 1] < 180.0) && (LLA_datas[0][i] >= 180.0 && LLA_datas[0][i] < 220.0)))
                                {
                                    Point[] points = new Point[stackData_count - subData_count];

                                    for (int j = 0; j < points.Length; j++)
                                    {
                                        points[j] = MercatorProjection(LLA_datas[0][(i + 1) - points.Length + j], LLA_datas[1][(i + 1) - points.Length + j]);
                                    }
                                    total_points_ephemeris.Add(points);
                                }
                                else
                                {
                                    Point[] points = new Point[stackData_count - subData_count + 1];
                                    for (int j = 0; j < points.Length - 1; j++)
                                    {
                                        points[j] = MercatorProjection(LLA_datas[0][(i + 1 + 1) - points.Length + j], LLA_datas[1][(i + 1 + 1) - points.Length + j]);
                                    }

                                    points[points.Length - 1] = MercatorProjection(LLA_datas[0][i + 1], LLA_datas[1][i + 1]);
                                    total_points_ephemeris.Add(points);
                                }
                            }
                            else
                            {
                                // 시작 부분 추가
                                if ((i + 1 == LLA_datas[0].Length) ||
                                    ((LLA_datas[0][i + 1] < 220.0 && LLA_datas[0][i + 1] >= 180.0) && (LLA_datas[0][i] > 140.0 && LLA_datas[0][i] < 180.0)) ||
                                    ((LLA_datas[0][i + 1] > 140.0 && LLA_datas[0][i + 1] < 180.0) && (LLA_datas[0][i] >= 180.0 && LLA_datas[0][i] < 220.0)))
                                {
                                    Point[] points = new Point[1 + stackData_count - subData_count];

                                    for (int j = 1; j < points.Length; j++)
                                    {
                                        points[j] = MercatorProjection(LLA_datas[0][(i + 1 + 1) - points.Length + (j - 1)], LLA_datas[1][(i + 1 + 1) - points.Length + (j - 1)]);
                                    }

                                    if (LLA_datas[0][(i + 1 + 1) - points.Length] < 180)
                                    {
                                        points[0] = MercatorProjection(179.9, LLA_datas[1][(i + 1 + 1) - points.Length]);
                                    }
                                    else
                                    {
                                        points[0] = MercatorProjection(180.0, LLA_datas[1][(i + 1 + 1) - points.Length]);
                                    }
                                    total_points_ephemeris.Add(points);
                                }
                                else
                                {
                                    Point[] points = new Point[1 + stackData_count - subData_count + 1];

                                    for (int j = 1; j < points.Length - 1; j++)
                                    {
                                        points[j] = MercatorProjection(LLA_datas[0][(i + 1 + 1 + 1) - points.Length + (j - 1)], LLA_datas[1][(i + 1 + 1 + 1) - points.Length + (j - 1)]);
                                    }

                                    if (LLA_datas[0][(i + 1 + 1 + 1) - points.Length] < 180)
                                    {
                                        points[0] = MercatorProjection(179.9, LLA_datas[1][(i + 1 + 1 + 1) - points.Length]);
                                    }
                                    else
                                    {
                                        points[0] = MercatorProjection(180.0, LLA_datas[1][(i + 1 + 1 + 1) - points.Length]);
                                    }
                                    points[points.Length - 1] = MercatorProjection(LLA_datas[0][i + 1], LLA_datas[1][i + 1]);
                                    total_points_ephemeris.Add(points);
                                }

                            }

                            Color color = current_color;
                            total_color_ephemeris.Add(color);

                        }

                        // graphics 카운팅 + stackData 갯수 초기화
                        graphics_count++;
                        stackData_count = 0;
                        if (subData_count != 0) { subData_count = 0; }

                    }
                }



            }

            // Ground Track 전시
            for (int i = 0; i < total_points_ephemeris.Count; i++)
            {
                Graphics graphics = GroundTrack.CreateGraphics();
                Pen pen = new Pen(total_color_ephemeris[i], 4f);
                graphics.DrawCurve(pen, total_points_ephemeris[i]);
            }

            // SkyView 전시
            int sky_count = 0;
            for (int i = 0; i < myTasks_ephemeris[dataGridView_rowIndex].PartDuration.Length; i++)
            {
                double[] sky_longitude = new double[myTasks_ephemeris[dataGridView_rowIndex].PartDuration[i]];
                double[] sky_latitude = new double[myTasks_ephemeris[dataGridView_rowIndex].PartDuration[i]];
                Array.Copy(LLA_datas[0], sky_count, sky_longitude, 0, sky_longitude.Length);
                Array.Copy(LLA_datas[1], sky_count, sky_latitude, 0, sky_latitude.Length);

                string skyView_color = "";
                if (myTasks_ephemeris[dataGridView_rowIndex].PartColor[i] == total_brush_ephemeris[0]) { skyView_color = "red"; }
                else if (myTasks_ephemeris[dataGridView_rowIndex].PartColor[i] == total_brush_ephemeris[1]) { skyView_color = "black"; }
                else if (myTasks_ephemeris[dataGridView_rowIndex].PartColor[i] == total_brush_ephemeris[2]) { skyView_color = "yellow"; }

                bool startOrbit = false; bool endOrbit = false;

                DrawDataOnPolarGragh(sky_longitude, sky_latitude, skyView_color, startOrbit, endOrbit);
                sky_count = sky_count + myTasks_ephemeris[dataGridView_rowIndex].PartDuration[i];

                if (i == 0)
                {
                    chart1.Series["white"].Points.AddXY(LLA_datas[0][0], LLA_datas[1][0]);
                    chart1.Series["white"].Points[0].Label = "Start";
                }
                if (i == myTasks_ephemeris[dataGridView_rowIndex].PartDuration.Length - 1)
                {
                    chart1.Series["white"].Points.AddXY(LLA_datas[0][LLA_datas[0].Length - 1], LLA_datas[1][LLA_datas[0].Length - 1]);
                    chart1.Series["white"].Points[1].Label = "End";
                }

            }

            // 위성/우주물체 정보 전시
            Display_EphemerisSatellite_Information(ephemeris_tuples[dataGridView_rowIndex].Item1, dataGridView_rowIndex);
            select_TLE_CPF = "ephemeris";
        }

        public void Display_EphemerisSatellite_Information(string satellite, int ephemeris_index)
        {
            if (display_satInformation != null)
            {
                if (display_satInformation.IsAlive == true)
                {
                    display_satInformation.Abort();
                }
                display_satInformation = null;
            }

            display_satInformation = new Thread(() =>
            {
                this.Invoke(new System.Action(() =>
                {
                    information_data1.Text = satellite;
                    information_data2.Text = "NSLR-OAS";
                    information_data6.Text = (sunRiseSet_time[0].ToLocalTime()).ToString("yyyy-MM-dd HH:mm");
                    information_data7.Text = (sunRiseSet_time[1].ToLocalTime()).ToString("yyyy-MM-dd HH:mm");
                    //information_data8.Text = "-";
                    information_data9.Text = "-";
                    information_data10.Text = ephemerisData_infos[ephemeris_index].MaxElevation + "°";
                }));

                int thread_count = 0;
                while (true)
                {
                    DateTime cur_UTC = DateTime.UtcNow;

                    // LLA 추출
                    double[] lla_result = EphemerisFileToPointingData(ephemeris_tuples[ephemeris_index].Item2, cur_UTC, 3);

                    // AzEl 추출
                    double[] azel_result = EphemerisFileToPointingData(ephemeris_tuples[ephemeris_index].Item2, cur_UTC, 0);

                    this.Invoke(new System.Action(() =>
                    {
                        information_data3.Text = String.Format("{0:0.00}", lla_result[2]) + " km";
                        information_data4.Text = String.Format("{0:0.00000}", azel_result[0]) + "°";
                        information_data5.Text = String.Format("{0:0.00000}", azel_result[1]) + "°";
                    }));

                    // Ground Track 실시간 위성 표시 (5초 주기)
                    if (thread_count % 5 == 0)
                    {
                        this.Invoke(new System.Action(() =>
                        {
                            // Ground Track 전시
                            GroundTrack.Image = null;
                            GroundTrack.Update();

                            for (int i = 0; i < total_points_ephemeris.Count; i++)
                            {
                                Graphics graphic = GroundTrack.CreateGraphics();
                                Pen pen = new Pen(total_color_ephemeris[i], 4f);
                                graphic.DrawCurve(pen, total_points_ephemeris[i]);
                            }

                            Graphics position_graphic = GroundTrack.CreateGraphics();
                            Bitmap satelliteIcon = new Bitmap(NSLR_ObservationControl.Properties.Resources.satOrbit1);
                            Point LLA_point = MercatorProjection(lla_result[0], lla_result[1]);
                            position_graphic.DrawImage(satelliteIcon, LLA_point.X - 25, LLA_point.Y - 25, 50, 50);

                        }));

                    }

                    thread_count++;
                    Thread.Sleep(1000);
                }
                
            });
            display_satInformation.Start();

        }

        public double[] EphemerisFileToPointingData(string ephemeris_file, DateTime current_UTC, int opt)
        {
            // opt별 Value 값 -  opt=0 : AzEl / opt=1 : RaDec(Geo) / opt=2 : RaDec(Topo) / opt=3 : LLA

            // Pointing Data 생성
            string[] target_string = current_UTC.ToString("yyyy-MM-dd-HH-mm-ss.fff").Split('-');
            double[] target_double = { double.Parse(target_string[0]), double.Parse(target_string[1]), double.Parse(target_string[2]), double.Parse(target_string[3]), 
                double.Parse(target_string[4]), double.Parse(target_string[5]) };
            double target_MJD = ConvertGreg2MJD(Global.timeSys, target_double);

            StringBuilder ephemerisFile = new StringBuilder(ephemeris_file);

            if (opt == 3)
            {
                double[] command = new double[3];
                GenerateLLA_Ephemeris(ephemerisFile, target_MJD, command);
                return command;
            }
            else
            {
                double[] siteloc = new double[3];
                GetSiteLocation(Global.laserSite, siteloc);

                double[] command = new double[2];
                GenerateCommand_Ephemeris(ephemerisFile, siteloc, target_MJD, opt, command);
                return command;
            }
        }

        public double[][] EphemerisFileToTrackingData(string ephemeris_file, DateTime period_start, DateTime period_end, double interval, int opt)
        {
            // opt별 Value 값 - opt=0 : AzEl / opt=1 : RaDec(Geo) / opt=2 : RaDec(Topo) / opt=3 : LLA

            // Tracking Data File 생성
            string[] start_date = period_start.ToString("yyyy-MM-dd").Split('-');
            string[] start_time = period_start.ToString("HH:mm:ss.fff").Split(':');
            double[] start_value = { double.Parse(start_date[0]), double.Parse(start_date[1]), double.Parse(start_date[2]),
                    double.Parse(start_time[0]), double.Parse(start_time[1]), double.Parse(start_time[2]) };
            double startMJD = ConvertGreg2MJD(Global.timeSys, start_value);

            string[] end_date = period_end.ToString("yyyy-MM-dd").Split('-');
            string[] end_time = period_end.ToString("HH:mm:ss.fff").Split(':');
            double[] end_value = { double.Parse(end_date[0]), double.Parse(end_date[1]), double.Parse(end_date[2]),
                    double.Parse(end_time[0]), double.Parse(end_time[1]), double.Parse(end_time[2]) };
            double endMJD = ConvertGreg2MJD(Global.timeSys, end_value);

            double stepSize = interval;

            StringBuilder fileName = new StringBuilder("Ephemeris_TrackingData" + ".txt");
            StringBuilder ephemerisFile = new StringBuilder(ephemeris_file);

            if (opt == 3)
            {
                GenerateLLAFile_Ephemeris(ephemerisFile, startMJD, endMJD, stepSize, fileName);
            }
            else
            {
                double[] siteLoc = new double[3];
                GetSiteLocation(Global.laserSite, siteLoc);
                GenerateCommandFile_Ephemeris(ephemerisFile, siteLoc, startMJD, endMJD, stepSize, fileName, opt);
            }

            // Tracking Data Read
            TimeSpan timeSpan = period_end - period_start;
            int period_count = (int)(timeSpan.TotalSeconds / interval);

            double[][] trackingData = new double[2][];
            for (int i = 0; i < trackingData.Length; i++) { trackingData[i] = new double[period_count]; }

            FileInfo fileInfo = new FileInfo("./Outputs/" + fileName.ToString());
            if (fileInfo.Exists)
            {
                int receive_count = 0;

                using (var reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                {
                    int count = 0;
                    while (!reader.EndOfStream)
                    {
                        count++;
                        string line = reader.ReadLine();
                        string[] line_words = line.Split('\t');

                        if (count >= 12)
                        {
                            if (line_words[0].Contains("END Ephemeris"))
                            {
                                // 끝
                                if (receive_count != period_count)
                                {
                                    MessageBox.Show("데이터 읽어오는 중 오류발생");
                                }
                            }
                            else if (true)
                            {

                                if (receive_count < period_count)
                                {
                                    // 형변환 실패 시, 오류 출력
                                    trackingData[0][receive_count] = double.Parse(line_words[1]);
                                    trackingData[1][receive_count] = double.Parse(line_words[2]);
                                    receive_count++;
                                }
                            }
                        }
                    }

                }

            }

            return trackingData;
        }


        private void LS_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (LS_checkBox.Checked == true)
            {
                if (EKF_checkBox.Checked == true) { EKF_checkBox.Checked = false; }
            }
        }

        private void EKF_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (EKF_checkBox.Checked == true)
            {
                if (LS_checkBox.Checked == true) { LS_checkBox.Checked = false; }
            }
        }

        private void CRDFile_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < CRDFile_dataGridView.Rows.Count)
            {
                string observation_time = CRDFile_dataGridView.SelectedRows[0].Cells[1].Value.ToString();
                string[] dateTime_array = observation_time.Split(' ');
                string[] date_array = dateTime_array[0].Split('-');
                string[] time_array = dateTime_array[1].Split(':');
                DateTime start_time = new DateTime(int.Parse(date_array[0]), int.Parse(date_array[1]), int.Parse(date_array[2]),
                    int.Parse(time_array[0]), int.Parse(time_array[1]), 0, DateTimeKind.Utc);
                DateTime end_time = start_time.AddHours(1);

                periodDateStart_dateTimePicker.Value = start_time;
                periodTimeStart_dateTimePicker.Value = start_time;
                periodDateEnd_dateTimePicker.Value = end_time;
                periodTimeEnd_dateTimePicker.Value = end_time;
            }

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




        public void SendToDatabase_ObservingSatelliteData() // ObservingSatellite_Information(관측위성 정보) 데이터베이스 저장
        {
            try
            {
                if (ObservingSatellite_Information.available_flag == true)
                {
                    // postgresql connect
                    NpgsqlConnection connect_sql = new NpgsqlConnection(dataPC_postgresql_connectStr);
                    connect_sql.Open();


                    using (NpgsqlCommand connect_command = new NpgsqlCommand())
                    {
                        connect_command.Connection = connect_sql;

                        // 기존 table data Delete
                        connect_command.CommandText = String.Format("DELETE FROM observingsatellite_information;");
                        connect_command.ExecuteNonQuery();

                        // 새로운 table data Insert
                        int data_index = 0;
                        for (DateTime dateTime = ObservingSatellite_Information.observingSatellite_startTime;
                            DateTime.Compare(dateTime, ObservingSatellite_Information.observingSatellite_endTime) == -1 || DateTime.Compare(dateTime, ObservingSatellite_Information.observingSatellite_endTime) == 0;
                            dateTime = dateTime.AddSeconds(ObservingSatellite_Information.observingSatellite_interval))
                        {
                            connect_command.CommandText = String.Format("INSERT INTO observingsatellite_information VALUES ('{0}', '{1}', '{2}', '{3}');"
                            , ObservingSatellite_Information.observingSatellite_name,   // 파일이름 > 위성이름 변경
                            dateTime.ToString("yyyy-MM-dd HH:mm:ss.ff"),
                            ObservingSatellite_Information.observingSatellite_RaDec[0][data_index],
                            ObservingSatellite_Information.observingSatellite_RaDec[1][data_index]);

                            connect_command.ExecuteNonQuery();
                            data_index++;
                        }

                        MessageBox.Show(data_index.ToString());


                    }
                    connect_sql.Close();


                }
                else
                {
                    MessageBox.Show("불가!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("오류 : " + ex.ToString());
            }
        }

        public static List<Tuple<DateTime, double, double, double, double>> TLE_Propagator
            (string sat_name, DateTime start_dateTime, DateTime end_dateTime, int targetInterval)  // 관측데이터 후처리용(TLE) : 각 시간대별 위성 X/Y/Z 값 산출
        {
            List<Tuple<DateTime, double, double, double, double>> result = new List<Tuple<DateTime, double, double, double, double>> ();

            TimeSpan diff = end_dateTime - start_dateTime;
            int totalSeconds = (int)diff.TotalSeconds;
            int index = (totalSeconds / targetInterval) + 1;

            // TLE Propagator
            IntPtr propagator_tleClass = CreateTLE();
            StringBuilder line1 = new StringBuilder(TLE_Information[sat_name].Line1);
            StringBuilder line2 = new StringBuilder(TLE_Information[sat_name].Line2);
            double[] epoch_time = new double[6];
            int noradID = (int)GetTLEData(propagator_tleClass, line1, line2, epoch_time);


            double[] start_time = { (double)start_dateTime.Year, (double)start_dateTime.Month, (double)start_dateTime.Day,
                (double)start_dateTime.Hour, (double)start_dateTime.Minute, (double)start_dateTime.Second };

            double epoch_MJD = ConvertGreg2MJD(Global.timeSys, epoch_time);
            double start_MJD = ConvertGreg2MJD(Global.timeSys, start_time);

            int nTime = index;
            double[] elapsedSecs = new double[nTime];
            for (int n = 0; n < nTime; n++)
            {
                elapsedSecs[n] = (start_MJD - epoch_MJD) * 86400.0 + (double)(targetInterval * n);
            }
            double[] targetEphemeris = new double[nTime * 6];
            int optCoord = 0;   // 0 : J2000, 1 : ECEF
            PropagateTLE(propagator_tleClass, elapsedSecs, nTime, targetEphemeris, optCoord);

            for (int i = 0; i < nTime; i++)
            {
                result.Add(Tuple.Create(start_dateTime.AddSeconds(targetInterval * i), targetEphemeris[(i * 6)], targetEphemeris[(i * 6) + 1], targetEphemeris[(i * 6) + 2], 1.0));
            }

            return result;
        }


        public static List<Tuple<DateTime, double, double, double, double>> CPF_Interpolation  // 관측데이터 후처리용(CPF) : CPF 데이터를 PCHIP(형상보존 3차 보간)으로 보간
            (List<Tuple<DateTime, double, double, double, double>> inputData, int currentInterval, int targetInterval)
        {
            if (inputData == null || inputData.Count < 2)
                throw new ArgumentException("입력 데이터가 충분하지 않습니다.");

            int n = inputData.Count;

            // 시간축을 double(초 단위)로 변환
            DateTime t0 = inputData[0].Item1;
            double[] t = new double[n];
            double[] x = new double[n];
            double[] y = new double[n];
            double[] z = new double[n];

            for (int i = 0; i < n; i++)
            {
                t[i] = (inputData[i].Item1 - t0).TotalSeconds;
                x[i] = inputData[i].Item2;
                y[i] = inputData[i].Item3;
                z[i] = inputData[i].Item4;
            }

            // 각 축별 PCHIP 기울기 계산
            double[] mx = ComputeSlopes(t, x);
            double[] my = ComputeSlopes(t, y);
            double[] mz = ComputeSlopes(t, z);

            // 출력 생성
            var result = new List<Tuple<DateTime, double, double, double, double>>();
            double rangeConst = 1.0;
            double endTime = t[n - 1];

            for (double tq = 0; tq <= endTime + 1e-9; tq += targetInterval)
            {
                double xq = EvalPCHIP(t, x, mx, tq);
                double yq = EvalPCHIP(t, y, my, tq);
                double zq = EvalPCHIP(t, z, mz, tq);

                DateTime tqTime = t0.AddSeconds(tq);
                result.Add(Tuple.Create(tqTime, xq, yq, zq, rangeConst));
            }

            return result;
        }

        private static double[] ComputeSlopes(double[] x, double[] y)
        {
            int n = x.Length;
            double[] h = new double[n - 1];
            double[] delta = new double[n - 1];

            for (int i = 0; i < n - 1; i++)
            {
                h[i] = x[i + 1] - x[i];
                delta[i] = (y[i + 1] - y[i]) / h[i];
            }

            double[] m = new double[n];

            if (n == 2)
            {
                m[0] = delta[0];
                m[1] = delta[0];
                return m;
            }

            // 시작점 기울기
            m[0] = ((2 * h[0] + h[1]) * delta[0] - h[0] * delta[1]) / (h[0] + h[1]);
            if (m[0] * delta[0] <= 0) m[0] = 0;
            else if (delta[0] * delta[1] < 0 && Math.Abs(m[0]) > 3 * Math.Abs(delta[0]))
                m[0] = 3 * delta[0];

            // 내부 기울기
            for (int i = 1; i < n - 1; i++)
            {
                if (delta[i - 1] == 0 || delta[i] == 0 || delta[i - 1] * delta[i] < 0)
                    m[i] = 0;
                else
                {
                    double w1 = 2 * h[i] + h[i - 1];
                    double w2 = h[i] + 2 * h[i - 1];
                    m[i] = (w1 + w2) / (w1 / delta[i - 1] + w2 / delta[i]);
                }
            }

            // 끝점 기울기
            m[n - 1] = ((2 * h[n - 2] + h[n - 3]) * delta[n - 2] - h[n - 2] * delta[n - 3]) / (h[n - 3] + h[n - 2]);
            if (m[n - 1] * delta[n - 2] <= 0)
                m[n - 1] = 0;
            else if (delta[n - 3] * delta[n - 2] < 0 && Math.Abs(m[n - 1]) > 3 * Math.Abs(delta[n - 2]))
                m[n - 1] = 3 * delta[n - 2];

            return m;
        }

        private static double EvalPCHIP(double[] x, double[] y, double[] m, double xq)
        {
            int n = x.Length;
            int i = 0;

            // 보간 구간 찾기
            while (i < n - 2 && xq > x[i + 1])
                i++;

            double h = x[i + 1] - x[i];
            double s = (xq - x[i]) / h;

            double y0 = y[i], y1 = y[i + 1];
            double m0 = m[i], m1 = m[i + 1];

            // Hermite basis
            double h00 = (2 * s * s * s - 3 * s * s + 1);
            double h10 = (s * s * s - 2 * s * s + s);
            double h01 = (-2 * s * s * s + 3 * s * s);
            double h11 = (s * s * s - s * s);

            return h00 * y0 + h10 * h * m0 + h01 * y1 + h11 * h * m1;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    //////////////////////////////////////// ObserveSchedule2 참조 Class ////////////////////////////////////////////////////////////////////////////////

    // 현재 관측 중인 위성 정보 Class (임시)
    public static class ObservingSatellite_Information
    {
        public static bool available_flag { get; set; }
        public static string observingSatellite_name { get; set; }

        public static DateTime observingSatellite_startTime { get; set; }
        public static DateTime observingSatellite_endTime { get; set; }

        public static double observingSatellite_interval { get; set; }

        public static double[][] observingSatellite_RaDec { get; set; }


        public static DateTime observingSatellite_endTime_LLA { get; set; }
        public static double[][] observingSatellite_LLA { get; set; }

        public static bool observinSatellite_keyhole { get; set; }

        static ObservingSatellite_Information()
        {
            available_flag = false;
            observingSatellite_name = "";
            observingSatellite_startTime = new DateTime();
            observingSatellite_endTime = new DateTime();
            observingSatellite_interval = 0.02;
            observingSatellite_RaDec = new double[2][];

            observingSatellite_endTime_LLA = new DateTime();
            observingSatellite_LLA = new double[3][];

            observinSatellite_keyhole = false;
        }

        public static void ObservingSatellite_SettingData(string name, DateTime startTime, DateTime endTime, double interval)
        {
            observingSatellite_name = name;
            observingSatellite_startTime = startTime;
            observingSatellite_endTime = endTime;
            observingSatellite_interval = interval;
        }

        public static void ObservingSatellite_SettingRaDec(double[][] RaDec)
        {
            for (int i = 0; i < observingSatellite_RaDec.Length; i++)
            {
                observingSatellite_RaDec[i] = new double[RaDec[i].Length];
                Array.Copy(RaDec[i], observingSatellite_RaDec[i], observingSatellite_RaDec[i].Length);
            }

        }


        public static void ObservingSatellite_SettingLLA(DateTime LLA_endTime, double[][] LLA)
        {
            observingSatellite_endTime_LLA = LLA_endTime;

            for (int i = 0; i < observingSatellite_LLA.Length; i++)
            {
                observingSatellite_LLA[i] = new double[LLA[i].Length];
                Array.Copy(LLA[i], observingSatellite_LLA[i], observingSatellite_LLA[i].Length);
            }

        }

        public static void ObservingSatellite_SettingKeyhole(bool flag)
        {
            observinSatellite_keyhole = flag;
        }

        public static ObservingSatelliteData GetData()
        {
            return new ObservingSatelliteData
            {
                AvailableFlag = available_flag,
                Name = observingSatellite_name,
                StartTime = observingSatellite_startTime,
                EndTime = observingSatellite_endTime,
                Interval = observingSatellite_interval,
                RaDec = observingSatellite_RaDec
            };
        }
    }
    public class ObservingSatelliteData
    {
        public bool AvailableFlag { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Interval { get; set; }
        public double[][] RaDec { get; set; }
        public string SatelliteName { get; set; }
    }

    public class ObservingSchedule_Information
    {
        public string orbit_type { get; set; }
        public string orbit_source { get; set; }
        public int norad_id { get; set; }

        public bool available_flag { get; set; }
        public string observingSchedule_name { get; set; }

        public DateTime observingSchedule_startTime { get; set; }
        public DateTime observingSchedule_endTime { get; set; }

        public double observingSchedule_interval { get; set; }

        public double[][] observingSchedule_RaDec { get; set; }

        public List<long> observingSchedule_utcMs { get; set; }
        public List<double> observingSchedule_Tof { get; set; }

        public ObservingSchedule_Information()
        {
            orbit_type = "";
            orbit_source = "";
            norad_id = 0;

            available_flag = false;
            observingSchedule_name = "";
            observingSchedule_startTime = new DateTime();
            observingSchedule_endTime = new DateTime();
            observingSchedule_interval = 0.02;
            observingSchedule_RaDec = new double[2][];
            observingSchedule_utcMs = new List<long>();
            observingSchedule_Tof = new List<double>();

        }

        public void ObservingSchedule_SettingOrbit(string type, string source, int id)
        {
            orbit_type = type;
            orbit_source = source;
            norad_id = id;
        }

        public void ObservingSchedule_SettingData(string name, DateTime startTime, DateTime endTime, double interval)
        {
            observingSchedule_name = name;
            observingSchedule_startTime = startTime;
            observingSchedule_endTime = endTime;
            observingSchedule_interval = interval;
        }

        public void ObservingSchedule_SettingRaDec(double[][] RaDec)
        {
            for (int i = 0; i < observingSchedule_RaDec.Length; i++)
            {
                observingSchedule_RaDec[i] = new double[RaDec[i].Length];
                Array.Copy(RaDec[i], observingSchedule_RaDec[i], observingSchedule_RaDec[i].Length);
            }
        }

        public void ObservingSchedule_SettingTof(List<long> utcMS, List<double> tof)
        {
            observingSchedule_utcMs = utcMS.ToList();
            observingSchedule_Tof = tof.ToList();
        }

    }

    // 필터링 기준 정보 Class
    public class Filtering_SatelliteValue
    {
        public static double Minimum_Altitude = 0.0;    // 최소 고도
        public static double Maximum_Altitude = 0.0;    // 최대 고도
        public static double Minimum_Elevation = 0.0;   // 최소 고각
        public static double Maximum_Elevation = 0.0;   // 최대 고각
        public static double Minimum_RCS = 0.0;         // 최소 RCS
        public static double Maximum_RCS = 0.0;         // 최대 RCS
        public static string Object_Type = "NONE";      // 타입
    }


    // 각 위성, 타입 및 RCS 정보 저장 Class
    public class SATCAT_Data
    {
        public string Object_Type { get; set; } = "NONE";
        public double RCS { get; set; } = 0.0;


    }

    // 각 위성, TLE 정보 저장 Class
    public class TLE
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public double Elevation { get; set; }
        public double Altitude { get; set; }
        public int Norad { get; set; }

        public SATCAT_Data SATCAT_Data = new SATCAT_Data();

        public TLE(string line1, string line2)
        {
            Line1 = line1;
            Line2 = line2;
        }
    }

    // TLE DataGridView 정보
    public class TaskGridView_Info
    {
        public int NoradID { get; set; }
        public string Satellite { get; set; }
        public double Altitude { get; set; }
        public double MaxElevation { get; set; }
        public double RCS { get; set; }
        public string Type { get; set; }

        public TaskGridView_Info(string name)
        {
            Satellite = name;
        }
    }

    // CPF DataGridView 정보
    public class CPFGridView_Info
    {
        public string Satellite { get; set; }
        public string Altitude { get; set; }
        public string MaxElevation { get; set; }

        public CPFGridView_Info(string name, string altitude, string maxElevation)
        {
            Satellite = name;
            Altitude = altitude;
            MaxElevation = maxElevation;
        }
    }

    // Reference DataGridView 정보
    public class Reference_Data
    {
        public string Local_Time { get; set; }
        public double Azimuth { get; set; }
        public double Elevation { get; set; }

        public Reference_Data(string time, double az, double el)
        {
            Local_Time = time;
            Azimuth = az;
            Elevation = el;
        }
    }

    // ObservationData_Info
    public class ObservationData_Info
    {
        public string Satellite { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Available { get; set; }

        public ObservationData_Info(string satellite, string startTime, string endTime, string available)
        {
            Satellite = satellite;
            StartTime = startTime;
            EndTime = endTime;
            Available = available;
        }
    }

    // CRDFile_Info
    public class CRDFile_info
    {
        public string Satellite { get; set; }
        public string Observation_Time {  get; set; }
        public string Data_Type { get; set; }
        public string Version { get; set; }
        public string Available { get; set; }

        public CRDFile_info(string satellite, string observation_time, string type, string version, string available)
        {
            Satellite = satellite;
            Observation_Time = observation_time;
            Data_Type = type;
            Version = version;
            Available = available;
        }
    }

    // EphimerisFile_Info
    public class EphimerisFile_Info
    {
        public string Satellite { get; set; }
        public string Available_StartTime {  get; set; }
        public string Available_EndTime { get; set; }
        public string Available { get; set; }

        public EphimerisFile_Info(string satellite, string startTime, string endTime, string available)
        {
            Satellite = satellite;
            Available_StartTime = startTime;
            Available_EndTime = endTime;
            Available = available;
        }
    }

    // EphemerisData_Info
    public class EphemerisData_Info
    {
        public string Satellite { get; set; }
        public string Altitude { get; set; }
        public string MaxElevation { get; set; }

        public EphemerisData_Info(string satellite, string altitude, string maxElevation)
        {
            Satellite = satellite;
            Altitude = altitude;
            MaxElevation = maxElevation;
        }
    }


    // Task 인터페이스에서 파생된 고유한 유형의 사용자 정의 작업(선택 사항)
    [Serializable]
    public class MyTask : Braincase.GanttChart.Task
    {
        public MyTask(ProjectManager manager) : base()
        {
            Manager = manager;
        }

        private ProjectManager Manager { get; set; }

        public new TimeSpan Start { get { return base.Start; } set { Manager.SetStart(this, value); } }
        public new TimeSpan End { get { return base.End; } set { Manager.SetEnd(this, value); } }
        public new TimeSpan Duration { get { return base.Duration; } set { Manager.SetDuration(this, value); } }
        public new float Complete { get { return base.Complete; } set { Manager.SetComplete(this, value); } }
    }

}
