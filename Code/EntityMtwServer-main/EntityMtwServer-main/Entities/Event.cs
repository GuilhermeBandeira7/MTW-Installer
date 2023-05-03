using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMtwServer.Entities
{
    public class Event
    {
        public long Id { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
