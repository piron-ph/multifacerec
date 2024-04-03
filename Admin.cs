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
    public partial class Admin : Form
    {
        CRUD crud = new CRUD();
        MySqlConnection connection = new MySqlConnection("datasource = localhost;port = 3306; Initial Catalog = 'e_log1'; username = root; password=");
        public Admin()
        {
            InitializeComponent();
        }

        private void Admin_Load(object sender, EventArgs e)
        {
            READ_usertype();
            READ_accounts();
            this.AutoScroll = true;
            // DEFAULT TIMEIN
            time_in_picker.Format = DateTimePickerFormat.Custom;
            time_in_picker.CustomFormat = "hh:mm tt";
            time_in_picker.ShowUpDown = true;

            late_time_picker.Format = DateTimePickerFormat.Custom;
            late_time_picker.CustomFormat = "hh:mm tt";
            late_time_picker.ShowUpDown = true;

            string qry1 = "SELECT * FROM timeout_tb";
            MySqlCommand cmd1 = new MySqlCommand(qry1, connection);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            MySqlDataReader dr1 = cmd1.ExecuteReader();
            while (dr1.Read())
            {
                time_in_picker.Text = (dr1["Default_Timein"].ToString());
                late_time_picker.Text = (dr1["Default_Late"].ToString());
            }
            dr1.Close();
            connection.Close();

        }

        public void DG_PROPERTIES()
        {
            try
            {
                //DATAGRIDVIEW PROPERTIES

                using (Font font = new Font(list_of_usertype_dgv.DefaultCellStyle.Font.FontFamily, 9, FontStyle.Regular))
                {
                    list_of_usertype_dgv.Columns["ID"].DefaultCellStyle.Font = font;
                    list_of_usertype_dgv.Columns["Usertype"].DefaultCellStyle.Font = font;
                }

                using (Font font = new Font(list_of_usertype_dgv.ColumnHeadersDefaultCellStyle.Font.FontFamily, 9, FontStyle.Regular))
                {
                    list_of_usertype_dgv.Columns["ID"].DefaultCellStyle.Font = font;
                    list_of_usertype_dgv.Columns["Usertype"].DefaultCellStyle.Font = font;
                }

                using (Font font = new Font(accounts_dgv.DefaultCellStyle.Font.FontFamily, 9, FontStyle.Regular))
                {
                    accounts_dgv.Columns["ID"].DefaultCellStyle.Font = font;
                    accounts_dgv.Columns["Username"].DefaultCellStyle.Font = font;
                    accounts_dgv.Columns["Password"].DefaultCellStyle.Font = font;
                }

                using (Font font = new Font(accounts_dgv.ColumnHeadersDefaultCellStyle.Font.FontFamily, 9, FontStyle.Regular))
                {
                    accounts_dgv.Columns["ID"].DefaultCellStyle.Font = font;
                    accounts_dgv.Columns["Username"].DefaultCellStyle.Font = font;
                    accounts_dgv.Columns["Password"].DefaultCellStyle.Font = font;
                }

                list_of_usertype_dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.DimGray;
                accounts_dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.DimGray;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        public void READ_usertype()
        {
            try
            {
                list_of_usertype_dgv.DataSource = null;
                crud.Read_Listof_usertype();
                list_of_usertype_dgv.DataSource = crud.dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void READ_accounts()
        {
            try
            {
                accounts_dgv.DataSource = null;
                crud.Read_accounts();
                accounts_dgv.DataSource = crud.dt1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void usertype_txbx_Enter(object sender, EventArgs e)
        {
            /*try
            {
                if (usertype_txbx.Text == "USERTYPE")
                {
                    usertype_txbx.Text = "";
                    usertype_txbx.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
        }

        private void usertype_txbx_Leave(object sender, EventArgs e)
        {
           /* try
            {
                if (usertype_txbx.Text == "")
                {
                    usertype_txbx.Text = "USERTYPE";
                    usertype_txbx.ForeColor = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
        }

        private void username_txbx_Enter(object sender, EventArgs e)
        {
            /*try
            {
                if (username_accnts_txbx.Text == "Username")
                {
                    username_accnts_txbx.Text = "";
                    username_accnts_txbx.ForeColor = Color.Black;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
        }

        private void username_txbx_Leave(object sender, EventArgs e)
        {
            /*try
            {
                if (username_accnts_txbx.Text == "")
                {
                    username_accnts_txbx.ForeColor = Color.Gray;
                    username_accnts_txbx.Text = "Username";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }*/
        }

        private void password_txbx_Enter(object sender, EventArgs e)
        {
            
        }

        private void password_txbx_Leave(object sender, EventArgs e)
        {
           
        }

        private void list_of_usertype_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //GET DATA
            DataGridView senderGrid_id = (DataGridView)sender;
            try
            {
                if (list_of_usertype_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    id_usertype_lbl.Text = (list_of_usertype_dgv.Rows[e.RowIndex].Cells[0].Value.ToString());
                    usertype_txbx.Text = (list_of_usertype_dgv.Rows[e.RowIndex].Cells[1].Value.ToString());

                    add_usertype_btn.Text = "Update";
                    usertype_txbx.ForeColor = Color.Black;
                }
                DG_PROPERTIES();
            }
            catch
            {
                MessageBox.Show("Don't Click the Header!");
                DG_PROPERTIES();
            }
        }

        private void accounts_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //GET DATA
            DataGridView senderGrid_id = (DataGridView)sender;
            try
            {
                if (accounts_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    id_accounts_lbl.Text = (accounts_dgv.Rows[e.RowIndex].Cells[0].Value.ToString());
                    username_accnts_txbx.Text = (accounts_dgv.Rows[e.RowIndex].Cells[1].Value.ToString());
                    password_txbx.Text = (accounts_dgv.Rows[e.RowIndex].Cells[2].Value.ToString());

                    add_account_btn.Text = "Update";
                    username_accnts_txbx.ForeColor = Color.Black;
                    password_txbx.ForeColor = Color.Black;
                }

                DG_PROPERTIES();
            }
            catch
            {
                MessageBox.Show("Don't Click the Header!");
                DG_PROPERTIES();
            }
        }


        private void add_usertype_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (!string.IsNullOrEmpty(usertype_txbx.Text))
                {
                    if (add_usertype_btn.Text == "Add")
                    {
                        crud.usertype = usertype_txbx.Text;
                        crud.Insert_usertype();
                        id_usertype_lbl.Text = "";
                        //usertype_txbx.Text = "USERTYPE";
                        //usertype_txbx.ForeColor = Color.DimGray;
                        usertype_txbx.Clear();
                        add_usertype_btn.Text = "Add";

                    }
                    else
                    {
                        update_usertype();
                        id_usertype_lbl.Text = "";
                        //usertype_txbx.Text = "USERTYPE";
                        //usertype_txbx.ForeColor = Color.DimGray;
                        usertype_txbx.Clear();
                        add_usertype_btn.Text = "Add";
                    }
                }
                else
                {
                    DG_PROPERTIES();
                    MessageBox.Show("Inputbox is empty! Please input data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


                READ_usertype();
                DG_PROPERTIES();
            }
            catch (Exception ex)
            {
                DG_PROPERTIES();
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void add_account_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if ((!string.IsNullOrEmpty(username_accnts_txbx.Text) && !string.IsNullOrEmpty(password_txbx.Text)))
                {
                    if (add_account_btn.Text == "Add")
                    {
                        DG_PROPERTIES();
                        crud.username = username_accnts_txbx.Text;
                        crud.password = password_txbx.Text;
                        crud.Insert_acccount();
                        id_accounts_lbl.Text = "";
                        username_accnts_txbx.Clear();
                        password_txbx.Clear();
                        add_account_btn.Text = "Add";
                    }
                    else
                    {
                        DG_PROPERTIES();
                        update_accounts();
                        id_accounts_lbl.Text = "";
                        username_accnts_txbx.Clear();
                        password_txbx.Clear();
                        add_account_btn.Text = "Add";
                    }

                }
                else
                {
                    DG_PROPERTIES();
                    MessageBox.Show("Inputbox might empty or incomplete! Please input username and password.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                READ_accounts();
                DG_PROPERTIES();
            }
            catch (Exception ex)
            {
                DG_PROPERTIES();
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void del_usertype_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(id_usertype_lbl.Text))
                {
                    DialogResult dlgr_del = MessageBox.Show("Are you sure you want to delete this data?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dlgr_del == DialogResult.Yes)
                    {
                        DG_PROPERTIES();
                        crud.id = id_usertype_lbl.Text;
                        crud.Delete_usertype();
                        READ_usertype();
                        id_usertype_lbl.Text = "";
                        //usertype_txbx.Text = "USERTYPE";
                        //usertype_txbx.ForeColor = Color.DimGray;
                        usertype_txbx.Clear();
                        add_usertype_btn.Text = "Add";
                    }

                    DG_PROPERTIES();
                }
                else
                {
                    DG_PROPERTIES();
                    MessageBox.Show("Nothing to delete! Please select data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DG_PROPERTIES();
            }
            catch (Exception ex)
            {
                DG_PROPERTIES();
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void del_account_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(id_accounts_lbl.Text))
                {
                    DialogResult dlgr_del = MessageBox.Show("Are you sure you want to delete this data?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dlgr_del == DialogResult.Yes)
                    {
                        crud.id = id_accounts_lbl.Text;
                        crud.Delete_account();
                        READ_accounts();
                        id_accounts_lbl.Text = "";
                        username_accnts_txbx.Clear();
                        password_txbx.Clear();
                        add_account_btn.Text = "Add";

                        DG_PROPERTIES();
                    }
                    DG_PROPERTIES();
                }
                else
                {
                    DG_PROPERTIES();
                    MessageBox.Show("Nothing to delete! Please select data.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                DG_PROPERTIES();
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void update_usertype()
        {
            try
            {
                crud.id = id_usertype_lbl.Text;
                crud.usertype = usertype_txbx.Text;
                crud.Update_usertype();
                add_usertype_btn.Text = "Add";
                //usertype_txbx.ForeColor = Color.Gray;
                //usertype_txbx.Text = "USERTYPE";
                DG_PROPERTIES();
            }
            catch (Exception ex)
            {
                DG_PROPERTIES();
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void update_accounts()
        {
            try
            {
                crud.id = id_accounts_lbl.Text;
                crud.username = username_accnts_txbx.Text;
                crud.password = password_txbx.Text;
                crud.Update_accounts();
                add_account_btn.Text = "Add";
                username_accnts_txbx.ForeColor = Color.Gray;
                password_txbx.ForeColor = Color.Gray;
                username_accnts_txbx.Clear();
                password_txbx.Clear();
                DG_PROPERTIES();
            }
            catch (Exception ex)
            {
                DG_PROPERTIES();
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void accounts_dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.Value != null)
            {
                e.Value = new String('#', e.Value.ToString().Length);

            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            password_txbx.PasswordChar = checkBox1.Checked ? '\0':'#';
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            username_accnts_txbx.Clear();
            password_txbx.Clear();
            add_account_btn.Text = "Add";
        }

        private void cancel_usertype_btn_Click(object sender, EventArgs e)
        {
            usertype_txbx.Clear();
            add_usertype_btn.Text = "Add";
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                crud.id = "1";
                crud.timein = time_in_picker.Text;
                crud.Update_Time_In();
                DG_PROPERTIES();
            }
            catch (Exception ex)
            {
                DG_PROPERTIES();
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                crud.id = "1";
                crud.late_time = late_time_picker.Text;
                crud.Update_Default_Late();
                DG_PROPERTIES();
            }
            catch (Exception ex)
            {
                DG_PROPERTIES();
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
