using System;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.IO.Ports;
using System.Data;
using System.Text.RegularExpressions;
using CRUD_Class.myclass;
using MySql.Data.MySqlClient;

namespace MultiFaceRec
{
    public partial class Face_Enroll_Admin : Form
    {
        CRUD crud = new CRUD();
        MySqlConnection connection = new MySqlConnection("datasource = localhost;port = 3306; Initial Catalog = 'e_log1'; username = root; password=");
        MySqlCommand command;
        MySqlDataAdapter adapter;
        DataTable table;

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public Face_Enroll_Admin()
        {
            InitializeComponent();
        }

        private void Face_Enroll_Admin_Load(object sender, EventArgs e)
        {
            READ_face_enrolled();
        }

        private void close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void READ_face_enrolled()
        {
            try
            {
                face_enroll_dgv.DataSource = null;
                crud.Read_face_enrolled();
                face_enroll_dgv.DataSource = crud.dt2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void searchData(string valueToSearch)
        {
            try
            {
                string query = "SELECT ID, Username, Image_name FROM face_enrollment_tb WHERE Key_number='1' AND CONCAT(`ID`, `Username`, `Image_name`) like '%" + valueToSearch + "%' ORDER BY ID ASC";
                command = new MySqlCommand(query, connection);
                adapter = new MySqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);
                face_enroll_dgv.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void admin_search_bx_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string valueToSearch = admin_search_bx.Text.ToString();
                searchData(valueToSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void face_enroll_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //GET DATA
            DataGridView senderGrid_id = (DataGridView)sender;
            try
            {
                if (face_enroll_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    id_face_enroll.Text = (face_enroll_dgv.Rows[e.RowIndex].Cells[0].Value.ToString());
                    usern_faceenroll_txbx.Text = (face_enroll_dgv.Rows[e.RowIndex].Cells[1].Value.ToString());
                    img_name_txbx.Text = (face_enroll_dgv.Rows[e.RowIndex].Cells[2].Value.ToString());

                    del_face_enroll_btn.Enabled = true;
                }
            }
            catch
            {
                MessageBox.Show("Don't click the header", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void del_face_enroll_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Regex reg = new Regex(@"([a-zA-Z]+)(\d+)");
                Match result = reg.Match(img_name_txbx.Text);

                string filepath;
                string numberPart = result.Groups[2].ToString();
                int pic_start = Convert.ToInt32(numberPart);
                int pic_end = pic_start + 1;
                int id2 = int.Parse(id_face_enroll.Text) + 1;

                if (!string.IsNullOrEmpty(id_face_enroll.Text))
                {
                    DialogResult dlgr_del = MessageBox.Show("Are you sure you want to delete this data?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dlgr_del == DialogResult.Yes)
                    {
                        crud.id1 = id_face_enroll.Text;
                        crud.id2 = id2;
                        crud.Delete_face_enrolled();
                        if (crud.is_del)
                        {
                            for (int x = 1; pic_start <= pic_end; x++)
                            {
                                filepath = Application.StartupPath + "/TrainedFaces/face" + pic_start + ".bmp";
                                File.Delete(filepath);
                                pic_start++;
                            }
                            crud.username = usern_faceenroll_txbx.Text;
                            crud.Verified_to_Reg();

                            READ_face_enrolled();
                            id_face_enroll.Text = "";
                            usern_faceenroll_txbx.Clear();
                            del_face_enroll_btn.Enabled = false;
                            MessageBox.Show("User was successfully deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete user!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }

                }
                else
                {
                    MessageBox.Show("Nothing to delete! Please select data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
    }
}
