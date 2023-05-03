using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMtwServer.Entities
{
    public class Student
    {
        public long Id { get; set; }
        public int Grade { get; set; }
        public double LiveHours { get; set; }
        public double RemoteHours { get; set; }
        public int LiveClass { get; set; }
        public int RemoteClass { get; set; }
        public string? StudentStatus { get; set; } = string.Empty;

        public List<Session>? AttendedClasses { get; set; }


        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("ClassId")]
        public Class? Class { get; set; }
    }
}
