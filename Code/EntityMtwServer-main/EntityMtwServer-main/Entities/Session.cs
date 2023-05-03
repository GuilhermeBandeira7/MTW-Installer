using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMtwServer.Entities
{
    public class Session
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; } = DateTime.Now;
        public DateTime EndDateTime { get; set; } = DateTime.Now;
        public bool Record { get; set; } = false;
        public string RecordPath { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public string Description { get; set; } = string.Empty;
        public string MainRtsp { get; set; } = string.Empty;
        public string SubRtsp { get; set; } = string.Empty;
        public string Requisition { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public bool Live { get; set; } = true;
        public string Filter { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public List<Event> Events { get; set; } = new List<Event>();
        public List<Group> Cells { get; set; } = new List<Group>();
        public List<Equipment>? Equipments { get; set; } = new List<Equipment>();


        [NotMapped]
        public bool Active { get; set; } = true;


        [ForeignKey("ClassId")]
        public Class? Class { get; set; } = null;
        public List<Student>? Students { get; set; } = new List<Student>();

        [ForeignKey("InstructorId")]
        public User? Instructor { get; set; } = null;

        [ForeignKey("InstructorDeviceId")]
        public DVC? InstructorDevice { get; set; } = null;
        

    }
}
