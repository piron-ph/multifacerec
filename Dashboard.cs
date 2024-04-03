using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class Dashboard : Form
    {

        public Dashboard()
        {
            InitializeComponent();
        }

        MySqlConnection connection = new MySqlConnection("datasource = localhost;port = 3306; Initial Catalog = 'e_log1'; username = root; password=");
        LogBook elog = new LogBook();
        bool check_monitor = false;

        private void Dashboard_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                register_back_btn.BringToFront();
                pictureBox1.BringToFront();
                disable_monitor_goback();
                CloseAllForm();
                panel_form.Controls.Clear();
                Registration register = new Registration();
                register.TopLevel = false;
                register.AutoScroll = false;
                panel_form.Controls.Add(register);
                register.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }      

        //MAIN BUTTONS HOVER
        private void button2_MouseHover(object sender, EventArgs e)
        {
            register_btn.BackColor = Color.DarkOrange;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            register_btn.BackColor = Color.Black;
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            list_of_user.BackColor = Color.DeepSkyBlue;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            list_of_user.BackColor = Color.Black;
        }

        private void elog_btn_MouseHover(object sender, EventArgs e)
        {
            elog_btn.BackColor = Color.OliveDrab;
        }

        private void elog_btn_MouseLeave(object sender, EventArgs e)
        {
            elog_btn.BackColor = Color.Black;
        }

        private void admin_btn_MouseHover(object sender, EventArgs e)
        {
            admin_btn.BackColor = Color.DodgerBlue;
        }

        private void admin_btn_MouseLeave(object sender, EventArgs e)
        {
            admin_btn.BackColor = Color.Black;
        }

        private void developer_btn_MouseHover(object sender, EventArgs e)
        {

        }

        private void developer_btn_MouseLeave(object sender, EventArgs e)
        {

        }

        public void CloseAllForm()
        {
            List<Form> AllButThisForm = Application.OpenForms.OfType<Form>().Where(frm => !frm.Name.Contains(this.Name)).ToList<Form>();
            foreach (Form othrFrm in AllButThisForm)
            {
                othrFrm.Close();
            }
        }

        //CONTROLS HOVER
        private void minimize_btn_MouseHover(object sender, EventArgs e)
        {
            minimize_grpbx.BackColor = Color.DodgerBlue;
        }

        private void close_btn_MouseHover(object sender, EventArgs e)
        {
            close_grpbx.BackColor = Color.Firebrick;
        }

        private void minimize_btn_MouseLeave(object sender, EventArgs e)
        {
            minimize_grpbx.BackColor = Color.Black;
        }

        private void close_btn_MouseLeave(object sender, EventArgs e)
        {
            close_grpbx.BackColor = Color.Black;
        }

        private void minimize_btn_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void close_btn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.ExitThread();
        }

        //CLICK
        private void register_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                elog_btn.Visible = true;
                elog_back_btn.Visible = false;
                disable_monitor_goback();
                CloseAllForm();
                panel_form.Controls.Clear();
                Registration register = new Registration();
                register.TopLevel = false;
                register.AutoScroll = false;
                panel_form.Controls.Add(register);
                register.Show();
                register_back_btn.BringToFront();
                pictureBox1.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void list_of_user_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                register_back_btn.SendToBack();
                elog_btn.Visible = true;
                elog_back_btn.Visible = false;
                disable_monitor_goback();
                CloseAllForm();
                panel_form.Controls.Clear();
                Edit edit = new Edit();
                edit.TopLevel = false;
                edit.AutoScroll = false;
                panel_form.Controls.Add(edit);
                edit.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void elog_btn_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                register_back_btn.SendToBack();
                monitor_btn.Visible = true;
                monitor_btn.Enabled = true;
                CloseAllForm();
                panel_form.Controls.Clear();
                LogBook elog = new LogBook();
                elog.TopLevel = false;
                elog.AutoScroll = false;
                panel_form.Controls.Add(elog);
                elog.Show();
                elog_back_btn.Visible = true;
                elog_btn.Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void analytics_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            register_back_btn.SendToBack();
            elog_btn.Visible = true;
            elog_back_btn.Visible = false;
            disable_monitor_goback();
            CloseAllForm();
        }

        private void admin_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            register_back_btn.SendToBack();
            elog_btn.Visible = true;
            elog_back_btn.Visible = false;
            disable_monitor_goback();
            CloseAllForm();
            panel_form.Controls.Clear();
            Admin admin = new Admin();
            admin.TopLevel = false;
            admin.AutoScroll = false;
            panel_form.Controls.Add(admin);
            admin.Show();
        }

        private void help_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            register_back_btn.SendToBack();
            elog_btn.Visible = true;
            elog_back_btn.Visible = false;
            disable_monitor_goback();
            CloseAllForm();
        }

        private void developer_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            register_back_btn.SendToBack();
            elog_btn.Visible = true;
            elog_back_btn.Visible = false;
            disable_monitor_goback();
            CloseAllForm();
        }

        private void restart_btn_Click(object sender, EventArgs e)
        {
            //DialogResult msg = MessageBox.Show("Are you sure you want to restart? \nThis will help the system to fixed some bugs and errors.", "Restart", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (msg == DialogResult.Yes)
            //{
                Application.Restart();
                System.Windows.Forms.Application.ExitThread();
            //}
        }

        private void monitor_btn_Click(object sender, EventArgs e)
        {
            if (check_monitor == false)
            {
                panel_btn.Visible = false;
                elog_icon.Visible = false;
                panel_red.Visible = false;

                panel_form.Dock = DockStyle.Fill;
                panel_form.Location = new Point(0, 45);
                check_monitor = true;
            }
            else
            {
                panel_btn.Visible = true;
                elog_icon.Visible = true;
                panel_red.Visible = true;

                panel_form.Dock = DockStyle.None;
                panel_form.Location = new Point(247, 45);
                check_monitor = false;
            }

        }

        public void disable_monitor_goback()
        {
            monitor_btn.Visible = false;
            monitor_btn.Enabled = false;

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void elog_icon_Click(object sender, EventArgs e)
        {

        }

        private void elog_back_btn_Click(object sender, EventArgs e)
        {

        }

        private void register_back_btn_Click(object sender, EventArgs e)
        {

        }
    }
}
