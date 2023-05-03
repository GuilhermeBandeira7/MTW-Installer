using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMtwServer.Entities
{

    public class CourseReport
    {
        public string Type { get; set; } = string.Empty;
        public double Grade { get; set; }
        public double ScheduledHours { get; set; }
        public double CompletedScheduledHours { get; set; }
        public double NeededHours { get; set; }
        public double CompletedHours { get; set; }
        public double StoppedHours { get; set; }
        public double MissedHours { get; set; }
        public double ScheduledClasses { get; set; }
        public double CompletedClasses { get; set; }
        public double MissedClasses { get; set; }
    }


    public class CurriculumReport
    {
        private List<CourseReport> courseReports = new List<CourseReport>();

        public CurriculumReport(List<CourseReport> courseReports)
        {
            CourseReports = courseReports;
        }

        public string Type { get; set; } = string.Empty;
        public double Grade { get; set; }
        public double ScheduledHours { get; set; }
        public double CompletedScheduledHours { get; set; }
        public double NeededHours { get; set; }
        public double CompletedHours { get; set; }
        public double StoppedHours { get; set; }
        public double MissedHours { get; set; }
        public double ScheduledClasses { get; set; }
        public double CompletedClasses { get; set; }
        public double MissedClasses { get; set; }


        public List<CourseReport> CourseReports
        {
            get
            {
                return courseReports;
            }
            set
            {
                if (value != null)
                {
                    if (value.Count > 0)
                    {
                        Grade = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("Grade").GetValue(x))).Average(), 2);
                        ScheduledHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("ScheduledHours").GetValue(x))).Sum(),2);
                        CompletedScheduledHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("CompletedScheduledHours").GetValue(x))).Sum(), 2);
                        NeededHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("NeededHours").GetValue(x))).Sum(), 2);
                        CompletedHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("CompletedHours").GetValue(x))).Sum(), 2);
                        StoppedHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("StoppedHours").GetValue(x))).Sum(), 2);
                        MissedHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("MissedHours").GetValue(x))).Sum(), 2);
                        ScheduledClasses = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("ScheduledClasses").GetValue(x))).Sum(), 2);
                        CompletedClasses = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("CompletedClasses").GetValue(x))).Sum(), 2);
                        MissedClasses = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("MissedClasses").GetValue(x))).Sum(), 2);
                        courseReports = value;
                    }
                }
            }
        }
    }


    public class Report
    {
        private List<CurriculumReport> curriculumReports;
        private List<CourseReport> courseReports;

        public Report()
        {
        }

        public Report(string role, string name, List<CurriculumReport> curriculumReports, List<CourseReport> courseReports)
        {
            Role = role;
            Name = name;
            CurriculumReports = curriculumReports;
            CourseReports = courseReports;
        }

        public string Role { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public double Grade { get; set; } = 0;
        public double CurriculumScheduledHours { get; set; }
        public double CurriculumCompletedScheduledHours { get; set; }
        public double CurriculumNeededHours { get; set; }
        public double CurriculumCompletedHours { get; set; }
        public double CurriculumStoppedHours { get; set; }
        public double CurriculumMissedHours { get; set; }
        public double CurriculumScheduledClasses { get; set; }
        public double CurriculumCompletedClasses { get; set; }
        public double CurriculumMissedClasses { get; set; }

        public List<CurriculumReport> CurriculumReports
        {
            get 
            {
                return curriculumReports;
            }
            set
            {
                if (value != null)
                {
                    if (value.Count > 0)
                    {
                        Grade = Math.Round(value.Select(x => Convert.ToDouble(typeof(CurriculumReport).GetProperty("Grade").GetValue(x))).Average(), 2);
                        CurriculumScheduledHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CurriculumReport).GetProperty("ScheduledHours").GetValue(x))).Sum(), 2);
                        CurriculumCompletedScheduledHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CurriculumReport).GetProperty("CompletedScheduledHours").GetValue(x))).Sum(), 2);
                        CurriculumNeededHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CurriculumReport).GetProperty("NeededHours").GetValue(x))).Sum(), 2);
                        CurriculumCompletedHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CurriculumReport).GetProperty("CompletedHours").GetValue(x))).Sum(), 2);
                        CurriculumStoppedHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CurriculumReport).GetProperty("StoppedHours").GetValue(x))).Sum(), 2);
                        CurriculumMissedHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CurriculumReport).GetProperty("MissedHours").GetValue(x))).Sum(), 2);
                        CurriculumScheduledClasses = Math.Round(value.Select(x => Convert.ToDouble(typeof(CurriculumReport).GetProperty("ScheduledClasses").GetValue(x))).Sum(),2);
                        CurriculumCompletedClasses = Math.Round(value.Select(x => Convert.ToDouble(typeof(CurriculumReport).GetProperty("CompletedClasses").GetValue(x))).Sum(), 2);
                        CurriculumMissedClasses = Math.Round(value.Select(x => Convert.ToDouble(typeof(CurriculumReport).GetProperty("MissedClasses").GetValue(x))).Sum(), 2);
                        curriculumReports = value;
                    }
                }
            }
        }


        public double CourseScheduledHours { get; set; }
        public double CourseCompletedScheduledHours { get; set; }
        public double CourseNeededHours { get; set; }
        public double CourseCompletedHours { get; set; }
        public double CourseStoppedHours { get; set; }
        public double CourseMissedHours { get; set; }
        public double CourseScheduledClasses { get; set; }
        public double CourseCompletedClasses { get; set; }
        public double CourseMissedClasses { get; set; }

        public List<CourseReport> CourseReports
        {
            get 
            {
                return courseReports; 
            }
            set 
            {
                if (value != null)
                {
                    if (value.Count > 0)
                    {
                        CourseScheduledHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("ScheduledHours").GetValue(x))).Sum(), 2);
                        CourseCompletedScheduledHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("CompletedScheduledHours").GetValue(x))).Sum(), 2);
                        CourseNeededHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("NeededHours").GetValue(x))).Sum(), 2);
                        CourseCompletedHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("CompletedHours").GetValue(x))).Sum(), 2);
                        CourseStoppedHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("StoppedHours").GetValue(x))).Sum(), 2);
                        CourseMissedHours = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("MissedHours").GetValue(x))).Sum(), 2);
                        CourseScheduledClasses = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("ScheduledClasses").GetValue(x))).Sum(), 2);
                        CourseCompletedClasses = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("CompletedClasses").GetValue(x))).Sum(), 2);
                        CourseMissedClasses = Math.Round(value.Select(x => Convert.ToDouble(typeof(CourseReport).GetProperty("MissedClasses").GetValue(x))).Sum(), 2);
                        courseReports = value;
                    }
                }
            } 
        }

    }
}
