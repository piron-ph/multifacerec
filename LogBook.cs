using System;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
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
using Emgu.CV.Face;
using System.Speech.Synthesis;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Data;
using System.Text.RegularExpressions;
using CRUD_Class.myclass;
using MySql.Data.MySqlClient;
using DlibDotNet;
using DlibDotNet.Extensions;
using System.Windows.Media.Media3D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static Emgu.CV.Face.FaceRecognizer;

namespace MultiFaceRec
{
    public partial class LogBook : Form
    {
        string finalname;
        Image<Bgr, Byte> currentFrame;
        VideoCapture grabber;
        CascadeClassifier face;
        Image<Gray, byte> result;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        DlibDotNet.Rectangle roi = new DlibDotNet.Rectangle(255, 0, 0, 2);
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

        CRUD crud = new CRUD();

        //Temperature
        private SerialPort myport;
        private string data;

        SpeechSynthesizer synth = new SpeechSynthesizer();

        public LogBook()
        {
            InitializeComponent();
            try
            {
                // DATETIME & TIMER
                date_login.Text = DateTime.Now.ToString("MMM dd yyy");
                time_login.Text = DateTime.Now.ToString("hh:mm tt");
                login_timer.Start();
                datetime_timer.Start();
                main_timer.Start();
                date_today.Text = DateTime.Now.ToString("MMM dd yyy");

                FontFace fontFace = FontFace.HersheyTriplex;
                double fontScale = 0.6d;
                MCvScalar fontColor = new MCvScalar(255, 255, 255); // Assuming white color
                int thickness = 1;

                // Load haarcascades for face detection
                if (File.Exists("haarcascade_frontalface_default.xml"))
                {
                    face = new CascadeClassifier("haarcascade_frontalface_default.xml");
                }
                else
                {
                    MessageBox.Show("Haarcascade file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (connection2 != null)
                {
                    string qry = "SELECT Username, Image_name FROM face_enrollment_tb";
                    MySqlCommand cmd = new MySqlCommand(qry, connection2);
                    if (connection2.State != ConnectionState.Open)
                    {
                        connection2.Open();
                    }
                    MySqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["Image_name"] != null && dr["Username"] != null)
                        {
                            string imageName = dr["Image_name"].ToString();
                            if (File.Exists(Application.StartupPath + "/TrainedFaces/" + imageName))
                            {
                                trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + imageName));
                                labels.Add(dr["Username"].ToString());
                            }
                            else
                            {
                                MessageBox.Show("Image file '" + imageName + "' not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    dr.Close();
                    connection2.Close();
                }
                else
                {
                    MessageBox.Show("Database connection is null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
        private IEnumerable<FilterInfo> filter_collection;
        private VideoCaptureDevice video_capture_device;
        private DlibDotNet.Rectangle faceRect;

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

                // DEFAULT TIMEIN
                string qry1 = "SELECT Default_Timein FROM timeout_tb";
                MySqlCommand cmd1 = new MySqlCommand(qry1, connection2);
                if (connection2 != null && connection2.State != ConnectionState.Open)
                {
                    connection2.Open();
                }
                MySqlDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    timein_lbl.Text = (dr1["Default_Timein"].ToString());
                }
                dr1.Close();
                if (connection2 != null && connection2.State != ConnectionState.Closed)
                {
                    connection2.Close();
                }

                // CAPTURE DEVICES
                if (filter_collection == null)
                {
                    FilterInfoCollection filter_collection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                    foreach (FilterInfo filter_info in filter_collection)
                    {
                        video_src_cmbx.Items.Add(filter_info.Name);
                        video_src_cmbx.SelectedIndex = 0;
                        video_capture_device = new VideoCaptureDevice();
                    }
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
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private bool IsLiveFunction(Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> currentFrame, DlibDotNet.Rectangle faceRect)
        {
            bool isLive = false;

            try
            {
                if (currentFrame == null || faceRect == null)
                {
                    Console.WriteLine("Current frame or face rectangle is null.");
                    return false;
                }

                System.Drawing.Rectangle emguRect = new System.Drawing.Rectangle((int)faceRect.Left, (int)faceRect.Top, (int)faceRect.Width, (int)faceRect.Height);

                // Convert the current frame to grayscale
                Emgu.CV.Image<Emgu.CV.Structure.Gray, byte> grayFrame = currentFrame.Convert<Emgu.CV.Structure.Gray, byte>();

                // Extract region of interest (ROI) from the current frame using the face rectangle
                Emgu.CV.Image<Emgu.CV.Structure.Gray, byte> faceImage = grayFrame.Copy(emguRect);

                double ear = CalculateEyeAspectRatio(faceImage);
                double blinkThreshold = 0.2;
                bool blinkingDetected = ear < blinkThreshold;
                DlibDotNet.Rectangle dlibRect = new DlibDotNet.Rectangle(emguRect.Left, emguRect.Top, emguRect.Right, emguRect.Bottom);
                bool headMovementDetected = DetectHeadMovement(grayFrame, dlibRect);

                // Determine if the face is live based on the results of blinking and head movement detection
                isLive = blinkingDetected && headMovementDetected;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error in IsLiveFunction: {ex.Message}");
            }

            return isLive;
        }



        private bool DetectBlinking(Image<Gray, byte> currentFrame)
        {
            try
            {
                if (currentFrame == null)
                {
                    Console.WriteLine("Current frame is null.");
                    return false;
                }

                double ear = CalculateEyeAspectRatio(currentFrame);
                double blinkThreshold = 0.2;
                bool isBlink = ear < blinkThreshold;
                return isBlink;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error in DetectBlinking: {ex.Message}");
                return false;
            }
        }


        private double CalculateEyeAspectRatio(Image<Gray, byte> currentFrame)
        {
            try
            {
                if (currentFrame == null)
                {
                    Console.WriteLine("Current frame is null.");
                    return 0.0;
                }
                PointF p1 = new PointF(0, 0);// Left eye inner corner
                PointF p2 = new PointF(0, 0);// Left eye outer corner
                PointF p3 = new PointF(0, 0);// Right eye inner corner
                PointF p4 = new PointF(0, 0);// Right eye outer corner
                PointF p5 = new PointF(0, 0);// Left eye top
                PointF p6 = new PointF(0, 0);// Left eye bottom

                double d1 = Math.Sqrt(Math.Pow(p2.X - p6.X, 2) + Math.Pow(p2.Y - p6.Y, 2));
                double d2 = Math.Sqrt(Math.Pow(p3.X - p5.X, 2) + Math.Pow(p3.Y - p5.Y, 2));
                double d3 = Math.Sqrt(Math.Pow(p1.X - p4.X, 2) + Math.Pow(p1.Y - p4.Y, 2));

                double ear = (d1 + d2) / (2 * d3);
                return ear;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error in CalculateEyeAspectRatio: {ex.Message}");
                return 0.0;
            }
        }


        private bool DetectHeadMovement(Image<Gray, byte> currentFrame, DlibDotNet.Rectangle roi)
        {
            bool isHeadMoved = false;

            try
            {
                if (currentFrame == null || roi == null)
                {
                    Console.WriteLine("Current frame or ROI is null.");
                    return false;
                }

                // Convert Emgu.CV image to System.Drawing.Bitmap
                Bitmap bitmap = currentFrame.ToBitmap();

                if (bitmap == null)
                {
                    Console.WriteLine("Failed to convert current frame to bitmap.");
                    return false;
                }

                // Convert System.Drawing.Bitmap to DlibDotNet.Array2D<byte>
                DlibDotNet.Array2D<byte> dlibImage = BitmapToDlibArray2D(bitmap);

                if (dlibImage == null)
                {
                    Console.WriteLine("Failed to convert bitmap to DlibDotNet.Array2D<byte>.");
                    return false;
                }

                // Load shape predictor model
                var shapePredictorModelFilePath = "shape_predictor_68_face_landmarks.dat";
                if (!File.Exists(shapePredictorModelFilePath))
                {
                    Console.WriteLine("Shape predictor model file not found.");
                    return false;
                }

                var shapePredictor = ShapePredictor.Deserialize(shapePredictorModelFilePath);

                if (shapePredictor == null)
                {
                    Console.WriteLine("Failed to deserialize shape predictor model.");
                    return false;
                }

                // Detect facial landmarks within the ROI
                var shape = shapePredictor.Detect(dlibImage, roi);

                if (shape == null)
                {
                    Console.WriteLine("Failed to detect facial landmarks.");
                    return false;
                }

                // Convert DlibDotNet.Point to System.Drawing.PointF
                var nosePoint = ToPointF(shape.GetPart(30));
                var leftEyePoint = ToPointF(shape.GetPart(36));
                var rightEyePoint = ToPointF(shape.GetPart(45));
                var mouthPoint = ToPointF(shape.GetPart(66));

                // Calculate angles between facial landmarks
                double angleEyesNose = CalculateAngleBetweenPoints(nosePoint, leftEyePoint, rightEyePoint);
                double angleEyesMouth = CalculateAngleBetweenPoints(mouthPoint, leftEyePoint, rightEyePoint);
                double angleThreshold = 15;

                // Check if head movement exceeds the threshold
                if (angleEyesNose > angleThreshold || angleEyesMouth > angleThreshold)
                {
                    isHeadMoved = true;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                Console.WriteLine($"Error in DetectHeadMovement: {ex.Message}");
            }

            return isHeadMoved;
        }


        // Helper method to convert Bitmap to DlibDotNet.Array2D<byte>
        private DlibDotNet.Array2D<byte> BitmapToDlibArray2D(Bitmap bitmap)
        {
            try
            {
                if (bitmap == null)
                {
                    Console.WriteLine("Bitmap is null.");
                    return null;
                }

                // Convert Bitmap to byte array
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                BitmapData bmpData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);

                if (bmpData == null)
                {
                    Console.WriteLine("Failed to lock bits of the bitmap.");
                    return null;
                }

                int numBytes = bmpData.Stride * bitmap.Height;
                byte[] imageData = new byte[numBytes];
                Marshal.Copy(bmpData.Scan0, imageData, 0, numBytes);
                bitmap.UnlockBits(bmpData);

                // Convert byte array to DlibDotNet.Array2D<byte>
                DlibDotNet.Array2D<byte> dlibImage = DlibDotNet.Dlib.LoadImageData<byte>(imageData, (uint)bitmap.Height, (uint)bitmap.Width, (uint)bmpData.Stride);

                return dlibImage;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error in BitmapToDlibArray2D: {ex.Message}");
                return null;
            }
        }





        private System.Drawing.PointF ToPointF(DlibDotNet.Point dlibPoint)
        {
            try
            {
                if (dlibPoint == null)
                {
                    Console.WriteLine("DlibPoint is null.");
                    return PointF.Empty;
                }

                return new System.Drawing.PointF((float)dlibPoint.X, (float)dlibPoint.Y);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error in ToPointF: {ex.Message}");
                return PointF.Empty;
            }
        }

        private double CalculateAngleBetweenPoints(PointF centerPoint, PointF point1, PointF point2)
        {
            try
            {
                if (centerPoint == null || point1 == null || point2 == null)
                {
                    Console.WriteLine("One or more points are null.");
                    return 0.0;
                }

                double angleRadians = Math.Atan2(point2.Y - centerPoint.Y, point2.X - centerPoint.X) -
                                      Math.Atan2(point1.Y - centerPoint.Y, point1.X - centerPoint.X);
                double angleDegrees = angleRadians * (180 / Math.PI);
                if (angleDegrees < 0)
                {
                    angleDegrees += 360;
                }
                return angleDegrees;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error in CalculateAngleBetweenPoints: {ex.Message}");
                return 0.0;
            }
        }


        private void login_timer_Tick(object sender, EventArgs e)
        {
            try
            {
                pwede_na_magout = false;
                if (!string.IsNullOrEmpty(identified_name_lbl.Text) )
                {

                    //----------------------------TIMEIN 1------------------------
                    bool checkuser = false;
                    int face_detected = Convert.ToInt32(face_detected_lbl.Text);
                    currentFrame = grabber.QueryFrame().ToImage<Bgr, byte>();
                    CvInvoke.Resize(currentFrame, currentFrame, new Size(320, 240), 0, 0, Inter.Cubic);
                    bool isLive = IsLiveFunction(currentFrame, faceRect);
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


                    if (checkuser == false && !string.IsNullOrEmpty(lname.Text) && (face_detected == 1 && isLive) && (DateTime.Parse(time_login.Text, CultureInfo.InvariantCulture) < DateTime.Parse("11:59 AM", CultureInfo.InvariantCulture)))
                    {
                        MessageBox.Show("Blink once and move your head to log in.", "Instructions", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        LOGIN();
                        READ_RECORD();
                    }
                    else
                    {
                        // If blinking or head movement is not detected
                        DialogResult result = MessageBox.Show("Failed to detect real face. Would you like to try again?", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error); ;

                        if (result == DialogResult.Retry)
                        {
                            // Retry
                            login_timer_Tick(sender, e);
                        }
                        else
                        {
                            // Exit
                            Application.Exit();
                        }
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
                        if ((checkuser == true && pwede_na_magout == true) && (!string.IsNullOrEmpty(lname.Text) && face_detected == 1 && isLive) && (DateTime.Parse(time_login.Text, CultureInfo.InvariantCulture) >= DateTime.Parse(set_timeout, CultureInfo.InvariantCulture)) && (DateTime.Parse(time_login.Text, CultureInfo.InvariantCulture) <= DateTime.Parse("11:59 AM", CultureInfo.InvariantCulture)))
                        {
                            MessageBox.Show("Blink once and move your head to log in.", "Instructions", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    if (!string.IsNullOrEmpty(lname.Text) && (face_detected == 1 && isLive) && (DateTime.Parse(time_login.Text, CultureInfo.InvariantCulture) >= DateTime.Parse("12:00 PM", CultureInfo.InvariantCulture)))
                    {
                        LOGIN2();
                        READ_RECORD();
                    }
                    else
                    {
                        // If blinking or head movement is not detected
                        DialogResult result = MessageBox.Show("Failed to detect real face. Would you like to try again?", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error); ;

                        if (result == DialogResult.Retry)
                        {
                            // Retry
                            login_timer_Tick(sender, e);
                        }
                        else
                        {
                            // Exit
                            Application.Exit();
                        }
                    }

                    //----------------------------TIMEOUT 2------------------------
                    if (check2 == true)
                    {
                        if ((may_login_2 == true && pwede_na_magout2 == true) && (!string.IsNullOrEmpty(lname.Text) && face_detected == 1 && isLive) && (DateTime.Parse(time_login.Text, CultureInfo.InvariantCulture) >= DateTime.Parse(set_timeout2, CultureInfo.InvariantCulture)) && (DateTime.Parse(time_login.Text, CultureInfo.InvariantCulture) > DateTime.Parse("12:00 PM", CultureInfo.InvariantCulture)))
                        {
                            LOGOUT2();
                            READ_RECORD();
                        }
                    }
                    else
                    {
                        // If blinking or head movement is not detected
                        DialogResult result = MessageBox.Show("Failed to detect real face. Would you like to try again?", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error); ;

                        if (result == DialogResult.Retry)
                        {
                            // Retry
                            login_timer_Tick(sender, e);
                        }
                        else
                        {
                            // Exit
                            Application.Exit();
                        }
                    }

                }
                else
                {
                    // User not recognized or not in database
                    DialogResult result = MessageBox.Show("You don't exist in the system or in the database, this could be an error, please try again or exit", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (result == DialogResult.Retry)
                    {
                        // Retry
                        login_timer_Tick(sender, e);
                    }
                    else
                    {
                        // Exit
                        Application.Exit();
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close data reader if it's not null
                if (drr != null && !drr.IsClosed)
                {
                    drr.Close();
                }

                // Close connection if it's not null and open
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }

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
                // Temperature Timer
                temp_timer.Stop();

                Cursor.Current = Cursors.WaitCursor;
                //Initialize the capture device
                grabber = new VideoCapture(num_of_device_cmbx.SelectedIndex);
                grabber.Set(Emgu.CV.CvEnum.CapProp.Fps, 400);
                grabber.Set(Emgu.CV.CvEnum.CapProp.FrameHeight, 240);
                grabber.Set(Emgu.CV.CvEnum.CapProp.FrameWidth, 320);
                grabber.QueryFrame();
                //Initialize the FrameGraber event
                grabber.ImageGrabbed += new EventHandler(FrameGrabber);
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
                panel2.Size = new System.Drawing.Size(635, 44);
                label12.Location = new System.Drawing.Point(405, 21);
                groupBox4.Size = new System.Drawing.Size(635, 605);
                label11.Location = new System.Drawing.Point(255, 15);
                record_dgv.Size = new System.Drawing.Size(610, 553);
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
                panel2.Size = new System.Drawing.Size(415, 44);
                label12.Location = new System.Drawing.Point(295, 21);
                groupBox4.Size = new System.Drawing.Size(415, 337);
                label11.Location = new System.Drawing.Point(154, 15);
                record_dgv.Size = new System.Drawing.Size(377, 282);
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

                // Get the current frame from the capture device
                Mat frame = grabber.QueryFrame();
                Emgu.CV.Image<Emgu.CV.Structure.Bgr, byte> currentFrame = frame.ToImage<Emgu.CV.Structure.Bgr, byte>().Resize(320, 240, Emgu.CV.CvEnum.Inter.Cubic);

                // Convert it to Grayscale
                Emgu.CV.Image<Emgu.CV.Structure.Gray, byte> gray = currentFrame.Convert<Emgu.CV.Structure.Gray, byte>();

                // Face Detector
                System.Drawing.Rectangle[] facesDetected = face.DetectMultiScale(gray);

                // Action for each element detected
                foreach (System.Drawing.Rectangle rect in facesDetected)
                {
                    t = t + 1;
                    Emgu.CV.Image<Emgu.CV.Structure.Gray, byte> result = gray.GetSubRect(rect).Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);

                    // Draw the face detected in the 0th (gray) channel with light green color
                    currentFrame.Draw(rect, new Emgu.CV.Structure.Bgr(Color.LightGreen), 2);

                    // Check if the face is live
                    DlibDotNet.Rectangle dlibRect = new DlibDotNet.Rectangle(rect.Left, rect.Top, rect.Right, rect.Bottom);
                    bool isLive = IsLiveFunction(currentFrame, dlibRect);

                    if (trainingImages.Count != 0)
                    {
                        // TermCriteria for face recognition with numbers of trained images like maxIteration
                        MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                        // Convert List<Image<Gray, byte>> to IInputArrayOfArrays
                        IInputArrayOfArrays trainingData = new VectorOfMat(trainingImages.Select(img => img.Mat).ToArray());

                        // Convert List<string> to an array of string labels
                        List<string> labelsList = labels.ToList();

                        // Eigen face recognizer
                        EigenFaceRecognizer recognizer = new EigenFaceRecognizer();

                        // Convert List<string> to an array of string labels
                        string[] labelsArray = labelsList.ToArray();

                        // Create an UMat to hold the labels data
                        UMat labelsData = new UMat(labelsArray.Length, 1, DepthType.Cv32S, 1);
                        labelsData.SetTo(labelsArray);

                        // Train the recognizer with training data and labels
                        recognizer.Train(trainingData, labelsData);

                        // Perform recognition on the result image
                        PredictionResult predictionResult = recognizer.Predict(result);

                        // Get the predicted label and distance
                        int predictedLabel = predictionResult.Label;
                        float distance = (float)predictionResult.Distance;



                        // Ensure predicted label is within the range of available labels
                        if (predictedLabel >= 0 && predictedLabel < labelsList.Count)
                        {
                            string predictedName = labels[predictedLabel];

                            finalname = predictedName;

                            // Draw the label for each face detected and recognized
                            if (string.IsNullOrEmpty(predictedName))
                            {
                                currentFrame.Draw("Unknown", new System.Drawing.Point(rect.X - 2, rect.Y - 2), Emgu.CV.CvEnum.FontFace.HersheyPlain, 1.0, new Emgu.CV.Structure.Bgr(Color.Red));
                            }
                            else
                            {
                                currentFrame.Draw(predictedName, new System.Drawing.Point(rect.X - 2, rect.Y - 2), Emgu.CV.CvEnum.FontFace.HersheyPlain, 1.0, new Emgu.CV.Structure.Bgr(Color.Lime));
                            }
                        }
                        else
                        {
                            // Handle case when predicted label is out of range
                            finalname = "Unknown";
                            currentFrame.Draw("Unknown", new System.Drawing.Point(rect.X - 2, rect.Y - 2), Emgu.CV.CvEnum.FontFace.HersheyPlain, 1.0, new Emgu.CV.Structure.Bgr(Color.Red));
                        }
                    }






                    NamePersons[t - 1] = name;
                    NamePersons.Add("");

                    //Set the number of faces detected on the scene
                    face_detected_lbl.Text = facesDetected.Length.ToString();
                }
                t = 0;

                //Names concatenation of persons recognized
                foreach (string person in NamePersons)
                {
                    names += person;
                }
                //Show the faces procesed and recognized
                imageBoxFrameGrabber.Image = currentFrame;
                identified_name_lbl.Text = names;
                if (string.IsNullOrEmpty(identified_name_lbl.Text))
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