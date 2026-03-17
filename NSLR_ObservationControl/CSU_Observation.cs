#define TLE_FROM_CELESTRAK

using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.Util;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;
using SGPdotNET.Propagation;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
//using SatelliteTracking.Properties;
using System.Collections;
using OpenTK.Input;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using OpenTK;
using static OpenTK.Graphics.OpenGL.GL;
using System.Windows.Forms.DataVisualization.Charting;
using NSLR_ObservationControl.Module;
using System.Threading;
using System.IO;
using System.Net;


namespace NSLR_ObservationControl
{
    public partial class CSU_Observation : UserControl, MainForm.ICollectData, MainForm.ICollectData2, UserControlManager.IKeyControl
    {
        Graphics graph;
        public static double AddAzOffSet;
        public static double AddElOffSet;
        public static tcspk tcspk_Satel;
        public static TargetInfo.Step TargetStep { get; set; }
        public Test_SatelliteAZEL _satelliteAZEL { get; set; }
        public CSU_Observation()
        {
            InitializeComponent();
            Init_chart();
          //  InitializeExternalLibrary();
            
        }



        public void Init_chart()
        {
            chart1.Series["Series1"].LabelBackColor = Color.Green;
            chart1.ChartAreas[0].AxisX.IsMarginVisible = false;
            chart1.ChartAreas[0].AxisY.IsMarginVisible = false;
            chart1.Series["Series1"].ChartType = SeriesChartType.Point;
            chart1.Series["Series1"].MarkerSize = 3;
            chart1.Series["Series1"].MarkerStyle = MarkerStyle.Circle;
            chart1.Series["Series1"].MarkerColor = Color.Yellow;
            chart1.Series["Series1"].Color = Color.Black;
            chart1.ChartAreas[0].AxisY.Minimum = -5000;
            chart1.ChartAreas[0].AxisY.Maximum = 5000;
            chart1.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
            chart1.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;
            chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Sans-serif", 12);
            chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            chart1.ChartAreas[0].AxisY.LabelStyle.Enabled = true;
            chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = true;
            chart1.ChartAreas[0].AxisX.Interval = 1000;
            chart1.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "0 'ms'";
            chart1.ChartAreas[0].AxisY.ScrollBar.Enabled = false;
            chart1.Series["Series1"].SmartLabelStyle.Enabled = true;
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "F9";
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "F9";
            chart1.Series["Series1"].LabelFormat = "F9";

            chart2.ChartAreas[0].AxisX.ScrollBar.Enabled = false;
            chart2.ChartAreas[0].AxisY.Minimum = 0;
         //   chart2.ChartAreas[0].AxisY.Maximum = 1000;
            chart2.Series["Series1"].ChartType = SeriesChartType.Bar;
            chart2.Series["Series1"].MarkerSize = 1;
            chart2.Series["Series1"].MarkerStyle = MarkerStyle.Circle;
            chart2.Series["Series1"].Color = Color.Yellow;
            chart2.Legends[0].Enabled = false;
            chart2.ChartAreas[0].AxisX.Minimum = -5000;
            chart2.ChartAreas[0].AxisX.Maximum = 5000;
            chart2.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
            chart2.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;
            chart2.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Black;
            chart2.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "0 ";

            chart1.MouseWheel += MouseWheelOnChart;
            chart1.MouseDown += Chart1_MouseDown;
            chart1.MouseMove += Chart1_MouseMove;
            chart1.MouseUp += Chart1_MouseUp;
        }
        protected void MouseWheelOnChart(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var yAxis = chart.ChartAreas[0].AxisY;
            double y, yMin, yMax;

            yMin = yAxis.ScaleView.ViewMinimum;
            yMax = yAxis.ScaleView.ViewMaximum;
            y = yAxis.PixelPositionToValue(e.Location.Y);

            if (e.Delta < 0)
            {
                ZoomOut(chart, y, yMin, yMax);
            }
            else
            {
                ZoomIn(chart, y, yMin, yMax);
            }
        }
        private void ZoomIn(Chart chart, double y, double yMin, double yMax)
        {
            double yMin2 = Math.Max(y - (y - yMin) * 0.8, chart1.ChartAreas[0].AxisY.Minimum);
            double yMax2 = Math.Min(y + (yMax - y) * 0.8, chart1.ChartAreas[0].AxisY.Maximum);

            chart1.ChartAreas[0].AxisY.ScaleView.Zoom(yMin2, yMax2);
            chart2.ChartAreas[0].AxisX.ScaleView.Zoom(yMin2, yMax2);
            chart1.ChartAreas[0].AxisY.Interval = (yMax2 - yMin2) / 10;
            chart2.ChartAreas[0].AxisX.Interval = (yMax2 - yMin2) / 10;

        }


