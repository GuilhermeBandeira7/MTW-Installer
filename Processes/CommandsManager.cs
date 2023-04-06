using System;
using System.Diagnostics;


namespace InstallerMTW.Processes
{
    /// <summary>
    /// Initialize linux bash and input and output data from the bash.
    /// </summary>
    public class CommandsManager
    {
        /// <summary>
        /// Initializes a process on the current Operating System.
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

        private bool isProcessRunning;

        public string linuxBashPath = @"/bin/bash";
        public string windowsCmdPath = "C:\\Windows\\System32\\cmd.exe";

        public CommandsManager()
        {
            systemProcess = new Process();
            isProcessRunning = false;
        }

        /// <summary>
        /// Opens an OS terminal and executes a specified command.
        /// </summary>
        /// <param name="command">Specified command to execute on the terminal.</param>
        public void OpenCmd(string command)
        {
            if (isProcessRunning) { systemProcess = new Process(); }
            systemProcess.StartInfo.FileName = windowsCmdPath;
            systemProcess.StartInfo.Arguments = command;
            systemProcess.StartInfo.UseShellExecute = true;
            systemProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            systemProcess.Start();
            ProcessIsRunning();
        }

        public void ExecuteBashCommand(string cmd)
        {
            if (isProcessRunning) { systemProcess = new Process(); }
            systemProcess.StartInfo.FileName = linuxBashPath;
            systemProcess.StartInfo.UseShellExecute = false;
            systemProcess.StartInfo.Verb = "runas";
            systemProcess.StartInfo.Arguments = $"-c \"" + cmd + "\"";
            systemProcess.Start();

            systemProcess.WaitForExit();
        }

        /// <summary>
        /// verifies if systemProcess is still running.
        /// </summary>
        /// <returns>True if it's running and False if it's been terminated.</returns>
        public bool ProcessIsRunning()
        {
            isProcessRunning = !systemProcess.HasExited ? true : false;
            if (isProcessRunning) { return true; } else { return false; }
        }

        /// <summary>
        /// Kills a running process
        /// </summary>
        /// <exception cref="ProcessException">If an attempt to kill an inexistente process was made the exception is thrown.</exception>
        public void KillProcessIfRunning()
        {
            if (systemProcess != null)
            {
                systemProcess.Kill();
            }
            else
            {
                throw new ProcessException("Can't kill a process that is null or is not running.");
            }
        }

    }
}
