using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMtwServer.Entities
{
    public class TelemetryMessage
    {
        public long Id { get; set; }
        public long TelemetryId { get; set; }
        public int Slot { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        [ForeignKey("TelemetryId")]
        public Telemetry Telemetry { get; set; }

    }
}
