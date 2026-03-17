using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl.Module
{
    public partial class circular : UserControl
    {
        public circular()
        {
            InitializeComponent();
        }


        PointF origin = new PointF(200f, 200f);
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

        private double ConvertRadian(double angle)
        {
            return angle * Math.PI / 180d;
        }

        private void circular_Paint(object sender, PaintEventArgs e)
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
    }
}
