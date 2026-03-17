namespace NSLR_ObservationControl.Module
{
    partial class SystemDiagnostic_RGG_SAT
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
            this.components = new System.ComponentModel.Container();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_connection_rgg = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label_RggCtrl = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label_GateWidth_m = new System.Windows.Forms.Label();
            this.label_GateStartOffset = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.led_P_B4 = new System.Windows.Forms.Label();
            this.led_P_B3 = new System.Windows.Forms.Label();
            this.led_P_B2 = new System.Windows.Forms.Label();
            this.led_P_B1 = new System.Windows.Forms.Label();
            this.led_P_B0 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label_GateStartOffset_m = new System.Windows.Forms.Label();
            this.label_GatePulseWidth = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_RggControl = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.numAvoidSO = new System.Windows.Forms.NumericUpDown();
            this.numAvoidWidth = new System.Windows.Forms.NumericUpDown();
            this.numTOF = new System.Windows.Forms.NumericUpDown();
            this.numUTC = new System.Windows.Forms.NumericUpDown();
            this.rb_RGGctrl_ToF = new System.Windows.Forms.RadioButton();
            this.rb_RGGctrl_Utc = new System.Windows.Forms.RadioButton();
            this.rb_RGGctrl_Gcal = new System.Windows.Forms.RadioButton();
            this.rb_RGGctrl_LUTinit = new System.Windows.Forms.RadioButton();
            this.rb_RGGctrl_LUTsend = new System.Windows.Forms.RadioButton();
            this.rb_RGGctrl_ready = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.numGPW = new System.Windows.Forms.NumericUpDown();
            this.numGPSO = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tranmissionReat_check = new System.Windows.Forms.CheckBox();
            this.btn_CBIT = new System.Windows.Forms.Button();
            this.btn_set_command = new System.Windows.Forms.Button();
            this.btn_IBIT = new System.Windows.Forms.Button();
            this.btn_PBIT = new System.Windows.Forms.Button();
            this.periodicSend_timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAvoidSO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAvoidWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTOF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUTC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGPW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGPSO)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.Transparent;
            this.groupBox5.Controls.Add(this.btn_connection_rgg);
            this.groupBox5.Controls.Add(this.groupBox2);
            this.groupBox5.Controls.Add(this.groupBox9);
            this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.groupBox5.ForeColor = System.Drawing.Color.Orange;
            this.groupBox5.Location = new System.Drawing.Point(19, 13);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(685, 700);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "    ப Range Ragte Gernerator   ";
            // 
            // btn_connection_rgg
            // 
            this.btn_connection_rgg.BackColor = System.Drawing.Color.DarkGray;
            this.btn_connection_rgg.Font = new System.Drawing.Font("Consolas", 18F);
            this.btn_connection_rgg.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btn_connection_rgg.Location = new System.Drawing.Point(469, 31);
            this.btn_connection_rgg.Name = "btn_connection_rgg";
            this.btn_connection_rgg.Size = new System.Drawing.Size(193, 45);
            this.btn_connection_rgg.TabIndex = 2;
            this.btn_connection_rgg.Text = "SAT RGG 연결";
            this.btn_connection_rgg.UseVisualStyleBackColor = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.groupBox2.Controls.Add(this.label_RggCtrl);
            this.groupBox2.Controls.Add(this.label20);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.label_GateWidth_m);
            this.groupBox2.Controls.Add(this.label_GateStartOffset);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.led_P_B4);
            this.groupBox2.Controls.Add(this.led_P_B3);
            this.groupBox2.Controls.Add(this.led_P_B2);
            this.groupBox2.Controls.Add(this.led_P_B1);
            this.groupBox2.Controls.Add(this.led_P_B0);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label_GateStartOffset_m);
            this.groupBox2.Controls.Add(this.label_GatePulseWidth);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label_RggControl);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Location = new System.Drawing.Point(24, 81);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(637, 195);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "상태정보";
            // 
            // label_RggCtrl
            // 
            this.label_RggCtrl.AutoSize = true;
            this.label_RggCtrl.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_RggCtrl.ForeColor = System.Drawing.Color.GreenYellow;
            this.label_RggCtrl.Location = new System.Drawing.Point(45, 91);
            this.label_RggCtrl.Name = "label_RggCtrl";
            this.label_RggCtrl.Size = new System.Drawing.Size(108, 16);
            this.label_RggCtrl.TabIndex = 18;
            this.label_RggCtrl.Text = "RGG Control";
            this.label_RggCtrl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label20.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label20.Location = new System.Drawing.Point(483, 157);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(105, 16);
            this.label20.TabIndex = 17;
            this.label20.Text = "B4_모터상태";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label5.Location = new System.Drawing.Point(371, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 16);
            this.label5.TabIndex = 17;
            this.label5.Text = "B3_모터동기";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label1.Location = new System.Drawing.Point(294, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 16);
            this.label1.TabIndex = 17;
            this.label1.Text = "B2_GPS";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label2.Location = new System.Drawing.Point(208, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "B1_FPGA";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label19.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label19.Location = new System.Drawing.Point(124, 157);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(69, 16);
            this.label19.TabIndex = 17;
            this.label19.Text = "B0_DDR";
            // 
            // label_GateWidth_m
            // 
            this.label_GateWidth_m.AutoSize = true;
            this.label_GateWidth_m.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_GateWidth_m.ForeColor = System.Drawing.Color.GreenYellow;
            this.label_GateWidth_m.Location = new System.Drawing.Point(475, 85);
            this.label_GateWidth_m.Name = "label_GateWidth_m";
            this.label_GateWidth_m.Size = new System.Drawing.Size(33, 25);
            this.label_GateWidth_m.TabIndex = 16;
            this.label_GateWidth_m.Text = "---";
            // 
            // label_GateStartOffset
            // 
            this.label_GateStartOffset.AutoSize = true;
            this.label_GateStartOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_GateStartOffset.ForeColor = System.Drawing.Color.Lime;
            this.label_GateStartOffset.Location = new System.Drawing.Point(384, 40);
            this.label_GateStartOffset.Name = "label_GateStartOffset";
            this.label_GateStartOffset.Size = new System.Drawing.Size(33, 25);
            this.label_GateStartOffset.TabIndex = 16;
            this.label_GateStartOffset.Text = "---";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(176, 85);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(164, 25);
            this.label18.TabIndex = 15;
            this.label18.Text = "Gate Pulse Width";
            // 
            // led_P_B4
            // 
            this.led_P_B4.AutoSize = true;
            this.led_P_B4.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold);
            this.led_P_B4.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_P_B4.Location = new System.Drawing.Point(514, 127);
            this.led_P_B4.Name = "led_P_B4";
            this.led_P_B4.Size = new System.Drawing.Size(30, 20);
            this.led_P_B4.TabIndex = 9;
            this.led_P_B4.Text = "●";
            // 
            // led_P_B3
            // 
            this.led_P_B3.AutoSize = true;
            this.led_P_B3.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold);
            this.led_P_B3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_P_B3.Location = new System.Drawing.Point(399, 127);
            this.led_P_B3.Name = "led_P_B3";
            this.led_P_B3.Size = new System.Drawing.Size(30, 20);
            this.led_P_B3.TabIndex = 8;
            this.led_P_B3.Text = "●";
            // 
            // led_P_B2
            // 
            this.led_P_B2.AutoSize = true;
            this.led_P_B2.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold);
            this.led_P_B2.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_P_B2.Location = new System.Drawing.Point(317, 127);
            this.led_P_B2.Name = "led_P_B2";
            this.led_P_B2.Size = new System.Drawing.Size(30, 20);
            this.led_P_B2.TabIndex = 7;
            this.led_P_B2.Text = "●";
            // 
            // led_P_B1
            // 
            this.led_P_B1.AutoSize = true;
            this.led_P_B1.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold);
            this.led_P_B1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_P_B1.Location = new System.Drawing.Point(236, 127);
            this.led_P_B1.Name = "led_P_B1";
            this.led_P_B1.Size = new System.Drawing.Size(30, 20);
            this.led_P_B1.TabIndex = 6;
            this.led_P_B1.Text = "●";
            // 
            // led_P_B0
            // 
            this.led_P_B0.AutoSize = true;
            this.led_P_B0.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold);
            this.led_P_B0.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.led_P_B0.Location = new System.Drawing.Point(145, 127);
            this.led_P_B0.Name = "led_P_B0";
            this.led_P_B0.Size = new System.Drawing.Size(30, 20);
            this.led_P_B0.TabIndex = 5;
            this.led_P_B0.Text = "●";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(64, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 25);
            this.label6.TabIndex = 3;
            this.label6.Text = "점검";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_GateStartOffset_m
            // 
            this.label_GateStartOffset_m.AutoSize = true;
            this.label_GateStartOffset_m.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_GateStartOffset_m.ForeColor = System.Drawing.Color.GreenYellow;
            this.label_GateStartOffset_m.Location = new System.Drawing.Point(475, 40);
            this.label_GateStartOffset_m.Name = "label_GateStartOffset_m";
            this.label_GateStartOffset_m.Size = new System.Drawing.Size(33, 25);
            this.label_GateStartOffset_m.TabIndex = 2;
            this.label_GateStartOffset_m.Text = "---";
            // 
            // label_GatePulseWidth
            // 
            this.label_GatePulseWidth.AutoSize = true;
            this.label_GatePulseWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_GatePulseWidth.ForeColor = System.Drawing.Color.Lime;
            this.label_GatePulseWidth.Location = new System.Drawing.Point(384, 85);
            this.label_GatePulseWidth.Name = "label_GatePulseWidth";
            this.label_GatePulseWidth.Size = new System.Drawing.Size(33, 25);
            this.label_GatePulseWidth.TabIndex = 2;
            this.label_GatePulseWidth.Text = "---";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(176, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 25);
            this.label4.TabIndex = 1;
            this.label4.Text = "Gate Start Offset";
            // 
            // label_RggControl
            // 
            this.label_RggControl.AutoSize = true;
            this.label_RggControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_RggControl.ForeColor = System.Drawing.Color.Lime;
            this.label_RggControl.Location = new System.Drawing.Point(76, 68);
            this.label_RggControl.Name = "label_RggControl";
            this.label_RggControl.Size = new System.Drawing.Size(33, 25);
            this.label_RggControl.TabIndex = 0;
            this.label_RggControl.Text = "---";
            this.label_RggControl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(51, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "RGG Ctrl";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox9
            // 
            this.groupBox9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.groupBox9.Controls.Add(this.label12);
            this.groupBox9.Controls.Add(this.label13);
            this.groupBox9.Controls.Add(this.numAvoidSO);
            this.groupBox9.Controls.Add(this.numAvoidWidth);
            this.groupBox9.Controls.Add(this.numTOF);
            this.groupBox9.Controls.Add(this.numUTC);
            this.groupBox9.Controls.Add(this.rb_RGGctrl_ToF);
            this.groupBox9.Controls.Add(this.rb_RGGctrl_Utc);
            this.groupBox9.Controls.Add(this.rb_RGGctrl_Gcal);
            this.groupBox9.Controls.Add(this.rb_RGGctrl_LUTinit);
            this.groupBox9.Controls.Add(this.rb_RGGctrl_LUTsend);
            this.groupBox9.Controls.Add(this.rb_RGGctrl_ready);
            this.groupBox9.Controls.Add(this.label11);
            this.groupBox9.Controls.Add(this.label9);
            this.groupBox9.Controls.Add(this.label8);
            this.groupBox9.Controls.Add(this.label7);
            this.groupBox9.Controls.Add(this.label10);
            this.groupBox9.Controls.Add(this.numGPW);
            this.groupBox9.Controls.Add(this.numGPSO);
            this.groupBox9.Controls.Add(this.panel1);
            this.groupBox9.Controls.Add(this.tranmissionReat_check);
            this.groupBox9.Controls.Add(this.btn_CBIT);
            this.groupBox9.Controls.Add(this.btn_set_command);
            this.groupBox9.Controls.Add(this.btn_IBIT);
            this.groupBox9.Controls.Add(this.btn_PBIT);
            this.groupBox9.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox9.Location = new System.Drawing.Point(24, 295);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox9.Size = new System.Drawing.Size(637, 387);
            this.groupBox9.TabIndex = 35;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "RGG Control";
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.DimGray;
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Font = new System.Drawing.Font("Arial", 10F);
            this.label12.Location = new System.Drawing.Point(287, 117);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(180, 34);
            this.label12.TabIndex = 57;
            this.label12.Text = "Avoid Set Position      Width";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.DimGray;
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(14, 117);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(135, 34);
            this.label13.TabIndex = 58;
            this.label13.Text = "Avoid Set Position  StartOffset";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numAvoidSO
            // 
            this.numAvoidSO.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numAvoidSO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.numAvoidSO.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numAvoidSO.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.numAvoidSO.Location = new System.Drawing.Point(150, 123);
            this.numAvoidSO.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numAvoidSO.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numAvoidSO.Name = "numAvoidSO";
            this.numAvoidSO.Size = new System.Drawing.Size(138, 31);
            this.numAvoidSO.TabIndex = 56;
            this.numAvoidSO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numAvoidSO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numAvoidSO_KeyDown);
            this.numAvoidSO.Leave += new System.EventHandler(this.numAvoidSO_Leave);
            // 
            // numAvoidWidth
            // 
            this.numAvoidWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numAvoidWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.numAvoidWidth.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numAvoidWidth.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.numAvoidWidth.Location = new System.Drawing.Point(467, 121);
            this.numAvoidWidth.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numAvoidWidth.Name = "numAvoidWidth";
            this.numAvoidWidth.Size = new System.Drawing.Size(149, 31);
            this.numAvoidWidth.TabIndex = 55;
            this.numAvoidWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numAvoidWidth.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numAvoidWidth_KeyDown);
            this.numAvoidWidth.Leave += new System.EventHandler(this.numAvoidWidth_Leave);
            // 
            // numTOF
            // 
            this.numTOF.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numTOF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.numTOF.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numTOF.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.numTOF.Location = new System.Drawing.Point(465, 170);
            this.numTOF.Maximum = new decimal(new int[] {
            -1,
            -1,
            0,
            0});
            this.numTOF.Name = "numTOF";
            this.numTOF.Size = new System.Drawing.Size(151, 31);
            this.numTOF.TabIndex = 54;
            this.numTOF.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numTOF.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numTOF_KeyDown);
            this.numTOF.Leave += new System.EventHandler(this.numTOF_Leave);
            // 
            // numUTC
            // 
            this.numUTC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numUTC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.numUTC.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numUTC.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.numUTC.Location = new System.Drawing.Point(148, 170);
            this.numUTC.Maximum = new decimal(new int[] {
            86399999,
            0,
            0,
            0});
            this.numUTC.Name = "numUTC";
            this.numUTC.Size = new System.Drawing.Size(138, 31);
            this.numUTC.TabIndex = 54;
            this.numUTC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numUTC.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numUTC_KeyDown);
            this.numUTC.Leave += new System.EventHandler(this.numUTC_Leave);
            // 
            // rb_RGGctrl_ToF
            // 
            this.rb_RGGctrl_ToF.Appearance = System.Windows.Forms.Appearance.Button;
            this.rb_RGGctrl_ToF.FlatAppearance.BorderColor = System.Drawing.Color.SlateGray;
            this.rb_RGGctrl_ToF.FlatAppearance.BorderSize = 2;
            this.rb_RGGctrl_ToF.FlatAppearance.CheckedBackColor = System.Drawing.Color.Navy;
            this.rb_RGGctrl_ToF.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.rb_RGGctrl_ToF.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.rb_RGGctrl_ToF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rb_RGGctrl_ToF.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rb_RGGctrl_ToF.ForeColor = System.Drawing.Color.Gainsboro;
            this.rb_RGGctrl_ToF.Location = new System.Drawing.Point(536, 25);
            this.rb_RGGctrl_ToF.Name = "rb_RGGctrl_ToF";
            this.rb_RGGctrl_ToF.Size = new System.Drawing.Size(79, 34);
            this.rb_RGGctrl_ToF.TabIndex = 53;
            this.rb_RGGctrl_ToF.TabStop = true;
            this.rb_RGGctrl_ToF.Tag = "OVR";
            this.rb_RGGctrl_ToF.Text = "6.OVR";
            this.rb_RGGctrl_ToF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rb_RGGctrl_ToF.UseVisualStyleBackColor = true;
            // 
            // rb_RGGctrl_Utc
            // 
            this.rb_RGGctrl_Utc.Appearance = System.Windows.Forms.Appearance.Button;
            this.rb_RGGctrl_Utc.FlatAppearance.BorderColor = System.Drawing.Color.SlateGray;
            this.rb_RGGctrl_Utc.FlatAppearance.BorderSize = 2;
            this.rb_RGGctrl_Utc.FlatAppearance.CheckedBackColor = System.Drawing.Color.Navy;
            this.rb_RGGctrl_Utc.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.rb_RGGctrl_Utc.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.rb_RGGctrl_Utc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rb_RGGctrl_Utc.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rb_RGGctrl_Utc.ForeColor = System.Drawing.Color.Gainsboro;
            this.rb_RGGctrl_Utc.Location = new System.Drawing.Point(463, 25);
            this.rb_RGGctrl_Utc.Name = "rb_RGGctrl_Utc";
            this.rb_RGGctrl_Utc.Size = new System.Drawing.Size(79, 34);
            this.rb_RGGctrl_Utc.TabIndex = 53;
            this.rb_RGGctrl_Utc.TabStop = true;
            this.rb_RGGctrl_Utc.Tag = "RANGE";
            this.rb_RGGctrl_Utc.Text = "5.Range";
            this.rb_RGGctrl_Utc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rb_RGGctrl_Utc.UseVisualStyleBackColor = true;
            // 
            // rb_RGGctrl_Gcal
            // 
            this.rb_RGGctrl_Gcal.Appearance = System.Windows.Forms.Appearance.Button;
            this.rb_RGGctrl_Gcal.FlatAppearance.BorderColor = System.Drawing.Color.SlateGray;
            this.rb_RGGctrl_Gcal.FlatAppearance.BorderSize = 2;
            this.rb_RGGctrl_Gcal.FlatAppearance.CheckedBackColor = System.Drawing.Color.Navy;
            this.rb_RGGctrl_Gcal.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.rb_RGGctrl_Gcal.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.rb_RGGctrl_Gcal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rb_RGGctrl_Gcal.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rb_RGGctrl_Gcal.ForeColor = System.Drawing.Color.Gainsboro;
            this.rb_RGGctrl_Gcal.Location = new System.Drawing.Point(391, 25);
            this.rb_RGGctrl_Gcal.Name = "rb_RGGctrl_Gcal";
            this.rb_RGGctrl_Gcal.Size = new System.Drawing.Size(79, 34);
            this.rb_RGGctrl_Gcal.TabIndex = 53;
            this.rb_RGGctrl_Gcal.TabStop = true;
            this.rb_RGGctrl_Gcal.Tag = "GCAL";
            this.rb_RGGctrl_Gcal.Text = "4.GCAL";
            this.rb_RGGctrl_Gcal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rb_RGGctrl_Gcal.UseVisualStyleBackColor = true;
            this.rb_RGGctrl_Gcal.CheckedChanged += new System.EventHandler(this.rb_RGGctrl_set);
            // 
            // rb_RGGctrl_LUTinit
            // 
            this.rb_RGGctrl_LUTinit.Appearance = System.Windows.Forms.Appearance.Button;
            this.rb_RGGctrl_LUTinit.FlatAppearance.BorderColor = System.Drawing.Color.SlateGray;
            this.rb_RGGctrl_LUTinit.FlatAppearance.BorderSize = 2;
            this.rb_RGGctrl_LUTinit.FlatAppearance.CheckedBackColor = System.Drawing.Color.Navy;
            this.rb_RGGctrl_LUTinit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.rb_RGGctrl_LUTinit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.rb_RGGctrl_LUTinit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rb_RGGctrl_LUTinit.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rb_RGGctrl_LUTinit.ForeColor = System.Drawing.Color.Gainsboro;
            this.rb_RGGctrl_LUTinit.Location = new System.Drawing.Point(287, 25);
            this.rb_RGGctrl_LUTinit.Name = "rb_RGGctrl_LUTinit";
            this.rb_RGGctrl_LUTinit.Size = new System.Drawing.Size(106, 34);
            this.rb_RGGctrl_LUTinit.TabIndex = 53;
            this.rb_RGGctrl_LUTinit.TabStop = true;
            this.rb_RGGctrl_LUTinit.Tag = "LUTINIT";
            this.rb_RGGctrl_LUTinit.Text = "3.LUT 초기화";
            this.rb_RGGctrl_LUTinit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rb_RGGctrl_LUTinit.UseVisualStyleBackColor = true;
            this.rb_RGGctrl_LUTinit.CheckedChanged += new System.EventHandler(this.rb_RGGctrl_set);
            // 
            // rb_RGGctrl_LUTsend
            // 
            this.rb_RGGctrl_LUTsend.Appearance = System.Windows.Forms.Appearance.Button;
            this.rb_RGGctrl_LUTsend.FlatAppearance.BorderColor = System.Drawing.Color.SlateGray;
            this.rb_RGGctrl_LUTsend.FlatAppearance.BorderSize = 2;
            this.rb_RGGctrl_LUTsend.FlatAppearance.CheckedBackColor = System.Drawing.Color.Navy;
            this.rb_RGGctrl_LUTsend.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.rb_RGGctrl_LUTsend.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.rb_RGGctrl_LUTsend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rb_RGGctrl_LUTsend.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rb_RGGctrl_LUTsend.ForeColor = System.Drawing.Color.Gainsboro;
            this.rb_RGGctrl_LUTsend.Location = new System.Drawing.Point(186, 25);
            this.rb_RGGctrl_LUTsend.Name = "rb_RGGctrl_LUTsend";
            this.rb_RGGctrl_LUTsend.Size = new System.Drawing.Size(103, 34);
            this.rb_RGGctrl_LUTsend.TabIndex = 53;
            this.rb_RGGctrl_LUTsend.TabStop = true;
            this.rb_RGGctrl_LUTsend.Tag = "LUTSEND";
            this.rb_RGGctrl_LUTsend.Text = "2.LUT송신";
            this.rb_RGGctrl_LUTsend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rb_RGGctrl_LUTsend.UseVisualStyleBackColor = true;
            this.rb_RGGctrl_LUTsend.CheckedChanged += new System.EventHandler(this.rb_RGGctrl_set);
            // 
            // rb_RGGctrl_ready
            // 
            this.rb_RGGctrl_ready.Appearance = System.Windows.Forms.Appearance.Button;
            this.rb_RGGctrl_ready.FlatAppearance.BorderColor = System.Drawing.Color.SlateGray;
            this.rb_RGGctrl_ready.FlatAppearance.BorderSize = 2;
            this.rb_RGGctrl_ready.FlatAppearance.CheckedBackColor = System.Drawing.Color.Navy;
            this.rb_RGGctrl_ready.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.rb_RGGctrl_ready.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.rb_RGGctrl_ready.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rb_RGGctrl_ready.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rb_RGGctrl_ready.ForeColor = System.Drawing.Color.Gainsboro;
            this.rb_RGGctrl_ready.Location = new System.Drawing.Point(118, 25);
            this.rb_RGGctrl_ready.Name = "rb_RGGctrl_ready";
            this.rb_RGGctrl_ready.Size = new System.Drawing.Size(70, 34);
            this.rb_RGGctrl_ready.TabIndex = 53;
            this.rb_RGGctrl_ready.TabStop = true;
            this.rb_RGGctrl_ready.Tag = "STANDBY";
            this.rb_RGGctrl_ready.Text = "1.대기";
            this.rb_RGGctrl_ready.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rb_RGGctrl_ready.UseVisualStyleBackColor = true;
            this.rb_RGGctrl_ready.CheckedChanged += new System.EventHandler(this.rb_RGGctrl_set);
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.DimGray;
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(286, 71);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(180, 34);
            this.label11.TabIndex = 52;
            this.label11.Text = "GatePulseStartOffset";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.DimGray;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(287, 164);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(179, 34);
            this.label9.TabIndex = 52;
            this.label9.Text = "LUT TOF";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.DimGray;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(13, 164);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(135, 34);
            this.label8.TabIndex = 52;
            this.label8.Text = "LUT UTC";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.DimGray;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(13, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 34);
            this.label7.TabIndex = 52;
            this.label7.Text = "GatePulseWidth";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.DimGray;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(14, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(104, 34);
            this.label10.TabIndex = 52;
            this.label10.Text = " RGG ctrl";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numGPW
            // 
            this.numGPW.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numGPW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.numGPW.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numGPW.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.numGPW.Location = new System.Drawing.Point(149, 76);
            this.numGPW.Maximum = new decimal(new int[] {
            65534,
            0,
            0,
            0});
            this.numGPW.Name = "numGPW";
            this.numGPW.Size = new System.Drawing.Size(138, 31);
            this.numGPW.TabIndex = 47;
            this.numGPW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numGPW.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numGPW_KeyDown);
            this.numGPW.Leave += new System.EventHandler(this.numGPW_Leave);
            // 
            // numGPSO
            // 
            this.numGPSO.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numGPSO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.numGPSO.Font = new System.Drawing.Font("Consolas", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numGPSO.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.numGPSO.Location = new System.Drawing.Point(466, 76);
            this.numGPSO.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numGPSO.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numGPSO.Name = "numGPSO";
            this.numGPSO.Size = new System.Drawing.Size(149, 31);
            this.numGPSO.TabIndex = 43;
            this.numGPSO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numGPSO.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numGPSO_KeyDown);
            this.numGPSO.Leave += new System.EventHandler(this.numGPSO_Leave);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Location = new System.Drawing.Point(37, 292);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(562, 10);
            this.panel1.TabIndex = 42;
            // 
            // tranmissionReat_check
            // 
            this.tranmissionReat_check.AutoSize = true;
            this.tranmissionReat_check.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tranmissionReat_check.Location = new System.Drawing.Point(47, 231);
            this.tranmissionReat_check.Name = "tranmissionReat_check";
            this.tranmissionReat_check.Size = new System.Drawing.Size(101, 31);
            this.tranmissionReat_check.TabIndex = 37;
            this.tranmissionReat_check.Text = "20Hz";
            this.tranmissionReat_check.UseVisualStyleBackColor = true;
            this.tranmissionReat_check.CheckedChanged += new System.EventHandler(this.tranmissionReat_check_CheckedChanged);
            // 
            // btn_CBIT
            // 
            this.btn_CBIT.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.btn_CBIT.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_CBIT.FlatAppearance.BorderSize = 3;
            this.btn_CBIT.FlatAppearance.CheckedBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_CBIT.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_CBIT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btn_CBIT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CBIT.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.btn_CBIT.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_CBIT.Location = new System.Drawing.Point(426, 323);
            this.btn_CBIT.Name = "btn_CBIT";
            this.btn_CBIT.Size = new System.Drawing.Size(165, 47);
            this.btn_CBIT.TabIndex = 35;
            this.btn_CBIT.Text = "CBIT";
            this.btn_CBIT.UseVisualStyleBackColor = false;
            this.btn_CBIT.Click += new System.EventHandler(this.btn_CBIT_Click);
            // 
            // btn_set_command
            // 
            this.btn_set_command.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.btn_set_command.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_set_command.FlatAppearance.BorderSize = 3;
            this.btn_set_command.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CornflowerBlue;
            this.btn_set_command.FlatAppearance.MouseOverBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_set_command.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_set_command.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_set_command.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_set_command.Location = new System.Drawing.Point(173, 217);
            this.btn_set_command.Name = "btn_set_command";
            this.btn_set_command.Size = new System.Drawing.Size(399, 59);
            this.btn_set_command.TabIndex = 31;
            this.btn_set_command.Text = "명령 전송";
            this.btn_set_command.UseVisualStyleBackColor = false;
            this.btn_set_command.Click += new System.EventHandler(this.btn_set_command_Click);
            // 
            // btn_IBIT
            // 
            this.btn_IBIT.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.btn_IBIT.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_IBIT.FlatAppearance.BorderSize = 3;
            this.btn_IBIT.FlatAppearance.CheckedBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_IBIT.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_IBIT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btn_IBIT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_IBIT.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.btn_IBIT.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_IBIT.Location = new System.Drawing.Point(234, 323);
            this.btn_IBIT.Name = "btn_IBIT";
            this.btn_IBIT.Size = new System.Drawing.Size(152, 47);
            this.btn_IBIT.TabIndex = 35;
            this.btn_IBIT.Text = "IBIT";
            this.btn_IBIT.UseVisualStyleBackColor = false;
            this.btn_IBIT.Click += new System.EventHandler(this.btn_IBIT_Click);
            // 
            // btn_PBIT
            // 
            this.btn_PBIT.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.btn_PBIT.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_PBIT.FlatAppearance.BorderSize = 3;
            this.btn_PBIT.FlatAppearance.CheckedBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_PBIT.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_PBIT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btn_PBIT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_PBIT.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold);
            this.btn_PBIT.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_PBIT.Location = new System.Drawing.Point(44, 323);
            this.btn_PBIT.Name = "btn_PBIT";
            this.btn_PBIT.Size = new System.Drawing.Size(152, 47);
            this.btn_PBIT.TabIndex = 35;
            this.btn_PBIT.Text = "PBIT";
            this.btn_PBIT.UseVisualStyleBackColor = false;
            this.btn_PBIT.Click += new System.EventHandler(this.btn_PBIT_Click);
            // 
            // periodicSend_timer
            // 
            this.periodicSend_timer.Tick += new System.EventHandler(this.periodicSend_timer_Tick);
            // 
            // SystemDiagnostic_RGG_SAT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.groupBox5);
            this.DoubleBuffered = true;
            this.Name = "SystemDiagnostic_RGG_SAT";
            this.Size = new System.Drawing.Size(723, 731);
            this.Load += new System.EventHandler(this.SystemDiagnostic_RGG_Load);
            this.groupBox5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAvoidSO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAvoidWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTOF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUTC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGPW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGPSO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox tranmissionReat_check;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button btn_set_command;
        private System.Windows.Forms.Timer periodicSend_timer;
        private System.Windows.Forms.Button btn_PBIT;
        private System.Windows.Forms.Button btn_CBIT;
        private System.Windows.Forms.Button btn_IBIT;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_connection_rgg;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label_GateStartOffset;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label led_P_B4;
        private System.Windows.Forms.Label led_P_B3;
        private System.Windows.Forms.Label led_P_B2;
        private System.Windows.Forms.Label led_P_B1;
        private System.Windows.Forms.Label led_P_B0;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label_GatePulseWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_RggControl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_GateStartOffset_m;
        private System.Windows.Forms.Label label_GateWidth_m;
        private System.Windows.Forms.Label label_RggCtrl;
        private System.Windows.Forms.NumericUpDown numGPSO;
        private System.Windows.Forms.NumericUpDown numGPW;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton rb_RGGctrl_Gcal;
        private System.Windows.Forms.RadioButton rb_RGGctrl_LUTinit;
        private System.Windows.Forms.RadioButton rb_RGGctrl_LUTsend;
        private System.Windows.Forms.RadioButton rb_RGGctrl_ready;
        private System.Windows.Forms.RadioButton rb_RGGctrl_ToF;
        private System.Windows.Forms.RadioButton rb_RGGctrl_Utc;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numTOF;
        private System.Windows.Forms.NumericUpDown numUTC;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numAvoidSO;
        private System.Windows.Forms.NumericUpDown numAvoidWidth;
    }
}
