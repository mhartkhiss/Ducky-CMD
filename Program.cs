using System;
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

            try
            {
                // get command line arguments
                string[] args = Environment.GetCommandLineArgs();

                // if command line argument is "crafting" then open CraftingTimer
                if (args.Length > 1 && args[1] == "crafting")
                {
                    Application.Run(new CraftingTimer());
                    return;
                }

                if (args.Length > 1 && args[1] == "iSave")
                {
                    Island island = new Island();
                    island.Save();
                    return;
                }

                if (args.Length > 1 && args[1] == "iDelete")
                {
                    Island island = new Island();
                    island.Delete();
                    return;
                }

                if (args.Length > 1 && args[1] == "iLoad")
                {
                    Island island = new Island();
                    island.Load();
                    return;
                }

                if (args.Length > 1 && args[1] == "iFull")
                {
                    Island island = new Island();
                    island.Full();
                    return;
                }

                //if command line argument contains "iSave_" then open IslandSave
                if (args.Length > 1 && args[1].Contains("iSaveFile_"))
                {
                    //extract the text after the underscore and pass it to SaveIslandFile
                    Island island = new Island();
                    island.SaveFile(args[1].Split('_')[1]);
                    return;
                }

                if (args.Length > 1 && args[1] == "farmDelete")
                {
                    Farm farm = new Farm();
                    farm.DeleteID();
                    return;
                }

                if (args.Length > 1 && args[1] == "farm")
                {
                    Farm farm = new Farm();
                    farm.Main();
                    return;
                }

                if (System.IO.Directory.Exists(@"Z:\MACRODUCK"))
                {
                    Application.Run(new DuckyVIP());
                }
                else
                {
                    Application.Run(new DuckyCMD());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
