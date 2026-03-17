namespace NSLR_ObservationControl.Module
{
    partial class TIME
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
            this.label_UTC = new System.Windows.Forms.Label();
            this.label_KST = new System.Windows.Forms.Label();
            this.label_UTCdate = new System.Windows.Forms.Label();
            this.label_KSTdate = new System.Windows.Forms.Label();
            this.label_KSTtime = new System.Windows.Forms.Label();
            this.label_UTCtime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label_UTC
            // 
            this.label_UTC.AutoSize = true;
            this.label_UTC.BackColor = System.Drawing.Color.Transparent;
            this.label_UTC.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_UTC.ForeColor = System.Drawing.Color.Tan;
            this.label_UTC.Location = new System.Drawing.Point(363, 4);
            this.label_UTC.Name = "label_UTC";
            this.label_UTC.Size = new System.Drawing.Size(70, 30);
            this.label_UTC.TabIndex = 10;
            this.label_UTC.Text = "UTC";
            this.label_UTC.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_KST
            // 
            this.label_KST.AutoSize = true;
            this.label_KST.BackColor = System.Drawing.Color.Transparent;
            this.label_KST.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_KST.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label_KST.Location = new System.Drawing.Point(76, 4);
            this.label_KST.Name = "label_KST";
            this.label_KST.Size = new System.Drawing.Size(66, 30);
            this.label_KST.TabIndex = 11;
            this.label_KST.Text = "KST";
            this.label_KST.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_UTCdate
            // 
            this.label_UTCdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_UTCdate.ForeColor = System.Drawing.Color.AliceBlue;
            this.label_UTCdate.Location = new System.Drawing.Point(428, 3);
            this.label_UTCdate.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_UTCdate.Name = "label_UTCdate";
            this.label_UTCdate.Size = new System.Drawing.Size(150, 35);
            this.label_UTCdate.TabIndex = 7;
            this.label_UTCdate.Text = "----------";
            this.label_UTCdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_KSTdate
            // 
            this.label_KSTdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_KSTdate.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label_KSTdate.Location = new System.Drawing.Point(138, 3);
            this.label_KSTdate.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_KSTdate.Name = "label_KSTdate";
            this.label_KSTdate.Size = new System.Drawing.Size(150, 35);
            this.label_KSTdate.TabIndex = 9;
            this.label_KSTdate.Text = "----------";
            this.label_KSTdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_KSTtime
            // 
            this.label_KSTtime.AutoSize = true;
            this.label_KSTtime.Font = new System.Drawing.Font("Arial Narrow", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_KSTtime.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label_KSTtime.Location = new System.Drawing.Point(69, 29);
            this.label_KSTtime.Margin = new System.Windows.Forms.Padding(0);
            this.label_KSTtime.Name = "label_KSTtime";
            this.label_KSTtime.Size = new System.Drawing.Size(148, 66);
            this.label_KSTtime.TabIndex = 8;
            this.label_KSTtime.Text = "--------";
            this.label_KSTtime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_UTCtime
            // 
            this.label_UTCtime.AutoSize = true;
            this.label_UTCtime.Font = new System.Drawing.Font("Arial Narrow", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_UTCtime.ForeColor = System.Drawing.Color.Tan;
            this.label_UTCtime.Location = new System.Drawing.Point(358, 29);
            this.label_UTCtime.Margin = new System.Windows.Forms.Padding(0);
            this.label_UTCtime.Name = "label_UTCtime";
            this.label_UTCtime.Size = new System.Drawing.Size(148, 66);
            this.label_UTCtime.TabIndex = 6;
            this.label_UTCtime.Text = "--------";
            this.label_UTCtime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TIME
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(54)))), ((int)(((byte)(63)))));
            this.Controls.Add(this.label_UTC);
            this.Controls.Add(this.label_KST);
            this.Controls.Add(this.label_UTCdate);
            this.Controls.Add(this.label_KSTdate);
            this.Controls.Add(this.label_KSTtime);
            this.Controls.Add(this.label_UTCtime);
            this.Name = "TIME";
            this.Size = new System.Drawing.Size(709, 86);
            this.Load += new System.EventHandler(this.TIME_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_UTC;
        private System.Windows.Forms.Label label_KST;
        private System.Windows.Forms.Label label_UTCdate;
        private System.Windows.Forms.Label label_KSTdate;
        private System.Windows.Forms.Label label_KSTtime;
        private System.Windows.Forms.Label label_UTCtime;
    }
}
