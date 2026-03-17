//#define TLE_FROM_CELESTRAK
using Braincase.GanttChart;
using SGPdotNET.CoordinateSystem;
using SGPdotNET.Observation;
using SGPdotNET.TLE;
using SGPdotNET.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using NSLR_ObservationControl.OrbitData;

namespace NSLR_ObservationControl
{
    public partial class CSU_ObserveSchedule : Form
    {
        ProjectManager _mManager = null;

        Graphics graph;
        bool drawpoint, drawicon = false;
        double LATITUDE_HERE = 37.386265;
        double LONGITUDE_HERE = 126.937321;
        bool ready = false;
        int satellite_selected = 0;

        Satellite sat;
        GroundStation groundStation;
        List<Satellite> visible;
        private double ratio = 1.0F;
        private Point imgPoint;
        private Rectangle imgRect;
        private Point clickPoint;
        private Point LastPoint;
        Dictionary<string, string> tleUrl = new Dictionary<string, string>();
        Timer timer = new Timer();
        Timer timer1 = new Timer();

        MyTask[] task_sat = new MyTask[1000];

        Angle minAngle = (Angle)30; //visibqweqwea
                                    //le 판단을 위한 적절한 각도,  추후 결정

        private Bitmap satelliteIcon;
        private int satelliteSize = 50;

        public CSU_ObserveSchedule()
        {
            InitializeComponent();

            // Create a Project and some Tasks
            _mManager = new ProjectManager();

            init_gui();           
    
            GetTleFromUrl();
            task_setting();

            satelliteIcon = new Bitmap(NSLR_ObservationControl.Properties.Resources.satOrbit1);
        }

 
        private void init_gui()
        {
            imgPoint = new Point(GroundTrack.Width / 2, GroundTrack.Height / 2);
            imgRect = new Rectangle(0, 0, GroundTrack.Width, GroundTrack.Height);
            ratio = 1.0;
            clickPoint = imgPoint;
            GroundTrack.Invalidate();
            timer.Interval = 100; // 1초
            timer.Tick += new EventHandler(timer_Tick);
            timer1.Interval = 3600000;
            timer1.Tick += new EventHandler(timer_Tick2);
            DateTime time = DateTime.Now;   //UtcNow;
            DateTime time1 = DateTime.UtcNow;
            string format = "yyyy-MM-dd tt HH:mm";
            this.Text = time.ToString(format) + " NOW  / " + time1.ToString(format) + " UTC";

            DateTime_Start.Text = time.ToString("yyyy 년 MM 월 dd 일 tt hh 시 mm 분");
            DateTime_End.Text = time.ToString("yyyy 년 MM 월 dd 일 tt hh 시 mm 분");

            // 임시 ////////////////////////////////////////////
            label_sat_altitude.Text = "19147.64";
            label_sat_az.Text = "199.1 SSW";
            label_sat_el.Text = "41.6";
            ////////////////////////////////////////////////////

            Color col = Color.Black;
            graph = GroundTrack.CreateGraphics();
            chartInit();
        }


        private void Form_ObserveSchedule_Load(object sender, EventArgs e)
        {
            timer.Stop();
            timer1.Stop();
            GroundTrack.Refresh();
            chart1.Series["Series1"].Points.Clear();

        }


        private void check_monitor()
        {
            Screen[] screens = Screen.AllScreens;
            //MessageBox.Show($"Screen : {screens.Length}");
            if (screens.Length > 1)     // Has more screen
            {
                Screen scrn = (screens[1].WorkingArea.Contains(this.Location)) ? screens[0] : screens[1];  //for Screen 0 / Screen 1
                this.Show();
                this.Location = new System.Drawing.Point(scrn.Bounds.Left, 0); //scrn.Bounds.Left  왼쪽 스크린에 
                this.WindowState = FormWindowState.Maximized;
            }
        }


