namespace NSLR_ObservationControl.Module
{
    partial class Observation_Control
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
            this.panel22 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.ObservationStart = new System.Windows.Forms.Button();
            this.ObservationStop = new System.Windows.Forms.Button();
            this.ObservationLazerStart = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btnStopScan = new System.Windows.Forms.Button();
            this.panel22.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel22
            // 
            this.panel22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(54)))), ((int)(((byte)(78)))));
            this.panel22.Controls.Add(this.btnStopScan);
            this.panel22.Controls.Add(this.button2);
            this.panel22.Controls.Add(this.label2);
            this.panel22.Location = new System.Drawing.Point(0, 0);
            this.panel22.Name = "panel22";
            this.panel22.Size = new System.Drawing.Size(770, 50);
            this.panel22.TabIndex = 155;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.label2.Size = new System.Drawing.Size(764, 50);
            this.label2.TabIndex = 1;
            this.label2.Text = "관측 제어";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ObservationStart
            // 
            this.ObservationStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ObservationStart.BackColor = System.Drawing.Color.OliveDrab;
            this.ObservationStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ObservationStart.FlatAppearance.BorderSize = 3;
            this.ObservationStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ObservationStart.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ObservationStart.ForeColor = System.Drawing.Color.GhostWhite;
            this.ObservationStart.Image = global::NSLR_ObservationControl.Properties.Resources.icons8_play_30;
            this.ObservationStart.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ObservationStart.Location = new System.Drawing.Point(136, 76);
            this.ObservationStart.Name = "ObservationStart";
            this.ObservationStart.Size = new System.Drawing.Size(120, 54);
            this.ObservationStart.TabIndex = 151;
            this.ObservationStart.Text = "시작";
            this.ObservationStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ObservationStart.UseVisualStyleBackColor = false;
            this.ObservationStart.Click += new System.EventHandler(this.button_observation_start);
            // 
            // ObservationStop
            // 
            this.ObservationStop.BackColor = System.Drawing.Color.OliveDrab;
            this.ObservationStop.FlatAppearance.BorderSize = 3;
            this.ObservationStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ObservationStop.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ObservationStop.ForeColor = System.Drawing.Color.GhostWhite;
            this.ObservationStop.Image = global::NSLR_ObservationControl.Properties.Resources.icons8_stop_30;
            this.ObservationStop.Location = new System.Drawing.Point(627, 77);
            this.ObservationStop.Name = "ObservationStop";
            this.ObservationStop.Size = new System.Drawing.Size(120, 54);
            this.ObservationStop.TabIndex = 153;
            this.ObservationStop.Text = "종료";
            this.ObservationStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ObservationStop.UseVisualStyleBackColor = false;
            this.ObservationStop.Click += new System.EventHandler(this.button3_Click);
            // 
            // ObservationLazerStart
            // 
            this.ObservationLazerStart.BackColor = System.Drawing.Color.OliveDrab;
            this.ObservationLazerStart.FlatAppearance.BorderSize = 3;
            this.ObservationLazerStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ObservationLazerStart.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ObservationLazerStart.ForeColor = System.Drawing.Color.GhostWhite;
            this.ObservationLazerStart.Image = global::NSLR_ObservationControl.Properties.Resources.icons8_explode_30;
            this.ObservationLazerStart.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.ObservationLazerStart.Location = new System.Drawing.Point(302, 77);
            this.ObservationLazerStart.Name = "ObservationLazerStart";
            this.ObservationLazerStart.Size = new System.Drawing.Size(120, 54);
            this.ObservationLazerStart.TabIndex = 154;
            this.ObservationLazerStart.Text = "L.발사";
            this.ObservationLazerStart.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ObservationLazerStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ObservationLazerStart.UseVisualStyleBackColor = false;
            this.ObservationLazerStart.Click += new System.EventHandler(this.button2_Click_2);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.OliveDrab;
            this.button4.FlatAppearance.BorderSize = 3;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button4.ForeColor = System.Drawing.Color.GhostWhite;
            this.button4.Image = global::NSLR_ObservationControl.Properties.Resources.icons8_explode_30;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.button4.Location = new System.Drawing.Point(464, 77);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(120, 54);
            this.button4.TabIndex = 156;
            this.button4.Text = "L.중지";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button1
            // 
            this.button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button1.BackColor = System.Drawing.Color.OliveDrab;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.FlatAppearance.BorderSize = 3;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.ForeColor = System.Drawing.Color.GhostWhite;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.Location = new System.Drawing.Point(8, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 52);
            this.button1.TabIndex = 151;
            this.button1.Text = "준비";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button_observation_ready);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(31, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 12);
            this.label1.TabIndex = 157;
            this.label1.Text = "label1";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.OliveDrab;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.ForeColor = System.Drawing.SystemColors.Control;
            this.button2.Location = new System.Drawing.Point(74, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 30);
            this.button2.TabIndex = 158;
            this.button2.Text = "자동 스캔";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.AutoScan_Click);
            // 
            // btnStopScan
            // 
            this.btnStopScan.BackColor = System.Drawing.Color.OliveDrab;
            this.btnStopScan.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStopScan.ForeColor = System.Drawing.SystemColors.Control;
            this.btnStopScan.Location = new System.Drawing.Point(184, 10);
            this.btnStopScan.Name = "btnStopScan";
            this.btnStopScan.Size = new System.Drawing.Size(104, 30);
            this.btnStopScan.TabIndex = 158;
            this.btnStopScan.Text = "스캔 중단";
            this.btnStopScan.UseVisualStyleBackColor = false;
            this.btnStopScan.Click += new System.EventHandler(this.btnStopScan_Click);
            // 
            // Observation_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.panel22);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ObservationStart);
            this.Controls.Add(this.ObservationStop);
            this.Controls.Add(this.ObservationLazerStart);
            this.Name = "Observation_Control";
            this.Size = new System.Drawing.Size(770, 178);
            this.panel22.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel22;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ObservationStart;
        private System.Windows.Forms.Button ObservationStop;
        private System.Windows.Forms.Button ObservationLazerStart;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnStopScan;
    }
}
