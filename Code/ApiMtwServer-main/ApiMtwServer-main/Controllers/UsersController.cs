#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityMtwServer;
using EntityMtwServer.Entities;
using MTWServerApi.Services;

namespace MTWServerApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserServices _services;
        private readonly ReportService _reportService;
        private readonly MasterServerContext _context;

        public UsersController(UserServices services, ReportService reportService, MasterServerContext context)
        {
            _services = services;
            _reportService = reportService;
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _services.GetAllUsers();
        }

        [HttpGet("availableGroups")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersNoGroup()
        {
            List<long> cellsGroupsIds = await _context.Groups.AsNoTracking().Where(x => x.Type == "Cell").Select(x => x.Id).ToListAsync();
            List<User> users = await _context.Users.AsNoTracking().Where(x => x.Type == "Preso").Include(x => x.Groups).ToListAsync();

            return users.Where(x => x.Groups.Select(g => g.Id).Intersect(cellsGroupsIds).Count() == 0).ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            return await _services.GetUserById(id);
        }

        // GET: api/Users/5
        [HttpGet("type/{type}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersByType(string type)
        {
            return await _context.Users.AsNoTracking().Where(x => x.Type == type).ToListAsync();
        }


        [HttpGet("{username}/{password}")]
        public async Task<ActionResult<User>> GetUser(string username, string password)
        {
            User user = await _services.GetUserByUsernamePassword(username, password);
            return user;
        }

      

        [HttpGet("studentReport")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserStudentReport()
        {
            try
            {
                List<User> users = await _context.Users.Where(x => _context.Students.Where(s => s.User.Id == x.Id).Count() > 0).ToListAsync();
                List<object> reports = new List<object>();

                foreach (User user in users)
                    reports.Add((await GetUserStudentReport(user.Id)).Value);

                return reports;

            }
            catch
            {
                return ValidationProblem();
            }
        }



        [HttpGet("studentReport/{id}")]
        public async Task<ActionResult<object>> GetUserStudentReport(long id)
        {
            return await _reportService.GetStudentReport(id);
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

            List<Session> classes = students
                .Select(x => x.AttendedClasses)
                .SelectMany(x => x)
                .ToList();

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

            foreach (KeyValuePair<string, Dictionary<string, List<Session>>> curriculumCourse in curriculumClasses
                .GroupBy(x => x.Class.CurriculumCourse.Name)
                .ToDictionary(g => g.Key, g => g.GroupBy(x => x.Class.Theme).ToDictionary(g => g.Key, g => g.ToList())))
            {

                List<CourseReport> reports = new List<CourseReport>();

                foreach (KeyValuePair<string, List<Session>> course in curriculumCourse.Value)
                {
                    CourseReport courseReport = new CourseReport();


                    courseReport.Type = course.Key;
                    courseReport.Grade = students.Where(x => x.Class != null).Where(x => course.Value.GroupBy(c => c.Class.Id).ToDictionary(g => g.Key, g => g.ToList()).Keys.Contains(x.Class.Id)).ToList().Select(x => x.Grade).Average();
                    courseReport.ScheduledHours = course.Value.GroupBy(x => x.Class.Id).ToDictionary(g => g.Key, g => g.ToList()).Values.Select(x => x.First().Class.ScheduledHours).Sum();
                    courseReport.CompletedScheduledHours = course.Value.GroupBy(x => x.Class.Id).ToDictionary(g => g.Key, g => g.ToList()).Values.Select(x => x.First().Class.CompletedScheduledHours).Sum();
                    courseReport.NeededHours = course.Value.First().Class.CurriculumCourse.Courses.Where(x => x.Type == course.Key).First().Duration;
                    courseReport.CompletedHours = course.Value.Where(x => x.EndDateTime <= DateTime.Now.Date.AddDays(1)).Select(x => (x.EndDateTime - x.StartDateTime).TotalHours).Sum() - TimeSpan.FromSeconds(Convert.ToDouble(course.Value.Select(x => x.Events.Select(e => e.Duration).Sum()).Sum())).TotalHours;
                    courseReport.StoppedHours = TimeSpan.FromSeconds(Convert.ToDouble(course.Value.Select(x => x.Events.Select(e => e.Duration).Sum()).Sum())).TotalHours;
                    courseReport.MissedHours = course.Value.GroupBy(x => x.Class.Id).ToDictionary(g => g.Key, g => g.ToList()).Values.Select(x => x.First().Class.ElapsedHours).Sum() - course.Value.Where(x => x.EndDateTime <= DateTime.Now.Date.AddDays(1)).Select(x => (x.EndDateTime - x.StartDateTime).TotalHours).Sum();
                    courseReport.ScheduledClasses = course.Value.Count();
                    courseReport.CompletedClasses = course.Value.Where(x => x.EndDateTime <= DateTime.Now.Date.AddDays(1)).Count();
                    courseReport.MissedClasses = course.Value.GroupBy(x => x.Class).ToDictionary(g => g.Key, g => g.ToList()).Select(x => x.Key.Sessions.Where(x => x.EndDateTime <= DateTime.Now.Date.AddDays(1)).Count()).Sum() - course.Value.Where(x => x.EndDateTime <= DateTime.Now.Date.AddDays(1)).Count();
                    reports.Add(courseReport);

                }

                CurriculumReport curriculumReport = new CurriculumReport(reports);
                curriculumReports.Add(curriculumReport);
            }

            foreach (KeyValuePair<string, List<Session>> course in courseClasses.GroupBy(x => x.Class.Course.Type).ToDictionary(g => g.Key, g => g.ToList()))
            {
                CourseReport courseReport = new CourseReport();
                courseReport.Type = course.Key;
                courseReport.Grade = students.Where(x => x.Class != null).Where(x => course.Value.GroupBy(c => c.Class.Id).ToDictionary(g => g.Key, g => g.ToList()).Keys.Contains(x.Class.Id)).ToList().Select(x => x.Grade).Average();
                courseReport.ScheduledHours = course.Value.GroupBy(x => x.Class.Id).ToDictionary(g => g.Key, g => g.ToList()).Values.Select(x => x.First().Class.ScheduledHours).Sum();
                courseReport.CompletedScheduledHours = course.Value.GroupBy(x => x.Class.Id).ToDictionary(g => g.Key, g => g.ToList()).Values.Select(x => x.First().Class.CompletedScheduledHours).Sum();
                courseReport.NeededHours = course.Value.First().Class.Course.Duration;
                courseReport.CompletedHours = course.Value.Where(x => x.EndDateTime < DateTime.Now).Select(x => (x.EndDateTime - x.StartDateTime).TotalHours).Sum() - TimeSpan.FromSeconds(Convert.ToDouble(course.Value.Select(x => x.Events.Select(e => e.Duration).Sum()).Sum())).TotalHours;
                courseReport.StoppedHours = TimeSpan.FromSeconds(Convert.ToDouble(course.Value.Select(x => x.Events.Select(e => e.Duration).Sum()).Sum())).TotalHours;
                courseReport.MissedHours = course.Value.GroupBy(x => x.Class.Id).ToDictionary(g => g.Key, g => g.ToList()).Values.Select(x => x.First().Class.ElapsedHours).Sum() - course.Value.Where(x => x.EndDateTime <= DateTime.Now.Date.AddDays(1)).Select(x => (x.EndDateTime - x.StartDateTime).TotalHours).Sum();
                courseReport.ScheduledClasses = course.Value.Count();
                courseReport.CompletedClasses = course.Value.Where(x => x.EndDateTime < DateTime.Now).Count();
                courseReport.MissedClasses = course.Value.GroupBy(x => x.Class).ToDictionary(g => g.Key, g => g.ToList()).Select(x => x.Key.Sessions.Where(x => x.EndDateTime <= DateTime.Now.Date.AddDays(1)).Count()).Sum() - course.Value.Where(x => x.EndDateTime <= DateTime.Now.Date.AddDays(1)).Count();
                courseReports.Add(courseReport);

            }

            return new Report(user.Type, user.Name, curriculumReports, courseReports);
        }
      
        [HttpGet("instructorReport/{id}")]
        public async Task<ActionResult<object>> GetUserInstructorReport(long id)
        {
            return await _reportService.GetInstructorReport(id);
        }

        [HttpGet("themeReport/{theme}")]
        public async Task<ActionResult<object>> GetUserInstructorReport(string theme)
        {
            return await _reportService.GetThemeReport(theme);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            await _services.UpdateUser(id, user);
            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            await _services.AddUser(user);
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            await _services.DeleteUser(id);
            return NoContent();
        }


    }
}
