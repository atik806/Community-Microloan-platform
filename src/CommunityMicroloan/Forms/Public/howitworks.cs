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
    public partial class howitworks : Form
    {
        public howitworks()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.nerdwallet.com/article/small-business/microloans#:~:text=Microloans%20typically%20work%20like%20traditional,vary%20based%20on%20your%20lender."); 
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            process p = new process();
            p.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Main m = new Main();
            m.Show();
            this.Hide();
        }
    }
}
