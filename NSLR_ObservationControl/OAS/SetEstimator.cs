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
    public partial class SetEstimator : Form
    {
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetObservatoryN(IntPtr obsClass);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern StringBuilder GetObservatory(IntPtr estClass);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern StringBuilder GetObservatoryList(IntPtr obsClass, int n);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetObservatory(IntPtr estClass, StringBuilder obs);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetObservable(IntPtr estClass, bool[] idxVec);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetObservable(IntPtr estClass, bool[] idxVec);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetUncertainty(IntPtr paramClass, double[] cov);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetUncertainty(IntPtr paramClass, double[] cov);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetMaximumIteration(IntPtr estClass);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetMaximumIteration(IntPtr estClass, int iterMax);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetTolerance(IntPtr estClass);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetTolerance(IntPtr estClass, double tol);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetDataEditor(IntPtr estClass);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetDataEditor(IntPtr estClass, bool flag);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetEditorThreshold(IntPtr estClass);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetEditorThreshold(IntPtr estClass, double thr);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetEstimatorTime(IntPtr estClass, StringBuilder type, double[] dateVec);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetEstimatorTime(IntPtr estClass, StringBuilder type, double[] dateVec);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetParameterEpochTime(IntPtr paramClass, double[] dateVec);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetParameterEpochTime(IntPtr paramClass, double[] dateVec);
        public SetEstimator()
        {
            InitializeComponent();
        }

        private void SetEstimator_Load(object sender, EventArgs e)
        {
            StringBuilder obsName = GetObservatory(Global.estClass);
            int obsN = GetObservatoryN(Global.obsClass);
            int idx = 9999;
            for (int n = 0; n < obsN; n++)
            {
                StringBuilder obsName_tmp = GetObservatoryList(Global.obsClass, n);
                Console.WriteLine($"SetEstimator_Load().....{obsName_tmp.ToString()}");//JcK 
                obsSite_comboBox.Items.Add(obsName_tmp.ToString());
                if (StringComparer.OrdinalIgnoreCase.Equals(obsName.ToString(), obsName_tmp.ToString())) 
                { 
                    idx = n;
                    Console.WriteLine($"SetEstimator_Load().....[{idx}]");//JcK 
                }
            }
            if (idx != 9999)
                obsSite_comboBox.SelectedIndex = idx;

            bool[] obsIdx = new bool[2];
            GetObservable(Global.estClass, obsIdx);
            range_checkBox.Checked = obsIdx[0];
            azel_checkBox.Checked = obsIdx[1];

            double[] epochTime = new double[6];
            GetParameterEpochTime(Global.paramClass, epochTime);
            epochTime1_textBox.Text = Convert.ToString(epochTime[0]);
            epochTime2_textBox.Text = Convert.ToString(epochTime[1]);
            epochTime3_textBox.Text = Convert.ToString(epochTime[2]);
            epochTime4_textBox.Text = Convert.ToString(epochTime[3]);
            epochTime5_textBox.Text = Convert.ToString(epochTime[4]);
            epochTime6_textBox.Text = Convert.ToString(epochTime[5]);

            StringBuilder epochType = new StringBuilder("start");
            double[] startTime = new double[6];
            GetEstimatorTime(Global.estClass, epochType, startTime);
            startTime1_textBox.Text = Convert.ToString(startTime[0]);
            startTime2_textBox.Text = Convert.ToString(startTime[1]);
            startTime3_textBox.Text = Convert.ToString(startTime[2]);
            startTime4_textBox.Text = Convert.ToString(startTime[3]);
            startTime5_textBox.Text = Convert.ToString(startTime[4]);
            startTime6_textBox.Text = Convert.ToString(startTime[5]);

            epochType = new StringBuilder("end");
            double[] finalTime = new double[6];
            GetEstimatorTime(Global.estClass, epochType, finalTime);
            finalTime1_textBox.Text = Convert.ToString(finalTime[0]);
            finalTime2_textBox.Text = Convert.ToString(finalTime[1]);
            finalTime3_textBox.Text = Convert.ToString(finalTime[2]);
            finalTime4_textBox.Text = Convert.ToString(finalTime[3]);
            finalTime5_textBox.Text = Convert.ToString(finalTime[4]);
            finalTime6_textBox.Text = Convert.ToString(finalTime[5]);

            double[] unc = new double[2];
            GetUncertainty(Global.paramClass, unc);
            uncPos_textBox.Text = Convert.ToString(unc[0]);
            uncVel_textBox.Text = Convert.ToString(unc[1]);

            int maxIter = GetMaximumIteration(Global.estClass);
            maxIter_textBox.Text = Convert.ToString(maxIter);

            double tol = GetTolerance(Global.estClass);
            tol_textBox.Text = Convert.ToString(tol);

            double thr = GetEditorThreshold(Global.estClass);
            editor_checkBox.Checked = GetDataEditor(Global.estClass);
            if (editor_checkBox.Checked)
            {
                thr_textBox.ReadOnly = false;
                thr_textBox.Text = Convert.ToString(thr);
            }
            else
            {
                thr_textBox.ReadOnly = true;
            }

        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            StringBuilder obsName = new StringBuilder(obsSite_comboBox.SelectedItem.ToString());
            SetObservatory(Global.estClass, obsName);

            bool[] obsIdx = new bool[2];
            obsIdx[0] = range_checkBox.Checked;
            obsIdx[1] = azel_checkBox.Checked;
            SetObservable(Global.estClass, obsIdx);

            double[] epochTime = new double[6];
            epochTime[0] = double.Parse(epochTime1_textBox.Text);
            epochTime[1] = double.Parse(epochTime2_textBox.Text);
            epochTime[2] = double.Parse(epochTime3_textBox.Text);
            epochTime[3] = double.Parse(epochTime4_textBox.Text);
            epochTime[4] = double.Parse(epochTime5_textBox.Text);
            epochTime[5] = double.Parse(epochTime6_textBox.Text);
            SetParameterEpochTime(Global.paramClass, epochTime);

            StringBuilder epochType = new StringBuilder("start");
            double[] startTime = new double[6];
            startTime[0] = double.Parse(startTime1_textBox.Text);
            startTime[1] = double.Parse(startTime2_textBox.Text);
            startTime[2] = double.Parse(startTime3_textBox.Text);
            startTime[3] = double.Parse(startTime4_textBox.Text);
            startTime[4] = double.Parse(startTime5_textBox.Text);
            startTime[5] = double.Parse(startTime6_textBox.Text);
            SetEstimatorTime(Global.estClass, epochType, startTime);

            epochType = new StringBuilder("end");
            double[] finalTime = new double[6];
            finalTime[0] = double.Parse(finalTime1_textBox.Text);
            finalTime[1] = double.Parse(finalTime2_textBox.Text);
            finalTime[2] = double.Parse(finalTime3_textBox.Text);
            finalTime[3] = double.Parse(finalTime4_textBox.Text);
            finalTime[4] = double.Parse(finalTime5_textBox.Text);
            finalTime[5] = double.Parse(finalTime6_textBox.Text);
            SetEstimatorTime(Global.estClass, epochType, finalTime);

            double[] unc = new double[2];
            unc[0] = double.Parse(uncPos_textBox.Text);
            unc[1] = double.Parse(uncVel_textBox.Text);
            SetUncertainty(Global.paramClass, unc);

            SetMaximumIteration(Global.estClass, Convert.ToInt16(maxIter_textBox.Text));

            SetTolerance(Global.estClass, double.Parse(tol_textBox.Text));

            SetDataEditor(Global.estClass, editor_checkBox.Checked);
            if (editor_checkBox.Checked)
            {
                SetEditorThreshold(Global.estClass, double.Parse(thr_textBox.Text));
            }

            this.Hide();

            Console.WriteLine("SetEstimator ...................... OK");
        }

        private void SetEstimator_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
