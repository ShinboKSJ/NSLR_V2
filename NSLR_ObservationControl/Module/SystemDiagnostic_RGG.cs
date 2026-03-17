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
    public partial class SystemDiagnostic_RGG : UserControl
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
        private string pAvoidSetPositionStartOffset = "0000";
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


        RGG_SAT_Controller rggSATcontrol;
        private bool connected { get; set; }
        //iniUtil ini;   // 만들

        private TcpClient interPC_client;
        private NetworkStream interPC_clientStream;

        private Timer update_timer;


        Label[] BitResult;

        public byte BitResult_value;
        public SystemDiagnostic_RGG()
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
            MessageBox.Show("CloseReason RGG...");
        }


        private async void SystemDiagnostic_RGG_Load(object sender, EventArgs e)
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
            cb_RggControl.DataSource = new BindingSource(comboSrc, null);
            cb_RggControl.DisplayMember = "Key";
            cb_RggControl.ValueMember = "Value";

            cb_LUT_Utc.Text = "0000000000000006";
            cb_LUT_Delay.Text = "0000000000000007";

            string data = "1A 01030101 0001 0002 0003 0004 0005 0000000000000006 000000000000007";

            FileInfo exefileinfo = new FileInfo(Application.ExecutablePath);
            string path = exefileinfo.Directory.FullName.ToString();
            string fileName = @"\CONFIG.ini";
            //ini = new iniUtil(path + fileName);
            //var port2Open = ini.GetIniValue("Setting", "PortOpen");
            //comboBox_port.SelectedIndex = comboBox_port.FindString(port2Open);
            //Log(LOG.I, THIS, $"Port2Open....{port2Open} : {comboBox_port.SelectedIndex}");

            string[] ports = SerialPort.GetPortNames();
            comboBox_port.Items.Clear();
            foreach (string comport in ports)
            {
                comboBox_port.Items.Add(comport);
            }
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
            if (e.IsConnected.Contains(IPC_MSG.CONNECTION_RGG_SAT))
            {
                btn_connection_rgg.Text = "SAT RGG 연결됨";
                btn_connection_rgg.BackColor = Color.DarkOrange;
                connected = true;
                log.Debug($"{IPC_MSG.CONNECTION_RGG_SAT}");
            }
            else if (e.IsConnected.Contains(IPC_MSG.DISCONNECTION_RGG_SAT))
            {
                btn_connection_rgg.Text = "SAT RGG 연결안됨";
                btn_connection_rgg.BackColor = Color.DarkGray;
                connected = false;
                log.Debug($"{IPC_MSG.DISCONNECTION_RGG_SAT}");
            }
            else if (e.IsConnected.Contains(IPC_MSG.CONNECTION_RGG_DEB))
            {
                btn_connection_rgg.Text = "DEB RGG 연결됨";
                btn_connection_rgg.BackColor = Color.DarkOrange;
                connected = true;
                log.Debug($"{IPC_MSG.CONNECTION_RGG_DEB}");
            }
            else if (e.IsConnected.Contains(IPC_MSG.DISCONNECTION_RGG_DEB))
            {
                btn_connection_rgg.Text = "DEB RGG 연결안됨";
                btn_connection_rgg.BackColor = Color.DarkGray;
                connected = false;
                log.Debug($"{IPC_MSG.DISCONNECTION_RGG_DEB}");
            }

        }

        private async void comboBox_port_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var count = 0;
            //MessageBox.Show($"comboBox...enter #{count++}");
            //comboBox_port.BackColor = Color.Gray;
            var port = comboBox_port.SelectedItem.ToString();
            log.Debug($"Open_Port({port})");

            await Task.Delay(100); // 비동기적으로 0.2초 딜레이
           
            try
            {
                await rggSATcontrol.Open();
                // 포트 열기 성공 후 처리
                log.Debug($"Port {port} opened successfully");
            }
            catch (Exception ex)
            {
                // 예외 처리
                log.Error($"Error opening port {port}: {ex.Message}");
                MessageBox.Show($"포트 열기 실패: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                //if (tranmissionReat_check.Checked) { periodicSend_timer.Enabled = true; }
                //else { periodicSend_timer.Enabled = false; }
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
        private void cb_RggControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_RggControl.Focused != true)
            {
                return;    // 포커스가 없는 상태에서의 변경은 처리안함
            } 
            string key = ((KeyValuePair<string, string>)cb_RggControl.SelectedItem).Key;
            string value = ((KeyValuePair<string, string>)cb_RggControl.SelectedItem).Value;
            pRGG_Control = value;
            Console.WriteLine($"RggControl_changed...pRGG_Control : {pRGG_Control}");
            rggSATcontrol.Cmd_Mode(pRGG_Control);
        }
        //[2] Gate Pulse Width
        private void numGPSO_ValueChanged(object sender, EventArgs e)
        {
            var gpso = (Int16)numGPSO.Value;
            pGatePulseStartOffset = gpso.ToString("X4");
            label_gpso.Text = pGatePulseStartOffset;
            label_SetGateSO_m.Text = (gpso * 5).ToString() + " (ns)";
            rggSATcontrol.Cmd_GatePulse_SO(pGatePulseStartOffset);
        }
        //[3] Gate Pulse Width 
        private void numGPW_ValueChanged(object sender, EventArgs e)
        {
            var gpw = (UInt16)numGPW.Value;
            pGatePulseWidth = gpw.ToString("X4");
            label_gpw.Text = pGatePulseWidth;
            rggSATcontrol.Cmd_GatePulse_W(pGatePulseWidth);
            label_SetGateW_m.Text  = (gpw * 5).ToString() + " (ns)"; 
        }
        //[4] Avoid Set Position Start Offset
        private void cb_AvoidSetPosStartOffset_SelectedIndexChanged(object sender, EventArgs e)
        {
            pAvoidSetPositionStartOffset = cb_AvoidSetPosStartOffset.SelectedItem.ToString();
            //rggSATcontrol.Cmd_GatePulse_SO(pAvoidSetPositionStartOffset);
        }
        //[5] Avoid Width 
        private void cb_AvoidWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            pAvoidWidth = cb_AvoidWidth.SelectedItem.ToString();
            rggSATcontrol.Cmd_Avoid_W(pAvoidWidth);
        }
        //[6] Lookup Table (UTC)
        private void cb_LUT_Utc_SelectedIndexChanged(object sender, EventArgs e)
        {
            pLUT_UTC = cb_LUT_Utc.SelectedItem.ToString();
            //Log(LOG.I, THIS, $"LUT UTC  {pLUT_UTC}");
            if (connected)
                rggSATcontrol.Cmd_Lut_Utc(pLUT_UTC);
        }
        //[7] Lookup Table (Delay)
        private void cb_LUT_Delay_SelectedIndexChanged(object sender, EventArgs e)
        {
            pLUT_Delay = cb_LUT_Delay.SelectedItem.ToString();
            //Log(LOG.I, THIS, $"LUT delay  {pLUT_Delay}");
            if (connected)
                rggSATcontrol.Cmd_Lut_Delay(pLUT_Delay);
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
