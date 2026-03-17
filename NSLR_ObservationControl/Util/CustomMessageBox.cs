using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl.Util
{
    public partial class CustomMessageBox : Form
    {
        public CustomMessageBox(string message, string title, string button1Text = "OK", string button2Text = "")
        {
            InitializeComponent();
            label_title.Text = title;
            lblMessage.Text = message;
            btnOK.Text = button1Text;
            btnCancel.Text = button2Text;
            btnCancel.Visible = !string.IsNullOrEmpty(button2Text);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
        }


        public static DialogResult Show(string message, string title = "Message", string button1Text = "OK", string button2Text = "")
        {
            using (var msgBox = new CustomMessageBox(message, title, button1Text, button2Text))
            {
                return msgBox.ShowDialog();
            }
        }

        private void lblMessage_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
