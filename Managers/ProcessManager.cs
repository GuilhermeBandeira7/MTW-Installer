//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;


namespace InstallerMTW.Managers {
    /// <summary>
    /// Initialize linux bash and input and output data from the bash.
    /// </summary>
    public class ProcessManager {

        private Process systemProcess { get; set; }

        public Process SystemProcess {
            get {
                if (systemProcess == null) { throw new ProcessException("The systemProcess has not been initialized."); }
                else { return systemProcess; }
            }
        }

        private bool isProcessRunning;

        private List<string> SelectedRange;

        public ProcessManager() {
            systemProcess = new Process();
            isProcessRunning = true;
            SelectedRange = new List<string>();
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
            Console.Clear();
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
                    RecordOptions();
                    break;
                default:
                    throw new ProcessException("Option not found");
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
        public void SetFilesAsEx(string filePath) {
            if (!isProcessRunning) { systemProcess = new Process(); isProcessRunning = true; }
            using (systemProcess) {
                systemProcess.StartInfo.FileName = "chmod";
                systemProcess.StartInfo.Verb = "runas";
                systemProcess.StartInfo.Arguments = $"+x {filePath}";
                systemProcess.StartInfo.UseShellExecute = false;
                systemProcess.StartInfo.CreateNoWindow = true;
                systemProcess.Start();
                systemProcess.WaitForExit();

                if (systemProcess.ExitCode != 0) {
                    throw new ProcessException("Failed to set file as executable.");
                }

                isProcessRunning = false;
            }
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
            ServiceManager.ChangeDirectory(filePath);
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

        public void RecordOptions() {
            string selectedOption = DialogManager.RecordOption();
            if (selectedOption != String.Empty && selectedOption != null) {
                switch (selectedOption) {
                    case "1":
                        RtspManager.GetPrimaryRtsp();
                        break;
                    case "2":
                        CreateService();break;
                    case "3":
                        RemoveRstp(); break;
                    case "4":
                        AlterRtsp(); break;
                    case "5":
                        break;
                }
            }
        }

        public void CreateService() {
            if (DialogManager.CreateRangeOfCameras()) {
                ServiceManager.CreateRangeOfRecordService(SelectedRange);
                SetSelectedRangeAsEx();
            }
            else {
                Console.WriteLine("Type the camera IP: ");
                string cameraIP = Console.ReadLine().ToString();
                if (cameraIP != null) {
                    string recordString = $"transport tcp - allowed_media_types video - i \"rtsp://admin:admin@{cameraIP}:8554\" -vcodec" +
                               "copy -map 0 -f segment -segment_time 20 -strftime 1 /home/records/camera/%Y%m%d%H%M%S.mkv";
                    ServiceManager.CreateShAndService(cameraIP, recordString);
                    SetFilesAsEx($"/etc/systemd/system/record_{cameraIP}.service");
                }
                else {
                    throw new ProcessException("The camera IP cannot be null.");
                }
            }

        }

        public void SetSelectedRangeAsEx() {
            if (SelectedRange.Count > 0) {
                foreach (string range in SelectedRange) {
                    SetFilesAsEx($"/etc/systemd/system/{range}");
                }
            }
            else {
                throw new ProcessException("The collection of selected cameras is empty.");
            }
        }

        public void AlterRtsp() {
            Console.WriteLine("type the camera IP to alter: ");
            string cameraIPtoChange = Console.ReadLine().ToString();
            Console.WriteLine("type the new camera IP: ");
            string newCameraIP = Console.ReadLine().ToString();
            ServiceManager.ChangeDirectory("/home/records");
            IEnumerable<string> file = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"*{cameraIPtoChange}.sh", SearchOption.AllDirectories);
            if (file.Any()) {
                OperationRtspProcess(file.First(), operation.Alter, newCameraIP);
            }
            else {
                throw new ProcessException("Any sh file was found.");
            }
            ServiceManager.ChangeDirectory("/etc/systemd/system");
            file.First().Remove(0);

            file = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"*{cameraIPtoChange}.service", SearchOption.AllDirectories);
            if (file.Any()) {
                OperationRtspProcess(file.First(), operation.Alter, newCameraIP);
            }
            else {
                throw new ProcessException("Any service file with the specified IP was found.");
            }
        }

        /// <summary>
        /// Removes a range of cameras or a unique selected camera.
        /// </summary>
        /// <exception cref="ProcessException"></exception>
        public void RemoveRstp() {
            IEnumerable<string> files = new HashSet<string>();
            if (DialogManager.RemoveRangeOfCameras()) {
                ServiceManager.ChangeDirectory("/etc/systemd/system");
                foreach (string service in SelectedRange) {
                    files = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"*{service}.service", SearchOption.AllDirectories);
                    RemoveOnRecordsDir();
                }
                foreach (string servive in files) {
                    OperationRtspProcess(servive, operation.Delete, string.Empty);
                }
            }
            else {
                Console.WriteLine("type the camera IP to delete: ");
                string cameraIP = Console.ReadLine().ToString();
                ServiceManager.ChangeDirectory("/etc/systemd/system");
                files = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"*{cameraIP}.service", SearchOption.AllDirectories);
                if (files.Any()) {
                    OperationRtspProcess(files.First(), operation.Delete, string.Empty);
                }
                else {
                    throw new ProcessException("Any servie file with specified name  found.");
                }
            }
        }

        /// <summary>
        /// Remove sh files in records directory
        /// </summary>
        private void RemoveOnRecordsDir() {
            IEnumerable<string> files = new HashSet<string>();
            ServiceManager.ChangeDirectory("/home/records");
            foreach (string service in SelectedRange) {
                files = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"*{service}.sh", SearchOption.AllDirectories);
            }
            foreach (string file in files) {
                OperationRtspProcess(file, operation.Delete, string.Empty);
            }
        }

        private void OperationRtspProcess(string fileName, operation op, string newFileName) {
            using (systemProcess) {
                systemProcess.StartInfo.FileName = "/bin/bash";
                systemProcess.StartInfo.Verb = "runas";
                if (op == operation.Delete) {
                    systemProcess.StartInfo.Arguments = $"-c sudo rm {fileName}";
                }
                else {
                    systemProcess.StartInfo.Arguments = $"-c sudo mv {fileName} {newFileName}";
                }
                systemProcess.StartInfo.CreateNoWindow = true;
                systemProcess.StartInfo.UseShellExecute = false;

                systemProcess.Start();
                systemProcess.WaitForExit();
                isProcessRunning = false;
            }
        }

        public enum operation {
            Delete = 0,
            Alter = 1
        }
    }
}
