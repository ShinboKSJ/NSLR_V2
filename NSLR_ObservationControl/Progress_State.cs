using Newtonsoft.Json.Linq;
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
    public partial class Progress_State : Form
    {
        public Progress_State()
        {
            InitializeComponent();
        }

        private void Progress_State_Load(object sender, EventArgs e)
        {
            // UI 서브모니터 전시 + 화면 중앙 위치
            Screen[] scr = Screen.AllScreens;

            if (scr.Length > 1)
            {
                //this.Location = scr[1].Bounds.Location;
                Rectangle workingArea = scr[1].WorkingArea;
                this.Location = new Point()
                {
                    X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                    Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
                };

            }
        }

        public void Set_ProgressBarMaximum(int num, int maximum)
        {
            switch (num)
            {
                case 1:
                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = maximum;
                    progressBar1.Step = 1;
                    progressBar1.Value = 0;
                    break;
                case 2:
                    progressBar2.Minimum = 0;
                    progressBar2.Maximum = maximum;
                    progressBar2.Step = 1;
                    progressBar2.Value = 0;
                    break;
                case 3:
                    progressBar3.Minimum = 0;
                    progressBar3.Maximum = maximum;
                    progressBar3.Step = 1;
                    progressBar3.Value = 0;
                    break;
            }

        }

        public void Set_ProgressBarValue(int num, int value)
        {
            switch (num)
            {
                case 1:
                    progressBar1.Value = value;
                    break;
                case 2:
                    progressBar2.Value = value;
                    break;
                case 3:
                    progressBar3.Value = value;
                    break;
            }

        }

        public void Set_StateText(int num, string textLine)
        {
            switch (num)
            {
                case 1:
                    state_label1.Text = textLine;
                    break;
                case 2:
                    state_label2.Text = textLine;
                    break;
                case 3:
                    state_label3.Text = textLine;
                    break;
            }
            
        }

    }
}
