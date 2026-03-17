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
    public partial class SetObject : Form
    {
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetParameter(IntPtr _sat, StringBuilder _name, double val);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetParameter(IntPtr _sat, StringBuilder _name);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetOrbit(IntPtr _sat, double[] _stateVec, StringBuilder _type, StringBuilder _coordSys,
                                        StringBuilder epochType, StringBuilder _epochSys, double[] epochTime);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern StringBuilder GetOrbitalState(IntPtr _sat, double[] stateVec);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern StringBuilder GetOrbitalCoordinate(IntPtr _sat);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern StringBuilder GetOrbitalLabel(IntPtr _sat, int i);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetEpochTime(IntPtr sat, double[] epochDate);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern StringBuilder GetEpochTimeSystem(IntPtr sat);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern StringBuilder GetEpochTimeFormat(IntPtr sat);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpdateParameterSatellite(IntPtr paramClass, IntPtr satClass);

        string[] allEpochType = { "Gregorian", "ModJulian" };
        string[] allEpochSystem = { "A1", "TAI", "UTC", "TDB", "TT" };
        string[] allStateTypes = { "Cartesian", "Keplerian", "BrouwerMeanShort", "BrouwerMeanLong" };
        string[] defaultCoordinates = { "EarthMJ2000Eq", "EarthMJ2000Ec", "EarthFixed", "EarthICRF", "LVLH" };
        public SetObject()
        {
            InitializeComponent();
        }

        private void SetObject_Load(object sender, EventArgs e)
        {
            int i = 0;
            int idx = 9999;

            // epoch
            StringBuilder epochSys = GetEpochTimeSystem(Global.sat);

            //Console.WriteLine(epochSys.ToString());
            foreach (var item in allEpochSystem)
            {
                timeSys_comboBox.Items.Add(allEpochSystem[i]);
                i++;
            }
            i = 0;
            foreach (var item in allEpochSystem)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(allEpochSystem[i], epochSys.ToString())) 
                { 
                    idx = i;
                    Console.WriteLine($"[SetObject State] {epochSys.ToString()}"); // UTC
                }
                i++;
            }
            timeSys_comboBox.SelectedIndex = idx;


            StringBuilder epochType = GetEpochTimeFormat(Global.sat);
            i = 0;
            idx = 9999;
            foreach (var item in allEpochType)
            {
                timeType_comboBox.Items.Add(allEpochType[i]);
                i++;
            }
            i = 0;
            foreach (var item in allEpochType)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(allEpochType[i], epochType.ToString())) 
                { 
                    idx = i;
                    Console.WriteLine($"[SetObject State] {epochType.ToString()}"); // Greogrian 
                }
                i++;
            }
            timeType_comboBox.SelectedIndex = idx;

            double[] epochDate = new double[6];
            double mjd = GetEpochTime(Global.sat, epochDate);
            epochMJD_textBox.Text = Convert.ToString(mjd);
            for (int j = 0; j < 6; j++)
            {
                switch (j)
                {
                    case 0: epoch1_textBox.Text = Convert.ToString(epochDate[j]); break;
                    case 1: epoch2_textBox.Text = Convert.ToString(epochDate[j]); break;
                    case 2: epoch3_textBox.Text = Convert.ToString(epochDate[j]); break;
                    case 3: epoch4_textBox.Text = Convert.ToString(epochDate[j]); break;
                    case 4: epoch5_textBox.Text = Convert.ToString(epochDate[j]); break;
                    case 5: epoch6_textBox.Text = Convert.ToString(epochDate[j]); break;
                }
            }
             
            
            // coordinate
            StringBuilder coordSys = GetOrbitalCoordinate(Global.sat);
            i = 0;
            idx = 9999;
            foreach (var item in defaultCoordinates)
            {
                coordSys_comboBox.Items.Add(defaultCoordinates[i]);
                i++;
            }
            i = 0; 
            foreach (var item in defaultCoordinates)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(defaultCoordinates[i], coordSys.ToString())) 
                { 
                    idx = i;
                    Console.WriteLine($"[SetObject State] {coordSys.ToString()}"); //EarthMJ2000Eq
                }
                i++;
            }
            coordSys_comboBox.SelectedIndex = idx;


            // state
            double[] stateVec = new double[6];
            StringBuilder stateType = GetOrbitalState(Global.sat, stateVec);

            i = 0;
            idx = 9999;
            foreach (var item in allStateTypes)
            {
                stateType_comboBox.Items.Add(allStateTypes[i]);
                i++;
            }
            i = 0;
            foreach (var item in allStateTypes)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(allStateTypes[i], stateType.ToString()))
                {
                    Console.WriteLine($"[SetObject State] {stateType.ToString()}"); //Cartesian
                    idx = i;
                }
                i++;
            }
            stateType_comboBox.SelectedIndex = idx;

            string[] labelSet = new string[6];
            StringBuilder label = new StringBuilder();
            for (int j = 0; j < 6; j++)
            {
                label = GetOrbitalLabel(Global.sat, j);
                switch (j)
                {
                    case 0:
                        state1_label.Text = label.ToString();
                        state1_textBox.Text = Convert.ToString(stateVec[j]);
                        break;
                    case 1:
                        state2_label.Text = label.ToString();
                        state2_textBox.Text = Convert.ToString(stateVec[j]);
                        break;
                    case 2:
                        state3_label.Text = label.ToString();
                        state3_textBox.Text = Convert.ToString(stateVec[j]);
                        break;
                    case 3:
                        state4_label.Text = label.ToString();
                        state4_textBox.Text = Convert.ToString(stateVec[j]);
                        break;
                    case 4:
                        state5_label.Text = label.ToString();
                        state5_textBox.Text = Convert.ToString(stateVec[j]);
                        break;
                    case 5:
                        state6_label.Text = label.ToString();
                        state6_textBox.Text = Convert.ToString(stateVec[j]);
                        break;
                }
            }

            // property
            
            StringBuilder _name = new StringBuilder("mass");
            double val = GetParameter(Global.sat, _name);

            mass_textBox.Text = Convert.ToString(val);
            _name = new StringBuilder("cd");
            val = GetParameter(Global.sat, _name);
            Cd_textBox.Text = Convert.ToString(val);

            _name = new StringBuilder("cr");
            val = GetParameter(Global.sat, _name);
            Cr_textBox.Text = Convert.ToString(val);

            _name = new StringBuilder("ck");
            val = GetParameter(Global.sat, _name);
            Ck_textBox.Text = Convert.ToString(val);

            _name = new StringBuilder("dragArea");
            val = GetParameter(Global.sat, _name);
            dragArea_textBox.Text = Convert.ToString(val);

            _name = new StringBuilder("srpArea");
            val = GetParameter(Global.sat, _name);
            srpArea_textBox.Text = Convert.ToString(val);

            _name = new StringBuilder("erpArea");
            val = GetParameter(Global.sat, _name);
            erpArea_textBox.Text = Convert.ToString(val);

            Console.WriteLine("SetObject_Load..................... OK !");
        }

        private void SetObject_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            double[] stateVec = new double[6];
            double[] epochTime = new double[6];

            for (int j = 0; j < 6; j++)
            {
                double stateVal = 0;
                double epochVal = 0;
                switch (j)
                {
                    case 0:
                        //stateVal = double.Parse(state1_label.Text);
                        stateVal = double.Parse(state1_textBox.Text);
                        epochVal = double.Parse(epoch1_textBox.Text);
                        break;
                    case 1:
                        //stateVal = double.Parse(state2_label.Text);
                        stateVal = double.Parse(state2_textBox.Text);
                        epochVal = double.Parse(epoch2_textBox.Text);
                        break;
                    case 2:
                        //stateVal = double.Parse(state3_label.Text);
                        stateVal = double.Parse(state3_textBox.Text);
                        epochVal = double.Parse(epoch3_textBox.Text);
                        break;
                    case 3:
                        //stateVal = double.Parse(state4_label.Text);
                        stateVal = double.Parse(state4_textBox.Text);
                        epochVal = double.Parse(epoch4_textBox.Text);
                        break;
                    case 4:
                        //stateVal = double.Parse(state5_label.Text);
                        stateVal = double.Parse(state5_textBox.Text);
                        epochVal = double.Parse(epoch5_textBox.Text);
                        break;
                    case 5:
                        //stateVal = double.Parse(state6_label.Text);
                        stateVal = double.Parse(state5_textBox.Text);
                        epochVal = double.Parse(epoch6_textBox.Text);
                        break;
                }
                stateVec[j] = stateVal;
                epochTime[j] = epochVal;
            }

            StringBuilder type = new StringBuilder(stateType_comboBox.SelectedItem.ToString());
            StringBuilder coordSys = new StringBuilder(coordSys_comboBox.SelectedItem.ToString());
            StringBuilder epochType = new StringBuilder(timeType_comboBox.SelectedItem.ToString());
            StringBuilder epochSys = new StringBuilder(timeSys_comboBox.SelectedItem.ToString());

            SetOrbit(Global.sat, stateVec, type, coordSys, epochType, epochSys, epochTime);


            // property
            StringBuilder _name = new StringBuilder("mass");
            double val = double.Parse(mass_textBox.Text);
            SetParameter(Global.sat, _name, val);
            _name = new StringBuilder("cd");
            val = double.Parse(Cd_textBox.Text);
            SetParameter(Global.sat, _name, val);
            _name = new StringBuilder("cr");
            val = double.Parse(Cr_textBox.Text);
            SetParameter(Global.sat, _name, val);
            _name = new StringBuilder("ck");
            val = double.Parse(Ck_textBox.Text);
            SetParameter(Global.sat, _name, val);
            _name = new StringBuilder("dragArea");
            val = double.Parse(dragArea_textBox.Text);
            SetParameter(Global.sat, _name, val);
            _name = new StringBuilder("srpArea");
            val = double.Parse(srpArea_textBox.Text);
            SetParameter(Global.sat, _name, val);
            _name = new StringBuilder("erpArea");
            val = double.Parse(erpArea_textBox.Text);
            SetParameter(Global.sat, _name, val);

            UpdateParameterSatellite(Global.paramClass, Global.sat);

            this.Hide();

            Console.WriteLine("SetObject ...................... OK");
        }

        private void timeType_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (timeType_comboBox.SelectedIndex == 0)
            {
                epochMJD_textBox.ReadOnly = true;
                epoch1_textBox.ReadOnly = false;
                epoch2_textBox.ReadOnly = false;
                epoch3_textBox.ReadOnly = false;
                epoch4_textBox.ReadOnly = false;
                epoch5_textBox.ReadOnly = false;
                epoch6_textBox.ReadOnly = false;
            }
            else
            {
                epochMJD_textBox.ReadOnly = false;
                epoch1_textBox.ReadOnly = true;
                epoch2_textBox.ReadOnly = true;
                epoch3_textBox.ReadOnly = true;
                epoch4_textBox.ReadOnly = true;
                epoch5_textBox.ReadOnly = true;
                epoch6_textBox.ReadOnly = true;
            }
        }
    }
}
