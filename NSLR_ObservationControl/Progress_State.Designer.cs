namespace NSLR_ObservationControl
{
    partial class Progress_State
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
            this.state_label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.state_label2 = new System.Windows.Forms.Label();
            this.progressBar3 = new System.Windows.Forms.ProgressBar();
            this.state_label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // state_label1
            // 
            this.state_label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.state_label1.Location = new System.Drawing.Point(44, 52);
            this.state_label1.Name = "state_label1";
            this.state_label1.Size = new System.Drawing.Size(244, 19);
            this.state_label1.TabIndex = 3;
            this.state_label1.Text = "환경 구성 전";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(47, 21);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(311, 27);
            this.progressBar1.TabIndex = 2;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(47, 84);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(311, 27);
            this.progressBar2.TabIndex = 2;
            // 
            // state_label2
            // 
            this.state_label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.state_label2.Location = new System.Drawing.Point(44, 115);
            this.state_label2.Name = "state_label2";
            this.state_label2.Size = new System.Drawing.Size(244, 19);
            this.state_label2.TabIndex = 3;
            this.state_label2.Text = "TLE 목록 생성 전";
            // 
            // progressBar3
            // 
            this.progressBar3.Location = new System.Drawing.Point(47, 147);
            this.progressBar3.Name = "progressBar3";
            this.progressBar3.Size = new System.Drawing.Size(311, 27);
            this.progressBar3.TabIndex = 2;
            // 
            // state_label3
            // 
            this.state_label3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.state_label3.Location = new System.Drawing.Point(44, 178);
            this.state_label3.Name = "state_label3";
            this.state_label3.Size = new System.Drawing.Size(244, 19);
            this.state_label3.TabIndex = 3;
            this.state_label3.Text = "CPF 목록 생성 전";
            // 
            // Progress_State
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 215);
            this.Controls.Add(this.state_label3);
            this.Controls.Add(this.state_label2);
            this.Controls.Add(this.state_label1);
            this.Controls.Add(this.progressBar3);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Progress_State";
            this.Text = "Progress_State";
            this.Load += new System.EventHandler(this.Progress_State_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label state_label1;
        public System.Windows.Forms.ProgressBar progressBar1;
        public System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label state_label2;
        public System.Windows.Forms.ProgressBar progressBar3;
        private System.Windows.Forms.Label state_label3;
    }
}