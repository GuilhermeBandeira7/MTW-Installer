using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    [Table("Telemetry")]
    public class Telemetry : Equipment
    {
        public string SerialNumber { get; set; }
        public ICollection<Gateway>? Gateways { get; set; }
        public string VirtualInputs { get; set; }
    }
}
