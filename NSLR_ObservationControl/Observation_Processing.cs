using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static log4net.Appender.RollingFileAppender;

namespace NSLR_ObservationControl
{
    public  class Observation_Processing
    {
        DateTime begin_dateTime = new DateTime();           // 관측 시작시간 [KST]
        DateTime end_dateTime = new DateTime();             // 관측 종료시간 [KST]

        string satellite_name = null;                       // 위성 이름
        string prediction_center = null;                    // 궤도 예측 데이터 관측소
        int cospar_id = 0;                                  // cospar 번호
        DateTime predicStart_dateTime = new DateTime();     // 예측 데이터 관측시작시간
        int predictStart_dayOfYear = 0;                     // 얘측 시작 년일수
        int predict_interval = 0;                           // 예측 시간 간격
        int predict_count = 0;                              // 예측 데이터 개수

        List<Tuple<DateTime, double, double, double, double>> predict_data = new List<Tuple<DateTime, double, double, double, double>>();   // 예측 데이터

        float temperature_pre = 0.0f;                       // 관측 전 온도 (단위 : 섭씨)
        float pressure_pre = 0.0f;                          // 관측 전 기압 (단위 : HPA)
        float temperature_post = 0.0f;                      // 관측 후 온도 (단위 : 섭씨)
        float pressure_post = 0.0f;                         // 관측 후 기압 (단위 : HPA)

        List<Tuple<int, double, double>> observation_data = new List<Tuple<int, double, double>>();     // 관측 데이터

        public Observation_Processing()
        {
            
        }

        
        
        public void ObservationToSOD(string orbit_fileName, DateTime pre_dateTime, DateTime post_dateTime, List<Tuple<int, double, double>> observation_data)
        {
            // Create SOD Sequence //

            // Initialize
            Initalize_Data();

            // Step 0 : Operation 데이터 설정
            Setting_OperationInformation(pre_dateTime, post_dateTime);

            // Step 1 : Predict 데이터 설정
            if (CSU_ObserveSchedule2.TLE_Information.ContainsKey(orbit_fileName) == true)   // orbit_fileName → TLE
            {
                Setting_PredictInformation("TLE", orbit_fileName, pre_dateTime, post_dateTime);
            }
            else if (File.Exists("./CPF/" + orbit_fileName) == true) // orbit_fileName → CPF
            {
                Setting_PredictInformation("CPF", orbit_fileName, pre_dateTime, post_dateTime);
            }
            else
            {
                MessageBox.Show("SOD 생성 실패 : 궤도정보 파일 찾을 수 없음.");
                return;
            }

            // Step2 : 기상 데이터 설정
#if LOCALTEST
            // [LOCALTEST] 기상 DB 미연동 — 표준 대기값 사용
            temperature_pre  = 15.0f;
            pressure_pre     = 1013.25f;
            temperature_post = 15.0f;
            pressure_post    = 1013.25f;
#else
            (bool step2_check, float[][] step2_data) = Setting_WeatherInformation(pre_dateTime, post_dateTime);
            if (step2_check == false)
            {
                MessageBox.Show("SOD 생성 실패 : 해당 시간에 대한 기상정보 없음.");
                return;
            }
            else
            {
                temperature_pre  = step2_data[0][0];
                pressure_pre     = step2_data[0][1];
                temperature_post = step2_data[1][0];
                pressure_post    = step2_data[1][1];
            }
#endif

            // Step3 : 관측 데이터 설정
            bool step3_check = Setting_ObservationInformation(observation_data);
            if (step3_check == false)
            {
                MessageBox.Show("SOD 생성 실패 : 관측 데이터 없음.");
                return;
            }

            // Step4 : 천문연 프로그램 SOD 형식 파일생성
            bool step4_check = Create_SOD();
            if (step4_check == false)
            {
                MessageBox.Show("SOD 생성 실패 : 해당 SOD 파일이 존재함.");
            }
            else
            {
                MessageBox.Show("SOD 생성 완료.");
            }
        }

        public void Initalize_Data()    // 전역변수 초기화
        {
            begin_dateTime = new DateTime();
            end_dateTime = new DateTime();

            satellite_name = null;
            prediction_center = null;
            cospar_id = 0;
            predicStart_dateTime = new DateTime();
            predictStart_dayOfYear = 0;
            predict_interval = 0;
            predict_count = 0;

            predict_data.Clear();

            temperature_pre = 0.0f;
            pressure_pre = 0.0f;
            temperature_post = 0.0f;
            pressure_post = 0.0f;

            observation_data.Clear();
        }

        public void Setting_OperationInformation(DateTime startUTC, DateTime endUTC)    // Step 0 : Operation 데이터 설정
        {
            begin_dateTime = startUTC.ToLocalTime();
            end_dateTime = endUTC.ToLocalTime();
        }

