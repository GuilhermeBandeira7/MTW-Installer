using System;
using System.Diagnostics;
using System.IO;
using System.Text;

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
        if (systemProcess == null) { throw new ProcessException("The systemProcess has not been initialied."); }
        else { return systemProcess; }
      }
    }

    private bool isProcessRunning;

    public CommandsManager()
    {
      systemProcess = new Process();
      isProcessRunning = true;
    }

    public void InstallSqlServer(string cmd)
    {
      if (!isProcessRunning) { systemProcess = new Process(); isProcessRunning = true; }
      using (systemProcess)
      {
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

    public void ExecuteBashCommand(string cmd)
    {
      if (!isProcessRunning) { systemProcess = new Process(); isProcessRunning = true; }
      using (systemProcess)
      {
        systemProcess.StartInfo.FileName = "/bin/bash";
        systemProcess.StartInfo.UseShellExecute = false;
        systemProcess.StartInfo.Verb = "runas";
        systemProcess.StartInfo.Arguments = $"-c " + cmd;
        systemProcess.StartInfo.CreateNoWindow = true;
        systemProcess.StartInfo.RedirectStandardInput = true;
        systemProcess.StartInfo.RedirectStandardOutput = true;

        string output = String.Empty;
        systemProcess.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
        {
          if (!string.IsNullOrEmpty(e.Data))
          {
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

    public void RestoreDatabase(string path, string scriptBackup)
    {
      if (!isProcessRunning) { systemProcess = new Process(); isProcessRunning = true; }
      using (systemProcess)
      {
        Process process = new Process();
        process.StartInfo.FileName = "/opt/mssql-tools/bin/sqlcmd";
        systemProcess.StartInfo.Verb = "runas";
        if (scriptBackup == "5")
        {
          process.StartInfo.Arguments = "-S localhost -U sa -P Senha@mtw -i " + path + "/masterserver.sql";
        }
        else if (scriptBackup == "6")
        {
          process.StartInfo.Arguments = "-S localhost -U sa -P Senha@mtw -i " + path + "/tmhub.sql";
        }
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        systemProcess.StartInfo.RedirectStandardOutput = true;

        systemProcess.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
        {
          if (!string.IsNullOrEmpty(e.Data))
          {
            Console.WriteLine(e.Data);
          }
        });

        process.Start();
        systemProcess.BeginOutputReadLine();
        process.WaitForExit();
        isProcessRunning = false;
      }
    }

    public void ExecuteInstallationScript(string installCmd)
    {
      //string scriptPath = "~/Projects/mtwinstaller/MTW-Installer/Scripts";
      string scriptPath = Directory.GetCurrentDirectory() + "/Scripts";
      // IEnumerable<string> scripts = GetScripDirectory(Directory.GetCurrentDirectory());
      // SetFilesAsEx(scripts.First());

      switch (installCmd)
      {
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
        default:
          System.Console.WriteLine("option not found."); break;
      }
    }

    private IEnumerable<string> GetScripDirectory(string filePath)
    {
      return Directory.EnumerateFiles(filePath, "*.sh", SearchOption.AllDirectories);
    }

    private void SetFilesAsEx(string filePath)
    {
      if (!isProcessRunning) { systemProcess = new Process(); isProcessRunning = true; }
      systemProcess.StartInfo.FileName = "/bin/bash";
      systemProcess.StartInfo.Verb = "runas";
      systemProcess.StartInfo.Arguments = $"chmod +x {filePath}";
    }

    public void ExecuteCmd(string cmd)
    {
      if (!isProcessRunning) { systemProcess = new Process(); }
      using (systemProcess)
      {
        systemProcess.StartInfo.FileName = "/bin/bash";
        systemProcess.StartInfo.Arguments = "-c \"" + cmd + "\"";
        systemProcess.StartInfo.CreateNoWindow = true;
        systemProcess.StartInfo.UseShellExecute = false;
        systemProcess.StartInfo.RedirectStandardOutput = true;
        systemProcess.StartInfo.RedirectStandardInput = true;
        systemProcess.StartInfo.RedirectStandardError = true;

        systemProcess.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
        {
          if (!string.IsNullOrEmpty(e.Data))
          {
            Console.WriteLine(e.Data);
          }
        });

        systemProcess.Start();
        systemProcess.BeginOutputReadLine();
        systemProcess.WaitForExit();
        isProcessRunning = false;
      }
    }
  }
}
