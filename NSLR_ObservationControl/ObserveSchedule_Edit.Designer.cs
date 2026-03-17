namespace NSLR_ObservationControl
{
    partial class ObserveSchedule_Edit
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ok_btn = new System.Windows.Forms.Button();
            this.cancel_btn = new System.Windows.Forms.Button();
            this.startTime_combo = new System.Windows.Forms.ComboBox();
            this.endTime_combo = new System.Windows.Forms.ComboBox();
            this.satelliteName_txt = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(29, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "위성 : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(29, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "추적 시작 시간 : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(29, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "추적 종료 시간 : ";
            // 
            // ok_btn
            // 
            this.ok_btn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ok_btn.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ok_btn.Location = new System.Drawing.Point(109, 173);
            this.ok_btn.Name = "ok_btn";
            this.ok_btn.Size = new System.Drawing.Size(87, 38);
            this.ok_btn.TabIndex = 1;
            this.ok_btn.Text = "등 록";
            this.ok_btn.UseVisualStyleBackColor = true;
            this.ok_btn.Click += new System.EventHandler(this.ok_btn_Click);
            // 
            // cancel_btn
            // 
            this.cancel_btn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cancel_btn.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cancel_btn.Location = new System.Drawing.Point(215, 173);
            this.cancel_btn.Name = "cancel_btn";
            this.cancel_btn.Size = new System.Drawing.Size(87, 38);
            this.cancel_btn.TabIndex = 1;
            this.cancel_btn.Text = "취 소";
            this.cancel_btn.UseVisualStyleBackColor = true;
            this.cancel_btn.Click += new System.EventHandler(this.cancel_btn_Click);
            // 
            // startTime_combo
            // 
            this.startTime_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.startTime_combo.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.startTime_combo.FormattingEnabled = true;
            this.startTime_combo.Location = new System.Drawing.Point(194, 67);
            this.startTime_combo.Name = "startTime_combo";
            this.startTime_combo.Size = new System.Drawing.Size(182, 29);
            this.startTime_combo.TabIndex = 2;
            this.startTime_combo.SelectedIndexChanged += new System.EventHandler(this.startTime_combo_SelectedIndexChanged);
            // 
            // endTime_combo
            // 
            this.endTime_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.endTime_combo.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.endTime_combo.FormattingEnabled = true;
            this.endTime_combo.Location = new System.Drawing.Point(194, 109);
            this.endTime_combo.Name = "endTime_combo";
            this.endTime_combo.Size = new System.Drawing.Size(182, 29);
            this.endTime_combo.TabIndex = 2;
            this.endTime_combo.SelectedIndexChanged += new System.EventHandler(this.endTime_combo_SelectedIndexChanged);
            // 
            // satelliteName_txt
            // 
            this.satelliteName_txt.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.satelliteName_txt.Location = new System.Drawing.Point(104, 30);
            this.satelliteName_txt.Name = "satelliteName_txt";
            this.satelliteName_txt.Size = new System.Drawing.Size(272, 25);
            this.satelliteName_txt.TabIndex = 0;
            this.satelliteName_txt.Text = "-";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(41, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(169, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "* 시작 시간 선택 시 활성화";
            // 
            // ObserveSchedule_Edit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Violet;
            this.ClientSize = new System.Drawing.Size(406, 234);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.endTime_combo);
            this.Controls.Add(this.startTime_combo);
            this.Controls.Add(this.cancel_btn);
            this.Controls.Add(this.ok_btn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.satelliteName_txt);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ObserveSchedule_Edit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "등 록";
            this.Load += new System.EventHandler(this.ObserveSchedule_Edit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ok_btn;
        private System.Windows.Forms.Button cancel_btn;
        private System.Windows.Forms.ComboBox startTime_combo;
        private System.Windows.Forms.ComboBox endTime_combo;
        private System.Windows.Forms.Label satelliteName_txt;
        private System.Windows.Forms.Label label5;
    }
}