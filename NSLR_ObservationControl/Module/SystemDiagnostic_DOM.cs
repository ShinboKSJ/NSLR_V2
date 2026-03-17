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
using System.Timers;

namespace NSLR_ObservationControl.Module
{
    public partial class SystemDiagnostic_DOM : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        //const string remoteIpAddress = "192.168.10.81"; //개폐돔 ICD
        public static bool connected { get; set; } = false;

        private DOM_Controller_v40 domController;

        Label[] AA_led;
        
        System.Timers.Timer AAupdateTimer;


        public SystemDiagnostic_DOM()
        { 
            InitializeComponent();

            domController = DOM_Controller_v40.instance;

            domController.DomeConnected += Connection_check;

            AA_led = new Label[] { label_led0 , label_led1, label_led2, label_led3, label_led4, label_led05, label_led06  };
            label_led0.ForeColor = Color.Green;
            AAupdateTimer = new System.Timers.Timer();
            AAupdateTimer.Elapsed += new ElapsedEventHandler(AAupdateTimer_Tick);
            AAupdateTimer.Interval = 300;

            if (domController.IsConnected())
                Connection_check(this, new ConnectedEventArgs("DOM_CONN"));
        }


        private void Connection_check(object sender, ConnectedEventArgs e)
        {

            if (e.IsConnected.Contains("DOM_CONN"))
            {
                connected = true;
                //log.Info(">> DOM_CONN");
                btn_dome_connection.BackColor = Color.DarkOrange;
                AAupdateTimer.Start();
            }
            else if (e.IsConnected.Contains("DOM_DISCONN"))
            {
                AAupdateTimer.Stop();
                //log.Info(">> DOM_DISCONN");
                btn_dome_connection.BackColor = Color.Gray;
                btn_dome_connection.ForeColor = System.Drawing.Color.White;
                connected = false;
            }
        }

                
        private void btn_PBIT_Click(object sender, EventArgs e)
        {
            domController.doPBIT();
        }

        private void btn_IBIT_Click(object sender, EventArgs e)
        {
            domController.doIBIT();
        }

        private void btn_CBIT_Click(object sender, EventArgs e)
        { 
            ;
        }

        private void btn_domeHomeSearch_Click(object sender, EventArgs e)
        {
            domController.doHomeSearch();
        }


        private void btn_domeOpen_Click(object sender, EventArgs e)
        {
            domController.doOpen();
        }

        private void btn_domeClose_Click(object sender, EventArgs e)
        {
            domController.doClose();
        }

        private void btn_domeStop_Click(object sender, EventArgs e)
        {
            domController.doStop();
        }

        private void btn_domeParking_Click(object sender, EventArgs e)
        {
            domController.doPark();
        }

        private void btn_domePosMove_Click(object sender, EventArgs e)
        {
            double PositionValue = (double)numericUpDown_Pos.Value;
            domController.doPositionMove(PositionValue, 30);
        }

        public bool OnTrackingNow = false;
        private async void btn_domTracking_Click(object sender, EventArgs e)
        {            
            OnTrackingNow = !OnTrackingNow; // Toggles the value

            double startValue = double.Parse(text_TrackingStart.Text);
            double intervalValue = double.Parse(text_TrackingInterval.Text);

            for (int i = 0; i <= 21 ; i++) //[1]~[7]
            {
                double interrimValue = startValue + (i * intervalValue);
                //domController.doTracking(interrimValue);
                await Task.Delay(13);
            }
        }


        void AAupdateTimer_Tick(object sender, ElapsedEventArgs e)
        {

            if(label_shutter.InvokeRequired)
                label_shutter.Invoke(new Action (() => label_shutter.Text = domController.strShutter));
            if(label_rain.InvokeRequired)
                label_rain.Invoke(new Action (() => label_rain.Text = domController.strRain));
            if(label_home.InvokeRequired)
                label_home.Invoke(new Action (() => label_home.Text = domController.strHome));
            if(label_drive.InvokeRequired)
                label_drive.Invoke(new Action (() => label_drive.Text = domController.strDrive));    
            if(label_position.InvokeRequired)
                label_position.Invoke(new Action (() => label_position.Text = domController.strPosition));
            
            //log.Info($"AA_update()..SHUT [{label_shutter.Text}]");
            //log.Info($"AA_update()..RAIN [{label_rain.Text}]");
            //log.Info($"AA_update()..HOME [{label_home.Text}]");
            //log.Info($"AA_update()..DRVE [{label_drive.Text}]");
            //log.Info($"AA_update()..POS. [{label_position.Text}]");
            byte inputByte = domController.strAA;
            //log.Info($"AA_update()..BITv [{inputByte}]");
            if(text_AA.InvokeRequired)
                text_AA.Invoke(new Action (() => text_AA.Text = inputByte.ToString()));

            //if (inputByte == 1) label_bit.Text = "PBIT";
            //else if (inputByte == 2) label_bit.Text = "IBIT";

            if (label_bit.InvokeRequired)
                label_bit.Invoke(new Action(() => label_bit.Text = domController.strBit));

//            label_bit.Text = domController.strBit;
            string binaryString = Convert.ToString(inputByte, 2).PadLeft(8, '0');
            //log.Info($"AA_update()..BITresult [{binaryString}]");
            for (int i = 0; i < binaryString.Length - 1; i++)
            {
                if (binaryString[i] == '1')
                {
                    AA_led[i].ForeColor = Color.Green;
                }
                else
                {
                    AA_led[i].ForeColor = Color.Red;
                }
            }

        }

        private void numericUpDown_Pos_ValueChanged(object sender, EventArgs e)
        {
            double data = (double)numericUpDown_Pos.Value;
        }
    }
}
