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
    public partial class lender_info : Form
    {
        public lender_info()
        {
            InitializeComponent();
            string[] gender = { "Male", "Female" };
            gender[0] = "Male";
            gender[1] = "Female";
            comboBox1.DataSource = gender;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Name = name.Text;
            string Email = textBox1.Text;
            string Phone = textBox2.Text;
            string Interest_rate = textBox3.Text;
            string password = textBox4.Text;
            string gender = comboBox1.Text;
            string nid = textBox5.Text;
            string address = richTextBox1.Text;
            string balance = textBox6.Text;

            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Please enter Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(balance))
            {
                MessageBox.Show("Please enter Balance.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Please enter Email.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(Phone))
            {
                MessageBox.Show("Please enter Phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(Interest_rate))
            {
                MessageBox.Show("Please enter Interest rate.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter Password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(nid))
            {
                MessageBox.Show("Please enter NID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Please enter Address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";

            string insertQuery = "INSERT INTO Table_Lender_info (name, email, address, password, gender, phone, nid, dob, interest_rate, Balance) VALUES (@name, @email, @address, @password, @gender, @phone, @nid, @dob, @interest_rate, @Balance)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", Name);
                    command.Parameters.AddWithValue("@email", Email);
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@gender", gender);
                    command.Parameters.AddWithValue("@phone", Phone);
                    command.Parameters.AddWithValue("@nid", nid);
                    command.Parameters.AddWithValue("@interest_rate", Interest_rate);
                    command.Parameters.AddWithValue("@Balance", balance);
                    command.Parameters.AddWithValue("@dob", dateTimePicker1.Value.Date);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Lender information saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Form1 form1 = new Form1();
                        form1.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Failed to save lender information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            admin Admin = new admin();
            Admin.Show();
            this.Hide();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }
    }
}