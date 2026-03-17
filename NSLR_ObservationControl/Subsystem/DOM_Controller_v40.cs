using log4net;
using mv.impact.acquire;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Packaging;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Timers;
using Timer = System.Timers.Timer;
using Newtonsoft.Json.Linq;
using NSLR_ObservationControl.Module;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace NSLR_ObservationControl.Subsystem
{
    //Jason Dome v40 :  OCU-Client / DOME-Server 
    public class DOM_Controller_v40 : IDOM
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static DOM_Controller_v40 _instance;
        public string Tag => "ALL";
        public event EventHandler<ConnectedEventArgs> DomeConnected;
        public event EventHandler<DomeStateEventArgs> StateChanged;

        private TcpClient client;
        private NetworkStream stream;
        private const int ReconnectionIntervalMs = 1000; // 1초마다 재연결 시도

        private TcpClient interPC_client;
        private NetworkStream interPC_clientStream;
        private bool connected { get; set; } = false;
        private bool connected_prev { get; set; } = false;

        private CancellationTokenSource reconnectionCts;

        private bool msging = false;
        private uint msging_cnt = 0;

        public int reconnection_try_cnt = 0;
        public bool IsInitialized { get; private set; }
        public string strShutter { get; set; } = "XX";
        public string strRain { get; set; } = "XX";
        public string strHome { get; set; } = "XX";
        public string strDrive { get; set; } = "XX";
        public string strPosition { get; set; } = "0.0";
        public string strBit { get; set; } = "XX";
        public byte strAA { get; private set; }

        readonly string[] str_SHUTTER_STATE = { "XX", "OPEN", "OPENing", "CLOSE", "CLOSing", "IDLE", "XX" };
        readonly string[] str_RAIN_STATE = { "NORAIN", "RAIN", "XX" };
        readonly string[] str_HOME_STATE = { "HOME", "NOHOME", "HOMMING", "XX" };  //GC0312
        readonly string[] str_DRIVE_STATE = { "XX", "POSITION", "TRACKING", "STOP", "PARKED", "XX" };
        readonly string[] str_BIT = { "CBIT", "PBIT", "IBIT", "XX" };


        const string CMD_HOME = "DOME HOME";
        const string CMD_OPEN = "DOME OPEN";
        const string CMD_CLOSE = "DOME CLOSE";
        const string CMD_STOP = "DOME STOP";
        const string CMD_PARK = "DOME PARK";
        const string CMD_PBIT = "BIT PBIT";
        const string CMD_IBIT = "BIT IBIT";

        enum SHUTTER_STATE { OPEN = 1, OPENing = 2, CLOSE = 3, CLOSing = 4, IDLE = 5, OPEN_CMD = 11, CLOSE_CMD = 33 }
        enum RAIN_STATE { NORAIN = 0, RAIN = 1, }
        enum HOME_STATE { NOHOME = 0, HOME = 1, HOMMING = 2, HOMMING1 = 3, }
        enum DRIVE_STATE { POSITION = 1, TRACKING = 2, STOP = 3, PARKED = 4, POS_CMD = 11, TRA_CMD = 22, STOP_CMD = 33, PARK_CMD = 44 }

        SHUTTER_STATE shutter_state_now = SHUTTER_STATE.CLOSE;
        RAIN_STATE rain_state_now = RAIN_STATE.NORAIN;
        HOME_STATE home_state_now = HOME_STATE.NOHOME;
        DRIVE_STATE drive_state_now = DRIVE_STATE.STOP;


        string state_now;
        string state_track_now;

        AID_Controller AID;

        private Timer trackingTimer;



        public DOM_Controller_v40()
        {
            AID = AID_Controller.instance;

            trackingTimer = new Timer(100); // 10Hz = 100ms
            trackingTimer.Elapsed += OnTrackingTimerElapsed;
            trackingTimer.Start();
        }

        public static DOM_Controller_v40 instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DOM_Controller_v40();
                }
                return _instance;
            }
        }

        public void Initialize()
        {
            try
            {
                //Create_InterPCclient();
                ConnectDom();
                IsInitialized = true;
            }
            catch (Exception ex)
            {
                log.Debug($"[Initialize] 연결 실패: {ex.Message}");
            }
        }

        private void ChangeConnected(bool bCon)
        {
            if (bCon)
            {
                connected = true;
                log.Info(IPC_MSG.CONNECTION_DOM);
                DomeConnected?.Invoke(this, new ConnectedEventArgs(IPC_MSG.CONNECTION_DOM));
                connected_prev = connected;
            }
            else
            {
                connected = false;
                log.Info(IPC_MSG.DISCONNECTION_DOM);
                DomeConnected?.Invoke(this, new ConnectedEventArgs(IPC_MSG.DISCONNECTION_DOM));
                connected_prev = connected;
            }
        }

        public bool IsConnected()
        {
            return connected;
        }

        public void ConnectDom()
        {
            Thread connectThread = new Thread(new ThreadStart(ConnectInBackground));
            connectThread.IsBackground = true;
            connectThread.Start();
        }

        private void ConnectInBackground()
        {
            while (true)
            {
                try
                {
                    if (client == null || !client.Connected)
                    {
                        client = new TcpClient();
                        client.Connect(NSLR_IP.DOM, SubSystem_PORT.DOM); // 서버의 IP와 포트
                        stream = client.GetStream();
                        ChangeConnected(true);
                        log.Debug("@ConnectInBackground() 서버에 접속되었습니다.");
                        break; // 성공적으로 연결되면 루프 종료
                    }
                }
                catch (Exception ex)
                {
                    log.Debug($"@ConnectInBackground() 서버접속 실패: {ex.Message}");
                    ChangeConnected(false);
                    Thread.Sleep(500); // 재시도 전 대기
                }
            }
            Thread dom_receiveThread = new Thread(new ThreadStart(ReceiveStatusUpdate));
            dom_receiveThread.Start();
        }


        private void ReceiveStatusUpdate()
        {
            byte[] buffer = new byte[1024];
            while (connected)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string status = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        status = status.Trim();
                        //status = status.Replace("\0", "");
                        //log.Info($"[RX ReceiveStatusUpdate() [{status}]");
                        OnStateChanged(ParseDomeState(status));
                    }
                    else
                    {
                        //log.Debug($"[DOME_v40] ReceiveStatusUpdatesAsync()  size 0 ==> HandleDisconnectionAsync]");
                        HandleDisconnection();
                    }
                }
                catch (Exception)
                {
                    HandleDisconnection();
                }
            }
        }


        Thread dom_receiveThread;
        private void ConnectWithRetries()
        {
            while (true)
            {
                try
                {
                    client = new TcpClient();
                    client.Connect(NSLR_IP.DOM, SubSystem_PORT.DOM);

                    stream = client.GetStream();
                    ChangeConnected(true);
                    log.Debug($"[Retry_Connect] 연결성공");
                    //byte[] data = { 0x01, 0x02, 0x03, 0x04 };
                    //await stream.WriteAsync(data, 0, data.Length);
                    dom_receiveThread = new Thread(new ThreadStart(ReceiveStatusUpdate));
                    dom_receiveThread.Start();
                    return; // 연결 성공
                }
                catch (Exception ex)
                {
                    ChangeConnected(false);
                    reconnection_try_cnt++;
                    Console.WriteLine($"[Retry_Connect] 연결 실패 #{reconnection_try_cnt}: {ex.Message}");
                    log.Fatal($"[Retry_Connect] 연결 실패: {ex.Message}");
                    //.Task.Delay(ReconnectionIntervalMs, cancellationToken);
                }
            }
        }


        private void HandleDisconnection()
        {
            client?.Close();
            stream?.Dispose();

            ChangeConnected(false);

            log.Warn("[DOME Server disconnection 재연결 시도!....돔시스템을 확인 바랍니다.");
            ConnectWithRetries();
        }

        public DOMUserData userData;
        private DomeState ParseDomeState(string status)
        {
            string[] data = status.Split('\x020');
            //if(msging)
            //Console.WriteLine($"[{data.Length}]ParseDomeState : {status}");
            ///// 각 데이터 변환 및 검증  //초기 상태 정보 "3 0 0 0 179.00 0 254"

            // [CONNECTION state]-----------------------
            if (status.Contains("FG CONNECTED"))
            { connected = true; log.Info($"[DOM RX] CONNECTED OK"); }
            // [HOME state]-----------------------------
            else if (status.Contains("HOME OK"))
            { home_state_now = HOME_STATE.HOME; log.Info($"[DOM RX] HOME OK"); }
            // [SHUTTER state ]------------------------
            else if (status.Contains("OPEN OK"))
            { shutter_state_now = SHUTTER_STATE.OPEN; log.Info($"[DOM RX] OPEN ACK"); }
            else if (status.Contains("CLOSE OK"))
            { shutter_state_now = SHUTTER_STATE.CLOSE; log.Info($"[DOM RX] CLOSE OK"); }
            // [DRIVE state]----------------------------
            else if (status.Contains("POS OK"))
            { drive_state_now = DRIVE_STATE.POSITION; log.Info($"[DOM RX] POS ACK"); }
            else if (status.Contains("TRA OK"))
            { drive_state_now = DRIVE_STATE.TRACKING; /*log.Info($"[DOM RX] TRACKING OK");*/ }
            else if (status.Contains("STOP OK"))
            { drive_state_now = DRIVE_STATE.STOP; log.Info($"[DOM RX] STOP OK"); }
            else if (status.Contains("DOME PARK OK"))
            { drive_state_now = DRIVE_STATE.PARKED; log.Info($"[DOM RX] PARK ACK"); }
            // [Status Info]---------------------------------------------------------------------------
            else if (
                data[0].Equals("FG") &&
                uint.TryParse(data[1], out uint value1) && value1 >= 0 && value1 <= 5 &&
                uint.TryParse(data[2], out uint value2) && value2 >= 0 && value2 <= 1 &&
                uint.TryParse(data[3], out uint value3) && value3 >= 0 && value3 <= 2 &&
                uint.TryParse(data[4], out uint value4) && value4 >= 0 && value4 <= 4 &&  //Dome이 돔구동상태 코드로 정의되지 않은 '0'을 계속 날리고 있다.
              double.TryParse(data[5], out double angle) && angle >= 0.0 && angle <= 360.0 &&
                uint.TryParse(data[6], out uint value6) && value6 >= 0 && value6 <= 2 &&
                uint.TryParse(data[7], out uint value7) && value7 >= 0 && value7 <= 254 &&
                //data[8].Equals("GF\0") //250219 돔으로 부터의 패턴
                data[8].Contains("GF")
            )
            {
                strShutter = str_SHUTTER_STATE[value1];
                strRain = str_RAIN_STATE[value2];
                strHome = str_HOME_STATE[value3];
                strDrive = str_DRIVE_STATE[value4];
                strPosition = angle.ToString("F2");

                /*
                if (Enum.TryParse(strShutter, out shutter_state_now))
                {
                    if (msging) log.Debug($"DOM RX...[SHUTTER] state : {shutter_state_now}");
                }
                else { if (msging) log.Debug($"DOM RX...[SHUTTER] state : 실패"); }
                
                if (Enum.TryParse(strRain, out rain_state_now))
                {
                    if (msging) log.Debug($"DOM RX... [RAIN] state : {rain_state_now}");
                }
                else { if (msging) log.Debug($"DOM RX... [RAIN] state  : 실패"); }

                if (Enum.TryParse(strHome, out home_state_now))
                {
                    if (msging) log.Debug($"DOM RX... [HOME] state : {home_state_now}");
                }
                else { if (msging) log.Debug($"DOM RX... [HOME] state  : 실패"); }

                if (Enum.TryParse(strDrive, out drive_state_now))
                {
                    if (msging) log.Debug($"DOM RX... [DRIVE] state : {drive_state_now}");
                }  else { if (msging) log.Debug($"DOM RX... [DRIVE] state  : 실패"); }
                */
                if (Enum.TryParse(strShutter, out shutter_state_now) && Enum.TryParse(strRain, out rain_state_now) && Enum.TryParse(strHome, out home_state_now) && Enum.TryParse(strDrive, out drive_state_now))
                {
                    if (msging) log.Info($"{shutter_state_now}  {rain_state_now}  {home_state_now}  {drive_state_now}  {strPosition}  BIT_{value6}  AA:{value7}");
                }

                if (value6 == 0) strBit = "CBIT";
                else if (value6 == 1) strBit = "PBIT";
                else if (value6 == 2) strBit = "IBIT";
                strAA = (byte)value7;

                /*
                Console.WriteLine("-----------------------------------------------------------------------------------------");
                Console.WriteLine($"Received data: {value1} {value2} {value3} {value4} {angle} {value6} {value7}");
                Console.WriteLine("-----------------------------------------------------------------------------------------");
                Console.WriteLine($"셔터상태 [{data[1]} {strShutter}] (1.OPEN 2.Opening 3.ClOSED 4.Cloging 5.IDLE)");
                Console.WriteLine($"강우상태 [{data[2]} {strRain}] (0.평상시 1.강우시)");
                Console.WriteLine($"돔홈상태 [{data[3]} {strHome}] (0.찾음 1.안찾음 2.HOMming)");
                Console.WriteLine($"돔구동상태 [{data[4]} {strDrive}] (1.Position 2.추적 3.정지 4.Parked)");
                Console.WriteLine($"돔현재위치 [{strPosition}]");
                Console.WriteLine($"BIT {strBit}  0(0.CBIT 1.PBIT 2.IBIT)");
                Console.WriteLine($"AA [{strAA} 0x{Convert.ToString(strAA, 16)}]\n\n");
                if(msging) log.Info($"Received data: {value1} {value2} {value3} {value4} {angle} {value6} {value7}");
                */
                if (msging_cnt++ > 5)
                {
                    msging_cnt = 0;
                    msging = true;
                }
                else { msging = false; }
                connected = true;
                
                userData = new DOMUserData
                {
                    ID = "DOME_RSP_DATA",
                    Shutter = strShutter,
                    Rain = strRain,
                    Home = strHome,
                    Driving = strDrive,
                    Position = strPosition,
                    Bit = strBit,
                    BitResult = Convert.ToString(strAA, 2).PadLeft(8, '0')
                };
                //log.Debug($"BitResult {userData.BitResult}");
            }
            else
            {
                log.Debug("Received invalid data: " + string.Join("--", data));
            }
            return DomeState.Initializing;
        }


        protected virtual void OnStateChanged(DomeState state)
        {
            StateChanged?.Invoke(this, new DomeStateEventArgs(state));
        }


        public void Disconnect()
        {
            reconnectionCts.Cancel();
            client?.Close();
            stream?.Dispose();
            ChangeConnected(false);
        }


        public void Start()
        {
            log.Info("Start");
        }

        public void End()
        {
            log.Info("End");
        }

        public void doPBIT()
        {
            log.Info("PBIT");
            ExecuteCommand(CMD_PBIT);
        }
        public void doIBIT()
        {
            ExecuteCommand(CMD_IBIT);
        }

        public string doHomeSearch()
        {
            //int value = (int)state_home;
            string value = strHome.ToString();

            //if (value.Equals("HOME"))
            if (true)
            {
                ExecuteCommand(CMD_HOME);
                log.Info($"[DomeV40_CMD] [{value}] to HOME");
                return "OK";
            }
            else
            {
                return "NOK";
            }
        }

        // [ Shutter State 
        public string  doOpen()
        {
            //int value = (int)state_shutter;
            string value = strShutter.ToString();

            if (value.Equals("CLOSE") || value.Equals("IDLE"))
            {
                ExecuteCommand(CMD_OPEN);
                log.Info($"[DomeV40_CMD] [{value}] to OPEN");
                return "OK";
            }
            else
            {
                return "NOK";
            }
        }

        public string doClose()
        {
            //int value = (int)state_shutter;
            string value = strShutter.ToString();

            if (value.Equals("OPEN") || value.Equals("IDLE"))
            {
                ExecuteCommand(CMD_CLOSE);
                log.Info($"[DomeV40_CMD] [{value}] to CLOSE"); 
                return "OK";
            }
            else
            {
                return "NOK";
            }
        }

