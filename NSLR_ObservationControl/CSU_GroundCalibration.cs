using log4net;
using Newtonsoft.Json.Linq;
using NSLR_ObservationControl.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NSLR_ObservationControl
{
    
    public partial class CSU_GroundCalibration : UserControl , MainForm.ICollectData
    {
        public delegate void ValueUpdatedEventHandler(double AzUpdate, double ElUpdate);
        public event ValueUpdatedEventHandler ValueUpdated;

        private double _azimuth;
        private double _elevation;

        public double Azimuth
        {
            get { return _azimuth; }
            set
            {
                _azimuth = value;
                OnValueUpdated();
            }
        }

        public double Elevation
        {
            get { return _elevation; }
            set
            {
                _elevation = value;
                OnValueUpdated();
            }
        }

        private void OnValueUpdated()
        {
            ValueUpdated?.Invoke(_azimuth, _elevation);
        }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public GroundTargetMount GroundTargetMount = null;
        public CSU_GroundCalibration()
        {
            InitializeComponent();

            //wms1.Log += Uc_Logger;
            //tms1.Log += Uc_Logger;
            //ops1.Log += Uc_Logger;
            //las1.Log += Uc_Logger;
            //oes1.Log += Uc_Logger;            
            //cds1.Log += Uc_Logger;
            //gps1.Log += Uc_Logger;
            Init_chart();
            GroundTargetMount = new GroundTargetMount();
            GroundTargetMount.UdpDataReceived += (az, el) =>
            {
                this.Azimuth = az;
                this.Elevation = el;
            };
            this.ValueUpdated += groundCalibration_Info1.OnAzElUpdated;

            log.Info($" [CSU_GroundCalibration] 생성 ");
        }
        public OPSUserData CollectDataFromOpticalSystem()
        {
           return opticalSystem1.CollectData();
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
            chart1.ChartAreas[0].AxisY.Minimum = Double.NaN;
            chart1.ChartAreas[0].AxisY.Maximum = Double.NaN;
            chart1.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
            chart1.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;
            chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Sans-serif", 12);
            chart1.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            chart1.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.White;
            chart1.ChartAreas[0].AxisY.LabelStyle.Enabled = true;
            chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = true;
            chart1.ChartAreas[0].AxisX.Interval = 10000;
            chart1.ChartAreas[0].AxisX.Maximum = 20000;
            chart1.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
            chart1.ChartAreas[0].AxisX.Minimum = 0;          
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = "0 'ps'";
            chart1.ChartAreas[0].AxisY.ScrollBar.Enabled = false;
            chart1.Series["Series1"].SmartLabelStyle.Enabled= true;
            //chart1.Series[0].YValuesPerPoint = 10;

            chart2.ChartAreas[0].AxisX.ScrollBar.Enabled = false;
            chart2.ChartAreas[0].AxisY.Minimum = 0;
            chart2.ChartAreas[0].AxisY.Maximum = 1000;
            chart2.Series["Series1"].ChartType = SeriesChartType.Bar;
            chart2.Series["Series1"].MarkerSize = 1;
            chart2.Series["Series1"].MarkerStyle = MarkerStyle.Circle;
            chart2.Series["Series1"].Color = Color.Yellow;
            chart2.Legends[0].Enabled = false;
            chart2.ChartAreas[0].AxisX.Minimum = Double.NaN;
            chart2.ChartAreas[0].AxisX.Maximum = Double.NaN;
            chart2.ChartAreas[0].AxisY.Enabled = AxisEnabled.True;
            chart2.ChartAreas[0].AxisX.Enabled = AxisEnabled.True;
            chart2.ChartAreas[0].AxisX.LabelStyle.ForeColor = Color.Black;
            chart2.ChartAreas[0].AxisY.LabelStyle.ForeColor = Color.White;
            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "0 ";

            // 드래그 이벤트 핸들러 등록
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

            if (e.Delta < 0) // Scrolled down for zoom out.
            {
                ZoomOut(chart, y, yMin, yMax);
            }
            else //if (e.Delta > 0) // Scrolled up for zoom in.
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

            yMin = yAxis.ScaleView.ViewMinimum;
            yMax = yAxis.ScaleView.ViewMaximum;
            y = yAxis.PixelPositionToValue(e.Location.Y);

            if (e.Button == MouseButtons.Left)
            {
                isDragging = false; // 드래그 종료

                rangeRearrange(chart, y, yMin, yMax);
                //dataRearrange();
            }
        }

        private void Chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Chart chart = (Chart)sender;
                int deltaX = e.X - startPoint.X; // X축 드래그 변위 계산
                int deltaY = startPoint.Y - e.Y; // Y축 드래그 변위 계산 (변경된 부분)

                // 드래그에 따라 차트의 X, Y 축 범위 조정
                chart.ChartAreas[0].AxisX.ScaleView.Position -= deltaX;
                chart.ChartAreas[0].AxisY.ScaleView.Position -= deltaY;

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
                // dataRearrange();
            }
        }
        private bool isDragging = false; // 드래그 중인지 여부를 나타내는 변수
        private Point startPoint; // 드래그 시작 지점을 저장하는 변수

        private void Chart1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true; // 드래그 시작
                startPoint = e.Location; // 드래그 시작 지점 저장
            }
        }

        private void groundCalibration_Info1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CalculateRMSInZoomedRange();
        }


        private List<double> GetDataInZoomedRange()
        {
            // MSChart에서 확대/축소된 범위의 X축 및 Y축 값을 가져옴
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
        public double rms { get; set; }
        public double avg { get; set; }

        private void CalculateRMSInZoomedRange()
        {
            List<double> dataInZoomedRange = GetDataInZoomedRange();
            (rms,avg) = CalculateRMS(dataInZoomedRange.ToArray());

            string rmsTime = rms.ToString("0.000 'ps'");

            // double distance = avg * 0.00029979;
            //  string Distance = distance.ToString("0.000 'm
            //label1.Text = $"{rmsTime} / {Distance}";
            label1.Text = $"{rmsTime} / {avg}";
        }
        public (double? rms, double? avg) GetValues()
        {
            return (rms, avg);
        }
        public double totalRMS
        {
            get { return totalRMS; }
            set { totalRMS = value; }
        }
/*        public double CalculateRMS()
        {
            // MSChart에서 확대/축소된 범위의 X축 및 Y축 값을 가져옴
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
            return totalRMS = CalculateRMS(dataInRange.ToArray());

        }*/

/*        private double CalculateRMS(double[] data)
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
        }*/

        private (double rms, double average) CalculateRMS(double[] data)
        {
            if (data == null || data.Length == 0)
                return (0.0, 0.0); 

            double sum = 0.0;
            foreach (var value in data)
            {
                sum += value;
            }

            double average = sum / data.Length;
            
            double sumOfSquares = 0.0;
            foreach (var value in data)
            {
                double diff = value - average;
                sumOfSquares += diff * diff;
        }

            double meanSquare = sumOfSquares / data.Length;
            double rms = Math.Sqrt(meanSquare); 

            return (rms, average);
        }
        private void CSU_GroundCalibration_Load(object sender, EventArgs e)
        {
            GroundCalibration_Info Info = new GroundCalibration_Info();
        }






        /*
        private void lgao_Click(object sender, EventArgs e)
        {
          //  if (Log != null)
               Log(LOG.I, "[CSU SystemMonitoring] LGAO ");
        }
        private void ldap_Click(object sender, EventArgs e)
        {
            Log(LOG.I, "[CSU SystemMonitoring] LDAP ");
        }


        
        private void Uc_Logger(LOG eLevel, object sender, string strLog)
        {
            Log(eLevel, $"[{sender.ToString()}]    {strLog}");
        }

        
        private void Log(LOG eLevel, string LogDesc)
        {
            DateTime dTime = DateTime.Now;
            DateTime time = dTime.AddTicks(-500 / 100);    // in nanoseconds  
            string LogInfo = $"{time.ToString("yyyy/MM/dd HH:mm:ss.ffffff")}   [{eLevel.ToString()}]  [{this} {LogDesc}";
            listBoxLog.Items.Insert(0, LogInfo);
        }

        private void Log(DateTime dTime, LOG eLevel, string LogDesc)
        {
            string LogInfo = $"{dTime:yyyy-MM-dd hh:mm:ss.fff}  [{eLevel.ToString()}]   {LogDesc}";
            listBoxLog.SelectedIndex = 0;
            listBoxLog.Items.Insert(0, LogInfo);
            listBoxLog.SetSelected(listBoxLog.Items.Count, true);
        }

        private void btn_LogClear_Click(object sender, EventArgs e)
        {
            listBoxLog.Items.Clear();
        }
        */

    }
}
