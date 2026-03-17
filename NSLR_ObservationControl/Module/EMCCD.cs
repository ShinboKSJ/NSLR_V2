using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json.Linq;
using PvDotNet;
using PvGUIDotNet;

namespace NSLR_ObservationControl.Module
{
    public partial class EMCCD : UserControl
    {
        //PvSerialBridge pvSerial;
        PvDeviceSerialPort pvSerial; 

        int ditgitalGAIN_value;
        int emGAIN_value;
        byte[] CMD_EM_GAIN1 = new byte[] { 0x53, 0x00, 0x03, 0x01, 0x01, 0x00, 0x50 }; //[5] MSB
        byte[] CMD_EM_GAIN2 = new byte[] { 0x53, 0x00, 0x03, 0x01, 0x02, 0x00, 0x50 }; //[5] LSB
        byte[] CMD_DIGITAL_GAIN1 = new byte[] { 0x53, 0x00, 0x03, 0x01, 0xc6, 0x00, 0x50 }; //[5] MSB
        byte[] CMD_DIGITAL_GAIN2 = new byte[] { 0x53, 0x00, 0x03, 0x01, 0xc7, 0x00, 0x50 }; //[5] LSB

        public EMCCD()
        {
            InitializeComponent();
            //InitializeEMCCDgain();
            // Handlers used to callbacks in the UI thread
                mDisconnectedHandler += new GenericHandler(OnDisconnected);
                mAcquisitionModeChangedHandler += new GenericHandler(OnAcquisitionModeChanged);
            // Create display thread, hook display event
            mDisplayThread = new PvDisplayThread();
            mDisplayThread.OnBufferDisplay += new OnBufferDisplay(OnBufferDisplay);
        }


        public int Control_DigitalGAIN
        {
            get { return ditgitalGAIN_value; }
            /*
            set
            {
                byte b0 = (byte)ditgitalGAIN_value;
                byte b1 = (byte)(ditgitalGAIN_value >> 8);
                CMD_DIGITAL_GAIN1[5] = b1;
                CMD_DIGITAL_GAIN2[5] = b0;
                Console.WriteLine($"set_em_gain({value})......b1[{b1}]  b0[{b0}]");                
                pvSerial.Write(CMD_DIGITAL_GAIN1);
                pvSerial.Write(CMD_DIGITAL_GAIN2);
                Console.WriteLine($"Read Serial.....[{pvSerial.Read(30)}]");
            }
            */
        }
        public int Control_EmGAIN
        {
            get { return emGAIN_value; }
            set
            {
                byte b0 = (byte)emGAIN_value;
                byte b1 = (byte)(emGAIN_value >> 8);
                CMD_EM_GAIN1[5] = b1;
                CMD_EM_GAIN1[5] = b0;
                Console.WriteLine($"set_em_gain({value})......b1[{b1}]  b0[{b0}]");
              //  pvSerial.SerialPort.
                pvSerial.Write(CMD_EM_GAIN1);
                pvSerial.Write(CMD_EM_GAIN1);
                Console.WriteLine($"Read Serial.....[{pvSerial.Read(30)}]");
            }
        }



        private void InitializeEMCCDgain()
        {
            pvSerial = new PvDeviceSerialPort();
            //pvSerial = new PvSerialBridge();
            //serialSetting.
            // PvSerialPortIPEngine
        }

        // Handler used to bring link disconnected event in the main UI thread
        private delegate void GenericHandler();
        private GenericHandler mDisconnectedHandler = null;
        // Handler used to bring acquisition mode changed event in the main UI thread
        private GenericHandler mAcquisitionModeChangedHandler = null;
        // Handler used to bring acquisition state manager callbacks in the main UI thread
        private GenericHandler mAcquisitionStateChangedHandler = null;
        // Main application objects: device, stream, pipeline
        public PvDevice mDevice = null;
        private PvStream mStream = null;
        private PvPipeline mPipeline = null;
        private PvCameraBridgeManagerForm mCameraBridgeManager = null;
        // Acquisition state manager
        private PvAcquisitionStateManager mAcquisitionManager = null;
        // Display thread
        private PvDisplayThread mDisplayThread = null;
        private static PvSystem mSystem = new PvSystem();

       // static string uniqueId = "00:11:1c:05:3c:34"; // this should be the unique ID of your device   //거창
        static string uniqueId = "00:11:1c:05:b7:c6"; 
        static PvDeviceInfo desiredDeviceInfo;

        private void EMCCD_Load(object sender, EventArgs e)
        {
            // connectCamera();
        }

        private void EMCCD_Leave(object sender, EventArgs e)
        {
             //Disconnect();
        }

        ////////////////////////// Device Serial Communication 기능추가 (Gain 및 ALC Control, Test용 Command Send 기능) ////////////////////////////////////////////////////////////////////

        byte[] get_register = { 0x53, 0x01, 0x03, 0x01, 0x05, 0x00, 0x50 };
        byte[] get_ALC1 = { 0x53, 0x01, 0x03, 0x01, 0x05, 0x23, 0x50 };
        byte[] get_ALC2 = { 0x53, 0x01, 0x03, 0x01, 0x05, 0x24, 0x50 };
        byte[] get_gain1 = { 0x53, 0x01, 0x03, 0x01, 0x05, 0xC6, 0x50 };
        byte[] get_gain2 = { 0x53, 0x01, 0x03, 0x01, 0x05, 0xC7, 0x50 };

        bool EMCCD_control = false;

        byte[] register_data = new byte[1];
        byte[] ALC_data = new byte[2];
        byte[] Gain_data = new byte[2];

