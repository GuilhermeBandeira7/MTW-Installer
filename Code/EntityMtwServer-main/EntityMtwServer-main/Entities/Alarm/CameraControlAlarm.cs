using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    public class CameraControlAlarm
    {
        public long Id { get; set; }
        public long EquipmentId { get; set; }
        public bool InOnAlarm { get; set; }
        public bool InOffAlarm { get; set; }
        public bool OutOnAlarm { get; set; }
        public bool OutOffAlarm { get; set; }

        [ForeignKey("EquipmentId")]
        Equipment Equipment { get; set; }

    }
}
