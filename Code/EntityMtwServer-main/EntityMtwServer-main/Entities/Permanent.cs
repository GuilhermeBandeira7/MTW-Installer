using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMtwServer.Entities
{
    public class Permanent
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string JobTitle { get; set; }
        public string Plate { get; set; }
        public string Registration { get; set; }
        public DateTime StartValidDate { get; set; }
        public DateTime EndValidDate { get; set; }

        [ForeignKey("ScheduleId")]
        public Schedule Schedule { get; set; }
        [ForeignKey("VehicleModelId")]
        public VehicleModel? VehicleModel { get; set; }






    }
}
