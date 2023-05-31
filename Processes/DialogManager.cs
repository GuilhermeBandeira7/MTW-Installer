using System;
using System.Xml.Serialization;

namespace InstallerMTW.Processes
{
  /// <summary>
  /// Manage the dialog with the user, get the user input and creates a CommandsManager instance to carry out commands.
  /// </summary>
  public sealed class DialogManager
  {
    private CommandsManager cmdManager { get; set; }
    private bool runDialog;

    public DialogManager()
    {
      cmdManager = new CommandsManager();
      runDialog = true;
    }

    /// <summary>
    /// Initialize the dialog with the user and get the user's input as paramater to CommandsManager methods.
    /// </summary>
    public void StartTerminalDialog()
    {
      Console.WriteLine("Welcome to MTW Installer! \n"
                    + "Make sure that you're running this app on Linux Ubuntu 18.04. \n"
                    + "Your Current Linux distribution is: ");
      System.Console.WriteLine();
      cmdManager.ExecuteCmd("lsb_release -a \n");


      try
      {
        while (runDialog == true)
        {
          Console.WriteLine("Select the desired package to install: \n[1] " +
           "MQTT \n[2] Nginx \n[3] SQL Server 2017 \n[4] Mssql-Tools \n[5] Restore MasterServer "
           + "\n[6] Restore TmHub \n[7] Git \n[8] Node Js 16x \n[9] ApiMtwServer");
          System.Console.WriteLine("type 'exit' to exit");

          string input = Console.ReadLine().ToString();

          if (input == "exit")
          {
            runDialog = false;
          }
          else
          {
            cmdManager.ExecuteInstallationScript(input);
          }

        }
      }
      catch (ProcessException e)
      {
        string error = e.Message;
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

    }
  }
}
