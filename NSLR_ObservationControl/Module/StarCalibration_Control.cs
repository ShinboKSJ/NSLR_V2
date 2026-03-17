using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NSLR_ObservationControl.StarDataTable;
using static NSLR_ObservationControl.CSU_StarCalibration;
using static NSLR_ObservationControl.Module.Observation_TMS;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace NSLR_ObservationControl.Module
{
    public partial class StarCalibration_Control : UserControl
    {

        public static NSLR_ObservationControl.StarDataTable dataTable = new NSLR_ObservationControl.StarDataTable();

        TaskControl taskControl = TaskControl.instance;
        public StarCalibration_Control()
        {
            InitializeComponent();
        }
        public static int hipnumber_temp;
        int hipnumber;
        double timestamp;
        private void StarcalStart_Click(object sender, EventArgs e)
        {
            taskControl.SetMutex("Star_Calibration");

            StarCalInfo.StarCalFlag = true;
            StarCalInfo.StarStartFlag = true;
            starStep = StarCalInfo.Step.One;
            hipnumber = hipnumber_temp; 
            operatingMode = TrackingMount.ReadyState;
        }
        private void NextStar_Click(object sender, EventArgs e)
        {
            dataTable._entries.Add(new ObservationEntry {Timestamp = Observation_TMS.Timestamp[3]+10 , HIPnumber=hipnumber, Az = tcspk.targetposition[0], El = tcspk.targetposition[1], AzOffset = CSU_StarCalibration.AddAzOffSet, ElOffset = CSU_StarCalibration.AddElOffSet });
            dataTable._entries2.Add(new ObservationEntry2 { Az = tcspk.targetposition[0], El = tcspk.targetposition[1], ObAz = tcspk.targetposition[0]+ CSU_StarCalibration.AddAzOffSet, ObEl = tcspk.targetposition[1] + CSU_StarCalibration.AddElOffSet });
            
            operatingMode = TrackingMount.InitEndState;
            controlCommand = TrackingMount.ServoDisable;
            starStep = StarCalInfo.Step.Three;
            CSU_StarCalibration.AddAzOffSet = 0;
            CSU_StarCalibration.AddElOffSet = 0;
            azUserOffset = 0;
            elUserOffset = 0;
            MessageBox.Show("다음 별을 선택해주세요.");
        }

        private void StarCalEnd_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("저장하시겠습니까?", "데이터 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                dataTable.SaveToFile();

                operatingMode = TrackingMount.InitEndState;
                controlCommand = TrackingMount.ServoDisable;
                starStep = StarCalInfo.Step.Three;
                StarCalInfo.StarCalFlag = false;
                StarCalInfo.StarStartFlag = true;
            }
            else if (result == DialogResult.No)
            {

            }
            
            taskControl.SetMutex("None");
        }
        public class CalibrationData
        {
            public double Ra { get; set; }
            public double Dec { get; set; }
            public double AzOffSet { get; set; }
            public double ElOffSet { get; set; }

            public static List<CalibrationData> calibrationDataList = new List<CalibrationData>();
            public override string ToString()
            {
                return $"{Ra},{Dec},{AzOffSet},{ElOffSet}";
            }

            public static CalibrationData FromString(string dataString)
            {
                var parts = dataString.Split(',');
                return new CalibrationData
                {
                    Ra = double.Parse(parts[0]),
                    Dec = double.Parse(parts[1]),
                    AzOffSet = double.Parse(parts[2]),
                    ElOffSet = double.Parse(parts[3]),
                };
            }
            public static void SaveCalibrationData(string filePath)
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (var data in calibrationDataList)
                    {
                        writer.WriteLine(data.ToString());
                    }
                }
            }

            public static void LoadCalibrationData(string filePath)
            {
                calibrationDataList.Clear();
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        calibrationDataList.Add(CalibrationData.FromString(line));
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog
            {
                SelectedPath = Application.StartupPath 
            };

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string strPath = fbd.SelectedPath;
                string[] supportedExtensions = { ".hwp", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf" ,".txt" };

                List<string> files = new List<string>();

                foreach (string fileFullName in Directory.GetFiles(strPath, "*.*", SearchOption.AllDirectories))
                {
                    if (supportedExtensions.Contains(Path.GetExtension(fileFullName).ToLower()))
                    {
                        files.Add(fileFullName);
                    }
                }

                listBoxFiles.Items.Clear();
                listBoxFiles.Items.AddRange(files.ToArray());
            }
        }

        /*   private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "데이터 파일 선택";
            openFileDialog.Filter = "TPoint 데이터 파일 (*.txt;*)|*.txt;*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFile = openFileDialog.FileName;
                string exePath = Path.Combine(Application.StartupPath, "tpointtest.exe");

                if (!File.Exists(exePath))
                {
                    MessageBox.Show("실행파일이 존재하지 않습니다.");
                    return;
                }

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = $"\"{selectedFile}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
            };

                Process proc = new Process { StartInfo = psi };

                List<string> outputLines = new List<string>();
                bool captureLines = false;

                proc.OutputDataReceived += (s, ea) =>
            {
                    if (string.IsNullOrEmpty(ea.Data))
                        return;

                    Console.WriteLine(ea.Data); 

                    if (ea.Data.Contains("coeff"))
                    {
                        captureLines = true;
                        outputLines.Clear();
                    }

                    if (captureLines)
                    {
                        outputLines.Add(ea.Data);
                    }

                    if (ea.Data.Contains("TPT 피팅 완료!"))
                {
                        captureLines = false;

                        string resultText = string.Join(Environment.NewLine, outputLines);
                        TPointCoefficients coeffs = ParseTPointCoefficients(resultText);
                        coeffsResult = coeffs;


                }
                };

                proc.Start();
                proc.BeginOutputReadLine();
                   proc.Close();
               }
           }*/
        private string FindExePath(string exeName)
        {
            string currentDir = Application.StartupPath;
            DirectoryInfo dir = new DirectoryInfo(currentDir);

            while (dir != null)
            {
                string candidate = Path.Combine(dir.FullName, exeName);
                if (File.Exists(candidate))
                    return candidate;

                dir = dir.Parent; 
            }

            return null;
        }
        private async void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "데이터 파일 선택";
            openFileDialog.Filter = "TPoint 데이터 파일 (*.txt;*)|*.txt;*";

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            string selectedFile = openFileDialog.FileName;


            string exePath = FindExePath("tpointtest.exe");

            if (exePath == null)
            {
                MessageBox.Show("tpointtest.exe 파일을 찾을 수 없습니다.");
                return;
            }/*
            string exePath = Path.Combine(Application.StartupPath, "tpointtest.exe");

            if (!File.Exists(exePath))
            {
                MessageBox.Show("실행파일이 존재하지 않습니다.");
                return;
            }*/

            Process proc = null;

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = $"\"{selectedFile}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true, 
                    CreateNoWindow = true
                };

                proc = new Process { StartInfo = psi };

                List<string> outputLines = new List<string>();
                bool captureLines = false;
                var tcs = new TaskCompletionSource<bool>();

                proc.OutputDataReceived += (s, ea) =>
                {
                    if (string.IsNullOrEmpty(ea.Data)) return;

                    Console.WriteLine(ea.Data);
                    if (ea.Data.Contains("coeff"))
                    {
                        captureLines = true;
                        outputLines.Clear();
                    }

                    if (captureLines)
                    {
                        outputLines.Add(ea.Data);
                    }

                    if (ea.Data.Contains("TPT 피팅 완료!"))
                    {
                        captureLines = false;
                        string resultText = string.Join(Environment.NewLine, outputLines);
                        coeffsResult = ParseTPointCoefficients(resultText);
                        tcs.TrySetResult(true);
                    }
                };

                proc.ErrorDataReceived += (s, ea) =>
                {
                    if (!string.IsNullOrEmpty(ea.Data))
                    {
                        Console.WriteLine("[ERR] " + ea.Data);
                    }
                };

                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();

                var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(30000));

                if (completedTask != tcs.Task)
                {
                    MessageBox.Show("프로세스 실행 시간 초과", "타임아웃",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (!proc.HasExited)
                {
                    proc.Kill();
                    proc.WaitForExit(5000);
                }

                MessageBox.Show("계수 추출 완료!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"프로세스 실행 중 오류 발생: {ex.Message}", "오류",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (proc != null)
                {
                    try
                    {
                        if (!proc.HasExited)
                        {
                            proc.Kill();
                            proc.WaitForExit(5000);
                        }

                        proc.Close();
                        proc.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"프로세스 정리 중 오류: {ex.Message}");
                    }
                }
            }
        }
        /* private async void button5_Click(object sender, EventArgs e)
         {
             OpenFileDialog openFileDialog = new OpenFileDialog();
             openFileDialog.Title = "데이터 파일 선택";
             openFileDialog.Filter = "TPoint 데이터 파일 (*.txt;*)|*.txt;*";

             if (openFileDialog.ShowDialog() != DialogResult.OK)
                 return;

             string selectedFile = openFileDialog.FileName;
             string exePath = Path.Combine(Application.StartupPath, "tpointtest.exe");

             if (!File.Exists(exePath))
             {
                 MessageBox.Show("실행파일이 존재하지 않습니다.");
                 return;
             }

             ProcessStartInfo psi = new ProcessStartInfo
             {
                 FileName = exePath,
                 Arguments = $"\"{selectedFile}\"",
                 UseShellExecute = false,
                 RedirectStandardOutput = true,
                 RedirectStandardError = true,
                 CreateNoWindow = true
             };

             Process proc = new Process { StartInfo = psi };

             List<string> outputLines = new List<string>();
             bool captureLines = false;

             var tcs = new TaskCompletionSource<bool>();

             proc.OutputDataReceived += (s, ea) =>
             {
                 if (string.IsNullOrEmpty(ea.Data)) return;

                 Console.WriteLine(ea.Data);

                 if (ea.Data.Contains("coeff"))
                 {
                     captureLines = true;
                     outputLines.Clear();
             }

                 if (captureLines)
                 {
                     outputLines.Add(ea.Data);
                 }

                 if (ea.Data.Contains("TPT 피팅 완료!"))
                 {
                     captureLines = false;

                     string resultText = string.Join(Environment.NewLine, outputLines);
                     coeffsResult = ParseTPointCoefficients(resultText);

                     // 종료 신호
                     tcs.TrySetResult(true);
                 }
             };

             proc.ErrorDataReceived += (s, ea) =>
             {
                 if (!string.IsNullOrEmpty(ea.Data))
                 {
                     Console.WriteLine("[ERR] " + ea.Data);
         }
             };

             proc.Start();
             proc.BeginOutputReadLine();
             proc.BeginErrorReadLine();

             await tcs.Task;

             if (!proc.HasExited)
             {
                 proc.Kill();
             }
             proc.Close();

             MessageBox.Show("계수 추출 완료!");
         }*/

        TPointCoefficients coeffsResult = new TPointCoefficients();
        public static class sync 
        {
            public static double Azsync { get; set; }
            public static double Elsync { get; set; }
        }

        private void Sync_Click(object sender, EventArgs e)
        {
            sync.Azsync = CSU_StarCalibration.AddAzOffSet;
            sync.Elsync = CSU_StarCalibration.AddElOffSet;
            CSU_StarCalibration.AddAzOffSet = 0;
            CSU_StarCalibration.AddElOffSet = 0;

        }
        public struct TPointCoefficients
        {
            public double IA;
            public double IE;
            public double NPAE;
            public double CA;
            public double AN;
            public double AW;
            public double ACES;
            public double ACEC;
            public double ECES;
            public double ECEC;
        }
        private TPointCoefficients ParseTPointCoefficients(string tptResult)
        {
            var coeffs = new TPointCoefficients();

            string pattern = @"\b(IA|IE|NPAE|CA|AN|AW|ACES|ACEC|ECES|ECEC)\b\s+[-+]?[\d\.]+\s+([-+]?[\d\.]+)";
            var matches = Regex.Matches(tptResult, pattern);

            foreach (Match match in matches)
            {
                string param = match.Groups[1].Value;
                string valueStr = match.Groups[2].Value;
                if (double.TryParse(valueStr, out double value))
                {
                    switch (param)
                    {
                        case "IA": coeffs.IA = value; break;
                        case "IE": coeffs.IE = value; break;
                        case "NPAE": coeffs.NPAE = value; break;
                        case "CA": coeffs.CA = value; break;
                        case "AN": coeffs.AN = value; break;
                        case "AW": coeffs.AW = value; break;
                        case "ACES": coeffs.ACES = value; break;
                        case "ACEC": coeffs.ACEC = value; break;
                        case "ECES": coeffs.ECES = value; break;
                        case "ECEC": coeffs.ECEC = value; break;
                    }
                }
            }
            return coeffs;
        }

        private void coeff_Click(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                Console.WriteLine($"---IA: {coeffsResult.IA:F2}");
                Console.WriteLine($"---IE: {coeffsResult.IE:F2}");
                Console.WriteLine($"---NPAE: {coeffsResult.NPAE:F2}");
                Console.WriteLine($"---CA: {coeffsResult.CA:F2}");
                Console.WriteLine($"---AN: {coeffsResult.AN:F2}");
                Console.WriteLine($"---AW: {coeffsResult.AW:F2}");
                Console.WriteLine($"---ACES: {coeffsResult.ACES:F2}");
                Console.WriteLine($"---ACEC: {coeffsResult.ACEC:F2}");
                Console.WriteLine($"---ECES: {coeffsResult.ECES:F2}");
                Console.WriteLine($"---ECEC: {coeffsResult.ECEC:F2}");
            });
        }
    }
}
