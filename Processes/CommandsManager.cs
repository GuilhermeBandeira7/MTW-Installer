using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace InstallerMTW.Processes {
    /// <summary>
    /// Initialize linux bash and input and output data from the bash.
    /// </summary>
    public class CommandsManager {
        /// <summary>
        /// Initializes a process on the current Operating System.
        /// </summary>
        private Process systemProcess { get; set; }

        public Process SystemProcess {
            get {
                if (systemProcess == null) { throw new ProcessException("The systemProcess has not been initialied."); }
                else { return systemProcess; }
            }
        }

        private bool isProcessRunning;

        public CommandsManager() {
            systemProcess = new Process();
            isProcessRunning = true;
        }

        public void ExecuteBashCommand(string cmd) {
            if (!isProcessRunning) { systemProcess = new Process(); }
            using (systemProcess) {
                systemProcess.StartInfo.FileName = "/bin/bash";
                systemProcess.StartInfo.UseShellExecute = false;
                systemProcess.StartInfo.Verb = "runas";
                systemProcess.StartInfo.Arguments = $"-c " + cmd;
                systemProcess.Start();
            }
        }


        public void ExecuteInstallationScript(string installCmd) {
            string path = Directory.GetCurrentDirectory();
            string target = path + "\\Scripts";

            switch (installCmd) {
                case "1":
                    ExecuteBashCommand(target + "\\mqtt-install.sh"); break;
                case "2":
                    ExecuteBashCommand(target + "\\nginx-install.sh"); break;
                case "3":
                    BashRedirectIO(target + "\\sqlserver-install.sh"); break;
            }
        }


        public void BashRedirectIO(string cmd) {
            using (systemProcess) {
                systemProcess.StartInfo.FileName = "C:\\WINDOWS\\system32\\cmd.exe";
                systemProcess.StartInfo.Arguments = "/k " + cmd;
                //systemProcess.StartInfo.FileName = "/bin/bash";
                //systemProcess.StartInfo.Arguments = "-c \"" + cmd + "\"";
                systemProcess.StartInfo.CreateNoWindow = true;
                systemProcess.StartInfo.UseShellExecute = false;
                systemProcess.StartInfo.RedirectStandardOutput = true;
                systemProcess.StartInfo.RedirectStandardInput = true;
                systemProcess.StartInfo.RedirectStandardError = true;

                systemProcess.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data)) {
                        Console.WriteLine(e.Data);
                    }
                });

                systemProcess.Start();
                systemProcess.BeginOutputReadLine();
                systemProcess.WaitForExit();
            }
        }

        /// <summary>
        /// verifies if systemProcess is still running.
        /// </summary>
        /// <returns>True if it's running and False if it's been terminated.</returns>
        public bool ProcessIsRunning() {
            isProcessRunning = !systemProcess.HasExited ? true : false;
            if (isProcessRunning) { return true; } else { return false; }
        }

        /// <summary>
        /// Kills a running process
        /// </summary>
        /// <exception cref="ProcessException">If an attempt to kill an inexistente process was made the exception is thrown.</exception>
        public void KillProcessIfRunning() {
            if (systemProcess != null) {
                isProcessRunning = false;
                systemProcess.Kill();
            }
            else {
                throw new ProcessException("Can't kill a process that is null or is not running.");
            }
        }

    }
}
