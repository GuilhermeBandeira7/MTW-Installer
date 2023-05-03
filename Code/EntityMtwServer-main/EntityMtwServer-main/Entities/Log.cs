using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMtwServer.Entities
{
    public class Log
    {
        public long Id { get; set; }
        public string Action { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User User { get; set; } = new User();
    }
}
