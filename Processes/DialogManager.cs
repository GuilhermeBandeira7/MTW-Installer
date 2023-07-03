using System;
using System.Xml.Serialization;
using EntityMtwServer.Entities;

namespace InstallerMTW.Processes
{
  /// <summary>
  /// Manage the dialog with the user, get the user input and creates a CommandsManager instance to carry out commands.
  /// </summary>
  public sealed class DialogManager
  {
    private CommandsManager cmdManager { get; set; }
    private bool runDialog;

    //range variable holds the range of RTSPs selected by the user.
    public static List<Equipment> NewSelectedRange = new List<Equipment>();

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
          Console.WriteLine(" \n[1] " +
           "MQTT \n[2] Nginx \n[3] SQL Server 2017 \n[4] Mssql-Tools \n[5] Restore MasterServer "
           + "\n[6] Restore TmHub \n[7] Git \n[8] Node Js 16x \n[9] ApiMtwServer \n[10] Record");
          System.Console.WriteLine("type 'exit' to exit");

          string input = Console.ReadLine().ToString();

          if (input == "exit")
          {
            runDialog = false;
          }
          else
          {
            cmdManager.RedirectCommand(input);
          }
        }
      }
      catch (ProcessException e)
      {
        string error = e.Message;
        Console.WriteLine(error);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }

    public static void RangeDialog(List<Equipment> list)
    {
      System.Console.WriteLine("Type the  Id of the first element of the range: ");
      int start = int.Parse(Console.ReadLine());
      System.Console.WriteLine("Type the Id of the last element of the range: ");
      int end = int.Parse(Console.ReadLine());

      if (ValidRange(start, end, list))
      {
        SelectedRange(start, end, list);
      }
      else
      {
        throw new ProcessException("Invalid selection of the range.");
      }

    }

    /// <summary>
    /// Select the desired range based on the user input and saves it within NewSelectedRange.
    /// </summary>
    public static void SelectedRange(int start, int end, List<Equipment> equipments)
    {
      if (NewSelectedRange.Count > 0)
      {
        NewSelectedRange.Clear();
      }
      for (int cont = start; cont <= end; cont++)
      {
        Equipment? element = equipments.FirstOrDefault(equip => equip.Id == cont);
        if (element != null && element.RtspConfigs.First().OutputRtsp != string.Empty)
        {
          NewSelectedRange.Add(element);
        }
      }
      PrintSelectedRange();
    }

    public static bool ValidRange(int start, int end, List<Equipment> equipments)
    {
      int numOfElements = equipments.Count;

      if (end >= start && end <= numOfElements)
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    private static void PrintSelectedRange()
    {
      Console.Clear();
      if (NewSelectedRange.Count > 0)
      {
        System.Console.WriteLine("elements in the selected range");
        foreach (Equipment equip in NewSelectedRange)
        {
          System.Console.WriteLine(equip.Id + " " + equip.RtspConfigs.First().OutputRtsp + " " + equip.Type);
        }
      }
      else
      {
        throw new ProcessException("The list of selected elements is empty.");
      }

    }

    public static string GetUserInput()
    {
      Console.WriteLine("[1]List \n[2]Create \n[3]Remove \n[4]Exit");
      string? selectedOption = Console.ReadLine().ToString();
      if (selectedOption != String.Empty)
      {
        return selectedOption;
      }
      else
      {
        throw new ProcessException("Input can't be Null value.");
      }
    }

    /// <summary>
    /// List all equipments on the database and calls DbManager to add all equipments to a new list.
    /// </summary>
    public static void ListOptions()
    {
      System.Console.WriteLine("[1]List equipments on Database \n[2]Select Range \n[3]Clear Selected Range \n[4]Exit");
      string? selectedOption = Console.ReadLine().ToString();
      if (selectedOption != String.Empty)
      {
        switch (selectedOption)
        {
          case "1":
            DbManager.PrintEquipmentDb();
            break;
          case "2":
            DbManager.PrintEquipmentDb();
            DbManager.AddEquimentsToList();
            break;
          case "3":
            ClearSelectedRange();
            break;
          case "4":
            break;
        }

      }
      else
      {
        throw new ProcessException("Input can't be Null value.");
      }
    }

    public static void ClearSelectedRange()
    {
      if (NewSelectedRange.Count > 0)
      {
        NewSelectedRange.Clear();
      }
      else
      {
        throw new ProcessException("Selected range is already empty.");
      }
    }

    public static string RemoveCameraDialog()
    {
      System.Console.WriteLine("type the Id of the camera to delete: ");
      string response = Console.ReadLine().ToString();
      return response;
    }
  }
}
