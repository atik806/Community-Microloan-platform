using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace borrowersignup
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary> 
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Screens navigate by hiding themselves instead of closing, so the
            // hidden start form would keep the process alive after the user
            // closes the last window. Exit once no form is visible any more.
            Application.Idle += (s, e) =>
            {
                if (!Application.OpenForms.Cast<Form>().Any(f => f.Visible))
                    Application.Exit();
            };

            Application.Run(new Main());
        }
    }
}
