using EntityMtwServer;
using EntityMtwServer.Entities;

namespace InstallerMTW.Processes
{
  public class DbManager
  {
    private static MasterServerContext _context = new MasterServerContext();

    public static void GetPrimaryRtsp()
    {
      List<Equipment> listID = new List<Equipment>();
      foreach (var rtsp in _context.Equipments)
      {
        if (rtsp != null)
        {
          System.Console.WriteLine(rtsp.Id + " " + rtsp.PrimaryRtsp);
          listID.Add(rtsp);
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
        DialogManager.RangeDialog(listID);
      }
    }
  }
}
