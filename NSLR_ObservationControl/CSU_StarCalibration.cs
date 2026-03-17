using mv.impact.acquire.helper;
using mv.impact.acquire;
using NSLR_ObservationControl.Module;
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
using OpenCvSharp;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Markup;
using static NSLR_ObservationControl.Module.StarCalibration_Setting;
using System.Web.UI.WebControls;
using Emgu.CV.Structure;
using Emgu.CV.OCR;
using static NSLR_ObservationControl.UserControlManager;
namespace NSLR_ObservationControl
{
    public partial class CSU_StarCalibration : UserControl, MainForm.ICollectData, MainForm.ICollectData2, UserControlManager.IKeyControl
    {
        ChartView StarMap = new ChartView();
        public static tcspk _tcspk;
        public static StarCalInfo.Step starStep { get;  set; }
        public CSU_StarCalibration()
        {
            InitializeComponent();
            chartInit();
            labelInit();
          //  InitializeExternalLibrary();
        }
/*        private void InitializeExternalLibrary()
        {
            _tcspk = .LoadObject_Star();
            observation_TMS1.ExternalLibraryObject = _tcspk;
            _tcspk.StarExcute();
        }*/
        public void reloadLibrary()
        {
            if (_tcspk == null)
            {
                observation_TMS1.ExternalLibraryObject = _tcspk;
                _tcspk.StarExcute();
            }
        }

