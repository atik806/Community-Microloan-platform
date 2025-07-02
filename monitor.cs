using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace borrowersignup
{
    public partial class monitor : Form
    {
        string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
        public monitor()
        {
            InitializeComponent();
            LoadPendingRequests();
        }
        private void LoadPendingRequests()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT borrower_id, f_name, email, phone, nid, status,dob FROM Table_b_info WHERE status = 'Pending'";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a borrower to accept.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int borrower_id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["borrower_id"].Value);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string updateQuery = "UPDATE Table_b_info SET status = 'Accepted' WHERE borrower_id = @borrower_id";
                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@borrower_id", borrower_id);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Borrower accepted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadPendingRequests();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a borrower to reject.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["borrower_id"].Value);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string updateQuery = "UPDATE Table_b_info SET status = 'Rejected' WHERE borrower_id = @borrower_id";
                    using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@borrower_id", id);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Borrower rejected successfully.", "Rejected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                LoadPendingRequests();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            login Login = new login();
            Login.Show();
            this.Hide();


        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Monitor_problem p = new Monitor_problem();
            p.Show();
            this.Hide();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Helper_requests a = new Helper_requests();
            a.Show();
            this.Hide();
        }
    }
}
