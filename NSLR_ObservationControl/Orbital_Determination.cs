using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using static NSLR_ObservationControl.Module.Observation_TMS;
using System.Threading;
using System.Threading.Tasks;

namespace NSLR_ObservationControl
{
    internal class Orbital_Determination    // 궤도 결정
    {
        ///////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LoadObservation(IntPtr obsClass, StringBuilder _fileName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetObservatoryName(IntPtr obsClass, int n, StringBuilder siteName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetNObsData(IntPtr obsClass, int n);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetObservationTime(IntPtr obsClass, int n, double[] _timeSet);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ConvertMJD2Greg(IntPtr timeSystem, double mjd, double[] greg);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetOrbit(IntPtr _sat, double[] _stateVec, StringBuilder _type, StringBuilder _coordSys, StringBuilder epochType, StringBuilder _epochSys, double[] epochTime);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpdateParameterSatellite(IntPtr paramClass, IntPtr satClass);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetObservatory(IntPtr estClass, StringBuilder obs);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetObservable(IntPtr estClass, bool[] idxVec);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetParameterEpochTime(IntPtr paramClass, double[] dateVec);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetEstimatorTime(IntPtr estClass, StringBuilder type, double[] dateVec);
        ///////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetOrbitalState(IntPtr _sat, double[] stateVec, StringBuilder type);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateObservation();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int LeastSquaresInitializer(IntPtr estClass, IntPtr paramClass, IntPtr obsClass, IntPtr estObsClass, IntPtr dynModel, double[] aprioriSolveforVec, double[] aprioriCov);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double LeastSquaresIterator(IntPtr estClass, IntPtr satClass, IntPtr paramClass, IntPtr obsClass, IntPtr dynModel, IntPtr meaModel, double[] aprioriSolveforVec, double[] aprioriCov, IntPtr resStr);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetResidualDataInfo(IntPtr resStr, int[] info);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetResidualRMS(IntPtr resStr, double[] _rms);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ConvergenceTest(IntPtr estClass, double costFnc, double oldCostFnc, bool[] divFlag, int iter);
        ///////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetSiteLocation(IntPtr obsSite, double[] siteLoc);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void InitializeResidual(IntPtr resStr);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AdaptiveKalmanFilter(IntPtr estClass, IntPtr satClass, IntPtr paramClass, IntPtr dynModel, IntPtr meaModel, IntPtr observatory,
            int[] _dataTypes, double[] _obsData, double timeTag, double[] envVec, double[] quality, double[] _delX, double[] _residual, double[] _covMat);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetResidualData(IntPtr resStr, double timeTag, double[] data, int n);
        ///////////////////////////////////////////////////////////////////////////////////

        public Orbital_Determination()
        {
            //Create_FullRate_CRDFile();
            Thread thread = new Thread(Run_ExtendedKalmanFilter);
            thread.Start();
            //Create_FullRate_CRDFile();
        }

        ~Orbital_Determination()
        {

        }

        public void Create_FullRate_CRDFile()    // 통합 레이저 범위 측정 데이터 형식(CRD, Full Rate) 생성
        {
            // CRD 파일 생성
            
            FileInfo fileInfo = new FileInfo("./Resources/Observation/" + "CRD_TestFile.txt");
            using (StreamWriter crd_writer = fileInfo.CreateText())
            {
                // H1 - Format Header (6항목) : 'CRD' or 'crd' / Format Version / Year of file production / Month of file production / Day of file production / Hour of file production
                crd_writer.WriteLine("h1 crd  1 2020 12 31 19");
                // H2 - Station Header (5항목) : Station name from official list / Crustal Dynamics Project Pad Identifier / Crustal Dynamics Project 2-digit system number / Crustal Dynamics Project 2-digit / Station Epoch Time Scale
                crd_writer.WriteLine("h2       MATM 7941 77  1  4");
                // H3 - Target Header (6항목) : Target name from official list / ILRS satellite identifier / SIC (Satellite Identification Code) / NORAD ID / Spacecraft epoch time scale / Target type
                crd_writer.WriteLine("h3 lageos1     7603901 1155 8820     0 1");
                // H4 - Session Header (21항목) : Data type / Starting year / Starting month / Starting day / Starting hour / Starting minute / Starting second / Ending year / Ending month / Ending day / Ending hour / Ending minute / Ending second / 
                // A release flag to indicate the data release / Tropospheric refraction correction applied indicator / Center of mass correction applied indicator / Receive amplitude correction applied indicator / Station system delay applied indicator / 
                // Spacecraft system delay appied indicator / Range type indicator / Data quality alert indicator
                crd_writer.WriteLine("h4  0 2020 12 31 17 47 59 2020 12 31 17 55 38  1 0 0 1 1 0 2 0");

                // C0 - System Configuration Record (6항목)
                crd_writer.WriteLine("c0 0 532.000 std1 ml1 mcp mt1");
                // C1 - Laser Configuration Record
                crd_writer.WriteLine("c1 0  ml1 Nd:VAN        1064.00      10.00     100.00   40.0   .00    1");
                // C2 - Detector Configuration Record
                crd_writer.WriteLine("c2 0  mcp MCP           532.000  10.10  -1.0  -1.0 photon-dependent      -1.0   .30  50.0  -1.0       none");
                // C3 - Timing System Configuration Record
                crd_writer.WriteLine("c3 0  mt1 Symmetricom_CS4000   Meinberg_LanTime     HTSI                 na     .0");

                if (testObservation_timeTag.Length == (testObservation_dataSet.Length / 3))
                {
                    for (int i = 0; i < testObservation_timeTag.Length; i++)
                    {
                        string[] ToF_datas = (testObservation_dataSet[i, 0].ToString(/*"F13"*/)).Split('.');

                        // 10 Record (8항목) : Seconds of day / Time of flight / System configuration id / Epoch event / Filter flag / Detector channel / Stop number / Receive amplitude
                        crd_writer.WriteLine("10" + " " + testObservation_timeTag[i].ToString(/*"F13"*/) + "   " + "." + ToF_datas[1]
                            + " " + "std1" + " " + "2" + " " + "2" + " " + "0" + " " + "0" + "    " + "30");
                        // 30 Record (6항목) : Seconds of day / Azimuth in degree / Elevation in degree / Direction flag / Angle origin indicator / Refraction corrected
                        crd_writer.WriteLine("30" + " " + testObservation_timeTag[i].ToString(/*"F13"*/) + " " + testObservation_dataSet[i, 1].ToString(/*"F4"*/) + "  " + testObservation_dataSet[i, 2].ToString(/*"F4"*/) 
                            + " " + "0" + " " + "2" + " " + "0");
                    }
                    
                }

                crd_writer.WriteLine("H8");
                crd_writer.WriteLine("H9");
            }
            MessageBox.Show("파일수정 완료!");
            
        }

