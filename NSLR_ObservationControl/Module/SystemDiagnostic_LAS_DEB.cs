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
    public partial class SystemDiagnostic_LAS_DEB : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SystemDiagnostic_LAS_DEB));

        // ////////////////////////
        // 레이저부 ICD
        // ////////////////////////
        const string MSG_DATA_BYTE_CNT = "00000004";
        public static bool connected { get; set; } = false;
        public string TX_PACKET { get; set; } = "00";
        public StringBuilder TX_DATA  = new StringBuilder();
        public string TX_LaserOpMode { get; set; } = "00";
        public string TX_LaserMode { get; set; } = "00";
        public string TX_LaserStartStop { get; set; } = "00";
        public string TX_LaserOpEnd { get; set; } = "00";
        public int laserStartStop { get; set; } = 0;
        public int laserOpState { get; set; } = 1;

        private LAS_DEB_Controller lasDEBcontrol;
        private Timer controlCmd_timer;

        Label[] CbitResult_Power, CbitResult_OpState;


        /// <summary>
        /// ///////////////////////////////
        /// Constructor 
        /// ///////////////////////////////
        /// </summary>
        public SystemDiagnostic_LAS_DEB()
        {
            InitializeComponent();

            lasDEBcontrol = LAS_DEB_Controller.instance;
            lasDEBcontrol.onConnected += LAS_Connection_check;

            CbitResult_Power = new Label[] { led_P_B0, led_P_B1, led_P_B2, led_P_B3, led_P_B4,led_P_B5 };
            CbitResult_OpState = new Label[] { led_O_B0, led_O_B1, led_O_B2, led_O_B3, led_O_B4, led_O_B5, led_O_B6 };

            controlCmd_timer = new Timer();
            controlCmd_timer.Tick += update_timer_Tick;
            controlCmd_timer.Interval = Convert.ToInt32(200); //200ms (5Hz)
            controlCmd_timer.Start();
        }

      
        private void LAS_Connection_check(object sender, ConnectedEventArgs e)
        {
            //Console.WriteLine($"LAS_Connection_check()..{e.IsConnected}");

            if (e.IsConnected.Equals(IPC_MSG.CONNECTION_LAS_DEB))
            {            
                deb_btn_connection.BackColor = Color.DarkOrange;
                connected = true;
                log.Info(IPC_MSG.CONNECTION_LAS_DEB);
            }
            else if (e.IsConnected.Equals(IPC_MSG.DISCONNECTION_LAS_DEB))
            {
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
                lasDEBcontrol.SetBIT(bit);
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


        private void update_timer_Tick(object sender, EventArgs e)
        {
            if (connected)
            {
                //if (text_AA.InvokeRequired)
                //text_AA.Invoke(new Action(() => text_AA.Text = inputByte.ToString()));

                label_OpMode.Text = lasDEBcontrol.D_OpMode;
                label_Mode.Text = lasDEBcontrol.D_Mode;
                label_FireState.Text = lasDEBcontrol.D_FireState;
                label_OpState.Text = lasDEBcontrol.D_OpState;

                var data1 = Convert.ToString(lasDEBcontrol.D_CBitResult1, 2).PadLeft(8, '0');
                var data2 = Convert.ToString(lasDEBcontrol.D_CBitResult2, 2).PadLeft(8, '0');
                data1 = ReverseBinaryString(data1);
                data2 = ReverseBinaryString(data2);

                for (int i = 0; i < CbitResult_Power.Length; i++)
                {
                    if (data1[i] == '1') { CbitResult_Power[i].ForeColor = Color.Red; }  // 1 : 고장
                    else { CbitResult_Power[i].ForeColor = Color.Green; }
                }
                for (int i = 0; i < CbitResult_OpState.Length; i++)
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
        }

        private void rb_ShutterClose_CheckedChanged(object sender, EventArgs e)
        {
            TX_LaserStartStop = "00";
        }

        private void rb_ready_CheckedChanged(object sender, EventArgs e)
        {
            TX_LaserOpMode = "02";
        }

        private void rg_OP_CheckedChanged(object sender, EventArgs e)
        {
            TX_LaserOpMode = "03";
        }

        private void rp_Check_CheckedChanged(object sender, EventArgs e)
        {
            TX_LaserOpMode = "04";
        }

        private void rb_Safe_CheckedChanged(object sender, EventArgs e)
        {
            TX_LaserOpMode = "05";
        }

        private void rb_LaserMode_Align_CheckedChanged(object sender, EventArgs e)
        {
            TX_LaserMode = "00";
        }

        private void rb_LaserMode_Tracking_CheckedChanged(object sender, EventArgs e)
        {
            TX_LaserMode = "01";
        }

        private void rb_LaserMode_Gcal_CheckedChanged(object sender, EventArgs e)
        {
            TX_LaserMode = "02";
        }

        private void rb_OpInitial_CheckedChanged(object sender, EventArgs e)
        {
            TX_LaserOpEnd = "00";
        }

        private void rb_OpEnd_CheckedChanged(object sender, EventArgs e)
        {
            TX_LaserOpEnd = "01";
        }

    }

}
