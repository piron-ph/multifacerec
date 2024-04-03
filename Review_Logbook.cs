using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.Shared;
using System.Configuration;
using System.Windows.Forms;
using CRUD_Class.myclass;
using MySql.Data.MySqlClient;

namespace MultiFaceRec
{
    public partial class Review_Logbook : Form
    {

        MySqlConnection connection = new MySqlConnection("datasource = localhost;port = 3306; Initial Catalog = 'e_log1'; username = root; password=");
        MySqlCommand command;
        MySqlDataAdapter adapter;
        DataTable table;

        bool checkme4 = false;
        int indx = 0;
        CRUD crud = new CRUD();
        SaveFileDialog sfd = new SaveFileDialog();



        public DataTable dt = new DataTable();
        private DataSet ds = new DataSet();

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;


        public Review_Logbook()
        {
            InitializeComponent();
            //DATETIME
            date_time_picker.Format = DateTimePickerFormat.Custom;
            date_time_picker.CustomFormat = "MMM dd yyy";

            time_in_picker.Format = DateTimePickerFormat.Custom;
            time_in_picker.CustomFormat = "hh:mm tt";
            time_in_picker.ShowUpDown = true;

            timer_now.Start();
        }
        private void Review_Logbook_Load(object sender, EventArgs e)
        {
            try
            {
                Clear_all();
                orderby_1.Items.Insert(0, "ID");
                orderby_1.SelectedIndex = 0;
                order_by_2.Items.Insert(0, "ASC");
                order_by_2.SelectedIndex = 0;

                //FIll Order By ComboBox
                string querry = "SELECT Order_By FROM order_by_tb";
                MySqlCommand cmd = new MySqlCommand(querry, connection);
                connection.Open();
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    orderby_1.Items.Add(dr["Order_By"].ToString());
                }
                dr.Close();
                connection.Close();

                data_grpbx.Visible = false;

                //Username Autocomplete
                username_autocomplete();

                //Retrieve Data From Logbook Table
                logbook_dgv.DataSource = null;
                Retrieve_Logbook();
                logbook_dgv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void username_autocomplete()
        {
            try
            {
                username_txbx.AutoCompleteMode = AutoCompleteMode.Suggest;
                username_txbx.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection col = new AutoCompleteStringCollection();

                string querry = "Select Username FROM register_tb";
                MySqlCommand cmd = new MySqlCommand(querry, connection);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    col.Add(dr["Username"].ToString());
                }
                username_txbx.AutoCompleteCustomSource = col;
                dr.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void del_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                time_in_picker.Visible = false;

                DialogResult question = MessageBox.Show("Are you sure to delete this data?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (question == DialogResult.Yes)
                {
                    crud.username = username_txbx.Text;
                    crud.logbook_date = date_time_picker.Text;
                    crud.Delete_Record();

                    add_btn.Enabled = true;
                    logbook_dgv.Enabled = true;
                    Clear_all();

                    logbook_dgv.DataSource = null;
                    Retrieve_Logbook();
                    logbook_dgv.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
            Clear_all();
        }

        public void searchData(string valueToSearch)
        {
            try
            {
                string query = "SELECT * FROM logbook_tb WHERE CONCAT(`Username`, `LastName`, `FirstName`, `MiddleInitial`, `Age`, `Gender`, `Barangay`, `Municipality`, `Province`, `Contact_No`, `Date`, `Time_In`, `Time_Out`, `Time_In_2`, `Time_Out_2`) like '%" + valueToSearch + "%' AND Date='" + date_time_picker.Text + "'";
                command = new MySqlCommand(query, connection);
                adapter = new MySqlDataAdapter(command);
                table = new DataTable();
                adapter.Fill(table);
                logbook_dgv.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void search_bx_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                string valueToSearch = search_bx.Text.ToString();
                searchData(valueToSearch);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Retrieve_Logbook()
        {
            try
            {
                dt.Clear();
                string query = "SELECT ID, Username, LastName, FirstName, MiddleInitial, Age, Gender, Contact_No, Barangay, Municipality, Province, Time_In, Time_Out, Time_In_2, Time_Out_2 FROM logbook_tb WHERE Date='" + date_time_picker.Text + "'";
                MySqlDataAdapter MDA = new MySqlDataAdapter(query, connection);
                MDA.Fill(ds);
                dt = ds.Tables[0];
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void date_time_picker_ValueChanged(object sender, EventArgs e)
        {
            logbook_dgv.DataSource = null;
            Retrieve_Logbook();
            logbook_dgv.DataSource = dt;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        public void Clear_all()
        {
            username_txbx.Text = "";
            lname.Text = "";
            fname.Text = "";
            mname.Text = "";
            age_txbx.Text = "";
            gender.Text = "";
            brgy.Text = "";
            municipality.Text = "";
            province.Text = "";
            contact.Text = "";
            time_in_picker.Visible = false;

            id_lbl.Text = "";
            data_grpbx.Visible = false;
            Disable_txbx_btn();
        }

        public void Disable_txbx_btn()
        {
            try
            {
                id_lbl.Enabled = false;
                username_txbx.Enabled = false;
                time_in_picker.Enabled = false;
                del_btn.Enabled = false;
                update_btn.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Enable_delete_edit()
        {
            try
            {
                del_btn.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Update_Logbook()
        {
            try
            {
                crud.username = username_txbx.Text;
                crud.lname = lname.Text;
                crud.fname = fname.Text;
                crud.mname = mname.Text;
                crud.age = age_txbx.Text;
                crud.gender = gender.Text;
                crud.brgy = brgy.Text;
                crud.municipality = municipality.Text;
                crud.province = province.Text;
                crud.contact = contact.Text;
                crud.time_login = time_in_picker.Text;

                crud.id = id_lbl.Text;
                crud.Update_logbook();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Manual_Login()
        {
            try
            {
                crud.username = username_txbx.Text;
                crud.lname = lname.Text;
                crud.fname = fname.Text;
                crud.mname = mname.Text;
                crud.age = age_txbx.Text;
                crud.gender = gender.Text;
                crud.brgy = brgy.Text;
                crud.municipality = municipality.Text;
                crud.province = province.Text;
                crud.contact = contact.Text;
                crud.date_login = date_lbl.Text;
                crud.time_login = time_lbl.Text;

                crud.Manual_Insert();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void add_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            logbook_dgv.DataSource = dt;
            logbook_dgv.Enabled = false;
            time_in_picker.Visible = false;
            time_lbl.Visible = true;
            select_lbl.Visible = true;
            update_btn.Text = "Login";
            data_grpbx.Visible = true;
            username_txbx.Enabled = true;
            update_btn.Enabled = true;
            date_now_lbl.Visible = true;
            date_lbl.Visible = true;
            add_btn.Enabled = false;
        }

        public void checkusername()
        {
            try
            {
                checkme4 = false;
                DataTable dt = new DataTable();
                dt.Clear();
                using (MySqlDataAdapter msda = new MySqlDataAdapter("SELECT Username, Date FROM logbook_tb", connection))
                {
                    msda.Fill(dt);
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (username_txbx.Text == (row["Username"].ToString()) && date_lbl.Text == (row["Date"].ToString()))
                    {
                        checkme4 = true;
                        MessageBox.Show(row["Username"].ToString() + " is already logged in today!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        username_txbx.Text = "";
                    }
                }
                connection.Close();
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
                if (username_txbx.Text == "" || lname.Text == "" || fname.Text == "" || mname.Text == "" || age_txbx.Text == "" || gender.Text == "" || brgy.Text == "" || municipality.Text == "" || province.Text == "" || contact.Text == "")
                {
                    MessageBox.Show("Incomplete Data! Please provide all the information needed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (update_btn.Text == "Update")
                    {
                        logbook_dgv.Enabled = true;
                        Update_Logbook();
                        add_btn.Enabled = true;
                        time_lbl.Visible = false;
                        Clear_all();
                        //Retrieve Logbook
                        logbook_dgv.DataSource = null;
                        Retrieve_Logbook();
                        logbook_dgv.DataSource = dt;
                    }
                    if (update_btn.Text == "Login")
                    {
                        checkusername();
                        if (checkme4 == false)
                        {
                            logbook_dgv.Enabled = true;
                            Manual_Login();
                            select_lbl.Visible = false;
                            add_btn.Enabled = true;
                            date_lbl.Visible = false;
                            date_now_lbl.Visible = false;
                            time_lbl.Visible = false;
                            Clear_all();
                            //Retrieve Logbook
                            logbook_dgv.DataSource = null;
                            Retrieve_Logbook();
                            logbook_dgv.DataSource = dt;
                        }
                        if (checkme4)
                        {
                            username_txbx.Text = "";
                            lname.Text = "";
                            fname.Text = "";
                            mname.Text = "";
                            age_txbx.Text = "";
                            gender.Text = "";
                            brgy.Text = "";
                            municipality.Text = "";
                            province.Text = "";
                            contact.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void review_dgv_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //GET DATA
            try
            {

                DataGridView senderGrid_id = (DataGridView)sender;

                if (logbook_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    time_in_picker.Enabled = false;
                    time_in_picker.Visible = true;

                    id_lbl.Text = (logbook_dgv.Rows[e.RowIndex].Cells[0].Value.ToString());
                    username_txbx.Text = (logbook_dgv.Rows[e.RowIndex].Cells[1].Value.ToString());
                    time_in_picker.Text = (logbook_dgv.Rows[e.RowIndex].Cells[11].Value.ToString());

                    username_txbx.Enabled = false;
                    update_btn.Enabled = true;
                    update_btn.Text = "Update";
                    data_grpbx.Visible = true;

                    time_lbl.Visible = false;
                    add_btn.Enabled = false;
                }

                if (!string.IsNullOrEmpty(id_lbl.Text) || !string.IsNullOrEmpty(username_txbx.Text) || !string.IsNullOrEmpty(time_in_picker.Text))
                {
                    Enable_delete_edit();
                }

            }
            catch
            {
                MessageBox.Show("Don't click the header!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }   

        private void username_txbx_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (!string.IsNullOrEmpty(username_txbx.Text))
                {
                    DataTable dt = new DataTable();
                    dt.Clear();
                    using (MySqlDataAdapter msda = new MySqlDataAdapter("SELECT LastName, FirstName, MiddleInitial, Birthdate, Gender, Barangay, Municipality, Province, Contact_No FROM register_tb WHERE Username='" + username_txbx.Text + "'", connection))
                    {
                        msda.Fill(dt);
                    }
                    foreach (DataRow row in dt.Rows)
                    {
                        lname.Text = (row["LastName"].ToString());
                        fname.Text = (row["FirstName"].ToString());
                        mname.Text = (row["MiddleInitial"].ToString());
                        DOB.Text = (row["Birthdate"].ToString());
                        gender.Text = (row["Gender"].ToString());
                        brgy.Text = (row["Barangay"].ToString());
                        municipality.Text = (row["Municipality"].ToString());
                        province.Text = (row["Province"].ToString());
                        contact.Text = (row["Contact_No"].ToString());
                    }
                    TimeSpan age = DateTime.Now - DOB.Value;
                    int years = DateTime.Now.Year - DOB.Value.Year;
                    if (DOB.Value.AddYears(years) > DateTime.Now) years--;
                    {
                        age_txbx.Text = years.ToString();
                    }

                    if (dt.Rows.Count <= 0)
                    {
                        lname.Text = "";
                        fname.Text = "";
                        mname.Text = "";
                        age_txbx.Text = "";
                        gender.Text = "";
                        brgy.Text = "";
                        municipality.Text = "";
                        province.Text = "";
                        contact.Text = "";
                    }
                    connection.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (update_btn.Text == "Update")
                {
                    logbook_dgv.Enabled = true;
                    data_grpbx.Visible = false;
                    time_in_picker.Enabled = false;
                    username_txbx.Enabled = false;
                    del_btn.Enabled = false;
                    username_txbx.Text = "";
                    time_in_picker.Visible = false;
                    add_btn.Enabled = true;
                    time_lbl.Visible = false;
                    Clear_all();
                    //Retrieve Logbook
                    logbook_dgv.DataSource = null;
                    Retrieve_Logbook();
                    logbook_dgv.DataSource = dt;
                }
                if(update_btn.Text == "Login")
                {
                    Clear_all();
                    logbook_dgv.Enabled = true;
                    select_lbl.Visible = false;
                    add_btn.Enabled = true;
                    date_lbl.Visible = false;
                    date_now_lbl.Visible = false;
                    time_lbl.Visible = false;
                    //Retrieve Logbook
                    logbook_dgv.DataSource = null;
                    Retrieve_Logbook();
                    logbook_dgv.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void timer_now_Tick(object sender, EventArgs e)
        {
            date_lbl.Text = DateTime.Now.ToString("MMM dd yyy");
            time_lbl.Text = DateTime.Now.ToString("hh:mm tt");
        }

        /*private void export_btn_Click(object sender, EventArgs e)
        {
            if (logbook_dgv.Rows.Count > 0)
            {
                DialogResult dlgr = MessageBox.Show("Before exporting the data, make sure the 'DATE' and 'ORDER BY' are set properly acccording to your desire format. Click 'OK' to confirm. ", "Export", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (dlgr == DialogResult.OK)
                {
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        sfd.InitialDirectory = "C";
                        sfd.Title = "SAVE AS EXCEL FILE";
                        sfd.FileName = "Logbook1";
                        sfd.Filter = "Excel Workbook|*.xlsx";

                        if (sfd.ShowDialog() != DialogResult.Cancel)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            Microsoft.Office.Interop.Excel.Application xcelApp = new Microsoft.Office.Interop.Excel.Application();
                            xcelApp.Application.Workbooks.Add(Type.Missing);
                            for (int i = 1; i < logbook_dgv.Columns.Count + 1; i++)
                            {
                                xcelApp.Cells[1, i] = logbook_dgv.Columns[i - 1].HeaderText;
                            }

                            for (int i = 0; i < logbook_dgv.Rows.Count; i++)
                            {
                                for (int j = 0; j < logbook_dgv.Columns.Count; j++)
                                {
                                    xcelApp.Cells[i + 2, j + 1] = logbook_dgv.Rows[i].Cells[j].Value.ToString();
                                }
                            }
                            xcelApp.Columns.AutoFit();

                            xcelApp.ActiveWorkbook.SaveCopyAs(sfd.FileName.ToString());
                            xcelApp.ActiveWorkbook.Saved = true;
                            xcelApp.Quit();
                            MessageBox.Show("Data was exported successfully.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("DataGridView is empty! No data can be exported.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }*/

        public class WorkHoursCalculator
        {
            public static TimeSpan ComputeTotalWorkHours(string timeIn1, string timeOut1, string timeIn2, string timeOut2)
            {
                // Parsing time strings to DateTime objects
                DateTime timeIn1DateTime = DateTime.ParseExact(timeIn1, "hh:mm tt", null);
                DateTime timeOut1DateTime = DateTime.ParseExact(timeOut1, "hh:mm tt", null);
                DateTime timeIn2DateTime = DateTime.ParseExact(timeIn2, "hh:mm tt", null);
                DateTime timeOut2DateTime = DateTime.ParseExact(timeOut2, "hh:mm tt", null);

                // Calculating total work hours
                TimeSpan workHours1 = timeOut1DateTime - timeIn1DateTime;
                TimeSpan workHours2 = timeOut2DateTime - timeIn2DateTime;

                // Total work hours
                TimeSpan totalWorkHours = workHours1 + workHours2;

                return totalWorkHours;
            }
        }

        private void print_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Last Name", typeof(string));
            dt.Columns.Add("First Name", typeof(string));
            dt.Columns.Add("Middle Initial", typeof(string));
            dt.Columns.Add("Age", typeof(string));
            dt.Columns.Add("Gender", typeof(string));
            dt.Columns.Add("Contact No.", typeof(string));
            dt.Columns.Add("Barangay", typeof(string));
            dt.Columns.Add("Municipality", typeof(string));
            dt.Columns.Add("Province", typeof(string));
            dt.Columns.Add("Time_In", typeof(string));
            dt.Columns.Add("Time_Out", typeof(string));
            dt.Columns.Add("Time_In_2", typeof(string));
            dt.Columns.Add("Time_Out_2", typeof(string));
            dt.Columns.Add("Total_Work_Hours", typeof(string)); // Added column for total work hours

            foreach (DataGridViewRow dgv in logbook_dgv.Rows)
            {
                // Compute total work hours
                TimeSpan totalWorkHours = WorkHoursCalculator.ComputeTotalWorkHours(
                    dgv.Cells[11].Value.ToString(), // Time_In
                    dgv.Cells[12].Value.ToString(), // Time_Out
                    dgv.Cells[13].Value.ToString(), // Time_In_2
                    dgv.Cells[14].Value.ToString()  // Time_Out_2
                );

                // Add row with total work hours
                dt.Rows.Add(
                    dgv.Cells[2].Value,
                    dgv.Cells[3].Value,
                    dgv.Cells[4].Value,
                    dgv.Cells[5].Value,
                    dgv.Cells[6].Value,
                    dgv.Cells[7].Value,
                    dgv.Cells[8].Value,
                    dgv.Cells[9].Value,
                    dgv.Cells[10].Value,
                    dgv.Cells[11].Value,
                    dgv.Cells[12].Value,
                    dgv.Cells[13].Value,
                    dgv.Cells[14].Value,
                    totalWorkHours.ToString("hh\\:mm") // Format total work hours as hh:mm
                );
            }

            ds.Tables.Add(dt);
            ds.WriteXmlSchema("print.xml");
            Report_LB frm = new Report_LB();
            CrystalReport1 crpt = new CrystalReport1();
            TextObject txt_obj = (TextObject)crpt.ReportDefinition.Sections["Section1"].ReportObjects["date_txbx"];
            TextObject attendees = (TextObject)crpt.ReportDefinition.Sections["Section1"].ReportObjects["number_attendees"];
            txt_obj.Text = date_time_picker.Text;
            attendees.Text = logbook_dgv.Rows.Count.ToString();
            crpt.SetDataSource(ds);
            frm.crystalReportViewer01.ReportSource = crpt;
            frm.crystalReportViewer01.Refresh();
            frm.Show();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        public void Get_Index()
        {
            string querry = "SELECT ID FROM order_by_tb WHERE Order_By='" + orderby_1.Text + "'";
            MySqlCommand cmd = new MySqlCommand(querry, connection);
            connection.Open();
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                indx = Convert.ToInt32((dr["ID"].ToString()));
            }
            dr.Close();
            connection.Close();
        }
        private void orderby_1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (logbook_dgv.Rows.Count > 0)
                {
                    if (order_by_2.Text == "ASC")
                    {
                        Get_Index();
                        logbook_dgv.Sort(logbook_dgv.Columns[indx], ListSortDirection.Ascending);
                    }
                    else if (order_by_2.Text == "DESC")
                    {
                        Get_Index();
                        logbook_dgv.Sort(logbook_dgv.Columns[indx], ListSortDirection.Descending);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void order_by_2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (logbook_dgv.Rows.Count > 0)
                {
                    if (order_by_2.Text == "ASC")
                    {
                        Get_Index();
                        logbook_dgv.Sort(logbook_dgv.Columns[indx], ListSortDirection.Ascending);
                    }
                    else if (order_by_2.Text == "DESC")
                    {
                        Get_Index();
                        logbook_dgv.Sort(logbook_dgv.Columns[indx], ListSortDirection.Descending);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Review_Logbook_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void Review_Logbook_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void Review_Logbook_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void export_btn_Click(object sender, EventArgs e)
        {

        }

        private void export_btn_MouseLeave(object sender, EventArgs e)
        {

        }

        private void button2_MouseHover(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void payroll_btn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://localhost/Payroll-System/");
        }

        private void report_btn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://localhost/ATT-REPORT/");
        }
    }
}
