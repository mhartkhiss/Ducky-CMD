using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ducky_CMD
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
            //if directory exist "Z:\MACRODUCK" then run the application
            //else show a message box
            if (System.IO.Directory.Exists(@"Z:\MACRODUCK"))
            {
                Application.Run(new DuckyVIP());
            }
            else
            {
                Application.Run(new DuckyCMD());
            }
        }
    }
}
