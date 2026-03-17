using mv.impact.acquire.helper;
using mv.impact.acquire;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Windows.Forms;
using System.Runtime.InteropServices.ComTypes;
using System.Net.Sockets;
using System.Threading;

using System.Diagnostics;
using System.Windows.Markup;
using System.Runtime.ExceptionServices;
//using System.Security;

namespace NSLR_ObservationControl
{
    internal class IRDisplay
    {
        public class MyConsoleRequestListener
        {
            public void requestReady(object sender, RequestReadyEventArgs e)
            {
                using (RequestReadyEventData data = e.data)
                {
                    if (data.request.isOK)
                    {
                        Console.WriteLine("Request {0} became ready. Image format: {1}({2}x{3}).", data.request.number, data.request.imagePixelFormat.readS(), data.request.imageWidth.read(), data.request.imageHeight.read());
                    }
                    else
                    {
                        Console.WriteLine("Request {0} has been reported with status '{1}'.", data.request.number, data.request.requestResult.readS());
                    }
                }
            }
        }

        public class MyDisplayRequestListener
        {
            private RequestReadyEventData data_ = null;
            public PictureBox pictureBox;
            //private const string CameraIpAddress = "169.254.7.76"; // 카메라 IP 주소
            private const string CameraIpAddress = "192.168.10.222"; // 카메라 IP 주소
            private const int CameraPort = 11313; // 카메라 포트 번호
            //private const int CameraPort = 8554; // 카메라 포트 번호
            private byte[] buffer;
            private TcpClient client;
            private NetworkStream stream;
            Thread thread;
            public Device device;
            int requestCount = 1;
            int imagesToSaveCount = 1;
            string[] arrayImageFormat = { "bmp", "jpg", "png", "tif" };
            string imageFormat;
            bool terminated;

