using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace borrowersignup
{
    public partial class loanrequest : Form
    {
        private int userId;
        private string userEmail;
        private string loggedEmail;
        private object loggedInUserId;

        public loanrequest(int userId, string email)
        {
            InitializeComponent();
            this.userId = userId;
            this.userEmail = email;
            string[] duration = {
                "Students: 6–24 months",
                "Small Businesses: 12–36 months",
                "Entrepreneurs: 12–60 months"
            };
            comboBox1.DataSource = duration;
        }

        public loanrequest(string loggedEmail, object loggedInUserId)
        {
            this.loggedEmail = loggedEmail;
            this.loggedInUserId = loggedInUserId;
        }

        private void loanrequest_Load(object sender, EventArgs e)
        {
            LoadLenderData();
        }

        private void LoadLenderData()
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = "SELECT * FROM Table_Lender_info"; 

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading lender data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

                private void button1_Click(object sender, EventArgs e)
                {
                    b_dashboard dashboard = new b_dashboard(userId, userEmail);
                    dashboard.Show();
                    this.Hide();
                }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a lender from the list.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ammount = textbox.Text.Trim();
            string duration = comboBox1.Text;
            string purpose = richTextBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(purpose))
            {
                MessageBox.Show("Please enter the purpose of the loan.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int lenderID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id"].Value); 

            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = "INSERT INTO LoanRequests (Ammount, Duration, Purpose, Status, Borrower_id, lender_id) " +
                           "VALUES (@Ammount, @Duration, @Purpose, @Status, @Borrower_id, @lender_id)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Ammount", ammount);
                    command.Parameters.AddWithValue("@Duration", duration);
                    command.Parameters.AddWithValue("@Purpose", purpose);
                    command.Parameters.AddWithValue("@Status", "Pending");
                    command.Parameters.AddWithValue("@Borrower_id", userId);
                    command.Parameters.AddWithValue("@lender_id", lenderID);

                    connection.Open();
                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Loan request submitted successfully. Wait for confirmation!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to submit loan request.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error submitting request: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: handle selection change
        }
    }
}
