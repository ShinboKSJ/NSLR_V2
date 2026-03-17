using log4net;
using log4net.Config;
using NSLR_ObservationControl.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Net.Http;
using OpenCvSharp;
using Newtonsoft.Json;
using System.Windows.Markup;
using System.Drawing.Drawing2D;
using System.Data.SqlClient;
using NSLR_ObservationControl.Subsystem;
using Newtonsoft.Json.Linq;
using Timer = System.Windows.Forms.Timer;

namespace NSLR_ObservationControl.Module
{
    public partial class SystemDiagnostic_AID : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // ////////////////////////
        // 개폐돔 ICD
        // ////////////////////////
        //const string remoteIpAddress = "192.168.10.91"; //개폐돔 ICD
        
        public static bool connected { get; set; } = false;

        public byte AID_state_OpMode { get; set; } = 0;
        public byte AID_state_RadarSync { get; set; } = 0;
        public byte AID_state_ADSBSync { get; set; } = 0;
        public byte AID_state_CBITresult { get; set; } = 0;
        public byte AID_state_PBITresult { get; set; } = 0;
        public byte AID_state_IBITresult { get; set; } = 0;


        AID_Controller aidController;
        private Timer survailanceTimer;
        private bool test_AID { get; set; } = false;
        public SystemDiagnostic_AID()
        {
            InitializeComponent();
            aidController = AID_Controller.instance;
        }

        private void SystemDiagnostic_AID_Load(object sender, EventArgs e)
        {

            aidController.AID_Connected += Connection_check;

            survailanceTimer = new Timer();
            survailanceTimer.Tick += survailanceTimer_Tick;
            survailanceTimer.Interval = 200; //5Hz   
            survailanceTimer.Start();
        }

        int count = 0;
        private void survailanceTimer_Tick(object sender, EventArgs e)
        {
            //var LogInfo = $"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff").PadRight(20)} ";
            
            //UpdatePacketData();
            //aidController
            if (aidController.isAirplaneDetected)
            {
                if((count++ %2) == 0)
                    label_AirplaneDetecred.Text = "항공기 감지 !!!";
                else
                    label_AirplaneDetecred.Text = "";

                log.Info("항공기 감지");
            }
            else
            {
                label_AirplaneDetecred.Text = "";
            }
            textBox_OpMode.Text = aidController.state_OPMODE;
            textBox_RadarSync.Text = aidController.state_RADAR;
            textBox_ADSbSync.Text = aidController.state_ADS_B;
            
            textBox_CBIT0.Text = aidController.state_CBIT_B0.ToString();
            textBox_CBIT1.Text = aidController.state_CBIT_B1.ToString();
            textBox_CBIT2.Text = aidController.state_CBIT_B2.ToString();
            textBox_CBITresult.Text = aidController.state_CBIT;

            textBox_PBIT0.Text = aidController.state_PBIT_B0.ToString();
            textBox_PBIT1.Text = aidController.state_PBIT_B1.ToString();
            textBox_PBIT2.Text = aidController.state_PBIT_B2.ToString();
            textBox_PBitresult.Text = aidController.state_PBIT;

            textBox_IBIT0.Text = aidController.state_PBIT_B0.ToString();
            textBox_IBIT1.Text = aidController.state_PBIT_B1.ToString();
            textBox_IBIT2.Text = aidController.state_PBIT_B2.ToString();
            textBox_IBITresult.Text = aidController.state_IBIT; 

            if (aidController.IsConnected())
            {
                btn_aid_connection.BackColor = Color.DarkOrange;
                connected = true;
                //Console.WriteLine("[SystemDiagnostic_AID] Connected");
                //log.Info("[SystemDiagnostic_AID] Connected");
            }
            else
            {
                btn_aid_connection.BackColor = Color.Gray;
                btn_aid_connection.ForeColor = System.Drawing.Color.White;
                connected = false;
                //Console.WriteLine("[SystemDiagnostic_AID] DisConnected xxxxxx");
                //log.Info("[SystemDiagnostic_AID] DisConnected");
            }
        }

        private void Connection_check(object sender, ConnectedEventArgs e)
        {
            //Console.WriteLine($"[SystemDiagnostic_AID] Connection_check()...[{e.IsConnected}]..");

            if (e.IsConnected.Contains(IPC_MSG.CONNECTION_AID))
            {
                btn_aid_connection.BackColor = Color.DarkOrange;
                connected = true;
                log.Info("[AID] Connected");
            }
            else if (e.IsConnected.Contains(IPC_MSG.DISCONNECTION_AID))
            {
                btn_aid_connection.BackColor = Color.Gray;
                btn_aid_connection.ForeColor = System.Drawing.Color.White;
                connected = false;
                log.Info("[AID] DisConnected");
            }
        }

        
        private void btn_PBIT_Click(object sender, EventArgs e)
        {
            aidController.SetBIT("PBIT");
        }

        private void btn_IBIT_Click(object sender, EventArgs e)
        {
            aidController.SetBIT("IBIT");
        }
            

        private void textBox_EL_MouseLeave(object sender, EventArgs e)
        {
            Double get_number;

            if (Double.TryParse(textBox_EL.Text, out get_number))
            {
                //Console.WriteLine("Valid Double. Number is : " + get_number);
            }
            else
            {
                //Console.WriteLine("Invalid Double");
            }
        }

        private void btn_cmd_AzEl_Click(object sender, EventArgs e)
        {

            if (double.TryParse(textBox_Az.Text, out double Az) && double.TryParse(textBox_EL.Text, out double El))
            {
                if (connected)
                {
                    aidController.SetAzEl(Az, El);
                }
            }
            else
            {
                MessageBox.Show("유효한 숫자를 입력해주세요.");
            }

        }


        private void AID_Test_CheckedChanged(object sender, EventArgs e)
        {
            aidController.RaiseDetectionEvent(test_AID = !test_AID);
            if (test_AID)
                { AID_Test.BackColor = Color.Green; }
            else 
                { AID_Test.BackColor = Color.DimGray; }
        }
    }
}
