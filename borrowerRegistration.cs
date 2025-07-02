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
    public partial class borrowerRegistration : Form
    {
        public borrowerRegistration()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            hepler_dashboard a = new hepler_dashboard();
            a.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Name = name.Text;
            string email = textBox1.Text;
            string phone = textBox2.Text;
            string nid = textBox3.Text;
            string address = richTextBox2.Text;
            string loanPurpose = richTextBox1.Text;
            string loanAmmount = textBox4.Text;
            string date = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string insertQuery = "INSERT INTO b_reg_helper (name, email, phone, nid, address, loan_purpose, status, ammount,date) VALUES (@name, @email, @phone, @nid, @address, @loanPurpose, @status, @ammount,@date)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@status", "Pending");
                    command.Parameters.AddWithValue("@name", Name);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@nid", nid);
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@loanPurpose", loanPurpose);
                    command.Parameters.AddWithValue("@ammount", loanAmmount);
                    command.Parameters.AddWithValue("@date", date);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Borrower registration request submitted and pending approval.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }
                    else
                    {
                        MessageBox.Show("Registration failed. Try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
            }
        }
    }
}
