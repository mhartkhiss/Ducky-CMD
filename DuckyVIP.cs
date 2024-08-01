using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Tesseract;

namespace Ducky_CMD
{
    public partial class DuckyVIP : Form
    {
        private static string rootPath = @"Z:\MACRODUCK";
        private string filePath = $@"{rootPath}\VIP_ON.txt";
        private bool previousFileExists = false;
        private System.Windows.Forms.Timer checkTimer;
        private string[] modeFiles = { "mode_Mining.txt", "mode_Fishing.txt", "mode_Farming.txt",
            "mode_Crafting.txt", "mode_OpenGift.txt"};
        private string[] modeMCR = {"4-mining.mcr", "1-START-duckyFishing.mcr", "6-START-farming only.mcr",
        "7-START-Crafting.mcr", "9-START-OpenGift.mcr"};
        private string[] modeNames = { "Mining", "Fishing", "Farming", "Crafting", "Open Gift" };
        private Color[] modeColors = { Color.FromArgb(255, 236, 161), Color.FromArgb(145, 210, 255), Color.FromArgb(164, 255, 164),
                   Color.FromArgb(255, 185, 79), Color.FromArgb(233, 154, 255)};
        private Color windowColor = Color.FromArgb(0, 2, 255);
        private Thread keywordCheckThread, miningThread;
        private bool isCheckingKeywords;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        private const int SW_RESTORE = 9;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;


        public DuckyVIP()
        {
            InitializeComponent();
            InitializeFileCheck();
            Clipboard.Clear();
        }
        private void InitializeFileCheck()
        {
            // Initial check
            CheckFileAndSendKeys();

            // Set up timer
            checkTimer = new System.Windows.Forms.Timer();
            checkTimer.Interval = 1000; // Check every second
            checkTimer.Tick += CheckTimer_Tick;
            checkTimer.Start();
        }

        private void CheckTimer_Tick(object sender, EventArgs e)
        {
            CheckFileAndSendKeys();
            CheckModeFiles();
        }

        private void CheckModeFiles()
        {
            for (int i = 0; i < modeFiles.Length; i++)
            {
                string modeFilePath = $@"{rootPath}\{modeFiles[i]}";
                bool modeFileExists = File.Exists(modeFilePath);

                if (modeFileExists)
                {
                    if (windowColor != modeColors[i])
                    {
                        kill_process("MacroRecorder");
                        OpenFileMinimized($@"{rootPath}\{modeMCR[i]}");
                        guna2HtmlLabel1.Text = "Mode: "+modeNames[i];
                        BackColor = modeColors[i];
                        windowColor = modeColors[i];
                    }
                    break;
                }
            }
        }

        private void kill_process(string processName)
        {
            foreach (var process in Process.GetProcessesByName(processName))
            {
                process.Kill();
            }
        }

        private void OpenFileMinimized(string mcrFilePath)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = mcrFilePath,
                    WindowStyle = ProcessWindowStyle.Minimized
                };

