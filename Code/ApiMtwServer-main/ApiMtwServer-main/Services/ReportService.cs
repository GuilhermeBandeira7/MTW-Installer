using EntityMtwServer;
using EntityMtwServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MTWServerApi.Services
{
    public class ReportService
    {
        private readonly MasterServerContext _context;

        public ReportService(MasterServerContext context)
        {
            _context = context;
        }

        public async Task<Report> GetStudentReport(long id)
        {
            User user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            List<Student> students = await _context.Students
              .AsNoTracking()
              .Include(x => x.AttendedClasses)
              .Include(x => x.Class)
              .Include(x => x.Class.Period)
              .Include(x => x.Class.Sessions)
              .Include(x => x.Class.CurriculumCourse)
              .Include(x => x.Class.CurriculumCourse.Courses)
              .Include(x => x.Class.Course)
              .Where(x => x.User.Id == id)
              .ToListAsync();

            foreach (Student st in students)
                st.AttendedClasses = await _context.Sessions
                    .AsNoTracking()
                    .Where(x => st.AttendedClasses.Select(c => c.Id).Contains(x.Id))
                    .Include(x => x.Events)
                    .Include(x => x.Class)
                    .Include(x => x.Class.Period)
                    .Include(x => x.Class.CurriculumCourse)
                    .Include(x => x.Class.CurriculumCourse.Courses)
                    .Include(x => x.Class.Course)
                    .ToListAsync();


            return GetReport(user.Type, user.Name, students.Select(x => x.AttendedClasses).SelectMany(x => x).ToList());
        }

        public async Task<ActionResult<object>> GetInstructorReport(long id)
        {
            User user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            List<Session> sessions = await _context.Sessions
                .AsNoTracking()
                .Where(x => x.Instructor.Id == id)
                .Include(x => x.Events)
                .Include(x => x.Class)
                .Include(x => x.Class.Period)
                .Include(x => x.Class.CurriculumCourse)
                .Include(x => x.Class.CurriculumCourse.Courses)
                .Include(x => x.Class.Course)
                .ToListAsync();
            return GetReport(user.Type, user.Name, sessions);
        }

        public async Task<ActionResult<object>> GetThemeReport(string theme)
        {
            List<Session> sessions = await _context.Sessions
                .AsNoTracking()
                .Where(x => x.Class.Theme == theme || x.Type == theme)
                .Include(x => x.Events)
                .Include(x => x.Class)
                .Include(x => x.Class.Period)
                .Include(x => x.Class.CurriculumCourse)
                .Include(x => x.Class.CurriculumCourse.Courses)
                .Include(x => x.Class.Course)
                .ToListAsync();
            return GetReport("theme", theme, sessions);
        }

        public Report GetReport(string type, string name, List<Session> classes)
        {
            List<Session> curriculumClasses = classes
              .Where(x => x.Class != null)
              .Where(x => x.Class.CurriculumCourse.Id > 0)
              .ToList();


            List<Session> courseClasses = classes
                .Where(x => x.Class != null)
                .Where(x => x.Class.Course.Id > 0)
                .ToList();

            List<Session> otherClasses = classes
                .Where(x => !curriculumClasses
                .Select(c => c.Id).Contains(x.Id) && !courseClasses.Select(c => c.Id).Contains(x.Id))
                .ToList();

            List<CurriculumReport> curriculumReports = new List<CurriculumReport>();
            List<CourseReport> courseReports = new List<CourseReport>();


            return new Report(type, name, CreateCurriculumReport(curriculumClasses), CreateCourseReport(courseClasses));
        }



        public List<CurriculumReport> CreateCurriculumReport(List<Session> curriculumClasses)
        {
            List<CurriculumReport> curriculumReports = new List<CurriculumReport>();

            foreach (KeyValuePair<string, List<Session>> curriculumCourse in curriculumClasses.GroupBy(x => x.Class.CurriculumCourse.Type).ToDictionary(g => g.Key, g => g.ToList()))
            {
                CurriculumReport curriculumReport = new CurriculumReport(CreateCourseReport(curriculumCourse.Value));
                curriculumReport.Type = curriculumCourse.Key;
                curriculumReports.Add(curriculumReport);
            }
            
            return curriculumReports;
        }


        public List<CourseReport> CreateCourseReport(List<Session> courseClasses)
        {
            List<CourseReport> courseReports = new List<CourseReport>();
            foreach (KeyValuePair<string, List<Session>> course in courseClasses.GroupBy(x => x.Class.Theme).ToDictionary(g => g.Key, g => g.ToList()))
            {
                CourseReport courseReport = new CourseReport();
                Dictionary<Class, List<Session>> sessionsByClass = course.Value.GroupBy(x => x.Class.Id).ToDictionary(g => g.First().Class, g => g.ToList());


                courseReport.Type = course.Key;
                //courseReport.Grade = students.Where(x => x.Class != null).Where(x => course.Value.GroupBy(c => c.Class.Id).ToDictionary(g => g.Key, g => g.ToList()).Keys.Contains(x.Class.Id)).ToList().Select(x => x.Grade).Average();
                courseReport.ScheduledHours = Math.Round(sessionsByClass.Keys.Select(x => x.ScheduledHours).Sum(), 2);
                courseReport.CompletedScheduledHours = Math.Round(sessionsByClass.Keys.Select(x => x.CompletedScheduledHours).Sum(), 2);
                courseReport.NeededHours = course.Value.First().Class.Course.Id > 0 ? course.Value.First().Class.Course.Duration : course.Value.First().Class.CurriculumCourse.Courses.Where(x => x.Type == course.Key).FirstOrDefault().Duration;
                courseReport.CompletedHours = Math.Round(course.Value.Where(x => x.EndDateTime < DateTime.Now).Select(x => (x.EndDateTime - x.StartDateTime).TotalHours).Sum() - TimeSpan.FromSeconds(Convert.ToDouble(course.Value.Select(x => x.Events.Select(e => e.Duration).Sum()).Sum())).TotalHours, 2);
                courseReport.StoppedHours = Math.Round(TimeSpan.FromSeconds(Convert.ToDouble(course.Value.Select(x => x.Events.Select(e => e.Duration).Sum()).Sum())).TotalHours, 2);
                courseReport.MissedHours = Math.Round(sessionsByClass.Keys.Select(x => x.ElapsedHours).Sum() - course.Value.Where(x => x.EndDateTime <= DateTime.Now.Date.AddDays(1)).Select(x => (x.EndDateTime - x.StartDateTime).TotalHours).Sum(), 2);
                courseReport.ScheduledClasses = course.Value.Count();
                courseReport.CompletedClasses = course.Value.Where(x => x.EndDateTime < DateTime.Now).Count();
                courseReport.MissedClasses = sessionsByClass.Keys.Select(x => x.NumberClasses).Sum() - course.Value.Where(x => x.EndDateTime <= DateTime.Now.Date.AddDays(1)).Count();
                courseReports.Add(courseReport);
            }

            return courseReports;
        }

  
    }
}
