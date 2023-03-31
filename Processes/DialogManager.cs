using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InstallerMTW.Processes
{
    /// <summary>
    /// Manage the dialog with the user, get the user input and creates a CommandsManager instance to execute the commands.
    /// </summary>
    public class DialogManager
    {
        public bool running { get; private set; }
        public CommandsManager terminal { get; private set; }
        string terminalPath = "C:\\Windows\\System32\\cmd.exe";

        public DialogManager()
        {
            terminal = new CommandsManager();
            running = true;
        }

        /// <summary>
        /// Initialize the dialog with the user and uses the user's input as paramater to CommandsManager classes.
        /// </summary>
        public void StartTerminalDialog()
        {
            Console.WriteLine("Welcome to MTW Installer!\n");

            while (running)
            {
                try
                {
                    Console.Write("do you wish to execute 'dir' command? [y/n]");
                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "y":
                            //"/k dir" '/k' executes a command and leaves the app open and '/c' executes command e close the app
                            terminal.OpenApplication(terminalPath, "/k dir"); break;
                        case "n":
                            running = false; break;
                        default:
                            throw new ProcessException("wrong input, type 'y' to execute the command or 'n' to exit.");
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