        void _mChart_TaskSelected(object sender, TaskMouseEventArgs e)
        {

            timer.Stop();
            timer1.Stop();
            GroundTrack.Image = null;
            GroundTrack.Update();
            chart1.Series["Series1"].Points.Clear();

            Console.WriteLine($"===================================================================================================");

            //_mTaskGrid.SelectedObjects = _mChart.SelectedTasks.Select(x => _mManager.IsPart(x) ? _mManager.SplitTaskOf(x) : x).ToArray();
            //_mResourceGrid.Items.Clear();
            //_mResourceGrid.Items.AddRange(_mManager.ResourcesOf(e.Task).Select(x => new ListViewItem(((MyResource)x).Name)).ToArray());
            lblStatus.Text = $"[TaskSelected] {e.Task.ToString()} ";
            Console.WriteLine($"-------------------------------------------------------------------------");
            Console.WriteLine($"[TaskSelected] {e.Task.ToString()} ");
            Console.WriteLine($"= [Sat TLE] ----------------------------------------------------------  =");
            sat = visible[_mManager.IndexOf(e.Task)];
            Console.WriteLine($"{sat.Tle}");
            Console.WriteLine($"-------------------------------------------------------------------------");

            label_sat_name.Text = e.Task.Name;
            //label_sat_src.Text = "Source?";
            //label_sat_altitude.Text = sat.Name;
            //label_sat_az.Text = sat.Tle.Line1.Atitude. ie.Task.atitude.ToString();/
            //label_sat_el.Text = longitude.ToString();//
            //var ss = sat.Predict();
            label_sat_riseTime.Text = "06.12.23";//
            label_sat_setTime.Text = "18.34.45";//
            //label_sat_keyhole.Text = "KeyHole?";
            //label_sat_sunAvoidance.Text = "SunAvoida";
            //label_sat_maxEl.Text = ss.ToGeodetic().Altitude.ToString();


            //Map_Box.Update();
            //this.Text = this.Text.Remove(30, 50);
            //this.Text = this.Text.Insert(30, sat.ToString());
            //satellite_selected = TaskGridView.SelectedIndex;
            tracking();
            predicted_tracking();
            Console.WriteLine($"===================================================================================================\n\n");
            timer.Start();
            timer1.Start();
        }

        void _mChart_TaskMouseOut(object sender, TaskMouseEventArgs e)
        {
            //lblStatus.Text = "";
            _mChart.Invalidate();
            lblStatus.Text = $"[TaskMouseOut] {e.Task.ToString()} ";
        }

        void _mChart_TaskMouseOver(object sender, TaskMouseEventArgs e)
        {
            lblStatus.Text = string.Format("[Task MouthOver] {0}  ~  {1}", _mManager.GetDateTime(e.Task.Start).ToLongDateString(), _mManager.GetDateTime(e.Task.End).ToLongDateString());
            _mChart.Invalidate();
        }

