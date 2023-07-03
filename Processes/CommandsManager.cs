//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using EntityMtwServer;
using EntityMtwServer.Entities;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        systemProcess.StartInfo.FileName = "/opt/mssql-tools/bin/sqlcmd";
        systemProcess.StartInfo.Verb = "runas";
        if (scriptBackup == "5")
        {
          systemProcess.StartInfo.Arguments = "-S localhost -U sa -P Senha@mtw -i " + path + "/masterserver.sql";
        }
        else if (scriptBackup == "6")
        {
          systemProcess.StartInfo.Arguments = "-S localhost -U sa -P Senha@mtw -i " + path + "/tmhub.sql";
        }
        systemProcess.StartInfo.UseShellExecute = false;
        systemProcess.StartInfo.CreateNoWindow = true;
        systemProcess.StartInfo.RedirectStandardOutput = true;

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

    /// <summary>
    /// Redirects the user input to the appropiate method. 
    /// </summary>
    /// <param name="installCmd">Input of the selected operation.</param>
    public void RedirectCommand(string installCmd)
    {
      try
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
      catch (ProcessException ex)
      {
        System.Console.WriteLine(ex.Message);
      }
      catch (Exception e)
      {
        System.Console.WriteLine(e.Message);
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
      //GoToPriorDirectory();
      ExecuteCmd("cd ..");
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
      string selectedOption = DialogManager.GetUserInput();
      switch (selectedOption)
      {
        case "1":
          //DbManager.GetAllAvailableEquipments();
          DialogManager.ListOptions();
          break;
        case "2":
          CreateService(); break;
        case "3":
          RemoveRstp(); break;
        case "4":
          break;
        default:
          throw new ProcessException("Invalid Option");
      }

    }
    public void RemoveRstp()
    {
      Console.Clear();
      System.Console.WriteLine("[1]Remove unique camera \n[2]Remove range of cameras");
      string option = Console.ReadLine().ToString();
      switch (option)
      {
        case "1":
          PrintAllFiles();
          string idToRemove = DialogManager.RemoveCameraDialog();
          RemoveSelected(idToRemove);
          break;
        case "2":
          RemoveRange();
          break;
        default:
          System.Console.WriteLine("Invalid option.");
          break;
      }

    }

    public void PrintAllFiles()
    {
      System.Console.WriteLine("Sh files and service files: ");
      System.Console.WriteLine();
      ChangeDirectory("/home/records");
      IEnumerable<string> files = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"record_*.sh", SearchOption.AllDirectories);
      foreach (var file in files)
      {
        System.Console.WriteLine(file);
      }

      System.Console.WriteLine();
      ChangeDirectory("/etc/systemd/system");
      IEnumerable<string> services = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"record_*.service", SearchOption.AllDirectories);
      foreach (var service in services)
      {
        System.Console.WriteLine(service);
      }
    }

    private void RemoveSelected(string cameraID)
    {
      ChangeDirectory("/home/records");
      File.Delete($"/home/records/record_{cameraID}.sh");

      ChangeDirectory("/etc/systemd/system");
      File.Delete($"/home/records/record_{cameraID}.service");
    }

    private void RemoveRange()
    {
      ChangeDirectory("/home/records");
      IEnumerable<string> shFiles = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"record_*.sh", SearchOption.AllDirectories);
      if (shFiles.Count() > 0)
      {
        foreach (var camera in shFiles)
        {
          File.Delete(camera);
        }
      }
      else
      {
        throw new ProcessException("Any sh file found.");
      }

      RemoveRangeOfServices();
    }

    private void RemoveRangeOfServices()
    {
      ChangeDirectory("/etc/systemd/system");
      IEnumerable<string> serviceFiles = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), $"record_*.service", SearchOption.AllDirectories);
      if (serviceFiles.Count() > 0)
      {
        foreach (var camera in serviceFiles)
        {
          File.Delete(camera);
        }
      }
      else
      {
        throw new ProcessException("Any service file found.");
      }
    }

    private void CreateService()
    {
      SelectedRange = DialogManager.NewSelectedRange;
      if (SelectedRange.Count == 0 || SelectedRange == null)
      {
        CreateUnique();
      }
      else
      {
        System.Console.WriteLine("Create service for selected range?[y/n]");
        string res = Console.ReadLine().ToString().ToUpper();
        if (res == "Y")
        {
          //if (SelectedRange.Count > 0) { SelectedRange.Clear(); }
          CreateRangeOfRecordService();
        }
      }

      RealoadAndStartService();
    }

    private void CreateUnique()
    {
      DbManager.PrintEquipmentDb();

      System.Console.WriteLine("Type the ID of the desired equipment: ");
      int selectedID = int.Parse(Console.ReadLine());
      Equipment? selected = DbManager._context.Equipments.FirstOrDefault(equip => equip.Id == selectedID);
      if (selected != null && selected.Id > 0)
      {
        string recordString = $"transport tcp - allowed_media_types video - i \"rtsp://admin:admin@record_{selected.Ip}:8554\" -vcodec" +
                   "copy -map 0 -f segment -segment_time 20 -strftime 1 /home/records/camera/%Y%m%d%H%M%S.mkv";
        CreateShAndService(selected, recordString);
      }

      else
      {
        throw new ProcessException("The camera IP cannot be null.");
      }
    }

    private void CreateRangeOfRecordService()
    {
      if (SelectedRange.Count > 0)
      {
        for (int cont = 0; cont <= SelectedRange.Count; cont++)
        {
          string recordString = $"transport tcp - allowed_media_types video - i \"rtsp://admin:admin@{SelectedRange.ElementAt(cont).Ip}:8554\" -vcodec" +
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
        SetFilesAsEx(Directory.GetCurrentDirectory() + "/" + fileName);
        CreateService($"record_{camera.Id}");
      }
      else
      {
        File.WriteAllText("/home/record/" + fileName, recordString);
      }
    }

    private void RealoadAndStartService()
    {
      ChangeDirectory("/etc/systemd/system");

      if (!isProcessRunning) { systemProcess = new Process(); }
      using (systemProcess)
      {
        systemProcess.StartInfo.FileName = "/bin/bash";
        systemProcess.StartInfo.Verb = "runas";
        systemProcess.StartInfo.Arguments = $"-c systemctl daemon-reload";
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
          Directory.CreateDirectory("/home/" + dirNameToCreate); //create records directory
          Directory.SetCurrentDirectory(dirNameToCreate);
          Directory.CreateDirectory("/home/" + dirNameToCreate + "/" + "cameras"); //create cameras directory
        }

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
