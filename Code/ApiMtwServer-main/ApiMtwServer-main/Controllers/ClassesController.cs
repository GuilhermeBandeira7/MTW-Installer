#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityMtwServer;
using EntityMtwServer.Entities;
using System.Collections.Generic;

namespace MTWServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public ClassesController(MasterServerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> Get()
        {
            return await _context.Classes.AsNoTracking()
                .Include(x => x.Course)
                .Include(x => x.CurriculumCourse)
                .Include(x => x.InstructorDevice)
                .Include(x => x.Students)
                .Include(x => x.Instructor)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> Get(long id)
        {
            var classroom = await _context.Classes.AsNoTracking()
                .Include(x => x.Course)
                .Include(x => x.CurriculumCourse)
                .Include(x => x.InstructorDevice)
                .Include(x => x.Instructor)
                .Include(x => x.Instructor.Equipments)
                .Include(x => x.Period)
                .Include(x => x.Students)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (classroom.CurriculumCourse != null)
                classroom.CurriculumCourse = await _context.CurriculumCourses.AsNoTracking().Include(x => x.Courses).Where(x => x.Id == classroom.CurriculumCourse.Id).FirstOrDefaultAsync();

            if (classroom == null)
            {
                return NotFound();
            }

            return classroom;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Class>> Put(long id, Class @class)
        {
            try
            {
                if (id != @class.Id)
                    return BadRequest();

                if (await CheckForConflict(@class))
                    return ValidationProblem();

                foreach (Student std in @class.Students)
                    std.User = await _context.Users.Include(x => x.Equipments).Where(x => x.Id == std.User.Id).FirstOrDefaultAsync();

                _context.Entry(@class).State = EntityState.Detached;
                Class dbClass = await _context.Classes.Where(x => x.Id == @class.Id).Include(x => x.Sessions).Include(x => x.Period).Include(x => x.Students).FirstOrDefaultAsync();


                if (@class.StartDateTime < DateTime.Now)
                {
                    _context.Entry(dbClass).State = EntityState.Detached;
                    bool instructorChange = dbClass.Instructor != @class.Instructor;
                    bool instructorDeviceChange = dbClass.InstructorDevice != @class.InstructorDevice;

                    foreach (Session s in _context.Sessions.Include(x => x.Instructor).Include(x => x.InstructorDevice).Include(x => x.Students).Include(x => x.Cells).Where(x => @class.Sessions.Select(s => s.Id).Contains(x.Id)))
                    {
                        if (s.StartDateTime > DateTime.Now)
                        {
                            s.Instructor = instructorChange ? await _context.Users.Where(x => x.Id == @class.Instructor.Id).FirstOrDefaultAsync() : s.Instructor;
                            s.InstructorDevice = instructorDeviceChange ? await _context.DVCs.Where(x => x.Id == @class.InstructorDevice.Id).FirstOrDefaultAsync() : s.InstructorDevice;

                            foreach (Student student in @class.Students)
                            {
                                if (!dbClass.Students.Select(x => x.Id).Contains(student.Id))
                                {
                                    student.User = await _context.Users.Include(x => x.Equipments).Where(x => x.Id == student.User.Id).FirstOrDefaultAsync();
                                    s.Students.Add(student);
                                    if (!s.Equipments.Select(x => x.Id).Contains(student.User.Equipments.First().Id))
                                        s.Equipments.Add(student.User.Equipments.First());
                                }
                            }

                            List<Group> cellsGroups = _context.Users.Where(x => @class.Students.Select(s => s.Id).Contains(x.Id)).Select(x => x.Groups.FirstOrDefault()).ToList();
                            List<User> users = _context.Users.Where(x => @class.Students.Select(s => s.User.Id).Contains(x.Id)).Include(x => x.Groups).ToList();

                            foreach (User u in users)
                            {
                                foreach (Group g in u.Groups)
                                    if (!cellsGroups.Select(x => x.Id).Contains(g.Id))
                                        cellsGroups.Add(g);
                            }

                            s.Cells = cellsGroups;
                            _context.Entry(s).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    await RemoveSessions(dbClass);
                    @class.Sessions.Clear();
                    AddSessions(@class);

                    foreach (Session s in @class.Sessions)
                    {
                        _context.Entry(s).State = EntityState.Added;
                        await _context.SaveChangesAsync();
                    }
                }

                _context.Entry(dbClass).State = EntityState.Detached;

                @class.Period = await _context.Schedules.Where(x => x.Id == @class.Period.Id).FirstOrDefaultAsync();
                @class.Instructor = await _context.Users.Where(x => x.Id == @class.Instructor.Id).FirstOrDefaultAsync();
                @class.InstructorDevice = await _context.DVCs.Where(x => x.Id == @class.InstructorDevice.Id).FirstOrDefaultAsync();
                if (@class.Course != null)
                    @class.Course = await _context.Courses.Where(x => x.Id == @class.Course.Id).FirstOrDefaultAsync();
                if (@class.CurriculumCourse != null)
                    @class.CurriculumCourse = await _context.CurriculumCourses.Where(x => x.Id == @class.CurriculumCourse.Id).FirstOrDefaultAsync();

                _context.Entry(@class).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return await Get(@class.Id);



            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Class>> Post(Class @class)
        {

            if (await CheckForConflict(@class))
                return ValidationProblem();

            foreach (Student std in @class.Students)
                std.User = await _context.Users.Include(x => x.Equipments).Where(x => x.Id == std.User.Id).FirstOrDefaultAsync();

            @class.Period = await _context.Schedules.Where(x => x.Id == @class.Period.Id).FirstOrDefaultAsync();
            @class.Instructor = await _context.Users.Where(x => x.Id == @class.Instructor.Id).FirstOrDefaultAsync();
            @class.InstructorDevice = await _context.DVCs.Where(x => x.Id == @class.InstructorDevice.Id).FirstOrDefaultAsync();
            if (@class.Course != null)
                @class.Course = await _context.Courses.Where(x => x.Id == @class.Course.Id).FirstOrDefaultAsync();
            if (@class.CurriculumCourse != null)
                @class.CurriculumCourse = await _context.CurriculumCourses.Include(x => x.Courses).Where(x => x.Id == @class.CurriculumCourse.Id).FirstOrDefaultAsync();
            AddSessions(@class);



            _context.Classes.Add(@class);
            await _context.SaveChangesAsync();

            return await Get(@class.Id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {

            Class @class = await _context.Classes.Include(x => x.Sessions).Include(x => x.Students).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (@class.StartDateTime > DateTime.Now)
            {
                foreach (Session s in await _context.Sessions.Include(x => x.Equipments).Include(x => x.Cells).Include(x => x.Students).Where(x => @class.Sessions.Select(s => s.Id).Contains(x.Id)).ToListAsync())
                {
                    s.Equipments.Clear();
                    s.Cells.Clear();
                    s.Students.Clear();
                    _context.Entry(s).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _context.Entry(s).State = EntityState.Detached;
                }

                _context.Sessions.RemoveRange(@class.Sessions);
                _context.Students.RemoveRange(@class.Students);

                if (@class == null)
                {
                    return NotFound();
                }

                _context.Classes.Remove(@class);
                await _context.SaveChangesAsync();
            }
            else
            {
                @class.State = false;

                for (int counter = 0; counter < @class.Sessions.Count; counter++)
                {
                    Session s = await _context.Sessions.Include(x => x.Equipments).Include(x => x.Cells).Where(x => x.Id == @class.Sessions[counter].Id).FirstOrDefaultAsync();
                    if (s.StartDateTime > DateTime.Now)
                    {
                        s.Equipments.Clear();
                        s.Cells.Clear();
                        _context.Entry(s).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        _context.Entry(s).State = EntityState.Detached;

                        _context.Sessions.Remove(s);
                        @class.Sessions.Remove(s);
                        counter--;

                    }

                }

                _context.Entry(@class).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            return NoContent();

        }

        private void AddSessions(Class @class)
        {
            DateTime classDateTime = @class.StartDateTime;


            while (classDateTime < @class.EndDateTime)
            {
                switch (classDateTime.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        if (@class.Period.Monday)
                            AddSessionToClass(@class, classDateTime, "MondayStart", "MondayEnd");
                        break;
                    case DayOfWeek.Tuesday:
                        if (@class.Period.Tuesday)
                            AddSessionToClass(@class, classDateTime, "TuesdayStart", "TuesdayEnd");
                        break;
                    case DayOfWeek.Wednesday:
                        if (@class.Period.Wednesday)
                            AddSessionToClass(@class, classDateTime, "WednesdayStart", "WednesdayEnd");
                        break;
                    case DayOfWeek.Thursday:
                        if (@class.Period.Thursday)
                            AddSessionToClass(@class, classDateTime, "ThursdayStart", "ThursdayEnd");
                        break;
                    case DayOfWeek.Friday:
                        if (@class.Period.Friday)
                            AddSessionToClass(@class, classDateTime, "FridayStart", "FridayEnd");
                        break;
                    case DayOfWeek.Saturday:
                        if (@class.Period.Saturday)
                            AddSessionToClass(@class, classDateTime, "SaturdayStart", "SaturdayEnd");
                        break;
                    case DayOfWeek.Sunday:
                        if (@class.Period.Sunday)
                            AddSessionToClass(@class, classDateTime, "SundayStart", "SundayEnd");
                        break;
                }

                classDateTime = classDateTime.AddDays(1);

            }
        }

        private async Task<bool> RemoveSessions(Class @class)
        {
            foreach (Session s in await _context.Sessions.Include(x => x.Equipments).Include(x => x.Cells).Where(x => @class.Sessions.Select(s => s.Id).Contains(x.Id)).ToListAsync())
            {
                s.Equipments.Clear();
                s.Cells.Clear();
                _context.Entry(s).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _context.Entry(s).State = EntityState.Detached;
            }

            _context.Sessions.RemoveRange(@class.Sessions);
            @class.Sessions.Clear();
            _context.Entry(@class).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        private Session AddSessionToClass(Class @class, DateTime dateTime, string StartWeekDay, string EndWeekDay)
        {
            Session session = new Session();
            session.Class = @class;
            foreach (Student student in @class.Students)
            {
                session.Equipments.Add(student.User.Equipments.First());
                session.Students.Add(student);
            }

            session.Name = @class.Course + "_" + @class.Theme + "_" + @class.Instructor.Name;
            session.StartDateTime = dateTime.Date.Add((TimeSpan)typeof(Schedule).GetProperty(StartWeekDay).GetValue(@class.Period));
            session.EndDateTime = dateTime.Date.Add((TimeSpan)typeof(Schedule).GetProperty(EndWeekDay).GetValue(@class.Period));
            session.Record = !@class.Period.MondayRemote;
            session.Instructor = @class.Instructor;
            session.InstructorDevice = @class.InstructorDevice;
            session.Live = !@class.Period.MondayRemote;
            session.Color = @class.Color;
          //  session.Type = @class.Type;

            List<Group> cellsGroups = _context.Users.Where(x => @class.Students.Select(s => s.Id).Contains(x.Id)).Select(x => x.Groups.FirstOrDefault()).ToList();
            List<Cell> cells = new List<Cell>();

            List<User> users = _context.Users.Where(x => @class.Students.Select(s => s.User.Id).Contains(x.Id)).Include(x => x.Groups).ToList();

            foreach (User u in users)
            {
                foreach (Group g in u.Groups)
                    if (!cellsGroups.Select(x => x.Id).Contains(g.Id))
                        cellsGroups.Add(g);
            }

            session.Cells = cellsGroups;

            @class.Sessions.Add(session);

            return session;
        }

        private async Task<bool> CheckForConflict(Class @class)
        {
            List<User> users = await _context.Users.AsNoTracking().Include(x => x.Equipments).Where(x => @class.Students.Select(u => u.User.Id).Contains(x.Id)).ToListAsync();
            List<DVC> dvcs = await _context.DVCs.Where(x => users.Select(u => u.Equipments.First().Id).Contains(x.Id)).ToListAsync();
            List<Session> sessions = await _context.Sessions.Include(x => x.Instructor).Include(x => x.InstructorDevice).Include(x => x.Equipments).
                Where(x => x.Class.State).
                Where(x => x.Equipments.Where(m => dvcs.Select(d => d.Id).Contains(m.Id)).Count() > 0 || x.Instructor.Id == @class.Instructor.Id || x.InstructorDevice.Id == @class.InstructorDevice.Id).
                Where(x => x.StartDateTime >= @class.StartDateTime && x.EndDateTime <= @class.EndDateTime).
                ToListAsync();


            List<Session> conflictingSessions = new List<Session>();
            foreach (Session s in sessions)
            {
                switch (s.StartDateTime.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        if (!(DateTime.Parse(@class.Period.MondayEndTime).TimeOfDay < s.StartDateTime.TimeOfDay || DateTime.Parse(@class.Period.MondayStartTime).TimeOfDay > s.EndDateTime.TimeOfDay))
                            if (!@class.Sessions.Select(x => x.Id).Contains(s.Id))
                                conflictingSessions.Add(s);
                        break;
                    case DayOfWeek.Tuesday:
                        if (!(DateTime.Parse(@class.Period.TuesdayEndTime).TimeOfDay < s.StartDateTime.TimeOfDay || DateTime.Parse(@class.Period.TuesdayStartTime).TimeOfDay > s.EndDateTime.TimeOfDay))
                            if (!@class.Sessions.Select(x => x.Id).Contains(s.Id))
                                conflictingSessions.Add(s);
                        break;
                    case DayOfWeek.Wednesday:
                        if (!(DateTime.Parse(@class.Period.WednesdayEndTime).TimeOfDay < s.StartDateTime.TimeOfDay || DateTime.Parse(@class.Period.WednesdayStartTime).TimeOfDay > s.EndDateTime.TimeOfDay))
                                if (!@class.Sessions.Select(x => x.Id).Contains(s.Id))
                                    conflictingSessions.Add(s);
                        break;
                    case DayOfWeek.Thursday:
                        if (!(DateTime.Parse(@class.Period.ThursdayEndTime).TimeOfDay < s.StartDateTime.TimeOfDay || DateTime.Parse(@class.Period.ThursdayStartTime).TimeOfDay > s.EndDateTime.TimeOfDay))
                                if (!@class.Sessions.Select(x => x.Id).Contains(s.Id))
                                    conflictingSessions.Add(s);
                        break;
                    case DayOfWeek.Friday:
                        if (!(DateTime.Parse(@class.Period.FridayEndTime).TimeOfDay < s.StartDateTime.TimeOfDay || DateTime.Parse(@class.Period.FridayStartTime).TimeOfDay > s.EndDateTime.TimeOfDay))
                                if (!@class.Sessions.Select(x => x.Id).Contains(s.Id))
                                    conflictingSessions.Add(s);
                        break;
                    case DayOfWeek.Saturday:
                        if (!(DateTime.Parse(@class.Period.SaturdayEndTime).TimeOfDay < s.StartDateTime.TimeOfDay || DateTime.Parse(@class.Period.SaturdayStartTime).TimeOfDay > s.EndDateTime.TimeOfDay))
                                if (!@class.Sessions.Select(x => x.Id).Contains(s.Id))
                                    conflictingSessions.Add(s);
                        break;
                    case DayOfWeek.Sunday:
                        if (!(DateTime.Parse(@class.Period.SundayEndTime).TimeOfDay < s.StartDateTime.TimeOfDay || DateTime.Parse(@class.Period.SundayStartTime).TimeOfDay > s.EndDateTime.TimeOfDay))
                                if (!@class.Sessions.Select(x => x.Id).Contains(s.Id))
                                    conflictingSessions.Add(s);
                        break;
                }

            }

            return conflictingSessions.Count() > 0;

        }
    }
}
