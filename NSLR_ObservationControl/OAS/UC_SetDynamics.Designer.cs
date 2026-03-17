namespace NSLR_ObservationControl.OAS
{
    partial class UC_SetDynamics
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.masses_groupBox = new System.Windows.Forms.GroupBox();
            this.ephem_comboBox = new System.Windows.Forms.ComboBox();
            this.ephem_label = new System.Windows.Forms.Label();
            this.venus_checkBox = new System.Windows.Forms.CheckBox();
            this.mars_checkBox = new System.Windows.Forms.CheckBox();
            this.pluto_checkBox = new System.Windows.Forms.CheckBox();
            this.neptune_checkBox = new System.Windows.Forms.CheckBox();
            this.uranus_checkBox = new System.Windows.Forms.CheckBox();
            this.saturn_checkBox = new System.Windows.Forms.CheckBox();
            this.jupiter_checkBox = new System.Windows.Forms.CheckBox();
            this.mercury_checkBox = new System.Windows.Forms.CheckBox();
            this.moon_checkBox = new System.Windows.Forms.CheckBox();
            this.sun_checkBox = new System.Windows.Forms.CheckBox();
            this.atmos_groupBox = new System.Windows.Forms.GroupBox();
            this.sw_normal_radioButton = new System.Windows.Forms.RadioButton();
            this.sw_data_radioButton = new System.Windows.Forms.RadioButton();
            this.sw_label = new System.Windows.Forms.Label();
            this.atmosModel_comboBox = new System.Windows.Forms.ComboBox();
            this.atmosModel_label = new System.Windows.Forms.Label();
            this.radiation_groupBox = new System.Windows.Forms.GroupBox();
            this.EarthRad_comboBox = new System.Windows.Forms.ComboBox();
            this.earthRad_label = new System.Windows.Forms.Label();
            this.solarShadow_comboBox = new System.Windows.Forms.ComboBox();
            this.solarShad_label = new System.Windows.Forms.Label();
            this.relCor_checkBox = new System.Windows.Forms.CheckBox();
            this.propagator_g = new System.Windows.Forms.GroupBox();
            this.propType_comboBox = new System.Windows.Forms.ComboBox();
            this.propType_label = new System.Windows.Forms.Label();
            this.gravity_groupBox = new System.Windows.Forms.GroupBox();
            this.tide_groupBox = new System.Windows.Forms.GroupBox();
            this.oceanTide_comboBox = new System.Windows.Forms.ComboBox();
            this.oceanTide_label = new System.Windows.Forms.Label();
            this.permanent_checkBox = new System.Windows.Forms.CheckBox();
            this.oceanPole_checkBox = new System.Windows.Forms.CheckBox();
            this.solidPole_checkBox = new System.Windows.Forms.CheckBox();
            this.EarthTide_checkBox = new System.Windows.Forms.CheckBox();
            this.gravMaxOrd_textBox = new System.Windows.Forms.TextBox();
            this.gravOrd_textBox = new System.Windows.Forms.TextBox();
            this.gravMaxDeg_textBox = new System.Windows.Forms.TextBox();
            this.gravDeg_textBox = new System.Windows.Forms.TextBox();
            this.gravOrder_label = new System.Windows.Forms.Label();
            this.gravDeg_label = new System.Windows.Forms.Label();
            this.gravModel_comboBox = new System.Windows.Forms.ComboBox();
            this.gravModel_label = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.masses_groupBox.SuspendLayout();
            this.atmos_groupBox.SuspendLayout();
            this.radiation_groupBox.SuspendLayout();
            this.propagator_g.SuspendLayout();
            this.gravity_groupBox.SuspendLayout();
            this.tide_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightGray;
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.masses_groupBox);
            this.groupBox1.Controls.Add(this.atmos_groupBox);
            this.groupBox1.Controls.Add(this.radiation_groupBox);
            this.groupBox1.Controls.Add(this.relCor_checkBox);
            this.groupBox1.Controls.Add(this.propagator_g);
            this.groupBox1.Controls.Add(this.gravity_groupBox);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(19, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(506, 712);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "< Dynamics >";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(179, 656);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 43);
            this.button1.TabIndex = 16;
            this.button1.Text = "Apply";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // masses_groupBox
            // 
            this.masses_groupBox.Controls.Add(this.ephem_comboBox);
            this.masses_groupBox.Controls.Add(this.ephem_label);
            this.masses_groupBox.Controls.Add(this.venus_checkBox);
            this.masses_groupBox.Controls.Add(this.mars_checkBox);
            this.masses_groupBox.Controls.Add(this.pluto_checkBox);
            this.masses_groupBox.Controls.Add(this.neptune_checkBox);
            this.masses_groupBox.Controls.Add(this.uranus_checkBox);
            this.masses_groupBox.Controls.Add(this.saturn_checkBox);
            this.masses_groupBox.Controls.Add(this.jupiter_checkBox);
            this.masses_groupBox.Controls.Add(this.mercury_checkBox);
            this.masses_groupBox.Controls.Add(this.moon_checkBox);
            this.masses_groupBox.Controls.Add(this.sun_checkBox);
            this.masses_groupBox.Location = new System.Drawing.Point(42, 430);
            this.masses_groupBox.Name = "masses_groupBox";
            this.masses_groupBox.Size = new System.Drawing.Size(426, 100);
            this.masses_groupBox.TabIndex = 13;
            this.masses_groupBox.TabStop = false;
            this.masses_groupBox.Text = "Point Masses";
            // 
            // ephem_comboBox
            // 
            this.ephem_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ephem_comboBox.FormattingEnabled = true;
            this.ephem_comboBox.Location = new System.Drawing.Point(129, 20);
            this.ephem_comboBox.Name = "ephem_comboBox";
            this.ephem_comboBox.Size = new System.Drawing.Size(181, 21);
            this.ephem_comboBox.TabIndex = 11;
            // 
            // ephem_label
            // 
            this.ephem_label.AutoSize = true;
            this.ephem_label.Location = new System.Drawing.Point(9, 23);
            this.ephem_label.Name = "ephem_label";
            this.ephem_label.Size = new System.Drawing.Size(142, 14);
            this.ephem_label.TabIndex = 10;
            this.ephem_label.Text = "Ephemeris Source";
            // 
            // venus_checkBox
            // 
            this.venus_checkBox.AutoSize = true;
            this.venus_checkBox.Location = new System.Drawing.Point(268, 52);
            this.venus_checkBox.Name = "venus_checkBox";
            this.venus_checkBox.Size = new System.Drawing.Size(71, 18);
            this.venus_checkBox.TabIndex = 9;
            this.venus_checkBox.Text = "Venus";
            this.venus_checkBox.UseVisualStyleBackColor = true;
            // 
            // mars_checkBox
            // 
            this.mars_checkBox.AutoSize = true;
            this.mars_checkBox.Location = new System.Drawing.Point(356, 52);
            this.mars_checkBox.Name = "mars_checkBox";
            this.mars_checkBox.Size = new System.Drawing.Size(62, 18);
            this.mars_checkBox.TabIndex = 8;
            this.mars_checkBox.Text = "Mars";
            this.mars_checkBox.UseVisualStyleBackColor = true;
            // 
            // pluto_checkBox
            // 
            this.pluto_checkBox.AutoSize = true;
            this.pluto_checkBox.Location = new System.Drawing.Point(356, 74);
            this.pluto_checkBox.Name = "pluto_checkBox";
            this.pluto_checkBox.Size = new System.Drawing.Size(65, 18);
            this.pluto_checkBox.TabIndex = 7;
            this.pluto_checkBox.Text = "Pluto";
            this.pluto_checkBox.UseVisualStyleBackColor = true;
            // 
            // neptune_checkBox
            // 
            this.neptune_checkBox.AutoSize = true;
            this.neptune_checkBox.Location = new System.Drawing.Point(268, 74);
            this.neptune_checkBox.Name = "neptune_checkBox";
            this.neptune_checkBox.Size = new System.Drawing.Size(86, 18);
            this.neptune_checkBox.TabIndex = 6;
            this.neptune_checkBox.Text = "Neptune";
            this.neptune_checkBox.UseVisualStyleBackColor = true;
            // 
            // uranus_checkBox
            // 
            this.uranus_checkBox.AutoSize = true;
            this.uranus_checkBox.Location = new System.Drawing.Point(169, 74);
            this.uranus_checkBox.Name = "uranus_checkBox";
            this.uranus_checkBox.Size = new System.Drawing.Size(78, 18);
            this.uranus_checkBox.TabIndex = 5;
            this.uranus_checkBox.Text = "Uranus";
            this.uranus_checkBox.UseVisualStyleBackColor = true;
            // 
            // saturn_checkBox
            // 
            this.saturn_checkBox.AutoSize = true;
            this.saturn_checkBox.Location = new System.Drawing.Point(85, 74);
            this.saturn_checkBox.Name = "saturn_checkBox";
            this.saturn_checkBox.Size = new System.Drawing.Size(74, 18);
            this.saturn_checkBox.TabIndex = 4;
            this.saturn_checkBox.Text = "Saturn";
            this.saturn_checkBox.UseVisualStyleBackColor = true;
            // 
            // jupiter_checkBox
            // 
            this.jupiter_checkBox.AutoSize = true;
            this.jupiter_checkBox.Location = new System.Drawing.Point(11, 74);
            this.jupiter_checkBox.Name = "jupiter_checkBox";
            this.jupiter_checkBox.Size = new System.Drawing.Size(74, 18);
            this.jupiter_checkBox.TabIndex = 3;
            this.jupiter_checkBox.Text = "Jupiter";
            this.jupiter_checkBox.UseVisualStyleBackColor = true;
            // 
            // mercury_checkBox
            // 
            this.mercury_checkBox.AutoSize = true;
            this.mercury_checkBox.Location = new System.Drawing.Point(169, 52);
            this.mercury_checkBox.Name = "mercury_checkBox";
            this.mercury_checkBox.Size = new System.Drawing.Size(85, 18);
            this.mercury_checkBox.TabIndex = 2;
            this.mercury_checkBox.Text = "Mercury";
            this.mercury_checkBox.UseVisualStyleBackColor = true;
            // 
            // moon_checkBox
            // 
            this.moon_checkBox.AutoSize = true;
            this.moon_checkBox.Location = new System.Drawing.Point(85, 52);
            this.moon_checkBox.Name = "moon_checkBox";
            this.moon_checkBox.Size = new System.Drawing.Size(68, 18);
            this.moon_checkBox.TabIndex = 1;
            this.moon_checkBox.Text = "Moon";
            this.moon_checkBox.UseVisualStyleBackColor = true;
            // 
            // sun_checkBox
            // 
            this.sun_checkBox.AutoSize = true;
            this.sun_checkBox.Location = new System.Drawing.Point(11, 52);
            this.sun_checkBox.Name = "sun_checkBox";
            this.sun_checkBox.Size = new System.Drawing.Size(55, 18);
            this.sun_checkBox.TabIndex = 0;
            this.sun_checkBox.Text = "Sun";
            this.sun_checkBox.UseVisualStyleBackColor = true;
            // 
            // atmos_groupBox
            // 
            this.atmos_groupBox.Controls.Add(this.sw_normal_radioButton);
            this.atmos_groupBox.Controls.Add(this.sw_data_radioButton);
            this.atmos_groupBox.Controls.Add(this.sw_label);
            this.atmos_groupBox.Controls.Add(this.atmosModel_comboBox);
            this.atmos_groupBox.Controls.Add(this.atmosModel_label);
            this.atmos_groupBox.Location = new System.Drawing.Point(42, 323);
            this.atmos_groupBox.Name = "atmos_groupBox";
            this.atmos_groupBox.Size = new System.Drawing.Size(426, 94);
            this.atmos_groupBox.TabIndex = 12;
            this.atmos_groupBox.TabStop = false;
            this.atmos_groupBox.Text = "Atmosphere";
            // 
            // sw_normal_radioButton
            // 
            this.sw_normal_radioButton.AutoSize = true;
            this.sw_normal_radioButton.Location = new System.Drawing.Point(278, 64);
            this.sw_normal_radioButton.Name = "sw_normal_radioButton";
            this.sw_normal_radioButton.Size = new System.Drawing.Size(75, 18);
            this.sw_normal_radioButton.TabIndex = 6;
            this.sw_normal_radioButton.TabStop = true;
            this.sw_normal_radioButton.Text = "Normal";
            this.sw_normal_radioButton.UseVisualStyleBackColor = true;
            // 
            // sw_data_radioButton
            // 
            this.sw_data_radioButton.AutoSize = true;
            this.sw_data_radioButton.Location = new System.Drawing.Point(172, 64);
            this.sw_data_radioButton.Name = "sw_data_radioButton";
            this.sw_data_radioButton.Size = new System.Drawing.Size(59, 18);
            this.sw_data_radioButton.TabIndex = 5;
            this.sw_data_radioButton.TabStop = true;
            this.sw_data_radioButton.Text = "Data";
            this.sw_data_radioButton.UseVisualStyleBackColor = true;
            // 
            // sw_label
            // 
            this.sw_label.AutoSize = true;
            this.sw_label.Location = new System.Drawing.Point(9, 66);
            this.sw_label.Name = "sw_label";
            this.sw_label.Size = new System.Drawing.Size(121, 14);
            this.sw_label.TabIndex = 4;
            this.sw_label.Text = "Space Weather";
            // 
            // atmosModel_comboBox
            // 
            this.atmosModel_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.atmosModel_comboBox.FormattingEnabled = true;
            this.atmosModel_comboBox.Location = new System.Drawing.Point(129, 31);
            this.atmosModel_comboBox.Name = "atmosModel_comboBox";
            this.atmosModel_comboBox.Size = new System.Drawing.Size(181, 21);
            this.atmosModel_comboBox.TabIndex = 3;
            // 
            // atmosModel_label
            // 
            this.atmosModel_label.AutoSize = true;
            this.atmosModel_label.Location = new System.Drawing.Point(33, 34);
            this.atmosModel_label.Name = "atmosModel_label";
            this.atmosModel_label.Size = new System.Drawing.Size(52, 14);
            this.atmosModel_label.TabIndex = 2;
            this.atmosModel_label.Text = "Model";
            // 
            // radiation_groupBox
            // 
            this.radiation_groupBox.Controls.Add(this.EarthRad_comboBox);
            this.radiation_groupBox.Controls.Add(this.earthRad_label);
            this.radiation_groupBox.Controls.Add(this.solarShadow_comboBox);
            this.radiation_groupBox.Controls.Add(this.solarShad_label);
            this.radiation_groupBox.Location = new System.Drawing.Point(42, 548);
            this.radiation_groupBox.Name = "radiation_groupBox";
            this.radiation_groupBox.Size = new System.Drawing.Size(426, 82);
            this.radiation_groupBox.TabIndex = 14;
            this.radiation_groupBox.TabStop = false;
            this.radiation_groupBox.Text = "Radiation Pressure";
            // 
            // EarthRad_comboBox
            // 
            this.EarthRad_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EarthRad_comboBox.FormattingEnabled = true;
            this.EarthRad_comboBox.Location = new System.Drawing.Point(169, 49);
            this.EarthRad_comboBox.Name = "EarthRad_comboBox";
            this.EarthRad_comboBox.Size = new System.Drawing.Size(181, 21);
            this.EarthRad_comboBox.TabIndex = 7;
            // 
            // earthRad_label
            // 
            this.earthRad_label.AutoSize = true;
            this.earthRad_label.Location = new System.Drawing.Point(33, 49);
            this.earthRad_label.Name = "earthRad_label";
            this.earthRad_label.Size = new System.Drawing.Size(121, 14);
            this.earthRad_label.TabIndex = 6;
            this.earthRad_label.Text = "Earth Radiation";
            // 
            // solarShadow_comboBox
            // 
            this.solarShadow_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.solarShadow_comboBox.FormattingEnabled = true;
            this.solarShadow_comboBox.Location = new System.Drawing.Point(169, 23);
            this.solarShadow_comboBox.Name = "solarShadow_comboBox";
            this.solarShadow_comboBox.Size = new System.Drawing.Size(181, 21);
            this.solarShadow_comboBox.TabIndex = 5;
            // 
            // solarShad_label
            // 
            this.solarShad_label.AutoSize = true;
            this.solarShad_label.Location = new System.Drawing.Point(33, 23);
            this.solarShad_label.Name = "solarShad_label";
            this.solarShad_label.Size = new System.Drawing.Size(111, 14);
            this.solarShad_label.TabIndex = 4;
            this.solarShad_label.Text = "Solar Shadow";
            // 
            // relCor_checkBox
            // 
            this.relCor_checkBox.AutoSize = true;
            this.relCor_checkBox.Location = new System.Drawing.Point(42, 638);
            this.relCor_checkBox.Name = "relCor_checkBox";
            this.relCor_checkBox.Size = new System.Drawing.Size(191, 18);
            this.relCor_checkBox.TabIndex = 15;
            this.relCor_checkBox.Text = "Relativistic Correction";
            this.relCor_checkBox.UseVisualStyleBackColor = true;
            // 
            // propagator_g
            // 
            this.propagator_g.Controls.Add(this.propType_comboBox);
            this.propagator_g.Controls.Add(this.propType_label);
            this.propagator_g.Location = new System.Drawing.Point(42, 38);
            this.propagator_g.Name = "propagator_g";
            this.propagator_g.Size = new System.Drawing.Size(426, 59);
            this.propagator_g.TabIndex = 8;
            this.propagator_g.TabStop = false;
            this.propagator_g.Text = "Propagator";
            // 
            // propType_comboBox
            // 
            this.propType_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.propType_comboBox.FormattingEnabled = true;
            this.propType_comboBox.Location = new System.Drawing.Point(129, 29);
            this.propType_comboBox.Name = "propType_comboBox";
            this.propType_comboBox.Size = new System.Drawing.Size(181, 21);
            this.propType_comboBox.TabIndex = 1;
            // 
            // propType_label
            // 
            this.propType_label.AutoSize = true;
            this.propType_label.Location = new System.Drawing.Point(33, 32);
            this.propType_label.Name = "propType_label";
            this.propType_label.Size = new System.Drawing.Size(43, 14);
            this.propType_label.TabIndex = 0;
            this.propType_label.Text = "Type";
            // 
            // gravity_groupBox
            // 
            this.gravity_groupBox.Controls.Add(this.tide_groupBox);
            this.gravity_groupBox.Controls.Add(this.gravMaxOrd_textBox);
            this.gravity_groupBox.Controls.Add(this.gravOrd_textBox);
            this.gravity_groupBox.Controls.Add(this.gravMaxDeg_textBox);
            this.gravity_groupBox.Controls.Add(this.gravDeg_textBox);
            this.gravity_groupBox.Controls.Add(this.gravOrder_label);
            this.gravity_groupBox.Controls.Add(this.gravDeg_label);
            this.gravity_groupBox.Controls.Add(this.gravModel_comboBox);
            this.gravity_groupBox.Controls.Add(this.gravModel_label);
            this.gravity_groupBox.Location = new System.Drawing.Point(42, 111);
            this.gravity_groupBox.Name = "gravity_groupBox";
            this.gravity_groupBox.Size = new System.Drawing.Size(426, 191);
            this.gravity_groupBox.TabIndex = 9;
            this.gravity_groupBox.TabStop = false;
            this.gravity_groupBox.Text = "Gravity";
            // 
            // tide_groupBox
            // 
            this.tide_groupBox.Controls.Add(this.oceanTide_comboBox);
            this.tide_groupBox.Controls.Add(this.oceanTide_label);
            this.tide_groupBox.Controls.Add(this.permanent_checkBox);
            this.tide_groupBox.Controls.Add(this.oceanPole_checkBox);
            this.tide_groupBox.Controls.Add(this.solidPole_checkBox);
            this.tide_groupBox.Controls.Add(this.EarthTide_checkBox);
            this.tide_groupBox.Location = new System.Drawing.Point(11, 91);
            this.tide_groupBox.Name = "tide_groupBox";
            this.tide_groupBox.Size = new System.Drawing.Size(398, 83);
            this.tide_groupBox.TabIndex = 8;
            this.tide_groupBox.TabStop = false;
            this.tide_groupBox.Text = "Tide";
            // 
            // oceanTide_comboBox
            // 
            this.oceanTide_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.oceanTide_comboBox.FormattingEnabled = true;
            this.oceanTide_comboBox.Location = new System.Drawing.Point(118, 55);
            this.oceanTide_comboBox.Name = "oceanTide_comboBox";
            this.oceanTide_comboBox.Size = new System.Drawing.Size(181, 21);
            this.oceanTide_comboBox.TabIndex = 5;
            // 
            // oceanTide_label
            // 
            this.oceanTide_label.AutoSize = true;
            this.oceanTide_label.Location = new System.Drawing.Point(17, 58);
            this.oceanTide_label.Name = "oceanTide_label";
            this.oceanTide_label.Size = new System.Drawing.Size(93, 14);
            this.oceanTide_label.TabIndex = 4;
            this.oceanTide_label.Text = "Ocean Tide";
            // 
            // permanent_checkBox
            // 
            this.permanent_checkBox.AutoSize = true;
            this.permanent_checkBox.Location = new System.Drawing.Point(290, 28);
            this.permanent_checkBox.Name = "permanent_checkBox";
            this.permanent_checkBox.Size = new System.Drawing.Size(104, 18);
            this.permanent_checkBox.TabIndex = 3;
            this.permanent_checkBox.Text = "Permanent";
            this.permanent_checkBox.UseVisualStyleBackColor = true;
            // 
            // oceanPole_checkBox
            // 
            this.oceanPole_checkBox.AutoSize = true;
            this.oceanPole_checkBox.Location = new System.Drawing.Point(184, 28);
            this.oceanPole_checkBox.Name = "oceanPole_checkBox";
            this.oceanPole_checkBox.Size = new System.Drawing.Size(115, 18);
            this.oceanPole_checkBox.TabIndex = 2;
            this.oceanPole_checkBox.Text = "Ocean Pole";
            this.oceanPole_checkBox.UseVisualStyleBackColor = true;
            // 
            // solidPole_checkBox
            // 
            this.solidPole_checkBox.AutoSize = true;
            this.solidPole_checkBox.Location = new System.Drawing.Point(87, 28);
            this.solidPole_checkBox.Name = "solidPole_checkBox";
            this.solidPole_checkBox.Size = new System.Drawing.Size(104, 18);
            this.solidPole_checkBox.TabIndex = 1;
            this.solidPole_checkBox.Text = "Solid Pole";
            this.solidPole_checkBox.UseVisualStyleBackColor = true;
            // 
            // EarthTide_checkBox
            // 
            this.EarthTide_checkBox.AutoSize = true;
            this.EarthTide_checkBox.Location = new System.Drawing.Point(18, 28);
            this.EarthTide_checkBox.Name = "EarthTide_checkBox";
            this.EarthTide_checkBox.Size = new System.Drawing.Size(64, 18);
            this.EarthTide_checkBox.TabIndex = 0;
            this.EarthTide_checkBox.Text = "Earth";
            this.EarthTide_checkBox.UseVisualStyleBackColor = true;
            // 
            // gravMaxOrd_textBox
            // 
            this.gravMaxOrd_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gravMaxOrd_textBox.Location = new System.Drawing.Point(375, 56);
            this.gravMaxOrd_textBox.Name = "gravMaxOrd_textBox";
            this.gravMaxOrd_textBox.ReadOnly = true;
            this.gravMaxOrd_textBox.Size = new System.Drawing.Size(35, 16);
            this.gravMaxOrd_textBox.TabIndex = 7;
            // 
            // gravOrd_textBox
            // 
            this.gravOrd_textBox.Location = new System.Drawing.Point(312, 53);
            this.gravOrd_textBox.Name = "gravOrd_textBox";
            this.gravOrd_textBox.Size = new System.Drawing.Size(35, 23);
            this.gravOrd_textBox.TabIndex = 6;
            // 
            // gravMaxDeg_textBox
            // 
            this.gravMaxDeg_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gravMaxDeg_textBox.Location = new System.Drawing.Point(194, 56);
            this.gravMaxDeg_textBox.Name = "gravMaxDeg_textBox";
            this.gravMaxDeg_textBox.ReadOnly = true;
            this.gravMaxDeg_textBox.Size = new System.Drawing.Size(35, 16);
            this.gravMaxDeg_textBox.TabIndex = 5;
            // 
            // gravDeg_textBox
            // 
            this.gravDeg_textBox.Location = new System.Drawing.Point(129, 53);
            this.gravDeg_textBox.Name = "gravDeg_textBox";
            this.gravDeg_textBox.Size = new System.Drawing.Size(35, 23);
            this.gravDeg_textBox.TabIndex = 4;
            // 
            // gravOrder_label
            // 
            this.gravOrder_label.AutoSize = true;
            this.gravOrder_label.Location = new System.Drawing.Point(250, 57);
            this.gravOrder_label.Name = "gravOrder_label";
            this.gravOrder_label.Size = new System.Drawing.Size(48, 14);
            this.gravOrder_label.TabIndex = 3;
            this.gravOrder_label.Text = "Order";
            // 
            // gravDeg_label
            // 
            this.gravDeg_label.AutoSize = true;
            this.gravDeg_label.Location = new System.Drawing.Point(33, 57);
            this.gravDeg_label.Name = "gravDeg_label";
            this.gravDeg_label.Size = new System.Drawing.Size(59, 14);
            this.gravDeg_label.TabIndex = 2;
            this.gravDeg_label.Text = "Degree";
            // 
            // gravModel_comboBox
            // 
            this.gravModel_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gravModel_comboBox.FormattingEnabled = true;
            this.gravModel_comboBox.Location = new System.Drawing.Point(129, 25);
            this.gravModel_comboBox.Name = "gravModel_comboBox";
            this.gravModel_comboBox.Size = new System.Drawing.Size(181, 21);
            this.gravModel_comboBox.TabIndex = 1;
            // 
            // gravModel_label
            // 
            this.gravModel_label.AutoSize = true;
            this.gravModel_label.Location = new System.Drawing.Point(33, 28);
            this.gravModel_label.Name = "gravModel_label";
            this.gravModel_label.Size = new System.Drawing.Size(52, 14);
            this.gravModel_label.TabIndex = 0;
            this.gravModel_label.Text = "Model";
            // 
            // UC_SetDynamics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "UC_SetDynamics";
            this.Size = new System.Drawing.Size(550, 754);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.masses_groupBox.ResumeLayout(false);
            this.masses_groupBox.PerformLayout();
            this.atmos_groupBox.ResumeLayout(false);
            this.atmos_groupBox.PerformLayout();
            this.radiation_groupBox.ResumeLayout(false);
            this.radiation_groupBox.PerformLayout();
            this.propagator_g.ResumeLayout(false);
            this.propagator_g.PerformLayout();
            this.gravity_groupBox.ResumeLayout(false);
            this.gravity_groupBox.PerformLayout();
            this.tide_groupBox.ResumeLayout(false);
            this.tide_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox masses_groupBox;
        private System.Windows.Forms.ComboBox ephem_comboBox;
        private System.Windows.Forms.Label ephem_label;
        private System.Windows.Forms.CheckBox venus_checkBox;
        private System.Windows.Forms.CheckBox mars_checkBox;
        private System.Windows.Forms.CheckBox pluto_checkBox;
        private System.Windows.Forms.CheckBox neptune_checkBox;
        private System.Windows.Forms.CheckBox uranus_checkBox;
        private System.Windows.Forms.CheckBox saturn_checkBox;
        private System.Windows.Forms.CheckBox jupiter_checkBox;
        private System.Windows.Forms.CheckBox mercury_checkBox;
        private System.Windows.Forms.CheckBox moon_checkBox;
        private System.Windows.Forms.CheckBox sun_checkBox;
        private System.Windows.Forms.GroupBox atmos_groupBox;
        private System.Windows.Forms.RadioButton sw_normal_radioButton;
        private System.Windows.Forms.RadioButton sw_data_radioButton;
        private System.Windows.Forms.Label sw_label;
        private System.Windows.Forms.ComboBox atmosModel_comboBox;
        private System.Windows.Forms.Label atmosModel_label;
        private System.Windows.Forms.GroupBox radiation_groupBox;
        private System.Windows.Forms.ComboBox EarthRad_comboBox;
        private System.Windows.Forms.Label earthRad_label;
        private System.Windows.Forms.ComboBox solarShadow_comboBox;
        private System.Windows.Forms.Label solarShad_label;
        private System.Windows.Forms.CheckBox relCor_checkBox;
        private System.Windows.Forms.GroupBox propagator_g;
        private System.Windows.Forms.ComboBox propType_comboBox;
        private System.Windows.Forms.Label propType_label;
        private System.Windows.Forms.GroupBox gravity_groupBox;
        private System.Windows.Forms.GroupBox tide_groupBox;
        private System.Windows.Forms.ComboBox oceanTide_comboBox;
        private System.Windows.Forms.Label oceanTide_label;
        private System.Windows.Forms.CheckBox permanent_checkBox;
        private System.Windows.Forms.CheckBox oceanPole_checkBox;
        private System.Windows.Forms.CheckBox solidPole_checkBox;
        private System.Windows.Forms.CheckBox EarthTide_checkBox;
        private System.Windows.Forms.TextBox gravMaxOrd_textBox;
        private System.Windows.Forms.TextBox gravOrd_textBox;
        private System.Windows.Forms.TextBox gravMaxDeg_textBox;
        private System.Windows.Forms.TextBox gravDeg_textBox;
        private System.Windows.Forms.Label gravOrder_label;
        private System.Windows.Forms.Label gravDeg_label;
        private System.Windows.Forms.ComboBox gravModel_comboBox;
        private System.Windows.Forms.Label gravModel_label;
    }
}
