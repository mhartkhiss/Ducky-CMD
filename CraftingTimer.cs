using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace Ducky_CMD
{
    public partial class CraftingTimer : Form
    {

        private int craftingTime = 0, totalTime = 0, timeDifference = 0;

        public CraftingTimer()
        {
            InitializeComponent();
            init();
        }


        public void init()
        {
            btn_green.Visible = false;
            btn_red.Visible = false;
            btn_yellow.Visible = false;
            btn_ice.Visible = false;
            btn_purple.Visible = false;
            btn_water.Visible = false;
            txt_timer.Text = "";
            txt_island.Text = "";

            if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\craftingTime.txt"))
            {
                totalTime = int.Parse(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\craftingTime.txt"));
                //calculate the time difference between the modified date of file and the current date and store it in timeDifference
                timeDifference = (int)(DateTime.Now - System.IO.File.GetLastWriteTime(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\craftingTime.txt")).TotalSeconds;
                craftingTime = totalTime - timeDifference;

            }
            else
            {

                craftingTime = (getOres() * 10);
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\craftingTime.txt", craftingTime.ToString());
            }

            timer1.Interval = 1000;
            timer1.Start();
            //Clipboard.SetText("CraftingTimerStarted");
            btnVisibility();



        }

        private void btnVisibility() {

            if (System.IO.File.Exists(@"C:\mine_green.txt") && !btn_green.Visible)
            {
                btn_green.Visible = true;
            }
            else if (!System.IO.File.Exists(@"C:\mine_green.txt") && btn_green.Visible)
            {
                btn_green.Visible = false;
            }

            if (System.IO.File.Exists(@"C:\mine_red.txt") && !btn_red.Visible)
            {
                btn_red.Visible = true;
            }
            else if (!System.IO.File.Exists(@"C:\mine_red.txt") && btn_red.Visible)
            {
                btn_red.Visible = false;
            }

            if (System.IO.File.Exists(@"C:\mine_yellow.txt") && !btn_yellow.Visible)
            {
                btn_yellow.Visible = true;
            }
            else if (!System.IO.File.Exists(@"C:\mine_yellow.txt") && btn_yellow.Visible)
            {
                btn_yellow.Visible = false;
            }

            if (System.IO.File.Exists(@"C:\mine_ice.txt") && !btn_ice.Visible)
            {
                btn_ice.Visible = true;
            }
            else if (!System.IO.File.Exists(@"C:\mine_ice.txt") && btn_ice.Visible)
            {
                btn_ice.Visible = false;
            }

            if (System.IO.File.Exists(@"C:\mine_purple.txt") && !btn_purple.Visible)
            {
                btn_purple.Visible = true;
            }
            else if (!System.IO.File.Exists(@"C:\mine_purple.txt") && btn_purple.Visible)
            {
                btn_purple.Visible = false;
            }

            if (System.IO.File.Exists(@"C:\mine_water.txt") && !btn_water.Visible)
            {
                btn_water.Visible = true;
            }
            else if (!System.IO.File.Exists(@"C:\mine_water.txt") && btn_water.Visible)
            {
                btn_water.Visible = false;
            }

            //set txt_island text to the value of the saved_island.txt file in the documents folder if it exists
            if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\saved_island.txt"))
            {
                //if the contents of the saved_island.txt file is greater than 0 then set the text of txt_island to the contents of the file put a "#" in front of the number
                if (int.Parse(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\saved_island.txt")) > 0)
                {
                    txt_island.Text = "#" + System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\saved_island.txt");
                }
            }
            else
            {
                txt_island.Text = "";
            }

        }

        private int getOres() {

            int left = 778, top = 376, width = 162, height = 125;
            int ores = 0;

            try
            {
                using (Bitmap screenshot = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(screenshot))
                    {
                        g.CopyFromScreen(new Point(left, top), Point.Empty, new Size(width, height));
                    }

                    string text;
                    using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                    {
                        using (var img = PixConverter.ToPix(screenshot))
                        {
                            using (var page = engine.Process(img))
                            {
                                text = page.GetText();
                                //extract the number from the text between "/" and ")"
                                var match1 = Regex.Match(text, @"\((\d+)");
                                var match2 = Regex.Match(text, @"\/(\d+)\)");
                                //var match3 = match2 minus match1
                                if (match2.Success && match1.Success)
                                {
                                    int number = int.Parse(match2.Groups[1].Value) - int.Parse(match1.Groups[1].Value);
                                    if (number > 0)
                                    {
                                        ores = number;
                                    }
                                }

                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return ores;
        }

        private void guna2ContainerControl1_DoubleClick(object sender, EventArgs e)
        {
            //copy "Z:\MACRODUCK\sync\craftingTime.txt" to Documents folder replace if it exists
            System.IO.File.Copy(@"Z:\MACRODUCK\sync\craftingTime.txt", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\craftingTime.txt", true);
            MessageBox.Show("Crafting Time Synced");
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            craftingTime--;
            btnVisibility();
            int hours = craftingTime / 3600;
            int minutes = (craftingTime % 3600) / 60;
            int seconds = craftingTime % 60;

            //if C:\px_"anytext".txt exists then set background to Control
            if (System.IO.File.Exists(@"C:\px_green.txt"))
            {
                BackColor = ColorTranslator.FromHtml("#89C651");
            }
            else if (System.IO.File.Exists(@"C:\px_red.txt"))
            {
                BackColor = ColorTranslator.FromHtml("#FD8686");
            }
            else if (System.IO.File.Exists(@"C:\px_yellow.txt"))
            {
                BackColor = ColorTranslator.FromHtml("#F2F0B8");
            }
            else if (System.IO.File.Exists(@"C:\px_ice.txt"))
            {
                BackColor = ColorTranslator.FromHtml("#ECF7FB");
            }
            else if (System.IO.File.Exists(@"C:\px_purple.txt"))
            {
                BackColor = ColorTranslator.FromHtml("#D3B8F2");
            }
            else if (System.IO.File.Exists(@"C:\px_water.txt"))
            {
                BackColor = ColorTranslator.FromHtml("#A5DEFF");
            }
            else
            {
                this.BackColor = Color.Black;
            }

            string timerText = "";

            if (hours > 0)
            {
                timerText += $"{hours}h ";
            }

            if (minutes > 0 || hours > 0)
            {
                timerText += $"{minutes}m ";
            }

            if (seconds > 0 || minutes > 0)
            {
                timerText += $"{seconds}s";
            }

            txt_timer.Text = timerText.TrimEnd();

            if (craftingTime <= 0)
            {
                timer1.Stop();
                System.Diagnostics.Process.Start(@"Z:\MACRODUCK\miningTime.exe");
                //delete the craftingTime.txt file in the documents folder
                if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\craftingTime.txt")) 
                { 
                    System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\craftingTime.txt");
                }
                /*if (System.Diagnostics.Process.GetProcessesByName("chrome").Length == 0)
                {
                    SendKeys.SendWait("^p");
                }*/
                this.Close();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
