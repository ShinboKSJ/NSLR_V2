namespace NSLR_ObservationControl.OAS
{
    partial class OAS_Controller
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
            this.uC_CommandGenerator1 = new NSLR_ObservationControl.OAS.UC_CommandGenerator();
            this.SuspendLayout();
            // 
            // uC_CommandGenerator1
            // 
            this.uC_CommandGenerator1.Location = new System.Drawing.Point(27, 39);
            this.uC_CommandGenerator1.Name = "uC_CommandGenerator1";
            this.uC_CommandGenerator1.Size = new System.Drawing.Size(744, 743);
            this.uC_CommandGenerator1.TabIndex = 0;
            // 
            // OAS_Controller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1723, 1304);
            this.Controls.Add(this.uC_CommandGenerator1);
            this.Name = "OAS_Controller";
            this.Text = "OAS_Controller";
            this.Load += new System.EventHandler(this.OAS_Controller_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UC_CommandGenerator uC_CommandGenerator1;
    }
}