        /*        public double[] StarCalStart(double tai)
                {
                    double[] TargetPosition = new double[2];
                    TargetPosition = tcspk.ObtainTargetPointed(tai);
                    return TargetPosition;
                }
                public double[][] TrackingData(double tai)
                {
                    double[][] TargetPosition = new double[2][];
                    for (int i = 0; i < TargetPosition.Length; i++)
                    {
                        TargetPosition[i] = new double[8];
                    }
                    TargetPosition =  tcspk.ObtainTargetTracked(tai);
                    return TargetPosition;
                }*/
        private void chartInit()
        {
            chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            chart1.Series["Series1"].MarkerSize = 3;
            chart1.Series["Series1"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            chart1.Series["Series1"].Color = Color.Black;
            chart1.Series["Series1"].LabelBackColor = Color.Green;
            chart1.Series["Series1"]["PolarDrawingStyle"] = "Marker";
           // chart1.Series["Series1"]["PolarDrawingStyle"] = "line";
            chart1.Series["Series1"].Points.AddXY(0, 0);

            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 360;
            chart1.ChartAreas[0].AxisY.Minimum = -90;
            chart1.ChartAreas[0].AxisY.Maximum = 90;
            chart1.ChartAreas[0].AxisX.Interval = 30;
            chart1.ChartAreas[0].AxisY.Interval = 30;


        }
        private void labelInit()
        {
            infoLabel.Font = new System.Drawing.Font("굴림", 14.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            infoLabel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
        }
        public OPSUserData CollectDataFromOpticalSystem()
        {
            return opticalSystem1.CollectData();
        }
        public TMSUserData CollectDataFromTrackingMountSystem()
        {
            return observation_TMS1.CollectData();
        }
        private List<DataPoint> dataPoints = new List<DataPoint>();

     
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            if (starCalibration_Setting1.dataList != null)
            {
                double originX = -180;
                double originY = 0;

                ChartArea chartArea = chart1.ChartAreas[0];
                double mouseX = CalculateTheta(chartArea.AxisX.PixelPositionToValue(e.X) + originX, 2 * (chartArea.AxisY.PixelPositionToValue(e.Y) + originY));
                var x = chartArea.AxisX.PixelPositionToValue(e.X) + originX;
                var y = 2 * chartArea.AxisY.PixelPositionToValue(e.Y);
                var r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                double mouseY = -90 + r;

                int roundedX = (int)Math.Round(mouseX);
                int roundedY = (int)Math.Round(mouseY);
                double radius = 1;
                StarCatalogItem closestData = FindClosestData(mouseX, mouseY, radius);

                if (closestData != null)
                {
                    StarCatalogItem selectedData = closestData;
                    SelectRowInDataGridView(selectedData);
                }
                else
                {
                }
                
            }
        }

        private void SelectRowInDataGridView(StarCatalogItem selectedData)
        {
            if (selectedData != null)
            {
                int selectedIndex = starCalibration_Setting1.dataList.IndexOf(selectedData);

                if (selectedIndex >= 0)
                {
                    starCalibration_Setting1.dataGridView_starCalibration.Rows[selectedIndex].Selected = true;
                    starCalibration_Setting1.dataGridView_starCalibration.CurrentCell = starCalibration_Setting1.dataGridView_starCalibration.Rows[selectedIndex].Cells[0]; 
                    UpdateLabel(starCalibration_Setting1.StarCatalogID, "StarCatalogID - ", selectedData.StarCatalogId.ToString());
                    UpdateLabel(starCalibration_Setting1.Vmag, selectedData.MagnitudeVariable.ToString());
                    UpdateLabel(starCalibration_Setting1.RaDeg, selectedData.RaDeg.ToString());
                    UpdateLabel(starCalibration_Setting1.DecDeg, selectedData.DecDeg.ToString());
                    UpdateLabel(starCalibration_Setting1.Plx, selectedData.Plx.ToString());
                    UpdateLabel(starCalibration_Setting1.pmRA, selectedData.pmRA.ToString());
                    UpdateLabel(starCalibration_Setting1.pmDE, selectedData.pmDE.ToString());
                    UpdateLabel(starCalibration_Setting1.e_pmRA, selectedData.e_pmRA.ToString());
                    UpdateLabel(starCalibration_Setting1.e_pmDE, selectedData.e_pmDE.ToString());
                    UpdateLabel(starCalibration_Setting1.ra_deg_err, selectedData.ra_deg_err.ToString());
                    UpdateLabel(starCalibration_Setting1.dec_deg_err, selectedData.dec_deg_err.ToString());
                    starCalibration_Setting1.HighlightChartPoint(selectedData);
                }
            }
            else
            {
                Console.WriteLine("선택된 범위 안에 별이 없습니다.");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //tcspk.demo();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //tcspk.StarCalibrationInit();
            /*  TrackStar(Ra,Dec,);*/
            _tcspk.StarExcute();
        }
        public void TrackStar(double ra, double dec, double lat, double lon, double alt)
        {
            //StarCalibrationInit();
            /*tcspk.calculatAzEl();*/
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
            if (starCalibration_Setting1.dataList != null)
            {
                double originX = -180;
                double originY = 0;

                ChartArea chartArea = chart1.ChartAreas[0];
                double mouseX = CalculateTheta(chartArea.AxisX.PixelPositionToValue(e.X) + originX, 2*(chartArea.AxisY.PixelPositionToValue(e.Y) + originY));
                var x = chartArea.AxisX.PixelPositionToValue(e.X) + originX;
                var y = 2*chartArea.AxisY.PixelPositionToValue(e.Y) ;
                var r = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
                double mouseY = -90 + r;
                double radius = 1;

                StarCatalogItem closestData = FindClosestData(mouseX, mouseY, radius);

                if (closestData != null)
                {
                    ClearLabel(infoLabel);
                    UpdateLabel(infoLabel, closestData.RaDeg.ToString(), closestData.DecDeg.ToString(), closestData.StarCatalogId.ToString());
                    infoLabel.Location = new System.Drawing.Point(e.X + 10, e.Y - 30);

                }
                else
                {
                    ClearLabel(infoLabel);
                }
            }
            }
            catch (Exception ex) { MessageBox.Show("Range Over"); }

        }
        private void ClearLabel(System.Windows.Forms.Label label)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() => label.Text = ""));
            }
            else
            {
                label.Text = "";
                label.Update();
            }
        }
        private double CalculateTheta(double y, double x)
        {
            double theta = (Math.Atan2(y, x) * (180 / Math.PI) + 360) % 360;
            return theta;
        }
        private void UpdateLabel(System.Windows.Forms.Label label, string newText1, string newText2,string newText3)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() => label.Text = $"RA: {newText1}, Dec: {newText2}, StarCatalogID: {newText3}"));
            }
            else
            {
                label.Text = $"RA: {newText1}, Dec: {newText2}, StarCatalogID: {newText3}";
                label.Update();
            }
        }
        private void UpdateLabel(System.Windows.Forms.Label label, string newText1, string newText2)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() => label.Text = $"{newText1}{newText2}"));
            }
            else
            {
                label.Text = $"{newText1}{newText2}";
                label.Update();
            }
        }
        private void UpdateLabel(System.Windows.Forms.Label label, string newText1)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() => label.Text = newText1 ));
            }
            else
            {
                label.Text = newText1;
                label.Update();
            }
        }

        private StarCatalogItem FindClosestData(double mouseX, double mouseY, double radius)
        {
            double minDistance = double.MaxValue;
            StarCatalogItem closestData = null;

            foreach (StarCatalogItem data in starCalibration_Setting1.dataList)
            {
                double distance = Math.Sqrt(Math.Pow(data.RaDeg - mouseX, 2) + Math.Pow(data.DecDeg - mouseY, 2));
                if (distance < radius && distance < minDistance)
                {
                    minDistance = distance;
                    closestData = data;
                }
            }
            return closestData;
        }
        public static double AddAzOffSet;
        public static double AddElOffSet;
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

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}