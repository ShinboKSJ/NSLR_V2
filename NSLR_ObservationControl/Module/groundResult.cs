using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static NSLR_ObservationControl.Form1;

namespace NSLR_ObservationControl.Module
{
    public partial class groundResult : Form
    {
        public static string connectionString = "host=192.168.0.55;Port=5432;Database=postgres;Username=postgres;Password=1234"; // PostgreSQL 연결 문자열

        public groundResult()
        {
            InitializeComponent();
            //- groundResultList();
        }
        private void groundResultList()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM ground";
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(sql, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                listView1.Clear();
                listView1.Columns.Add("date_time"); // date_time 열 추가
                listView1.Columns.Add("rms"); // rms 열 추가

                foreach (DataRow row in dataTable.Rows)
                {
                    ListViewItem item = new ListViewItem(row.Field<DateTime>("date_time").ToString("yyyy-MM-dd HH:mm:ss")); // date_time 값을 DateTime 형식으로 변환해서 문자열로 반환
                    item.SubItems.Add(row.Field<double>("rms").ToString()); // rms 값을 double 형식으로 가져와서 문자열로 반환
                    listView1.Items.Add(item);
                }
                this.listView1.View = View.Details;
                listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                // 각 열의 셀을 가운데 정렬합니다.
                foreach (ColumnHeader column in listView1.Columns)
                {
                    column.TextAlign = HorizontalAlignment.Center;
                }

            }
        }

        /*        PointF origin = new PointF(200f, 200f);
                float angle = 45f; // 각도
                float r = 100; // 반지름
                float startOffset = 10;

                Pen linePen = new Pen(Color.Black, 1f);
                Pen ellipsePen = new Pen(Color.Blue, 1f);

                Brush degree_0_dotBrush = new SolidBrush(Color.Red);
                Brush degree_90_dotBrush = new SolidBrush(Color.Yellow);
                Brush degree_180_dotBrush = new SolidBrush(Color.DarkBlue);
                Brush degree_270_dotBrush = new SolidBrush(Color.Green);

                Brush degree_result_dotBrush = new SolidBrush(Color.BlueViolet);

                private void Form1_Paint(object sender, PaintEventArgs e)
                {
                    //외곽선 그리기
                    e.Graphics.DrawRectangle(linePen, startOffset, startOffset, origin.X * 2, origin.Y * 2);
                    //x축 그리기
                    e.Graphics.DrawLine(linePen, new PointF(startOffset, origin.Y + startOffset), new PointF(origin.X * 2 + startOffset, origin.Y + startOffset));
                    //y축 그리기
                    e.Graphics.DrawLine(linePen, new PointF(origin.X + startOffset, startOffset), new PointF(origin.X + startOffset, origin.Y * 2 + startOffset));

                    // 원그리기
                    e.Graphics.DrawEllipse(ellipsePen, startOffset + origin.X - r, startOffset + origin.Y - r, 2 * r, 2 * r);

                    // 0도
                    float x = r * (float)Math.Cos(ConvertRadian(0d));
                    float y = r * (float)Math.Sin(ConvertRadian(0d));
                    e.Graphics.FillEllipse(degree_0_dotBrush, startOffset + origin.X + x - 5f, startOffset + origin.Y + y - 5f, 10f, 10f);
                    // 90도
                    x = r * (float)Math.Cos(ConvertRadian(90d));
                    y = r * (float)Math.Sin(ConvertRadian(90d));
                    e.Graphics.FillEllipse(degree_90_dotBrush, startOffset + origin.X + x - 5f, startOffset + origin.Y + y - 5f, 10f, 10f);

                    // 180도
                    x = r * (float)Math.Cos(ConvertRadian(180d));
                    y = r * (float)Math.Sin(ConvertRadian(180d));
                    e.Graphics.FillEllipse(degree_180_dotBrush, startOffset + origin.X + x - 5f, startOffset + origin.Y + y - 5f, 10f, 10f);

                    // 270도
                    x = r * (float)Math.Cos(ConvertRadian(270d));
                    y = r * (float)Math.Sin(ConvertRadian(270d));
                    e.Graphics.FillEllipse(degree_270_dotBrush, startOffset + origin.X + x - 5f, startOffset + origin.Y + y - 5f, 10f, 10f);

                    // 실제 구하는 좌표 값
                    x = (r + 10) * (float)Math.Cos(ConvertRadian(90d + 45d));
                    y = (r + 10) * (float)Math.Sin(ConvertRadian(90d + 45d));
                    e.Graphics.FillEllipse(degree_result_dotBrush, startOffset + origin.X + x - 5f, startOffset + origin.Y + y - 5f, 10f, 10f);

                    MessageBox.Show("Form1...Paint");

                }

                private double ConvertRadian(double angle)
                {
                    return angle * Math.PI / 180d;
                }*/

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groundResult_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;

            listView1.Font = new Font("맑은 고딕", 16, FontStyle.Regular);

            listView1.Columns.Add("시간");
            listView1.Columns.Add("AVG");
            listView1.Columns.Add("RMS");

            listView1.Columns[0].TextAlign = HorizontalAlignment.Center; 
            listView1.Columns[1].TextAlign = HorizontalAlignment.Center; 
            listView1.Columns[2].TextAlign = HorizontalAlignment.Center; 
            AdjustColumnWidths();

            listView1.Resize += (s, ev) => AdjustColumnWidths();
        }
        private void AdjustColumnWidths()
        {
            if (listView1.Columns.Count == 0) return;

            int totalWidth = listView1.ClientSize.Width;

            double[] ratios = { 0.3, 0.35, 0.35 };

            for (int i = 0; i < listView1.Columns.Count && i < ratios.Length; i++)
            {
                listView1.Columns[i].Width = (int)(totalWidth * ratios[i]);
            }
        }
    }
}