        public void Setting_PredictInformation(string orbit_type, string fileName, DateTime start_utc, DateTime end_utc) // Step 1 : Predict 데이터 설정
        {
            string satelliteName = "";
            string predictionCenter = "";
            int cosparID = 0;
            DateTime predictStartDateTime = new DateTime();
            int predictStartDayOfYear = 0;
            int predictInterval = -1;
            int predictCount = 0;

            List<Tuple<DateTime, double, double, double, double>> predictData = new List<Tuple<DateTime, double, double, double, double>>();

            int ephemeris_interval = 12;

            if (orbit_type == "TLE")
            {
                DateTime temp_start = start_utc.AddSeconds(-1 * ephemeris_interval * 10);
                DateTime start = new DateTime(temp_start.Year, temp_start.Month, temp_start.Day, temp_start.Hour, temp_start.Minute, 0, DateTimeKind.Utc);
                DateTime temp_end = end_utc.AddSeconds(ephemeris_interval * 10);
                DateTime end = new DateTime(temp_end.Year, temp_end.Month, temp_end.Day, temp_end.Hour, temp_end.Minute, 0, DateTimeKind.Utc);

                satelliteName = fileName;
                predictionCenter = "";
                cosparID = 8606101; // 위성에 맞는 cosparID 추출 → 천문연 프로그램 로딩문제로 cosparID 고정(ID : 8606101)
                predictStartDateTime = start;
                predictStartDayOfYear = predictStartDateTime.DayOfYear;
                predictInterval = ephemeris_interval;

                predictData = CSU_ObserveSchedule2.TLE_Propagator(fileName, start, end, ephemeris_interval);

                predictCount = predictData.Count;
            }
            else if (orbit_type == "CPF")
            {
                FileInfo fileInfo = new FileInfo("./CPF/" + fileName);
                double temp = -1.0;

                using (var cpf_reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                {
                    while (!cpf_reader.EndOfStream)
                    {
                        string line = cpf_reader.ReadLine();
                        string[] line_words = line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);


                        switch (line_words[0])
                        {
                            case "H1":
                            case "h1":
                                satelliteName = line_words[10];
                                predictionCenter = line_words[3];
                                break;
                            case "H2":
                            case "h2":
                                cosparID = int.Parse(line_words[1]);
                                break;
                            case "10":

                                // predictInterval 산출을 위해, CPF 상 첫번째 10번 항목(데이터)는 버림.
                                if (temp == -1.0)
                                {
                                    temp = double.Parse(line_words[3]);
                                    break;
                                }
                                else if (temp != -1.0 && predictInterval == -1)
                                {
                                    predictInterval = Math.Abs((int)(double.Parse(line_words[3]) - temp));
                                }


                                // CPF 상에서, MJD(날짜) ↔ SecondsOfDay(하루중 초) 매칭을 위해 DateTime 변환해야함.
                                DateTime cur_dateTime = ConvertMJDToDateTime(int.Parse(line_words[2])) + TimeSpan.FromSeconds(double.Parse(line_words[3]));

                                if (DateTime.Compare(cur_dateTime, start_utc - TimeSpan.FromSeconds(predictInterval)) == 1 &&
                                    DateTime.Compare(cur_dateTime, end_utc + TimeSpan.FromSeconds(predictInterval * 4)) == -1)
                                {
                                    // CPF Period (X) → Observation Period (X) → Predict Period (O, 최종)
                                    if (predictData.Count == 0)
                                    {
                                        predictStartDateTime = cur_dateTime;
                                        predictStartDayOfYear = predictStartDateTime.DayOfYear;
                                    }

                                    predictCount++;

                                    predictData.Add(Tuple.Create(cur_dateTime, double.Parse(line_words[5]), double.Parse(line_words[6]), double.Parse(line_words[7]), 1.0));
                                }

                                break;
                        }

                    }
                }

                predictData = CSU_ObserveSchedule2.CPF_Interpolation(predictData, predictInterval, ephemeris_interval);

                cosparID = 8606101; // 위성에 맞는 cosparID 추출 → 천문연 프로그램 로딩문제로 cosparID 고정(ID : 8606101)
                predictInterval = ephemeris_interval;
                predictCount = predictData.Count;
            }

            satellite_name = satelliteName;
            prediction_center = predictionCenter;
            cospar_id = cosparID;
            predicStart_dateTime = predictStartDateTime;
            predictStart_dayOfYear = predictStartDayOfYear;
            predict_interval = predictInterval;
            predict_count = predictCount;

            predict_data = predictData.ToList();
        }

