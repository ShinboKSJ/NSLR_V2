using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace NSLR_ObservationControl.Module
{
    public partial class debuggingPlot : UserControl
    {
        public debuggingPlot()
        {
            InitializeComponent();
            LoadAndPlotData();
            Setting();
        }
        private void Setting()
        {
            chartAzimuth.MouseWheel += Chart_MouseWheel;
            chartElevation.MouseWheel += Chart_MouseWheel;

            this.HandleDestroyed += debuggingPlot_HandleDestroyed;
        }
        private void Chart_MouseWheel(object sender, MouseEventArgs e)
        {
            var chart = sender as Chart;
            if (chart == null) return;

            double zoomFactor = 2.0; 

            if (e.Delta > 0)
            {
                ZoomChart(chart, zoomFactor);
            }
            else if (e.Delta < 0)
            {
                ZoomChart(chart, 1 / zoomFactor);
            }
        }
        private void ZoomChart(Chart chart, double zoomFactor)
        {
            var chartArea = chart.ChartAreas[0];

            double xMin = chartArea.AxisX.ScaleView.ViewMinimum;
            double xMax = chartArea.AxisX.ScaleView.ViewMaximum;
            double xCenter = (xMin + xMax) / 2;
            double xRange = (xMax - xMin) / zoomFactor;
            chartArea.AxisX.ScaleView.Zoom(xCenter - xRange / 2, xCenter + xRange / 2);

            double yMin = chartArea.AxisY.ScaleView.ViewMinimum;
            double yMax = chartArea.AxisY.ScaleView.ViewMaximum;
            double yCenter = (yMin + yMax) / 2;
            double yRange = (yMax - yMin) / zoomFactor;
            chartArea.AxisY.ScaleView.Zoom(yCenter - yRange / 2, yCenter + yRange / 2);
        }
        private void LoadAndPlotData()
        {
            if (!File.Exists("parsed_data.txt"))
            {
                MessageBox.Show("parsed_data.txt 파일이 존재하지 않습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] lines = File.ReadAllLines("parsed_data.txt");
            List<double> timestamps = new List<double>();
            List<List<double>> azimuthDataLists = new List<List<double>>();
            List<List<double>> elevationDataLists = new List<List<double>>();

            for (int i = 0; i < 8; i++)
            {
                azimuthDataLists.Add(new List<double>());
                elevationDataLists.Add(new List<double>());
            }

            foreach (var line in lines)
            {
                if (line.StartsWith("packetTime: "))
                {
                    var time = double.Parse(line.Substring(12).Trim());
                    timestamps.Add(time);
                }
                else if (line.StartsWith("AzimuthData: "))
                {
                    var data = line.Substring(13).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
                    for (int i = 0; i < 8; i++)
                    {
                        azimuthDataLists[i].Add(data[i]);
                    }
                }
                else if (line.StartsWith("ElevationData: "))
                {
                    var data = line.Substring(15).Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
                    for (int i = 0; i < 8; i++)
                    {
                        elevationDataLists[i].Add(data[i]);
                    }
                }
            }

            PlotAzimuthGraph(timestamps, azimuthDataLists);
            PlotElevationGraph(timestamps, elevationDataLists);
        }
        private void PlotAzimuthGraph(List<double> timestamps, List<List<double>> azimuthDataLists)
        {
            chartAzimuth.Series.Clear();
            chartAzimuth.ChartAreas[0].AxisY.Minimum = double.MaxValue;
            chartAzimuth.ChartAreas[0].AxisY.Maximum = double.MinValue;

            for (int i = 0; i < azimuthDataLists.Count; i++)
            {
                var series = new Series($"Azimuth Point {i + 1}")
                {
                    ChartType = SeriesChartType.Point
                };

                for (int j = 0; j < timestamps.Count; j++)
                {
                    if (j < azimuthDataLists[i].Count)
                    {
                        double value = azimuthDataLists[i][j];
                        series.Points.AddXY(timestamps[j], value);

                        if (value < chartAzimuth.ChartAreas[0].AxisY.Minimum)
                            chartAzimuth.ChartAreas[0].AxisY.Minimum = value - 0.01;
                        if (value > chartAzimuth.ChartAreas[0].AxisY.Maximum)
                            chartAzimuth.ChartAreas[0].AxisY.Maximum = value + 0.01; 
                    }
                }

                chartAzimuth.Series.Add(series);
            }
            chartAzimuth.Invalidate(); 
        }

        private void PlotElevationGraph(List<double> timestamps, List<List<double>> elevationDataLists)
        {
            chartElevation.Series.Clear();
            chartElevation.ChartAreas[0].AxisY.Minimum = double.MaxValue;
            chartElevation.ChartAreas[0].AxisY.Maximum = double.MinValue;

            for (int i = 0; i < elevationDataLists.Count; i++)
            {
                var series = new Series($"Elevation Point {i + 1}")
                {
                    ChartType = SeriesChartType.Point
                };

                for (int j = 0; j < timestamps.Count; j++)
                {
                    if (j < elevationDataLists[i].Count)
                    {
                        double value = elevationDataLists[i][j];
                        series.Points.AddXY(timestamps[j], value);

                       
                        if (value < chartElevation.ChartAreas[0].AxisY.Minimum)
                            chartElevation.ChartAreas[0].AxisY.Minimum = value - 0.01; 
                        if (value > chartElevation.ChartAreas[0].AxisY.Maximum)
                            chartElevation.ChartAreas[0].AxisY.Maximum = value + 0.01; 
                    }
                }

                chartElevation.Series.Add(series);
            }
            chartElevation.Invalidate(); 
        }
        private void debuggingPlot_HandleDestroyed(object sender, EventArgs e)
        {
            chartAzimuth.MouseWheel -= Chart_MouseWheel;
            chartElevation.MouseWheel -= Chart_MouseWheel;
        }
    }
}
