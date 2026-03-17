namespace NSLR_ObservationControl.OAS
{
    partial class ResidualPlotViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.plot_groupBox = new System.Windows.Forms.GroupBox();
            this.residual_chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.plot_groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.residual_chart)).BeginInit();
            this.SuspendLayout();
            // 
            // plot_groupBox
            // 
            this.plot_groupBox.Controls.Add(this.residual_chart);
            this.plot_groupBox.Location = new System.Drawing.Point(12, 12);
            this.plot_groupBox.Name = "plot_groupBox";
            this.plot_groupBox.Size = new System.Drawing.Size(504, 357);
            this.plot_groupBox.TabIndex = 1;
            this.plot_groupBox.TabStop = false;
            this.plot_groupBox.Text = "Plots";
            // 
            // residual_chart
            // 
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.LabelStyle.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisX.LabelStyle.TruncatedLabels = true;
            chartArea1.Name = "ChartArea1";
            chartArea2.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea2.Name = "ChartArea2";
            chartArea3.AxisX.LabelStyle.Format = "1.0";
            chartArea3.AxisX.Title = "Elapsed Time (sec)";
            chartArea3.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea3.Name = "ChartArea3";
            this.residual_chart.ChartAreas.Add(chartArea1);
            this.residual_chart.ChartAreas.Add(chartArea2);
            this.residual_chart.ChartAreas.Add(chartArea3);
            this.residual_chart.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.residual_chart.Legends.Add(legend1);
            this.residual_chart.Location = new System.Drawing.Point(3, 17);
            this.residual_chart.Name = "residual_chart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series1.Legend = "Legend1";
            series1.MarkerColor = System.Drawing.Color.Black;
            series1.MarkerSize = 3;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Series1";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            series2.ChartArea = "ChartArea2";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series2.Legend = "Legend1";
            series2.MarkerColor = System.Drawing.Color.Black;
            series2.MarkerSize = 3;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series2.Name = "Series2";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            series3.ChartArea = "ChartArea3";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series3.Legend = "Legend1";
            series3.MarkerColor = System.Drawing.Color.Black;
            series3.MarkerSize = 3;
            series3.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series3.Name = "Series3";
            series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Single;
            this.residual_chart.Series.Add(series1);
            this.residual_chart.Series.Add(series2);
            this.residual_chart.Series.Add(series3);
            this.residual_chart.Size = new System.Drawing.Size(498, 337);
            this.residual_chart.TabIndex = 1;
            this.residual_chart.Text = "residual_chart";
            title1.Alignment = System.Drawing.ContentAlignment.TopCenter;
            title1.DockedToChartArea = "ChartArea1";
            title1.Name = "Title1";
            title1.Text = "Range Residual (m)";
            title2.Alignment = System.Drawing.ContentAlignment.TopCenter;
            title2.DockedToChartArea = "ChartArea2";
            title2.Name = "Title2";
            title2.Text = "Azimuth Residual (arcsec)";
            title3.Alignment = System.Drawing.ContentAlignment.TopCenter;
            title3.DockedToChartArea = "ChartArea3";
            title3.Name = "Title3";
            title3.Text = "Elevation Residual (arcsec)";
            this.residual_chart.Titles.Add(title1);
            this.residual_chart.Titles.Add(title2);
            this.residual_chart.Titles.Add(title3);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(522, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(189, 356);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Statistics";
            // 
            // ResidualPlotViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 381);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.plot_groupBox);
            this.Name = "ResidualPlotViewer";
            this.Text = "NSLR-OAS::Residual Plot Viewer";
            this.Load += new System.EventHandler(this.ResidualPlotViewer_Load);
            this.plot_groupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.residual_chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox plot_groupBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart residual_chart;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}