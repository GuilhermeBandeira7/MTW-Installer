using EntityMtwServer;
using EntityMtwServer.Entities;

namespace InstallerMTW.Processes
{
  public class DbManager
  {
    private static MasterServerContext _context = new MasterServerContext();

    //Holds all equipments in the database
    public static List<Equipment> EquipmentList = new List<Equipment>();

    public static void GetPrimaryRtsp()
    {
      foreach (var rtsp in _context.Equipments)
      {
        if (rtsp != null)
        {
          System.Console.WriteLine(rtsp.Id + " " + rtsp.PrimaryRtsp);
          EquipmentList.Add(rtsp);
        }
        else
        {
          throw new ProcessException("Coundn't find any rtsp on the database.");
        }
      }
    }

    public static Equipment CreateEquipment()
    {
      System.Console.WriteLine("Camera IP: ");
      string ip = Console.ReadLine().ToString();
      Equipment newCamera = new Equipment()
      {
        Ip = ip
      };
      return newCamera;
    }
  }
}
