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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace borrowersignup
{
    public partial class repay_loan : Form
    {
        private int loggedBorrowerId;
        private string loggedEmail;
        string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
        
        public repay_loan(int userId, string email)
        {
            InitializeComponent();
            loggedBorrowerId = GetBorrowerIdFromEmail(email);
            loggedEmail = email;
            loadDetails();
            showRemainingDue();
            LoadDueAmount();

            string [] type = {"Bkash", "Nagad", "Rocket" };
            type[0] = "Bkash";
            type[1] = "Nagad";
            type[2] = "Rocket";
            comboBox1.DataSource = type;




        }

        private int GetBorrowerIdFromEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Email is empty. Cannot look up borrower ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = "SELECT borrower_id FROM Table_b_info WHERE email = @email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@email", email);
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    return id;
                }
                else
                {
                    MessageBox.Show("Borrower not found with the given email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
        }
        private void loadDetails()
        {
            string query = "SELECT * FROM Table_b_info WHERE email = @email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@email", loggedEmail);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        textBox3.Text = reader["f_name"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No record found with this email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //for showing remaining due amount
        // For showing remaining due amount
        private void showRemainingDue()
        {
            if (loggedBorrowerId == -1)
            {
                MessageBox.Show("Invalid borrower ID. Cannot calculate remaining due.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }







            string query = "SELECT Ammount FROM LoanRequests WHERE Borrower_id = @Borrower_id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Borrower_id", loggedBorrowerId);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        // Convert to decimal if Ammount is decimal, or int if it's int in your database
                        decimal remainingDue = Convert.ToDecimal(result);
                        textBox4.Text = remainingDue.ToString("0.00"); // Shows two decimal places
                    }
                    else
                    {
                        MessageBox.Show("No loan record found for this borrower.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        textBox4.Text = "0.00";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error fetching remaining due: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }







        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string repayAmountText = textBox1.Text.Trim();

            if (!int.TryParse(repayAmountText, out int repayAmount) || repayAmount <= 0)
            {
                MessageBox.Show("Please enter a valid repayment amount.");
                return;
            }

            if (!int.TryParse(textBox4.Text.Trim(), out int currentDue))
            {
                MessageBox.Show("Cannot read current due amount.");
                return;
            }

            if (repayAmount > currentDue)
            {
                MessageBox.Show("Repayment amount exceeds remaining due.");
                return;
            }

            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectDueQuery = @"
            SELECT TOP 1 Id 
            FROM LoanRequests 
            WHERE borrower_id = @borrowerId AND status = 'Accepted' 
            ORDER BY Id DESC";

                int loanId = 0;

                using (SqlCommand selectCmd = new SqlCommand(selectDueQuery, connection))
                {
                    selectCmd.Parameters.AddWithValue("@borrowerId", loggedBorrowerId);

                    using (SqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            loanId = Convert.ToInt32(reader["Id"]);
                        }
                        else
                        {
                            MessageBox.Show("No accepted loan found for this borrower.");
                            return;
                        }
                    }
                }

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Deduct repayment from due amount
                        string updateDueQuery = @"
                    UPDATE loanRequests 
                    SET due_amount = due_amount - @repayAmount 
                    WHERE Id = @loanId";

                        using (SqlCommand updateCmd = new SqlCommand(updateDueQuery, connection, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@repayAmount", repayAmount);
                            updateCmd.Parameters.AddWithValue("@loanId", loanId);
                            updateCmd.ExecuteNonQuery();
                        }

                        // Update total_money table
                        string updateBankQuery = "UPDATE total_money SET [available_money] = [available_money] + @repayAmount";

                        using (SqlCommand updateBankCmd = new SqlCommand(updateBankQuery, connection, transaction))
                        {
                            updateBankCmd.Parameters.AddWithValue("@repayAmount", repayAmount);
                            updateBankCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        MessageBox.Show("Repayment successful.");

                        // Reload remaining due amount
                        LoadDueAmount();

                        // Clear input
                        textBox1.Text = "";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error processing repayment: " + ex.Message);
                    }
                }
            }
        }


        private void LoadDueAmount()
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT TOP 1 due_amount 
            FROM loanRequests 
            WHERE borrower_id = @borrowerId AND status = 'Accepted' 
            ORDER BY Id DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@borrowerId", loggedBorrowerId);

                    try
                    {
                        connection.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            int remainingDue = Convert.ToInt32(result);
                            textBox4.Text = remainingDue.ToString();
                            button1.Enabled = remainingDue > 0;
                        }
                        else
                        {
                            textBox4.Text = "0";
                            button1.Enabled = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error loading due amount: " + ex.Message);
                        textBox4.Text = "0";
                        button1.Enabled = false;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            b_dashboard dashboard = new b_dashboard(loggedBorrowerId, loggedEmail);
            dashboard.Show();
            this.Hide();
        }
    }
}
