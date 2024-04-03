using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.WinForms;
using MySql.Data.MySqlClient;
using CRUD_Class.myclass;
using System.Windows.Forms;
using LiveCharts.Wpf;

namespace MultiFaceRec
{
    public partial class Analytics : Form
    {
        CRUD crud = new CRUD();
        MySqlConnection connection = new MySqlConnection("datasource = localhost;port = 3306; Initial Catalog = 'e_log'; username = root; password=");
        public DataTable dt1 = new DataTable();
        private DataSet ds1 = new DataSet();
        public Analytics()
        {
            InitializeComponent();
        }

        private void Analytics_Load(object sender, EventArgs e)
        {
            Read_sales();
            pieChart1.LegendLocation = LegendLocation.Bottom;

        }
        public void Read_sales()
        {
            try
            {
                dt1.Clear();
                string query = "SELECT Month, Total FROM example_tb";
                MySqlDataAdapter MDA = new MySqlDataAdapter(query, connection);
                MDA.Fill(ds1);
                dt1 = ds1.Tables[0];

                sales_dgv.DataSource = null;
                sales_dgv.DataSource = dt1;

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Func<ChartPoint, string> labelPoint = chartpoint => string.Format("{0} ({1:P})", chartpoint.Y, chartpoint.Participation);
                SeriesCollection series = new SeriesCollection();
                for (int rows = 0; rows < sales_dgv.Rows.Count; rows++)
                {
                    series.Add(new PieSeries() { Title = sales_dgv.Rows[rows].Cells[0].Value.ToString(), Values = new ChartValues<int> { Convert.ToInt32(sales_dgv.Rows[rows].Cells[1].Value) }, DataLabels = true, LabelPoint = labelPoint });
                    pieChart1.Series = series;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
