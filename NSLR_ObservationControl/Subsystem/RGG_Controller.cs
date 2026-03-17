using Emgu.Util;
using log4net;
using Newtonsoft.Json;
using NSLR_ObservationControl.Network;
using NSLR_ObservationControl.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace NSLR_ObservationControl.Subsystem
{
    public class RGG_Controller : IRGG
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static RGG_Controller _instance;
        public event EventHandler<ConnectedEventArgs> SerialConnected;
        public bool IsInitialized { get; private set; }
        string state_now;
        public enum ProcessState
        {
            Inactive,
            Active,
            Paused,
            Terminated
        }

        public enum Command
        {
            Begin,
            End,
            Pause,
            Resume,
            Exit
        }

        private string rggClass { get; set; } = "SAT";
        public ulong seq_num_rx { get; set; }
        public ulong seq_num_tx { get; set; }
        public static bool connected { get; set; }
        public string portNow { get; set; }

        //Packet Size
        string pLEN = "1A"; //1Byte (26) 

        //[1] 
        string pRGG_Control = "0001"; //2Byte 

        //[2,3]Gate Pulse
        static string pGatePulseWidth = "0000"; //2Byte
        static string pGatePulseStartOffset = "0000"; //2Byte

        //[4,5]Avoid 
        static string pAvoidSetPositionStartOffset = "0000"; //2Byte
        static string pAvoidWidth = "0000"; //2Byte

        //[6,7] LookUpTable 
        static string pLUT_UTC = "0000000000000000"; //8Byte
        static string pLUT_Delay = "0000000000000000"; //8Byte


        static string pMSG_ID = "01030001"; //4Byte
        static string pCHECKSUM = ""; //1Byte
        static string pPacket = "";

        const string CTRL = "01030101";
        const string PBIT = "01030102";
        const string IBIT = "01030103";
        const string CBIT = "01030104";

        Serial_RGG rggSerial;

        private TcpClient interPC_client;
        private NetworkStream interPC_clientStream;

        public static RGG_Controller instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RGG_Controller();
                }
                log.Info("Instance");

                return _instance;
            }
        }


        private void ChangeConnected(string bCon)
        {
            if (bCon.Contains("Connected"))
            {
                connected = true;
                log.Info("RGG_CONNECTED");
                SerialConnected?.Invoke(this, new ConnectedEventArgs("Connected"));
            }
            else
            {
                connected = false;
                log.Info("RGG_DISCONNECTED");
                SerialConnected?.Invoke(this, new ConnectedEventArgs("Disconnected"));
            }
        }

        public void OnPacketReceivedEvent(byte[] recvData)
        {

            string strData;
            string whatBIT;

            log.Info($"[RX] {BitConverter.ToString(recvData).Replace("-", string.Empty)}");
            //MessageBox.Show(BitConverter.ToString(recvData).Replace("-", string.Empty));

            //Data 전달 to 운영제어PC 
            //((BitConverter.ToString(recvData)).Replace("-", string.Empty));

            var strPacket = (BitConverter.ToString(recvData)).Replace("-", string.Empty);
            var strDataLen = strPacket.Substring(2, 2);  //DataLength  1Byt
            var DataLen = Convert.ToInt32(strDataLen, 16);
            var strMSGID = strPacket.Substring(4, 8);  //ID 4Byte            


            //엔디언 변경
            strMSGID = ConvertEndian(strMSGID); 


            StringBuilder stringBuilder = new StringBuilder();

            if (strMSGID.Contains("0103"))
                connected = true;
            else
                connected = false;

            string[] searchStrings = { PBIT, IBIT, CBIT };
            foreach (string searchString in searchStrings)
            {
                if (strMSGID.Contains(searchString))
                {
                    strData = strPacket.Substring(12, 2); //Data NB 
                    var data = int.Parse(strData);
                    //2023.11 By Little Endian 
                    //cb_DDR.Checked = (setting & 1) != 0;
                    //cb_FPGAclk.Checked = (setting & 2) != 0;
                    //cb_GPScom.Checked = (setting & 4) != 0;

                    if (strMSGID.Equals(PBIT)) whatBIT = "RGG_PBIT_RSP";
                    else if (strMSGID.Equals(IBIT)) whatBIT = "RGG_IBIT_RSP";
                    else if (strMSGID.Equals(CBIT)) whatBIT = "RGG_CBIT_RSP";
                    else whatBIT = "";
                    var whichBIT = new MONITOR_RGG_BIT
                    {
                        ID = whatBIT,
                        DDR = ((data & 4) != 0),
                        FPG = ((data & 2) != 0),
                        GPS = ((data & 1) != 0),
                    };
                    string jsonData = JsonConvert.SerializeObject(whichBIT);
                    InterPC_Messaging(jsonData);
                    Console.WriteLine($"[BIT_RESULT packet] {jsonData}");
                    log.Info(jsonData);
                }
            }

            if (strMSGID.Contains(CTRL))
            {
                strData = strPacket.Substring(12, DataLen * 2); //Data NB     

                var whatCTRL = new MONITOR_RGG_DATA
                {
                    ID = "RGG_RSP_DATA",
                    RGGctrl = strData.Substring(0, 4),
                    GatePulseWidth = strData.Substring(4, 4),
                    GatePulseStartOffset = strData.Substring(8, 4),
                    AvoidSetPositionStartOffset = strData.Substring(12, 4),
                    AvoidWidth = strData.Substring(16, 4),
                    LookupTableUTC = strData.Substring(20, 16),
                    LookupTableDelay = strData.Substring(36, 16),
                };
                string jsonData = JsonConvert.SerializeObject(whatCTRL);
                InterPC_Messaging(jsonData);
                log.Info(jsonData);


            }
            //seq_num_rx++;
            //label_rcvdPacket_counter.Text = seq_num_rx.ToString();
        }


        public void Open_Port(string satOrDebris)
        {

            rggSerial.OpenSerial(satOrDebris); //LabTest 

            /*  Target Setting.......Don't remove it.
            if (rggClass.Equals("SAT"))  
                rggSerial.OpenSerial("COM5"); //거창NSLR - 인공위성용
            else if (rggClass.Equals("DEB")) 
                rggSerial.OpenSerial("COM6");//거창NSLR - 우주물체용
            */
            portNow = satOrDebris;
        }


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
                    interPC_client.Connect(NSLR_IP.OCU_OPC, InterPC_PORT.OES);
                    log.Info("RGG_InterPC [_CONNECTION_]");
                    interPC_clientStream = interPC_client.GetStream();
                }
                catch (Exception ex)
                {
                    log.Info("RGG_InterPC_DISconnected");
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
                    log.Debug($">>>>>>>  Get OPC Command [{bytesRead}]{interPCmessage}");
                    if (interPCmessage.Contains("PBIT"))
                        Cmd_Bit("PBIT");
                    else if (interPCmessage.Contains("IBIT"))
                        Cmd_Bit("IBIT");
                    else if (interPCmessage.Contains("CBIT"))
                        Cmd_Bit("CBIT");

                }
            }
            catch (Exception ex)
            {
                //log.Debug($"Error on RGG interPC_receiveMessage() : {ex.Message}");
                ConnectInBackground();
            }

        }

        private void InterPC_Messaging(string jsonMessage)
        {
            if (interPC_client.Connected)
            {
                try
                {
                    byte[] jsonData = Encoding.UTF8.GetBytes(jsonMessage);
                    interPC_clientStream.Write(jsonData, 0, jsonData.Length);// 직렬화된 데이터 전송
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





        private string ConvertEndian(string code)
        {
            int length = code.Length;
            //Console.WriteLine($"Length : {length}");
            StringBuilder sb = new StringBuilder();

            for (int i = length - 2; i >= 0; i = i - 2) //문자열역전
            {
                sb.Append(code.Substring(i, 2));
            }
            string result = sb.ToString(); //다시 string 타입변환
            //Console.WriteLine(result);
            return result;
        }


        ///
        private string Checksum(string sTmpMsg)
        {
            //sTmpMsg = pLEN + pMSG_ID + pDATA;            
            byte[] convertArr = new byte[(sTmpMsg.Length) / 2];
            for (int i = 0; i < convertArr.Length; i++)
            {
                convertArr[i] = Convert.ToByte(sTmpMsg.Substring(i * 2, 2), 16);
            }
            int checksum = 0;
            //Step1: Exclusive OR values.            
            foreach (byte value in convertArr)
            {
                Debug.WriteLine($"EXOR : {checksum} ^ {value.ToString("X")} = {checksum ^ value}");
                checksum ^= value;
            }
            //-checksum = 256 - checksum;
            checksum &= 0xFF; // FFFFFF replace
            return checksum.ToString("X2");
        }

        /// <summary>
        /// Issue RGG COMMAND - Control 
        /// </summary>
        /// <param name="mode"></param>
        public void Cmd_Mode(string mode)
        {
            pRGG_Control = mode;
            Set_Command();
        }

        /// <summary>
        /// Issue RGG COMMAND - Gate Pulse Width 
        /// </summary>
        /// <param name="gpw"></param>
        public void Cmd_GatePulse_W(string gpw)
        {
            pGatePulseWidth = gpw;
            Set_Command();
        }

        /// <summary>
        /// Issue RGG COMMAND - Gate Pulse Start Offset 
        /// </summary>
        /// <param name="gpso"></param>
        public void Cmd_GatePulse_SO(string gpso)
        {
            pGatePulseStartOffset = gpso;
            Set_Command();
        }

        /// <summary>
        /// Issue RGG COMMAND - Avoid SetPostion Start Offset
        /// </summary>
        /// <param name="avso"></param>
        public void Cmd_Avoid_SO(string avso)
        {
            pAvoidSetPositionStartOffset = avso;
            Set_Command();
        }

        /// <summary>
        /// Issue RGG COMMAND - Avoid Width
        /// </summary>
        /// <param name="avw"></param>
        public void Cmd_Avoid_W(string avw)
        {
            pAvoidWidth = avw;
            Set_Command();
        }

        /// <summary>
        /// ssue RGG COMMAND - LUT Utc
        /// </summary>
        /// <param name="lut_utc"></param>
        /// 
        public void Cmd_Lut_Utc(string lut_utc)
        {
            pLUT_UTC = lut_utc;
            Set_Command();
        }

        /// <summary>
        /// Issue RGG COMMAND - LUT Delay
        /// </summary>
        /// <param name="lut_delay"></param>
        public void Cmd_Lut_Delay(string lut_delay)
        {
            pLUT_Delay = lut_delay;
            Set_Command();
        }

        /// <summary>
        /// SendPacket from GUI 
        /// </summary>
        /// <param name="packet"></param>

        public void GUISendPacket(string packet)
        {
            if (connected)
            {
                rggSerial.SendData(packet);
                Debug.WriteLine($"[OES_RGG] SetCommand  {packet}");
                //else { periodicSend_timer.Enabled = false; }
            }
            else
            {
                MessageBox.Show("RGG Disconnction 입니다. Serial Port를 확인이 필요합니다.");
            }
        }


        public void Set_Command()
        {
            if (connected)
            {
                pLEN = "1A";
                pMSG_ID = ConvertEndian(ControlCommand.MSG_ID_RGG_OP_CMD);
                var pData = ConvertEndian(pRGG_Control) + ConvertEndian(pGatePulseWidth) + ConvertEndian(pGatePulseStartOffset) +
                    ConvertEndian(pAvoidSetPositionStartOffset) + ConvertEndian(pAvoidWidth) + ConvertEndian(pLUT_UTC) + ConvertEndian(pLUT_Delay);
                pCHECKSUM = Checksum(pLEN + pMSG_ID + pData);

                pPacket = ControlCommand.MSG_SOF + pLEN + pMSG_ID + pData + pCHECKSUM + ControlCommand.MSG_EOT;

                seq_num_tx = rggSerial.SendData(pPacket);
                Debug.WriteLine($"[OES_RGG] SetCommand  {pPacket}");
                //else { periodicSend_timer.Enabled = false; }
            }
            else
            {
                MessageBox.Show("RGG Disconnction 입니다. Serial Port를 확인이 필요합니다.");
            }
        }

        public void Cmd_Bit(string cmd)
        {
            if (connected)
            {
                if ((cmd.Equals("PBIT") || cmd.Equals("IBIT") || cmd.Equals("CBIT")))
                {
                    pLEN = "00";

                    if (cmd.Equals("PBIT")) { pMSG_ID = ControlCommand.MSG_ID_RGG_PBIT_CMD; }
                    else if (cmd.Equals("IBIT")) { pMSG_ID = ControlCommand.MSG_ID_RGG_IBIT_CMD; }
                    else if (cmd.Equals("CBIT")) { pMSG_ID = ControlCommand.MSG_ID_RGG_CBIT_CMD; }

                    pCHECKSUM = Checksum(pLEN + pMSG_ID);
                    pPacket = ControlCommand.MSG_SOF + pLEN + ConvertEndian(pMSG_ID) + pCHECKSUM + ControlCommand.MSG_EOT;
                    seq_num_tx = rggSerial.SendData(pPacket);
                }
            }
            else
            {
                MessageBox.Show("RGG Disconnction 입니다. Serial Port를 확인이 필요합니다.");
            }
        }


        public void Initialize()
        {

            if (rggSerial != null)
            {
                rggSerial.Dispose();
            }
            rggSerial = new Serial_RGG();
            /*
            if (SatOrDebris.Equals("SAT") || (SatOrDebris.Equals("DEB")))
            {
                rggClass = SatOrDebris;
            }
            */
            rggSerial.OnConnectedEvent += ChangeConnected;
            rggSerial.OnPacketReceivedEvent += OnPacketReceivedEvent;

            Create_InterPCclient();
            IsInitialized = true;
        }

        public void doPBIT()
        {
            //Do PBIT
            state_now = "PBIT";
            log.Info(state_now);
            Cmd_Bit(state_now);
        }


        public void Start()
        {
            // 시작 로직
            state_now = "Start";
            log.Info(state_now);
        }

        public void End()
        {
            // 중지 로직
            state_now = "Stop";
            log.Info(state_now);
        }

        public string UpdateStatus()
        {
            // 상태 업데이트 로직
            return state_now;

        }


        public void ExecuteCommand(string command)
        {
            // 명령 실행 로직
            state_now = "ExecuteCommand:[ ]";
            log.Info(state_now);
        }

    }

}
