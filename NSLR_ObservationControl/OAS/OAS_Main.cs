using Microsoft.Win32;
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
    public partial class OAS_Main : Form
    {
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void SetConfiguration(IntPtr _config, StringBuilder _name, StringBuilder _val);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetSolarSystem(IntPtr _solarSystem, StringBuilder ephemName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetConstants(IntPtr _OASCONST, StringBuilder eopFileName, StringBuilder eopFormat, StringBuilder deltaTFileName, StringBuilder dTAIFileName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateSolarSystem();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateOrbitDynamics();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateEstimator();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateParameters(IntPtr satClass);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateTroposphereModel();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateRelativisticCorrection();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateReferencePointCorrection();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateLighttimeCorrection();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateCenterCorrection();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateMeasurementModel();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateObservation();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateOrbit();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateSatellite();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateGroundSite(double[] loc, StringBuilder siteName, double waveLength);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateTLE();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateTimeSystem();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateCoordinateSystem();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateConstants();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreatePropagator();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateConfiguration();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateResidual();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetConfigurationValue(IntPtr _config, StringBuilder name);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LoadObservation(IntPtr obsClass, StringBuilder _fileName);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetOrbit(IntPtr _sat, double[] _stateVec, StringBuilder _type, StringBuilder _coordSys,
                                        StringBuilder epochType, StringBuilder _epochSys, double[] epochTime);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetObservatory(IntPtr estClass, StringBuilder obs);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetObservable(IntPtr estClass, bool[] idxVec);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetEstimatorTime(IntPtr estClass, StringBuilder type, double[] dateVec);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetParameterEpochTime(IntPtr paramClass, double[] dateVec);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpdateParameterSatellite(IntPtr paramClass, IntPtr satClass);

        public OAS_Main()
        {
            InitializeComponent();
        }

        private void setDyn_button_Click(object sender, EventArgs e)
        {
            SetDynamics setDyn = new SetDynamics();
            setDyn.Show();
        }

        private void OAS_Main_Load(object sender, EventArgs e)
        {

            Global.config = CreateConfiguration();
            Global.solarSystem = CreateSolarSystem();
            Global.OASCONST = CreateConstants();

            Global.dynModel = CreateOrbitDynamics();

            Global.sat = CreateSatellite();

            double[] siteLoc = { 126.16659925, 36.70035547, 0.234866 };
            StringBuilder siteName = new StringBuilder("Geochang");
            double waveLength = 532;

            Global.laserSite = CreateGroundSite(siteLoc, siteName, waveLength);

            Global.meaModel = CreateMeasurementModel();
            Global.obsClass = CreateObservation();

            Global.estClass = CreateEstimator();
            Global.paramClass = CreateParameters(Global.sat);

            Global.timeSys = CreateTimeSystem();
            Global.coordSys = CreateCoordinateSystem();

            Global.residual = CreateResidual();

            /*
            Console.WriteLine($"==========< OAS_CONFIGURATION >==============================");
            Console.WriteLine($"-------------------------------------------------------------");  
            Console.WriteLine($"=[config] > {Global.config}");
            Console.WriteLine($"=[solarSystem] > {Global.solarSystem}");
            Console.WriteLine($"=[OASCONST] > {Global.OASCONST}");
            Console.WriteLine($"=[dynModel] > {Global.dynModel}");
            Console.WriteLine($"=[sat] > {Global.sat}");
            Console.WriteLine($"=[laserSite] > {Global.laserSite}");
            Console.WriteLine($"=[meaModel] > {Global.meaModel}");
            Console.WriteLine($"=[obsClass] > {Global.obsClass}");
            Console.WriteLine($"=[estClass] > {Global.estClass}");
            Console.WriteLine($"=[paramClass] > {Global.paramClass}");
            Console.WriteLine($"=[timeSys] > {Global.timeSys}");
            Console.WriteLine($"=[coordSys] > {Global.coordSys}");
            Console.WriteLine($"=[residual] > {Global.residual}");
            Console.WriteLine($"-------------------------------------------------------------");
            */
        }

        private void setObject_button_Click(object sender, EventArgs e)
        {
            SetObject setObj = new SetObject();
            setObj.Show();
        }

        private void setMea_button_Click(object sender, EventArgs e)
        {
            SetMeasurement setMea = new SetMeasurement();
            setMea.Show();
        }

        /*
          obsFileDialog.InitialDirectory = Application.StartupPath + @"\Resources"; // "./Resources/Observation";
          if (obsFileDialog.ShowDialog() == DialogResult.OK)
          {
              string filePath = obsFileDialog.FileName;
              Console.WriteLine(filePath.ToString());
              try
              {
                  // 파일 읽기
                  StringBuilder fileContent = new StringBuilder();
                  using (StreamReader reader = new StreamReader(filePath))
                  {
                      string line;
                      while ((line = reader.ReadLine()) != null)
                      {
                          fileContent.AppendLine(line);
                      }
                  }

                  // 파일 내용 출력
                  Console.WriteLine("파일 내용:");
                  Console.WriteLine(fileContent.ToString());
              }
              catch (Exception ex)
              {
                  // 오류 처리
                  Console.WriteLine("오류 발생: " + ex.Message);
              }

              //LoadObservation(Global.obsClass, filePath);
              MessageBox.Show("Observation data is loaded.", "NSLR-OAS", MessageBoxButtons.OK);
          }
         */

        private void loadObs_button_Click(object sender, EventArgs e)
        {
            obsFileDialog.InitialDirectory = Application.StartupPath + @"\Resources\Observation"; 
            if (obsFileDialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder filePath = new StringBuilder(obsFileDialog.FileName);
                LoadObservation(Global.obsClass, filePath);
                Console.WriteLine("Observation data is loaded.");

            }
        }

        private void setEst_button_Click(object sender, EventArgs e)
        {
            SetEstimator setEst = new SetEstimator();
            setEst.Show();
        }

        private void run_button_Click(object sender, EventArgs e)
        {
            int estFlag = LeastSquares(sender, e);
        }

        private void genEph_button_Click(object sender, EventArgs e)
        {
            ephemerisGenerator ephGen = new ephemerisGenerator();
            ephGen.Show();
        }

        private void genCMD_button_Click(object sender, EventArgs e)
        {
            CommandGenerator cmdGen = new CommandGenerator();
            cmdGen.Show();
        }

        private void bodyLoc_button_Click(object sender, EventArgs e)
        {
            BodyLocationGenerator bodyGen = new BodyLocationGenerator();
            bodyGen.Show();
        }

        private void viewRes_button_Click(object sender, EventArgs e)
        {
            ResidualPlotViewer resViewer = new ResidualPlotViewer();
            resViewer.Show();
        }

        private void testRunLS_button_Click(object sender, EventArgs e)
        {
            //StringBuilder CRDFile = new StringBuilder(Application.StartupPath + @"\Resources\Observation\lageos1_20210102_test.frd");
            obsFileDialog.InitialDirectory = Application.StartupPath + @"\Resources";
            if (obsFileDialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder filePath = new StringBuilder(obsFileDialog.FileName);
                LoadObservation(Global.obsClass, filePath);
                MessageBox.Show("Observation data is loaded.", "NSLR-OAS", MessageBoxButtons.OK);
                double[] stateVec = { 9140.11102616297, 7852.43515050818, 2575.81911612253, 2.13175968954918, -0.753413759771749, -5.20670191770466 };
                double[] epochDate = { 2020, 12, 31, 17, 47, 59.4039997558594 };
                StringBuilder type = new StringBuilder("Cartesian");
                StringBuilder coord = new StringBuilder("EarthMJ2000Eq");
                StringBuilder epochType = new StringBuilder("ModJulian");
                StringBuilder epochSys = new StringBuilder("UTC");
                SetOrbit(Global.sat, stateVec, type, coord, epochType, epochSys, epochDate);

                UpdateParameterSatellite(Global.paramClass, Global.sat);

                StringBuilder obsName = new StringBuilder("MATERA, ITALY");
                bool[] obsIdx = { true, true };
                double[] startDate = { 2020, 12, 31, 17, 47, 59 };
                double[] finalTime = { 2020, 12, 31, 17, 48, 14 };

                SetObservatory(Global.estClass, obsName);
                SetObservable(Global.estClass, obsIdx);
                SetParameterEpochTime(Global.paramClass, epochDate);
                epochType = new StringBuilder("start");
                SetEstimatorTime(Global.estClass, epochType, startDate);
                epochType = new StringBuilder("end");
                SetEstimatorTime(Global.estClass, epochType, finalTime);

                int flag = LeastSquares(sender, e);
            }
        }

        private void testRunEKF_button_Click(object sender, EventArgs e)
        {
            StringBuilder CRDFile = new StringBuilder("./Resources/Observation/lageos1_20210102_test.frd");
            LoadObservation(Global.obsClass, CRDFile);
            double[] stateVec = { 9140.11102616297, 7852.43515050818, 2575.81911612253, 2.13175968954918, -0.753413759771749, -5.20670191770466 };
            double[] epochDate = { 2020, 12, 31, 17, 47, 59.4039997558594 };
            StringBuilder type = new StringBuilder("Cartesian");
            StringBuilder coord = new StringBuilder("EarthMJ2000Eq");
            StringBuilder epochType = new StringBuilder("ModJulian");
            StringBuilder epochSys = new StringBuilder("UTC");
            SetOrbit(Global.sat, stateVec, type, coord, epochType, epochSys, epochDate);

            UpdateParameterSatellite(Global.paramClass, Global.sat);

            StringBuilder obsName = new StringBuilder("MATERA, ITALY");
            bool[] obsIdx = { true, true };
            double[] startDate = { 2020, 12, 31, 17, 47, 59 };
            double[] finalTime = { 2020, 12, 31, 17, 48, 14 };

            SetObservatory(Global.estClass, obsName);
            SetObservable(Global.estClass, obsIdx);
            SetParameterEpochTime(Global.paramClass, epochDate);
            epochType = new StringBuilder("start");
            SetEstimatorTime(Global.estClass, epochType, startDate);
            epochType = new StringBuilder("end");
            SetEstimatorTime(Global.estClass, epochType, finalTime);

            EKF(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
