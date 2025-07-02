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
    public partial class login : Form
    {
        internal static object name;
        private int userId;

        public login()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Gmail = gmail.Text.Trim();
            string Password = password.Text.Trim();

            // Input validation
            if (string.IsNullOrWhiteSpace(Gmail))
            {
                MessageBox.Show("Please enter Gmail.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Please enter Password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Monitor temp login
            if (Gmail == "monitor@gmail.com" && Password == "123")
            {
                MessageBox.Show("Monitor login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                monitor m = new monitor();
                m.Show();
                this.Hide();
                return;
            }

            string conStr = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";

            // Monitor login
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string monitorQuery = "SELECT COUNT(*) FROM monitor_info WHERE email = @email AND password = @password";
                using (SqlCommand cmd = new SqlCommand(monitorQuery, con))
                {
                    cmd.Parameters.AddWithValue("@email", Gmail);
                    cmd.Parameters.AddWithValue("@password", Password);

                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("Monitor login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        new monitor().Show();
                        this.Hide();
                        return;
                    }
                }
                con.Close();
            }

            // Admin login
            if (Gmail == "admin@gmail.com" && Password == "123")
            {
                MessageBox.Show("Admin login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                new admin().Show();
                this.Hide();
                return;
            }

            // Lender login
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string lenderQuery = "SELECT COUNT(*) FROM Table_Lender_info WHERE email = @email AND password = @password";
                using (SqlCommand cmd = new SqlCommand(lenderQuery, con))
                {
                    cmd.Parameters.AddWithValue("@email", Gmail);
                    cmd.Parameters.AddWithValue("@password", Password);

                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("Lender login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        

                        lender_dashboard dashboard = new lender_dashboard(Gmail);
                        dashboard.Show();
                        this.Hide();
                        return;
                    }
                }
                con.Close();
            }

            // Borrower login
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = "SELECT borrower_id, email FROM Table_b_info WHERE email = @email AND password = @password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@email", Gmail);
                cmd.Parameters.AddWithValue("@password", Password);

                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userId = Convert.ToInt32(reader["borrower_id"]);
                        string userEmail = reader["email"].ToString();

                        // ✅ Only login here
                        MessageBox.Show("Login successful!");
                        b_dashboard dashboard = new b_dashboard(userId, userEmail);
                        dashboard.Show();
                        this.Hide();
                        return;
                    }
                    else
                    {
                        // ❌ Show only if login failed
                        MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }




            // Helper login
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string helperQuery = "SELECT COUNT(*) FROM helper_info WHERE email = @email AND password = @password";
                using (SqlCommand cmd = new SqlCommand(helperQuery, con))
                {
                    cmd.Parameters.AddWithValue("@email", Gmail);
                    cmd.Parameters.AddWithValue("@password", Password);

                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("Helper login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        new hepler_dashboard().Show();
                        this.Hide();
                        return;
                    }
                }
                con.Close();
            }

            MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Main main = new Main();
            main.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            forgetPassword forgetPasswordForm = new forgetPassword();
            forgetPasswordForm.Show();
            this.Hide();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            password.UseSystemPasswordChar = !password.UseSystemPasswordChar;

            if (password.UseSystemPasswordChar)
            {
                button5.Text = " ";
            }
            else
            {
                button5.Text = "  ";
            }
        }
    }
}
