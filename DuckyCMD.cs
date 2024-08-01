using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Drawing;
using System.Net.NetworkInformation;

namespace Ducky_CMD
{
    public partial class DuckyCMD : Form
    {
        public static string directory = null;
        private const string CONFIG_FILE = "config.json";
        private DateTime last_jackpotTime = DateTime.Now;
        private DateTime latest_jackpotTime = DateTime.Now;
        private DateTime auto_startTime = DateTime.Now;
        private FileSystemWatcher multiFileWatcher;
        private string[] file_list = { "500.txt", "1000.txt", "2000.txt", "3000.txt", "hold.txt" };
        private string[] mode_list = { "mode_Mining.txt", "mode_Fishing.txt", "mode_Farming.txt", 
            "mode_Crafting.txt", "mode_OpenGift.txt"};
        private Color[] modeColors = { Color.FromArgb(255, 236, 161), Color.FromArgb(145, 210, 255), Color.FromArgb(164, 255, 164),
                   Color.FromArgb(255, 185, 79), Color.FromArgb(233, 154, 255)};
        private string[] gap_list = null;
        private string dummyText = null;
        private int counter=0, minutes=0, seconds = 0;
        private Image[] mode_icon = { Properties.Resources.pickaxe, Properties.Resources.fishing, Properties.Resources.carrots,
            Properties.Resources.blacksmith, Properties.Resources.gift};
        private int deducted_seconds = 12;
        IGN_form ign_form = null;
        private bool showToasts = false, resetBtnClicked = false, miningStarted = false;

        public DuckyCMD()
        {
            InitializeComponent();
            InitializeConfig();
            initialize();
            InitializeFileWatcher();
        }
        private void InitializeConfig()
        {
            if (!File.Exists(CONFIG_FILE))
            {
                check_directory();
            }
            else
            {
                LoadConfig();
            }
        }

        

        private void initialize() {

            latest_jackpotTime = File.GetLastWriteTime(directory + "/lastjackpot.txt");

            label_dcm.Text = File.ReadAllText(directory + "/total_dcm.txt");
            label_jackpot.Text = File.ReadAllText(directory + "/jackpot_counter.txt");
            label_currentTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            label_latestJackpotTime.Text = latest_jackpotTime.ToString("hh:mm:ss tt");
            label_lastJackpotTime.Text = "-----------";
            label_autoStartTime.Text = "-----------";
            label_timeDiffStart.Text = "";
            label_timeDiffStart.Visible = false;
            label_timeDiff.Text = "";

            string jsonString = File.ReadAllText(CONFIG_FILE);
            JObject config = JObject.Parse(jsonString);
            //check if jackpot_notif exists in the config file
            if (config["jackpot_notif"] != null)
            {
                toggle_jackpot_notif.Checked = (bool)config["jackpot_notif"];
            }
            //else add jackpot_notif to the config file
            else
            {
                config["jackpot_notif"] = true;
                jsonString = JsonConvert.SerializeObject(config, Formatting.Indented);
                File.WriteAllText(CONFIG_FILE, jsonString);
                toggle_jackpot_notif.Checked = true;
            }

            //check if jackpot_notif_sounds exists in the config file
            if (config["jackpot_notif_sounds"] != null)
            {
                toggle_jackpot_sounds.Checked = (bool)config["jackpot_notif_sounds"];
            }





            for (int i = 0; i < file_list.Length; i++)
            {
                if (File.Exists(Path.Combine(directory, file_list[i])))
                {
                    comboBox_miningSpeed.SelectedIndex = i + 1;
                    break;
                }
            }
            if (comboBox_miningSpeed.SelectedIndex == -1)
                comboBox_miningSpeed.SelectedIndex = 0;

            

            for (int i = 0; i < mode_list.Length; i++)
            {
                if (File.Exists(Path.Combine(directory, mode_list[i])))
                {
                    comboBox_mode.SelectedIndex = i; 
                    break; 
                }
            }
            if (comboBox_mode.SelectedIndex == -1)
                comboBox_mode.SelectedIndex = 0;

            if (File.Exists(directory + "/settings_OpenGiftCenter.txt"))
            {
                toggle_openGift.Checked = true;
            }

            periodic_timer.Start();

        }


