using System;
using System.Diagnostics;
using System.IO;


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

        public CommandsManager()
        {
            systemProcess = new Process();
            isProcessRunning = true;
        }

        public void ExecuteBashCommand(string cmd)
        {
            if (!isProcessRunning) { systemProcess = new Process(); }
            systemProcess.StartInfo.FileName = "/bin/bash";
            systemProcess.StartInfo.UseShellExecute = false;
            systemProcess.StartInfo.Verb = "runas";
            systemProcess.StartInfo.Arguments = $"-c " + cmd;
            systemProcess.Start();

            systemProcess.WaitForExit();
        }


        public void ExecuteInstallationScript(string installCmd)
        {
            string path = Directory.GetCurrentDirectory();
            string target = path + "\\Scripts";

            switch (installCmd)
            {
                case "1": 
                    ExecuteBashCommand(target + "\\dotnet-install.sh"); break;

                case "2": 
                    ExecuteBashCommand(target + "\\nginx-install.sh"); break;
                
                case "3": 
                    ExecuteBashCommand(target + "\\sqlserver-install.sh");break;      
            }
        }


        public async Task BashRedirectIO(string cmd)
        {
            systemProcess.StartInfo.FileName = $"/bin/bash";
            systemProcess.StartInfo.UseShellExecute = false;
            systemProcess.StartInfo.Arguments = $"-c " + cmd;
            //systemProcess.StartInfo.CreateNoWindow = true;
            systemProcess.StartInfo.RedirectStandardOutput = true;
            //systemProcess.StartInfo.RedirectStandardInput = true;
            //systemProcess.StartInfo.RedirectStandardError = true;
            systemProcess.Start();
            isProcessRunning = true;
            //systemProcess.ErrorDataReceived += SystemProcess_ErrorDataReceived;          

            StreamReader reader = systemProcess.StandardOutput;
            await RedirectBashDataToConsole(reader);

            systemProcess.WaitForExit();
        }

        private async Task RedirectBashDataToConsole(StreamReader reader)
        {
            string output = string.Empty;
            while (!reader.EndOfStream)
            {
                output = await reader.ReadLineAsync();
                Console.WriteLine(output);
            }
        }

        private void SystemProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
           string output = e.Data;
            Console.WriteLine(output);
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
                isProcessRunning = false;
                systemProcess.Kill();
            }
            else
            {
                throw new ProcessException("Can't kill a process that is null or is not running.");
            }
        }

    }
}
