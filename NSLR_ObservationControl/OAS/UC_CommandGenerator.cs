using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl.OAS
{
    public partial class UC_CommandGenerator : UserControl
    {
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateTLE();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetTLEData(IntPtr tleClass, StringBuilder line1, StringBuilder line2, double[] epochTime);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void PropagateTLE(IntPtr tleClass, double[] elapsedSecs, int N, double[,] ephemeris);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GenerateCommand_TLE(IntPtr tleClass, double startMJD, double finalMJD, double stepSize, StringBuilder fileName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ReadCPF(StringBuilder cpfFileName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double LoadCPFInfo(IntPtr cpfData, StringBuilder targetName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GenerateCommand_CPF(IntPtr cpfData, double startMJD, double finalMJD, double stepSize, StringBuilder _fileName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ConvertMJD2Greg(IntPtr timeSystem, double mjd, double[] greg);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ConvertGreg2MJD(IntPtr timeSystem, double[] greg);

        IntPtr tleClass;
        public string fileName = "./Resources/activeTLE_230830.TLE";
        public string[] tleData;

        string[] cpfFiles;
        IntPtr cpfData;

        public UC_CommandGenerator()
        {
            InitializeComponent();
        }

        private void UC_CommandGenerator_Load(object sender, EventArgs e)
        {
            
            // TLE
            tleClass = CreateTLE();
            tleData = System.IO.File.ReadAllLines(fileName);
            int idx = 0;
            while (idx < tleData.Length)
            {
                listBox_TLE.Items.Add(tleData[idx]);
                idx = idx + 3;
            }
            stepSize_TLE_textBox.Text = "60";

            // CPF
            cpfFiles = Directory.GetFiles(@"./Resources/Ephemeris/CPF", "*.*");
            foreach (var cpfFile in cpfFiles)
            {
                string fileName = Path.GetFileName(cpfFile);
                listBox_CPF.Items.Add(fileName);
            }
            stepSize_CPF_textBox.Text = "60";
            
        }

        private void listBox_TLE_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = listBox_TLE.SelectedIndex;
            StringBuilder line1 = new StringBuilder(tleData[idx * 3 + 1]);
            StringBuilder line2 = new StringBuilder(tleData[idx * 3 + 2]);
            double[] epochTime = new double[6];
            double noradID = GetTLEData(tleClass, line1, line2, epochTime);

            NORAD_textBox.Text = Convert.ToString(noradID);

            epochTime1_TLE_textBox.Text = Convert.ToString(epochTime[0]);
            epochTime2_TLE_textBox.Text = Convert.ToString(epochTime[1]);
            epochTime3_TLE_textBox.Text = Convert.ToString(epochTime[2]);
            epochTime4_TLE_textBox.Text = Convert.ToString(epochTime[3]);
            epochTime5_TLE_textBox.Text = Convert.ToString(epochTime[4]);
            epochTime6_TLE_textBox.Text = Convert.ToString(epochTime[5]);

            startTime1_TLE_textBox.Text = Convert.ToString(epochTime[0]);
            startTime2_TLE_textBox.Text = Convert.ToString(epochTime[1]);
            startTime3_TLE_textBox.Text = Convert.ToString(epochTime[2]);
            startTime4_TLE_textBox.Text = Convert.ToString(epochTime[3]);
            startTime5_TLE_textBox.Text = Convert.ToString(epochTime[4]);
            startTime6_TLE_textBox.Text = Convert.ToString(epochTime[5]);

            double mjd = ConvertGreg2MJD(Global.timeSys, epochTime);
            double[] finalTime = new double[6];
            ConvertMJD2Greg(Global.timeSys, mjd + 1, finalTime);
            finalTime1_TLE_textBox.Text = Convert.ToString(finalTime[0]);
            finalTime2_TLE_textBox.Text = Convert.ToString(finalTime[1]);
            finalTime3_TLE_textBox.Text = Convert.ToString(finalTime[2]);
            finalTime4_TLE_textBox.Text = Convert.ToString(finalTime[3]);
            finalTime5_TLE_textBox.Text = Convert.ToString(finalTime[4]);
            finalTime6_TLE_textBox.Text = Convert.ToString(finalTime[5]);
        }

        private void OK_TLE_button_Click(object sender, EventArgs e)
        {
            if (listBox_TLE.SelectedIndex < 0)
            {
                MessageBox.Show("Select target.", "NSLR-OAS", MessageBoxButtons.OK);
                return;
            }

            double[] epochTime = new double[6];
            double[] startTime = new double[6];
            double[] finalTime = new double[6];

            epochTime[0] = double.Parse(epochTime1_TLE_textBox.Text);
            epochTime[1] = double.Parse(epochTime2_TLE_textBox.Text);
            epochTime[2] = double.Parse(epochTime3_TLE_textBox.Text);
            epochTime[3] = double.Parse(epochTime4_TLE_textBox.Text);
            epochTime[4] = double.Parse(epochTime5_TLE_textBox.Text);
            epochTime[5] = double.Parse(epochTime6_TLE_textBox.Text);
            double epochMJD = ConvertGreg2MJD(Global.timeSys, epochTime);

            startTime[0] = double.Parse(startTime1_TLE_textBox.Text);
            startTime[1] = double.Parse(startTime2_TLE_textBox.Text);
            startTime[2] = double.Parse(startTime3_TLE_textBox.Text);
            startTime[3] = double.Parse(startTime4_TLE_textBox.Text);
            startTime[4] = double.Parse(startTime5_TLE_textBox.Text);
            startTime[5] = double.Parse(startTime6_TLE_textBox.Text);
            double startMJD = ConvertGreg2MJD(Global.timeSys, startTime);

            finalTime[0] = double.Parse(finalTime1_TLE_textBox.Text);
            finalTime[1] = double.Parse(finalTime2_TLE_textBox.Text);
            finalTime[2] = double.Parse(finalTime3_TLE_textBox.Text);
            finalTime[3] = double.Parse(finalTime4_TLE_textBox.Text);
            finalTime[4] = double.Parse(finalTime5_TLE_textBox.Text);
            finalTime[5] = double.Parse(finalTime6_TLE_textBox.Text);
            double finalMJD = ConvertGreg2MJD(Global.timeSys, finalTime);

            double stepSize = double.Parse(stepSize_TLE_textBox.Text);

            string targetName = listBox_TLE.SelectedItem.ToString().Trim();
            string fileTime = DateTime.Now.ToString("yyMMdd+hhmm");
            StringBuilder fileName = new StringBuilder(targetName + "_RADEC_" + fileTime + ".txt");

            GenerateCommand_TLE(tleClass, startMJD, finalMJD, stepSize, fileName);

            MessageBox.Show("Command file is generated.", "NSLR-OAS", MessageBoxButtons.OK);
        }

        private void listBox_CPF_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = listBox_CPF.SelectedIndex;
            StringBuilder cpfFile = new StringBuilder(cpfFiles[idx]);
            cpfData = ReadCPF(cpfFile);
            StringBuilder targetName = new StringBuilder();
            double epochMJD = LoadCPFInfo(cpfData, targetName);
            targetName_textBox.Text = targetName.ToString();
            double[] epochTime = new double[6];
            ConvertMJD2Greg(Global.timeSys, epochMJD, epochTime);

            epochTime1_CPF_textBox.Text = Convert.ToString(epochTime[0]);
            epochTime2_CPF_textBox.Text = Convert.ToString(epochTime[1]);
            epochTime3_CPF_textBox.Text = Convert.ToString(epochTime[2]);
            epochTime4_CPF_textBox.Text = Convert.ToString(epochTime[3]);
            epochTime5_CPF_textBox.Text = Convert.ToString(epochTime[4]);
            epochTime6_CPF_textBox.Text = Convert.ToString(epochTime[5]);

            startTime1_CPF_textBox.Text = Convert.ToString(epochTime[0]);
            startTime2_CPF_textBox.Text = Convert.ToString(epochTime[1]);
            startTime3_CPF_textBox.Text = Convert.ToString(epochTime[2]);
            startTime4_CPF_textBox.Text = Convert.ToString(epochTime[3]);
            startTime5_CPF_textBox.Text = Convert.ToString(epochTime[4]);
            startTime6_CPF_textBox.Text = Convert.ToString(epochTime[5]);

            double[] finalTime = new double[6];
            ConvertMJD2Greg(Global.timeSys, epochMJD + 1, finalTime);
            finalTime1_CPF_textBox.Text = Convert.ToString(finalTime[0]);
            finalTime2_CPF_textBox.Text = Convert.ToString(finalTime[1]);
            finalTime3_CPF_textBox.Text = Convert.ToString(finalTime[2]);
            finalTime4_CPF_textBox.Text = Convert.ToString(finalTime[3]);
            finalTime5_CPF_textBox.Text = Convert.ToString(finalTime[4]);
            finalTime6_CPF_textBox.Text = Convert.ToString(finalTime[5]);
        }

        private void OK_CPF_button_Click(object sender, EventArgs e)
        {
            if (listBox_CPF.SelectedIndex < 0)
            {
                MessageBox.Show("Select CPF file.", "NSLR-OAS", MessageBoxButtons.OK);
                return;
            }

            double[] epochTime = new double[6];
            double[] startTime = new double[6];
            double[] finalTime = new double[6];

            epochTime[0] = double.Parse(epochTime1_CPF_textBox.Text);
            epochTime[1] = double.Parse(epochTime2_CPF_textBox.Text);
            epochTime[2] = double.Parse(epochTime3_CPF_textBox.Text);
            epochTime[3] = double.Parse(epochTime4_CPF_textBox.Text);
            epochTime[4] = double.Parse(epochTime5_CPF_textBox.Text);
            epochTime[5] = double.Parse(epochTime6_CPF_textBox.Text);
            double epochMJD = ConvertGreg2MJD(Global.timeSys, epochTime);

            startTime[0] = double.Parse(startTime1_CPF_textBox.Text);
            startTime[1] = double.Parse(startTime2_CPF_textBox.Text);
            startTime[2] = double.Parse(startTime3_CPF_textBox.Text);
            startTime[3] = double.Parse(startTime4_CPF_textBox.Text);
            startTime[4] = double.Parse(startTime5_CPF_textBox.Text);
            startTime[5] = double.Parse(startTime6_CPF_textBox.Text);
            double startMJD = ConvertGreg2MJD(Global.timeSys, startTime);

            finalTime[0] = double.Parse(finalTime1_CPF_textBox.Text);
            finalTime[1] = double.Parse(finalTime2_CPF_textBox.Text);
            finalTime[2] = double.Parse(finalTime3_CPF_textBox.Text);
            finalTime[3] = double.Parse(finalTime4_CPF_textBox.Text);
            finalTime[4] = double.Parse(finalTime5_CPF_textBox.Text);
            finalTime[5] = double.Parse(finalTime6_CPF_textBox.Text);
            double finalMJD = ConvertGreg2MJD(Global.timeSys, finalTime);

            double stepSize = double.Parse(stepSize_CPF_textBox.Text);

            string targetName = targetName_textBox.Text;
            string fileTime = DateTime.Now.ToString("yyMMdd+hhmm");
            StringBuilder fileName = new StringBuilder(targetName + "_RADEC_" + fileTime + ".txt");

            GenerateCommand_CPF(cpfData, startMJD, finalMJD, stepSize, fileName);

            MessageBox.Show("Command file is generated.", "NSLR-OAS", MessageBoxButtons.OK);
        }
    }
}
