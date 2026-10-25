using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace borrowersignup
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            login Login =new login();
            Login.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            about About = new about();
            About.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            howitworks a = new howitworks();
            a.Show();
            this.Hide();
        }
    }
}
