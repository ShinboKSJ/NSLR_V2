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

namespace NSLR_ObservationControl
{
    public partial class UserManagement_Password : Form
    {
        public UserManagement_Password()
        {
            InitializeComponent();
        }

        private void UserManagement_Password_Load(object sender, EventArgs e)
        {

        }

        private void execution_btn_Click(object sender, EventArgs e)    // 확인
        {
            if (string.IsNullOrEmpty(newPassword_txtBox.Text))
            {
                MessageBox.Show("새 비밀번호를 입력하세요.");
                return;
            }
            else if (string.IsNullOrEmpty(checkPassword_txtBox.Text))
            {
                MessageBox.Show("비밀번호 확인을 입력하세요.");
                return;
            }
            else if (newPassword_txtBox.Text != checkPassword_txtBox.Text)
            {
                MessageBox.Show("비밀번호가 일치하지 않습니다.");
                return;
            }

            try
            {
                string connect_str = String.Format("Server=localhost;Port=5432;Database=postgres;User Id={0};password={1}", MainForm.mainForm.login_information[3], MainForm.mainForm.login_information[5]);
                NpgsqlConnection change_connect = new NpgsqlConnection(connect_str);
                change_connect.Open();

                // postgres 현재 사용자 비밀번호 변경
                using (NpgsqlCommand change_command = new NpgsqlCommand())
                {
                    change_command.Connection = change_connect;
                    change_command.CommandText = String.Format("ALTER USER {0} WITH PASSWORD '{1}';"
                        , MainForm.mainForm.login_information[3], newPassword_txtBox.Text);

                    change_command.ExecuteNonQuery();
                }
                change_connect.Close();

                MainForm.mainForm.login_information[5] = newPassword_txtBox.Text;   // login 정보 변경

                MessageBox.Show("비밀번호 변경 완료");
                this.Close();   // form 종료


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void close_btn_Click(object sender, EventArgs e)    // 닫기
        {
            this.Close();
        }

        
    }
}
