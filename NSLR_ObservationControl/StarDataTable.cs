using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSLR_ObservationControl
{
    public class StarDataTable
    {
        public List<ObservationEntry> _entries;
        public List<ObservationEntry2> _entries2;
        public StarDataTable()
        {
            _entries = new List<ObservationEntry>();
            _entries2 = new List<ObservationEntry2>();
        }

        public void AddEntry(double timestamp, int HIPnumber, double az, double el, double azOffset, double elOffset)
        {
            _entries.Add(new ObservationEntry {Timestamp = timestamp, HIPnumber = HIPnumber, Az = az, El = el, AzOffset = azOffset, ElOffset = elOffset });
        }
        public void AddEntry(double az, double el, double Obaz, double Obel)
        {
            _entries2.Add(new ObservationEntry2 {Az = az, El = el, ObAz = Obaz, ObEl = Obel });
        }

        public interface IClearable
        {
            void ClearData();
        }
        public void SaveToFile()
        {
            string directoryPath = Path.Combine(Application.StartupPath, "StarDataSet");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("{0,-10}{1,-11}{2,-20}{3,-20}{4,-10}{5,-10}","Timestmap","HIPnumber","Az","El","AzOffset","ElOffset");
                foreach (var entry in _entries)
                {
                       writer.WriteLine("{0,-10}{1,-11}{2,-20:F6}{3,-20:F6}{4,-10}{5,-10}", entry.Timestamp,entry.HIPnumber,entry.Az,entry.El,entry.AzOffset,entry.ElOffset);
                }
            }           
            string filePath2 = Path.Combine(directoryPath, "ModelingFormat " + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".dat");
            using (StreamWriter writer = new StreamWriter(filePath2))
            {
                writer.WriteLine(Path.GetFileName(filePath2));
                writer.WriteLine();
                writer.WriteLine("35 35 24.36");
                foreach (var entry in _entries2)
                {
                    writer.WriteLine($"{entry.Az}, {entry.El}, {entry.ObAz}, {entry.ObEl}");
        }
            }
        }

        public List<ObservationEntry> GetEntries()
        {
            return _entries;
        }

        public class ObservationEntry
        {
            public double Timestamp { get; set; }
            public int HIPnumber {  get; set; }
            public double Az { get; set; }
            public double El { get; set; }
            public double AzOffset { get; set; }
            public double ElOffset { get; set; }
        }
        public class ObservationEntry2
        {
            public double Az { get; set; }
            public double El { get; set; }
            public double ObAz { get; set; }
            public double ObEl { get; set; }
        }
    }
}
