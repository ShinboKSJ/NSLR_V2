namespace NSLR_ObservationControl.OAS
{
    partial class SetEstimator
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.obsSite_comboBox = new System.Windows.Forms.ComboBox();
            this.azel_checkBox = new System.Windows.Forms.CheckBox();
            this.range_checkBox = new System.Windows.Forms.CheckBox();
            this.time_groupBox = new System.Windows.Forms.GroupBox();
            this.epochTime3_textBox = new System.Windows.Forms.TextBox();
            this.epochTime4_textBox = new System.Windows.Forms.TextBox();
            this.epochTime5_textBox = new System.Windows.Forms.TextBox();
            this.epochTime6_textBox = new System.Windows.Forms.TextBox();
            this.epochTime2_textBox = new System.Windows.Forms.TextBox();
            this.epochTime1_textBox = new System.Windows.Forms.TextBox();
            this.finalTime3_textBox = new System.Windows.Forms.TextBox();
            this.finalTime4_textBox = new System.Windows.Forms.TextBox();
            this.finalTime5_textBox = new System.Windows.Forms.TextBox();
            this.finalTime6_textBox = new System.Windows.Forms.TextBox();
            this.finalTime2_textBox = new System.Windows.Forms.TextBox();
            this.finalTime1_textBox = new System.Windows.Forms.TextBox();
            this.startTime3_textBox = new System.Windows.Forms.TextBox();
            this.startTime4_textBox = new System.Windows.Forms.TextBox();
            this.startTime5_textBox = new System.Windows.Forms.TextBox();
            this.startTime6_textBox = new System.Windows.Forms.TextBox();
            this.startTime2_textBox = new System.Windows.Forms.TextBox();
            this.startTime1_textBox = new System.Windows.Forms.TextBox();
            this.epochTime_label = new System.Windows.Forms.Label();
            this.finalTime_label = new System.Windows.Forms.Label();
            this.startTime_label = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.uncVel_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.uncPos_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.editor_checkBox = new System.Windows.Forms.CheckBox();
            this.thr_textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tol_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.maxIter_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cancel_button = new System.Windows.Forms.Button();
            this.ok_button = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.time_groupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.obsSite_comboBox);
            this.groupBox1.Controls.Add(this.azel_checkBox);
            this.groupBox1.Controls.Add(this.range_checkBox);
            this.groupBox1.Location = new System.Drawing.Point(22, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(317, 105);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Observation";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "Observatory";
            // 
            // obsSite_comboBox
            // 
            this.obsSite_comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.obsSite_comboBox.FormattingEnabled = true;
            this.obsSite_comboBox.Location = new System.Drawing.Point(114, 29);
            this.obsSite_comboBox.Name = "obsSite_comboBox";
            this.obsSite_comboBox.Size = new System.Drawing.Size(193, 20);
            this.obsSite_comboBox.TabIndex = 2;
            // 
            // azel_checkBox
            // 
            this.azel_checkBox.AutoSize = true;
            this.azel_checkBox.Location = new System.Drawing.Point(159, 68);
            this.azel_checkBox.Name = "azel_checkBox";
            this.azel_checkBox.Size = new System.Drawing.Size(127, 16);
            this.azel_checkBox.TabIndex = 1;
            this.azel_checkBox.Text = "Azimuth/Elevation";
            this.azel_checkBox.UseVisualStyleBackColor = true;
            // 
            // range_checkBox
            // 
            this.range_checkBox.AutoSize = true;
            this.range_checkBox.Location = new System.Drawing.Point(26, 68);
            this.range_checkBox.Name = "range_checkBox";
            this.range_checkBox.Size = new System.Drawing.Size(60, 16);
            this.range_checkBox.TabIndex = 0;
            this.range_checkBox.Text = "Range";
            this.range_checkBox.UseVisualStyleBackColor = true;
            // 
            // time_groupBox
            // 
            this.time_groupBox.Controls.Add(this.epochTime3_textBox);
            this.time_groupBox.Controls.Add(this.epochTime4_textBox);
            this.time_groupBox.Controls.Add(this.epochTime5_textBox);
            this.time_groupBox.Controls.Add(this.epochTime6_textBox);
            this.time_groupBox.Controls.Add(this.epochTime2_textBox);
            this.time_groupBox.Controls.Add(this.epochTime1_textBox);
            this.time_groupBox.Controls.Add(this.finalTime3_textBox);
            this.time_groupBox.Controls.Add(this.finalTime4_textBox);
            this.time_groupBox.Controls.Add(this.finalTime5_textBox);
            this.time_groupBox.Controls.Add(this.finalTime6_textBox);
            this.time_groupBox.Controls.Add(this.finalTime2_textBox);
            this.time_groupBox.Controls.Add(this.finalTime1_textBox);
            this.time_groupBox.Controls.Add(this.startTime3_textBox);
            this.time_groupBox.Controls.Add(this.startTime4_textBox);
            this.time_groupBox.Controls.Add(this.startTime5_textBox);
            this.time_groupBox.Controls.Add(this.startTime6_textBox);
            this.time_groupBox.Controls.Add(this.startTime2_textBox);
            this.time_groupBox.Controls.Add(this.startTime1_textBox);
            this.time_groupBox.Controls.Add(this.epochTime_label);
            this.time_groupBox.Controls.Add(this.finalTime_label);
            this.time_groupBox.Controls.Add(this.startTime_label);
            this.time_groupBox.Location = new System.Drawing.Point(23, 158);
            this.time_groupBox.Name = "time_groupBox";
            this.time_groupBox.Size = new System.Drawing.Size(316, 155);
            this.time_groupBox.TabIndex = 1;
            this.time_groupBox.TabStop = false;
            this.time_groupBox.Text = "Time";
            // 
            // epochTime3_textBox
            // 
            this.epochTime3_textBox.Location = new System.Drawing.Point(195, 113);
            this.epochTime3_textBox.Name = "epochTime3_textBox";
            this.epochTime3_textBox.Size = new System.Drawing.Size(24, 21);
            this.epochTime3_textBox.TabIndex = 20;
            // 
            // epochTime4_textBox
            // 
            this.epochTime4_textBox.Location = new System.Drawing.Point(224, 113);
            this.epochTime4_textBox.Name = "epochTime4_textBox";
            this.epochTime4_textBox.Size = new System.Drawing.Size(24, 21);
            this.epochTime4_textBox.TabIndex = 19;
            // 
            // epochTime5_textBox
            // 
            this.epochTime5_textBox.Location = new System.Drawing.Point(253, 113);
            this.epochTime5_textBox.Name = "epochTime5_textBox";
            this.epochTime5_textBox.Size = new System.Drawing.Size(24, 21);
            this.epochTime5_textBox.TabIndex = 18;
            // 
            // epochTime6_textBox
            // 
            this.epochTime6_textBox.Location = new System.Drawing.Point(282, 113);
            this.epochTime6_textBox.Name = "epochTime6_textBox";
            this.epochTime6_textBox.Size = new System.Drawing.Size(24, 21);
            this.epochTime6_textBox.TabIndex = 17;
            // 
            // epochTime2_textBox
            // 
            this.epochTime2_textBox.Location = new System.Drawing.Point(166, 113);
            this.epochTime2_textBox.Name = "epochTime2_textBox";
            this.epochTime2_textBox.Size = new System.Drawing.Size(24, 21);
            this.epochTime2_textBox.TabIndex = 16;
            // 
            // epochTime1_textBox
            // 
            this.epochTime1_textBox.Location = new System.Drawing.Point(113, 113);
            this.epochTime1_textBox.Name = "epochTime1_textBox";
            this.epochTime1_textBox.Size = new System.Drawing.Size(48, 21);
            this.epochTime1_textBox.TabIndex = 15;
            // 
            // finalTime3_textBox
            // 
            this.finalTime3_textBox.Location = new System.Drawing.Point(195, 77);
            this.finalTime3_textBox.Name = "finalTime3_textBox";
            this.finalTime3_textBox.Size = new System.Drawing.Size(24, 21);
            this.finalTime3_textBox.TabIndex = 14;
            // 
            // finalTime4_textBox
            // 
            this.finalTime4_textBox.Location = new System.Drawing.Point(224, 77);
            this.finalTime4_textBox.Name = "finalTime4_textBox";
            this.finalTime4_textBox.Size = new System.Drawing.Size(24, 21);
            this.finalTime4_textBox.TabIndex = 13;
            // 
            // finalTime5_textBox
            // 
            this.finalTime5_textBox.Location = new System.Drawing.Point(253, 77);
            this.finalTime5_textBox.Name = "finalTime5_textBox";
            this.finalTime5_textBox.Size = new System.Drawing.Size(24, 21);
            this.finalTime5_textBox.TabIndex = 12;
            // 
            // finalTime6_textBox
            // 
            this.finalTime6_textBox.Location = new System.Drawing.Point(282, 77);
            this.finalTime6_textBox.Name = "finalTime6_textBox";
            this.finalTime6_textBox.Size = new System.Drawing.Size(24, 21);
            this.finalTime6_textBox.TabIndex = 11;
            // 
            // finalTime2_textBox
            // 
            this.finalTime2_textBox.Location = new System.Drawing.Point(166, 77);
            this.finalTime2_textBox.Name = "finalTime2_textBox";
            this.finalTime2_textBox.Size = new System.Drawing.Size(24, 21);
            this.finalTime2_textBox.TabIndex = 10;
            // 
            // finalTime1_textBox
            // 
            this.finalTime1_textBox.Location = new System.Drawing.Point(113, 77);
            this.finalTime1_textBox.Name = "finalTime1_textBox";
            this.finalTime1_textBox.Size = new System.Drawing.Size(48, 21);
            this.finalTime1_textBox.TabIndex = 9;
            // 
            // startTime3_textBox
            // 
            this.startTime3_textBox.Location = new System.Drawing.Point(195, 39);
            this.startTime3_textBox.Name = "startTime3_textBox";
            this.startTime3_textBox.Size = new System.Drawing.Size(24, 21);
            this.startTime3_textBox.TabIndex = 8;
            // 
            // startTime4_textBox
            // 
            this.startTime4_textBox.Location = new System.Drawing.Point(224, 39);
            this.startTime4_textBox.Name = "startTime4_textBox";
            this.startTime4_textBox.Size = new System.Drawing.Size(24, 21);
            this.startTime4_textBox.TabIndex = 7;
            // 
            // startTime5_textBox
            // 
            this.startTime5_textBox.Location = new System.Drawing.Point(253, 39);
            this.startTime5_textBox.Name = "startTime5_textBox";
            this.startTime5_textBox.Size = new System.Drawing.Size(24, 21);
            this.startTime5_textBox.TabIndex = 6;
            // 
            // startTime6_textBox
            // 
            this.startTime6_textBox.Location = new System.Drawing.Point(282, 39);
            this.startTime6_textBox.Name = "startTime6_textBox";
            this.startTime6_textBox.Size = new System.Drawing.Size(24, 21);
            this.startTime6_textBox.TabIndex = 5;
            // 
            // startTime2_textBox
            // 
            this.startTime2_textBox.Location = new System.Drawing.Point(166, 39);
            this.startTime2_textBox.Name = "startTime2_textBox";
            this.startTime2_textBox.Size = new System.Drawing.Size(24, 21);
            this.startTime2_textBox.TabIndex = 4;
            // 
            // startTime1_textBox
            // 
            this.startTime1_textBox.Location = new System.Drawing.Point(113, 39);
            this.startTime1_textBox.Name = "startTime1_textBox";
            this.startTime1_textBox.Size = new System.Drawing.Size(48, 21);
            this.startTime1_textBox.TabIndex = 3;
            // 
            // epochTime_label
            // 
            this.epochTime_label.AutoSize = true;
            this.epochTime_label.Location = new System.Drawing.Point(23, 116);
            this.epochTime_label.Name = "epochTime_label";
            this.epochTime_label.Size = new System.Drawing.Size(74, 12);
            this.epochTime_label.TabIndex = 2;
            this.epochTime_label.Text = "Epoch Time";
            // 
            // finalTime_label
            // 
            this.finalTime_label.AutoSize = true;
            this.finalTime_label.Location = new System.Drawing.Point(23, 80);
            this.finalTime_label.Name = "finalTime_label";
            this.finalTime_label.Size = new System.Drawing.Size(65, 12);
            this.finalTime_label.TabIndex = 1;
            this.finalTime_label.Text = "Final Time";
            // 
            // startTime_label
            // 
            this.startTime_label.AutoSize = true;
            this.startTime_label.Location = new System.Drawing.Point(22, 42);
            this.startTime_label.Name = "startTime_label";
            this.startTime_label.Size = new System.Drawing.Size(63, 12);
            this.startTime_label.TabIndex = 0;
            this.startTime_label.Text = "Start Time";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.uncVel_textBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.uncPos_textBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(24, 333);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(315, 66);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Uncertainty";
            // 
            // uncVel_textBox
            // 
            this.uncVel_textBox.Location = new System.Drawing.Point(269, 29);
            this.uncVel_textBox.Name = "uncVel_textBox";
            this.uncVel_textBox.Size = new System.Drawing.Size(36, 21);
            this.uncVel_textBox.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(163, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Velocity (km/s)";
            // 
            // uncPos_textBox
            // 
            this.uncPos_textBox.Location = new System.Drawing.Point(115, 29);
            this.uncPos_textBox.Name = "uncPos_textBox";
            this.uncPos_textBox.Size = new System.Drawing.Size(36, 21);
            this.uncPos_textBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Position (km)";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.tol_textBox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.maxIter_textBox);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(24, 424);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(315, 100);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Parameters";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.editor_checkBox);
            this.groupBox4.Controls.Add(this.thr_textBox);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Location = new System.Drawing.Point(177, 16);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(129, 75);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Data Editing";
            // 
            // editor_checkBox
            // 
            this.editor_checkBox.AutoSize = true;
            this.editor_checkBox.Location = new System.Drawing.Point(16, 21);
            this.editor_checkBox.Name = "editor_checkBox";
            this.editor_checkBox.Size = new System.Drawing.Size(56, 16);
            this.editor_checkBox.TabIndex = 9;
            this.editor_checkBox.Text = "Apply";
            this.editor_checkBox.UseVisualStyleBackColor = true;
            // 
            // thr_textBox
            // 
            this.thr_textBox.Location = new System.Drawing.Point(87, 43);
            this.thr_textBox.Name = "thr_textBox";
            this.thr_textBox.Size = new System.Drawing.Size(36, 21);
            this.thr_textBox.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "Threshold";
            // 
            // tol_textBox
            // 
            this.tol_textBox.Location = new System.Drawing.Point(115, 70);
            this.tol_textBox.Name = "tol_textBox";
            this.tol_textBox.Size = new System.Drawing.Size(36, 21);
            this.tol_textBox.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "Tolerance";
            // 
            // maxIter_textBox
            // 
            this.maxIter_textBox.Location = new System.Drawing.Point(115, 34);
            this.maxIter_textBox.Name = "maxIter_textBox";
            this.maxIter_textBox.Size = new System.Drawing.Size(36, 21);
            this.maxIter_textBox.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Max.Iteration";
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(264, 550);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(75, 23);
            this.cancel_button.TabIndex = 5;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // ok_button
            // 
            this.ok_button.Location = new System.Drawing.Point(167, 550);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(75, 23);
            this.ok_button.TabIndex = 4;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // SetEstimator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 585);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.time_groupBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "SetEstimator";
            this.Text = "SetEstimator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetEstimator_FormClosing);
            this.Load += new System.EventHandler(this.SetEstimator_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.time_groupBox.ResumeLayout(false);
            this.time_groupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox azel_checkBox;
        private System.Windows.Forms.CheckBox range_checkBox;
        private System.Windows.Forms.GroupBox time_groupBox;
        private System.Windows.Forms.TextBox epochTime3_textBox;
        private System.Windows.Forms.TextBox epochTime4_textBox;
        private System.Windows.Forms.TextBox epochTime5_textBox;
        private System.Windows.Forms.TextBox epochTime6_textBox;
        private System.Windows.Forms.TextBox epochTime2_textBox;
        private System.Windows.Forms.TextBox epochTime1_textBox;
        private System.Windows.Forms.TextBox finalTime3_textBox;
        private System.Windows.Forms.TextBox finalTime4_textBox;
        private System.Windows.Forms.TextBox finalTime5_textBox;
        private System.Windows.Forms.TextBox finalTime6_textBox;
        private System.Windows.Forms.TextBox finalTime2_textBox;
        private System.Windows.Forms.TextBox finalTime1_textBox;
        private System.Windows.Forms.TextBox startTime3_textBox;
        private System.Windows.Forms.TextBox startTime4_textBox;
        private System.Windows.Forms.TextBox startTime5_textBox;
        private System.Windows.Forms.TextBox startTime6_textBox;
        private System.Windows.Forms.TextBox startTime2_textBox;
        private System.Windows.Forms.TextBox startTime1_textBox;
        private System.Windows.Forms.Label epochTime_label;
        private System.Windows.Forms.Label finalTime_label;
        private System.Windows.Forms.Label startTime_label;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox uncVel_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox uncPos_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tol_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox maxIter_textBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox editor_checkBox;
        private System.Windows.Forms.TextBox thr_textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox obsSite_comboBox;
    }
}