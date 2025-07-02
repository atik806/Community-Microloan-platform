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
    public partial class monitor_info : Form
    {
        public monitor_info()
        {
            InitializeComponent();
            string[] gender = { "Male", "Female" };
            gender[0] = "Male";
            gender[1] = "Female";
            comboBox1.DataSource = gender;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Name = name.Text;
            string email = textBox1.Text;
            var phone = textBox2.Text;
            string password = textBox4.Text;
            string gender = comboBox1.Text;
            string address = richTextBox1.Text;
            var nid = textBox5.Text;
            //string dob = dateTimePicker1.Text;

            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";

            string insertQuery = "INSERT INTO monitor_info (name,email,phone,date,nid,address,password) VALUES (@name,@email,@phone,@date,@nid,@address,@password)";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {

                    cmd.Parameters.AddWithValue("name", Name);
                    cmd.Parameters.AddWithValue("email", email);
                    cmd.Parameters.AddWithValue("phone", phone);
                    cmd.Parameters.AddWithValue("password", password);
                    cmd.Parameters.AddWithValue("gender", gender);
                    cmd.Parameters.AddWithValue("address", address);
                    cmd.Parameters.AddWithValue("nid", nid);
                    cmd.Parameters.AddWithValue("date", dateTimePicker1.Value.Date);

                    int rowAffected = cmd.ExecuteNonQuery();
                    if (rowAffected > 0)
                    {
                        MessageBox.Show("Monitor info save successfully!", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        login l = new login();
                        l.Show();
                        this.Hide();
                        return;

                       

                    }
                    else
                    {
                        MessageBox.Show("Failed to save Monitor information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
