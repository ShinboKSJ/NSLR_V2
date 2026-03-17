using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Controls;

namespace NSLR_ObservationControl.Module
{
    public partial class ConfigSetting : System.Windows.Forms.UserControl
    {

        private Dictionary<string, Dictionary<string, double>> targets
            = new Dictionary<string, Dictionary<string, double>>();

        public ConfigSetting()
        {
            InitializeComponent();
            LoadTargets("targets.txt");
            ApplyToLabels();

            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox2.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox3.SelectionAlignment = HorizontalAlignment.Center;
            richTextBox4.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void LoadTargets(string filePath)
        {
            targets.Clear();
            if (!File.Exists(filePath))
                return;

            string currentSection = null;

            foreach (var line in File.ReadAllLines(filePath))
            {
                string trimmed = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("#"))
                    continue;

                if (trimmed.StartsWith("[") && trimmed.EndsWith("]"))
                {
                    currentSection = trimmed.Trim('[', ']');
                    targets[currentSection] = new Dictionary<string, double>();
                }
                else if (currentSection != null)
                {
                    var parts = trimmed.Split('=');
                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim();
                        if (double.TryParse(parts[1].Trim(), out double value))
                        {
                            targets[currentSection][key] = value;
                        }
                    }
                }
            }
        }

        private void ApplyToLabels()
        {
            if (targets.ContainsKey("SatelliteTarget"))
            {
                var sat = targets["SatelliteTarget"];
                label58.Text = sat["Azimuth"].ToString();
                label60.Text = sat["Elevation"].ToString();
                label63.Text = sat["Range"].ToString();
            }

            if (targets.ContainsKey("SpaceObjectTarget"))
            {
                var obj = targets["SpaceObjectTarget"];
                label64.Text = obj["Azimuth"].ToString();
                label66.Text = obj["Elevation"].ToString();
                label68.Text = obj["Range"].ToString();
            }
        }


        public string ShowFileOpenDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "파일 오픈 예제창";
            ofd.FileName = "test";
            ofd.Filter = "그림 파일 (*.jpg, *.gif, *.bmp) | *.jpg; *.gif; *.bmp; | 모든 파일 (*.*) | *.*";

            DialogResult dr = ofd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                string fileName = ofd.SafeFileName;
                string fileFullName = ofd.FileName;
                string filePath = fileFullName.Replace(fileName, "");

                label1.Text = "File Name  : " + fileName;
                label2.Text = "Full Name  : " + fileFullName;
                label3.Text = "File Path  : " + filePath;
                return fileFullName;
            }
            else if (dr == DialogResult.Cancel)
            {
                return "";
            }
            return "";
        }


        private void directory_observeResult_Click(object sender, EventArgs e)
        {
            ShowFileOpenDialog();

        }

        private void ConfigSetting_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
  
    }

        private void button2_Click(object sender, EventArgs e)
        {
            if (double.TryParse(richTextBox1.Text, out double az) && double.TryParse(richTextBox2.Text, out double el))
            {
                SystemSettings.SaveSLRValues(az, el);
            }
            else
            {
                MessageBox.Show("SLR 값이 올바르지 않습니다. 숫자를 입력하세요.");
            }

            if (double.TryParse(richTextBox3.Text, out double dlt1) && double.TryParse(richTextBox4.Text, out double dlt2))
            {
                SystemSettings.SaveDLTValues(dlt1, dlt2);
            }
            else
            {
                MessageBox.Show("DLT 값이 올바르지 않습니다. 숫자를 입력하세요.");
            }
        }
    }
    public class TargetPosition
    {
        public string Type { get; set; }   
        public double Azimuth { get; set; }
        public double Elevation { get; set; }
        public double Range { get; set; }
    }
    public static class SystemSettings
    {
        public static event Action SLRValuesChanged;
        public static event Action DLTValuesChanged;
/*        public static void SaveSLRValues(double az, double el)
        {
            Properties.Settings.Default.SLR_AzValue = az;
            Properties.Settings.Default.SLR_ElValue = el;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            SLRValuesChanged?.Invoke();
        }

        public static void SaveDLTValues(double az, double el)
        {
            Properties.Settings.Default.DLT_AzValue = az;
            Properties.Settings.Default.DLT_ElValue = el;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
            DLTValuesChanged?.Invoke();
        }

        public static (double Az, double El) GetSLRValues()
        {
            return (Properties.Settings.Default.SLR_AzValue,
                    Properties.Settings.Default.SLR_ElValue);
        }

        public static (double Az, double El) GetDLTValues()
        {
            return (Properties.Settings.Default.DLT_AzValue,
                    Properties.Settings.Default.DLT_ElValue);
        }*/
        public static void SaveSLRValues(double az, double el)
        {
            var s = Properties.Settings.Default;
            s.SLR_AzValue = az;
            s.SLR_ElValue = el;

            s.Reload();

            SLRValuesChanged?.Invoke();
        }

        public static void SaveDLTValues(double az, double el)
        {
            var s = Properties.Settings.Default;
            s.DLT_AzValue = az;
            s.DLT_ElValue = el;

            s.Reload();

            DLTValuesChanged?.Invoke();
        }

        public static (double Az, double El) GetSLRValues()
        {
            var s = Properties.Settings.Default;
            return (s.SLR_AzValue, s.SLR_ElValue);
        }

        public static (double Az, double El) GetDLTValues()
        {
            var s = Properties.Settings.Default;
            return (s.DLT_AzValue, s.DLT_ElValue);
        }
    }
}