                Process.Start(startInfo);
                Console.WriteLine($"Started {mcrFilePath} in minimized window");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting process: {ex.Message}");
            }
        }

        private void CheckFileAndSendKeys()
        {
            bool currentFileExists = File.Exists(filePath);

            if (currentFileExists != previousFileExists)
            {
                IntPtr chromeHandle = FindWindow(null, "DuckyVIP");
                if (chromeHandle != IntPtr.Zero)
                {
                    SetForegroundWindow(chromeHandle);
                    // Wait a bit for the window switch to take effect
                    System.Threading.Thread.Sleep(100);

                    if (currentFileExists)
                    {
                        SendKeys.SendWait("^p");
                    }
                    else
                    {
                        SendKeys.SendWait("^q");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to find the DuckyCity - Google Chrome window.");
                }

                previousFileExists = currentFileExists;
            }
        }

        
        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            guna2PictureBox2.Visible = guna2ToggleSwitch1.Checked;
            guna2PictureBox1.Visible = !guna2ToggleSwitch1.Checked;

            if (guna2ToggleSwitch1.Checked)
            {
                SwitchAndResizeWindow("DuckyCity - Google Chrome", 400, 700);
                isCheckingKeywords = true;
                keywordCheckThread = new Thread(CheckForKeywords);
                keywordCheckThread.Start();
            }
            else
            {
                isCheckingKeywords = false;
                keywordCheckThread?.Join();
            }
        }

        private void SwitchAndResizeWindow(string windowTitle, int width, int height)
        {
            IntPtr windowHandle = FindWindow(null, windowTitle);
            if (windowHandle != IntPtr.Zero)
            {
                // Check if the window is minimized
                if (IsIconic(windowHandle))
                {
                    // Restore the window if it's minimized
                    ShowWindow(windowHandle, SW_RESTORE);
                    Thread.Sleep(100); // Give it some time to restore
                }

                // Bring the window to the foreground
                SetForegroundWindow(windowHandle);

                // Resize the window
                SetWindowPos(windowHandle, IntPtr.Zero, 0, 0, width, height, SWP_NOMOVE | SWP_NOZORDER);

                Thread.Sleep(100); // Wait for window switch and resize to take effect
            }
            else
            {
                MessageBox.Show($"The window '{windowTitle}' was not found. Please open the window and try again.",
                                "Window Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CheckForKeywords()
        {
            /*if (File.Exists(@"Z:\MACRODUCK\lastjackpot.txt"))
            {
                FileInfo fi = new FileInfo(@"Z:\MACRODUCK\lastjackpot.txt");
                if (fi.LastWriteTime > DateTime.Now.AddMinutes(-1))
                {
                    return;
                }
            }*/

            string keywordsFilePath = @"Z:\MACRODUCK\names.txt";
            string counterFilePath = @"Z:\MACRODUCK\jackpot_counter.txt";
            string totalDcmFilePath = @"Z:\MACRODUCK\total_dcm.txt";

            //int left = 26, top = 185, width = 461, height = 68;
            int left = 66, top = 198, width = 395, height = 87;
            Rectangle region = new Rectangle(left, top, width, height);

            var keywords = LoadKeywords(keywordsFilePath);
            //add "winning" and "for" to the list of keywords
            keywords = keywords.Concat(new string[] { "for"}).ToArray();

            while (isCheckingKeywords)
            {
                if (File.Exists(@"Z:\MACRODUCK\lastjackpot.txt"))
                {
                    FileInfo fi = new FileInfo(@"Z:\MACRODUCK\lastjackpot.txt");
                    if (fi.LastWriteTime < DateTime.Now.AddMinutes(-1))
                    {
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
                                        }
                                    }
                                }

                                var foundKeyword = keywords.FirstOrDefault(keyword => text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);

                                if (!string.IsNullOrEmpty(foundKeyword) && !text.Contains("Onchain"))
                                {

                                    if (File.Exists(@"Z:\MACRODUCK\lastjackpot.txt"))
                                    {
                                        FileInfo fi2 = new FileInfo(@"Z:\MACRODUCK\lastjackpot.txt");
                                        if (fi2.LastWriteTime < DateTime.Now.AddMinutes(-1))
                                        {
                                            Console.WriteLine($"Keyword '{foundKeyword}' found!");
                                            if (foundKeyword != "for")
                                            {
                                                using (StreamWriter sw = File.CreateText(@"Z:\MACRODUCK\jackpot.config"))
                                                {
                                                    sw.WriteLine(foundKeyword);
                                                    sw.WriteLine($"got <b>{ExtractNumberFromText(text)} $DCM</b> when mining.");
                                                }
                                                IncrementCounter(counterFilePath);
                                                Thread.Sleep(100);
                                                int number = ExtractNumberFromText(text);
                                                if (number > 0)
                                                {
                                                    IncrementTotalDcm(totalDcmFilePath, number);
                                                    Console.WriteLine($"Incremented total_dcm by {number}");
                                                    Thread.Sleep(100);
                                                }
                                            }

                                            File.SetLastWriteTime(@"Z:\MACRODUCK\lastjackpot.txt", DateTime.Now);
                                            
                                        }
                                    }
                                    RemoveFileSafely(@"Z:\MACRODUCK\VIP_ON.txt");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error in CheckForKeywords: {ex.Message}");
                        }
                    }
                }
                

                Thread.Sleep(500);  // Wait for 2 seconds before checking again
            }
        }


        private string[] LoadKeywords(string filePath)
        {
            return File.ReadAllLines(filePath).Select(line => line.Trim()).ToArray();
        }

        private void IncrementCounter(string filePath)
        {
            try
            {
                int counter = 0;
                if (File.Exists(filePath))
                {
                    counter = int.Parse(File.ReadAllText(filePath).Trim());
                }
                counter++;
                File.WriteAllText(filePath, counter.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to increment counter: {e}");
            }
        }

        private void IncrementTotalDcm(string filePath, int incrementValue)
        {
            try
            {
                int totalDcm = 0;
                if (File.Exists(filePath))
                {
                    totalDcm = int.Parse(File.ReadAllText(filePath).Trim());
                }
                totalDcm += incrementValue;
                File.WriteAllText(filePath, totalDcm.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to increment total DCM: {e}");
            }
        }

        private int ExtractNumberFromText(string text)
        {
            var match = Regex.Match(text, @"of (\d+)");
            return match.Success ? int.Parse(match.Groups[1].Value) : 0;
        }


        private void RemoveFileSafely(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private void DuckyVIP_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop keyword checking thread
            isCheckingKeywords = false;
            keywordCheckThread?.Join();

            // Stop the timer
            checkTimer?.Stop();
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

            string keywordsFilePath = @"Z:\MACRODUCK\names.txt";

            int left = 66, top = 198, width = 395, height = 87;
            Rectangle region = new Rectangle(left, top, width, height);

            var keywords = LoadKeywords(keywordsFilePath);
            //add "winning" and "for" to the list of keywords
            keywords = keywords.Concat(new string[] { "winning", "for" }).ToArray();


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
                                MessageBox.Show(text);
                            }
                        }
                    }

                    var foundKeyword = keywords.FirstOrDefault(keyword => text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);

                    if (!string.IsNullOrEmpty(foundKeyword))
                    {

                        if (foundKeyword != "winning" && foundKeyword != "for")
                        {
                            MessageBox.Show($"Keyword '{foundKeyword}' found!");
                        }


                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CheckForKeywords: {ex.Message}");
            }
        }

        private void toggle_IsolateMSpeed_CheckedChanged(object sender, EventArgs e)
        {
            comboBox_miningSpeed.Visible = toggle_IsolateMSpeed.Checked;
            if (toggle_IsolateMSpeed.Checked)
            {
                comboBox_miningSpeed.SelectedIndex = 2;
            }
            else
            {
                Clipboard.Clear();
            }
        }

        private void comboBox_miningSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            //set clipboard to the selected value
            Clipboard.SetText(comboBox_miningSpeed.SelectedItem.ToString());
        }

        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            SwitchAndResizeWindow("DuckyCity - Google Chrome", 400, 700);
            kill_process("AutoHotkeyU64");
            kill_process("timerHandler");
            kill_process("timerHandler2");
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            SwitchAndResizeWindow("DuckyCity - Google Chrome", 400, 700);
            kill_process("AutoHotkeyU64");
            kill_process("timerHandler");
            kill_process("timerHandler2");
        }

    }
}