        public void OnSatelliteDataUpdated(DateTime startTime, DateTime endTime, string satelliteName, int noradId)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => OnSatelliteDataUpdated(startTime, endTime, satelliteName, noradId)));
                return;
            }

            progressBar.Minimum = 0;
            progressBar.Maximum = (int)(endTime - startTime).TotalSeconds;

            var label1 = panel1.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblSatelliteName");
            if (label1 != null)
                label1.Text = satelliteName;

            var label2 = panel1.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblNoradId");
            if (label2 != null)
                label2.Text = (noradId == 0) ? "-" : noradId.ToString();

            ObservTime.Text = $"{startTime.ToLocalTime():HH:mm:ss} ~ {endTime.ToLocalTime():HH:mm:ss}";

            StartProgressUpdateLoop(startTime, endTime);
        }

        private Thread progressThread;
        private bool progressRunning = false;
        public void StartProgressUpdateLoop(DateTime startTime, DateTime endTime)
        {
            if (progressRunning)
                return;

            progressRunning = true;

            progressThread = new Thread(() =>
            {
                try
                {
                    while (progressRunning)
                    {
                        double elapsed;
                        if (DateTime.UtcNow <= startTime)
                        {
                            elapsed = 0;
                        }
                        else
                        {
                            elapsed = (DateTime.UtcNow - startTime).TotalSeconds;
                        }
                        double total = (endTime - startTime).TotalSeconds;
                        if (total <= 0)
                            total = 0.0001;
                        if (elapsed > total)
                            elapsed = total;

                        double progressRatio = elapsed / total; // 0.0 ~ 1.0
                        progressRatio = Math.Max(0.0, Math.Min(progressRatio, 1.0));
                        int progressValue = (int)(progressRatio * progressBar.Maximum);

                        this.BeginInvoke((Action)(() =>
                        {
                            if (progressBar.Value != progressValue)
                                progressBar.Value = progressValue;
                        }));

                        if (elapsed >= total)
                            break;

                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Progress Thread Error] {ex.Message}");
                }
                finally
                {
                    progressRunning = false;
                }
            });

            progressThread.IsBackground = true;
            progressThread.Start();
        }

        public void StopProgressLoop()
        {
            progressRunning = false;
            if (progressThread != null && progressThread.IsAlive)
                progressThread.Join(200);
        }


        private void ZoomOut(Chart chart, double y, double yMin, double yMax)
        {
            double yMin2 = Math.Max(y - (y - yMin) / 0.8, chart.ChartAreas[0].AxisY.Minimum);
            double yMax2 = Math.Min(y + (yMax - y) / 0.8, chart.ChartAreas[0].AxisY.Maximum);

            chart.ChartAreas[0].AxisY.ScaleView.Zoom(yMin2, yMax2);
            chart2.ChartAreas[0].AxisX.ScaleView.Zoom(yMin2, yMax2);
            chart1.ChartAreas[0].AxisY.Interval = (yMax2 - yMin2) / 10;
            chart2.ChartAreas[0].AxisX.Interval = (yMax2 - yMin2) / 10;
        }
        private void rangeRearrange(Chart chart, double y, double yMin, double yMax)
        {
            double yMin2 = Math.Max(y - (y - yMin), chart.ChartAreas[0].AxisY.Minimum);
            double yMax2 = Math.Min(y + (yMax - y), chart.ChartAreas[0].AxisY.Maximum);

            chart.ChartAreas[0].AxisY.ScaleView.Zoom(yMin2, yMax2);
            chart2.ChartAreas[0].AxisX.ScaleView.Zoom(yMin2, yMax2);
            chart1.ChartAreas[0].AxisY.Interval = (yMax2 - yMin2) / 10;
            chart2.ChartAreas[0].AxisX.Interval = (yMax2 - yMin2) / 10;
        }

        private void Chart1_MouseUp(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var yAxis = chart.ChartAreas[0].AxisY;
            double y, yMin, yMax;

            HitTestResult result = chart.HitTest(e.X, e.Y);

            if (result.ChartArea != null && result.ChartElementType == ChartElementType.PlottingArea)
            {
            yMin = yAxis.ScaleView.ViewMinimum;
            yMax = yAxis.ScaleView.ViewMaximum;
            y = yAxis.PixelPositionToValue(e.Location.Y);

            if (e.Button == MouseButtons.Left)
            {
                isDragging = false; // 드래그 종료

                rangeRearrange(chart, y, yMin, yMax);
            }
        }
            else
            {
                return;
            }
        }

        private void Chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Chart chart = (Chart)sender;
                int deltaX = e.X - startPoint.X; // X축 드래그 변위 계산
                int deltaY = startPoint.Y - e.Y; // Y축 드래그 변위 계산 

                chart.ChartAreas[0].AxisX.ScaleView.Position -= deltaX;
                chart.ChartAreas[0].AxisY.ScaleView.Position -= deltaY;

                try
                {
                var chartArea = chart.ChartAreas[0];
                var chartAreaPosition = chartArea.Position;
                var chartAreaInnerPlotPosition = chartArea.InnerPlotPosition;

                var chartAreaRect = new RectangleF(
    chartAreaPosition.X + chartAreaInnerPlotPosition.X,
    chartAreaPosition.Y + chartAreaInnerPlotPosition.Y,
    chartAreaInnerPlotPosition.Width,
    chartAreaInnerPlotPosition.Height
);

                var chartAreaMousePoint = new PointF(
       (e.X - chartAreaRect.Left) / chartAreaRect.Width,
       (e.Y - chartAreaRect.Top) / chartAreaRect.Height
   );

                    double yValue = chartArea.AxisY.PixelPositionToValue(chartAreaMousePoint.Y);
                chart.Series[0].ToolTip = $"Y: {yValue}";
            }
                catch (Exception ex)
                {
                    Console.WriteLine($"PixelPositionToValue 오류 발생: {ex.Message}");
                }
            }
        }
        private bool isDragging = false;
        private Point startPoint;

        private void Chart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                startPoint = e.Location;
            }
