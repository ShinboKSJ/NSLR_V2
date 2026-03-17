namespace NSLR_ObservationControl.Module
{
    partial class SystemDiagnostic_GPS
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("LEAP Indicator");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Clock Sync Check");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Receive TimeStamp");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Transmit TimeStamp");
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.groupBox3.Controls.Add(this.textBox6);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.treeView2);
            this.groupBox3.Controls.Add(this.button8);
            this.groupBox3.Controls.Add(this.button9);
            this.groupBox3.Font = new System.Drawing.Font("굴림", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.groupBox3.Location = new System.Drawing.Point(11, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(700, 700);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "GPS";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(349, 97);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(137, 30);
            this.textBox6.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(187, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "NTP Version";
            // 
            // treeView2
            // 
            this.treeView2.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.treeView2.CheckBoxes = true;
            this.treeView2.Location = new System.Drawing.Point(112, 195);
            this.treeView2.Name = "treeView2";
            treeNode1.Name = "노드1";
            treeNode1.Text = "LEAP Indicator";
            treeNode2.Name = "노드0";
            treeNode2.Text = "Clock Sync Check";
            treeNode3.Name = "노드0";
            treeNode3.Text = "Receive TimeStamp";
            treeNode4.Name = "노드1";
            treeNode4.Text = "Transmit TimeStamp";
            this.treeView2.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            this.treeView2.Size = new System.Drawing.Size(470, 269);
            this.treeView2.TabIndex = 7;
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.button8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button8.Location = new System.Drawing.Point(501, 594);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(148, 41);
            this.button8.TabIndex = 5;
            this.button8.Text = "해제";
            this.button8.UseVisualStyleBackColor = false;
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.button9.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.button9.Location = new System.Drawing.Point(71, 594);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(148, 41);
            this.button9.TabIndex = 6;
            this.button9.Text = "연결";
            this.button9.UseVisualStyleBackColor = false;
            // 
            // Diagnostic_GPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.Controls.Add(this.groupBox3);
            this.Name = "Diagnostic_GPS";
            this.Size = new System.Drawing.Size(722, 727);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
    }
}
