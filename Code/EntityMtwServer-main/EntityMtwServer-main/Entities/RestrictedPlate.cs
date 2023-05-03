using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMtwServer.Entities
{
    public class RestrictedPlate
    {
        public long Id { get; set; }
        public string Plate { get; set; }
        public string Description { get; set; }
        public DateTime StartValidDate { get; set; }
        public DateTime EndValidDate { get; set; }

        [ForeignKey("VehicleModelId")]
        public VehicleModel? VehicleModel { get; set; }

    }
}
