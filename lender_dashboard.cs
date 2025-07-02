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
    public partial class lender_dashboard : Form
    {
        private string loggedEmail;
        private int loggedLenderId;

        public lender_dashboard(string email)
        {
            InitializeComponent();
            loggedLenderId = GetLenderIdFromEmail(email);
            loggedEmail = email; 
            loadDetails();       
            loadPendingRequests();
        }

        private int GetLenderIdFromEmail(string email)
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = "SELECT Id FROM Table_Lender_info WHERE Email = @Email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@email", email);
                connection.Open();
                object result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : -1;
            }
        }



        private void loadDetails()
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = "SELECT * FROM Table_Lender_info WHERE email = @email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@email", loggedEmail);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    textBox1.Text = reader["name"].ToString(); 
                }
                else
                {
                    MessageBox.Show("No lender found with the provided email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void loadPendingRequests()
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = "SELECT Id, Ammount, Duration, Purpose, Status FROM LoanRequests WHERE Status = 'Pending' AND lender_id = @LenderId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LenderId", loggedLenderId);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
        }

        string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";


        private void Accept_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a loan request from the list.");
                return;
            }

            int requestId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
            decimal loanAmount = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells["Ammount"].Value);

            decimal lenderTotal = 0;
            decimal bankTotal = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Get SUM(balance) from lender table
                string queryLender = "SELECT SUM(balance) FROM Table_Lender_info";
                SqlCommand cmdLender = new SqlCommand(queryLender, connection);
                var lenderResult = cmdLender.ExecuteScalar();
                if (lenderResult != DBNull.Value)
                    lenderTotal = Convert.ToDecimal(lenderResult);

                // Get bankowns
                string queryBank = "SELECT bankowns FROM total_money";
                SqlCommand cmdBank = new SqlCommand(queryBank, connection);
                var bankResult = cmdBank.ExecuteScalar();
                if (bankResult != DBNull.Value)
                    bankTotal = Convert.ToDecimal(bankResult);

                // Combined total
                decimal totalAvailable = lenderTotal + bankTotal;

                if (loanAmount > totalAvailable)
                {
                    MessageBox.Show("Insufficient total available funds.");
                    return;
                }

                // Deduct from bankowns (as an example)
                decimal newBankTotal = bankTotal - loanAmount;

                if (newBankTotal < 0)
                {
                    MessageBox.Show("Bank does not have enough funds. Lender deduction logic not implemented.");
                    return;
                }

                // Update bankowns in database
                string updateQuery = "UPDATE total_money SET bankowns = @bankowns";
                SqlCommand updateCmd = new SqlCommand(updateQuery, connection);
                updateCmd.Parameters.AddWithValue("@bankowns", newBankTotal);
                updateCmd.ExecuteNonQuery();

                // Update loan status
                UpdateLoanStatus(requestId, "Accepted");

                MessageBox.Show("Loan accepted and deducted from total available.");
            }
        }




        private void Reject_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a loan request to reject.");
                return;
            }

            int requestId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);
            UpdateLoanStatus(requestId, "Rejected");
        }

        private void UpdateLoanStatus(int requestId, string status)
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = "UPDATE LoanRequests SET Status = @Status WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Id", requestId);
                connection.Open();
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show($"Loan request {status.ToLower()} successfully.");
                    loadPendingRequests();
                }
                else
                {
                    MessageBox.Show("Operation failed.");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Lender_problem p = new Lender_problem(loggedEmail);
            p.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = "SELECT * FROM Table_b_info";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
            {
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            login login = new login();
            login.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            login l = new login();
            l.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void lender_dashboard_Load(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
