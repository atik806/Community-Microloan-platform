using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace borrowersignup
{
    public partial class bankreserve : Form
    {
        public bankreserve()
        {
            InitializeComponent();
            loadDeatils();
        }
        string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
        private void loadDeatils()
        {
            decimal lenderTotal = 0;
            decimal bankTotal = 0;

            
            string queryLender = "SELECT SUM(balance) FROM Table_Lender_info";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryLender, connection))
                {
                    connection.Open();
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        lenderTotal = Convert.ToDecimal(result);
                        textBox2.Text = lenderTotal.ToString();
                    }
                }
            }

            string queryBank = "SELECT bankowns FROM total_money";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryBank, connection))
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        bankTotal = Convert.ToDecimal(reader["bankowns"]);
                        textBox1.Text = bankTotal.ToString();
                    }
                }
            }

            decimal totalAvailable = lenderTotal + bankTotal;
            textBox4.Text = totalAvailable.ToString();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void bankreserve_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            admin a = new admin();
            a.Show();
            this.Hide();
        }
    }
}
