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
    public partial class helper_reg : Form
    {
        public helper_reg()
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
            string email = textBox1.Text;
            string phone = textBox2.Text;
            string nid = textBox5.Text;
            string password = textBox4.Text;
            string address = richTextBox1.Text;
            string date = dateTimePicker1.Text;
            string gender = comboBox1.Text;

            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string insertQuery = "INSERT INTO helper_info (name, email, address, password, gender, phone, nid, date) VALUES (@name, @email, @address, @password, @gender, @phone, @nid, @date)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", Name);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@gender", gender);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@nid", nid);
                    //command.Parameters.AddWithValue("@date", date);
                   
                    command.Parameters.AddWithValue("@date", dateTimePicker1.Value.Date);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Helper information saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            admin a = new admin();
            a.Show();
            this.Hide();
        }
    }
}
