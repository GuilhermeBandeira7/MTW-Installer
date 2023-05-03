using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    [Table("Server")]
    public class Server : Equipment
    {
        public bool TelemetryServer { get; set; }
        public bool LprServer { get; set; }
        public bool MasterEyeServer { get; set; }
        public bool DigifortServer { get; set; }
        public bool RecordServer { get; set; }
        public bool SessionServer { get; set; }
        public bool RtspServer { get; set; }

        public ICollection<Equipment> ServerEquipments { get; set; } = new List<Equipment>(); //
        public ICollection<Group> ServerGroups { get; set; } = new List<Group>(); //
    }
}
