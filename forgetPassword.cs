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
using System.Xml.Linq;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace borrowersignup
{
    public partial class forgetPassword : Form
    {
        public forgetPassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            string f_email = textBox1.Text;
            string connectionString = "Data Source=ATIK\\SQLEXPRESS;Initial Catalog=b_info;Integrated Security=True";
            string query = "SELECT f_name, Email FROM Table_b_info WHERE Email = @Email";
            if (dataGridView1.SelectedRows.Count > 0)
            
                {
                    MessageBox.Show("Please select you account.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //string query = "SELECT * FROM Users WHERE Email = @Email";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@Email", f_email);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dt;
                    }
                    else
                    {
                        MessageBox.Show("Email not found.");
                        dataGridView1.DataSource = null;
                    }
                }
            }
            

        

        private void forgetPassword_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Please enter an email.");
                return;
            }

            updatepassword updateForm = new updatepassword(email); 
            updateForm.ShowDialog(); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            login a = new login();
            a.Show();
            this.Hide();

        }
    }
}