        /////////////////////////////////////////////////////////////////////////////////////////
        #region Main Menu
        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var fs = System.IO.File.OpenWrite(dialog.FileName))
                    {
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        bf.Serialize(fs, _mManager);
                    }
                }
            }
        }

        private void mnuFileOpen_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var fs = System.IO.File.OpenRead(dialog.FileName))
                    {
                        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        _mManager = bf.Deserialize(fs) as ProjectManager;
                        if (_mManager == null)
                        {
                            MessageBox.Show("Unable to load ProjectManager. Data structure might have changed from previous verions", "Gantt Chart", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            _mChart.Init(_mManager);
                            _mChart.Invalidate();
                        }
                    }
                }
            }
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            //this.Close();
        }

        private void mnuViewDaysDayOfWeek_Click(object sender, EventArgs e)
        {
            _mChart.TimeResolution = TimeResolution.Week;
            _mChart.Invalidate();
        }

        private void mnuFileNew_Click(object sender, EventArgs e)
        {
            // start a new Project and init the chart with the project
            _mManager = new ProjectManager();
            _mManager.Add(new Braincase.GanttChart.Task() { Name = "New Task" });
            _mChart.Init(_mManager);
            _mChart.Invalidate();
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Please visit http://www.jakesee.com/net-c-winforms-gantt-chart-control/ for more help and details", "Braincase Solutions - Gantt Chart", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                System.Diagnostics.Process.Start("http://www.jakesee.com/net-c-winforms-gantt-chart-control/");
            }
        }

        private void mnuViewRelationships_Click(object sender, EventArgs e)
        {
            _mChart.ShowRelations = mnuViewRelationships.Checked = !mnuViewRelationships.Checked;
            _mChart.Invalidate();
        }

        private void mnuViewSlack_Click(object sender, EventArgs e)
        {
            _mChart.ShowSlack = mnuViewSlack.Checked = !mnuViewSlack.Checked;
            _mChart.Invalidate();
        }

        private void mnuViewIntructions_Click(object sender, EventArgs e)
        {
            _mChart.Invalidate();
        }

        #region Timescale Views ///////////////////////////////////////////////
        private void mnuViewDays_Click(object sender, EventArgs e)
        {
            _mChart.TimeResolution = TimeResolution.Day;
            _ClearTimeResolutionMenu();
            mnuViewDays.Checked = true;
            _mChart.Invalidate();
        }
        private void mnuViewWeeks_Click(object sender, EventArgs e)
        {
            _mChart.TimeResolution = TimeResolution.Week;
            _ClearTimeResolutionMenu();
            mnuViewWeeks.Checked = true;
            _mChart.Invalidate();
        }
        private void mnuViewHours_Click(object sender, EventArgs e)
        {
            _mChart.TimeResolution = TimeResolution.Hour;
            _ClearTimeResolutionMenu();
            mnuViewHours.Checked = true;
            _mChart.Invalidate();
        }
        private void _ClearTimeResolutionMenu()
        {
            mnuViewDays.Checked = false;
            mnuViewWeeks.Checked = false;
            mnuViewHours.Checked = false;
        }
        #endregion Timescale Views   /////////////////////////////////////////////
        #endregion Main Menu
        /////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////
        #region Sidebar

        private void _mDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            //_mManager.Start = _mStartDatePicker.Value;
            var span = DateTime.Today - _mManager.Start;
            _mManager.Now = span;

            _mChart.Invalidate();
        }

        private void _mPropertyGrid_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            _mChart.Invalidate();
        }
        private void _mNowDatePicker_ValueChanged(object sender, EventArgs e)
        {
            //TimeSpan span = _mNowDatePicker.Value - _mStartDatePicker.Value;
            //_mManager.Now = span.Add(new TimeSpan(1, 0, 0, 0));
            //_mChart.Invalidate();
        }
        private void _mScrollDatePicker_ValueChanged(object sender, EventArgs e)
        {
            //_mChart.ScrollTo(_mScrollDatePicker.Value);
            _mChart.Invalidate();
        }
        private void _mTaskGridView_SelectionChanged(object sender, EventArgs e)
        {
            //if (TaskGridView.SelectedRows.Count > 0)
            {
                //var task = TaskGridView.SelectedRows[0].DataBoundItem as Braincase.GanttChart.Task;
                //_mChart.ScrollTo(task);
            }
        }
        private void TaskGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0 && e.RowIndex < TaskGridView.Rows.Count)
            {
                var task = TaskGridView.Rows[e.RowIndex].DataBoundItem as Task;
                _mChart.ScrollTo(task);
            }
        }

        #endregion Sidebar
        /////////////////////////////////////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////////////////////////////////////
        #region custom task and resource   
        /// <summary>
        /// A custom resource of your own type (optional)
        /// </summary>
        [Serializable]
        public class MyResource
        {
            public string Name { get; set; }
        }
        /// <summary>
        /// A custom task of your own type deriving from the Task interface (optional)
        /// </summary>
        [Serializable]
        public class MyTask : Task
        {
            public MyTask(ProjectManager manager) : base() { Manager = manager; }

            private ProjectManager Manager { get; set; }

            //public int Number { get; set; }
            //public new TimeSpan Start { get { return base.Start; } set { Manager.SetStart(this, value); } }
            //public new TimeSpan End { get { return base.End; } set { Manager.SetEnd(this, value); } }
            //public new TimeSpan Duration { get { return base.Duration; } set { Manager.SetDuration(this, value); } }
            //public new float Complete { get { return base.Complete; } set { Manager.SetComplete(this, value); } }

            //public string Name { get; set; }
            //public string Info_Source { get; set; }
            //public string Info_Altitude { get; set; }  //고도
            //public string Info_Azimuth { get; set; } //방위각
            //public string Info_Elevation { get; set; } //고각
            //public string Info_RisingTime { get; set; } //뜨는 시각(KST)
            //public string Info_SetTime { get; set; } //지는 시각(KST)
            //public string Info_Keyhole { get; set; } //Keyhole(KST)
            //public string Info_SunAvoidance { get; set; } //태양회피
            //public string Info_MaxElevation { get; set; } //최대고각
            public Satellite sat;
        }
        #endregion custom task and resource  
        /////////////////////////////////////////////////////////////////////////////////////////


        const int SCHEDULE_LIST_SIZE = 5;
        private void task_setting()
        {

            // 임무 수행률.......Just a 테스트 
            /*
            task_sat[1].Complete = 0.9f;
            task_sat[2].Complete = 0.2f;
            task_sat[3].Complete = 0.5f;
            task_sat[4].Complete = 0.7f;
            task_sat[5].Complete = 0.1f;            
            */
            // Initialize the Chart with our ProjectManager and CreateTaskDelegate
            _mChart.Init(_mManager);
            _mChart.CreateTaskDelegate = delegate () { return new MyTask(_mManager); };

            // Attach event listeners for events we are interested in
            _mChart.TaskMouseOver += new EventHandler<TaskMouseEventArgs>(_mChart_TaskMouseOver);
            _mChart.TaskMouseOut += new EventHandler<TaskMouseEventArgs>(_mChart_TaskMouseOut);
            _mChart.TaskSelected += new EventHandler<TaskMouseEventArgs>(_mChart_TaskSelected);
            //_mChart.PaintOverlay += _mOverlay.ChartOverlayPainter;
            _mChart.AllowTaskDragDrop = false;   //Jason GUI에서 일정을 변경하지 못하게(관측 일정은 궤도 정보 파일로 부터 정해지므로)
            
            ///////Options
            //Demostrate splitting a task
            //_mManager.Split(task_sat[7], new MyTask(_mManager), new MyTask(_mManager), TimeSpan.FromDays(12));
            // set some tooltips to show the resources in each task
            //_mChart.SetToolTip(sat2, string.Join(", ", _mManager.ResourcesOf(sat2).Select(x => (x as MyResource).Name)));
            //_mChart.SetToolTip(sat3, string.Join(", ", _mManager.ResourcesOf(sat3).Select(x => (x as MyResource).Name)));
            //_mChart.SetToolTip(sat7, string.Join(", ", _mManager.ResourcesOf(sat7).Select(x => (x as MyResource).Name)));

            // Set Time information
            var span = DateTime.Today - _mManager.Start;
            _mManager.Now = span; // set the "Now" marker at the correct date            
            _mChart.TimeResolution = TimeResolution.Day; // Set the chart to display in days in header


            // Init the rest of the UI
            TaskGridView.DataSource = new BindingSource(_mManager.Tasks, null);
            TaskGridView.CellClick += TaskGridView_CellClick;
            TaskGridView.RowHeadersVisible = false;
            TaskGridView.AutoResizeColumns();

        }


        //public static async Task TLEUpdate(string link, int name)
        //{            
        //    WebClient Client = new WebClient();
        //    Client.DownloadFile(link, TLE_Path + "\\" + tlenames[name]);
        //    Console.WriteLine($"TLEUpdate] TLE Link:[{link}] " + TLE_Path + "\\" + tlenames[name] + " was successfully downloaded!");
        //}

        public void GetTleFromUrl()
        {
#if TLE_FROM_CELESTRAK
            // Extracted from Celestrak        
            tleUrl.Add("url_Active", "https://celestrak.org/NORAD/elements/gp.php?GROUP=active&FORMAT=tle");

            var url = tleUrl.FirstOrDefault(x => x.Key == "url_Active").Value; // Remote URL            
            var provider = new RemoteTleProvider(true, new Uri(url)); // Create a provider
            var tles = provider.GetTles();   // Get every TLE
            var satellites = tles.Select(pair => new Satellite(pair.Value)).ToList();
#else   //////////////////////////////////////////////////////////////////////////

    #if  TLE_Class
            //----TleReader tleReader = new TleReader();
            if (tleReader.isCompleteListing)
               Console.WriteLine($">>>> LTE {string.Join("\n", tleReader.TleList)}");
    #endif

/*            
            var satellites = tles.Select(pair => new Satellite(pair.Value)).ToList();
            var satellites = new Satellite(pair.Value)).ToList();
            var tles = new List<string>();
*/
            
            string TLE_Path = System.IO.Path.Combine(Application.StartupPath, @"OrbitData\TLE\active.txt");
            // Create a provider
            var provider = new LocalTleProvider(true, TLE_Path);
            // Get every TLE
            var tles = provider.GetTles();
            var satellites = tles.Select(pair => new Satellite(pair.Value)).ToList();
#endif

            groundStation = new GroundStation(new GeodeticCoordinate(Angle.FromDegrees(LATITUDE_HERE), Angle.FromDegrees(LONGITUDE_HERE), 0));//현 관측소 위치설정
            drawicon = true;
            graph.DrawEllipse(new Pen(Color.Black), (int)(LONGITUDE_HERE * 2 + 360), 180 - (int)(LATITUDE_HERE * 2), 55, 55);
            drawicon = false;            
            SelectVisibleSatellite(satellites, groundStation, minAngle);
            ready = true; //Jason 최초 SelectedIndexChanged event 발생(False Alarm)을  무시하기 위한
        }

        public int SelectVisibleSatellite(List<Satellite> satellites, GroundStation gs, Angle minAngle)
        {
            Random random = new Random();
            int cnt = 0;


            visible = new List<Satellite>();
            visible.Clear();
            List<string> sat_list = new List<string>();
            
            foreach (var satellite in satellites)
            {
                try
                {
                    var satPos = satellite.Predict();
                    //Console.WriteLine($"Predict() POS:{satPos}  minAngle:[{minAngle}], satPos.Time:{satPos.Time} ");                    
                    if (gs.IsVisible(satPos, minAngle, satPos.Time))
                    {
                        visible.Add(satellite);
                        Console.WriteLine($"[O] Visible SAT     #{visible.Count}  {satellite.Name} >>> Predict() POS:{satPos}");
                    }
                    else
                    {
                        Console.WriteLine($"-- NoneVisible SAT #{visible.Count}  {satellite.Name}");
                    }
                }
                catch (Exception ex)
                {
                    // 위성 소멸 예외 처리
                    if (ex.Message.Contains("decay"))
                    {
                        Console.WriteLine($"[CSU_ObserveSchedule] Exception: Satellite {satellite.Name} has decayed.");
                    }
                    else
                    {
                    //    Console.WriteLine($"......throw.{ex.Message}....");
                    //throw;
                    }
                }                
                Console.WriteLine($" cnt [{cnt++}].....");
            }
            for (var i = 0; i < visible.Count; i++)
            {
                sat = visible[i];
                Console.WriteLine($"[CSU_ObserveSchedule] SATt Visible:[#{i}] {sat.Name}");
                //task_sat[i] = new MyTask(_mManager) { Name = $"{sat.Name} [#{i}]", Number = i, Info_Source = sat.Tle.IntDesignator, Info_Elevation = sat.Tle.Inclination.ToString() };
                task_sat[i] = new MyTask(_mManager) { Name = $"{sat.Name}]", /*Info_Altitude = sat.Tle.Inclination.ToString()*/  };
                _mManager.Add(task_sat[i]);

                _mManager.SetStart(task_sat[i], TimeSpan.FromDays(random.Next(1, 35)));
                _mManager.SetDuration(task_sat[i], TimeSpan.FromDays(random.Next(7, 63)));
                sat_list.Add(sat.Name);
                //listBox_satellite.Items.Add(sat.Name);
                //Console.WriteLine($"[CSU_ObserveSchedule] Sat Visible:[#{i}] {sat.Name}, Designator = {task_sat[i].Info_Source}  Elevation = {task_sat[i].Info_Elevation}");
            }
            return 1;
        }




        private void chartInit()
        {
            chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Polar;
            chart1.Series["Series1"].MarkerSize = 15;
            chart1.Series["Series1"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            chart1.Series["Series1"].Color = Color.Black;
            chart1.Series["Series1"].LabelBackColor = Color.Green;
            chart1.Series["Series1"]["PolarDrawingStyle"] = "Marker";
            chart1.Series["Series1"]["AreaDrawingStyle"] = "Polygon";
        }
        public void DrawDataOnPolarGraph(int longitude, int latitude, int mode)
        {

            chart1.Series["Series1"].Points.AddXY(longitude, latitude);
            Console.WriteLine($"DrawDataOnPolarGraph().....longitude {longitude}, latitude {latitude}");
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            tracking();
        }
        private void timer_Tick2(object sender, EventArgs e)
        {
            predicted_tracking();
        }

       
        private void tracking()
        {
            var observations = groundStation.Observe(sat, DateTime.Now);
            var ss = sat.Predict(DateTime.Now);
            Debug.WriteLine(ss);
            var longitude = Convert.ToInt32(ss.ToGeodetic().Longitude.Degrees);
            var latitude = Convert.ToInt32(ss.ToGeodetic().Latitude.Degrees);
            int mode = 1;
            Point projectedPoint = MercatorProjection(longitude, latitude);

            //graph.FillEllipse(new SolidBrush(Color.Red), projectedPoint.X, projectedPoint.Y, 10, 10);
            graph.DrawImage(satelliteIcon, projectedPoint.X, projectedPoint.Y, satelliteSize, satelliteSize);
            Console.WriteLine($"tracking().....graph.FillEllipse({projectedPoint.X},{projectedPoint.Y})");
            DrawDataOnPolarGraph(longitude, latitude, mode);
            //GroundTrack.Invalidate();
            this.Invalidate();
        }
        private void predicted_tracking()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = startDate.AddDays(1);
            TimeSpan predictionInterval = TimeSpan.FromHours(0.5);
            TimeSpan timeSpan = TimeSpan.FromSeconds(1);
            Point? previousPoint = null;

            int mode = 0;
            for (DateTime currentTime = startDate; currentTime <= endDate; currentTime += predictionInterval)
            {
                var ss = sat.Predict(currentTime);
                Debug.WriteLine(ss);
                var longitude = Convert.ToInt32(ss.ToGeodetic().Longitude.Degrees);
                var latitude = Convert.ToInt32(ss.ToGeodetic().Latitude.Degrees);
                Point projectedPoint = MercatorProjection(longitude, latitude);
                graph.FillEllipse(new SolidBrush(Color.DarkOrange), projectedPoint.X, projectedPoint.Y, 15, 15);

                //Color.DarkOrange
                //graph.DrawImage(satelliteIcon, projectedPoint.X, projectedPoint.Y, satelliteSize, satelliteSize);
                //graph.DrawEllipse(Pens.Black, projectedPoint.X, projectedPoint.Y, 25, 25);
                Console.WriteLine($"Cooordinates : [ {projectedPoint.X}, {projectedPoint.Y}] ");


                if (previousPoint.HasValue)
                {
                    //graph.DrawLine(new Pen(Color.Yellow,5), previousPoint.Value, projectedPoint);
                }
                previousPoint = projectedPoint;
                DrawDataOnPolarGraph(longitude, latitude, mode);
            }

            // 폼 다시 그리기
            Invalidate();
        }

         private void CSU_ObseveSchedule_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            timer1.Stop();

            //Overriding Closing this with Alt+F4 
            //Closing Only by MainFrom closing.
            e.Cancel = true; 
            
        }

        private void TaskGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            timer.Stop();
            timer1.Stop();
            GroundTrack.Image = null;
            GroundTrack.Update();
            chart1.Series["Series1"].Points.Clear();

            Console.WriteLine($"===================================================================================================");

            //_mTaskGrid.SelectedObjects = _mChart.SelectedTasks.Select(x => _mManager.IsPart(x) ? _mManager.SplitTaskOf(x) : x).ToArray();
            //_mResourceGrid.Items.Clear();
            //_mResourceGrid.Items.AddRange(_mManager.ResourcesOf(e.Task).Select(x => new ListViewItem(((MyResource)x).Name)).ToArray());

            //DataGridViewRow row = TaskGridView.Rows[e.RowIndex];            
            //string name = row.Cells[0].Value.ToString();



            //Console.WriteLine($"= [Sat TLE] {name} ----------------------------------------------------  =");
            sat = visible[_mManager.IndexOf(task_sat[e.RowIndex])];
            Console.WriteLine($"{sat.Tle}");
            Console.WriteLine($"-------------------------------------------------------------------------");

            label_sat_name.Text = sat.Tle.Name;
            //label_sat_src.Text = "Source?";
            //label_sat_altitude.Text = sat.Name;
            //label_sat_az.Text = sat.Tle.Line1.Atitude. ie.Task.atitude.ToString();/
            //label_sat_el.Text = longitude.ToString();//
            //var ss = sat.Predict();
            label_sat_riseTime.Text = "06.12.23";//
            label_sat_setTime.Text = "18.34.45";//
            //label_sat_keyhole.Text = "KeyHole?";
            //label_sat_sunAvoidance.Text = "SunAvoida";
            //label_sat_maxEl.Text = ss.ToGeodetic().Altitude.ToString();


            //Map_Box.Update();
            //this.Text = this.Text.Remove(30, 50);
            //this.Text = this.Text.Insert(30, sat.ToString());
            //satellite_selected = TaskGridView.SelectedIndex;
            tracking();
            predicted_tracking();
            Console.WriteLine($"===================================================================================================\n\n");
            timer.Start();
            timer1.Start();
        }


        private void dateTimePicker1_ValueChanged_1(object sender, EventArgs e)
        {
            DateTime dt1 = dateTimePicker1.Value;
            Console.WriteLine($"START :  {dt1.ToString()}");

            DateTime_Start.Text = dt1.ToString("yyyy 년 MM 월 dd 일 tt hh 시 mm 분");
        }


        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt1 = dateTimePicker2.Value;
            Console.WriteLine($"END :  {dt1.ToString()}");

            DateTime_End.Text = dt1.ToString("yyyy 년 MM 월 dd 일 tt hh 시 mm 분");
        }

        private void CSU_ObserveSchedule_KeyDown(object sender, KeyEventArgs e)
        { 
            if(e.KeyData == ( Keys.Alt | Keys.F4))
                e.Handled = false;

            if(e.Alt && e.KeyCode == Keys.F4)
                e.Handled = false;
        }


        private Point MercatorProjection(double longitude, double latitude)
        {
            const double longitude2map = 360;
            const double latitude2map = 170;

            double x = (longitude + 180) * (GroundTrack.Width / longitude2map);
            double y = ((-1) * latitude + 90) * (GroundTrack.Height / latitude2map);

            return new Point((int)x, (int)y);
        }


    }

}