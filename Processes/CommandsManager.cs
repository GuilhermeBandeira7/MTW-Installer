using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Text;

namespace InstallerMTW.Processes {
    /// <summary>
    /// Initialize linux bash and input and output data from the bash.
    /// </summary>
    public class CommandsManager {

        private Process systemProcess { get; set; }

        public Process SystemProcess {
            get {
                if (systemProcess == null) { throw new ProcessException("The systemProcess has not been initialized."); }
                else { return systemProcess; }
            }
        }

        private bool isProcessRunning;

        public CommandsManager() {
            systemProcess = new Process();
            isProcessRunning = true;
        }

        /// <summary>
        /// Runs a script to install the sql server express 2017.
        /// </summary>
        /// <param name="cmd"></param>
        public void InstallSqlServer(string cmd) {
            if (!isProcessRunning) { systemProcess = new Process(); isProcessRunning = true; }
            using (systemProcess) {
                systemProcess.StartInfo.FileName = "/bin/bash";
                systemProcess.StartInfo.UseShellExecute = false;
                systemProcess.StartInfo.Verb = "runas";
                systemProcess.StartInfo.Arguments = $"-c " + cmd;
                systemProcess.StartInfo.CreateNoWindow = true;
                systemProcess.Start();
                systemProcess.WaitForExit();
                isProcessRunning = false;
            }
        }

        public void TestFunction(int a= 0, int b=0) {

        }

        /// <summary>
        /// Automatically run a set of commands that require 'yes/no' input.
        /// </summary>
        /// <param name="cmd"></param>
        public void ExecuteBashCommand(string cmd) {
            if (!isProcessRunning) { systemProcess = new Process(); isProcessRunning = true; }
            using (systemProcess) {
                systemProcess.StartInfo.FileName = "/bin/bash";
                systemProcess.StartInfo.UseShellExecute = false;
                systemProcess.StartInfo.Verb = "runas";
                systemProcess.StartInfo.Arguments = $"-c " + cmd;
                systemProcess.StartInfo.CreateNoWindow = true;
                systemProcess.StartInfo.RedirectStandardInput = true;
                systemProcess.StartInfo.RedirectStandardOutput = true;

                string output = String.Empty;
                systemProcess.OutputDataReceived += new DataReceivedEventHandler((sender, e) => {
                    if (!string.IsNullOrEmpty(e.Data)) {
                        Console.WriteLine(e.Data);
                    }
                });

                systemProcess.Start();
                systemProcess.BeginOutputReadLine();
                systemProcess.StandardInput.WriteLine("Y");
                systemProcess.WaitForExit();
                isProcessRunning = false;
            }
        }

        /// <summary>
        /// Restore a database carrying out scripts.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="scriptBackup"></param>
        public void RestoreDatabase(string path, string scriptBackup) {
            if (!isProcessRunning) { systemProcess = new Process(); isProcessRunning = true; }
            using (systemProcess) {
                Process process = new Process();
                process.StartInfo.FileName = "/opt/mssql-tools/bin/sqlcmd";
                systemProcess.StartInfo.Verb = "runas";
                if (scriptBackup == "5") {
                    process.StartInfo.Arguments = "-S localhost -U sa -P Senha@mtw -i " + path + "/masterserver.sql";
                }
                else if (scriptBackup == "6") {
                    process.StartInfo.Arguments = "-S localhost -U sa -P Senha@mtw -i " + path + "/tmhub.sql";
                }
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                systemProcess.StartInfo.RedirectStandardOutput = true;

                systemProcess.OutputDataReceived += new DataReceivedEventHandler((sender, e) => {
                    if (!string.IsNullOrEmpty(e.Data)) {
                        Console.WriteLine(e.Data);
                    }
                });

                process.Start();
                systemProcess.BeginOutputReadLine();
                process.WaitForExit();
                isProcessRunning = false;
            }
        }

        /// <summary>
        /// Redirects the user input to the appropiate method. 
        /// </summary>
        /// <param name="installCmd">Input of the selected operation.</param>
        public void RedirectCommand(string installCmd) {
            string scriptPath = Directory.GetCurrentDirectory() + "/Scripts";

            switch (installCmd) {
                case "1":
                    ExecuteBashCommand(scriptPath + "/mqtt-install.sh"); break;
                case "2":
                    ExecuteBashCommand(scriptPath + "/nginx-install.sh"); break;
                case "3":
                    InstallSqlServer(scriptPath + "/sqlserver-script.sh"); break;
                case "4":
                    ExecuteBashCommand(scriptPath + "/mssqltools-install.sh"); break;
                case "5":
                    RestoreDatabase(scriptPath, "5"); break;
                case "6":
                    RestoreDatabase(scriptPath, "6"); break;
                case "7":
                    ExecuteBashCommand(scriptPath + "/git-install.sh"); break;
                case "8":
                    ExecuteCmd(scriptPath + "/nodejsSixteen-install.sh"); break;
                case "9":
                    GitClone("git clone https://fersilva1995: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@github.com/fersilva1995/ApiClientMtwServer.git");
                    GitClone("git clone https://fersilva1995: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@github.com/fersilva1995/ApiMtwServer.git");
                    GitClone("git clone https://fersilva1995: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@github.com/fersilva1995/EntityMtwServer.git");
                    GitClone("git clone https://fersilva1995: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@github.com/fersilva1995/UtilsMtwServer.git");
                    GitClone("git clone https://fersilva1995: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@github.com/fersilva1995/MTWServerVue.git"); break;
                case "10":
                    StartRecording(); 
                    break;
                default:
                    System.Console.WriteLine("option not found."); break;
            }
        }

