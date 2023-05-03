using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityMtwServer;
using EntityMtwServer.Entities;

namespace MTWServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public StudentsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            if (_context.Students == null)
            {
                return NotFound();
            }

            List<Student> students = await _context.Students.AsNoTracking().Include(x => x.User).Include(x => x.AttendedClasses).ToListAsync();

            foreach (Student student in students)
            {
                student.LiveHours = student.AttendedClasses.Where(x => x.Live).Select(x => (x.EndDateTime - x.StartDateTime).TotalHours).Sum();
                student.RemoteHours = student.AttendedClasses.Where(x => !x.Live).Select(x => (x.EndDateTime - x.StartDateTime).TotalHours).Sum();
            }

            return students;
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(long id)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            var student = await _context.Students.AsNoTracking().Include(x => x.User).Include(x => x.AttendedClasses).Where(x => x.Id == id).FirstOrDefaultAsync();
            student.LiveHours = student.AttendedClasses.Where(x => x.Live).Select(x => (x.EndDateTime - x.StartDateTime).TotalHours).Sum();
            student.RemoteHours = student.AttendedClasses.Where(x => !x.Live).Select(x => (x.EndDateTime - x.StartDateTime).TotalHours).Sum();

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        /*
        [HttpGet("{id/report}")]
        public async Task<ActionResult<object>> GetReport(long id)
        {
            object report = new object
            {

            };
            if (_context.Students == null)
            {
                return NotFound();
            }
            var student = await _context.Students.AsNoTracking().Include(x => x.User).Include(x => x.AttendedClasses).Where(x => x.Id == id).FirstOrDefaultAsync();
            student.LiveHours = student.AttendedClasses.Where(x => x.Live).Select(x => (x.EndDateTime - x.StartDateTime).TotalHours).Sum();
            student.RemoteHours = student.AttendedClasses.Where(x => !x.Live).Select(x => (x.EndDateTime - x.StartDateTime).TotalHours).Sum();

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }*/



        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(long id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'MasterServerContext.Students'  is null.");
            }
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(long id)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            Student? student = await _context.Students.Include(x => x.User).Where(x => x.Id == id).FirstOrDefaultAsync();
            Group? studentGroupCell = await _context.Groups.Where(x => x.Users.Select(u => u.Id).Contains(student.User.Id)).FirstOrDefaultAsync();
            Equipment? studentEquipment = await _context.Equipments.Where(x => x.Users.Select(u => u.Id).Contains(student.User.Id)).FirstOrDefaultAsync();
            if (student == null)
            {
                return NotFound();
            }

            foreach (Session s in _context.Sessions.Include(x => x.Students).Include(x => x.Cells).Include(x => x.Equipments).Where(x => x.Students.Select(s => s.Id).Contains(id)).ToList())
            {
                bool sameGroup = false;
                if (s.Students != null)
                {
                    foreach (Student sessionStudent in await _context.Students.Include(x => x.User).Where(x => s.Students.Select(s => s.Id).Contains(x.Id)).ToListAsync())
                    {
                        Group sessionStudentGroupCell = await _context.Groups.Where(x => x.Users.Select(u => u.Id).Contains(sessionStudent.User.Id)).FirstOrDefaultAsync();
                        if (sessionStudentGroupCell.Id == studentGroupCell.Id && sessionStudent.Id != student.Id)
                            sameGroup = true;
                    }
                }

                if (!sameGroup)
                {
                    s.Cells.Remove(studentGroupCell);
                    s.Equipments.Remove(studentEquipment);
                    s.Students.Remove(student);
                    _context.Entry(s).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(long id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
