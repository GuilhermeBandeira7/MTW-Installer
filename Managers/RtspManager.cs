using EntityMtwServer;
using EntityMtwServer.Entities;

namespace InstallerMTW.Managers {
    public class RtspManager {
        private static MasterServerContext _context = new MasterServerContext();

        private static List<Equipment> listID = new List<Equipment>();

        public static void GetPrimaryRtsp() {
            {
                foreach (var rtsp in _context.Equipments) {
                    if (rtsp != null) {
                        Console.WriteLine(rtsp.Id + " " + rtsp.PrimaryRtsp);
                        listID.Add(rtsp);
                    }
                    else {
                        throw new ProcessException("Coundn't find any rtsp on the database.");
                    }
                }
                Console.WriteLine("Select range of ID's to operate?[y/n]");
                string response = Console.ReadLine();
                if (response == "y") {
                    DialogManager.RangeDialog(listID);
                }
            }
        }
    }
}
