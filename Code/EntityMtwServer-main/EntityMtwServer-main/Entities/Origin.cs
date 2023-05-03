using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    public class Origin
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string OriginCode { get; set; }
        [ForeignKey("TelemetryId")]
        public Telemetry? Telemetry { get; set; }

    }
}