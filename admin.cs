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
    public partial class admin : Form
    {
        public admin()
        {
            InitializeComponent();
            load_user();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            login Login = new login();
            Login.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
                lender_info lender =new lender_info();
            lender.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            problems problem = new problems();
            problem.Show();
            this.Hide();
        }


        private void load_user()
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";

            string query = @"
            SELECT 'Lender' AS Role, Name, Phone, Email, Address, Gender FROM table_lender_info";

            //SELECT 'Monitor' AS Role, Name, Phone, Email, Address, Gender FROM Monitors
            //UNION
            //SELECT 'Helper' AS Role, Name, Phone, Email, Address, Gender FROM Helpers;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        } 
        

        private void search(string keyword)
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = @"
            SELECT 'Lender' AS Role, name, phone, email, address
            FROM table_lender_info
            WHERE name LIKE @searchTerm 
            OR phone LIKE @searchTerm 
            OR email LIKE @searchTerm 
            OR address LIKE @searchTerm 
            OR gender LIKE @searchTerm";

       
    


            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchTerm", "%" +keyword+ "%");

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable resultTable = new DataTable();
                    adapter.Fill(resultTable);
                    dataGridView1.DataSource = resultTable;
                   
                }
            }
        }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        private void button7_Click(object sender, EventArgs e)
        {
            string txtSearch = textSearch.Text;
            search(txtSearch.Trim());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            load_user();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            bankreserve b = new bankreserve();
            b.Show();
            this.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            monitor_info a = new monitor_info();
            a.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            edit_user_info a = new edit_user_info();
            a.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            helper_reg a = new helper_reg();
            a.Show();
            this.Hide();

        }
    }
}
