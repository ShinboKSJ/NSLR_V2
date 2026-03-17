using NSLR_ObservationControl.Module;
using NSLR_ObservationControl.Subsystem;
using SGPdotNET.Observation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using static NSLR_ObservationControl.ControlCommand;

namespace NSLR_ObservationControl
{
    internal class Laser_NavigationTracking     // 레이저 탐색 및 추적 Class
    {
        LAS_SAT_Controller las_controller;
        public Laser_NavigationTracking()
        {
            //las_controller = new LAS_Controller();  // 삭제 검토

        }

        ~Laser_NavigationTracking()
        {

        }



        //////////////////////////////////////////// 탐색 경로 TEST (Part Test) ///////////////////////////////////////////

        // Event Time를 통한 반환률 산출 제외, 레이저 조사/중지 제외, 정밀탐색 여부 Check 제외
        public static bool searching_startFlag = false;
        public static double searching_azUserOffset = 0;
        public static double searching_elUserOffset = 0;
        Thread searching_thread;

        public void temp_SearchingPattern_Function(double min_distance/*최소이동거리*/, int max_number/*최대구동횟수*/)     // 탐색패턴 생성
        {
            // 추적마운트 최대구동횟수 제한
            if (max_number < 2)
            {
                System.Windows.Forms.MessageBox.Show("적용할 수 없는 최대구동횟수입니다.");
                return;
            }

            // 추적마운트 최소 이동거리 - az : 0.3 이하, el : 0.1 이하 / 추적마운트 가동 범위 - -270deg ~ 270deg
            double temp_height = Math.Sqrt((min_distance * min_distance) - ((min_distance / 2.0) * (min_distance / 2.0)));
            if (min_distance > 0.1 || min_distance <= 0 ||
                (Observation_TMS.azUserOffset + (min_distance * max_number)) > 270 ||
                (Observation_TMS.azUserOffset - (min_distance * (max_number - 1))) < -270 ||
                (Observation_TMS.elUserOffset + (temp_height * (max_number - 1))) > 85 ||
                (Observation_TMS.elUserOffset - (temp_height * (max_number - 1))) < 30)
            {
                System.Windows.Forms.MessageBox.Show("적용할 수 없는 최소이동거리 입니다.");
                return;
            }

            if (searching_startFlag == true)
            {
                System.Windows.Forms.MessageBox.Show("탐색 불가!");
                return;
            }

            if (searching_thread != null)
            {
                if (searching_thread.IsAlive)
                {
                    searching_thread.Abort();
                }
                searching_thread = null;
            }

            searching_thread = new Thread(() =>
            {
                try
                {
                    // 현재 AZ/EL 사용자 옵셋값
                    searching_azUserOffset = Observation_TMS.azUserOffset;
                    searching_elUserOffset = Observation_TMS.elUserOffset;

                    // AZ/EL 옵셋 변경값 설정
                    double azUserOffset_settingValue = min_distance;
                    double elUserOffset_settingValue = Math.Sqrt((min_distance * min_distance) - ((min_distance / 2.0) * (min_distance / 2.0)));

                    // Flag On
                    searching_startFlag = true;

                    for (int i = 0; i < max_number - 1; i++)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            for (int k = 0; k <= i; k++)
                            {
                                switch (j)
                                {
                                    case 0:
                                        searching_azUserOffset += azUserOffset_settingValue / 2.0;
                                        searching_elUserOffset += elUserOffset_settingValue;
                                        break;
                                    case 1:
                                        searching_azUserOffset += azUserOffset_settingValue;
                                        break;
                                    case 2:
                                        searching_azUserOffset += azUserOffset_settingValue / 2.0;
                                        searching_elUserOffset -= elUserOffset_settingValue;
                                        break;
                                    case 3:
                                        searching_azUserOffset -= azUserOffset_settingValue / 2.0;
                                        searching_elUserOffset -= elUserOffset_settingValue;
                                        break;
                                    case 4:
                                        searching_azUserOffset -= azUserOffset_settingValue;
                                        break;
                                    case 5:
                                        searching_azUserOffset -= azUserOffset_settingValue;
                                        k = i;  // 한번만 실행
                                        break;
                                    case 6:
                                        searching_azUserOffset -= azUserOffset_settingValue / 2.0;
                                        searching_elUserOffset += elUserOffset_settingValue;
                                        break;
                                }



                                //Console.WriteLine("&& 탐색패턴 옵셋적용 &&" + "\n\r" + "Searching_azUserOffset : " + searching_azUserOffset + " / " + "Searching_elUserOffset : " + searching_elUserOffset + "\n\r");

                                // 추적마운트 Delay 설정
                                Thread.Sleep(1000);
                            }

                        }
                    }

                    // Flag Off
                    searching_startFlag = false;
                    System.Windows.Forms.MessageBox.Show("탐색 완료");
                }
                catch
                {
                    searching_startFlag = false;
                    searching_thread.Abort();
                    System.Windows.Forms.MessageBox.Show("searching 중단");
                }

            });

