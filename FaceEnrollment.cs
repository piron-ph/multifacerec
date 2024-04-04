
//Face Detection and Recognition In Real Time
//Using EmguCV cross platform .Net wrapper to the Intel OpenCV image processing library for C#.Net
//Writed by Mark Lester F. Rabi & Justine Jade J. Rayala
//IT Student of MinSU Calapan City Campus
// marklesterrabi@gmail.com
//Regards from Arangin Naujan Oriental Mindoro, Philippines

using System;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Media;
using System.IO;
using System.Threading.Tasks;
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
using Microsoft.Office.Interop.Excel;
using System.Windows;
using DlibDotNet;
using static Emgu.CV.Face.FaceRecognizer;
using System.Linq;
namespace MultiFaceRec
{
    public partial class FrmPrincipal : Form
    {
        string finalname;
        Image<Bgr, Byte> currentFrame;
        VideoCapture grabber;
        CascadeClassifier face;
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        int bilang;
        string name, names = null;
        string filepath;


        MySqlConnection connection = new MySqlConnection("datasource=localhost;port=3306;Initial Catalog=e_log1;username=root;password=");
        MySqlConnection connection2 = new MySqlConnection("datasource=localhost;port=3306;Initial Catalog=e_log1;username=root;password=");
        CRUD crud = new CRUD();
        Face_Enroll_Admin admin = new Face_Enroll_Admin();

        FilterInfoCollection filter_collection;
        VideoCaptureDevice video_capture_device;

        Regex reg = new Regex(@"([a-zA-Z]+)(\d+)");