        public DateTime ConvertMJDToDateTime(int mjd_value)
        {
            // 입력 파라미터는 일 단위(정수값) MJD
            double mjd = (double)mjd_value;
            DateTime mjdEpoch = new DateTime(1858, 11, 17, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateTime = mjdEpoch.AddDays(mjd);
            
            return dateTime;
        }

        string dataPC_postgresql_connectStr = String.Format("Server={0};Port={1};Database=postgres;User Id={2};password={3}", "192.168.10.13", "5432", "postgres", "1234");
        //string postgresql_connectStr = String.Format("Server={0};Port={1};Database=postgres;User Id={2};password={3}", "192.168.10.10", "55432", "postgres", "1234");

        public (bool, float[][]) Setting_WeatherInformation(DateTime start_UTC, DateTime end_UTC)    // Step2 : 기상 데이터 설정
        {
            bool exist_data = false;
            float[][] wms_data = new float[2][];

            DateTime start_local = (new DateTime(start_UTC.Year, start_UTC.Month, start_UTC.Day, start_UTC.Hour, 
                start_UTC.Minute, 0, DateTimeKind.Utc)).ToLocalTime();
            DateTime end_local = (new DateTime(end_UTC.Year, end_UTC.Month, end_UTC.Day, end_UTC.Hour, 
                end_UTC.Minute, 0, DateTimeKind.Utc)).ToLocalTime();

            //  WMS 기상정보 불러오기
            using (NpgsqlConnection connect = new NpgsqlConnection(dataPC_postgresql_connectStr))
            {
                try
                {
                    connect.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand())
                    {
                        command.Connection = connect;

                        // WMS 기상정보 Searching
                        command.CommandText = "SELECT * FROM nslr_wmsinformation;";
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Start Time
                                if (reader["date"].ToString() == start_local.ToString("yyyy-MM-dd") && reader["time"].ToString() == start_local.ToString("HH:mm:ss"))
                                {
                                    wms_data[0] = new float[] { float.Parse(reader["temperature"].ToString()), float.Parse(reader["pressure"].ToString()) };
                                    wms_data[1] = new float[] { float.Parse(reader["temperature"].ToString()), float.Parse(reader["pressure"].ToString()) };
                                    break;
                                }

                                // End Time (관측 강제종료 가능성 있음, Start Time 의 기상정보로 대체)
                                /*
                                if (reader["date"].ToString() == end_local.ToString("yyyy-MM-dd") && reader["time"].ToString() == end_local.ToString("HH:mm:ss"))
                                {
                                    wms_data[1] = new float[] { float.Parse(reader["temperature"].ToString()), float.Parse(reader["pressure"].ToString())};
                                }
                                */
                            }
                        }

                        connect.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Postgresql : WMS Searching 오류발생!");
                }
                finally
                {
                    connect.Close();
                }

            }

            if (wms_data[0] == null || wms_data[1] == null)
            {
                exist_data = false;
                for (int i = 0; i < wms_data.Length; i++) { wms_data[i] = new float[] { 0.0f, 0.0f }; }
            }
            else
            {
                exist_data = true;
            }

            return (exist_data, wms_data);
        }

        public bool Setting_ObservationInformation(List<Tuple<int, double, double>> data)   // Step3 : 관측 데이터 설정
        {
            if (data.Count > 0)
            {
                observation_data = data
    .Select(d => new Tuple<int, double, double>(d.Item1, d.Item2, d.Item3 * 1000))
    .ToList();

                return true;
            }
            else { return false; }
            
        }

