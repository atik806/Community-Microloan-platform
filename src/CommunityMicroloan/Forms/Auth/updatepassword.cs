using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace borrowersignup
{
    public partial class updatepassword : Form
    {
        private const int MinPasswordLength = 6;

        private readonly int _borrowerId;

        // Only opened by forgetPassword after the borrower's email, NID and phone
        // have been verified, so the target account is fixed by its id.
        internal updatepassword(int borrowerId)
        {
            InitializeComponent();
            _borrowerId = borrowerId;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string newPassword = textBox1.Text.Trim();
            string confirmPassword = textBox2.Text.Trim();

            if (newPassword.Length < MinPasswordLength)
            {
                MessageBox.Show($"Password must be at least {MinPasswordLength} characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("The two passwords don't match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "UPDATE Table_b_info SET password = @password WHERE borrower_id = @borrower_id";

            using (SqlConnection connection = new SqlConnection(Db.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@password", newPassword);
                command.Parameters.AddWithValue("@borrower_id", _borrowerId);

                connection.Open();
                if (command.ExecuteNonQuery() == 0)
                {
                    MessageBox.Show("Account not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            MessageBox.Show("Password updated successfully. Please log in with your new password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
