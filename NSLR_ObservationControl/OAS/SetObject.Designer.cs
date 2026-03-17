namespace NSLR_ObservationControl.OAS
{
    partial class SetObject
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.orbit_groupBox = new System.Windows.Forms.GroupBox();
            this.coordinate_groupBox = new System.Windows.Forms.GroupBox();
            this.states_groupBox = new System.Windows.Forms.GroupBox();
            this.epoch_groupBox = new System.Windows.Forms.GroupBox();
            this.property_groupBox = new System.Windows.Forms.GroupBox();
            this.cancel_button = new System.Windows.Forms.Button();
            this.ok_button = new System.Windows.Forms.Button();
            this.coordSys_comboBox = new System.Windows.Forms.ComboBox();
            this.timeSys_comboBox = new System.Windows.Forms.ComboBox();
            this.timeSystem_label = new System.Windows.Forms.Label();
            this.timeType_label = new System.Windows.Forms.Label();
            this.timeType_comboBox = new System.Windows.Forms.ComboBox();
            this.epoch1_textBox = new System.Windows.Forms.TextBox();
            this.epoch2_textBox = new System.Windows.Forms.TextBox();
            this.epoch3_textBox = new System.Windows.Forms.TextBox();
            this.epoch5_textBox = new System.Windows.Forms.TextBox();
            this.epoch4_textBox = new System.Windows.Forms.TextBox();
            this.epoch6_textBox = new System.Windows.Forms.TextBox();
            this.epochMJD_textBox = new System.Windows.Forms.TextBox();
            this.stateType_comboBox = new System.Windows.Forms.ComboBox();
            this.stateType_label = new System.Windows.Forms.Label();
            this.state1_label = new System.Windows.Forms.Label();
            this.state1_textBox = new System.Windows.Forms.TextBox();
            this.state2_textBox = new System.Windows.Forms.TextBox();
            this.state2_label = new System.Windows.Forms.Label();
            this.state3_textBox = new System.Windows.Forms.TextBox();
            this.state3_label = new System.Windows.Forms.Label();
            this.state4_textBox = new System.Windows.Forms.TextBox();
            this.state4_label = new System.Windows.Forms.Label();
            this.state5_textBox = new System.Windows.Forms.TextBox();
            this.state5_label = new System.Windows.Forms.Label();
            this.state6_textBox = new System.Windows.Forms.TextBox();
            this.state6_label = new System.Windows.Forms.Label();
            this.mass_textBox = new System.Windows.Forms.TextBox();
            this.mass_label = new System.Windows.Forms.Label();
            this.Cd_label = new System.Windows.Forms.Label();
            this.Cd_textBox = new System.Windows.Forms.TextBox();
            this.Cr_label = new System.Windows.Forms.Label();
            this.Cr_textBox = new System.Windows.Forms.TextBox();
            this.Ck_label = new System.Windows.Forms.Label();
            this.Ck_textBox = new System.Windows.Forms.TextBox();
            this.erpArea_label = new System.Windows.Forms.Label();
            this.erpArea_textBox = new System.Windows.Forms.TextBox();
            this.srpArea_label = new System.Windows.Forms.Label();
            this.srpArea_textBox = new System.Windows.Forms.TextBox();
            this.dragArea_label = new System.Windows.Forms.Label();
            this.dragArea_textBox = new System.Windows.Forms.TextBox();
            this.orbit_groupBox.SuspendLayout();
            this.coordinate_groupBox.SuspendLayout();
            this.states_groupBox.SuspendLayout();
            this.epoch_groupBox.SuspendLayout();
            this.property_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // orbit_groupBox
            // 
            this.orbit_groupBox.Controls.Add(this.coordinate_groupBox);
            this.orbit_groupBox.Controls.Add(this.states_groupBox);
            this.orbit_groupBox.Controls.Add(this.epoch_groupBox);
            this.orbit_groupBox.Location = new System.Drawing.Point(20, 21);
            this.orbit_groupBox.Name = "orbit_groupBox";
            this.orbit_groupBox.Size = new System.Drawing.Size(462, 292);
            this.orbit_groupBox.TabIndex = 0;
            this.orbit_groupBox.TabStop = false;
            this.orbit_groupBox.Text = "Orbit";
            // 
            // coordinate_groupBox
            // 
            this.coordinate_groupBox.Controls.Add(this.coordSys_comboBox);
            this.coordinate_groupBox.Location = new System.Drawing.Point(19, 202);
            this.coordinate_groupBox.Name = "coordinate_groupBox";
            this.coordinate_groupBox.Size = new System.Drawing.Size(200, 66);
            this.coordinate_groupBox.TabIndex = 1;
            this.coordinate_groupBox.TabStop = false;
            this.coordinate_groupBox.Text = "Coordinate System";
            // 
            // states_groupBox
            // 
            this.states_groupBox.Controls.Add(this.state6_textBox);
            this.states_groupBox.Controls.Add(this.state6_label);
            this.states_groupBox.Controls.Add(this.state5_textBox);
            this.states_groupBox.Controls.Add(this.state5_label);
            this.states_groupBox.Controls.Add(this.state4_textBox);
            this.states_groupBox.Controls.Add(this.state4_label);
            this.states_groupBox.Controls.Add(this.state3_textBox);
            this.states_groupBox.Controls.Add(this.state3_label);
            this.states_groupBox.Controls.Add(this.state2_textBox);
            this.states_groupBox.Controls.Add(this.state2_label);
            this.states_groupBox.Controls.Add(this.state1_textBox);
            this.states_groupBox.Controls.Add(this.state1_label);
            this.states_groupBox.Controls.Add(this.stateType_comboBox);
            this.states_groupBox.Controls.Add(this.stateType_label);
            this.states_groupBox.Location = new System.Drawing.Point(225, 31);
            this.states_groupBox.Name = "states_groupBox";
            this.states_groupBox.Size = new System.Drawing.Size(220, 237);
            this.states_groupBox.TabIndex = 1;
            this.states_groupBox.TabStop = false;
            this.states_groupBox.Text = "States";
            // 
            // epoch_groupBox
            // 
            this.epoch_groupBox.Controls.Add(this.epochMJD_textBox);
            this.epoch_groupBox.Controls.Add(this.epoch6_textBox);
            this.epoch_groupBox.Controls.Add(this.epoch5_textBox);
            this.epoch_groupBox.Controls.Add(this.epoch4_textBox);
            this.epoch_groupBox.Controls.Add(this.epoch3_textBox);
            this.epoch_groupBox.Controls.Add(this.epoch2_textBox);
            this.epoch_groupBox.Controls.Add(this.epoch1_textBox);
            this.epoch_groupBox.Controls.Add(this.timeType_comboBox);
            this.epoch_groupBox.Controls.Add(this.timeType_label);
            this.epoch_groupBox.Controls.Add(this.timeSys_comboBox);
            this.epoch_groupBox.Controls.Add(this.timeSystem_label);
            this.epoch_groupBox.Location = new System.Drawing.Point(19, 31);
            this.epoch_groupBox.Name = "epoch_groupBox";
            this.epoch_groupBox.Size = new System.Drawing.Size(200, 146);
            this.epoch_groupBox.TabIndex = 0;
            this.epoch_groupBox.TabStop = false;
            this.epoch_groupBox.Text = "Epoch Time";
            // 
            // property_groupBox
            // 
            this.property_groupBox.Controls.Add(this.erpArea_label);
            this.property_groupBox.Controls.Add(this.erpArea_textBox);
            this.property_groupBox.Controls.Add(this.srpArea_label);
            this.property_groupBox.Controls.Add(this.srpArea_textBox);
            this.property_groupBox.Controls.Add(this.dragArea_label);
            this.property_groupBox.Controls.Add(this.dragArea_textBox);
            this.property_groupBox.Controls.Add(this.Ck_label);
            this.property_groupBox.Controls.Add(this.Ck_textBox);
            this.property_groupBox.Controls.Add(this.Cr_label);
            this.property_groupBox.Controls.Add(this.Cr_textBox);
            this.property_groupBox.Controls.Add(this.Cd_label);
            this.property_groupBox.Controls.Add(this.Cd_textBox);
            this.property_groupBox.Controls.Add(this.mass_label);
            this.property_groupBox.Controls.Add(this.mass_textBox);
            this.property_groupBox.Location = new System.Drawing.Point(20, 347);
            this.property_groupBox.Name = "property_groupBox";
            this.property_groupBox.Size = new System.Drawing.Size(462, 167);
            this.property_groupBox.TabIndex = 1;
            this.property_groupBox.TabStop = false;
            this.property_groupBox.Text = "Property";
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(407, 549);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(75, 23);
            this.cancel_button.TabIndex = 5;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // ok_button
            // 
            this.ok_button.Location = new System.Drawing.Point(310, 549);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(75, 23);
            this.ok_button.TabIndex = 4;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // coordSys_comboBox
            // 
            this.coordSys_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coordSys_comboBox.FormattingEnabled = true;
            this.coordSys_comboBox.Location = new System.Drawing.Point(10, 23);
            this.coordSys_comboBox.Name = "coordSys_comboBox";
            this.coordSys_comboBox.Size = new System.Drawing.Size(181, 20);
            this.coordSys_comboBox.TabIndex = 2;
            // 
            // timeSys_comboBox
            // 
            this.timeSys_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.timeSys_comboBox.FormattingEnabled = true;
            this.timeSys_comboBox.Location = new System.Drawing.Point(94, 32);
            this.timeSys_comboBox.Name = "timeSys_comboBox";
            this.timeSys_comboBox.Size = new System.Drawing.Size(97, 20);
            this.timeSys_comboBox.TabIndex = 3;
            // 
            // timeSystem_label
            // 
            this.timeSystem_label.AutoSize = true;
            this.timeSystem_label.Location = new System.Drawing.Point(20, 35);
            this.timeSystem_label.Name = "timeSystem_label";
            this.timeSystem_label.Size = new System.Drawing.Size(48, 12);
            this.timeSystem_label.TabIndex = 2;
            this.timeSystem_label.Text = "System";
            // 
            // timeType_label
            // 
            this.timeType_label.AutoSize = true;
            this.timeType_label.Location = new System.Drawing.Point(26, 66);
            this.timeType_label.Name = "timeType_label";
            this.timeType_label.Size = new System.Drawing.Size(34, 12);
            this.timeType_label.TabIndex = 4;
            this.timeType_label.Text = "Type";
            // 
            // timeType_comboBox
            // 
            this.timeType_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.timeType_comboBox.FormattingEnabled = true;
            this.timeType_comboBox.Location = new System.Drawing.Point(94, 63);
            this.timeType_comboBox.Name = "timeType_comboBox";
            this.timeType_comboBox.Size = new System.Drawing.Size(97, 20);
            this.timeType_comboBox.TabIndex = 5;
            this.timeType_comboBox.SelectedIndexChanged += new System.EventHandler(this.timeType_comboBox_SelectedIndexChanged);
            // 
            // epoch1_textBox
            // 
            this.epoch1_textBox.Location = new System.Drawing.Point(10, 89);
            this.epoch1_textBox.Name = "epoch1_textBox";
            this.epoch1_textBox.Size = new System.Drawing.Size(40, 21);
            this.epoch1_textBox.TabIndex = 6;
            // 
            // epoch2_textBox
            // 
            this.epoch2_textBox.Location = new System.Drawing.Point(55, 89);
            this.epoch2_textBox.Name = "epoch2_textBox";
            this.epoch2_textBox.Size = new System.Drawing.Size(24, 21);
            this.epoch2_textBox.TabIndex = 7;
            // 
            // epoch3_textBox
            // 
            this.epoch3_textBox.Location = new System.Drawing.Point(84, 89);
            this.epoch3_textBox.Name = "epoch3_textBox";
            this.epoch3_textBox.Size = new System.Drawing.Size(24, 21);
            this.epoch3_textBox.TabIndex = 8;
            // 
            // epoch5_textBox
            // 
            this.epoch5_textBox.Location = new System.Drawing.Point(142, 89);
            this.epoch5_textBox.Name = "epoch5_textBox";
            this.epoch5_textBox.Size = new System.Drawing.Size(24, 21);
            this.epoch5_textBox.TabIndex = 10;
            // 
            // epoch4_textBox
            // 
            this.epoch4_textBox.Location = new System.Drawing.Point(113, 89);
            this.epoch4_textBox.Name = "epoch4_textBox";
            this.epoch4_textBox.Size = new System.Drawing.Size(24, 21);
            this.epoch4_textBox.TabIndex = 9;
            // 
            // epoch6_textBox
            // 
            this.epoch6_textBox.Location = new System.Drawing.Point(171, 89);
            this.epoch6_textBox.Name = "epoch6_textBox";
            this.epoch6_textBox.Size = new System.Drawing.Size(24, 21);
            this.epoch6_textBox.TabIndex = 11;
            // 
            // epochMJD_textBox
            // 
            this.epochMJD_textBox.Location = new System.Drawing.Point(113, 116);
            this.epochMJD_textBox.Name = "epochMJD_textBox";
            this.epochMJD_textBox.Size = new System.Drawing.Size(82, 21);
            this.epochMJD_textBox.TabIndex = 12;
            // 
            // stateType_comboBox
            // 
            this.stateType_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stateType_comboBox.FormattingEnabled = true;
            this.stateType_comboBox.Location = new System.Drawing.Point(106, 32);
            this.stateType_comboBox.Name = "stateType_comboBox";
            this.stateType_comboBox.Size = new System.Drawing.Size(97, 20);
            this.stateType_comboBox.TabIndex = 7;
            // 
            // stateType_label
            // 
            this.stateType_label.AutoSize = true;
            this.stateType_label.Location = new System.Drawing.Point(35, 33);
            this.stateType_label.Name = "stateType_label";
            this.stateType_label.Size = new System.Drawing.Size(34, 12);
            this.stateType_label.TabIndex = 6;
            this.stateType_label.Text = "Type";
            this.stateType_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // state1_label
            // 
            this.state1_label.AutoSize = true;
            this.state1_label.Location = new System.Drawing.Point(35, 72);
            this.state1_label.Name = "state1_label";
            this.state1_label.Size = new System.Drawing.Size(34, 12);
            this.state1_label.TabIndex = 8;
            this.state1_label.Text = "Type";
            this.state1_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // state1_textBox
            // 
            this.state1_textBox.Location = new System.Drawing.Point(106, 71);
            this.state1_textBox.Name = "state1_textBox";
            this.state1_textBox.Size = new System.Drawing.Size(97, 21);
            this.state1_textBox.TabIndex = 13;
            // 
            // state2_textBox
            // 
            this.state2_textBox.Location = new System.Drawing.Point(106, 97);
            this.state2_textBox.Name = "state2_textBox";
            this.state2_textBox.Size = new System.Drawing.Size(97, 21);
            this.state2_textBox.TabIndex = 15;
            // 
            // state2_label
            // 
            this.state2_label.AutoSize = true;
            this.state2_label.Location = new System.Drawing.Point(35, 98);
            this.state2_label.Name = "state2_label";
            this.state2_label.Size = new System.Drawing.Size(34, 12);
            this.state2_label.TabIndex = 14;
            this.state2_label.Text = "Type";
            this.state2_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // state3_textBox
            // 
            this.state3_textBox.Location = new System.Drawing.Point(106, 123);
            this.state3_textBox.Name = "state3_textBox";
            this.state3_textBox.Size = new System.Drawing.Size(97, 21);
            this.state3_textBox.TabIndex = 17;
            // 
            // state3_label
            // 
            this.state3_label.AutoSize = true;
            this.state3_label.Location = new System.Drawing.Point(35, 124);
            this.state3_label.Name = "state3_label";
            this.state3_label.Size = new System.Drawing.Size(34, 12);
            this.state3_label.TabIndex = 16;
            this.state3_label.Text = "Type";
            this.state3_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // state4_textBox
            // 
            this.state4_textBox.Location = new System.Drawing.Point(106, 149);
            this.state4_textBox.Name = "state4_textBox";
            this.state4_textBox.Size = new System.Drawing.Size(97, 21);
            this.state4_textBox.TabIndex = 19;
            // 
            // state4_label
            // 
            this.state4_label.AutoSize = true;
            this.state4_label.Location = new System.Drawing.Point(35, 150);
            this.state4_label.Name = "state4_label";
            this.state4_label.Size = new System.Drawing.Size(34, 12);
            this.state4_label.TabIndex = 18;
            this.state4_label.Text = "Type";
            this.state4_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // state5_textBox
            // 
            this.state5_textBox.Location = new System.Drawing.Point(106, 175);
            this.state5_textBox.Name = "state5_textBox";
            this.state5_textBox.Size = new System.Drawing.Size(97, 21);
            this.state5_textBox.TabIndex = 21;
            // 
            // state5_label
            // 
            this.state5_label.AutoSize = true;
            this.state5_label.Location = new System.Drawing.Point(35, 176);
            this.state5_label.Name = "state5_label";
            this.state5_label.Size = new System.Drawing.Size(34, 12);
            this.state5_label.TabIndex = 20;
            this.state5_label.Text = "Type";
            this.state5_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // state6_textBox
            // 
            this.state6_textBox.Location = new System.Drawing.Point(106, 201);
            this.state6_textBox.Name = "state6_textBox";
            this.state6_textBox.Size = new System.Drawing.Size(97, 21);
            this.state6_textBox.TabIndex = 23;
            // 
            // state6_label
            // 
            this.state6_label.AutoSize = true;
            this.state6_label.Location = new System.Drawing.Point(35, 202);
            this.state6_label.Name = "state6_label";
            this.state6_label.Size = new System.Drawing.Size(34, 12);
            this.state6_label.TabIndex = 22;
            this.state6_label.Text = "Type";
            this.state6_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mass_textBox
            // 
            this.mass_textBox.Location = new System.Drawing.Point(130, 42);
            this.mass_textBox.Name = "mass_textBox";
            this.mass_textBox.Size = new System.Drawing.Size(100, 21);
            this.mass_textBox.TabIndex = 0;
            // 
            // mass_label
            // 
            this.mass_label.AutoSize = true;
            this.mass_label.Location = new System.Drawing.Point(30, 45);
            this.mass_label.Name = "mass_label";
            this.mass_label.Size = new System.Drawing.Size(64, 12);
            this.mass_label.TabIndex = 1;
            this.mass_label.Text = "Mass (kg)";
            // 
            // Cd_label
            // 
            this.Cd_label.AutoSize = true;
            this.Cd_label.Location = new System.Drawing.Point(276, 71);
            this.Cd_label.Name = "Cd_label";
            this.Cd_label.Size = new System.Drawing.Size(21, 12);
            this.Cd_label.TabIndex = 3;
            this.Cd_label.Text = "Cd";
            // 
            // Cd_textBox
            // 
            this.Cd_textBox.Location = new System.Drawing.Point(331, 68);
            this.Cd_textBox.Name = "Cd_textBox";
            this.Cd_textBox.Size = new System.Drawing.Size(100, 21);
            this.Cd_textBox.TabIndex = 2;
            // 
            // Cr_label
            // 
            this.Cr_label.AutoSize = true;
            this.Cr_label.Location = new System.Drawing.Point(276, 98);
            this.Cr_label.Name = "Cr_label";
            this.Cr_label.Size = new System.Drawing.Size(18, 12);
            this.Cr_label.TabIndex = 5;
            this.Cr_label.Text = "Cr";
            // 
            // Cr_textBox
            // 
            this.Cr_textBox.Location = new System.Drawing.Point(331, 95);
            this.Cr_textBox.Name = "Cr_textBox";
            this.Cr_textBox.Size = new System.Drawing.Size(100, 21);
            this.Cr_textBox.TabIndex = 4;
            // 
            // Ck_label
            // 
            this.Ck_label.AutoSize = true;
            this.Ck_label.Location = new System.Drawing.Point(276, 125);
            this.Ck_label.Name = "Ck_label";
            this.Ck_label.Size = new System.Drawing.Size(20, 12);
            this.Ck_label.TabIndex = 7;
            this.Ck_label.Text = "Ck";
            // 
            // Ck_textBox
            // 
            this.Ck_textBox.Location = new System.Drawing.Point(331, 122);
            this.Ck_textBox.Name = "Ck_textBox";
            this.Ck_textBox.Size = new System.Drawing.Size(100, 21);
            this.Ck_textBox.TabIndex = 6;
            // 
            // erpArea_label
            // 
            this.erpArea_label.AutoSize = true;
            this.erpArea_label.Location = new System.Drawing.Point(17, 128);
            this.erpArea_label.Name = "erpArea_label";
            this.erpArea_label.Size = new System.Drawing.Size(90, 12);
            this.erpArea_label.TabIndex = 19;
            this.erpArea_label.Text = "ERP Area (m2)";
            // 
            // erpArea_textBox
            // 
            this.erpArea_textBox.Location = new System.Drawing.Point(130, 125);
            this.erpArea_textBox.Name = "erpArea_textBox";
            this.erpArea_textBox.Size = new System.Drawing.Size(100, 21);
            this.erpArea_textBox.TabIndex = 18;
            // 
            // srpArea_label
            // 
            this.srpArea_label.AutoSize = true;
            this.srpArea_label.Location = new System.Drawing.Point(17, 101);
            this.srpArea_label.Name = "srpArea_label";
            this.srpArea_label.Size = new System.Drawing.Size(90, 12);
            this.srpArea_label.TabIndex = 17;
            this.srpArea_label.Text = "SRP Area (m2)";
            // 
            // srpArea_textBox
            // 
            this.srpArea_textBox.Location = new System.Drawing.Point(130, 98);
            this.srpArea_textBox.Name = "srpArea_textBox";
            this.srpArea_textBox.Size = new System.Drawing.Size(100, 21);
            this.srpArea_textBox.TabIndex = 16;
            // 
            // dragArea_label
            // 
            this.dragArea_label.AutoSize = true;
            this.dragArea_label.Location = new System.Drawing.Point(17, 74);
            this.dragArea_label.Name = "dragArea_label";
            this.dragArea_label.Size = new System.Drawing.Size(92, 12);
            this.dragArea_label.TabIndex = 15;
            this.dragArea_label.Text = "Drag Area (m2)";
            // 
            // dragArea_textBox
            // 
            this.dragArea_textBox.Location = new System.Drawing.Point(130, 71);
            this.dragArea_textBox.Name = "dragArea_textBox";
            this.dragArea_textBox.Size = new System.Drawing.Size(100, 21);
            this.dragArea_textBox.TabIndex = 14;
            // 
            // SetObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 584);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.property_groupBox);
            this.Controls.Add(this.orbit_groupBox);
            this.Name = "SetObject";
            this.Text = "SetObject";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetObject_FormClosing);
            this.Load += new System.EventHandler(this.SetObject_Load);
            this.orbit_groupBox.ResumeLayout(false);
            this.coordinate_groupBox.ResumeLayout(false);
            this.states_groupBox.ResumeLayout(false);
            this.states_groupBox.PerformLayout();
            this.epoch_groupBox.ResumeLayout(false);
            this.epoch_groupBox.PerformLayout();
            this.property_groupBox.ResumeLayout(false);
            this.property_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox orbit_groupBox;
        private System.Windows.Forms.GroupBox property_groupBox;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.GroupBox coordinate_groupBox;
        private System.Windows.Forms.GroupBox states_groupBox;
        private System.Windows.Forms.GroupBox epoch_groupBox;
        private System.Windows.Forms.ComboBox coordSys_comboBox;
        private System.Windows.Forms.ComboBox timeSys_comboBox;
        private System.Windows.Forms.Label timeSystem_label;
        private System.Windows.Forms.TextBox state6_textBox;
        private System.Windows.Forms.Label state6_label;
        private System.Windows.Forms.TextBox state5_textBox;
        private System.Windows.Forms.Label state5_label;
        private System.Windows.Forms.TextBox state4_textBox;
        private System.Windows.Forms.Label state4_label;
        private System.Windows.Forms.TextBox state3_textBox;
        private System.Windows.Forms.Label state3_label;
        private System.Windows.Forms.TextBox state2_textBox;
        private System.Windows.Forms.Label state2_label;
        private System.Windows.Forms.TextBox state1_textBox;
        private System.Windows.Forms.Label state1_label;
        private System.Windows.Forms.ComboBox stateType_comboBox;
        private System.Windows.Forms.Label stateType_label;
        private System.Windows.Forms.TextBox epochMJD_textBox;
        private System.Windows.Forms.TextBox epoch6_textBox;
        private System.Windows.Forms.TextBox epoch5_textBox;
        private System.Windows.Forms.TextBox epoch4_textBox;
        private System.Windows.Forms.TextBox epoch3_textBox;
        private System.Windows.Forms.TextBox epoch2_textBox;
        private System.Windows.Forms.TextBox epoch1_textBox;
        private System.Windows.Forms.ComboBox timeType_comboBox;
        private System.Windows.Forms.Label timeType_label;
        private System.Windows.Forms.Label erpArea_label;
        private System.Windows.Forms.TextBox erpArea_textBox;
        private System.Windows.Forms.Label srpArea_label;
        private System.Windows.Forms.TextBox srpArea_textBox;
        private System.Windows.Forms.Label dragArea_label;
        private System.Windows.Forms.TextBox dragArea_textBox;
        private System.Windows.Forms.Label Ck_label;
        private System.Windows.Forms.TextBox Ck_textBox;
        private System.Windows.Forms.Label Cr_label;
        private System.Windows.Forms.TextBox Cr_textBox;
        private System.Windows.Forms.Label Cd_label;
        private System.Windows.Forms.TextBox Cd_textBox;
        private System.Windows.Forms.Label mass_label;
        private System.Windows.Forms.TextBox mass_textBox;
    }
}