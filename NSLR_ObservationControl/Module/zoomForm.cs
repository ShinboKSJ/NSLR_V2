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
    public partial class zoomForm : Form
    {
        private PictureBox pictureBox;
        public zoomForm()
        {
            InitializeComponent();
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Normal
            };
            this.Controls.Add(pictureBox);
            this.Text = "Zoom View";
            this.ClientSize = new Size(208, 208);
        }
        public void UpdateImage(Bitmap bitmap)
        {
            if (pictureBox.Image != null)
                pictureBox.Image.Dispose();

            pictureBox.Image = (Bitmap)bitmap.Clone();
        }

/*        public void UpdateImage(byte[] rawBytes, int width, int height)
        {
            if (rawBytes == null || rawBytes.Length != width * height)
                return;

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            ColorPalette palette = bmp.Palette;
            for (int i = 0; i < 256; i++)
                palette.Entries[i] = Color.FromArgb(i, i, i);
            bmp.Palette = palette;

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            Marshal.Copy(rawBytes, 0, bmpData.Scan0, rawBytes.Length);

            bmp.UnlockBits(bmpData);

            pictureBox.Image?.Dispose();
            pictureBox.Image = new Bitmap(bmp, width * 4, height * 4);  
            bmp.Dispose();
        }*/
    }
}
