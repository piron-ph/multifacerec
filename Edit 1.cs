using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD_Class.myclass;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class Edit : Form
    {

        MySqlConnection connection = new MySqlConnection("datasource = localhost;port = 3306; Initial Catalog = 'e_log1'; username = root; password=");
        MySqlCommand command;
        MySqlDataAdapter adapter;
        DataTable table;
        CRUD crud = new CRUD();
        string birthdate;
        string reg_date;

        public Edit()
        {
            try
            {
                InitializeComponent();
                u_reg_date.Format = DateTimePickerFormat.Custom;
                u_reg_date.CustomFormat = "MMM dd yyy";
                DISABLE();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void Edit_Load(object sender, EventArgs e)
        {
            try
            {
                u_date_birth_bx.Format = DateTimePickerFormat.Custom;
                u_date_birth_bx.CustomFormat = "MMM dd yyy";
                u_reg_date.Format = DateTimePickerFormat.Custom;
                u_reg_date.CustomFormat = "MMM dd yyy";
                READ2();
                brgy();
                muncplty();
                province();
                usertype();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //READ
        public void READ()
        {
            try
            {
                edit_dgv.DataSource = null;
                crud.Read_verified();
                edit_dgv.DataSource = crud.dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void READ2()
        {
            try
            {
                edit_dgv.DataSource = null;
                crud.Read_registered();
                edit_dgv.DataSource = crud.dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void registered_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                label5.Text = "reg";
                READ2();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void register_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //GET DATA
            DataGridView senderGrid_id = (DataGridView)sender;
            try
            {
                if (edit_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    id_lbl.Text = (edit_dgv.Rows[e.RowIndex].Cells[0].Value.ToString());
                    u_username.Text = (edit_dgv.Rows[e.RowIndex].Cells[1].Value.ToString());
                    u_lname.Text = (edit_dgv.Rows[e.RowIndex].Cells[2].Value.ToString());
                    u_fname.Text = (edit_dgv.Rows[e.RowIndex].Cells[3].Value.ToString());
                    u_mname.Text = (edit_dgv.Rows[e.RowIndex].Cells[4].Value.ToString());
                    u_date_birth_bx.Text = (edit_dgv.Rows[e.RowIndex].Cells[5].Value.ToString());
                    u_gender.Text = (edit_dgv.Rows[e.RowIndex].Cells[6].Value.ToString());
                    u_brgy.Text = (edit_dgv.Rows[e.RowIndex].Cells[7].Value.ToString());
                    u_municiplty.Text = (edit_dgv.Rows[e.RowIndex].Cells[8].Value.ToString());
                    u_province.Text = (edit_dgv.Rows[e.RowIndex].Cells[9].Value.ToString());
                    u_contact.Text = (edit_dgv.Rows[e.RowIndex].Cells[10].Value.ToString());
                    u_usertype.Text = (edit_dgv.Rows[e.RowIndex].Cells[11].Value.ToString());
                    u_status.Text = (edit_dgv.Rows[e.RowIndex].Cells[12].Value.ToString());
                    u_reg_date.Text = (edit_dgv.Rows[e.RowIndex].Cells[13].Value.ToString());

                    ENABLE();
                }
                if (u_status.Text == "Verified")
                {
                    u_status.ForeColor = Color.MediumSeaGreen;
                }
                else
                {
                    u_status.ForeColor = Color.Coral;
                }
            }
            catch
            {
                MessageBox.Show("Don't Click the Header!");
            }
        }

        private void verified_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                label5.Text = "ver";
                READ();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void CLEAR2()
        {
            try
            {
                u_username.Text = "";
                u_lname.Text = "";
                u_mname.Text = "";
                u_fname.Text = "";
                u_date_birth_bx.Text = "";
                u_gender.Text = "";
                u_brgy.Text = "";
                u_municiplty.Text = "";
                u_province.Text = "";
                u_contact.Text = "";
                u_usertype.Text = "";
                u_status.Text = "";
                u_reg_date.Text = "";

                id_lbl.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void DISABLE()
        {
            try
            {
                update_btn.Enabled = false;
                delete_btn.Enabled = false;
                cancel_btn.Enabled = false;

                u_lname.Enabled = false;
                u_mname.Enabled = false;
                u_fname.Enabled = false;
                u_date_birth_bx.Enabled = false;
                u_gender.Enabled = false;
                u_brgy.Enabled = false;
                u_municiplty.Enabled = false;
                u_province.Enabled = false;
                u_contact.Enabled = false;
                u_usertype.Enabled = false;
                u_status.Enabled = false;
                u_reg_date.Enabled = false;

                id_lbl.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void ENABLE()
        {
            try
            {
                update_btn.Enabled = true;
                delete_btn.Enabled = true;
                cancel_btn.Enabled = true;

                u_lname.Enabled = true;
                u_mname.Enabled = true;
                u_fname.Enabled = true;
                u_date_birth_bx.Enabled = true;
                u_gender.Enabled = true;
                u_brgy.Enabled = true;
                u_municiplty.Enabled = true;
                u_province.Enabled = true;
                u_contact.Enabled = true;
                u_usertype.Enabled = true;
                u_status.Enabled = true;
                u_reg_date.Enabled = true;

                id_lbl.Enabled = true;
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
                    u_usertype.Items.Add(dr["Usertype"].ToString());
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
                u_brgy.AutoCompleteMode = AutoCompleteMode.Suggest;
                u_brgy.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection col = new AutoCompleteStringCollection();

                string querry = "Select Barangay from barangay_tb";
                MySqlCommand cmd = new MySqlCommand(querry, connection);
                connection.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    col.Add(dr["Barangay"].ToString());
                }
                dr.Close();

                u_brgy.AutoCompleteCustomSource = col;
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
                u_municiplty.AutoCompleteMode = AutoCompleteMode.Suggest;
                u_municiplty.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection col = new AutoCompleteStringCollection();

                string querry = "Select Municipality from municipality_tb";
                MySqlCommand cmd = new MySqlCommand(querry, connection);
                connection.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    col.Add(dr["Municipality"].ToString());
                }
                dr.Close();

                u_municiplty.AutoCompleteCustomSource = col;
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
                u_province.AutoCompleteMode = AutoCompleteMode.Suggest;
                u_province.AutoCompleteSource = AutoCompleteSource.CustomSource;
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

                u_province.AutoCompleteCustomSource = col;
                connection.Close();
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
                string query = "SELECT * FROM register_tb WHERE Status='REGISTERED' AND CONCAT(`Username`, `LastName`, `FirstName`, `MiddleInitial`, `Birthdate`, `Gender`, `Barangay`, `Municipality`, `Province`, `Contact_No`, `Usertype`, `Status`, `Registration_Date`) like '%" + valueToSearch + "%'";
                command = new MySqlCommand(query, connection);
                adapter = new MySqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);
                edit_dgv.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void searchData2(string valueToSearch2)
        {
            try
            {
                string query = "SELECT * FROM register_tb WHERE Status='VERIFIED' AND CONCAT(`Username`, `LastName`, `FirstName`, `MiddleInitial`, `Birthdate`, `Gender`, `Barangay`, `Municipality`, `Province`, `Contact_No`, `Usertype`, `Status`, `Registration_Date`) like '%" + valueToSearch2 + "%'";
                command = new MySqlCommand(query, connection);
                adapter = new MySqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);
                edit_dgv.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void search_bx_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (label5.Text == "reg")
                {
                    string valueToSearch = search_bx.Text.ToString();
                    searchData(valueToSearch);
                }
                else
                {
                    string valueToSearch2 = search_bx.Text.ToString();
                    searchData2(valueToSearch2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void update_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                DateTime DOB = u_date_birth_bx.Value;
                birthdate = DOB.ToString("MMM dd yyy").ToUpper();

                DateTime reg = u_reg_date.Value;
                reg_date = reg.ToString("MMM dd yyy").ToUpper();

                if (u_username.Text == "" || u_lname.Text == "" || u_fname.Text == "" || u_gender.Text == "" || u_brgy.Text == "" || u_municiplty.Text == "" || u_province.Text == "" || u_contact.Text == "" || u_usertype.Text == "" || u_reg_date.Text == "")
                {
                    MessageBox.Show("Incomplete Data! Please provide all the information needed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    UPDATE();
                    if(u_status.Text == "REGISTERED")
                    {
                        READ2();
                    }
                    else
                    {
                        READ();
                    }
                    CLEAR2();
                    DISABLE();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        //UPDATE
        public void UPDATE()
        {
            try
            {
                crud.username = u_username.Text;
                crud.lname = u_lname.Text;
                crud.fname = u_fname.Text;
                crud.mname = u_mname.Text;
                crud.birthdate = birthdate;
                crud.gender = u_gender.Text;
                crud.brgy = u_brgy.Text;
                crud.municipality = u_municiplty.Text;
                crud.province = u_province.Text;
                crud.contact = u_contact.Text;
                crud.usertype = u_usertype.Text;
                crud.reg_date = reg_date;

                crud.id = id_lbl.Text;
                crud.Update_data();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void delete_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (u_status.Text == "REGISTERED")
                {
                    DialogResult question = MessageBox.Show("Are you sure to delete this data?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (question == DialogResult.Yes)
                    {
                        DELETE();
                        if (u_status.Text == "REGISTERED")
                        {
                            READ2();
                        }
                        else
                        {
                            READ();
                        }
                        CLEAR2();
                        DISABLE();
                    }
                }
                else
                {
                    MessageBox.Show("Warning: Cannot delete verified user! You need to remove first the enrolled face in 'Face-Enroll Form'.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        //DELETE
        public void DELETE()
        {
            try
            {
                crud.id = id_lbl.Text;
                crud.Delete_data();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            CLEAR2();
            DISABLE();
        }

        private void u_lname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(u_lname.Text, "^[a-zA-Z\\s]+$") && u_lname.Text != "")
                {
                    MessageBox.Show("It only accepts alphabetical characters!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    u_lname.Text = u_lname.Text.Substring(0, u_lname.Text.Length - 1);
                    u_lname.SelectionStart = u_lname.Text.Length;
                    u_lname.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void u_fname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(u_fname.Text, "^[a-zA-Z\\s]+$") && u_fname.Text != "")
                {
                    MessageBox.Show("It only accepts alphabetical characters!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    u_fname.Text = u_fname.Text.Substring(0, u_fname.Text.Length - 1);
                    u_fname.SelectionStart = u_fname.Text.Length;
                    u_fname.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void u_mname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(u_mname.Text, "^[a-zA-Z]+$") && u_mname.Text != "")
                {
                    MessageBox.Show("It only accepts alphabetical characters!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    u_mname.Text = u_mname.Text.Substring(0, u_mname.Text.Length - 1);
                    u_mname.SelectionStart = u_mname.Text.Length;
                    u_mname.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void u_gender_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(u_gender.Text, "^[a-zA-Z]+$") && u_gender.Text != "")
                {
                    MessageBox.Show("It only accepts alphabetical characters!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    u_gender.Text = u_gender.Text.Substring(0, u_gender.Text.Length - 1);
                    u_gender.SelectionStart = u_gender.Text.Length;
                    u_gender.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void u_contact_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(u_contact.Text, "^[0-9]+$") && u_contact.Text != "")
                {
                    MessageBox.Show("It only accepts numeric values!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    u_contact.Text = u_contact.Text.Substring(0, u_contact.Text.Length - 1);
                    u_contact.SelectionStart = u_contact.Text.Length;
                    u_contact.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void edit_dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
