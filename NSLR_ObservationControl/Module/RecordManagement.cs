using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl.Module
{
    public partial class RecordManagement : UserControl
    {
        private Panel panel_submenu;
        private RadioButton button_airplaneWarning;
        private RadioButton button_resultOfGroundCal;
        private RadioButton button_resultOfStarCal;
        private RadioButton button_satellite;
        private RadioButton button_starCatalog;
        private RadioButton button_environment;
        private RadioButton button_faultHistory;
        private RadioButton button_accessHistory;
        private RadioButton button_statisticsObservation;
        private RadioButton button_resultOfObservation;
        private Panel panel_RecordMngMain;
        private RadioButton button_TLECPF;

        RecordManagement_TLECPF screen_TLECPF = new RecordManagement_TLECPF();
        RecordManagement_AccessHistory screen_AccessHistory = new RecordManagement_AccessHistory();
        RecordManagement_FaultHistory screen_faultHistory = new RecordManagement_FaultHistory();
        RecordManagement_Environment screen_Environment = new RecordManagement_Environment();
        RecordManagement_StarCatalog screen_StarCatalog = new RecordManagement_StarCatalog();
        RecordManagement_Satellite screen_Satellite = new RecordManagement_Satellite();
        RecordManagement_ResultOfStarCal screen_ResultOfStarCal = new RecordManagement_ResultOfStarCal();
        RecordManagement_ResultOfGroundCal screen_ResultOfGroundCal = new RecordManagement_ResultOfGroundCal();
        RecordManagement_AirplaneWarning screen_AirplaneWarning = new RecordManagement_AirplaneWarning();
        RecordManagement_ResultOfObservation screen_ResultOfObservation  = new RecordManagement_ResultOfObservation();
        RecordManagement_StatisticsOfObservation screen_StatisticsOfObservation = new RecordManagement_StatisticsOfObservation();
        
        public RecordManagement()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.panel_submenu = new System.Windows.Forms.Panel();
            this.button_statisticsObservation = new System.Windows.Forms.RadioButton();
            this.button_resultOfObservation = new System.Windows.Forms.RadioButton();
            this.button_airplaneWarning = new System.Windows.Forms.RadioButton();
            this.button_resultOfGroundCal = new System.Windows.Forms.RadioButton();
            this.button_resultOfStarCal = new System.Windows.Forms.RadioButton();
            this.button_satellite = new System.Windows.Forms.RadioButton();
            this.button_starCatalog = new System.Windows.Forms.RadioButton();
            this.button_environment = new System.Windows.Forms.RadioButton();
            this.button_faultHistory = new System.Windows.Forms.RadioButton();
            this.button_accessHistory = new System.Windows.Forms.RadioButton();
            this.button_TLECPF = new System.Windows.Forms.RadioButton();
            this.panel_RecordMngMain = new System.Windows.Forms.Panel();
            this.panel_submenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_submenu
            // 
            this.panel_submenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(54)))), ((int)(((byte)(78)))));
            this.panel_submenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_submenu.Controls.Add(this.button_statisticsObservation);
            this.panel_submenu.Controls.Add(this.button_resultOfObservation);
            this.panel_submenu.Controls.Add(this.button_airplaneWarning);
            this.panel_submenu.Controls.Add(this.button_resultOfGroundCal);
            this.panel_submenu.Controls.Add(this.button_resultOfStarCal);
            this.panel_submenu.Controls.Add(this.button_satellite);
            this.panel_submenu.Controls.Add(this.button_starCatalog);
            this.panel_submenu.Controls.Add(this.button_environment);
            this.panel_submenu.Controls.Add(this.button_faultHistory);
            this.panel_submenu.Controls.Add(this.button_accessHistory);
            this.panel_submenu.Controls.Add(this.button_TLECPF);
            this.panel_submenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel_submenu.Location = new System.Drawing.Point(0, 0);
            this.panel_submenu.Name = "panel_submenu";
            this.panel_submenu.Size = new System.Drawing.Size(213, 1939);
            this.panel_submenu.TabIndex = 1;
            // 
            // button_statisticsObservation
            // 
            this.button_statisticsObservation.Appearance = System.Windows.Forms.Appearance.Button;
            this.button_statisticsObservation.BackColor = System.Drawing.Color.AliceBlue;
            this.button_statisticsObservation.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_statisticsObservation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_statisticsObservation.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_statisticsObservation.ForeColor = System.Drawing.Color.Black;
            this.button_statisticsObservation.Location = new System.Drawing.Point(4, 791);
            this.button_statisticsObservation.Name = "button_statisticsObservation";
            this.button_statisticsObservation.Size = new System.Drawing.Size(204, 65);
            this.button_statisticsObservation.TabIndex = 0;
            this.button_statisticsObservation.Text = "관측 통계 자료";
            this.button_statisticsObservation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button_statisticsObservation.UseVisualStyleBackColor = false;
            this.button_statisticsObservation.CheckedChanged += new System.EventHandler(this.rBtn_CheckedChanged);
            this.button_statisticsObservation.Click += new System.EventHandler(this.menu_button_Click);
            // 
            // button_resultOfObservation
            // 
            this.button_resultOfObservation.Appearance = System.Windows.Forms.Appearance.Button;
            this.button_resultOfObservation.BackColor = System.Drawing.Color.AliceBlue;
            this.button_resultOfObservation.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_resultOfObservation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_resultOfObservation.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_resultOfObservation.ForeColor = System.Drawing.Color.Black;
            this.button_resultOfObservation.Location = new System.Drawing.Point(4, 716);
            this.button_resultOfObservation.Name = "button_resultOfObservation";
            this.button_resultOfObservation.Size = new System.Drawing.Size(204, 65);
            this.button_resultOfObservation.TabIndex = 0;
            this.button_resultOfObservation.Text = "관측결과";
            this.button_resultOfObservation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button_resultOfObservation.UseVisualStyleBackColor = false;
            this.button_resultOfObservation.CheckedChanged += new System.EventHandler(this.rBtn_CheckedChanged);
            this.button_resultOfObservation.Click += new System.EventHandler(this.menu_button_Click);
            // 
            // button_airplaneWarning
            // 
            this.button_airplaneWarning.Appearance = System.Windows.Forms.Appearance.Button;
            this.button_airplaneWarning.BackColor = System.Drawing.Color.AliceBlue;
            this.button_airplaneWarning.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_airplaneWarning.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_airplaneWarning.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_airplaneWarning.ForeColor = System.Drawing.Color.Black;
            this.button_airplaneWarning.Location = new System.Drawing.Point(4, 641);
            this.button_airplaneWarning.Name = "button_airplaneWarning";
            this.button_airplaneWarning.Size = new System.Drawing.Size(204, 65);
            this.button_airplaneWarning.TabIndex = 0;
            this.button_airplaneWarning.Text = "항공기 접근정보 ";
            this.button_airplaneWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button_airplaneWarning.UseVisualStyleBackColor = false;
            this.button_airplaneWarning.CheckedChanged += new System.EventHandler(this.rBtn_CheckedChanged);
            this.button_airplaneWarning.Click += new System.EventHandler(this.menu_button_Click);
            // 
            // button_resultOfGroundCal
            // 
            this.button_resultOfGroundCal.Appearance = System.Windows.Forms.Appearance.Button;
            this.button_resultOfGroundCal.BackColor = System.Drawing.Color.AliceBlue;
            this.button_resultOfGroundCal.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_resultOfGroundCal.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_resultOfGroundCal.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_resultOfGroundCal.ForeColor = System.Drawing.Color.Black;
            this.button_resultOfGroundCal.Location = new System.Drawing.Point(4, 567);
            this.button_resultOfGroundCal.Name = "button_resultOfGroundCal";
            this.button_resultOfGroundCal.Size = new System.Drawing.Size(204, 65);
            this.button_resultOfGroundCal.TabIndex = 0;
            this.button_resultOfGroundCal.Text = "지상 보정 결과";
            this.button_resultOfGroundCal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button_resultOfGroundCal.UseVisualStyleBackColor = false;
            this.button_resultOfGroundCal.CheckedChanged += new System.EventHandler(this.rBtn_CheckedChanged);
            this.button_resultOfGroundCal.Click += new System.EventHandler(this.menu_button_Click);
            // 
            // button_resultOfStarCal
            // 
            this.button_resultOfStarCal.Appearance = System.Windows.Forms.Appearance.Button;
            this.button_resultOfStarCal.BackColor = System.Drawing.Color.AliceBlue;
            this.button_resultOfStarCal.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_resultOfStarCal.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_resultOfStarCal.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_resultOfStarCal.ForeColor = System.Drawing.Color.Black;
            this.button_resultOfStarCal.Location = new System.Drawing.Point(4, 494);
            this.button_resultOfStarCal.Name = "button_resultOfStarCal";
            this.button_resultOfStarCal.Size = new System.Drawing.Size(204, 65);
            this.button_resultOfStarCal.TabIndex = 0;
            this.button_resultOfStarCal.Text = "별 보정 결과";
            this.button_resultOfStarCal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button_resultOfStarCal.UseVisualStyleBackColor = false;
            this.button_resultOfStarCal.CheckedChanged += new System.EventHandler(this.rBtn_CheckedChanged);
            this.button_resultOfStarCal.Click += new System.EventHandler(this.menu_button_Click);
            // 
            // button_satellite
            // 
            this.button_satellite.Appearance = System.Windows.Forms.Appearance.Button;
            this.button_satellite.BackColor = System.Drawing.Color.AliceBlue;
            this.button_satellite.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_satellite.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_satellite.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_satellite.ForeColor = System.Drawing.Color.Black;
            this.button_satellite.Location = new System.Drawing.Point(4, 421);
            this.button_satellite.Name = "button_satellite";
            this.button_satellite.Size = new System.Drawing.Size(204, 65);
            this.button_satellite.TabIndex = 0;
            this.button_satellite.Text = "위성정보";
            this.button_satellite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button_satellite.UseVisualStyleBackColor = false;
            this.button_satellite.CheckedChanged += new System.EventHandler(this.rBtn_CheckedChanged);
            this.button_satellite.Click += new System.EventHandler(this.menu_button_Click);
            // 
            // button_starCatalog
            // 
            this.button_starCatalog.Appearance = System.Windows.Forms.Appearance.Button;
            this.button_starCatalog.BackColor = System.Drawing.Color.AliceBlue;
            this.button_starCatalog.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_starCatalog.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_starCatalog.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_starCatalog.ForeColor = System.Drawing.Color.Black;
            this.button_starCatalog.Location = new System.Drawing.Point(4, 346);
            this.button_starCatalog.Name = "button_starCatalog";
            this.button_starCatalog.Size = new System.Drawing.Size(204, 65);
            this.button_starCatalog.TabIndex = 0;
            this.button_starCatalog.Text = "별 목록";
            this.button_starCatalog.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button_starCatalog.UseVisualStyleBackColor = false;
            this.button_starCatalog.CheckedChanged += new System.EventHandler(this.rBtn_CheckedChanged);
            this.button_starCatalog.Click += new System.EventHandler(this.menu_button_Click);
            // 
            // button_environment
            // 
            this.button_environment.Appearance = System.Windows.Forms.Appearance.Button;
            this.button_environment.BackColor = System.Drawing.Color.AliceBlue;
            this.button_environment.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_environment.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_environment.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_environment.ForeColor = System.Drawing.Color.Black;
            this.button_environment.Location = new System.Drawing.Point(4, 273);
            this.button_environment.Name = "button_environment";
            this.button_environment.Size = new System.Drawing.Size(204, 65);
            this.button_environment.TabIndex = 0;
            this.button_environment.Text = "날씨";
            this.button_environment.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button_environment.UseVisualStyleBackColor = false;
            this.button_environment.CheckedChanged += new System.EventHandler(this.rBtn_CheckedChanged);
            this.button_environment.Click += new System.EventHandler(this.menu_button_Click);
            // 
            // button_faultHistory
            // 
            this.button_faultHistory.Appearance = System.Windows.Forms.Appearance.Button;
            this.button_faultHistory.BackColor = System.Drawing.Color.AliceBlue;
            this.button_faultHistory.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_faultHistory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_faultHistory.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_faultHistory.ForeColor = System.Drawing.Color.Black;
            this.button_faultHistory.Location = new System.Drawing.Point(4, 199);
            this.button_faultHistory.Name = "button_faultHistory";
            this.button_faultHistory.Size = new System.Drawing.Size(204, 65);
            this.button_faultHistory.TabIndex = 0;
            this.button_faultHistory.Text = "고장이력";
            this.button_faultHistory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button_faultHistory.UseVisualStyleBackColor = false;
            this.button_faultHistory.CheckedChanged += new System.EventHandler(this.rBtn_CheckedChanged);
            this.button_faultHistory.Click += new System.EventHandler(this.menu_button_Click);
            // 
            // button_accessHistory
            // 
            this.button_accessHistory.Appearance = System.Windows.Forms.Appearance.Button;
            this.button_accessHistory.BackColor = System.Drawing.Color.AliceBlue;
            this.button_accessHistory.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.button_accessHistory.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_accessHistory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_accessHistory.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_accessHistory.ForeColor = System.Drawing.Color.Black;
            this.button_accessHistory.Location = new System.Drawing.Point(4, 126);
            this.button_accessHistory.Name = "button_accessHistory";
            this.button_accessHistory.Size = new System.Drawing.Size(204, 65);
            this.button_accessHistory.TabIndex = 0;
            this.button_accessHistory.Text = "접속이력";
            this.button_accessHistory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button_accessHistory.UseVisualStyleBackColor = false;
            this.button_accessHistory.CheckedChanged += new System.EventHandler(this.rBtn_CheckedChanged);
            this.button_accessHistory.Click += new System.EventHandler(this.menu_button_Click);
            // 
            // button_TLECPF
            // 
            this.button_TLECPF.Appearance = System.Windows.Forms.Appearance.Button;
            this.button_TLECPF.BackColor = System.Drawing.Color.AliceBlue;
            this.button_TLECPF.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button_TLECPF.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_TLECPF.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_TLECPF.ForeColor = System.Drawing.Color.Black;
            this.button_TLECPF.Location = new System.Drawing.Point(4, 52);
            this.button_TLECPF.Name = "button_TLECPF";
            this.button_TLECPF.Size = new System.Drawing.Size(204, 65);
            this.button_TLECPF.TabIndex = 0;
            this.button_TLECPF.Text = "TLE / CPF";
            this.button_TLECPF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button_TLECPF.UseVisualStyleBackColor = false;
            this.button_TLECPF.CheckedChanged += new System.EventHandler(this.rBtn_CheckedChanged);
            this.button_TLECPF.Click += new System.EventHandler(this.menu_button_Click);
            // 
            // panel_RecordMngMain
            // 
            this.panel_RecordMngMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel_RecordMngMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_RecordMngMain.Location = new System.Drawing.Point(214, 0);
            this.panel_RecordMngMain.Name = "panel_RecordMngMain";
            this.panel_RecordMngMain.Size = new System.Drawing.Size(3495, 2000);
            this.panel_RecordMngMain.TabIndex = 2;
            // 
            // RecordManagement
            // 
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.panel_RecordMngMain);
            this.Controls.Add(this.panel_submenu);
            this.Name = "RecordManagement";
            this.Size = new System.Drawing.Size(3712, 1939);
            this.panel_submenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }
      


        private void menu_button_Click(object sender, EventArgs e)
        {

            Control ctrl = (Control)sender;
            //MessageBox.Show($"{ctrl.Name}");
            panel_RecordMngMain.Controls.Clear();
            
            switch (ctrl.Name)
            {                
                case "button_TLECPF":
                    panel_RecordMngMain.Controls.Add(screen_TLECPF);
                    //Uc_StateIndicator("CSU_System_Monitoring", "READY");
                    break;

                case "button_accessHistory":                    
                    panel_RecordMngMain.Controls.Add(screen_AccessHistory);
                    break;

                case "button_faultHistory":
                    panel_RecordMngMain.Controls.Add(screen_faultHistory);                    
                    break;

                case "button_environment":
                    panel_RecordMngMain.Controls.Add(screen_Environment);                   
                    break;

                case "button_starCatalog":
                    panel_RecordMngMain.Controls.Add(screen_StarCatalog);
                    break;

                case "button_satellite":
                    panel_RecordMngMain.Controls.Add(screen_Satellite);
                    break;

                case "button_resultOfStarCal":
                    panel_RecordMngMain.Controls.Add(screen_ResultOfStarCal);
                    break;

                case "button_resultOfGroundCal":
                    panel_RecordMngMain.Controls.Add(screen_ResultOfGroundCal);
                    break;

                case "button_airplaneWarning":
                    panel_RecordMngMain.Controls.Add(screen_AirplaneWarning);
                    break;

                case "button_resultOfObservation":
                    panel_RecordMngMain.Controls.Add(screen_ResultOfObservation);
                    break;

                case "button_statisticsObservation":
                    panel_RecordMngMain.Controls.Add(screen_StatisticsOfObservation);
                    break;

            }

        }

        void rBtn_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb == null)
            {
                MessageBox.Show("Sender is not a RadioButton");
                return;
            }

            // Ensure that the RadioButton.Checked property
            // changed to true.
            if (rb.Checked)
            {
                // Keep track of the selected RadioButton by saving a reference
                // to it.
                //rb.BackColor = Color.LightSteelBlue;
                rb.BackColor = Color.Goldenrod;
                //rb.BackColor = Color.FromArgb(255, 192, 255);
                //rb.BackColor = Color.FromArgb(0xffc0C5);
            }
            else
            {
                rb.BackColor = Color.AliceBlue;
            }
        }





    }
}
