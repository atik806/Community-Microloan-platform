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
    public partial class borrowerUpdate : Form
    {
        private int borrower_id;

        public borrowerUpdate(int id)
        {
            InitializeComponent();
            borrower_id = id;         
            LoadMonitorData();        
        }

        private void LoadMonitorData()
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = "SELECT * FROM Table_b_info WHERE borrower_id = @borrower_id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@borrower_id", borrower_id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    textBox1.Text = reader["f_name"].ToString();
                    textBox4.Text = reader["email"].ToString();
                    textBox3.Text = reader["phone"].ToString();
                    textBox2.Text = reader["password"].ToString();
                    comboBox1.Text = reader["gender"].ToString();
                    textBox5.Text = reader["nid"].ToString();
                    richTextBox1.Text = reader["address"].ToString();

                    // Check for NULL before converting date
                    if (reader["dob"] != DBNull.Value)
                    {
                        dateTimePicker1.Value = Convert.ToDateTime(reader["dob"]);
                    }
                    else
                    {
                        dateTimePicker1.Value = DateTime.Today;
                    }
                }
                else
                {
                    MessageBox.Show("No borrower found with the given ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            all_b_info a = new all_b_info();
            a.Show();
            this.Hide();
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
            string dob = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";

            string updateQuery = "UPDATE Table_b_info " +
                     "SET f_name = @f_name, phone = @phone, password = @password, address = @address, dob = @dob " +
                     "WHERE email = @email AND nid = @nid";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@f_name", name);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@password", password);
                    //command.Parameters.AddWithValue("@gender", gender);
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@dob", dob);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@nid", nid);


                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Borrower information updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No matching record found to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

        }
    }
}
