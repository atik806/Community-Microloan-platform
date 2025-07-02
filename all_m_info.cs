using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace borrowersignup
{
    public partial class all_m_info : Form
    {
        public all_m_info()
        {
            InitializeComponent();
            load_user();
        }

        // Load all monitor users
        private void load_user()
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = "SELECT id, Name, Phone, Email, nid, date FROM monitor_info";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;

                // Optional: hide the ID column if you don't want to show it
                if (dataGridView1.Columns.Contains("id"))
                {
                    dataGridView1.Columns["id"].Visible = false;
                }
            }
        }

        // Search function
        private void search(string keyword)
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = @"
                SELECT id, name, phone, email, nid, date
                FROM monitor_info
                WHERE name LIKE @searchTerm 
                OR phone LIKE @searchTerm 
                OR email LIKE @searchTerm 
                OR nid LIKE @searchTerm 
                OR date LIKE @searchTerm";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@searchTerm", "%" + keyword + "%");
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable resultTable = new DataTable();
                        adapter.Fill(resultTable);
                        dataGridView1.DataSource = resultTable;

                        if (dataGridView1.Columns.Contains("id"))
                        {
                            dataGridView1.Columns["id"].Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int monitorId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);
            MonitorUpdate updateForm = new MonitorUpdate(monitorId);
            updateForm.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string txtSearch = textSearch.Text.Trim();
            if (!string.IsNullOrEmpty(txtSearch))
                search(txtSearch);
            else
                MessageBox.Show("Please enter a search term.");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            load_user();
        }

        private void all_m_info_Load(object sender, EventArgs e) 
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) 
        { 
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            edit_user_info a = new edit_user_info();
            a.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete the selected user?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Get selected borrower's ID
                    int monitorId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);

                    string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
                    string query = "DELETE FROM monitor_info WHERE id = @id";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = monitorId;
                        conn.Open();

                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            MessageBox.Show("User deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            load_user(); // Refresh table
                        }
                        else
                        {
                            MessageBox.Show("User not found or could not be deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during deletion: " + ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
