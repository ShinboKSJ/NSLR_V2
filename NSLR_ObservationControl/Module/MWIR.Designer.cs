namespace NSLR_ObservationControl.Module
{
    partial class MWIR
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
            this.pictureBox_preview = new System.Windows.Forms.PictureBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_preview)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_preview
            // 
            this.pictureBox_preview.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_preview.Name = "pictureBox_preview";
            this.pictureBox_preview.Size = new System.Drawing.Size(770, 382);
            this.pictureBox_preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_preview.TabIndex = 2;
            this.pictureBox_preview.TabStop = false;
            this.pictureBox_preview.Click += new System.EventHandler(this.pictureBox_preview_Click);
            this.pictureBox_preview.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_preview_Paint);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.Navy;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.Location = new System.Drawing.Point(619, 332);
            this.button6.Margin = new System.Windows.Forms.Padding(0);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(67, 28);
            this.button6.TabIndex = 49;
            this.button6.Tag = "12";
            this.button6.Text = "영상 저장";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.videoSave_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.Navy;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(50, 345);
            this.button5.Margin = new System.Windows.Forms.Padding(0);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(65, 28);
            this.button5.TabIndex = 41;
            this.button5.Tag = "1";
            this.button5.Text = "배율 ▼";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.MWIRFunciton_Click);
            // 
            // button17
            // 
            this.button17.BackColor = System.Drawing.Color.Navy;
            this.button17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button17.ForeColor = System.Drawing.Color.White;
            this.button17.Location = new System.Drawing.Point(538, 332);
            this.button17.Margin = new System.Windows.Forms.Padding(0);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(70, 28);
            this.button17.TabIndex = 42;
            this.button17.Tag = "12";
            this.button17.Text = "AGC Auto";
            this.button17.UseVisualStyleBackColor = false;
            this.button17.Click += new System.EventHandler(this.MWIRFunciton_Click);
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.Navy;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.ForeColor = System.Drawing.Color.White;
            this.button7.Location = new System.Drawing.Point(50, 317);
            this.button7.Margin = new System.Windows.Forms.Padding(0);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(65, 28);
            this.button7.TabIndex = 40;
            this.button7.Tag = "2";
            this.button7.Text = "배율 ▲";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.MWIRFunciton_Click);
            // 
            // button16
            // 
            this.button16.BackColor = System.Drawing.Color.Navy;
            this.button16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button16.ForeColor = System.Drawing.Color.White;
            this.button16.Location = new System.Drawing.Point(462, 317);
            this.button16.Margin = new System.Windows.Forms.Padding(0);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(65, 28);
            this.button16.TabIndex = 43;
            this.button16.Tag = "11";
            this.button16.Text = "Gain ▲";
            this.button16.UseVisualStyleBackColor = false;
            this.button16.Click += new System.EventHandler(this.MWIRFunciton_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.Navy;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.ForeColor = System.Drawing.Color.White;
            this.button8.Location = new System.Drawing.Point(203, 332);
            this.button8.Margin = new System.Windows.Forms.Padding(0);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(67, 28);
            this.button8.TabIndex = 39;
            this.button8.Tag = "3";
            this.button8.Text = "자동 초점";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.MWIRFunciton_Click);
            // 
            // button15
            // 
            this.button15.BackColor = System.Drawing.Color.Navy;
            this.button15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button15.ForeColor = System.Drawing.Color.White;
            this.button15.Location = new System.Drawing.Point(462, 345);
            this.button15.Margin = new System.Windows.Forms.Padding(0);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(65, 28);
            this.button15.TabIndex = 44;
            this.button15.Tag = "10";
            this.button15.Text = "Gain ▼";
            this.button15.UseVisualStyleBackColor = false;
            this.button15.Click += new System.EventHandler(this.MWIRFunciton_Click);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.Navy;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.ForeColor = System.Drawing.Color.White;
            this.button9.Location = new System.Drawing.Point(127, 345);
            this.button9.Margin = new System.Windows.Forms.Padding(0);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(65, 28);
            this.button9.TabIndex = 38;
            this.button9.Tag = "4";
            this.button9.Text = "초점 ▼";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.MWIRFunciton_Click);
            // 
            // button14
            // 
            this.button14.BackColor = System.Drawing.Color.Navy;
            this.button14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button14.ForeColor = System.Drawing.Color.White;
            this.button14.Location = new System.Drawing.Point(373, 317);
            this.button14.Margin = new System.Windows.Forms.Padding(0);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(77, 28);
            this.button14.TabIndex = 45;
            this.button14.Tag = "9";
            this.button14.Text = "노출시간 ▲";
            this.button14.UseVisualStyleBackColor = false;
            this.button14.Click += new System.EventHandler(this.MWIRFunciton_Click);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.Navy;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.ForeColor = System.Drawing.Color.White;
            this.button10.Location = new System.Drawing.Point(127, 317);
            this.button10.Margin = new System.Windows.Forms.Padding(0);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(65, 28);
            this.button10.TabIndex = 37;
            this.button10.Tag = "5";
            this.button10.Text = "초점 ▲";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.MWIRFunciton_Click);
            // 
            // button13
            // 
            this.button13.BackColor = System.Drawing.Color.Navy;
            this.button13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button13.ForeColor = System.Drawing.Color.White;
            this.button13.Location = new System.Drawing.Point(373, 345);
            this.button13.Margin = new System.Windows.Forms.Padding(0);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(77, 28);
            this.button13.TabIndex = 46;
            this.button13.Tag = "8";
            this.button13.Text = "노출시간 ▼";
            this.button13.UseVisualStyleBackColor = false;
            // 
            // button11
            // 
            this.button11.AutoSize = true;
            this.button11.BackColor = System.Drawing.Color.Navy;
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.ForeColor = System.Drawing.Color.White;
            this.button11.Location = new System.Drawing.Point(283, 317);
            this.button11.Margin = new System.Windows.Forms.Padding(0);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(80, 28);
            this.button11.TabIndex = 48;
            this.button11.Tag = "6";
            this.button11.Text = "NUC point1";
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new System.EventHandler(this.MWIRFunciton_Click);
            // 
            // button12
            // 
            this.button12.AutoSize = true;
            this.button12.BackColor = System.Drawing.Color.Navy;
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button12.ForeColor = System.Drawing.Color.White;
            this.button12.Location = new System.Drawing.Point(283, 345);
            this.button12.Margin = new System.Windows.Forms.Padding(0);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(80, 28);
            this.button12.TabIndex = 47;
            this.button12.Tag = "7";
            this.button12.Text = "NUC point2";
            this.button12.UseVisualStyleBackColor = false;
            this.button12.Click += new System.EventHandler(this.MWIRFunciton_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MWIR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.pictureBox_preview);
            this.Name = "MWIR";
            this.Size = new System.Drawing.Size(770, 382);
            this.Load += new System.EventHandler(this.MWIR_Load);
            this.Leave += new System.EventHandler(this.MWIR_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_preview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox pictureBox_preview;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Timer timer1;
    }
}
