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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Web.UI.WebControls;
using System.Windows.Documents;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace NSLR_ObservationControl.Subsystem
{
    internal class LAS_Controller : ILAS
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static LAS_Controller _instance;
        public MONITOR_LAS_DATA monitor_las_data = new MONITOR_LAS_DATA(); // 관측제어부 PC 모니터링용
        public event EventHandler<ConnectedEventArgs> onConnected;
        public bool IsInitialized { get; private set; }
        string state_now;
        int randumValue;
        Random rand = new Random();
        // ////////////////////////
        // 레이저부 ICD
        // ////////////////////////
        private string laserClass { get; set; } = "SAT";

        const string MSG_DATA_BYTE_CNT = "00000004";
        public static bool connected { get; set; } = false;
        public string TX_PACKET { get; set; } = "00";
        public string TX_DATA { get; set; } = "00000000";
        public string TX_LaserOpMode { get; set; } = "00";
        public string TX_LaserMode { get; set; } = "00";
        public string TX_LaserStartStop { get; set; } = "00";
        public string TX_LaserOpState { get; set; } = "00";
        public int laserStartStop { get; set; } = 0;
        public int laserOpState { get; set; } = 1;

        Tcp_Server LAS_Server;
        public static bool interPc_connected { get; set; } = false;

        private TcpClient interPC_client;
        private NetworkStream interPC_clientStream;

        
        public static LAS_Controller instance
        {            
            get
            {
                if (_instance == null)
                {
                    _instance = new LAS_Controller();
                }
                log.Info($"Instance");
                return _instance;
            }
        }


        public void Initialize()
        {
            Create_LasServer();
            Create_InterPCclient();
            IsInitialized = true;
            randumValue = rand.Next(100);
        }

        public void Start()
        {


        }
        public void End()
        {

        }
        public string UpdateStatus()
        {
            return state_now;
        }

        public bool isSatelliteLaserMode()
        {
            if(laserClass.Equals("SAT"))
                return true;
            else
                return false;
        }


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
                log.Debug("LAS...Dispose()");
            }
            else
            {
                if(laserClass.Equals("SAT"))
                { 
                    ////////////////////////////////////////////////////////////////////////////
                    ///                   Latch event Handlers                               //
                    //////////////////////////////////////////////////////////////////////////
                    LAS_Server = new Tcp_Server(SubSystem_PORT.LAS_SAT);                   //
                    LAS_Server.OnPacketReceivedEvent += TcpServer_OnPacketReceivedEvent;  //
                    LAS_Server.OnConnectedEvent += ChangeConnected;                      //
                    //////////////////////////////////////////////////////////////////////
                }
                else if(laserClass.Equals("DEB"))
                {
                    LAS_Server = new Tcp_Server(SubSystem_PORT.LAS_DEB);                   //
                    LAS_Server.OnPacketReceivedEvent += TcpServer_OnPacketReceivedEvent;  //
                    LAS_Server.OnConnectedEvent += ChangeConnected;                      //

                }
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
                //btn_connection.BackColor = Color.DarkOrange;
                //btn_connection.ForeColor = System.Drawing.Color.DarkOrange;
                connected = true;
                log.Info("LAS_CONNECTED");
                onConnected?.Invoke(this, new ConnectedEventArgs("LAS_CON"));

            }
            else
            {
                //btn_connection.BackColor = Color.Gray;
                //btn_connection.ForeColor = System.Drawing.Color.White;
                connected = false;
                log.Info("LAS_DISCONNECTED");
                onConnected?.Invoke(this, new ConnectedEventArgs("LAS_DISCON"));
            }
        }



        /// <summary>
        /// Create_InterPCclient
        /// :InterPC(OPC-OBC) messaging
        /// </summary>
        private void Create_InterPCclient()
        {
            Thread connectThread = new Thread(new ThreadStart(ConnectInBackground));
            connectThread.IsBackground = true;
            connectThread.Start();
        }
        private void ConnectInBackground()
        {
            interPC_client = new TcpClient();
             
            while (!interPC_client.Connected)
            {
                try
                {
                    interPC_client.Connect(NSLR_IP.OCU_OPC, InterPC_PORT.LAS);
                    log.Info($"LAS_InterPC [{laserClass}] [_CONNECTION_]");
                    interPC_clientStream = interPC_client.GetStream();
                }
                catch (Exception ex)
                {
                    log.Info($"LAS_InterPC [{laserClass}] DISconnected");
                    Thread.Sleep(500);
                }
            }
            Thread interPC_receiveThread = new Thread(new ThreadStart(interPC_receiveMessage));
            interPC_receiveThread.Start();
        }
        private void interPC_receiveMessage()
        {
            try
            {
                while (true)
                {
                    byte[] receiveData = new byte[100];
                    var bytesRead = interPC_clientStream.Read(receiveData, 0, 100);
                    if (bytesRead == 0)
                        break;
                    string interPCmessage = Encoding.ASCII.GetString(receiveData, 0, bytesRead);
                    Console.WriteLine($">>>>>>>  Get OPC Command [{laserClass}] [{bytesRead}]{interPCmessage}");
                    if (interPCmessage.Contains("PBIT"))
                        SetBIT("PBIT");
                    else if (interPCmessage.Contains("IBIT"))
                        SetBIT("IBIT");
                }
            }
            catch (Exception ex)
            {
                //log.Debug($"Error on LAS interPC_receiveMessage() : {ex.Message}");
                ConnectInBackground();
            }

        }


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
        private void TcpServer_OnPacketReceivedEvent(byte[] recvData, byte[] stx, byte[] msgID, byte[] seqNNum,  byte[] dataCNT, byte[] data, byte[] etx)
        {
            var MsgStr = "";

            var RcvdData = (BitConverter.ToString(recvData)).Replace("-", string.Empty);
            var SOM = (BitConverter.ToString(stx)).Replace("-", string.Empty);
            var MsgID = (BitConverter.ToString(msgID)).Replace("-", string.Empty);
            var DataLen = (BitConverter.ToString(dataCNT)).Replace("-", string.Empty);
            var Data = (BitConverter.ToString(data)).Replace("-", string.Empty);
            var EOM = (BitConverter.ToString(etx)).Replace("-", string.Empty);

            log.Debug($"RX:[{laserClass}][{RcvdData}]");

            if (MsgID.Equals(ControlCommand.MSG_ID_LASER_OP_RSP))
                MsgStr = "STATUS";
            else if (MsgID.Equals(ControlCommand.MSG_ID_LASER_PBIT_RSP))
                MsgStr = "PBIT";
            else if (MsgID.Equals(ControlCommand.MSG_ID_LASER_IBIT_RSP))
                MsgStr = "IBIT";

            if (MsgStr.Equals("STATUS"))
            {
                try
                {
                    string jsonMessage = JsonConvert.SerializeObject(new MONITOR_LAS_DATA
                    {                        
                        ID = "LAS_RSP_CTRL",
                        LASopMode = Data.Substring(0, 2),
                        LaserMode = Data.Substring(2, 2),
                        LaserFireState = Data.Substring(4, 2),
                        LASopState = Data.Substring(6, 2),
                        CbitResult1 = Data.Substring(8, 2),
                        CbitResult2 = Data.Substring(10, 2),
                    });
                    if (interPC_client.Connected)
                    {
                        log.Debug($"InterPC_Messaging [{laserClass}] {jsonMessage}");
                        InterPC_Messaging(jsonMessage);
                    }

                    // 관측제어부 PC 모니터링용
                    monitor_las_data.ID = "LAS_RSP_CTRL";
                    monitor_las_data.LASopMode = Data.Substring(0, 2);
                    monitor_las_data.LaserMode = Data.Substring(2, 2);
                    monitor_las_data.LaserFireState = Data.Substring(4, 2);
                    monitor_las_data.LASopState = Data.Substring(6, 2);
                    monitor_las_data.CbitResult1 = Data.Substring(8, 2);
                    monitor_las_data.CbitResult2 = Data.Substring(10, 2);
                }
                catch (Exception ex)
                {
                    // 예외 처리: 서버와의 통신 중 문제가 발생한 경우
                    log.Debug($"PacketReceiveEvent: [{laserClass}] {ex.Message}");
                }
            }
            else if (MsgStr.Contains("PBIT"))
            {
                try
                {
                    string jsonMessage = JsonConvert.SerializeObject(new MONITOR_LAS_BIT
                    {
                        ID = "LAS_RSP_PBIT",
                        BitResult1 = Data.Substring(0, 2),
                        BitResult2 = Data.Substring(2, 2),
                    });
                    if (interPC_client.Connected)
                    {
                        log.Debug($"InterPC_Messaging [{laserClass}] {jsonMessage}");
                        InterPC_Messaging(jsonMessage);
                    }
                    
                }
                catch (Exception ex)
                {
                    // 예외 처리: 서버와의 통신 중 문제가 발생한 경우
                    log.Debug($"PacketReceiveEvent: [{laserClass}] {ex.Message}");
                }
            }
            else if (MsgStr.Contains("IBIT"))
            {
                try
                {
                    string jsonMessage = JsonConvert.SerializeObject(new MONITOR_LAS_BIT
                    {
                        ID = "LAS_RSP_IBIT",
                        BitResult1 = Data.Substring(0, 2),
                        BitResult2 = Data.Substring(2, 2),
                    });
                    if (interPC_client.Connected)
                    {
                        log.Debug($"InterPC_Messaging [{laserClass}] {jsonMessage}");
                        InterPC_Messaging(jsonMessage);
                    }                    
                }
                catch (Exception ex)
                {
                    // 예외 처리: 서버와의 통신 중 문제가 발생한 경우
                    log.Debug($"PacketReceiveEvent: {ex.Message}");
                }
            }
            //Console.WriteLine($"[Connection:{connected}] RcvPacket [{MsgStr}] [{RcvdData}] Sof [{SOM}] ID [{MsgID}] DataLen [{DataLen}] Data [{Data}] Eof [{EOM}]");
        }




        private void InterPC_Messaging(string jsonMessage)
        {
            if (interPC_client.Connected)
            {
                try
                {
                    byte[] jsonData = Encoding.UTF8.GetBytes(jsonMessage);
                    interPC_clientStream.Write(jsonData, 0, jsonData.Length); // 직렬화된 데이터 전송
                    //log.Debug("InterPC_Messaging()..." + jsonMessage);
                    //interPC_client.Close();
                }
                catch (Exception ex)
                {
                    log.Error($"InterPC_Messaging(): {ex.Message}");
                }
            }
            else 
            {
                log.Error($"InterPC_Messaging()...InterPC Disconnected");
            }
        }


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
                log.Info($"Send [{bit}] Command");
                LAS_Server.SendData(TX_PACKET);
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
                //btn_laser_startStop.Text = "레이저 조사시작";
                //btn_laser_startStop.BackColor = Color.Blue;
                //btn_laser_startStop.ForeColor = Color.Orange;
            }
            else if (laserStartStop == 0)
            {
                //btn_laser_startStop.Text = "레이저 조사중지";
                //btn_laser_startStop.BackColor = Color.LightGray;
                //btn_laser_startStop.ForeColor = Color.Black;
            }
            */
            TX_PACKET = ControlCommand.MSG_SOM + ControlCommand.MSG_ID_LASER_OP_CMD + MSG_DATA_BYTE_CNT + data + ControlCommand.MSG_EOM;
            if (connected)
                LAS_Server.SendData(TX_PACKET);
        }
       
    }
}
