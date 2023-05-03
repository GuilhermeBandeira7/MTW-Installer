using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    [Table("Record")]
    public class Record : Equipment
    {
        public string RecordPath { get; set; }
        public int Duration { get; set; }
        public string RemoteUser { get; set; }
        public string RemotePassword { get; set; }
    }
}
