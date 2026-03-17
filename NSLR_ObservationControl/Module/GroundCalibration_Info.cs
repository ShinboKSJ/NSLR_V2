using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
using Npgsql;
using static NSLR_ObservationControl.Tcp;
using static NSLR_ObservationControl.ControlCommand;
using static NSLR_ObservationControl.CSU_GroundCalibration;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static NSLR_ObservationControl.Module.groundResult;
using NSLR_ObservationControl.Subsystem;

namespace NSLR_ObservationControl.Module
{
    public partial class GroundCalibration_Info : UserControl
    {
        static StringBuilder consoleOutput = new StringBuilder(); // Console 출력값을 저장할 StringBuilder 객체
                                                                  //MainForm mainForm = new MainForm();
        static StringBuilder consoleRawData = new StringBuilder();
        public Socket clientSocket;
        public NetworkStream stream; // 소켓의 데이터 흐름

        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;
        private const string SatelliteIP_ET = "192.168.10.51";
        private const string DebIP_ET = "192.168.10.61";

        const int SERVER_MODE = 0;
        private int Op_mode = 0;
        const string MSG_NO_CONNECT = "Waiting for a Client.......";
        const string MSG_CONNECT = " +++ Connected  +++";
        //  const string OPPONENT_IP = "192.168.0.31";
        const int PORT = 1111;

        Tcp cTcpServer;
        CSU_GroundCalibration csu_groundcalibration;

        int count = 0;
        int countSet = 10000;

        Dictionary<int, int> data = new Dictionary<int, int>();

        TaskControl taskControl = TaskControl.instance;

        private LAS_SAT_Controller lasSATcontrol = LAS_SAT_Controller.instance;
        private LAS_DEB_Controller lasDEBcontrol = LAS_DEB_Controller.instance;
        private RGG_SAT_Controller rggSATcontrol = RGG_SAT_Controller.instance;
        private RGG_DEB_Controller rggDEBcontrol = RGG_DEB_Controller.instance;
        private DOM_Controller_v40 domControler;
        public void OnAzElUpdated(double az, double el)
        {
            this.BeginInvoke((MethodInvoker)(() =>
            {
                ((Label)this.panel_OPS.Controls["label55"]).Text = az.ToString("F5");
                ((Label)this.panel_OPS.Controls["label38"]).Text = el.ToString("F5");
            }));
        }
        public void chart_update(int sum, int count)
        {
            //   Console.WriteLine("chart_updating");
            var parentControl = this.Parent;
            var chartControl = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
            var chart1 = (Chart)chartControl;
            //chart1.Series[0].Points.AddXY(count, sum);
            //var parentControl = this.Parent;
            var chartControl2 = parentControl.Controls.Find("chart2", true).FirstOrDefault() as Chart;
            var chart2 = (Chart)chartControl2;
            Console.WriteLine(count);
            chart2.Series[0].Points.AddXY(sum, data[sum]);
        }

