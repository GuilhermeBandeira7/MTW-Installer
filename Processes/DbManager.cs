using EntityMtwServer;
using EntityMtwServer.Entities;

namespace InstallerMTW.Processes
{
  public class DbManager
  {
    private static MasterServerContext _context = new MasterServerContext();

    //Holds all equipments in the database
    public static List<Equipment> EquipmentList = new List<Equipment>();

    public static void GetAllAvailableEquipments()
    {
      foreach (var rtsp in _context.Equipments)
      {
        if (rtsp.PrimaryRtsp != String.Empty)
        {
          EquipmentList.Add(rtsp);
        }
      }
    }

    public static void GetPrimaryRtsp()
    {
      foreach (var rtsp in EquipmentList)
      {
        if (rtsp.PrimaryRtsp != String.Empty)
        {
          System.Console.WriteLine(rtsp.Id + " " + rtsp.PrimaryRtsp);
        }

        if (EquipmentList.Count == 0)
        {
          throw new ProcessException("Could not find any equipment on the database.");
        }
      }
      if (DialogManager.CreateRangeOfCameras())
      {
        DialogManager.RangeDialog(EquipmentList);
      }
    }
  }
}
