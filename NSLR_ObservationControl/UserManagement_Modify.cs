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
    public partial class UserManagement_Modify : Form
    {
        string current_version;
        string[] modify_information = new string[5];
        bool check_id = false;

        public UserManagement_Modify()  // 생성자(사용자 등록)
        {
            InitializeComponent();
            current_version = "ragister";

            this.Text = "사용자 등록";
            execution_btn.Text = "등  록";
        }

        public UserManagement_Modify(string[] information)  // 생성자(사용자 정보 수정)
        {
            InitializeComponent();
            current_version = "modify";

            this.Text = "사용자 정보 수정";
            execution_btn.Text = "수  정";

            for(int i = 0; i < modify_information.Length; i++)
            {
                modify_information[i] = information[i];
            }

            // 선택된 사용자 정보 전시 (권한,이름,소속,아이디)
            if (information[1] == "administrator")
            {
                authority_comboBox.SelectedIndex = 0;
            }
            else if (information[1] == "member")
            {
                authority_comboBox.SelectedIndex = 1;
            }
            name_txtBox.Text = information[2];
            division_txtBox.Text = information[3];
            id_txtBox.Text = information[0];
            

        }

        private void id_txtBox_TextChanged(object sender, EventArgs e)
        {
            check_id = false;
        }

        private void check_btn_Click(object sender, EventArgs e)    // 중복확인
        {
            if (string.IsNullOrEmpty(id_txtBox.Text))
            {
                MessageBox.Show("아이디를 입력하세요.");
                return;
            }

            bool available_id = true;
            try
            {
                string connect_str = String.Format("Server=localhost;Port=5432;Database=postgres;User Id={0};password={1}", MainForm.mainForm.login_information[3], MainForm.mainForm.login_information[5]);
                NpgsqlConnection check_connect = new NpgsqlConnection(connect_str);
                check_connect.Open();

                using (NpgsqlCommand check_command = new NpgsqlCommand())
                {
                    check_command.Connection = check_connect;
                    check_command.CommandText = "SELECT * FROM NSLR_User;";

                    using (NpgsqlDataReader reader = check_command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader["user_id"].ToString() == id_txtBox.Text)
                            {
                                available_id = false;
                            }

                        }

                        if (current_version == "modify" && id_txtBox.Text == modify_information[0])
                        {
                            available_id = true;
                        }
                    }

                }

                check_connect.Close();

                // 현재 login 사용자 아이디 수정 시
                if (current_version == "modify" &&  modify_information[0] == MainForm.mainForm.login_information[3] && modify_information[1] == MainForm.mainForm.login_information[0] &&
                    modify_information[2] == MainForm.mainForm.login_information[1] && modify_information[3] == MainForm.mainForm.login_information[2])
                {
                    if (id_txtBox.Text != modify_information[0])
                    {
                        check_id = false;
                        MessageBox.Show("로그인 중인 계정의 아이디는 변경할 수 없습니다.");
                        id_txtBox.Text = modify_information[0];
                        return;
                    }
                    
                }

                if (available_id == true)   // 사용 가능
                {
                    check_id = true;
                    MessageBox.Show("사용 가능한 아이디입니다.");
                }
                else   // 사용 불가능
                {
                    check_id = false;
                    MessageBox.Show("사용 중인 아이디입니다.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void execution_btn_Click(object sender, EventArgs e)    // 등록 or 수정
        {
            if (string.IsNullOrEmpty(authority_comboBox.Text))   // 등급 입력여부 Check
            {
                MessageBox.Show("등급을 입력하세요.");
                return;
            }
            else if (string.IsNullOrEmpty(name_txtBox.Text))  // 이름 입력여부 Check
            {
                MessageBox.Show("이름을 입력하세요.");
                return;
            }
            else if (string.IsNullOrEmpty(division_txtBox.Text))  // 소속 입력여부 Check
            {
                MessageBox.Show("소속을 입력하세요.");
                return;
            }
            else if (string.IsNullOrEmpty(id_txtBox.Text))  // 아이디 입력여부 Check
            {
                MessageBox.Show("아이디를 입력하세요.");
                return;
            }
            else if (string.IsNullOrEmpty(password_txtBox.Text))  // 비밀번호 입력여부 Check
            {
                MessageBox.Show("비밀번호를 입력하세요.");
                return;
            }

            if (check_id == false)
            {
                MessageBox.Show("아이디 중복확인 하십시요.");
                return;
            }
            if (current_version == "ragister")  // 사용자 등록
            {
                try
                {
                    string connect_str = String.Format("Server=localhost;Port=5432;Database=postgres;User Id={0};password={1}", MainForm.mainForm.login_information[3], MainForm.mainForm.login_information[5]);
                    NpgsqlConnection register_connect = new NpgsqlConnection(connect_str);
                    register_connect.Open();

                    // postgres 사용자 등록
                    using (NpgsqlCommand register_command = new NpgsqlCommand())
                    {
                        register_command.Connection = register_connect;
                        register_command.CommandText = String.Format("CREATE USER {0} PASSWORD '{1}' SUPERUSER;" + "INSERT INTO NSLR_User VALUES ('{2}', '{3}', '{4}', '{5}', '{6}');"
                            , id_txtBox.Text, password_txtBox.Text, authority_comboBox.SelectedItem.ToString(), name_txtBox.Text, division_txtBox.Text, id_txtBox.Text, "-");

                        register_command.ExecuteNonQuery();
                    }
                    register_connect.Close();

                    // txtBox 초기화
                    authority_comboBox.SelectedIndex = -1;
                    name_txtBox.Text = string.Empty;
                    division_txtBox.Text = string.Empty;
                    id_txtBox.Text = string.Empty;
                    password_txtBox.Text = string.Empty;

                    MessageBox.Show("사용자 등록 완료");
                    this.Close();   // form 종료
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else if (current_version == "modify")   // 사용자 정보 수정
            {
                try
                {
                    string connect_str = String.Format("Server=localhost;Port=5432;Database=postgres;User Id={0};password={1}", MainForm.mainForm.login_information[3], MainForm.mainForm.login_information[5]);
                    NpgsqlConnection modify_connect = new NpgsqlConnection(connect_str);
                    modify_connect.Open();

                    // postgres 사용자 정보 수정
                    using (NpgsqlCommand modify_command = new NpgsqlCommand())
                    {
                        modify_command.Connection = modify_connect;

                        if (modify_information[0] == id_txtBox.Text)
                        {
                            modify_command.CommandText = String.Format("ALTER USER {0} WITH PASSWORD '{1}';" +
                            "UPDATE NSLR_User SET user_authority='{2}', user_name='{3}', user_division='{4}' WHERE user_authority='{5}' AND user_name='{6}' AND user_division='{7}' AND user_id='{8}';"
                            , modify_information[0], password_txtBox.Text,
                            authority_comboBox.SelectedItem.ToString(), name_txtBox.Text, division_txtBox.Text,
                            modify_information[1], modify_information[2], modify_information[3], modify_information[0]);
                        }
                        else
                        {
                            modify_command.CommandText = String.Format("ALTER USER {0} RENAME TO {1};" + "ALTER USER {2} WITH PASSWORD '{3}';" +
                            "UPDATE NSLR_User SET user_authority='{4}', user_name='{5}', user_division='{6}', user_id='{7}' WHERE user_authority='{8}' AND user_name='{9}' AND user_division='{10}' AND user_id='{11}';"
                            , modify_information[0], id_txtBox.Text, id_txtBox.Text, password_txtBox.Text,
                            authority_comboBox.SelectedItem.ToString(), name_txtBox.Text, division_txtBox.Text, id_txtBox.Text,
                            modify_information[1], modify_information[2], modify_information[3], modify_information[0]);
                        }

                        modify_command.ExecuteNonQuery();
                    }
                    modify_connect.Close();

                    // 현재 login 사용자 정보 수정 시
                    if (modify_information[0] == MainForm.mainForm.login_information[3] && modify_information[1] == MainForm.mainForm.login_information[0] &&
                        modify_information[2] == MainForm.mainForm.login_information[1] && modify_information[3] == MainForm.mainForm.login_information[2])   
                    {
                        MainForm.mainForm.login_information[0] = authority_comboBox.SelectedItem.ToString();    // 등급
                        MainForm.mainForm.login_information[1] = name_txtBox.Text;                              // 이름
                        MainForm.mainForm.login_information[2] = division_txtBox.Text;                           // 소속
                        //MainForm.mainForm.login_information[3] = id_txtBox.Text;                                 // 아이디
                        MainForm.mainForm.login_information[5] = password_txtBox.Text;                           // 비밀번호
                    }

                    // txtBox 초기화
                    authority_comboBox.SelectedIndex = -1;
                    name_txtBox.Text = string.Empty;
                    division_txtBox.Text = string.Empty;
                    id_txtBox.Text = string.Empty;
                    password_txtBox.Text = string.Empty;

                    MessageBox.Show("사용자 정보 수정 완료");
                    this.Close();   // form 종료
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                
            }
        }

        private void close_btn_Click(object sender, EventArgs e)    // 닫기
        {
            this.Close();
        }

        
    }
}
