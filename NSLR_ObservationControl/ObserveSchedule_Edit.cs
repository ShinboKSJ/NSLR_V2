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
    public partial class ObserveSchedule_Edit : Form
    {
        private CSU_ObserveSchedule2 parent_form;
        DateTime standard_dateTime;
        string selected_satelliteName = "";
        List<string> total_names = new List<string>();
        List<int> total_durations = new List<int>();

        int start_index;    // 위성추적 시작시점
        int end_index;      // 위성추적 종료시점

        public ObserveSchedule_Edit(CSU_ObserveSchedule2 csu_observeSchedule2, DateTime dateTime, string satellite_name, List<string> total_satelliteNames, List<int> total_satelliteDurations)
        {
            InitializeComponent();

            parent_form = csu_observeSchedule2;
            standard_dateTime = dateTime;
            selected_satelliteName = satellite_name;
            total_names = total_satelliteNames;
            total_durations = total_satelliteDurations;
        }

        private void ObserveSchedule_Edit_Load(object sender, EventArgs e)
        {
            // 선택된 위성이름 Setting
            satelliteName_txt.Text = selected_satelliteName;

            // 지정할 수 있는 시작 시간 Searching
            DateTime standard_localTime = standard_dateTime.ToLocalTime();
            for (int i = 0; i < total_names.Count; i++)
            {
                
                if (total_names[i] == "empty")
                {
                    for(int j = 0; j < total_durations[i]; j++)
                    {

                        DateTime add_dateTime = standard_localTime.AddHours(j);
                        startTime_combo.Items.Add(add_dateTime.Year.ToString() + "-" + add_dateTime.Month.ToString() + "-" + add_dateTime.Day.ToString() + " " + add_dateTime.Hour.ToString() + "시");
                    }
                }

                standard_localTime = standard_localTime.AddHours(total_durations[i]);
            }

            endTime_combo.Visible = false;
            
        }

        
        private void startTime_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 시작 시점 설정
            string[] split_string = startTime_combo.SelectedItem.ToString().Split(' ');
            string[] split_date = split_string[0].Split('-');
            string[] split_time = split_string[1].Split('시');
            DateTime start_dateTime = new DateTime(int.Parse(split_date[0]), int.Parse(split_date[1]), int.Parse(split_date[2]), int.Parse(split_time[0]), 0, 0);

            TimeSpan start_timeSpan = start_dateTime - standard_dateTime.ToLocalTime();
            start_index = start_timeSpan.Hours;

            // 지정할 수 있는 종료 시간 Searching
            if(endTime_combo.Items.Count != 0 ) { endTime_combo.Items.Clear(); }

            int check_index = 0;
            DateTime standard_localTime = standard_dateTime.ToLocalTime();
            for (int i = 0; i < total_names.Count; i++)
            {
                int j;
                for (j = 0; j <= total_durations[i]; j++)
                {
                    if (check_index > start_index)
                    {
                        if (total_names[i] == "empty")
                        {
                            DateTime add_dateTime = standard_localTime.AddHours(check_index);
                            endTime_combo.Items.Add(add_dateTime.Year.ToString() + "-" + add_dateTime.Month.ToString() + "-" + add_dateTime.Day.ToString() + " " + add_dateTime.Hour.ToString() + "시");
                        }
                        else { break; }
                        
                    }

                    if (j != total_durations[i]) { check_index++; }
                }
                
                if (j != total_durations[i] + 1) { break; }
            }

            endTime_combo.Visible = true;
        }

        private void endTime_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 종료 시점 설정
            string[] split_string = endTime_combo.SelectedItem.ToString().Split(' ');
            string[] split_date = split_string[0].Split('-');
            string[] split_time = split_string[1].Split('시');
            DateTime end_dateTime = new DateTime(int.Parse(split_date[0]), int.Parse(split_date[1]), int.Parse(split_date[2]), int.Parse(split_time[0]), 0, 0);

            TimeSpan end_timeSpan = end_dateTime - standard_dateTime.ToLocalTime();
            end_index = end_timeSpan.Hours;
        }

        private void ok_btn_Click(object sender, EventArgs e)
        {
            // 추적 시작시간 및 종료시간 입력 여부 확인 후 pass
            if (startTime_combo.SelectedIndex == -1 || endTime_combo.SelectedIndex == -1)
            {
                MessageBox.Show("추적 시작/종료 시간을 선택하세요.");
            }
            else
            {
                // 추가 위성 스케줄러 List에 반영
                List<string> temp_names = new List<string>();
                List<int> temp_durations = new List<int>();
                int check_index = 0;
                for (int i = 0; i < total_names.Count; i++)
                {
                    bool addition_flag = false;
                    for (int j = 0; j <= total_durations[i]; j++)
                    {
                        if (check_index == start_index)
                        {
                            // 이전 Block 체크 및 추가
                            if (j != 0)
                            {
                                temp_names.Add(total_names[i]);
                                temp_durations.Add(j);
                            }
                        }
                        else if (check_index == end_index)
                        {
                            // 추가 위성에 대한 Block 생성
                            temp_names.Add(selected_satelliteName);
                            temp_durations.Add(end_index - start_index);

                            // 다음 Block 체크 및 추가
                            if (j < total_durations[i])
                            {
                                temp_names.Add(total_names[i]);
                                temp_durations.Add(total_durations[i] - j);
                            }

                            // flag : 추가 완료
                            addition_flag = true;
                        }

                        if (j != total_durations[i]) { check_index++; }
                    }

                    if (addition_flag == false)
                    {
                        temp_names.Add(total_names[i]);
                        temp_durations.Add(total_durations[i]);
                    }
                    
                }

                // 부모 Form (CSU_ObserveSchedule2.cs) 으로 데이터 전달 및 현재 Form 종료
                //parent_form.ReceiveData_addSchedule(temp_names, temp_durations);
                this.Close();
            }

            
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            // 현재 Form 종료
            this.Close();
        }

        
    }
}