            public MyDisplayRequestListener(PictureBox pictureBox)
            {
                this.pictureBox = pictureBox;
                pictureBox.Image = null;
            }
            /*            private void adjustGain()
                        {
                            // Get the ImageProcessing object
                            mv.impact.acquire.ImageProcessing imgProc = new mv.impact.acquire.ImageProcessing(device);

                            // Get the GainOffsetKneeChannelParameters for the first channel
                            mv.impact.acquire.GainOffsetKneeChannelParameters gainParams = imgProc.getGainOffsetKneeParameter(0);

                            // Set the gain
                            double newGain = 0.5; // This is an arbitrary value. Adjust as necessary.
                            gainParams.gain = newGain;
                        }*/
            public void requestStop()
            {
                if (data_ != null)
                {
                    data_.Dispose();
                    data_ = null;
                }
                
            }
            public void requestReady(object sender, RequestReadyEventArgs e)
            {
                RequestReadyEventData data = e.data;

                if (data.request.isOK)
                {
                    int width = data.request.imageWidth.read();
                    int height = data.request.imageHeight.read();
                    int bytesPerPixel = data.request.imageBytesPerPixel.read();
                    int linePitch = data.request.imageLinePitch.read();

                    byte[] imageData = new byte[width * height * bytesPerPixel];

                    Marshal.Copy(data.request.imageData.read(), imageData, 0, imageData.Length);
                    try
                    {

                        byte[] adjustedData = AdjustImageContrast(imageData, width, height);
                        Bitmap grayscaleBitmap = Convert8BitToBitmap(adjustedData, width, height);

                        pictureBox.Invoke((MethodInvoker)delegate
                        {
                            using (var stream = new MemoryStream())
                            {
                                grayscaleBitmap.Save(stream, ImageFormat.Bmp);
                                stream.Seek(0, SeekOrigin.Begin);
                                pictureBox.Image?.Dispose();
                                pictureBox.Image = System.Drawing.Image.FromStream(stream);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to display image: " + ex.Message);
                        pictureBox.Image = null;
                    }
                    if (data_ != null)
                    {
                        data_.Dispose();
                    }
                    data_ = data;


                }
                else
                {
                    Console.WriteLine("Request {0} has been reported with status '{1}'.", data.request.number, data.request.requestResult.readS());
                    data.Dispose();


                    // imageData 배열 해제
                    //Marshal.FreeHGlobal(data.request.imageData.read());
                }
            }

            private Bitmap Convert8BitToBitmap(byte[] data, int width, int height)
            {
                Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                ColorPalette palette = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    palette.Entries[i] = Color.FromArgb(i, i, i);
                }
                bitmap.Palette = palette;
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                Marshal.Copy(data, 0, bitmapData.Scan0, data.Length);
                bitmap.UnlockBits(bitmapData);
                return bitmap;
            }

            public byte[] AdjustImageContrast(byte[] mono16Data, int width, int height)
            {
                double minVal, maxVal;
                Mat srcMat = new Mat(height, width, MatType.CV_16UC1, mono16Data);

                Mat adjustedMat = new Mat();
                int linePitch = width * 2; // Mono16 stride
                Cv2.MinMaxLoc(srcMat, out minVal, out maxVal);
                srcMat.ConvertTo(adjustedMat, MatType.CV_8U, 255.0 / (maxVal - minVal), -minVal * 255.0 / (maxVal - minVal));
                byte[] adjustedData = new byte[921600];
                Marshal.Copy(adjustedMat.Data, adjustedData, 0, adjustedData.Length);

                return adjustedData;
            }
            private void InitializeCameraConnection()
            {
                try
                {
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to connect to the camera: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            public void SendCommand(string command)
            {
                client = new TcpClient(CameraIpAddress, CameraPort);
                stream = client.GetStream();
                buffer = new byte[1024];
                try
                {
                    byte[] commandBytes = CreateMessage(command);
                    stream.Write(commandBytes, 0, commandBytes.Length);

                    // 응답 수신
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    //string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    byte[] response = buffer;
                    string value;

                    //string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // 응답 처리
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to send command to the camera: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                stream.Close();
            }
            public string result { get; set; }
            public string result2 { get; set; }
            int valueInt;
            double valueDouble;
            string value;
            public void RecvValue(string command, int Type)
            {
                client = new TcpClient(CameraIpAddress, CameraPort);
                stream = client.GetStream();
                buffer = new byte[1024];

                try
                {
                    byte[] commandBytes = CreateMessage(command);
                    stream.Write(commandBytes, 0, commandBytes.Length);

                    // 응답 수신
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    //string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    byte[] response = buffer;

                    if (Type == 1)
                    {
                        value = Encoding.ASCII.GetString(response, 9, 5);
                        valueInt = Convert.ToInt32(value);
                        valueInt = valueInt - 500;

                    }
                    else if (Type == 2)
                    {
                        value = Encoding.ASCII.GetString(response, 9, 5);
                        valueInt = Convert.ToInt32(value);
                        valueInt = valueInt + 500;
                    }
                    else if (Type == 4)
                    {
                        value = Encoding.ASCII.GetString(response, 9, Convert.ToInt32(response[1]) - 7);
                        valueInt = Convert.ToInt32(value);
                        valueInt = valueInt - 200;

                        if (valueInt - 200 < -10000) valueInt = -10000;
                        Debug.WriteLine(Convert.ToString(valueInt));
                    }
                    else if (Type == 5)
                    {
                        value = Encoding.ASCII.GetString(response, 9, Convert.ToInt32(response[1]) - 7);
                        valueInt = Convert.ToInt32(value);
                        valueInt = valueInt + 200;
                        if (valueInt + 200 > 32000) valueInt = 32000;

                        Debug.WriteLine(Convert.ToString(valueInt));
                    }


                    else if (Type == 8)
                    {
                        value = Encoding.ASCII.GetString(response, 9, Convert.ToInt32(response[1]) - 9);
                        valueInt = Convert.ToInt32(value);
                        valueInt = valueInt - 100;

                        Debug.WriteLine(Convert.ToString(valueInt));
                    }
                    else if (Type == 9)
                    {
                        value = Encoding.ASCII.GetString(response, 9, Convert.ToInt32(response[1]) - 9);
                        valueInt = Convert.ToInt32(value);
                        valueInt = valueInt + 100;
                        Debug.WriteLine(Convert.ToString(valueInt));
                    }
                    else if (Type == 10)
                    {
                        int startIndex = -1;
                        int endIndex = -1;
                        int commaCount = 0;
                        int index;
                        for (int i = 0; i < response.Length; i++)
                        {
                            if (response[i] == 0x2c)
                            {
                                commaCount++;
                                if (commaCount == 7)
                                {
                                    startIndex = i + 1;
                                }
                                else if (commaCount == 8 && startIndex != -1)
                                {
                                    endIndex = i;

                                }
                            }
                        }
                        index = endIndex - startIndex;
                        value = Encoding.ASCII.GetString(response, startIndex, index);
                        valueDouble = Convert.ToDouble(value);
                        valueDouble = valueDouble - 1.0;
                        Debug.WriteLine(Convert.ToString(valueDouble));
                    }
                    else if (Type == 11)
                    {
                        int startIndex = -1;
                        int endIndex = -1;
                        int commaCount = 0;
                        int index;
                        for (int i = 0; i < response.Length; i++)
                        {
                            if (response[i] == 0x2c)
                            {
                                commaCount++;
                                if (commaCount == 7)
                                {
                                    startIndex = i + 1;
                                }
                                else if (commaCount == 8 && startIndex != -1)
                                {
                                    endIndex = i;

                                }
                            }
                        }
                        index = endIndex - startIndex;
                        value = Encoding.ASCII.GetString(response, startIndex, index);
                        valueDouble = Convert.ToDouble(value);
                        valueDouble = valueDouble + 1.0;
                        Debug.WriteLine(Convert.ToString(valueDouble));
                    }
                    result = Convert.ToString(valueInt);
                    result2 = Convert.ToString(valueDouble);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to send command to the camera: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                stream.Close();
            }
            private byte[] CreateMessage(string command)
            {
                // 데이터 생성
                byte[] dataBytes = Encoding.ASCII.GetBytes(command);
                // 메시지 구조 생성
                int messageSize = dataBytes.Length; // STX(1) + Size(2) + Data + CS(1) + ETX(1)
                byte[] message = new byte[messageSize + 5];
                // STX
                message[0] = 0x02;
                // Size
                message[1] = (byte)(dataBytes.Length); // 데이터 길이 + 4 (STX, Size, CS, ETX)
                message[2] = (byte)(dataBytes.Length >> 8);
                // Data
                Array.Copy(dataBytes, 0, message, 3, dataBytes.Length);
                // CS
                byte checksum = CalculateChecksum(message, 3, messageSize); // Size부터 CS 직전까지의 데이터로 체크섬 계산
                message[messageSize + 3] = checksum;
                // ETX
                message[messageSize + 4] = 0x03;

                return message;
            }

            private byte CalculateChecksum(byte[] data, int startIndex, int length)
            {
                byte checksum = 0x00;
                for (int i = startIndex; i < startIndex + length; i++)
                {
                    checksum += data[i];
                }
                return checksum;
            }
            public void videoSave()
            {
                int frameWidth = pictureBox.Width;
                int frameHeight = pictureBox.Height;

                VideoWriter writer = new VideoWriter("output.avi", FourCC.MJPG, 30, new OpenCvSharp.Size(frameWidth, frameHeight), false);

                int targetFrameCount = 10 * 30; // 10초에 해당하는 30fps의 총 프레임 수
                Debug.WriteLine(writer.IsOpened());

                for (int frameIndex = 0; frameIndex < targetFrameCount; frameIndex++)
                {
                    Mat frame = new Mat();

                    Bitmap bitmap = (Bitmap)pictureBox.Image;
                    Mat mat = ConvertToGrayScaleMat(bitmap);
                    writer.Write(mat);
                }
                writer.Release();

            }
            private Mat ConvertToGrayScaleMat(Bitmap bitmap)
            {
                Mat mat = new Mat();

                Mat grayMat = new Mat();
                BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
                if (bmpData.Scan0 != null)
                {
                    int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                    int stride = bmpData.Stride;
                    byte[] imageData = new byte[stride * bitmap.Height];
                    Marshal.Copy(bmpData.Scan0, imageData, 0, imageData.Length);
                    bitmap.UnlockBits(bmpData);
                    mat = Cv2.ImDecode(imageData, ImreadModes.Color);
                    Cv2.CvtColor(mat, grayMat, ColorConversionCodes.BGR2GRAY);

                    return mat;
                }
                else
                {
                    bitmap.UnlockBits(bmpData);
                    return mat;
                }
            }
        }
    }
}