        PvDeviceSerialPort IPort;
        Thread receiveThread;
        public void DeviceSerial_Connect()
        {
            IPort = new PvDeviceSerialPort();

            // 통신 Setting >> 추가여부 확인
            mDevice.Parameters.SetEnumValue("BulkSelector", "Bulk0");
            mDevice.Parameters.SetEnumValue("BulkMode", "UART");
            mDevice.Parameters.SetEnumValue("BulkBaudRate", "Baud115200");
            mDevice.Parameters.SetEnumValue("BulkNumOfStopBits", "One");
            mDevice.Parameters.SetEnumValue("BulkParity", "None");
            mDevice.Parameters.SetBooleanValue("BulkLoopback", false);

            // Port Open
            IPort.Open(mDevice, PvDeviceSerial.Bulk0);

            if (IPort.IsOpened == true)
            {
                UInt32 ISize = 32/*IPort.RxBufferSize*/;
                IPort.RxBufferSize = ISize * 2;

                
                receiveThread = new Thread(() =>
                {
                    Thread.Sleep(100);
                    while (true)
                    {
                        // Data Receive Part
                        if (IPort.RxBytesReady > 0)
                        {
                            byte[] receiveData = IPort.Read();

                            if (EMCCD_control == false)
                            {

                                if (receiveData.Length == 5)
                                {
                                    // Get Camera Control Register 0
                                    Array.Copy(receiveData, register_data, register_data.Length);
                                    this.Invoke(new System.Action(() =>
                                    {
                                        receive_txtbox.Clear();
                                        receive_txtbox.Text += ("0x" + register_data[0].ToString("x2") + "(register) - ");
                                        if ((register_data[0] & 0x02) != 0)    // 현재 : Auto gain is enabled
                                        {
                                         //   AGCAuto_btn.Text = "Auto Gain(Disable) / ALC(Disable)";
                                            AGCAuto_btn.Text = "00";

                                            // Gain Up/Down 비활성화
                                            gainSet1_btn.Enabled = false;
                                            gainSet2_btn.Enabled = false;
                                        }
                                        else    // 현재 : Auto gain is disabled
                                        {
                                          //  AGCAuto_btn.Text = "Auto Gain(Enable) / ALC(Enable)";
                                            AGCAuto_btn.Text = "00";

                                            // Gain Up/Down 활성화
                                            gainSet1_btn.Enabled = true;
                                            gainSet2_btn.Enabled = true;
                                        }
                                    }));

                                    // Get ALC level
                                    Array.Copy(receiveData, register_data.Length, ALC_data, 0, ALC_data.Length);
                                    this.Invoke(new System.Action(() =>
                                    {
                                        receive_txtbox.Text += ("0x" + ALC_data[0].ToString("x2") + "/0x" + ALC_data[1].ToString("x2") + "(ALC) - ");
                                    }));

                                    // Get Digital Gain
                                    Array.Copy(receiveData, register_data.Length + ALC_data.Length, 
                                        Gain_data, 0, Gain_data.Length);
                                    this.Invoke(new System.Action(() =>
                                    {
                                        receive_txtbox.Text += ("0x" + Gain_data[0].ToString("x2") + "/0x" + Gain_data[1].ToString("x2") + "(Gain)");
                                    }));

                                    EMCCD_control = true;
                                }
                                else
                                {
                                    MessageBox.Show("EMCCD Control 불가 >> Disconnect Device Serial!");
                                    DeviceSerial_Disconnect();
                                }
                                
                            }
                            else
                            {
                                this.Invoke(new System.Action(() =>
                                {
                                    receive_txtbox.Clear();

                                    for (int i = 0; i < receiveData.Length; i++)
                                    {
                                        receive_txtbox.Text += ("0x" + receiveData[i].ToString("x2") + " ");
                                    }
                                }));
                                 
                            }
                            
                        }

                    }
                    
                });

                // Get Camera Control Register 0
                DeviceSerial_Write(get_register);

                // Get ALC level
                DeviceSerial_Write(get_ALC1);
                DeviceSerial_Write(get_ALC2);

                // Get Digital Gain
                DeviceSerial_Write(get_gain1);
                DeviceSerial_Write(get_gain2);

                // Receive Thread 시작
                receiveThread.Start();

            }
            else
            {
                MessageBox.Show("eBUS : Device Serial Connect >> 실패");
            }

        }

        public void DeviceSerial_Disconnect()
        {
            if (receiveThread != null)
            {
                if (receiveThread.IsAlive)
                {
                    receiveThread.Abort();
                }
                receiveThread = null;
            }

            if (IPort != null && IPort.IsOpened)
            {
                IPort.Close();
                IPort = null;
            }

            EMCCD_control = false;
        }

        public void DeviceSerial_Write(byte[] bytes)
        {
            // Data Send Part
            uint CompletedData = IPort.Write(bytes);

            
            if (CompletedData != bytes.Length)  // Fail
            {
                MessageBox.Show("Data가 정상적으로 전송되지 않았습니다.");
            }
            else   // Succeed
            {
                
                receive_txtbox.Text =  "0x" + register_data[0].ToString("x2") + "(register) - " +
                    "0x" + ALC_data[0].ToString("x2") + "/0x" + ALC_data[1].ToString("x2") + "(ALC) - " +
                    "0x" + Gain_data[0].ToString("x2") + "/0x" + Gain_data[1].ToString("x2") + "(Gain)" + 
                    " >> Commend Succeed";
                
            }
            
        }