        /// <summary>
        /// Goes to the directory where .sh files are located.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private IEnumerable<string> GetScripDirectory(string filePath) {
            return Directory.EnumerateFiles(filePath, "*.sh", SearchOption.AllDirectories);
        }

        /// <summary>
        /// Automatically set a file as executable.
        /// </summary>
        /// <param name="filePath"></param>
        private void SetFilesAsEx(string filePath) {
            if (!isProcessRunning) { systemProcess = new Process(); isProcessRunning = true; }
            systemProcess.StartInfo.FileName = "/bin/bash";
            systemProcess.StartInfo.Verb = "runas";
            systemProcess.StartInfo.Arguments = $"chmod +x {filePath}";
        }

        /// <summary>
        /// Run a command on the bash
        /// </summary>
        /// <param name="cmd"></param>
        public void ExecuteCmd(string cmd) {
            if (!isProcessRunning) { systemProcess = new Process(); }
            using (systemProcess) {
                systemProcess.StartInfo.FileName = "/bin/bash";
                systemProcess.StartInfo.Verb = "runas";
                systemProcess.StartInfo.Arguments = "-c \"" + cmd + "\"";
                systemProcess.StartInfo.CreateNoWindow = true;
                systemProcess.StartInfo.UseShellExecute = false;
                systemProcess.StartInfo.RedirectStandardOutput = true;
                systemProcess.StartInfo.RedirectStandardInput = true;
                systemProcess.StartInfo.RedirectStandardError = true;

                systemProcess.OutputDataReceived += new DataReceivedEventHandler((sender, e) => {
                    if (!string.IsNullOrEmpty(e.Data)) {
                        Console.WriteLine(e.Data);
                    }
                });

                systemProcess.Start();
                systemProcess.BeginOutputReadLine();
                systemProcess.WaitForExit();
                isProcessRunning = false;
            }
        }

        public void GitClone(string cmd) {
            string filePath = Directory.GetCurrentDirectory() + "/Code";
            ChangeDirectory(filePath);
            if (!isProcessRunning) { systemProcess = new Process(); }
            using (systemProcess) {
                systemProcess.StartInfo.FileName = "/bin/bash";
                systemProcess.StartInfo.Verb = "runas";
                systemProcess.StartInfo.Arguments = "-c \"" + cmd + "\"";
                systemProcess.StartInfo.CreateNoWindow = true;
                systemProcess.StartInfo.UseShellExecute = false;
                systemProcess.StartInfo.RedirectStandardOutput = true;
                systemProcess.StartInfo.RedirectStandardInput = true;
                systemProcess.StartInfo.RedirectStandardError = true;

                systemProcess.OutputDataReceived += new DataReceivedEventHandler((sender, e) => {
                    if (!string.IsNullOrEmpty(e.Data)) {
                        Console.WriteLine(e.Data);
                    }
                });

                systemProcess.Start();
                systemProcess.BeginOutputReadLine();
                systemProcess.WaitForExit();
                isProcessRunning = false;
            }
        }

        /// <summary>
        /// Initializes the recording process of a camera.
        /// </summary>
        /// <exception cref="ProcessException"></exception>
        public void StartRecording() {
            Console.WriteLine("Type the camera IP: ");
            string cameraIP = String.Empty;
            cameraIP = Console.ReadLine().ToString();
            string recordString = $"transport tcp - allowed_media_types video - i \"rtsp://admin:admin@{cameraIP}:8554\" -vcodec" +
                            "copy -map 0 -f segment -segment_time 20 -strftime 1 /home/records/%Y%m%d%H%M%S.mkv";
            if (cameraIP != String.Empty && cameraIP != null) {
                ChangeAndCreateDirectory("/home", "records");
                string fileName = "record.sh";
                IEnumerable<string> filesList = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.sh", SearchOption.AllDirectories);
                if(!filesList.Contains(fileName)) {
                    using (FileStream newFile = new FileStream(fileName, FileMode.CreateNew)) {
                        using (StreamWriter writer = new StreamWriter(newFile)) {
                            writer.WriteLine(recordString);
                            writer.Flush(); //Despeja o buffer para o stream
                        };
                    };
                }
                else {
                    File.WriteAllText("/home/record/"+fileName, recordString);
                }
            }
            else {
                throw new ProcessException("Camera IP cannot be null.");
            }
        }

        /// <summary>
        /// Change the linux directory to home and creates a record directory to save records of a camera.
        /// </summary>
        /// <param name="dirToChange"></param>
        /// <param name="dirNameToCreate"></param>
        /// <exception cref="ProcessException"></exception>
        public void ChangeAndCreateDirectory(string dir, string dirNameToCreate) {
            if (Directory.Exists(dir)) {
                Directory.SetCurrentDirectory(dir);  //home
                Directory.CreateDirectory("/home/" + dirNameToCreate); //records
                Directory.SetCurrentDirectory(dirNameToCreate);
                Console.WriteLine("Current Directory: "+ Directory.GetCurrentDirectory());
            }
            else {
                throw new ProcessException("This directory does not exist.");
            }
        }

        public void ChangeDirectory(string filePath) {
            if (Directory.Exists(filePath)) {
                Directory.SetCurrentDirectory(filePath);
                Console.WriteLine("The current directory is: " + Directory.GetCurrentDirectory());
            }
        }
    }
}
