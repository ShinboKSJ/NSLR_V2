//#define NO_LOGIN_MODE
#define LOGIN_MODE


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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace NSLR_ObservationControl
{
    public partial class Login : Form
    {
        string dataPC_postgresql_connectStr = String.Format("Server={0};Port={1};Database=postgres;User Id={2};password={3}", "192.168.10.13", "5432", "postgres", "1234");
        //private string connectionString = "Host=localhost;Port=55432;Database=postgres;Username=postgres;Password=1234"; // PostgreSQL 연결 문자열

        public Login()
        {
            InitializeComponent();
            button1.Select();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)  // 계정생성?????
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("아이디와 비밀번호를 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (NpgsqlConnection connection = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO users (username, password) VALUES (@username, @password)";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("계정 생성이 완료되었습니다.", "계정 생성 성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("계정 생성에 실패하였습니다.", "계정 생성 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("계정 생성 중 오류가 발생하였습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        
        private void LOGIN_Click(object sender, EventArgs e)
        {
            string login_state = "disconnected";
            string[] login_information = new string[6]; // 등급, 이름, 소속, 아이디, 최종접속일, 비밀번호

            try
            {
                
                // 데이터베이스 접속
                string connect_str = String.Format("Server={0};Port={1};Database=postgres;User Id={2};password={3}", "192.168.10.13", "5432", txtUsername.Text, txtPassword.Text);
                NpgsqlConnection connect = new NpgsqlConnection(connect_str);
                connect.Open();

                if (connect.State == ConnectionState.Open)
                {
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        // User Table 존재여부 Check
                        command.CommandText = "SELECT * FROM pg_catalog.pg_tables WHERE schemaname = 'public';";
                        bool exist_table = false;
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (reader["tablename"].ToString() == "NSLR_User")
                                {
                                    exist_table = true;
                                    break;
                                }
                            }
                        }

                        // User Table 없을 시 생성
                        if (exist_table == false)
                        {
                            command.CommandText = "CREATE TABLE NSLR_User (                   " +
                                                  "         user_authority       varchar(50), " +
                                                  "         user_name            varchar(50), " +
                                                  "         user_division        varchar(50), " +
                                                  "         user_id              varchar(50), " +
                                                  "         user_date            varchar(50)  " +
                                                  ");                                         ";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText = "SELECT * FROM NSLR_User;";

                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (txtUsername.Text == reader["user_id"].ToString() && reader["user_authority"].ToString() == "administrator")
                                {
                                    // 관리자 모드일 때
                                    login_state = "connected";

                                    login_information[0] = reader["user_authority"].ToString();
                                    login_information[1] = reader["user_name"].ToString();
                                    login_information[2] = reader["user_division"].ToString();
                                    login_information[3] = reader["user_id"].ToString();
                                    login_information[5] = txtPassword.Text;
                                    break;
                                }
                                else if (txtUsername.Text == reader["user_id"].ToString() && reader["user_authority"].ToString() == "member")
                                {
                                    // 유저 모드일 때
                                    login_state = "connected";

                                    login_information[0] = reader["user_authority"].ToString();
                                    login_information[1] = reader["user_name"].ToString();
                                    login_information[2] = reader["user_division"].ToString();
                                    login_information[3] = reader["user_id"].ToString();
                                    login_information[5] = txtPassword.Text;
                                    break;
                                }
                            }
                        }

                        // 최종접속일 변경
                        if (login_state == "connected")
                        {
                            string tempDate = DateTime.Now.ToString("yyyy-MM-dd");
                            
                            command.CommandText = String.Format("UPDATE NSLR_User SET user_date='{0}' WHERE user_authority='{1}' AND user_name='{2}' AND user_division='{3}' AND user_id='{4}';"
                                , tempDate, login_information[0], login_information[1], login_information[2], login_information[3]);

                            command.ExecuteNonQuery();

                            login_information[4] = tempDate;
                        }
                        

                    }
                    
                    
                }
                connect.Close();

            }
            catch/*(Exception ex)*/
            {
                //MessageBox.Show("ID/PW 잘못되었습니다.");
            }

            if (login_state == "connected")
            {
                // MainForm 이동
                this.Visible = false;
                MainForm mainform = new MainForm(login_information);
                mainform.ShowDialog();
            }
            else   // 개발용(No Login) > 추후 삭제
            {
                // MainForm 이동
                this.Visible = false;
                MainForm mainform = new MainForm();
                mainform.ShowDialog();
            }

            /*
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("아이디와 비밀번호를 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("로그인에 성공하였습니다.", "로그인 성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            MainForm mainform = new MainForm();
                            mainform.Show();
                            connection.Close();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("아이디 또는 비밀번호가 일치하지 않습니다.", "로그인 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("로그인 중 오류가 발생하였습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                }
                

            }
            */


        }




    }
}
