namespace NSLR_ObservationControl.Module
{
    partial class EMCCD
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
            this.display = new PvGUIDotNet.PvDisplayControl();
            this.button_DigitalGain = new System.Windows.Forms.Button();
            this.textBox_DigitalGain = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button_EmGain = new System.Windows.Forms.Button();
            this.textBox_EMgain = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.receive_txtbox = new System.Windows.Forms.TextBox();
            this.send_txtbox = new System.Windows.Forms.TextBox();
            this.CommandSend_btn = new System.Windows.Forms.Button();
            this.AGCAuto_btn = new System.Windows.Forms.Button();
            this.gainSet1_btn = new System.Windows.Forms.Button();
            this.gainSet2_btn = new System.Windows.Forms.Button();
            this.ALCSet1_btn = new System.Windows.Forms.Button();
            this.ALCSet2_btn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // display
            // 
            this.display.BackColor = System.Drawing.Color.Transparent;
            this.display.BackgroundColor = System.Drawing.Color.DarkGray;
            this.display.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.display.Location = new System.Drawing.Point(0, 0);
            this.display.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            this.display.Name = "display";
            this.display.ROI = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.display.Size = new System.Drawing.Size(770, 382);
            this.display.TabIndex = 8;
            this.display.TextOverlay = "";
            this.display.TextOverlayColor = System.Drawing.Color.Lime;
            this.display.TextOverlayOffsetX = 10;
            this.display.TextOverlayOffsetY = 10;
            this.display.TextOverlaySize = 18;
            this.display.Paint += new System.Windows.Forms.PaintEventHandler(this.display_Paint);
            this.display.MouseClick += new System.Windows.Forms.MouseEventHandler(this.display_MouseClick);
            // 
            // button_DigitalGain
            // 
            this.button_DigitalGain.ForeColor = System.Drawing.Color.MidnightBlue;
            this.button_DigitalGain.Location = new System.Drawing.Point(653, 3);
            this.button_DigitalGain.Name = "button_DigitalGain";
            this.button_DigitalGain.Size = new System.Drawing.Size(114, 23);
            this.button_DigitalGain.TabIndex = 11;
            this.button_DigitalGain.Text = "Set Digital Gain ";
            this.button_DigitalGain.UseVisualStyleBackColor = true;
            this.button_DigitalGain.Visible = false;
            this.button_DigitalGain.Click += new System.EventHandler(this.button_DigitalGain_Click);
            // 
            // textBox_DigitalGain
            // 
            this.textBox_DigitalGain.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox_DigitalGain.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_DigitalGain.Location = new System.Drawing.Point(586, 0);
            this.textBox_DigitalGain.Name = "textBox_DigitalGain";
            this.textBox_DigitalGain.Size = new System.Drawing.Size(61, 26);
            this.textBox_DigitalGain.TabIndex = 10;
            this.textBox_DigitalGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_DigitalGain.Visible = false;
            this.textBox_DigitalGain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_DigitalGain_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(485, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 14);
            this.label6.TabIndex = 9;
            this.label6.Text = "DigitalGain";
            this.label6.Visible = false;
            // 
            // button_EmGain
            // 
            this.button_EmGain.ForeColor = System.Drawing.Color.MidnightBlue;
            this.button_EmGain.Location = new System.Drawing.Point(653, 32);
            this.button_EmGain.Name = "button_EmGain";
            this.button_EmGain.Size = new System.Drawing.Size(114, 23);
            this.button_EmGain.TabIndex = 14;
            this.button_EmGain.Text = "Set Digital Gain ";
            this.button_EmGain.UseVisualStyleBackColor = true;
            this.button_EmGain.Visible = false;
            this.button_EmGain.Click += new System.EventHandler(this.button_EmGain_Click);
            // 
            // textBox_EMgain
            // 
            this.textBox_EMgain.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBox_EMgain.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_EMgain.Location = new System.Drawing.Point(586, 29);
            this.textBox_EMgain.Name = "textBox_EMgain";
            this.textBox_EMgain.Size = new System.Drawing.Size(61, 26);
            this.textBox_EMgain.TabIndex = 13;
            this.textBox_EMgain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_EMgain.Visible = false;
            this.textBox_EMgain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox_EMgain_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(493, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 14);
            this.label1.TabIndex = 12;
            this.label1.Text = "EM Gain";
            this.label1.Visible = false;
            // 
            // receive_txtbox
            // 
            this.receive_txtbox.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.receive_txtbox.Location = new System.Drawing.Point(10, 348);
            this.receive_txtbox.Multiline = true;
            this.receive_txtbox.Name = "receive_txtbox";
            this.receive_txtbox.Size = new System.Drawing.Size(113, 29);
            this.receive_txtbox.TabIndex = 59;
            this.receive_txtbox.Visible = false;
            // 
            // send_txtbox
            // 
            this.send_txtbox.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.send_txtbox.Location = new System.Drawing.Point(10, 316);
            this.send_txtbox.Multiline = true;
            this.send_txtbox.Name = "send_txtbox";
            this.send_txtbox.Size = new System.Drawing.Size(113, 29);
            this.send_txtbox.TabIndex = 60;
            this.send_txtbox.Visible = false;
            // 
            // CommandSend_btn
            // 
            this.CommandSend_btn.AutoSize = true;
            this.CommandSend_btn.BackColor = System.Drawing.Color.SpringGreen;
            this.CommandSend_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CommandSend_btn.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CommandSend_btn.ForeColor = System.Drawing.Color.Black;
            this.CommandSend_btn.Location = new System.Drawing.Point(342, 344);
            this.CommandSend_btn.Margin = new System.Windows.Forms.Padding(0);
            this.CommandSend_btn.Name = "CommandSend_btn";
            this.CommandSend_btn.Size = new System.Drawing.Size(133, 32);
            this.CommandSend_btn.TabIndex = 53;
            this.CommandSend_btn.Tag = "6";
            this.CommandSend_btn.Text = "Command Send";
            this.CommandSend_btn.UseVisualStyleBackColor = false;
            this.CommandSend_btn.Visible = false;
            this.CommandSend_btn.Click += new System.EventHandler(this.CommandSend_btn_Click);
            // 
            // AGCAuto_btn
            // 
            this.AGCAuto_btn.AutoSize = true;
            this.AGCAuto_btn.BackColor = System.Drawing.Color.SpringGreen;
            this.AGCAuto_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AGCAuto_btn.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.AGCAuto_btn.ForeColor = System.Drawing.Color.Black;
            this.AGCAuto_btn.Location = new System.Drawing.Point(10, 176);
            this.AGCAuto_btn.Margin = new System.Windows.Forms.Padding(0);
            this.AGCAuto_btn.Name = "AGCAuto_btn";
            this.AGCAuto_btn.Size = new System.Drawing.Size(33, 32);
            this.AGCAuto_btn.TabIndex = 54;
            this.AGCAuto_btn.Tag = "5";
            this.AGCAuto_btn.Text = "00";
            this.AGCAuto_btn.UseVisualStyleBackColor = false;
            this.AGCAuto_btn.Click += new System.EventHandler(this.EMCCDFunction_Click);
            // 
            // gainSet1_btn
            // 
            this.gainSet1_btn.AutoSize = true;
            this.gainSet1_btn.BackColor = System.Drawing.Color.SpringGreen;
            this.gainSet1_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gainSet1_btn.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gainSet1_btn.ForeColor = System.Drawing.Color.Black;
            this.gainSet1_btn.Location = new System.Drawing.Point(10, 218);
            this.gainSet1_btn.Margin = new System.Windows.Forms.Padding(0);
            this.gainSet1_btn.Name = "gainSet1_btn";
            this.gainSet1_btn.Size = new System.Drawing.Size(61, 32);
            this.gainSet1_btn.TabIndex = 55;
            this.gainSet1_btn.Tag = "3";
            this.gainSet1_btn.Text = "Gain ▲";
            this.gainSet1_btn.UseVisualStyleBackColor = false;
            this.gainSet1_btn.Click += new System.EventHandler(this.EMCCDFunction_Click);
            // 
            // gainSet2_btn
            // 
            this.gainSet2_btn.AutoSize = true;
            this.gainSet2_btn.BackColor = System.Drawing.Color.SpringGreen;
            this.gainSet2_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gainSet2_btn.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gainSet2_btn.ForeColor = System.Drawing.Color.Black;
            this.gainSet2_btn.Location = new System.Drawing.Point(10, 259);
            this.gainSet2_btn.Margin = new System.Windows.Forms.Padding(0);
            this.gainSet2_btn.Name = "gainSet2_btn";
            this.gainSet2_btn.Size = new System.Drawing.Size(61, 32);
            this.gainSet2_btn.TabIndex = 56;
            this.gainSet2_btn.Tag = "4";
            this.gainSet2_btn.Text = "Gain ▼";
            this.gainSet2_btn.UseVisualStyleBackColor = false;
            this.gainSet2_btn.Click += new System.EventHandler(this.EMCCDFunction_Click);
            // 
            // ALCSet1_btn
            // 
            this.ALCSet1_btn.AutoSize = true;
            this.ALCSet1_btn.BackColor = System.Drawing.Color.SpringGreen;
            this.ALCSet1_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ALCSet1_btn.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ALCSet1_btn.ForeColor = System.Drawing.Color.Black;
            this.ALCSet1_btn.Location = new System.Drawing.Point(10, 300);
            this.ALCSet1_btn.Margin = new System.Windows.Forms.Padding(0);
            this.ALCSet1_btn.Name = "ALCSet1_btn";
            this.ALCSet1_btn.Size = new System.Drawing.Size(113, 32);
            this.ALCSet1_btn.TabIndex = 57;
            this.ALCSet1_btn.Tag = "1";
            this.ALCSet1_btn.Text = "노출시간(ALC) ▲";
            this.ALCSet1_btn.UseVisualStyleBackColor = false;
            this.ALCSet1_btn.Click += new System.EventHandler(this.EMCCDFunction_Click);
            // 
            // ALCSet2_btn
            // 
            this.ALCSet2_btn.AutoSize = true;
            this.ALCSet2_btn.BackColor = System.Drawing.Color.SpringGreen;
            this.ALCSet2_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ALCSet2_btn.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ALCSet2_btn.ForeColor = System.Drawing.Color.Black;
            this.ALCSet2_btn.Location = new System.Drawing.Point(10, 340);
            this.ALCSet2_btn.Margin = new System.Windows.Forms.Padding(0);
            this.ALCSet2_btn.Name = "ALCSet2_btn";
            this.ALCSet2_btn.Size = new System.Drawing.Size(113, 32);
            this.ALCSet2_btn.TabIndex = 58;
            this.ALCSet2_btn.Tag = "2";
            this.ALCSet2_btn.Text = "노출시간(ALC) ▼";
            this.ALCSet2_btn.UseVisualStyleBackColor = false;
            this.ALCSet2_btn.Click += new System.EventHandler(this.EMCCDFunction_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(692, 256);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 61;
            this.button1.Text = "저장 시작";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SaveStart_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(692, 285);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 62;
            this.button2.Text = "저장 종료";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.SaveStop_Click);
            // 
            // EMCCD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.receive_txtbox);
            this.Controls.Add(this.send_txtbox);
            this.Controls.Add(this.CommandSend_btn);
            this.Controls.Add(this.AGCAuto_btn);
            this.Controls.Add(this.gainSet1_btn);
            this.Controls.Add(this.gainSet2_btn);
            this.Controls.Add(this.ALCSet1_btn);
            this.Controls.Add(this.ALCSet2_btn);
            this.Controls.Add(this.button_EmGain);
            this.Controls.Add(this.textBox_EMgain);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_DigitalGain);
            this.Controls.Add(this.textBox_DigitalGain);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.display);
            this.Name = "EMCCD";
            this.Size = new System.Drawing.Size(770, 382);
            this.Load += new System.EventHandler(this.EMCCD_Load);
            this.Leave += new System.EventHandler(this.EMCCD_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public PvGUIDotNet.PvDisplayControl display;
        private System.Windows.Forms.Button button_DigitalGain;
        private System.Windows.Forms.TextBox textBox_DigitalGain;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_EmGain;
        private System.Windows.Forms.TextBox textBox_EMgain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox receive_txtbox;
        private System.Windows.Forms.TextBox send_txtbox;
        private System.Windows.Forms.Button CommandSend_btn;
        private System.Windows.Forms.Button AGCAuto_btn;
        private System.Windows.Forms.Button gainSet1_btn;
        private System.Windows.Forms.Button gainSet2_btn;
        private System.Windows.Forms.Button ALCSet1_btn;
        private System.Windows.Forms.Button ALCSet2_btn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}
