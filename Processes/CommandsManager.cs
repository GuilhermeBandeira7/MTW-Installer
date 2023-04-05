using System;
using System.Diagnostics;
using System.Globalization;
using CliWrap;
using CliWrap.Buffered;

namespace InstallerMTW.Processes
{
    /// <summary>
    /// Manager of all commands required to be executed by the Program.
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

        //this variable indicates if the process is running or not.
        private bool isRunning;

        public string linuxBashPath = @"/bin/bash";
        public string windowsCmdPath = "C:\\Windows\\System32\\cmd.exe";

        //variable for linux commands
        #region LinuxCommands
        public string getSigningKey = @"wget https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
            sudo dpkg -i packages-microsoft-prod.deb
            rm packages-microsoft-prod.deb";
        public string installAspnetcore = @"sudo apt-get update && \ 
            sudo apt-get install -y aspnetcore-runtime-7.0";
        public string installRuntime = @"sudo apt-get install -y dotnet-runtime-7.0";
        #endregion

        public CommandsManager()
        {
            systemProcess = new Process();
            isRunning = false;
        }

        /// <summary>
        /// Opens an OS terminal and executes a specified command.
        /// </summary>
        /// <param name="command">Specified command to execute on the terminal.</param>
        public void OpenApplication(string operatingSystem, string command)
        {
            if(isRunning) { systemProcess = new Process(); }
            using(systemProcess)
            {
                systemProcess.StartInfo.FileName = operatingSystem;
                systemProcess.StartInfo.Arguments = command;
                systemProcess.StartInfo.UseShellExecute = true;
                systemProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                systemProcess.Start();
                ProcessIsRunning();
            }            
        }

        public async Task OpenTerminal()
        {
            var result = await Cli.Wrap("C:\\Users\\MTW\\Desktop\\IntallerMTW\\commands\\commands.bat")
                .WithWorkingDirectory("C:\\Users\\MTW\\Desktop\\IntallerMTW\\commands")
                //.WithArguments(new[] { "--version" })
                .ExecuteBufferedAsync();
            Console.WriteLine(result.StandardOutput);
        }

        public async Task OpenLinuxTerminal()
        {
            var result = await Cli.Wrap(@"/home/mwt/MTWInstaller/commands/terminal.sh")
                .WithWorkingDirectory(@"/home/mwt/MTWInstaller/commands/")
                .ExecuteBufferedAsync();
            Console.WriteLine(result.StandardOutput);
        }

        /// <summary>
        /// Installs the dotnet runtime
        /// </summary>
        /// <param name="command">desired command to be executed on the terminal.</param>
        public void InstallDotnetRuntime(string command)
        {
            try
            {              
                OpenApplication(linuxBashPath, command);
            }
            catch (ProcessException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// verifies if systemProcess is still running.
        /// </summary>
        /// <returns>True if it's running and False if it's been terminated.</returns>
        public bool ProcessIsRunning()
        {
            isRunning = !systemProcess.HasExited ? true : false;
            if(isRunning) { return true; } else{ return false; }
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
