using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Plate { get; set; }

        [ForeignKey("VehicleModelId")]
        public VehicleModel VehicleModel { get; set; } = new VehicleModel();
    }
}
