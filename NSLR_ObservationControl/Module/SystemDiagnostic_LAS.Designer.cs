namespace NSLR_ObservationControl.Module
{
    partial class SystemDiagnostic_LAS
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.led_P_B5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label_OpState = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.led_O_B7 = new System.Windows.Forms.Label();
            this.led_O_B6 = new System.Windows.Forms.Label();
            this.led_O_B5 = new System.Windows.Forms.Label();
            this.led_O_B4 = new System.Windows.Forms.Label();
            this.led_O_B3 = new System.Windows.Forms.Label();
            this.led_O_B2 = new System.Windows.Forms.Label();
            this.led_O_B1 = new System.Windows.Forms.Label();
            this.led_O_B0 = new System.Windows.Forms.Label();
            this.led_P_B4 = new System.Windows.Forms.Label();
            this.led_P_B3 = new System.Windows.Forms.Label();
            this.led_P_B2 = new System.Windows.Forms.Label();
            this.led_P_B1 = new System.Windows.Forms.Label();
            this.led_P_B0 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label_OpMode = new System.Windows.Forms.Label();
            this.label_FireState = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_Mode = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rb_ShutterOpen = new System.Windows.Forms.RadioButton();
            this.rb_ShutterClose = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_LaserMode = new System.Windows.Forms.ComboBox();
            this.cb_LaserOpMode = new System.Windows.Forms.ComboBox();
            this.btn_laserOpState = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_IBIT = new System.Windows.Forms.Button();
            this.btn_PBIT = new System.Windows.Forms.Button();
            this.deb_btn_connection = new System.Windows.Forms.Button();
            this.sat_btn_connection = new System.Windows.Forms.Button();
            this.groupBox7.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBox7.Controls.Add(this.groupBox2);
            this.groupBox7.Controls.Add(this.groupBox1);
            this.groupBox7.Controls.Add(this.deb_btn_connection);
            this.groupBox7.Controls.Add(this.sat_btn_connection);
            this.groupBox7.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox7.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.groupBox7.Location = new System.Drawing.Point(20, 15);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(700, 700);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "  Laser System  ";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.led_P_B5);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.label_OpState);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.led_O_B7);
            this.groupBox2.Controls.Add(this.led_O_B6);
            this.groupBox2.Controls.Add(this.led_O_B5);
            this.groupBox2.Controls.Add(this.led_O_B4);
            this.groupBox2.Controls.Add(this.led_O_B3);
            this.groupBox2.Controls.Add(this.led_O_B2);
            this.groupBox2.Controls.Add(this.led_O_B1);
            this.groupBox2.Controls.Add(this.led_O_B0);
            this.groupBox2.Controls.Add(this.led_P_B4);
            this.groupBox2.Controls.Add(this.led_P_B3);
            this.groupBox2.Controls.Add(this.led_P_B2);
            this.groupBox2.Controls.Add(this.led_P_B1);
            this.groupBox2.Controls.Add(this.led_P_B0);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label_OpMode);
            this.groupBox2.Controls.Add(this.label_FireState);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label_Mode);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Location = new System.Drawing.Point(26, 85);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(648, 210);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "상태정보";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label10.Location = new System.Drawing.Point(469, 156);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(27, 16);
            this.label10.TabIndex = 19;
            this.label10.Text = "B5";
            // 
            // led_P_B5
            // 
            this.led_P_B5.AutoSize = true;
            this.led_P_B5.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_P_B5.Location = new System.Drawing.Point(468, 126);
            this.led_P_B5.Name = "led_P_B5";
            this.led_P_B5.Size = new System.Drawing.Size(30, 20);
            this.led_P_B5.TabIndex = 18;
            this.led_P_B5.Text = "●";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label8.Location = new System.Drawing.Point(257, 157);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(158, 16);
            this.label8.TabIndex = 17;
            this.label8.Text = "※ 1 : Red : 고장   ";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label20.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label20.Location = new System.Drawing.Point(418, 157);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(28, 16);
            this.label20.TabIndex = 17;
            this.label20.Text = "B4";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label19.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label19.Location = new System.Drawing.Point(220, 157);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(27, 16);
            this.label19.TabIndex = 17;
            this.label19.Text = "B0";
            // 
            // label_OpState
            // 
            this.label_OpState.AutoSize = true;
            this.label_OpState.Font = new System.Drawing.Font("굴림", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_OpState.ForeColor = System.Drawing.Color.Lime;
            this.label_OpState.Location = new System.Drawing.Point(494, 85);
            this.label_OpState.Name = "label_OpState";
            this.label_OpState.Size = new System.Drawing.Size(46, 23);
            this.label_OpState.TabIndex = 16;
            this.label_OpState.Text = "---";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(309, 85);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(101, 20);
            this.label18.TabIndex = 15;
            this.label18.Text = "운용 상태";
            // 
            // led_O_B7
            // 
            this.led_O_B7.AutoSize = true;
            this.led_O_B7.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_O_B7.Location = new System.Drawing.Point(570, 179);
            this.led_O_B7.Name = "led_O_B7";
            this.led_O_B7.Size = new System.Drawing.Size(30, 20);
            this.led_O_B7.TabIndex = 14;
            this.led_O_B7.Text = "●";
            // 
            // led_O_B6
            // 
            this.led_O_B6.AutoSize = true;
            this.led_O_B6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_O_B6.Location = new System.Drawing.Point(518, 179);
            this.led_O_B6.Name = "led_O_B6";
            this.led_O_B6.Size = new System.Drawing.Size(30, 20);
            this.led_O_B6.TabIndex = 14;
            this.led_O_B6.Text = "●";
            // 
            // led_O_B5
            // 
            this.led_O_B5.AutoSize = true;
            this.led_O_B5.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_O_B5.Location = new System.Drawing.Point(466, 179);
            this.led_O_B5.Name = "led_O_B5";
            this.led_O_B5.Size = new System.Drawing.Size(30, 20);
            this.led_O_B5.TabIndex = 14;
            this.led_O_B5.Text = "●";
            // 
            // led_O_B4
            // 
            this.led_O_B4.AutoSize = true;
            this.led_O_B4.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_O_B4.Location = new System.Drawing.Point(417, 179);
            this.led_O_B4.Name = "led_O_B4";
            this.led_O_B4.Size = new System.Drawing.Size(30, 20);
            this.led_O_B4.TabIndex = 14;
            this.led_O_B4.Text = "●";
            // 
            // led_O_B3
            // 
            this.led_O_B3.AutoSize = true;
            this.led_O_B3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_O_B3.Location = new System.Drawing.Point(366, 179);
            this.led_O_B3.Name = "led_O_B3";
            this.led_O_B3.Size = new System.Drawing.Size(30, 20);
            this.led_O_B3.TabIndex = 13;
            this.led_O_B3.Text = "●";
            // 
            // led_O_B2
            // 
            this.led_O_B2.AutoSize = true;
            this.led_O_B2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_O_B2.Location = new System.Drawing.Point(316, 179);
            this.led_O_B2.Name = "led_O_B2";
            this.led_O_B2.Size = new System.Drawing.Size(30, 20);
            this.led_O_B2.TabIndex = 12;
            this.led_O_B2.Text = "●";
            // 
            // led_O_B1
            // 
            this.led_O_B1.AutoSize = true;
            this.led_O_B1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_O_B1.Location = new System.Drawing.Point(263, 179);
            this.led_O_B1.Name = "led_O_B1";
            this.led_O_B1.Size = new System.Drawing.Size(30, 20);
            this.led_O_B1.TabIndex = 11;
            this.led_O_B1.Text = "●";
            // 
            // led_O_B0
            // 
            this.led_O_B0.AutoSize = true;
            this.led_O_B0.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_O_B0.Location = new System.Drawing.Point(217, 179);
            this.led_O_B0.Name = "led_O_B0";
            this.led_O_B0.Size = new System.Drawing.Size(30, 20);
            this.led_O_B0.TabIndex = 10;
            this.led_O_B0.Text = "●";
            // 
            // led_P_B4
            // 
            this.led_P_B4.AutoSize = true;
            this.led_P_B4.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_P_B4.Location = new System.Drawing.Point(417, 127);
            this.led_P_B4.Name = "led_P_B4";
            this.led_P_B4.Size = new System.Drawing.Size(30, 20);
            this.led_P_B4.TabIndex = 9;
            this.led_P_B4.Text = "●";
            // 
            // led_P_B3
            // 
            this.led_P_B3.AutoSize = true;
            this.led_P_B3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_P_B3.Location = new System.Drawing.Point(366, 127);
            this.led_P_B3.Name = "led_P_B3";
            this.led_P_B3.Size = new System.Drawing.Size(30, 20);
            this.led_P_B3.TabIndex = 8;
            this.led_P_B3.Text = "●";
            // 
            // led_P_B2
            // 
            this.led_P_B2.AutoSize = true;
            this.led_P_B2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_P_B2.Location = new System.Drawing.Point(315, 127);
            this.led_P_B2.Name = "led_P_B2";
            this.led_P_B2.Size = new System.Drawing.Size(30, 20);
            this.led_P_B2.TabIndex = 7;
            this.led_P_B2.Text = "●";
            // 
            // led_P_B1
            // 
            this.led_P_B1.AutoSize = true;
            this.led_P_B1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_P_B1.Location = new System.Drawing.Point(265, 127);
            this.led_P_B1.Name = "led_P_B1";
            this.led_P_B1.Size = new System.Drawing.Size(30, 20);
            this.led_P_B1.TabIndex = 6;
            this.led_P_B1.Text = "●";
            // 
            // led_P_B0
            // 
            this.led_P_B0.AutoSize = true;
            this.led_P_B0.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_P_B0.Location = new System.Drawing.Point(217, 127);
            this.led_P_B0.Name = "led_P_B0";
            this.led_P_B0.Size = new System.Drawing.Size(30, 20);
            this.led_P_B0.TabIndex = 5;
            this.led_P_B0.Text = "●";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(69, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 20);
            this.label7.TabIndex = 4;
            this.label7.Text = "점검(동작)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(69, 127);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 20);
            this.label6.TabIndex = 3;
            this.label6.Text = "점검(전원)";
            // 
            // label_OpMode
            // 
            this.label_OpMode.AutoSize = true;
            this.label_OpMode.Font = new System.Drawing.Font("굴림", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_OpMode.ForeColor = System.Drawing.Color.Lime;
            this.label_OpMode.Location = new System.Drawing.Point(179, 40);
            this.label_OpMode.Name = "label_OpMode";
            this.label_OpMode.Size = new System.Drawing.Size(46, 23);
            this.label_OpMode.TabIndex = 2;
            this.label_OpMode.Text = "---";
            // 
            // label_FireState
            // 
            this.label_FireState.AutoSize = true;
            this.label_FireState.Font = new System.Drawing.Font("굴림", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_FireState.ForeColor = System.Drawing.Color.Lime;
            this.label_FireState.Location = new System.Drawing.Point(179, 85);
            this.label_FireState.Name = "label_FireState";
            this.label_FireState.Size = new System.Drawing.Size(46, 23);
            this.label_FireState.TabIndex = 2;
            this.label_FireState.Text = "---";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(69, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "조사상태";
            // 
            // label_Mode
            // 
            this.label_Mode.AutoSize = true;
            this.label_Mode.Font = new System.Drawing.Font("굴림", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Mode.ForeColor = System.Drawing.Color.Lime;
            this.label_Mode.Location = new System.Drawing.Point(419, 40);
            this.label_Mode.Name = "label_Mode";
            this.label_Mode.Size = new System.Drawing.Size(46, 23);
            this.label_Mode.TabIndex = 0;
            this.label_Mode.Text = "---";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(311, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "모드상태";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(69, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "운용모드";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.groupBox1.Controls.Add(this.rb_ShutterOpen);
            this.groupBox1.Controls.Add(this.rb_ShutterClose);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.cb_LaserMode);
            this.groupBox1.Controls.Add(this.cb_LaserOpMode);
            this.groupBox1.Controls.Add(this.btn_laserOpState);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btn_IBIT);
            this.groupBox1.Controls.Add(this.btn_PBIT);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Location = new System.Drawing.Point(26, 319);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(648, 360);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "  제어명령  ";
            // 
            // rb_ShutterOpen
            // 
            this.rb_ShutterOpen.Appearance = System.Windows.Forms.Appearance.Button;
            this.rb_ShutterOpen.BackColor = System.Drawing.Color.LightSlateGray;
            this.rb_ShutterOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.rb_ShutterOpen.ForeColor = System.Drawing.Color.GreenYellow;
            this.rb_ShutterOpen.Location = new System.Drawing.Point(69, 157);
            this.rb_ShutterOpen.Name = "rb_ShutterOpen";
            this.rb_ShutterOpen.Size = new System.Drawing.Size(158, 42);
            this.rb_ShutterOpen.TabIndex = 42;
            this.rb_ShutterOpen.TabStop = true;
            this.rb_ShutterOpen.Text = "셔터열기";
            this.rb_ShutterOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rb_ShutterOpen.UseVisualStyleBackColor = false;
            this.rb_ShutterOpen.CheckedChanged += new System.EventHandler(this.rb_ShutterOpen_CheckedChanged);
            // 
            // rb_ShutterClose
            // 
            this.rb_ShutterClose.Appearance = System.Windows.Forms.Appearance.Button;
            this.rb_ShutterClose.BackColor = System.Drawing.Color.LightSlateGray;
            this.rb_ShutterClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold);
            this.rb_ShutterClose.ForeColor = System.Drawing.Color.GreenYellow;
            this.rb_ShutterClose.Location = new System.Drawing.Point(249, 157);
            this.rb_ShutterClose.Name = "rb_ShutterClose";
            this.rb_ShutterClose.Size = new System.Drawing.Size(158, 42);
            this.rb_ShutterClose.TabIndex = 42;
            this.rb_ShutterClose.TabStop = true;
            this.rb_ShutterClose.Text = "셔터닫기";
            this.rb_ShutterClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rb_ShutterClose.UseVisualStyleBackColor = false;
            this.rb_ShutterClose.CheckedChanged += new System.EventHandler(this.rb_ShutterClose_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Location = new System.Drawing.Point(59, 236);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(536, 10);
            this.panel1.TabIndex = 41;
            // 
            // cb_LaserMode
            // 
            this.cb_LaserMode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_LaserMode.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.cb_LaserMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_LaserMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_LaserMode.Font = new System.Drawing.Font("굴림", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_LaserMode.ForeColor = System.Drawing.SystemColors.Info;
            this.cb_LaserMode.FormattingEnabled = true;
            this.cb_LaserMode.Items.AddRange(new object[] {
            "0x00 레이저 정렬모드(저에너지)",
            "0x01 인공위성추적모드(고에너지)"});
            this.cb_LaserMode.Location = new System.Drawing.Point(255, 87);
            this.cb_LaserMode.Name = "cb_LaserMode";
            this.cb_LaserMode.Size = new System.Drawing.Size(333, 29);
            this.cb_LaserMode.TabIndex = 40;
            this.cb_LaserMode.SelectedIndexChanged += new System.EventHandler(this.cb_LaserMode_SelectedIndexChanged);
            // 
            // cb_LaserOpMode
            // 
            this.cb_LaserOpMode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_LaserOpMode.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.cb_LaserOpMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_LaserOpMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_LaserOpMode.Font = new System.Drawing.Font("굴림", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_LaserOpMode.ForeColor = System.Drawing.SystemColors.Info;
            this.cb_LaserOpMode.FormattingEnabled = true;
            this.cb_LaserOpMode.Items.AddRange(new object[] {
            "0x00 NA",
            "0x01 초기/종료 상태",
            "0x02 준비상태",
            "0x03 운용상태",
            "0x04 점검상태",
            "0x05 안전상태"});
            this.cb_LaserOpMode.Location = new System.Drawing.Point(65, 87);
            this.cb_LaserOpMode.Name = "cb_LaserOpMode";
            this.cb_LaserOpMode.Size = new System.Drawing.Size(161, 29);
            this.cb_LaserOpMode.TabIndex = 40;
            this.cb_LaserOpMode.SelectedIndexChanged += new System.EventHandler(this.cb_LaserOpMode_SelectedIndexChanged);
            // 
            // btn_laserOpState
            // 
            this.btn_laserOpState.BackColor = System.Drawing.Color.LightSlateGray;
            this.btn_laserOpState.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_laserOpState.FlatAppearance.BorderSize = 2;
            this.btn_laserOpState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_laserOpState.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_laserOpState.ForeColor = System.Drawing.Color.GreenYellow;
            this.btn_laserOpState.Location = new System.Drawing.Point(430, 157);
            this.btn_laserOpState.Name = "btn_laserOpState";
            this.btn_laserOpState.Size = new System.Drawing.Size(158, 42);
            this.btn_laserOpState.TabIndex = 39;
            this.btn_laserOpState.Text = "운용 상태";
            this.btn_laserOpState.UseVisualStyleBackColor = false;
            this.btn_laserOpState.Click += new System.EventHandler(this.btn_laserOpState_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(255, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(333, 34);
            this.label2.TabIndex = 36;
            this.label2.Text = "       레이저 모드         ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(65, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 34);
            this.label1.TabIndex = 36;
            this.label1.Text = "     운용모드    ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_IBIT
            // 
            this.btn_IBIT.BackColor = System.Drawing.Color.Indigo;
            this.btn_IBIT.Enabled = false;
            this.btn_IBIT.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_IBIT.FlatAppearance.BorderSize = 3;
            this.btn_IBIT.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_IBIT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btn_IBIT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_IBIT.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_IBIT.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_IBIT.Location = new System.Drawing.Point(334, 266);
            this.btn_IBIT.Name = "btn_IBIT";
            this.btn_IBIT.Size = new System.Drawing.Size(254, 64);
            this.btn_IBIT.TabIndex = 35;
            this.btn_IBIT.Text = "IBIT";
            this.btn_IBIT.UseVisualStyleBackColor = false;
            this.btn_IBIT.Click += new System.EventHandler(this.btn_IBIT_Click);
            // 
            // btn_PBIT
            // 
            this.btn_PBIT.BackColor = System.Drawing.Color.Indigo;
            this.btn_PBIT.Enabled = false;
            this.btn_PBIT.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_PBIT.FlatAppearance.BorderSize = 3;
            this.btn_PBIT.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_PBIT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btn_PBIT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_PBIT.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_PBIT.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_PBIT.Location = new System.Drawing.Point(64, 266);
            this.btn_PBIT.Name = "btn_PBIT";
            this.btn_PBIT.Size = new System.Drawing.Size(241, 64);
            this.btn_PBIT.TabIndex = 35;
            this.btn_PBIT.Text = "PBIT";
            this.btn_PBIT.UseVisualStyleBackColor = false;
            this.btn_PBIT.Click += new System.EventHandler(this.btn_PBIT_Click);
            // 
            // deb_btn_connection
            // 
            this.deb_btn_connection.BackColor = System.Drawing.Color.DarkGray;
            this.deb_btn_connection.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deb_btn_connection.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.deb_btn_connection.Location = new System.Drawing.Point(463, 29);
            this.deb_btn_connection.Name = "deb_btn_connection";
            this.deb_btn_connection.Size = new System.Drawing.Size(211, 43);
            this.deb_btn_connection.TabIndex = 2;
            this.deb_btn_connection.Text = "DEB LAS 연결";
            this.deb_btn_connection.UseVisualStyleBackColor = false;
            // 
            // sat_btn_connection
            // 
            this.sat_btn_connection.BackColor = System.Drawing.Color.DarkGray;
            this.sat_btn_connection.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sat_btn_connection.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.sat_btn_connection.Location = new System.Drawing.Point(24, 29);
            this.sat_btn_connection.Name = "sat_btn_connection";
            this.sat_btn_connection.Size = new System.Drawing.Size(211, 43);
            this.sat_btn_connection.TabIndex = 2;
            this.sat_btn_connection.Text = "SAT LAS 연결";
            this.sat_btn_connection.UseVisualStyleBackColor = false;
            // 
            // SystemDiagnostic_LAS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.groupBox7);
            this.DoubleBuffered = true;
            this.Name = "SystemDiagnostic_LAS";
            this.Size = new System.Drawing.Size(723, 731);
            this.groupBox7.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button sat_btn_connection;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_IBIT;
        private System.Windows.Forms.Button btn_PBIT;        
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_laserOpState;
        private System.Windows.Forms.ComboBox cb_LaserOpMode;
        private System.Windows.Forms.ComboBox cb_LaserMode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label_Mode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_FireState;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label led_P_B4;
        private System.Windows.Forms.Label led_P_B3;
        private System.Windows.Forms.Label led_P_B2;
        private System.Windows.Forms.Label led_P_B1;
        private System.Windows.Forms.Label led_P_B0;
        private System.Windows.Forms.Label led_O_B5;
        private System.Windows.Forms.Label led_O_B4;
        private System.Windows.Forms.Label led_O_B3;
        private System.Windows.Forms.Label led_O_B2;
        private System.Windows.Forms.Label led_O_B1;
        private System.Windows.Forms.Label led_O_B0;
        private System.Windows.Forms.Label label_OpState;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label led_O_B7;
        private System.Windows.Forms.Label led_O_B6;
        private System.Windows.Forms.Button deb_btn_connection;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label led_P_B5;
        private System.Windows.Forms.RadioButton rb_ShutterOpen;
        private System.Windows.Forms.RadioButton rb_ShutterClose;
        private System.Windows.Forms.Label label_OpMode;
    }
}
