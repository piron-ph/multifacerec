using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiFaceRec
{
    public partial class Messagebox : Form
    {
        public Messagebox()
        {
            InitializeComponent();
            count_lbl.Text = "5";
        }

        int x = 0;

        private void Messagebox_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            x = x + 1;
            if(x == 1)
            {
                count_lbl.Text = "5";
            }
            if (x == 2)
            {
                count_lbl.Text = "4";
            }
            if (x == 3)
            {
                count_lbl.Text = "3";
            }
            if (x == 4)
            {
                count_lbl.Text = "2";
            }
            if (x == 5)
            {
                count_lbl.Text = "1";
            }
            if (x == 6)
            {
                count_lbl.Text = "0";
                this.Close();
            }

        }
    }
}