        private void EMCCDFunction_Click(object sender, EventArgs e)
        {
            if (EMCCD_control != true)
            {
                return;
            }

            Button btn = sender as Button;

            byte[] register_cmd = { 0x53, 0x00, 0x03, 0x01, 0x00, 0x00/*Data*/, 0x50 };
            byte[] ALC_data1 = { 0x53, 0x00, 0x03, 0x01, 0x23, 0x00/*Data*/, 0x50 };
            byte[] ALC_data2 = { 0x53, 0x00, 0x03, 0x01, 0x24, 0x00/*Data*/, 0x50 };
            byte[] Gain_data1 = { 0x53, 0x00, 0x03, 0x01, 0xC6, 0x00/*Data*/, 0x50 };
            byte[] Gain_data2 = { 0x53, 0x00, 0x03, 0x01, 0xC7, 0x00/*Data*/, 0x50 };

            switch (Convert.ToInt32(btn.Tag))
            {
                case 1: // 노출시간 Up
                    if (((ALC_data[0] % 16) < 15))
                    {
                        ALC_data[0] += (byte)0x01;
                    }
                    ALC_data1[5] = ALC_data[0];
                    DeviceSerial_Write(ALC_data1);  // MM : 2번째 M 변경

                    ALC_data2[5] = ALC_data[1];
                    DeviceSerial_Write(ALC_data2);  // LL : 변경소요 없음
                    break;

                case 2: // 노출시간 Down
                    if (((ALC_data[0] % 16) > 0))
                    {
                        ALC_data[0] -= (byte)0x01;
                    }
                    ALC_data1[5] = ALC_data[0];
                    DeviceSerial_Write(ALC_data1);  // MM : 2번째 M 변경

                    ALC_data2[5] = ALC_data[1];
                    DeviceSerial_Write(ALC_data2);  // LL : 변경소요 없음
                    break;

                case 3: // Gain Up
                    if ((Gain_data[1] / 16) < 15)
                    {
                        Gain_data[1] += (byte)0x10;
                    }
                    Gain_data1[5] = Gain_data[0];
                    DeviceSerial_Write(Gain_data1); // MM : 변경소요 없음

                    Gain_data2[5] = Gain_data[1];
                    DeviceSerial_Write(Gain_data2); // LL : 1번째 L 변경
                    break;
                case 4: // Gain Down
                    if ((Gain_data[1] / 16) > 0)
                    {
                        Gain_data[1] -= (byte)0x10;
                    }
                    Gain_data1[5] = Gain_data[0];
                    DeviceSerial_Write(Gain_data1); // MM : 변경소요 없음

                    Gain_data2[5] = Gain_data[1];
                    DeviceSerial_Write(Gain_data2); // LL : 1번째 L 변경
                    break;
                case 5: // AGC Auto
                    if ((register_data[0] & 0x02) != 0)    // 현재 : Auto gain is enabled
                    {
                        register_data[0] -= (byte)0x02;
                        register_cmd[5] = register_data[0];
                        DeviceSerial_Write(register_cmd);

                        AGCAuto_btn.Text = "00";

                        // Gain Up/Down 활성화
                        gainSet1_btn.Enabled = true;
                        gainSet2_btn.Enabled = true;
                    }
                    else    // 현재 : Auto gain is disabled
                    {
                        register_data[0] += (byte)0x02;
                        register_cmd[5] = register_data[0];
                        Gain_data[0] = (byte)0x01; Gain_data[1] = (byte)0x00;   // Auto Gain 변경 시, Gain 수치가 Default값으로 자동 변경됨
                        DeviceSerial_Write(register_cmd);

                        AGCAuto_btn.Text = "00";

                        // Gain Up/Down 비활성화
                        gainSet1_btn.Enabled = false;
                        gainSet2_btn.Enabled = false;
                    }
                    break;
            }

        }

        private void CommandSend_btn_Click(object sender, EventArgs e)
        {
            if (EMCCD_control != true)
            {
                return;
            }

            string[] datas = send_txtbox.Text.Split(' ');
            byte[] bytes = new byte[datas.Length];

            try
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = Convert.ToByte(datas[i], 16);
                }
            }
            catch
            {
                MessageBox.Show("잘못된 Command 형식입니다." + "\r\n" + "Ex) 53 01 03 01 05 00 50");
                return;
            }
            
