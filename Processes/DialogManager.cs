using System;
using System.Xml.Serialization;

namespace InstallerMTW.Processes
{
  /// <summary>
  /// Manage the dialog with the user, get the user input and creates a CommandsManager instance to execute the commands.
  /// </summary>
  public class DialogManager
  {
    private CommandsManager cmdManager { get; set; }

    public DialogManager()
    {
      cmdManager = new CommandsManager();
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
      cmdManager.CheckOSVersion("lsb_release -a \n");


      try
      {
        Console.WriteLine("Select the desired package to install: \n[1] " +
            "MQTT \n[2] Nginx \n[3] SQL Server 2017");

        //string input = Console.ReadLine().ToString(); ;
        string input = "1";

        switch (input)
        {
          case "1":
            cmdManager.ExecuteInstallationScript(input); break;
          case "2":
            cmdManager.ExecuteInstallationScript(input); break;
          case "3":
            cmdManager.ExecuteInstallationScript(input); break;

          default: Console.WriteLine("Option not found."); break;
        }
      }
      catch (ProcessException e)
      {
        Console.WriteLine(e.Message);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

    }
  }
}
