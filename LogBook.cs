using System;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Speech.Synthesis;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Data;
using System.Text.RegularExpressions;
using CRUD_Class.myclass;
using MySql.Data.MySqlClient;

namespace MultiFaceRec
{
    public partial class LogBook : Form
    {
        string finalname;
        Image<Bgr, Byte> currentFrame;
        public Emgu.CV.Capture grabber;
        HaarCascade face;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.6d, 0.6d);
        Image<Gray, byte> result;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, t;
        string name, names = null;

        int i = 0;
        string timein;
        string timeout;
        bool pwede_na_magout;
        string my_last_timein;

        DateTime login;
        DateTime logout;

        int temp_cnt = 0;
        string sr_name;
        bool check_monitor = false;

        string get_timein, set_timeout;
        bool check1 = false;
        string event_stat_1;
        bool checkuser = true;
        bool pwede_na_magout2;

        string get_timein2, set_timeout2;
        bool may_login_2 = false;
        bool check2 = false;
        bool may_laman_2, check3 = false;
        string full_name = "";

        //CONNECTION
        MySqlConnection connection = new MySqlConnection("datasource = localhost;port = 3306; Initial Catalog = 'e_log1'; username = root; password=");
        MySqlConnection connection2 = new MySqlConnection("datasource = localhost;port = 3306; Initial Catalog = 'e_log1'; username = root; password=");
        MySqlDataReader drr;
        MySqlDataReader drr2;

        //INITIALIZE OTHER FORM
        CRUD crud = new CRUD();

        //Temperature
        private SerialPort myport;
        private string data;

        FilterInfoCollection filter_collection;
        VideoCaptureDevice video_capture_device;

        SpeechSynthesizer synth = new SpeechSynthesizer();

