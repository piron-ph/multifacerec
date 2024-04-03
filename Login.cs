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
    public partial class Login : Form
    {
        Dashboard dash = new Dashboard();
        MySqlConnection connection = new MySqlConnection("datasource = localhost;port = 3306; Initial Catalog = 'e_log1'; username = root; password=");
        bool check;
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

                if (check)
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Invalid username or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                dr.Close();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            LOGIN();
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
            if(e.KeyCode == Keys.Enter)
            {
                LOGIN();
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
