using mv.impact.acquire.helper;
using mv.impact.acquire;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NSLR_ObservationControl.IRDisplay;
using System.Diagnostics;
using System.Threading;
using OpenCvSharp;
using System.Timers;
using OpenCvSharp.Extensions;
using System.Diagnostics.Metrics;
using mv.impact.acquire.examples.helper;
using PvDotNet;

namespace NSLR_ObservationControl.Module
{
    public partial class MWIR : UserControl
    {
        //public static MWIR mWIR = new MWIR();

        private RequestProvider rp;
        MyDisplayRequestListener displayListener;

        // private MyDisplayRequestListener displayListener;

        private VideoWriter videoWriter;

        public Device pDev;
        private int counter = 0;
        public MWIR()
        {
            InitializeComponent();

            mv.impact.acquire.LibraryPath.init();

            try
            {
                pDev = DeviceManager.getDevice(1); // MWIR 영상 시작 시
            }
            catch
            {
                //MessageBox.Show("No Found Device");
            }
            //rp = new RequestProvider(pDev);
        }

        
        private void MWIR_Load(object sender, EventArgs e)
        {
           
           mv.impact.acquire.LibraryPath.init();
            
            /*
            if (pDev == null)
            {
                MessageBox.Show("Unable to continue!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            */
            
            try
            {
                /*               if (rp != null)
               {
                   rp = null;
                   rp.onRequestReady -= displayListener.requestReady;
               }*/


                //rp = new RequestProvider(pDev);
                //displayListener = new MyDisplayRequestListener(pictureBox_preview);
                //rp.onRequestReady += displayListener.requestReady;
                //rp.acquisitionStart();

                //MWIR_DisplayStart();
            }
            catch (ImpactAcquireException ex)
            {
                //MessageBox.Show($"An error occurred while opening the device {pDev.serial} (error code: {ex.Message}).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        ///////////////////////////////////////////////// MWIR 영상 송출 문제 >> 수정안////////////////////////////////////////////////////


        public void MWIR_DisplayStart() // MWIR 영상 시작 시
        {
            try
            {
                if (pDev != null)
                {
                    rp = new RequestProvider(pDev);
                    displayListener = new MyDisplayRequestListener(pictureBox_preview);
                    rp.onRequestReady += displayListener.requestReady;
                    rp.acquisitionStart();
                }
                
            }
            catch (ImpactAcquireException ex)
            {
                MessageBox.Show("No Connect Debvice");
            }

            
        }

        public void MWIR_DisplayStop()  // MWIR 영상 종료시
        {
            if (rp != null)
            {
                rp.onRequestReady -= displayListener.requestReady;
                rp.acquisitionStop();
                rp = null;
            }
            
            if (displayListener != null)
            {
                displayListener.requestStop();
                displayListener = null;
            }

        }

        public void MWIR_DevClose() // DEV 초기화
        {
            if (pDev != null)
            {
                pDev.close();
                pDev = null;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        
        
        
        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private async void MWIRFunciton_Click(object sender, EventArgs e)
        {
            MyDisplayRequestListener irdisplay = new MyDisplayRequestListener(this.pictureBox_preview);
            Button btn = sender as Button;
            int tempValue;
           // if (!await semaphore.WaitAsync(0)) return;
            try
            {

                switch (Convert.ToInt32(btn.Tag))
                {
                    case 1:
                        irdisplay.RecvValue("GZP,", 1);
                        irdisplay.SendCommand("SZP," + irdisplay.result2 + ",0,");

                        break;
                    case 2:
                        irdisplay.RecvValue("GZP,", 2);
                        irdisplay.SendCommand("SZP," + irdisplay.result2 + ",0,");

                        break;
                        
                    case 3:
                        irdisplay.SendCommand("AFO,");
                        break;
                    case 4:
                        irdisplay.RecvValue("GFP,", 4);
                        irdisplay.SendCommand("SFP," + irdisplay.result + ",");
                        break;
                    case 5:
                        irdisplay.RecvValue("GFP,", 5);
                        irdisplay.SendCommand("SFP," + irdisplay.result + ",");
                        break;
                    case 6:
                        irdisplay.SendCommand("UN1,");
                        break;
                    case 7:
                        irdisplay.SendCommand("UN2,");
                        break;
                    case 8:
                        irdisplay.RecvValue("GIT", 8);
                        Debug.WriteLine(irdisplay.result);
                        irdisplay.SendCommand("INT " + irdisplay.result);
                        break;
                    case 9:
                        irdisplay.RecvValue("GIT", 9);

                        Debug.WriteLine(irdisplay.result);
                        irdisplay.SendCommand("INT " + irdisplay.result);
                        break;
                    case 10:
                        irdisplay.RecvValue("GCS,", 10);
                        irdisplay.SendCommand("DGA," + irdisplay.result2 + ",");
                        /*                if (device.ImageProcessing.GetSupportedParameter.Contains(Parameters.ImageGain))
                                        {
                                            device.ImageProcessing.Gain -= 0.1f;
                                        }
                                        else
                                        {
                                            Console.WriteLine("This device does not support ImageGain parameter.");
                                        }*/
                        
                        break;
                    case 11:
                        irdisplay.RecvValue("GCS,", 11);
                        irdisplay.SendCommand("DGA," + irdisplay.result2 + ",");
                        /*                if (ImageProcessing.GetSupportedParameter.Contains(Parameters.ImageGain))
                                        {
                                            ImageProcessing.Gain += 0.1f;
                                        }
                                        else
                                        {
                                            Console.WriteLine("This device does not support ImageGain parameter.");
                                        }*/
                        break;
                    case 12:
                        irdisplay.SendCommand("AAG,1,");

                        break;
                }
            }
            finally
            {
         //       semaphore.Release();
            }
        }


        private void videoSave_Click(object sender, EventArgs e)
        {
            videoWriter?.Release();
            videoWriter?.Dispose();

            // Start recording
            string outputPath = "E://output" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".avi";
            videoWriter = new VideoWriter(outputPath, FourCC.MJPG, 30, new OpenCvSharp.Size(1280, 720), true);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Bitmap copy = null;

            lock (pictureBox_preview)
            {
                if (pictureBox_preview.Image != null)
                {
                    copy = new Bitmap(pictureBox_preview.Image);
                }
            }

            if (copy != null)
            {
                using (var image = copy)
                {
                    var frame = BitmapConverter.ToMat(image);
                    videoWriter.Write(frame);
                    counter++;
                }
            }

            if (counter >= 300)
            {
                timer1.Stop();
                videoWriter.Release();
                videoWriter = null;
                counter = 0;
            }
        }

        private void MWIR_Leave(object sender, EventArgs e)
        {
        }

        private void pictureBox_preview_Click(object sender, EventArgs e)
        {
            System.Drawing.Point clickPoint = pictureBox_preview.PointToClient(Cursor.Position);

            int xInPictureBox = clickPoint.X - (pictureBox_preview.Width / 2);
            int yInPictureBox = (pictureBox_preview.Height / 2) - clickPoint.Y;

            CalculateDistance(clickPoint);
        }
        private void CalculateDistance(System.Drawing.Point point)
        {
            int centerX = pictureBox_preview.Width / 2;
            int centerY = pictureBox_preview.Height / 2;

            double distance = Math.Sqrt(Math.Pow(point.X - centerX, 2) + Math.Pow(point.Y - centerY, 2));
            MessageBox.Show($"중심점까지의 거리 : {distance}");
        }

        private void pictureBox_preview_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int centerX = pictureBox_preview.Width / 2;
            int centerY = pictureBox_preview.Height / 2;
            int markerSize = 10;

            g.DrawLine(Pens.Yellow, centerX, centerY - markerSize / 2, centerX, centerY + markerSize / 2);
            g.DrawLine(Pens.Yellow, centerX - markerSize / 2, centerY, centerX + markerSize / 2, centerY);
        }
    }
}
