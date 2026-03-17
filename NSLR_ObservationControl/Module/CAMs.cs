using OpenCvSharp.Extensions;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NSLR_ObservationControl.IRDisplay.MyDisplayRequestListener;
using static NSLR_ObservationControl.IRDisplay;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Timers;
using System.IO;
using System.Text.RegularExpressions;
using mv.impact.acquire.display;
using mv.impact.acquire.helper;
using mv.impact.acquire;
using PvDotNet;
using static NSLR_ObservationControl.Subsystem.ASICameraDll2;

namespace NSLR_ObservationControl.Module
{
    public partial class CAMs : UserControl
    {
        public static CAMs cAMs = new CAMs();

        private System.Drawing.Point LastPoint;
        private Bitmap img;

        private double ratio = 1.0F;
        private System.Drawing.Point imgPoint;
        private Rectangle imgRect;
        private System.Drawing.Point clickPoint;
        readonly Pen gridLineColor = Pens.Yellow;

        private System.Windows.Forms.Timer timer;
        private int counter = 0;

        //public MWIR mwir = new MWIR();
        public EMCCD emccd = new EMCCD();

        public starCAM_ASI2600MMPro starCAM = new starCAM_ASI2600MMPro();
        ASI_ERROR_CODE starCAMstate = ASI_ERROR_CODE.ASI_ERROR_END;

        public CAMs()
        {
            InitializeComponent();
            // img = new Bitmap(Properties.Resources.sky_cam_preview);
            //imgPoint = new System.Drawing.Point(pictureBox_preview.Width / 2, pictureBox_preview.Height / 2);
            //imgRect = new Rectangle(0, 0, pictureBox_preview.Width, pictureBox_preview.Height);
            // ratio = 1.0;
            // clickPoint = imgPoint;

            // pictureBox_preview.Invalidate();
            //   pictureBox_preview.MouseWheel += new MouseEventHandler(pictureBox_preview_MouseWheel);

            //emccd = new EMCCD();
            //mwir = new MWIR();
        }
        
        private void CAMs_Load(object sender, EventArgs e)
        {
        }
        #region picturebox 제어
        private void pictureBox_preview_MouseWheel(object sender, MouseEventArgs e)
        {
            int lines = e.Delta * SystemInformation.MouseWheelScrollLines / 120;
            PictureBox pb = (PictureBox)sender;

            if (lines > 0)
            {
                ratio *= 1.1F;
                if (ratio > 100.0) ratio = 100.0f;

                imgRect.Width = (int)Math.Round(pictureBox_preview.Width * ratio);
                imgRect.Height = (int)Math.Round(pictureBox_preview.Height * ratio);
                imgRect.X = -(int)Math.Round(1.1F * (imgPoint.X - imgRect.X) - imgPoint.X);
                imgRect.Y = -(int)Math.Round(1.1F * (imgPoint.Y - imgRect.Y) - imgPoint.Y);
                Console.WriteLine($"pictureBox1_MouseWheel().+..lines:{lines} ratio:{ratio} img X,Y {imgRect.X},{imgRect.Y}");
            }
            else if (lines < 0)
            {
                ratio *= 0.9F;
                if (ratio < 1) ratio = 1;

                imgRect.Width = (int)Math.Round(pictureBox_preview.Width * ratio);
                imgRect.Height = (int)Math.Round(pictureBox_preview.Height * ratio);
                imgRect.X = -(int)Math.Round(0.9F * (imgPoint.X - imgRect.X) - imgPoint.X);
                imgRect.Y = -(int)Math.Round(0.9F * (imgPoint.Y - imgRect.Y) - imgPoint.Y);
                Console.WriteLine($"pictureBox1_MouseWheel().-..lines:{lines} ratio:{ratio} img X,Y {imgRect.X},{imgRect.Y}");
            }

            if (imgRect.X > 0) imgRect.X = 0;
            if (imgRect.Y > 0) imgRect.Y = 0;
            if (imgRect.X + imgRect.Width < pictureBox_preview.Width) imgRect.X = pictureBox_preview.Width - imgRect.Width;
            if (imgRect.Y + imgRect.Height < pictureBox_preview.Height) imgRect.Y = pictureBox_preview.Height - imgRect.Height;
            pictureBox_preview.Invalidate();
        }




    private void pictureBox_preview_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int centerX = pictureBox_preview.Width / 2;
            int centerY = pictureBox_preview.Height / 2;
            int markerSize = 10;

            // 중심 좌표를 사용하여 노란색으로 채워진 원 그리기
            //  g.FillEllipse(Brushes.Yellow, centerX - markerSize / 2, centerY - markerSize / 2, markerSize, markerSize);

            // 십자모양으로 중심 표시 그리기
            g.DrawLine(Pens.Yellow, centerX, centerY - markerSize / 2, centerX, centerY + markerSize / 2);
            g.DrawLine(Pens.Yellow, centerX - markerSize / 2, centerY, centerX + markerSize / 2, centerY);

        }




        private void pictureBox_preview_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                imgRect.X = imgRect.X + (int)Math.Round((double)(e.X - clickPoint.X) / 9);
                //imgRect.X = imgRect.X + e.X - clickPoint.X;
                if (imgRect.X >= 0) imgRect.X = 0;
                if (Math.Abs(imgRect.X) >= Math.Abs(imgRect.Width - pictureBox_preview.Width)) imgRect.X = -(imgRect.Width - pictureBox_preview.Width);

