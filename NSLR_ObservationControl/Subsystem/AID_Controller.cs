using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using NSLR_ObservationControl.Network;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using System.Web.UI;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;
using mv.impact.acquire;
using System.IO.Packaging;
using NSLR_ObservationControl.Module;

namespace NSLR_ObservationControl.Subsystem
{
    public class AID_Controller : IAID
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static AID_Controller _instance;
        public string Tag => "ALL";
        public event EventHandler<ConnectedEventArgs> AID_Connected;
        public event EventHandler<PauseEventArgs> Airplane_Detected;


        private LAS_SAT_Controller laser;

        //////////////////////////
        //   항공기탐지 ICD
        //////////////////////////

        Tcp_Server AirPlaneDetector_Server;
        private TcpClient connectedClient;

        public bool IsInitialized { get; private set; }
        private bool connected { get; set; } = false;
        private bool connected_prev {  get; set; } = false; 


        public string TX_PACKET { get; set; } = "00";
        public string TX_DATA { get; set; } = "00000000";

        const string MSG_DATA_BYTE_CNT = "00000004";

        public string state_ID = "";
        public string state_OPMODE = "00";
        public string state_RADAR = "00";
        public string state_ADS_B = "00";
        public string state_CBIT = "00";
        public string state_PBIT = "00";
        public string state_IBIT = "00";
        public byte state_CBIT_B0 = 0;
        public byte state_CBIT_B1 = 0;
        public byte state_CBIT_B2 = 0;
        public byte state_PBIT_B0 = 0;
        public byte state_PBIT_B1 = 0;
        public byte state_PBIT_B2 = 0;
        public byte state_IBIT_B0 = 0;
        public byte state_IBIT_B1 = 0;
        public byte state_IBIT_B2 = 0;


        public bool isAirplaneDetected { get; set; } = false;
        private bool AirplaneDetected_Previous { get; set; } = false;

        private Timer survailanceTimer = new Timer();

        public AIDUserData userData;

