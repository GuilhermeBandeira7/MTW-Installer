using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EntityMtwServer.Entities
{
    public class Class
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Course? Course { get; set; } = new Course();
        public CurriculumCourse? CurriculumCourse { get; set; } = new CurriculumCourse();
        public string Theme { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public bool State { get; set; } = true;
        public string Type { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; } = DateTime.Now;
        public DateTime EndDateTime { get; set; } = DateTime.Now;
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Session> Sessions { get; set; } = new List<Session>();

        [ForeignKey("InstructorId")]
        public User? Instructor { get; set; }

        [ForeignKey("ScheduleId")]
        public Schedule? Period { get; set; }

        [ForeignKey("InstructorDeviceId")]
        public DVC? InstructorDevice { get; set; } = null;

        [JsonIgnore]
        [NotMapped]
        private double scheduledHours;

        [JsonIgnore]
        [NotMapped]
        private double completedScheduledHours;

        [JsonIgnore]
        [NotMapped]
        public double ScheduledHours
        {
            get
            {
                DateTime dateTime = StartDateTime;
                scheduledHours = 0;

                while (dateTime < EndDateTime)
                {
                    DayOfWeek dayOfWeek = dateTime.DayOfWeek;
                    scheduledHours += ((TimeSpan)this.Period.GetType().GetProperty(dayOfWeek.ToString() + "End").GetValue(this.Period) -
                                (TimeSpan)this.Period.GetType().GetProperty(dayOfWeek.ToString() + "Start").GetValue(this.Period)).TotalHours;
                    dateTime = dateTime.AddDays(1);
                }

                return scheduledHours;
            }
            set { scheduledHours = value; }
        }

        [JsonIgnore]
        [NotMapped]
        public double CompletedScheduledHours
        {
            get
            {
                DateTime dateTime = StartDateTime;
                completedScheduledHours = 0;

                while (dateTime < EndDateTime)
                {
                    DayOfWeek dayOfWeek = dateTime.DayOfWeek;
                    if (dateTime <= DateTime.Now.Date.AddDays(1))
                        completedScheduledHours += ((TimeSpan)this.Period.GetType().GetProperty(dayOfWeek.ToString() + "End").GetValue(this.Period) -
                                    (TimeSpan)this.Period.GetType().GetProperty(dayOfWeek.ToString() + "Start").GetValue(this.Period)).TotalHours;
                    dateTime = dateTime.AddDays(1);
                }

                return completedScheduledHours;
            }
            set { completedScheduledHours = value; }
        }

        [JsonIgnore]
        [NotMapped]
        public double ElapsedHours
        {
            get
            {
                DateTime dateTime = StartDateTime;
                scheduledHours = 0;

                while (dateTime <= EndDateTime)
                {
                    DayOfWeek dayOfWeek = dateTime.DayOfWeek;
                    if (dateTime < DateTime.Now.Date.AddDays(1))
                        scheduledHours += ((TimeSpan)this.Period.GetType().GetProperty(dayOfWeek.ToString() + "End").GetValue(this.Period) -
                                    (TimeSpan)this.Period.GetType().GetProperty(dayOfWeek.ToString() + "Start").GetValue(this.Period)).TotalHours;
                    dateTime = dateTime.AddDays(1);
                }

                return scheduledHours;
            }
            set { scheduledHours = value; }
        }

        [JsonIgnore]
        [NotMapped]
        public double NumberClasses
        {
            get
            {
                DateTime dateTime = StartDateTime;
                double classes = 0;

                while (dateTime <= EndDateTime)
                {
                    DayOfWeek dayOfWeek = dateTime.DayOfWeek;
                    if (dateTime < DateTime.Now.Date.AddDays(1))
                        if (Convert.ToBoolean(Period.GetType().GetProperty(dayOfWeek.ToString()).GetValue(Period)))
                            classes++;

                    dateTime = dateTime.AddDays(1);
                }

                return classes;
            }
            set { scheduledHours = value; }
        }


    }
}
