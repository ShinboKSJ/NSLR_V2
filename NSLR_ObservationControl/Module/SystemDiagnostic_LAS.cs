using log4net;
using log4net.Config;
using log4net.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSLR_ObservationControl.Network;
using NSLR_ObservationControl.Subsystem;
using SGPdotNET.CoordinateSystem;
using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using Timer = System.Windows.Forms.Timer;

namespace NSLR_ObservationControl.Module
{
    public partial class SystemDiagnostic_LAS : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SystemDiagnostic_LAS));

        // ////////////////////////
        // 레이저부 ICD
        // ////////////////////////
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

        private TcpClient interPC_client;
        private NetworkStream interPC_clientStream;

        private LAS_SAT_Controller lasSATcontrol;
        private Timer controlCmd_timer;

        Label[] CbitResult_Power, CbitResult_OpState;


        /// <summary>
        /// ///////////////////////////////
        /// Constructor 
        /// ///////////////////////////////
        /// </summary>
        public SystemDiagnostic_LAS()
        {
            InitializeComponent();

            lasSATcontrol = LAS_SAT_Controller.instance;
            lasSATcontrol.onConnected += LAS_Connection_check;

            CbitResult_Power = new Label[] { led_P_B0, led_P_B1, led_P_B2, led_P_B3, led_P_B4,led_P_B5 }; //추가 
            CbitResult_OpState = new Label[] { led_O_B0, led_O_B1, led_O_B2, led_O_B3, led_O_B4, led_O_B5, led_O_B6, led_O_B7 };

            controlCmd_timer = new Timer();
            controlCmd_timer.Tick += update_timer_Tick;
            controlCmd_timer.Interval = Convert.ToInt32(200); //200ms (5Hz)
            controlCmd_timer.Start();
        }

      
        private void LAS_Connection_check(object sender, ConnectedEventArgs e)
        {
            //Console.WriteLine($"LAS_Connection_check()..{e.IsConnected}");

            if (e.IsConnected.Equals(IPC_MSG.CONNECTION_LAS_SAT))
            {
                sat_btn_connection.BackColor = Color.DarkOrange;
                deb_btn_connection.BackColor = Color.DarkGray;
                connected = true;
                log.Info(IPC_MSG.CONNECTION_LAS_SAT);
            }
            else if(e.IsConnected.Equals(IPC_MSG.DISCONNECTION_LAS_SAT))
            {
                sat_btn_connection.BackColor = Color.DarkGray;
                deb_btn_connection.BackColor = Color.DarkGray;
                connected = false;
                log.Info(IPC_MSG.DISCONNECTION_LAS_SAT);
            }
            else if (e.IsConnected.Equals(IPC_MSG.CONNECTION_LAS_DEB))
            {
                sat_btn_connection.BackColor = Color.DarkGray;
                deb_btn_connection.BackColor = Color.DarkOrange;
                connected = true;
                log.Info(IPC_MSG.CONNECTION_LAS_DEB);
            }
            else if (e.IsConnected.Equals(IPC_MSG.DISCONNECTION_LAS_DEB))
            {
                sat_btn_connection.BackColor = Color.DarkGray;
                deb_btn_connection.BackColor = Color.DarkGray;
                connected = false;
                log.Info(IPC_MSG.DISCONNECTION_LAS_DEB);
            }
        }


        /// <summary>
        /// ///////////////////////////////
        /// Command do_PBIT
        /// ///////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetBIT(string bit)
        {
            if (connected)
            {
                lasSATcontrol.SetBIT(bit);
                log.Info($"Send {bit} Command");
            }
        }
        private void btn_PBIT_Click(object sender, EventArgs e)
        {
            SetBIT("PBIT");
        }
        private void btn_IBIT_Click(object sender, EventArgs e)
        {
            SetBIT("IBIT");
        }


        /// <summary>
        /// ///////////////////////////////
        /// Command Laser Operation State
        /// ///////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_laserOpState_Click(object sender, EventArgs e)
        {
            laserOpState ^= 1;
            TX_LaserOpState = string.Format("{0:x2}", laserOpState);
            if (laserOpState == 0)
            {
                btn_laserOpState.Text = "레이저 운용초기"; //24.11.13 LAS 무동작
                btn_laserOpState.BackColor = Color.Blue;
                btn_laserOpState.ForeColor = Color.Orange;
            }
            else if (laserOpState == 1)
            {
                btn_laserOpState.Text = "레이저 운용종료"; //24.11.13  LAS STOP, 레이저 및 LD, 전원공급기 Off
                btn_laserOpState.BackColor = Color.LightGray;
                btn_laserOpState.ForeColor = Color.Black;
            }
            UpdatePacketData();
        }

        /// <summary>
        /// ///////////////////////////////
        /// Command Laser Operation Mode 
        /// ///////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_LaserOpMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            TX_LaserOpMode = string.Format("{0:x2}", cb_LaserOpMode.SelectedIndex);
            UpdatePacketData(); 
        }

        private void cb_LaserMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            TX_LaserMode = string.Format("{0:x2}", cb_LaserMode.SelectedIndex);
            UpdatePacketData();
        }


        /// <summary>
        /// ///////////////////////////////
        /// Periodic Sending Packet
        /// ///////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void update_timer_Tick(object sender, EventArgs e)
        {
            if (connected)
            {
                //if (text_AA.InvokeRequired)
                //text_AA.Invoke(new Action(() => text_AA.Text = inputByte.ToString()));

                label_OpMode.Text = lasSATcontrol.D_OpMode;
                label_Mode.Text = lasSATcontrol.D_Mode;
                label_FireState.Text = lasSATcontrol.D_FireState;
                label_OpState.Text = lasSATcontrol.D_OpState;

                var data1 = Convert.ToString(lasSATcontrol.D_BitResult1, 2).PadLeft(8, '0');
                var data2 = Convert.ToString(lasSATcontrol.D_BitResult2, 2).PadLeft(8, '0');
                data1 = ReverseBinaryString(data1);
                data2 = ReverseBinaryString(data2);

                for (int i = 0; i < 6; i++)
                {
                    if (data1[i] == '1') { CbitResult_Power[i].ForeColor = Color.Red; }  // 1 : 고장
                    else { CbitResult_Power[i].ForeColor = Color.Green; }
                }
                for (int i = 0; i < 6; i++)
                {
                    if (data2[i] == '1') { CbitResult_OpState[i].ForeColor = Color.Red; } // 1 : 고장
                    else { CbitResult_OpState[i].ForeColor = Color.Green; }
                }

            }
        }
        public static string ReverseBinaryString(string binaryString)
        {
            char[] charArray = binaryString.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private void rb_ShutterOpen_CheckedChanged(object sender, EventArgs e)
        {
            TX_LaserStartStop = "01";
            
            rb_ShutterOpen.BackColor = Color.Blue;
            rb_ShutterOpen.ForeColor = Color.Orange;

            rb_ShutterClose.BackColor = Color.LightSlateGray;
            UpdatePacketData();
        }

        private void rb_ShutterClose_CheckedChanged(object sender, EventArgs e)
        {
            TX_LaserStartStop = "00";

            rb_ShutterClose.BackColor = Color.Blue;
            rb_ShutterClose.ForeColor = Color.Orange;

            rb_ShutterOpen.BackColor = Color.LightSlateGray;
            UpdatePacketData();
        }

        /*
        private void label_OpMode_Click(object sender, EventArgs e)
        {
            ////////////////////////////////////////////////////////////////////////////
            //if (D_OpMode.Equals("05"))
            {
                if (TaskNow.CurrentSystemObject == TaskNow.SystemObject.SLR)
                    MessageBox.Show($"[LAS_SAT]: >> D_OpMode {lasSATcontrol.D_OpMode}");
                else if (TaskNow.CurrentSystemObject == TaskNow.SystemObject.DLT)
                    MessageBox.Show($"[LAS_DEB]: >> D_OpMode {lasSATcontrol.D_OpMode}");
            }
            ////////////////////////////////////////////////////////////////////////////
        }
        */
 

        /// <summary>
        /// ///////////////////////////////
        /// Initiating Command 
        /// ///////////////////////////////
        /// </summary>
        private void UpdatePacketData()
        {
            TX_LaserOpState = "00"; //일반적으로 운용시에 운용종료를 할 일은 없다.

            TX_DATA = TX_LaserOpMode + TX_LaserMode + TX_LaserStartStop + TX_LaserOpState;
            log.Info($"[SetCommand DATA] :({TX_DATA}) >>  LasOpMode[{TX_LaserOpMode}]  LasMode[{TX_LaserMode}]  LasStartStop[{TX_LaserStartStop}]  LasOpState[{TX_LaserOpState}]");            

            if (TX_DATA is "")
            {
                MessageBox.Show("Data 선택 부터 하세요.");
                return;
            }
            lasSATcontrol.SetCommand(TX_DATA);
        }
    }

}
