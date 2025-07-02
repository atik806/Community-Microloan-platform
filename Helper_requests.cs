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
    public partial class Helper_requests : Form
    {
        public Helper_requests()
        {
            InitializeComponent();
            LoadPendingRequests();
        }

        string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
        private void LoadPendingRequests()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT id,name, email, phone, nid, status, date FROM b_reg_helper WHERE status = 'Pending'";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;       
            }
        }

        
        private void Accept_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to accept.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int helperid = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string updateQuery = "UPDATE b_reg_helper SET status = 'Accepted' WHERE id = @id";  
                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@id", helperid);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Accepted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadPendingRequests();
        }

        private void Reject_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to reject.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string updateQuery = "UPDATE b_reg_helper SET status = 'Rejected' WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("User rejected successfully.", "Rejected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            LoadPendingRequests();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            monitor a = new monitor();
            a.Show();
            this.Hide();

        }
    }
}
