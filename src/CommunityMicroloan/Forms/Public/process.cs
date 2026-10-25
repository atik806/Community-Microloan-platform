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
    public partial class process : Form
    {
        public process()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            howitworks h = new howitworks();
            h.Show();
            this.Hide();
        }
    }
}
