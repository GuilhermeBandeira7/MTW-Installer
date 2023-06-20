//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using EntityMtwServer;
using EntityMtwServer.Entities;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Text;

namespace InstallerMTW.Processes
{
  /// <summary>
  /// Initialize linux bash and input and output data from the bash.
  /// </summary>
  public class CommandsManager
  {

    private Process systemProcess { get; set; }

    public Process SystemProcess
    {
      get
      {
        if (systemProcess == null) { throw new ProcessException("The systemProcess has not been initialized."); }
        else { return systemProcess; }
      }
    }

    private bool isProcessRunning;

    private List<Equipment> SelectedRange;

    public CommandsManager()
    {
      systemProcess = new Process();
      isProcessRunning = true;
      SelectedRange = new List<Equipment>();
    }

    /// <summary>
    /// Runs a script to install the sql server express 2017.
    /// </summary>
    /// <param name="cmd"></param>
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

    /// <summary>
    /// Automatically run a set of commands that require 'yes/no' input.
    /// </summary>
    /// <param name="cmd"></param>
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

    /// <summary>
    /// Restore a database carrying out scripts.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="scriptBackup"></param>
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

    /// <summary>
    /// Redirects the user input to the appropiate method. 
    /// </summary>
    /// <param name="installCmd">Input of the selected operation.</param>
    public void RedirectCommand(string installCmd)
    {
      string scriptPath = Directory.GetCurrentDirectory() + "/Scripts";
      Console.Clear();
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
        case "8":
          ExecuteCmd(scriptPath + "/nodejsSixteen-install.sh"); break;
        case "9":
          GitClone("git clone https://fersilva1995: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@github.com/fersilva1995/ApiClientMtwServer.git");
          GitClone("git clone https://fersilva1995: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@github.com/fersilva1995/ApiMtwServer.git");
          GitClone("git clone https://fersilva1995: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@github.com/fersilva1995/EntityMtwServer.git");
          GitClone("git clone https://fersilva1995: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@github.com/fersilva1995/Utils.git");
          GitClone("git clone https://fersilva1995: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@github.com/fersilva1995/MTWServer.git"); break;
        case "10":
          RecordOptions();
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
    private IEnumerable<string> GetScripDirectory(string filePath)
    {
      return Directory.EnumerateFiles(filePath, "*.sh", SearchOption.AllDirectories);
    }

    /// <summary>
    /// Automatically set a file as executable.
    /// </summary>
    /// <param name="filePath"></param>
    private void SetFilesAsEx(string filePath)
    {
      if (!isProcessRunning) { systemProcess = new Process(); isProcessRunning = true; }
      using (systemProcess)
      {
        systemProcess.StartInfo.FileName = "chmod";
        systemProcess.StartInfo.Verb = "runas";
        systemProcess.StartInfo.Arguments = $"+x {filePath}";
        systemProcess.StartInfo.UseShellExecute = false;
        systemProcess.StartInfo.CreateNoWindow = true;
        systemProcess.Start();
        systemProcess.WaitForExit();

        if (systemProcess.ExitCode != 0)
        {
          System.Console.WriteLine("Failed to set executable permission");
        }
        else
        {
          System.Console.WriteLine("Executable permission set on the file");
        }

        isProcessRunning = false;
      }
    }

    /// <summary>
    /// Run a command on the bash
    /// </summary>
    /// <param name="cmd"></param>
    public void ExecuteCmd(string cmd)
    {
      if (!isProcessRunning) { systemProcess = new Process(); }
      using (systemProcess)
      {
        systemProcess.StartInfo.FileName = "/bin/bash";
        systemProcess.StartInfo.Verb = "runas";
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

    public void GoToPriorDirectory()
    {
      if (!isProcessRunning) { systemProcess = new Process(); }
      using (systemProcess)
      {
        systemProcess.StartInfo.FileName = "/bin/bash";
        systemProcess.StartInfo.Verb = "runas";
        systemProcess.StartInfo.Arguments = "-c \"cd ..";
        systemProcess.StartInfo.CreateNoWindow = true;
        systemProcess.StartInfo.UseShellExecute = false;

        systemProcess.Start();
        systemProcess.WaitForExit();
        isProcessRunning = false;
      }
    }

    public void GitClone(string cmd)
    {
      GoToPriorDirectory();
      if (!Directory.Exists("Code"))
      {
        ExecuteCmd("mkdir Code");
      }
      ChangeDirectory("Code");

      if (!isProcessRunning) { systemProcess = new Process(); }
      using (systemProcess)
      {
        systemProcess.StartInfo.FileName = "/bin/bash";
        systemProcess.StartInfo.Verb = "runas";
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

    public void RecordOptions()
    {
      Console.Clear();
      Console.WriteLine("[1]List \n[2]Create \n[3]Remove \n[4]Change \n[5]Exit");
      string selectedOption = Console.ReadLine().ToString();
      if (selectedOption != String.Empty && selectedOption != null)
      {
        switch (selectedOption)
        {
          case "1":
            DbManager.GetPrimaryRtsp();
            //SelectedRange = DialogManager.range;
            break;
          case "2":
            CreateService(); break;
          case "3":
            RemoveRstp(); break;
          case "4":
            AlterRtsp(); break;
          case "5":
            break;
        }
      }
      else
      {
        throw new ProcessException("Invalid option.");
      }
    }

    public void AlterRtsp()
    {
      System.Console.WriteLine("Type the camera ID to alter: ");
      long id = long.Parse(Console.ReadLine());
      System.Console.WriteLine("Type the new Camera IP: ");
      string ip = Console.ReadLine().ToString();

      foreach (Equipment camera in SelectedRange)
      {
        if (camera.Id == id)
        {
          camera.Ip = ip;
        }
      }

      AlterRtspOnDirectories(id, ip);
    }

    private void AlterRtspOnDirectories(long id, string ip)
    {
      ChangeDirectory("/home/records");
      IEnumerable<string> file = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"*record_{id}.sh", SearchOption.AllDirectories);
      if (file.Any())
      {
        OperationRtspProcess(file.First(), operation.Alter, ip);
      }
      else
      {
        throw new ProcessException("Any sh file was found.");
      }
      ChangeDirectory("/etc/systemd/system");
      file.First().Remove(0);

      file = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"*record_{id}.service", SearchOption.AllDirectories);
      if (file.Any())
      {
        OperationRtspProcess(file.First(), operation.Alter, ip);
      }
      else
      {
        throw new ProcessException("Any service file with the specified IP was found.");
      }
    }

    public void RemoveRstp()
    {
      if (!DialogManager.RemoveRange())
      {
        Console.WriteLine("type the camera IP to delete: ");
        string cameraIP = Console.ReadLine().ToString();
        RemoveSelected(cameraIP);

      }
      else
      {
        RemoveRange();
      }

    }

    private void RemoveSelected(string cameraIP)
    {
      ChangeDirectory("/home/records");
      IEnumerable<string> file = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"*{cameraIP}.sh", SearchOption.AllDirectories);
      if (file.Any() && file.Count() == 1)
      {
        OperationRtspProcess(file.First(), operation.Delete, string.Empty);
      }
      else
      {
        throw new ProcessException("Any sh file was found.");
      }
      ChangeDirectory("/etc/systemd/system");
      file.First().Remove(0);

      file = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"*{cameraIP}.service", SearchOption.AllDirectories);
      if (file.Any() && file.Count() == 1)
      {
        OperationRtspProcess(file.First(), operation.Delete, string.Empty);
      }
      else
      {
        throw new ProcessException("Any service file with the specified IP was found.");
      }
    }

    private void RemoveRange()
    {
      if (SelectedRange.Count > 0)
      {
        foreach (Equipment camera in SelectedRange)
        {
          ChangeDirectory("/home/record");
          OperationRtspProcess($"record_{camera.Id}.sh", operation.Delete, string.Empty);

          ChangeDirectory("/etc/systemd/system");
          OperationRtspProcess($"record_{camera.Id}.service", operation.Delete, string.Empty);
        }
      }
      else
      {
        throw new ProcessException("The range of selected cameras is empty.");
      }
    }

    private void OperationRtspProcess(string fileName, operation op, string newFileName)
    {
      if (!isProcessRunning) { systemProcess = new Process(); }
      using (systemProcess)
      {
        systemProcess.StartInfo.FileName = "/bin/bash";
        systemProcess.StartInfo.Verb = "runas";
        if (op == operation.Delete)
        {
          systemProcess.StartInfo.Arguments = $"-c sudo rm {fileName}";
        }
        else
        {
          systemProcess.StartInfo.Arguments = $"-c sudo mv {fileName} {newFileName}";
        }
        systemProcess.StartInfo.CreateNoWindow = true;
        systemProcess.StartInfo.UseShellExecute = false;

        systemProcess.Start();
        systemProcess.WaitForExit();
        isProcessRunning = false;
      }
    }

    private void CreateService()
    {
      if (DialogManager.CreateRangeOfCameras())
      {
        //Creates a range of cameras from the selected range of cameras on the database
        DialogManager.RangeDialog(DbManager.EquipmentList);
        //SelectedRange variable receives the created range of cameras
        SelectedRange = DialogManager.NewSelectedRange;
        CreateRangeOfRecordService();
      }
      else
      {
        Equipment camera = DbManager.CreateEquipment();
        if (camera.Ip != null)
        {
          string recordString = $"transport tcp - allowed_media_types video - i \"rtsp://admin:admin@{camera.Id}:8554\" -vcodec" +
                     "copy -map 0 -f segment -segment_time 20 -strftime 1 /home/records/camera/%Y%m%d%H%M%S.mkv";
          CreateShAndService(camera, recordString);
        }
        else
        {
          throw new ProcessException("The camera IP cannot be null.");
        }
      }
    }

    private void CreateRangeOfRecordService()
    {
      if (SelectedRange != null && SelectedRange.Count > 0)
      {
        for (int cont = 0; cont <= SelectedRange.Count; cont++)
        {
          string recordString = $"transport tcp - allowed_media_types video - i \"rtsp://admin:admin@{SelectedRange.ElementAt(cont).Id}:8554\" -vcodec" +
                   "copy -map 0 -f segment -segment_time 20 -strftime 1 /home/records/camera/%Y%m%d%H%M%S.mkv";
          CreateShAndService(SelectedRange.ElementAt(cont), recordString);
        }
      }
      else
      {
        throw new ProcessException("The range of selected RTSP is null.");
      }
    }

    private void CreateShAndService(Equipment camera, string recordString)
    {
      ChangeAndCreateDirectory("/home", "records");
      IEnumerable<string> filesList = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.sh", SearchOption.AllDirectories);
      string fileName = $"record_{camera.Id}.sh";
      if (!filesList.Contains(fileName))
      {
        using (FileStream newFile = new FileStream(fileName, FileMode.CreateNew))
        {
          using (StreamWriter writer = new StreamWriter(newFile))
          {
            writer.WriteLine(recordString);
            writer.Flush(); //Despeja o buffer para o stream
          };
        };
        CreateService($"record_{camera.Id}");
      }
      else
      {
        File.WriteAllText("/home/record/" + fileName, recordString);
      }
    }

    private void RealoadAndStartService(string serviceName)
    {
      ChangeDirectory("/etc/systemd/system");

      if (!isProcessRunning) { systemProcess = new Process(); }
      using (systemProcess)
      {
        systemProcess.StartInfo.FileName = "/bin/bash";
        systemProcess.StartInfo.Verb = "runas";
        systemProcess.StartInfo.ArgumentList.Add($"-c systemctl daemon-reload");
        systemProcess.StartInfo.ArgumentList.Add($"-c systemctl start {serviceName}");
        systemProcess.StartInfo.CreateNoWindow = true;
        systemProcess.StartInfo.UseShellExecute = false;

        systemProcess.Start();
        systemProcess.WaitForExit();
        isProcessRunning = false;
      }
    }

    public void CreateService(string fileName)
    {
      ChangeDirectory("/etc/systemd/system");
      string nameService = $"{fileName}.service";
      Console.WriteLine(Directory.GetCurrentDirectory());
      EditServiceScript(fileName);
    }

    /// <summary>
    /// Change the linux directory to home and creates a record directory to save records of a camera.
    /// </summary>
    /// <param name="dirToChange"></param>
    /// <param name="dirNameToCreate"></param>
    /// <exception cref="ProcessException"></exception>
    public void ChangeAndCreateDirectory(string dir, string dirNameToCreate)
    {
      if (Directory.Exists(dir))
      {
        Directory.SetCurrentDirectory(dir);  //home
        if (Directory.Exists(dirNameToCreate)) //if records already exists
        {
          Directory.SetCurrentDirectory(dirNameToCreate); //set records as current directory
        }
        else
        {
          Directory.CreateDirectory("/home/" + dirNameToCreate); //create records
          Directory.SetCurrentDirectory(dirNameToCreate);
        }
        Console.WriteLine("Current Directory: " + Directory.GetCurrentDirectory());
      }
      else
      {
        throw new ProcessException("This directory does not exist.");
      }
    }

    public void ChangeDirectory(string filePath)
    {
      if (Directory.Exists(filePath))
      {
        Directory.SetCurrentDirectory(filePath);
      }
      else
      {
        throw new ProcessException("The file path does not exist.");
      }
    }

    public void EditServiceScript(string serviceName) //Refazer esse método de maneira inteligente
    {
      using (FileStream file = new FileStream(Directory.GetCurrentDirectory() + $"/{serviceName}.service", FileMode.CreateNew))
      {
        using (StreamWriter write = new StreamWriter(file))
        {
          write.WriteLine("[Unit]");
          write.WriteLine("Description=CameraStream");
          write.WriteLine("StartLimitBurst=0");
          write.WriteLine(" ");
          write.WriteLine("[Service]");
          write.WriteLine("WorkingDirectory=/home/records/");
          write.WriteLine($"ExecStart=/bin/bash /home/records/{serviceName}" + ".sh");
          write.WriteLine("Restart=always ");
          write.WriteLine("RestartSec=10  ");
          write.WriteLine(" ");
          write.WriteLine("[Install] ");
          write.WriteLine("WantedBy=multi-user.target");

          write.Flush();
        }
      }

      SetFilesAsEx(Directory.GetCurrentDirectory() + $"/{serviceName}.service");
    }
  }

  public enum operation
  {
    Delete = 0,
    Alter = 1
  }
}
