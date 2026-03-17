using PvDotNet;
using SGPdotNET.Observation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NSLR_ObservationControl
{
    public class Test_SatelliteAZEL
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateConfiguration();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateConstants();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateGroundSite(double[] loc, StringBuilder siteName, double waveLength);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateTimeSystem();
        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateTLE();
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetTLEData(IntPtr tleClass, StringBuilder line1, StringBuilder line2, double[] epochTime);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ConvertMJD2Greg(IntPtr timeSystem, double mjd, double[] greg);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double ConvertGreg2MJD(IntPtr timeSystem, double[] greg);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetSiteLocation(IntPtr obsSite, double[] loc);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GenerateCommandFile_TLE(IntPtr tleClass, double[] siteLoc, double startMJD, double finalMJD, double stepSize, StringBuilder fileName, int opt);
        [DllImport("NSLR-OAS.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GenerateCommand_TLE(IntPtr tleClass, double[] siteLoc, double targetMJD, int opt, double[] command);

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        public IntPtr tleClass;
        Dictionary<string, TLE> TLE_Information = new Dictionary<string, TLE>();
        string recentTLE_directory = "./TLE";
        string recentTLE_file = "recent_tle.txt";

        public Test_SatelliteAZEL()
        {
            // 궤도 라이브러리 객체 Create
            Global.config = CreateConfiguration();
            //Global.solarSystem = CreateSolarSystem();
            Global.OASCONST = CreateConstants();
            //Global.dynModel = CreateOrbitDynamics();
            //Global.sat = CreateSatellite();
            double[] siteLoc = { 127.9192, 35.5901, 0.923302744 };
            StringBuilder siteName = new StringBuilder("Geochang");
            double waveLength = 532;
            Global.laserSite = CreateGroundSite(siteLoc, siteName, waveLength);
            //Global.meaModel = CreateMeasurementModel();
            //Global.estClass = CreateEstimator();
            //Global.paramClass = CreateParameters(Global.sat);
            Global.timeSys = CreateTimeSystem();
            //Global.coordSys = CreateCoordinateSystem();
            //Global.residual = CreateResidual();

            // TLE 객체 생성
            tleClass = CreateTLE();

            // TLE Reader 쓰레드 시작
            System.Threading.Thread TLE_thread = new System.Threading.Thread(URL_TLE_Reader);
            //TLE_thread.Start();
            //TLE_thread.Join();
        }

        ~Test_SatelliteAZEL()
        {

        }

        public void Test_Example()
        {
            // Data table File 생성(LAGEOS 1)
            double[] startTime = { 2024, 8, 20, 19, 0, 0.000000 };
            double[] finalTime = { 2024, 8, 20, 19, 0, 1.000000 };
            GenerateFile_AZEL("LAGEOS 1", startTime, finalTime);

            // Get Only AZEL Data(LAGEOS 1)
            double[] AZEL_result = GetData_AZEL("LAGEOS 1");
            System.Windows.Forms.MessageBox.Show("Azimuth(deg) : " + AZEL_result[0] + " / Elevation : " + AZEL_result[1]);
            double[] AZEL_result2 = GetData_AZEL("LAGEOS 1");
            System.Windows.Forms.MessageBox.Show("Azimuth(deg) : " + AZEL_result2[0] + " / Elevation : " + AZEL_result2[1]);
            double[] AZEL_result3 = GetData_AZEL("LAGEOS 1");
            System.Windows.Forms.MessageBox.Show("Azimuth(deg) : " + AZEL_result3[0] + " / Elevation : " + AZEL_result3[1]);
            double[] AZEL_result4 = GetData_AZEL("LAGEOS 1");
            System.Windows.Forms.MessageBox.Show("Azimuth(deg) : " + AZEL_result4[0] + " / Elevation : " + AZEL_result4[1]);
            double[] AZEL_result5 = GetData_AZEL("LAGEOS 1");
            System.Windows.Forms.MessageBox.Show("Azimuth(deg) : " + AZEL_result5[0] + " / Elevation : " + AZEL_result5[1]);


            // 코멘트(By 진석)
            // 추출되는 AZEL이 부정확하게(Epoch Time에서 멀어질 수록) 나오는 위성 있음!
            // 잘 선택해서 해야할 듯요
            // LAGEOS 1 >> 이거 추천 (Epoch 시점하고 멀어져도 잘나옴.

            // AZEL 값을 50Hz 주기로 뽑다보니 기간(StartTime ~ finalTime)을 길게 하면 데이터File 생성하는데 오래걸려요...
            // 고려해서 기간 설정이나 호출 시점 잡아야 할것 같애요. 생성하는데 Thread 써야할거 같구요.
            // 해당사항 고려하면 그냥 실시간으로 데이터값 받는데 나을거 같애요.
            // 필요하면 그 전에 Data Table 생성해서 NaV 값 나오는지 확인해야할 거같고요.

            // 일단 오늘 초안 보내볼테니 어떻게 사용할지만 대략적으로 구상하시고
            // 다시 얘기하면서 최적화 시켜야 할거 같애요,
        }

        public void URL_TLE_Reader()    // TLE 파일 최신본 업로드 및 TLE 정보 업데이트
        {
            // 프로그램 시작 시 TLE 파일 최신여부 Check (서버 액세스 거부 방지용)
            if (!Directory.Exists(recentTLE_directory))
            {
                System.Windows.Forms.MessageBox.Show("TLE 디렉토리 없음...");
                return;
            }

            FileInfo fileInfo = new FileInfo(recentTLE_directory + "/" + recentTLE_file);
            if (fileInfo.Exists)
            {

                if (((fileInfo.LastWriteTime).Year == DateTime.Now.Year) && ((fileInfo.LastWriteTime).Month == DateTime.Now.Month) && ((fileInfo.LastWriteTime).Day == DateTime.Now.Day))
                {
                    // TLE 정보만 업데이트
                    using (var tle_reader = new StreamReader(fileInfo.FullName, Encoding.UTF8))
                    {
                        while (!tle_reader.EndOfStream)
                        {
                            string name = tle_reader.ReadLine().Trim();
                            string line1 = tle_reader.ReadLine();
                            string line2 = tle_reader.ReadLine();

                            if (TLE_Information.ContainsKey(name) == false)
                            {
                                TLE_Information.Add(name, new TLE(line1, line2));
                            }
                        }
                    }

                    return;
                }

                fileInfo.Delete();
            }

            // TLE 파일 업로드 및 정보 업데이트
            string strURL;
            HttpWebRequest webRequest;
            HttpWebResponse webResponse;

            strURL = "https://celestrak.org/NORAD/elements/gp.php?GROUP=active&FORMAT=tle";
            webRequest = (HttpWebRequest)WebRequest.Create(strURL);
            webRequest.Method = "GET";
            webResponse = (HttpWebResponse)webRequest.GetResponse();

            using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
            {
                // 파일 생성 및 TLE 정보 불러오기
                using (StreamWriter writer = fileInfo.CreateText())
                {
                    while (!reader.EndOfStream)
                    {
                        string name = reader.ReadLine().Trim();
                        string line1 = reader.ReadLine();
                        string line2 = reader.ReadLine();

                        writer.WriteLine(name);
                        writer.WriteLine(line1);
                        writer.WriteLine(line2);

                        if (TLE_Information.ContainsKey(name) == false)
                        {
                            TLE_Information.Add(name, new TLE(line1, line2));
                        }

                    }
                }

            }

        }


        public void GenerateFile_AZEL(string satellite, double[] startTime/*Data 받을 시작 시각*/, double[] finalTime/*Data 받을 종료 시각*/)
        {
            if (TLE_Information.ContainsKey(satellite) == false)
            {
                System.Windows.Forms.MessageBox.Show("해당 위성에 대한 TLE 정보가 없습니다.");
                return;
            }

            // 위성에 맞는 TLE Searching
            StringBuilder line1 = new StringBuilder(TLE_Information[satellite].Line1);
            StringBuilder line2 = new StringBuilder(TLE_Information[satellite].Line2);

            // 위성에 대한 Epoch Time 산출 및 TLE 정보 입력
            double[] epochTime = new double[6];
            GetTLEData(tleClass, line1, line2, epochTime);

            // 참고 데이터 Setting
            double startMJD = ConvertGreg2MJD(Global.timeSys, startTime);
            double finalMJD = ConvertGreg2MJD(Global.timeSys, finalTime);
            double stepSize = 0.02;

            StringBuilder fileName = new StringBuilder(satellite + "_AZEL" + ".txt");

            double[] siteloc = new double[3];
            GetSiteLocation(Global.laserSite, siteloc);

            GenerateCommandFile_TLE(tleClass, siteloc, startMJD, finalMJD, stepSize, fileName, 0);

            System.Windows.Forms.MessageBox.Show("AZEL File is generated.");
        }
        double[] offset_data = { -0.07, -0.05, -0.03, -0.01, 0.01, 0.03, 0.05, 0.07 };
        public double[][] Temp_GetData_AZEL(string satellite, double currentUTCMillis)
        {
            double[][] positionData = new double[2][];

            if (TLE_Information.ContainsKey(satellite) == false)
            {
                System.Windows.Forms.MessageBox.Show("해당 위성에 대한 TLE 정보가 없습니다.");
                return positionData;
            }

            for (int i = 0; i < positionData.Length; i++) { positionData[i] = new double[offset_data.Length]; }

            // 위성에 맞는 TLE Searching
            StringBuilder line1 = new StringBuilder(TLE_Information[satellite].Line1);
            StringBuilder line2 = new StringBuilder(TLE_Information[satellite].Line2);

            // 위성에 대한 Epoch Time 산출 및 TLE 정보 입력
            double[] epochTime = new double[6];
            GetTLEData(tleClass, line1, line2, epochTime);

            ///////////////////////////////////////////////////////////////////////

            DateTime dateTime = new DateTime((long)currentUTCMillis);
            DateTime[] dateTimes = new DateTime[offset_data.Length];
            for (int i = 0; i < dateTimes.Length; i++)
            {
                dateTimes[i] = dateTime.AddSeconds(offset_data[i]);
            }

            double[] siteloc = new double[3];
            GetSiteLocation(Global.laserSite, siteloc);

            double currentMJD;
            double[] result = new double[2];
            for (int i = 0; i < dateTimes.Length; i++)
            {
                string[] split_time = (dateTimes[i].ToString("yyyy-MM-dd-HH-mm-ss.fff")).Split('-');
                double[] double_time = new double[6];
                for (int j = 0; j < double_time.Length; j++)
                {
                    double_time[j] = double.Parse(split_time[j]);
                }

                currentMJD = ConvertGreg2MJD(Global.timeSys, double_time);
                GenerateCommand_TLE(tleClass, siteloc, currentMJD, 0, result);

                positionData[0][i] = result[0];
                positionData[1][i] = result[1];
            }

            return positionData;

        }
        /*        double[] offset_data = { -0.07, -0.05, -0.03, -0.01, 0.01, 0.03, 0.05, 0.07 };
                public double[][] GetDatas_AZEL(string satellite, double currentUTCMillis)
                {
                    double[][] positionData = new double[2][];

                    if (TLE_Information.ContainsKey(satellite) == false)
                    {
                        System.Windows.Forms.MessageBox.Show("해당 위성에 대한 TLE 정보가 없습니다.");
                        return positionData;
                    }

                    for (int i = 0; i < positionData.Length; i++) { positionData[i] = new double[offset_data.Length]; }

                    // 위성에 맞는 TLE Searching
                    StringBuilder line1 = new StringBuilder(TLE_Information[satellite].Line1);
                    StringBuilder line2 = new StringBuilder(TLE_Information[satellite].Line2);

                    // 위성에 대한 Epoch Time 산출 및 TLE 정보 입력
                    double[] epochTime = new double[6];
                    GetTLEData(tleClass, line1, line2, epochTime);


                    DateTime dateTime = new DateTime((long)currentUTCMillis);
                    string[] split_time = (dateTime.ToString("yyyy-MM-dd-HH-mm-ss.fff")).Split('-');
                    double[] current_time = new double[6];
                    for (int i = 0; i < current_time.Length; i++)
                    {
                        current_time[i] = double.Parse(split_time[i]);
                    }

                    double[] siteloc = new double[3];
                    GetSiteLocation(Global.laserSite, siteloc);

                    double currentMJD;
                    double[] result = new double[2];
                    for (int i = 0; i < offset_data.Length; i++)
                    {
                        current_time[5] += offset_data[i];
                        currentMJD = ConvertGreg2MJD(Global.timeSys, current_time);
                        GenerateCommand_TLE(tleClass, siteloc, currentMJD, 0, result);

                        positionData[0][i] = result[0];
                        positionData[1][i] = result[1];

                        current_time[5] -= offset_data[i];
                    }

                    return positionData;
                }*/

        // Only 한 시점에 대한 AZEL Data 리턴
        public double[] GetData_AZEL(string satellite)
        {
            double[] result = new double[2];

            if (TLE_Information.ContainsKey(satellite) == false)
            {
                System.Windows.Forms.MessageBox.Show("해당 위성에 대한 TLE 정보가 없습니다.");
                return result;
            }

            // 위성에 맞는 TLE Searching
            StringBuilder line1 = new StringBuilder(TLE_Information[satellite].Line1);
            StringBuilder line2 = new StringBuilder(TLE_Information[satellite].Line2);

            // 위성에 대한 Epoch Time 산출 및 TLE 정보 입력
            double[] epochTime = new double[6];
            GetTLEData(tleClass, line1, line2, epochTime);

            // 참고 데이터 Setting
            string str_time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.ffffff");
            string[] split_time = str_time.Split('-');
            double[] current_time = new double[6];
            for (int i = 0; i < current_time.Length; i++)
            {
                current_time[i] = double.Parse(split_time[i]);
            }
            double currentMJD = ConvertGreg2MJD(Global.timeSys, current_time);

            double[] siteloc = new double[3];
            GetSiteLocation(Global.laserSite, siteloc);

            GenerateCommand_TLE(tleClass, siteloc, currentMJD, 0, result);

            return result;
        }



    }

    // 각 위성, TLE 정보 저장 Class
    /*
    public class TLE
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }

        public TLE(string line1, string line2)
        {
            Line1 = line1;
            Line2 = line2;
        }
    }
    */
}
