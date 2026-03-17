namespace NSLR_ObservationControl.OAS
{
    public partial class OAS_Main
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
            this.setDyn_button = new System.Windows.Forms.Button();
            this.setObject_button = new System.Windows.Forms.Button();
            this.setMea_button = new System.Windows.Forms.Button();
            this.loadObs_button = new System.Windows.Forms.Button();
            this.obsFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.delta1_textBox = new System.Windows.Forms.TextBox();
            this.delta1_label = new System.Windows.Forms.Label();
            this.delta2_label = new System.Windows.Forms.Label();
            this.delta3_label = new System.Windows.Forms.Label();
            this.delta4_label = new System.Windows.Forms.Label();
            this.delta5_label = new System.Windows.Forms.Label();
            this.delta6_label = new System.Windows.Forms.Label();
            this.delta2_textBox = new System.Windows.Forms.TextBox();
            this.delta3_textBox = new System.Windows.Forms.TextBox();
            this.delta6_textBox = new System.Windows.Forms.TextBox();
            this.delta4_textBox = new System.Windows.Forms.TextBox();
            this.delta5_textBox = new System.Windows.Forms.TextBox();
            this.rms1_label = new System.Windows.Forms.Label();
            this.rms1_textBox = new System.Windows.Forms.TextBox();
            this.rms2_label = new System.Windows.Forms.Label();
            this.rms2_textBox = new System.Windows.Forms.TextBox();
            this.rms3_label = new System.Windows.Forms.Label();
            this.rms3_textBox = new System.Windows.Forms.TextBox();
            this.iter_label = new System.Windows.Forms.Label();
            this.iterN_textBox = new System.Windows.Forms.TextBox();
            this.perIdx_label = new System.Windows.Forms.Label();
            this.perIdx_textBox = new System.Windows.Forms.TextBox();
            this.result_groupBox = new System.Windows.Forms.GroupBox();
            this.viewCov_button = new System.Windows.Forms.Button();
            this.viewRes_button = new System.Windows.Forms.Button();
            this.setEst_button = new System.Windows.Forms.Button();
            this.genCMD_button = new System.Windows.Forms.Button();
            this.genEph_button = new System.Windows.Forms.Button();
            this.obs_groupBox = new System.Windows.Forms.GroupBox();
            this.run_button = new System.Windows.Forms.Button();
            this.exit_button = new System.Windows.Forms.Button();
            this.bodyLoc_button = new System.Windows.Forms.Button();
            this.testRunLS_button = new System.Windows.Forms.Button();
            this.testRunEKF_button = new System.Windows.Forms.Button();
            this.setLaser_button = new System.Windows.Forms.Button();
            this.result_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // setDyn_button
            // 
            this.setDyn_button.Location = new System.Drawing.Point(23, 62);
            this.setDyn_button.Name = "setDyn_button";
            this.setDyn_button.Size = new System.Drawing.Size(190, 22);
            this.setDyn_button.TabIndex = 2;
            this.setDyn_button.Text = "Set Dynamics Model";
            this.setDyn_button.UseVisualStyleBackColor = true;
            this.setDyn_button.Click += new System.EventHandler(this.setDyn_button_Click);
            // 
            // setObject_button
            // 
            this.setObject_button.Location = new System.Drawing.Point(23, 124);
            this.setObject_button.Name = "setObject_button";
            this.setObject_button.Size = new System.Drawing.Size(190, 22);
            this.setObject_button.TabIndex = 3;
            this.setObject_button.Text = "Set Object";
            this.setObject_button.UseVisualStyleBackColor = true;
            this.setObject_button.Click += new System.EventHandler(this.setObject_button_Click);
            // 
            // setMea_button
            // 
            this.setMea_button.Location = new System.Drawing.Point(23, 93);
            this.setMea_button.Name = "setMea_button";
            this.setMea_button.Size = new System.Drawing.Size(190, 22);
            this.setMea_button.TabIndex = 4;
            this.setMea_button.Text = "Set Measurement Model";
            this.setMea_button.UseVisualStyleBackColor = true;
            this.setMea_button.Click += new System.EventHandler(this.setMea_button_Click);
            // 
            // loadObs_button
            // 
            this.loadObs_button.Location = new System.Drawing.Point(23, 31);
            this.loadObs_button.Name = "loadObs_button";
            this.loadObs_button.Size = new System.Drawing.Size(190, 22);
            this.loadObs_button.TabIndex = 5;
            this.loadObs_button.Text = "Load Observation";
            this.loadObs_button.UseVisualStyleBackColor = true;
            this.loadObs_button.Click += new System.EventHandler(this.loadObs_button_Click);
            // 
            // obsFileDialog
            // 
            this.obsFileDialog.FileName = "obsFileName";
            // 
            // delta1_textBox
            // 
            this.delta1_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.delta1_textBox.Location = new System.Drawing.Point(134, 62);
            this.delta1_textBox.Name = "delta1_textBox";
            this.delta1_textBox.ReadOnly = true;
            this.delta1_textBox.Size = new System.Drawing.Size(100, 14);
            this.delta1_textBox.TabIndex = 6;
            // 
            // delta1_label
            // 
            this.delta1_label.AutoSize = true;
            this.delta1_label.Location = new System.Drawing.Point(50, 63);
            this.delta1_label.Name = "delta1_label";
            this.delta1_label.Size = new System.Drawing.Size(56, 12);
            this.delta1_label.TabIndex = 7;
            this.delta1_label.Text = "△X (km)";
            this.delta1_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // delta2_label
            // 
            this.delta2_label.AutoSize = true;
            this.delta2_label.Location = new System.Drawing.Point(50, 91);
            this.delta2_label.Name = "delta2_label";
            this.delta2_label.Size = new System.Drawing.Size(56, 12);
            this.delta2_label.TabIndex = 8;
            this.delta2_label.Text = "△Y (km)";
            this.delta2_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // delta3_label
            // 
            this.delta3_label.AutoSize = true;
            this.delta3_label.Location = new System.Drawing.Point(50, 119);
            this.delta3_label.Name = "delta3_label";
            this.delta3_label.Size = new System.Drawing.Size(56, 12);
            this.delta3_label.TabIndex = 9;
            this.delta3_label.Text = "△Z (km)";
            this.delta3_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // delta4_label
            // 
            this.delta4_label.AutoSize = true;
            this.delta4_label.Location = new System.Drawing.Point(40, 147);
            this.delta4_label.Name = "delta4_label";
            this.delta4_label.Size = new System.Drawing.Size(76, 12);
            this.delta4_label.TabIndex = 10;
            this.delta4_label.Text = "△Vx (km/s)";
            this.delta4_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // delta5_label
            // 
            this.delta5_label.AutoSize = true;
            this.delta5_label.Location = new System.Drawing.Point(40, 175);
            this.delta5_label.Name = "delta5_label";
            this.delta5_label.Size = new System.Drawing.Size(76, 12);
            this.delta5_label.TabIndex = 11;
            this.delta5_label.Text = "△Vy (km/s)";
            this.delta5_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // delta6_label
            // 
            this.delta6_label.AutoSize = true;
            this.delta6_label.Location = new System.Drawing.Point(40, 203);
            this.delta6_label.Name = "delta6_label";
            this.delta6_label.Size = new System.Drawing.Size(76, 12);
            this.delta6_label.TabIndex = 12;
            this.delta6_label.Text = "△Vz (km/s)";
            this.delta6_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // delta2_textBox
            // 
            this.delta2_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.delta2_textBox.Location = new System.Drawing.Point(134, 90);
            this.delta2_textBox.Name = "delta2_textBox";
            this.delta2_textBox.ReadOnly = true;
            this.delta2_textBox.Size = new System.Drawing.Size(100, 14);
            this.delta2_textBox.TabIndex = 13;
            // 
            // delta3_textBox
            // 
            this.delta3_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.delta3_textBox.Location = new System.Drawing.Point(134, 118);
            this.delta3_textBox.Name = "delta3_textBox";
            this.delta3_textBox.ReadOnly = true;
            this.delta3_textBox.Size = new System.Drawing.Size(100, 14);
            this.delta3_textBox.TabIndex = 14;
            // 
            // delta6_textBox
            // 
            this.delta6_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.delta6_textBox.Location = new System.Drawing.Point(134, 202);
            this.delta6_textBox.Name = "delta6_textBox";
            this.delta6_textBox.ReadOnly = true;
            this.delta6_textBox.Size = new System.Drawing.Size(100, 14);
            this.delta6_textBox.TabIndex = 15;
            // 
            // delta4_textBox
            // 
            this.delta4_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.delta4_textBox.Location = new System.Drawing.Point(134, 146);
            this.delta4_textBox.Name = "delta4_textBox";
            this.delta4_textBox.ReadOnly = true;
            this.delta4_textBox.Size = new System.Drawing.Size(100, 14);
            this.delta4_textBox.TabIndex = 16;
            // 
            // delta5_textBox
            // 
            this.delta5_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.delta5_textBox.Location = new System.Drawing.Point(134, 174);
            this.delta5_textBox.Name = "delta5_textBox";
            this.delta5_textBox.ReadOnly = true;
            this.delta5_textBox.Size = new System.Drawing.Size(100, 14);
            this.delta5_textBox.TabIndex = 17;
            // 
            // rms1_label
            // 
            this.rms1_label.AutoSize = true;
            this.rms1_label.Location = new System.Drawing.Point(279, 63);
            this.rms1_label.Name = "rms1_label";
            this.rms1_label.Size = new System.Drawing.Size(78, 12);
            this.rms1_label.TabIndex = 19;
            this.rms1_label.Text = "△Range (m)";
            this.rms1_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rms1_textBox
            // 
            this.rms1_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rms1_textBox.Location = new System.Drawing.Point(388, 62);
            this.rms1_textBox.Name = "rms1_textBox";
            this.rms1_textBox.ReadOnly = true;
            this.rms1_textBox.Size = new System.Drawing.Size(100, 14);
            this.rms1_textBox.TabIndex = 18;
            // 
            // rms2_label
            // 
            this.rms2_label.AutoSize = true;
            this.rms2_label.Location = new System.Drawing.Point(278, 91);
            this.rms2_label.Name = "rms2_label";
            this.rms2_label.Size = new System.Drawing.Size(81, 12);
            this.rms2_label.TabIndex = 21;
            this.rms2_label.Text = "△Azimuth (\")";
            this.rms2_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rms2_textBox
            // 
            this.rms2_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rms2_textBox.Location = new System.Drawing.Point(388, 90);
            this.rms2_textBox.Name = "rms2_textBox";
            this.rms2_textBox.ReadOnly = true;
            this.rms2_textBox.Size = new System.Drawing.Size(100, 14);
            this.rms2_textBox.TabIndex = 20;
            // 
            // rms3_label
            // 
            this.rms3_label.AutoSize = true;
            this.rms3_label.Location = new System.Drawing.Point(275, 119);
            this.rms3_label.Name = "rms3_label";
            this.rms3_label.Size = new System.Drawing.Size(86, 12);
            this.rms3_label.TabIndex = 23;
            this.rms3_label.Text = "△Elevation (\")";
            this.rms3_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rms3_textBox
            // 
            this.rms3_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rms3_textBox.Location = new System.Drawing.Point(388, 118);
            this.rms3_textBox.Name = "rms3_textBox";
            this.rms3_textBox.ReadOnly = true;
            this.rms3_textBox.Size = new System.Drawing.Size(100, 14);
            this.rms3_textBox.TabIndex = 22;
            // 
            // iter_label
            // 
            this.iter_label.AutoSize = true;
            this.iter_label.Location = new System.Drawing.Point(50, 28);
            this.iter_label.Name = "iter_label";
            this.iter_label.Size = new System.Drawing.Size(49, 12);
            this.iter_label.TabIndex = 25;
            this.iter_label.Text = "Iteration";
            this.iter_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // iterN_textBox
            // 
            this.iterN_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.iterN_textBox.Location = new System.Drawing.Point(134, 27);
            this.iterN_textBox.Name = "iterN_textBox";
            this.iterN_textBox.ReadOnly = true;
            this.iterN_textBox.Size = new System.Drawing.Size(100, 14);
            this.iterN_textBox.TabIndex = 24;
            // 
            // perIdx_label
            // 
            this.perIdx_label.AutoSize = true;
            this.perIdx_label.Location = new System.Drawing.Point(261, 28);
            this.perIdx_label.Name = "perIdx_label";
            this.perIdx_label.Size = new System.Drawing.Size(108, 12);
            this.perIdx_label.TabIndex = 27;
            this.perIdx_label.Text = "Perfomance Index";
            this.perIdx_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // perIdx_textBox
            // 
            this.perIdx_textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.perIdx_textBox.Location = new System.Drawing.Point(388, 27);
            this.perIdx_textBox.Name = "perIdx_textBox";
            this.perIdx_textBox.ReadOnly = true;
            this.perIdx_textBox.Size = new System.Drawing.Size(100, 14);
            this.perIdx_textBox.TabIndex = 26;
            // 
            // result_groupBox
            // 
            this.result_groupBox.Controls.Add(this.viewCov_button);
            this.result_groupBox.Controls.Add(this.viewRes_button);
            this.result_groupBox.Controls.Add(this.perIdx_label);
            this.result_groupBox.Controls.Add(this.iter_label);
            this.result_groupBox.Controls.Add(this.iterN_textBox);
            this.result_groupBox.Controls.Add(this.rms3_label);
            this.result_groupBox.Controls.Add(this.rms2_label);
            this.result_groupBox.Controls.Add(this.perIdx_textBox);
            this.result_groupBox.Controls.Add(this.rms3_textBox);
            this.result_groupBox.Controls.Add(this.rms1_label);
            this.result_groupBox.Controls.Add(this.rms2_textBox);
            this.result_groupBox.Controls.Add(this.delta5_textBox);
            this.result_groupBox.Controls.Add(this.rms1_textBox);
            this.result_groupBox.Controls.Add(this.delta4_textBox);
            this.result_groupBox.Controls.Add(this.delta6_textBox);
            this.result_groupBox.Controls.Add(this.delta3_textBox);
            this.result_groupBox.Controls.Add(this.delta2_textBox);
            this.result_groupBox.Controls.Add(this.delta6_label);
            this.result_groupBox.Controls.Add(this.delta5_label);
            this.result_groupBox.Controls.Add(this.delta4_label);
            this.result_groupBox.Controls.Add(this.delta3_label);
            this.result_groupBox.Controls.Add(this.delta2_label);
            this.result_groupBox.Controls.Add(this.delta1_label);
            this.result_groupBox.Controls.Add(this.delta1_textBox);
            this.result_groupBox.Location = new System.Drawing.Point(233, 23);
            this.result_groupBox.Name = "result_groupBox";
            this.result_groupBox.Size = new System.Drawing.Size(506, 242);
            this.result_groupBox.TabIndex = 28;
            this.result_groupBox.TabStop = false;
            this.result_groupBox.Text = "Result";
            // 
            // viewCov_button
            // 
            this.viewCov_button.Location = new System.Drawing.Point(298, 198);
            this.viewCov_button.Name = "viewCov_button";
            this.viewCov_button.Size = new System.Drawing.Size(190, 22);
            this.viewCov_button.TabIndex = 33;
            this.viewCov_button.Text = "View Covariance Matrix";
            this.viewCov_button.UseVisualStyleBackColor = true;
            // 
            // viewRes_button
            // 
            this.viewRes_button.Location = new System.Drawing.Point(298, 170);
            this.viewRes_button.Name = "viewRes_button";
            this.viewRes_button.Size = new System.Drawing.Size(190, 22);
            this.viewRes_button.TabIndex = 32;
            this.viewRes_button.Text = "View Residual Plot";
            this.viewRes_button.UseVisualStyleBackColor = true;
            this.viewRes_button.Click += new System.EventHandler(this.viewRes_button_Click);
            // 
            // setEst_button
            // 
            this.setEst_button.Location = new System.Drawing.Point(23, 155);
            this.setEst_button.Name = "setEst_button";
            this.setEst_button.Size = new System.Drawing.Size(190, 22);
            this.setEst_button.TabIndex = 29;
            this.setEst_button.Text = "Set Estimator";
            this.setEst_button.UseVisualStyleBackColor = true;
            this.setEst_button.Click += new System.EventHandler(this.setEst_button_Click);
            // 
            // genCMD_button
            // 
            this.genCMD_button.Location = new System.Drawing.Point(23, 242);
            this.genCMD_button.Name = "genCMD_button";
            this.genCMD_button.Size = new System.Drawing.Size(190, 22);
            this.genCMD_button.TabIndex = 31;
            this.genCMD_button.Text = "Tracking Command Generator";
            this.genCMD_button.UseVisualStyleBackColor = true;
            this.genCMD_button.Click += new System.EventHandler(this.genCMD_button_Click);
            // 
            // genEph_button
            // 
            this.genEph_button.Location = new System.Drawing.Point(23, 214);
            this.genEph_button.Name = "genEph_button";
            this.genEph_button.Size = new System.Drawing.Size(190, 22);
            this.genEph_button.TabIndex = 30;
            this.genEph_button.Text = "Ephemeris Generator";
            this.genEph_button.UseVisualStyleBackColor = true;
            this.genEph_button.Click += new System.EventHandler(this.genEph_button_Click);
            // 
            // obs_groupBox
            // 
            this.obs_groupBox.Location = new System.Drawing.Point(22, 289);
            this.obs_groupBox.Name = "obs_groupBox";
            this.obs_groupBox.Size = new System.Drawing.Size(191, 187);
            this.obs_groupBox.TabIndex = 32;
            this.obs_groupBox.TabStop = false;
            this.obs_groupBox.Text = "Observation";
            // 
            // run_button
            // 
            this.run_button.Location = new System.Drawing.Point(389, 354);
            this.run_button.Name = "run_button";
            this.run_button.Size = new System.Drawing.Size(75, 23);
            this.run_button.TabIndex = 33;
            this.run_button.Text = "Run";
            this.run_button.UseVisualStyleBackColor = true;
            this.run_button.Click += new System.EventHandler(this.run_button_Click);
            // 
            // exit_button
            // 
            this.exit_button.Location = new System.Drawing.Point(470, 354);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(75, 23);
            this.exit_button.TabIndex = 34;
            this.exit_button.Text = "Exit";
            this.exit_button.UseVisualStyleBackColor = true;
            // 
            // bodyLoc_button
            // 
            this.bodyLoc_button.Location = new System.Drawing.Point(23, 186);
            this.bodyLoc_button.Name = "bodyLoc_button";
            this.bodyLoc_button.Size = new System.Drawing.Size(190, 22);
            this.bodyLoc_button.TabIndex = 35;
            this.bodyLoc_button.Text = "Body Location Generator";
            this.bodyLoc_button.UseVisualStyleBackColor = true;
            this.bodyLoc_button.Click += new System.EventHandler(this.bodyLoc_button_Click);
            // 
            // testRunLS_button
            // 
            this.testRunLS_button.Location = new System.Drawing.Point(380, 289);
            this.testRunLS_button.Name = "testRunLS_button";
            this.testRunLS_button.Size = new System.Drawing.Size(110, 23);
            this.testRunLS_button.TabIndex = 36;
            this.testRunLS_button.Text = "Test Run (LS)";
            this.testRunLS_button.UseVisualStyleBackColor = true;
            this.testRunLS_button.Click += new System.EventHandler(this.testRunLS_button_Click);
            // 
            // testRunEKF_button
            // 
            this.testRunEKF_button.Location = new System.Drawing.Point(514, 289);
            this.testRunEKF_button.Name = "testRunEKF_button";
            this.testRunEKF_button.Size = new System.Drawing.Size(110, 23);
            this.testRunEKF_button.TabIndex = 37;
            this.testRunEKF_button.Text = "Test Run (EKF)";
            this.testRunEKF_button.UseVisualStyleBackColor = true;
            this.testRunEKF_button.Click += new System.EventHandler(this.testRunEKF_button_Click);
            // 
            // setLaser_button
            // 
            this.setLaser_button.Location = new System.Drawing.Point(639, 380);
            this.setLaser_button.Name = "setLaser_button";
            this.setLaser_button.Size = new System.Drawing.Size(190, 22);
            this.setLaser_button.TabIndex = 38;
            this.setLaser_button.Text = "Set Laser";
            this.setLaser_button.UseVisualStyleBackColor = true;
            this.setLaser_button.Click += new System.EventHandler(this.button1_Click);
            // 
            // OAS_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1201, 826);
            this.Controls.Add(this.setLaser_button);
            this.Controls.Add(this.testRunEKF_button);
            this.Controls.Add(this.testRunLS_button);
            this.Controls.Add(this.bodyLoc_button);
            this.Controls.Add(this.exit_button);
            this.Controls.Add(this.run_button);
            this.Controls.Add(this.obs_groupBox);
            this.Controls.Add(this.genCMD_button);
            this.Controls.Add(this.genEph_button);
            this.Controls.Add(this.setEst_button);
            this.Controls.Add(this.result_groupBox);
            this.Controls.Add(this.loadObs_button);
            this.Controls.Add(this.setMea_button);
            this.Controls.Add(this.setObject_button);
            this.Controls.Add(this.setDyn_button);
            this.Name = "OAS_Main";
            this.Text = "NSLR-OAS::Main";
            this.Load += new System.EventHandler(this.OAS_Main_Load);
            this.result_groupBox.ResumeLayout(false);
            this.result_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button setDyn_button;
        private System.Windows.Forms.Button setObject_button;
        private System.Windows.Forms.Button setMea_button;
        private System.Windows.Forms.Button loadObs_button;
        private System.Windows.Forms.OpenFileDialog obsFileDialog;
        private System.Windows.Forms.TextBox delta1_textBox;
        private System.Windows.Forms.Label delta1_label;
        private System.Windows.Forms.Label delta2_label;
        private System.Windows.Forms.Label delta3_label;
        private System.Windows.Forms.Label delta4_label;
        private System.Windows.Forms.Label delta5_label;
        private System.Windows.Forms.Label delta6_label;
        private System.Windows.Forms.TextBox delta2_textBox;
        private System.Windows.Forms.TextBox delta3_textBox;
        private System.Windows.Forms.TextBox delta6_textBox;
        private System.Windows.Forms.TextBox delta4_textBox;
        private System.Windows.Forms.TextBox delta5_textBox;
        private System.Windows.Forms.Label rms1_label;
        private System.Windows.Forms.TextBox rms1_textBox;
        private System.Windows.Forms.Label rms2_label;
        private System.Windows.Forms.TextBox rms2_textBox;
        private System.Windows.Forms.Label rms3_label;
        private System.Windows.Forms.TextBox rms3_textBox;
        private System.Windows.Forms.Label iter_label;
        private System.Windows.Forms.TextBox iterN_textBox;
        private System.Windows.Forms.Label perIdx_label;
        private System.Windows.Forms.TextBox perIdx_textBox;
        private System.Windows.Forms.GroupBox result_groupBox;
        private System.Windows.Forms.Button viewCov_button;
        private System.Windows.Forms.Button viewRes_button;
        private System.Windows.Forms.Button setEst_button;
        private System.Windows.Forms.Button genCMD_button;
        private System.Windows.Forms.Button genEph_button;
        private System.Windows.Forms.GroupBox obs_groupBox;
        private System.Windows.Forms.Button run_button;
        private System.Windows.Forms.Button exit_button;
        private System.Windows.Forms.Button bodyLoc_button;
        private System.Windows.Forms.Button testRunLS_button;
        private System.Windows.Forms.Button testRunEKF_button;
        private System.Windows.Forms.Button setLaser_button;
    }
}