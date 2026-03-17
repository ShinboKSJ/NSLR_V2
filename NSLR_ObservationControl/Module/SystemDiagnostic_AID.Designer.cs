namespace NSLR_ObservationControl.Module
{
    partial class SystemDiagnostic_AID
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
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.AID_Test = new System.Windows.Forms.CheckBox();
            this.label_AirplaneDetecred = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_ADSbSync = new System.Windows.Forms.TextBox();
            this.textBox_IBITresult = new System.Windows.Forms.TextBox();
            this.textBox_IBIT2 = new System.Windows.Forms.TextBox();
            this.textBox_IBIT1 = new System.Windows.Forms.TextBox();
            this.textBox_IBIT0 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_PBitresult = new System.Windows.Forms.TextBox();
            this.textBox_CBITresult = new System.Windows.Forms.TextBox();
            this.textBox_PBIT2 = new System.Windows.Forms.TextBox();
            this.textBox_PBIT1 = new System.Windows.Forms.TextBox();
            this.textBox_PBIT0 = new System.Windows.Forms.TextBox();
            this.label_PBIT = new System.Windows.Forms.Label();
            this.textBox_CBIT3 = new System.Windows.Forms.TextBox();
            this.textBox_CBIT2 = new System.Windows.Forms.TextBox();
            this.textBox_CBIT1 = new System.Windows.Forms.TextBox();
            this.textBox_CBIT0 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_RadarSync = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_OpMode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_cmd_AzEl = new System.Windows.Forms.Button();
            this.textBox_Az = new System.Windows.Forms.TextBox();
            this.textBox_EL = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_IBIT = new System.Windows.Forms.Button();
            this.btn_PBIT = new System.Windows.Forms.Button();
            this.btn_aid_connection = new System.Windows.Forms.Button();
            this.ttimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox7.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.Color.Transparent;
            this.groupBox7.Controls.Add(this.AID_Test);
            this.groupBox7.Controls.Add(this.label_AirplaneDetecred);
            this.groupBox7.Controls.Add(this.groupBox2);
            this.groupBox7.Controls.Add(this.groupBox1);
            this.groupBox7.Controls.Add(this.btn_aid_connection);
            this.groupBox7.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox7.ForeColor = System.Drawing.Color.Orange;
            this.groupBox7.Location = new System.Drawing.Point(10, 13);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(700, 700);
            this.groupBox7.TabIndex = 2;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "    ✈️ Airplane Detection      ";
            // 
            // AID_Test
            // 
            this.AID_Test.Appearance = System.Windows.Forms.Appearance.Button;
            this.AID_Test.AutoSize = true;
            this.AID_Test.BackColor = System.Drawing.Color.DimGray;
            this.AID_Test.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.AID_Test.ForeColor = System.Drawing.Color.DarkGray;
            this.AID_Test.Location = new System.Drawing.Point(573, 345);
            this.AID_Test.Name = "AID_Test";
            this.AID_Test.Size = new System.Drawing.Size(93, 28);
            this.AID_Test.TabIndex = 48;
            this.AID_Test.Text = "AID Test";
            this.AID_Test.UseVisualStyleBackColor = false;
            this.AID_Test.CheckedChanged += new System.EventHandler(this.AID_Test_CheckedChanged);
            // 
            // label_AirplaneDetecred
            // 
            this.label_AirplaneDetecred.AutoSize = true;
            this.label_AirplaneDetecred.BackColor = System.Drawing.Color.Firebrick;
            this.label_AirplaneDetecred.Font = new System.Drawing.Font("Consolas", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_AirplaneDetecred.Location = new System.Drawing.Point(130, 40);
            this.label_AirplaneDetecred.Name = "label_AirplaneDetecred";
            this.label_AirplaneDetecred.Size = new System.Drawing.Size(0, 47);
            this.label_AirplaneDetecred.TabIndex = 47;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.groupBox2.Controls.Add(this.textBox_ADSbSync);
            this.groupBox2.Controls.Add(this.textBox_IBITresult);
            this.groupBox2.Controls.Add(this.textBox_IBIT2);
            this.groupBox2.Controls.Add(this.textBox_IBIT1);
            this.groupBox2.Controls.Add(this.textBox_IBIT0);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.textBox_PBitresult);
            this.groupBox2.Controls.Add(this.textBox_CBITresult);
            this.groupBox2.Controls.Add(this.textBox_PBIT2);
            this.groupBox2.Controls.Add(this.textBox_PBIT1);
            this.groupBox2.Controls.Add(this.textBox_PBIT0);
            this.groupBox2.Controls.Add(this.label_PBIT);
            this.groupBox2.Controls.Add(this.textBox_CBIT3);
            this.groupBox2.Controls.Add(this.textBox_CBIT2);
            this.groupBox2.Controls.Add(this.textBox_CBIT1);
            this.groupBox2.Controls.Add(this.textBox_CBIT0);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.textBox_RadarSync);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.textBox_OpMode);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Location = new System.Drawing.Point(24, 102);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(648, 227);
            this.groupBox2.TabIndex = 46;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "상태정보";
            // 
            // textBox_ADSbSync
            // 
            this.textBox_ADSbSync.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox_ADSbSync.ForeColor = System.Drawing.Color.Lime;
            this.textBox_ADSbSync.Location = new System.Drawing.Point(331, 66);
            this.textBox_ADSbSync.Name = "textBox_ADSbSync";
            this.textBox_ADSbSync.Size = new System.Drawing.Size(139, 30);
            this.textBox_ADSbSync.TabIndex = 60;
            this.textBox_ADSbSync.Text = "０";
            this.textBox_ADSbSync.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_IBITresult
            // 
            this.textBox_IBITresult.BackColor = System.Drawing.Color.Black;
            this.textBox_IBITresult.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_IBITresult.ForeColor = System.Drawing.Color.GreenYellow;
            this.textBox_IBITresult.Location = new System.Drawing.Point(411, 181);
            this.textBox_IBITresult.Name = "textBox_IBITresult";
            this.textBox_IBITresult.Size = new System.Drawing.Size(183, 30);
            this.textBox_IBITresult.TabIndex = 59;
            this.textBox_IBITresult.Text = "0";
            this.textBox_IBITresult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_IBIT2
            // 
            this.textBox_IBIT2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox_IBIT2.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_IBIT2.ForeColor = System.Drawing.Color.GreenYellow;
            this.textBox_IBIT2.Location = new System.Drawing.Point(331, 181);
            this.textBox_IBIT2.Name = "textBox_IBIT2";
            this.textBox_IBIT2.Size = new System.Drawing.Size(59, 30);
            this.textBox_IBIT2.TabIndex = 56;
            this.textBox_IBIT2.Text = "0";
            this.textBox_IBIT2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_IBIT1
            // 
            this.textBox_IBIT1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox_IBIT1.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_IBIT1.ForeColor = System.Drawing.Color.GreenYellow;
            this.textBox_IBIT1.Location = new System.Drawing.Point(256, 181);
            this.textBox_IBIT1.Name = "textBox_IBIT1";
            this.textBox_IBIT1.Size = new System.Drawing.Size(59, 30);
            this.textBox_IBIT1.TabIndex = 57;
            this.textBox_IBIT1.Text = "0";
            this.textBox_IBIT1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_IBIT0
            // 
            this.textBox_IBIT0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox_IBIT0.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_IBIT0.ForeColor = System.Drawing.Color.GreenYellow;
            this.textBox_IBIT0.Location = new System.Drawing.Point(177, 181);
            this.textBox_IBIT0.Name = "textBox_IBIT0";
            this.textBox_IBIT0.Size = new System.Drawing.Size(59, 30);
            this.textBox_IBIT0.TabIndex = 58;
            this.textBox_IBIT0.Text = "0";
            this.textBox_IBIT0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label11.Location = new System.Drawing.Point(75, 187);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(84, 18);
            this.label11.TabIndex = 55;
            this.label11.Text = "IBIT 결과";
            // 
            // textBox_PBitresult
            // 
            this.textBox_PBitresult.BackColor = System.Drawing.Color.Black;
            this.textBox_PBitresult.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_PBitresult.ForeColor = System.Drawing.Color.GreenYellow;
            this.textBox_PBitresult.Location = new System.Drawing.Point(411, 143);
            this.textBox_PBitresult.Name = "textBox_PBitresult";
            this.textBox_PBitresult.Size = new System.Drawing.Size(183, 30);
            this.textBox_PBitresult.TabIndex = 53;
            this.textBox_PBitresult.Text = "0";
            this.textBox_PBitresult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_CBITresult
            // 
            this.textBox_CBITresult.BackColor = System.Drawing.Color.Black;
            this.textBox_CBITresult.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_CBITresult.ForeColor = System.Drawing.Color.GreenYellow;
            this.textBox_CBITresult.Location = new System.Drawing.Point(485, 104);
            this.textBox_CBITresult.Name = "textBox_CBITresult";
            this.textBox_CBITresult.Size = new System.Drawing.Size(109, 30);
            this.textBox_CBITresult.TabIndex = 52;
            this.textBox_CBITresult.Text = "0";
            this.textBox_CBITresult.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_PBIT2
            // 
            this.textBox_PBIT2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox_PBIT2.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_PBIT2.ForeColor = System.Drawing.Color.GreenYellow;
            this.textBox_PBIT2.Location = new System.Drawing.Point(331, 143);
            this.textBox_PBIT2.Name = "textBox_PBIT2";
            this.textBox_PBIT2.Size = new System.Drawing.Size(59, 30);
            this.textBox_PBIT2.TabIndex = 49;
            this.textBox_PBIT2.Text = "0";
            this.textBox_PBIT2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_PBIT1
            // 
            this.textBox_PBIT1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox_PBIT1.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_PBIT1.ForeColor = System.Drawing.Color.GreenYellow;
            this.textBox_PBIT1.Location = new System.Drawing.Point(256, 143);
            this.textBox_PBIT1.Name = "textBox_PBIT1";
            this.textBox_PBIT1.Size = new System.Drawing.Size(59, 30);
            this.textBox_PBIT1.TabIndex = 50;
            this.textBox_PBIT1.Text = "0";
            this.textBox_PBIT1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_PBIT0
            // 
            this.textBox_PBIT0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox_PBIT0.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_PBIT0.ForeColor = System.Drawing.Color.GreenYellow;
            this.textBox_PBIT0.Location = new System.Drawing.Point(177, 143);
            this.textBox_PBIT0.Name = "textBox_PBIT0";
            this.textBox_PBIT0.Size = new System.Drawing.Size(59, 30);
            this.textBox_PBIT0.TabIndex = 51;
            this.textBox_PBIT0.Text = "0";
            this.textBox_PBIT0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_PBIT
            // 
            this.label_PBIT.AutoSize = true;
            this.label_PBIT.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_PBIT.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label_PBIT.Location = new System.Drawing.Point(68, 151);
            this.label_PBIT.Name = "label_PBIT";
            this.label_PBIT.Size = new System.Drawing.Size(92, 18);
            this.label_PBIT.TabIndex = 48;
            this.label_PBIT.Text = "PBIT 결과";
            // 
            // textBox_CBIT3
            // 
            this.textBox_CBIT3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox_CBIT3.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_CBIT3.ForeColor = System.Drawing.Color.GreenYellow;
            this.textBox_CBIT3.Location = new System.Drawing.Point(411, 104);
            this.textBox_CBIT3.Name = "textBox_CBIT3";
            this.textBox_CBIT3.Size = new System.Drawing.Size(59, 30);
            this.textBox_CBIT3.TabIndex = 47;
            this.textBox_CBIT3.Text = "0";
            this.textBox_CBIT3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_CBIT2
            // 
            this.textBox_CBIT2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox_CBIT2.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_CBIT2.ForeColor = System.Drawing.Color.GreenYellow;
            this.textBox_CBIT2.Location = new System.Drawing.Point(331, 104);
            this.textBox_CBIT2.Name = "textBox_CBIT2";
            this.textBox_CBIT2.Size = new System.Drawing.Size(59, 30);
            this.textBox_CBIT2.TabIndex = 47;
            this.textBox_CBIT2.Text = "0";
            this.textBox_CBIT2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_CBIT1
            // 
            this.textBox_CBIT1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox_CBIT1.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_CBIT1.ForeColor = System.Drawing.Color.GreenYellow;
            this.textBox_CBIT1.Location = new System.Drawing.Point(256, 104);
            this.textBox_CBIT1.Name = "textBox_CBIT1";
            this.textBox_CBIT1.Size = new System.Drawing.Size(59, 30);
            this.textBox_CBIT1.TabIndex = 47;
            this.textBox_CBIT1.Text = "0";
            this.textBox_CBIT1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_CBIT0
            // 
            this.textBox_CBIT0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox_CBIT0.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_CBIT0.ForeColor = System.Drawing.Color.GreenYellow;
            this.textBox_CBIT0.Location = new System.Drawing.Point(177, 104);
            this.textBox_CBIT0.Name = "textBox_CBIT0";
            this.textBox_CBIT0.Size = new System.Drawing.Size(59, 30);
            this.textBox_CBIT0.TabIndex = 47;
            this.textBox_CBIT0.Text = "0";
            this.textBox_CBIT0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(61, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(100, 18);
            this.label8.TabIndex = 46;
            this.label8.Text = "CBIT  결과";
            // 
            // textBox_RadarSync
            // 
            this.textBox_RadarSync.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox_RadarSync.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_RadarSync.ForeColor = System.Drawing.Color.Lime;
            this.textBox_RadarSync.Location = new System.Drawing.Point(175, 66);
            this.textBox_RadarSync.Name = "textBox_RadarSync";
            this.textBox_RadarSync.Size = new System.Drawing.Size(140, 30);
            this.textBox_RadarSync.TabIndex = 45;
            this.textBox_RadarSync.Text = "0";
            this.textBox_RadarSync.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(481, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 18);
            this.label2.TabIndex = 41;
            this.label2.Text = "ADS-B 연결";
            // 
            // textBox_OpMode
            // 
            this.textBox_OpMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.textBox_OpMode.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_OpMode.ForeColor = System.Drawing.Color.Lime;
            this.textBox_OpMode.Location = new System.Drawing.Point(265, 28);
            this.textBox_OpMode.Name = "textBox_OpMode";
            this.textBox_OpMode.Size = new System.Drawing.Size(205, 30);
            this.textBox_OpMode.TabIndex = 45;
            this.textBox_OpMode.Text = "0";
            this.textBox_OpMode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(53, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 18);
            this.label6.TabIndex = 41;
            this.label6.Text = "레이더 연결";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(61, 39);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(186, 18);
            this.label7.TabIndex = 41;
            this.label7.Text = "항공기탐지 운용모드";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.btn_cmd_AzEl);
            this.groupBox1.Controls.Add(this.textBox_Az);
            this.groupBox1.Controls.Add(this.textBox_EL);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btn_IBIT);
            this.groupBox1.Controls.Add(this.btn_PBIT);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Location = new System.Drawing.Point(24, 381);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(648, 300);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "  제어명령  ";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Location = new System.Drawing.Point(54, 173);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(546, 10);
            this.panel1.TabIndex = 47;
            // 
            // btn_cmd_AzEl
            // 
            this.btn_cmd_AzEl.BackColor = System.Drawing.Color.LightSlateGray;
            this.btn_cmd_AzEl.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_cmd_AzEl.FlatAppearance.BorderSize = 2;
            this.btn_cmd_AzEl.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_cmd_AzEl.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btn_cmd_AzEl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_cmd_AzEl.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cmd_AzEl.ForeColor = System.Drawing.Color.GreenYellow;
            this.btn_cmd_AzEl.Location = new System.Drawing.Point(458, 68);
            this.btn_cmd_AzEl.Name = "btn_cmd_AzEl";
            this.btn_cmd_AzEl.Size = new System.Drawing.Size(125, 70);
            this.btn_cmd_AzEl.TabIndex = 46;
            this.btn_cmd_AzEl.Text = "지향각";
            this.btn_cmd_AzEl.UseVisualStyleBackColor = false;
            this.btn_cmd_AzEl.Click += new System.EventHandler(this.btn_cmd_AzEl_Click);
            // 
            // textBox_Az
            // 
            this.textBox_Az.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.textBox_Az.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_Az.ForeColor = System.Drawing.SystemColors.Info;
            this.textBox_Az.Location = new System.Drawing.Point(151, 87);
            this.textBox_Az.Name = "textBox_Az";
            this.textBox_Az.Size = new System.Drawing.Size(134, 38);
            this.textBox_Az.TabIndex = 45;
            this.textBox_Az.Text = "0";
            this.textBox_Az.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_EL
            // 
            this.textBox_EL.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.textBox_EL.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_EL.ForeColor = System.Drawing.SystemColors.Info;
            this.textBox_EL.Location = new System.Drawing.Point(305, 87);
            this.textBox_EL.Name = "textBox_EL";
            this.textBox_EL.Size = new System.Drawing.Size(134, 38);
            this.textBox_EL.TabIndex = 45;
            this.textBox_EL.Text = "0";
            this.textBox_EL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_EL.MouseLeave += new System.EventHandler(this.textBox_EL_MouseLeave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(200, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 18);
            this.label3.TabIndex = 44;
            this.label3.Text = "Az.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(27, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 18);
            this.label4.TabIndex = 41;
            this.label4.Text = "-335~335°";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(357, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 18);
            this.label1.TabIndex = 41;
            this.label1.Text = "El.";
            // 
            // btn_IBIT
            // 
            this.btn_IBIT.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.btn_IBIT.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btn_IBIT.FlatAppearance.BorderSize = 3;
            this.btn_IBIT.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_IBIT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btn_IBIT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_IBIT.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_IBIT.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_IBIT.Location = new System.Drawing.Point(341, 203);
            this.btn_IBIT.Name = "btn_IBIT";
            this.btn_IBIT.Size = new System.Drawing.Size(248, 69);
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
            this.btn_PBIT.FlatAppearance.MouseDownBackColor = System.Drawing.Color.RoyalBlue;
            this.btn_PBIT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSteelBlue;
            this.btn_PBIT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_PBIT.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_PBIT.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_PBIT.Location = new System.Drawing.Point(66, 203);
            this.btn_PBIT.Name = "btn_PBIT";
            this.btn_PBIT.Size = new System.Drawing.Size(248, 69);
            this.btn_PBIT.TabIndex = 35;
            this.btn_PBIT.Text = "PBIT";
            this.btn_PBIT.UseVisualStyleBackColor = false;
            this.btn_PBIT.Click += new System.EventHandler(this.btn_PBIT_Click);
            // 
            // btn_aid_connection
            // 
            this.btn_aid_connection.BackColor = System.Drawing.Color.DarkGray;
            this.btn_aid_connection.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_aid_connection.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btn_aid_connection.Location = new System.Drawing.Point(479, 36);
            this.btn_aid_connection.Name = "btn_aid_connection";
            this.btn_aid_connection.Size = new System.Drawing.Size(193, 45);
            this.btn_aid_connection.TabIndex = 2;
            this.btn_aid_connection.Text = "연결";
            this.btn_aid_connection.UseVisualStyleBackColor = false;
            // 
            // SystemDiagnostic_AID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.groupBox7);
            this.DoubleBuffered = true;
            this.Name = "SystemDiagnostic_AID";
            this.Size = new System.Drawing.Size(723, 731);
            this.Load += new System.EventHandler(this.SystemDiagnostic_AID_Load);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btn_aid_connection;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_IBIT;
        private System.Windows.Forms.Button btn_PBIT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Az;
        private System.Windows.Forms.TextBox textBox_EL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_RadarSync;
        private System.Windows.Forms.TextBox textBox_OpMode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_CBIT3;
        private System.Windows.Forms.TextBox textBox_CBIT2;
        private System.Windows.Forms.TextBox textBox_CBIT1;
        private System.Windows.Forms.TextBox textBox_CBIT0;
        private System.Windows.Forms.TextBox textBox_PBIT2;
        private System.Windows.Forms.TextBox textBox_PBIT1;
        private System.Windows.Forms.TextBox textBox_PBIT0;
        private System.Windows.Forms.Label label_PBIT;
        private System.Windows.Forms.Timer ttimer;
        private System.Windows.Forms.TextBox textBox_CBITresult;
        private System.Windows.Forms.TextBox textBox_PBitresult;
        private System.Windows.Forms.TextBox textBox_IBITresult;
        private System.Windows.Forms.TextBox textBox_IBIT2;
        private System.Windows.Forms.TextBox textBox_IBIT1;
        private System.Windows.Forms.TextBox textBox_IBIT0;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btn_cmd_AzEl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_AirplaneDetecred;
        private System.Windows.Forms.TextBox textBox_ADSbSync;
        private System.Windows.Forms.CheckBox AID_Test;
    }
}
