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
    public partial class ResidualPlotViewer : Form
    {
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetResidualRMS(IntPtr resStr, double[] rms);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetResidualDataInfo(IntPtr resStr, int[] info);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetResidualTime(IntPtr resStr, double[] timeTags);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetResidualData(IntPtr resStr, int n, double[] data);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetEditedResiduaIdx(IntPtr resStr, double[] editedIdx);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetEpochTime(IntPtr sat, double[] epochDate);
        public ResidualPlotViewer()
        {
            InitializeComponent();
        }

        private void ResidualPlotViewer_Load(object sender, EventArgs e)
        {
            int[] info = new int[2];
            GetResidualDataInfo(Global.residual, info);
            double[] timeTag = new double[info[0]];
            GetResidualTime(Global.residual, timeTag);
            //double[] epochDate = new double[6];
            //double mjd = GetEpochTime(Global.sat, epochDate);
            double mjd = timeTag[0];
            for (int i = 0; i < info[0]; i++)
            {
                timeTag[i] = 86400 * (timeTag[i] - mjd);
            }
            for (int i = 0; i < info[1]; i++)
            {
                double[] data = new double[info[0]];
                GetResidualData(Global.residual, i, data);
                residual_chart.Series[i].Points.DataBindXY(timeTag, data);
            }
        }
    }
}
