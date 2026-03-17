using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace NSLR_ObservationControl.Module
{
    public partial class ChartView : UserControl
    {
        
        ChartData chart_Data = new ChartData(); //Data for Chart

        DataTable dtMain = new DataTable();

        public ChartView()
        {
            InitializeComponent();
            
            DataGridSet();
        }

        // ChartData를 구조체 처럼 가지고 있을 Class
        public class ChartData
        {
            DataTable _ChartMain;
            //SeriesChartType _ChartType = SeriesChartType.Polar; //극좌표계를 기본으로 
            SeriesChartType _ChartType = SeriesChartType.Radar; //극좌표계를 기본으로 
            public DataTable ChartMain { get => _ChartMain; set => _ChartMain = value; }
            public SeriesChartType ChartType { get => _ChartType; set => _ChartType = value; }
        }

        #region Function
        /// <summary>
        /// Chart에 Data를 Load 합니다. 
        /// </summary>
        /// <param name="cData"></param>
        private void ChartDataLoad(ChartData cData)
        {
            mapChart.Series.Clear();  // 기존 시리즈 내용 삭제

            //for (int idx = 0; idx < 20; idx++)
            //   mapChart.Series.Points.AddXY(idx, idx * 10);


            //mapChart.Titles.Add("별지도");  // Screen 생성 시 Chart의 Title 생성
            //mapChart.BackColor = Color.Black;
            //mapChart.ForeColor = Color.White; 

            DataTable dt = chart_Data.ChartMain;
            
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow oRow in dt.Rows)
                {
                    Series series = mapChart.Series.Add(oRow["STAR_NAME"].ToString());
                    series.ChartType = cData.ChartType;  // 차트 종류
                    //series.Points.AddXY(30,30 );
                    //series.Points.AddXY(30, 60);
                    //series.Points.AddXY(30, 90);
                    //series.Points.AddXY(30, 120);

                    /*
                    series.Points.AddXY(0.00555556, 50.7389);
                    series.Points.AddXY(0.00666667 , 32.7833 );
                    series.Points.AddXY(0.00800000 , 32.7823 );
                    series.Points.AddXY(0.01680560 , 6.2419  );
                    series.Points.AddXY(0.01790000 , 4.5000  );
                    series.Points.AddXY(0.01799000 , 4.5406  );
                    series.Points.AddXY(0.02186110 , 7.7478);
                    series.Points.AddXY(0.02222220 , 13.1108);
                    series.Points.AddXY(0.02408330 , 31.4328);
                    series.Points.AddXY(0.02216000 , 34.5257);
                    series.Points.AddXY(0.02500000 , 31.4411);
                    series.Points.AddXY(0.02541000 , 11.3461);
                    series.Points.AddXY(0.03483000 , 16.5903);
                    series.Points.AddXY(0.03597220 , 2.9406);
                    series.Points.AddXY(0.03863890 , 12.9708);
                    series.Points.AddXY(0.03935000 , 4.0897);
                    series.Points.AddXY(0.04069440 , 3.3519);*/
                }
            }
        }
        #endregion




        #region Function
        /// <summary>
        /// Chart Data 생성
        /// </summary>
        private void DataGridSet()
        {
            dtMain.Clear();
            dgView.DataSource = null;

            dtMain = new DataTable();
            DataColumn col_star = new DataColumn("STAR_NAME", typeof(string));
            DataColumn col_Ra = new DataColumn("RA", typeof(double));
            DataColumn col_Dec = new DataColumn("DEC",typeof(double));
            dtMain.Columns.Add(col_star);
            dtMain.Columns.Add(col_Ra);
            dtMain.Columns.Add(col_Dec);            
            //Random rd = new Random();
            dtMain.Rows.Add("BA102", 0.00555556, 50.7389);
            dtMain.Rows.Add("AB001", 0.00666667, 32.7833);

            dgView.DataSource = dtMain;
        }
        #endregion

        private void mapChart_Click(object sender, EventArgs e)
        {
            ChartDataLoad(chart_Data);
        }

    }

}
