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
    public partial class RecordManagement_AccessHistory : UserControl
    {
        
        struct user
        {
            public string name;
            public int grade;
        }

        
        public RecordManagement_AccessHistory()
        {
            InitializeComponent();
            init_dataGridView();
        }


        void init_dataGridView()
        {

            dataGridView_AccessHistory.Font = new Font("Tahoma", 10, FontStyle.Regular);
            dataGridView_AccessHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 14, FontStyle.Bold);  //헤더 크기

            dataGridView_AccessHistory.AutoResizeColumns();
            // Configure the details DataGridView so that its columns automatically
            // adjust their widths when the data changes.
            dataGridView_AccessHistory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dataGridView_AccessHistory.Columns.Add("fTime", "접속시각");
            dataGridView_AccessHistory.Columns.Add("fUser", "사용자");
            dataGridView_AccessHistory.Columns.Add("fGrade", "등급");
            dataGridView_AccessHistory.Columns.Add("f1", "...");
            dataGridView_AccessHistory.Columns.Add("f2", "...");
            dataGridView_AccessHistory.Columns.Add("f3", "...");
            dataGridView_AccessHistory.Columns.Add("f4", "...");
           // dataGridView_AccessHistory.Sort(new RowComparer(SortOrder.Ascending)); 

            user user1;
            user user2;
            user user3;

            user1.name = "User1";
            user1.grade = 1;
            user2.name = "User2";
            user2.grade = 2;
            user3.name = "User3";
            user3.grade = 3;

            user[] randUser = { user1, user2, user3 };
            

            Random rnd  = new Random();
            var rand = rnd.Next(1, 10);
            DateTime randTime = DateTime.Now.AddDays(-14);

            for(int a= 0; a< 50; a++)
            {
                var newTime = randTime.AddDays(rnd.Next(10)).AddHours(rnd.Next(24)).AddMinutes(rnd.Next(59)).AddSeconds(rnd.Next(59));
                var who = rnd.Next(3);
                dataGridView_AccessHistory.Rows.Add(newTime.ToString(), randUser[who].name, randUser[who].grade);

            }


        }

    }
}