        string sod_directory = "./SOD";
        public bool Create_SOD()    // Step4 : 천문연 프로그램 SOD 형식 파일생성
        {
            DirectoryInfo sod_directoryInfo = new DirectoryInfo(sod_directory);
            if (sod_directoryInfo.Exists == false) { sod_directoryInfo.Create(); }

            string sod_fileName = DateTime.Now.ToString("yyyyMMddHHmmss_") + satellite_name + ".sod";
            FileInfo sod_fileInfo = new FileInfo(sod_directory + "/" + sod_fileName);

            // 동일 SOD File 생성 방지
            if (sod_fileInfo.Exists == true) { return false; }

            using (StreamWriter writer = sod_fileInfo.CreateText())
            {
                // [Title]
                writer.WriteLine("HSLR-10 Satellite Observation Data(SOD) File");
                writer.WriteLine("");

                // [Operation Info]
                writer.WriteLine(";Operation Info");
                writer.WriteLine(";Format (Name:Value)");
                writer.WriteLine("BeginDateTime:" + begin_dateTime.ToString("yyyy-MM-dd") + "T" + begin_dateTime.ToString("HH:mm:ss"));
                writer.WriteLine("EndDateTime:" + end_dateTime.ToString("yyyy-MM-dd") + "T" + end_dateTime.ToString("HH:mm:ss"));
                writer.WriteLine("ObservationDataCount:" + observation_data.Count.ToString("D1"));
                writer.WriteLine("");

                // [Predict Info]
                writer.WriteLine(";Predict Info");
                writer.WriteLine(";Format (Name:Value)");
                writer.WriteLine("SatelliteName:" + satellite_name);
                writer.WriteLine("PredictionCenter:" + prediction_center);
                writer.WriteLine("COSPARNo:" + cospar_id.ToString());
                writer.WriteLine("PredictStartDateTime:" + predicStart_dateTime.ToString("yyyy-MM-ddTHH:mm:ss"));
                writer.WriteLine("PredictStartDayOfYear:" + predictStart_dayOfYear.ToString());
                writer.WriteLine("PredictInterval:" + predict_interval.ToString());
                writer.WriteLine("PredictCount:" + predict_count.ToString());
                writer.WriteLine("");

                // [Predict Data]
                writer.WriteLine(";Predict Data");
                writer.WriteLine(";format(PredictData:Time:X:Y:Z:Range(km/1way)");
                foreach (Tuple<DateTime, double, double, double, double> data in predict_data)
                {
                    writer.WriteLine("PredictData" + ":" + data.Item1.ToString("HHmmss") + ":" + 
                        data.Item2.ToString("F6") + ":" + data.Item3.ToString("F6") + ":" + data.Item4.ToString("F6") + ":" + data.Item5.ToString("F6"));
                }
                writer.WriteLine("");

                // [Observation Info]
                writer.WriteLine(";Observation Info");
                writer.WriteLine(";Format (Name:Value)");
                writer.WriteLine("Temperature_pre:" + temperature_pre.ToString("0.000"));
                writer.WriteLine("Pressure_pre:" + pressure_pre.ToString("0.000"));
                writer.WriteLine("Temperature_post:" + temperature_post.ToString("0.000"));
                writer.WriteLine("Pressure_post:" + pressure_post.ToString("0.000"));
                writer.WriteLine("");

                // [Observation Data]
                writer.WriteLine(";Observation Data");
                writer.WriteLine(";Format (Type:Epoch:TimeOfFlight)");
                writer.WriteLine(";Type[1:Identified/0:Noise/-9:Garbage]");
                writer.WriteLine(";Epoch[Seconds Of Day]");
                writer.WriteLine(";TimeOfFlight[2Way][ms]");
                foreach (Tuple<int, double, double> data in observation_data)
                {
                    writer.WriteLine(data.Item1.ToString("D1") + ":" + data.Item2.ToString("F12") + ":" + data.Item3.ToString("F9"));
                }
                writer.WriteLine("");

            }

            return true;
        }


        private const double SpeedOfLight = 299792458.0;    // 단위 : m/s
        private const double Bin_Size = 60.0;    // 단위 : 초
        public List<NormalPointOutput> ConvertToNormalPointFormat(List<Tuple<int, double, double, double, double>> origin_data)
        {
            List<NormalPointOutput> normalPoint_group = new List<NormalPointOutput>();

            if (origin_data == null || origin_data.Count == 0)
            {
                MessageBox.Show("관측 데이터 존재하지 않음.");
                return normalPoint_group;
            }

            double firstTime = origin_data.First().Item2;
            double lastTime = origin_data.Last().Item2;

            for (double binStart = firstTime; binStart <= lastTime; binStart += Bin_Size)
            {
                double binEnd = binStart + Bin_Size;

                var total_binData = origin_data.Where(t => t.Item2 >= binStart && t.Item2 < binEnd).ToList();
                var valid_binData = total_binData.Where(t => t.Item1 == 1).ToList();

                if (valid_binData.Count == 0) { continue; }

                var range = valid_binData.Select(t => (SpeedOfLight * t.Item3) / 2.0).ToList();

                double average_secondsOfDay = valid_binData.Average(t => t.Item2);
                double average_range = range.Average();
                double rms = Math.Sqrt(range.Sum(r => Math.Pow(r - average_range, 2)) / range.Count);

                normalPoint_group.Add(new NormalPointOutput
                {
                    TotalCount = total_binData.Count,
                    ValidCount = valid_binData.Count,
                    AverageSecondsOfDay = average_secondsOfDay,
                    AverageRange = average_range,
                    RMS = rms
                });
            }

            return normalPoint_group;
        }

    }

    public class NormalPointOutput
    {
        public int TotalCount { get; set; }
        public int ValidCount { get; set; }
        public double AverageSecondsOfDay { get; set; }
        public double AverageRange { get; set; }
        public double RMS { get; set; }
    }


}
