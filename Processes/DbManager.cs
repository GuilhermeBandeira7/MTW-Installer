using EntityMtwServer;
using EntityMtwServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace InstallerMTW.Processes
{
  public class DbManager
  {
    public static MasterServerContext _context = new MasterServerContext();

    //Holds all equipments of the database
    public static List<Equipment> EquipmentList = new List<Equipment>();


    public static void PrintEquipmentDb()
    {
      foreach (var rtsp in _context.Equipments.Include(x => x.RtspConfigs))
      {
        if (rtsp.RtspConfigs.Count > 0)
        {
          System.Console.WriteLine(rtsp.Id + " " + rtsp.RtspConfigs.First().OutputRtsp + " " + rtsp.Type);
        }
      }
    }

    /// <summary>
    /// Add all equipments on the database to the EquipementList object and Calls dialog manager for range selection.
    /// </summary>
    public static void AddEquimentsToList()
    {
      if (EquipmentList.Count > 0)
      {
        EquipmentList.Clear();
      }
      foreach (var rtsp in _context.Equipments.Include(x => x.RtspConfigs))
      {
        if (rtsp.RtspConfigs.Count > 0)
        {
          EquipmentList.Add(rtsp);
        }
      }

      DialogManager.RangeDialog(EquipmentList);
    }
  }
}
