
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
        private string? userInput;

        public DialogManager()
        {
            terminal = new CommandsManager();
            running = true;
        }

        /// <summary>
        /// Initialize the dialog with the user and uses the user's input as paramater to CommandsManager methods.
        /// </summary>
        public void StartTerminalDialog()
        {
            Console.WriteLine("Welcome to MTW Installer!\n");

            while (running)
            {
                try
                {
                    Console.Write("do you wish to execute 'dir' command on cmd windows? [y/n]");
                    userInput = Console.ReadLine();
                    OpenCmd(userInput);
                    Console.WriteLine("do you wish to execute 'ls' command on linux bash? [y/n] ");
                    userInput = Console.ReadLine(); 
                    OpenBash(userInput);
                    Console.WriteLine("Install .NET Runtime 7.0 on Linux Ubuntu 18.04? [y/n]");
                    userInput= Console.ReadLine();
                    InstallDotNet(userInput);
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

        /// <summary>
        /// Opens cmd on windows and does the command passed as parameter of the OpenApplication method.
        /// </summary>
        /// <param name="input">Input from the user.</param>
        /// <exception cref="ProcessException"></exception>
        private void OpenCmd(string input)
        {
            switch (input)
            {
                case "y":
                    //"/k dir" '/k' executes a command and leaves the app open and '/c' executes command e close the app
                    terminal.OpenApplication(terminal.windowsCmdPath, "/k dir");
                    terminal.OpenApplication(terminal.windowsCmdPath, "/k echo 'hello world'"); break;
                case "n":
                    running = false; break;
                default:
                    throw new ProcessException("wrong input, type 'y' to execute the command or 'n' to exit.");
            }
        }

        /// <summary>
        /// Opens bash on Linux and does the command passed as parameter of the OpenApplication method.
        /// </summary>
        /// <param name="input">Input from the user.</param>
        /// <exception cref="ProcessException"></exception>
        private void OpenBash(string input)
        {
            switch (input)
            {
                case "y":
                    //"-i ls" '-i' executes a command and leaves the terminal open.
                    terminal.OpenApplication(terminal.linuxBashPath, "-i ls"); break;
                case "n":
                    running = false; break;
                default:
                    throw new ProcessException("wrong input, type 'y' to execute the command or 'n' to exit.");
            }
        }

        private void InstallDotNet(string input)
        {
            switch (input)
            {
                case "y":
                    //"-i ls" '-i' executes a command and leaves the terminal open.
                    terminal.InstallDotnetRuntime(terminal.getSigningKey);
                    terminal.InstallDotnetRuntime(terminal.installAspnetcore);
                    terminal.InstallDotnetRuntime(terminal.installRuntime);
                    break;
                case "n":
                    running = false; break;
                default:
                    throw new ProcessException("wrong input, type 'y' to execute the command or 'n' to exit.");
            }           
        }
    }
}