/*        public string doShutter()
        {
            //int value = (int)state_shutter;
            string value = strShutter.ToString();

            if (value.Equals("OPEN") || value.Equals("IDLE"))
            {
                ExecuteCommand(CMD_CLOSE);
               // log.Info($"[DomeV40] [{value}] >> CLOSE");
                return "OK";
            }
            else if (value.Equals("CLOSE") || value.Equals("IDLE"))
            {
                ExecuteCommand(CMD_OPEN);
                log.Info($"[DomeV40_CMD] [{value}] >> OPEN");
                return "OK";
            }
            else
            {
                return "NOK";
            }
        }*/
        public ShutterResult doShutter()
        {
            string value = strShutter.ToString();

            if (value.Equals("OPEN"))
            {
                string result = ExecuteCommand(CMD_CLOSE);
                log.Info($"[DomeV40_CMD] [{value}] >> CLOSE");
                return new ShutterResult
                {
                    Success = result.Equals("OK"),
                    TargetState = "CLOSING",
                    CurrentState = value
                };
            }
            else if (value.Equals("CLOSE") || value.Equals("IDLE"))
            {
                string result = ExecuteCommand(CMD_OPEN);
                log.Info($"[DomeV40_CMD] [{value}] >> OPEN");
                return new ShutterResult
                {
                    Success = result.Equals("OK"),
                    TargetState = "OPENING",
                    CurrentState = value
                };
            }
            else if (value.Equals("OPENING") || value.Equals("CLOSING"))
            {
                log.Info($"[DomeV40_CMD] [{value}] >> Already in motion");
                return new ShutterResult
                {
                    Success = false,
                    TargetState = value,
                    CurrentState = value
                };
            }
            else
            {
                return new ShutterResult
                {
                    Success = false,
                    TargetState = "UNKNOWN",
                    CurrentState = value
                };
            }
        }
        public class ShutterResult
        {
            public bool Success { get; set; }
            public string TargetState { get; set; }
            public string CurrentState { get; set; }
        }
        //  Shutter State ]


        // [  DRIVING state ------------------------------------------
        public string doStop()
        {
            //int value = (int)state_drive;
            string value = strDrive.ToString();

            if (value.Equals("POSITION") || value.Equals("TRACKING"))
            {
                ExecuteCommand(CMD_STOP);
                log.Info($"[DomeV40_CMD] [{value}] to STOP");
                return "OK";
            }
            else
            {
                return "NOK";
            }
        }

        public string doPark()
        {
            //int value = (int)state_drive;
            string value = strDrive.ToString();

            if (value.Equals("STOP"))
            {
                ExecuteCommand(CMD_PARK);
                log.Info($"[DomeV40_CMD] [{value}] to PARK");
                return "OK";
            }
            else
            {
                return "NOK";
            }
        }
        public string doPositionMove(double Azimuth, double Elevation)
        {
            double NewPosData;
            AID.SetAzEl(Azimuth, Elevation);

            //int value = (int)state_drive;
            string value = strDrive.ToString();

            if (strDrive.Equals("STOP"))
            {
                // 값 보정
                if (Azimuth < 0) NewPosData = Azimuth + 360;
                else NewPosData = Azimuth;
                // 포맷팅 (소수점 4자리)
                string PosCommand = NewPosData.ToString("0.00");
                log.Info($"[DomeV40_CMD] doPositionMove[{Azimuth}] [{value}] to POS");
                ExecuteCommand($"POS {PosCommand}");
                return "OK";
            }
            else
            {
                return "NOK";
            }
        }

        /*
        int timing = 0;
        public string doTracking_A(double[] numbers)
        {
            if(timing++ > 5 )  { timing = 0;  }
            else { return "WAIT"; }


            //위치값 음수일때 변환
           for(int i=0; i<numbers.Length; i++)
            {
                if (numbers[i] < 0)
                    numbers[i] += 360.00;
            }

            string TRA_DATA = string.Join(" ", numbers.Select(n => n.ToString("F2")));

            if (numbers.Length != 8)
            {
                throw new ArgumentException("배열은 정확히 8개의 요소를 포함해야 합니다.");
                //return $"NOK:DataSize {numbers.Length}";
            }
            //int value = (int)state_drive;
            string value = strDrive.ToString();
            if (value.Equals("STOP"))
            {
                log.Info($"[DomeV40] doTracking[{TRA_DATA}] [{value}] to TRA");
                ExecuteCommand($"TRA {TRA_DATA}");
                return "OK";
            }
            else
            {
                return $"NOK:DriveState-{value}";
            }
        }
        */
        double[][] a;
        double[] b;
        bool flag = false;
        double[] AzData = new double[8];


        private double[] DomeTrackingPosition(double currentUTCMillis)
        {
            double[] position = new double[8];

            for (var i = 0; i < Observation_TMS.timestamps_dome.Length - 4; i++)
            {
                if (currentUTCMillis < Observation_TMS.timestamps_dome[i])
                {
                    for (int j = -3; j <= 4; j++)
                    {
                        int index = i + j;

                        if (index >= 0 && index < Observation_TMS.timestamps_dome.Length)
                        {
                            position[j + 3] = Observation_TMS.AzElData_Dome[0][index];
                        }
                    }

                    return position;
                }
            }
            return position;
        }


        private bool isFirstCommand = true;
        private Queue<string> azimuthBuffer = new Queue<string>();
        public void doTracking(double trackData)
        {
            double newTrackData = (trackData < 0) ? trackData + 360 : trackData;
            string formattedAzimuth = newTrackData.ToString("0.00");

            if (isFirstCommand)
            {
                azimuthBuffer.Enqueue(formattedAzimuth);
                if (azimuthBuffer.Count >= 8)
                {
                    SendTrackingCommand();
                    isFirstCommand = false;
                    trackingTimer.Start();
                }
            }
            else
            {
                if (azimuthBuffer.Count >= 8)
                {
                    azimuthBuffer.Dequeue();
                }
                azimuthBuffer.Enqueue(formattedAzimuth);
            }
        }

        bool StopFlag = false;
        private void OnTrackingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            //if (azimuthBuffer.Count == 8)
            //    SendTrackingCommand();
            if (!flag && Observation_TMS.controlCommand == TrackingMount.TrackingMode)
            {
                a = Observation_TMS.AzElData_Dome;
                b = Observation_TMS.timestamps_dome;
                flag = true;                
            }
            if (flag && Observation_TMS.controlCommand == TrackingMount.TrackingMode && TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.S_CAL) //
            {

            }
            else if (flag && Observation_TMS.controlCommand == TrackingMount.TrackingMode ) //
            {

                StopFlag = false;

                DateTime currentUTC = DateTime.UtcNow;
                DateTime currentUTCDate = currentUTC.Date;
                double currentUTCMillis = (currentUTC - currentUTCDate).TotalMilliseconds;
                AzData = DomeTrackingPosition(currentUTCMillis);

                for (int i = 0; i < AzData.Length; i++)
                {
                    if (AzData[i] < 0)
                        AzData[i] += 360.00;
                }

                string TRA_DATA = string.Join(" ", AzData.Select(n => n.ToString("F2")));

                if (AzData.Length != 8)
                {
                    throw new ArgumentException("배열은 정확히 8개의 요소를 포함해야 합니다.");
                    //return $"NOK:DataSize {numbers.Length}";
                }
                //int value = (int)state_drive;
                string value = strDrive.ToString();

               // log.Info($"[DomeV40] doTracking[{TRA_DATA}]");
                ExecuteCommand($"TRA {TRA_DATA}");
                //return "OK";

            }

            else if(Observation_TMS.controlCommand == TrackingMount.ServoDisable && !StopFlag)
            {
                doStop();
                StopFlag = true;
                flag = false;
            }

        }
        private void SendTrackingCommand()
        {
            //int value = (int)state_drive;
            string value = strDrive.ToString();

            if (value.Equals("STOP"))
            {
                string trackCommand = string.Join(" ", azimuthBuffer);
               // log.Info($"[DomeV40] doTracking[{trackCommand}] [{value}] to TRA");
                ExecuteCommand($"TRA {trackCommand}");
            }
        }
        /// ] Driving state
        

        private string ExecuteCommand(string cmd)
        {
            var COMMAND = cmd + "\n\0";
            //log.Info($"[DomeV40] ExecuteCommand[{cmd}]");
            try
            {
                if (client != null && client.Connected)
                {
                    NetworkStream stream = client.GetStream();

                    if (stream != null)
                    {
                        byte[] sendData = Encoding.ASCII.GetBytes(COMMAND);
                        //log.Info($"ExecuteCommand[{cmd}]:{string.Join(" ", sendData)}]");
                        stream.Write(sendData, 0, sendData.Length);
                    }
                    else
                    {
                        ConnectInBackground();
                        log.Fatal($"ExecuteCommand[{cmd}]..Error: Client is not connected or clientStream is null.");
                        return "NOK";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Fatal($"[Exception] ExecuteCommand[{cmd}] : " + ex.Message);
                return "NOK";
            }
            //////////////////////////////////////////////////////////////////////////
            if (cmd.Contains(CMD_HOME))
                home_state_now = HOME_STATE.HOME;
            else if (cmd.Contains(CMD_OPEN))
                shutter_state_now = SHUTTER_STATE.OPEN_CMD;
            else if (cmd.Contains(CMD_CLOSE))
                shutter_state_now = SHUTTER_STATE.CLOSE_CMD;
            else if (cmd.Contains(CMD_STOP))
                drive_state_now = DRIVE_STATE.STOP_CMD;
            //////////////////////////////////////////////////////////////////////////
            return "OK";
        }

        #region ============ BackGroundWorks =================
        private BackgroundWorker bWorker;
        private void Initialize_bgWorker()
        {
            bWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            bWorker.DoWork += bgWorker_DoWork;
            bWorker.ProgressChanged += bgWorker_ProgressChanged;
            bWorker.RunWorkerCompleted += bgWorker_RunWorkerCompleted;
        }
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ExecuteCommand(state_now);
            log.Info("doTracking().. [trackingBgWorker.RunWorkerAsync()]............");
        }

        System.Windows.Controls.ProgressBar progressBar = new System.Windows.Controls.ProgressBar();

        void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            progressBar.Value = e.ProgressPercentage; // 진행률 업데이트
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                //log.Debug("작업이 취소되었습니다.");
            }
            else
            {
                //log.Debug("작업이 완료되었습니다.");
            }

        }
        #endregion


    }


    public class DomeStateEventArgs : EventArgs
    {
        public DomeState State { get; }

        public DomeStateEventArgs(DomeState state)
        {
            State = state;
        }
    }

    public enum DomeState
    {
        Unknown,
        Initializing,
        Opening,
        FullyOpen
        // 추가 상태들...
    }
}
