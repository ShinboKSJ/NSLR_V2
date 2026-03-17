namespace NSLR_ObservationControl.Module
{
    partial class debuggingPlot
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartAzimuth = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartElevation = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartAzimuth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartElevation)).BeginInit();
            this.SuspendLayout();
            // 
            // chartAzimuth
            // 
            chartArea1.Name = "ChartArea1";
            this.chartAzimuth.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartAzimuth.Legends.Add(legend1);
            this.chartAzimuth.Location = new System.Drawing.Point(40, 31);
            this.chartAzimuth.Name = "chartAzimuth";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartAzimuth.Series.Add(series1);
            this.chartAzimuth.Size = new System.Drawing.Size(2122, 577);
            this.chartAzimuth.TabIndex = 0;
            this.chartAzimuth.Text = "chart1";
            // 
            // chartElevation
            // 
            chartArea2.Name = "ChartArea1";
            this.chartElevation.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartElevation.Legends.Add(legend2);
            this.chartElevation.Location = new System.Drawing.Point(40, 667);
            this.chartElevation.Name = "chartElevation";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartElevation.Series.Add(series2);
            this.chartElevation.Size = new System.Drawing.Size(2122, 577);
            this.chartElevation.TabIndex = 1;
            this.chartElevation.Text = "chart2";
            // 
            // debuggingPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.Controls.Add(this.chartElevation);
            this.Controls.Add(this.chartAzimuth);
            this.Name = "debuggingPlot";
            this.Size = new System.Drawing.Size(2210, 1343);
            ((System.ComponentModel.ISupportInitialize)(this.chartAzimuth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartElevation)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartAzimuth;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartElevation;
    }
}
