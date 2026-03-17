namespace NSLR_ObservationControl.Module
{
    partial class StarCAM_FullScreen
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
            this.pictureBox_FullStarCam = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_FullStarCam)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_FullStarCam
            // 
            this.pictureBox_FullStarCam.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox_FullStarCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_FullStarCam.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_FullStarCam.Name = "pictureBox_FullStarCam";
            this.pictureBox_FullStarCam.Size = new System.Drawing.Size(3444, 1421);
            this.pictureBox_FullStarCam.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_FullStarCam.TabIndex = 0;
            this.pictureBox_FullStarCam.TabStop = false;
            this.pictureBox_FullStarCam.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_FullStarCam_Paint);
            this.pictureBox_FullStarCam.DoubleClick += new System.EventHandler(this.pictureBox_FullStarCam_DoubleClick);
            // 
            // StarCAM_FullScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Beige;
            this.ClientSize = new System.Drawing.Size(3444, 1421);
            this.Controls.Add(this.pictureBox_FullStarCam);
            this.Name = "StarCAM_FullScreen";
            this.Text = "FullScreenStarCam";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_FullStarCam)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_FullStarCam;
    }
}