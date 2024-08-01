using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ducky_CMD
{
    public partial class IGN_form : Form
    {
        private string directory = DuckyCMD.directory;
        public IGN_form()
        {
            InitializeComponent();
            loadIGN();
        }


        private void loadIGN()
        {
           
            //load the IGN from the file and display it in the textbox "directory\names.txt"
            if (System.IO.File.Exists(directory + @"\names.txt"))
            {
                string[] lines = System.IO.File.ReadAllLines(directory + @"\names.txt");
                foreach (string line in lines)
                {
                    guna2TextBox1.Text += line + Environment.NewLine;
                }
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {

            System.IO.File.WriteAllText(directory + @"\names.txt", guna2TextBox1.Text);
            MessageBox.Show("Saved ! Pls relaunch the DuckyVIP of the VMware", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }

}
