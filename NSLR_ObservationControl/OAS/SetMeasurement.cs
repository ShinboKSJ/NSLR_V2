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
    public partial class SetMeasurement : Form
    {
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern StringBuilder GetLighttimeCorrection(IntPtr meaModel);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetCenterCorrection(IntPtr meaModel);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetReferencePointCorrection(IntPtr meaModel, StringBuilder type);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetRelativisticDelay(IntPtr meaModel);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetTroposphereModelCorrection(IntPtr meaModel);

        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetLighttimeCorrection(IntPtr meaModel, StringBuilder type);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCenterCorrection(IntPtr meaModel, double rad);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetReferencePointCorrection(IntPtr meaModel, StringBuilder type, bool val);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetRelativisticDelay(IntPtr meaModel, bool val);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetTroposphereModelCorrection(IntPtr meaModel, bool val);


        string[] allLTTTypes = { "none", "1way", "2way" };
        public SetMeasurement()
        {
            InitializeComponent();
        }

        private void SetMeasurement_Load(object sender, EventArgs e)
        {
            StringBuilder lttCor = GetLighttimeCorrection(Global.meaModel);
            int i = 0;
            int idx = 9999;
            foreach (var item in allLTTTypes)
            {
                lttType_comboBox.Items.Add(allLTTTypes[i]);
                if (StringComparer.OrdinalIgnoreCase.Equals(allLTTTypes[i], lttCor.ToString())) { idx = i; }
                i++;
            }
            lttType_comboBox.SelectedIndex = idx;

            double rad = GetCenterCorrection(Global.meaModel);
            if (rad != 0)
            {
                com_checkBox.Checked = true;
                comRad_textBox.ReadOnly = false;
                comRad_textBox.Text = Convert.ToString(rad);
            }
            else
            {
                com_checkBox.Checked = false;
                comRad_textBox.ReadOnly = true;
            }

            StringBuilder type = new StringBuilder("plateTectonic");
            plateTec_checkBox.Checked = GetReferencePointCorrection(Global.meaModel, type);
            type = new StringBuilder("solidEarthTide");
            solidEarth_checkBox.Checked = GetReferencePointCorrection(Global.meaModel, type);
            type = new StringBuilder("oceanTide");
            oceanTide_checkBox.Checked = GetReferencePointCorrection(Global.meaModel, type);
            type = new StringBuilder("poleTide");
            poleTide_checkBox.Checked = GetReferencePointCorrection(Global.meaModel, type);

            relDelay_checkBox.Checked = GetRelativisticDelay(Global.meaModel);
            tropoDelay_checkBox.Checked = GetTroposphereModelCorrection(Global.meaModel);
        }

        private void SetMeasurement_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void com_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            comRad_textBox.ReadOnly = !com_checkBox.Checked;
            if (com_checkBox.Checked) { comRad_textBox.Text = "0"; }
            else { comRad_textBox.Text = ""; }
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            StringBuilder lttCor = new StringBuilder(lttType_comboBox.SelectedItem.ToString());
            SetLighttimeCorrection(Global.meaModel, lttCor);

            if (com_checkBox.Checked)
            {
                SetCenterCorrection(Global.meaModel, Convert.ToInt32(comRad_textBox.Text));
            }

            StringBuilder type = new StringBuilder("plateTectonic");
            SetReferencePointCorrection(Global.meaModel, type, plateTec_checkBox.Checked);
            type = new StringBuilder("solidEarthTide");
            SetReferencePointCorrection(Global.meaModel, type, solidEarth_checkBox.Checked);
            type = new StringBuilder("oceanTide");
            SetReferencePointCorrection(Global.meaModel, type, oceanTide_checkBox.Checked);
            type = new StringBuilder("poleTide");
            SetReferencePointCorrection(Global.meaModel, type, poleTide_checkBox.Checked);

            SetRelativisticDelay(Global.meaModel, relDelay_checkBox.Checked);
            SetTroposphereModelCorrection(Global.meaModel, tropoDelay_checkBox.Checked);

            this.Hide();

            Console.WriteLine("SetMeasurement ...................... OK");
        }
    }
}
