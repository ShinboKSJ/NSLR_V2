namespace NSLR_ObservationControl
{
    partial class CSU_StarCalibration
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
            this.panel_right = new System.Windows.Forms.Panel();
            this.opticalSystem1 = new NSLR_ObservationControl.Module.OpticalSystem();
            this.observation_TMS1 = new NSLR_ObservationControl.Module.Observation_TMS();
            this.caMs1 = new NSLR_ObservationControl.Module.CAMs();
            this.starCalibration_Control1 = new NSLR_ObservationControl.Module.StarCalibration_Control();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.starCalibration_Setting1 = new NSLR_ObservationControl.Module.StarCalibration_Setting();
            this.panel_right.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_right
            // 
            this.panel_right.Controls.Add(this.opticalSystem1);
            this.panel_right.Controls.Add(this.observation_TMS1);
            this.panel_right.Controls.Add(this.caMs1);
            this.panel_right.Controls.Add(this.starCalibration_Control1);
            this.panel_right.Location = new System.Drawing.Point(2477, 3);
            this.panel_right.Name = "panel_right";
            this.panel_right.Size = new System.Drawing.Size(783, 1414);
            this.panel_right.TabIndex = 0;
            // 
            // opticalSystem1
            // 
            this.opticalSystem1.Location = new System.Drawing.Point(0, 874);
            this.opticalSystem1.Name = "opticalSystem1";
            this.opticalSystem1.Size = new System.Drawing.Size(782, 341);
            this.opticalSystem1.TabIndex = 7;
            // 
            // observation_TMS1
            // 
            this.observation_TMS1._satel_ExternalLibrary = null;
            this.observation_TMS1.AutoSize = true;
            this.observation_TMS1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.observation_TMS1.ExternalLibraryObject = null;
            this.observation_TMS1.Location = new System.Drawing.Point(2, 437);
            this.observation_TMS1.Margin = new System.Windows.Forms.Padding(0);
            this.observation_TMS1.Name = "observation_TMS1";
            this.observation_TMS1.Size = new System.Drawing.Size(781, 431);
            this.observation_TMS1.TabIndex = 6;
            // 
            // caMs1
            // 
            this.caMs1.BackColor = System.Drawing.Color.Transparent;
            this.caMs1.Location = new System.Drawing.Point(0, 3);
            this.caMs1.Name = "caMs1";
            this.caMs1.Size = new System.Drawing.Size(777, 428);
            this.caMs1.TabIndex = 3;
            // 
            // starCalibration_Control1
            // 
            this.starCalibration_Control1.AutoSize = true;
            this.starCalibration_Control1.BackColor = System.Drawing.Color.Transparent;
            this.starCalibration_Control1.Location = new System.Drawing.Point(0, 1218);
            this.starCalibration_Control1.Margin = new System.Windows.Forms.Padding(0);
            this.starCalibration_Control1.Name = "starCalibration_Control1";
            this.starCalibration_Control1.Size = new System.Drawing.Size(780, 210);
            this.starCalibration_Control1.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(575, 60);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(128, 23);
            this.button2.TabIndex = 4;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(585, 123);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.Black;
            this.chart1.BorderlineColor = System.Drawing.Color.Transparent;
            this.chart1.BorderSkin.BorderColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea1.AxisX2.Interval = 30D;
            chartArea1.AxisX2.LabelStyle.ForeColor = System.Drawing.Color.Tomato;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.BackColor = System.Drawing.Color.Silver;
            chartArea1.BorderColor = System.Drawing.Color.Coral;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Cursor = System.Windows.Forms.Cursors.Cross;
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(554, 3);
            this.chart1.Margin = new System.Windows.Forms.Padding(0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(1918, 1511);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "f";
            this.chart1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseClick);
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(583, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "생성된 데이터 수 :  - 개";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.BackColor = System.Drawing.Color.Black;
            this.infoLabel.ForeColor = System.Drawing.Color.White;
            this.infoLabel.Location = new System.Drawing.Point(585, 96);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(0, 12);
            this.infoLabel.TabIndex = 4;
            // 
            // starCalibration_Setting1
            // 
            this.starCalibration_Setting1.BackColor = System.Drawing.Color.Black;
            this.starCalibration_Setting1.Location = new System.Drawing.Point(0, 0);
            this.starCalibration_Setting1.Margin = new System.Windows.Forms.Padding(0);
            this.starCalibration_Setting1.Name = "starCalibration_Setting1";
            this.starCalibration_Setting1.Size = new System.Drawing.Size(550, 1511);
            this.starCalibration_Setting1.TabIndex = 5;
            // 
            // CSU_StarCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.Controls.Add(this.starCalibration_Setting1);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.panel_right);
            this.DoubleBuffered = true;
            this.Name = "CSU_StarCalibration";
            this.Size = new System.Drawing.Size(3262, 2160);
            this.panel_right.ResumeLayout(false);
            this.panel_right.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_right;
        private Module.StarCalibration_Control starCalibration_Control1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        public Module.CAMs caMs1;
        public Module.OpticalSystem opticalSystem1;
        public Module.Observation_TMS observation_TMS1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label infoLabel;
        private Module.StarCalibration_Setting starCalibration_Setting1;
    }
}
