namespace NSLR_ObservationControl.Module
{
    partial class starCAM_ASI2600MMPro
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
            this.btnCapture = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label_ExposureTime_value = new System.Windows.Forms.Label();
            this.cb_flip = new System.Windows.Forms.ComboBox();
            this.pictureBox_starCAM = new System.Windows.Forms.PictureBox();
            this.label_brightness_value = new System.Windows.Forms.Label();
            this.label_Gamma_value = new System.Windows.Forms.Label();
            this.trackBar_Gain = new System.Windows.Forms.TrackBar();
            this.trackBar_Exposure = new System.Windows.Forms.TrackBar();
            this.trackBar_Brightness = new System.Windows.Forms.TrackBar();
            this.trackBar_Gamma = new System.Windows.Forms.TrackBar();
            this.label_temp = new System.Windows.Forms.Label();
            this.label_temperature = new System.Windows.Forms.Label();
            this.label_Gain = new System.Windows.Forms.Label();
            this.label_gain_value = new System.Windows.Forms.Label();
            this.label_ExposureTime = new System.Windows.Forms.Label();
            this.label_brightness = new System.Windows.Forms.Label();
            this.label_Gamma = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_starCAM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Gain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Exposure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Brightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Gamma)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCapture
            // 
            this.btnCapture.BackColor = System.Drawing.Color.Gray;
            this.btnCapture.FlatAppearance.BorderSize = 5;
            this.btnCapture.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnCapture.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            this.btnCapture.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCapture.ForeColor = System.Drawing.Color.Black;
            this.btnCapture.Location = new System.Drawing.Point(590, 330);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(165, 37);
            this.btnCapture.TabIndex = 2;
            this.btnCapture.Text = "Capture";
            this.btnCapture.UseVisualStyleBackColor = false;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(584, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 14);
            this.label4.TabIndex = 0;
            this.label4.Text = "Flip Control";
            this.label4.Visible = false;
            // 
            // label_ExposureTime_value
            // 
            this.label_ExposureTime_value.AutoSize = true;
            this.label_ExposureTime_value.BackColor = System.Drawing.Color.Transparent;
            this.label_ExposureTime_value.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_ExposureTime_value.Location = new System.Drawing.Point(62, 96);
            this.label_ExposureTime_value.Name = "label_ExposureTime_value";
            this.label_ExposureTime_value.Size = new System.Drawing.Size(43, 14);
            this.label_ExposureTime_value.TabIndex = 0;
            this.label_ExposureTime_value.Text = "0000";
            this.label_ExposureTime_value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_flip
            // 
            this.cb_flip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_flip.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.cb_flip.Enabled = false;
            this.cb_flip.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cb_flip.ForeColor = System.Drawing.Color.Linen;
            this.cb_flip.FormattingEnabled = true;
            this.cb_flip.Items.AddRange(new object[] {
            "0 NONE ",
            "1 Horizontal",
            "2 Vertical",
            "3 Both "});
            this.cb_flip.Location = new System.Drawing.Point(690, 69);
            this.cb_flip.Name = "cb_flip";
            this.cb_flip.Size = new System.Drawing.Size(61, 21);
            this.cb_flip.TabIndex = 2;
            this.cb_flip.Visible = false;
            // 
            // pictureBox_starCAM
            // 
            this.pictureBox_starCAM.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_starCAM.Location = new System.Drawing.Point(2, 0);
            this.pictureBox_starCAM.Name = "pictureBox_starCAM";
            this.pictureBox_starCAM.Size = new System.Drawing.Size(770, 385);
            this.pictureBox_starCAM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_starCAM.TabIndex = 0;
            this.pictureBox_starCAM.TabStop = false;
            this.pictureBox_starCAM.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_starCAM_Paint);
            this.pictureBox_starCAM.DoubleClick += new System.EventHandler(this.pictureBox_starCAM_DoubleClick);
            // 
            // label_brightness_value
            // 
            this.label_brightness_value.AutoSize = true;
            this.label_brightness_value.BackColor = System.Drawing.Color.Transparent;
            this.label_brightness_value.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_brightness_value.Location = new System.Drawing.Point(15, 256);
            this.label_brightness_value.Name = "label_brightness_value";
            this.label_brightness_value.Size = new System.Drawing.Size(25, 14);
            this.label_brightness_value.TabIndex = 6;
            this.label_brightness_value.Text = "00";
            this.label_brightness_value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Gamma_value
            // 
            this.label_Gamma_value.AutoSize = true;
            this.label_Gamma_value.BackColor = System.Drawing.Color.Transparent;
            this.label_Gamma_value.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Gamma_value.Location = new System.Drawing.Point(70, 254);
            this.label_Gamma_value.Name = "label_Gamma_value";
            this.label_Gamma_value.Size = new System.Drawing.Size(25, 14);
            this.label_Gamma_value.TabIndex = 6;
            this.label_Gamma_value.Text = "00";
            // 
            // trackBar_Gain
            // 
            this.trackBar_Gain.AllowDrop = true;
            this.trackBar_Gain.Location = new System.Drawing.Point(16, 114);
            this.trackBar_Gain.Maximum = 700;
            this.trackBar_Gain.Name = "trackBar_Gain";
            this.trackBar_Gain.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_Gain.Size = new System.Drawing.Size(45, 100);
            this.trackBar_Gain.TabIndex = 7;
            this.trackBar_Gain.Value = 200;
            this.trackBar_Gain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBar_Gain_MouseUp);
            // 
            // trackBar_Exposure
            // 
            this.trackBar_Exposure.Location = new System.Drawing.Point(68, 114);
            this.trackBar_Exposure.Maximum = 3000;
            this.trackBar_Exposure.Minimum = 32;
            this.trackBar_Exposure.Name = "trackBar_Exposure";
            this.trackBar_Exposure.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_Exposure.Size = new System.Drawing.Size(45, 100);
            this.trackBar_Exposure.TabIndex = 7;
            this.trackBar_Exposure.Value = 280;
            this.trackBar_Exposure.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBar_Exposure_MouseUp);
            // 
            // trackBar_Brightness
            // 
            this.trackBar_Brightness.Location = new System.Drawing.Point(16, 270);
            this.trackBar_Brightness.Maximum = 20;
            this.trackBar_Brightness.Name = "trackBar_Brightness";
            this.trackBar_Brightness.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_Brightness.Size = new System.Drawing.Size(45, 100);
            this.trackBar_Brightness.TabIndex = 7;
            this.trackBar_Brightness.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBar_Brightness_MouseUp);
            // 
            // trackBar_Gamma
            // 
            this.trackBar_Gamma.Location = new System.Drawing.Point(68, 270);
            this.trackBar_Gamma.Maximum = 20;
            this.trackBar_Gamma.Name = "trackBar_Gamma";
            this.trackBar_Gamma.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_Gamma.Size = new System.Drawing.Size(45, 100);
            this.trackBar_Gamma.TabIndex = 7;
            this.trackBar_Gamma.Value = 10;
            this.trackBar_Gamma.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBar_Gamma_MouseUp);
            // 
            // label_temp
            // 
            this.label_temp.AutoSize = true;
            this.label_temp.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_temp.Location = new System.Drawing.Point(715, 15);
            this.label_temp.Name = "label_temp";
            this.label_temp.Size = new System.Drawing.Size(0, 14);
            this.label_temp.TabIndex = 8;
            // 
            // label_temperature
            // 
            this.label_temperature.AutoSize = true;
            this.label_temperature.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_temperature.ForeColor = System.Drawing.Color.Yellow;
            this.label_temperature.Location = new System.Drawing.Point(14, 15);
            this.label_temperature.Name = "label_temperature";
            this.label_temperature.Size = new System.Drawing.Size(0, 20);
            this.label_temperature.TabIndex = 9;
            // 
            // label_Gain
            // 
            this.label_Gain.AutoSize = true;
            this.label_Gain.BackColor = System.Drawing.Color.Transparent;
            this.label_Gain.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Gain.ForeColor = System.Drawing.Color.Gray;
            this.label_Gain.Location = new System.Drawing.Point(10, 72);
            this.label_Gain.Name = "label_Gain";
            this.label_Gain.Size = new System.Drawing.Size(41, 14);
            this.label_Gain.TabIndex = 0;
            this.label_Gain.Text = "Gain";
            // 
            // label_gain_value
            // 
            this.label_gain_value.AutoSize = true;
            this.label_gain_value.BackColor = System.Drawing.Color.Transparent;
            this.label_gain_value.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_gain_value.Location = new System.Drawing.Point(14, 97);
            this.label_gain_value.Name = "label_gain_value";
            this.label_gain_value.Size = new System.Drawing.Size(34, 14);
            this.label_gain_value.TabIndex = 0;
            this.label_gain_value.Text = "000";
            this.label_gain_value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_ExposureTime
            // 
            this.label_ExposureTime.AutoSize = true;
            this.label_ExposureTime.BackColor = System.Drawing.Color.Transparent;
            this.label_ExposureTime.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_ExposureTime.ForeColor = System.Drawing.Color.Gray;
            this.label_ExposureTime.Location = new System.Drawing.Point(64, 72);
            this.label_ExposureTime.Name = "label_ExposureTime";
            this.label_ExposureTime.Size = new System.Drawing.Size(42, 14);
            this.label_ExposureTime.TabIndex = 0;
            this.label_ExposureTime.Text = "Exp.";
            // 
            // label_brightness
            // 
            this.label_brightness.AutoSize = true;
            this.label_brightness.BackColor = System.Drawing.Color.Transparent;
            this.label_brightness.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_brightness.ForeColor = System.Drawing.Color.Gray;
            this.label_brightness.Location = new System.Drawing.Point(14, 234);
            this.label_brightness.Name = "label_brightness";
            this.label_brightness.Size = new System.Drawing.Size(33, 14);
            this.label_brightness.TabIndex = 6;
            this.label_brightness.Text = "Bri.";
            // 
            // label_Gamma
            // 
            this.label_Gamma.AutoSize = true;
            this.label_Gamma.BackColor = System.Drawing.Color.Transparent;
            this.label_Gamma.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Gamma.ForeColor = System.Drawing.Color.Gray;
            this.label_Gamma.Location = new System.Drawing.Point(65, 234);
            this.label_Gamma.Name = "label_Gamma";
            this.label_Gamma.Size = new System.Drawing.Size(46, 14);
            this.label_Gamma.TabIndex = 6;
            this.label_Gamma.Text = "Gam.";
            // 
            // starCAM_ASI2600MMPro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.label_temperature);
            this.Controls.Add(this.label_temp);
            this.Controls.Add(this.trackBar_Gamma);
            this.Controls.Add(this.trackBar_Brightness);
            this.Controls.Add(this.trackBar_Exposure);
            this.Controls.Add(this.trackBar_Gain);
            this.Controls.Add(this.label_Gamma);
            this.Controls.Add(this.label_Gamma_value);
            this.Controls.Add(this.label_brightness);
            this.Controls.Add(this.label_brightness_value);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cb_flip);
            this.Controls.Add(this.label_ExposureTime);
            this.Controls.Add(this.label_ExposureTime_value);
            this.Controls.Add(this.btnCapture);
            this.Controls.Add(this.label_gain_value);
            this.Controls.Add(this.label_Gain);
            this.Controls.Add(this.pictureBox_starCAM);
            this.ForeColor = System.Drawing.Color.Cornsilk;
            this.Name = "starCAM_ASI2600MMPro";
            this.Size = new System.Drawing.Size(775, 388);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_starCAM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Gain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Exposure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Brightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Gamma)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_ExposureTime_value;
        private System.Windows.Forms.ComboBox cb_flip;
        private System.Windows.Forms.PictureBox pictureBox_starCAM;
        private System.Windows.Forms.Label label_brightness_value;
        private System.Windows.Forms.Label label_Gamma_value;
        private System.Windows.Forms.TrackBar trackBar_Gain;
        private System.Windows.Forms.TrackBar trackBar_Exposure;
        private System.Windows.Forms.TrackBar trackBar_Brightness;
        private System.Windows.Forms.TrackBar trackBar_Gamma;
        private System.Windows.Forms.Label label_temp;
        private System.Windows.Forms.Label label_temperature;
        private System.Windows.Forms.Label label_Gain;
        private System.Windows.Forms.Label label_gain_value;
        private System.Windows.Forms.Label label_ExposureTime;
        private System.Windows.Forms.Label label_brightness;
        private System.Windows.Forms.Label label_Gamma;
    }
}
