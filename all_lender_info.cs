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
    public partial class all_lender_info : Form
    {
        public all_lender_info()
        {
            InitializeComponent();
            load_user();    
        }

        private void button2_Click(object sender, EventArgs e)
        {
            edit_user_info a = new edit_user_info();
            a.Show();
            this.Hide();


        }

        private void load_user()
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = "SELECT id, name,phone, email, nid FROM Table_Lender_info";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;

                if (dataGridView1.Columns.Contains("id"))
                {
                    dataGridView1.Columns["id"].Visible = false;
                }
            }
        }


        private void search(string keyword)
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = @"
                SELECT name, email, phone, email, nid
                FROM Table_Lender_info
                WHERE name LIKE @searchTerm 
                OR email LIKE @searchTerm 
                OR id LIKE @searchTerm 
                OR phone LIKE @searchTerm 
                OR address LIKE @searchTerm"; 
                //OR dob LIKE @searchTerm ";

            //OR dob LIKE @searchTerm";

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

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int lenderid = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value);
            lender_update a = new lender_update(lenderid);
            a.Show();
            this.Hide();
        }
    }
}
