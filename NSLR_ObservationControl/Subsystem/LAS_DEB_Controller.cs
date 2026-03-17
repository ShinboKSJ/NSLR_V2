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
    public class LAS_DEB_Controller : ILAS
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LAS_DEB_Controller));
        
        public MONITOR_LAS_DATA monitor_las_data = new MONITOR_LAS_DATA(); //모니터링
        public event EventHandler<ConnectedEventArgs> onConnected;

        private static LAS_DEB_Controller _instance;
        public string Tag => "DLT";
        private const string laserClass = "DEB";
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

        public LAS_DEB_Controller()
        {
        }
        public static LAS_DEB_Controller instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LAS_DEB_Controller();
                }
                //log.Info($"Instance");
                return _instance;
            }
        }

        public void Initialize()
        {
            Create_LasServer();
            //Create_InterPCclient();
            userData = new LASUserData
            {
                ID = "INIT",
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
                LAS_Server = new Tcp_Server(SubSystem_PORT.LAS_DEB);                   //
                LAS_Server.OnPacketReceivedEvent += TcpServer_OnPacketReceivedEvent;  //
                LAS_Server.OnConnectedEvent += ChangeConnected;                      //
                //////////////////////////////////////////////////////////////////////
            }
        }
        /// <summary>
        /// ChangeConnected
        /// : Connection management 
        /// </summary>
        /// <param name="bCon"></param>
        private void ChangeConnected(bool bCon)
        {
            if (bCon)
                {
                connected = true;
                if (connected_prev != connected)
                {
                    log.Info(IPC_MSG.CONNECTION_LAS_DEB);
                    onConnected?.Invoke(this, new ConnectedEventArgs(IPC_MSG.CONNECTION_LAS_DEB));
                    connected_prev = connected;
                }
            }
            else
            {
                connected = false;
                if (connected_prev != connected)
                {
                    log.Info(IPC_MSG.DISCONNECTION_LAS_DEB);
                    onConnected?.Invoke(this, new ConnectedEventArgs(IPC_MSG.DISCONNECTION_LAS_DEB));
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
        public string D_FireState , D_OpState;
        public byte D_PBitResult1, D_PBitResult2;
        public byte D_CBitResult1, D_CBitResult2;
        public byte D_IBitResult1, D_IBitResult2;

        
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
                D_OpState = Data.Substring(6, 2);
                D_CBitResult1 = recvData[16];
                D_CBitResult2 = recvData[17];

                userData = new LASUserData
                {
                    ID = "DEB.LAS_RSP_CTRL",
                    LASopMode = D_OpMode,
                    LaserMode = D_Mode,
                    LaserFireState = D_FireState,
                    LASopEnd = D_OpState,
                    BITResult1 = D_CBitResult1,
                    BITResult2 = D_CBitResult2,
                }; 
            }
            else if (MsgID.Equals(ControlCommand.MSG_ID_LASER_PBIT_RSP))
            {
                D_PBitResult1 = recvData[5];
                D_PBitResult2 = recvData[6];

                userData = new LASUserData
                {
                    ID = "DEB.LAS_RSP_PBIT",
                    BITResult1 = D_PBitResult1,
                    BITResult2 = D_PBitResult2,
                };
            }
            else if (MsgID.Equals(ControlCommand.MSG_ID_LASER_IBIT_RSP))
            {
                D_IBitResult1 = recvData[5];
                userData = new LASUserData
                {
                    ID = "DEB.LAS_RSP_IBIT",
                    BITResult1 = D_IBitResult1,
                    BITResult2 = D_IBitResult2,
                };
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
            /*
            laserStartStop ^= 1;
            TX_LaserStartStop = string.Format("{0:x2}", laserStartStop);
            //MessageBox.Show($"Laser StartStop : {TX_LaserStartStop}");
            if (laserStartStop == 1)
            {
                //btn_laser_shutter.Text = "레이저 조사시작";
                //btn_laser_shutter.BackColor = Color.Blue;
                //btn_laser_shutter.ForeColor = Color.Orange;
            }
            else if (laserStartStop == 0)
            {
                //btn_laser_shutter.Text = "레이저 조사중지";
                //btn_laser_shutter.BackColor = Color.LightGray;
                //btn_laser_shutter.ForeColor = Color.Black;
            }
            */
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
        /*        public void laserReady_DebTrack()  { SetCommand("03010000"); }
                public void laserStart_DebTrack()  { SetCommand("03010100"); }
                public void laserStop_DebTrack()   { SetCommand("03010000"); }

                //For Ground Calibration - Range Mode
                public void laserReady_GroundCal() { SetCommand("03020000"); }
                public void laserStart_GroundCal() { SetCommand("03020100"); }
                public void laserStop_GroundCal()  { SetCommand("03020000"); }

                public void laserOp_PowerOff()     { SetCommand("02000001"); }   */
/*        public void laserReady_DebTrack()  { SetCommand("03020000"); }
        public void laserStart_DebTrack()  { SetCommand("03010100"); }
        public void laserStop_DebTrack()   { SetCommand("03010000"); }

        //For Ground Calibration - Range Mode
        public void laserReady_GroundCal() { SetCommand("03020000"); }
        public void laserStart_GroundCal() { SetCommand("03010100"); }
        public void laserStop_GroundCal()  { SetCommand("03010000"); }
        public void laserExit_GroundCal() { SetCommand("00020000"); } //ND filter off*/


     //    0923 지상보정 모드 /관측 모드 적용 TEST 예정

        public void laserReady_DebTrack() { SetCommand("03010000"); }
        public void laserStart_DebTrack() { SetCommand("03010100"); }
        public void laserStop_DebTrack() { SetCommand("03010000"); }

        //For Ground Calibration - Range Mode
        public void laserReady_GroundCal() { SetCommand("03020000"); }
        public void laserStart_GroundCal() { SetCommand("03020100"); }
        public void laserStop_GroundCal() { SetCommand("03020000"); }
        public void laserExit_GroundCal() { SetCommand("00020000"); } //ND filter off




        // 지상 보정 0917 test
        // 웜업/노말 모드 의미 파악 1번째 바이트 (모드) : 00/03 둘다 test 진행해봄
/*         
           시작 : 웜업/셔터 off => 노말 / 셔터 on
           레이저발사 : 노말 / 셔터 on
           종료 : 노말 / 셔터 off

           시작 : 웜업/셔터 off
           레이저발사 : 노말 / 셔터 on
           종료 : 노말 / 셔터 off
*/

        public void laserOp_PowerOff()     { SetCommand("02000001"); }

    }
}
