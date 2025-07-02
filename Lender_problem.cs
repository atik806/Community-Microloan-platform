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
    public partial class Lender_problem : Form
    {
        private string loggedEmail;
        public Lender_problem(string email)
        {
            InitializeComponent();
            loggedEmail = email; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string problem = richTextBox1.Text;
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string insertQuery = "INSERT INTO Table_L_probelm (Lender_Problem) VALUES (@Lender_Problem)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Lender_Problem", problem);
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Problem reported successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error reporting problem: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lender_dashboard d = new lender_dashboard(loggedEmail);
            d.Show();
            this.Hide();

        }
    }
}
