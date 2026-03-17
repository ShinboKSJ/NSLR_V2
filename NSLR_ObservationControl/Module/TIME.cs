using System;
using System.Windows.Forms;

namespace NSLR_ObservationControl.Module
{
    public partial class TIME : UserControl
    {
        private Timer clockTimer;

        public TIME()
        {
            InitializeComponent();
            this.Disposed += TIME_Disposed;
        }

        private void TIME_Load(object sender, EventArgs e)
        {
            clockTimer = new Timer();
            clockTimer.Interval = 1000; 
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();

            UpdateClock(); 
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            UpdateClock();
        }

        private void UpdateClock()
        {
            DateTime kstNow = DateTime.Now;
            DateTime utcNow = DateTime.UtcNow;

            label_KSTdate.Text = kstNow.ToString("yy.MM.dd");
            label_KSTtime.Text = kstNow.ToString("HH:mm:ss");

            label_UTCdate.Text = utcNow.ToString("yy.MM.dd");
            label_UTCtime.Text = utcNow.ToString("HH:mm:ss");
        }

        private void TIME_Disposed(object sender, EventArgs e)
        {
            if (clockTimer != null)
            {
                clockTimer.Stop();
                clockTimer.Tick -= ClockTimer_Tick;
                clockTimer.Dispose();
                clockTimer = null;
            }
        }
    }
}
