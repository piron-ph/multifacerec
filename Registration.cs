using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Media;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using CRUD_Class.myclass;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using Microsoft.Win32;
using Emgu.CV.Util;
using System.Speech.Synthesis;
using System.Globalization;
using System.IO.Ports;
using System.Windows.Forms;


namespace MultiFaceRec
{
    public partial class Registration : Form
    {
        string finalname;
        Image<Bgr, Byte> currentFrame;
        VideoCapture grabber;
        CascadeClassifier face;
        Image<Gray, byte> result1, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        int bilang;
        string name, names = null;
        string filepath;

        MySqlConnection connection1 = new MySqlConnection("datasource = localhost;port = 3306; Initial Catalog = 'e_log1'; username = root; password=");
        MySqlConnection connection2 = new MySqlConnection("datasource = localhost;port = 3306; Initial Catalog = 'e_log1'; username = root; password=");
        CRUD crud1 = new CRUD();
        Face_Enroll_Admin admin = new Face_Enroll_Admin();

        FilterInfoCollection filter_collection;
        VideoCaptureDevice video_capture_device;

        Regex reg = new Regex(@"([a-zA-Z]+)(\d+)");

        //MySQL Connection
        MySqlConnection connection = new MySqlConnection("datasource = localhost;port = 3306; Initial Catalog = 'e_log1'; username = root; password=");
        CRUD crud = new CRUD();
        string birthdate;
        bool check;
        string result;

        //IsLiveFace FUNCTION
        public bool IsLiveFace(Image<Gray, byte> faceImage)
        {
            // blink detection
            bool isBlink = IsBlinkDetected(faceImage);
            // headmovement detection
            bool isHeadMoved = IsHeadMoved(faceImage);
            // blink and head detection result =
            bool isLive = isBlink && isHeadMoved;
            return isLive;
        }

        // BLINK DETECTION
        private bool IsBlinkDetected(Image<Gray, byte> faceImage)
        {
            double ear = CalculateEyeAspectRatio(faceImage);
            double blinkThreshold = 0.2;
            bool isBlink = ear < blinkThreshold;
            return isBlink;
        }

