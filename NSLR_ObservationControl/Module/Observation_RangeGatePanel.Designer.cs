namespace NSLR_ObservationControl.Module
{
    partial class Observation_RangeGatePanel
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
            this.label3 = new System.Windows.Forms.Label();
            this.label_LebelSet = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label_rggMode = new System.Windows.Forms.Label();
            this.label_RGG = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label_NewGateWidth = new System.Windows.Forms.Label();
            this.numericUpDown_GateWidth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_GateSO = new System.Windows.Forms.NumericUpDown();
            this.label_NewGateSO = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label_GateSONow = new System.Windows.Forms.Label();
            this.label_GateWidthNow = new System.Windows.Forms.Label();
            this.btn_SetGateControl = new System.Windows.Forms.Button();
            this.panel22.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GateWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GateSO)).BeginInit();
            this.SuspendLayout();
            // 
            // panel22
            // 
            this.panel22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(54)))), ((int)(((byte)(78)))));
            this.panel22.Controls.Add(this.label2);
            this.panel22.Controls.Add(this.label3);
            this.panel22.Controls.Add(this.label_LebelSet);
            this.panel22.Controls.Add(this.label1);
            this.panel22.Controls.Add(this.label_rggMode);
            this.panel22.Controls.Add(this.label_RGG);
            this.panel22.Location = new System.Drawing.Point(0, -2);
            this.panel22.Margin = new System.Windows.Forms.Padding(0);
            this.panel22.Name = "panel22";
            this.panel22.Size = new System.Drawing.Size(780, 50);
            this.panel22.TabIndex = 48;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(529, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 38);
            this.label2.TabIndex = 1;
            this.label2.Text = "세팅";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(637, 6);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(126, 38);
            this.label3.TabIndex = 1;
            this.label3.Text = "콘솔 ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label_LebelSet
            // 
            this.label_LebelSet.Font = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_LebelSet.ForeColor = System.Drawing.Color.Gray;
            this.label_LebelSet.Location = new System.Drawing.Point(389, 3);
            this.label_LebelSet.Margin = new System.Windows.Forms.Padding(0);
            this.label_LebelSet.Name = "label_LebelSet";
            this.label_LebelSet.Size = new System.Drawing.Size(133, 43);
            this.label_LebelSet.TabIndex = 1;
            this.label_LebelSet.Text = "레벨조정";
            this.label_LebelSet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_LebelSet.Click += new System.EventHandler(this.레벨조정_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(248, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 43);
            this.label1.TabIndex = 1;
            this.label1.Text = "현재레벨";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_rggMode
            // 
            this.label_rggMode.BackColor = System.Drawing.Color.MidnightBlue;
            this.label_rggMode.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_rggMode.ForeColor = System.Drawing.Color.DarkOrange;
            this.label_rggMode.Location = new System.Drawing.Point(113, 8);
            this.label_rggMode.Margin = new System.Windows.Forms.Padding(0);
            this.label_rggMode.Name = "label_rggMode";
            this.label_rggMode.Size = new System.Drawing.Size(126, 35);
            this.label_rggMode.TabIndex = 1;
            this.label_rggMode.Text = "RGG모드";
            this.label_rggMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_rggMode.Click += new System.EventHandler(this.label_rggMode_Click);
            // 
            // label_RGG
            // 
            this.label_RGG.Font = new System.Drawing.Font("맑은 고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_RGG.ForeColor = System.Drawing.Color.White;
            this.label_RGG.Location = new System.Drawing.Point(3, 4);
            this.label_RGG.Margin = new System.Windows.Forms.Padding(0);
            this.label_RGG.Name = "label_RGG";
            this.label_RGG.Size = new System.Drawing.Size(110, 40);
            this.label_RGG.TabIndex = 1;
            this.label_RGG.Text = "RGG";
            this.label_RGG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_RGG.Click += new System.EventHandler(this.label_Click);
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.label19.Font = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(0, 48);
            this.label19.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.label19.Name = "label19";
            this.label19.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.label19.Size = new System.Drawing.Size(240, 50);
            this.label19.TabIndex = 114;
            this.label19.Text = "GATE Pulse Width";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_NewGateWidth
            // 
            this.label_NewGateWidth.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label_NewGateWidth.Font = new System.Drawing.Font("Consolas", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_NewGateWidth.ForeColor = System.Drawing.Color.Black;
            this.label_NewGateWidth.Location = new System.Drawing.Point(385, 48);
            this.label_NewGateWidth.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label_NewGateWidth.Name = "label_NewGateWidth";
            this.label_NewGateWidth.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.label_NewGateWidth.Size = new System.Drawing.Size(139, 50);
            this.label_NewGateWidth.TabIndex = 139;
            this.label_NewGateWidth.Text = "40";
            this.label_NewGateWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_NewGateWidth.MouseHover += new System.EventHandler(this.label_NewGateWidth_MouseHover);
            // 
            // numericUpDown_GateWidth
            // 
            this.numericUpDown_GateWidth.BackColor = System.Drawing.Color.LightGray;
            this.numericUpDown_GateWidth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDown_GateWidth.Font = new System.Drawing.Font("Consolas", 23F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_GateWidth.Location = new System.Drawing.Point(628, 58);
            this.numericUpDown_GateWidth.Name = "numericUpDown_GateWidth";
            this.numericUpDown_GateWidth.Size = new System.Drawing.Size(135, 39);
            this.numericUpDown_GateWidth.TabIndex = 147;
            this.numericUpDown_GateWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown_GateWidth.ValueChanged += new System.EventHandler(this.numericUpDown_GateWidth_ValueChanged);
            // 
            // numericUpDown_GateSO
            // 
            this.numericUpDown_GateSO.BackColor = System.Drawing.Color.LightGray;
            this.numericUpDown_GateSO.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDown_GateSO.Font = new System.Drawing.Font("Consolas", 23F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_GateSO.Location = new System.Drawing.Point(628, 109);
            this.numericUpDown_GateSO.Name = "numericUpDown_GateSO";
            this.numericUpDown_GateSO.Size = new System.Drawing.Size(135, 39);
            this.numericUpDown_GateSO.TabIndex = 147;
            this.numericUpDown_GateSO.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown_GateSO.ValueChanged += new System.EventHandler(this.numericUpDown_GateSO_ValueChanged);
            // 
            // label_NewGateSO
            // 
            this.label_NewGateSO.BackColor = System.Drawing.Color.LightSteelBlue;
            this.label_NewGateSO.Font = new System.Drawing.Font("Consolas", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_NewGateSO.ForeColor = System.Drawing.Color.Black;
            this.label_NewGateSO.Location = new System.Drawing.Point(385, 101);
            this.label_NewGateSO.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label_NewGateSO.Name = "label_NewGateSO";
            this.label_NewGateSO.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.label_NewGateSO.Size = new System.Drawing.Size(139, 50);
            this.label_NewGateSO.TabIndex = 139;
            this.label_NewGateSO.Text = "0";
            this.label_NewGateSO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.label12.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(0, 101);
            this.label12.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.label12.Name = "label12";
            this.label12.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.label12.Size = new System.Drawing.Size(241, 50);
            this.label12.TabIndex = 148;
            this.label12.Text = " GATE Pulse Start Offset";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_GateSONow
            // 
            this.label_GateSONow.BackColor = System.Drawing.Color.SeaShell;
            this.label_GateSONow.Font = new System.Drawing.Font("Consolas", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_GateSONow.ForeColor = System.Drawing.Color.Black;
            this.label_GateSONow.Location = new System.Drawing.Point(243, 101);
            this.label_GateSONow.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label_GateSONow.Name = "label_GateSONow";
            this.label_GateSONow.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.label_GateSONow.Size = new System.Drawing.Size(139, 50);
            this.label_GateSONow.TabIndex = 139;
            this.label_GateSONow.Text = "0";
            this.label_GateSONow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_GateWidthNow
            // 
            this.label_GateWidthNow.BackColor = System.Drawing.Color.SeaShell;
            this.label_GateWidthNow.Font = new System.Drawing.Font("Consolas", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_GateWidthNow.ForeColor = System.Drawing.Color.Black;
            this.label_GateWidthNow.Location = new System.Drawing.Point(243, 48);
            this.label_GateWidthNow.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.label_GateWidthNow.Name = "label_GateWidthNow";
            this.label_GateWidthNow.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.label_GateWidthNow.Size = new System.Drawing.Size(139, 50);
            this.label_GateWidthNow.TabIndex = 139;
            this.label_GateWidthNow.Text = "0";
            this.label_GateWidthNow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_SetGateControl
            // 
            this.btn_SetGateControl.BackColor = System.Drawing.Color.OliveDrab;
            this.btn_SetGateControl.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btn_SetGateControl.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_SetGateControl.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_SetGateControl.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btn_SetGateControl.Location = new System.Drawing.Point(526, 47);
            this.btn_SetGateControl.Name = "btn_SetGateControl";
            this.btn_SetGateControl.Size = new System.Drawing.Size(100, 105);
            this.btn_SetGateControl.TabIndex = 149;
            this.btn_SetGateControl.Text = "적용";
            this.btn_SetGateControl.UseVisualStyleBackColor = false;
            this.btn_SetGateControl.Click += new System.EventHandler(this.btn_SetGateControl_Click);
            // 
            // Observation_RangeGatePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.label_NewGateSO);
            this.Controls.Add(this.label_GateWidthNow);
            this.Controls.Add(this.btn_SetGateControl);
            this.Controls.Add(this.label_NewGateWidth);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.numericUpDown_GateSO);
            this.Controls.Add(this.numericUpDown_GateWidth);
            this.Controls.Add(this.label_GateSONow);
            this.Controls.Add(this.panel22);
            this.Name = "Observation_RangeGatePanel";
            this.Size = new System.Drawing.Size(780, 155);
            this.Load += new System.EventHandler(this.Observation_RangeGatePanel_Load);
            this.panel22.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GateWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_GateSO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel22;
        private System.Windows.Forms.Label label_RGG;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label_NewGateWidth;
        private System.Windows.Forms.NumericUpDown numericUpDown_GateWidth;
        private System.Windows.Forms.NumericUpDown numericUpDown_GateSO;
        private System.Windows.Forms.Label label_NewGateSO;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label_GateSONow;
        private System.Windows.Forms.Label label_GateWidthNow;
        private System.Windows.Forms.Button btn_SetGateControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_LebelSet;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_rggMode;
    }
}
