using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace Ducky_CMD
{
    public partial class DuckyCMD : Form
    {
        public static string directory = null;
        private const string CONFIG_FILE = "config.json";
        private DateTime last_jackpotTime = DateTime.Now;
        private DateTime latest_jackpotTime = DateTime.Now;
        private DateTime auto_startTime = DateTime.Now;
        private FileSystemWatcher multiFileWatcher, islandWatcher, farmWatcher;
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
        private bool showToasts = false, miningStarted = false, logSingleLine = false;
        private string total_dokdok_time = null;

        public DuckyCMD()
        {
            InitializeComponent();
            InitializeConfig();
            initialize();
            InitializeFileWatcher();
            refreshLogs();
            DashboardUpdate();
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
            //get the last jackpot time from the config file
            string jsonString1 = File.ReadAllText(CONFIG_FILE);
            JObject config2 = JObject.Parse(jsonString1);
            label_lastJackpotTime.Text = "-----------";
            if (config2["last_jackpot_time"] != null)
            {
                label_lastJackpotTime.Text = config2["last_jackpot_time"].ToString();
            }

            label_dcm.Text = File.ReadAllText(directory + "/total_dcm.txt");
            label_jackpot.Text = File.ReadAllText(directory + "/jackpot_counter.txt");
            dash_jackpot.Text = label_jackpot.Text;
            dash_totaldcm.Text = label_dcm.Text + " $DCM";
            label_currentTime.Text = DateTime.Now.ToString("hh:mm:ss tt");
            label_latestJackpotTime.Text = latest_jackpotTime.ToString("hh:mm:ss tt");
            label_autoStartTime.Text = "-----------";
            label_timeDiffStart.Text = "";
            label_timeDiffStart.Visible = false;
            label_timeDiff.Text = "";
            dash_gap.Text = "";
            dash_bookmark.Text = "";
            dash_iIce.Text = "";
            dash_iPurple.Text = "";
            dash_iGreen.Text = "";
            dash_iWater.Text = "";
            dash_iYellow.Text = "";
            dash_iRed.Text = "";

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

            if(File.Exists(directory + "/settings/bookmark_island.txt"))
            {
                toggle_bookmark.Checked = true;
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
        private void stopActions()
        {
            try
            {
                try { 

                    File.Delete(directory + "/VIP_ON.txt");
                }
                catch (IOException ex)
                {
                    writeLog("Error in stopActions() deleting file config : " + ex.Message);
                }

                if (button_play.Text == "STOP")
                {
                    button_play.Text = "Play";
                    button_play.FillColor = System.Drawing.Color.FromArgb(222, 172, 128);
                    button_play.FillColor2 = System.Drawing.Color.FromArgb(255, 128, 128);
                    miningStarted = false;


                }

                if (toggle_jackpot_notif.Checked)
                {
                    if (showToasts)
                    {
                        try
                        {
                            string ign = File.ReadLines(directory + "/jackpot.config").First();
                            string dcm = File.ReadLines(directory + "/jackpot.config").Skip(1).First();
                            new ToastNotification(ign, dcm).Show();
                            showToasts = false;
                            writeLog(ign + " " + dcm);
                            System.Threading.Tasks.Task.Run(() => soundPlayer("winning_jackpot.wav"));
                        }
                        catch (IOException ex)
                        {
                            writeLog("Error in stopActions() : " + ex.Message);
                        }


                    }
                    else
                    {
                        if (toggle_gapupdate_sound.Checked)
                            System.Threading.Tasks.Task.Run(() => soundPlayer("updated_jackpot_time.wav"));
                    }
                }

            }
            catch (Exception ex) { writeLog("Error in stopActions() : " + ex.Message); }

        }


        private void button_play_Toggle()
        {
            if (button_play.Text == "Play")
            {
                File.WriteAllText(directory + "/VIP_ON.txt", "1");
                button_play.Text = "STOP";
                button_play.FillColor = System.Drawing.Color.Gray;
                button_play.FillColor2 = System.Drawing.Color.Gray;
                if (toggle_start_sound.Checked)
                    System.Threading.Tasks.Task.Run(() => soundPlayer("mining_start.wav"));
                miningStarted = true;
            }
            else
            {
                stopActions();
                toggle_autoStart.Checked = false;
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
                        label_timeDiffStart.Text = "- " + time_diff2.Minutes + " min, " + time_diff2.Seconds + " sec";
                    }
                    else
                    {
                        label_timeDiffStart.Text = "- " + time_diff2.Seconds + " sec";
                    }
                    if(counter > 0)
                    {
                        counter = 0;
                    }
                    total_dokdok_time = label_timeDiffStart.Text + ", wala ka dokdok";
                }
                else
                {
                    label_timeDiffStart.FillColor = Color.Orange;
                    counter++;
                    //convert counter to minutes and seconds and display it on the label_timeDiffStart
                    minutes = counter / 60;
                    seconds = counter % 60;
                    if (minutes == 0)
                    {
                        label_timeDiffStart.Text = "+ " + seconds + " sec";
                    }
                    else
                    {
                        label_timeDiffStart.Text = "+ " + minutes + " min, " + seconds + " sec";
                    }
                    total_dokdok_time = label_timeDiffStart.Text;

                }
            }
            else
                label_timeDiffStart.Text = "";

            

            dash_miningstart.Text = label_timeDiffStart.Text;
            dash_miningstart.ForeColor = label_timeDiffStart.FillColor;

        }

        private void DashboardUpdate() {

            //count the number of txt files in the directory/farmQueue folder and set dash_farmQueue to the number of files
            string[] files = Directory.GetFiles(directory + "/farmQueue", "*.txt");
            dash_queue.Text = files.Length.ToString();

            //count the number of txt files including the subdirectory in the directory/islands and set dash_islands to the number of files
            // Get all .txt files, excluding "0.txt"
            var island_files = Directory.GetFiles(directory + "/islands", "*.txt", SearchOption.AllDirectories)
                                 .Where(file => Path.GetFileName(file) != "0.txt")
                                 .ToArray();

            // Update the dash_bookmark text with the count of files
            dash_bookmark.Text = island_files.Length.ToString();

            var purple_files = Directory.GetFiles(directory + "/islands/purple", "*.txt", SearchOption.AllDirectories)
                                 .Where(file => Path.GetFileName(file) != "0.txt")
                                 .ToArray();
            dash_iPurple.Text = "= " + purple_files.Length.ToString();

            var green_files = Directory.GetFiles(directory + "/islands/green", "*.txt", SearchOption.AllDirectories)
                                 .Where(file => Path.GetFileName(file) != "0.txt")
                                 .ToArray();
            dash_iGreen.Text = "= " + green_files.Length.ToString();

            var water_files = Directory.GetFiles(directory + "/islands/water", "*.txt", SearchOption.AllDirectories)
                                 .Where(file => Path.GetFileName(file) != "0.txt")
                                 .ToArray();
            dash_iWater.Text = "= " + water_files.Length.ToString();

            //do the same for ice
            var ice_files = Directory.GetFiles(directory + "/islands/ice", "*.txt", SearchOption.AllDirectories)
                                 .Where(file => Path.GetFileName(file) != "0.txt")
                                 .ToArray();
            dash_iIce.Text = "= " + ice_files.Length.ToString();

            //do the same for yellow
            var yellow_files = Directory.GetFiles(directory + "/islands/yellow", "*.txt", SearchOption.AllDirectories)
                                 .Where(file => Path.GetFileName(file) != "0.txt")
                                 .ToArray();
            dash_iYellow.Text = "= " + yellow_files.Length.ToString();

            //do the same for red
            var red_files = Directory.GetFiles(directory + "/islands/red", "*.txt", SearchOption.AllDirectories)
                                 .Where(file => Path.GetFileName(file) != "0.txt")
                                 .ToArray();
            dash_iRed.Text = "= " + red_files.Length.ToString();

        }

        
        private void writeLog(string message)
        {
            string log = message + "/" + DateTime.Now.ToString("MMMM dd, yyyy (hh:mm:ss tt)");
            //remove any html tags from the message
            log = Regex.Replace(log, "<.*?>", String.Empty);

            File.AppendAllText(directory + "/logs.txt", log + Environment.NewLine);
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
            try {

                if (label_lastJackpotTime.Text == "-----------")
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
                dash_gap.Text = gaptext;

                gaptext = gaptext + " <span style=\"color: #FF5733; font-weight: bold;\">(" + total_dokdok_time + ")</span>";
                //total_dokdok_time = "wala ka dokdok" + total_dokdok_time;

                // Limit the gap_list to 10 items
                gap_list = gap_list == null
                    ? new string[] { gaptext }
                    : new string[] { gaptext }.Concat(gap_list).Take(20).ToArray();

                string htmlText = "<html>" + string.Join("<br>", gap_list) + "</html>";
                tooltip_Gaps.SetToolTip(label_timeDiff, htmlText);


                auto_startTime = latest_jackpotTime.Add(time_diff1).AddSeconds(-deducted_seconds);
                label_autoStartTime.Text = auto_startTime.ToString("hh:mm:ss tt");


            }
            catch (Exception ex)
            {
                writeLog("Error in timeDiffUpdate(): " + ex.Message);

            }
            

        }


        private void InitializeFileWatcher()
        {
            multiFileWatcher = new FileSystemWatcher(directory);
            multiFileWatcher.Filter = "*.txt";
            multiFileWatcher.Changed += OnFileChanged;
            multiFileWatcher.EnableRaisingEvents = true;

            //create new filewatcher for the directory/islands folder including the subdirectories and set the filter to *.txt
            islandWatcher = new FileSystemWatcher(directory + "/islands");
            islandWatcher.Filter = "*.*";
            islandWatcher.IncludeSubdirectories = true;
            islandWatcher.Changed += OnFileChangedIsland;
            islandWatcher.Created += OnFileChangedIsland;
            islandWatcher.Deleted += OnFileChangedIsland;
            islandWatcher.EnableRaisingEvents = true;

            //create new filewatcher for the directory/farmQueue folder and set the filter to *.txt
            farmWatcher = new FileSystemWatcher(directory + "/farmQueue");
            farmWatcher.Filter = "*.txt";
            farmWatcher.Changed += OnFileChangedFarm;
            farmWatcher.Created += OnFileChangedFarm;
            farmWatcher.Deleted += OnFileChangedFarm;
            farmWatcher.EnableRaisingEvents = true;
        }

        private void OnFileChangedFarm(object sender, FileSystemEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                DashboardUpdate();
            });
        }

        private void OnFileChangedIsland(object sender, FileSystemEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                DashboardUpdate();
            });
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
                        configFileUpdate("last_jackpot_time", last_jackpotTime.ToString("hh:mm:ss tt"));
                        while (true)
                        {
                            try
                            {
                                latest_jackpotTime = File.GetLastWriteTime(directory + "/lastjackpot.txt");
                                break;
                            }
                            catch (IOException)
                            {
                                System.Threading.Thread.Sleep(100);
                            }
                        }
                        label_latestJackpotTime.Text = latest_jackpotTime.ToString("hh:mm:ss tt");
                        label_lastJackpotTime.Text = last_jackpotTime.ToString("hh:mm:ss tt");
                        timeDiffUpdate();
                        stopActions();
                        break;
                    case "total_dcm.txt":
                        try
                        {
                            label_dcm.Text = SafeReadFile(Path.Combine(directory, "total_dcm.txt"));
                            dash_totaldcm.Text = label_dcm.Text +" $DCM";
                            showToasts = true;
                        }
                        catch (IOException ex)
                        {
                            writeLog("Error reading total_dcm.txt: " + ex.Message);
                            label_dcm.Text = "Error reading file";
                        }
                        break;
                    case "jackpot_counter.txt":
                        try
                        {
                            label_jackpot.Text = SafeReadFile(Path.Combine(directory, "jackpot_counter.txt"));
                            dash_jackpot.Text = label_jackpot.Text;
                        }
                        catch (IOException ex)
                        {
                            writeLog("Error reading jackpot_counter.txt: " + ex.Message);
                            label_jackpot.Text = "Error reading file";
                        }
                        break;

                    case "logs.txt":
                        logSingleLine = true;
                        refreshLogs();
                        break;
                }
            });
        }

        private void refreshLogs() {
            try
            {
                string[] lines = File.ReadAllLines(directory + "/logs.txt");
                Array.Reverse(lines);
                if (!logSingleLine) { 
                    flowLayoutPanel1.Controls.Clear();
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split('/');
                        PopulateFlowLayoutPanel(parts[0], parts[1]);
                    }
                    //set dash_logsize to the file size of logs.txt, set to kb, mb
                    
                }
                else
                {
                    string log = lines[0];
                    string[] parts = log.Split('/');
                    PopulateFlowLayoutPanel(parts[0], parts[1]);
                    logSingleLine = false;
                }

                FileInfo logFile = new FileInfo(directory + "/logs.txt");
                double size = logFile.Length / 1024.0;
                if (size > 1024)
                {
                    dash_logsize.Text = "Log size: " + (size / 1024).ToString("0.00") + " MB";
                }
                else
                {
                    dash_logsize.Text = "Log size: " + size.ToString("0.00") + " KB";
                }
            }
            catch (IOException ex)
            {
                writeLog("Error reading logs.txt: " + ex.Message);
            }
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
            multiFileWatcher.EnableRaisingEvents = false;
            File.WriteAllText(directory + "/total_dcm.txt", "0");
            File.WriteAllText(directory + "/jackpot_counter.txt", "0");
            label_dcm.Text = "0";
            label_jackpot.Text = "0";
            dash_jackpot.Text = "0";
            dash_totaldcm.Text = "0 $DCM";
            multiFileWatcher.EnableRaisingEvents = true;
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

        private void toggle_bookmark_CheckedChanged(object sender, EventArgs e)
        {
            //delete or create the file in the directory/settings/bookmark_island.txt
            if (toggle_bookmark.Checked)
            {
                File.WriteAllText(directory + "/settings/bookmark_island.txt", "1");
            }
            else
            {
                File.Delete(directory + "/settings/bookmark_island.txt");
            }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void tabDashboard_Click(object sender, EventArgs e)
        {

        }

        private void toggle_jackpot_sounds_CheckedChanged(object sender, EventArgs e)
        {
            configFileUpdate("jackpot_notif_sounds", toggle_jackpot_sounds.Checked.ToString());
            if (toggle_jackpot_sounds.Checked)
            {
                toggle_gapupdate_sound.Visible = true;
                toggle_start_sound.Visible = true;
                guna2HtmlLabel10.Visible = true;
                guna2HtmlLabel11.Visible = true;
            }
            else
            {
                toggle_gapupdate_sound.Visible = false;
                toggle_start_sound.Visible = false;
                guna2HtmlLabel10.Visible = false;
                guna2HtmlLabel11.Visible = false;
            }
        }

        private void guna2PictureBox8_Click(object sender, EventArgs e)
        {
            //open the directory/farmQueue folder
            System.Diagnostics.Process.Start(directory + "/farmQueue");

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            //show confirmation
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to reset all full island? Only do this after 8:00 AM", "Island Reset", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                resetIslandFiles();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            //clear the logs.txt file, show a confirmation message
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear the logs?", "Clear Logs", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                File.WriteAllText(directory + "/logs.txt", "");
                refreshLogs();
            }

        }

        private void guna2HtmlLabel15_Click(object sender, EventArgs e)
        {
            //open the directory/islands folder
            System.Diagnostics.Process.Start(directory + "/islands");
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            //if button text is "Show All", change the button text to "Today" and show the logs for today in flowLayoutPanel1
            //else change the button text to "Show All" and show all the logs in flowLayoutPanel1
            if (guna2Button2.Text == "Show All")
            {
                guna2Button2.Text = "Today";
                string[] lines = File.ReadAllLines(directory + "/logs.txt");
                Array.Reverse(lines);
                flowLayoutPanel1.Controls.Clear();
                foreach (string line in lines)
                {
                    string[] parts = line.Split('/');
                    // Assuming parts[1] contains the date string
                    DateTime logDate = DateTime.ParseExact(parts[1].Trim(), "MMMM dd, yyyy (hh:mm:ss tt)", System.Globalization.CultureInfo.InvariantCulture);
                    if (logDate.Date == DateTime.Now.Date)
                    {
                        PopulateFlowLayoutPanel(parts[0], parts[1]);
                    }
                }
            }
            else
            {
                guna2Button2.Text = "Show All";
                refreshLogs();
            }
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

        private void PopulateFlowLayoutPanel(string message, string date)
        {

            var item = new LogMessage
            {
                Message = message,
                Date = date,
                MessageColor = message.Contains("catching") ? ColorTranslator.FromHtml("#99D9EA") :
                   message.Contains("Onchain") ? ColorTranslator.FromHtml("#FFC5E8") :
                   Color.MistyRose
            };
            flowLayoutPanel1.Controls.Add(item);
            if(logSingleLine)
            {
                flowLayoutPanel1.Controls.SetChildIndex(item, 0);
            }
        }

        private void resetIslandFiles() {

            //get all .full files in the directory/islands folder including the subdirectories and change the extension to .txt
            var full_files = Directory.GetFiles(directory + "/islands", "*.full", SearchOption.AllDirectories)
                                 .Where(file => Path.GetFileName(file) != "0.full")
                                 .ToArray();

            islandWatcher.EnableRaisingEvents = false;

            foreach (string file in full_files)
            {
                string newFile = Path.ChangeExtension(file, ".txt");
                File.Move(file, newFile);
            }

            DashboardUpdate();
            islandWatcher.EnableRaisingEvents = true;

        }

    }
}