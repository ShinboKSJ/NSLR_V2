namespace NSLR_ObservationControl.OAS
{
    partial class SetLaser
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
            this.lon_textBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.alt_textBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.wl_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.name_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.lat_textBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lon_textBox
            // 
            this.lon_textBox.Location = new System.Drawing.Point(124, 23);
            this.lon_textBox.Name = "lon_textBox";
            this.lon_textBox.Size = new System.Drawing.Size(55, 21);
            this.lon_textBox.TabIndex = 38;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(203, 12);
            this.label5.TabIndex = 36;
            this.label5.Text = "Altitude                                   km";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(205, 12);
            this.label4.TabIndex = 37;
            this.label4.Text = "Longitude                               deg";
            // 
            // alt_textBox
            // 
            this.alt_textBox.Location = new System.Drawing.Point(124, 75);
            this.alt_textBox.Name = "alt_textBox";
            this.alt_textBox.Size = new System.Drawing.Size(55, 21);
            this.alt_textBox.TabIndex = 33;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lat_textBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lon_textBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.alt_textBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 110);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Location";
            // 
            // wl_textBox
            // 
            this.wl_textBox.Location = new System.Drawing.Point(136, 50);
            this.wl_textBox.Name = "wl_textBox";
            this.wl_textBox.Size = new System.Drawing.Size(55, 21);
            this.wl_textBox.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 12);
            this.label1.TabIndex = 41;
            this.label1.Text = "Wave Length                           nm";
            // 
            // name_textBox
            // 
            this.name_textBox.Location = new System.Drawing.Point(136, 20);
            this.name_textBox.Name = "name_textBox";
            this.name_textBox.Size = new System.Drawing.Size(98, 21);
            this.name_textBox.TabIndex = 42;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 12);
            this.label2.TabIndex = 43;
            this.label2.Text = "Name";
            // 
            // ok_button
            // 
            this.ok_button.Location = new System.Drawing.Point(64, 201);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(75, 23);
            this.ok_button.TabIndex = 44;
            this.ok_button.Text = "OK";
            this.ok_button.UseVisualStyleBackColor = true;
            this.ok_button.Click += new System.EventHandler(this.ok_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(145, 201);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(75, 23);
            this.cancel_button.TabIndex = 45;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            // 
            // lat_textBox
            // 
            this.lat_textBox.Location = new System.Drawing.Point(124, 50);
            this.lat_textBox.Name = "lat_textBox";
            this.lat_textBox.Size = new System.Drawing.Size(55, 21);
            this.lat_textBox.TabIndex = 39;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(206, 12);
            this.label3.TabIndex = 40;
            this.label3.Text = "Latitude                                  deg";
            // 
            // SetLaser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 235);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.ok_button);
            this.Controls.Add(this.name_textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.wl_textBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Name = "SetLaser";
            this.Text = "SetLaser";
            this.Load += new System.EventHandler(this.SetLaser_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox lon_textBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox alt_textBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox wl_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox name_textBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.TextBox lat_textBox;
        private System.Windows.Forms.Label label3;
    }
}