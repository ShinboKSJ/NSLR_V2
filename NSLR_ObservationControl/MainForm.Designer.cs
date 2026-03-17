namespace NSLR_ObservationControl
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_Title = new System.Windows.Forms.Label();
            this.panelBottomBar = new System.Windows.Forms.Panel();
            this.label_WaveLengthE = new System.Windows.Forms.Label();
            this.OpMsg = new System.Windows.Forms.Label();
            this.panel_Title = new System.Windows.Forms.Panel();
            this.button_FormMin = new System.Windows.Forms.Button();
            this.button_FormClose = new System.Windows.Forms.Button();
            this.panel_MainMenu = new System.Windows.Forms.Panel();
            this.radioButton_SystemDiagnostic = new System.Windows.Forms.RadioButton();
            this.radioButton_RecordManagement = new System.Windows.Forms.RadioButton();
            this.radioButton_UserManagemnet = new System.Windows.Forms.RadioButton();
            this.radioButton_ConfigSetting = new System.Windows.Forms.RadioButton();
            this.radioButton_StarCalibration = new System.Windows.Forms.RadioButton();
            this.radioButton_GroundCalibration = new System.Windows.Forms.RadioButton();
            this.radioButton_Ranging = new System.Windows.Forms.RadioButton();
            this.panel_Main = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox_Laser = new System.Windows.Forms.PictureBox();
            this.buttonSAT = new System.Windows.Forms.Button();
            this.buttonDEB = new System.Windows.Forms.Button();
            this.displaySchedule_button = new System.Windows.Forms.Button();
            this.downloadCPF_button = new System.Windows.Forms.Button();
            this.panel_Mode = new System.Windows.Forms.Panel();
            this.label_Mode = new System.Windows.Forms.Label();
            this.panel_State = new System.Windows.Forms.Panel();
            this.PauseIndicator = new System.Windows.Forms.Label();
            this.label_State = new System.Windows.Forms.Label();
            this.button_systemSetting = new System.Windows.Forms.Button();
            this.EmergencyStop = new System.Windows.Forms.Button();
            this.time1 = new NSLR_ObservationControl.Module.TIME();
            this.time2 = new NSLR_ObservationControl.Module.TIME();
            this.panelBottomBar.SuspendLayout();
            this.panel_Title.SuspendLayout();
            this.panel_MainMenu.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Laser)).BeginInit();
            this.panel_Mode.SuspendLayout();
            this.panel_State.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_Title
            // 
            this.label_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.label_Title.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Title.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_Title.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Title.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Title.ForeColor = System.Drawing.Color.Transparent;
            this.label_Title.Location = new System.Drawing.Point(0, 0);
            this.label_Title.Margin = new System.Windows.Forms.Padding(0);
            this.label_Title.Name = "label_Title";
            this.label_Title.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.label_Title.Size = new System.Drawing.Size(3840, 35);
            this.label_Title.TabIndex = 0;
            this.label_Title.Text = "NSLR 운용 소프트웨어 - 관리자";
            this.label_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelBottomBar
            // 
            this.panelBottomBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(63)))));
            this.panelBottomBar.Controls.Add(this.label_WaveLengthE);
            this.panelBottomBar.Controls.Add(this.OpMsg);
            this.panelBottomBar.Location = new System.Drawing.Point(0, 2105);
            this.panelBottomBar.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottomBar.Name = "panelBottomBar";
            this.panelBottomBar.Size = new System.Drawing.Size(3840, 54);
            this.panelBottomBar.TabIndex = 1;
            // 
            // label_WaveLengthE
            // 
            this.label_WaveLengthE.AutoSize = true;
            this.label_WaveLengthE.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_WaveLengthE.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label_WaveLengthE.Location = new System.Drawing.Point(3562, 14);
            this.label_WaveLengthE.Name = "label_WaveLengthE";
            this.label_WaveLengthE.Size = new System.Drawing.Size(193, 31);
            this.label_WaveLengthE.TabIndex = 1;
            this.label_WaveLengthE.Text = "파장변환기 E 레벨";
            // 
            // OpMsg
            // 
            this.OpMsg.AutoSize = true;
            this.OpMsg.BackColor = System.Drawing.Color.Transparent;
            this.OpMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpMsg.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.OpMsg.Location = new System.Drawing.Point(140, 7);
            this.OpMsg.Name = "OpMsg";
            this.OpMsg.Size = new System.Drawing.Size(445, 39);
            this.OpMsg.TabIndex = 0;
            this.OpMsg.Text = "Operation 정보를 나타냅니다.    ";
            this.OpMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel_Title
            // 
            this.panel_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.panel_Title.Controls.Add(this.button_FormMin);
            this.panel_Title.Controls.Add(this.button_FormClose);
            this.panel_Title.Controls.Add(this.label_Title);
            this.panel_Title.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_Title.Location = new System.Drawing.Point(0, 0);
            this.panel_Title.Margin = new System.Windows.Forms.Padding(0);
            this.panel_Title.Name = "panel_Title";
            this.panel_Title.Size = new System.Drawing.Size(3840, 35);
            this.panel_Title.TabIndex = 2;
            this.panel_Title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_Title_MouseDown);
            this.panel_Title.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_Title_MouseMove);
            this.panel_Title.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel_Title_MouseUp);
            // 
            // button_FormMin
            // 
            this.button_FormMin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.button_FormMin.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_FormMin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.button_FormMin.FlatAppearance.BorderSize = 0;
            this.button_FormMin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.button_FormMin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.IndianRed;
            this.button_FormMin.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_FormMin.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.button_FormMin.Location = new System.Drawing.Point(3742, 0);
            this.button_FormMin.Margin = new System.Windows.Forms.Padding(0);
            this.button_FormMin.Name = "button_FormMin";
            this.button_FormMin.Size = new System.Drawing.Size(49, 35);
            this.button_FormMin.TabIndex = 3;
            this.button_FormMin.Text = "―";
            this.button_FormMin.UseVisualStyleBackColor = false;
            this.button_FormMin.Click += new System.EventHandler(this.button_FormMin_Click);
            // 
            // button_FormClose
            // 
            this.button_FormClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(10)))), ((int)(((byte)(10)))), ((int)(((byte)(10)))));
            this.button_FormClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_FormClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.button_FormClose.FlatAppearance.BorderSize = 0;
            this.button_FormClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(58)))), ((int)(((byte)(52)))));
            this.button_FormClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.button_FormClose.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_FormClose.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.button_FormClose.Location = new System.Drawing.Point(3791, 0);
            this.button_FormClose.Margin = new System.Windows.Forms.Padding(0);
            this.button_FormClose.Name = "button_FormClose";
            this.button_FormClose.Size = new System.Drawing.Size(49, 35);
            this.button_FormClose.TabIndex = 1;
            this.button_FormClose.Text = "X";
            this.button_FormClose.UseVisualStyleBackColor = false;
            this.button_FormClose.Click += new System.EventHandler(this.button_FormClose_Click);
            // 
            // panel_MainMenu
            // 
            this.panel_MainMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(63)))));
            this.panel_MainMenu.Controls.Add(this.radioButton_SystemDiagnostic);
            this.panel_MainMenu.Controls.Add(this.radioButton_RecordManagement);
            this.panel_MainMenu.Controls.Add(this.radioButton_UserManagemnet);
            this.panel_MainMenu.Controls.Add(this.radioButton_ConfigSetting);
            this.panel_MainMenu.Controls.Add(this.radioButton_StarCalibration);
            this.panel_MainMenu.Controls.Add(this.radioButton_GroundCalibration);
            this.panel_MainMenu.Controls.Add(this.radioButton_Ranging);
            this.panel_MainMenu.Location = new System.Drawing.Point(0, 129);
            this.panel_MainMenu.Margin = new System.Windows.Forms.Padding(0);
            this.panel_MainMenu.Name = "panel_MainMenu";
            this.panel_MainMenu.Size = new System.Drawing.Size(127, 1974);
            this.panel_MainMenu.TabIndex = 59;
            // 
            // radioButton_SystemDiagnostic
            // 
            this.radioButton_SystemDiagnostic.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_SystemDiagnostic.BackColor = System.Drawing.Color.LightGray;
            this.radioButton_SystemDiagnostic.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.radioButton_SystemDiagnostic.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.radioButton_SystemDiagnostic.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.radioButton_SystemDiagnostic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton_SystemDiagnostic.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_SystemDiagnostic.ForeColor = System.Drawing.Color.Black;
            this.radioButton_SystemDiagnostic.Image = global::NSLR_ObservationControl.Properties.Resources.icons8_diagnostic_100_1;
            this.radioButton_SystemDiagnostic.Location = new System.Drawing.Point(2, 1006);
            this.radioButton_SystemDiagnostic.Name = "radioButton_SystemDiagnostic";
            this.radioButton_SystemDiagnostic.Size = new System.Drawing.Size(127, 153);
            this.radioButton_SystemDiagnostic.TabIndex = 67;
            this.radioButton_SystemDiagnostic.TabStop = true;
            this.radioButton_SystemDiagnostic.Text = "System Diagnostic";
            this.radioButton_SystemDiagnostic.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.radioButton_SystemDiagnostic.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.radioButton_SystemDiagnostic.UseVisualStyleBackColor = false;
            this.radioButton_SystemDiagnostic.CheckedChanged += new System.EventHandler(this.radioButton_SystemDiagnostic_CheckedChanged);
            // 
            // radioButton_RecordManagement
            // 
            this.radioButton_RecordManagement.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_RecordManagement.BackColor = System.Drawing.Color.LightGray;
            this.radioButton_RecordManagement.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.radioButton_RecordManagement.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.radioButton_RecordManagement.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.radioButton_RecordManagement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton_RecordManagement.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_RecordManagement.ForeColor = System.Drawing.Color.Black;
            this.radioButton_RecordManagement.Image = global::NSLR_ObservationControl.Properties.Resources.record_managemnet_100;
            this.radioButton_RecordManagement.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.radioButton_RecordManagement.Location = new System.Drawing.Point(1, 839);
            this.radioButton_RecordManagement.Name = "radioButton_RecordManagement";
            this.radioButton_RecordManagement.Size = new System.Drawing.Size(127, 153);
            this.radioButton_RecordManagement.TabIndex = 66;
            this.radioButton_RecordManagement.TabStop = true;
            this.radioButton_RecordManagement.Text = "Record Managemnt";
            this.radioButton_RecordManagement.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.radioButton_RecordManagement.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.radioButton_RecordManagement.UseVisualStyleBackColor = false;
            this.radioButton_RecordManagement.CheckedChanged += new System.EventHandler(this.radioButton_RecordManagement_CheckedChanged);
            // 
            // radioButton_UserManagemnet
            // 
            this.radioButton_UserManagemnet.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_UserManagemnet.BackColor = System.Drawing.Color.LightGray;
            this.radioButton_UserManagemnet.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.radioButton_UserManagemnet.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.radioButton_UserManagemnet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton_UserManagemnet.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_UserManagemnet.ForeColor = System.Drawing.Color.Black;
            this.radioButton_UserManagemnet.Image = global::NSLR_ObservationControl.Properties.Resources.user_management_100;
            this.radioButton_UserManagemnet.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.radioButton_UserManagemnet.Location = new System.Drawing.Point(2, 672);
            this.radioButton_UserManagemnet.Name = "radioButton_UserManagemnet";
            this.radioButton_UserManagemnet.Size = new System.Drawing.Size(127, 153);
            this.radioButton_UserManagemnet.TabIndex = 65;
            this.radioButton_UserManagemnet.TabStop = true;
            this.radioButton_UserManagemnet.Text = "User Managemnt";
            this.radioButton_UserManagemnet.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.radioButton_UserManagemnet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.radioButton_UserManagemnet.UseVisualStyleBackColor = false;
            this.radioButton_UserManagemnet.CheckedChanged += new System.EventHandler(this.radioButton_UserManagemnet_CheckedChanged);
            // 
            // radioButton_ConfigSetting
            // 
            this.radioButton_ConfigSetting.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_ConfigSetting.BackColor = System.Drawing.Color.LightGray;
            this.radioButton_ConfigSetting.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.radioButton_ConfigSetting.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.radioButton_ConfigSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton_ConfigSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_ConfigSetting.ForeColor = System.Drawing.Color.Black;
            this.radioButton_ConfigSetting.Image = global::NSLR_ObservationControl.Properties.Resources.configurationSetting_100;
            this.radioButton_ConfigSetting.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.radioButton_ConfigSetting.Location = new System.Drawing.Point(2, 507);
            this.radioButton_ConfigSetting.Name = "radioButton_ConfigSetting";
            this.radioButton_ConfigSetting.Size = new System.Drawing.Size(127, 153);
            this.radioButton_ConfigSetting.TabIndex = 64;
            this.radioButton_ConfigSetting.TabStop = true;
            this.radioButton_ConfigSetting.Text = "Config.  Setting";
            this.radioButton_ConfigSetting.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.radioButton_ConfigSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.radioButton_ConfigSetting.UseVisualStyleBackColor = false;
            this.radioButton_ConfigSetting.CheckedChanged += new System.EventHandler(this.radioButton_ConfigSetting_CheckedChanged);
            // 
            // radioButton_StarCalibration
            // 
            this.radioButton_StarCalibration.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_StarCalibration.BackColor = System.Drawing.Color.LightGray;
            this.radioButton_StarCalibration.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.radioButton_StarCalibration.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.radioButton_StarCalibration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton_StarCalibration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_StarCalibration.ForeColor = System.Drawing.Color.Black;
            this.radioButton_StarCalibration.Image = global::NSLR_ObservationControl.Properties.Resources.icons8_starCalibration_100;
            this.radioButton_StarCalibration.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.radioButton_StarCalibration.Location = new System.Drawing.Point(2, 343);
            this.radioButton_StarCalibration.Name = "radioButton_StarCalibration";
            this.radioButton_StarCalibration.Size = new System.Drawing.Size(127, 153);
            this.radioButton_StarCalibration.TabIndex = 62;
            this.radioButton_StarCalibration.TabStop = true;
            this.radioButton_StarCalibration.Text = "Star Calibration";
            this.radioButton_StarCalibration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton_StarCalibration.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.radioButton_StarCalibration.UseVisualStyleBackColor = false;
            this.radioButton_StarCalibration.CheckedChanged += new System.EventHandler(this.btn_StarCalibration_CheckedChanged);
            // 
            // radioButton_GroundCalibration
            // 
            this.radioButton_GroundCalibration.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_GroundCalibration.BackColor = System.Drawing.Color.LightGray;
            this.radioButton_GroundCalibration.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.radioButton_GroundCalibration.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.radioButton_GroundCalibration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton_GroundCalibration.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_GroundCalibration.ForeColor = System.Drawing.Color.Black;
            this.radioButton_GroundCalibration.Image = global::NSLR_ObservationControl.Properties.Resources.icons8_ranging_100;
            this.radioButton_GroundCalibration.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.radioButton_GroundCalibration.Location = new System.Drawing.Point(2, 178);
            this.radioButton_GroundCalibration.Name = "radioButton_GroundCalibration";
            this.radioButton_GroundCalibration.Size = new System.Drawing.Size(127, 153);
            this.radioButton_GroundCalibration.TabIndex = 62;
            this.radioButton_GroundCalibration.TabStop = true;
            this.radioButton_GroundCalibration.Text = "Ground Calibration";
            this.radioButton_GroundCalibration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton_GroundCalibration.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.radioButton_GroundCalibration.UseVisualStyleBackColor = false;
            this.radioButton_GroundCalibration.CheckedChanged += new System.EventHandler(this.btn_GroundCalibration_CheckedChanged);
            // 
            // radioButton_Ranging
            // 
            this.radioButton_Ranging.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_Ranging.BackColor = System.Drawing.Color.LightGray;
            this.radioButton_Ranging.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.radioButton_Ranging.FlatAppearance.MouseOverBackColor = System.Drawing.Color.OliveDrab;
            this.radioButton_Ranging.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton_Ranging.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_Ranging.ForeColor = System.Drawing.Color.Black;
            this.radioButton_Ranging.Image = global::NSLR_ObservationControl.Properties.Resources.icons8_satellite_100;
            this.radioButton_Ranging.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.radioButton_Ranging.Location = new System.Drawing.Point(2, 13);
            this.radioButton_Ranging.Name = "radioButton_Ranging";
            this.radioButton_Ranging.Size = new System.Drawing.Size(127, 153);
            this.radioButton_Ranging.TabIndex = 62;
            this.radioButton_Ranging.TabStop = true;
            this.radioButton_Ranging.Text = "Ranging";
            this.radioButton_Ranging.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton_Ranging.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.radioButton_Ranging.UseVisualStyleBackColor = false;
            this.radioButton_Ranging.CheckedChanged += new System.EventHandler(this.btn_Raning_CheckedChanged);
            // 
            // panel_Main
            // 
            this.panel_Main.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.panel_Main.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel_Main.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel_Main.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.panel_Main.Location = new System.Drawing.Point(129, 129);
            this.panel_Main.Name = "panel_Main";
            this.panel_Main.Size = new System.Drawing.Size(3711, 1974);
            this.panel_Main.TabIndex = 60;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(63)))));
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox_Laser, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonSAT, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonDEB, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.displaySchedule_button, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.downloadCPF_button, 4, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 35);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(824, 92);
            this.tableLayoutPanel1.TabIndex = 61;
            // 
            // pictureBox_Laser
            // 
            this.pictureBox_Laser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(63)))));
            this.pictureBox_Laser.BackgroundImage = global::NSLR_ObservationControl.Properties.Resources.gif_observertaory;
            this.pictureBox_Laser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox_Laser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_Laser.ErrorImage = null;
            this.pictureBox_Laser.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_Laser.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox_Laser.Name = "pictureBox_Laser";
            this.pictureBox_Laser.Size = new System.Drawing.Size(115, 92);
            this.pictureBox_Laser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Laser.TabIndex = 49;
            this.pictureBox_Laser.TabStop = false;
            this.pictureBox_Laser.Click += new System.EventHandler(this.pictureBox_Laser_Click);
            // 
            // buttonSAT
            // 
            this.buttonSAT.BackColor = System.Drawing.Color.White;
            this.buttonSAT.BackgroundImage = global::NSLR_ObservationControl.Properties.Resources.satellite;
            this.buttonSAT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSAT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSAT.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.buttonSAT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.buttonSAT.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonSAT.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSAT.Location = new System.Drawing.Point(118, 3);
            this.buttonSAT.Name = "buttonSAT";
            this.buttonSAT.Size = new System.Drawing.Size(109, 86);
            this.buttonSAT.TabIndex = 52;
            this.buttonSAT.UseVisualStyleBackColor = false;
            this.buttonSAT.Click += new System.EventHandler(this.buttonSatellite_Click);
            // 
            // buttonDEB
            // 
            this.buttonDEB.BackColor = System.Drawing.Color.White;
            this.buttonDEB.BackgroundImage = global::NSLR_ObservationControl.Properties.Resources.debris;
            this.buttonDEB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonDEB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDEB.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Navy;
            this.buttonDEB.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonDEB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDEB.Location = new System.Drawing.Point(233, 3);
            this.buttonDEB.Name = "buttonDEB";
            this.buttonDEB.Size = new System.Drawing.Size(109, 86);
            this.buttonDEB.TabIndex = 53;
            this.buttonDEB.UseVisualStyleBackColor = false;
            this.buttonDEB.Click += new System.EventHandler(this.buttonDebris_Click);
            // 
            // displaySchedule_button
            // 
            this.displaySchedule_button.BackColor = System.Drawing.Color.MediumOrchid;
            this.displaySchedule_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displaySchedule_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.displaySchedule_button.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displaySchedule_button.Location = new System.Drawing.Point(348, 3);
            this.displaySchedule_button.Name = "displaySchedule_button";
            this.displaySchedule_button.Size = new System.Drawing.Size(232, 86);
            this.displaySchedule_button.TabIndex = 54;
            this.displaySchedule_button.Text = "스케줄러 전시";
            this.displaySchedule_button.UseVisualStyleBackColor = false;
            this.displaySchedule_button.Click += new System.EventHandler(this.displaySchedule_button_Click);
            // 
            // downloadCPF_button
            // 
            this.downloadCPF_button.BackColor = System.Drawing.Color.RoyalBlue;
            this.downloadCPF_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.downloadCPF_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.downloadCPF_button.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadCPF_button.Location = new System.Drawing.Point(586, 3);
            this.downloadCPF_button.Name = "downloadCPF_button";
            this.downloadCPF_button.Size = new System.Drawing.Size(235, 86);
            this.downloadCPF_button.TabIndex = 54;
            this.downloadCPF_button.Text = "CPF Download";
            this.downloadCPF_button.UseVisualStyleBackColor = false;
            this.downloadCPF_button.Click += new System.EventHandler(this.downloadCPF_button_Click);
            // 
            // panel_Mode
            // 
            this.panel_Mode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(63)))));
            this.panel_Mode.Controls.Add(this.label_Mode);
            this.panel_Mode.Location = new System.Drawing.Point(823, 35);
            this.panel_Mode.Name = "panel_Mode";
            this.panel_Mode.Size = new System.Drawing.Size(791, 93);
            this.panel_Mode.TabIndex = 62;
            // 
            // label_Mode
            // 
            this.label_Mode.BackColor = System.Drawing.Color.Transparent;
            this.label_Mode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Mode.Font = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Mode.ForeColor = System.Drawing.Color.DarkOrange;
            this.label_Mode.Location = new System.Drawing.Point(0, 0);
            this.label_Mode.Name = "label_Mode";
            this.label_Mode.Size = new System.Drawing.Size(791, 93);
            this.label_Mode.TabIndex = 0;
            this.label_Mode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Mode.MouseHover += new System.EventHandler(this.label_Mode_MouseHover);
            // 
            // panel_State
            // 
            this.panel_State.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(63)))));
            this.panel_State.Controls.Add(this.PauseIndicator);
            this.panel_State.Controls.Add(this.label_State);
            this.panel_State.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.panel_State.Location = new System.Drawing.Point(1615, 35);
            this.panel_State.Name = "panel_State";
            this.panel_State.Size = new System.Drawing.Size(1342, 93);
            this.panel_State.TabIndex = 63;
            // 
            // PauseIndicator
            // 
            this.PauseIndicator.BackColor = System.Drawing.Color.Lime;
            this.PauseIndicator.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PauseIndicator.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold);
            this.PauseIndicator.ForeColor = System.Drawing.SystemColors.Desktop;
            this.PauseIndicator.Location = new System.Drawing.Point(1027, 1);
            this.PauseIndicator.Margin = new System.Windows.Forms.Padding(0);
            this.PauseIndicator.Name = "PauseIndicator";
            this.PauseIndicator.Size = new System.Drawing.Size(276, 91);
            this.PauseIndicator.TabIndex = 1;
            this.PauseIndicator.Text = "일시정지 해제 ";
            this.PauseIndicator.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.PauseIndicator.Click += new System.EventHandler(this.PauseIndicator_Click);
            // 
            // label_State
            // 
            this.label_State.BackColor = System.Drawing.Color.Transparent;
            this.label_State.Font = new System.Drawing.Font("맑은 고딕", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_State.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label_State.Location = new System.Drawing.Point(15, 17);
            this.label_State.Name = "label_State";
            this.label_State.Size = new System.Drawing.Size(512, 64);
            this.label_State.TabIndex = 1;
            this.label_State.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_systemSetting
            // 
            this.button_systemSetting.BackColor = System.Drawing.Color.LightSteelBlue;
            this.button_systemSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_systemSetting.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button_systemSetting.Image = global::NSLR_ObservationControl.Properties.Resources.icons8_settting100;
            this.button_systemSetting.Location = new System.Drawing.Point(0, 1804);
            this.button_systemSetting.Name = "button_systemSetting";
            this.button_systemSetting.Size = new System.Drawing.Size(127, 166);
            this.button_systemSetting.TabIndex = 63;
            this.button_systemSetting.Text = "System Config.";
            this.button_systemSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.button_systemSetting.UseVisualStyleBackColor = false;
            // 
            // EmergencyStop
            // 
            this.EmergencyStop.BackColor = System.Drawing.Color.Lime;
            this.EmergencyStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.EmergencyStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmergencyStop.ForeColor = System.Drawing.Color.Black;
            this.EmergencyStop.Location = new System.Drawing.Point(2957, 35);
            this.EmergencyStop.Margin = new System.Windows.Forms.Padding(0);
            this.EmergencyStop.Name = "EmergencyStop";
            this.EmergencyStop.Size = new System.Drawing.Size(275, 93);
            this.EmergencyStop.TabIndex = 64;
            this.EmergencyStop.Text = "긴급정지 해제";
            this.EmergencyStop.UseVisualStyleBackColor = false;
            this.EmergencyStop.Click += new System.EventHandler(this.btn_EmergencyStop_Click);
            // 
            // time1
            // 
            this.time1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.time1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(63)))));
            this.time1.Location = new System.Drawing.Point(18441, 35);
            this.time1.Margin = new System.Windows.Forms.Padding(0);
            this.time1.Name = "time1";
            this.time1.Size = new System.Drawing.Size(628, 92);
            this.time1.TabIndex = 48;
            // 
            // time2
            // 
            this.time2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(63)))));
            this.time2.Location = new System.Drawing.Point(3232, 35);
            this.time2.Name = "time2";
            this.time2.Size = new System.Drawing.Size(607, 93);
            this.time2.TabIndex = 65;
            this.time2.MouseHover += new System.EventHandler(this.time2_MouseHover);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1941, 1078);
            this.Controls.Add(this.time2);
            this.Controls.Add(this.EmergencyStop);
            this.Controls.Add(this.panel_State);
            this.Controls.Add(this.time1);
            this.Controls.Add(this.panel_Mode);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel_MainMenu);
            this.Controls.Add(this.panelBottomBar);
            this.Controls.Add(this.panel_Main);
            this.Controls.Add(this.panel_Title);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "0";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.Form_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.panelBottomBar.ResumeLayout(false);
            this.panelBottomBar.PerformLayout();
            this.panel_Title.ResumeLayout(false);
            this.panel_MainMenu.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Laser)).EndInit();
            this.panel_Mode.ResumeLayout(false);
            this.panel_State.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label_Title;
        private System.Windows.Forms.Panel panelBottomBar;
        private System.Windows.Forms.Panel panel_Title;
        private System.Windows.Forms.Button button_FormClose;
        private System.Windows.Forms.Button button_FormMin;
        private Module.TIME time1;
        private System.Windows.Forms.Panel panel_MainMenu;
        private System.Windows.Forms.Panel panel_Main;
        private System.Windows.Forms.PictureBox pictureBox_Laser;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel_Mode;
        private System.Windows.Forms.Panel panel_State;
        private System.Windows.Forms.Label OpMsg;
        private System.Windows.Forms.RadioButton radioButton_StarCalibration;
        private System.Windows.Forms.RadioButton radioButton_GroundCalibration;
        private System.Windows.Forms.RadioButton radioButton_Ranging;
        private System.Windows.Forms.Button button_systemSetting;
        private System.Windows.Forms.Label label_Mode;
        private System.Windows.Forms.Label label_State;
        private System.Windows.Forms.RadioButton radioButton_ConfigSetting;
        private System.Windows.Forms.RadioButton radioButton_RecordManagement;
        private System.Windows.Forms.RadioButton radioButton_UserManagemnet;
        private System.Windows.Forms.RadioButton radioButton_SystemDiagnostic;
        private System.Windows.Forms.Button EmergencyStop;
        private System.Windows.Forms.Button buttonDEB;
        private System.Windows.Forms.Button buttonSAT;
        private Module.TIME time2;
        private System.Windows.Forms.Label PauseIndicator;
        private System.Windows.Forms.Button displaySchedule_button;
        private System.Windows.Forms.Label label_WaveLengthE;
        private System.Windows.Forms.Button downloadCPF_button;
    }
}