        private void SaveConfig()
        {
            var config = new { Directory = directory };
            string jsonString = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(CONFIG_FILE, jsonString);
        }

        private void LoadConfig()
        {
            string jsonString = File.ReadAllText(CONFIG_FILE);
            JObject config = JObject.Parse(jsonString);
            directory = config["Directory"].ToString();
            check_directory();
        }

        private void check_directory()
        {
            if (!File.Exists(directory + "/1-START-duckyFishing.mcr"))
            {
                using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                {
                    folderBrowserDialog.Description = "Select the MACRODUCK folder in your Main PC";
                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        directory = folderBrowserDialog.SelectedPath;
                        if (!File.Exists(directory + "/1-START-duckyFishing.mcr"))
                        {
                            MessageBox.Show("Invalid MACRODUCK folder. Please select the correct folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            check_directory();
                        }
                        SaveConfig();
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
            }
        }

        private void periodic_timer_Tick(object sender, EventArgs e)
        {
            label_currentTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            //if toggle_autoStart is on and current time is greater than auto_startTime, create VIP_ON.txt on the directory
            if (toggle_autoStart.Checked && DateTime.Now >= auto_startTime && label_autoStartTime.Text != "-----------" && !miningStarted)
            {
                button_play_Toggle();
            }

            //set label_timeDiffStart to the time difference between auto_startTime and current time, if auto_startTime is greater than current time, add "-" to the string
            if (toggle_autoStart.Checked && label_autoStartTime.Text != "-----------") { 
                TimeSpan time_diff2 = auto_startTime - DateTime.Now;
                if (time_diff2 > TimeSpan.Zero)
                {
                    label_timeDiffStart.FillColor = System.Drawing.Color.FromArgb(255, 128, 128);
                    if (time_diff2.Minutes > 0)
                    {
                        label_timeDiffStart.Text = time_diff2.Minutes + " min, " + time_diff2.Seconds + " sec";
                    }
                    else
                    {
                        label_timeDiffStart.Text = time_diff2.Seconds + " sec";
                    }
                    if(counter > 0)
                    {
                        counter = 0;
                    }
                }
                else
                {
                    label_timeDiffStart.FillColor = System.Drawing.Color.Orange;
                    counter++;
                    //convert counter to minutes and seconds and display it on the label_timeDiffStart
                    minutes = counter / 60;
                    seconds = counter % 60;
                    if (minutes == 0)
                    {
                        label_timeDiffStart.Text = seconds + " sec";
                    }
                    else
                    {
                        label_timeDiffStart.Text = minutes + " min, " + seconds + " sec";
                    }

                    /*if(counter < 18)
                    {
                        comboBox_miningSpeed.SelectedIndex = 3;
                    }
                    else if (counter < 35)
                    {
                        comboBox_miningSpeed.SelectedIndex = 5;
                    }*/

                }
            }
            else
                label_timeDiffStart.Text = "";
        }

        private void stopActions()
        {
            File.Delete(directory + "/VIP_ON.txt");
            if (button_play.Text == "STOP") { 
                button_play.Text = "Play";
                button_play.FillColor = System.Drawing.Color.FromArgb(222, 172, 128);
                button_play.FillColor2 = System.Drawing.Color.FromArgb(255, 128, 128);
                miningStarted = false;

                
            }
            if (toggle_jackpot_notif.Checked)
            {
                if (showToasts)
                {
                    string ign = File.ReadLines(directory + "/jackpot.config").First();
                    string dcm = File.ReadLines(directory + "/jackpot.config").Skip(1).First();
                    new ToastNotification(ign, dcm).Show();
                    showToasts = false;
                    System.Threading.Tasks.Task.Run(() => soundPlayer("winning_jackpot.wav"));

                }
                else
                {
                    System.Threading.Tasks.Task.Run(() => soundPlayer("updated_jackpot_time.wav"));
                }
            }
        }

        private void soundPlayer(string soundFile)
        {
            if (toggle_jackpot_sounds.Checked)
            {
                using (System.Media.SoundPlayer player = new System.Media.SoundPlayer(directory + "/sounds/" + soundFile))
                {
                    player.Play();
                }
            }
        }

        private void timeDiffUpdate()
        {
            if(label_lastJackpotTime.Text == "-----------")
            {
                return;
            }
            TimeSpan time_diff1 = latest_jackpotTime - last_jackpotTime;
            string gaptext;
            if (time_diff1.Hours > 0)
            {
                gaptext = time_diff1.Hours + " hours, " + time_diff1.Minutes + " minutes, " + time_diff1.Seconds + " seconds gap";
            }
            else if (time_diff1.Minutes > 0)
            {
                gaptext = time_diff1.Minutes + " minutes, " + time_diff1.Seconds + " seconds gap";
            }
            else
            {
                gaptext = time_diff1.Seconds + " seconds gap";
            }
            label_timeDiff.Text = gaptext;
            gap_list = gap_list == null ? new string[] { gaptext } : new string[] { gaptext }.Concat(gap_list).ToArray();
            string htmlText = "<html>" + string.Join("<br>", gap_list) + "</html>";
            tooltip_Gaps.SetToolTip(label_timeDiff, htmlText);

            auto_startTime = latest_jackpotTime.Add(time_diff1).AddSeconds(-deducted_seconds);
            label_autoStartTime.Text = auto_startTime.ToString("hh:mm:ss tt");

        }


        private void InitializeFileWatcher()
        {
            multiFileWatcher = new FileSystemWatcher(directory);
            multiFileWatcher.Filter = "*.txt";
            multiFileWatcher.Changed += OnFileChanged;
            multiFileWatcher.EnableRaisingEvents = true;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                string fileName = Path.GetFileName(e.FullPath).ToLower();
                switch (fileName)
                {
                    case "lastjackpot.txt":
                        last_jackpotTime = latest_jackpotTime;
                        latest_jackpotTime = File.GetLastWriteTime(directory + "/lastjackpot.txt");
                        label_latestJackpotTime.Text = latest_jackpotTime.ToString("hh:mm:ss tt");
                        label_lastJackpotTime.Text = last_jackpotTime.ToString("hh:mm:ss tt");
                        timeDiffUpdate();
                        stopActions();
                        break;
                    case "total_dcm.txt":
                        try
                        {
                            label_dcm.Text = SafeReadFile(Path.Combine(directory, "total_dcm.txt"));
                            
                            if (resetBtnClicked == false)
                            {
                                showToasts = true;
                            }
                            else
                            {
                                resetBtnClicked = false;
                            }


                        }
                        catch (IOException ex)
                        {
                            // Log the error and/or show a user-friendly message
                            Console.WriteLine($"Error reading total_dcm.txt: {ex.Message}");
                            label_dcm.Text = "Error reading file";
                        }
                        break;
                    case "jackpot_counter.txt":
                        try
                        {
                            label_jackpot.Text = SafeReadFile(Path.Combine(directory, "jackpot_counter.txt"));
                        }
                        catch (IOException ex)
                        {
                            // Log the error and/or show a user-friendly message
                            Console.WriteLine($"Error reading jackpot_counter.txt: {ex.Message}");
                            label_jackpot.Text = "Error reading file";
                        }
                        break;
                }
            });
        }

