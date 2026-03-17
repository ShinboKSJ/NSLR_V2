using log4net;
using mv.impact.acquire;
using NSLR_ObservationControl;
using NSLR_ObservationControl.Subsystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl.Module
{
    public partial class Observation_RangeGatePanel : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        RGG_SAT_Controller satRGGcontrol = RGG_SAT_Controller.instance;
        RGG_DEB_Controller debRGGcontrol = RGG_DEB_Controller.instance;


        private Timer update_timer;
        private const string GateWidth = "40";


        public Observation_RangeGatePanel()
        {
            InitializeComponent();

            numericUpDown_GateWidth.Maximum = 0xffff;
            numericUpDown_GateWidth.Minimum = 0;
            numericUpDown_GateWidth.Value = 40;

            numericUpDown_GateSO.Maximum = 32767;
            numericUpDown_GateSO.Minimum = -32768;
            numericUpDown_GateSO.Value = 0;
        }

        private void Observation_RangeGatePanel_Load(object sender, EventArgs e)
        {
                update_timer = new Timer();
                update_timer.Tick += update_timer_Tick;
                update_timer.Interval = Convert.ToInt32(200); //50ms (20Hz)
                update_timer.Start();
        }

        private void rggModeSet()
        {
            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            {
                if (TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.RANGE)
                {
                    if (!satRGGcontrol.IsRangeMode())
            {
                        log.Info("RGG : SLR-RANGE 모드 세팅 ");
                satRGGcontrol.SetRangeMode(); // RGG 준비 - Range Mode
            }
        }
                else if (TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.G_CAL)
                {
                    if (!satRGGcontrol.IsGCalMode())
                    {
                        log.Info("RGG : SLR-GCAL 모드 세팅 ");
                        satRGGcontrol.SetGrndCalMode(); // RGG 준비 - GroundCal Mode
                    }
                }
            }
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                if (TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.RANGE)
                {
                    if (!debRGGcontrol.IsRangeMode())
                    {
                        log.Info("RGG : DLT-RANGE 모드 세팅 ");
                        debRGGcontrol.SetRangeMode(); // RGG 준비 - Range Mode
                    }
                }
                else if (TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.G_CAL)
                {
                    if (!debRGGcontrol.IsGCalMode())
                    {
                        log.Info("RGG : DLT-GCAL 모드 세팅 ");
                        debRGGcontrol.SetGrndCalMode(); // RGG 준비 - GroundCal Mode 
                    }
                }
            }
        }

        private bool msging = false;
        private uint msging_cnt = 0;
        private void update_timer_Tick(object sender, EventArgs e)
        {
            if (satRGGcontrol.IsConnected())
            {
                if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            {     
                if (satRGGcontrol.RggControl.Equals(RGG_MODE.Ranging))
                {
                    label_rggMode.Text = "Range Mode";
                    label_rggMode.BackColor = Color.MidnightBlue;
                    label_rggMode.ForeColor = Color.Orange;
                }
                else if (satRGGcontrol.RggControl.Equals(RGG_MODE.GroundCAL))
                {
                    label_rggMode.Text = "G.CAL Mode";
                    label_rggMode.BackColor = Color.MidnightBlue;
                    label_rggMode.ForeColor = Color.Lime;
                }
                    label_RGG.Text = "SRGG";

                label_GateSONow.Text = satRGGcontrol.RggGatePulseStartOffset;
                label_GateWidthNow.Text = satRGGcontrol.RggGatePulseWidth;
                    //if (msging)
                    //    log.Info($">>SAT RGG status :[Mode:{satRGGcontrol.RggControl}] GateSO:{label_GateSONow.Text} / GateW:{label_GateWidthNow.Text}");
                }
                else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
                {
                    if (debRGGcontrol.RggControl.Equals(RGG_MODE.Ranging))
                    {
                        label_rggMode.Text = "Range Mode";
                        label_rggMode.BackColor = Color.MidnightBlue;
                        label_rggMode.ForeColor = Color.Orange;
                    }
                    else if (debRGGcontrol.RggControl.Equals(RGG_MODE.GroundCAL))
                    {
                        label_rggMode.Text = "G.CAL Mode";
                        label_rggMode.BackColor = Color.MidnightBlue;
                        label_rggMode.ForeColor = Color.Lime;
                    }
                    label_RGG.Text = "DRGG";

                    label_GateSONow.Text = debRGGcontrol.RggGatePulseStartOffset;
                    label_GateWidthNow.Text = debRGGcontrol.RggGatePulseWidth;
                    //if (msging)
                    //    log.Info($">>DEB RGG status :[Mode:{debRGGcontrol.RggControl}] GateSO:{label_GateSONow.Text} / GateW:{label_GateWidthNow.Text}");
                }
                if (msging_cnt++ > 20) { msging_cnt = 0;  msging = true; } else { msging = false; }
            }
            //rggModeSet();
        }

        private void numericUpDown_GateSO_ValueChanged(object sender, EventArgs e)
        {
            label_NewGateSO.Text = numericUpDown_GateSO.Value.ToString();
        }
        private void numericUpDown_GateWidth_ValueChanged(object sender, EventArgs e)
        {
            label_NewGateWidth.Text = numericUpDown_GateWidth.Value.ToString();
        }

        private void 레벨조정_Click(object sender, EventArgs e)
        {
            label_NewGateWidth.Text = GateWidth;
        }

        private void btn_SetGateControl_Click(object sender, EventArgs e)
        {
           var gpw = (UInt16)numericUpDown_GateWidth.Value;
           var gpso = (Int16)numericUpDown_GateSO.Value;

           var str_gpw = gpw.ToString("X4");
           var str_gpso = gpso.ToString("X4");
           log.Info($"GateControl( Width: {gpw}({str_gpw})/ {gpso}({str_gpso})"); 
            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            {
                if (TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.RANGE)
           satRGGcontrol.Cmd_GateControl(RGG_MODE.Ranging,str_gpw , str_gpso);

                else if (TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.G_CAL)
                    satRGGcontrol.Cmd_GateControl(RGG_MODE.GroundCAL, str_gpw, str_gpso);
            }
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                if (TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.RANGE)
                    debRGGcontrol.Cmd_GateControl(RGG_MODE.Ranging, str_gpw, str_gpso);

                else if (TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.G_CAL)
                    debRGGcontrol.Cmd_GateControl(RGG_MODE.GroundCAL, str_gpw, str_gpso);
            }

        }

        private void label_NewGateWidth_MouseHover(object sender, EventArgs e)
        {
            var gpw = (UInt16)numericUpDown_GateWidth.Value;
            var gpso = (int)numericUpDown_GateSO.Value;

            var str_gpw = gpw.ToString("X4");
            var str_gpso = gpso.ToString("X4");
            log.Info($"GateControl( Width: {gpw}({str_gpw})/ {gpso}({str_gpso})");
        }

        private void label3_Click(object sender, EventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                MessageBox.Show("CTRL 키가 눌린 상태에서 버튼이 클릭되었습니다.");
            }
            else
            {
                MessageBox.Show("CTRL 키 없이 버튼이 클릭되었습니다.");
            }
        }

        private void label_Click(object sender, EventArgs e)
        {
            if(DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            {
                if (TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.RANGE)
                    satRGGcontrol.SetRangeMode(); // RGG 준비 - Range Mode
                else if (TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.G_CAL)
                    satRGGcontrol.SetGrndCalMode(); // RGG 준비 - GroundCl Mode
            }
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                if (TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.RANGE)
                    debRGGcontrol.SetRangeMode(); // RGG 준비 - Range Mode
                else if (TaskModeNow.CurrentSystemObject == TaskModeNow.SystemObject.G_CAL)
                    debRGGcontrol.SetGrndCalMode(); // RGG 준비 - GroundCl Mode
            }
            /////////////////////////////////////////////////////////
        }

        private void label_rggMode_Click(object sender, EventArgs e)
        {
            rggModeSet();
        }
    }
}
