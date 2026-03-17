//#define NO_CAM_TEST

/*
//FileName: ucCamera.cs
//FileType: Visual C# Source file
//Author : Jason Jay
//Created On : 2022.10.13 
//Last Modified On : 2023.5.10 
//Copy Rights : ShinBo Ltd.
//Description : Class for Star Camera ASI2600MMPro, control interface  DLL's given.
*/
using Emgu.CV.OCR;
using log4net;
using log4net.Util;
using NSLR_ObservationControl.Util;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static NSLR_ObservationControl.Subsystem.ASICameraDll2;
using static NSLR_ObservationControl.Util.RawBitmap;

namespace NSLR_ObservationControl.Module
{
    public partial class starCAM_ASI2600MMPro : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        Subsystem.ASICameraDll2 asicamera = new Subsystem.ASICameraDll2();

        const string THIS = "starCAM";
        protected int cooler_onoff;
        protected int target_temperature;
        protected int gain;
        protected int exposureTime;
        protected int brightness;
        protected int gamma;
        protected int flip;

        int ID = 0;  //우선 1번 CAM 만

        int sensor_temperature;
        public int cameras;
        string property_name;
        string resolution;

        public ASI_CAMERA_INFO pASICameraInfo;
        ASI_BAYER_PATTERN bayerPattern;
        ASI_EXPOSURE_STATUS exp_status;
        ASI_CONTROL_CAPS caps;
        ASI_CONTROL_TYPE control_types;

        string supportedBins;
        string supportedVideoFormat;

        IntPtr ImageBuf;  //For Preview
        ASI_IMG_TYPE IMG_TYPE = ASI_IMG_TYPE.ASI_IMG_RAW8;
        ///////////////////////////////////////////////////////////////////////////
        ///  2024.5.23 Available data
        private int ROI_X = 3124;  //6248  3124  1562  781  390  195  97
        private int ROI_Y = 2088;  //4176  2088  1044  522  261  130  65  
        private int _WIDTH = 3124;
        private int _HEIGHT = 2088;
        /////////////////////////////////////////////////////////////////////////// 
        private int START_POS_X = 0;
        private int START_POS_Y = 0;
        private int BIN = 2;
        private int TARGET_TEMP = 170;
        private bool _running = false;

        private int DEFAULT_GAIN = 400;
        private int DEFAULT_EXPOSURE = 1000;
        private int DEFAULT_GAMMA = 15;
        private int DEFAULT_BRIGHTNESS = 15;


        private int WIDTH
        {
            get { return _WIDTH; }
            set { _WIDTH = value; }
        }
        private int HEIGHT
        {
            get { return _HEIGHT; }
            set { _HEIGHT = value; }
        }

        public bool running
        {
            get { return _running; }
            set { _running = value; } 
        }

        // //////////////////////////////////////////////
        private BackgroundWorker bgWorker;
        private BackgroundWorker bgWorkerPreview;
        // //////////////////////////////////////////////

       
        StarCAM_FullScreen fullScreenStarCam;


        public starCAM_ASI2600MMPro()
        {
            InitializeComponent();
        }



        public ASI_ERROR_CODE InitializeCamera()
        {
            ASI_ERROR_CODE result;

#if NO_CAM_TEST
            result = ASI_ERROR_CODE.ASI_SUCCESS;
#else

            result = Initialization();
            if (result != ASI_ERROR_CODE.ASI_SUCCESS)
            {
                MessageBox.Show("스타카메라 초기화 에러");
                return result;
            }
#endif
            ASISetControlValue(ID, ASI_CONTROL_TYPE.ASI_TARGET_TEMP, TARGET_TEMP); //200:

            #region ============ BackGroundWorks =================
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(BackgroundWorker1_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker1_RunWorkerCompleted);
            bgWorker.RunWorkerAsync();

            bgWorkerPreview = new BackgroundWorker();
            bgWorkerPreview.WorkerReportsProgress = true;
            bgWorkerPreview.WorkerSupportsCancellation = true;
            bgWorkerPreview.DoWork += new DoWorkEventHandler(bgWorkerPreview_DoWork);
            bgWorkerPreview.ProgressChanged += bgWorkerPreview_ProgressChanged;
            bgWorkerPreview.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorkerPreview_RunWorkerCompleted);
            #endregion

