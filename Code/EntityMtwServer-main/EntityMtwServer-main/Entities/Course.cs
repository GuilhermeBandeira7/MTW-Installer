using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMtwServer.Entities
{
    public class Course
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Duration { get; set; } = 1;
        public string Type { get; set; } = "";
    }
}
