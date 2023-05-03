using EntityMtwServer;
using EntityMtwServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MTWServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumCoursesController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public CurriculumCoursesController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/<TypeFieldsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurriculumCourse>>> Get()
        {
            return await _context.CurriculumCourses.Include(x => x.Courses).ToListAsync();
        }

        // GET api/<TypeFieldsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CurriculumCourse>> Get(long id)
        {
            var course = await _context.CurriculumCourses.Where(x => x.Id == id).Include(x => x.Courses).FirstOrDefaultAsync();

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // POST api/<TypeFieldsController>
        [HttpPost]
        public async Task<ActionResult<CurriculumCourse>> Post(CurriculumCourse course)
        {
            course.Courses = course.Courses.Where(x => x.Id != 0).ToList();
            course.Courses = await _context.Courses.Where(x => course.Courses.Select(c => c.Id).Contains(x.Id)).ToListAsync();
            _context.CurriculumCourses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = course.Id }, course);
        }

        // PUT api/<TypeFieldsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CurriculumCourse>> Put(long id, CurriculumCourse course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            List<Class> classes = await _context.Classes.Where(x => x.CurriculumCourse.Id == id).ToListAsync();
            if (classes.Count > 0)
                return Problem("Not possible to update ongoing course", null, 403, "Not allowed");

            CurriculumCourse dbCourse = await _context.CurriculumCourses.Include(x => x.Courses).Where(x => x.Id == id).FirstOrDefaultAsync();
            dbCourse.Courses = await _context.Courses.Where(x => course.Courses.Select(c => c.Id).Contains(x.Id)).ToListAsync();
            dbCourse.Name = course.Name;
            dbCourse.Type = course.Type;

            _context.Entry(dbCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE api/<TypeFieldsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CurriculumCourse>> Delete(long id)
        {
            var course = await _context.CurriculumCourses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            List<Class> classes = await _context.Classes.Where(x => x.CurriculumCourse.Id == id).ToListAsync();
            if (classes.Count > 0)
                return Problem("Not possible to update ongoing course", null, 403, "Not allowed");

            _context.CurriculumCourses.Remove(course);
            await _context.SaveChangesAsync();

            return course;
        }
    }
}
