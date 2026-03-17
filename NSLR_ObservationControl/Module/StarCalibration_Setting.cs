using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Markup;
using static NSLR_ObservationControl.Module.StarCalibration_Control;
using static NSLR_ObservationControl.StarDataTable;
using System.Web.UI.WebControls;
using SwissEphNet;
using System.IO;
using System.Timers;
using System.Threading;


namespace NSLR_ObservationControl.Module
{
    public partial class StarCalibration_Setting : System.Windows.Forms.UserControl, IClearable
    {
        //private static string connectionString = "host=\"192.168.10.13\";Port=5432;Database=postgres;Username=postgres;Password=1234"; // 데이터관리 PC connection
        private static string connectionString = "host=localhost;Port=5432;Database=postgres;Username=postgres;Password=1234"; // PostgreSQL 연결 문자열

        DataTable table_starCalibration = new DataTable();
        DataTable table_starCalibration_Result = new DataTable();
        
        public List<StarCatalogItem> dataList;

        public static double elevationMin;

        private tcspk _tcspk;
        static SwissEph swe = new SwissEph();
        public StarCalibration_Setting()
        {
            InitializeComponent();
            StarCatalogListInit();
            richTextBoxInit();
            init();
            _tcspk = new tcspk();

            //행성
            string ephemerisPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ephe");
            swe.swe_set_ephe_path(ephemerisPath);
        }
        private void richTextBoxInit()
        {
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox2.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox3.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox4.SelectionAlignment = HorizontalAlignment.Center;
        }
        private void StarCatalogListInit()
        {
            table_starCalibration.Columns.Add("번호", typeof(int));
            table_starCalibration.Columns.Add("등급", typeof(double));
            table_starCalibration.Columns.Add("고도", typeof(double));
            table_starCalibration.Columns.Add("방위", typeof(double));
        }
        private void init()
        {
            elevationMin = 20;
        }
        public class StarCatalogItem
        {
            public int StarCatalogId { get; set; }
            public double MagnitudeVariable { get; set; }
            public double RaDeg { get; set; }
            public double DecDeg { get; set; }
            public double Plx { get; set; }
            public double pmRA { get; set; }
            public double pmDE { get; set; }
            public double e_pmRA { get; set; }
            public double e_pmDE { get; set; }
            public double ra_deg_err { get; set; }
            public double dec_deg_err { get; set; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var parentControl = this.Parent.Parent;
            var chartControl = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
            var chart = (Chart)chartControl;
            chart.Series["Series1"].Points.Clear();
            System.Windows.Forms.Label countCheck = parentControl.Controls.Find("label1", true).FirstOrDefault() as System.Windows.Forms.Label;

            int count = 0;
            if (double.TryParse(richTextBox3.Text, out double magnitudeThreshold))
            {
                string ControlSensitive = richTextBox4.Text;
                if (!string.IsNullOrEmpty(ControlSensitive) &&
                    double.TryParse(ControlSensitive, out double elevationMin) &&
                    elevationMin >= 20 && elevationMin <= 90)
                {
                    MessageBox.Show("조건에 부합하는 값입니다.");
                }
                else
                {
                    MessageBox.Show("범위를 벗어나 20.0 Deg 로 셋팅합니다.");
                    elevationMin = 20;
                    richTextBox4.Text = elevationMin.ToString();
                }

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = $"SELECT star_catalog_id, magnitude_variable, ra_deg, dec_deg, parallax, ra_proper_motion, dec_proper_motion,varflag FROM star_catalog_j2000 WHERE magnitude_variable < {magnitudeThreshold}";

                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataList = new List<StarCatalogItem>();

                    foreach (DataRow row in dataTable.Rows)
                    {
                        StarCatalogItem item = new StarCatalogItem
                        {
                            StarCatalogId = Convert.ToInt32(row["star_catalog_id"]),
                            MagnitudeVariable = Convert.ToDouble(row["magnitude_variable"]),
                            RaDeg = Convert.ToDouble(row["ra_deg"]),
                            DecDeg = Convert.ToDouble(row["dec_deg"]),
                            Plx = Convert.ToDouble(row["parallax"]), 
                            pmRA = Convert.ToDouble(row["ra_proper_motion"]), 
                            pmDE = Convert.ToDouble(row["dec_proper_motion"]), 
                            /*                            e_pmRA = Convert.ToDouble(row["ra_proper_motion_err"]), 
                            e_pmDE = Convert.ToDouble(row["dec_proper_motion_err"]),
                            ra_deg_err = Convert.ToDouble(row["ra_deg_err"]),
                                                        dec_deg_err = Convert.ToDouble(row["dec_deg_err"])*/

                        };
      /*                  var ElresultChk = _tcspk.ChkElRange(item.DecDeg, elevationMin);
                        if (ElresultChk == true) dataList.Add(item); */        // 필터 o
                        dataList.Add(item); // 필터 x
                    }
                    dataGridView_starCalibration.DataSource = dataList.Select(x => new { x.StarCatalogId, x.MagnitudeVariable, x.RaDeg, x.DecDeg }).ToList();
                }

                foreach (var data in dataList)
                {
                   
                    if (data.MagnitudeVariable < 1)
                    {
                        chart.Series["Series1"].Points.AddXY(data.RaDeg, data.DecDeg);
                        chart.Series["Series1"].Points.Last().MarkerSize = 10;
                    }
                    else if (data.MagnitudeVariable < 2)
                    {
                        chart.Series["Series1"].Points.AddXY(data.RaDeg, data.DecDeg);
                        chart.Series["Series1"].Points.Last().MarkerSize = 8;
                    }
                    else if (data.MagnitudeVariable < 3)
                    {
                        chart.Series["Series1"].Points.AddXY(data.RaDeg, data.DecDeg);
                        chart.Series["Series1"].Points.Last().MarkerSize = 6;
                    }
                    else if (data.MagnitudeVariable < 4)
                    {
                        chart.Series["Series1"].Points.AddXY(data.RaDeg, data.DecDeg);
                        chart.Series["Series1"].Points.Last().MarkerSize = 4;
                    }
                    else
                    {
                        chart.Series["Series1"].Points.AddXY(data.RaDeg, data.DecDeg);
                        chart.Series["Series1"].Points.Last().MarkerSize = 2;
                    }
                    count++;

                }
                UpdateLabel(countCheck,$"생성된 데이터 수 : {count} 개");
            }
            else
            {
                MessageBox.Show("별 등급 입력 확인");
            }
        }
        private void UpdateLabel(System.Windows.Forms.Label label, string newText)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() => label.Text = newText));
            }
            else
            {
                label.Text = newText;
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
        private void dataGridView_starCalibration_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataList.Count) 
            {
                var selectedData = dataList[e.RowIndex];
                Ra = selectedData.RaDeg;
                Deg = selectedData.DecDeg;
                HighlightChartPoint(selectedData);
                UpdateLabel(StarCatalogID, "StarCatalogId - ", selectedData.StarCatalogId.ToString());
                UpdateLabel(Vmag, selectedData.MagnitudeVariable.ToString());
                UpdateLabel(RaDeg, selectedData.RaDeg.ToString());
                UpdateLabel(DecDeg, selectedData.DecDeg.ToString());
                UpdateLabel(Plx, selectedData.Plx.ToString());
                UpdateLabel(pmRA, selectedData.pmRA.ToString());
                UpdateLabel(pmDE, selectedData.pmDE.ToString());
                UpdateLabel(e_pmRA, selectedData.e_pmRA.ToString());
                UpdateLabel(e_pmDE, selectedData.e_pmDE.ToString());
                UpdateLabel(ra_deg_err, selectedData.ra_deg_err.ToString());
                UpdateLabel(dec_deg_err, selectedData.dec_deg_err.ToString());
                StarCalibration_Control.hipnumber_temp = selectedData.StarCatalogId;
            }
        }
        
        public static double Ra, Deg;
        public static double height = 0.85;
        public void HighlightChartPoint(StarCatalogItem selectedData)
        {
            var parentControl = this.Parent.Parent;
            var chartControl = parentControl.Controls.Find("chart1", true).FirstOrDefault() as Chart;
            var chart = (Chart)chartControl;
            chart.Series["Series1"].Points.Clear();
            foreach (var data in dataList)
            {
                DataPoint point = new DataPoint(data.RaDeg, data.DecDeg);
                if (data.MagnitudeVariable < 1)
                {
                    chart.Series["Series1"].Points.AddXY(data.RaDeg, data.DecDeg);
                    chart.Series["Series1"].Points.Last().MarkerSize = 10;
                }
                else if (data.MagnitudeVariable < 2)
                {
                    chart.Series["Series1"].Points.AddXY(data.RaDeg, data.DecDeg);
                    chart.Series["Series1"].Points.Last().MarkerSize = 8;
                }
                else if (data.MagnitudeVariable < 3)
                {
                    chart.Series["Series1"].Points.AddXY(data.RaDeg, data.DecDeg);
                    chart.Series["Series1"].Points.Last().MarkerSize = 6;
                }
                else if (data.MagnitudeVariable < 4)
                {
                    chart.Series["Series1"].Points.AddXY(data.RaDeg, data.DecDeg);
                    chart.Series["Series1"].Points.Last().MarkerSize = 4;
                }
                else
                {
                    chart.Series["Series1"].Points.AddXY(data.RaDeg, data.DecDeg);
                    chart.Series["Series1"].Points.Last().MarkerSize = 2;
                }
                if (data == selectedData)
                {
                    Ra = data.RaDeg;
                    Deg = data.DecDeg;
                    point.Color = Color.Yellow;
                    point.MarkerStyle = MarkerStyle.Circle;  
                    point.MarkerSize = 14;  

                    chart.Series["Series1"].Points.Add(point);

                    DataPoint highlightCircle = new DataPoint(data.RaDeg, data.DecDeg);
                    highlightCircle.MarkerStyle = MarkerStyle.Circle;
                    highlightCircle.MarkerSize = 20; 
                    highlightCircle.MarkerBorderColor = Color.Red;  
                    highlightCircle.MarkerBorderWidth = 3; 
                    highlightCircle.Color = Color.Transparent;  
                    chart.Series["Series1"].Points.Add(highlightCircle);
            }
        }
        }

        private void SaveStarData_Click(object sender, EventArgs e)
        {
            dataTable.SaveToFile();
        }
        public void ClearData()
        {
            dataTable._entries.Clear();
        }
        private void DeleteStarData_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(Color.Beige), e.Bounds);
            Rectangle paddedBounds = e.Bounds;
            int yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
            paddedBounds.Offset(1, yOffset);
        }

        public static bool planetModeFlag = false;
        public static int planetFlag;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                string selectedPlanet = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

                double jd = swe.swe_julday(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day,
                                 DateTime.UtcNow.Hour + DateTime.UtcNow.Minute / 60.0, SwissEph.SE_GREG_CAL);
                double[] xx = new double[6];
                string serr = "";
                int iflag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_EQUATORIAL | SwissEph.SEFLG_J2000;
                planetFlag = GetPlanetFlag(selectedPlanet);
                if (planetFlag == -9999) return;
                int ret = swe.swe_calc(jd, planetFlag, iflag, xx, ref serr);

                Ra = xx[0];
                Deg = xx[1];


                dataGridView1.Rows[e.RowIndex].Cells[1].Value = $"{Ra}";
                dataGridView1.Rows[e.RowIndex].Cells[2].Value = $"{Deg}";
            }
        }

        private void InitializePlanetList()
        {
            if (dataGridView1.Columns.Count == 0) 
            {
                dataGridView1.Columns.Add("Planet", "행성");
                dataGridView1.Columns.Add("RA", "RA");
                dataGridView1.Columns.Add("Dec", "Dec");
            }

            var planets = new[] { "수성 (MERCURY)", "금성 (VENUS)", "지구", "화성 (MARS)", "목성 (JUPITER)", "토성 (SATURN)", "천왕성 (URANUS)", "해왕성 (NEPTUNE)" };

            foreach (var planet in planets)
            {
                dataGridView1.Rows.Add(planet, "", "");
            }
            dataGridView1.CellClick += dataGridView1_CellClick;
        }
        private void ClearPlanetList()
        {
            dataGridView1.Rows.Clear();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            planetModeFlag = !planetModeFlag;

            if (planetModeFlag)
            {
                button5.Text = "행성 모드 ON"; 
                button5.BackColor = Color.Lime; 
                InitializePlanetList();
                CalculationStart();
            }
            else
            {
                button5.Text = "행성 모드 OFF"; 
                button5.BackColor = Color.LightGray; 
                ClearPlanetList();
                CalculationStop();
            }
        }

        private int GetPlanetFlag(string planetName)
        {
            switch (planetName)
            {
                // 수성
                case "수성 (MERCURY)":
                    return SwissEph.SE_MERCURY;

                // 금성
                case "금성 (VENUS)":
                    return SwissEph.SE_VENUS;

                // 지구
                case "지구":
                    return SwissEph.SE_EARTH;

                // 화성
                case "화성 (MARS)":
                    return SwissEph.SE_MARS;

                // 목성
                case "목성 (JUPITER)":
                    return SwissEph.SE_JUPITER;

                // 토성
                case "토성 (SATURN)":
                    return SwissEph.SE_SATURN;

                // 천왕성
                case "천왕성 (URANUS)":
                    return SwissEph.SE_URANUS;

                // 해왕성
                case "해왕성 (NEPTUNE)":
                    return SwissEph.SE_NEPTUNE;

                default: return -9999;
                 //   return SwissEph.SE_JUPITER;
            }
        }
        static System.Timers.Timer timer;
        static int count = 0;
        private void CalculationStart()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += GetPlanetRADEC;
            timer.Start();

            Console.WriteLine("RA, Dec 계산 시작...");
        }
        private void CalculationStop()
        {
            timer.Stop();
            Console.WriteLine("RA, Dec 계산 중지...");
        }
        static void GetPlanetRADEC(object sender, ElapsedEventArgs e)
        {
            double jd = swe.swe_julday(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day,
                              DateTime.UtcNow.Hour + DateTime.UtcNow.Minute / 60.0, SwissEph.SE_GREG_CAL);
            double[] xx = new double[6];
            string serr = "";

            int iflag = SwissEph.SEFLG_SWIEPH | SwissEph.SEFLG_EQUATORIAL | SwissEph.SEFLG_J2000;
            if (planetFlag == -9999 || !planetModeFlag) { return; }
            int ret = swe.swe_calc(jd, planetFlag, iflag, xx, ref serr);

            Ra = xx[0]; 
            Deg = xx[1];
            count++;
            Console.WriteLine("$Ra ={Ra} dec={Deg}");
        }
 
    }
}
