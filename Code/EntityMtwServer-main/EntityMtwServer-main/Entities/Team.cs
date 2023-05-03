using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMtwServer.Entities
{
    public class Team
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Members { get; set; } = new List<User>();

    }
}