        private string SafeReadFile(string filePath, int maxRetries = 5, int retryDelay = 100)
        {
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    return File.ReadAllText(filePath);
                }
                catch (IOException ex) when (i < maxRetries - 1 &&
                    (ex is FileNotFoundException || ex is DirectoryNotFoundException || ex is PathTooLongException))
                {
                    // If the file doesn't exist or the path is invalid, don't retry
                    throw;
                }
                catch (IOException) when (i < maxRetries - 1)
                {
                    // Wait before trying again
                    System.Threading.Thread.Sleep(retryDelay);
                }
            }
            throw new IOException($"Unable to read file after {maxRetries} attempts: {filePath}");
        }

        private void button_play_Toggle()
        {
            if (button_play.Text == "Play")
            {
                File.WriteAllText(directory + "/VIP_ON.txt", "1");
                button_play.Text = "STOP";
                button_play.FillColor = System.Drawing.Color.Gray;
                button_play.FillColor2 = System.Drawing.Color.Gray;
                System.Threading.Tasks.Task.Run(() => soundPlayer("mining_start.wav"));
                miningStarted = true;
            }
            else
            {
                stopActions();
                toggle_autoStart.Checked = false;
            }
        }

        private void button_play_Click(object sender, EventArgs e)
        {
            
            button_play_Toggle();
        }

        private void combobox_miningSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (string file in file_list)
            {
                File.Delete(directory + "/" + file);
            }

            if (comboBox_miningSpeed.SelectedIndex != 0)
            {
                File.WriteAllText(directory + "/" + file_list[comboBox_miningSpeed.SelectedIndex - 1], "1");
            }

            /*if(comboBox_miningSpeed.SelectedIndex == 2)
            {
                //deducted_seconds = 22;
                //deducted_seconds = 30;
            }
            else
            {
                //deducted_seconds = 30;
            }*/
        }

        private void comboBox_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //delete all files in the directory that is in the mode_list
            foreach (string file in mode_list)
            {
                File.Delete(directory + "/" + file);
            }
            File.WriteAllText(directory + "/" + mode_list[comboBox_mode.SelectedIndex], "1");
            comboBox_mode.FillColor = modeColors[comboBox_mode.SelectedIndex];
            pictureBox1.Image = mode_icon[comboBox_mode.SelectedIndex];

        }


        private void label_latestJackpotTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.SelectNextControl(this.label_dcm, true, true, true, true);

            }
        }

        private void label_latestJackpotTime_MouseClick(object sender, MouseEventArgs e)
        {
            label_latestJackpotTime.FillColor = System.Drawing.Color.FromArgb(255, 255, 255);
            dummyText = label_latestJackpotTime.Text;
        }

        private void label_latestJackpotTime_Leave(object sender, EventArgs e)
        {
            label_latestJackpotTime.FillColor = System.Drawing.Color.FromArgb(255, 224, 192);
            if (label_latestJackpotTime.Text == dummyText)
            {
                return;
            }
            try
            {
                latest_jackpotTime = DateTime.Parse(label_latestJackpotTime.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid time. Example '01:57:53 PM'");
                label_latestJackpotTime.Text = latest_jackpotTime.ToString("hh:mm:ss tt");
                return;
            }
            latest_jackpotTime = DateTime.Parse(label_latestJackpotTime.Text);
            timeDiffUpdate();
        }

        private void label_lastJackpotTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.SelectNextControl(this.label_dcm, true, true, true, true);
            }
        }

        private void label_lastJackpotTime_Leave(object sender, EventArgs e)
        {
            label_lastJackpotTime.FillColor = System.Drawing.Color.FromArgb(255, 224, 192);
            if (label_lastJackpotTime.Text == dummyText)
            {
                return;
            }
            try
            {
                last_jackpotTime = DateTime.Parse(label_lastJackpotTime.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid time. Example '01:57:53 PM'");
                label_lastJackpotTime.Text = last_jackpotTime.ToString("hh:mm:ss tt");
                return;
            }
            last_jackpotTime = DateTime.Parse(label_lastJackpotTime.Text);
            timeDiffUpdate();
        }

        private void label_lastJackpotTime_MouseClick(object sender, MouseEventArgs e)
        {
            label_lastJackpotTime.FillColor = System.Drawing.Color.FromArgb(255, 255, 255);
            dummyText = label_lastJackpotTime.Text;
        }

        private void label_autoStartTime_MouseClick(object sender, MouseEventArgs e)
        {

            label_autoStartTime.FillColor = System.Drawing.Color.FromArgb(255, 255, 255);
            dummyText = label_autoStartTime.Text;
        }

        private void label_autoStartTime_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Enter)
            {
                this.SelectNextControl(this.label_dcm, true, true, true, true);
            }
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            resetBtnClicked = true;
            File.WriteAllText(directory + "/total_dcm.txt", "0");
            File.WriteAllText(directory + "/jackpot_counter.txt", "0");
        }

        private void IGN_List_Click(object sender, EventArgs e)
        {
            if (ign_form == null)
            {
                ign_form = new IGN_form();
                ign_form.FormClosed += (s, args) => ign_form = null;
                ign_form.Show();

                if (this.Location.X + this.Width + ign_form.Width > Screen.PrimaryScreen.Bounds.Width)
                {
                    ign_form.Location = new Point(this.Location.X - ign_form.Width, this.Location.Y);
                }
                else
                {
                    ign_form.Location = new Point(this.Location.X + this.Width, this.Location.Y);
                }
            }
            else
            {
                ign_form.BringToFront();
            }

        }


        private void IGN_List_MouseHover(object sender, EventArgs e)
        {
            IGN_List.Cursor = Cursors.Hand;
        }

        private void comboBox_miningSpeed_MouseEnter(object sender, EventArgs e)
        {
            text_dokdokspeed.Visible = true;
        }

        private void comboBox_miningSpeed_MouseLeave(object sender, EventArgs e)
        {
            text_dokdokspeed.Visible = false;
        }

        private void toggle_openGift_CheckedChanged(object sender, EventArgs e)
        {
            toggle_openGiftLabel.Text = toggle_openGift.Checked ? "Open Center Only" : "Open Random Gift";

            if (toggle_openGift.Checked)
            {
                File.WriteAllText(directory + "/settings_OpenGiftCenter.txt", "1");
                for (int i = 1; i <= 9; i++)
                {
                    if(i != 5)
                    {
                        this.Controls.Find("img_Gift" + i, true)[0].Visible = false;
                    }
                }
            }
            else
            {
                File.Delete(directory + "/settings_OpenGiftCenter.txt");
                for (int i = 1; i <= 9; i++)
                {
                    this.Controls.Find("img_Gift" + i, true)[0].Visible = true;
                }
            }
        }

        private void toggle_jackpot_notif_CheckedChanged(object sender, EventArgs e)
        {
            if (toggle_jackpot_notif.Checked)
            {
                toggle_jackpot_sounds.Visible = true;
                guna2HtmlLabel8.Visible = true;
                
            }
            else
            {
                toggle_jackpot_sounds.Checked = false;
                toggle_jackpot_sounds.Visible = false;
                guna2HtmlLabel8.Visible = false;
            }

            //save the toggle_jackpot_notif value to the config file
            configFileUpdate("jackpot_notif", toggle_jackpot_notif.Checked.ToString());
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {
            // new ToastNotification("Aqua", "got 3123 $DCM when mining").Show();
            string ign = File.ReadLines(directory + "/jackpot.config").First();
            string dcm = File.ReadLines(directory + "/jackpot.config").Skip(1).First();
            new ToastNotification(ign, dcm).Show();

        }

        private void toggle_jackpot_sounds_CheckedChanged(object sender, EventArgs e)
        {
            configFileUpdate("jackpot_notif_sounds", toggle_jackpot_sounds.Checked.ToString());
        }

        private void label_autoStartTime_Leave(object sender, EventArgs e)
        {

            label_autoStartTime.FillColor = System.Drawing.Color.RosyBrown;
            if (label_autoStartTime.Text == dummyText)
            {
                return;
            }
            try
            {
                auto_startTime = DateTime.Parse(label_autoStartTime.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid time. Example '01:57:53 PM'");
                label_autoStartTime.Text = auto_startTime.ToString("hh:mm:ss tt");
                return;
            }
            auto_startTime = DateTime.Parse(label_autoStartTime.Text);
        }

        private void toggle_autoStart_CheckedChanged(object sender, EventArgs e)
        {

            if (toggle_autoStart.Checked)
            {
                if(label_autoStartTime.Text != "-----------")
                {
                    label_timeDiffStart.Visible = true;
                    comboBox_mode.SelectedIndex = 0;
                }
            }
            else
            {
                label_timeDiffStart.Visible = false;
            }
        }

        private void configFileUpdate(string key, string value)
        {
            string jsonString = File.ReadAllText(CONFIG_FILE);
            JObject config = JObject.Parse(jsonString);
            config[key] = value;
            jsonString = JsonConvert.SerializeObject(config, Formatting.Indented);
            File.WriteAllText(CONFIG_FILE, jsonString);
        }
    }
}