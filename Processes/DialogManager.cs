using System;
using System.Xml.Serialization;

namespace InstallerMTW.Processes
{
    /// <summary>
    /// Manage the dialog with the user, get the user input and creates a CommandsManager instance to execute the commands.
    /// </summary>
    public class DialogManager
    {
        public bool running { get; private set; }
        private CommandsManager cmdManager { get; set; }
        private string? userInput;

        public DialogManager()
        {
            cmdManager = new CommandsManager();
            running = true;
        }

        /// <summary>
        /// Initialize the dialog with the user and get the user's input as paramater to CommandsManager methods.
        /// </summary>
        public void StartTerminalDialog()
        {
            Console.WriteLine("Welcome to MTW Installer!\n");

            while (running)
            {
                try
                {                   
                    Console.Write("Type in the command:");
                    userInput = Console.ReadLine();
                    if(userInput == "exit"){
                        break;
                    }
                    cmdManager.ExecuteBashCommand(userInput);
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
