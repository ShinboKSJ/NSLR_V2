using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl.OAS
{
    public partial class SetLaser : Form
    {
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern StringBuilder GetSiteName(IntPtr obsSite);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetSiteName(IntPtr obsSite, StringBuilder siteName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetSiteWavelength(IntPtr obsSite);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetSiteWaveLength(IntPtr obsSite, double waveLength);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetSiteLocation(IntPtr obsSite, double[] loc);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetSiteLocation(IntPtr obsSite, double[] loc);
        public SetLaser()
        {
            InitializeComponent();
        }

        private void SetLaser_Load(object sender, EventArgs e)
        {
            StringBuilder siteName = GetSiteName(Global.laserSite);
            double waveLength = GetSiteWavelength(Global.laserSite);
            double[] siteLoc = new double[3];
            GetSiteLocation(Global.laserSite, siteLoc);

            name_textBox.Text = siteName.ToString();
            wl_textBox.Text = Convert.ToString(waveLength);
            lon_textBox.Text = Convert.ToString(siteLoc[0]);
            lat_textBox.Text = Convert.ToString(siteLoc[1]);
            alt_textBox.Text = Convert.ToString(siteLoc[2]);
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            double[] siteLoc = new double[3];

            StringBuilder siteName = new StringBuilder(name_textBox.Text);
            double waveLength = Double.Parse(wl_textBox.Text);

            siteLoc[0] = Double.Parse(lon_textBox.Text);
            siteLoc[1] = Double.Parse(lat_textBox.Text);
            siteLoc[2] = Double.Parse(alt_textBox.Text);

            SetSiteName(Global.laserSite, siteName);
            SetSiteWaveLength(Global.laserSite, waveLength);
            SetSiteLocation(Global.laserSite, siteLoc);

            this.Hide();

            Console.WriteLine("SetLaser...................... OK");
        }
    }
}