            Prepare_Preview(0);
            bgWorkerPreview.RunWorkerAsync();
            result = ASISetControlValue(ID, ASI_CONTROL_TYPE.ASI_GAIN, DEFAULT_GAIN);
            result = ASISetControlValue(ID, ASI_CONTROL_TYPE.ASI_EXPOSURE, DEFAULT_EXPOSURE);
            result = ASISetControlValue(ID, ASI_CONTROL_TYPE.ASI_BRIGHTNESS, DEFAULT_BRIGHTNESS);
            result = ASISetControlValue(ID, ASI_CONTROL_TYPE.ASI_GAMMA, DEFAULT_GAMMA);

            trackBar_Gain.Value = DEFAULT_GAIN;
            label_gain_value.Text = DEFAULT_GAIN.ToString();

            trackBar_Exposure.Value = DEFAULT_EXPOSURE;
            label_ExposureTime_value.Text = DEFAULT_EXPOSURE.ToString();

            trackBar_Brightness.Value = DEFAULT_BRIGHTNESS;
            label_brightness_value.Text = DEFAULT_BRIGHTNESS.ToString();

            trackBar_Gamma.Value = DEFAULT_GAMMA;
            label_Gamma_value.Text = DEFAULT_GAMMA.ToString();

            return result;
        }

        #region ============ BackGroundWorks =================
        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //Make a work delay 
            Thread.Sleep(1000);
            //e.Result = "This text was set safely by BackgroundWorker.";
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Console.WriteLine($"BackGrndWork_RunWorkerCompleted()...{e.Result.ToString()}");
            var control_value = SensorTemperature;
            var control_value1 = TargetTemperature;

            if (SensorTemperature > 220)
                Control_CoolerOnOff = 1;
            else
                Control_CoolerOnOff = 0;

            //Console.WriteLine($"{THIS} Check the Sensor temperature : {control_value / 10}°C   TargetTemperature : {control_value1}°C");
            label_temperature.Text = $"{control_value / 10}°C";

            bgWorker.RunWorkerAsync();
        }
        private void bgWorkerPreview_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            int i = 0;

            //label_temper.Text = SensorTemperature.ToString();
            //Make a work delay
            Thread.Sleep(50);

            while (true)//for (int i = 0; i <= 100; i++)
            {
               // if (bgWorkerPreview.CancellationPending == true)
               if(worker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                worker.ReportProgress(i++);
                System.Threading.Thread.Sleep(300);
                if (i > 100)
                    i = 0;
            }
            //e.Result = 42;
            //Console.WriteLine($"backgroundWorker_preview_DoWork()   CheckBox Preview ");
        }

        void bgWorkerPreview_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Preview(0);
            //Console.WriteLine($"bgWorkerPreview_ProgressChanged()  Working {e.ProgressPercentage}%");
        }

        private void bgWorkerPreview_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Console.WriteLine($"bgWorkerPreview_RunWorkerCompleted()  Preview [ (checkBox1.Checked) {_running} ] / Worker Cancelation {e.Cancelled}");
        }
        #endregion


        public ASI_CAMERA_INFO GetCameraProperty()
        {
            return pASICameraInfo;
        }


        #region ============ StarCAM Control ============
        public int Control_CoolerOnOff
        {
            get { return cooler_onoff; }
            set
            {
                ASISetControlValue(ID, ASI_CONTROL_TYPE.ASI_COOLER_ON, value);
                cooler_onoff = ASIGetControlValue(ID, ASI_CONTROL_TYPE.ASI_COOLER_ON);
            }
        }
        public int Control_Gain
        {
            get { return gain; }
            set
            {
                ASISetControlValue(ID, ASI_CONTROL_TYPE.ASI_GAIN, value);
                gain = ASIGetControlValue(ID, ASI_CONTROL_TYPE.ASI_GAIN);
            }
        }

        public int Control_ExposureTime
        {
            //The Shorter the value, the faster the fps achieved.
            // FPS = 1000 / exposure Time(in millisecond, ms)
            // 4 Example,  20ms provides 1000 / 20 = 50FPS 
            get { return exposureTime; }
            set
            {
                ASISetControlValue(ID, ASI_CONTROL_TYPE.ASI_EXPOSURE, value);
                exposureTime = ASIGetControlValue(ID, ASI_CONTROL_TYPE.ASI_EXPOSURE);
            }
        }

        public int Control_Brightness
        {
            // Brightness(offset) 
            // Offset value  added to the output data to avoid any data negative.
            // Y
            get { return exposureTime; }
            set
            {
                ASISetControlValue(ID, ASI_CONTROL_TYPE.ASI_BRIGHTNESS, value);
                brightness = ASIGetControlValue(ID, ASI_CONTROL_TYPE.ASI_BRIGHTNESS);
            }
        }

        public int Control_Gamma
        {
            get { return exposureTime; }
            set
            {
                ASISetControlValue(ID, ASI_CONTROL_TYPE.ASI_GAMMA, value);
                gamma = ASIGetControlValue(ID, ASI_CONTROL_TYPE.ASI_GAMMA);
                Console.WriteLine($"gamma = {gamma.ToString()}");
            }
        }



        public int Control_Flip
        {
            get { return flip; }
            set
            {
                ASISetControlValue(ID, ASI_CONTROL_TYPE.ASI_FLIP, value);
                Console.WriteLine($"Flip Set {value}");
                flip = ASIGetControlValue(ID, ASI_CONTROL_TYPE.ASI_FLIP);
                Console.WriteLine(THIS, $"Flip Get {flip}");
            }
        }

        public int SensorTemperature
        {
            get
            {
                sensor_temperature = ASIGetControlValue(ID, ASI_CONTROL_TYPE.ASI_TEMPERATURE);
                return sensor_temperature;
            }
        }

        public int TargetTemperature
        {
            get
            {
                target_temperature = ASIGetControlValue(ID, ASI_CONTROL_TYPE.ASI_TARGET_TEMP);
                //Log(LOG.I, THIS, $"[CMD] ({target_temperature}) <--- ASIGetControlValue()......");
                //Console.WriteLine($"[CAM] GET teget_temperature ... {target_temperature}");
                return target_temperature;
            }
            set
            {
                ASISetControlValue(ID, ASI_CONTROL_TYPE.ASI_TARGET_TEMP, value);
                target_temperature = ASIGetControlValue(ID, ASI_CONTROL_TYPE.ASI_TARGET_TEMP);
                //Log(LOG.I, THIS, $"[CMD] ({target_temperature}) <--- ASISetControlValue()......");
                //Console.WriteLine($"[CAM] PUT teget_temperature ... {target_temperature}");
            }
        }
        #endregion


        #region   ===== PREVIEW  =======            
        public ASI_ERROR_CODE Prepare_Preview(int cam_id)
        {
            ASI_ERROR_CODE result;
            //int iDropFrame;
            //int image_count = 0;

#if NO_CAM_TEST

            result = ASI_ERROR_CODE.ASI_SUCCESS;
            Console.WriteLine($"[Prepare_Preview] ({result}) ");
#else
            result = ASISetROIFormat(cam_id, ROI_X, ROI_Y, BIN, IMG_TYPE);
            //Console.WriteLine($"[CMD] ({result}) <--- ASISetROIFormat({cam_id} {ROI_X}, {ROI_Y} , bin2 ,  {IMG_TYPE})......");

            result = ASIStartVideoCapture(cam_id);
            //Console.WriteLine($"[CMD] ({result}) <--- ASIStartVideoCapture({cam_id}......");
#endif
            return result;
        }

        public void Preview(int cam_id)
        {
            ASI_ERROR_CODE result;
            int size = WIDTH * HEIGHT;
            IntPtr imageBuf = Marshal.AllocCoTaskMem(size);

            try
            {
                int exposureTime = ASIGetControlValue(cam_id, ASI_CONTROL_TYPE.ASI_EXPOSURE);
                exposureTime /= 1000;
                result = ASIGetVideoData(cam_id, imageBuf, size, exposureTime * 2 + 500);

                if (result == ASI_ERROR_CODE.ASI_SUCCESS)
                {
                    using (Mat mat = new Mat(HEIGHT, WIDTH, MatType.CV_8U, imageBuf))
                    {
                        Bitmap bmp = BitmapConverter.ToBitmap(mat);

                        // UI 스레드에서 이미지 갱신
                        if (pictureBox_starCAM.InvokeRequired)
                        {
                            pictureBox_starCAM.Invoke(new Action(() =>
                {
                    if (pictureBox_starCAM.Image != null)
                        pictureBox_starCAM.Image.Dispose();
                    pictureBox_starCAM.Image = bmp;

                    if (fullScreenMode)
                    {
                        if (fullScreenStarCam.Picture != null)
                            fullScreenStarCam.Picture.Dispose();
                        fullScreenStarCam.Picture = (Bitmap)bmp.Clone();
                    }
                }));
                        }
                        else
                        {
                            if (pictureBox_starCAM.Image != null)
                                pictureBox_starCAM.Image.Dispose();
                            pictureBox_starCAM.Image = bmp;

                            if (fullScreenMode)
                            {
                                if (fullScreenStarCam.Picture != null)
                                    fullScreenStarCam.Picture.Dispose();
                                fullScreenStarCam.Picture = (Bitmap)bmp.Clone();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
              
            }
            finally { Marshal.FreeCoTaskMem(imageBuf); }
        }

        private Bitmap make_random_bitmap(int width, int height)
        {
            Bitmap StarCamBmp = new Bitmap(width, height);

            Random rand = new Random();
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //generate random ARGB value
                    int a = rand.Next(256);
                    int r = rand.Next(256);
                    int g = rand.Next(256);
                    int b = rand.Next(256);
                    //set ARGB value
                    StarCamBmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }
            return StarCamBmp;
        }
        public void starcamDispose()
        {
            if (bgWorkerPreview != null)
            {
                if(bgWorkerPreview != null)
                {
                    bgWorkerPreview.CancelAsync();
                    while (bgWorkerPreview.IsBusy)
                    {
                        Application.DoEvents();
                        Thread.Sleep(10);
                    }
                }
                bgWorkerPreview.DoWork -= bgWorkerPreview_DoWork;
                bgWorkerPreview.ProgressChanged -= bgWorkerPreview_ProgressChanged;
                bgWorkerPreview.RunWorkerCompleted -= bgWorkerPreview_RunWorkerCompleted;
                bgWorkerPreview = null;
            }
            if (bgWorker != null)
            {
                bgWorker.DoWork -= BackgroundWorker1_DoWork;
                bgWorker.RunWorkerCompleted -= BackgroundWorker1_RunWorkerCompleted;
                bgWorker = null;
            }
        }


        public string DoCapture()
        {
            string file = @"Capture_" + DateTime.Now.ToString("MM-dd-yyyy_hh-mm-ss") + ".png";
            pictureBox_starCAM.Image.Save(Application.StartupPath + file, System.Drawing.Imaging.ImageFormat.Png);
            return file;
        }

        public void Stop_Preview()
        {
            ASI_ERROR_CODE result;

            bgWorkerPreview.CancelAsync();
            result = ASIStopVideoCapture(ID);
            //Console.WriteLine($"[CMD] ({result}) <--- ASIStartVideoCapture()......");
        }

        private ASI_ERROR_CODE Initialization()
        {
            ASI_ERROR_CODE result;

            cameras = asicamera.ASIGetNumOfConnectedCameras();
            pASICameraInfo.NumOfCameras = cameras;
            if (cameras < 1)
            {
                //MessageBox.Show("There's No Camera !!");
                log.Debug("There's No Camera !!");
                return ASI_ERROR_CODE.ASI_ERROR_INVALID_INDEX;
            }
            log.Debug($"카메라 {cameras} 개 인식됨!!");
            // Device Info
            log.Info("---------------------------------------------------------------------------------------------------------------------------------------------------");
            for (int i = 0; i < cameras; i++)
            {
                result = ASIGetCameraProperty(out pASICameraInfo, i);//per cam index
                property_name = Encoding.Default.GetString(pASICameraInfo.name);
                ID = pASICameraInfo.CameraID;
                log.Info($"Get Property CamID : [{ID}] {property_name} ");
                log.Info("---------------------------------------------------------------------------------------------------------------------------------------------------");
                resolution = $"[{pASICameraInfo.MaxWidth}] x [{pASICameraInfo.MaxHeight}]";
                log.Info($"Get Property MaxWidth x MaxHeight :{resolution}");

                bayerPattern = pASICameraInfo.BayerPattern;
                log.Info($"Get Property ColorCam? : [{pASICameraInfo.IsColorCam is ASI_BOOL.ASI_TRUE}]");
                log.Info($"Get Property BayerPattern : [{bayerPattern}]");
                supportedBins = string.Join(" ", pASICameraInfo.SupportedBins);
                log.Info($"Get Property SupportedBins : {supportedBins}");//SupportedBins
                supportedVideoFormat = string.Join("  ", pASICameraInfo.SupportedVideoFormat);
                log.Info($"Get Property SupportedVideoFormat : {supportedVideoFormat}");//supportedVideoFormat
                log.Info($"Get Property Pixel Size  {pASICameraInfo.PixelSize}");
                log.Info("---------------------------------------------------------------------------------------------------------------------------------------------------");

            //#1 Open the Camera
                log.Info("\n");
                result = ASIOpenCamera(ID);
                log.Info($"[INITIAL] ({result}) <--- ASIOpenCamera(0)");

            //#2 Init the Camera
                result = ASIInitCamera(ID);
                log.Info($"[INITIAL] ({result}) <--- ASIInitCamera(0)");

            //#3 ASIGetNumOfControls
                result = ASIGetNumOfControls(ID, out int numberOfControls);
                log.Info("----------------------------------------------------------------------------");
                log.Info($"[INITIAL] ({result}) <--- ASIGetNumOfControls().....[Number Of Controls] ({numberOfControls})");
                log.Info("----------------------------------------------------------------------------");
            //[CMD] (ASI_SUCCESS) ASIGetNumOfControls().......[Number Of Controls] (15)

            //#4
            int control_value;
            for (int num = 0; num < numberOfControls; num++)
            {
                    result = ASIGetControlCaps(ID, num, out caps);
                    control_value = ASIGetControlValue(ID, (ASI_CONTROL_TYPE)num);
                    log.Info($"[INITIAL] ({result}) <--- ASIGetControlCaps().......[Control #{num}] ({caps.Description} : {control_value} ])     (** default : {caps.DefaultValue}   Range : {caps.MinValue} ~ {caps.MaxValue})");
            }
            //// Result ////
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #0] (Gain : [ 120 ])     (** default : 200   Range : 0 ~ 700)
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #1] (Exposure Time(us) : [ 10000 ])     (** default : 10000   Range : 32 ~ 2000000000)
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #2] (offset : [ 50 ])     (** default : 1   Range : 0 ~ 240)
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #3] (The total data transfer rate percentage : [ 0 ])     (** default : 50   Range : 40 ~ 100)
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #4] (Flip: 0->None 1->Horiz 2->Vert 3->Both : [ 0 ])     (** default : 0   Range : 0 ~ 3)
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #5] (Auto exposure maximum gain value : [ 1 ])     (** default : 350   Range : 0 ~ 700)
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #6] (Auto exposure maximum exposure value(unit ms) : [ 100 ])     (** default : 100   Range : 1 ~ 60000)
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #7] (Auto exposure target brightness value : [ 0 ])     (** default : 100   Range : 50 ~ 160)
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #8] (Is hardware bin2:0->No 1->Yes : [ 0 ])     (** default : 0   Range : 0 ~ 1)
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #9] (Is high speed mode:0->No 1->Yes : [ 0 ])     (** default : 0   Range : 0 ~ 1)
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #10] (Sensor temperature(degrees Celsius) : [ 350 ])     (** default : 20   Range : -500 ~ 1000)
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #11] (Cooler power percent : [ 30000 ])     (** default : 0   Range : 0 ~ 100)
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #12] (Target temperature(cool camera only) : [ 100 ])     (** default : 0   Range : -40 ~ 30)
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #13] (turn on/off cooler(cool camera only) : [ 0 ])     (** default : 0   Range : 0 ~ 1)
            //[CMD] (ASI_SUCCESS) ASIGetControlCaps().......[Control #14] (turn on/off anti dew heater(cool camera only) : [ 0 ])     (** default : 0   Range : 0 ~ 1)

            //  Set image size and format --> ASISetROIFormat
            result = ASISetROIFormat(ID, ROI_X, ROI_Y, BIN, IMG_TYPE);
                log.Info($"[INITIAL] ({result}) <--- ASISetROIFormat().....");

            //  Set start position when ROI --> ASISetStartPos
            result = ASISetStartPos(ID, START_POS_X, START_POS_Y);
                log.Info($"[INITIAL] ({result}) <--- ASISetStartPos().....");
            }

            //result = Prepare_Preview();
            //if(result == ASI_ERROR_CODE.ASI_SUCCESS)
            //{
            //    _running = true;
            //    Console.WriteLine("----ASI poreview  success");
            //}
            _running = true;
            return ASI_ERROR_CODE.ASI_SUCCESS;
        }
#endregion

        private void btnCapture_Click(object sender, EventArgs e)
        {
            //Prepare_Preview();
            //bgWorkerPreview.RunWorkerAsync();
            
            if (_running)
            {
                Stop_Preview();
            }
            var file = DoCapture();
            System.Diagnostics.Process.Start("explorer.exe", Application.StartupPath + file);
        }


        private void pictureBox_starCAM_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int centerX = pictureBox_starCAM.Width / 2;
            int centerY = pictureBox_starCAM.Height / 2;
            int markerSize = 40;

            g.DrawLine(Pens.Yellow, centerX, centerY - markerSize / 2, centerX, centerY + markerSize / 2);
            g.DrawLine(Pens.Yellow, centerX - markerSize / 2, centerY, centerX + markerSize / 2, centerY);
        }

        private void trackBar_Gain_MouseUp(object sender, MouseEventArgs e)
        {
            Control_Gain = trackBar_Gain.Value;
            label_gain_value.Text = $"{gain.ToString()}";
            log.Debug($"trackBar_Gain_MouseUp({gain}) ");
        }

        private void trackBar_Exposure_MouseUp(object sender, MouseEventArgs e)
        {
            Control_ExposureTime = trackBar_Exposure.Value;
            label_ExposureTime_value.Text = $"{exposureTime.ToString()}";
            //label_FPS.Text = $"FPS {(Control_ExposureTime/1000).ToString()}";
            log.Debug($"trackBar_Exposure_MouseUp({exposureTime}) ");
        }

        private void trackBar_Brightness_MouseUp(object sender, MouseEventArgs e)
        {
            Control_Brightness = trackBar_Brightness.Value;
            label_brightness_value.Text = $"{brightness.ToString()}";
            log.Debug($"trackBar_Brightness_MouseUp({brightness}) ");
        }

        private void trackBar_Gamma_MouseUp(object sender, MouseEventArgs e)
        {
            Control_Gamma = trackBar_Gamma.Value;
            label_Gamma_value.Text = $"{gamma.ToString()}";
            log.Debug($"trackBar_Gamma_MouseUp({gamma}) ");
        }

        bool fullScreenMode = false;
        private void pictureBox_starCAM_DoubleClick(object sender, EventArgs e)
        {            
            fullScreenStarCam = new StarCAM_FullScreen();
            //fullScreenStarCam.Picture = make_random_bitmap(1920,1080);
            fullScreenStarCam.Show();
            fullScreenMode = true;

        }
    }
}
