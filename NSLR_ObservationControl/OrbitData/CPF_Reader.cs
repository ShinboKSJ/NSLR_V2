using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl.OrbitData
{
    internal class CPF_Reader
    {
        public bool isCompleteListing { get; set; } = false;

        public CPF_Reader()
        {
          
        }

        public bool CPF_listup()
        {
            // 특정 디렉토리 경로 설정
            string CPF_Path = Application.StartupPath;

            // DataSource 객체 생성
            DataSource dataSource = new DataSource();

            // 디렉토리에 있는 모든 파일을 읽어들여 구조화된 데이터 얻기
            List<CPFData> cpfDataList = dataSource.ReadCPFFiles(CPF_Path);

            // 구조화된 데이터 출력
            foreach (var cpfData in cpfDataList)
            {
                Console.WriteLine(cpfData);
            }
            return true;

        }
    }


    public class DataSource
    {
        public List<CPFData> ReadCPFFiles(string directoryPath)
        {
            List<CPFData> cpfDataList = new List<CPFData>();

            try
            {
                // 지정된 디렉토리의 모든 파일 경로 얻기
                string[] filePaths = Directory.GetFiles(directoryPath);

                foreach (var filePath in filePaths)
                {
                    CPFData cpfData = ReadCPFFile(filePath);
                    if (cpfData != null)
                    {
                        cpfDataList.Add(cpfData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading files in directory '{directoryPath}': {ex.Message}");
            }

            return cpfDataList;
        }

        private CPFData ReadCPFFile(string filePath)
        {
            try
            {
                // 파일에서 필요한 정보를 읽어옴 (파싱 코드는 실제 CPF 파일의 형식에 맞게 수정 필요)
                DateTime timestamp = DateTime.Now; // 예시로 현재 시간 사용
                Vector3 position = new Vector3(0.0f, 0.0f, 0.0f); // 예시 위치
                Vector3 velocity = new Vector3(1.0f, 2.0f, 0.5f); // 예시 속도

                // CPFData 객체 생성
                CPFData cpfData = new CPFData(timestamp, position, velocity);

                return cpfData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading CPF file '{filePath}': {ex.Message}");
                return null;
            }
        }
    }

    public class CPFData
    {
        int formatVersion;
        string producer;
        int[] productionTime = new int[4];

        int[] stationCode = new int[3];
        int stationTimeScale;           // 0:  UTC(USNO), 1: UTC(GPS), 7: UTC(BIPM), 1-2,5-6,8-9: obsolete time scales, >10: UTC(station time scales)

        string targetName;
        int[] targetID = new int[3];                // [0] ILRS, [1] SIC, [2] NORAD
        int targetTimeScale;            // 0: not used, 1: UTC, 2: SC time scale
        int targetClass;                // 0: debris, 1: passive ref., 2: DO NOT USE, 3: sync.trans., 4: async.trans., 4: other

        double[] startingDate = new double[6];
        double[] endingDate = new double[6];
        double stepSize;

        public DateTime Timestamp { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }

        public CPFData(DateTime timestamp, Vector3 position, Vector3 velocity)
        {
            Timestamp = timestamp;
            Position = position;
            Velocity = velocity;
        }

        public override string ToString()
        {
            return $"Timestamp: {Timestamp}, Position: {Position}, Velocity: {Velocity}";
        }
    }   

}
