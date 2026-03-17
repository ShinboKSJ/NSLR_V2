namespace NSLR_ObservationControl
{
    partial class AddTLE_Form
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
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.name_label = new System.Windows.Forms.Label();
            this.line1_label = new System.Windows.Forms.Label();
            this.line2_label = new System.Windows.Forms.Label();
            this.name_textBox = new System.Windows.Forms.TextBox();
            this.line1_textBox = new System.Windows.Forms.TextBox();
            this.line2_textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ok_button
            // 
            this.ok_button.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ok_button.Location = new System.Drawing.Point(290, 165);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(113, 45);
            this.ok_button.TabIndex = 0;
            this.ok_button.Text = "적용";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cancel_button.Location = new System.Drawing.Point(427, 165);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(113, 45);
            this.cancel_button.TabIndex = 0;
            this.cancel_button.Text = "취소";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // name_label
            // 
            this.name_label.AutoSize = true;
            this.name_label.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.name_label.Location = new System.Drawing.Point(31, 26);
            this.name_label.Name = "name_label";
            this.name_label.Size = new System.Drawing.Size(88, 25);
            this.name_label.TabIndex = 1;
            this.name_label.Text = "위성명 : ";
            // 
            // line1_label
            // 
            this.line1_label.AutoSize = true;
            this.line1_label.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.line1_label.Location = new System.Drawing.Point(25, 68);
            this.line1_label.Name = "line1_label";
            this.line1_label.Size = new System.Drawing.Size(94, 25);
            this.line1_label.TabIndex = 1;
            this.line1_label.Text = "TLE (1) : ";
            // 
            // line2_label
            // 
            this.line2_label.AutoSize = true;
            this.line2_label.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.line2_label.Location = new System.Drawing.Point(25, 112);
            this.line2_label.Name = "line2_label";
            this.line2_label.Size = new System.Drawing.Size(94, 25);
            this.line2_label.TabIndex = 1;
            this.line2_label.Text = "TLE (2) : ";
            // 
            // name_textBox
            // 
            this.name_textBox.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.name_textBox.Location = new System.Drawing.Point(125, 26);
            this.name_textBox.Name = "name_textBox";
            this.name_textBox.Size = new System.Drawing.Size(675, 29);
            this.name_textBox.TabIndex = 2;
            // 
            // line1_textBox
            // 
            this.line1_textBox.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.line1_textBox.Location = new System.Drawing.Point(125, 68);
            this.line1_textBox.Name = "line1_textBox";
            this.line1_textBox.Size = new System.Drawing.Size(675, 29);
            this.line1_textBox.TabIndex = 2;
            // 
            // line2_textBox
            // 
            this.line2_textBox.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.line2_textBox.Location = new System.Drawing.Point(125, 112);
            this.line2_textBox.Name = "line2_textBox";
            this.line2_textBox.Size = new System.Drawing.Size(675, 29);
            this.line2_textBox.TabIndex = 2;
            // 
            // AddTLE_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.ClientSize = new System.Drawing.Size(830, 234);
            this.Controls.Add(this.line2_textBox);
            this.Controls.Add(this.line1_textBox);
            this.Controls.Add(this.name_textBox);
            this.Controls.Add(this.line2_label);
            this.Controls.Add(this.line1_label);
            this.Controls.Add(this.name_label);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ok_button);
            this.Name = "AddTLE_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TLE 추가";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Label name_label;
        private System.Windows.Forms.Label line1_label;
        private System.Windows.Forms.Label line2_label;
        private System.Windows.Forms.TextBox name_textBox;
        private System.Windows.Forms.TextBox line1_textBox;
        private System.Windows.Forms.TextBox line2_textBox;
    }
}