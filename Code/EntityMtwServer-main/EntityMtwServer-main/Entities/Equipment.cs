using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    [Table("Equipment")]
    public class Equipment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public EquipmentType Tipo { get; set; } = EquipmentType.Vazio;
        public string PrimaryRtsp { get; set; } = string.Empty;
        public string SencondaryRtsp { get; set; } = string.Empty;
        public string PrimaryStreamingRtsp { get; set; } = string.Empty;
        public string SencondaryStreamingRtsp { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public ICollection<Group> Groups { get; set; } = new List<Group>(); 
        public ICollection<User>? Users { get; set; } = new List<User>(); 
        public ICollection<Server>? Servers { get; set; } = new List<Server>();
        public ICollection<Session>? Sessions { get; set; } = new List<Session>();
    }

    public enum EquipmentType
    {
        Vazio,
        Camera,
        Switch,
        Computador,
        Modem,
        Roteador,
        Servidor,
        Telefone,
        Telemetria,
        GateWay,
        Radio,
        Medidor,
        NVR,
        DVR,
        HD,
        Outro
    }
}
