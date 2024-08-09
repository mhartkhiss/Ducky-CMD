using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tesseract;
using System.Windows.Forms;
using System.IO;

namespace Ducky_CMD
{
    internal class Island
    {


        public void Save()
        {
            int islandNumber = DetectIslandNumber();
            if (islandNumber > 0)
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\saved_island.txt";
                File.WriteAllText(path, islandNumber.ToString());
            }
        }

        public void Delete()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\saved_island.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void Load()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\saved_island.txt";
            if (File.Exists(path))
            {
                string islandNumber = File.ReadAllText(path);
                Clipboard.SetText(islandNumber);
            }
            else
            {
                Pick();
            }
        }

        public int GetTotalIslandsAvailable() 
        {

            string path = @"Z:\MACRODUCK\islands";
            int totalIslands = 0;
            try
            {
                //set totalIslands to the number of .txt files in the path include subfolders, exlude file that start with "0"
                totalIslands = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories).Where(f => !f.StartsWith("0")).Count();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return totalIslands;
        }

        public void Full()
        {
            int islandNumber = DetectIslandNumber();
            if (islandNumber > 0)
            {
                string parentFolder = @"Z:\MACRODUCK\islands";
                //get the full path of the island file
                string path = Directory.GetFiles(parentFolder, $"{islandNumber}.txt", SearchOption.AllDirectories).FirstOrDefault();
                if (path != null)
                {
                    //change extension to .full using File.Move
                    string newPath = path.Replace(".txt", ".full");
                    File.Move(path, newPath);

                }
            }
        }

        public void Pick() {

            if (GetTotalIslandsAvailable() == 0)
            {
                Clipboard.SetText("0");
                return;
            }

            string islandType = "random";
            string[] islands = { "red", "water", "purple", "ice", "yellow", "green" };

            foreach (string island in islands)
            {
                if (File.Exists($@"C:\px_{island}.txt"))
                {
                    islandType = island;
                    break;
                }
            }

            if(islandType == "random" && File.Exists(@"C:\rainbow.txt"))
            {
                foreach (string island in islands)
                {
                    if (!File.Exists($@"C:\mine_{island}.txt"))
                    {
                        islandType = island;
                        break;
                    }
                }
            }

            string parentFolder = @"Z:\MACRODUCK\islands";

            if (islandType == "random")
            {
                //get the subDirectory of parentFolder that has the most files
                string folder = Directory.GetDirectories(parentFolder).OrderByDescending(d => new System.IO.DirectoryInfo(d).GetFiles().Length).FirstOrDefault();
                if (folder != null)
                {
                    //get random .txt file from the folder
                    string path = Directory.GetFiles(folder, "*.txt").OrderBy(f => Guid.NewGuid()).FirstOrDefault();
                    if (path != null)
                    {
                        string islandNumber = File.ReadAllText(path);
                        Clipboard.SetText(islandNumber);
                    }

                }
            }
            else
            {
                string folder = Directory.GetDirectories(parentFolder).Where(d => d.Contains(islandType)).FirstOrDefault();
                if (folder != null)
                {
                    //get random .txt file from the folder
                    string path = Directory.GetFiles(folder, "*.txt").OrderBy(f => Guid.NewGuid()).FirstOrDefault();
                    if (path != null)
                    {
                        string islandNumber = File.ReadAllText(path);
                        Clipboard.SetText(islandNumber);
                    }

                }
            }


        }



        public void SaveFile(string folder) {

            string islandNumber = DetectIslandNumber().ToString();
            string path = $@"Z:\MACRODUCK\islands\{folder}\{islandNumber}.txt";
            string path2 = $@"Z:\MACRODUCK\islands\{folder}\{islandNumber}.full";
            if (!File.Exists(path) && !File.Exists(path2))
            {
                File.WriteAllText(path, islandNumber);
            }
        }

        private int DetectIslandNumber()
        {

            int left = 908, top = 222, width = 80, height = 29;
            int islandNumber = 0;

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
                                var match1 = Regex.Match(text, @"\#(\d+)");
                                //var match3 = match2 minus match1
                                if (match1.Success && match1.Success)
                                {
                                    int number = int.Parse(match1.Groups[1].Value);
                                    if (number > 0)
                                    {
                                        islandNumber = number;
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

            return islandNumber;
        }
    }


}
