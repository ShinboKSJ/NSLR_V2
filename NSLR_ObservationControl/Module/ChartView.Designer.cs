namespace NSLR_ObservationControl.Module
{
    partial class ChartView
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
            this.mapChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.mapChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).BeginInit();
            this.SuspendLayout();
            // 
            // mapChart
            // 
            chartArea1.Name = "ChartArea1";
            this.mapChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.mapChart.Legends.Add(legend1);
            this.mapChart.Location = new System.Drawing.Point(32, 21);
            this.mapChart.Name = "mapChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.mapChart.Series.Add(series1);
            this.mapChart.Size = new System.Drawing.Size(1156, 1504);
            this.mapChart.TabIndex = 0;
            this.mapChart.Text = "chart1";            
            this.mapChart.Click += new System.EventHandler(this.mapChart_Click);
            // 
            // dgView
            // 
            this.dgView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgView.Location = new System.Drawing.Point(1221, 21);
            this.dgView.Name = "dgView";
            this.dgView.RowTemplate.Height = 23;
            this.dgView.Size = new System.Drawing.Size(582, 1504);
            this.dgView.TabIndex = 1;
            // 
            // ChartView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.dgView);
            this.Controls.Add(this.mapChart);
            this.Name = "ChartView";
            this.Size = new System.Drawing.Size(1834, 1562);
            ((System.ComponentModel.ISupportInitialize)(this.mapChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart mapChart;
        private System.Windows.Forms.DataGridView dgView;
    }
}
