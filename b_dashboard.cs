using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace borrowersignup
{
    public partial class b_dashboard : Form
    {
        private int loggedBorrowerId;
        private string loggedEmail;

         string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
        //private int userId;
        //private string email;

        public b_dashboard(int userId, string email)
        {
            InitializeComponent();
            loggedBorrowerId = GetBorrowerIdFromEmail(email);
            loggedEmail = email;
            loadLenderData();
            loadDetails();
        }

        private void loadLenderData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT id, name, email, phone, interest_rate, gender, nid FROM Table_Lender_info";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                //adapter.SelectCommand.Parameters.AddWithValue("@id", loggedBorrowerId);

                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
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
                        textBox1.Text = reader["f_name"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No record found with this email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
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


        private void button1_Click(object sender, EventArgs e)
        {
            // Fix for CS0103: Define 'loggedInUserId' and ensure it is assigned the correct value.  
            int loggedInUserId = loggedBorrowerId; // Assuming 'loggedBorrowerId' is the correct ID to pass.  

            // Fix for CS1503: Ensure the constructor of 'loanrequest' accepts the correct types.  
            loanrequest loanrequest = new loanrequest(loggedInUserId, loggedEmail);
            loanrequest.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            b_problem pr = new b_problem();
            pr.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            login Lo = new login();
            Lo.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            login l = new login();
            l.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            repay_loan a = new repay_loan(loggedBorrowerId, loggedEmail);
            a.Show();
            this.Hide();
        }
    }
}