        private double CalculateEyeAspectRatio(Image<Gray, byte> faceImage)
        {
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


        // HEADMOVEMENT DETECTION
        private bool IsHeadMoved(Image<Gray, byte> faceImage)
        {
            bool isHeadMoved = false;

            var shape = shapePredictor.Detect(faceImage);

            var nosePoint = shape.GetPart(30);
            var leftEyePoint = shape.GetPart(36);
            var rightEyePoint = shape.GetPart(45);
            var mouthPoint = shape.GetPart(66);

            double angleEyesNose = CalculateAngleBetweenPoints(nosePoint, leftEyePoint, rightEyePoint);
            double angleEyesMouth = CalculateAngleBetweenPoints(mouthPoint, leftEyePoint, rightEyePoint);
            double angleTreshold = 15;

            if (angleEyesNose > angleThreshold || angleEyesMouth > angleThreshold)
            {
                isHeadMoved = true;
            }

            return isHeadMoved;
        }

        private double CalculateAngleBetweenPoints(PointF centerPoint, PointF point1, PointF point2)
        {
            double angleRadians = Math.Atan2(point2.Y - centerPoint.Y, point2.X - centerPoint.X) -
                                  Math.Atan2(point1.Y - centerPoint.Y, point1.X - centerPoint.X);
            double angleDegrees = angleRadians * (180 / Math.PI);
            if (angleDegrees < 0)
            {
                angleDegrees += 360;
            }
            return angleDegrees;
        }



        public Registration()
        {
            try
            {
                InitializeComponent();
                //Disable capture button & list of names
                capture_btn.Enabled = false;
                names_cmbx.Enabled = false;

                FontFace fontFace = FontFace.HersheyTriplex;
                double fontScale = 0.6d;
                MCvScalar fontColor = new MCvScalar(255, 255, 255); // Assuming white color
                int thickness = 1;

                //Load haarcascades for face detection
                face = new HaarCascade("haarcascade_frontalface_default.xml");

                string qry = "SELECT Username, Image_name FROM face_enrollment_tb ORDER BY ID ASC";
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

                reg_date1.Text = DateTime.Now.ToString("MMM dd yyy").ToUpper();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void Registration_Load(object sender, EventArgs e)
        {
            try
            {
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

                Read_names();

                date_birth_bx.Format = DateTimePickerFormat.Custom;
                date_birth_bx.CustomFormat = "MMM dd yyy";

                usertype();
            }
            catch (Exception ex)
            {
                usertype();
                num_of_device_cmbx.SelectedIndex = 0;
            }

        }

        public void Read_names()
        {
            try
            {
                string stat = "Registered";
                string querry = "SELECT Username FROM register_tb WHERE Status='" + stat + "' ORDER BY Username ASC";
                MySqlCommand cmd = new MySqlCommand(querry, connection);
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                MySqlDataReader dr1 = cmd.ExecuteReader();
                while (dr1.Read())
                {
                    names_cmbx.Items.Add(dr1["Username"].ToString());
                }
                dr1.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (contact_txbx.Text.Length != 0)
                {
                    result = contact_txbx.Text.Substring(0, 2);
                }
                else
                {
                    result = "";
                }
                TimeSpan age = DateTime.Now - date_birth_bx.Value;
                int ages;
                int years = DateTime.Now.Year - date_birth_bx.Value.Year;
                if (date_birth_bx.Value.AddYears(years) > DateTime.Now) years--;
                {
                    ages = years;
                }
                DateTime DOB = date_birth_bx.Value;
                birthdate = DOB.ToString("MMM dd yyy").ToUpper();
                if (username_txbx.Text == "" || lname.Text == "" || fname.Text == "" || gender_combx.Text == "" || brgy_txbx.Text == "" || municipality_txbx.Text == "" || province_txbx.Text == "" || contact_txbx.Text == "" || combo_usertype.Text == "" || reg_date1.Text == "")
                {
                    MessageBox.Show("Incomplete Data! Please provide all the information needed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (ages < 7)
                {
                    MessageBox.Show("Age under 7 years old is not allowed! Please register a user atleast 7 years old and above!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (contact_txbx.TextLength < 11 || result != "09")
                {
                    MessageBox.Show("Invalid contact number!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    INSERT();
                    CLEAR1();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void INSERT()
        {
            try
            {
                crud.username = username_txbx.Text;
                crud.lname = lname.Text;
                crud.fname = fname.Text;
                crud.mname = m_initial.Text;
                crud.birthdate = birthdate;
                crud.gender = gender_combx.Text;
                crud.brgy = brgy_txbx.Text;
                crud.municipality = municipality_txbx.Text;
                crud.province = province_txbx.Text;
                crud.contact = contact_txbx.Text;
                crud.usertype = combo_usertype.Text;
                crud.status = "REGISTERED";
                crud.reg_date = reg_date1.Text;

                crud.Create_data();
            }
            catch (Exception ex)
            {

            }
        }

        public void CLEAR1()
        {
            try
            {
                username_txbx.Text = "";
                lname.Text = "";
                m_initial.Text = "";
                fname.Text = "";
                gender_combx.Text = "";
                brgy_txbx.Text = "";
                municipality_txbx.Text = "";
                province_txbx.Text = "";
                contact_txbx.Text = "";
                combo_usertype.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void usertype()
        {
            try
            {
                string querry = "Select Usertype from usertype_tb";
                MySqlCommand cmd = new MySqlCommand(querry, connection);
                connection.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    combo_usertype.Items.Add(dr["Usertype"].ToString());
                }
                dr.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void brgy()
        {
            try
            {
                brgy_txbx.AutoCompleteMode = AutoCompleteMode.Suggest;
                brgy_txbx.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection col = new AutoCompleteStringCollection();

                string querry = "Select Barangay from barangay_tb WHERE Province='" + province_txbx.Text + "' AND Municipality='" + municipality_txbx.Text + "'";
                MySqlCommand cmd = new MySqlCommand(querry, connection);
                connection.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    col.Add(dr["Barangay"].ToString());
                }
                dr.Close();

                brgy_txbx.AutoCompleteCustomSource = col;
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void muncplty()
        {
            try
            {
                municipality_txbx.AutoCompleteMode = AutoCompleteMode.Suggest;
                municipality_txbx.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection col = new AutoCompleteStringCollection();

                string querry = "Select Municipality from barangay_tb WHERE Province='" + province_txbx.Text + "'";
                MySqlCommand cmd = new MySqlCommand(querry, connection);
                connection.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    col.Add(dr["Municipality"].ToString());
                }
                dr.Close();

                municipality_txbx.AutoCompleteCustomSource = col;
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void province()
        {
            try
            {
                province_txbx.AutoCompleteMode = AutoCompleteMode.Suggest;
                province_txbx.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection col = new AutoCompleteStringCollection();

                string querry = "Select Province from province_tb";
                MySqlCommand cmd = new MySqlCommand(querry, connection);
                connection.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    col.Add(dr["Province"].ToString());
                }
                dr.Close();

                province_txbx.AutoCompleteCustomSource = col;
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void checkusername()
        {
            try
            {
                string querry = "Select Username from register_tb";
                MySqlCommand cmd = new MySqlCommand(querry, connection);
                connection.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (username_txbx.Text == (dr["Username"].ToString()))
                    {
                        MessageBox.Show("Username is already exist!");
                        int len = username_txbx.Text.Length;
                        int a = len - 1;
                        check = true;

                    }
                }
                dr.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void username_txbx_TextChanged(object sender, EventArgs e)
        {
            try
            {
                checkusername();
                if (check)
                {
                    username_txbx.Text = "";
                }
                check = false;

                if (!System.Text.RegularExpressions.Regex.IsMatch(username_txbx.Text, "^[a-zA-Z0-9]+$") && username_txbx.Text != "")
                {
                    MessageBox.Show("It only accepts alphabetical characters and numbers!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    username_txbx.Text = username_txbx.Text.Substring(0, username_txbx.Text.Length - 1);
                    username_txbx.SelectionStart = username_txbx.Text.Length;
                    username_txbx.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(lname.Text, "^[a-zA-Z\\s]+$") && lname.Text != "")
                {
                    MessageBox.Show("It only accepts alphabetical characters!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    lname.Text = lname.Text.Substring(0, lname.Text.Length - 1);
                    lname.SelectionStart = lname.Text.Length;
                    lname.SelectionLength = 0;
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
                //Initialize the capture device
                grabber = new Emgu.CV.Capture(num_of_device_cmbx.SelectedIndex);
                grabber.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FPS, 60);
                grabber.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 240);
                grabber.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 320);
                grabber.QueryFrame();
                //Initialize the FrameGraber event
                Application.Idle += new EventHandler(FrameGrabber);
                //Enable capture button
                capture_btn.Enabled = true;
                names_cmbx.Enabled = true;
                enable_btn.Enabled = false;
                num_of_device_cmbx.Enabled = false;
            }
            catch (Exception ex)
            {
                enable_btn.Enabled = true;
                num_of_device_cmbx.Enabled = true;
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void capture_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(names_cmbx.Text))
            {
                MessageBox.Show("Select username first!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = MessageBox.Show("The system will capture 2 samples, make sure the user's face is directly positioned on the front camera. Click ok to start.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    timer1.Start();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                int last_img_num;
                string qry001 = "SELECT Image_name FROM face_enrollment_tb ORDER BY ID DESC LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(qry001, connection2);
                if (connection2.State != ConnectionState.Open)
                {
                    connection2.Open();
                }
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    image_name_txbx.Text = (dr["Image_name"].ToString());
                }
                dr.Close();
                if (!string.IsNullOrEmpty(image_name_txbx.Text))
                {
                    Match rslt = reg.Match(image_name_txbx.Text);
                    string numberPart = rslt.Groups[2].ToString();
                    last_img_num = Convert.ToInt32(numberPart);
                }
                else
                {
                    last_img_num = 0;
                }
                connection2.Close();

                int i;
                int key_number;
                bilang = bilang + 1;

                //Trained face counter
                ContTrain = ContTrain + 1;

                //Get a gray frame from capture device
                gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

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
                    TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = result1.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                trainingImages.Add(TrainedFace);
                labels.Add(names_cmbx.SelectedItem.ToString());

                //Show face added in gray scale

                imageBox1.Image = result1.Resize(200, 200, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Write the number of triained faces in a file text for further load
                //File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + Environment.NewLine + "%");

                SoundPlayer player = new SoundPlayer(Application.StartupPath + "/SoundEffects/camera-shutter.wav");
                player.Play();

                //Write the labels of triained faces in a file text for further load
                for (i = 1; i < trainingImages.ToArray().Length + 1; i++)
                {
                    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + (last_img_num + 1) + ".bmp");
                    //File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + Environment.NewLine + "%");
                }
                if (bilang >= 2)
                {
                    key_number = 0;
                }
                else
                {
                    key_number = 1;
                }
                crud.key_num = key_number;
                crud.username = names_cmbx.Text;
                crud.image_name = "face" + (last_img_num + 1) + ".bmp";
                crud.Insert_face_enroll();

                if (crud.is_insert)
                {
                    crud.username = names_cmbx.Text;
                    crud.Verified_user();
                }
                else
                {
                    filepath = Application.StartupPath + "/TrainedFaces/face" + (last_img_num + 1) + ".bmp";
                    File.Delete(filepath);
                    bilang = 0;
                    timer1.Stop();
                    imageBox1.Image = null;
                    names_cmbx.SelectedIndex = -1;
                    names_cmbx.Items.Clear();
                    Read_names();
                    MessageBox.Show("Failed to register!", "MySQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

                if (bilang >= 2)
                {
                    bilang = 0;
                    timer1.Stop();
                    imageBox1.Image = null;
                    MessageBox.Show(names_cmbx.SelectedItem.ToString() + "´s face detected and added successfully:)", "Trained Image", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    names_cmbx.SelectedIndex = -1;
                    names_cmbx.Items.Clear();
                    Read_names();
                }

            }
            catch (Exception ex)
            {
                timer1.Stop();
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void FrameGrabber(object sender, EventArgs e)
        {
            try
            {
                //label3.Text = "0";
                //label4.Text = "";
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
                    result1 = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                    //draw the face detected in the 0th (gray) channel with light green color

                    currentFrame.Draw(f.rect, new Bgr(Color.LightGreen), 2);

                    bool isLiveFace = IsLiveFace(result1);

                    if (isLiveFace)
                    {
                        // Draw live face label in green color
                        currentFrame.Draw("Live Face", ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Lime));
                    }
                    else
                    {
                        // Draw not live face label in red color
                        currentFrame.Draw("Not Live Face", ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Red));
                    }


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

                        name = recognizer.Recognize(result1);

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
                    //label3.Text = facesDetected[0].Length.ToString();

                }
                t = 0;

                //Names concatenation of persons recognized
                for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
                {
                    names = names + NamePersons[nnn];
                }
                //Show the faces procesed and recognized
                imageBoxFrameGrabber.Image = currentFrame;
                names = "";
                //Clear the list(vector) of names
                NamePersons.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void face_enroll_admin_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Form formBackground = new Form();

                using (Face_Enroll_Admin face_admin = new Face_Enroll_Admin())
                {
                    formBackground.StartPosition = FormStartPosition.Manual;
                    formBackground.FormBorderStyle = FormBorderStyle.None;
                    formBackground.Opacity = .50d;
                    formBackground.BackColor = Color.Black;
                    formBackground.WindowState = FormWindowState.Maximized;
                    formBackground.Location = this.Location;
                    formBackground.ShowInTaskbar = false;
                    formBackground.Show();

                    face_admin.Owner = formBackground;
                    face_admin.ShowDialog();

                    formBackground.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void face_enroll_admin_MouseHover(object sender, EventArgs e)
        {
            panel_btn.BackColor = Color.DeepSkyBlue;
        }

        private void face_enroll_admin_MouseLeave(object sender, EventArgs e)
        {
            panel_btn.BackColor = Color.SteelBlue;
        }

        private void Registration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (grabber != null)
            {
                Application.Idle -= FrameGrabber;
                grabber.Dispose();
            }
        }

        private void enable_btn_MouseHover(object sender, EventArgs e)
        {
            enable_groupBox.BackColor = Color.DodgerBlue;
        }

        private void enable_btn_MouseLeave(object sender, EventArgs e)
        {
            enable_groupBox.BackColor = Color.Gray;
        }

        private void capture_btn_MouseHover(object sender, EventArgs e)
        {
            capture_groupBox.BackColor = Color.LightSeaGreen;
        }

        private void capture_btn_MouseLeave(object sender, EventArgs e)
        {
            capture_groupBox.BackColor = Color.Gray;
        }

        private void back_to_register_Click(object sender, EventArgs e)
        {
            face_enroll_header.Visible = false;
            face_enroll_header.SendToBack();
            face_enroll_grpbx.Visible = false;
            face_enroll_grpbx.SendToBack();

        }

        private void go_to_face_enrollment_Click(object sender, EventArgs e)
        {
            face_enroll_header.Visible = true;
            face_enroll_header.BringToFront();
            face_enroll_grpbx.Visible = true;
            face_enroll_grpbx.BringToFront();

            names_cmbx.Items.Clear();
            Read_names();

        }

        private void brgy_txbx_TextChanged(object sender, EventArgs e)
        {
            //brgy();
        }

        private void municipality_txbx_TextChanged(object sender, EventArgs e)
        {
            //muncplty();
        }

        private void fname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(fname.Text, "^[a-zA-Z\\s]+$") && fname.Text != "")
                {
                    MessageBox.Show("It only accepts alphabetical characters!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    fname.Text = fname.Text.Substring(0, fname.Text.Length - 1);
                    fname.SelectionStart = fname.Text.Length;
                    fname.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void m_initial_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(m_initial.Text, "^[a-zA-Z]+$") && m_initial.Text != "")
                {
                    MessageBox.Show("It only accepts alphabetical characters!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    m_initial.Text = m_initial.Text.Substring(0, m_initial.Text.Length - 1);
                    m_initial.SelectionStart = m_initial.Text.Length;
                    m_initial.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void gender_combx_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(gender_combx.Text, "^[a-zA-Z]+$") && gender_combx.Text != "")
                {
                    MessageBox.Show("It only accepts alphabetical characters!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    gender_combx.Text = gender_combx.Text.Substring(0, gender_combx.Text.Length - 1);
                    gender_combx.SelectionStart = gender_combx.Text.Length;
                    gender_combx.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void contact_txbx_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(contact_txbx.Text, "^[0-9]+$") && contact_txbx.Text != "")
                {
                    MessageBox.Show("It only accepts numeric values!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    contact_txbx.Text = contact_txbx.Text.Substring(0, contact_txbx.Text.Length - 1);
                    contact_txbx.SelectionStart = contact_txbx.Text.Length;
                    contact_txbx.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