            // 탐색 시작
            searching_thread.Start();


        }

        //////////////////////////////////////////// 레이저 탐색 /////////////////////////////////////////////


        // 추적마운트 AZ/EL 사용자 옵셋 SET

        // 추적마운트 AZ/EL 사용자 옵셋 GET

        public static int total_maxLaserCount = 0;
        public static int total_laserCount = 0;
        public static int return_laserCount = 0;
        public static bool calculate_flag = false;

        public double Calculate_ReceptionRate(int totalData)    // 레이저 반환율 계산
        {
            total_maxLaserCount = totalData;
            total_laserCount = 0;
            return_laserCount = 0;
            //calculate_flag = true;

            Task task_laserCounting = new Task(() =>
            {
                while (true)
                {
                    // 레이저 조사 상태 : 조사 중 일때
                    if (las_controller.monitor_las_data.LaserFireState == "01")
                    {
                        if (total_laserCount == 0 && calculate_flag == false)
                        {
                            calculate_flag = true;
                        }

                        if (calculate_flag == false) { break; }
                    }
                    
                    Thread.Sleep(100);
                }
            });

            task_laserCounting.Start();
            task_laserCounting.Wait();

            return (((double)return_laserCount / total_laserCount) * 100.0);
        }

        public bool SearchingPattern_Function(double min_distance/*최소이동거리*/, int max_number/*최대구동횟수*/)     // 탐색패턴 생성
        {
            // 반환율
            double reception_rate = 0.0;

            // 현재 AZ/EL 사용자 옵셋값
            double azUserOffset = NSLR_ObservationControl.Module.Observation_TMS.azUserOffset;
            double elUserOffset = NSLR_ObservationControl.Module.Observation_TMS.elUserOffset;

            // AZ/EL 옵셋 변경값 설정
            double azUserOffset_settingValue = min_distance;
            double elUserOffset_settingValue = Math.Sqrt((min_distance * min_distance) - ((min_distance / 2.0) * (min_distance / 2.0)));


            for (int i = 0; i < max_number - 1; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    for (int k = 0; k <= i; k++)
                    {
                        switch (j)
                        {
                            case 0:
                                azUserOffset += azUserOffset_settingValue / 2.0;
                                elUserOffset += elUserOffset_settingValue;
                                break;
                            case 1:
                                azUserOffset += azUserOffset_settingValue;
                                break;
                            case 2:
                                azUserOffset += azUserOffset_settingValue / 2.0;
                                elUserOffset -= elUserOffset_settingValue;
                                break;
                            case 3:
                                azUserOffset -= azUserOffset_settingValue / 2.0;
                                elUserOffset -= elUserOffset_settingValue;
                                break;
                            case 4:
                                azUserOffset -= azUserOffset_settingValue;
                                break;
                            case 5:
                                azUserOffset -= azUserOffset_settingValue;
                                k = i;  // 한번만 실행
                                break;
                            case 6:
                                azUserOffset -= azUserOffset_settingValue / 2.0;
                                elUserOffset += elUserOffset_settingValue;
                                break;
                        }

                        Console.WriteLine("&& 탐색패턴 옵셋적용 &&" + "\n\r" + "azUserOffset : " + azUserOffset + " / " + "elUserOffset : " + elUserOffset + "\n\r");

                        if (las_controller.monitor_las_data.LaserFireState == "00")  // 레이저 조사 상태 : 조사 중지 및 완료
                        {
                            // 추적마운트 옵셋 적용
                            NSLR_ObservationControl.Module.Observation_TMS.azUserOffset = azUserOffset;
                            NSLR_ObservationControl.Module.Observation_TMS.elUserOffset = elUserOffset;
                        }

                        // 추적마운트 옵셋 적용여부 Check >> 이후 다음작업 수행
                        Task task_userOffsetCheck = new Task(() =>
                        {
                            Thread.Sleep(200);

                            while (true)
                            {
                                // 추적마운트 옵셋변경 확인여부 >> 확인필요
                                // 추적마운트 구동상태 : Servo Disable 일 때(임시)
                                if (Observation_TMS.TMScontrolCommand == (ushort)0x0302)
                                {
                                    break;
                                }
                            }
                        });

                        task_userOffsetCheck.Start();
                        task_userOffsetCheck.Wait();


                        // 레이저 조사 명령 : 조사 시작 >> 레이저부 제어명령으로 Control
                        las_controller.TX_LaserStartStop = "01";

                        // 반환율 산출
                        reception_rate = Calculate_ReceptionRate(20000);

                        // 레이저 조사 명령 : 조사 중지 >> 레이저부 제어명령으로 Control
                        las_controller.TX_LaserStartStop = "00";

                        // 탐색 진행여부(Continue or Stop) 결정
                        if (reception_rate > 50.0/*임시*/)
                        {
                            // 정밀탐색 수행
                            DetailedSearchingPattern_Function(min_distance, max_number);
                            return true;    // 레이저 탐색 >> Succeed
                        }


                    }

                }
            }

            return false;   // 레이저 탐색 > Fail

        }

        public double DetailedSearchingPattern_Function(double min_distance/*최소이동거리*/, int max_number/*최대구동횟수*/)     // 정밀탐색패턴 생성
        {
            // 현재 AZ/EL 사용자 옵셋값 
            double azUserOffset = NSLR_ObservationControl.Module.Observation_TMS.azUserOffset;
            double elUserOffset = NSLR_ObservationControl.Module.Observation_TMS.elUserOffset;

            // AZ/EL 옵셋 변경값 설정
            double azUserOffset_settingValue = min_distance / max_number;
            double elUserOffset_settingValue = Math.Sqrt(((min_distance / max_number) * (min_distance / max_number)) - (((min_distance / max_number) / 2.0) * ((min_distance / max_number) / 2.0)));

            // 추적마운트 각 옵셋별 반환율 저장
            double reception_rate = 0.0;
            double[][][][] reception_rates = new double[max_number - 1][][][];

            for (int i = 0; i < reception_rates.Length; i++)
            {
                reception_rates[i] = new double[7][][];
            }

            for (int i = 0; i < reception_rates.Length; i++)
            {
                for (int j = 0; j < reception_rates[i].Length; j++)
                {
                    if (j == 5)
                    {
                        reception_rates[i][j] = new double[1][];
                    }
                    else
                    {
                        reception_rates[i][j] = new double[i + 1][];
                    }

                    for (int k = 0; k < reception_rates[i][j].Length; k++)
                    {
                        reception_rates[i][j][k] = new double[3];    // 1. 추적마운트 AZ Offset값 2. EL Offset값 3. 해당 AZ/EL 옵셋값일때 반환율
                    }
                }
            }


            for (int i = 0; i < max_number - 1; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    for (int k = 0; k <= i; k++)
                    {
                        switch (j)
                        {
                            case 0:
                                azUserOffset += azUserOffset_settingValue / 2.0;
                                elUserOffset += elUserOffset_settingValue;
                                break;
                            case 1:
                                azUserOffset += azUserOffset_settingValue;
                                break;
                            case 2:
                                azUserOffset += azUserOffset_settingValue / 2.0;
                                elUserOffset -= elUserOffset_settingValue;
                                break;
                            case 3:
                                azUserOffset -= azUserOffset_settingValue / 2.0;
                                elUserOffset -= elUserOffset_settingValue;
                                break;
                            case 4:
                                azUserOffset -= azUserOffset_settingValue;
                                break;
                            case 5:
                                azUserOffset -= azUserOffset_settingValue;
                                k = i;  // 한번만 실행
                                break;
                            case 6:
                                azUserOffset -= azUserOffset_settingValue / 2.0;
                                elUserOffset += elUserOffset_settingValue;
                                break;
                        }

                        Console.WriteLine("&& 정밀탐색패턴 옵셋적용 &&" + "\n\r" + "azUserOffset : " + azUserOffset + " / " + "elUserOffset : " + elUserOffset + "\n\r");

                        if (las_controller.monitor_las_data.LaserFireState == "00")  // 레이저 조사 상태 : 조사 중지 및 완료
                        {
                            // 추적마운트 옵셋 적용
                            NSLR_ObservationControl.Module.Observation_TMS.azUserOffset = azUserOffset;
                            NSLR_ObservationControl.Module.Observation_TMS.elUserOffset = elUserOffset;
                        }

                        // 추적마운트 옵셋 적용여부 Check >> 이후 다음작업 수행
                        Task task_userOffsetCheck = new Task(() =>
                        {
                            Thread.Sleep(200);

                            while (true)
                            {
                                // 추적마운트 옵셋변경 확인여부 >> 확인필요
                                // 추적마운트 구동상태 : Servo Disable 일 때
                                if (Observation_TMS.TMScontrolCommand == (ushort)0x0302)
                                {
                                    break;
                                }
                            }
                        });

                        task_userOffsetCheck.Start();
                        task_userOffsetCheck.Wait();


                        // 레이저 조사 명령 : 조사 시작 >> 레이저부 제어명령으로 Control
                        las_controller.TX_LaserStartStop = "01";

                        // 반환율 산출
                        reception_rate = Calculate_ReceptionRate(20000);

                        // 레이저 조사 명령 : 조사 중지 >> 레이저부 제어명령으로 Control
                        las_controller.TX_LaserStartStop = "00";

                        // 추적마운트 옵셋 및 반환율 저장
                        reception_rates[i][j][k][0] = azUserOffset;
                        reception_rates[i][j][k][1] = elUserOffset;
                        reception_rates[i][j][k][2] = reception_rate;



                    }

                }
            }

            // 최고 반환율에 따른 추적마운트 최종 옵셋 결정

            double final_azUserOffset = 0.0, final_elUserOffset = 0.0, final_receptionRate = 0.0;

            for (int i = 0; i < reception_rates.Length; i++)
            {
                for (int j = 0; j < reception_rates[i].Length; j++)
                {
                    for (int k = 0; k < reception_rates[i][j].Length; k++)
                    {
                        if (i == 0 && j == 0 && k == 0)
                        {
                            final_azUserOffset = reception_rates[i][j][k][0];
                            final_elUserOffset = reception_rates[i][j][k][1];
                            final_receptionRate = reception_rates[i][j][k][2];
                        }
                        else
                        {
                            if (final_receptionRate < reception_rates[i][j][k][2])
                            {
                                final_azUserOffset = reception_rates[i][j][k][0];
                                final_elUserOffset = reception_rates[i][j][k][1];
                                final_receptionRate = reception_rates[i][j][k][2];
                            }
                        }

                    }
                }
            }

            Console.WriteLine("&& 정밀탐색패턴 최종옵셋적용 &&" + "\n\r" + "final_azUserOffset : " + final_azUserOffset + " / " + "final_elUserOffset : " + final_elUserOffset + "\n\r");

            if (las_controller.monitor_las_data.LaserFireState == "00")  // 레이저 조사 상태 : 조사 중지 및 완료
            {
                // 추적마운트 최종 옵셋 적용
                NSLR_ObservationControl.Module.Observation_TMS.azUserOffset = final_azUserOffset;
                NSLR_ObservationControl.Module.Observation_TMS.elUserOffset = final_elUserOffset;
            }

            // 추적마운트 옵셋 적용여부 Check >> 이후 다음작업 수행
            Task task_finalUserOffsetCheck = new Task(() =>
            {
                Thread.Sleep(200);

                while (true)
                {
                    // 추적마운트 옵셋변경 확인여부 >> 확인필요
                    // 추적마운트 구동상태 : Servo Disable 일 때
                    if (Observation_TMS.TMScontrolCommand == (ushort)0x0302)
                    {
                        break;
                    }
                }
            });

            task_finalUserOffsetCheck.Start();
            task_finalUserOffsetCheck.Wait();

            // 최종 반환율 Return/출력
            Console.WriteLine("정밀탐색 결과(최고 반환율) : " + final_receptionRate);
            return final_receptionRate;

        }

        //////////////////////////////////////////// 레이저 추적 /////////////////////////////////////////////

        public const double receptionRate_highValue = 80.0;
        public const double receptionRate_middleValue = 60.0;
        public const double receptionRate_lowValue = 0.0;
        Thread automatic_tracking;

        public void LaserTracking_Start(Satellite Sat, int msValue/*200~400ms*/)  // 자동추적 Setting 값 설정 및 추적 시작
        {
            Satellite satellite = Sat;

            // 선택된 물체에 대한 스케줄링 정보 GET
            var eciCoordinate = satellite.Predict(DateTime.Now);
            var longitude = Convert.ToDouble(eciCoordinate.ToGeodetic().Longitude.Degrees);  // 경도
            var latitude = Convert.ToDouble(eciCoordinate.ToGeodetic().Latitude.Degrees);    // 위도

            // 해당부분 중복으로인해 추후 제외여부 판단
            if (las_controller.monitor_las_data.LaserFireState == "00")  // 레이저 조사 상태 : 조사 중지 및 완료
            {
                // 추적마운트 AZ/EL 지향각 데이터 설정
                for (int i = 0; i < 8; i++)
                {
                    Observation_TMS.azimuthData[i] = longitude;
                }

                for (int i = 0; i < 8; i++)
                {
                    Observation_TMS.elevationData[i] = latitude;
                }
            }

            // 자동추적 상태 결정
            if (LaserTracking_StatusDecision())
            {
                automatic_tracking = new Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            // 스케줄링에 따른, 현재 시작 기준 위/경도 데이터 저장
                            eciCoordinate = satellite.Predict(DateTime.Now);
                            longitude = Convert.ToDouble(eciCoordinate.ToGeodetic().Longitude.Degrees);  // 경도
                            latitude = Convert.ToDouble(eciCoordinate.ToGeodetic().Latitude.Degrees);    // 위도

                            Console.WriteLine("&& 자동추적 위/경도 데이터 &&" + "\n\r" + "longitude : " + longitude + " / " + "latitude : " + latitude + "\n\r");

                            if (las_controller.monitor_las_data.LaserFireState == "00")  // 레이저 조사 상태 : 조사 중지 및 완료
                            {
                                // 추적마운트 AZ/EL 지향각 데이터 설정
                                for (int i = 0; i < 8; i++)
                                {
                                    Observation_TMS.azimuthData[i] = longitude;
                                }

                                for (int i = 0; i < 8; i++)
                                {
                                    Observation_TMS.elevationData[i] = latitude;
                                }
                            }

                            // 추적마운트 위치 완료 확인
                            Thread.Sleep(200);

                            while (true)
                            {
                                // 추적마운트 위치 완료 확인여부 >> 확인필요
                                // 추적마운트 구동상태 : Servo Disable 일 때
                                if (Observation_TMS.TMScontrolCommand == (ushort)0x0302)
                                {
                                    break;
                                }

                                // 추적마운트 AZ/EL 실제 위치(상태정보)가 설정한 위/경도 데이터(제어명령)와 일치할 때
                                if (Observation_TMS.TMSAZPositon == longitude && Observation_TMS.TMSELPosition == latitude)
                                {
                                    break;
                                }

                            }

                            

                            // 레이저 반환율 측정 >> 반환율에 따른 추적진행여부 판단
                            if (LaserTracking_StatusDecision()) { Thread.Sleep(msValue); }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("레이저 반환율 기준치 미달로 인한 레이저 추적 중지");
                                break;
                            }
                        }
                        catch(Exception e)
                        {
                            LaserTracking_Stop();
                            System.Windows.Forms.MessageBox.Show("레이저 자동추적 실패 : " + e.Message);
                        }
                    }

                });

                automatic_tracking.Start();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("레이저 반환율 기준치 미달로 인한 레이저 추적 미실시");
            }

        }

        public void LaserTracking_Stop()    // 자동추적 종료
        {
            if (automatic_tracking.IsAlive)
            {
                automatic_tracking.Abort();
                automatic_tracking = null;
            }

        }

        public bool LaserTracking_StatusDecision()  // 레이저 반환율 확인 및 자동추적 상태 결정
        {
            bool execution_flag = false;    // false : 추적 미실시, true : 추적 실시

            // 레이저 조사 명령 : 조사 시작 >> 레이저부 제어명령으로 Control
            las_controller.TX_LaserStartStop = "01";

            // 레이저 반환율 측정
            double reception_rate = 0.0;
            reception_rate = Calculate_ReceptionRate(10000);

            // 레이저 조사 명령 : 조사 중지 >> 레이저부 제어명령으로 Control
            las_controller.TX_LaserStartStop = "00";


            // 레이저 반환율에 따른 상태결정
            if (reception_rate > receptionRate_highValue)
            {
                // 레이저 반환율 (상) >> 자동추적 수행
                execution_flag = true;
            }
            else if (reception_rate > receptionRate_middleValue)
            {
                // 레이저 반환율 (중) >> 정밀탐색 수행
                reception_rate = DetailedSearchingPattern_Function(2.0/*임시*/, 5/*임시*/);
                if (reception_rate > receptionRate_highValue)
                {
                    execution_flag = true;
                }
            }
            else if (reception_rate > receptionRate_lowValue)
            {
                // 레이저 반환율 (하) >> 레이저 탐색 수행
                SearchingPattern_Function(2.0/*임시*/, 5/*임시*/);

            }

            return execution_flag;
        }

    }
}
