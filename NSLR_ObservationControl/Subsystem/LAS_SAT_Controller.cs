using log4net;
using log4net.Config;
using Newtonsoft.Json;
using NSLR_ObservationControl.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Documents;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace NSLR_ObservationControl.Subsystem
{
    public class LAS_SAT_Controller : ILAS
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LAS_SAT_Controller));
        
        public MONITOR_LAS_DATA monitor_las_data = new MONITOR_LAS_DATA(); //모니터링
        public event EventHandler<ConnectedEventArgs> onConnected;
        
        private static LAS_SAT_Controller _instance;
        public string Tag => "SLR";
        private const string laserClass = "SAT";
        private Tcp_Server LAS_Server;
        //private TcpClient interPC_client;
        private NetworkStream interPC_clientStream;
        public bool IsInitialized { get; private set; }
        const string MSG_DATA_BYTE_CNT = "00000004";
        private bool connected { get; set; } = false;
        private bool connected_prev { get; set; }
        public string TX_PACKET { get; set; } = "00";
        public string TX_DATA { get; set; } = "00000000";
        public string TX_LaserOpMode { get; set; } = "00";
        public string TX_LaserMode { get; set; } = "00";
        public string TX_LaserStartStop { get; set; } = "00";
        public string TX_LaserOpEnd { get; set; } = "00";
        public int laserStartStop { get; set; } = 0;
        public int laserOpState { get; set; } = 1;

        public string strBitResult1 { get; set; }
        public string strBitResult2 { get; set; }
        public float WaveLengthConverterE = 0;
        public LASUserData userData;

        public LAS_SAT_Controller()
        {

        }
        public static LAS_SAT_Controller instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LAS_SAT_Controller();
                }
                //log.Info($"Instance");
                return _instance;
            }
        }

        public void Initialize()
        {
            Create_LasServer();
            userData = new LASUserData
            {
                ID = "SAT.LAS_RSP_CTRL",
                LASopMode = "0",
                LaserMode = "0",
                LaserFireState = "0",
                LASopEnd = "0",
                BITResult1 = 0,
                BITResult2 = 0,
            };
            IsInitialized = true;
            log.Info("Initialize()");
        }

        public void Start()
        {        
            log.Info("Start");
        }

        public void End()
        {
            log.Info("End");
        }


        #region [ Server ] //////////////////////////////////////////////////////////////////////
        /// <summary>
        ///  Create_LasServer 
        ///  : ICD LAS 
        /// </summary>
        private void Create_LasServer()
        {
            //Server for SubSystem messaging
            if (LAS_Server != null)
            {
                LAS_Server.Dispose();
                //log.Debug("LAS...Dispose()");
            }
            else
            {
                ////////////////////////////////////////////////////////////////////////////
                ///                   Latch event Handlers                               //
                //////////////////////////////////////////////////////////////////////////
                LAS_Server = new Tcp_Server(SubSystem_PORT.LAS_SAT);                   //
                LAS_Server.OnPacketReceivedEvent += TcpServer_OnPacketReceivedEvent;  //
                LAS_Server.OnConnectedEvent += ChangeConnected;                      //
                //////////////////////////////////////////////////////////////////////
            }
        }
        /// <summary>
        /// ChangeConnected
        /// : Connection management 
        /// </summary>
        /// <param nam e="bCon"></param>
        private void ChangeConnected(bool bCon)
        {
            if (bCon)
            {
                connected = true;
                if (connected_prev != connected)
                {
                    log.Info(IPC_MSG.CONNECTION_LAS_SAT);
                    onConnected?.Invoke(this, new ConnectedEventArgs(IPC_MSG.CONNECTION_LAS_SAT));
                    connected_prev = connected;
                }
            }
            else
            {
                    connected = false;
                if (connected_prev != connected)
                {
                    log.Info(IPC_MSG.DISCONNECTION_LAS_SAT);
                    onConnected?.Invoke(this, new ConnectedEventArgs(IPC_MSG.DISCONNECTION_LAS_SAT));
                    connected_prev = connected;
                }
          }
        }

        public bool IsConnected()
        {
            return connected;
        }

        private bool running = false;

        public string D_OpMode, D_Mode;
        public string D_FireState, D_OpEnd;
        public byte D_BitResult1, D_BitResult2;
        public byte D_WaveLengthConverterE;
        /// <summary>
        /// ///////////////////////////////
        /// OnPacketReceived Event 
        /// ///////////////////////////////
        /// </summary>
        /// <param name="recvData"></param>
        /// <param name="STX"></param>
        /// <param name="MsgID"></param>
        /// <param name="dataCNT"></param>
        /// <param name="data"></param>
        /// <param name="ETX"></param>
        /// 
        //private void TcpServer_OnPacketReceivedEvent(byte[] recvData, byte[] stx, byte[] msgID, byte[] seqNNum, byte[] dataCNT, byte[] data, byte[] etx)
        private void TcpServer_OnPacketReceivedEvent(byte[] recvData, byte[] msgID, byte[] data)
        {
            var RcvdData = (BitConverter.ToString(recvData)).Replace("-", string.Empty);
            var MsgID = (BitConverter.ToString(msgID)).Replace("-", string.Empty);
            var Data = (BitConverter.ToString(data)).Replace("-", string.Empty);

            //log.Info($"RX:[{laserClass}]Rcv[{RcvdData}] MsgID[{MsgID}] Data[{Data}]");

            if (MsgID.Equals(ControlCommand.MSG_ID_LASER_OP_RSP))
            {
                D_OpMode = Data.Substring(0, 2);
                D_Mode = Data.Substring(2, 2);
                D_FireState = Data.Substring(4, 2);
                D_OpEnd = Data.Substring(6, 2);
                D_BitResult1 = recvData[16];
                D_BitResult2 = recvData[17];
                D_WaveLengthConverterE = recvData[18]; //2025.05.15 장호우선임 요청사항 : 파장증폭기 수치 확인 

                WaveLengthConverterE = (float)D_WaveLengthConverterE / 10;
                bool B5_InterLock = (D_BitResult1 & (1 << 5)) != 0;
                bool B6_Filter    = (D_BitResult1 & (1 << 6)) != 0;
                bool B7_Shutter   = (D_BitResult2 & (1 << 7)) != 0;
                //log.Debug($">>PWR [{Convert.ToString(D_BitResult1, 2).PadLeft(8, '0')}] bit interesting > B5:{B5_InterLock} B6:{B6_Filter}");
                //log.Debug($">>OPR [{Convert.ToString(D_BitResult2, 2).PadLeft(8, '0')}] bit interesting > B7:{B7_Shutter}");

                try
                {
                    userData = new LASUserData
                    {
                        ID = "SAT.LAS_RSP_CTRL",
                        LASopMode = D_OpMode,
                        LaserMode = D_Mode,
                        LaserFireState = D_FireState,
                        LASopEnd = D_OpEnd,
                        BITResult1 = D_BitResult1,
                        BITResult2 = D_BitResult2,
                    };

                    // 관측제어부 PC 모니터링용
                    monitor_las_data.ID = "LAS_RSP_CTRL";
                    monitor_las_data.LASopMode = D_OpMode;
                    monitor_las_data.LaserMode = D_Mode;
                    monitor_las_data.LaserFireState = D_FireState;
                    monitor_las_data.LASopEnd = D_OpEnd;
                    monitor_las_data.CbitResult1 = D_BitResult1;
                    monitor_las_data.CbitResult2 = D_BitResult2;
                }
                catch (Exception ex)
                {
                    // 예외 처리: 서버와의 통신 중 문제가 발생한 경우
                    log.Info($"PacketReceiveEvent: [{laserClass}] {ex.Message}");
                }
            }
            else if (MsgID.Equals(ControlCommand.MSG_ID_LASER_PBIT_RSP))
            {
                D_BitResult1 = recvData[5];
                D_BitResult2 = recvData[6];

                try
                {
                    userData = new LASUserData
                    {
                        ID = "SAT.LAS_RSP_PBIT",
                        BITResult1 = D_BitResult1,
                        BITResult2 = D_BitResult2,
                    };
                }
                catch (Exception ex)
                {
                    //log.Debug($"PacketReceiveEvent: [{laserClass}] {ex.Message}");
                }
            }
            else if (MsgID.Equals(ControlCommand.MSG_ID_LASER_IBIT_RSP))
            {
                D_BitResult1 = recvData[5];
                try
                {
                    userData = new LASUserData
                    {
                        ID = "SAT.LAS_RSP_IBIT",
                        BITResult1 = D_BitResult1,
                    };
                }
                catch (Exception ex)
                {
                    //log.Debug($"PacketReceiveEvent: {ex.Message}");
                }
            }
            //Console.WriteLine($"[Connection:{connected}] RcvPacket [{MsgStr}] [{RcvdData}] Sof [{SOM}] ID [{MsgID}] DataLen [{DataLen}] Data [{Data}] Eof [{EOM}]");
        }
        #endregion  //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary>
        /// ///////////////////////////////
        /// Command do_PBIT
        /// ///////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        public void doPBIT()
        {
            log.Info("PBIT");
            SetBIT("PBIT");
        }
        public void SetBIT(string bit)
        {
            if (bit.Equals("PBIT"))
                TX_PACKET = ControlCommand.MSG_SOM + ControlCommand.MSG_ID_LASER_PBIT_CMD + "00000000" + ControlCommand.MSG_EOM;
            else if (bit.Equals("IBIT"))
                TX_PACKET = ControlCommand.MSG_SOM + ControlCommand.MSG_ID_LASER_IBIT_CMD + "00000000" + ControlCommand.MSG_EOM; 
            
            if (TX_PACKET is "")
            {
                Console.WriteLine("Data 선택 부터 하세요.");
                return;
            }

            if (connected)
            {
                LAS_Server.SendData(TX_PACKET);
                //log.Info($"Send [{bit}] Command OK");
            }
        }


        /// <summary>
        /// ///////////////////////////////
        /// Command Laser Start/Stp
        /// ///////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetCommand(string data)
        {
            TX_PACKET = ControlCommand.MSG_SOM + ControlCommand.MSG_ID_LASER_OP_CMD + MSG_DATA_BYTE_CNT + data + ControlCommand.MSG_EOM;
            if (connected)
                LAS_Server.SendData(TX_PACKET);
        }
        /// <summary>
        ///  1번째 바이트 => 03 고정
        ///  2번째 바이트 => 레이저모드 (  03 : 필터 ON / 02 : 필터 OFF / 01 : 기본 모드)
        ///  3번째 바이트 => 셔터 ( 00 : Close, 01 : Open )
        ///  4번째 바이트 => 01 : 레이저 OFF
        /// </summary>
        //For Observation - Range Mode

        public void laserReady_SatTrack()  {  SetCommand("00020000"); } //필터 OFF : 장호우 선임 요청  //0617 잠시 필터 on (02 - > 03 )기존은 02
        public void laserStart_SatTrack()  {  SetCommand("00010100"); }
        public void laserStop_SatTrack()   {  SetCommand("00010000"); }

        //For Ground Calibration - Range Mode
        public void laserReady_GroundCal() {  SetCommand("00030000"); } //필터 ON //셔터 Close    // 250616 한윤호 선임연구원 요청으로 필터 on => 필터 off 로 변경. ( 03 -> 02 )
        public void laserStart_GroundCal() {  SetCommand("00030100"); } //필터 ON //셔터 Open     
        public void laserStop_GroundCal()  {  SetCommand("00010000"); } //셔터 Close
        public void laserExit_GroundCal() { SetCommand("00030000"); } //ND filter off

        public void laserOp_PowerOff()     {  SetCommand("02000001"); }
    }
}
