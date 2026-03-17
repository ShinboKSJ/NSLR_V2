using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NSLR_ObservationControl.Module.Observation_Control;
namespace NSLR_ObservationControl.Module
{
    public partial class ScanOption : Form
    {
        public ScanOption()
        {
            InitializeComponent();
            RangeValue = 2;
            TickOffsetValue = 15.4;
            StayTimeValue = 5;
            textBoxRange.Text = RangeValue.ToString();
            textBoxTickOffset.Text = TickOffsetValue.ToString();
            textBoxStayTime.Text = StayTimeValue.ToString();
        }
        public int RangeValue { get; private set; }
        public double TickOffsetValue { get; private set; }
        public double StayTimeValue { get; private set; }

        public event EventHandler<scanInfo> ScanConfirmed;

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxRange.Text, out int range) && (range % 2 == 1) &&
    double.TryParse(textBoxTickOffset.Text, out double tickOffset) &&
    double.TryParse(textBoxStayTime.Text, out double stayTime))
            {
                RangeValue = range;
                TickOffsetValue = tickOffset;
                StayTimeValue = stayTime;

                ScanConfirmed?.Invoke(this, new scanInfo
                {
                    range = RangeValue,
                    tickOffset = TickOffsetValue,
                    stayTime = StayTimeValue
                });

                Close();
            }
            else
            {
                MessageBox.Show("입력값을 확인해주세요. 숫자만 입력 가능합니다.");
            }
        }
    }
}
