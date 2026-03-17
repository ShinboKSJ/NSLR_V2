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
    public partial class OAS_Controller : Form
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


        public OAS_Controller()
        {
            InitializeComponent();
        }

        private void OAS_Controller_Load(object sender, EventArgs e)
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
        }

    }
}
