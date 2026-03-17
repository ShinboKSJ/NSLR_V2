namespace NSLR_ObservationControl.Module
{
    partial class SystemDiagnostic_TMS
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cb_Hz = new System.Windows.Forms.CheckBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.radioButton_pbit = new System.Windows.Forms.RadioButton();
            this.radioButton_ibit = new System.Windows.Forms.RadioButton();
            this.radioButton_cbit = new System.Windows.Forms.RadioButton();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.btn_set_command = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cb_RggControl = new System.Windows.Forms.ComboBox();
            this.cb_LUT_Utc = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cb_LUT_Delay = new System.Windows.Forms.ComboBox();
            this.cb_GatePulsWidth = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cb_GatePulseStartOffset = new System.Windows.Forms.ComboBox();
            this.cb_AvoidWidth = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cb_AvoidSetPosStartOffset = new System.Windows.Forms.ComboBox();
            this.btn_connection_rgg = new System.Windows.Forms.Button();
            this.groupBox5.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBox5.Controls.Add(this.cb_Hz);
            this.groupBox5.Controls.Add(this.groupBox10);
            this.groupBox5.Controls.Add(this.groupBox9);
            this.groupBox5.Controls.Add(this.btn_connection_rgg);
            this.groupBox5.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.groupBox5.Location = new System.Drawing.Point(12, 13);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(700, 700);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Range Ragte Gernerator";
            // 
            // cb_Hz
            // 
            this.cb_Hz.AutoSize = true;
            this.cb_Hz.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_Hz.Location = new System.Drawing.Point(489, 59);
            this.cb_Hz.Name = "cb_Hz";
            this.cb_Hz.Size = new System.Drawing.Size(101, 31);
            this.cb_Hz.TabIndex = 37;
            this.cb_Hz.Text = "20Hz";
            this.cb_Hz.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            this.groupBox10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.groupBox10.Controls.Add(this.radioButton_pbit);
            this.groupBox10.Controls.Add(this.radioButton_ibit);
            this.groupBox10.Controls.Add(this.radioButton_cbit);
            this.groupBox10.Location = new System.Drawing.Point(45, 441);
            this.groupBox10.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox10.Size = new System.Drawing.Size(601, 183);
            this.groupBox10.TabIndex = 36;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "BIT";
            // 
            // radioButton_pbit
            // 
            this.radioButton_pbit.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_pbit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.radioButton_pbit.FlatAppearance.BorderColor = System.Drawing.Color.GhostWhite;
            this.radioButton_pbit.FlatAppearance.BorderSize = 3;
            this.radioButton_pbit.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumSlateBlue;
            this.radioButton_pbit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.radioButton_pbit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.radioButton_pbit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton_pbit.ForeColor = System.Drawing.SystemColors.Info;
            this.radioButton_pbit.Location = new System.Drawing.Point(65, 73);
            this.radioButton_pbit.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_pbit.Name = "radioButton_pbit";
            this.radioButton_pbit.Size = new System.Drawing.Size(135, 45);
            this.radioButton_pbit.TabIndex = 34;
            this.radioButton_pbit.TabStop = true;
            this.radioButton_pbit.Text = "PBIT";
            this.radioButton_pbit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton_pbit.UseVisualStyleBackColor = false;
            // 
            // radioButton_ibit
            // 
            this.radioButton_ibit.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_ibit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.radioButton_ibit.FlatAppearance.BorderColor = System.Drawing.Color.GhostWhite;
            this.radioButton_ibit.FlatAppearance.BorderSize = 3;
            this.radioButton_ibit.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumSlateBlue;
            this.radioButton_ibit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.radioButton_ibit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.radioButton_ibit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton_ibit.ForeColor = System.Drawing.SystemColors.Info;
            this.radioButton_ibit.Location = new System.Drawing.Point(234, 73);
            this.radioButton_ibit.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_ibit.Name = "radioButton_ibit";
            this.radioButton_ibit.Size = new System.Drawing.Size(135, 45);
            this.radioButton_ibit.TabIndex = 33;
            this.radioButton_ibit.TabStop = true;
            this.radioButton_ibit.Text = "IBIT";
            this.radioButton_ibit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton_ibit.UseVisualStyleBackColor = false;
            // 
            // radioButton_cbit
            // 
            this.radioButton_cbit.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_cbit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.radioButton_cbit.FlatAppearance.BorderColor = System.Drawing.Color.GhostWhite;
            this.radioButton_cbit.FlatAppearance.BorderSize = 3;
            this.radioButton_cbit.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumSlateBlue;
            this.radioButton_cbit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.radioButton_cbit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.radioButton_cbit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton_cbit.ForeColor = System.Drawing.SystemColors.Info;
            this.radioButton_cbit.Location = new System.Drawing.Point(410, 73);
            this.radioButton_cbit.Margin = new System.Windows.Forms.Padding(2);
            this.radioButton_cbit.Name = "radioButton_cbit";
            this.radioButton_cbit.Size = new System.Drawing.Size(135, 45);
            this.radioButton_cbit.TabIndex = 32;
            this.radioButton_cbit.TabStop = true;
            this.radioButton_cbit.Text = "CBIT";
            this.radioButton_cbit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton_cbit.UseVisualStyleBackColor = false;
            // 
            // groupBox9
            // 
            this.groupBox9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.groupBox9.Controls.Add(this.btn_set_command);
            this.groupBox9.Controls.Add(this.label10);
            this.groupBox9.Controls.Add(this.label7);
            this.groupBox9.Controls.Add(this.cb_RggControl);
            this.groupBox9.Controls.Add(this.cb_LUT_Utc);
            this.groupBox9.Controls.Add(this.label8);
            this.groupBox9.Controls.Add(this.cb_LUT_Delay);
            this.groupBox9.Controls.Add(this.cb_GatePulsWidth);
            this.groupBox9.Controls.Add(this.label12);
            this.groupBox9.Controls.Add(this.label9);
            this.groupBox9.Controls.Add(this.label13);
            this.groupBox9.Controls.Add(this.cb_GatePulseStartOffset);
            this.groupBox9.Controls.Add(this.cb_AvoidWidth);
            this.groupBox9.Controls.Add(this.label11);
            this.groupBox9.Controls.Add(this.cb_AvoidSetPosStartOffset);
            this.groupBox9.Location = new System.Drawing.Point(41, 162);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox9.Size = new System.Drawing.Size(603, 244);
            this.groupBox9.TabIndex = 35;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "RGG Control";
            // 
            // btn_set_command
            // 
            this.btn_set_command.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.btn_set_command.FlatAppearance.BorderSize = 3;
            this.btn_set_command.FlatAppearance.MouseDownBackColor = System.Drawing.Color.MediumSlateBlue;
            this.btn_set_command.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btn_set_command.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_set_command.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_set_command.Location = new System.Drawing.Point(193, 163);
            this.btn_set_command.Name = "btn_set_command";
            this.btn_set_command.Size = new System.Drawing.Size(298, 62);
            this.btn_set_command.TabIndex = 31;
            this.btn_set_command.Text = "CTRL CMD";
            this.btn_set_command.UseVisualStyleBackColor = false;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.MidnightBlue;
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label10.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label10.Location = new System.Drawing.Point(293, 131);
            this.label10.Margin = new System.Windows.Forms.Padding(2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(139, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "5.AvoidWidth";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.MidnightBlue;
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label7.Location = new System.Drawing.Point(30, 43);
            this.label7.Margin = new System.Windows.Forms.Padding(2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "1. RGG ctrl";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_RggControl
            // 
            this.cb_RggControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.cb_RggControl.DropDownHeight = 140;
            this.cb_RggControl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_RggControl.DropDownWidth = 130;
            this.cb_RggControl.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_RggControl.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.cb_RggControl.FormattingEnabled = true;
            this.cb_RggControl.IntegralHeight = false;
            this.cb_RggControl.Items.AddRange(new object[] {
            "0x00 : N/A",
            "0x01 : 대기",
            "0x02 : LUT 송신",
            "0x03 : Ground  Calibration",
            "0x04 : Range",
            "0x05 : Overlap Avoidance",
            "0x06 : 안전상태 진입"});
            this.cb_RggControl.Location = new System.Drawing.Point(27, 63);
            this.cb_RggControl.Margin = new System.Windows.Forms.Padding(2);
            this.cb_RggControl.Name = "cb_RggControl";
            this.cb_RggControl.Size = new System.Drawing.Size(117, 27);
            this.cb_RggControl.TabIndex = 17;
            // 
            // cb_LUT_Utc
            // 
            this.cb_LUT_Utc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.cb_LUT_Utc.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_LUT_Utc.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.cb_LUT_Utc.FormattingEnabled = true;
            this.cb_LUT_Utc.Items.AddRange(new object[] {
            "0000000000000000",
            "1111111111111111",
            "2222222222222222",
            "3333333333333333",
            "8888888888888888",
            "AAAAAAAAAAAAAAAA",
            "BBBBBBBBBBBBBBBB",
            "FFFFFFFFFFFFFFFF"});
            this.cb_LUT_Utc.Location = new System.Drawing.Point(446, 62);
            this.cb_LUT_Utc.Margin = new System.Windows.Forms.Padding(2);
            this.cb_LUT_Utc.Name = "cb_LUT_Utc";
            this.cb_LUT_Utc.Size = new System.Drawing.Size(132, 27);
            this.cb_LUT_Utc.TabIndex = 30;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.MidnightBlue;
            this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label8.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label8.Location = new System.Drawing.Point(160, 41);
            this.label8.Margin = new System.Windows.Forms.Padding(2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "2.GatePulseWidth";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_LUT_Delay
            // 
            this.cb_LUT_Delay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.cb_LUT_Delay.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_LUT_Delay.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.cb_LUT_Delay.FormattingEnabled = true;
            this.cb_LUT_Delay.Items.AddRange(new object[] {
            "0000000000000000",
            "1111111111111111",
            "2222222222222222",
            "3333333333333333",
            "8888888888888888",
            "AAAAAAAAAAAAAAAA",
            "BBBBBBBBBBBBBBBB",
            "FFFFFFFFFFFFFFFF"});
            this.cb_LUT_Delay.Location = new System.Drawing.Point(446, 98);
            this.cb_LUT_Delay.Margin = new System.Windows.Forms.Padding(2);
            this.cb_LUT_Delay.Name = "cb_LUT_Delay";
            this.cb_LUT_Delay.Size = new System.Drawing.Size(131, 27);
            this.cb_LUT_Delay.TabIndex = 29;
            // 
            // cb_GatePulsWidth
            // 
            this.cb_GatePulsWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.cb_GatePulsWidth.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_GatePulsWidth.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.cb_GatePulsWidth.FormattingEnabled = true;
            this.cb_GatePulsWidth.Items.AddRange(new object[] {
            "0000",
            "1212",
            "3434",
            "5656",
            "ABBA",
            "CDDC",
            "FEFE",
            "FFFF"});
            this.cb_GatePulsWidth.Location = new System.Drawing.Point(157, 62);
            this.cb_GatePulsWidth.Margin = new System.Windows.Forms.Padding(2);
            this.cb_GatePulsWidth.Name = "cb_GatePulsWidth";
            this.cb_GatePulsWidth.Size = new System.Drawing.Size(108, 27);
            this.cb_GatePulsWidth.TabIndex = 20;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.MidnightBlue;
            this.label12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label12.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label12.Location = new System.Drawing.Point(449, 130);
            this.label12.Margin = new System.Windows.Forms.Padding(2);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(123, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "7.LUT (Delay)";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.MidnightBlue;
            this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label9.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label9.Location = new System.Drawing.Point(148, 129);
            this.label9.Margin = new System.Windows.Forms.Padding(2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(130, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "3.GatePulseStartOffset";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.MidnightBlue;
            this.label13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label13.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label13.Location = new System.Drawing.Point(455, 37);
            this.label13.Margin = new System.Windows.Forms.Padding(2);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(122, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "6.LUT (UTC)";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_GatePulseStartOffset
            // 
            this.cb_GatePulseStartOffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.cb_GatePulseStartOffset.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_GatePulseStartOffset.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.cb_GatePulseStartOffset.FormattingEnabled = true;
            this.cb_GatePulseStartOffset.Items.AddRange(new object[] {
            "8000",
            "cccc",
            "0000",
            "3fff",
            "7f00"});
            this.cb_GatePulseStartOffset.Location = new System.Drawing.Point(157, 98);
            this.cb_GatePulseStartOffset.Margin = new System.Windows.Forms.Padding(2);
            this.cb_GatePulseStartOffset.Name = "cb_GatePulseStartOffset";
            this.cb_GatePulseStartOffset.Size = new System.Drawing.Size(108, 27);
            this.cb_GatePulseStartOffset.TabIndex = 22;
            // 
            // cb_AvoidWidth
            // 
            this.cb_AvoidWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.cb_AvoidWidth.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_AvoidWidth.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.cb_AvoidWidth.FormattingEnabled = true;
            this.cb_AvoidWidth.Items.AddRange(new object[] {
            "0000",
            "1212",
            "3434",
            "5656",
            "ABBA",
            "CDDC",
            "EFFE",
            "FFFF"});
            this.cb_AvoidWidth.Location = new System.Drawing.Point(304, 98);
            this.cb_AvoidWidth.Margin = new System.Windows.Forms.Padding(2);
            this.cb_AvoidWidth.Name = "cb_AvoidWidth";
            this.cb_AvoidWidth.Size = new System.Drawing.Size(117, 27);
            this.cb_AvoidWidth.TabIndex = 26;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.MidnightBlue;
            this.label11.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label11.Location = new System.Drawing.Point(288, 38);
            this.label11.Margin = new System.Windows.Forms.Padding(2);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(145, 18);
            this.label11.TabIndex = 24;
            this.label11.Text = "4.AvoidSetPos.StartOffset";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_AvoidSetPosStartOffset
            // 
            this.cb_AvoidSetPosStartOffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.cb_AvoidSetPosStartOffset.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_AvoidSetPosStartOffset.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.cb_AvoidSetPosStartOffset.FormattingEnabled = true;
            this.cb_AvoidSetPosStartOffset.Items.AddRange(new object[] {
            "8000",
            "cccc",
            "0000",
            "3fff",
            "7f00"});
            this.cb_AvoidSetPosStartOffset.Location = new System.Drawing.Point(304, 61);
            this.cb_AvoidSetPosStartOffset.Margin = new System.Windows.Forms.Padding(2);
            this.cb_AvoidSetPosStartOffset.Name = "cb_AvoidSetPosStartOffset";
            this.cb_AvoidSetPosStartOffset.Size = new System.Drawing.Size(116, 27);
            this.cb_AvoidSetPosStartOffset.TabIndex = 25;
            // 
            // btn_connection_rgg
            // 
            this.btn_connection_rgg.BackColor = System.Drawing.Color.DarkGray;
            this.btn_connection_rgg.Enabled = false;
            this.btn_connection_rgg.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btn_connection_rgg.Location = new System.Drawing.Point(110, 59);
            this.btn_connection_rgg.Name = "btn_connection_rgg";
            this.btn_connection_rgg.Size = new System.Drawing.Size(148, 41);
            this.btn_connection_rgg.TabIndex = 2;
            this.btn_connection_rgg.Text = "연결";
            this.btn_connection_rgg.UseVisualStyleBackColor = false;
            // 
            // SystemDiagnostic_TMS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.groupBox5);
            this.Name = "SystemDiagnostic_TMS";
            this.Size = new System.Drawing.Size(724, 728);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cb_Hz;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.RadioButton radioButton_pbit;
        private System.Windows.Forms.RadioButton radioButton_ibit;
        private System.Windows.Forms.RadioButton radioButton_cbit;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button btn_set_command;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cb_RggControl;
        private System.Windows.Forms.ComboBox cb_LUT_Utc;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cb_LUT_Delay;
        private System.Windows.Forms.ComboBox cb_GatePulsWidth;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cb_GatePulseStartOffset;
        private System.Windows.Forms.ComboBox cb_AvoidWidth;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cb_AvoidSetPosStartOffset;
        private System.Windows.Forms.Button btn_connection_rgg;
    }
}
