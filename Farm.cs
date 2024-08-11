using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Ducky_CMD
{
    internal class Farm
    {
        public void GenerateID()
        {
            Random random = new Random();
            int id;
            string path;

            // Loop until a unique ID is generated
            do
            {
                id = random.Next(1, 1000);
                path = @"Z:\MACRODUCK\farmQueue\" + id + ".txt";
            } while (File.Exists(path));

            // Create the file and write the ID to the Documents folder
            File.Create(path).Close();
            File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "farmID.txt"), id.ToString());
        }


        public void DeleteID()
        {
            string path1 = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\farmID.txt";
            if (File.Exists(path1))
            {
                string farmId = File.ReadAllText(path1);
                string path2 = @"Z:\MACRODUCK\farmQueue\" + farmId + ".txt";
                File.Delete(path1);
                if (File.Exists(path2))
                {
                    File.Delete(path2);
                }
            }
        }

        private void ProcessFarmQueue()
        {
            try
            {
                string file_documentsFarmId = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\farmID.txt";

                // If farmID.txt doesn't exist, generate a new ID and retry
                while (!File.Exists(file_documentsFarmId))
                {
                    GenerateID();
                }

                string farmId = File.ReadAllText(file_documentsFarmId);
                bool found = false;

                

                while (!found)
                {
                    // Get all files in farmQueue folder, sort by date modified from oldest to newest, and get the first file
                    string[] files = Directory.GetFiles(@"Z:\MACRODUCK\farmQueue\", "*.txt", SearchOption.AllDirectories);
                    Array.Sort(files, (x, y) => File.GetLastWriteTime(x).CompareTo(File.GetLastWriteTime(y)));

                    if (files.Length > 0)
                    {
                        string first_file = Path.GetFileNameWithoutExtension(files[0]);
                        // Check if first_file name is equal to farmId
                        if (first_file == farmId)
                        {
                            Clipboard.SetText("continue");
                            found = true;
                        }
                        else
                        {
                            Thread.Sleep(1000); // Sleep for 1 second before retrying
                        }
                    }
                    else {
                        Clipboard.SetText("continue");
                        found = true;
                    }
                }
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void Main()
        {

            ProcessFarmQueue();
        }
            
    }
}
