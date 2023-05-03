using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Time { get; set; } = 5;
        public string Type { get; set; } = string.Empty;


        public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
        public ICollection<User> Users { get; set; } = new List<User>();

        [InverseProperty("ServerGroups")]
        public ICollection<Server> Servers { get; set; } = new List<Server>();
        public ICollection<Group> Subgroups { get; set; } = new List<Group>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();

    }
}
