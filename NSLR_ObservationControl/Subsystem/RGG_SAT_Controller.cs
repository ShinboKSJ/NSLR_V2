using log4net;
using NSLR_ObservationControl.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;


namespace NSLR_ObservationControl.Subsystem
{
    public class RGG_SAT_Controller : IRGG 
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public event EventHandler<ConnectedEventArgs> SerialConnected;
        
        private static RGG_SAT_Controller _instance;

        public string Tag => "SLR";

        private const string rggClass = "RGG_SAT";
        Serial_RGG rggSerial;
        private System.Timers.Timer monitorConnection_Timer;
        private bool connected { get; set; }
        public bool IsInitialized { get; private set; }

        private ulong seq_num_rx = 0;
        private ulong seq_num_tx = 0;

        private bool connected_prev { get; set; } = false;
        public string portNow { get; set; }

        private string state_now;
        public string strBitResultCode { get; set; } = "00000000";
        public string RggControl { get; set; } = "0000";

        public double RggTimeBias { get; set; }
        public string RggGatePulseStartOffset { get; set; }
        public Int16 n_RggGatePulseStartOffset { get; set; }
        public string RggGatePulseWidth { get; set; }
        public UInt16 n_RggGatePulseWidth { get; set; }
        private string RggAvoidSetPositionStartOffset {  get; set; }
        private string RggAvoidWidth {  get; set; }
        private string RggLookupTableSize {  get; set; }
        private string RggUtcNow { get; set; }
        public double  n_RggUtcNow { get; set; }
        private string RggTofNow { get; set; }
        public double n_RggTofNow { get; set; }

        private double tempRggUtcNow;
        private double tempRggTofNow;

        //Packet Size
        //string pLEN = "1A"; //1Byte (26) 
        //string pLEN = "AA"; //25.01.16 Jason 170 Byte for ToF Info 
        string pLEN = "5A"; //25.01.16 Jason 90 Byte for ToF Info 
        //[1] 
        string pRGG_Control = "0001"; //2Byte 

        //[2,3]Gate Pulse
        private string pGatePulseWidth = "0000"; //2Byte
        static string pGatePulseStartOffset = "0000"; //2Byte

        //[4,5]Avoid 
        static string pAvoidSetPositionStartOffset = "0000"; //2Byte
        static string pAvoidWidth = "0000"; //2Byte

        //[6,7] LookUpTable 
        //static string pLUT_UTC = "0000000000000000"; //8Byte
        //static string pLUT_Delay = "0000000000000000"; //8Byte
        static string pLUT_UTC = new string('0', 80);  
        static string pLUT_Delay = new string('0', 80);
        static string pLUT_DATA = "0000000000000000"; //160Byte

        static string pMSG_ID = "01030001"; //4Byte
        static string pCHECKSUM = ""; //1Byte
        static string pPacket = "";

        const string CTRL = "01030101";
        const string PBIT = "01030102";
        const string IBIT = "01030103";
        const string CBIT = "01030104";
        const string TOF = "01030105";

        const int TOF_COUNT = 5; //관측스케줄에 의한 궤도정보(ToF) 갯수 

        public RGGUserData userData;
        public RGGUserData2 userData2;

        public RGG_SAT_Controller()
        {
            monitorConnection_Timer = new System.Timers.Timer(1000);
            monitorConnection_Timer.Elapsed += MonitorConnection;
            monitorConnection_Timer.Start();
        }

