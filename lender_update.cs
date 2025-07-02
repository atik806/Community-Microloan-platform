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
    public partial class lender_update : Form
    {
        private int lenderId;
        public lender_update(int id)
        {
            InitializeComponent();
            lenderId = id;
            LoadMonitorData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            edit_user_info a = new edit_user_info();
            a.Show();
            this.Hide();

        }
        private void LoadMonitorData()
        {
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = "SELECT * FROM Table_Lender_info WHERE id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id",lenderId);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    name.Text = reader["name"].ToString();
                    textBox1.Text = reader["email"].ToString();
                    textBox2.Text = reader["phone"].ToString();
                    textBox4.Text = reader["password"].ToString();
                    textBox3.Text = reader["interest_rate"].ToString();
                    textBox5.Text = reader["nid"].ToString();
                    richTextBox1.Text = reader["address"].ToString();
                    comboBox1.Text = reader["gender"].ToString();

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

        
        private void button1_Click(object sender, EventArgs e)
        {
            {
                string Name = name.Text;
                string email = textBox1.Text;
                string phone = textBox2.Text;
                string password = textBox4.Text;
                string gender = comboBox1.Text;
                string nid = textBox5.Text;
                string address = richTextBox1.Text;
                string dob = dateTimePicker1.Value.ToString("yyyy-MM-dd");

                string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";

                string updateQuery = "UPDATE Table_Lender_info " +
                                     "SET name = @name, phone = @phone, password = @password, address = @address, dob = @dob " +
                                     "WHERE email = @email AND nid= @nid";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@name", Name);
                        command.Parameters.AddWithValue("@phone", phone);
                        command.Parameters.AddWithValue("@password", password);
                        command.Parameters.AddWithValue("@gender", gender);
                        command.Parameters.AddWithValue("@address", address);
                        command.Parameters.AddWithValue("@dob", dob);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@nid", nid);


                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Lender information updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No matching record found to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void lender_update_Load(object sender, EventArgs e)
        {

        }
    }
}
