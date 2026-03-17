using Emgu.CV.Tiff;
using SGPdotNET.TLE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl.OrbitData
{
    internal class TLE_Reader
    {
        private List<TLE> tleList;
        public bool isCompleteListing { get; set; } = false; 

        public TLE_Reader()
        {
            ReadTleFromFile();
        }

        public List<TLE> TleList  { get { return tleList; }  }

        private void ReadTleFromFile()
        {
            string TLE_Path = System.IO.Path.Combine(Application.StartupPath, "active.txt");

            tleList = new List<TLE>();

            try
            {
                string[] lines = File.ReadAllLines(TLE_Path);

                for (int i = 0; i < lines.Length; i += 3)
                {
                    if (i + 2 < lines.Length)
                    {
                        TLE tle = new TLE
                        {
                            SatName = lines[i], 
                            Line1   = lines[i + 1],
                            Line2   = lines[i + 2]

                        };

                        tleList.Add(tle);
                    }
                }
                isCompleteListing = true;
                //Console.WriteLine($"TLE List >> {string.Join("\n", tleList)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading TLE file: {ex.Message}");
                isCompleteListing = false;
            }
        }

    }


    public class TLE
    {
        public string SatName { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }

        public float tilt_degree {  get; set; } 

        public override string ToString()
        {
            //return $"[ {SatName.TrimEnd(' ')} ] {Line1} / {Line2}]\n";
            return $"[ {SatName} ] [고각 {tilt_degree}]  {Line1} / {Line2}]\n";
        }
    }


}
