using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    public class Action
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Authorization { get; set; } = string.Empty;
        public string TypePerson { get; set; } = string.Empty;
        public string Plate { get; set; } = string.Empty;
        public string ActionValue { get; set; } = string.Empty;
        public string Parameters { get; set; } = string.Empty;

        [ForeignKey("OriginId")]
        public Origin? Origin { get; set; } = new Origin();

    }
}
