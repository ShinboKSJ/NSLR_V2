namespace NSLR_ObservationControl
{
    partial class CSU_GroundCalibration
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.observation_RangeGatePanel1 = new NSLR_ObservationControl.Module.Observation_RangeGatePanel();
            this.opticalSystem1 = new NSLR_ObservationControl.Module.OpticalSystem();
            this.groundCalibration_Info1 = new NSLR_ObservationControl.Module.GroundCalibration_Info();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel1.Controls.Add(this.observation_RangeGatePanel1);
            this.panel1.Controls.Add(this.opticalSystem1);
            this.panel1.Controls.Add(this.chart2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.groundCalibration_Info1);
            this.panel1.Controls.Add(this.chart1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(3673, 2001);
            this.panel1.TabIndex = 3;
            // 
            // chart2
            // 
            this.chart2.BackColor = System.Drawing.Color.Black;
            chartArea3.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea3.AxisX.LineColor = System.Drawing.Color.White;
            chartArea3.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea3.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea3.AxisY.LineColor = System.Drawing.Color.White;
            chartArea3.BackColor = System.Drawing.Color.Black;
            chartArea3.InnerPlotPosition.Auto = false;
            chartArea3.InnerPlotPosition.Height = 94.54364F;
            chartArea3.InnerPlotPosition.Width = 84.93033F;
            chartArea3.InnerPlotPosition.X = 10F;
            chartArea3.InnerPlotPosition.Y = 1.10526F;
            chartArea3.Name = "ChartArea1";
            chartArea3.Position.Auto = false;
            chartArea3.Position.Height = 95F;
            chartArea3.Position.Width = 91F;
            chartArea3.Position.X = 1F;
            chartArea3.Position.Y = 5F;
            this.chart2.ChartAreas.Add(chartArea3);
            legend2.Enabled = false;
            legend2.ForeColor = System.Drawing.Color.White;
            legend2.Name = "Legend1";
            this.chart2.Legends.Add(legend2);
            this.chart2.Location = new System.Drawing.Point(2149, 0);
            this.chart2.Margin = new System.Windows.Forms.Padding(0);
            this.chart2.Name = "chart2";
            series3.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.Top;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series3.LabelBackColor = System.Drawing.Color.Transparent;
            series3.LabelForeColor = System.Drawing.Color.White;
            series3.Legend = "Legend1";
            series3.MarkerColor = System.Drawing.Color.Transparent;
            series3.MarkerSize = 3;
            series3.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series3.Name = "Series1";
            this.chart2.Series.Add(series3);
            this.chart2.Size = new System.Drawing.Size(316, 1492);
            this.chart2.TabIndex = 10;
            this.chart2.Text = "chart1";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(539, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 41);
            this.label1.TabIndex = 8;
            this.label1.Text = " ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.OliveDrab;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(409, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(108, 41);
            this.button1.TabIndex = 7;
            this.button1.Text = "RMS calculation";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.Black;
            this.chart1.BorderSkin.BorderColor = System.Drawing.Color.White;
            chartArea4.AxisX.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea4.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea4.AxisX.LineColor = System.Drawing.Color.White;
            chartArea4.AxisX2.LineColor = System.Drawing.Color.White;
            chartArea4.AxisY.LineColor = System.Drawing.Color.White;
            chartArea4.AxisY2.LineColor = System.Drawing.Color.White;
            chartArea4.BackColor = System.Drawing.Color.Black;
            chartArea4.BorderColor = System.Drawing.Color.Transparent;
            chartArea4.InnerPlotPosition.Auto = false;
            chartArea4.InnerPlotPosition.Height = 94.5435F;
            chartArea4.InnerPlotPosition.Width = 94.83517F;
            chartArea4.InnerPlotPosition.X = 4F;
            chartArea4.InnerPlotPosition.Y = 1.10526F;
            chartArea4.Name = "ChartArea1";
            chartArea4.Position.Auto = false;
            chartArea4.Position.Height = 95F;
            chartArea4.Position.Width = 98F;
            chartArea4.Position.X = 1F;
            chartArea4.Position.Y = 5F;
            this.chart1.ChartAreas.Add(chartArea4);
            this.chart1.Location = new System.Drawing.Point(3, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.chart1.Name = "chart1";
            series4.BackImageAlignment = System.Windows.Forms.DataVisualization.Charting.ChartImageAlignmentStyle.Top;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series4.LabelBackColor = System.Drawing.Color.Transparent;
            series4.LabelForeColor = System.Drawing.Color.White;
            series4.MarkerColor = System.Drawing.Color.Black;
            series4.MarkerSize = 1;
            series4.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series4.Name = "Series1";
            this.chart1.Series.Add(series4);
            this.chart1.Size = new System.Drawing.Size(2142, 1491);
            this.chart1.TabIndex = 9;
            this.chart1.Text = "chart1";
            // 
            // observation_RangeGatePanel1
            // 
            this.observation_RangeGatePanel1.AutoSize = true;
            this.observation_RangeGatePanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.observation_RangeGatePanel1.Location = new System.Drawing.Point(2470, 987);
            this.observation_RangeGatePanel1.Name = "observation_RangeGatePanel1";
            this.observation_RangeGatePanel1.Size = new System.Drawing.Size(780, 155);
            this.observation_RangeGatePanel1.TabIndex = 12;
            // 
            // opticalSystem1
            // 
            this.opticalSystem1.Location = new System.Drawing.Point(2470, 1148);
            this.opticalSystem1.Name = "opticalSystem1";
            this.opticalSystem1.Size = new System.Drawing.Size(770, 341);
            this.opticalSystem1.TabIndex = 11;
            // 
            // groundCalibration_Info1
            // 
            this.groundCalibration_Info1.BackColor = System.Drawing.Color.Transparent;
            this.groundCalibration_Info1.ForeColor = System.Drawing.Color.Transparent;
            this.groundCalibration_Info1.Location = new System.Drawing.Point(2475, 4);
            this.groundCalibration_Info1.Margin = new System.Windows.Forms.Padding(0);
            this.groundCalibration_Info1.Name = "groundCalibration_Info1";
            this.groundCalibration_Info1.Size = new System.Drawing.Size(780, 980);
            this.groundCalibration_Info1.TabIndex = 3;
            this.groundCalibration_Info1.Load += new System.EventHandler(this.groundCalibration_Info1_Load);
            // 
            // CSU_GroundCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Name = "CSU_GroundCalibration";
            this.Size = new System.Drawing.Size(3840, 2139);
            this.Load += new System.EventHandler(this.CSU_GroundCalibration_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        public Module.OpticalSystem opticalSystem1;
        public Module.GroundCalibration_Info groundCalibration_Info1;
        private Module.Observation_RangeGatePanel observation_RangeGatePanel1;
    }
}