                imgRect.Y = imgRect.Y + (int)Math.Round((double)(e.Y - clickPoint.Y) / 9);
                //imgRect.Y = imgRect.Y + e.Y - clickPoint.Y;
                if (imgRect.Y >= 0) imgRect.Y = 0;
                if (Math.Abs(imgRect.Y) >= Math.Abs(imgRect.Height - pictureBox_preview.Height)) imgRect.Y = -(imgRect.Height - pictureBox_preview.Height);
            }
            else
            {
                LastPoint = e.Location;
            }
            pictureBox_preview.Invalidate();
            //Console.WriteLine($"PictureBox1_MouseMove()...{imgRect.X},{imgRect.Y}");           
        }

        private void pictureBox_preview_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox_preview_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                clickPoint = new System.Drawing.Point(e.X, e.Y);
            }
            pictureBox_preview.Invalidate();
            //Console.WriteLine($"PictureBox1_MouseMove()...{e.X},{e.Y}");

        }
        #endregion
        RecordManagement_AccessHistory recordManagement_AccessHistory = new RecordManagement_AccessHistory();



        private void deviceSelected_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                switch (Convert.ToInt32(btn.Tag))
                {
                    case 1:
                        // MWIR 영상 송출 문제 >> 임시용
                        MainForm.mainForm.mwir.MWIR_DisplayStop();


                        SwitchToUserControl(MainForm.mainForm.mwir);

                        // MWIR 영상 송출 문제 >> 임시용
                        MainForm.mainForm.mwir.MWIR_DisplayStart();
                        
                        break;
                    case 2:
                        emccd.Disconnect();
                  
                        SwitchToUserControl(emccd);
                        emccd.connectCamera();
                        break;


                    case 3:
                        starCAM.starcamDispose();
                        SwitchToUserControl(starCAM);
                     //   if (starCAMstate != ASI_ERROR_CODE.ASI_SUCCESS)
                     //   {
                      //      if (starCAM.running == false)
                       //     {
                                starCAMstate = starCAM.InitializeCamera();
                        //    }
                      //  }
                        break;

                    case 4:
                        break;

                }
            }
            catch { }
        }
        private void SwitchToUserControl(UserControl userControl)
        {
            /*            panel1.Controls.Clear();
                        panel1.Controls.Add(userControl);
                        userControl.Dock = DockStyle.Fill;*/
            foreach (Control control in panel1.Controls)
            {
                    panel1.Controls.Remove(control);
            }
            panel1.Controls.Add(userControl);
            userControl.Dock = DockStyle.Fill;
           // panel2.Visible=true;
        }


        private void CAMs_Leave(object sender, EventArgs e)
        {
            //MWIR.rp.acquisitionStop();
            //mwir.Dispose();
            // emccd.Dispose();
            //emccd.OnLinkDisconnected(emccd.mDevice);
            
        }
        UserControl TMSUsercontrol;
        public static double UserOffsetTick { get; }
        private void Mount_Left_Click(object sender, EventArgs e)
        {
            NSLR_ObservationControl.Module.Observation_TMS.azUserOffset -= NSLR_ObservationControl.Module.Observation_TMS.UserOffsetTick;
            Console.WriteLine($"AzUserOffset : {NSLR_ObservationControl.Module.Observation_TMS.azUserOffset}");
        }

        private void Mount_Right_Click(object sender, EventArgs e)
        {
            NSLR_ObservationControl.Module.Observation_TMS.azUserOffset += NSLR_ObservationControl.Module.Observation_TMS.UserOffsetTick;
            Console.WriteLine($"AzUserOffset : {NSLR_ObservationControl.Module.Observation_TMS.azUserOffset}");
        }

        private void Mount_Down_Click(object sender, EventArgs e)
        {
            NSLR_ObservationControl.Module.Observation_TMS.elUserOffset -= NSLR_ObservationControl.Module.Observation_TMS.UserOffsetTick;
            Console.WriteLine($"elUserOffset : {NSLR_ObservationControl.Module.Observation_TMS.elUserOffset}");
        }
        private bool isDragging = false;
        private System.Drawing.Point lastPoint;
        private void Mount_Up_Click(object sender, EventArgs e)
        {
            NSLR_ObservationControl.Module.Observation_TMS.elUserOffset += NSLR_ObservationControl.Module.Observation_TMS.UserOffsetTick;
            Console.WriteLine($"elUserOffset : {NSLR_ObservationControl.Module.Observation_TMS.elUserOffset}");
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            isDragging = true;
            lastPoint = e.Location;
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging=false;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if(isDragging)
            {
                int deltaX = e.Location.X - lastPoint.X;
                int deltaY = e.Location.Y - lastPoint.Y;


                lastPoint = e.Location;
            }
        }
        private double AddAzOffSet = 0;
        private double AddElOffSet = 0;
        private bool isControlActive = false;

        private void UpdateOffsets()
        {
            // AddAzOffSet과 AddElOffSet 값을 UI 또는 로직에 반영하는 코드 작성
            Console.WriteLine($"Azimuth Offset: {AddAzOffSet}, Elevation Offset: {AddElOffSet}");
        }

        private void ToggleControl()
        {
            isControlActive = !isControlActive;
        }

        private void HandleKeyPress(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    AddAzOffSet -= UserOffsetTick; 
                    break;
                case Keys.Right:
                    AddAzOffSet += UserOffsetTick;
                    break;
                case Keys.Up:
                    AddElOffSet += UserOffsetTick; 
                    break;
                case Keys.Down:
                    AddElOffSet -= UserOffsetTick; 
                    break;
            }
            UpdateOffsets(); 
        }

        private void CAMs_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyPress(e);
            e.Handled = true;

        }
    }
}
