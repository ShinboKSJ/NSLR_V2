using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl
{
    public partial class Checking_OvservingData : Form
    {
        DateTime standard_time = new DateTime();
        double standard_interval;
        double[][] radec_data = new double[2][];

        List<Check_Data> check_Datas = new List<Check_Data>();

        public Checking_OvservingData(DateTime startTime, double interval, double[][] radecData)
        {
            InitializeComponent();

            standard_time = startTime;
            standard_interval = interval;

            for (int i = 0; i < radec_data.Length; i++)
            {
                radec_data[i] = new double[radecData[i].Length];
                Array.Copy(radecData[i], radec_data[i], radec_data[i].Length);
            }
            
        }

        private void Checking_OvservingData_Load(object sender, EventArgs e)
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor =
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;

            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 14,
                FontStyle.Bold);
            dataGridView1.DefaultCellStyle.Font = new Font("맑은 고딕", 12, FontStyle.Regular);
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            for (int i = 0; i < radec_data[0].Length; i++)
            {
                Check_Data check_Data = new Check_Data((standard_time.AddSeconds(i * standard_interval)).ToString("yyyy-MM-dd HH:mm:ss.ff"));
                check_Data.Ra = radec_data[0][i];
                check_Data.Dec = radec_data[1][i];
                check_Datas.Add(check_Data);
            }

            dataGridView1.DataSource = check_Datas;
            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ClearSelection();
        }



    }

    public class Check_Data
    {
        public string Local_Time { get; set; }
        public double Ra { get; set; }
        public double Dec { get; set; }

        public Check_Data(string time)
        {
            Local_Time = time;
        }
    }
}
