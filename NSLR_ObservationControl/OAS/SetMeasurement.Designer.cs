namespace NSLR_ObservationControl.OAS
{
    partial class SetMeasurement
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
            this.ltt_groupBox = new System.Windows.Forms.GroupBox();
            this.com_groupBox = new System.Windows.Forms.GroupBox();
            this.refP_groupBox = new System.Windows.Forms.GroupBox();
            this.relDelay_checkBox = new System.Windows.Forms.CheckBox();
            this.tropoDelay_checkBox = new System.Windows.Forms.CheckBox();
            this.cancel_button = new System.Windows.Forms.Button();
            this.ok_button = new System.Windows.Forms.Button();
            this.lttType_label = new System.Windows.Forms.Label();
            this.lttType_comboBox = new System.Windows.Forms.ComboBox();
            this.com_checkBox = new System.Windows.Forms.CheckBox();
            this.com_label = new System.Windows.Forms.Label();
            this.comRad_textBox = new System.Windows.Forms.TextBox();
            this.plateTec_checkBox = new System.Windows.Forms.CheckBox();
            this.solidEarth_checkBox = new System.Windows.Forms.CheckBox();
            this.poleTide_checkBox = new System.Windows.Forms.CheckBox();
            this.oceanTide_checkBox = new System.Windows.Forms.CheckBox();
            this.ltt_groupBox.SuspendLayout();
            this.com_groupBox.SuspendLayout();
            this.refP_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ltt_groupBox
            // 
            this.ltt_groupBox.Controls.Add(this.lttType_comboBox);
            this.ltt_groupBox.Controls.Add(this.lttType_label);
            this.ltt_groupBox.Location = new System.Drawing.Point(25, 26);
            this.ltt_groupBox.Name = "ltt_groupBox";
            this.ltt_groupBox.Size = new System.Drawing.Size(355, 78);
            this.ltt_groupBox.TabIndex = 0;
            this.ltt_groupBox.TabStop = false;
            this.ltt_groupBox.Text = "Light-Time Correction";
            // 
            // com_groupBox
            // 
            this.com_groupBox.Controls.Add(this.comRad_textBox);
            this.com_groupBox.Controls.Add(this.com_label);
            this.com_groupBox.Controls.Add(this.com_checkBox);
            this.com_groupBox.Location = new System.Drawing.Point(25, 130);
            this.com_groupBox.Name = "com_groupBox";
            this.com_groupBox.Size = new System.Drawing.Size(355, 72);
            this.com_groupBox.TabIndex = 1;
            this.com_groupBox.TabStop = false;
            this.com_groupBox.Text = "Center-of-Mass Correction";
            // 
            // refP_groupBox
            // 
            this.refP_groupBox.Controls.Add(this.oceanTide_checkBox);
            this.refP_groupBox.Controls.Add(this.poleTide_checkBox);
            this.refP_groupBox.Controls.Add(this.solidEarth_checkBox);
            this.refP_groupBox.Controls.Add(this.plateTec_checkBox);
            this.refP_groupBox.Location = new System.Drawing.Point(25, 229);
            this.refP_groupBox.Name = "refP_groupBox";
            this.refP_groupBox.Size = new System.Drawing.Size(355, 91);
            this.refP_groupBox.TabIndex = 1;
            this.refP_groupBox.TabStop = false;
            this.refP_groupBox.Text = "Referenct Point Correction";
            // 
            // relDelay_checkBox
            // 
            this.relDelay_checkBox.AutoSize = true;
            this.relDelay_checkBox.Location = new System.Drawing.Point(25, 336);
            this.relDelay_checkBox.Name = "relDelay_checkBox";
            this.relDelay_checkBox.Size = new System.Drawing.Size(120, 16);
            this.relDelay_checkBox.TabIndex = 2;
            this.relDelay_checkBox.Text = "Relativistic Delay";
            this.relDelay_checkBox.UseVisualStyleBackColor = true;
            // 
            // tropoDelay_checkBox
            // 
            this.tropoDelay_checkBox.AutoSize = true;
            this.tropoDelay_checkBox.Location = new System.Drawing.Point(25, 358);
            this.tropoDelay_checkBox.Name = "tropoDelay_checkBox";
            this.tropoDelay_checkBox.Size = new System.Drawing.Size(135, 16);
            this.tropoDelay_checkBox.TabIndex = 3;
            this.tropoDelay_checkBox.Text = "Tropospheric Delay";
            this.tropoDelay_checkBox.UseVisualStyleBackColor = true;
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(305, 395);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(75, 23);
            this.cancel_button.TabIndex = 5;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            // 
            // ok_button
            // 
            this.ok_button.Location = new System.Drawing.Point(208, 395);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(75, 23);
            this.ok_button.TabIndex = 4;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // lttType_label
            // 
            this.lttType_label.AutoSize = true;
            this.lttType_label.Location = new System.Drawing.Point(54, 41);
            this.lttType_label.Name = "lttType_label";
            this.lttType_label.Size = new System.Drawing.Size(34, 12);
            this.lttType_label.TabIndex = 0;
            this.lttType_label.Text = "Type";
            // 
            // lttType_comboBox
            // 
            this.lttType_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lttType_comboBox.FormattingEnabled = true;
            this.lttType_comboBox.Location = new System.Drawing.Point(183, 41);
            this.lttType_comboBox.Name = "lttType_comboBox";
            this.lttType_comboBox.Size = new System.Drawing.Size(121, 20);
            this.lttType_comboBox.TabIndex = 1;
            // 
            // com_checkBox
            // 
            this.com_checkBox.AutoSize = true;
            this.com_checkBox.Location = new System.Drawing.Point(32, 35);
            this.com_checkBox.Name = "com_checkBox";
            this.com_checkBox.Size = new System.Drawing.Size(15, 14);
            this.com_checkBox.TabIndex = 0;
            this.com_checkBox.UseVisualStyleBackColor = true;
            this.com_checkBox.CheckedChanged += new System.EventHandler(this.com_checkBox_CheckedChanged);
            // 
            // com_label
            // 
            this.com_label.AutoSize = true;
            this.com_label.Location = new System.Drawing.Point(93, 36);
            this.com_label.Name = "com_label";
            this.com_label.Size = new System.Drawing.Size(69, 12);
            this.com_label.TabIndex = 1;
            this.com_label.Text = "Radius (m)";
            // 
            // comRad_textBox
            // 
            this.comRad_textBox.Location = new System.Drawing.Point(183, 32);
            this.comRad_textBox.Name = "comRad_textBox";
            this.comRad_textBox.Size = new System.Drawing.Size(100, 21);
            this.comRad_textBox.TabIndex = 2;
            // 
            // plateTec_checkBox
            // 
            this.plateTec_checkBox.AutoSize = true;
            this.plateTec_checkBox.Location = new System.Drawing.Point(26, 36);
            this.plateTec_checkBox.Name = "plateTec_checkBox";
            this.plateTec_checkBox.Size = new System.Drawing.Size(105, 16);
            this.plateTec_checkBox.TabIndex = 0;
            this.plateTec_checkBox.Text = "Plate Tectonic";
            this.plateTec_checkBox.UseVisualStyleBackColor = true;
            // 
            // solidEarth_checkBox
            // 
            this.solidEarth_checkBox.AutoSize = true;
            this.solidEarth_checkBox.Location = new System.Drawing.Point(197, 36);
            this.solidEarth_checkBox.Name = "solidEarth_checkBox";
            this.solidEarth_checkBox.Size = new System.Drawing.Size(114, 16);
            this.solidEarth_checkBox.TabIndex = 1;
            this.solidEarth_checkBox.Text = "Solid Earth Tide";
            this.solidEarth_checkBox.UseVisualStyleBackColor = true;
            // 
            // poleTide_checkBox
            // 
            this.poleTide_checkBox.AutoSize = true;
            this.poleTide_checkBox.Location = new System.Drawing.Point(197, 58);
            this.poleTide_checkBox.Name = "poleTide_checkBox";
            this.poleTide_checkBox.Size = new System.Drawing.Size(78, 16);
            this.poleTide_checkBox.TabIndex = 2;
            this.poleTide_checkBox.Text = "Pole Tide";
            this.poleTide_checkBox.UseVisualStyleBackColor = true;
            // 
            // oceanTide_checkBox
            // 
            this.oceanTide_checkBox.AutoSize = true;
            this.oceanTide_checkBox.Location = new System.Drawing.Point(26, 58);
            this.oceanTide_checkBox.Name = "oceanTide_checkBox";
            this.oceanTide_checkBox.Size = new System.Drawing.Size(90, 16);
            this.oceanTide_checkBox.TabIndex = 3;
            this.oceanTide_checkBox.Text = "Ocean Tide";
            this.oceanTide_checkBox.UseVisualStyleBackColor = true;
            // 
            // SetMeasurement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 438);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.tropoDelay_checkBox);
            this.Controls.Add(this.relDelay_checkBox);
            this.Controls.Add(this.refP_groupBox);
            this.Controls.Add(this.com_groupBox);
            this.Controls.Add(this.ltt_groupBox);
            this.Name = "SetMeasurement";
            this.Text = "SetMeasurement";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetMeasurement_FormClosing);
            this.Load += new System.EventHandler(this.SetMeasurement_Load);
            this.ltt_groupBox.ResumeLayout(false);
            this.ltt_groupBox.PerformLayout();
            this.com_groupBox.ResumeLayout(false);
            this.com_groupBox.PerformLayout();
            this.refP_groupBox.ResumeLayout(false);
            this.refP_groupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox ltt_groupBox;
        private System.Windows.Forms.GroupBox com_groupBox;
        private System.Windows.Forms.GroupBox refP_groupBox;
        private System.Windows.Forms.CheckBox relDelay_checkBox;
        private System.Windows.Forms.CheckBox tropoDelay_checkBox;
        private System.Windows.Forms.ComboBox lttType_comboBox;
        private System.Windows.Forms.Label lttType_label;
        private System.Windows.Forms.TextBox comRad_textBox;
        private System.Windows.Forms.Label com_label;
        private System.Windows.Forms.CheckBox com_checkBox;
        private System.Windows.Forms.CheckBox oceanTide_checkBox;
        private System.Windows.Forms.CheckBox poleTide_checkBox;
        private System.Windows.Forms.CheckBox solidEarth_checkBox;
        private System.Windows.Forms.CheckBox plateTec_checkBox;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Button ok_button;
    }
}