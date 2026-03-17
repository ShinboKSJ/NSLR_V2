using NSLR_ObservationControl.Subsystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl.Module
{
    public partial class StarCAM_FullScreen : Form
    {

        private static StarCAM_FullScreen _instance;
        Image picture;
        public Image Picture
        {
            get
            {
                return picture;
            }
            set
            {
                picture = value;
                pictureBox_FullStarCam.Image = picture;
            }
        }
        
        public static StarCAM_FullScreen instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StarCAM_FullScreen();
                }
                return _instance;
            }
        }
        
        public StarCAM_FullScreen()
        {
            InitializeComponent();
        }


        ~ StarCAM_FullScreen()
        {
            pictureBox_FullStarCam.Image = null;
        }

        private void pictureBox_FullStarCam_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int centerX = pictureBox_FullStarCam.Width / 2;
            int centerY = pictureBox_FullStarCam.Height / 2;
            int markerSize = 10;

            g.DrawLine(Pens.Yellow, centerX, centerY - markerSize / 2, centerX, centerY + markerSize / 2);
            g.DrawLine(Pens.Yellow, centerX - markerSize / 2, centerY, centerX + markerSize / 2, centerY);
        }

        private void pictureBox_FullStarCam_DoubleClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
