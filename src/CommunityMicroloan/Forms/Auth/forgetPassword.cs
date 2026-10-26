using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace borrowersignup
{
    public partial class forgetPassword : Form
    {
        private const int MaxAttempts = 5;

        private int failedAttempts;
        private int verifiedBorrowerId = -1;

        public forgetPassword()
        {
            InitializeComponent();
        }

        // Verify: the email alone is not proof of ownership, so the NID and phone
        // number on the account must match too before a reset is allowed.
        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim();
            string nidText = textBox2.Text.Trim();
            string phoneText = textBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(nidText) || string.IsNullOrWhiteSpace(phoneText))
            {
                MessageBox.Show("Please enter your email, NID and phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int borrowerId = -1;

            // nid and phone are int columns; non-numeric input simply can't match.
            if (int.TryParse(nidText, out int nid) && int.TryParse(phoneText, out int phone))
            {
                string query = "SELECT borrower_id FROM Table_b_info WHERE email = @email AND nid = @nid AND phone = @phone";

                using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@nid", nid);
                    command.Parameters.AddWithValue("@phone", phone);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        borrowerId = Convert.ToInt32(result);
                }
            }

            if (borrowerId == -1)
            {
                failedAttempts++;
                if (failedAttempts >= MaxAttempts)
                {
                    MessageBox.Show("Too many failed attempts. Please try again later.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    BackToLogin();
                    return;
                }

                // Same message whichever field is wrong, so the form can't be used
                // to find out which emails are registered.
                MessageBox.Show("Those details don't match any account.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            verifiedBorrowerId = borrowerId;
            textBox1.ReadOnly = textBox2.ReadOnly = textBox3.ReadOnly = true;
            button1.Enabled = false;
            button2.Enabled = true;
            MessageBox.Show("Account verified. You can now set a new password.", "Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void forgetPassword_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (verifiedBorrowerId == -1)
            {
                MessageBox.Show("Please verify your account first.");
                return;
            }

            using (updatepassword updateForm = new updatepassword(verifiedBorrowerId))
            {
                if (updateForm.ShowDialog(this) == DialogResult.OK)
                    BackToLogin();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BackToLogin();
        }

        private void BackToLogin()
        {
            login a = new login();
            a.Show();
            this.Hide();
        }
    }
}