        public LogBook()
        {
            InitializeComponent();
            try
            {
                //DATETIME & TIMER
                date_login.Text = DateTime.Now.ToString("MMM dd yyy");
                time_login.Text = DateTime.Now.ToString("hh:mm tt");
                login_timer.Start();
                datetime_timer.Start();
                main_timer.Start();
                date_today.Text = DateTime.Now.ToString("MMM dd yyy");

                //Load haarcascades for face detection
                face = new HaarCascade("haarcascade_frontalface_default.xml");

                string qry = "SELECT Username, Image_name FROM face_enrollment_tb";
                MySqlCommand cmd = new MySqlCommand(qry, connection2);
                if (connection2.State != ConnectionState.Open)
                {
                    connection2.Open();
                }
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + dr["Image_name"].ToString()));
                    labels.Add(dr["Username"].ToString());
                }
                dr.Close();
                connection2.Close();

                DISABLE_TEXTBX();
                READ_RECORD();
            }
            catch (Exception ex)
            {
                login_timer.Stop();
                datetime_timer.Stop();
                main_timer.Stop();
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        string logbook_date = DateTime.Now.ToString("MMM dd yyy");
        Review_Logbook review_logbook = new Review_Logbook();

        List<string> ComPortNames(String VID, String PID)
        {
            String pattern = String.Format("^VID_{0}.PID_{1}", VID, PID);
            Regex _rx = new Regex(pattern, RegexOptions.IgnoreCase);
            List<string> comports = new List<string>();
            RegistryKey rk1 = Registry.LocalMachine;
            RegistryKey rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");
            foreach (String s3 in rk2.GetSubKeyNames())
            {
                RegistryKey rk3 = rk2.OpenSubKey(s3);
                foreach (String s in rk3.GetSubKeyNames())
                {
                    if (_rx.Match(s).Success)
                    {
                        RegistryKey rk4 = rk3.OpenSubKey(s);
                        foreach (String s2 in rk4.GetSubKeyNames())
                        {
                            RegistryKey rk5 = rk4.OpenSubKey(s2);
                            RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                            comports.Add((string)rk6.GetValue("PortName"));
                        }
                    }
                }
            }
            return comports;
        }

        private void LogBook_Load(object sender, EventArgs e)
        {
            try
            {
                this.AutoScroll = true;
                hr_mn_lbl.Text = DateTime.Now.ToString("hh:mm");
                seconds_lbl.Text = DateTime.Now.ToString("ss");
                am_or_pm_lbl.Text = DateTime.Now.ToString("tt");
                date_lbl.Text = DateTime.Now.ToString("MMM dd yyy");
                day_lbl.Text = DateTime.Now.ToString("ddd");

                //DEFAULT TIMEIN
                string qry1 = "SELECT Default_Timein FROM timeout_tb";
                MySqlCommand cmd1 = new MySqlCommand(qry1, connection2);
                if (connection2.State != ConnectionState.Open)
                {
                    connection2.Open();
                }
                MySqlDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    timein_lbl.Text = (dr1["Default_Timein"].ToString());
                }
                dr1.Close();
                connection2.Close();

                //CAPTURE DEVICES
                filter_collection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                foreach (FilterInfo filter_info in filter_collection)
                {
                    video_src_cmbx.Items.Add(filter_info.Name);
                    video_src_cmbx.SelectedIndex = 0;
                    video_capture_device = new VideoCaptureDevice();
                }

                int devices = video_src_cmbx.Items.Count;
                int x = 0;
                do
                {
                    num_of_device_cmbx.Items.Add(x);
                    x++;

                } while (x < devices);

                num_of_device_cmbx.SelectedIndex = 1;

                timer1.Start();
                login = DateTime.Parse(time_login.Text);
                //logout = DateTime.ParseExact(time_out_lbl.Text, "hh:mm tt", CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                num_of_device_cmbx.SelectedIndex = 0;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                login = DateTime.Parse(time_login.Text);
                //logout = DateTime.ParseExact(time_out_lbl.Text, "hh:mm tt", CultureInfo.CurrentCulture);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                timer1.Stop();
            }
        }

        public void DISABLE_TEXTBX()
        {
            try
            {
                lname.Enabled = false;
                mname.Enabled = false;
                fname.Enabled = false;
                age_txbx.Enabled = false;
                gender.Enabled = false;
                brgy.Enabled = false;
                municiplty.Enabled = false;
                province.Enabled = false;
                contact.Enabled = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void identified_name_lbl_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Read_Data();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Read_Data()
        {
            try
            {
                //DISPLAY USER'S DATA
                string usern = identified_name_lbl.Text;
                string q = "SELECT Username, LastName, FirstName, MiddleInitial, Birthdate, Gender, Barangay, Municipality, Province, Contact_No FROM register_tb WHERE Username LIKE '" + usern + "'";
                MySqlCommand cmd1 = new MySqlCommand(q, connection);
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                MySqlDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    username.Text = dr1["Username"].ToString();
                    lname.Text = dr1["LastName"].ToString();
                    fname.Text = dr1["FirstName"].ToString();
                    mname.Text = dr1["MiddleInitial"].ToString();
                    brgy.Text = dr1["Barangay"].ToString();
                    municiplty.Text = dr1["Municipality"].ToString();
                    province.Text = dr1["Province"].ToString();
                    DOB.Text = dr1["Birthdate"].ToString();
                    gender.Text = dr1["Gender"].ToString();
                    contact.Text = dr1["Contact_No"].ToString();
                }
                TimeSpan age = DateTime.Now - DOB.Value;
                int years = DateTime.Now.Year - DOB.Value.Year;
                if (DOB.Value.AddYears(years) > DateTime.Now) years--;
                {
                    age_txbx.Text = years.ToString();
                }
                dr1.Close();
                connection.Close();
            }
            

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void login_timer_Tick(object sender, EventArgs e)
        {
            try
            {
                pwede_na_magout = false;
                if (!string.IsNullOrEmpty(identified_name_lbl.Text))
                {

                    //----------------------------TIMEIN 1------------------------
                    bool checkuser = false;
                    int face_detected = Convert.ToInt32(face_detected_lbl.Text);
                    string querry = "SELECT Username, Date, Time_In FROM logbook_tb";
                    MySqlCommand cmd = new MySqlCommand(querry, connection);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    drr = cmd.ExecuteReader();
                    while (drr.Read())
                    {
                        if (username.Text == (drr["Username"].ToString()) && date_today.Text == (drr["Date"].ToString()))
                        {
                            checkuser = true;
                        }
                    }
                    drr.Close();
                    connection.Close();


                    if (checkuser == false && !string.IsNullOrEmpty(lname.Text) && (face_detected == 1) && (DateTime.Parse(time_login.Text, CultureInfo.InvariantCulture) < DateTime.Parse("11:59 AM", CultureInfo.InvariantCulture)))
                    {
                        LOGIN();
                        READ_RECORD();
                    }

                    //----------------------------TIMEOUT 1 ------------------------

                    using (MySqlConnection connection2 = new MySqlConnection("datasource = localhost; port = 3306; Initial Catalog = 'e_log1'; username = root; password="))
                    {
                        MySqlCommand cmd2;
                        if (connection2.State != ConnectionState.Open)
                        {
                            connection2.Open();
                        }
                        string querry2 = "SELECT Time_In, Time_Out FROM logbook_tb WHERE Username='" + username.Text + "' AND Date='" + date_login.Text + "' ";
                        using (cmd2 = new MySqlCommand(querry2, connection2))
                        {
                            using (drr2 = cmd2.ExecuteReader())
                            {
                                if (drr2 != null)
                                {
                                    while (drr2.Read())
                                    {
                                        timein = (drr2["Time_In"].ToString());
                                        if (string.IsNullOrEmpty((drr2["Time_Out"].ToString())))
                                        {
                                            pwede_na_magout = true;
                                        }
                                        else
                                        {
                                            pwede_na_magout = false;

                                        }
                                    }
                                }
                            }
                            drr2.Close();
                        }
                    }
                    connection2.Close();

                    /*DateTime d1 = DateTime.ParseExact(timein, "hh:mm tt", CultureInfo.CurrentCulture);
                    DateTime d2 = d1.AddMinutes(1);*/

                    string querry001 = "SELECT * FROM logbook_tb WHERE Username='" + username.Text + "' AND Date='" + date_login.Text + "' ";
                    MySqlCommand cmd1 = new MySqlCommand(querry001, connection);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    drr = cmd1.ExecuteReader();
                    while (drr.Read())
                    {
                        string tm1 = drr["Time_In"].ToString();
                        if (tm1 != "")
                        {
                            check1 = true;
                            get_timein = drr["Time_In"].ToString();
                            string endTime = "0:1";
                            DateTime duration = DateTime.Parse(get_timein).Add(TimeSpan.Parse(endTime));
                            set_timeout = duration.ToString();
                        }
                        string tm2 = drr["Time_In_2"].ToString();
                        if (tm2 != "")
                        {
                            check2 = true;
                            get_timein2 = drr["Time_In_2"].ToString();
                            string endTime2 = "0:1";
                            DateTime duration2 = DateTime.Parse(get_timein2).Add(TimeSpan.Parse(endTime2));
                            set_timeout2 = duration2.ToString();
                        }
                    }
                    drr.Close();
                    connection.Close();

                    // LOGOUT 1
                    if (check1 == true)
                    {
                        if ((checkuser == true && pwede_na_magout == true) && (!string.IsNullOrEmpty(lname.Text) && face_detected == 1) && (DateTime.Parse(time_login.Text, CultureInfo.InvariantCulture) >= DateTime.Parse(set_timeout, CultureInfo.InvariantCulture)) && (DateTime.Parse(time_login.Text, CultureInfo.InvariantCulture) <= DateTime.Parse("11:59 AM", CultureInfo.InvariantCulture)))
                        {
                            LOGOUT();
                            READ_RECORD();
                        }
                    }

                    //----------------------------TIME-IN 2------------------------
                    //CHECK IF USER HAS LOGIN2

                    string querry3 = "SELECT * FROM logbook_tb";
                    MySqlCommand cmd3 = new MySqlCommand(querry3, connection);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    drr = cmd3.ExecuteReader();
                    while (drr.Read())
                    {
                        if (username.Text == (drr["Username"].ToString()) && date_today.Text == (drr["Date"].ToString()))
                        {
                            may_laman_2 = true;
                            if (!string.IsNullOrEmpty((drr["Time_In_2"].ToString())))
                            {
                                may_login_2 = true;
                            }
                            else
                            {
                                may_login_2 = false;
                            }
                            if (string.IsNullOrEmpty((drr["Time_Out_2"].ToString())))
                            {
                                pwede_na_magout2 = true;
                            }
                            else
                            {
                                pwede_na_magout2 = false;
                            }
                        }
                    }
                    drr.Close();
                    connection.Close();

                    //LOGIN 2 
                    if (!string.IsNullOrEmpty(lname.Text) && (face_detected == 1) && (DateTime.Parse(time_login.Text, CultureInfo.InvariantCulture) >= DateTime.Parse("12:00 PM", CultureInfo.InvariantCulture)))
                    {
                        LOGIN2();
                        READ_RECORD();
                    }

                    //----------------------------TIMEOUT 2------------------------
                    if (check2 == true)
                    {
                        if ((may_login_2 == true && pwede_na_magout2 == true) && (!string.IsNullOrEmpty(lname.Text) && face_detected == 1) && (DateTime.Parse(time_login.Text, CultureInfo.InvariantCulture) >= DateTime.Parse(set_timeout2, CultureInfo.InvariantCulture)) && (DateTime.Parse(time_login.Text, CultureInfo.InvariantCulture) > DateTime.Parse("12:00 PM", CultureInfo.InvariantCulture)))
                        {
                            LOGOUT2();
                            READ_RECORD();
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                drr.Close();
                connection.Close();
                login_timer.Stop();
            }


        }

        public void Clear_Dash()
        {
            dash_usern.Text = "";
            dash_lname.Text = "";
            dash_fname.Text = "";
            dash_middle.Text = "";
            dash_province.Text = "";
            dash_municipality.Text = "";
            dash_brgy.Text = "";
            dash_time.Text = "";
            dash_age.Text = "";
            gender_lbl.Text = "";
            timeout_dash_lbl.Text = "";
            dash_time_in_2.Text = "";
            dash_timeout_2.Text = "";
        }

        public void LOGIN()
        {
            try
            {
                dash_time.Visible = true;

                dash_timer.Start();
                Clear_Dash();
                dash_usern.Text = username.Text;
                dash_lname.Text = lname.Text;
                dash_fname.Text = fname.Text;
                dash_middle.Text = mname.Text;
                dash_province.Text = province.Text;
                dash_municipality.Text = municiplty.Text;
                dash_brgy.Text = brgy.Text;
                dash_time.Text = time_login.Text;
                dash_age.Text = age_txbx.Text;
                gender_lbl.Text = gender.Text;

                crud.username = username.Text;
                crud.lname = lname.Text;
                crud.fname = fname.Text;
                crud.mname = mname.Text;
                crud.age = age_txbx.Text;
                crud.gender = gender.Text;
                crud.brgy = brgy.Text;
                crud.municipality = municiplty.Text;
                crud.province = province.Text;
                crud.contact = contact.Text;
                crud.date_login = date_login.Text;
                crud.time_login = time_login.Text;

                crud.Insert_Record();


            }
            catch (Exception ex)
            {
                dash_timer.Stop();
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void LOGIN2()
        {
            try
            {
                if (may_laman_2 == true)
                {
                    if (may_login_2 == false)
                    {
                        timeout_dash_lbl.Text = "";
                        dash_time.Visible = true;

                        dash_timer.Start();
                        Clear_Dash();
                        dash_usern.Text = username.Text;
                        dash_lname.Text = lname.Text;
                        dash_fname.Text = fname.Text;
                        dash_middle.Text = mname.Text;
                        dash_province.Text = province.Text;
                        dash_municipality.Text = municiplty.Text;
                        dash_brgy.Text = brgy.Text;
                        dash_time_in_2.Text = time_login.Text;

                        crud.username = username.Text;
                        crud.lname = lname.Text;
                        crud.fname = fname.Text;
                        crud.mname = mname.Text;
                        crud.age = age_txbx.Text;
                        crud.gender = gender.Text;
                        crud.brgy = brgy.Text;
                        crud.municipality = municiplty.Text;
                        crud.province = province.Text;
                        crud.contact = contact.Text;
                        crud.date_login = date_login.Text;
                        crud.timein_2 = time_login.Text;

                        crud.Update_Time_in_2();
                    }
                }
                else
                {
                    timeout_dash_lbl.Text = "";
                    dash_time.Visible = true;

                    dash_timer.Start();
                    Clear_Dash();
                    dash_usern.Text = username.Text;
                    dash_lname.Text = lname.Text;
                    dash_fname.Text = fname.Text;
                    dash_middle.Text = mname.Text;
                    dash_province.Text = province.Text;
                    dash_municipality.Text = municiplty.Text;
                    dash_brgy.Text = brgy.Text;
                    dash_time_in_2.Text = time_login.Text;

                    crud.username = username.Text;
                    crud.lname = lname.Text;
                    crud.fname = fname.Text;
                    crud.mname = mname.Text;
                    crud.age = age_txbx.Text;
                    crud.gender = gender.Text;
                    crud.brgy = brgy.Text;
                    crud.municipality = municiplty.Text;
                    crud.province = province.Text;
                    crud.contact = contact.Text;
                    crud.date_login = date_login.Text;
                    crud.timein_2 = time_login.Text;

                    crud.Insert_New_Time_in_2();
                }


            }
            catch (Exception ex)
            {
                dash_timer.Stop();
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void LOGOUT()
        {
            try
            {

                dash_time.Visible = true;
                dash_time.BringToFront();

                dash_timer.Start();
                Clear_Dash();
                dash_usern.Text = username.Text;
                dash_lname.Text = lname.Text;
                dash_fname.Text = fname.Text;
                dash_middle.Text = mname.Text;
                dash_province.Text = province.Text;
                dash_municipality.Text = municiplty.Text;
                dash_brgy.Text = brgy.Text;
                dash_time.Text = time_login.Text;
                dash_age.Text = age_txbx.Text;
                gender_lbl.Text = gender.Text;
                timeout_dash_lbl.Text = time_login.Text;

                string querry = "SELECT * FROM logbook_tb WHERE Username='" + username.Text + "' AND Date='" + date_login.Text + "' ";
                MySqlCommand cmd = new MySqlCommand(querry, connection);
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                drr = cmd.ExecuteReader();
                while (drr.Read())
                {
                    dash_time.Text = drr["Time_In"].ToString();
                }
                drr.Close();
                connection.Close();

                crud.username = username.Text;
                crud.date_login = date_login.Text;
                crud.timeout = time_login.Text;
                crud.Logout();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void LOGOUT2()
        {
            try
            {

                string querry = "SELECT * FROM logbook_tb WHERE Username='" + username.Text + "' AND Date='" + date_login.Text + "' ";
                MySqlCommand cmd = new MySqlCommand(querry, connection);
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                drr = cmd.ExecuteReader();
                string tmo = "";
                while (drr.Read())
                {
                    dash_time_in_2.Text = drr["Time_In_2"].ToString();
                    tmo = drr["Time_Out_2"].ToString();
                }
                drr.Close();
                connection.Close();

                if (string.IsNullOrEmpty(tmo))
                {
                    dash_time.Visible = true;
                    dash_time.BringToFront();
                    dash_timeout_2.Visible = true;
                    dash_timeout_2.BringToFront();

                    dash_timer.Start();
                    Clear_Dash();
                    dash_usern.Text = username.Text;
                    dash_lname.Text = lname.Text;
                    dash_fname.Text = fname.Text;
                    dash_middle.Text = mname.Text;
                    dash_province.Text = province.Text;
                    dash_municipality.Text = municiplty.Text;
                    dash_brgy.Text = brgy.Text;

                    dash_timeout_2.Text = time_login.Text;

                    crud.username = username.Text;
                    crud.date_login = date_login.Text;
                    crud.timeout_2 = time_login.Text;
                    crud.Insert_Time_out_2();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void READ_RECORD()
        {
            try
            {
                record_dgv.DataSource = null;
                crud.logbook_date = logbook_date;
                crud.Read_Logbook();
                record_dgv.DataSource = crud.dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void enable_btn_MouseHover(object sender, EventArgs e)
        {
            enable_groupBox.BackColor = Color.DodgerBlue;
        }

        private void enable_btn_MouseLeave(object sender, EventArgs e)
        {
            enable_groupBox.BackColor = Color.SteelBlue;
        }

        private void review_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Form formBackground = new Form();

                using (Review_Logbook review_logbook = new Review_Logbook())
                {
                    formBackground.StartPosition = FormStartPosition.Manual;
                    formBackground.FormBorderStyle = FormBorderStyle.None;
                    formBackground.Opacity = .50d;
                    formBackground.BackColor = Color.Black;
                    formBackground.WindowState = FormWindowState.Maximized;
                    formBackground.Location = this.Location;
                    formBackground.ShowInTaskbar = false;
                    formBackground.Show();

                    review_logbook.Owner = formBackground;
                    review_logbook.ShowDialog();

                    formBackground.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void enable_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                enable_btn.Enabled = false;
                temp_timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Check_Temp_Port()
        {
            try
            {
                    //Temperature Timer
                    temp_timer.Stop();

                    Cursor.Current = Cursors.WaitCursor;
                    //Initialize the capture device
                    grabber = new Emgu.CV.Capture(num_of_device_cmbx.SelectedIndex);
                    grabber.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FPS, 400);
                    grabber.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 240);
                    grabber.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 320);
                    grabber.QueryFrame();
                    //Initialize the FrameGraber event
                    Application.Idle += new EventHandler(FrameGrabber);
                    enable_btn.Enabled = false;
                    num_of_device_cmbx.Enabled = false;
                    enable_lbl.ForeColor = Color.LightGray;
                
            }
            catch (Exception ex)
            {
                num_of_device_cmbx.Enabled = true;
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LogBook_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                grabber.QueryFrame(); Application.Idle -= FrameGrabber; grabber.Dispose();
                enable_btn.Enabled = true;
                enable_lbl.ForeColor = Color.White;
                imageBoxFrameGrabber.Image = null;
            }
            catch
            {
                //Nothing to do
            }
        }

        private void main_timer_Tick_1(object sender, EventArgs e)
        {
            try
            {

                record_dgv.DataSource = null;
                crud.logbook_date = date_login.Text;
                crud.Read_Logbook();
                record_dgv.DataSource = crud.dt;

                hr_mn_lbl.Text = DateTime.Now.ToString("hh:mm");
                seconds_lbl.Text = DateTime.Now.ToString("ss");
                am_or_pm_lbl.Text = DateTime.Now.ToString("tt");
                date_lbl.Text = DateTime.Now.ToString("MMM dd yyy");
                day_lbl.Text = DateTime.Now.ToString("ddd");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                main_timer.Stop();
            }
        }

        private void temp_timer_Tick(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                temp_cnt = temp_cnt + 1;
                if (temp_cnt >= 3)
                {
                    temp_timer.Stop();
                    Check_Temp_Port();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temp_timer.Stop();
            }
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void datetime_timer_Tick(object sender, EventArgs e)
        {
            try
            {
                date_login.Text = DateTime.Now.ToString("MMM dd yyy");
                time_login.Text = DateTime.Now.ToString("hh:mm tt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                datetime_timer.Stop();
            }
        }

        private void LogBook_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(myport)))
            {
                if (myport.IsOpen)
                {
                    myport.Close();
                }
            }
            if (grabber != null)
            {
                Application.Idle -= FrameGrabber;
                grabber.Dispose();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dash_timer_Tick(object sender, EventArgs e)
        {
            try
            {
                i = i + 1;
                if (i >= 10)
                {
                    Clear_Dash();
                    i = 0;
                    dash_timer.Stop();
                }
            }
            catch (Exception ex)
            {
                dash_timer.Stop();
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void dash_age_Click(object sender, EventArgs e)
        {

        }

        private void groupBox28_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try {
                grabber.QueryFrame(); Application.Idle -= FrameGrabber; grabber.Dispose();
                enable_btn.Enabled = true;
                enable_lbl.ForeColor = Color.White;
                imageBoxFrameGrabber.Image = null;
            }
            catch
            {
                //Nothing to do
            }
        }

        /*private void button1_Click(object sender, EventArgs e)
        {
            string current_str = DateTime.Now.ToString("hh:mm tt");
            DateTime current_time = DateTime.ParseExact(current_str, "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);

            DateTime default_time = DateTime.ParseExact(time_out_lbl.Text, "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            int res = DateTime.Compare(current_time, default_time);
            if (res == 0)
            {
                MessageBox.Show("0");
            }
            else if(res == 1)
            {
                MessageBox.Show("1");
            }
            else
            {
                MessageBox.Show("-1");
            }
        }*/

        private void monitor_btn_Click_1(object sender, EventArgs e)
        {
            if (check_monitor == false)
            {
                panel2.Size = new Size(635, 44);
                label12.Location = new Point(405, 21);
                groupBox4.Size = new Size(635, 605);
                label11.Location = new Point(255, 15);
                record_dgv.Size = new Size(610, 553);
                record_dgv.Visible = false;
                groupBox_a.Visible = true;
                groupBox_b.Visible = true;
                groupBox_c.Visible = true;
                groupBox_d.Visible = true;
                groupBox_e.Visible = true;
                review_btn.Visible = false;
                groupBox5.Visible = false;
                groupBox30.Visible = false;
                groupBox34.Visible = false;
                enable_groupBox.Visible = false;
                label2.Visible = false;
                face_detected_lbl.Visible = false;
                groupBox15.Visible = false;
                groupBox1.Visible = false;
                clock_grpbx.Visible = true;
                check_monitor = true;
            }
            else
            {
                panel2.Size = new Size(415, 44);
                label12.Location = new Point(295, 21);
                groupBox4.Size = new Size(415, 337);
                label11.Location = new Point(154, 15);
                record_dgv.Size = new Size(377, 282);
                record_dgv.Visible = true;
                groupBox_a.Visible = false;
                groupBox_b.Visible = false;
                groupBox_c.Visible = false;
                groupBox_d.Visible = false;
                groupBox_e.Visible = false;
                review_btn.Visible = true;
                groupBox5.Visible = true;
                groupBox5.BringToFront();
                enable_groupBox.Visible = true;
                groupBox30.Visible = true;
                groupBox34.Visible = true;
                label2.Visible = true;
                face_detected_lbl.Visible = true;
                groupBox15.Visible = true;
                groupBox1.Visible = true;
                clock_grpbx.Visible = false;
                check_monitor = false;
            }

        }

        void FrameGrabber(object sender, EventArgs e)
        {
            try
            {
                face_detected_lbl.Text = "0";
                NamePersons.Add("");


                //Get the current frame form capture device
                currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Convert it to Grayscale
                gray = currentFrame.Convert<Gray, Byte>();

                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
              face,
              1.2,
              15,
              Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
              new Size(30, 30));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    t = t + 1;
                    result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    //draw the face detected in the 0th (gray) channel with light green color
                    currentFrame.Draw(f.rect, new Bgr(Color.LightGreen), 2);


                    if (trainingImages.ToArray().Length != 0)
                    {

                        //TermCriteria for face recognition with numbers of trained images like maxIteration
                        MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                        //Eigen face recognizer
                        EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                           trainingImages.ToArray(),
                           labels.ToArray(),
                           3000,
                           ref termCrit);

                        name = recognizer.Recognize(result);

                        finalname = name;
                        //Draw the label for each face detected and recognized
                        if (string.IsNullOrEmpty(name))
                        {
                            currentFrame.Draw(string.IsNullOrEmpty(name) ? "Unknown" : name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Red));
                        }
                        else
                        {
                            currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Lime));
                        }
                    }

                        NamePersons[t - 1] = name;
                        NamePersons.Add("");

                    //Set the number of faces detected on the scene
                    face_detected_lbl.Text = facesDetected[0].Length.ToString();    

                }
                t = 0;

                //Names concatenation of persons recognized
                for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
                {
                    names = names + NamePersons[nnn];
                }
                //Show the faces procesed and recognized
                imageBoxFrameGrabber.Image = currentFrame;
                identified_name_lbl.Text = names;
                if(string.IsNullOrEmpty(identified_name_lbl.Text))
                {
                    lname.Clear();
                    fname.Clear();
                    mname.Clear();
                    brgy.Clear();
                    municiplty.Clear();
                    province.Clear();
                    age_txbx.Clear();
                    gender.Text = "";
                    contact.Clear();
                    username.Text = "";
                }
                names = "";
                //Clear the list(vector) of names
                NamePersons.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        } 
    }
}
