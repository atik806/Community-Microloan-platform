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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace borrowersignup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string[] gender = new string[2];
            gender[0] = "Male";
            gender[1] = "Female";
            comboBox1.DataSource = gender;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string email = textBox4.Text;
            string gender = comboBox1.Text;
            var phone = textBox3.Text;
            var nid = textBox5.Text;
            string password = textBox2.Text;
            string address = richTextBox1.Text;
            //string dob = dateTimePicker1.Value.ToString("yyyy-MM-dd");


            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter Name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please enter Email.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Please enter Address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter Password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(gender))
            {
                MessageBox.Show("Please select Gender.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Please enter Phone number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(nid))
            {
                MessageBox.Show("Please enter National ID (NID).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            

            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string checkQuery = "SELECT COUNT(*) FROM Table_b_info WHERE f_name = @name OR email = @email";

            string insertQuery = "INSERT INTO Table_b_info (f_name, email, address, password, gender, phone, nid, dob, status) VALUES (@f_name, @email, @address, @password, @gender, @phone, @nid, @dob, @status)";

            //string insertQuery = "INSERT INTO Table_b_info (f_name, email, address, password, gender, phone, dob, nid, status) VALUES (@f_name, @email, @address, @password, @gender, @phone, @nid, @dob, @status)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                {
                    checkCmd.Parameters.AddWithValue("@name", name);
                    checkCmd.Parameters.AddWithValue("@email", email);
                    //checkCmd.Parameters.AddWithValue("@phone", phone);
                    checkCmd.Parameters.AddWithValue("@nid", nid);
                    //checkCmd.Parameters.AddWithValue("@password", password);
                    //checkCmd.Parameters.AddWithValue("@address", address);
                    

                    int existingCount = (int)checkCmd.ExecuteScalar();

                    if (existingCount > 0)
                    {
                        MessageBox.Show("User with this Name already exists.", "Signup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@f_name", name);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@address", address);     
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@gender", gender);     
                    command.Parameters.AddWithValue("@phone", phone);       
                    command.Parameters.AddWithValue("@nid", nid);           
                    command.Parameters.AddWithValue("@status", "Pending"); 
                    command.Parameters.AddWithValue("@dob", dateTimePicker1.Value.Date);
                    int rowsAffected = command.ExecuteNonQuery();

                  
                    //int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Signup request submitted and pending approval.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        login lo = new login();
                        lo.Show();
                        this.Hide();

                    }
                    else
                    {
                        MessageBox.Show("Signup failed. Try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            login login = new login();
            login.Show();
            this.Hide();
        }
    }
}
