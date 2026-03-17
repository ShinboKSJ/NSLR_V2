 using log4net;
using log4net.Config;
using Newtonsoft.Json;
using NSLR_ObservationControl.Network;
using NSLR_ObservationControl.Subsystem;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Policy;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Markup;
using Timer = System.Windows.Forms.Timer;

namespace NSLR_ObservationControl.Module
{
    public partial class SystemDiagnostic_RGG_SAT : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ulong seq_num_rx { get; set; }
        public ulong seq_num_tx { get; set; }

        private UInt16 RggControl { get; set; }
        private UInt16 RggGatePulseWidth { get; set; }
        private Int16  RggGatePulseStartOffset { get; set; }

        private string pLEN = "1A"; //26
        private string pRGG_Control = "0001";
        private string pGatePulseWidth = "0000";
        private string pGatePulseStartOffset = "0000";
        private string pAvoidWidth = "0000";
        private string pAvoidStartOffset = "0000";
        private string pLUT_UTC = "0000000000000000";
        private string pLUT_Delay = "0000000000000000";
        private string pMSG_ID = "01030001";
        private string pDATA = "";
        private string pCHECKSUM = "";
        private string pPacket = "";

        string[] rggControlName = { "0000", "0001", "0002", "0003", "0004", "0005", "0006" };
        
        private long msg_sequence { get; set; } = 0;
        public string message { get; set; }
        public static string SenderAddress { get; set; }
        public static string UserName { get; set; }


        private RGG_SAT_Controller rggSATcontrol;
        private bool connected { get; set; } = false;
        //iniUtil ini;   // 만들


        private Timer update_timer;


        Label[] BitResult;

        public byte BitResult_value;
        public SystemDiagnostic_RGG_SAT()
        {
            InitializeComponent();

            rggSATcontrol = RGG_SAT_Controller.instance;

            rggSATcontrol.SerialConnected += RggControler_SerialConnected;

            BitResult = new Label[] { led_P_B0, led_P_B1, led_P_B2, led_P_B3, led_P_B4 };

            numGPSO.Minimum = -32768;
            numGPSO.Maximum = 32767;

            numGPW.Minimum =  0;
            numGPW.Maximum = 65535;

            update_timer = new Timer();
            update_timer.Tick += update_timer_Tick;
            update_timer.Interval = Convert.ToInt32(50); //50ms (20Hz)
            update_timer.Start();

            //portFinder_timer = new Timer();
            //portFinder_timer.Tick += portFinder_timer_Tick;
            //portFinder_timer.Interval = Convert.ToInt32(500); //50ms (20Hz)
        }


