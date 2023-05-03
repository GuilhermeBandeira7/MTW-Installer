using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    [Table("CameraControl")]
    public class CameraControl : Equipment
    {

        public string Model { get; set; } = string.Empty;
        public string CommandPort { get; set; } = string.Empty;
        public string MediaPort { get; set; } = string.Empty;


        public string CameraInOnName { get; set; } = string.Empty;
        public string CameraInOffName { get; set; } = string.Empty;
        public string CameraOutOnName { get; set; } = string.Empty;
        public string CameraOutOffName { get; set; } = string.Empty;

        public string CameraInOnColor { get; set; } = string.Empty;
        public string CameraInOffColor { get; set; } = string.Empty;
        public string CameraOutOnColor { get; set; } = string.Empty;
        public string CameraOutOffColor { get; set; } = string.Empty;


        public bool CameraOutputPulse { get; set; }
        public int CameraOutputPeriod { get; set; }
        public bool CameraInStatus { get; set; }
        public bool CameraOutStatus { get; set; }

        public int alarmTime { get; set; }

        [ForeignKey("TelemetryId")]
        public Telemetry Telemetry { get; set; } = new Telemetry();
    }
}