        public void Test_Function() // 기존(Test용) CRD 파일에서 Row Data 추출용
        {
            FileInfo read_fileInfo = new FileInfo("./Resources/Observation/lageos1_20210102_test.frd");
            FileInfo write_fileInfo = new FileInfo("./Resources/Observation/Data.txt");
            using (StreamReader reader = new StreamReader(read_fileInfo.FullName, Encoding.UTF8))
            {
                using (StreamWriter writer = write_fileInfo.CreateText())
                {
                    int count = 0;
                    string data_str = "";

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] line_words = line.Split(' ');


                        if (line_words[0] == "10")
                        {
                            count++;
                            if (count > 1)
                            {
                                data_str += (", " + line_words[1]);
                            }
                            else
                            {
                                data_str += line_words[1];
                            }

                        }
                    }
                    writer.WriteLine(data_str);
                    MessageBox.Show(count.ToString());
                }

            }

            MessageBox.Show("완료!");
        }

        public void Run_WeightedLeastSquares1()  // 가중최소자승법 기반 궤도결정 (1)
        {
            MessageBox.Show("궤도결정 시작!");
            StringBuilder CRDFile = new StringBuilder("./Resources/Observation/lageos1_20210102_test.frd");
            LoadObservation(Global.obsClass, CRDFile);

            StringBuilder siteName = new StringBuilder();
            GetObservatoryName(Global.obsClass, 0, siteName);
            // 관측소 이름
            //obsSite_textBox.Text = siteName.ToString();
            //obsSite_textBox.Update();

            int nObs = GetNObsData(Global.obsClass, 0);
            // 데이터 갯수
            //nObs_textBox.Text = nObs.ToString();
            //nObs_textBox.Update();

            double[] obsTimeSet = new double[2];
            GetObservationTime(Global.obsClass, 0, obsTimeSet);
            double[] startDate = new double[6];
            double[] finalTime = new double[6];
            ConvertMJD2Greg(Global.timeSys, obsTimeSet[0], startDate);
            // 시작시간
            //obsTimeIni_textBox.Text = startDate[0].ToString() + " " + startDate[1].ToString() + " " + startDate[2].ToString() + " " + startDate[3].ToString() + ":" + startDate[4].ToString() + ":" + startDate[5].ToString();
            //obsTimeIni_textBox.Update();
            ConvertMJD2Greg(Global.timeSys, obsTimeSet[0] + 20.0 / 86400, finalTime);
            // 종료시간
            //obsTimeFin_textBox.Text = finalTime[0].ToString() + " " + finalTime[1].ToString() + " " + finalTime[2].ToString() + " " + finalTime[3].ToString() + ":" + finalTime[4].ToString() + ":" + finalTime[5].ToString();
            //obsTimeFin_textBox.Update();

            double[] stateVec = { 9140.11102616297, 7852.43515050818, 2575.81911612253, 2.13175968954918, -0.753413759771749, -5.20670191770466 };
            double[] epochDate = { 2020, 12, 31, 17, 47, 59.4039997558594 };
            StringBuilder type = new StringBuilder("Cartesian");
            StringBuilder coord = new StringBuilder("EarthMJ2000Eq");
            StringBuilder epochType = new StringBuilder("Gregorian");
            StringBuilder epochSys = new StringBuilder("UTC");
            SetOrbit(Global.sat, stateVec, type, coord, epochType, epochSys, epochDate);

            UpdateParameterSatellite(Global.paramClass, Global.sat);

            StringBuilder obsName = new StringBuilder("MATERA, ITALY");
            bool[] obsIdx = { true, true };

            SetObservatory(Global.estClass, obsName);
            SetObservable(Global.estClass, obsIdx);
            SetParameterEpochTime(Global.paramClass, epochDate);
            epochType = new StringBuilder("start");
            SetEstimatorTime(Global.estClass, epochType, startDate);
            epochType = new StringBuilder("end");
            SetEstimatorTime(Global.estClass, epochType, finalTime);

            // Result Label/TextBox 초기화
            //LabelInitializer(sender, 0);
            // 파라미터 필요한지 여부 Check
            int flag = Run_WeightedLeastSquares2(/*sender, e*/);
        }

        public int Run_WeightedLeastSquares2() // 가중최소자승법 기반 궤도결정 (2)
        {
            // initialize
            bool[] divFlag = { false };

            int nSolveforParams = 6;
            double[] aprioriSolveforVec = new double[nSolveforParams];
            double[] aprioriCov = new double[nSolveforParams];
            StringBuilder type = new StringBuilder();
            GetOrbitalState(Global.sat, aprioriSolveforVec, type);

            IntPtr estObsClass = CreateObservation();
            int iterMax = LeastSquaresInitializer(Global.estClass, Global.paramClass, Global.obsClass, estObsClass, Global.dynModel,
                                        aprioriSolveforVec, aprioriCov);

            double[,] totalSolveforVec = new double[iterMax, nSolveforParams];
            for (int i = 0; i < nSolveforParams; i++)
            {
                totalSolveforVec[0, i] = aprioriSolveforVec[i];
            }

            // initial guess
            double oldCostFnc = LeastSquaresIterator(Global.estClass, Global.sat, Global.paramClass, estObsClass, Global.dynModel, Global.meaModel,
            aprioriSolveforVec, aprioriCov, Global.residual);

            int[] info = new int[2];
            GetResidualDataInfo(Global.residual, info);
            double[] rms = new double[info[1]];
            GetResidualRMS(Global.residual, rms);

            // Result TextBox 초기화
            //iterN_textBox.Text = "0";
            //iterN_textBox.Update();
            //perIdx_textBox.Text = oldCostFnc.ToString("F5");
            //perIdx_textBox.Update();
            MessageBox.Show(0.ToString() + "/" + iterMax.ToString());

            double[] solveforVec = new double[6];
            double[] deltaX = new double[6];
            GetOrbitalState(Global.sat, solveforVec, type);
            for (int i = 0; i < nSolveforParams; i++)
            {
                totalSolveforVec[0, i] = solveforVec[i];
                deltaX[i] = solveforVec[i] - aprioriSolveforVec[i];
            }
            // Result TextBox Set
            //LabelUpdater(sender, deltaX, rms);

            // Main iteration
            int estFlag = 9999;
            double costFnc;
            for (int iter = 1; iter <= iterMax; iter++)
            {
                costFnc = LeastSquaresIterator(Global.estClass, Global.sat, Global.paramClass, estObsClass, Global.dynModel, Global.meaModel,
                    aprioriSolveforVec, aprioriCov, Global.residual);

                // Result TextBox Set
                //iterN_textBox.Text = Convert.ToString(iter);
                //iterN_textBox.Update();
                //perIdx_textBox.Text = costFnc.ToString("F5");
                //perIdx_textBox.Update();
                MessageBox.Show(iter.ToString() + "/" + iterMax.ToString());

                GetResidualRMS(Global.residual, rms);

                GetOrbitalState(Global.sat, solveforVec, type);
                for (int i = 0; i < nSolveforParams; i++)
                {
                    totalSolveforVec[0, i] = solveforVec[i];
                    deltaX[i] = solveforVec[i] - aprioriSolveforVec[i];
                }

                // Result TextBox Set
                //LabelUpdater(sender, deltaX, rms);

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
            MessageBox.Show("궤도결정 끝!");
            return estFlag;
        }

        public void Run_ExtendedKalmanFilter()  // 확장칼만필터 기반 궤도결정
        {
            MessageBox.Show("궤도결정(EKF) 시작!");
            StringBuilder CRDFile = new StringBuilder("./Resources/Observation/lageos1_20210102_test.frd");
            LoadObservation(Global.obsClass, CRDFile);

            StringBuilder siteName = new StringBuilder();
            GetObservatoryName(Global.obsClass, 0, siteName);
            // 관측소 이름
            //obsSite_textBox.Text = siteName.ToString();
            //obsSite_textBox.Update();

            int nObs = GetNObsData(Global.obsClass, 0);
            // 데이터 갯수
            //nObs_textBox.Text = nObs.ToString();
            //nObs_textBox.Update();

            double[] obsTimeSet = new double[2];
            GetObservationTime(Global.obsClass, 0, obsTimeSet);
            double[] startDate = new double[6];
            double[] finalTime = new double[6];
            ConvertMJD2Greg(Global.timeSys, obsTimeSet[0], startDate);
            // 시작 시간
            //obsTimeIni_textBox.Text = startDate[0].ToString() + " " + startDate[1].ToString() + " " + startDate[2].ToString() + " " + startDate[3].ToString() + ":" + startDate[4].ToString() + ":" + startDate[5].ToString();
            //obsTimeIni_textBox.Update();
            ConvertMJD2Greg(Global.timeSys, obsTimeSet[0] + 20.0 / 86400, finalTime);
            // 종료 시간
            //obsTimeFin_textBox.Text = finalTime[0].ToString() + " " + finalTime[1].ToString() + " " + finalTime[2].ToString() + " " + finalTime[3].ToString() + ":" + finalTime[4].ToString() + ":" + finalTime[5].ToString();
            //obsTimeFin_textBox.Update();

            double[] stateVec = { 9140.11102616297, 7852.43515050818, 2575.81911612253, 2.13175968954918, -0.753413759771749, -5.20670191770466 };
            double[] epochDate = { 2020, 12, 31, 17, 47, 59.4039997558594 };
            StringBuilder type = new StringBuilder("Cartesian");
            StringBuilder coord = new StringBuilder("EarthMJ2000Eq");
            StringBuilder epochType = new StringBuilder("Gregorian");
            StringBuilder epochSys = new StringBuilder("UTC");
            SetOrbit(Global.sat, stateVec, type, coord, epochType, epochSys, epochDate);

            UpdateParameterSatellite(Global.paramClass, Global.sat);

            bool[] obsIdx = { true, true };

            SetObservatory(Global.estClass, siteName);
            SetObservable(Global.estClass, obsIdx);
            SetParameterEpochTime(Global.paramClass, epochDate);
            epochType = new StringBuilder("start");
            SetEstimatorTime(Global.estClass, epochType, startDate);
            epochType = new StringBuilder("end");
            SetEstimatorTime(Global.estClass, epochType, finalTime);

            double[] siteLoc = { 16.70461606558835, 40.64866793742697, 0.5373781809048523 };
            SetSiteLocation(Global.laserSite, siteLoc);

            int[] dataTypes = { 10, 20, 21 };
            double[] obsData = new double[3];
            double[] environment = { 0, 0, 0 };
            double[] quality = { 1.66782047599076e-11, 2.77777777777778e-05, 2.77777777777778e-05 };

            double[] envVec = { 1050, 0.8, 273 };

            double[] delX = new double[6];
            double[] residual = new double[3];
            double[] covMat = new double[36];

            // DateSet 대체(임시)
            //double[] timeTag = new double[1237];
            //double[,] dataSet = new double[1237, 3];
            //int n = GetTestDataSet(timeTag, dataSet);
            int n = testObservation_timeTag.Length;

            // Result Label/TextBox 초기화
            //LabelInitializer(sender, 1);

            InitializeResidual(Global.residual);
            for (int i = 0; i < n; i++)
            {
                if (testObservation_timeTag[i]/*timeTag[i]*/ > obsTimeSet[0] + 20.0 / 86400) { break; }
                // Result TextBox Set
                //iterN_textBox.Text = i.ToString();
                //iterN_textBox.Update();

                for (int j = 0; j < 3; j++)
                    obsData[j] = testObservation_dataSet[i, j]/*dataSet[i, j]*/;

                AdaptiveKalmanFilter(Global.estClass, Global.sat, Global.paramClass, Global.dynModel, Global.meaModel, Global.laserSite,
                dataTypes, obsData, testObservation_timeTag[i]/*timeTag[i]*/, envVec, quality,
                delX, residual, covMat);

                SetResidualData(Global.residual, testObservation_timeTag[i]/*timeTag[i]*/, residual, 3);

                // Result TextBox Set
                //LabelUpdater(sender, delX, residual);

                /*
                for (int j = 0; j < 3; j++)
                    quality[j] = Math.Abs(residual[j]);
                */
                MessageBox.Show(i + " / " + n);
            }
            MessageBox.Show("궤도결정(EKF) 끝!");
        }

        public int GetTestDataSet(double[] timetag, double[,] dataSet)
        {
            /*
            for (int n=0;n< _timetag.Length; n++)
            {
                timetag[n] = _timetag[n];
                for (int i = 0; i < 3; i++)
                {
                    dataSet[n, i] = _dataSet[n, i];
                }
            }
            //timetag = _timetag;
            //dataSet = _dataSet;

            return _timetag.Length;
            */
            return 0;
        }
        
        ////////////////////////////////////////////////////////////////// Observation Data (임시) //////////////////////////////////////////////////////////////////
        
        // 시간 (Length:1237)
        double[] testObservation_timeTag = { 64079.4040000145358, 64079.5040000145411, 64079.7040000145436, 64081.0040000145434, 64081.3040000145469, 64081.4040000145444, 64083.4040000145408, 64087.4040000145385, 64087.8040000145371, 64088.0040000145390, 64088.9040000145413, 64089.1040000145414, 64089.3040000145382, 64089.6040000145202, 64090.3040000145377, 64090.8040000145327, 64091.0040000145455, 64091.3040000145377, 64091.8040000145388, 64092.2040000145399, 64095.0040000145468, 64095.8040000145329, 64096.8040000145375, 64097.4040000145363, 64098.2040000145378, 64099.8040000145411, 64100.7040000145321, 64100.9040000145455, 64101.2040000145398, 64101.4040000145398, 64102.3040000145396, 64102.5040000145344, 64102.6040000145406, 64102.8040000145407, 64103.0040000145347, 64103.1040000145384, 64103.2040000145360, 64103.4040000145450, 64103.6040000145388, 64104.0040000145478, 64104.1040000145378, 64104.3040000145384, 64104.5040000145435, 64104.7040000145416, 64104.8040000145459, 64104.9040000145347, 64105.0040000145405, 64105.1040000145397, 64105.4040000145395, 64105.7040000145417, 64106.1040000145418, 64106.3040000145394, 64106.6040000145414, 64106.9040000145406, 64107.1040000145382, 64107.2040000145490, 64107.6040000145496, 64108.1040000145429, 64108.4040000145414, 64108.5040000145417, 64108.7040000145431, 64108.9040000145377, 64109.1040000145405, 64109.3040000145408, 64109.4040000145420, 64109.5040000145470, 64109.6040000145425, 64109.7040000145375, 64110.1040000145455, 64110.5040000145404, 64110.6040000145314, 64110.7040000145466, 64110.8040000145430, 64110.9040000145409, 64111.0040000145398, 64111.2040000145455, 64111.3040000145360, 64111.4040000145431, 64111.8040000145449, 64112.0040000145518, 64112.3040000145334, 64112.4040000145425, 64112.5040000145377, 64112.6040000145362, 64112.7040000145364, 64113.0040000145414, 64113.3040000145273, 64113.7040000145433, 64113.8040000145395, 64113.9040000145362, 64114.0040000145399, 64114.2040000145374, 64114.8040000145372, 64115.0040000145474, 64115.3040000145388, 64115.6040000145347, 64115.7040000145415, 64115.8040000145496, 64116.1040000145346, 64116.3040000145365, 64117.0040000145373, 64117.1040000145406, 64117.2040000145353, 64117.3040000145365, 64117.6040000145359, 64117.7040000145321, 64119.4040000145433, 64119.9040000145340, 64120.3040000145351, 64120.5040000145365, 64120.8040000145291, 64121.2040000145360, 64121.3040000145378, 64121.9040000145351, 64122.1040000145307, 64122.2040000145294, 64122.3040000145296, 64122.5040000145402, 64122.7040000145438, 64122.8040000145361, 64123.0040000145426, 64123.1040000145402, 64123.4040000145393, 64123.7040000145356, 64123.9040000145344, 64124.1040000145342, 64124.5040000145346, 64124.6040000145384, 64124.7040000145344, 64124.8040000145363, 64124.9040000145337, 64125.1040000145366, 64125.4040000145306, 64125.5040000145307, 64125.9040000145326, 64126.0040000145212, 64126.1040000145422, 64126.2040000145363, 64126.4040000145392, 64127.4040000145374, 64127.7040000145369, 64128.0040000145433, 64130.9040000145462, 64131.1040000145427, 64131.3040000145409, 64131.6040000145474, 64131.9040000145427, 64132.0040000145442, 64132.2040000145315, 64132.5040000145415, 64132.6040000145427, 64133.4040000145365, 64133.6040000145366, 64133.8040000145319, 64134.2040000145380, 64134.3040000145349, 64134.4040000145422, 64134.5040000145428, 64134.9040000145430, 64135.7040000145462, 64136.0040000145337, 64136.1040000145475, 64136.4040000145429, 64136.5040000145306, 64136.8040000145428, 64136.9040000145386, 64137.1040000145362, 64137.2040000145412, 64137.4040000145428, 64137.6040000145433, 64137.9040000145441, 64138.9040000145383, 64139.3040000145425, 64139.5040000145446, 64140.1040000145444, 64140.4040000145407, 64140.7040000145437, 64141.0040000145357, 64141.1040000145416, 64141.6040000145436, 64141.7040000145393, 64142.1040000145408, 64142.2040000145438, 64142.5040000145406, 64142.6040000145423, 64142.9040000145433, 64143.0040000145358, 64143.3040000145422, 64143.6040000145376, 64144.0040000145428, 64144.2040000145371, 64144.5040000145363, 64144.6040000145180, 64145.0040000145451, 64145.1040000145323, 64145.2040000145361, 64145.4040000145454, 64145.5040000145436, 64145.6040000145419, 64145.7040000145434, 64145.8040000145364, 64145.9040000145367, 64146.0040000145309, 64146.1040000145360, 64146.8040000145420, 64147.0040000145456, 64147.2040000145410, 64147.5040000145356, 64147.6040000145463, 64147.7040000145368, 64147.8040000145383, 64148.1040000145372, 64148.5040000145356, 64148.8040000145373, 64149.1040000145412, 64149.5040000145381, 64149.6040000145380, 64149.7040000145403, 64150.1040000145430, 64150.3040000145356, 64151.1040000145405, 64151.3040000145396, 64151.8040000145414, 64152.0040000145454, 64152.4040000145358, 64152.7040000145336, 64153.0040000145387, 64153.2040000145330, 64153.3040000145327, 64153.5040000145342, 64153.7040000145416, 64153.8040000145358, 64154.4040000145355, 64155.7040000145252, 64156.2040000145335, 64156.4040000145406, 64156.5040000145400, 64156.9040000145415, 64157.2040000145399, 64157.3040000145417, 64157.4040000145388, 64157.7040000145362, 64158.3040000145365, 64158.4040000145350, 64158.9040000145401, 64159.1040000145434, 64159.2040000145360, 64159.6040000145468, 64159.9040000145426, 64160.8040000145378, 64160.9040000145368, 64161.1040000145363, 64161.7040000145350, 64161.8040000145378, 64161.9040000145351, 64162.0040000145465, 64162.4040000145408, 64162.5040000145279, 64162.7040000145284, 64162.8040000145395, 64162.9040000145333, 64163.0040000145411, 64163.1040000145423, 64163.3040000145354, 64163.4040000145358, 64163.6040000145398, 64164.4040000145397, 64164.5040000145374, 64164.6040000145412, 64164.9040000145357, 64165.0040000145390, 64165.8040000145440, 64166.4040000145435, 64166.7040000145366, 64166.8040000145344, 64166.9040000145445, 64167.0040000145343, 64167.4040000145395, 64167.8040000145358, 64168.2040000145377, 64168.4040000145384, 64168.7040000145393, 64168.9040000145381, 64169.1040000145330, 64169.4040000145345, 64170.5040000145433, 64170.6040000145346, 64170.7040000145434, 64170.9040000145395, 64171.2040000145386, 64172.2040000145391, 64172.6040000145377, 64172.8040000145368, 64173.0040000145357, 64173.5040000145386, 64173.7040000145402, 64173.9040000145243, 64174.3040000145447, 64174.5040000145390, 64174.7040000145345, 64174.8040000145390, 64175.1040000145432, 64175.2040000145398, 64175.4040000145342, 64175.6040000145336, 64176.3040000145366, 64176.7040000145379, 64176.8040000145352, 64177.1040000145414, 64177.4040000145432, 64177.5040000145358, 64177.8040000145404, 64178.0040000145343, 64178.5040000145421, 64178.7040000145398, 64178.8040000145403, 64178.9040000145353, 64179.1040000145423, 64179.4040000145446, 64179.6040000145412, 64179.7040000145376, 64180.3040000145403, 64180.4040000145201, 64180.5040000145397, 64180.6040000145503, 64180.7040000145412, 64181.5040000145528, 64181.6040000145344, 64181.7040000145404, 64181.8040000145496, 64182.0040000145449, 64182.1040000145446, 64182.3040000145418, 64183.0040000145463, 64183.1040000145471, 64183.3040000145362, 64183.4040000145419, 64183.8040000145394, 64183.9040000145354, 64184.0040000145433, 64184.1040000145422, 64184.4040000145468, 64184.5040000145507, 64184.8040000145423, 64185.0040000145493, 64185.3040000145487, 64185.4040000145489, 64185.5040000145513, 64185.6040000145545, 64185.8040000145475, 64186.4040000145406, 64187.3040000145454, 64187.5040000145349, 64188.2040000145411, 64188.4040000145444, 64189.4040000145437, 64189.9040000145468, 64190.0040000145405, 64190.1040000145404, 64190.2040000145370, 64190.7040000145515, 64191.1040000145479, 64191.2040000145472, 64191.9040000145445, 64192.3040000145460, 64193.0040000145381, 64193.5040000145400, 64193.6040000145440, 64193.9040000145421, 64194.7040000145374, 64195.0040000145420, 64195.3040000145327, 64196.3040000145436, 64196.4040000145391, 64196.8040000145521, 64196.9040000145441, 64200.1040000145436, 64200.7040000145428, 64201.0040000145407, 64201.1040000145454, 64201.7040000145451, 64201.8040000145415, 64203.1040000145409, 64203.2040000145444, 64203.4040000145450, 64203.5040000145412, 64203.6040000145356, 64204.1040000145416, 64204.2040000145460, 64204.4040000145433, 64204.5040000145385, 64205.0040000145506, 64205.1040000145349, 64206.5040000145418, 64207.0040000145344, 64207.1040000145489, 64207.6040000145413, 64207.7040000145362, 64208.3040000145391, 64210.0040000145436, 64210.5040000145430, 64211.6040000145440, 64212.3040000145409, 64213.1040000145424, 64213.3040000145450, 64213.4040000145396, 64213.8040000145443, 64214.1040000145371, 64214.4040000145350, 64218.6040000145398, 64218.9040000145348, 64219.0040000145389, 64219.1040000145281, 64220.4040000145353, 64220.7040000145356, 64220.9040000145383, 64221.4040000145367, 64222.1040000145351, 64222.3040000145394, 64222.4040000145316, 64222.5040000145384, 64222.9040000145411, 64223.7040000145367, 64224.1040000145372, 64224.2040000145341, 64225.6040000145421, 64226.2040000145342, 64227.2040000145336, 64227.3040000145317, 64227.7040000145415, 64228.3040000145372, 64229.5040000145377, 64229.8040000145397, 64230.4040000145409, 64231.0040000145396, 64232.2040000145430, 64232.5040000145330, 64233.5040000145358, 64233.6040000145479, 64233.7040000145428, 64234.5040000145372, 64234.7040000145420, 64235.1040000145328, 64236.1040000145426, 64236.8040000145387, 64238.1040000145438, 64238.2040000145413, 64238.4040000145348, 64238.5040000145397, 64238.6040000145355, 64239.1040000145446, 64239.3040000145443, 64239.4040000145411, 64239.7040000145461, 64240.1040000145424, 64240.4040000145399, 64240.6040000145433, 64240.7040000145398, 64240.8040000145367, 64241.0040000145357, 64241.3040000145433, 64241.6040000145397, 64241.8040000145407, 64242.1040000145387, 64242.5040000145401, 64242.6040000145370, 64242.8040000145346, 64242.9040000145389, 64243.0040000145368, 64243.1040000145309, 64243.2040000145397, 64243.8040000145349, 64244.0040000145439, 64244.1040000145413, 64244.2040000145438, 64244.3040000145386, 64244.9040000145430, 64245.2040000145372, 64245.3040000145430, 64245.4040000145403, 64245.7040000145385, 64245.8040000145351, 64245.9040000145406, 64246.4040000145432, 64246.8040000145340, 64247.6040000145361, 64247.7040000145369, 64248.0040000145408, 64248.3040000145349, 64248.4040000145370, 64248.7040000145331, 64248.8040000145353, 64249.1040000145365, 64249.2040000145374, 64249.4040000145347, 64249.6040000145395, 64249.9040000145323, 64250.3040000145300, 64250.6040000145411, 64251.1040000145295, 64251.8040000145392, 64251.9040000145396, 64252.3040000145400, 64252.4040000145378, 64252.6040000145372, 64252.7040000145482, 64252.8040000145470, 64252.9040000145374, 64253.2040000145432, 64254.0040000145341, 64254.6040000145411, 64255.1040000145380, 64255.3040000145398, 64255.8040000145345, 64256.0040000145425, 64256.1040000145420, 64257.2040000145369, 64257.4040000145335, 64257.5040000145475, 64257.6040000145395, 64257.8040000145403, 64257.9040000145406, 64258.4040000145400, 64258.5040000145432, 64258.7040000145206, 64259.0040000145398, 64259.3040000145430, 64259.4040000145334, 64259.8040000145394, 64259.9040000145315, 64260.0040000145365, 64260.1040000145324, 64260.2040000145311, 64261.2040000145334, 64261.3040000145412, 64261.4040000145377, 64261.7040000145423, 64262.3040000145420, 64262.8040000145328, 64262.9040000145095, 64263.2040000145431, 64263.7040000145338, 64263.8040000145383, 64264.1040000145363, 64264.3040000145235, 64264.5040000145356, 64264.6040000145255, 64264.9040000145432, 64265.0040000145387, 64265.2040000145350, 64265.6040000145356, 64266.1040000145370, 64266.6040000145348, 64267.1040000145340, 64267.5040000145328, 64267.7040000145344, 64268.1040000145369, 64268.2040000145336, 64268.6040000145373, 64268.8040000145354, 64268.9040000145410, 64269.1040000145344, 64269.6040000145453, 64270.2040000145361, 64270.3040000145352, 64270.8040000145410, 64271.2040000145362, 64271.4040000145349, 64271.7040000145356, 64271.8040000145405, 64272.1040000145391, 64272.2040000145419, 64272.6040000145312, 64273.3040000145367, 64273.8040000145430, 64274.2040000145393, 64274.3040000145455, 64274.4040000145320, 64274.9040000145493, 64275.1040000145452, 64275.2040000145353, 64275.4040000145419, 64275.8040000145366, 64276.0040000145440, 64276.3040000145391, 64276.4040000145287, 64277.3040000145481, 64277.5040000145338, 64277.6040000145409, 64278.2040000145381, 64278.3040000145424, 64278.4040000145403, 64278.5040000145387, 64278.6040000145419, 64278.9040000145372, 64279.0040000145316, 64279.1040000145354, 64279.2040000145402, 64279.8040000145363, 64279.9040000145427, 64280.4040000145447, 64280.7040000145355, 64281.0040000145391, 64281.1040000145453, 64281.3040000145398, 64281.5040000145395, 64281.8040000145338, 64282.1040000145416, 64282.6040000145371, 64283.1040000145426, 64283.4040000145318, 64284.1040000145427, 64284.9040000145394, 64285.0040000145328, 64285.7040000145394, 64285.9040000145348, 64286.5040000145407, 64286.6040000145424, 64286.8040000145501, 64287.1040000145466, 64287.2040000145508, 64287.4040000145270, 64287.8040000145420, 64288.0040000145469, 64288.1040000145502, 64288.6040000145478, 64288.9040000145425, 64289.0040000145319, 64289.1040000145496, 64289.2040000145447, 64289.4040000145486, 64289.5040000145397, 64289.7040000145495, 64290.0040000145420, 64290.1040000145434, 64290.2040000145437, 64290.3040000145486, 64290.4040000145449, 64290.5040000145477, 64290.6040000145525, 64290.7040000145519, 64291.1040000145462, 64291.2040000145461, 64291.3040000145366, 64291.4040000145488, 64291.8040000145394, 64293.1040000145525, 64293.4040000145499, 64293.7040000145395, 64294.2040000145453, 64294.6040000145452, 64294.9040000145378, 64295.2040000145403, 64295.4040000145388, 64295.6040000145412, 64295.7040000145463, 64295.9040000145419, 64296.1040000145368, 64296.2040000145392, 64296.5040000145416, 64296.6040000145486, 64296.8040000145448, 64296.9040000145391, 64297.0040000145451, 64297.1040000145343, 64297.5040000145429, 64298.2040000145475, 64298.8040000145365, 64299.3040000145398, 64299.4040000145398, 64299.5040000145416, 64299.8040000145440, 64300.1040000145469, 64300.3040000145427, 64300.6040000145371, 64300.7040000145419, 64300.8040000145459, 64301.0040000145431, 64301.2040000145444, 64302.3040000145394, 64302.4040000145454, 64302.5040000145466, 64303.7040000145443, 64304.1040000145414, 64304.3040000145492, 64304.6040000145474, 64305.5040000145456, 64305.6040000145485, 64305.9040000145420, 64306.4040000145459, 64306.5040000145427, 64306.6040000145405, 64306.8040000145447, 64306.9040000145400, 64307.1040000145434, 64307.3040000145549, 64309.3040000145508, 64309.4040000145425, 64310.0040000145440, 64310.8040000145408, 64311.0040000145400, 64311.4040000145401, 64312.8040000145302, 64312.9040000145471, 64313.0040000145520, 64313.1040000145397, 64314.1040000145408, 64314.3040000145404, 64314.4040000145450, 64314.6040000145438, 64314.8040000145405, 64315.0040000145452, 64315.6040000145444, 64315.7040000145522, 64315.8040000145415, 64316.2040000145348, 64316.4040000145421, 64316.5040000145454, 64316.6040000145361, 64317.9040000145382, 64318.1040000145394, 64318.3040000145425, 64318.5040000145446, 64318.9040000145213, 64319.2040000145400, 64319.4040000145390, 64319.9040000145356, 64320.0040000145269, 64320.1040000145408, 64320.2040000145447, 64320.3040000145364, 64321.2040000145334, 64321.3040000145376, 64321.4040000145376, 64321.5040000145425, 64321.6040000145350, 64321.9040000145366, 64322.8040000145292, 64323.2040000145341, 64323.4040000145417, 64323.5040000145413, 64324.2040000145411, 64324.4040000145391, 64324.5040000145493, 64325.4040000145467, 64325.6040000145479, 64326.4040000145464, 64326.5040000145405, 64326.6040000145463, 64326.7040000145398, 64326.8040000145404, 64327.0040000145354, 64327.1040000145433, 64327.2040000145412, 64327.3040000145467, 64327.8040000145408, 64328.0040000145420, 64328.9040000145463, 64329.2040000145414, 64329.6040000145482, 64331.9040000145457, 64332.4040000145321, 64333.0040000145401, 64333.4040000145412, 64333.5040000145376, 64333.7040000145451, 64334.0040000145312, 64334.1040000145461, 64335.6040000145436, 64335.8040000145381, 64336.0040000145474, 64336.3040000145341, 64336.4040000145433, 64336.7040000145399, 64336.9040000145386, 64337.0040000145414, 64337.2040000145396, 64337.4040000145389, 64337.5040000145411, 64337.6040000145351, 64337.7040000145361, 64337.8040000145393, 64338.4040000145364, 64339.1040000145435, 64340.2040000145392, 64340.5040000145363, 64340.8040000145405, 64340.9040000145393, 64341.2040000145341, 64341.5040000145406, 64341.8040000145427, 64342.2040000145362, 64343.3040000145409, 64343.9040000145360, 64344.5040000145369, 64345.4040000145434, 64345.8040000145413, 64346.5040000145414, 64347.1040000145389, 64347.2040000145420, 64349.6040000145414, 64350.3040000145383, 64350.5040000145356, 64350.8040000145486, 64351.3040000145427, 64352.0040000145506, 64352.1040000145415, 64352.4040000145460, 64352.5040000145433, 64353.3040000145408, 64353.4040000145427, 64353.8040000145543, 64354.1040000145436, 64354.4040000145472, 64354.6040000145481, 64355.1040000145437, 64355.2040000145479, 64355.3040000145473, 64355.5040000145460, 64355.6040000145482, 64356.0040000145420, 64356.2040000145538, 64356.5040000145448, 64356.6040000145485, 64357.7040000145503, 64357.9040000145508, 64358.0040000145510, 64358.4040000145456, 64358.5040000145382, 64358.7040000145450, 64358.9040000145464, 64360.1040000145458, 64360.2040000145380, 64360.4040000145406, 64360.7040000145450, 64361.3040000145467, 64361.5040000145428, 64361.8040000145469, 64361.9040000145421, 64362.7040000145370, 64363.6040000145508, 64363.7040000145543, 64364.1040000145480, 64364.2040000145508, 64364.5040000145453, 64364.6040000145428, 64364.8040000145383, 64365.1040000145464, 64365.3040000145414, 64365.9040000145455, 64367.0040000145414, 64367.1040000145382, 64368.1040000145371, 64370.4040000145418, 64370.7040000145433, 64371.3040000145451, 64371.5040000145476, 64372.3040000145430, 64372.5040000145470, 64372.6040000145509, 64373.4040000145427, 64373.5040000145375, 64373.6040000145436, 64373.7040000145459, 64374.0040000145416, 64374.3040000145361, 64374.5040000145492, 64374.7040000145426, 64375.1040000145409, 64375.5040000145406, 64375.6040000145391, 64375.7040000145390, 64375.8040000145419, 64376.0040000145462, 64376.1040000145526, 64376.3040000145443, 64376.5040000145395, 64376.6040000145490, 64376.7040000145438, 64376.8040000145446, 64377.0040000145454, 64377.1040000145403, 64377.2040000145421, 64378.2040000145381, 64378.9040000145435, 64379.0040000145439, 64379.7040000145449, 64380.2040000145410, 64380.4040000145481, 64380.5040000145449, 64380.8040000145457, 64380.9040000145449, 64382.1040000145441, 64382.3040000145473, 64382.6040000145464, 64382.9040000145414, 64383.5040000145440, 64385.6040000145460, 64385.8040000145416, 64389.0040000145364, 64389.5040000145371, 64390.4040000145404, 64391.1040000145501, 64392.0040000145467, 64392.5040000145385, 64392.8040000145368, 64394.2040000145496, 64394.4040000145402, 64395.6040000145363, 64396.2040000145431, 64396.5040000145345, 64397.2040000145407, 64398.1040000145381, 64398.6040000145392, 64398.7040000145350, 64399.6040000145420, 64401.6040000145298, 64402.3040000145422, 64403.1040000145325, 64403.7040000145283, 64403.8040000145365, 64403.9040000145337, 64404.4040000145364, 64404.6040000145361, 64405.6040000145444, 64406.6040000145411, 64407.4040000145392, 64408.1040000145386, 64409.6040000145404, 64409.7040000145303, 64410.0040000145391, 64410.2040000145421, 64410.5040000145383, 64410.6040000145372, 64411.2040000145331, 64413.0040000145385, 64413.2040000145385, 64413.3040000145430, 64413.7040000145237, 64413.8040000145432, 64413.9040000145392, 64414.3040000145403, 64415.1040000145434, 64415.3040000145342, 64415.5040000145394, 64415.8040000145328, 64416.0040000145334, 64416.1040000145399, 64416.6040000145393, 64417.3040000145330, 64417.9040000145437, 64418.1040000145325, 64418.3040000145436, 64418.4040000145448, 64418.6040000145335, 64418.7040000145402, 64419.2040000145334, 64419.5040000145402, 64419.6040000145376, 64420.0040000145377, 64420.1040000145422, 64420.4040000145409, 64421.4040000145421, 64421.5040000145358, 64421.7040000145388, 64422.9040000145390, 64423.2040000145357, 64423.7040000145491, 64423.9040000145395, 64424.5040000145438, 64425.5040000145369, 64425.7040000145452, 64426.1040000145337, 64426.4040000145362, 64426.9040000145441, 64427.5040000145474, 64427.7040000145445, 64427.8040000145417, 64428.4040000145438, 64428.5040000145380, 64428.6040000145460, 64428.7040000145527, 64429.1040000145441, 64429.4040000145472, 64429.7040000145438, 64430.2040000145407, 64430.3040000145491, 64430.9040000145487, 64431.1040000145499, 64431.4040000145458, 64431.5040000145463, 64432.5040000145523, 64432.8040000145463, 64432.9040000145552, 64433.6040000145476, 64434.1040000145417, 64434.6040000145498, 64435.1040000145477, 64435.3040000145335, 64436.8040000145416, 64436.9040000145449, 64437.1040000145438, 64441.2040000145417, 64443.5040000145399, 64446.5040000145408, 64448.0040000145410, 64448.3040000145401, 64448.7040000145442, 64449.3040000145440, 64449.4040000145376, 64449.5040000145496, 64449.6040000145498, 64449.7040000145365, 64450.0040000145334, 64450.1040000145400, 64450.6040000145438, 64450.7040000145459, 64450.8040000145429, 64451.0040000145446, 64451.2040000145456, 64451.3040000145475, 64451.5040000145447, 64451.7040000145456, 64451.9040000145449, 64452.1040000145455, 64452.2040000145486, 64452.3040000145403, 64452.4040000145498, 64452.6040000145374, 64452.8040000145411, 64452.9040000145384, 64453.0040000145413, 64453.6040000145354, 64454.6040000145425, 64454.7040000145369, 64455.0040000145444, 64455.3040000145418, 64455.5040000145451, 64455.7040000145358, 64456.0040000145464, 64456.1040000145346, 64456.4040000145460, 64456.6040000145398, 64456.7040000145487, 64457.0040000145471, 64457.1040000145457, 64457.4040000145433, 64457.5040000145473, 64457.6040000145483, 64457.7040000145439, 64457.8040000145534, 64458.0040000145473, 64458.1040000145519, 64458.2040000145460, 64458.3040000145443, 64458.6040000145498, 64459.6040000145488, 64460.2040000145430, 64460.5040000145443, 64460.7040000145506, 64461.0040000145494, 64461.1040000145486, 64461.6040000145454, 64461.9040000145376, 64462.1040000145437, 64462.3040000145469, 64462.6040000145459, 64462.9040000145479, 64463.0040000145451, 64463.1040000145454, 64463.6040000145408, 64463.7040000145446, 64464.1040000145437, 64464.2040000145391, 64464.4040000145476, 64465.0040000145461, 64465.5040000145515, 64466.0040000145432, 64466.1040000145430, 64466.2040000145461, 64466.3040000145480, 64466.8040000145446, 64466.9040000145443, 64467.1040000145486, 64467.2040000145449, 64467.4040000145486, 64467.9040000145453, 64468.1040000145404, 64468.4040000145494, 64468.6040000145437, 64468.7040000145423, 64468.8040000145524, 64469.0040000145510, 64469.1040000145501, 64469.2040000145240, 64469.5040000145459, 64469.8040000145428, 64470.2040000145506, 64470.3040000145477, 64470.5040000145464, 64471.2040000145383, 64471.7040000145417, 64472.2040000145421, 64472.4040000145449, 64473.0040000145415, 64473.3040000145360, 64473.4040000145493, 64473.6040000145370, 64473.9040000145433, 64474.1040000145441, 64474.6040000145380, 64475.2040000145406, 64475.4040000145431, 64475.7040000145356, 64475.8040000145375, 64475.9040000145343, 64476.0040000145355, 64476.4040000145495, 64477.3040000145495, 64477.6040000145391, 64478.0040000145381, 64479.0040000145427, 64480.0040000145395, 64480.3040000145332, 64480.4040000145428, 64480.9040000145422, 64481.8040000145400, 64482.0040000145386, 64482.5040000145384, 64483.8040000145277, 64484.6040000145398, 64485.2040000145428, 64485.4040000145468, 64486.1040000145400, 64486.2040000145474, 64486.3040000145416, 64486.4040000145344, 64486.7040000145370, 64487.2040000145408, 64487.5040000145365, 64488.1040000145422, 64488.2040000145461, 64488.3040000145423, 64488.6040000145471, 64489.0040000145521, 64489.8040000145418, 64491.1040000145393, 64491.9040000145398, 64492.7040000145421, 64493.3040000145442, 64493.6040000145405, 64495.3040000145285, 64495.5040000145417, 64495.8040000145283, 64496.1040000145365, 64497.1040000145327, 64497.2040000145390, 64497.7040000145403, 64497.9040000145387, 64498.2040000145372, 64498.3040000145326, 64498.4040000145303, 64498.6040000145281, 64498.8040000145372, 64500.1040000145490, 64500.4040000145383, 64500.5040000145405, 64502.1040000145432, 64502.7040000145453, 64502.9040000145402, 64503.4040000145387, 64503.7040000145439, 64503.9040000145372, 64504.1040000145385, 64504.9040000145410, 64505.4040000145348, 64505.6040000145409, 64506.2040000145471, 64506.9040000145376, 64507.1040000145369, 64507.2040000145322, 64507.3040000145460, 64507.6040000145376, 64507.7040000145385, 64508.3040000145360, 64508.4040000145340, 64508.5040000145274, 64508.6040000145366, 64508.8040000145386, 64508.9040000145378, 64509.2040000145367, 64509.3040000145408, 64509.9040000145410, 64510.1040000145411, 64510.4040000145433, 64510.6040000145347, 64510.9040000145388, 64511.0040000145328, 64511.1040000145409, 64511.3040000145404, 64511.8040000145345, 64511.9040000145400, 64512.0040000145402, 64512.1040000145424, 64512.4040000145389, 64512.9040000145438, 64513.7040000145532, 64514.0040000145500, 64514.3040000145434, 64514.7040000145490, 64514.9040000145429, 64515.3040000145490, 64517.5040000145454, 64521.7040000145432, 64521.8040000145441, 64522.0040000145396, 64522.1040000145273, 64522.6040000145457, 64523.1040000145420, 64523.2040000145358, 64524.2040000145424, 64525.9040000145483, 64526.0040000145395, 64526.6040000145432, 64527.1040000145458, 64528.0040000145429, 64530.4040000145472, 64531.2040000145433, 64532.2040000145399, 64534.0040000145408, 64534.7040000145473, 64535.9040000145419, 64536.6040000145523, 64536.7040000145419, 64537.0040000145525, 64537.3040000145428, 64538.1040000145390 };
        // ToF, 방위각, 고각 (Length:1237)
        double[,] testObservation_dataSet = { {0.05127506 ,148.03910000 ,32.05040000},
                                    {0.05127614 ,148.04330000 ,32.04790000},
                                    {0.05127830 ,148.05170000 ,32.04300000},
                                    {0.05129235 ,148.10630000 ,32.01090000},
                                    {0.05129560 ,148.11890000 ,32.00350000},
                                    {0.05129668 ,148.12310000 ,32.00100000},
                                    {0.05131835 ,148.20700000 ,31.95160000},
                                    {0.05136182 ,148.37430000 ,31.85260000},
                                    {0.05136617 ,148.39100000 ,31.84270000},
                                    {0.05136835 ,148.39940000 ,31.83780000},
                                    {0.05137816 ,148.43690000 ,31.81550000},
                                    {0.05138034 ,148.44530000 ,31.81050000},
                                    {0.05138253 ,148.45360000 ,31.80560000},
                                    {0.05138580 ,148.46610000 ,31.79810000},
                                    {0.05139344 ,148.49530000 ,31.78080000},
                                    {0.05139890 ,148.51610000 ,31.76840000},
                                    {0.05140109 ,148.52440000 ,31.76340000},
                                    {0.05140437 ,148.53690000 ,31.75590000},
                                    {0.05140984 ,148.55770000 ,31.74350000},
                                    {0.05141421 ,148.57430000 ,31.73360000},
                                    {0.05144489 ,148.69050000 ,31.66410000},
                                    {0.05145367 ,148.72370000 ,31.64420000},
                                    {0.05146466 ,148.76510000 ,31.61930000},
                                    {0.05147126 ,148.78990000 ,31.60440000},
                                    {0.05148006 ,148.82300000 ,31.58450000},
                                    {0.05149769 ,148.88910000 ,31.54460000},
                                    {0.05150762 ,148.92620000 ,31.52220000},
                                    {0.05150983 ,148.93450000 ,31.51720000},
                                    {0.05151314 ,148.94680000 ,31.50970000},
                                    {0.05151535 ,148.95510000 ,31.50470000},
                                    {0.05152529 ,148.99210000 ,31.48230000},
                                    {0.05152750 ,149.00040000 ,31.47730000},
                                    {0.05152861 ,149.00450000 ,31.47480000},
                                    {0.05153082 ,149.01270000 ,31.46980000},
                                    {0.05153303 ,149.02100000 ,31.46480000},
                                    {0.05153413 ,149.02510000 ,31.46230000},
                                    {0.05153524 ,149.02920000 ,31.45980000},
                                    {0.05153745 ,149.03740000 ,31.45480000},
                                    {0.05153967 ,149.04560000 ,31.44990000},
                                    {0.05154409 ,149.06210000 ,31.43990000},
                                    {0.05154520 ,149.06620000 ,31.43740000},
                                    {0.05154742 ,149.07440000 ,31.43240000},
                                    {0.05154963 ,149.08260000 ,31.42740000},
                                    {0.05155185 ,149.09080000 ,31.42240000},
                                    {0.05155296 ,149.09500000 ,31.41990000},
                                    {0.05155406 ,149.09910000 ,31.41740000},
                                    {0.05155517 ,149.10320000 ,31.41490000},
                                    {0.05155628 ,149.10730000 ,31.41240000},
                                    {0.05155961 ,149.11960000 ,31.40490000},
                                    {0.05156293 ,149.13190000 ,31.39740000},
                                    {0.05156737 ,149.14830000 ,31.38740000},
                                    {0.05156959 ,149.15650000 ,31.38240000},
                                    {0.05157292 ,149.16880000 ,31.37490000},
                                    {0.05157625 ,149.18110000 ,31.36740000},
                                    {0.05157847 ,149.18930000 ,31.36240000},
                                    {0.05157958 ,149.19340000 ,31.35990000},
                                    {0.05158402 ,149.20980000 ,31.34990000},
                                    {0.05158958 ,149.23030000 ,31.33740000},
                                    {0.05159292 ,149.24260000 ,31.32990000},
                                    {0.05159403 ,149.24670000 ,31.32740000},
                                    {0.05159625 ,149.25490000 ,31.32240000},
                                    {0.05159848 ,149.26300000 ,31.31740000},
                                    {0.05160070 ,149.27120000 ,31.31240000},
                                    {0.05160293 ,149.27940000 ,31.30740000},
                                    {0.05160404 ,149.28350000 ,31.30490000},
                                    {0.05160515 ,149.28760000 ,31.30240000},
                                    {0.05160627 ,149.29170000 ,31.29990000},
                                    {0.05160738 ,149.29580000 ,31.29740000},
                                    {0.05161184 ,149.31210000 ,31.28740000},
                                    {0.05161629 ,149.32850000 ,31.27740000},
                                    {0.05161741 ,149.33260000 ,31.27490000},
                                    {0.05161852 ,149.33660000 ,31.27240000},
                                    {0.05161964 ,149.34070000 ,31.26990000},
                                    {0.05162075 ,149.34480000 ,31.26740000},
                                    {0.05162186 ,149.34890000 ,31.26490000},
                                    {0.05162409 ,149.35710000 ,31.25990000},
                                    {0.05162521 ,149.36110000 ,31.25740000},
                                    {0.05162632 ,149.36520000 ,31.25490000},
                                    {0.05163079 ,149.38160000 ,31.24480000},
                                    {0.05163302 ,149.38970000 ,31.23980000},
                                    {0.05163637 ,149.40190000 ,31.23230000},
                                    {0.05163748 ,149.40600000 ,31.22980000},
                                    {0.05163860 ,149.41010000 ,31.22730000},
                                    {0.05163972 ,149.41420000 ,31.22480000},
                                    {0.05164083 ,149.41830000 ,31.22230000},
                                    {0.05164418 ,149.43050000 ,31.21480000},
                                    {0.05164753 ,149.44270000 ,31.20730000},
                                    {0.05165200 ,149.45900000 ,31.19720000},
                                    {0.05165312 ,149.46310000 ,31.19470000},
                                    {0.05165424 ,149.46720000 ,31.19220000},
                                    {0.05165536 ,149.47120000 ,31.18970000},
                                    {0.05165759 ,149.47940000 ,31.18470000},
                                    {0.05166431 ,149.50380000 ,31.16970000},
                                    {0.05166654 ,149.51190000 ,31.16460000},
                                    {0.05166990 ,149.52410000 ,31.15710000},
                                    {0.05167326 ,149.53630000 ,31.14960000},
                                    {0.05167438 ,149.54040000 ,31.14710000},
                                    {0.05167550 ,149.54450000 ,31.14460000},
                                    {0.05167886 ,149.55670000 ,31.13700000},
                                    {0.05168110 ,149.56480000 ,31.13200000},
                                    {0.05168895 ,149.59320000 ,31.11450000},
                                    {0.05169007 ,149.59730000 ,31.11190000},
                                    {0.05169119 ,149.60140000 ,31.10940000},
                                    {0.05169231 ,149.60540000 ,31.10690000},
                                    {0.05169568 ,149.61760000 ,31.09940000},
                                    {0.05169680 ,149.62170000 ,31.09690000},
                                    {0.05171589 ,149.69060000 ,31.05420000},
                                    {0.05172151 ,149.71080000 ,31.04160000},
                                    {0.05172601 ,149.72700000 ,31.03160000},
                                    {0.05172826 ,149.73510000 ,31.02650000},
                                    {0.05173164 ,149.74730000 ,31.01900000},
                                    {0.05173614 ,149.76350000 ,31.00890000},
                                    {0.05173727 ,149.76750000 ,31.00640000},
                                    {0.05174402 ,149.79180000 ,30.99130000},
                                    {0.05174628 ,149.79980000 ,30.98630000},
                                    {0.05174740 ,149.80390000 ,30.98380000},
                                    {0.05174853 ,149.80790000 ,30.98130000},
                                    {0.05175078 ,149.81600000 ,30.97620000},
                                    {0.05175304 ,149.82410000 ,30.97120000},
                                    {0.05175417 ,149.82810000 ,30.96870000},
                                    {0.05175642 ,149.83620000 ,30.96360000},
                                    {0.05175755 ,149.84020000 ,30.96110000},
                                    {0.05176093 ,149.85240000 ,30.95360000},
                                    {0.05176432 ,149.86450000 ,30.94600000},
                                    {0.05176658 ,149.87250000 ,30.94100000},
                                    {0.05176883 ,149.88060000 ,30.93600000},
                                    {0.05177335 ,149.89670000 ,30.92590000},
                                    {0.05177448 ,149.90080000 ,30.92340000},
                                    {0.05177561 ,149.90480000 ,30.92080000},
                                    {0.05177674 ,149.90880000 ,30.91830000},
                                    {0.05177787 ,149.91290000 ,30.91580000},
                                    {0.05178013 ,149.92090000 ,30.91080000},
                                    {0.05178352 ,149.93300000 ,30.90320000},
                                    {0.05178465 ,149.93700000 ,30.90070000},
                                    {0.05178917 ,149.95320000 ,30.89060000},
                                    {0.05179030 ,149.95720000 ,30.88810000},
                                    {0.05179144 ,149.96120000 ,30.88560000},
                                    {0.05179257 ,149.96520000 ,30.88310000},
                                    {0.05179483 ,149.97330000 ,30.87800000},
                                    {0.05180615 ,150.01350000 ,30.85280000},
                                    {0.05180955 ,150.02560000 ,30.84520000},
                                    {0.05181295 ,150.03760000 ,30.83770000},
                                    {0.05184585 ,150.15400000 ,30.76450000},
                                    {0.05184813 ,150.16210000 ,30.75950000},
                                    {0.05185040 ,150.17010000 ,30.75440000},
                                    {0.05185381 ,150.18210000 ,30.74680000},
                                    {0.05185722 ,150.19410000 ,30.73930000},
                                    {0.05185836 ,150.19810000 ,30.73670000},
                                    {0.05186063 ,150.20610000 ,30.73170000},
                                    {0.05186405 ,150.21810000 ,30.72410000},
                                    {0.05186519 ,150.22210000 ,30.72160000},
                                    {0.05187429 ,150.25410000 ,30.70140000},
                                    {0.05187657 ,150.26210000 ,30.69630000},
                                    {0.05187885 ,150.27010000 ,30.69120000},
                                    {0.05188341 ,150.28610000 ,30.68110000},
                                    {0.05188455 ,150.29010000 ,30.67860000},
                                    {0.05188569 ,150.29410000 ,30.67610000},
                                    {0.05188683 ,150.29810000 ,30.67350000},
                                    {0.05189139 ,150.31410000 ,30.66340000},
                                    {0.05190052 ,150.34600000 ,30.64320000},
                                    {0.05190394 ,150.35800000 ,30.63560000},
                                    {0.05190508 ,150.36200000 ,30.63310000},
                                    {0.05190851 ,150.37390000 ,30.62550000},
                                    {0.05190965 ,150.37790000 ,30.62300000},
                                    {0.05191308 ,150.38990000 ,30.61540000},
                                    {0.05191422 ,150.39390000 ,30.61280000},
                                    {0.05191651 ,150.40180000 ,30.60780000},
                                    {0.05191765 ,150.40580000 ,30.60520000},
                                    {0.05191994 ,150.41380000 ,30.60020000},
                                    {0.05192222 ,150.42170000 ,30.59510000},
                                    {0.05192565 ,150.43370000 ,30.58750000},
                                    {0.05193710 ,150.47350000 ,30.56220000},
                                    {0.05194168 ,150.48940000 ,30.55210000},
                                    {0.05194397 ,150.49740000 ,30.54700000},
                                    {0.05195084 ,150.52120000 ,30.53180000},
                                    {0.05195428 ,150.53310000 ,30.52420000},
                                    {0.05195772 ,150.54500000 ,30.51660000},
                                    {0.05196116 ,150.55700000 ,30.50900000},
                                    {0.05196231 ,150.56090000 ,30.50640000},
                                    {0.05196805 ,150.58080000 ,30.49380000},
                                    {0.05196919 ,150.58470000 ,30.49120000},
                                    {0.05197379 ,150.60060000 ,30.48110000},
                                    {0.05197493 ,150.60460000 ,30.47860000},
                                    {0.05197838 ,150.61650000 ,30.47090000},
                                    {0.05197953 ,150.62040000 ,30.46840000},
                                    {0.05198297 ,150.63230000 ,30.46080000},
                                    {0.05198412 ,150.63630000 ,30.45830000},
                                    {0.05198757 ,150.64820000 ,30.45070000},
                                    {0.05199102 ,150.66010000 ,30.44300000},
                                    {0.05199562 ,150.67590000 ,30.43290000},
                                    {0.05199792 ,150.68380000 ,30.42780000},
                                    {0.05200137 ,150.69570000 ,30.42020000},
                                    {0.05200252 ,150.69960000 ,30.41770000},
                                    {0.05200713 ,150.71550000 ,30.40750000},
                                    {0.05200828 ,150.71940000 ,30.40500000},
                                    {0.05200943 ,150.72340000 ,30.40240000},
                                    {0.05201173 ,150.73130000 ,30.39740000},
                                    {0.05201289 ,150.73520000 ,30.39480000},
                                    {0.05201404 ,150.73920000 ,30.39230000},
                                    {0.05201519 ,150.74320000 ,30.38970000},
                                    {0.05201634 ,150.74710000 ,30.38720000},
                                    {0.05201749 ,150.75110000 ,30.38470000},
                                    {0.05201865 ,150.75500000 ,30.38210000},
                                    {0.05201980 ,150.75900000 ,30.37960000},
                                    {0.05202787 ,150.78660000 ,30.36180000},
                                    {0.05203017 ,150.79450000 ,30.35670000},
                                    {0.05203248 ,150.80240000 ,30.35160000},
                                    {0.05203594 ,150.81420000 ,30.34400000},
                                    {0.05203710 ,150.81820000 ,30.34150000},
                                    {0.05203825 ,150.82210000 ,30.33890000},
                                    {0.05203940 ,150.82610000 ,30.33640000},
                                    {0.05204287 ,150.83790000 ,30.32880000},
                                    {0.05204749 ,150.85370000 ,30.31860000},
                                    {0.05205095 ,150.86550000 ,30.31100000},
                                    {0.05205442 ,150.87730000 ,30.30330000},
                                    {0.05205904 ,150.89310000 ,30.29320000},
                                    {0.05206020 ,150.89700000 ,30.29060000},
                                    {0.05206135 ,150.90100000 ,30.28810000},
                                    {0.05206598 ,150.91670000 ,30.27790000},
                                    {0.05206829 ,150.92460000 ,30.27280000},
                                    {0.05207755 ,150.95610000 ,30.25250000},
                                    {0.05207987 ,150.96390000 ,30.24740000},
                                    {0.05208566 ,150.98360000 ,30.23460000},
                                    {0.05208797 ,150.99150000 ,30.22960000},
                                    {0.05209261 ,151.00720000 ,30.21940000},
                                    {0.05209609 ,151.01890000 ,30.21170000},
                                    {0.05209957 ,151.03070000 ,30.20410000},
                                    {0.05210189 ,151.03860000 ,30.19900000},
                                    {0.05210305 ,151.04250000 ,30.19650000},
                                    {0.05210537 ,151.05040000 ,30.19140000},
                                    {0.05210769 ,151.05820000 ,30.18630000},
                                    {0.05210885 ,151.06210000 ,30.18370000},
                                    {0.05211581 ,151.08570000 ,30.16840000},
                                    {0.05213092 ,151.13660000 ,30.13530000},
                                    {0.05213673 ,151.15620000 ,30.12260000},
                                    {0.05213906 ,151.16400000 ,30.11750000},
                                    {0.05214022 ,151.16790000 ,30.11490000},
                                    {0.05214487 ,151.18360000 ,30.10470000},
                                    {0.05214837 ,151.19530000 ,30.09710000},
                                    {0.05214953 ,151.19920000 ,30.09450000},
                                    {0.05215069 ,151.20310000 ,30.09200000},
                                    {0.05215419 ,151.21480000 ,30.08430000},
                                    {0.05216118 ,151.23830000 ,30.06900000},
                                    {0.05216234 ,151.24220000 ,30.06650000},
                                    {0.05216817 ,151.26170000 ,30.05370000},
                                    {0.05217050 ,151.26950000 ,30.04860000},
                                    {0.05217167 ,151.27340000 ,30.04600000},
                                    {0.05217633 ,151.28900000 ,30.03580000},
                                    {0.05217983 ,151.30070000 ,30.02820000},
                                    {0.05219034 ,151.33580000 ,30.00520000},
                                    {0.05219151 ,151.33970000 ,30.00260000},
                                    {0.05219385 ,151.34750000 ,29.99750000},
                                    {0.05220086 ,151.37090000 ,29.98220000},
                                    {0.05220203 ,151.37480000 ,29.97970000},
                                    {0.05220319 ,151.37870000 ,29.97710000},
                                    {0.05220436 ,151.38260000 ,29.97450000},
                                    {0.05220904 ,151.39810000 ,29.96430000},
                                    {0.05221021 ,151.40200000 ,29.96180000},
                                    {0.05221255 ,151.40980000 ,29.95670000},
                                    {0.05221372 ,151.41370000 ,29.95410000},
                                    {0.05221489 ,151.41760000 ,29.95150000},
                                    {0.05221606 ,151.42150000 ,29.94900000},
                                    {0.05221723 ,151.42540000 ,29.94640000},
                                    {0.05221957 ,151.43310000 ,29.94130000},
                                    {0.05222074 ,151.43700000 ,29.93880000},
                                    {0.05222308 ,151.44480000 ,29.93370000},
                                    {0.05223245 ,151.47590000 ,29.91320000},
                                    {0.05223362 ,151.47980000 ,29.91060000},
                                    {0.05223480 ,151.48370000 ,29.90810000},
                                    {0.05223831 ,151.49530000 ,29.90040000},
                                    {0.05223948 ,151.49920000 ,29.89790000},
                                    {0.05224886 ,151.53020000 ,29.87740000},
                                    {0.05225590 ,151.55350000 ,29.86200000},
                                    {0.05225943 ,151.56510000 ,29.85440000},
                                    {0.05226060 ,151.56900000 ,29.85180000},
                                    {0.05226177 ,151.57290000 ,29.84930000},
                                    {0.05226295 ,151.57680000 ,29.84670000},
                                    {0.05226765 ,151.59220000 ,29.83650000},
                                    {0.05227235 ,151.60770000 ,29.82620000},
                                    {0.05227705 ,151.62320000 ,29.81600000},
                                    {0.05227940 ,151.63100000 ,29.81090000},
                                    {0.05228293 ,151.64260000 ,29.80320000},
                                    {0.05228528 ,151.65030000 ,29.79810000},
                                    {0.05228763 ,151.65800000 ,29.79290000},
                                    {0.05229116 ,151.66960000 ,29.78530000},
                                    {0.05230411 ,151.71210000 ,29.75710000},
                                    {0.05230529 ,151.71600000 ,29.75450000},
                                    {0.05230647 ,151.71990000 ,29.75190000},
                                    {0.05230883 ,151.72760000 ,29.74680000},
                                    {0.05231236 ,151.73920000 ,29.73910000},
                                    {0.05232415 ,151.77770000 ,29.71350000},
                                    {0.05232887 ,151.79320000 ,29.70320000},
                                    {0.05233123 ,151.80090000 ,29.69810000},
                                    {0.05233359 ,151.80860000 ,29.69300000},
                                    {0.05233950 ,151.82780000 ,29.68020000},
                                    {0.05234186 ,151.83550000 ,29.67500000},
                                    {0.05234422 ,151.84320000 ,29.66990000},
                                    {0.05234895 ,151.85860000 ,29.65960000},
                                    {0.05235131 ,151.86630000 ,29.65450000},
                                    {0.05235368 ,151.87400000 ,29.64940000},
                                    {0.05235486 ,151.87790000 ,29.64680000},
                                    {0.05235841 ,151.88940000 ,29.63910000},
                                    {0.05235959 ,151.89320000 ,29.63660000},
                                    {0.05236196 ,151.90090000 ,29.63140000},
                                    {0.05236432 ,151.90860000 ,29.62630000},
                                    {0.05237261 ,151.93550000 ,29.60830000},
                                    {0.05237734 ,151.95090000 ,29.59800000},
                                    {0.05237853 ,151.95470000 ,29.59550000},
                                    {0.05238208 ,151.96620000 ,29.58780000},
                                    {0.05238564 ,151.97770000 ,29.58010000},
                                    {0.05238682 ,151.98160000 ,29.57750000},
                                    {0.05239038 ,151.99310000 ,29.56980000},
                                    {0.05239275 ,152.00080000 ,29.56470000},
                                    {0.05239868 ,152.01990000 ,29.55180000},
                                    {0.05240105 ,152.02760000 ,29.54670000},
                                    {0.05240224 ,152.03140000 ,29.54410000},
                                    {0.05240342 ,152.03530000 ,29.54150000},
                                    {0.05240580 ,152.04290000 ,29.53640000},
                                    {0.05240936 ,152.05440000 ,29.52870000},
                                    {0.05241173 ,152.06210000 ,29.52360000},
                                    {0.05241292 ,152.06590000 ,29.52100000},
                                    {0.05242004 ,152.08890000 ,29.50560000},
                                    {0.05242123 ,152.09270000 ,29.50300000},
                                    {0.05242242 ,152.09650000 ,29.50040000},
                                    {0.05242361 ,152.10030000 ,29.49780000},
                                    {0.05242480 ,152.10420000 ,29.49530000},
                                    {0.05243431 ,152.13480000 ,29.47470000},
                                    {0.05243550 ,152.13860000 ,29.47210000},
                                    {0.05243669 ,152.14240000 ,29.46960000},
                                    {0.05243788 ,152.14620000 ,29.46700000},
                                    {0.05244026 ,152.15390000 ,29.46180000},
                                    {0.05244145 ,152.15770000 ,29.45930000},
                                    {0.05244383 ,152.16530000 ,29.45410000},
                                    {0.05245216 ,152.19200000 ,29.43610000},
                                    {0.05245335 ,152.19590000 ,29.43360000},
                                    {0.05245573 ,152.20350000 ,29.42840000},
                                    {0.05245692 ,152.20730000 ,29.42580000},
                                    {0.05246169 ,152.22260000 ,29.41550000},
                                    {0.05246288 ,152.22640000 ,29.41300000},
                                    {0.05246407 ,152.23020000 ,29.41040000},
                                    {0.05246526 ,152.23400000 ,29.40780000},
                                    {0.05246884 ,152.24540000 ,29.40010000},
                                    {0.05247003 ,152.24930000 ,29.39750000},
                                    {0.05247361 ,152.26070000 ,29.38980000},
                                    {0.05247599 ,152.26830000 ,29.38470000},
                                    {0.05247957 ,152.27970000 ,29.37690000},
                                    {0.05248077 ,152.28350000 ,29.37440000},
                                    {0.05248196 ,152.28740000 ,29.37180000},
                                    {0.05248315 ,152.29120000 ,29.36920000},
                                    {0.05248554 ,152.29880000 ,29.36410000},
                                    {0.05249270 ,152.32160000 ,29.34860000},
                                    {0.05250346 ,152.35580000 ,29.32540000},
                                    {0.05250585 ,152.36340000 ,29.32030000},
                                    {0.05251422 ,152.39000000 ,29.30220000},
                                    {0.05251661 ,152.39760000 ,29.29710000},
                                    {0.05252858 ,152.43560000 ,29.27130000},
                                    {0.05253457 ,152.45450000 ,29.25840000},
                                    {0.05253576 ,152.45830000 ,29.25580000},
                                    {0.05253696 ,152.46210000 ,29.25320000},
                                    {0.05253816 ,152.46590000 ,29.25070000},
                                    {0.05254415 ,152.48490000 ,29.23780000},
                                    {0.05254895 ,152.50000000 ,29.22750000},
                                    {0.05255015 ,152.50380000 ,29.22490000},
                                    {0.05255854 ,152.53030000 ,29.20680000},
                                    {0.05256334 ,152.54550000 ,29.19650000},
                                    {0.05257175 ,152.57190000 ,29.17840000},
                                    {0.05257775 ,152.59080000 ,29.16550000},
                                    {0.05257895 ,152.59460000 ,29.16290000},
                                    {0.05258256 ,152.60600000 ,29.15520000},
                                    {0.05259218 ,152.63620000 ,29.13450000},
                                    {0.05259579 ,152.64750000 ,29.12680000},
                                    {0.05259940 ,152.65880000 ,29.11910000},
                                    {0.05261143 ,152.69650000 ,29.09320000},
                                    {0.05261264 ,152.70030000 ,29.09060000},
                                    {0.05261746 ,152.71540000 ,29.08030000},
                                    {0.05261866 ,152.71910000 ,29.07770000},
                                    {0.05265727 ,152.83950000 ,28.99500000},
                                    {0.05266452 ,152.86200000 ,28.97950000},
                                    {0.05266815 ,152.87330000 ,28.97170000},
                                    {0.05266936 ,152.87700000 ,28.96910000},
                                    {0.05267661 ,152.89960000 ,28.95360000},
                                    {0.05267782 ,152.90330000 ,28.95100000},
                                    {0.05269356 ,152.95200000 ,28.91740000},
                                    {0.05269477 ,152.95580000 ,28.91480000},
                                    {0.05269719 ,152.96330000 ,28.90960000},
                                    {0.05269841 ,152.96700000 ,28.90700000},
                                    {0.05269962 ,152.97070000 ,28.90440000},
                                    {0.05270568 ,152.98950000 ,28.89150000},
                                    {0.05270689 ,152.99320000 ,28.88890000},
                                    {0.05270931 ,153.00070000 ,28.88370000},
                                    {0.05271053 ,153.00440000 ,28.88110000},
                                    {0.05271659 ,153.02310000 ,28.86820000},
                                    {0.05271780 ,153.02690000 ,28.86560000},
                                    {0.05273480 ,153.07920000 ,28.82930000},
                                    {0.05274087 ,153.09780000 ,28.81630000},
                                    {0.05274209 ,153.10160000 ,28.81380000},
                                    {0.05274816 ,153.12020000 ,28.80080000},
                                    {0.05274938 ,153.12390000 ,28.79820000},
                                    {0.05275668 ,153.14630000 ,28.78260000},
                                    {0.05277737 ,153.20960000 ,28.73860000},
                                    {0.05278346 ,153.22820000 ,28.72560000},
                                    {0.05279687 ,153.26910000 ,28.69700000},
                                    {0.05280541 ,153.29510000 ,28.67890000},
                                    {0.05281518 ,153.32480000 ,28.65810000},
                                    {0.05281762 ,153.33220000 ,28.65290000},
                                    {0.05281884 ,153.33590000 ,28.65030000},
                                    {0.05282373 ,153.35080000 ,28.63990000},
                                    {0.05282739 ,153.36190000 ,28.63210000},
                                    {0.05283106 ,153.37300000 ,28.62440000},
                                    {0.05288249 ,153.52840000 ,28.51520000},
                                    {0.05288617 ,153.53940000 ,28.50740000},
                                    {0.05288740 ,153.54310000 ,28.50480000},
                                    {0.05288863 ,153.54680000 ,28.50220000},
                                    {0.05290459 ,153.59480000 ,28.46840000},
                                    {0.05290827 ,153.60580000 ,28.46060000},
                                    {0.05291073 ,153.61320000 ,28.45540000},
                                    {0.05291687 ,153.63160000 ,28.44240000},
                                    {0.05292548 ,153.65740000 ,28.42420000},
                                    {0.05292794 ,153.66470000 ,28.41900000},
                                    {0.05292917 ,153.66840000 ,28.41640000},
                                    {0.05293040 ,153.67210000 ,28.41380000},
                                    {0.05293533 ,153.68680000 ,28.40330000},
                                    {0.05294518 ,153.71620000 ,28.38250000},
                                    {0.05295010 ,153.73090000 ,28.37210000},
                                    {0.05295134 ,153.73450000 ,28.36950000},
                                    {0.05296860 ,153.78590000 ,28.33300000},
                                    {0.05297600 ,153.80790000 ,28.31740000},
                                    {0.05298835 ,153.84460000 ,28.29130000},
                                    {0.05298958 ,153.84820000 ,28.28870000},
                                    {0.05299452 ,153.86290000 ,28.27830000},
                                    {0.05300194 ,153.88480000 ,28.26270000},
                                    {0.05301678 ,153.92870000 ,28.23140000},
                                    {0.05302049 ,153.93960000 ,28.22360000},
                                    {0.05302792 ,153.96160000 ,28.20790000},
                                    {0.05303535 ,153.98350000 ,28.19230000},
                                    {0.05305022 ,154.02720000 ,28.16100000},
                                    {0.05305394 ,154.03820000 ,28.15310000},
                                    {0.05306635 ,154.07460000 ,28.12700000},
                                    {0.05306759 ,154.07820000 ,28.12440000},
                                    {0.05306883 ,154.08190000 ,28.12180000},
                                    {0.05307877 ,154.11100000 ,28.10090000},
                                    {0.05308125 ,154.11820000 ,28.09570000},
                                    {0.05308622 ,154.13280000 ,28.08530000},
                                    {0.05309866 ,154.16910000 ,28.05910000},
                                    {0.05310737 ,154.19450000 ,28.04090000},
                                    {0.05312355 ,154.24170000 ,28.00690000},
                                    {0.05312480 ,154.24530000 ,28.00430000},
                                    {0.05312729 ,154.25250000 ,27.99900000},
                                    {0.05312854 ,154.25620000 ,27.99640000},
                                    {0.05312978 ,154.25980000 ,27.99380000},
                                    {0.05313602 ,154.27790000 ,27.98080000},
                                    {0.05313851 ,154.28510000 ,27.97550000},
                                    {0.05313976 ,154.28880000 ,27.97290000},
                                    {0.05314350 ,154.29960000 ,27.96510000},
                                    {0.05314849 ,154.31410000 ,27.95460000},
                                    {0.05315223 ,154.32490000 ,27.94680000},
                                    {0.05315473 ,154.33220000 ,27.94150000},
                                    {0.05315598 ,154.33580000 ,27.93890000},
                                    {0.05315723 ,154.33940000 ,27.93630000},
                                    {0.05315972 ,154.34660000 ,27.93110000},
                                    {0.05316347 ,154.35750000 ,27.92320000},
                                    {0.05316722 ,154.36830000 ,27.91540000},
                                    {0.05316971 ,154.37560000 ,27.91020000},
                                    {0.05317346 ,154.38640000 ,27.90230000},
                                    {0.05317846 ,154.40080000 ,27.89180000},
                                    {0.05317971 ,154.40440000 ,27.88920000},
                                    {0.05318221 ,154.41170000 ,27.88400000},
                                    {0.05318346 ,154.41530000 ,27.88140000},
                                    {0.05318471 ,154.41890000 ,27.87880000},
                                    {0.05318596 ,154.42250000 ,27.87610000},
                                    {0.05318721 ,154.42610000 ,27.87350000},
                                    {0.05319472 ,154.44770000 ,27.85780000},
                                    {0.05319722 ,154.45490000 ,27.85260000},
                                    {0.05319847 ,154.45850000 ,27.85000000},
                                    {0.05319973 ,154.46210000 ,27.84740000},
                                    {0.05320098 ,154.46570000 ,27.84470000},
                                    {0.05320849 ,154.48740000 ,27.82900000},
                                    {0.05321225 ,154.49820000 ,27.82120000},
                                    {0.05321350 ,154.50180000 ,27.81860000},
                                    {0.05321475 ,154.50540000 ,27.81590000},
                                    {0.05321851 ,154.51620000 ,27.80810000},
                                    {0.05321977 ,154.51980000 ,27.80550000},
                                    {0.05322102 ,154.52340000 ,27.80290000},
                                    {0.05322729 ,154.54140000 ,27.78980000},
                                    {0.05323230 ,154.55570000 ,27.77930000},
                                    {0.05324234 ,154.58450000 ,27.75830000},
                                    {0.05324359 ,154.58810000 ,27.75570000},
                                    {0.05324736 ,154.59890000 ,27.74790000},
                                    {0.05325113 ,154.60960000 ,27.74000000},
                                    {0.05325238 ,154.61320000 ,27.73740000},
                                    {0.05325615 ,154.62400000 ,27.72950000},
                                    {0.05325741 ,154.62760000 ,27.72690000},
                                    {0.05326117 ,154.63830000 ,27.71900000},
                                    {0.05326243 ,154.64190000 ,27.71640000},
                                    {0.05326494 ,154.64910000 ,27.71120000},
                                    {0.05326746 ,154.65630000 ,27.70590000},
                                    {0.05327123 ,154.66700000 ,27.69810000},
                                    {0.05327626 ,154.68140000 ,27.68760000},
                                    {0.05328003 ,154.69210000 ,27.67970000},
                                    {0.05328632 ,154.71000000 ,27.66660000},
                                    {0.05329513 ,154.73510000 ,27.64830000},
                                    {0.05329639 ,154.73870000 ,27.64570000},
                                    {0.05330143 ,154.75300000 ,27.63520000},
                                    {0.05330269 ,154.75660000 ,27.63250000},
                                    {0.05330521 ,154.76370000 ,27.62730000},
                                    {0.05330647 ,154.76730000 ,27.62470000},
                                    {0.05330773 ,154.77090000 ,27.62210000},
                                    {0.05330899 ,154.77440000 ,27.61940000},
                                    {0.05331277 ,154.78520000 ,27.61160000},
                                    {0.05332285 ,154.81370000 ,27.59060000},
                                    {0.05333042 ,154.83520000 ,27.57480000},
                                    {0.05333673 ,154.85300000 ,27.56170000},
                                    {0.05333926 ,154.86010000 ,27.55650000},
                                    {0.05334557 ,154.87800000 ,27.54340000},
                                    {0.05334810 ,154.88510000 ,27.53810000},
                                    {0.05334936 ,154.88870000 ,27.53550000},
                                    {0.05336326 ,154.92780000 ,27.50660000},
                                    {0.05336579 ,154.93500000 ,27.50140000},
                                    {0.05336705 ,154.93850000 ,27.49870000},
                                    {0.05336832 ,154.94210000 ,27.49610000},
                                    {0.05337085 ,154.94920000 ,27.49090000},
                                    {0.05337211 ,154.95280000 ,27.48820000},
                                    {0.05337844 ,154.97060000 ,27.47510000},
                                    {0.05337970 ,154.97410000 ,27.47250000},
                                    {0.05338223 ,154.98120000 ,27.46720000},
                                    {0.05338603 ,154.99190000 ,27.45940000},
                                    {0.05338983 ,155.00250000 ,27.45150000},
                                    {0.05339109 ,155.00610000 ,27.44880000},
                                    {0.05339616 ,155.02030000 ,27.43830000},
                                    {0.05339743 ,155.02390000 ,27.43570000},
                                    {0.05339869 ,155.02740000 ,27.43310000},
                                    {0.05339996 ,155.03100000 ,27.43050000},
                                    {0.05340123 ,155.03450000 ,27.42780000},
                                    {0.05341390 ,155.07000000 ,27.40160000},
                                    {0.05341517 ,155.07360000 ,27.39890000},
                                    {0.05341644 ,155.07710000 ,27.39630000},
                                    {0.05342024 ,155.08770000 ,27.38840000},
                                    {0.05342786 ,155.10900000 ,27.37270000},
                                    {0.05343420 ,155.12670000 ,27.35950000},
                                    {0.05343547 ,155.13030000 ,27.35690000},
                                    {0.05343928 ,155.14090000 ,27.34900000},
                                    {0.05344563 ,155.15860000 ,27.33590000},
                                    {0.05344690 ,155.16210000 ,27.33320000},
                                    {0.05345071 ,155.17270000 ,27.32530000},
                                    {0.05345326 ,155.17980000 ,27.32010000},
                                    {0.05345580 ,155.18690000 ,27.31480000},
                                    {0.05345707 ,155.19040000 ,27.31220000},
                                    {0.05346088 ,155.20100000 ,27.30430000},
                                    {0.05346216 ,155.20460000 ,27.30170000},
                                    {0.05346470 ,155.21160000 ,27.29640000},
                                    {0.05346979 ,155.22580000 ,27.28590000},
                                    {0.05347615 ,155.24340000 ,27.27280000},
                                    {0.05348251 ,155.26110000 ,27.25960000},
                                    {0.05348888 ,155.27870000 ,27.24650000},
                                    {0.05349398 ,155.29280000 ,27.23590000},
                                    {0.05349652 ,155.29990000 ,27.23070000},
                                    {0.05350162 ,155.31400000 ,27.22010000},
                                    {0.05350290 ,155.31750000 ,27.21750000},
                                    {0.05350799 ,155.33160000 ,27.20700000},
                                    {0.05351054 ,155.33870000 ,27.20170000},
                                    {0.05351182 ,155.34220000 ,27.19910000},
                                    {0.05351437 ,155.34920000 ,27.19380000},
                                    {0.05352075 ,155.36680000 ,27.18070000},
                                    {0.05352841 ,155.38790000 ,27.16490000},
                                    {0.05352968 ,155.39150000 ,27.16220000},
                                    {0.05353607 ,155.40900000 ,27.14910000},
                                    {0.05354118 ,155.42310000 ,27.13850000},
                                    {0.05354373 ,155.43010000 ,27.13330000},
                                    {0.05354757 ,155.44070000 ,27.12540000},
                                    {0.05354884 ,155.44420000 ,27.12270000},
                                    {0.05355268 ,155.45470000 ,27.11480000},
                                    {0.05355396 ,155.45820000 ,27.11220000},
                                    {0.05355907 ,155.47230000 ,27.10170000},
                                    {0.05356802 ,155.49680000 ,27.08320000},
                                    {0.05357442 ,155.51440000 ,27.07010000},
                                    {0.05357954 ,155.52840000 ,27.05950000},
                                    {0.05358082 ,155.53190000 ,27.05690000},
                                    {0.05358210 ,155.53540000 ,27.05430000},
                                    {0.05358851 ,155.55290000 ,27.04110000},
                                    {0.05359107 ,155.55990000 ,27.03580000},
                                    {0.05359235 ,155.56340000 ,27.03320000},
                                    {0.05359491 ,155.57040000 ,27.02790000},
                                    {0.05360004 ,155.58440000 ,27.01740000},
                                    {0.05360260 ,155.59140000 ,27.01210000},
                                    {0.05360645 ,155.60190000 ,27.00420000},
                                    {0.05360773 ,155.60540000 ,27.00160000},
                                    {0.05361928 ,155.63690000 ,26.97780000},
                                    {0.05362184 ,155.64390000 ,26.97260000},
                                    {0.05362313 ,155.64740000 ,26.96990000},
                                    {0.05363083 ,155.66830000 ,26.95410000},
                                    {0.05363211 ,155.67180000 ,26.95150000},
                                    {0.05363340 ,155.67530000 ,26.94880000},
                                    {0.05363468 ,155.67880000 ,26.94620000},
                                    {0.05363597 ,155.68230000 ,26.94360000},
                                    {0.05363982 ,155.69280000 ,26.93570000},
                                    {0.05364111 ,155.69620000 ,26.93300000},
                                    {0.05364239 ,155.69970000 ,26.93040000},
                                    {0.05364368 ,155.70320000 ,26.92770000},
                                    {0.05365139 ,155.72420000 ,26.91190000},
                                    {0.05365267 ,155.72760000 ,26.90930000},
                                    {0.05365910 ,155.74510000 ,26.89610000},
                                    {0.05366296 ,155.75550000 ,26.88820000},
                                    {0.05366682 ,155.76600000 ,26.88030000},
                                    {0.05366811 ,155.76950000 ,26.87760000},
                                    {0.05367068 ,155.77640000 ,26.87230000},
                                    {0.05367326 ,155.78340000 ,26.86710000},
                                    {0.05367712 ,155.79380000 ,26.85920000},
                                    {0.05368098 ,155.80430000 ,26.85120000},
                                    {0.05368742 ,155.82160000 ,26.83800000},
                                    {0.05369386 ,155.83900000 ,26.82480000},
                                    {0.05369773 ,155.84950000 ,26.81690000},
                                    {0.05370675 ,155.87380000 ,26.79850000},
                                    {0.05371707 ,155.90150000 ,26.77730000},
                                    {0.05371836 ,155.90500000 ,26.77470000},
                                    {0.05372740 ,155.92930000 ,26.75620000},
                                    {0.05372998 ,155.93620000 ,26.75090000},
                                    {0.05373773 ,155.95700000 ,26.73510000},
                                    {0.05373902 ,155.96050000 ,26.73240000},
                                    {0.05374160 ,155.96740000 ,26.72720000},
                                    {0.05374548 ,155.97780000 ,26.71920000},
                                    {0.05374677 ,155.98130000 ,26.71660000},
                                    {0.05374936 ,155.98820000 ,26.71130000},
                                    {0.05375453 ,156.00200000 ,26.70070000},
                                    {0.05375712 ,156.00900000 ,26.69550000},
                                    {0.05375841 ,156.01240000 ,26.69280000},
                                    {0.05376488 ,156.02970000 ,26.67960000},
                                    {0.05376876 ,156.04010000 ,26.67170000},
                                    {0.05377005 ,156.04360000 ,26.66900000},
                                    {0.05377135 ,156.04700000 ,26.66640000},
                                    {0.05377264 ,156.05050000 ,26.66380000},
                                    {0.05377523 ,156.05740000 ,26.65850000},
                                    {0.05377652 ,156.06080000 ,26.65580000},
                                    {0.05377911 ,156.06770000 ,26.65050000},
                                    {0.05378300 ,156.07810000 ,26.64260000},
                                    {0.05378429 ,156.08160000 ,26.64000000},
                                    {0.05378559 ,156.08500000 ,26.63730000},
                                    {0.05378688 ,156.08850000 ,26.63470000},
                                    {0.05378818 ,156.09190000 ,26.63200000},
                                    {0.05378947 ,156.09540000 ,26.62940000},
                                    {0.05379077 ,156.09880000 ,26.62680000},
                                    {0.05379207 ,156.10230000 ,26.62410000},
                                    {0.05379725 ,156.11610000 ,26.61350000},
                                    {0.05379855 ,156.11950000 ,26.61090000},
                                    {0.05379984 ,156.12300000 ,26.60830000},
                                    {0.05380114 ,156.12640000 ,26.60560000},
                                    {0.05380632 ,156.14020000 ,26.59500000},
                                    {0.05382319 ,156.18500000 ,26.56070000},
                                    {0.05382708 ,156.19530000 ,26.55270000},
                                    {0.05383098 ,156.20570000 ,26.54480000},
                                    {0.05383747 ,156.22290000 ,26.53160000},
                                    {0.05384267 ,156.23660000 ,26.52100000},
                                    {0.05384657 ,156.24700000 ,26.51310000},
                                    {0.05385047 ,156.25730000 ,26.50510000},
                                    {0.05385306 ,156.26410000 ,26.49980000},
                                    {0.05385567 ,156.27100000 ,26.49460000},
                                    {0.05385697 ,156.27440000 ,26.49190000},
                                    {0.05385957 ,156.28130000 ,26.48660000},
                                    {0.05386217 ,156.28820000 ,26.48130000},
                                    {0.05386347 ,156.29160000 ,26.47870000},
                                    {0.05386737 ,156.30190000 ,26.47070000},
                                    {0.05386867 ,156.30540000 ,26.46810000},
                                    {0.05387127 ,156.31220000 ,26.46280000},
                                    {0.05387258 ,156.31570000 ,26.46020000},
                                    {0.05387388 ,156.31910000 ,26.45750000},
                                    {0.05387518 ,156.32250000 ,26.45490000},
                                    {0.05388039 ,156.33620000 ,26.44430000},
                                    {0.05388950 ,156.36020000 ,26.42580000},
                                    {0.05389732 ,156.38080000 ,26.40990000},
                                    {0.05390384 ,156.39790000 ,26.39660000},
                                    {0.05390514 ,156.40130000 ,26.39400000},
                                    {0.05390644 ,156.40480000 ,26.39130000},
                                    {0.05391036 ,156.41500000 ,26.38340000},
                                    {0.05391427 ,156.42530000 ,26.37550000},
                                    {0.05391688 ,156.43210000 ,26.37020000},
                                    {0.05392079 ,156.44240000 ,26.36220000},
                                    {0.05392210 ,156.44580000 ,26.35960000},
                                    {0.05392340 ,156.44920000 ,26.35690000},
                                    {0.05392601 ,156.45610000 ,26.35160000},
                                    {0.05392862 ,156.46290000 ,26.34630000},
                                    {0.05394299 ,156.50050000 ,26.31720000},
                                    {0.05394429 ,156.50390000 ,26.31460000},
                                    {0.05394560 ,156.50730000 ,26.31190000},
                                    {0.05396128 ,156.54830000 ,26.28010000},
                                    {0.05396651 ,156.56190000 ,26.26950000},
                                    {0.05396913 ,156.56870000 ,26.26420000},
                                    {0.05397306 ,156.57900000 ,26.25630000},
                                    {0.05398484 ,156.60960000 ,26.23240000},
                                    {0.05398615 ,156.61300000 ,26.22980000},
                                    {0.05399007 ,156.62320000 ,26.22180000},
                                    {0.05399662 ,156.64020000 ,26.20860000},
                                    {0.05399793 ,156.64360000 ,26.20590000},
                                    {0.05399924 ,156.64700000 ,26.20330000},
                                    {0.05400186 ,156.65380000 ,26.19800000},
                                    {0.05400317 ,156.65720000 ,26.19530000},
                                    {0.05400580 ,156.66400000 ,26.19000000},
                                    {0.05400842 ,156.67080000 ,26.18470000},
                                    {0.05403465 ,156.73880000 ,26.13170000},
                                    {0.05403597 ,156.74210000 ,26.12900000},
                                    {0.05404385 ,156.76250000 ,26.11310000},
                                    {0.05405436 ,156.78960000 ,26.09190000},
                                    {0.05405698 ,156.79640000 ,26.08660000},
                                    {0.05406224 ,156.80990000 ,26.07600000},
                                    {0.05408066 ,156.85730000 ,26.03890000},
                                    {0.05408197 ,156.86070000 ,26.03620000},
                                    {0.05408329 ,156.86410000 ,26.03360000},
                                    {0.05408460 ,156.86750000 ,26.03090000},
                                    {0.05409777 ,156.90120000 ,26.00440000},
                                    {0.05410040 ,156.90800000 ,25.99910000},
                                    {0.05410172 ,156.91140000 ,25.99640000},
                                    {0.05410436 ,156.91810000 ,25.99110000},
                                    {0.05410699 ,156.92490000 ,25.98580000},
                                    {0.05410963 ,156.93160000 ,25.98050000},
                                    {0.05411754 ,156.95190000 ,25.96460000},
                                    {0.05411886 ,156.95520000 ,25.96190000},
                                    {0.05412017 ,156.95860000 ,25.95920000},
                                    {0.05412545 ,156.97210000 ,25.94860000},
                                    {0.05412809 ,156.97880000 ,25.94330000},
                                    {0.05412941 ,156.98220000 ,25.94070000},
                                    {0.05413073 ,156.98560000 ,25.93800000},
                                    {0.05414788 ,157.02940000 ,25.90350000},
                                    {0.05415053 ,157.03610000 ,25.89820000},
                                    {0.05415317 ,157.04280000 ,25.89290000},
                                    {0.05415581 ,157.04960000 ,25.88760000},
                                    {0.05416109 ,157.06300000 ,25.87690000},
                                    {0.05416506 ,157.07310000 ,25.86900000},
                                    {0.05416770 ,157.07980000 ,25.86370000},
                                    {0.05417431 ,157.09660000 ,25.85040000},
                                    {0.05417564 ,157.10000000 ,25.84770000},
                                    {0.05417696 ,157.10330000 ,25.84510000},
                                    {0.05417828 ,157.10670000 ,25.84240000},
                                    {0.05417960 ,157.11010000 ,25.83980000},
                                    {0.05419151 ,157.14030000 ,25.81580000},
                                    {0.05419283 ,157.14360000 ,25.81320000},
                                    {0.05419416 ,157.14700000 ,25.81050000},
                                    {0.05419548 ,157.15030000 ,25.80790000},
                                    {0.05419681 ,157.15370000 ,25.80520000},
                                    {0.05420078 ,157.16380000 ,25.79720000},
                                    {0.05421270 ,157.19390000 ,25.77330000},
                                    {0.05421800 ,157.20730000 ,25.76270000},
                                    {0.05422065 ,157.21400000 ,25.75740000},
                                    {0.05422197 ,157.21740000 ,25.75470000},
                                    {0.05423126 ,157.24080000 ,25.73610000},
                                    {0.05423391 ,157.24750000 ,25.73080000},
                                    {0.05423523 ,157.25080000 ,25.72820000},
                                    {0.05424718 ,157.28090000 ,25.70420000},
                                    {0.05424983 ,157.28760000 ,25.69890000},
                                    {0.05426045 ,157.31430000 ,25.67760000},
                                    {0.05426178 ,157.31770000 ,25.67500000},
                                    {0.05426311 ,157.32100000 ,25.67230000},
                                    {0.05426444 ,157.32440000 ,25.66970000},
                                    {0.05426577 ,157.32770000 ,25.66700000},
                                    {0.05426842 ,157.33440000 ,25.66170000},
                                    {0.05426975 ,157.33770000 ,25.65900000},
                                    {0.05427108 ,157.34100000 ,25.65640000},
                                    {0.05427241 ,157.34440000 ,25.65370000},
                                    {0.05427906 ,157.36110000 ,25.64040000},
                                    {0.05428172 ,157.36770000 ,25.63510000},
                                    {0.05429368 ,157.39770000 ,25.61120000},
                                    {0.05429768 ,157.40770000 ,25.60320000},
                                    {0.05430300 ,157.42110000 ,25.59250000},
                                    {0.05433364 ,157.49760000 ,25.53130000},
                                    {0.05434030 ,157.51420000 ,25.51800000},
                                    {0.05434831 ,157.53410000 ,25.50210000},
                                    {0.05435364 ,157.54740000 ,25.49140000},
                                    {0.05435498 ,157.55070000 ,25.48880000},
                                    {0.05435765 ,157.55730000 ,25.48340000},
                                    {0.05436165 ,157.56730000 ,25.47550000},
                                    {0.05436298 ,157.57060000 ,25.47280000},
                                    {0.05438302 ,157.62030000 ,25.43290000},
                                    {0.05438569 ,157.62690000 ,25.42750000},
                                    {0.05438837 ,157.63350000 ,25.42220000},
                                    {0.05439238 ,157.64350000 ,25.41420000},
                                    {0.05439371 ,157.64680000 ,25.41160000},
                                    {0.05439772 ,157.65670000 ,25.40360000},
                                    {0.05440040 ,157.66330000 ,25.39830000},
                                    {0.05440174 ,157.66660000 ,25.39560000},
                                    {0.05440441 ,157.67320000 ,25.39030000},
                                    {0.05440709 ,157.67980000 ,25.38490000},
                                    {0.05440843 ,157.68310000 ,25.38230000},
                                    {0.05440976 ,157.68650000 ,25.37960000},
                                    {0.05441110 ,157.68980000 ,25.37700000},
                                    {0.05441244 ,157.69310000 ,25.37430000},
                                    {0.05442047 ,157.71290000 ,25.35830000},
                                    {0.05442984 ,157.73600000 ,25.33970000},
                                    {0.05444458 ,157.77230000 ,25.31040000},
                                    {0.05444861 ,157.78220000 ,25.30240000},
                                    {0.05445263 ,157.79210000 ,25.29440000},
                                    {0.05445397 ,157.79540000 ,25.29170000},
                                    {0.05445799 ,157.80530000 ,25.28370000},
                                    {0.05446202 ,157.81510000 ,25.27570000},
                                    {0.05446604 ,157.82500000 ,25.26770000},
                                    {0.05447141 ,157.83820000 ,25.25710000},
                                    {0.05448618 ,157.87440000 ,25.22780000},
                                    {0.05449424 ,157.89410000 ,25.21180000},
                                    {0.05450230 ,157.91380000 ,25.19580000},
                                    {0.05451440 ,157.94340000 ,25.17180000},
                                    {0.05451978 ,157.95650000 ,25.16110000},
                                    {0.05452920 ,157.97940000 ,25.14250000},
                                    {0.05453728 ,157.99910000 ,25.12650000},
                                    {0.05453863 ,158.00240000 ,25.12380000},
                                    {0.05457097 ,158.08090000 ,25.05980000},
                                    {0.05458041 ,158.10380000 ,25.04110000},
                                    {0.05458311 ,158.11030000 ,25.03580000},
                                    {0.05458716 ,158.12010000 ,25.02780000},
                                    {0.05459391 ,158.13650000 ,25.01440000},
                                    {0.05460336 ,158.15930000 ,24.99580000},
                                    {0.05460471 ,158.16260000 ,24.99310000},
                                    {0.05460876 ,158.17230000 ,24.98510000},
                                    {0.05461011 ,158.17560000 ,24.98240000},
                                    {0.05462093 ,158.20170000 ,24.96110000},
                                    {0.05462228 ,158.20490000 ,24.95840000},
                                    {0.05462769 ,158.21800000 ,24.94770000},
                                    {0.05463174 ,158.22770000 ,24.93970000},
                                    {0.05463580 ,158.23750000 ,24.93170000},
                                    {0.05463851 ,158.24400000 ,24.92640000},
                                    {0.05464527 ,158.26030000 ,24.91300000},
                                    {0.05464663 ,158.26350000 ,24.91040000},
                                    {0.05464798 ,158.26680000 ,24.90770000},
                                    {0.05465069 ,158.27330000 ,24.90240000},
                                    {0.05465204 ,158.27650000 ,24.89970000},
                                    {0.05465746 ,158.28950000 ,24.88900000},
                                    {0.05466017 ,158.29600000 ,24.88370000},
                                    {0.05466423 ,158.30580000 ,24.87570000},
                                    {0.05466558 ,158.30900000 ,24.87300000},
                                    {0.05468049 ,158.34480000 ,24.84360000},
                                    {0.05468320 ,158.35120000 ,24.83830000},
                                    {0.05468456 ,158.35450000 ,24.83560000},
                                    {0.05468998 ,158.36750000 ,24.82490000},
                                    {0.05469134 ,158.37070000 ,24.82230000},
                                    {0.05469405 ,158.37720000 ,24.81690000},
                                    {0.05469677 ,158.38370000 ,24.81160000},
                                    {0.05471305 ,158.42250000 ,24.77950000},
                                    {0.05471441 ,158.42580000 ,24.77690000},
                                    {0.05471713 ,158.43230000 ,24.77150000},
                                    {0.05472120 ,158.44200000 ,24.76350000},
                                    {0.05472935 ,158.46140000 ,24.74750000},
                                    {0.05473207 ,158.46790000 ,24.74220000},
                                    {0.05473615 ,158.47760000 ,24.73410000},
                                    {0.05473751 ,158.48080000 ,24.73150000},
                                    {0.05474838 ,158.50660000 ,24.71010000},
                                    {0.05476063 ,158.53570000 ,24.68600000},
                                    {0.05476199 ,158.53890000 ,24.68340000},
                                    {0.05476743 ,158.55180000 ,24.67270000},
                                    {0.05476879 ,158.55510000 ,24.67000000},
                                    {0.05477288 ,158.56470000 ,24.66200000},
                                    {0.05477424 ,158.56800000 ,24.65930000},
                                    {0.05477696 ,158.57440000 ,24.65400000},
                                    {0.05478105 ,158.58410000 ,24.64600000},
                                    {0.05478377 ,158.59050000 ,24.64060000},
                                    {0.05479195 ,158.60990000 ,24.62460000},
                                    {0.05480694 ,158.64530000 ,24.59520000},
                                    {0.05480830 ,158.64850000 ,24.59250000},
                                    {0.05482195 ,158.68060000 ,24.56580000},
                                    {0.05485335 ,158.75450000 ,24.50430000},
                                    {0.05485745 ,158.76410000 ,24.49630000},
                                    {0.05486566 ,158.78340000 ,24.48020000},
                                    {0.05486839 ,158.78980000 ,24.47490000},
                                    {0.05487933 ,158.81540000 ,24.45350000},
                                    {0.05488207 ,158.82180000 ,24.44810000},
                                    {0.05488344 ,158.82500000 ,24.44550000},
                                    {0.05489439 ,158.85060000 ,24.42410000},
                                    {0.05489576 ,158.85380000 ,24.42140000},
                                    {0.05489713 ,158.85700000 ,24.41870000},
                                    {0.05489850 ,158.86020000 ,24.41600000},
                                    {0.05490261 ,158.86980000 ,24.40800000},
                                    {0.05490672 ,158.87940000 ,24.40000000},
                                    {0.05490946 ,158.88580000 ,24.39470000},
                                    {0.05491220 ,158.89220000 ,24.38930000},
                                    {0.05491768 ,158.90490000 ,24.37860000},
                                    {0.05492316 ,158.91770000 ,24.36790000},
                                    {0.05492453 ,158.92090000 ,24.36520000},
                                    {0.05492590 ,158.92410000 ,24.36250000},
                                    {0.05492727 ,158.92730000 ,24.35990000},
                                    {0.05493002 ,158.93370000 ,24.35450000},
                                    {0.05493139 ,158.93690000 ,24.35180000},
                                    {0.05493413 ,158.94330000 ,24.34650000},
                                    {0.05493687 ,158.94960000 ,24.34110000},
                                    {0.05493825 ,158.95280000 ,24.33850000},
                                    {0.05493962 ,158.95600000 ,24.33580000},
                                    {0.05494099 ,158.95920000 ,24.33310000},
                                    {0.05494373 ,158.96560000 ,24.32780000},
                                    {0.05494511 ,158.96880000 ,24.32510000},
                                    {0.05494648 ,158.97200000 ,24.32240000},
                                    {0.05496021 ,159.00380000 ,24.29570000},
                                    {0.05496982 ,159.02610000 ,24.27690000},
                                    {0.05497119 ,159.02930000 ,24.27420000},
                                    {0.05498081 ,159.05160000 ,24.25550000},
                                    {0.05498769 ,159.06750000 ,24.24210000},
                                    {0.05499044 ,159.07380000 ,24.23680000},
                                    {0.05499181 ,159.07700000 ,24.23410000},
                                    {0.05499594 ,159.08650000 ,24.22610000},
                                    {0.05499731 ,159.08970000 ,24.22340000},
                                    {0.05501382 ,159.12780000 ,24.19130000},
                                    {0.05501658 ,159.13420000 ,24.18590000},
                                    {0.05502071 ,159.14370000 ,24.17790000},
                                    {0.05502484 ,159.15320000 ,24.16990000},
                                    {0.05503310 ,159.17220000 ,24.15380000},
                                    {0.05506205 ,159.23870000 ,24.09760000},
                                    {0.05506481 ,159.24510000 ,24.09220000},
                                    {0.05510900 ,159.34610000 ,24.00650000},
                                    {0.05511592 ,159.36190000 ,23.99310000},
                                    {0.05512836 ,159.39030000 ,23.96900000},
                                    {0.05513805 ,159.41230000 ,23.95020000},
                                    {0.05515051 ,159.44060000 ,23.92610000},
                                    {0.05515743 ,159.45630000 ,23.91270000},
                                    {0.05516159 ,159.46580000 ,23.90470000},
                                    {0.05518100 ,159.50970000 ,23.86720000},
                                    {0.05518377 ,159.51600000 ,23.86180000},
                                    {0.05520042 ,159.55370000 ,23.82960000},
                                    {0.05520875 ,159.57250000 ,23.81360000},
                                    {0.05521291 ,159.58190000 ,23.80550000},
                                    {0.05522263 ,159.60380000 ,23.78680000},
                                    {0.05523514 ,159.63200000 ,23.76260000},
                                    {0.05524209 ,159.64760000 ,23.74920000},
                                    {0.05524348 ,159.65070000 ,23.74650000},
                                    {0.05525600 ,159.67890000 ,23.72240000},
                                    {0.05528383 ,159.74130000 ,23.66880000},
                                    {0.05529359 ,159.76310000 ,23.65000000},
                                    {0.05530473 ,159.78800000 ,23.62860000},
                                    {0.05531310 ,159.80670000 ,23.61250000},
                                    {0.05531449 ,159.80980000 ,23.60980000},
                                    {0.05531589 ,159.81290000 ,23.60710000},
                                    {0.05532286 ,159.82850000 ,23.59370000},
                                    {0.05532565 ,159.83470000 ,23.58830000},
                                    {0.05533961 ,159.86580000 ,23.56150000},
                                    {0.05535358 ,159.89680000 ,23.53470000},
                                    {0.05536475 ,159.92170000 ,23.51320000},
                                    {0.05537454 ,159.94340000 ,23.49440000},
                                    {0.05539552 ,159.98980000 ,23.45420000},
                                    {0.05539692 ,159.99290000 ,23.45150000},
                                    {0.05540112 ,160.00220000 ,23.44350000},
                                    {0.05540392 ,160.00840000 ,23.43810000},
                                    {0.05540812 ,160.01770000 ,23.43000000},
                                    {0.05540952 ,160.02080000 ,23.42730000},
                                    {0.05541792 ,160.03930000 ,23.41120000},
                                    {0.05544314 ,160.09490000 ,23.36290000},
                                    {0.05544595 ,160.10110000 ,23.35760000},
                                    {0.05544735 ,160.10420000 ,23.35490000},
                                    {0.05545296 ,160.11650000 ,23.34410000},
                                    {0.05545436 ,160.11960000 ,23.34150000},
                                    {0.05545577 ,160.12270000 ,23.33880000},
                                    {0.05546138 ,160.13500000 ,23.32800000},
                                    {0.05547261 ,160.15970000 ,23.30660000},
                                    {0.05547541 ,160.16580000 ,23.30120000},
                                    {0.05547822 ,160.17200000 ,23.29580000},
                                    {0.05548244 ,160.18120000 ,23.28780000},
                                    {0.05548524 ,160.18740000 ,23.28240000},
                                    {0.05548665 ,160.19050000 ,23.27970000},
                                    {0.05549367 ,160.20590000 ,23.26630000},
                                    {0.05550351 ,160.22740000 ,23.24750000},
                                    {0.05551195 ,160.24580000 ,23.23140000},
                                    {0.05551476 ,160.25200000 ,23.22600000},
                                    {0.05551757 ,160.25810000 ,23.22070000},
                                    {0.05551898 ,160.26120000 ,23.21800000},
                                    {0.05552179 ,160.26730000 ,23.21260000},
                                    {0.05552320 ,160.27040000 ,23.20990000},
                                    {0.05553023 ,160.28580000 ,23.19650000},
                                    {0.05553445 ,160.29500000 ,23.18840000},
                                    {0.05553586 ,160.29800000 ,23.18570000},
                                    {0.05554149 ,160.31030000 ,23.17500000},
                                    {0.05554290 ,160.31340000 ,23.17230000},
                                    {0.05554712 ,160.32260000 ,23.16430000},
                                    {0.05556121 ,160.35320000 ,23.13740000},
                                    {0.05556262 ,160.35630000 ,23.13470000},
                                    {0.05556544 ,160.36240000 ,23.12940000},
                                    {0.05558235 ,160.39910000 ,23.09710000},
                                    {0.05558658 ,160.40830000 ,23.08910000},
                                    {0.05559364 ,160.42360000 ,23.07560000},
                                    {0.05559646 ,160.42970000 ,23.07030000},
                                    {0.05560493 ,160.44800000 ,23.05410000},
                                    {0.05561904 ,160.47860000 ,23.02730000},
                                    {0.05562187 ,160.48470000 ,23.02190000},
                                    {0.05562752 ,160.49690000 ,23.01120000},
                                    {0.05563176 ,160.50600000 ,23.00310000},
                                    {0.05563882 ,160.52130000 ,22.98970000},
                                    {0.05564731 ,160.53960000 ,22.97360000},
                                    {0.05565013 ,160.54570000 ,22.96820000},
                                    {0.05565155 ,160.54870000 ,22.96550000},
                                    {0.05566003 ,160.56700000 ,22.94940000},
                                    {0.05566145 ,160.57000000 ,22.94670000},
                                    {0.05566286 ,160.57310000 ,22.94400000},
                                    {0.05566428 ,160.57610000 ,22.94130000},
                                    {0.05566994 ,160.58830000 ,22.93060000},
                                    {0.05567418 ,160.59740000 ,22.92250000},
                                    {0.05567843 ,160.60650000 ,22.91440000},
                                    {0.05568551 ,160.62170000 ,22.90100000},
                                    {0.05568693 ,160.62480000 ,22.89830000},
                                    {0.05569542 ,160.64300000 ,22.88220000},
                                    {0.05569826 ,160.64910000 ,22.87680000},
                                    {0.05570251 ,160.65820000 ,22.86880000},
                                    {0.05570392 ,160.66120000 ,22.86610000},
                                    {0.05571810 ,160.69150000 ,22.83920000},
                                    {0.05572235 ,160.70060000 ,22.83110000},
                                    {0.05572377 ,160.70370000 ,22.82840000},
                                    {0.05573370 ,160.72490000 ,22.80960000},
                                    {0.05574080 ,160.74000000 ,22.79620000},
                                    {0.05574789 ,160.75520000 ,22.78280000},
                                    {0.05575499 ,160.77030000 ,22.76930000},
                                    {0.05575783 ,160.77640000 ,22.76390000},
                                    {0.05577914 ,160.82170000 ,22.72360000},
                                    {0.05578056 ,160.82470000 ,22.72090000},
                                    {0.05578341 ,160.83080000 ,22.71560000},
                                    {0.05584176 ,160.95440000 ,22.60530000},
                                    {0.05587455 ,161.02350000 ,22.54350000},
                                    {0.05591738 ,161.11350000 ,22.46280000},
                                    {0.05593882 ,161.15840000 ,22.42250000},
                                    {0.05594311 ,161.16740000 ,22.41440000},
                                    {0.05594884 ,161.17930000 ,22.40360000},
                                    {0.05595742 ,161.19730000 ,22.38750000},
                                    {0.05595885 ,161.20030000 ,22.38480000},
                                    {0.05596028 ,161.20320000 ,22.38210000},
                                    {0.05596172 ,161.20620000 ,22.37940000},
                                    {0.05596315 ,161.20920000 ,22.37670000},
                                    {0.05596744 ,161.21820000 ,22.36870000},
                                    {0.05596887 ,161.22120000 ,22.36600000},
                                    {0.05597603 ,161.23610000 ,22.35250000},
                                    {0.05597747 ,161.23910000 ,22.34980000},
                                    {0.05597890 ,161.24210000 ,22.34710000},
                                    {0.05598176 ,161.24800000 ,22.34180000},
                                    {0.05598463 ,161.25400000 ,22.33640000},
                                    {0.05598606 ,161.25700000 ,22.33370000},
                                    {0.05598893 ,161.26290000 ,22.32830000},
                                    {0.05599179 ,161.26890000 ,22.32290000},
                                    {0.05599466 ,161.27490000 ,22.31760000},
                                    {0.05599752 ,161.28080000 ,22.31220000},
                                    {0.05599896 ,161.28380000 ,22.30950000},
                                    {0.05600039 ,161.28680000 ,22.30680000},
                                    {0.05600183 ,161.28980000 ,22.30410000},
                                    {0.05600469 ,161.29570000 ,22.29870000},
                                    {0.05600756 ,161.30170000 ,22.29330000},
                                    {0.05600899 ,161.30470000 ,22.29070000},
                                    {0.05601043 ,161.30760000 ,22.28800000},
                                    {0.05601903 ,161.32550000 ,22.27180000},
                                    {0.05603338 ,161.35520000 ,22.24490000},
                                    {0.05603482 ,161.35820000 ,22.24220000},
                                    {0.05603912 ,161.36710000 ,22.23420000},
                                    {0.05604343 ,161.37610000 ,22.22610000},
                                    {0.05604630 ,161.38200000 ,22.22070000},
                                    {0.05604918 ,161.38790000 ,22.21530000},
                                    {0.05605348 ,161.39690000 ,22.20720000},
                                    {0.05605492 ,161.39980000 ,22.20460000},
                                    {0.05605923 ,161.40870000 ,22.19650000},
                                    {0.05606210 ,161.41470000 ,22.19110000},
                                    {0.05606354 ,161.41760000 ,22.18840000},
                                    {0.05606785 ,161.42650000 ,22.18030000},
                                    {0.05606929 ,161.42950000 ,22.17760000},
                                    {0.05607360 ,161.43840000 ,22.16960000},
                                    {0.05607504 ,161.44140000 ,22.16690000},
                                    {0.05607648 ,161.44430000 ,22.16420000},
                                    {0.05607791 ,161.44730000 ,22.16150000},
                                    {0.05607935 ,161.45030000 ,22.15880000},
                                    {0.05608223 ,161.45620000 ,22.15340000},
                                    {0.05608367 ,161.45920000 ,22.15070000},
                                    {0.05608510 ,161.46210000 ,22.14810000},
                                    {0.05608654 ,161.46510000 ,22.14540000},
                                    {0.05609086 ,161.47400000 ,22.13730000},
                                    {0.05610525 ,161.50360000 ,22.11040000},
                                    {0.05611388 ,161.52140000 ,22.09420000},
                                    {0.05611820 ,161.53020000 ,22.08620000},
                                    {0.05612108 ,161.53620000 ,22.08080000},
                                    {0.05612540 ,161.54500000 ,22.07270000},
                                    {0.05612684 ,161.54800000 ,22.07000000},
                                    {0.05613404 ,161.56280000 ,22.05660000},
                                    {0.05613837 ,161.57160000 ,22.04850000},
                                    {0.05614125 ,161.57750000 ,22.04310000},
                                    {0.05614413 ,161.58340000 ,22.03770000},
                                    {0.05614846 ,161.59230000 ,22.02960000},
                                    {0.05615278 ,161.60120000 ,22.02160000},
                                    {0.05615422 ,161.60410000 ,22.01890000},
                                    {0.05615566 ,161.60710000 ,22.01620000},
                                    {0.05616287 ,161.62180000 ,22.00270000},
                                    {0.05616432 ,161.62480000 ,22.00000000},
                                    {0.05617009 ,161.63660000 ,21.98930000},
                                    {0.05617153 ,161.63950000 ,21.98660000},
                                    {0.05617442 ,161.64540000 ,21.98120000},
                                    {0.05618307 ,161.66310000 ,21.96500000},
                                    {0.05619029 ,161.67780000 ,21.95160000},
                                    {0.05619751 ,161.69250000 ,21.93810000},
                                    {0.05619896 ,161.69550000 ,21.93540000},
                                    {0.05620040 ,161.69840000 ,21.93270000},
                                    {0.05620184 ,161.70140000 ,21.93010000},
                                    {0.05620907 ,161.71610000 ,21.91660000},
                                    {0.05621051 ,161.71900000 ,21.91390000},
                                    {0.05621340 ,161.72490000 ,21.90850000},
                                    {0.05621485 ,161.72790000 ,21.90580000},
                                    {0.05621774 ,161.73370000 ,21.90040000},
                                    {0.05622496 ,161.74840000 ,21.88700000},
                                    {0.05622786 ,161.75430000 ,21.88160000},
                                    {0.05623219 ,161.76310000 ,21.87350000},
                                    {0.05623509 ,161.76900000 ,21.86810000},
                                    {0.05623653 ,161.77190000 ,21.86550000},
                                    {0.05623798 ,161.77490000 ,21.86280000},
                                    {0.05624087 ,161.78070000 ,21.85740000},
                                    {0.05624232 ,161.78370000 ,21.85470000},
                                    {0.05624376 ,161.78660000 ,21.85200000},
                                    {0.05624810 ,161.79540000 ,21.84390000},
                                    {0.05625244 ,161.80420000 ,21.83580000},
                                    {0.05625823 ,161.81600000 ,21.82510000},
                                    {0.05625968 ,161.81890000 ,21.82240000},
                                    {0.05626258 ,161.82480000 ,21.81700000},
                                    {0.05627271 ,161.84530000 ,21.79820000},
                                    {0.05627995 ,161.85990000 ,21.78470000},
                                    {0.05628720 ,161.87460000 ,21.77120000},
                                    {0.05629009 ,161.88040000 ,21.76580000},
                                    {0.05629879 ,161.89800000 ,21.74970000},
                                    {0.05630314 ,161.90680000 ,21.74160000},
                                    {0.05630459 ,161.90970000 ,21.73890000},
                                    {0.05630749 ,161.91550000 ,21.73350000},
                                    {0.05631184 ,161.92430000 ,21.72550000},
                                    {0.05631474 ,161.93010000 ,21.72010000},
                                    {0.05632199 ,161.94480000 ,21.70660000},
                                    {0.05633070 ,161.96230000 ,21.69050000},
                                    {0.05633360 ,161.96810000 ,21.68510000},
                                    {0.05633795 ,161.97690000 ,21.67700000},
                                    {0.05633940 ,161.97980000 ,21.67430000},
                                    {0.05634086 ,161.98270000 ,21.67160000},
                                    {0.05634231 ,161.98560000 ,21.66890000},
                                    {0.05634812 ,161.99730000 ,21.65810000},
                                    {0.05636119 ,162.02350000 ,21.63390000},
                                    {0.05636555 ,162.03230000 ,21.62580000},
                                    {0.05637136 ,162.04390000 ,21.61510000},
                                    {0.05638590 ,162.07310000 ,21.58810000},
                                    {0.05640044 ,162.10210000 ,21.56120000},
                                    {0.05640480 ,162.11090000 ,21.55310000},
                                    {0.05640626 ,162.11380000 ,21.55040000},
                                    {0.05641354 ,162.12830000 ,21.53700000},
                                    {0.05642664 ,162.15440000 ,21.51270000},
                                    {0.05642955 ,162.16020000 ,21.50740000},
                                    {0.05643683 ,162.17480000 ,21.49390000},
                                    {0.05645578 ,162.21240000 ,21.45890000},
                                    {0.05646744 ,162.23560000 ,21.43730000},
                                    {0.05647619 ,162.25300000 ,21.42120000},
                                    {0.05647911 ,162.25880000 ,21.41580000},
                                    {0.05648932 ,162.27900000 ,21.39690000},
                                    {0.05649078 ,162.28190000 ,21.39420000},
                                    {0.05649224 ,162.28480000 ,21.39160000},
                                    {0.05649370 ,162.28770000 ,21.38890000},
                                    {0.05649808 ,162.29640000 ,21.38080000},
                                    {0.05650538 ,162.31080000 ,21.36730000},
                                    {0.05650976 ,162.31950000 ,21.35920000},
                                    {0.05651853 ,162.33680000 ,21.34310000},
                                    {0.05651999 ,162.33970000 ,21.34040000},
                                    {0.05652145 ,162.34260000 ,21.33770000},
                                    {0.05652583 ,162.35130000 ,21.32960000},
                                    {0.05653168 ,162.36280000 ,21.31880000},
                                    {0.05654337 ,162.38590000 ,21.29730000},
                                    {0.05656239 ,162.42330000 ,21.26230000},
                                    {0.05657410 ,162.44630000 ,21.24070000},
                                    {0.05658581 ,162.46940000 ,21.21920000},
                                    {0.05659460 ,162.48660000 ,21.20300000},
                                    {0.05659899 ,162.49520000 ,21.19490000},
                                    {0.05662391 ,162.54400000 ,21.14910000},
                                    {0.05662684 ,162.54980000 ,21.14380000},
                                    {0.05663124 ,162.55840000 ,21.13570000},
                                    {0.05663564 ,162.56700000 ,21.12760000},
                                    {0.05665031 ,162.59560000 ,21.10070000},
                                    {0.05665178 ,162.59850000 ,21.09800000},
                                    {0.05665912 ,162.61280000 ,21.08450000},
                                    {0.05666206 ,162.61850000 ,21.07910000},
                                    {0.05666646 ,162.62710000 ,21.07100000},
                                    {0.05666793 ,162.63000000 ,21.06830000},
                                    {0.05666940 ,162.63280000 ,21.06560000},
                                    {0.05667234 ,162.63860000 ,21.06020000},
                                    {0.05667527 ,162.64430000 ,21.05490000},
                                    {0.05669438 ,162.68140000 ,21.01980000},
                                    {0.05669879 ,162.69000000 ,21.01180000},
                                    {0.05670026 ,162.69290000 ,21.00910000},
                                    {0.05672379 ,162.73850000 ,20.96600000},
                                    {0.05673262 ,162.75560000 ,20.94980000},
                                    {0.05673556 ,162.76130000 ,20.94440000},
                                    {0.05674292 ,162.77560000 ,20.93090000},
                                    {0.05674734 ,162.78410000 ,20.92290000},
                                    {0.05675028 ,162.78980000 ,20.91750000},
                                    {0.05675323 ,162.79550000 ,20.91210000},
                                    {0.05676501 ,162.81830000 ,20.89050000},
                                    {0.05677238 ,162.83250000 ,20.87710000},
                                    {0.05677533 ,162.83820000 ,20.87170000},
                                    {0.05678417 ,162.85530000 ,20.85550000},
                                    {0.05679450 ,162.87510000 ,20.83670000},
                                    {0.05679745 ,162.88080000 ,20.83130000},
                                    {0.05679892 ,162.88370000 ,20.82860000},
                                    {0.05680040 ,162.88650000 ,20.82590000},
                                    {0.05680482 ,162.89500000 ,20.81780000},
                                    {0.05680630 ,162.89780000 ,20.81510000},
                                    {0.05681515 ,162.91490000 ,20.79890000},
                                    {0.05681663 ,162.91770000 ,20.79620000},
                                    {0.05681810 ,162.92050000 ,20.79350000},
                                    {0.05681958 ,162.92340000 ,20.79090000},
                                    {0.05682253 ,162.92900000 ,20.78550000},
                                    {0.05682401 ,162.93190000 ,20.78280000},
                                    {0.05682844 ,162.94040000 ,20.77470000},
                                    {0.05682992 ,162.94320000 ,20.77200000},
                                    {0.05683878 ,162.96020000 ,20.75580000},
                                    {0.05684173 ,162.96590000 ,20.75040000},
                                    {0.05684616 ,162.97440000 ,20.74240000},
                                    {0.05684912 ,162.98000000 ,20.73700000},
                                    {0.05685355 ,162.98850000 ,20.72890000},
                                    {0.05685503 ,162.99140000 ,20.72620000},
                                    {0.05685651 ,162.99420000 ,20.72350000},
                                    {0.05685946 ,162.99990000 ,20.71810000},
                                    {0.05686685 ,163.01400000 ,20.70460000},
                                    {0.05686833 ,163.01680000 ,20.70190000},
                                    {0.05686981 ,163.01960000 ,20.69920000},
                                    {0.05687129 ,163.02250000 ,20.69660000},
                                    {0.05687573 ,163.03100000 ,20.68850000},
                                    {0.05688312 ,163.04510000 ,20.67500000},
                                    {0.05689496 ,163.06770000 ,20.65340000},
                                    {0.05689940 ,163.07610000 ,20.64540000},
                                    {0.05690384 ,163.08460000 ,20.63730000},
                                    {0.05690976 ,163.09590000 ,20.62650000},
                                    {0.05691272 ,163.10150000 ,20.62110000},
                                    {0.05691864 ,163.11280000 ,20.61030000},
                                    {0.05695124 ,163.17480000 ,20.55110000},
                                    {0.05701358 ,163.29270000 ,20.43790000},
                                    {0.05701506 ,163.29550000 ,20.43520000},
                                    {0.05701804 ,163.30120000 ,20.42980000},
                                    {0.05701952 ,163.30400000 ,20.42710000},
                                    {0.05702695 ,163.31800000 ,20.41370000},
                                    {0.05703438 ,163.33200000 ,20.40020000},
                                    {0.05703587 ,163.33480000 ,20.39750000},
                                    {0.05705074 ,163.36280000 ,20.37060000},
                                    {0.05707604 ,163.41030000 ,20.32480000},
                                    {0.05707753 ,163.41310000 ,20.32210000},
                                    {0.05708646 ,163.42980000 ,20.30590000},
                                    {0.05709391 ,163.44380000 ,20.29240000},
                                    {0.05710732 ,163.46890000 ,20.26820000},
                                    {0.05714310 ,163.53570000 ,20.20350000},
                                    {0.05715504 ,163.55800000 ,20.18200000},
                                    {0.05716997 ,163.58580000 ,20.15500000},
                                    {0.05719686 ,163.63570000 ,20.10650000},
                                    {0.05720732 ,163.65520000 ,20.08770000},
                                    {0.05722526 ,163.68840000 ,20.05530000},
                                    {0.05723573 ,163.70780000 ,20.03650000},
                                    {0.05723723 ,163.71060000 ,20.03380000},
                                    {0.05724172 ,163.71890000 ,20.02570000},
                                    {0.05724621 ,163.72720000 ,20.01760000},
                                    {0.05725819 ,163.74930000 ,19.99610000}};
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        

    }
}
