using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private Color[] modeColors = { Color.FromArgb(255, 253, 76), Color.FromArgb(145, 210, 255), Color.FromArgb(164, 255, 164),
                   Color.FromArgb(255, 204, 0), Color.FromArgb(229, 128, 255)};
        private Color windowColor = Color.FromArgb(0, 2, 255);
        private Thread keywordCheckThread;
        private bool isCheckingKeywords;

        public DuckyVIP()
        {
            InitializeComponent();
            InitializeFileCheck();
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
                        LaunchMcrFileMinimized($@"{rootPath}\{modeMCR[i]}");
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

        private void LaunchMcrFileMinimized(string mcrFilePath)
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
                if (currentFileExists)
                {
                    SendKeys.SendWait("^p");
                }
                else
                {
                    SendKeys.SendWait("^q");
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

        private void CheckForKeywords()
        {
            string keywordsFilePath = $@"{rootPath}\names.txt";
            string counterFilePath = $@"{rootPath}\jackpot_counter.txt";
            string totalDcmFilePath = $@"{rootPath}\total_dcm.txt";

            int left = 26, top = 185, width = 461, height = 68;
            int right = left + width, bottom = top + height;
            Rectangle region = new Rectangle(left, top, right, left);

            var keywords = LoadKeywords(keywordsFilePath);

            while (isCheckingKeywords)
            {
                try
                {
                    using (Bitmap screenshot = new Bitmap(width, height))
                    {
                        using (Graphics g = Graphics.FromImage(screenshot))
                        {
                            g.CopyFromScreen(new Point(left, top), Point.Empty, new Size(width, height));
                        }
                        screenshot.Save(@"Z:\MACRODUCK\screenshot.png");

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
                        MessageBox.Show(text);

                        var foundKeyword = keywords.FirstOrDefault(keyword => text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);

                        if (!string.IsNullOrEmpty(foundKeyword))
                        {
                            Console.WriteLine($"Keyword '{foundKeyword}' found!");
                            MessageBox.Show($"Keyword '{foundKeyword}' found!");
                            if (foundKeyword != "winning" && foundKeyword != "for")
                            {
                                IncrementCounter(counterFilePath);
                                int number = ExtractNumberFromText(text);
                                if (number > 0)
                                {
                                    IncrementTotalDcm(totalDcmFilePath, number);
                                    Console.WriteLine($"Incremented total_dcm by {number}");
                                }
                            }

                            OpenFile(@"Z:\MACRODUCK\lastjackpot.ahk");
                            Thread.Sleep(1000);
                            RemoveFileSafely(@"Z:\MACRODUCK\VIP_ON.txt");
                            Thread.Sleep(65000);  // Wait for 65 seconds
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in CheckForKeywords: {ex.Message}");
                }

                Thread.Sleep(2000);  // Wait for 2 seconds before checking again
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

        private void OpenFile(string filePath)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C start /min \"\" \"{filePath}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                Process.Start(startInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to open file {filePath}: {e}");
            }
        }

        private void RemoveFileSafely(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

    }
}
