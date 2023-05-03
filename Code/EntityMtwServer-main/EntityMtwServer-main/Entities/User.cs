using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{    
    public class User
    {
        
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Registration { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public bool Active { get; set; }
        public ICollection<Group>? Groups { get; set; }
        public ICollection<Equipment>? Equipments { get; set; }

        [ForeignKey("ProfileId")]
        public Profile? Profile { get; set; }

    }
}
