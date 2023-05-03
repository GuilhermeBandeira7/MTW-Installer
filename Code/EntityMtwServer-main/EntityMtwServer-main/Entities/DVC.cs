using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    [Table("DVC")]
    public class DVC : Equipment
    {
        public string SerialNumber { get; set; } = string.Empty;
        public string Function { get; set; } = string.Empty;
        public string OperationalSystem { get; set; } = string.Empty;
        public bool VideoEnable { get; set; } = false;
        public bool AudioEnable { get; set; } = true;
        public bool PermanentStream { get; set; } = false;
        public bool Status { get; set; } = true;
        public DateTime StatusDateTime { get; set; } = DateTime.Now;

        [ForeignKey("TelemetryId")]
        public Telemetry? Telemetry { get; set; }

        [ForeignKey("ServerId")]
        public Server? Server { get; set; }
    }
}