        public FrmPrincipal()
        {
            try
            {
                InitializeComponent();

                FontFace fontFace = FontFace.HersheyTriplex;
                double fontScale = 0.6d;
                MCvScalar fontColor = new MCvScalar(255, 255, 255); // Assuming white color
                int thickness = 1;

                // Render text on the image
                CvInvoke.PutText(currentFrame, "Your Text Here", new System.Drawing.Point(10, 50), fontFace, fontScale, fontColor, thickness);

                //Disable capture button & list of names
                capture_btn.Enabled = false;
                names_cmbx.Enabled = false;

                //Load haarcascades for face detection
                face = new CascadeClassifier("haarcascade_frontalface_default.xml");

                string qry = "SELECT Username, Image_name FROM face_enrollment_tb ORDER BY ID ASC";
                MySqlCommand cmd = new MySqlCommand(qry, connection2);
                if (connection2.State != ConnectionState.Open)
                {
                    connection2.Open();
                }
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string imageFilePath = System.Windows.Forms.Application.StartupPath + "/TrainedFaces/" + dr["Image_name"].ToString();
                    labels.Add(dr["Username"].ToString());
                }
                dr.Close();
                connection2.Close();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
  

        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            try
            {
                filter_collection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                foreach(FilterInfo filter_info in filter_collection)
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

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
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
                System.Windows.Forms.MessageBox.Show(ex.Message, "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //Initialize the capture device
                grabber = new VideoCapture(num_of_device_cmbx.SelectedIndex);
                grabber.Set(Emgu.CV.CvEnum.CapProp.Fps, 400);
                grabber.Set(Emgu.CV.CvEnum.CapProp.FrameHeight, 240);
                grabber.Set(Emgu.CV.CvEnum.CapProp.FrameWidth, 320);
                grabber.QueryFrame();
                //Initialize the FrameGraber event
                System.Windows.Forms.Application.Idle += new EventHandler(FrameGrabber);
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
                System.Windows.Forms.MessageBox.Show(ex.Message, "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }

        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(names_cmbx.Text))
                {
                    System.Windows.Forms.MessageBox.Show("Select username first!", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else
                {
                    DialogResult result = System.Windows.Forms.MessageBox.Show("The system will capture 11 samples, make sure the user's face is directly positioned on the front camera. Click ok to start.", "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    if (result == DialogResult.OK)
                    {
                        timer1.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
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
                var grayFrame = new Mat();
                grabber.Retrieve(grayFrame, 0);
                CvInvoke.CvtColor(grayFrame, grayFrame, ColorConversion.Bgr2Gray);

                // Face Detector
                var facesDetected = face.DetectMultiScale(
                    grayFrame,
                    1.2,
                    15,
                    new System.Drawing.Size(30, 30),
                    System.Drawing.Size.Empty);


                //Action for each element detected
                foreach (var faceRect in facesDetected)
                {
                    TrainedFace = currentFrame.Copy(faceRect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);
                trainingImages.Add(TrainedFace);
                labels.Add(names_cmbx.SelectedItem.ToString());

                //Show face added in gray scale

                imageBox1.Image = TrainedFace;

                TrainedFace.ToBitmap().Save(System.Windows.Forms.Application.StartupPath + "/TrainedFaces/face" + (last_img_num + 1) + ".bmp");

                //Write the number of triained faces in a file text for further load
                //File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + Environment.NewLine + "%");

                System.Media.SoundPlayer player = new System.Media.SoundPlayer(System.Windows.Forms.Application.StartupPath + "/SoundEffects/camera-shutter.wav");
                player.Play();

                //Write the labels of triained faces in a file text for further load
                for (i = 1; i < trainingImages.ToArray().Length + 1; i++)
                {
                    trainingImages.ToArray()[i - 1].Save(System.Windows.Forms.Application.StartupPath + "/TrainedFaces/face" + (last_img_num + 1) + ".bmp");
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
                    filepath = System.Windows.Forms.Application.StartupPath + "/TrainedFaces/face" + (last_img_num + 1) + ".bmp";
                    File.Delete(filepath);
                    bilang = 0;
                    timer1.Stop();
                    imageBox1.Image = null;
                    names_cmbx.SelectedIndex = -1;
                    names_cmbx.Items.Clear();
                    Read_names();
                    System.Windows.Forms.MessageBox.Show("Failed to register!", "MySQL Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                }

                if (bilang >= 11)
                {
                    bilang = 0;
                    timer1.Stop();
                    imageBox1.Image = null;
                    System.Windows.Forms.MessageBox.Show(names_cmbx.SelectedItem.ToString() + "´s face detected and added successfully:)", "Trained Image", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    names_cmbx.SelectedIndex = -1;
                    names_cmbx.Items.Clear();
                    Read_names();
                }

            }
            catch (Exception ex)
            {
                timer1.Stop();
                System.Windows.Forms.MessageBox.Show(ex.Message, "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }

        }

        void FrameGrabber(object sender, EventArgs e)
        {
            try
            {
                label3.Text = "0";
                NamePersons.Add("");

                // Null checks
                if (grabber == null)
                {
                    Console.WriteLine("Error: Grabber is null in FrameGrabber function.");
                    return;
                }

                // Get the current frame from the capture device
                currentFrame = grabber.QueryFrame().ToImage<Bgr, byte>();

                // Null checks for currentframe
                if (currentFrame == null)
                {
                    Console.WriteLine("Error: CurrentFrame is null in FrameGrabber function.");
                    return;
                }

                Mat resizedFrame = new Mat();
                CvInvoke.Resize(currentFrame, resizedFrame, new System.Drawing.Size(320, 240), 0, 0, Inter.Cubic);
                currentFrame = resizedFrame.ToImage<Bgr, byte>();



                // Convert it to Grayscale
                gray = currentFrame.Convert<Gray, byte>();

                // null checks gray
                if (gray == null)
                {
                    Console.WriteLine("Error: Gray is null in FrameGrabber function.");
                    return;
                }

                // Face Detector
                var faceCascade = new CascadeClassifier("haarcascade_frontalface_default.xml");
                var facesDetected = faceCascade.DetectMultiScale(gray, 1.1, 10, System.Drawing.Size.Empty);


                // Action for each element detected
                foreach (var rect in facesDetected)
                {
                    t = t + 1;
                    Emgu.CV.Image<Emgu.CV.Structure.Gray, byte> result = gray.GetSubRect(rect).Resize(100, 100, Emgu.CV.CvEnum.Inter.Cubic);

                    // Draw the face detected in the 0th (gray) channel with light green color
                    currentFrame.Draw(rect, new Emgu.CV.Structure.Bgr(Color.LightGreen), 2);

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
                }


                // Set the number of faces detected on the scene
                label3.Text = facesDetected.Length.ToString();

                // Names concatenation of persons recognized
                for (int nnn = 0; nnn < facesDetected.Length; nnn++)
                {
                    names = names + NamePersons[nnn];
                }

                // Show the processed and recognized faces
                if (imageBoxFrameGrabber != null)
                {
                    imageBoxFrameGrabber.Image = currentFrame;
                }
                else
                {
                    Console.WriteLine("Error: ImageBoxFrameGrabber is null in FrameGrabber function.");
                }

                names = "";
                // Clear the list(vector) of names
                NamePersons.Clear();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
        }


        private void close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void face_enroll_admin_MouseHover(object sender, EventArgs e)
        {
            panel_btn.BackColor = Color.DeepSkyBlue;
        }

        private void face_enroll_admin_MouseLeave(object sender, EventArgs e)
        {
            panel_btn.BackColor = Color.SteelBlue;
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
                System.Windows.Forms.MessageBox.Show(ex.Message, "Information", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (grabber != null)
            {
                System.Windows.Forms.Application.Idle -= FrameGrabber;
                grabber.Dispose();
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

        private void capture_btn_MouseHover(object sender, EventArgs e)
        {
            capture_groupBox.BackColor = Color.LightSeaGreen;
        }

        private void capture_btn_MouseLeave(object sender, EventArgs e)
        {
            capture_groupBox.BackColor = Color.SeaGreen;
        }
    }
}