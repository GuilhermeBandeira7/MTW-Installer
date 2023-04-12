using System;
using System.Xml.Serialization;

namespace InstallerMTW.Processes
{
    /// <summary>
    /// Manage the dialog with the user, get the user input and creates a CommandsManager instance to execute the commands.
    /// </summary>
    public class DialogManager
    {
        public bool dialogIsRunning { get; private set; }
        public CommandsManager cmdManager { get; set; }

        public DialogManager()
        {
            cmdManager = new CommandsManager();
            dialogIsRunning = true;
        }

        /// <summary>
        /// Initialize the dialog with the user and get the user's input as paramater to CommandsManager methods.
        /// </summary>
        public void StartTerminalDialog()
        {
            Console.WriteLine("Welcome to MTW Installer!\n");

            while (dialogIsRunning)
            {
                try
                {
                    Console.WriteLine("Select the desired action: \n[1] " +
                        ".NET 7 SDK \n[2] Nginx \n[3] SQL Server 2017");
                    string input = Console.ReadLine().ToString();
                    if(input == "1")
                    {
                        cmdManager.ExecuteInstallationScript("");
                    }
                }
                catch (ProcessException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