        private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // FormClosing 이벤트에서 이벤트 핸들러를 해제
            //MessageBox.Show("CloseReason RGG...");
        }

        private void SystemDiagnostic_RGG_Load(object sender, EventArgs e)
        {

            // ComboBox init
            Dictionary<string, string> comboSrc = new Dictionary<string, string>();
            comboSrc.Add("0x00 N/A", "0000");
            comboSrc.Add("0x01 대기", "0001");
            comboSrc.Add("0x02 LUT송신", "0002");
            comboSrc.Add("0x03 LUT초기화", "0003");
            comboSrc.Add("0x04 Grnd.Cal", "0004");
            comboSrc.Add("0x05 Range", "0005");
            comboSrc.Add("0x06 Overlap avoid", "0006");
            comboSrc.Add("0x07 안전상태 진입", "0007");
            
            update_timer = new Timer();
            update_timer.Tick += update_timer_Tick;
            update_timer.Interval = Convert.ToInt32(50); //50ms (20Hz)
            update_timer.Start();
        }

        //private  void portFinder_timer_Tick()
        //{
        //    string[] ports = SerialPort.GetPortNames();
        //    comboBox_port.Items.Clear();
        //    foreach (string comport in ports)
        //    {
        //        comboBox_port.Items.Add(comport);
        //        rggSATcontrol.Open_Port(comport);
        //        //await Task.Delay(300); // 비동기적으로 0.2초 딜레이
        //        do_BIT("PBIT");
        //    }
        //}

        private void RggControler_SerialConnected(object sender, ConnectedEventArgs e)
        {
            if(e.IsConnected.Equals(IPC_MSG.CONNECTION_RGG_SAT))
            connected = true;
            else
                connected = false;
        }

        private void do_BIT(string bit)
        {

            if (connected)
            {

                if (bit.Equals("PBIT"))
                    rggSATcontrol.Cmd_Bit("PBIT");
                else if (bit.Equals("CBIT"))
                    rggSATcontrol.Cmd_Bit("CBIT");
                else if (bit.Equals("IBIT"))
                    rggSATcontrol.Cmd_Bit("IBIT");
            }
            else
            {
                MessageBox.Show("Serial Port를 먼저 Open해 주세요");
            }
        }


        private void sendData_repeat_period_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        //[1] RGG Control
        private void rb_RGGctrl_set(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb == null || !rb.Checked) return;

            string selectedTag = rb.Tag.ToString();
            switch (selectedTag)
            {
                case "STANBY":
                    pRGG_Control = "0001";
                    rggSATcontrol.Cmd_Mode(pRGG_Control);
                    break;

                case "LUTSEND":
                    pRGG_Control = "0002";
                    rggSATcontrol.Cmd_Mode(pRGG_Control);
                    break;

                case "LUTINIT":
                    pRGG_Control = "0003";
                    rggSATcontrol.Cmd_Mode(pRGG_Control);
                    break;

                case "GCAL":
                    pRGG_Control = "0004";
                    rggSATcontrol.Cmd_Mode(pRGG_Control);
                    break;

               case "RANGE":
                    pRGG_Control = "0005";
                    rggSATcontrol.Cmd_Mode(pRGG_Control);
                    break;

                case "OVR":
                    pRGG_Control = "0006";
                    rggSATcontrol.Cmd_Mode(pRGG_Control);
                    break;
            }
        }
        
        //[2] Gate Pulse Width 
        private void numGPW_Leave(object sender, EventArgs e)
        {
            var gpw = (UInt16)numGPW.Value;
            pGatePulseWidth = gpw.ToString("X4");
            rggSATcontrol.Cmd_GatePulse_W(pGatePulseWidth);
        }
        private void numGPW_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var gpw = (UInt16)numGPW.Value;
                pGatePulseWidth = gpw.ToString("X4");
                rggSATcontrol.Cmd_GatePulse_W(pGatePulseWidth);
            }
        }

        //[3] Gate Pulse Start Offset
        private void numGPSO_Leave(object sender, EventArgs e)
        {
            var gpso = (Int16)numGPSO.Value;
            pGatePulseStartOffset = gpso.ToString("X4");
            rggSATcontrol.Cmd_GatePulse_SO(pGatePulseStartOffset);
        }

        private void numGPSO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var gpso = (Int16)numGPSO.Value;
                pGatePulseStartOffset = gpso.ToString("X4");
                rggSATcontrol.Cmd_GatePulse_SO(pGatePulseStartOffset);
            }
        }

        //[4] Avoid Set Position Start Offset
        private void numAvoidSO_Leave(object sender, EventArgs e)
        {
            var avoidSo = (UInt16)numAvoidSO.Value;
            pAvoidStartOffset = avoidSo.ToString("X4");
            rggSATcontrol.Cmd_GatePulse_W(pAvoidStartOffset);
        }

        private void numAvoidSO_KeyDown(object sender, KeyEventArgs e)
        { 
            if (e.KeyCode == Keys.Enter)
            {
                var avoidSo = (UInt16)numAvoidSO.Value;
                pAvoidStartOffset = avoidSo.ToString("X4");
                rggSATcontrol.Cmd_GatePulse_W(pAvoidStartOffset);
            }
        }

        //[5] Avoid Width
        private void numAvoidWidth_Leave(object sender, EventArgs e)
        {
            var avoidWith = (UInt16)numAvoidWidth.Value;
            pAvoidWidth = avoidWith.ToString("X4");
            rggSATcontrol.Cmd_GatePulse_W(pAvoidWidth);
        }

        private void numAvoidWidth_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var avoidWith = (UInt16)numAvoidWidth.Value;
                pAvoidWidth = avoidWith.ToString("X4");
                rggSATcontrol.Cmd_GatePulse_W(pAvoidWidth);
            }
        }

        //[6] Lookup Table (UTC)
        private void numUTC_Leave(object sender, EventArgs e)
        {
            var UTC = (UInt16)numUTC.Value;
            pLUT_UTC = UTC.ToString("X4");
            rggSATcontrol.Cmd_Lut_Utc(pLUT_UTC);
        }

        private void numUTC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var UTC = (UInt16)numUTC.Value;
                pLUT_UTC = UTC.ToString("X4");
                rggSATcontrol.Cmd_Lut_Utc(pLUT_UTC);
            }
        }


        //[7] Lookup Table (Delay ,ToF)        
        private void numTOF_Leave(object sender, EventArgs e)
        {
            var TOF = (UInt16)numTOF.Value;
            pLUT_Delay = TOF.ToString("X4");
            rggSATcontrol.Cmd_Lut_Delay(pLUT_Delay);
        }

        private void numTOF_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var TOF = (UInt16)numTOF.Value;
                pLUT_Delay = TOF.ToString("X4");
                rggSATcontrol.Cmd_Lut_Delay(pLUT_Delay);
            }
        }

        // ControlCommand
        private void btn_set_command_Click(object sender, EventArgs e)
        {
            if (connected)
            {
                rggSATcontrol.Set_Command();
                //if (tranmissionReat_check.Checked) { periodicSend_timer.Enabled = true; }
                //else { periodicSend_timer.Enabled = false; }
            }
            else
            {
                MessageBox.Show("Serial Port를 먼저 Open해 주세요");
            }
        }
       
        private void btn_PBIT_Click(object sender, EventArgs e)
        {
            rggSATcontrol.Cmd_Bit("PBIT");
        }
        private void btn_IBIT_Click(object sender, EventArgs e)
        {
            rggSATcontrol.Cmd_Bit("IBIT");
        }
        private void btn_CBIT_Click(object sender, EventArgs e)
        {
            rggSATcontrol.Cmd_Bit("CBIT");

        }


        private void periodicSend_timer_Tick(object sender, EventArgs e)
        {
            if (connected)
            {
                rggSATcontrol.Set_Command();
            }
            else
            {
                MessageBox.Show($"serial port를 먼저 open해 주세요");
            }
        }
        private void tranmissionReat_check_CheckedChanged(object sender, EventArgs e)
        {
            if (tranmissionReat_check.Checked)
            {
                if (connected)
                    periodicSend_timer.Enabled = true;
            }
            else
            {
                periodicSend_timer.Enabled = false;
            }
        }

        private void radioButton_ctrl_CheckedChanged(object sender, EventArgs e)
        {
            pLEN = "1A";
        }

        private void update_timer_Tick(object sender, EventArgs e)
        {
            if (connected)
            {
                //if (text_AA.InvokeRequired)
                //text_AA.Invoke(new Action(() => text_AA.Text = inputByte.ToString()));

                if (rggSATcontrol.IsConnected())
                    btn_connection_rgg.BackColor = Color.DarkOrange;
                else
                    btn_connection_rgg.BackColor = Color.DarkGray;

                label_RggControl.Text = rggSATcontrol.RggControl;
                if (label_RggControl.Text.Equals("00"))
                    label_RggCtrl.Text = "    N/A     ";
                else if (label_RggControl.Text.Equals("01"))
                    label_RggCtrl.Text = "    대기     ";
                else if (label_RggControl.Text.Equals("02"))
                    label_RggCtrl.Text = "  LUT송신   ";
                else if (label_RggControl.Text.Equals("03"))
                    label_RggCtrl.Text = "  LUTInit   ";
                else if (label_RggControl.Text.Equals("04"))
                    label_RggCtrl.Text = "  GRND CAL  ";
                else if (label_RggControl.Text.Equals("05"))
                    label_RggCtrl.Text = "   RANGE    ";
                else if (label_RggControl.Text.Equals("06"))
                    label_RggCtrl.Text = "OVERap Avoid";
                else if (label_RggControl.Text.Equals("07"))
                    label_RggCtrl.Text = "안전상태진입";

                label_GateStartOffset.Text = rggSATcontrol.RggGatePulseStartOffset;
                label_GatePulseWidth.Text = rggSATcontrol.RggGatePulseWidth;

                label_GateStartOffset_m.Text = (rggSATcontrol.n_RggGatePulseStartOffset *5).ToString() + " (ns)"; 
                label_GateWidth_m.Text = (rggSATcontrol.n_RggGatePulseWidth*5).ToString() + " (ns)"; ;
                
                var data = ReverseBinaryString(rggSATcontrol.strBitResultCode);

                for (int i = 0; i < 5; i++)
                {
                    if (data[i] == '1') { BitResult[i].ForeColor = Color.Red; }  // 1 : 고장
                    else { BitResult[i].ForeColor = Color.Green; }
                }
            }
        }

        public static string ReverseBinaryString(string binaryString)
        {
            char[] charArray = binaryString.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }


    }
}
