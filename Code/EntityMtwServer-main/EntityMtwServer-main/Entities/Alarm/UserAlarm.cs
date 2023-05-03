using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMtwServer.Entities
{
    public class UserAlarm
    {
        public long Id {get;set;}
        public string Name {get;set;}
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string OriginEmail { get; set; }
        public string Password { get; set; }
        public string TargetEmail { get; set; }
        public string TargetPhone { get; set; }
        public bool SmsCheck { get; set; }


        [ForeignKey("ScheduleId")]
        public Schedule? Schedule { get; set; }


    }
}
