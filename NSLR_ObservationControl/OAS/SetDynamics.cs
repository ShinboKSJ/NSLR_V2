using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl.OAS
{
    public partial class SetDynamics : Form
    {
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetNumberOfGravityModels(IntPtr _od);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetGravityModel(IntPtr _od, int[] degord);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetGravityModel(IntPtr _od, StringBuilder model, int deg, int ord);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetGravityModelList(IntPtr _od, int i, int[] maxDegOrd);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetOceanTideModel(IntPtr _od);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetOceanTideModel(IntPtr _od, StringBuilder model, int deg, int ord);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetTideOption(IntPtr _od, StringBuilder opt);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetTideOption(IntPtr _od, StringBuilder opt, bool flag);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetPropagatorType(IntPtr _od);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPropagatorType(IntPtr _od, StringBuilder type);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetSpaceWeatherOption(IntPtr _od);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetSpaceWeatherOption(IntPtr _od, bool flag);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetAtmosphereModel(IntPtr _od);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetAtmosphereModel(IntPtr _od, StringBuilder model);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetEphemerisSource();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetEphemerisSource(StringBuilder ephem);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetPointMasses(IntPtr _od, int[] bodyIdx);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetPointMasses(IntPtr _od, int[] bodyIdx);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GetRadiationPressureOption(IntPtr _od, int param);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetRadiationPressureOption(IntPtr _od, int param, StringBuilder _model);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetRelativisticCorrection(IntPtr _od);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetRelativisticCorrection(IntPtr _od, bool opt);

        string[] propTypes = { "ABM113", "RK2", "RK4", "RK4TI", "RK45", "RK78", "RK89" };
        string[] gravModelList;
        int[,] gravMaxDegOrd;
        string[] oceanTideModelList = { "None", "FES2004", "FES2014", "CSR3" };
        string[] atmosphereModelList = { "None", "Exponential", "JacchiaRoberts", "MSISE00", "JacchiaLineberry" };
        string[] ephemerisList = { "DE405", "DE430", "DE436" };
        string[] solarShadowModelList = { "None", "Oblate", "Spherical", "Geometrical", "Cylindrical" };
        string[] EarthRadiationModelList = { "None", "STK", "numerical", "analytical" };

        public SetDynamics()
        {
            InitializeComponent();
        }

        private void SetDynamics_Load(object sender, EventArgs e)
        {
            /////////////////////
            //  Propagator
            /////////////////////
            IntPtr propTypePtr = GetPropagatorType(Global.dynModel);
            string propTypestr = Marshal.PtrToStringAnsi(propTypePtr);
            Console.WriteLine($"[SetDynamics_Load (Propagator)] {propTypePtr} {propTypestr} ");
            Marshal.FreeHGlobal(propTypePtr);
            
            int idx = 9999;
            int i = 0;
            foreach (string str in propTypes)
            {
                propType_comboBox.Items.Add(propTypes[i]);
                if (StringComparer.OrdinalIgnoreCase.Equals(propTypes[i], propTypestr))
                { 
                    idx = i;
                    Console.WriteLine($"[PROPAGATOR Model]  ({propTypes[i]} [{idx}])");
                }
                i++;
            }
            propType_comboBox.SelectedIndex = idx;


            /////////////////////
            //  Gravity
            /////////////////////
            int[] degord = new int[4];
            int nGrav = GetNumberOfGravityModels(Global.dynModel);
            IntPtr gravModelPtr = GetGravityModel(Global.dynModel, degord);
            string gravModelstr = Marshal.PtrToStringAnsi(gravModelPtr);
            Marshal.FreeHGlobal(gravModelPtr);
            gravDeg_textBox.Text = Convert.ToString(degord[0]);
            gravMaxDeg_textBox.Text = "/ " + Convert.ToString(degord[1]);
            gravOrd_textBox.Text = Convert.ToString(degord[2]);
            gravMaxOrd_textBox.Text = "/ " + Convert.ToString(degord[3]);
            Console.WriteLine($"[SetDynamics_Load (GRAVITY)]  [{gravModelstr}] : #Grav[{nGrav}], Deg {Convert.ToString(degord[0])}, MaxDeg {Convert.ToString(degord[1])}, Ord {Convert.ToString(degord[2])}, MaxOrd {Convert.ToString(degord[3])}");

            idx = 9999;
            gravModelList = new string[nGrav];
            gravMaxDegOrd = new int[nGrav, 2];
            int[] gravMaxDegOrd_tmp = new int[2];
            for (int n = 0; n < nGrav; n++)
            {
                IntPtr gravModelListPtr = GetGravityModelList(Global.dynModel, n, gravMaxDegOrd_tmp);
                string gravModelListstr = Marshal.PtrToStringAnsi(gravModelListPtr);
                Marshal.FreeHGlobal(gravModelListPtr);
                gravModelList[n] = gravModelListstr;
                gravMaxDegOrd[n, 0] = gravMaxDegOrd_tmp[0];
                gravMaxDegOrd[n, 1] = gravMaxDegOrd_tmp[1];
                gravModel_comboBox.Items.Add(gravModelList[n]);
                if (StringComparer.OrdinalIgnoreCase.Equals(gravModelList[n], gravModelstr)) 
                { 
                    idx = n;
                    Console.WriteLine($"[GRAVITY Model]  ({gravModelList[n]} [{idx}])");
                }
            }
            gravModel_comboBox.SelectedIndex = idx;


            /////////////////////
            //  Tide
            /////////////////////
            StringBuilder opt = new StringBuilder("EarthTide");
            bool earthTide = GetTideOption(Global.dynModel, opt);
            EarthTide_checkBox.Checked = earthTide;

            opt = new StringBuilder("solidPole");
            bool solidPole = GetTideOption(Global.dynModel, opt);
            solidPole_checkBox.Checked = solidPole;

            opt = new StringBuilder("oceanPole");
            bool oceanPole = GetTideOption(Global.dynModel, opt);
            oceanPole_checkBox.Checked = oceanPole;

            opt = new StringBuilder("permanentTide");
            bool permanentTide = GetTideOption(Global.dynModel, opt);
            permanent_checkBox.Checked = permanentTide;

            IntPtr oceanTideModelPtr = GetOceanTideModel(Global.dynModel);
            string oceanTideModelstr = Marshal.PtrToStringAnsi(oceanTideModelPtr);
            Marshal.FreeHGlobal(oceanTideModelPtr);
            Console.WriteLine($"[TIDE Model] : earthTide[{earthTide}], solidPole {solidPole}, oceanPole {oceanPole}, permanentTide {permanentTide}");
            
            idx = 9999;
            i = 0;

            foreach (string str in oceanTideModelList)
            {
                oceanTide_comboBox.Items.Add(oceanTideModelList[i]);
                if (StringComparer.OrdinalIgnoreCase.Equals(oceanTideModelList[i], oceanTideModelstr)) 
                { 
                    idx = i;
                    Console.WriteLine($"[TIDE Model] ({oceanTideModelList[i]} [{idx}])");
                }
                i++;
            }
            oceanTide_comboBox.SelectedIndex = idx;


            /////////////////////
            //  Atmosphere
            /////////////////////
            bool swOpt = GetSpaceWeatherOption(Global.dynModel);
            if (swOpt) { sw_data_radioButton.Checked = true; }
            else { sw_normal_radioButton.Checked = true; }

            IntPtr atmosphereModelPtr = GetAtmosphereModel(Global.dynModel);
            string atmosphereModelstr = Marshal.PtrToStringAnsi(atmosphereModelPtr);
            Marshal.FreeHGlobal(atmosphereModelPtr);
            Console.WriteLine($"[ATMOSPHERE Model] ({atmosphereModelstr} swOpt[{swOpt}])");

            idx = 9999;
            i = 0;
            foreach (string str in atmosphereModelList)
            {
                atmosModel_comboBox.Items.Add(atmosphereModelList[i]);
                if (StringComparer.OrdinalIgnoreCase.Equals(atmosphereModelList[i], atmosphereModelstr)) 
                { 
                    idx = i;
                    Console.WriteLine($"[ATMOSPHERE Model] ({atmosphereModelList[i]} [{idx}])");
                }
                i++;
            }
            atmosModel_comboBox.SelectedIndex = idx;


            /////////////////////
            //  Point Masses
            /////////////////////
            IntPtr ephemPtr = GetEphemerisSource();
            string ephemStr = Marshal.PtrToStringAnsi(ephemPtr);
            idx = 9999;
            i = 0;
            foreach (string ephem in ephemerisList)
            {
                ephem_comboBox.Items.Add(ephem);
                if (StringComparer.OrdinalIgnoreCase.Equals(ephemerisList[i], ephemStr)) 
                { 
                    idx = i;
                    Console.WriteLine($"[POINT MASSES - Ephemeris] ({ephemerisList[i]} [{idx}])");
                }
                i++;
            }
            ephem_comboBox.SelectedIndex = idx;

            int[] bodyIdx = new int[10];
            GetPointMasses(Global.dynModel, bodyIdx);
            if (bodyIdx[0] == 1) { sun_checkBox.Checked = true; }
            if (bodyIdx[1] == 1) { moon_checkBox.Checked = true; }
            if (bodyIdx[2] == 1) { mercury_checkBox.Checked = true; }
            if (bodyIdx[3] == 1) { venus_checkBox.Checked = true; }
            if (bodyIdx[4] == 1) { mars_checkBox.Checked = true; }
            if (bodyIdx[5] == 1) { jupiter_checkBox.Checked = true; }
            if (bodyIdx[6] == 1) { saturn_checkBox.Checked = true; }
            if (bodyIdx[7] == 1) { uranus_checkBox.Checked = true; }
            if (bodyIdx[8] == 1) { neptune_checkBox.Checked = true; }
            if (bodyIdx[9] == 1) { pluto_checkBox.Checked = true; }


            ////////////////////////////////
            //  Radiation Pressure
            ////////////////////////////////
            IntPtr solarShadowPtr = GetRadiationPressureOption(Global.dynModel, 1);
            string solarShadowStr = Marshal.PtrToStringAnsi(solarShadowPtr);
            Marshal.FreeHGlobal(solarShadowPtr);

            idx = 9999;
            i = 0;
            foreach (string str in solarShadowModelList)
            {
                solarShadow_comboBox.Items.Add(solarShadowModelList[i]);
                if (StringComparer.OrdinalIgnoreCase.Equals(solarShadowModelList[i], solarShadowStr))
                {
                    idx = i;
                    Console.WriteLine($"[RadiationPresure - SOLAR SHADOW] ({solarShadowStr} [{idx})");
                }
                i++;
            }
            solarShadow_comboBox.SelectedIndex = idx;


            //Earth Radiation
            IntPtr earthRadiationPtr = GetRadiationPressureOption(Global.dynModel, 2);
            string earthRadiationStr = Marshal.PtrToStringAnsi(earthRadiationPtr);
            Marshal.FreeHGlobal(earthRadiationPtr);
            
            idx = 9999;
            i = 0;
            foreach (string str in EarthRadiationModelList)
            {
                EarthRad_comboBox.Items.Add(EarthRadiationModelList[i]);
                Console.WriteLine($"Adding [ {EarthRadiationModelList[i]} ]");
                i++;
            }

            i = 0;
            foreach (string str in EarthRadiationModelList)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(EarthRadiationModelList[i], earthRadiationStr))
                {
                    idx = i;
                    Console.WriteLine($"[RadiationPresure - EarthRadiation] ({earthRadiationStr} [{idx}])");
                }
                i++;
            }


            EarthRad_comboBox.SelectedIndex = idx;

            // relativistic correction
            relCor_checkBox.Checked = GetRelativisticCorrection(Global.dynModel);
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            // propagator
            StringBuilder propType = new StringBuilder(propType_comboBox.SelectedItem.ToString());
            SetPropagatorType(Global.dynModel, propType);

            // gravity
            StringBuilder gravModel = new StringBuilder(gravModel_comboBox.SelectedItem.ToString());
            int deg = Convert.ToInt16(gravDeg_textBox.Text);
            int ord = Convert.ToInt16(gravOrd_textBox.Text);
            SetGravityModel(Global.dynModel, gravModel, deg, ord);

            // tide
            StringBuilder oceanTideModel = new StringBuilder(oceanTide_comboBox.SelectedItem.ToString());
            
            StringBuilder tideOpt = new StringBuilder("EarthTide");

            SetTideOption(Global.dynModel, tideOpt, EarthTide_checkBox.Checked);
            
            tideOpt = new StringBuilder("solidPole");
            SetTideOption(Global.dynModel, tideOpt, solidPole_checkBox.Checked);

            tideOpt = new StringBuilder("oceanPole");
            SetTideOption(Global.dynModel, tideOpt, oceanPole_checkBox.Checked);

            tideOpt = new StringBuilder("permanentTide");
            SetTideOption(Global.dynModel, tideOpt, permanent_checkBox.Checked);

            SetOceanTideModel(Global.dynModel, oceanTideModel, deg, ord);

            // atmosphere
            StringBuilder atmosModel = new StringBuilder(atmosModel_comboBox.SelectedItem.ToString());
            SetAtmosphereModel(Global.dynModel, atmosModel);

            SetSpaceWeatherOption(Global.dynModel, sw_data_radioButton.Checked);

            // point masses
            StringBuilder ephemeris = new StringBuilder(ephem_comboBox.SelectedItem.ToString());
            SetEphemerisSource(ephemeris);

            int[] bodyIdx = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            if (sun_checkBox.Checked) { bodyIdx[0] = 1; }
            if (moon_checkBox.Checked) { bodyIdx[1] = 1; }
            if (mercury_checkBox.Checked) { bodyIdx[2] = 1; }
            if (venus_checkBox.Checked) { bodyIdx[3] = 1; }
            if (mars_checkBox.Checked) { bodyIdx[4] = 1; }
            if (jupiter_checkBox.Checked) { bodyIdx[5] = 1; }
            if (saturn_checkBox.Checked) { bodyIdx[6] = 1; }
            if (uranus_checkBox.Checked) { bodyIdx[7] = 1; }
            if (neptune_checkBox.Checked) { bodyIdx[8] = 1; }
            if (pluto_checkBox.Checked) { bodyIdx[9] = 1; }
            SetPointMasses(Global.dynModel, bodyIdx);

            // radiation
            StringBuilder solarShadowModel = new StringBuilder(solarShadow_comboBox.SelectedItem.ToString());
            SetRadiationPressureOption(Global.dynModel, 1, solarShadowModel);

            StringBuilder EarthRadiationModel = new StringBuilder(EarthRad_comboBox.SelectedItem.ToString());
            SetRadiationPressureOption(Global.dynModel, 2, EarthRadiationModel);

            // rel
            SetRelativisticCorrection(Global.dynModel, relCor_checkBox.Checked);

            this.Hide();

            Console.WriteLine("SetDynamics_Load ......................OK");
        }

        private void SetDynamics_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void gravModel_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n = gravModel_comboBox.SelectedIndex;
            gravMaxDeg_textBox.Text = "/ " + Convert.ToString(gravMaxDegOrd[n, 0]);
            gravMaxOrd_textBox.Text = "/ " + Convert.ToString(gravMaxDegOrd[n, 1]);
        }

    }
}
