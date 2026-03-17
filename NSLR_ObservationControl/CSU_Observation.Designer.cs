namespace NSLR_ObservationControl
{
    partial class CSU_Observation
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ObservTime = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblNoradId = new System.Windows.Forms.Label();
            this.lblSatelliteName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.observation_RangeGatePanel1 = new NSLR_ObservationControl.Module.Observation_RangeGatePanel();
            this.opticalSystem1 = new NSLR_ObservationControl.Module.OpticalSystem();
            this.observation_TMS2 = new NSLR_ObservationControl.Module.Observation_TMS();
            this.caMs1 = new NSLR_ObservationControl.Module.CAMs();
            this.observation_Control1 = new NSLR_ObservationControl.Module.Observation_Control();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
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
            this.chart2.Location = new System.Drawing.Point(2456, 1);
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
            this.chart2.Size = new System.Drawing.Size(361, 1595);
            this.chart2.TabIndex = 12;
            this.chart2.Text = "chart1";
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
            chartArea4.InnerPlotPosition.Width = 92.83517F;
            chartArea4.InnerPlotPosition.X = 6F;
            chartArea4.InnerPlotPosition.Y = 1.10526F;
            chartArea4.Name = "ChartArea1";
            chartArea4.Position.Auto = false;
            chartArea4.Position.Height = 95F;
            chartArea4.Position.Width = 98F;
            chartArea4.Position.X = 1F;
            chartArea4.Position.Y = 5F;
            this.chart1.ChartAreas.Add(chartArea4);
            this.chart1.Location = new System.Drawing.Point(3, 1);
            this.chart1.Margin = new System.Windows.Forms.Padding(3, 4, 0, 4);
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
            this.chart1.Size = new System.Drawing.Size(2448, 1594);
            this.chart1.TabIndex = 11;
            this.chart1.Text = "chart1";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(616, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 51);
            this.label1.TabIndex = 14;
            this.label1.Text = " ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.OliveDrab;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(467, 24);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 51);
            this.button1.TabIndex = 13;
            this.button1.Text = "RMS calculation";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.ObservTime);
            this.panel2.Controls.Add(this.progressBar);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(551, 1602);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1244, 256);
            this.panel2.TabIndex = 16;
            // 
            // ObservTime
            // 
            this.ObservTime.AutoSize = true;
            this.ObservTime.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ObservTime.ForeColor = System.Drawing.Color.White;
            this.ObservTime.Location = new System.Drawing.Point(518, 189);
            this.ObservTime.Name = "ObservTime";
            this.ObservTime.Size = new System.Drawing.Size(213, 34);
            this.ObservTime.TabIndex = 2;
            this.ObservTime.Text = "01 : 12 : 73";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(57, 122);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1133, 46);
            this.progressBar.TabIndex = 1;
            this.progressBar.Value = 100;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(54)))), ((int)(((byte)(78)))));
            this.label4.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(2, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(1241, 51);
            this.label4.TabIndex = 0;
            this.label4.Text = "Progress";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.lblNoradId);
            this.panel1.Controls.Add(this.lblSatelliteName);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(3, 1602);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(540, 254);
            this.panel1.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(54)))), ((int)(((byte)(78)))));
            this.label7.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(218, 1);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(319, 51);
            this.label7.TabIndex = 0;
            this.label7.Text = "Target Info";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(54)))), ((int)(((byte)(78)))));
            this.label2.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(2, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(215, 51);
            this.label2.TabIndex = 0;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.label9.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(3, 189);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(214, 61);
            this.label9.TabIndex = 0;
            this.label9.Text = "Source";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.label8.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(2, 122);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(215, 61);
            this.label8.TabIndex = 0;
            this.label8.Text = "NoradID";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.White;
            this.label12.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(219, 189);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(319, 61);
            this.label12.TabIndex = 0;
            this.label12.Text = "-";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblNoradId
            // 
            this.lblNoradId.BackColor = System.Drawing.Color.White;
            this.lblNoradId.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblNoradId.ForeColor = System.Drawing.Color.Black;
            this.lblNoradId.Location = new System.Drawing.Point(219, 122);
            this.lblNoradId.Name = "lblNoradId";
            this.lblNoradId.Size = new System.Drawing.Size(319, 61);
            this.lblNoradId.TabIndex = 0;
            this.lblNoradId.Text = " ";
            this.lblNoradId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSatelliteName
            // 
            this.lblSatelliteName.BackColor = System.Drawing.Color.White;
            this.lblSatelliteName.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSatelliteName.ForeColor = System.Drawing.Color.Black;
            this.lblSatelliteName.Location = new System.Drawing.Point(219, 56);
            this.lblSatelliteName.Name = "lblSatelliteName";
            this.lblSatelliteName.Size = new System.Drawing.Size(319, 61);
            this.lblSatelliteName.TabIndex = 0;
            this.lblSatelliteName.Text = " ";
            this.lblSatelliteName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.label6.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(3, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(214, 61);
            this.label6.TabIndex = 0;
            this.label6.Text = "Target";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label15);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label13);
            this.panel3.Location = new System.Drawing.Point(1801, 1603);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(617, 256);
            this.panel3.TabIndex = 16;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label15.Font = new System.Drawing.Font("굴림", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(344, 130);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(264, 49);
            this.label15.TabIndex = 3;
            this.label15.Text = "3814.21km";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("굴림", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(24, 134);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(344, 37);
            this.label14.TabIndex = 3;
            this.label14.Text = "Average Distance :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(722, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(213, 34);
            this.label3.TabIndex = 2;
            this.label3.Text = "00 : 12 : 73";
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(54)))), ((int)(((byte)(78)))));
            this.label13.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(0, 1);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(614, 51);
            this.label13.TabIndex = 0;
            this.label13.Text = "Result";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.button2);
            this.panel4.Controls.Add(this.label18);
            this.panel4.Controls.Add(this.label19);
            this.panel4.Location = new System.Drawing.Point(2424, 1603);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(217, 256);
            this.panel4.TabIndex = 16;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.OliveDrab;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button2.ForeColor = System.Drawing.SystemColors.Control;
            this.button2.Location = new System.Drawing.Point(41, 114);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(137, 71);
            this.button2.TabIndex = 3;
            this.button2.Text = "생성";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label18.ForeColor = System.Drawing.Color.White;
            this.label18.Location = new System.Drawing.Point(722, 189);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(213, 34);
            this.label18.TabIndex = 2;
            this.label18.Text = "00 : 12 : 73";
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(54)))), ((int)(((byte)(78)))));
            this.label19.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label19.ForeColor = System.Drawing.Color.White;
            this.label19.Location = new System.Drawing.Point(0, 1);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(214, 51);
            this.label19.TabIndex = 0;
            this.label19.Text = "SOD 생성";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // observation_RangeGatePanel1
            // 
            this.observation_RangeGatePanel1.AutoSize = true;
            this.observation_RangeGatePanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.observation_RangeGatePanel1.Location = new System.Drawing.Point(2830, 1259);
            this.observation_RangeGatePanel1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.observation_RangeGatePanel1.Name = "observation_RangeGatePanel1";
            this.observation_RangeGatePanel1.Size = new System.Drawing.Size(891, 194);
            this.observation_RangeGatePanel1.TabIndex = 21;
            // 
            // opticalSystem1
            // 
            this.opticalSystem1.AutoSize = true;
            this.opticalSystem1.Location = new System.Drawing.Point(2830, 1460);
            this.opticalSystem1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.opticalSystem1.Name = "opticalSystem1";
            this.opticalSystem1.Size = new System.Drawing.Size(883, 429);
            this.opticalSystem1.TabIndex = 20;
            // 
            // observation_TMS2
            // 
            this.observation_TMS2._satel_ExternalLibrary = null;
            this.observation_TMS2.AutoSize = true;
            this.observation_TMS2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.observation_TMS2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.observation_TMS2.ExternalLibraryObject = null;
            this.observation_TMS2.Location = new System.Drawing.Point(2830, 734);
            this.observation_TMS2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.observation_TMS2.Name = "observation_TMS2";
            this.observation_TMS2.Size = new System.Drawing.Size(892, 517);
            this.observation_TMS2.TabIndex = 19;
            // 
            // caMs1
            // 
            this.caMs1.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.caMs1.BackColor = System.Drawing.Color.Transparent;
            this.caMs1.Location = new System.Drawing.Point(2830, -4);
            this.caMs1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.caMs1.Name = "caMs1";
            this.caMs1.Size = new System.Drawing.Size(881, 546);
            this.caMs1.TabIndex = 18;
            // 
            // observation_Control1
            // 
            this.observation_Control1.AutoSize = true;
            this.observation_Control1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.observation_Control1.Location = new System.Drawing.Point(2830, 540);
            this.observation_Control1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.observation_Control1.Name = "observation_Control1";
            this.observation_Control1.ObservationData = null;
            this.observation_Control1.Size = new System.Drawing.Size(891, 189);
            this.observation_Control1.TabIndex = 6;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.button3);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.label10);
            this.panel5.Location = new System.Drawing.Point(2647, 1605);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(177, 254);
            this.panel5.TabIndex = 16;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.OliveDrab;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button3.ForeColor = System.Drawing.SystemColors.Control;
            this.button3.Location = new System.Drawing.Point(33, 112);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(108, 71);
            this.button3.TabIndex = 3;
            this.button3.Text = "실행";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.DPS_Compute);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(722, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(213, 34);
            this.label5.TabIndex = 2;
            this.label5.Text = "00 : 12 : 73";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(54)))), ((int)(((byte)(78)))));
            this.label10.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(0, 1);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(176, 51);
            this.label10.TabIndex = 0;
            this.label10.Text = "DPS";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CSU_Observation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.observation_RangeGatePanel1);
            this.Controls.Add(this.opticalSystem1);
            this.Controls.Add(this.observation_TMS2);
            this.Controls.Add(this.caMs1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.observation_Control1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "CSU_Observation";
            this.Size = new System.Drawing.Size(4198, 2501);
            this.Load += new System.EventHandler(this.CSU_Observation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private Module.Observation_Control observation_Control1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label ObservTime;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblNoradId;
        private System.Windows.Forms.Label lblSatelliteName;
        private System.Windows.Forms.Label label6;
        public Module.CAMs caMs1;
        public Module.OpticalSystem opticalSystem1;
        public Module.Observation_TMS observation_TMS2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private Module.Observation_RangeGatePanel observation_RangeGatePanel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
    }
}
