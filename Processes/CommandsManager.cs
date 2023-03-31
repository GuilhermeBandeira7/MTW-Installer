using System.Diagnostics;


namespace InstallerMTW.Processes
{
    /// <summary>
    /// Manager of all commands required to be executed by the Program.
    /// </summary>
    public class CommandsManager
    {
        /// <summary>
        /// initialize a process on linux os.
        /// </summary>
        private Process systemProcess { get; set; }

        public Process SystemProcess
        {
            get
            {
                if (systemProcess == null)
                    return systemProcess;
                else { throw new ProcessException("This process was already initialized."); }
            }
        }

        public CommandsManager()
        {
            systemProcess = new Process();
        }

        /// <summary>
        /// Opens an OS terminal and executes a specified command.
        /// </summary>
        /// <param name="filePath">Path to the executable file of the shell.</param>
        /// <param name="command">Specified command to execute on the terminal.</param>
        public void OpenApplication(string filePath, string command)
        {
            using(systemProcess)
            {
                systemProcess.StartInfo.FileName = filePath;
                systemProcess.StartInfo.Arguments = command;
                systemProcess.StartInfo.UseShellExecute = true;
                systemProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                systemProcess.Start();
            }            
        }

    }
}
