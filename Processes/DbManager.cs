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
      System.Console.WriteLine("Select range of ID's to operate?[y/n]");
      string response = Console.ReadLine();
      if (response == "y")
      {
        DialogManager.RangeDialog(EquipmentList);
      }
    }

    public static Equipment CreateEquipment()
    {
      System.Console.WriteLine("Equipment name: ");
      string name = Console.ReadLine().ToString();
      System.Console.WriteLine("user: ");
      string user = Console.ReadLine().ToString();
      System.Console.WriteLine("Password: ");
      string password = Console.ReadLine().ToString();
      System.Console.WriteLine("Camera IP: ");
      string ip = Console.ReadLine().ToString();
      System.Console.WriteLine("Primary RTSP: ");
      string primaryRtsp = Console.ReadLine().ToString();

      Equipment newCamera = new Equipment()
      {
        Name = name,
        User = user,
        Password = password,
        Ip = ip,
        PrimaryRtsp = primaryRtsp
      };
      _context.Add(newCamera);
      return newCamera;
    }
  }
}
