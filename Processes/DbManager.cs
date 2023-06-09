using EntityMtwServer;
using EntityMtwServer.Entities;

namespace InstallerMTW.Processes {
    public static class DbManager {

        private static MasterServerContext _context;

        public static IEnumerable<string> GetPrimaryRtsp() {
            HashSet<string> result = new HashSet<string>();
            foreach (var rtsp in _context.Equipments) {
                if (rtsp != null) {
                    result.Add(rtsp.PrimaryRtsp);
                }
                return result;
            }
            throw new ProcessException("Coundn't find any rtsp on the database.");
        }
    }
}