        /*        public void chart2_update(int sum)
                {
                    var parentControl = this.Parent;
                    var chartControl = parentControl.Controls.Find("chart2", true).FirstOrDefault() as Chart;
                    var chart2 = (Chart)chartControl;

                    *//*            if (data.ContainsKey(sum))
                                {
                                    data[sum]++;
                                }
                                else
                                {
                                    data[sum] = 1;
                                }
                                //  chart2.Series[0].Points.AddXY(data[sum], sum);
                                chart2.Series[0].Points.AddXY(sum,data[sum]);*//*
                    lock (lockObject)
                    {
                        if (data.ContainsKey(sum))
                        {
                            data[sum]++;
                        }
                        else
                        {
                            data[sum] = 1;
                        }
                        chart2.Series[0].Points.AddXY(sum, data[sum]);
                    }
                }*/
        private int minY = 0;
        private int maxY = int.MinValue;
        public void chartInit(int sum,int CalculatedGCdistance)
        {
            var parentControl = this.Parent;
            var chartControl = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
            var chart1 = (Chart)chartControl;
            var chartControl2 = parentControl.Controls.Find("chart2", true).FirstOrDefault() as Chart;
            var chart2 = (Chart)chartControl2;
            /*chart1.ChartAreas[0].AxisY.Minimum = sum - 20000;
            chart1.ChartAreas[0].AxisY.Maximum = sum + 20000;
            chart2.ChartAreas[0].AxisX.Minimum = sum - 20000;
            chart2.ChartAreas[0].AxisX.Maximum = sum + 20000;
 */
            //chart1.ChartAreas[0].AxisY.Minimum = sum - 100000;
            //chart1.ChartAreas[0].AxisY.Maximum = sum + 100000;
            //chart2.ChartAreas[0].AxisX.Minimum = sum - 100000;
            //chart2.ChartAreas[0].AxisX.Maximum = sum + 100000;
            //chart1.ChartAreas[0].AxisY.Interval = 2000;
            //chart2.ChartAreas[0].AxisX.Interval = 2000;
            //chart1.ChartAreas[0].AxisX.Maximum = 20000;
           // minY = Math.Max(minY, sum); 
           // maxY = Math.Max(maxY, sum);/*
            chart1.ChartAreas[0].AxisY.Interval = 20000;
            chart1.ChartAreas[0].AxisX.Maximum = 5000;
            chart1.ChartAreas[0].AxisX.Interval = 2500;
            chart1.ChartAreas[0].AxisY.Minimum = CalculatedGCdistance - 200000; 
            chart1.ChartAreas[0].AxisY.Maximum = CalculatedGCdistance + 200000; 
            chart1.ChartAreas[0].AxisY.Interval = 20000;
            chart2.ChartAreas[0].AxisX.Minimum = CalculatedGCdistance - 200000;
            chart2.ChartAreas[0].AxisX.Maximum = CalculatedGCdistance + 200000;
            chart2.ChartAreas[0].AxisX.Interval = 20000;
        }
        public void chartInit(int CalculatedGCdistance)
        {
            var parentControl = this.Parent;
            var chartControl = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
            var chart1 = (Chart)chartControl;
            var chartControl2 = parentControl.Controls.Find("chart2", true).FirstOrDefault() as Chart;
            var chart2 = (Chart)chartControl2;
            /*chart1.ChartAreas[0].AxisY.Minimum = sum - 20000;
            chart1.ChartAreas[0].AxisY.Maximum = sum + 20000;
            chart2.ChartAreas[0].AxisX.Minimum = sum - 20000;
            chart2.ChartAreas[0].AxisX.Maximum = sum + 20000;
 */
            //chart1.ChartAreas[0].AxisY.Minimum = sum - 100000;
            //chart1.ChartAreas[0].AxisY.Maximum = sum + 100000;
            //chart2.ChartAreas[0].AxisX.Minimum = sum - 100000;
            //chart2.ChartAreas[0].AxisX.Maximum = sum + 100000;
            //chart1.ChartAreas[0].AxisY.Interval = 2000;
            //chart2.ChartAreas[0].AxisX.Interval = 2000;
            //chart1.ChartAreas[0].AxisX.Maximum = 20000;
            // minY = Math.Max(minY, sum); 
            // maxY = Math.Max(maxY, sum);/*
            chart1.ChartAreas[0].AxisY.Interval = 20000;
            chart1.ChartAreas[0].AxisX.Maximum = 5000;
            chart1.ChartAreas[0].AxisX.Interval = 2500;
            chart1.ChartAreas[0].AxisY.Minimum =  0;
            chart1.ChartAreas[0].AxisY.Maximum =  600000;
            chart1.ChartAreas[0].AxisY.Interval = 20000;
            chart2.ChartAreas[0].AxisX.Minimum = 0;
            chart2.ChartAreas[0].AxisX.Maximum =  600000;
            chart2.ChartAreas[0].AxisX.Interval = 20000;
        }
        public readonly double InitAzPointingPosition;
        public readonly double InitElPointingPosition;

        public GroundCalibration_Info()
        {
            InitializeComponent();
            domControler = DOM_Controller_v40.instance;

            if (double.TryParse(richTextBox1.Text, out double az))
            {
                InitAzPointingPosition = az;
        }
            else
            {
                InitAzPointingPosition = 0; 
            }
            if (double.TryParse(richTextBox2.Text, out double el))
            {
                InitElPointingPosition = el;
            }
            else
            {
                InitElPointingPosition = 0;
            }
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox2.SelectionAlignment = HorizontalAlignment.Center;
            
        }

