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
    public partial class ephemerisGenerator : Form
    {
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenerateEphemeris(IntPtr dynModel, IntPtr sat, double startMJD, double finalMJD, double stepSize, StringBuilder _fileName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetEpochTime(IntPtr sat, double[] epochDate);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ConvertMJD2Greg(IntPtr timeSys, double mjd, double[] greg);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ConvertGreg2MJD(IntPtr timeSystem, double[] greg);

        public ephemerisGenerator()
        {
            InitializeComponent();
        }

        private void ephemerisGenerator_Load(object sender, EventArgs e)
        {
            double[] epochDate = new double[6];
            double mjd = GetEpochTime(Global.sat, epochDate);
            epochTime1_textBox.Text = Convert.ToString(epochDate[0]);
            epochTime2_textBox.Text = Convert.ToString(epochDate[1]);
            epochTime3_textBox.Text = Convert.ToString(epochDate[2]);
            epochTime4_textBox.Text = Convert.ToString(epochDate[3]);
            epochTime5_textBox.Text = Convert.ToString(epochDate[4]);
            epochTime6_textBox.Text = Convert.ToString(epochDate[5]);

            startTime1_textBox.Text = Convert.ToString(epochDate[0]);
            startTime2_textBox.Text = Convert.ToString(epochDate[1]);
            startTime3_textBox.Text = Convert.ToString(epochDate[2]);
            startTime4_textBox.Text = Convert.ToString(epochDate[3]);
            startTime5_textBox.Text = Convert.ToString(epochDate[4]);
            startTime6_textBox.Text = Convert.ToString(epochDate[5]);

            double[] finalDate = new double[6];
            ConvertMJD2Greg(Global.timeSys, mjd + 1.0, finalDate);
            finalTime1_textBox.Text = Convert.ToString(finalDate[0]);
            finalTime2_textBox.Text = Convert.ToString(finalDate[1]);
            finalTime3_textBox.Text = Convert.ToString(finalDate[2]);
            finalTime4_textBox.Text = Convert.ToString(finalDate[3]);
            finalTime5_textBox.Text = Convert.ToString(finalDate[4]);
            finalTime6_textBox.Text = Convert.ToString(finalDate[5]);

            stepSize_textBox.Text = "60";
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            double[] startTime = new double[6];
            double[] finalTime = new double[6];

            startTime[0] = double.Parse(startTime1_textBox.Text);
            startTime[1] = double.Parse(startTime2_textBox.Text);
            startTime[2] = double.Parse(startTime3_textBox.Text);
            startTime[3] = double.Parse(startTime4_textBox.Text);
            startTime[4] = double.Parse(startTime5_textBox.Text);
            startTime[5] = double.Parse(startTime6_textBox.Text);
            double startMJD = ConvertGreg2MJD(Global.timeSys, startTime);

            finalTime[0] = double.Parse(finalTime1_textBox.Text);
            finalTime[1] = double.Parse(finalTime2_textBox.Text);
            finalTime[2] = double.Parse(finalTime3_textBox.Text);
            finalTime[3] = double.Parse(finalTime4_textBox.Text);
            finalTime[4] = double.Parse(finalTime5_textBox.Text);
            finalTime[5] = double.Parse(finalTime6_textBox.Text);
            double finalMJD = ConvertGreg2MJD(Global.timeSys, finalTime);

            double stepSize = double.Parse(stepSize_textBox.Text);

            string fileTime = DateTime.Now.ToString("yyMMdd+hhmm");
            StringBuilder fileName = new StringBuilder("ephemeris_" + fileTime + ".txt");

            GenerateEphemeris(Global.dynModel, Global.sat, startMJD, finalMJD, stepSize, fileName);
            MessageBox.Show("Ephemeris file is generated.", "NSLR-OAS", MessageBoxButtons.OK);
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

