using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NSLR_ObservationControl.OAS
{
    public partial class OAS_Main : Form
    {

        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetMaximumIteration(IntPtr estClass);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr LeastSquaresInitializer(IntPtr estClass, IntPtr paramClass, IntPtr obsClass,
                                        double[] aprioriSolveforVec, double[,] aprioriCov);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double LeastSquaresIterator(IntPtr estClass, IntPtr satClass, IntPtr paramClass, IntPtr obsClass, IntPtr dynModel, IntPtr meaModel,
        double[] aprioriSolveforVec, IntPtr resStr);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ConvergenceTest(IntPtr estClass, double costFnc, double oldCostFnc, bool[] divFlag, int iter);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern StringBuilder GetOrbitalState(IntPtr _sat, double[] stateVec);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetResidualDataInfo(IntPtr resStr, int[] info);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetResidualRMS(IntPtr resStr, double[] _rms);
        [StructLayout(LayoutKind.Sequential)]
        public struct Residual
        {
            [MarshalAs(UnmanagedType.LPStr)]
            string siteName;
            double[] timeTags;
            int[] editedIdx;
            int dimension;
            [MarshalAs(UnmanagedType.LPStr)]
            string[] types;
            [MarshalAs(UnmanagedType.LPStr)]
            string[] units;
            double[,] data;
        }
        private int LeastSquares(object sender, EventArgs e)
        {
            bool[] divFlag = { false };

            int iterMax = GetMaximumIteration(Global.estClass);
            int nSolveforParams = 6;
            double[] aprioriSolveforVec = new double[nSolveforParams];
            double[,] aprioriCov = new double[nSolveforParams, nSolveforParams];
            LeastSquaresInitializer(Global.estClass, Global.paramClass, Global.obsClass, aprioriSolveforVec, aprioriCov);

            double[,] totalSolveforVec = new double[iterMax, nSolveforParams];
            for (int i = 0; i < nSolveforParams; i++)
            {
                totalSolveforVec[0, i] = aprioriSolveforVec[i];
            }

            double oldCostFnc = LeastSquaresIterator(Global.estClass, Global.sat, Global.paramClass, Global.obsClass, Global.dynModel, Global.meaModel,
            aprioriSolveforVec, Global.residual);

            int[] info = new int[2];
            GetResidualDataInfo(Global.residual, info);
            double[] rms = new double[info[0]];
            GetResidualRMS(Global.residual, rms);
            for (int i = 0; i < info[1]; i++)
            {
                switch (i)
                {
                    case 0:
                        rms1_textBox.Text = rms[0].ToString("F5");
                        rms1_textBox.Update();
                        break;
                    case 1:
                        rms2_textBox.Text = rms[1].ToString("F5");
                        rms2_textBox.Update();
                        break;
                    case 2:
                        rms3_textBox.Text = rms[2].ToString("F5");
                        rms3_textBox.Update();
                        break;
                }
            }

            double[] solveforVec = new double[6];
            iterN_textBox.Text = "0";
            iterN_textBox.Update();
            perIdx_textBox.Text = oldCostFnc.ToString("F5");
            perIdx_textBox.Update();
            GetOrbitalState(Global.sat, solveforVec);
            for (int i = 0; i < nSolveforParams; i++)
            {
                totalSolveforVec[0, i] = solveforVec[i];
            }
            for (int i = 0; i < nSolveforParams; i++)
            {
                double var = solveforVec[i] - aprioriSolveforVec[i];
                switch (i)
                {
                    case 0:
                        delta1_textBox.Text = var.ToString("F5");
                        delta1_textBox.Update();
                        break;
                    case 1:
                        delta2_textBox.Text = var.ToString("F5");
                        delta2_textBox.Update();
                        break;
                    case 2:
                        delta3_textBox.Text = var.ToString("F5");
                        delta3_textBox.Update();
                        break;
                    case 3:
                        delta4_textBox.Text = var.ToString("F5");
                        delta4_textBox.Update();
                        break;
                    case 4:
                        delta5_textBox.Text = var.ToString("F5");
                        delta5_textBox.Update();
                        break;
                    case 5:
                        delta6_textBox.Text = var.ToString("F5");
                        delta6_textBox.Update();
                        break;
                }
            }

            // Main iteration
            int estFlag = 9999;
            double costFnc;
            for (int iter = 1; iter <= iterMax; iter++)
            {
                costFnc = LeastSquaresIterator(Global.estClass, Global.sat, Global.paramClass, Global.obsClass, Global.dynModel, Global.meaModel,
                    aprioriSolveforVec, Global.residual);

                iterN_textBox.Text = Convert.ToString(iter);
                iterN_textBox.Update();
                perIdx_textBox.Text = costFnc.ToString("F5");
                perIdx_textBox.Update();

                GetResidualRMS(Global.residual, rms);
                for (int i = 0; i < info[1]; i++)
                {
                    switch (i)
                    {
                        case 0:
                            rms1_textBox.Text = rms[0].ToString("F5");
                            rms1_textBox.Update();
                            break;
                        case 1:
                            rms2_textBox.Text = rms[1].ToString("F5");
                            rms2_textBox.Update();
                            break;
                        case 2:
                            rms3_textBox.Text = rms[2].ToString("F5");
                            rms3_textBox.Update();
                            break;
                    }
                }

                GetOrbitalState(Global.sat, solveforVec);
                for (int i = 0; i < nSolveforParams; i++)
                {
                    totalSolveforVec[0, i] = solveforVec[i];
                }
                for (int i = 0; i < nSolveforParams; i++)
                {
                    double var = solveforVec[i] - aprioriSolveforVec[i];
                    switch (i)
                    {
                        case 0:
                            delta1_textBox.Text = var.ToString("F5");
                            delta1_textBox.Update();
                            break;
                        case 1:
                            delta2_textBox.Text = var.ToString("F5");
                            delta2_textBox.Update();
                            break;
                        case 2:
                            delta3_textBox.Text = var.ToString("F5");
                            delta3_textBox.Update();
                            break;
                        case 3:
                            delta4_textBox.Text = var.ToString("F5");
                            delta4_textBox.Update();
                            break;
                        case 4:
                            delta5_textBox.Text = var.ToString("F5");
                            delta5_textBox.Update();
                            break;
                        case 5:
                            delta6_textBox.Text = var.ToString("F5");
                            delta6_textBox.Update();
                            break;
                    }
                }
                // Converge test
                estFlag = ConvergenceTest(Global.estClass, costFnc, oldCostFnc, divFlag, iter);
                if (estFlag == 10)
                {
                    oldCostFnc = costFnc;
                }
                else
                {
                    break;
                }
            }

            return estFlag;
        }
    }
}