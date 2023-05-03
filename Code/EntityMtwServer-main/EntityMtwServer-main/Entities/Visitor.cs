using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    public class Visitor
    {
        public long Id { get; set; }
        public string Plate { get; set; }
        public string Title { get; set; }
        public DateTime AuthorizationDate { get; set; }
        public string Name { get; set; }

  
        [ForeignKey("VehicleModelId")]
        public VehicleModel? VehicleModel { get; set; }
        [ForeignKey("PeriodId")]
        public Period Period { get; set; }

    }
}
