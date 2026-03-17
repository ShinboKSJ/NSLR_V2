namespace NSLR_ObservationControl
{
    partial class UserManagement_Password
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
            this.close_btn = new System.Windows.Forms.Button();
            this.execution_btn = new System.Windows.Forms.Button();
            this.checkPassword_txtBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label_Title = new System.Windows.Forms.Label();
            this.newPassword_txtBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // close_btn
            // 
            this.close_btn.AutoSize = true;
            this.close_btn.BackColor = System.Drawing.Color.Silver;
            this.close_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.close_btn.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.close_btn.ForeColor = System.Drawing.Color.Black;
            this.close_btn.Location = new System.Drawing.Point(232, 145);
            this.close_btn.Name = "close_btn";
            this.close_btn.Size = new System.Drawing.Size(93, 40);
            this.close_btn.TabIndex = 178;
            this.close_btn.Text = "닫  기";
            this.close_btn.UseVisualStyleBackColor = false;
            this.close_btn.Click += new System.EventHandler(this.close_btn_Click);
            // 
            // execution_btn
            // 
            this.execution_btn.AutoSize = true;
            this.execution_btn.BackColor = System.Drawing.Color.Silver;
            this.execution_btn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.execution_btn.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.execution_btn.ForeColor = System.Drawing.Color.Black;
            this.execution_btn.Location = new System.Drawing.Point(110, 145);
            this.execution_btn.Name = "execution_btn";
            this.execution_btn.Size = new System.Drawing.Size(93, 40);
            this.execution_btn.TabIndex = 179;
            this.execution_btn.Text = "확  인";
            this.execution_btn.UseVisualStyleBackColor = false;
            this.execution_btn.Click += new System.EventHandler(this.execution_btn_Click);
            // 
            // checkPassword_txtBox
            // 
            this.checkPassword_txtBox.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkPassword_txtBox.Location = new System.Drawing.Point(187, 87);
            this.checkPassword_txtBox.Name = "checkPassword_txtBox";
            this.checkPassword_txtBox.PasswordChar = '*';
            this.checkPassword_txtBox.Size = new System.Drawing.Size(196, 29);
            this.checkPassword_txtBox.TabIndex = 184;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(37, 87);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(139, 25);
            this.label1.TabIndex = 181;
            this.label1.Text = "비밀번호 확인";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Title
            // 
            this.label_Title.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label_Title.AutoSize = true;
            this.label_Title.BackColor = System.Drawing.Color.Transparent;
            this.label_Title.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Title.ForeColor = System.Drawing.Color.White;
            this.label_Title.Location = new System.Drawing.Point(37, 42);
            this.label_Title.Margin = new System.Windows.Forms.Padding(0);
            this.label_Title.Name = "label_Title";
            this.label_Title.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.label_Title.Size = new System.Drawing.Size(120, 25);
            this.label_Title.TabIndex = 182;
            this.label_Title.Text = "새 비밀번호";
            this.label_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // newPassword_txtBox
            // 
            this.newPassword_txtBox.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.newPassword_txtBox.Location = new System.Drawing.Point(187, 42);
            this.newPassword_txtBox.Name = "newPassword_txtBox";
            this.newPassword_txtBox.PasswordChar = '*';
            this.newPassword_txtBox.Size = new System.Drawing.Size(196, 29);
            this.newPassword_txtBox.TabIndex = 184;
            // 
            // UserManagement_Password
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(434, 226);
            this.Controls.Add(this.newPassword_txtBox);
            this.Controls.Add(this.checkPassword_txtBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_Title);
            this.Controls.Add(this.close_btn);
            this.Controls.Add(this.execution_btn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "UserManagement_Password";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "비밀번호 변경";
            this.Load += new System.EventHandler(this.UserManagement_Password_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close_btn;
        private System.Windows.Forms.Button execution_btn;
        private System.Windows.Forms.TextBox checkPassword_txtBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_Title;
        private System.Windows.Forms.TextBox newPassword_txtBox;
    }
}