        public static AID_Controller instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AID_Controller();
                    //log.Info("new object");
                }
                //else { log.Info("Instance"); }
                return _instance;
            }
        }

        public AID_Controller()
        {
            laser = LAS_SAT_Controller.instance; //일시정지 - 레이저 연동
        }

        public void Initialize()
        {
            survailanceTimer.Tick += survailanceTimer_Tick;
            survailanceTimer.Interval = 200; //5Hz  

            userData = new AIDUserData
            {
                ID = state_ID,
                AIDopMode = state_OPMODE,
                RadarSyncState = state_RADAR,
                ADSbSyncState = state_ADS_B,
                BITResult = state_CBIT,
            };

            initialize_networks();
            log.Info("Initialize()");
            survailanceTimer.Start();
        }

        public void doPBIT()
        {
            log.Info("PBIT");
        }

        public void Start()
        {
            log.Info("Start");
        }
        public void End()
        {
            log.Info("End");
        }


        public void initialize_networks()
        {
            Create_AirPlaneDetectorServer();
        }
        private void survailanceTimer_Tick(object sender, EventArgs e)
        {
            seqNum++;
            string seqNumString = seqNum.ToString("D8");
           // Console.WriteLine($"{Observation_TMS.TMSAZPositon.ToString()}");
            //Console.WriteLine($"{Observation_TMS.TMSELPosition.ToString()}");

            byte[] Az1 = BitConverter.GetBytes(Observation_TMS.TMSAZPositon);
            byte[] El1 = BitConverter.GetBytes(Observation_TMS.TMSELPosition);
            AzString = BitConverter.ToString(Az1).Replace("-", "");
            ElString = BitConverter.ToString(El1).Replace("-", "");

            TX_PACKET = ControlCommand.MSG_SOM + ControlCommand.MSG_ID_AID_OP_CMD + "11111111" + "0000000f" + AzString + ElString + ControlCommand.MSG_EOM;
            //TX_PACKET = ControlCommand.MSG_SOM + ControlCommand.MSG_ID_AID_OP_CMD + "11111111" + "0000000f" + Observation_TMS.TMSAZPositon.ToString() + Observation_TMS.TMSELPosition.ToString() + ControlCommand.MSG_EOM;
            if (connected)
            {
            //    Console.WriteLine($"[ToAirplaneDetector] SeqNum #{seqNum} Az {AzString} , El {ElString}"); // 출력: 123.46
            //    Console.WriteLine($"AID set cmd : {TX_PACKET}");
                AirPlaneDetector_Server.SendData(TX_PACKET);
            }

            //survailanceTimer.Stop();
        }



        private void Create_AirPlaneDetectorServer()
        {
            //Server for SubSystem messaging
            if (AirPlaneDetector_Server != null)
            {
                AirPlaneDetector_Server.Dispose();
                log.Debug("LAS...Dispose()");
            }
            else
            {
                ////////////////////////////////////////////////////////////////////////////
                ///                   Latch event Handlers                               //
                //////////////////////////////////////////////////////////////////////////
                AirPlaneDetector_Server = new Tcp_Server(SubSystem_PORT.AID);
                AirPlaneDetector_Server.OnPacketReceivedEvent -= TcpServer_OnPacketReceivedEvent;
                AirPlaneDetector_Server.OnPacketReceivedEvent += TcpServer_OnPacketReceivedEvent;
                AirPlaneDetector_Server.OnConnectedEvent += ChangeConnected;
            }
        }

        private void ChangeConnected(bool bCon)
        {
            if (bCon)
            {
                connected = true;
                if (connected_prev != connected)
                {
                    log.Info(IPC_MSG.CONNECTION_AID);
                    AID_Connected?.Invoke(this, new ConnectedEventArgs(IPC_MSG.CONNECTION_AID));
                    connected_prev = connected;
                }
            }
            else
            {
                connected = false;
                if (connected_prev != connected)
                {
                    log.Info(IPC_MSG.DISCONNECTION_AID);
                    AID_Connected?.Invoke(this, new ConnectedEventArgs(IPC_MSG.DISCONNECTION_AID));
                    connected_prev = connected;
                }
            }
        }

        public bool IsConnected()
        {
            return connected;
        }

        public void RaiseDetectionEvent(bool detection)
        {            
            if (detection)
            {
                isAirplaneDetected = true;
                if (AirplaneDetected_Previous != isAirplaneDetected)
                {
                    laser.laserStop_SatTrack();
                    AirplaneDetected_Previous = isAirplaneDetected;
                    Airplane_Detected?.Invoke(this, new PauseEventArgs("PAUSE"));
                }
            }
            else 
            {
                isAirplaneDetected = false;
                if (AirplaneDetected_Previous != isAirplaneDetected)
                {
                    AirplaneDetected_Previous = isAirplaneDetected;
                    Airplane_Detected?.Invoke(this, new PauseEventArgs("RESUME"));
                }
            }
        }
    
        private int ExtractBits(string hexString, int startBit, int length)
        {            
            byte value = Convert.ToByte(hexString, 16); //Hex string을 byte로 변환
            int mask = ((1 << length) - 1) << startBit; //Masking
            return (value & mask) >> startBit;//BITting
        }


        private void TcpServer_OnPacketReceivedEvent(byte[] recvData, byte[] msgID, byte[] data)
        {
            var MsgStr = "";
             var RcvdData = (BitConverter.ToString(recvData)).Replace("-", string.Empty);
            //var SOM = (BitConverter.ToString(stx)).Replace("-", string.Empty);
            var MsgID = (BitConverter.ToString(msgID)).Replace("-", string.Empty);
            //var DataLen = (BitConverter.ToString(dataCNT)).Replace("-", string.Empty);
            var Data = (BitConverter.ToString(data)).Replace("-", string.Empty);
            //var EOM = (BitConverter.ToString(etx)).Replace("-", string.Empty);

            //log.Debug($"[AID] RX: [{RcvdData}]");

            if (MsgID.Equals(ControlCommand.MSG_ID_AID_STAT_RSP))
                MsgStr = "STATUS";
            else if (MsgID.Equals(ControlCommand.MSG_ID_AID_PBIT_RSP))
                MsgStr = "PBIT";
            else if (MsgID.Equals(ControlCommand.MSG_ID_AID_IBIT_RSP))
                MsgStr = "IBIT";

            if (MsgStr.Equals("STATUS"))
            {
                if (ExtractBits(Data.Substring(6, 2), 4, 1) == 1)
                { RaiseDetectionEvent(true); }
                else
                {  RaiseDetectionEvent(false); }
                state_ID = MsgStr;
                state_OPMODE = Data.Substring(0, 2);
                state_RADAR = Data.Substring(2, 2);
                state_ADS_B = Data.Substring(4, 2);
                state_CBIT = Data.Substring(6, 2);
                state_CBIT_B0 = (byte)ExtractBits(Data.Substring(6, 2), 0, 1);
                state_CBIT_B1 = (byte)ExtractBits(Data.Substring(6, 2), 1, 1);
                state_CBIT_B2 = (byte)ExtractBits(Data.Substring(6, 2), 2, 1);

                userData.ID = state_ID;
                userData.AIDopMode = state_OPMODE;
                userData.RadarSyncState = state_RADAR;
                userData.ADSbSyncState = state_ADS_B;
                userData.BITResult = state_CBIT;
            }
            else if (MsgStr.Contains("PBIT"))
            {
                state_ID = MsgStr;
                state_PBIT = Data.Substring(0, 2);
                state_PBIT_B0 = (byte)ExtractBits(state_PBIT, 0, 1);
                state_PBIT_B1 = (byte)ExtractBits(state_PBIT, 1, 1);
                state_PBIT_B2 = (byte)ExtractBits(state_PBIT, 2, 1);

                userData.ID = state_ID;
                userData.BITResult = state_PBIT;
            }
            else if (MsgStr.Contains("IBIT"))
            {
                state_ID = MsgStr;
                state_IBIT = Data.Substring(0, 2);
                state_IBIT_B0 = (byte)ExtractBits(state_PBIT, 0, 1);
                state_IBIT_B1 = (byte)ExtractBits(state_PBIT, 1, 1);
                state_IBIT_B2 = (byte)ExtractBits(state_PBIT, 2, 1);

                userData.ID = state_ID;
                userData.BITResult = state_IBIT;
            }
        }


        public void SetBIT(string bit)
        {
            if (bit.Equals("PBIT"))
                TX_PACKET = ControlCommand.MSG_SOM + ControlCommand.MSG_ID_AID_PBIT_CMD + "00000000" + "00000000"+ ControlCommand.MSG_EOM;
            else if (bit.Equals("IBIT"))
                TX_PACKET = ControlCommand.MSG_SOM + ControlCommand.MSG_ID_AID_IBIT_CMD + "00000000" + "00000000"+ ControlCommand.MSG_EOM;

            if (TX_PACKET is "")
            {
                Console.WriteLine("Data 선택 부터 하세요.");
                return;
            }

            if (connected)
            {
                log.Info($"Send [{bit}] Command");
                AirPlaneDetector_Server.SendData(TX_PACKET);
            }
        }


        UInt32 seqNum = 0;
        string AzString;
        string ElString;
        /// <summary>
        /// ///////////////////////////////
        /// Command Laser Start/Stp
        /// ///////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SetAzEl(double Az, double El)
        {
            //AzString = ConvertToHexString(Az);
            //ElString = ConvertToHexString(El);

            byte[] Az1 = BitConverter.GetBytes(Az);
            byte[] El1 = BitConverter.GetBytes(El);
            AzString = BitConverter.ToString(Az1).Replace("-", "");
            ElString = BitConverter.ToString(El1).Replace("-", "");

         

        }

        public static string ConvertToHexString(double number)
        {
            // 정수 부분과 소수 부분 분리
            int integerPart = (int)number;
            double fractionalPart = number - integerPart;

            // 정수 부분을 16진수로 변환
            string integerHex = integerPart.ToString("X8");

            // 소수 부분을 16진수로 변환
            int fractionalHex = 0;
            for (int i = 0; i < 4; i++)
            {
                fractionalPart *= 16;
                int digit = (int)fractionalPart;
                fractionalHex = (fractionalHex << 4) | digit;
                fractionalPart -= digit;
            }            
            return $"{integerHex}{fractionalHex:X8}";
        }
    }
}

