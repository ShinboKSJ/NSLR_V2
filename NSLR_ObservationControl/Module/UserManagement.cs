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

namespace NSLR_ObservationControl.Module
{
    public partial class UserManagement : UserControl
    {
        public UserManagement()
        {
            InitializeComponent();

            // 내 계정, dataGridView Select Event 제거
            this.information_gridView.DefaultCellStyle.SelectionBackColor = this.information_gridView.DefaultCellStyle.BackColor;
            this.information_gridView.DefaultCellStyle.SelectionForeColor = this.information_gridView.DefaultCellStyle.ForeColor;

            // 관리자 설정, dataGridView ColumnHeader Font 설정 
            user_gridView.ColumnHeadersDefaultCellStyle.Font = new Font("맑은 고딕", 16, FontStyle.Bold);

        }

        private void UserManagement_Load(object sender, EventArgs e)
        {

            if (MainForm.mainForm.login_information[0] == "administrator")  // 관리자 모드 일때
            {
                // 내 계정 Update
                Update_LoginInformation();

                // 관리자 설정 Update
                Update_DataGridView();
            }
            else if (MainForm.mainForm.login_information[0] == "member")    // 유저 모드 일때
            {
                // 관리자 설정 미전시
                label21.Visible = false;
                register_btn.Visible = false;
                delete_btn.Visible = false;
                modify_btn.Visible = false;
                user_gridView.Visible = false;

                // 내 계정 Update
                Update_LoginInformation();
            }
            
        }

        private void Update_LoginInformation()  // 내 계정 업데이트
        {
            information_gridView.Rows.Clear();  // 그리드뷰 초기화

            information_gridView.Rows.Add("아이디", MainForm.mainForm.login_information[3]);
            information_gridView.Rows.Add("등급", MainForm.mainForm.login_information[0]);
            information_gridView.Rows.Add("이름", MainForm.mainForm.login_information[1]);
            information_gridView.Rows.Add("소속", MainForm.mainForm.login_information[2]);
            information_gridView.Rows.Add("최종접속일", MainForm.mainForm.login_information[4]);
        }

        private void passwordChange_btn_Click(object sender, EventArgs e)   // 비밀번호 변경
        {
            UserManagement_Password userManagement_Password = new UserManagement_Password();
            userManagement_Password.FormClosed += new FormClosedEventHandler(userManagement_Password_FormClosed);
            userManagement_Password.ShowDialog();
        }

        void userManagement_Password_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 비밀번호 변경 후 Update
            if (MainForm.mainForm.login_information[0] == "administrator")  // 관리자 모드 일때
            {
                Update_LoginInformation();
                Update_DataGridView();
            }
            else if (MainForm.mainForm.login_information[0] == "member")    // 유저 모드 일때
            {
                Update_LoginInformation();
            }
            
            
        }

        private void Update_DataGridView()  // 관리자 설정 업데이트
        {
            user_gridView.Rows.Clear(); // 그리드뷰 초기화

            int user_count = 0;
            try
            {
                string connect_str = String.Format("Server=localhost;Port=55432;Database=postgres;User Id={0};password={1}", MainForm.mainForm.login_information[3], MainForm.mainForm.login_information[5]);
                NpgsqlConnection update_connect = new NpgsqlConnection(connect_str);
                update_connect.Open();

                using (NpgsqlCommand update_command = new NpgsqlCommand())
                {
                    update_command.Connection = update_connect;
                    update_command.CommandText = "SELECT * FROM NSLR_User;";

                    using (NpgsqlDataReader reader = update_command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if ((reader["user_id"].ToString() == "postgres") && (reader["user_authority"].ToString() == "administrator") && (reader["user_name"].ToString() == "postgres"))
                            {
                                // 미출력 (개발자용)
                            }
                            else
                            {
                                user_gridView.Rows.Add(reader["user_id"], reader["user_authority"], reader["user_name"], reader["user_division"], reader["user_date"]);
                                user_count++;
                            }

                        }

                        if (user_count < 5)     // dataGridView 행 채우기
                        {
                            for(int i = 0; i < 5 - user_count; i++)
                            {
                                user_gridView.Rows.Add("", "", "", "", "");
                            }
                        }

                    }
                    
                }

                update_connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void register_btn_Click(object sender, EventArgs e)     // 등록
        {
            UserManagement_Modify userManagement_modify1 = new UserManagement_Modify();
            userManagement_modify1.FormClosed += new FormClosedEventHandler(userManagement_modify1_FormClosed);
            userManagement_modify1.ShowDialog();
        }

        void userManagement_modify1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 등록 후 Update
            Update_DataGridView();
        }

        private void delete_btn_Click(object sender, EventArgs e)       // 삭제
        {
            // 선택된 사용자 정보 저장
            string[] userData = new string[5];
            for (int i = 0; i < userData.Length; i++)
            {
                userData[i] = user_gridView.SelectedRows[0].Cells[i].Value.ToString();
            }

            // login 계정 삭제 시
            if (userData[0] == MainForm.mainForm.login_information[3] && userData[1] == MainForm.mainForm.login_information[0] &&
                userData[2] == MainForm.mainForm.login_information[1] && userData[3] == MainForm.mainForm.login_information[2])
            {
                MessageBox.Show("로그인 중인 계정은 삭제할 수 없습니다.");
                return;
            }

            try
            {
                string connect_str = String.Format("Server=localhost;Port=55432;Database=postgres;User Id={0};password={1}", MainForm.mainForm.login_information[3], MainForm.mainForm.login_information[5]);
                NpgsqlConnection delete_connect = new NpgsqlConnection(connect_str);
                delete_connect.Open();

                // postgres 사용자 삭제
                using (NpgsqlCommand delete_command = new NpgsqlCommand())
                {
                    delete_command.Connection = delete_connect;
                    delete_command.CommandText = String.Format("DROP USER {0};" + "DELETE FROM NSLR_User WHERE user_authority='{1}' AND user_name='{2}' AND user_division='{3}' AND user_id='{4}';"
                        , userData[0], userData[1], userData[2], userData[3], userData[0]);

                    delete_command.ExecuteNonQuery();
                }
                delete_connect.Close();

                MessageBox.Show("사용자 삭제 완료");

                // 삭제 후 Update
                Update_DataGridView();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void modify_btn_Click(object sender, EventArgs e)       // 수정
        {
            // 선택된 사용자 정보 저장
            string[] userData = new string[5];
            for (int i = 0; i < userData.Length; i++)
            {
                userData[i] = user_gridView.SelectedRows[0].Cells[i].Value.ToString();
            }

            UserManagement_Modify userManagement_modify2 = new UserManagement_Modify(userData);
            userManagement_modify2.FormClosed += new FormClosedEventHandler(userManagement_modify2_FormClosed);
            userManagement_modify2.ShowDialog();
        }

        void userManagement_modify2_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 수정 후 Update
            Update_LoginInformation();
            Update_DataGridView();
        }


    }
}