        double rms1;
        private void sql_Init()
        {
            string sql = "SELECT date_time, rms FROM ground ORDER BY date_time DESC LIMIT 1";

            // NpgsqlConnection 객체를 생성합니다.
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                // NpgsqlCommand 객체를 생성합니다.
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    // NpgsqlConnection을 Open 합니다.
                    connection.Open();
                    NpgsqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        DateTime datetime = reader.GetDateTime(0);
                        rms1 = reader.GetDouble(1);
                        label10.Text = datetime.ToString();
                        label17.Text = rms1.ToString("F3") + " ps";
                    }
                    reader.Close();
                    connection.Close();
                }
            }
        }
        const int CalculatedGCdistance = 679177;
        //const int CalculatedGCdistance = 825150; //101.8062m + 21.88m
        private async void button_groundCal_start_Click(object sender, EventArgs e)
        {
            button_GroundCalApply.Enabled = true;
            string ip = "192.168.10.51";
            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            {
                rggSATcontrol.SetGrndCalMode(); // RGG 준비 - Range Mode
                ip = "192.168.10.51";
            }
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
        {
                rggDEBcontrol.SetGrndCalMode(); // RGG 준비 - Range Mode
                ip = "192.168.10.61";
                lasDEBcontrol.laserStart_GroundCal();
            }

            int ReadResponse = 0;
            // 이벤트 타이밍 시스템의 IP 주소와 포트 번호 설정
            //string ip = "192.168.0.11";
            int port = 33228;

            taskControl.SetMutex("Ground_Calibration");

            button_groundCal_start.Enabled = false;
            button_groundCal_shoot.Enabled = false;
            button_groundCal_stop.Enabled = false;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(ip, port);
            stream = new NetworkStream(clientSocket);
            Console.WriteLine("Connected to server");

            byte[] peer3 = new byte[]
               {
                            0xbc, 0x01, 0x00,0x00
               };
            await Task.Run(() =>
            {
                stream.Write(peer3, 0, peer3.Length);
            });
            byte[] buffer = new byte[12];
            bool flag = false;
            while (!flag)
            {
                ReadResponse = stream.Read(buffer, 0, buffer.Length);
                for (int i = 0; i < ReadResponse; i += 1)
                {
                    if (buffer.Skip(0).Take(1).SequenceEqual(new byte[] { 0xbc }))
                    {
                        if (buffer.Skip(1).Take(3).SequenceEqual(new byte[] { 0x01, 0x00, 0x00 }))
                        {
                            Console.WriteLine(buffer.Skip(0).Take(12));
                            button_groundCal_start.Enabled = false;
                            button_groundCal_shoot.Enabled = true;
                            button_groundCal_stop.Enabled = true;
                            flag = true;
                        }
                    }
                }
            }
            byte[] peer1 = new byte[]{
                            0x09, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,0x02,
                            0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00,0xe8, 0x03, 0x00, 0x00
                        }; // 1000k
                           //1k
            /*        byte[] peer1 = new byte[]{
                         0x09, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,0x02,
                         0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00,0x01, 0x00, 0x00, 0x00
                     };*/
            stream.Write(peer1, 0, peer1.Length);

            var parentControl = this.Parent;
            var chartControl = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
            var chart1 = (Chart)chartControl;
            var chartControl2 = parentControl.Controls.Find("chart2", true).FirstOrDefault() as Chart;
            var chart2 = (Chart)chartControl2;
            foreach (Series series in chart1.Series)
            {
                chart1.Series[0].Points.Clear();
            }

            foreach (Series series in chart2.Series)
            {
                chart2.Series[0].Points.Clear();
            }
            count = 0;
            data.Clear();
        }
        private List<(int High, int Low)> RawDataSet = new List<(int, int)>();
        private async void button_groundCal_shoot_Click_1(object sender, EventArgs e)
        {
            var parentControl = this.Parent;
            var chartControl = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
            var chart1 = (Chart)chartControl;

            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            {
                lasSATcontrol.laserStart_GroundCal();
            }
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                lasDEBcontrol.laserStart_GroundCal();
            }
            byte[] peer2 = new byte[]{
                0xe7, 0x03, 0x00, 0x00, 0x80, 0x00, 0x00, 0x00
            };
            stream.Write(peer2, 0, peer2.Length);
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;
            button_groundCal_start.Enabled = false;
            button_groundCal_shoot.Enabled = false;
            button_groundCal_stop.Enabled = true;
            button_GroundCalApply.Enabled = true;

            int sum = 0;
            int highValue = 0;
            int lowValue = 0;
            int bufferSize = 1024;
            List<int> dataBuffer = new List<int>();
            int Threshold16ms = 10000000;
            while (!cancellationToken.IsCancellationRequested)
            {
                if (stream == null || stream.CanRead == false || stream.CanWrite == false)
                {
                    break;
                }
                try
                {
                    byte[] buffer = new byte[bufferSize];
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    for (int i = 0; i < bytesRead; i += 16)
                    {
                        // 레이저 반환율 Checking >> total_laserCount ////////////////////////////////////
                        if (Laser_NavigationTracking.calculate_flag)
                        {
                            if (Laser_NavigationTracking.total_laserCount > Laser_NavigationTracking.total_maxLaserCount)
                            {
                                Laser_NavigationTracking.calculate_flag = false;
                            }
                            else
                            {
                                Laser_NavigationTracking.total_laserCount++;
                            }
                        }
                        //////////////////////////////////////////////////////////////////////////////////

                        byte[] A = buffer.Skip(i + 0).Take(4).ToArray();
                        byte[] B = buffer.Skip(i + 8).Take(4).ToArray();
                        if (bytesRead - i >= 16)
                        {
                            //totalCount++;
                            if (Math.Abs(BitConverter.ToInt32(A, 0)) == BitConverter.ToInt32(B, 0))
                            {
                                // 레이저 반환율 Checking >> return_laserCount ////////////////////////////////////
                                if (Laser_NavigationTracking.calculate_flag)
                                {
                                    Laser_NavigationTracking.return_laserCount++;
                                }
                                //////////////////////////////////////////////////////////////////////////////////

                                byte[] highBytes = buffer.Skip(i + 4).Take(4).ToArray();
                                byte[] lowBytes = buffer.Skip(i + 12).Take(4).ToArray();
                                highValue = BitConverter.ToInt32(highBytes, 0);
                                lowValue = BitConverter.ToInt32(lowBytes, 0);

                                sum = (int)Math.Abs(highValue - lowValue);
                                if (sum > Threshold16ms)
                                {
                                    continue;
                                }
                              //  Console.WriteLine("HH : " + highValue);
                               // Console.WriteLine("LL : " + lowValue);
                                sum -= CalculatedGCdistance;
                                Console.WriteLine("SUM : " + sum);
                                dataBuffer.Add(sum);
                                RawDataSet.Add((highValue, lowValue));
                             /*   if (count == 0)
                                {
                                    chartInit(sum, CalculatedGCdistance);
                                }*/
                                count++;
                                if (data.ContainsKey(sum))
                                {
                                    data[sum]++;
                                }
                                else
                                {
                                    data[sum] = 1;
                                }
                                if (count % 100 == 0)  //300
                                {
                                    this.Invoke((MethodInvoker)delegate
                                    {
                                        ProcessDataBuffer(dataBuffer);
                                        dataBuffer.Clear();

                                    });
                                }
                                if (count == chart1.ChartAreas[0].AxisX.Maximum && chart1.ChartAreas[0].AxisX.Maximum != countSet)
                                {
                                    double newMax = chart1.ChartAreas[0].AxisX.Maximum + 10000;

                                    chart1.ChartAreas[0].AxisX.Maximum = (newMax > countSet) ? countSet : newMax;
                                }
                                this.Invoke((MethodInvoker)delegate
                                {

                                    chart_update(sum, count);

                                    if (count == countSet)
                                    {
                                        if (cTcpServer != null)
                                        {
                                            cTcpServer.Dispose();
                                        }
                                        if (stream != null)
                                        {
                                            stream.Dispose();
                                            stream = null;
                                        }
                                        if (clientSocket != null && clientSocket.Connected)
                                        {
                                            clientSocket.Shutdown(SocketShutdown.Both);
                                            clientSocket.Close();
                                            clientSocket = null;
                                        }

                                        button_groundCal_start.Enabled = true;
                                        button_groundCal_shoot.Enabled = false;
                                        button_groundCal_stop.Enabled = false;

                                        Console.WriteLine("종료");

                                    }
                                });
                            }
                            else if (BitConverter.ToInt32(B, 0) < 0) i -= 8;
                        }
                    }
                }
                catch (ObjectDisposedException ex)
                {
                    break;
                }
            }
        }
        public void ProcessDataBuffer(List<int> dataBuffer)
        {
            var parentControl = this.Parent;
            var chartControl = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
            var chart1 = (Chart)chartControl;
            foreach (int data in dataBuffer)
            {
                chart1.Series[0].Points.Add(data);
            }
            chart1.Update();
        }
        private async void button_groundCal_stop_Click(object sender, EventArgs e)
        {
            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
                      {
                lasSATcontrol.laserStop_GroundCal();
            }
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                lasDEBcontrol.laserStop_GroundCal();
            }

            if (stream != null)
            {
                await stream.FlushAsync();
                await Task.Delay(100); // ReadAsync 작업이 완료되도록 대기
                stream.Dispose();
                stream = null;
            }
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                clientSocket = null;
            }
            button_groundCal_start.Enabled = true;
            button_groundCal_shoot.Enabled = false;
            button_groundCal_stop.Enabled = false;
            count = 0;

            taskControl.SetMutex("None"); //Jason 24.11.18 다른 임무 가능

        }
        public void EmergencyStop()
        {
            if (stream != null)
            {
                stream.FlushAsync();
                stream.Dispose();
                stream = null;
            }
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                clientSocket = null;
            }
            button_groundCal_start.Enabled = true;
            button_groundCal_shoot.Enabled = false;
            button_groundCal_stop.Enabled = false;
            count = 0;
        }
        private async void button_groundCal_start_Click_1(object sender, EventArgs e)
        {
            GroudCalSystemInit();
            chartInit(CalculatedGCdistance);
            button_GroundCalApply.Enabled = true;

            string ip = "192.168.10.51";
            int port = 33228;

            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            {
                lasSATcontrol.laserReady_GroundCal();
                rggSATcontrol.SetGrndCalMode(); // RGG 준비 - Range Mode
                ip = SatelliteIP_ET;
            }
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                lasDEBcontrol.laserReady_GroundCal();
                rggDEBcontrol.SetGrndCalMode(); // RGG 준비 - Range Mode
                ip = DebIP_ET;
            }

            int ReadResponse = 0;

            button_groundCal_start.Enabled = false;
            button_groundCal_shoot.Enabled = false;
            button_groundCal_stop.Enabled = false;
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(ip, port);
            stream = new NetworkStream(clientSocket);
            Console.WriteLine("Connected to server");

            byte[] peer3 = new byte[]
               {
                            0xbc, 0x01, 0x00,0x00
               };
            await Task.Run(() =>
            {
                stream.Write(peer3, 0, peer3.Length);
            });
            byte[] buffer = new byte[12];
            bool flag = false;
            while (!flag)
            {
                ReadResponse = stream.Read(buffer, 0, buffer.Length);
                for (int i = 0; i < ReadResponse; i += 1)
                {
                    if (buffer.Skip(0).Take(1).SequenceEqual(new byte[] { 0xbc }))
                    {
                        if (buffer.Skip(1).Take(3).SequenceEqual(new byte[] { 0x01, 0x00, 0x00 }))
                        {
                            Console.WriteLine(buffer.Skip(0).Take(12));
                            button_groundCal_start.Enabled = false;
                            button_groundCal_shoot.Enabled = true;
                            button_groundCal_stop.Enabled = true;
                            flag = true;
                        }
                    }
                }
            }
            byte[] peer1 = new byte[]{
                            0x09, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,0x02,
                            0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00,0xe8, 0x03, 0x00, 0x00
                        }; // 1000k
                           //1k
            /*        byte[] peer1 = new byte[]{
                         0x09, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,0x02,
                         0x00, 0x00, 0x00, 0x40, 0x00, 0x00, 0x00,0x01, 0x00, 0x00, 0x00
                     };*/
            stream.Write(peer1, 0, peer1.Length);
        }
        private void GroudCalSystemInit()
        {
            var parentControl = this.Parent;
            var chartControl = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
            var chart1 = (Chart)chartControl;
            var chartControl2 = parentControl.Controls.Find("chart2", true).FirstOrDefault() as Chart;
            var chart2 = (Chart)chartControl2;
            foreach (Series series in chart1.Series)
            {
                chart1.Series[0].Points.Clear();
            }
            foreach (Series series in chart2.Series)
            {
                chart2.Series[0].Points.Clear();
            }
            count = 0;
            data.Clear();
        }
        private void button_groundCal_shootStop_Click(object sender, EventArgs e)
        {
            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            {
                lasSATcontrol.laserStop_GroundCal();
            }
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {
                lasDEBcontrol.laserStop_GroundCal();
            }
        }
        double? rms, avg;
        private void button3_Click(object sender, EventArgs e)
        {
            DateTime clickTime = DateTime.Now;
            var parentControl = this.Parent.Parent as CSU_GroundCalibration;

            if (parentControl != null)
            {
                (rms, avg) = parentControl.GetValues();
                GlobalVariables.SystemDelay = (int)avg;
            }
            else
            {
                MessageBox.Show("부모 컨트롤이 예상한 타입이 아닙니다."); // 디버깅용
            }
            string newLine = Console_WriteLine(rms, avg, clickTime);
            if (string.IsNullOrEmpty(newLine))
            {
                return;
            }
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "GroundCalibrationResult");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = Path.Combine(folderPath, "GroundCalibrationResult.txt");

            File.AppendAllText(filePath, newLine + Environment.NewLine);

            Console.WriteLine("데이터 값이 파일에 저장되었습니다.");
        }
        static string Console_WriteLine(double? value, double? value2, DateTime time)
        {
            if (value == null || value2 == null || value == 0 || value2 == 0)
            {
                MessageBox.Show(" 산출된 결과 확인 필요", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null; 
            }

            return $"[{time:yyyy-MM-dd HH:mm:ss}] AVG : {value2.Value} ps , RMS : {value.Value} ps";
        }
        static void Console_WriteLine(int value, int value2) // 디버깅용
        {
            consoleOutput.AppendLine($"A : {value} , B : {value2}");
        }
        static string Console_WriteLine(List<(int High, int Low)> RawData)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var pair in RawData)
            {
                sb.AppendLine($"startTime : {pair.High} , EndTime : {pair.Low}");
            }

            return sb.ToString();
        }
        private void button_target_turn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int inputValue))
            {
                countSet = inputValue;
                var parentControl = this.Parent;
                var chartControl = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
                var chart1 = (Chart)chartControl;
                //chart1.ChartAreas[0].AxisX.Maximum = countSet;
            }
        }

        private void button_GroundCalApply_Click(object sender, EventArgs e)
        {
            var parentControl = this.Parent;
            var chartControl = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
            var chart1 = (Chart)chartControl;

            double xMin = chart1.ChartAreas[0].AxisX.Minimum;
            double xMax = chart1.ChartAreas[0].AxisX.Maximum;
            double yMin = chart1.ChartAreas[0].AxisY.Minimum;
            double yMax = chart1.ChartAreas[0].AxisY.Maximum;

            List<double> dataInRange = new List<double>();

            foreach (var series in chart1.Series)
            {
                foreach (var point in series.Points)
                {
                    double xValue = point.XValue;
                    double yValue = point.YValues[0];

                    if (xValue >= xMin && xValue <= xMax && yValue >= yMin && yValue <= yMax)
                    {
                        dataInRange.Add(yValue);
                    }
                }
            }
            double totalRMS = CalculateRMS(dataInRange.ToArray());
            label11.Text = totalRMS.ToString("F3") + " ps";
            label4.Text = DateTime.Now.ToString();
            // SQL 쿼리 문자열
            string sql = "INSERT INTO ground (date_time, rms) VALUES (@datetime, @rms)";

            // NpgsqlConnection 객체를 생성합니다.
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@datetime", DateTime.Now);
                    command.Parameters.AddWithValue("@rms", totalRMS);
                    connection.Open();

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("RMS 정보가 저장되었습니다.");
                    }
                    else
                    {
                        Console.WriteLine("RMS 정보 저장이 실패되었습니다.");
                    }
                }
            }
            double value1, value2;
            double result;
            result = totalRMS - rms1;
            label13.Text = result.ToString("F3") + " ps";
            label8.Text = DateTime.Now.ToString();
            if (avg.HasValue)
            {
                GlobalVariables.SystemDelay = avg.Value;
                MessageBox.Show("SystemDelay 적용 : {avg.Value}");
        }
        }

        private double CalculateRMS(double[] data)
        {
            // 데이터에 대한 RMS 값을 계산하는 로직
            double sumOfSquares = 0.0;
            double sumOfSquares2 = 0.0;
            foreach (var value in data)
            {
                sumOfSquares += value * value;
            }
            double meanSquare = sumOfSquares / data.Length;
            double mean = Math.Sqrt(meanSquare);
            foreach (var value in data)
            {
                sumOfSquares2 += (value - mean) * (value - mean);
            }
            double meanSquare2 = sumOfSquares2 / data.Length;
            double rms = Math.Sqrt(meanSquare2);

            return rms;
        }
        private void button_groudCalResult_Click(object sender, EventArgs e)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "GroundCalibrationResult");
            string filePath = Path.Combine(folderPath, "GroundCalibrationResult.txt");

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                groundResult groundresult = new groundResult();
                groundresult.Show();

                groundresult.listView1.Items.Clear();

                foreach (string line in lines)
                {
                    string time = line.Split(']')[0].Trim('[');

                    int avgStartIndex = line.IndexOf("AVG : ") + 6;  
                    int rmsStartIndex = line.IndexOf("RMS : ") + 6;  

                    string avg = line.Substring(avgStartIndex, line.IndexOf(" ps") - avgStartIndex).Trim();
                    string rms = line.Substring(rmsStartIndex, line.LastIndexOf(" ps") - rmsStartIndex).Trim();

                    var listViewItem = new ListViewItem(time);  
                    listViewItem.SubItems.Add(avg);  
                    listViewItem.SubItems.Add(rms);  

                    groundresult.listView1.Items.Add(listViewItem);
                }
            }
            else
            {
                MessageBox.Show("저장된 데이터가 없습니다.");
            }
        }
        public double AzValue { get; private set; }
        public double ElValue { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {
  /*          double a, b;

            bool isAValid = double.TryParse(richTextBox1.Text, out a);
            bool isBValid = double.TryParse(richTextBox2.Text, out b);
            if (isAValid && isBValid)
            {
                AzValue = a;
                ElValue = b;
                domControler.doPositionMove(AzValue, ElValue);
                AccessParentControl(AzValue, ElValue);
            }
            else
            {
                MessageBox.Show("Label 값 중 숫자로 변환할 수 없는 항목이 있습니다.", "변환 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }*/
            if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.SLR)
            {

            }
            else if (DutyModeNow.CurrentSystemObject == DutyModeNow.SystemObject.DLT)
            {

            }

        }
        public void AccessParentControl(double Az, double El)
        {
            var parentControl = this.Parent.Parent as CSU_GroundCalibration;
            if (parentControl != null)
            {
                parentControl.GroundTargetMount.groundTargetPoiting( Az,  El);
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            switch (Convert.ToInt32(btn.Tag))
            {
                case 1: // EL UP
                    GroundTargetMount.elUserOffset += UserOffsetTick;
                    break;
                case 2: // AZ LEFT
                    GroundTargetMount.azUserOffset -= UserOffsetTick;
                    break;
                case 3: // EL DOWN
                    GroundTargetMount.elUserOffset -= UserOffsetTick;
                    break;
                case 4: // AZ RIGHT
                    GroundTargetMount.azUserOffset += UserOffsetTick;
                    break;

            }
        }
        public double UserOffsetTick { get; set; } = 0.001;

        private void button2_Click(object sender, EventArgs e)
        {
            var parentControl = this.Parent.Parent as CSU_GroundCalibration;

            string newLine = Console_WriteLine(RawDataSet);
            if (string.IsNullOrEmpty(newLine))
            {
                return;
            }

            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GroundCalibrationRawData");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filePath = Path.Combine(folderPath, $"GroundCalRawData_{timeStamp}.txt");

            File.WriteAllText(filePath, newLine);

            Console.WriteLine("데이터 값이 파일에 저장되었습니다.");
        }

        private void button24_Click(object sender, EventArgs e)
        {
            string ControlSensitive = textBox5.Text;
            if (!string.IsNullOrEmpty(ControlSensitive) &&
                double.TryParse(ControlSensitive, out double Sensitive) &&
                Sensitive >= 0.000001 && Sensitive <= 1)
            {
                UserOffsetTick = Sensitive;
                offset.Text = $"Offset 민감도 - {UserOffsetTick}";
            }
            else
            {
                MessageBox.Show("0.000001 ~ 1 사이의 값을 입력하세요.");
            }
        }


        }
    }
