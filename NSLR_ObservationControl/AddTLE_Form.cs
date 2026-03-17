using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl
{
    public partial class AddTLE_Form : Form
    {
        public string satellite_name { get; set; } = string.Empty;
        public string line1 { get; set; } = string.Empty;
        public string line2 { get; set; } = string.Empty;

        public AddTLE_Form()
        {
            InitializeComponent();
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            satellite_name = name_textBox.Text;
            line1 = line1_textBox.Text;
            line2 = line2_textBox.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


    }
}