            DeviceSerial_Write(bytes);
        }

        public void connectCamera()
        {
            if (mDevice == null)
            {
                Console.WriteLine("mDevice null");
            }
            try
            {
                PvDeviceGEV.SetIPConfiguration(uniqueId, "192.168.10.251", "255.255.255.0", "0.0.0.0");
                desiredDeviceInfo = mSystem.FindDevice(uniqueId);
                if (desiredDeviceInfo == null)
                {
                    //MessageBox.Show("Device not found");
                    return;
                }
                //  Cursor = Cursors.WaitCursor;
                Connect(desiredDeviceInfo);
                //   Cursor = Cursors.Default;

                if (mDevice != null)
                {
                    PvGenParameterArray parameters = mDevice.Parameters;

                    if (parameters != null)
                    {
                        PvGenParameter testPatternParam = parameters.Get("TestPattern");

                        if (testPatternParam != null && testPatternParam.IsAvailable)
                        {
                            PvGenEnum testPatternEnum = testPatternParam as PvGenEnum;

                            if (testPatternEnum != null)
                            {
                                testPatternEnum.ValueString = "Off";
                            }
                            else
                            {
                                MessageBox.Show("Failed to cast TestPattern parameter as enum");
                            }
                        }
                        else
                        {
                            MessageBox.Show("TestPattern parameter not available");
                        }
                    }
                }
                else
                {
                    //MessageBox.Show("Device not connected");
                }
            }
            catch (Exception ex)
            {
                // Print the exception message
                //MessageBox.Show(ex.Message);
            }
            StartAcquisition();
        }
        public void connectCamera1()
        {

            try
            {
                string uniqueId = "00:11:1c:05:3c:34"; // this should be the unique ID of your device
                PvDeviceInfo desiredDeviceInfo;
                PvDeviceGEV.SetIPConfiguration(uniqueId, "192.168.10.251", "255.255.255.0", "0.0.0.0");

                desiredDeviceInfo = mSystem.FindDevice(uniqueId);
                if (desiredDeviceInfo == null)
                {
                    MessageBox.Show("Device not found");
                    return;
                }

                //  Cursor = Cursors.WaitCursor;
                Connect(desiredDeviceInfo);

                StartAcquisition();
            }
            catch (Exception ex)

            {
                // Print the exception message
                MessageBox.Show(ex.Message);
            }
        }
        private void StartAcquisition()
        {
            // Get payload size
            if (mDevice == null) return;
            UInt32 lPayloadSize = mDevice.PayloadSize;

            // Propagate to pipeline to make sure buffers are big enough
            mPipeline.BufferSize = lPayloadSize;

            // Reset pipeline
            mPipeline.Reset();

            // Reset stream statistics
            PvGenCommand lResetStats = mStream.Parameters.GetCommand("Reset");
            lResetStats.Execute();

            // Reset display thread stats (mostly frames displayed per seconds)
            mDisplayThread.ResetStatistics();

            // Use acquisition manager to send the acquisition start command to the device
            mAcquisitionManager.Start();

            // Single source application, for simplicity we start all configured camera bridges
            for (int i = 0; i < mCameraBridgeManager.BridgeCount; i++)
            {
                PvCameraBridge lBridge = mCameraBridgeManager.GetBridge(i);
                if ((lBridge != null) &&
                    (lBridge.IsConnected))
                {
                    lBridge.StartAcquisition();
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////
            //Console.WriteLine($"StartAcquisition()....mDeivce Open ?? [{mDevice.IsConnected}]");
            //openSerial();
            DeviceSerial_Connect();
            ///////////////////////////////////////////////////////////////////////////////////////////

        }

        /// <summary>
        /// Stops the acquisition.
        /// </summary>
        public void StopAcquisition()
        {
            // Single source application, for simplicity we stop all configured camera bridges
            for (int i = 0; i < mCameraBridgeManager.BridgeCount; i++)
            {
                PvCameraBridge lBridge = mCameraBridgeManager.GetBridge(i);
                if ((lBridge != null) &&
                    (lBridge.IsConnected))
                {
                    lBridge.StopAcquisition();
                }
            }

            // Use acquisition manager to send the acquisition stop command to the device
            mAcquisitionManager.Stop();
        }
        private zoomForm zoomForm = new zoomForm();
        private Bitmap lastBitmap = null;
        private object bitmapLock = new object();
        private PvImage latestImage = null;
        private readonly object imageLock = new object();
        void OnBufferDisplay(PvDisplayThread aDisplayThread, PvBuffer aBuffer)
        {
            try
            {
                 display.Display(aBuffer);
                DrawCrosshairOverlay();
                if (aBuffer.IsTrailerValid && aBuffer.Image != null)
                {
                    latestImage = aBuffer.Image; 
                }
                if (isSaving && selectedPoint.HasValue && aBuffer.IsTrailerValid)
                {
                    byte[] cropped = CropImageRegion(aBuffer.Image, selectedPoint.Value);
                 // byte[] cropped = CropImageRegion(aBuffer.Image);
                    if (cropped != null)
                    {
                        croppedFrameQueue.Enqueue(cropped); 
                    }
                }
             /*   if (selectedPoint.HasValue && zoomForm != null && aBuffer.IsTrailerValid)
                {
                    PvImage image = aBuffer.Image;
                    if (image == null) return;

                    uint x = (uint)(selectedPoint.Value.X - 26);
                    uint y = (uint)(selectedPoint.Value.Y - 26);

                    if (x < 0) x = 0;
                    if (y < 0) y = 0;
                    if (x + 52 > image.Width) x = image.Width - 52;
                    if (y + 52 > image.Height) y = image.Height - 52;

                    byte[] zoomBytes = new byte[52 * 52];
                    unsafe
                    {
                        byte* src = (byte*)image.DataPointer;
                        uint stride = image.Width;
                        for (int row = 0; row < 52; row++)
                        {
                            for (int col = 0; col < 52; col++)
                            {
                                zoomBytes[row * 52 + col] = src[(y + row) * stride + (x + col)];
                            }
                        }
                    }

                    zoomForm.UpdateImage(zoomBytes, 52, 52);
                }*/
                lock (bitmapLock)
                {
                    lastBitmap?.Dispose();
                    lastBitmap = ConvertBufferToBitmap(aBuffer);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public Point? ConvertClickToImageCoord(int clickX, int clickY, Size displaySize, Size imageSize)
        {
            var (offsetX, offsetY, renderWidth, renderHeight) = GetRenderInfo(imageSize, displaySize);

            if (clickX < offsetX || clickX >= offsetX + renderWidth ||
                clickY < offsetY || clickY >= offsetY + renderHeight)
                return null;

            int imageX = (int)((clickX - offsetX) * ((double)imageSize.Width / renderWidth));
            int imageY = (int)((clickY - offsetY) * ((double)imageSize.Height / renderHeight));
            return new Point(imageX, imageY);
        }
        private byte[] CropImageRegion(PvImage image)
        {
            const int cropSize = 52;
            int half = cropSize / 2;
            if (image == null) return null;

            uint x = (uint)(display.Width / 2 - half);
            uint y = (uint)(display.Height / 2 - half);

            if (x + cropSize > image.Width) x = image.Width - cropSize;
            if (y + cropSize > image.Height) y = image.Height - cropSize;

            byte[] result = new byte[cropSize * cropSize];

            unsafe
            {
                byte* src = (byte*)image.DataPointer;
                uint stride = image.Width;

                for (int row = 0; row < cropSize; row++)
                {
                    for (int col = 0; col < cropSize; col++)
                    {
                        result[row * cropSize + col] = src[(y + (uint)row) * stride + (x + (uint)col)];
                    }
                }
            }

            return result;
        }
        private (int offsetX, int offsetY, int renderWidth, int renderHeight)
    GetRenderInfo(Size imageSize, Size displaySize)
        {
            double imageAspect = (double)imageSize.Width / imageSize.Height;
            double displayAspect = (double)displaySize.Width / displaySize.Height;

            int renderWidth, renderHeight, offsetX = 0, offsetY = 0;

            if (displayAspect > imageAspect)
            {
                renderHeight = displaySize.Height;
                renderWidth = (int)(renderHeight * imageAspect);
                offsetX = (displaySize.Width - renderWidth) / 2;
            }
            else
            {
                renderWidth = displaySize.Width;
                renderHeight = (int)(renderWidth / imageAspect);
                offsetY = (displaySize.Height - renderHeight) / 2;
            }

            return (offsetX, offsetY, renderWidth, renderHeight);
        }
        private byte[] CropImageRegion(PvImage image, Point screenPoint)
        {
            const int cropSize = 52;
            int half = cropSize / 2;
            if (image == null) return null;

            Point? imagePoint = ConvertClickToImageCoord(
                screenPoint.X, screenPoint.Y,
                display.Size,
                new Size((int)image.Width, (int)image.Height));

            if (!imagePoint.HasValue)
                return null; 

            int centerX = imagePoint.Value.X;
            int centerY = imagePoint.Value.Y;
          
            int x = Math.Max(0, Math.Min((int)(image.Width - cropSize), centerX - half));
            int y = Math.Max(0, Math.Min((int)(image.Height - cropSize), centerY - half));

            byte[] result = new byte[cropSize * cropSize];

            unsafe
            {
                byte* src = (byte*)image.DataPointer;
                int stride = (int)image.Width;

                for (int row = 0; row < cropSize; row++)
                {
                    for (int col = 0; col < cropSize; col++)
                    {
                        result[row * cropSize + col] = src[(y + row) * stride + (x + col)];
                    }
                }
            }

            return result;
        }
        /*private byte[] CropImageRegion(PvImage image, Point selectedPoint)
        {
            const int cropSize = 52;
            int half = cropSize / 2;
            if (image == null) return null;

            double scaleX = (double)image.Width / display.Width;
            double scaleY = (double)image.Height / display.Height;

            uint centerX = (uint)(selectedPoint.X * scaleX);
            uint centerY = (uint)(selectedPoint.Y * scaleY);

            uint x = (uint)Math.Max(0, Math.Min(image.Width - cropSize, centerX - half));
            uint y = (uint)Math.Max(0, Math.Min(image.Height - cropSize, centerY - half));

            byte[] result = new byte[cropSize * cropSize];

            unsafe
            {
                byte* src = (byte*)image.DataPointer;
                uint stride = image.Width;

                for (int row = 0; row < cropSize; row++)
                {
                    for (int col = 0; col < cropSize; col++)
                    {
                        result[row * cropSize + col] = src[(y + (uint)row) * stride + (x + (uint)col)];
                    }
                }
            }
            return result;
        }*/

        private bool isSaving = false;
        private ConcurrentQueue<byte[]> croppedFrameQueue = new ConcurrentQueue<byte[]>();
        private CancellationTokenSource cts;
        private Task savingTask;
        private string saveFolder;
        private DateTime startTime;
        private void CropAndSaveRawImage(PvBuffer aBuffer, Point selectedPoint)
        {
            const int cropSize = 52;
            int halfCrop = cropSize / 2;

            PvImage image = aBuffer.Image;
            if (image == null) return;

            int centerX = selectedPoint.X;
            int centerY = selectedPoint.Y;

            uint x = (uint)Math.Max(0, centerX - halfCrop);
            uint y = (uint)Math.Max(0, centerY - halfCrop);

            if (x + cropSize > image.Width) x = image.Width - cropSize;
            if (y + cropSize > image.Height) y = image.Height - cropSize;

            byte[] zoomBytes = new byte[cropSize * cropSize];

            unsafe
            {
                byte* src = (byte*)image.DataPointer;
                uint stride = image.Width;

                for (int row = 0; row < cropSize; row++)
                {
                    for (int col = 0; col < cropSize; col++)
                    {
                        zoomBytes[row * cropSize + col] = src[(y + (uint)row) * stride + (x + (uint)col)];
                    }
                }
            }

            string fileName = $"Cropped_{DateTime.Now:yyyyMMdd_HHmmss_fff}.raw";
            string savePath = Path.Combine("CroppedImages", fileName);

            Directory.CreateDirectory("CroppedImages");

            File.WriteAllBytes(savePath, zoomBytes);

            Console.WriteLine($"Saved cropped raw image to: {savePath}");
        }
        private Bitmap ConvertBufferToBitmap(PvBuffer buffer)
        {
            unsafe
            {
                if (buffer == null || !buffer.IsHeaderValid || !buffer.IsTrailerValid || buffer.Image.DataPointer == null)
                    return null;
            }


            if (!buffer.IsTrailerValid || !buffer.IsHeaderValid)
            {
                return null;
            }

            if (buffer.Image.IsImageDropped || buffer.Image.IsDataOverrun)
            {
                return null;
            }

            if (buffer.Image.PixelType != PvPixelType.RGB8)
            {
                return null;
            }

            int width = (int)buffer.Image.Width;
            int height = (int)buffer.Image.Height;
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

            ColorPalette palette = bmp.Palette;
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            bmp.Palette = palette;

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),
                                               ImageLockMode.WriteOnly, bmp.PixelFormat);

            unsafe
            {
                byte* pData = (byte*)buffer.Image.DataPointer;
                Buffer.MemoryCopy(pData, bmpData.Scan0.ToPointer(), bmpData.Stride * height, width);
            }

            bmp.UnlockBits(bmpData); 

            return bmp;
        }
        private void DrawCrosshairOverlay()
        {
            using (Graphics g = display.CreateGraphics())
            {
                int centerX = display.Width / 2;
                int centerY = display.Height / 2;
                int lineLength = 10;
                g.DrawLine(Pens.Yellow, centerX - lineLength, centerY, centerX + lineLength, centerY);
                g.DrawLine(Pens.Yellow, centerX, centerY - lineLength, centerX, centerY + lineLength);
            }
        }
        void Connect(PvDeviceInfo aDI)
        {
            // Just in case we came here still connected...
            Disconnect();

            if (aDI == null)
            {
                MessageBox.Show("GigE Vision devices not currently supported by this sample.", Text);
                return;
            }

            try
            {
                // Create and connect the device controller based on the selected device
                mDevice = PvDevice.CreateAndConnect(aDI);
                // Create and open stream
                mStream = PvStream.CreateAndOpen(aDI);

                // GigE Vision specific connection steps
                if (aDI.Type == PvDeviceInfoType.GEV)
                {
                    PvDeviceGEV lDeviceGEV = mDevice as PvDeviceGEV;
                    PvStreamGEV lStreamGEV = mStream as PvStreamGEV;

                    // Negotiate packet size
                    lDeviceGEV.NegotiatePacketSize();
                    //lDeviceGEV.SetPacketSize(9014);

                    // Set stream destination to our stream object
                    lDeviceGEV.SetStreamDestination(lStreamGEV.LocalIPAddress, lStreamGEV.LocalPort);
                }

                // Create pipeline - requires stream
                mPipeline = new PvPipeline(mStream);
            }
            catch (PvException ex)
            {
                // Failure at some point, display and abort
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Disconnect();

                return;
            }

            // Register to all events of the parameters in the device's node map
            foreach (PvGenParameter lParameter in mDevice.Parameters)
            {
                lParameter.OnParameterUpdate += new OnParameterUpdateHandler(OnParameterChanged);
            }

            // Connect link disconnection handler
            mDevice.OnLinkDisconnected += new OnLinkDisconnectedHandler(OnLinkDisconnected);

            // Update device attributes
            UpdateAttributes(aDI);

            // Fill acquisition mode combo box


            // Ready image reception
            StartStreaming();

            // Create camera bridge manager
            mCameraBridgeManager = new PvCameraBridgeManagerForm();
            mCameraBridgeManager.Device = mDevice;
            mCameraBridgeManager.Stream = mStream;

            // Sync the UI with our new status
        }



        void UpdateAttributes(PvDeviceInfo aDI)
        {
            string lVendorName = "";
            string lModelName = "";
            string lMACAddress = "";
            string lIPAddress = "";
            string lGUID = "";
            string lUserDefinedName = "";

            if (aDI != null)
            {
                // Get device attributes
                lVendorName = aDI.VendorName;
                lModelName = aDI.ModelName;
                lMACAddress = "N/A";
                lIPAddress = "N/A";
                lGUID = "N/A";
                lUserDefinedName = aDI.UserDefinedName;

                // GigE Vision specific device attributes
                PvDeviceInfoGEV lDeviceInfoGEV = aDI as PvDeviceInfoGEV;
                if (lDeviceInfoGEV != null)
                {
                    lMACAddress = lDeviceInfoGEV.MACAddress;
                    lIPAddress = lDeviceInfoGEV.IPAddress;
                }

                // USB3 Vision specific device attributes
                PvDeviceInfoU3V lDeviceInfoU3V = aDI as PvDeviceInfoU3V;
                if (lDeviceInfoU3V != null)
                {
                    lGUID = lDeviceInfoU3V.DeviceGUID;
                }
            }

            /*            // Fill device attribute text boxes
                        manufacturerTextBox.Text = lVendorName;
                        modelTextBox.Text = lModelName;
                        macAddressTextBox.Text = lMACAddress;
                        ipAddressTextBox.Text = lIPAddress;
                        guidTextBox.Text = lGUID;
                        nameTextBox.Text = lUserDefinedName;*/
        }

        /// <summary>
        /// Disconnects from the device
        /// </summary>
        public void Disconnect()
        {
            // DeviceSerial DisConnect
            DeviceSerial_Disconnect();

            StopStreaming();

            if (mCameraBridgeManager != null)
            {
                mCameraBridgeManager.Close();
                mCameraBridgeManager.Dispose();
                mCameraBridgeManager = null;
            }

            if (mPipeline != null)
            {
                mPipeline.Dispose();
                mPipeline = null;
            }

            if (mStream != null)
            {
                mStream.Close();
                mStream.Dispose();
                mStream = null;
            }

            if (mDevice != null)
            {
                if (mDevice.IsConnected)
                {
                    // Disconnect events.
                    mDevice.OnLinkDisconnected -= new OnLinkDisconnectedHandler(OnLinkDisconnected);
                    foreach (PvGenParameter lP in mDevice.Parameters)
                    {
                        lP.OnParameterUpdate -= new OnParameterUpdateHandler(OnParameterChanged);
                    }

                    mDevice.Disconnect();
                    mDevice.Dispose();
                    mDevice = null;
                }
            }
            if (display != null)
            {
                display.Clear();
                display.Invalidate();  
            }
            UpdateAttributes(null);

            // Sync the UI with our new status
        }

        /// <summary>
        /// GenICam parameter invalidation event, registered for all parameters.
        /// </summary>
        /// <param name="aParameter"></param>
        void OnParameterChanged(PvGenParameter aParameter)
        {
            string lName = aParameter.Name;
            if (lName == "AcquisitionMode")
            {
                // Have main UI thread update the acquisition mode combo box status
                BeginInvoke(mAcquisitionModeChangedHandler);
            }
        }

        /// <summary>
        /// Acquisition mode event handler in main thread.
        /// </summary>
        private void OnAcquisitionModeChanged()
        {
            // Get parameter
            PvGenEnum lParameter = mDevice.Parameters.GetEnum("AcquisitionMode");

            // Update value: find which matches in the combo box
            string lValue = lParameter.ValueString;

        }

        /// <summary>
        /// Direct acquisition state changed handler. Bring back to main UI thread.
        /// </summary>
        /// <param name="aDevice"></param>
        /// <param name="aStream"></param>
        /// <param name="aSource"></param>
        /// <param name="aState"></param>
        void OnAcquisitionStateChanged(PvDevice aDevice, PvStream aStream, uint aSource, PvAcquisitionState aState)
        {
            // Invoke event in main UI thread.
            BeginInvoke(mAcquisitionStateChangedHandler);
        }




        /// <summary>
        /// Closes a GenICam browser form.
        /// </summary>
        /// <param name="aForm"></param>
        private void CloseGenWindow(Form aForm)
        {
            aForm.Hide();
        }

        /// <summary>
        /// Direct device disconnect handler. Just jump back to main UI thread.
        /// </summary>
        /// <param name="aDevice"></param>
        public void OnLinkDisconnected(PvDevice aDevice)
        {
            BeginInvoke(mDisconnectedHandler);
        }

        /// <summary>
        /// Reaction to device disconnected event: stop streaming, close device connection.
        /// </summary>
        private void OnDisconnected()
        {
            //MessageBox.Show("Connection to device lost.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            StopStreaming();
            Disconnect();
        }

        /// <summary>
        /// Setups streaming. After calling this method the application is ready to receive data.
        /// StartAcquisition will instruct the device to actually start sending data.
        /// </summary>
        private void StartStreaming()
        {
            // Configure status control

            // Start threads
            mDisplayThread.Start(mPipeline, mDevice.Parameters);
            mDisplayThread.Priority = PvThreadPriority.AboveNormal;

            // Configure acquisition state manager
            mAcquisitionManager = new PvAcquisitionStateManager(mDevice, mStream);
            mAcquisitionManager.OnAcquisitionStateChanged += new OnAcquisitionStateChanged(OnAcquisitionStateChanged);

            // Start pipeline
            mPipeline.Start();
        }

        /// <summary>
        /// Stops streaming. After calling this method the application is no longer armed or ready
        /// to receive data.
        /// </summary>
        public void StopStreaming()
        {
            if (!mDisplayThread.IsRunning)
            {
                return;
            }

            // Status control is configured in StartStreaming, must release 
            // resources in StopStreaming

            // Stop display thread
            mDisplayThread.Stop(false);

            // Release acquisition manager
            mAcquisitionManager.Dispose();
            mAcquisitionManager = null;

            // Stop pipeline
            if (mPipeline.IsStarted)
            {
                mPipeline.Stop();
            }

            // Wait on display thread
            mDisplayThread.WaitComplete();
        }

/*        private void display_Click(object sender, EventArgs e)
        {
            System.Drawing.Point clickPoint = display.PointToClient(Cursor.Position);

            int xInPictureBox = clickPoint.X - (display.Width / 2);
            int yInPictureBox = (display.Height / 2) - clickPoint.Y;

            CalculateDistance(clickPoint);
        }
        private void CalculateDistance(System.Drawing.Point point)
        {
            int centerX = display.Width / 2;
            int centerY = display.Height / 2;

            double distance = Math.Sqrt(Math.Pow(point.X - centerX, 2) + Math.Pow(point.Y - centerY, 2));
            MessageBox.Show($"중심점까지의 거리 : {distance}");
        }
*/
        private void display_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int centerX = display.Width / 2;
            int centerY = display.Height / 2;
            int markerSize = 10;

            g.DrawLine(Pens.Yellow, centerX, centerY - markerSize / 2, centerX, centerY + markerSize / 2);
            g.DrawLine(Pens.Yellow, centerX - markerSize / 2, centerY, centerX + markerSize / 2, centerY);
        }

        private void textBox_DigitalGain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int.TryParse(textBox_DigitalGain.Text, out int ditgitalGAIN_value);
            }
            Console.WriteLine($"DigitalGainSet : {ditgitalGAIN_value}");
        }

        private void textBox_EMgain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int.TryParse(textBox_EMgain.Text, out int emGAIN_value);
            }
        }


        private void openSerial()
        {
            byte[] GetSystemStatus = { 0x49, 0x50 };            
            pvSerial.Open(mDevice, PvDeviceSerial.Serial0);
            pvSerial.Write(GetSystemStatus);
            Console.WriteLine($"isOpen...{pvSerial.IsOpened}  Read Serial.....[{pvSerial.Read(20)}]");
        }




        private void button_DigitalGain_Click(object sender, EventArgs e)
        {
            //Control_DigitalGAIN = ditgitalGAIN_value;
            ditgitalGAIN_value = 3 * 256;
            byte b0 = (byte)ditgitalGAIN_value;
            byte b1 = (byte)(ditgitalGAIN_value >> 8);
            CMD_DIGITAL_GAIN1[5] = b1;
            CMD_DIGITAL_GAIN2[5] = b0;
            Console.WriteLine($"set_em_gain({ditgitalGAIN_value})......b1[{b1}]  b0[{b0}]");
            pvSerial.Write(CMD_DIGITAL_GAIN1);
            pvSerial.Write(CMD_DIGITAL_GAIN2);
            Console.WriteLine($"Read Serial.....[{pvSerial.Read(30)}]");

        }

        private void button_EmGain_Click(object sender, EventArgs e)
        {
            Control_EmGAIN = emGAIN_value;
        }
        private Point? selectedPoint = null;
        private void display_MouseClick(object sender, MouseEventArgs e)
        {
            PvImage image = GetLatestImage();
            selectedPoint = e.Location;
            var offsetHelper = CreateOffsetHelper(image);
            (double a, double b )= offsetHelper.GetAzElOffsetFromClick(e.X, e.Y);
            CSU_Observation.AddAzOffSet += a;
            CSU_Observation.AddElOffSet += b;
            Console.WriteLine($"Selected crop center: {selectedPoint.Value}");
            Console.WriteLine($"a  = {a} , b = {b}");
            /*            lock (bitmapLock)
                        {
                            if (lastBitmap == null) return;

                            int x = e.X;
                            int y = e.Y;

                            float scaleX = (float)lastBitmap.Width / display.Width;
                            float scaleY = (float)lastBitmap.Height / display.Height;

                            int imageX = (int)(x * scaleX);
                            int imageY = (int)(y * scaleY);

                            const int cropSize = 52;
                            int half = cropSize / 2;

                            int cropX = Math.Max(0, Math.Min(imageX - half, lastBitmap.Width - cropSize));
                            int cropY = Math.Max(0, Math.Min(imageY - half, lastBitmap.Height - cropSize));

                            Rectangle cropRect = new Rectangle(cropX, cropY, cropSize, cropSize);

                            Bitmap zoomBitmap = lastBitmap.Clone(cropRect, lastBitmap.PixelFormat);

                            if (zoomForm.IsDisposed) zoomForm = new ZoomForm();
                            zoomForm.UpdateImage(zoomBitmap);
                            if (!zoomForm.Visible) zoomForm.Show();
                            else zoomForm.BringToFront();
                        }*/
        }
        private PvImage GetLatestImage()
        {
            lock (imageLock)
            {
                return latestImage;
            }
        }
        private AutoOffsetThroughMouse CreateOffsetHelper(PvImage image)
        {
            double fullFovX = 0.6;
            double fullFovY = 0.4;

            double imageAspect = (double)image.Width / image.Height;
            double displayAspect = (double)display.Width / display.Height;

            // visibleFovX/Y: fullFOV를 display 전체 픽셀 폭/높이에 매핑하기 위한 보정값.
            // PvDisplayControl은 종횡비 유지(Zoom) 방식으로 표시하므로,
            // display와 image의 종횡비 차이만큼 유효 FOV를 확장해야 함.
            double visibleFovX = fullFovX;
            double visibleFovY = fullFovY;

            if (displayAspect > imageAspect)
            {
                // display가 image보다 가로로 넓음 → 좌우에 검은 여백 발생
                // image가 display 높이를 채움: 표시 폭 = display.Height * imageAspect
                // scaleX = fullFovX / (display.Height * imageAspect)
                //        = (fullFovX * displayAspect / imageAspect) / display.Width
                visibleFovX *= displayAspect / imageAspect;
            }
            else if (displayAspect < imageAspect)
            {
                // display가 image보다 세로로 길음 → 상하에 검은 여백 발생
                // image가 display 폭을 채움: 표시 높이 = display.Width / imageAspect
                // scaleY = fullFovY / (display.Width / imageAspect)
                //        = (fullFovY * imageAspect / displayAspect) / display.Height
                visibleFovY *= imageAspect / displayAspect;
            }

            // 클릭 좌표(e.X, e.Y)는 display 픽셀 공간이므로 resolutionX/Y에 display 크기를 사용
            return new AutoOffsetThroughMouse(
                fovXDeg: visibleFovX,
                fovYDeg: visibleFovY,
                resolutionX: (uint)display.Width,
                resolutionY: (uint)display.Height);
        }


        private void SaveStart_Click(object sender, EventArgs e)
        {
            croppedFrameQueue = new ConcurrentQueue<byte[]>(); 
            isSaving = true;
            StartSavingCroppedRawData(); 
            Console.WriteLine("Started saving cropped raw video");
        }

        private void SaveStop_Click(object sender, EventArgs e)
        {
            isSaving = false;
            StopSavingCroppedRawData();
            Console.WriteLine("Stopped saving and finalized cropped raw video");
        }
        private void StopSavingCroppedRawData()
        {
            cts?.Cancel();
            savingTask?.Wait();

            string fileName = Path.Combine(saveFolder, $"cropped_sequence.raw");

            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            using (var bw = new BinaryWriter(fs))
            {
                long ticks = startTime.Ticks;
                bw.Write(ticks);

                while (croppedFrameQueue.TryDequeue(out byte[] frame))
                {
                    bw.Write(frame);
                }
            }

            Console.WriteLine($"Stopped saving and flushed remaining frames to: {fileName}");
        }
        /*        private void SaveCroppedVideo()
                {
                    string folder = Path.Combine("SavedCroppedFrames", DateTime.Now.ToString("yyyyMMdd_HHmmss"));
                    Directory.CreateDirectory(folder);

                    int index = 0;
                    foreach (var frame in croppedFrames)
                    {
                        using (Bitmap bmp = new Bitmap(52, 52, PixelFormat.Format8bppIndexed))
                        {
                            ColorPalette palette = bmp.Palette;
                            for (int i = 0; i < 256; i++) palette.Entries[i] = Color.FromArgb(i, i, i);
                            bmp.Palette = palette;

                            BitmapData data = bmp.LockBits(new Rectangle(0, 0, 52, 52), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                            Marshal.Copy(frame, 0, data.Scan0, frame.Length);
                            bmp.UnlockBits(data);

                            string fileName = Path.Combine(folder, $"frame_{index++:D4}.png");
                            bmp.Save(fileName, ImageFormat.Png);
                        }
                    }

                    Console.WriteLine($"Saved {index} cropped frames to: {folder}");
                }
                private void SaveCroppedRawData()
                {
                    string folder = Path.Combine("SavedCroppedFrames", DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_raw");
                    Directory.CreateDirectory(folder);

                    int index = 0;
                    foreach (var frame in croppedFrames)
                    {
                        string fileName = Path.Combine(folder, $"frame_{index++:D4}.raw");
                        File.WriteAllBytes(fileName, frame);
                    }

                    Console.WriteLine($"Saved {index} cropped raw frames to: {folder}");
                }*/
        private void StartSavingCroppedRawData()
        {
            cts = new CancellationTokenSource();
            startTime = DateTime.UtcNow;
            saveFolder = Path.Combine(@"\\LGAOOCU\NSLR_GuideCamera", startTime.ToString("yyyyMMdd_HHmmss_fff") + "_raw");
            Directory.CreateDirectory(saveFolder);

           // savingTask = Task.Run(() => SaveLoop(cts.Token));
            Console.WriteLine("Started saving cropped raw frames...");
        }

        /*        private void StopSavingCroppedRawData()
                {
                    cts?.Cancel();
                    savingTask?.Wait();
                    Console.WriteLine("Stopped saving cropped raw frames.");
                }*/

        private void SaveLoop(CancellationToken token)
        {
            string fileName = Path.Combine(saveFolder, $"cropped_sequence.raw");

            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            using (var bw = new BinaryWriter(fs))
            {
                long ticks = startTime.Ticks;
                bw.Write(ticks); 

                while (!token.IsCancellationRequested)
                {
                    if (croppedFrameQueue.TryDequeue(out byte[] frame))
                    {
                        bw.Write(frame);
                    }
                    else
                    {
                        Thread.Sleep(5);
                    }
                }
               
                while (croppedFrameQueue.TryDequeue(out byte[] remaining))
                {
                    bw.Write(remaining);
                }
            }

            Console.WriteLine($"Saved all cropped frames to: {fileName}");
        }

    }
    public class AutoOffsetThroughMouse
    {
        private readonly double fovXDeg; // 가로 FOV (deg)
        private readonly double fovYDeg; // 세로 FOV (deg)
        private readonly uint resolutionX; // 해상도 가로
        private readonly uint resolutionY; // 해상도 세로

        public AutoOffsetThroughMouse(double fovXDeg, double fovYDeg, uint resolutionX, uint resolutionY)
        {
            this.fovXDeg = fovXDeg;
            this.fovYDeg = fovYDeg;
            this.resolutionX = resolutionX;
            this.resolutionY = resolutionY;
        }

        public (double azTarget, double elTarget) GetAzElOffsetFromClick(int x, int y)
        {
            double cx = resolutionX / 2.0;
            double cy = resolutionY / 2.0;

            double scaleX = fovXDeg / resolutionX;
            double scaleY = fovYDeg / resolutionY;

            double dx = x - cx;
            double dy = cy - y;

            double deltaAz = dx * scaleX;
            double deltaEl = dy * scaleY;
            return (deltaAz, deltaEl);
        }
    }

}
