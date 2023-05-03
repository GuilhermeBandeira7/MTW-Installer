using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    public class Gateway
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string GatewayIp { get; set; } = string.Empty;
        public int GatewayPort { get; set; } = 11000;
        public string ServerRemoto { get; set; } = string.Empty;
    }
}
