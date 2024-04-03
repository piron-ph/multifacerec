using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class Login : Form
    {
        Dashboard dash = new Dashboard();
        MySqlConnection connection = new MySqlConnection("datasource = localhost;port = 3306; Initial Catalog = 'e_log1'; username = root; password=");
        bool check;
        int loginAttempts = 0;
        DateTime cooldownEndTime;
        bool systemLocked = false;

        public Login()
        {
            InitializeComponent();
        }

        public void LOGIN()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                check = false;

                if (systemLocked)
                {
                    if (username_txbx.Text == "admin" && password_txbx.Text == "admin")
                    {
                        check = true;
                        systemLocked = false; // Unlocking the system
                    }
                }
                else
                {
                    string querry = "Select Username, Password from security_tb";
                    MySqlCommand cmd = new MySqlCommand(querry, connection);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    MySqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        if (username_txbx.Text == (dr["Username"].ToString()) && password_txbx.Text == (dr["Password"].ToString()))
                        {
                            check = true;
                        }

                    }

                    dr.Close();
                    connection.Close();
                }

                if (check)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    if (systemLocked)
                    {
                        MessageBox.Show("System is locked. Use admin credentials to unlock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        loginAttempts++;
                        if (loginAttempts >= 5)
                        {
                            cooldownEndTime = DateTime.Now.AddMinutes(5);
                            MessageBox.Show("Wrong password, please try again within 5 minutes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            loginAttempts = 0; // Resetting login attempts after cooldown triggered
                        }
                        else if (loginAttempts > 5 && DateTime.Now < cooldownEndTime)
                        {
                            MessageBox.Show("Please wait for the cooldown period to finish.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (loginAttempts >= 3 && DateTime.Now > cooldownEndTime)
                        {
                            systemLocked = true;
                            MessageBox.Show("System is locked. Use admin credentials to unlock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            if (DateTime.Now < cooldownEndTime && !systemLocked)
            {
                MessageBox.Show("Please wait for the cooldown period to finish.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                LOGIN();
            }
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void close_btn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.ExitThread();
        }

        private void password_txbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (DateTime.Now < cooldownEndTime && !systemLocked)
                {
                    MessageBox.Show("Please wait for the cooldown period to finish.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    LOGIN();
                }
            }
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
