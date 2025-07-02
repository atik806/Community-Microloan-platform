using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace borrowersignup
{
    public partial class b_problem : Form
    {
        private int userId;
        private string userEmail;

        public b_problem()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            b_dashboard da = new b_dashboard(userId, userEmail);
            da.Show();
            this.Hide();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            string problem = richTextBox1.Text;
            if (string.IsNullOrWhiteSpace(problem))
            {
                MessageBox.Show("Please enter Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string insertQuery = "INSERT INTO Table_b_problems (Borrower_problems) VALUES (@Borrower_problems)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Borrower_problems", problem);
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

    }
}