        public static RGG_SAT_Controller instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RGG_SAT_Controller();
                }
                //log.Info("Instance");
                return _instance;
            }
        }


        public void Initialize()
        {
            if (rggSerial != null)
            {
                rggSerial.Dispose();
            }
            rggSerial = new Serial_RGG(rggClass);

            rggSerial.OnPacketReceivedEvent -= OnPacketReceivedEvent;
            rggSerial.OnPacketReceivedEvent += OnPacketReceivedEvent;
                        
            userData = new RGGUserData
            {
                ID = "INIT",
                RggControl = "0",
                GatePulseWidth = "0",
                GatePulseSO = "0",
                RcvdLUTSize = "0",
                UtcNow = "0",
                TofNow = "0",
                BIT = "INIT",
                BITResult = "00000000"
            };
            
            _ = Open();
            log.Info("Initialize()");
        }

        public async Task Open()
        {
            try
            {
                await Task.Run(() => rggSerial.OpenSerial(Serial_PORT.RGG_SAT));
                IsInitialized = true;
            }
            catch (Exception ex)
            {
                log.Fatal($"포트 열기 실패: {ex.Message}");
                IsInitialized = false;
            }
        }

        private ulong _lastSequence = 0;
        private void MonitorConnection(object sender, ElapsedEventArgs e)
        {
            if (_lastSequence == seq_num_rx)
            {
                ChangeConnected(false);
            }
            else
            {
                ChangeConnected(true);
            }
            _lastSequence = seq_num_rx;
        }

        private void ChangeConnected(bool bCon)
        {
            if(bCon)
            {
                connected = true;
                if (connected_prev != connected)
                {
                    log.Info(IPC_MSG.CONNECTION_RGG_SAT);
                    SerialConnected?.Invoke(this, new ConnectedEventArgs(IPC_MSG.CONNECTION_RGG_SAT));
                    connected_prev = connected;
                }
            }
            else
            {
                connected = false;
                if (connected_prev != connected)
                {
                    log.Info(IPC_MSG.DISCONNECTION_RGG_SAT);
                    SerialConnected?.Invoke(this, new ConnectedEventArgs(IPC_MSG.DISCONNECTION_RGG_SAT));
                    connected_prev = connected;
                }
            }
        }

        public bool IsConnected()
        {
            return connected;
        }

        
        public void OnPacketReceivedEvent(byte[] recvData)
        { 
            string strData;

            //log.Debug($"[RGG_SAT] {BitConverter.ToString(recvData)}");
            
            var strPacket = (BitConverter.ToString(recvData)).Replace("-", string.Empty);
            var strDataLen = strPacket.Substring(2, 2);  //DataLength  1Byt
            var DataLen = Convert.ToInt32(strDataLen, 16);
            var strMSGID = strPacket.Substring(4, 8);  //ID 4Byte
            
            //엔디언 변경
            strMSGID = ConvertEndian(strMSGID); 

            StringBuilder stringBuilder = new StringBuilder();

            string[] searchStrings = { PBIT, IBIT, CBIT };
            foreach (string searchString in searchStrings)
            {
                if (strMSGID.Contains(searchString))
                {
                    ChangeConnected(true);

                    //strData = strPacket.Substring(12, 2); //Data NB 
                    //var data = byte.Parse(strData);
                    //Console.WriteLine($"Rcvd: BITresult {data}");
                    //BitResult_value = data;
                    strBitResultCode = Convert.ToString(recvData[6], 2).PadLeft(8, '0');
                    //Console.WriteLine($"Rcvd: BITresult {strBitResultCode}");

                    // Set UserData
                    if (strMSGID.Equals(PBIT))
                    {
                        userData.ID = "PBIT";
                        userData.BIT = "PBIT";
                        userData.BITResult = strBitResultCode;

                    }
                    else if (strMSGID.Equals(IBIT))
                    {
                        userData.ID = "IBIT";
                        userData.BIT = "IBIT";
                        userData.BITResult = strBitResultCode;
                    }
                    else if (strMSGID.Equals(CBIT)) 
                    { 
                        userData.ID = "CBIT";
                        userData.BIT = "CBIT";
                        userData.BITResult = strBitResultCode;
                    }

                    if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                    {
                        if ((seq_num_rx % 10) == 0)
                            log.Debug($"0{userData.ID} Presult:{userData.BITResult}");
                        if ((seq_num_rx % 10) == 1)
                            log.Debug($"1{userData.ID} Presult:{userData.BITResult}");
                    }
                }
            }

            if (strMSGID.Contains(CTRL))
            {
                ChangeConnected(true);

                strData = strPacket.Substring(12, DataLen * 2); //Data NB

                RggControl = ConvertEndian(strData.Substring(0, 4));
                RggGatePulseWidth = ConvertEndian(strData.Substring(4, 4));
                RggGatePulseStartOffset = ConvertEndian(strData.Substring(8, 4));
                /*     RggAvoidSetPositionStartOffset = ConvertEndian(strData.Substring(12, 4));
                     RggAvoidWidth = ConvertEndian(strData.Substring(16, 4));*/
                string strRggTimeBias = ConvertEndian(strData.Substring(12, 8));
                if(Double.TryParse(strRggTimeBias,out double parsedTimeBias))
                {
                    RggTimeBias = parsedTimeBias;
                }
                RggLookupTableSize = ConvertEndian(strData.Substring(20, 16));
                RggUtcNow = ConvertEndian(strData.Substring(36, 16));
                RggTofNow = ConvertEndian(strData.Substring(52, 16));

                n_RggGatePulseStartOffset = Convert.ToInt16(RggGatePulseStartOffset, 16);
                n_RggGatePulseWidth = Convert.ToUInt16(RggGatePulseWidth, 16);

                if (Double.TryParse(RggUtcNow, out tempRggUtcNow)) { n_RggUtcNow = tempRggUtcNow; }
                if (Double.TryParse(RggTofNow, out tempRggTofNow)) { n_RggTofNow = tempRggTofNow; }

                //log.Debug($">>RGG GatePls.Start.Off {n_RggGatePulseStartOffset} >> {5* n_RggGatePulseStartOffset} (ns) ");
                //log.Debug($">>RGG G.Pls.Width       {n_RggGatePulseWidth} >> {5 * n_RggGatePulseWidth} (ns) ");

                // Set UserData
                userData.ID = "RGG_RSP_DATA";
                userData.RggControl = RggControl;
                userData.GatePulseWidth = RggGatePulseWidth;
                userData.GatePulseSO = RggGatePulseStartOffset;
                userData.RcvdLUTSize = ConvertEndian(strData.Substring(20, 16));
                userData.UtcNow = RggUtcNow;
                userData.TofNow = RggTofNow;

                uint control = Convert.ToUInt32(RggControl, 16);
                uint gateWidth = Convert.ToUInt32(RggGatePulseWidth, 16);
                int gateOffset = Convert.ToInt32(RggGatePulseStartOffset, 16);

                ulong lookupTableSize = Convert.ToUInt64(RggLookupTableSize, 16);
                ulong utcNow = Convert.ToUInt64(RggUtcNow, 16);
                ulong tofNow = Convert.ToUInt64(RggTofNow, 16);
                //RGG_UTCns.Add(utcNow);
               // RGG_TOF.Add(tofNow);
                log.Debug($"gatewidth {gateWidth} gateOffset {gateOffset} utcNow {utcNow} tofNow {tofNow} TimeBias {RggTimeBias}");
                if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                {
                    if ((seq_num_rx % 10) == 0)
                        log.Debug($"0Seq#{seq_num_rx} [{userData.ID}] RGGctrl:{userData.RggControl} GPW:{userData.GatePulseWidth} GPSO:{userData.GatePulseSO} RcvdLUT:{userData.RcvdLUTSize} UTCnow:{userData.UtcNow} ToFnow:{userData.TofNow} ");
                    if ((seq_num_rx % 10) == 1)
                        log.Debug($"1Seq#{seq_num_rx} [{userData.ID}] RGGctrl:{userData.RggControl} GPW:{userData.GatePulseWidth} GPSO:{userData.GatePulseSO} RcvdLUT:{userData.RcvdLUTSize} UTCnow:{userData.UtcNow} ToFnow:{userData.TofNow} ");
                }
            }
            else if (strMSGID.Contains(TOF))
            {
                ChangeConnected(true);

                strData = strPacket.Substring(12, DataLen * 2); //Data NB

                RggUtcNow = ConvertEndian(strData.Substring(0, 16));
                RggTofNow = ConvertEndian(strData.Substring(16, 16));
                ulong utcNowns = Convert.ToUInt64(RggUtcNow, 16);
                ulong tofNowns = Convert.ToUInt64(RggTofNow, 16);
                RGG_UTCns.Add(utcNowns);
                RGG_TOF.Add(tofNowns);
                var timestamp = new RggTimestamp(utcNowns, tofNowns);
                RGG_Timestamps.Enqueue(timestamp);
                log.Debug($"utcNow {utcNowns} tofNow {tofNowns}");
           /*     if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                {
                    if ((seq_num_rx % 10) == 0)
                        log.Debug($"0Seq#{seq_num_rx} [{userData.ID}] RGGctrl:{userData.RggControl} GPW:{userData.GatePulseWidth} GPSO:{userData.GatePulseSO} RcvdLUT:{userData.RcvdLUTSize} UTCnow:{userData.UtcNow} ToFnow:{userData.TofNow} ");
                    if ((seq_num_rx % 10) == 1)
                        log.Debug($"1Seq#{seq_num_rx} [{userData.ID}] RGGctrl:{userData.RggControl} GPW:{userData.GatePulseWidth} GPSO:{userData.GatePulseSO} RcvdLUT:{userData.RcvdLUTSize} UTCnow:{userData.UtcNow} ToFnow:{userData.TofNow} ");
                }*/
            }
            Interlocked.Increment(ref Unsafe.As<ulong, long>(ref seq_num_rx));
            //label_rcvdPacket_counter.Text = seq_num_rx.ToString();
        }
        public ConcurrentQueue<RggTimestamp> RGG_Timestamps = new ConcurrentQueue<RggTimestamp>();
        public List<ulong> RGG_UTCns = new List<ulong>();
        public List<ulong> RGG_TOF = new List<ulong>();
        public struct RggTimestamp
        {
            public double UtcSeconds;  
            public double TofSeconds;  

            public RggTimestamp(ulong utcNs, ulong tofNs)
            {
                UtcSeconds = utcNs / 1_000_000_000.0;
                TofSeconds = tofNs / 1_000_000_000.0;
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
            //checksum &= 0xFF; // FFFFFF replace
            return checksum.ToString("X2");
        }

        /// <summary>
        /// Issue RGG COMMAND - Control 
        /// </summary>
        /// <param name="mode"></param>
        //[1]
        public void Cmd_Mode(string mode)
        {
            pRGG_Control = mode;
            log.Debug($"SAT_RGG_Mode를 변경합니다. [{pRGG_Control}]");
            Set_Command();
        }
        /// <summary>
        /// Issue RGG COMMAND - Gate Pulse Width 
        /// </summary>
        /// <param name="gpw"></param>
        //[2]
        public void Cmd_GateControl(string mode, string gpw, string gpso)
        {
            pRGG_Control = mode;
            pGatePulseWidth = gpw; //.ToString("X4");
            pGatePulseStartOffset = gpso; //.ToString("X4");
            log.Debug($"SAT_RGG_GateControl Width:{gpw}, StartOffset:{gpso}");
            Set_Command();
        }

        public void Cmd_GateControl_N(string mode, UInt16 gpw, int gpso)
        {
            //pRGG_Control = mode;
            pGatePulseWidth = gpw.ToString("X4"); //.ToString("X4");
                        
            pGatePulseStartOffset = gpso.ToString("X8").Substring(4);
            log.Debug($"[RGG controller] >> [GateControl] Width:{gpw}, StartOffset:{gpso}");
            Set_Command();
        }

        public void Cmd_GatePulse_W(string gpw)
        {
            pGatePulseWidth = gpw; //.ToString("X4");
            log.Debug($"[RGG controller] >> [GateControl] Width:{gpw}");
            //Set_Command();
        }

        public void Cmd_GatePulse_SO(string gpso)
        {
            pGatePulseStartOffset = gpso; //.ToString("X4");
            log.Debug($"[RGG controller] >> [GateControl] StartOffset:{gpso}");
            //Set_Command();
        }


        //

        /// <summary>
        /// Issue RGG COMMAND - Avoid SetPostion Start Offset
        /// </summary>
        /// <param name="avso"></param>
       //[4]
        public void Cmd_Avoid_SO(string avso)
        {
            pAvoidSetPositionStartOffset = avso;
            log.Debug("Cmd_Avoid_SO()");
            //Set_Command();
        }
        /// <summary>
        /// Issue RGG COMMAND - Avoid Width
        /// </summary>
        /// <param name="avw"></param>
        //[5]
        public void Cmd_Avoid_W(string avw)
        {
            pAvoidWidth = avw;
            //log.Debug("Cmd_Avoid_W()");
            //Set_Command();
        }

        /// <summary>
        /// ssue RGG COMMAND - LUT Utc
        /// </summary>
        /// <param name="lut_utc"></param>
        /// 
        //[6]
        public void Cmd_Lut_Utc(string lut_utc)
        {
            pLUT_UTC = lut_utc;
            //log.Debug("Cmd_Lut_Utc()");
            //Set_Command();
        }

        /// <summary>
        /// Issue RGG COMMAND - LUT Delay
        /// </summary>
        /// <param name="lut_delay"></param>
        //[7]
        public void Cmd_Lut_Delay(string lut_delay)
        {
            pLUT_Delay = lut_delay;
            log.Debug("Cmd_Lut_Delay()");
            //Set_Command();
        }

        public void Set_Lut(string lut_utc, string lut_delay)
        {
            pRGG_Control = "0002"; // 2: LUT 송신
            pLUT_UTC = lut_utc;
            pLUT_Delay = lut_delay;
            log.Debug($"Set_Lut()  UTC {lut_utc} DELAY {lut_delay}");
            Set_Command();
        }

        public bool Set_LUT_Clear()
        {
            pRGG_Control = "0003"; // 3: LUT 초기화
            //pLUT_UTC = "0";
            //pLUT_Delay = "0";
            Set_Command();
            return true;
        }
        public string rggTimeBias = "0000";

        public void Set_Command()
        {
            if (connected)
            {
                pLEN = "5A";
                //pLEN = "AA";
                pMSG_ID = ConvertEndian(ControlCommand.MSG_ID_RGG_OP_CMD);
                var pData = ConvertEndian(pRGG_Control) + ConvertEndian(pGatePulseWidth) + ConvertEndian(pGatePulseStartOffset) +
                    ConvertEndian(pAvoidSetPositionStartOffset) + ConvertEndian(pAvoidWidth) + ConvertEndian(pLUT_UTC) + ConvertEndian(pLUT_Delay);
                pCHECKSUM = Checksum(pLEN + pMSG_ID + pData);

                pPacket = ControlCommand.MSG_SOF + pLEN + pMSG_ID + pData + pCHECKSUM + ControlCommand.MSG_EOT;

                ////////////////////////////////////////////
                seq_num_tx = rggSerial.SendData(pPacket);
                ////////////////////////////////////////////
                //log.Debug($"[RGG] SetCommand : {ControlCommand.MSG_SOF}-{pLEN}-{pMSG_ID}-{pData}-{pCHECKSUM}-{ControlCommand.MSG_EOT}");
                Console.WriteLine($"[RGG] SetCommand : {pLEN}-{pMSG_ID}-{ConvertEndian(pRGG_Control)}-{ConvertEndian(pGatePulseWidth)}-{ConvertEndian(pGatePulseStartOffset)}" +
    $"-{ConvertEndian(rggTimeBias)}-{ConvertEndian(pAvoidWidth)}-{ConvertEndian(pLUT_UTC)}-{ConvertEndian(pLUT_Delay)}-{pCHECKSUM}");

            }
            else
            {
                //MessageBox.Show("[RGG_SAT] Set_Command() RGG Disconnction 입니다. Serial Port를 확인이 필요합니다.");
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
                //s("[RGG_SAT] Cmd_Bit() RGG Disconnction 입니다. Serial Port를 확인이 필요합니다.");
            }
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
            state_now = "End";
            log.Info(state_now);
        }

        public string Anomaly_Check()
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


        public Int16 Get_GatePulseStartOffset()
        {
            return n_RggGatePulseStartOffset;
        }
        public string Get_rggGatePulseWidth()
        {
            return RggGatePulseWidth;
        }

        public bool IsRangeMode()
        {
            if (pRGG_Control.Equals(RGG_MODE.Ranging))
            {
                return true;
            }
            else return false;
        }
        public bool IsGCalMode()
        {
            if (pRGG_Control.Equals(RGG_MODE.GroundCAL))
            {
                return true;
            }
            else return false;
        }


        public bool SetRangeMode()
        {
            Cmd_Mode(RGG_MODE.Ranging);

            return true;
        }
        public bool SetStandbyMode()
        {
            Cmd_Mode(RGG_MODE.Standby);

            return true;
        }

        public bool SetGrndCalMode()
        {
            Cmd_Mode(RGG_MODE.GroundCAL);
            return true;
        }


        private ObservationData observationData;
        public static List<long> interpolated_UtcMs;
        public static List<double> interpolated_ToF;
        public static List<long> interpolated_UtcMs_G;
        public static List<double> interpolated_ToF_G;
        private double intervalSeconds = 0.1;

        public async Task SetObservationData(string type, ObservationData data)
        {
            observationData = data;

            ////////////// 고도에 따른 Tof Interval 설정 ////////////////////////
            /* 라그랑주 보간법 적용으로 인해 삭제
            if (observationData.LLA_data[2][0] < 2000.0)    // 저궤도 (10 ms)
            {
                intervalSeconds = 0.01;
            }
            else if (observationData.LLA_data[2][0] < 22000.0)  // 중궤도 (20 ms)
            {
                intervalSeconds = 0.02;
            }
            else    // 고궤도 (100 ms)
            {
                intervalSeconds = 0.1;
            }
            */
            /////////////////////////////////////////////////////////////////////

            if (observationData == null || observationData.LLA_data == null || observationData.LLA_data.Length < 2)
            {
                Console.WriteLine("Observation data or RaDec data is missing.");
            }

            log.Fatal("================================================== 1. SetObservationData ==");
            log.Error("========<<<<<    RGG SetObservationData >>>>>>              ===============");
            log.Fatal("===========================================================================");
            log.Error($" {observationData.SatelliteName}");
            log.Error($" ({observationData.StartTime}) ~ ({observationData.EndTime})");
            log.Error($" UTCms:{observationData.UtcMilliseconds}  Interval_second:{observationData.Interval_second}");
            log.Fatal("===========================================================================");

            // 자체 산출 Tof 적용 //////////////////////////////
            //InterpolateData();
            ////////////////////////////////////////////////////

            // NSLR-OAS Tof 적용 ///////////////////////////////
            if (type == "RGG_TLE")
            {
                interpolated_ToF = new List<double>();
                interpolated_ToF_G = new List<double>();
                interpolated_UtcMs = new List<long>();
                interpolated_UtcMs_G = new List<long>();

                interpolated_ToF_G = MainForm.mainForm.csu_observeSchedule2.Compute_Light_Travel_time(observationData.SatelliteName,
   observationData.StartTime, observationData.EndTime.AddSeconds(observationData.Interval_second), (int)(0.02 * 1000));
                interpolated_ToF = MainForm.mainForm.csu_observeSchedule2.Compute_Light_Travel_time(observationData.SatelliteName,
                    observationData.StartTime, observationData.EndTime.AddSeconds(observationData.Interval_second), (int)(intervalSeconds * 1000));
                for (int i = 0; i < interpolated_ToF_G.Count; i++)
                {
                    DateTime interpolated_dateTime = observationData.StartTime.AddSeconds(0.02 * i);
                    long utcMs_G = (long)(interpolated_dateTime.TimeOfDay.TotalMilliseconds);
                    interpolated_UtcMs_G.Add(utcMs_G);
                }
                for (int i = 0; i < interpolated_ToF.Count; i++)
                {
                    DateTime interpolated_dateTime = observationData.StartTime.AddSeconds(intervalSeconds * i);
                    long utcMs = (long)(interpolated_dateTime.TimeOfDay.TotalMilliseconds);
                    interpolated_UtcMs.Add(utcMs);
                }
            }
            else if (type == "RGG_CPF")
            {
                interpolated_ToF = new List<double>();
                interpolated_UtcMs = new List<long>();

                interpolated_ToF = MainForm.mainForm.csu_observeSchedule2.Compute_Light_Travel_time_CPF(observationData.SatelliteName,
                    observationData.StartTime, observationData.EndTime.AddSeconds(observationData.Interval_second), (int)(intervalSeconds * 1000));
                
                for (int i = 0; i < interpolated_ToF.Count; i++)
                {
                    DateTime interpolated_dateTime = observationData.StartTime.AddSeconds(intervalSeconds * i);
                    long utcMs = (long)(interpolated_dateTime.TimeOfDay.TotalMilliseconds);
                    interpolated_UtcMs.Add(utcMs);
                }
            }

            ////////////////////////////////////////////////////
            

            if (interpolated_UtcMs == null || interpolated_ToF == null)
            {
                Console.WriteLine("Interpolation failed: Data lists are not initialized.");
            }

            /// Debug용 파일저장 (Tof/Ms) //////////////////////
            string debug_directoryPath = "./Debug_Tof";
            DirectoryInfo debug_directoryInfo = new DirectoryInfo(debug_directoryPath);
            if (debug_directoryInfo.Exists == false) { debug_directoryInfo.Create(); }

            string debug_fileName = observationData.StartTime.ToLocalTime().ToString("yyyyMMddHHmmss") + "_" + observationData.SatelliteName;
            FileInfo debug_fileInfo = new FileInfo(debug_directoryPath + "/" + debug_fileName + ".txt");
            using (StreamWriter writer = debug_fileInfo.CreateText())
            {
                for (int i = 0; i < interpolated_ToF.Count; i++)
                {
                    writer.WriteLine(interpolated_UtcMs[i].ToString() + " / " + interpolated_ToF[i].ToString());
                }
                
            }
            ////////////////////////////////////////////////////
            await Start2SendLUT();
        }


        private void InterpolateData()
        {
            interpolated_UtcMs = new List<long>();
            interpolated_ToF = new List<double>();


            if (observationData == null || observationData.LLA_data == null || observationData.LLA_data.Length < 2)
            {
                Console.WriteLine("Observation data or RaDec data is missing.");
                return;
            }

            for (int i = 0; i < observationData.LLA_data[0].Length; i++)
            {
                // IntervalSeconds = 0.1 >> Tof 100ms 주기로 전송
                if ((((observationData.Interval_second * 100)/*interval:50ms*/ * i) % (intervalSeconds * 100)/*100ms*/) == 0)
                {
                    DateTime currentTime = observationData.StartTime.AddSeconds(observationData.Interval_second * i);

                    double LONG = observationData.LLA_data[0][i];
                    double LAT = observationData.LLA_data[1][i];
                    double ALTITUDE = observationData.LLA_data[2][i];

                    long utcMs = (long)(currentTime.TimeOfDay.TotalMilliseconds);
                    interpolated_UtcMs.Add(utcMs);

                    double Tof = RGG_LUT.CalculateToF2(LONG, LAT, ALTITUDE);
                    interpolated_ToF.Add(Tof);
                }

            }
            
        }

        Thread RggSAT_Tof_Thread;  // RGG LookUp Table 전송용 Thread
        int sending_count = 5;  // Default : 5
        private int currentIndex = 0;
        private uint packet_count = 0;
        Stopwatch sw = new Stopwatch();

        public async Task Start2SendLUT()
        {
            sending_count = 5/*7*/;  // 1 Packet에 5쌍씩 Setting >> 1 Packet Tof 개수 변경 시 해당부분만 수정
            currentIndex = 0;
            packet_count = 0;

            sw.Reset();
            sw.Start();

            await Task.Run(() =>
            {
                while (currentIndex < interpolated_ToF.Count)
                {
                    byte[] packet = CreateRGGPacket();
                    SendPacketToRGG(packet);
                    currentIndex += sending_count;

                    Thread.Sleep(10); // 10ms
                }

                currentIndex = 0;
                packet_count = 0;

                Console.WriteLine($"[RGG_SAT_Controller] LUT for [{observationData.SatelliteName} {observationData.StartTime} ~ {observationData.EndTime}] Transmission completed");

                sw.Stop();
                Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  Time : {sw.ElapsedMilliseconds}ms  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            });
        }

        int sat_num = 1;
        public async Task Start2SendLUT_Schedule()
        {
            sending_count = 5;
            currentIndex = 0;
            packet_count = 0;
            sat_num = 1;

            sw.Reset();
            sw.Start();

            await Task.Run(() =>
            {
                foreach (var observingSchedule_information in CSU_ObserveSchedule2.observingSchedule_informations)
                {
                    while (currentIndex < observingSchedule_information.observingSchedule_Tof.Count)
                    {
                        byte[] packet = CreateRGGPacket_Schedule(observingSchedule_information.observingSchedule_utcMs, observingSchedule_information.observingSchedule_Tof);
                        SendPacketToRGG_Schedule(packet);
                        currentIndex += sending_count;
                        Thread.Sleep(10); // 10ms
                    }

                    sending_count = 5;
                    currentIndex = 0;
                    packet_count = 0;
                    sat_num++;
                }

                

                sw.Stop();
                Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  Time : {sw.ElapsedMilliseconds}ms  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
            });
        }


        private byte[] CreateRGGPacket()
        {
            if (currentIndex + sending_count > interpolated_ToF.Count)
            {
                sending_count = interpolated_ToF.Count - currentIndex;
            }
            byte[] packet = new byte[sending_count * 16 + 15];

            byte length = (byte)(sending_count * 16 + 10);
            byte[] msgID = BitConverter.GetBytes(0x01030001);
            Buffer.BlockCopy(BitConverter.GetBytes(length), 0, packet, 0, 1);
            Buffer.BlockCopy(msgID, 0, packet, 1, 4);

            byte[] rggControl = BitConverter.GetBytes(0x0002);
            ushort gatePulseW = 0x0000;//[GatePulseW]
            ushort gatePulseS = 0x0000;//[GatePulseS]
            ushort aPosition = 0x0000;//[A.Position]
            ushort aWidth = 0x0000;//[A.Width]
            Buffer.BlockCopy(rggControl, 0, packet, 5, 2); // RGG_Control
            Buffer.BlockCopy(BitConverter.GetBytes(gatePulseW), 0, packet, 7, 2);   // GatePulseW
            Buffer.BlockCopy(BitConverter.GetBytes(gatePulseS), 0, packet, 9, 2);   // GatePulseS
            Buffer.BlockCopy(BitConverter.GetBytes(aPosition), 0, packet, 11, 2);     // A.Position
            Buffer.BlockCopy(BitConverter.GetBytes(aWidth), 0, packet, 13, 2);       // A.Width

            // n쌍(sending_count)의 데이터 추가
            for (int i = 0; i < sending_count; i++)
            {
                int offset = 15 + (i * 16); // 각 페어의 시작 위치 계산

                double utcMs = interpolated_UtcMs[currentIndex + i];
                byte[] utcMS_bytes = BitConverter.GetBytes((ulong)utcMs);

                double tof = interpolated_ToF[currentIndex + i];
                tof = tof * 1000000000;
                UInt64 tof_temp = (UInt64)(tof / 5);
                byte[] tof_bytes = BitConverter.GetBytes(tof_temp);

                Buffer.BlockCopy(utcMS_bytes, 0, packet, offset, 8);
                Buffer.BlockCopy(tof_bytes, 0, packet, offset + 8, 8);                
            }
            Console.WriteLine($"CreateRGGPacket()...{BitConverter.ToString(packet)}");
            return packet;
        }

        public byte[] CreateRGGPacket_Schedule(List<long> utcMs_data, List<double> tof_data)
        {
            if (currentIndex + sending_count > tof_data.Count)
            {
                sending_count = tof_data.Count - currentIndex;
            }
            byte[] packet = new byte[sending_count * 16 + 15];

            byte length = (byte)(sending_count * 16 + 10);
            byte[] msgID = BitConverter.GetBytes(0x01030001);
            Buffer.BlockCopy(BitConverter.GetBytes(length), 0, packet, 0, 1);
            Buffer.BlockCopy(msgID, 0, packet, 1, 4);

            byte[] rggControl = BitConverter.GetBytes(0x0002);
            ushort gatePulseW = 0x0000;//[GatePulseW]
            ushort gatePulseS = 0x0000;//[GatePulseS]
            ushort aPosition = 0x0000;//[A.Position]
            ushort aWidth = 0x0000;//[A.Width]
            Buffer.BlockCopy(rggControl, 0, packet, 5, 2); // RGG_Control
            Buffer.BlockCopy(BitConverter.GetBytes(gatePulseW), 0, packet, 7, 2);   // GatePulseW
            Buffer.BlockCopy(BitConverter.GetBytes(gatePulseS), 0, packet, 9, 2);   // GatePulseS
            Buffer.BlockCopy(BitConverter.GetBytes(aPosition), 0, packet, 11, 2);     // A.Position
            Buffer.BlockCopy(BitConverter.GetBytes(aWidth), 0, packet, 13, 2);       // A.Width

            // n쌍(sending_count)의 데이터 추가
            for (int i = 0; i < sending_count; i++)
            {
                int offset = 15 + (i * 16); // 각 페어의 시작 위치 계산

                double utcMs = utcMs_data[currentIndex + i];
                byte[] utcMS_bytes = BitConverter.GetBytes((ulong)utcMs);

                double tof = tof_data[currentIndex + i];
                tof = tof * 1000000000;
                UInt64 tof_temp = (UInt64)(tof / 5);
                byte[] tof_bytes = BitConverter.GetBytes(tof_temp);

                Buffer.BlockCopy(utcMS_bytes, 0, packet, offset, 8);
                Buffer.BlockCopy(tof_bytes, 0, packet, offset + 8, 8);
            }
            Console.WriteLine($"CreateRGGPacket()...{BitConverter.ToString(packet)}");
            return packet;
        }


        private void SendPacketToRGG(byte[] packet)
        {
            try
            {
                byte[] finalPacket = new byte[packet.Length + 3]; // STX + 데이터 + 체크섬 + ETX

                finalPacket[0] = 0x7E; // STX
                Buffer.BlockCopy(packet, 0, finalPacket, 1, packet.Length); // 데이터

                byte checksum = CalculateChecksum(packet);  // 체크섬 계산
                finalPacket[finalPacket.Length - 2] = checksum; // 체크섬
                finalPacket[finalPacket.Length - 1] = 0xFE; // ETX

                seq_num_tx = rggSerial.SendData(finalPacket);
                Console.WriteLine($"패킷 전송 완료 :#{++packet_count} {finalPacket}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"SendPacketToRGG 오류: {ex.Message}");
            }
            
        }

        private void SendPacketToRGG_Schedule(byte[] packet)
        {
            try
            {
                byte[] finalPacket = new byte[packet.Length + 3]; // STX + 데이터 + 체크섬 + ETX

                finalPacket[0] = 0x7E; // STX
                Buffer.BlockCopy(packet, 0, finalPacket, 1, packet.Length); // 데이터

                byte checksum = CalculateChecksum(packet);  // 체크섬 계산
                finalPacket[finalPacket.Length - 2] = checksum; // 체크섬
                finalPacket[finalPacket.Length - 1] = 0xFE; // ETX

                seq_num_tx = rggSerial.SendData(finalPacket);
                Console.WriteLine($"패킷 전송 완료({sat_num}/{CSU_ObserveSchedule2.observingSchedule_informations.Count}) :#{++packet_count} {finalPacket}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"SendPacketToRGG 오류({sat_num}/{CSU_ObserveSchedule2.observingSchedule_informations.Count}) : {ex.Message}");
            }

        }

        private byte CalculateChecksum(byte[] data)
        {
            byte checksum = 0;

            for (int i = 0; i < data.Length; i++)
            {
                checksum ^= data[i]; // XOR 방식으로 체크섬 계산
            }

            return checksum;
        }

    }

}