/*        private void InitializeExternalLibrary()
        {
            //_satelliteAZEL = new Test_SatelliteAZEL();
            tcspk_Satel = new tcspk();
            observation_TMS2.ExternalLibraryObject = tcspk_Satel;
            tcspk_Satel.StarExcute();
        }*/


        }
        public void reloadLibrary()
        {
            if (tcspk_Satel == null)
            {
                observation_TMS2.ExternalLibraryObject = tcspk_Satel;
                tcspk_Satel.StarExcute();
        }
        }
        public OPSUserData CollectDataFromOpticalSystem()
        {
            return opticalSystem1.CollectData();
        }
        public TMSUserData CollectDataFromTrackingMountSystem()
        {
            return observation_TMS2.CollectData();
        }

        private void CSU_Observation_Load(object sender, EventArgs e)
        {
        }

        
        public void HandleKeyPress(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    AddAzOffSet -= Observation_TMS.UserOffsetTick;
                    break;
                case Keys.D:
                    AddAzOffSet += Observation_TMS.UserOffsetTick;
                    break;
                case Keys.W:
                    AddElOffSet += Observation_TMS.UserOffsetTick;
                    break;
                case Keys.S:
                    AddElOffSet -= Observation_TMS.UserOffsetTick;
                    break;
            }
            UpdateOffsets();
        }

        private void UpdateOffsets()
        {
            Console.WriteLine($"Azimuth Offset: {AddAzOffSet}, Elevation Offset: {AddElOffSet}");
        }

        public void Execute_LaserSearching()    // 레이저 탐색 시작
        {
            Laser_NavigationTracking laser_NavigationTracking = new Laser_NavigationTracking();

            laser_NavigationTracking.temp_SearchingPattern_Function(0.1, 3); // 최소이동거리, 최대이동횟수
        }

        public void CreateDataFile_LaserSearching()     // 탐색경로 위치데이터 파일 생성
        {
            FileInfo fileInfo = new FileInfo("./" + "searching_positionData.txt");

            using (StreamWriter writer = fileInfo.CreateText())
            {
                double distance = 0.1;
                int number = 3;

                // 현재 AZ/EL 사용자 옵셋값
                double file_azUserOffset = Observation_TMS.azUserOffset;
                double file_elUserOffset = Observation_TMS.elUserOffset;

                // AZ/EL 옵셋 변경값 설정
                double file_azSettingValue = distance;
                double file_elSettingValue = Math.Sqrt((distance * distance) - ((distance / 2.0) * (distance / 2.0)));

                int count = 0;

                for (int i = 0; i < number - 1; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        for (int k = 0; k <= i; k++)
                        {
                            switch (j)
                            {
                                case 0:
                                    file_azUserOffset += file_azSettingValue / 2.0;
                                    file_elUserOffset += file_elSettingValue;
                                    break;
                                case 1:
                                    file_azUserOffset += file_azSettingValue;
                                    break;
                                case 2:
                                    file_azUserOffset += file_azSettingValue / 2.0;
                                    file_elUserOffset -= file_elSettingValue;
                                    break;
                                case 3:
                                    file_azUserOffset -= file_azSettingValue / 2.0;
                                    file_elUserOffset -= file_elSettingValue;
                                    break;
                                case 4:
                                    file_azUserOffset -= file_azSettingValue;
                                    break;
                                case 5:
                                    file_azUserOffset -= file_azSettingValue;
                                    k = i;  // 한번만 실행
                                    break;
                                case 6:
                                    file_azUserOffset -= file_azSettingValue / 2.0;
                                    file_elUserOffset += file_elSettingValue;
                                    break;
                            }

                            count++;
                            writer.WriteLine(count.ToString() + ". azUserOffset : " + file_azUserOffset.ToString() +
                                " / elUserOffset : " + file_elUserOffset.ToString());
                        }

                    }
                }


            }
            MessageBox.Show("searching Data 생성완료!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CalculateRMSInZoomedRange();
        }
        private void CalculateRMSInZoomedRange()
        {
            List<double> dataInZoomedRange = GetDataInZoomedRange();

            double rms = CalculateRMS(dataInZoomedRange.ToArray());
            label1.Text = rms.ToString("0.00 'ps'");
        }
        private List<double> GetDataInZoomedRange()
        {
            var parentControl = this.Parent;
            var chartControl = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
            var chart1 = (Chart)chartControl;
            double xMin = chart1.ChartAreas[0].AxisX.ScaleView.ViewMinimum;
            double xMax = chart1.ChartAreas[0].AxisX.ScaleView.ViewMaximum;
            double yMin = chart1.ChartAreas[0].AxisY.ScaleView.ViewMinimum;
            double yMax = chart1.ChartAreas[0].AxisY.ScaleView.ViewMaximum;

            List<double> dataInZoomedRange = new List<double>();
            foreach (var series in chart1.Series)
            {
                foreach (var point in series.Points)
                {
                    double xValue = point.XValue;
                    double yValue = point.YValues[0];

                    if (xValue >= xMin && xValue <= xMax && yValue >= yMin && yValue <= yMax)
                    {
                        dataInZoomedRange.Add(yValue);
                    }
                }
            }

            return dataInZoomedRange;
        }
        private double CalculateRMS(double[] data)
        {
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
        Observation_Processing observation_Processing = new Observation_Processing();
        private void button2_Click(object sender, EventArgs e)
        {
            var satelliteData = ObservingSatellite_Information.GetData();

            var observationDataValueTuple = observation_Control1.ObservationData;
            if (observationDataValueTuple == null || !observationDataValueTuple.Any())
            {
                MessageBox.Show("Observation 데이터가 없습니다. 데이터를 먼저 수집하세요.",
                                "알림",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }
            List<Tuple<int, double, double>> observationData = observationDataValueTuple
                .Select(v => Tuple.Create(v.Id, v.StartTime, v.TofMs))
                .ToList();
            if (observationData.Count == 0)
            {
                MessageBox.Show("Observation 데이터가 없습니다. 데이터를 먼저 수집하세요.",
                                "알림",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            observation_Processing.ObservationToSOD(
                satelliteData.Name,
                satelliteData.StartTime,
                satelliteData.EndTime,
                observationData);
        }

        private void DPS_Compute(object sender, EventArgs e)
        {
            try
            {
                string baseDir = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
                string dpsFolderPath = Path.Combine(baseDir, @"DPS\DPS_EXE\Release");
                string dpsExePath = Path.Combine(dpsFolderPath, "DPS.exe");
                if (File.Exists(dpsExePath))
                {
                    Process externalProcess = new Process();
                    externalProcess.StartInfo.FileName = dpsExePath;
                    externalProcess.StartInfo.WorkingDirectory = dpsFolderPath;

                    externalProcess.Start();
                }
                else
                {
                    MessageBox.Show("DPS.exe 파일을 찾을 수 없습니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DPS 실행 중 오류가 발생했습니다: {ex.Message}");
            }

        }
    }
}