namespace NSLR_ObservationControl.Module
{
    partial class CAMs
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox_preview = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_preview)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.OliveDrab;
            this.button1.FlatAppearance.BorderSize = 3;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(0, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(168, 44);
            this.button1.TabIndex = 0;
            this.button1.Tag = "1";
            this.button1.Text = "IR";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.deviceSelected_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.OliveDrab;
            this.button2.FlatAppearance.BorderSize = 3;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(180, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(191, 44);
            this.button2.TabIndex = 0;
            this.button2.Tag = "2";
            this.button2.Text = "EMCCD";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.deviceSelected_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.OliveDrab;
            this.button3.FlatAppearance.BorderSize = 3;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(382, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(183, 44);
            this.button3.TabIndex = 0;
            this.button3.Tag = "3";
            this.button3.Text = "스타카메라";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.deviceSelected_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.OliveDrab;
            this.button4.FlatAppearance.BorderSize = 3;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(578, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(189, 44);
            this.button4.TabIndex = 0;
            this.button4.Tag = "4";
            this.button4.Text = "추적마운트";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.deviceSelected_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox_preview);
            this.panel1.Location = new System.Drawing.Point(1, 54);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(770, 382);
            this.panel1.TabIndex = 37;
            // 
            // pictureBox_preview
            // 
            this.pictureBox_preview.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_preview.Name = "pictureBox_preview";
            this.pictureBox_preview.Size = new System.Drawing.Size(770, 382);
            this.pictureBox_preview.TabIndex = 1;
            this.pictureBox_preview.TabStop = false;
            this.pictureBox_preview.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_preview_Paint);
            this.pictureBox_preview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_preview_MouseDown);
            this.pictureBox_preview.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_preview_MouseMove);
            this.pictureBox_preview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox_preview_MouseUp);
            // 
            // CAMs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.DoubleBuffered = true;
            this.Name = "CAMs";
            this.Size = new System.Drawing.Size(771, 437);
            this.Load += new System.EventHandler(this.CAMs_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CAMs_KeyDown);
            this.Leave += new System.EventHandler(this.CAMs_Leave);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_preview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        public System.Windows.Forms.PictureBox pictureBox_preview;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Panel panel1;
    }
}
