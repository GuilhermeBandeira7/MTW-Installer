using EntityMtwServer;
using EntityMtwServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MTWServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public CoursesController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/<TypeFieldsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> Get()
        {
            return await _context.Courses.ToListAsync();
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetSingle()
        {
            return await _context.Courses.FromSqlRaw("select * from dbo.Courses where CurriculumCourseId IS NULL").ToListAsync();
        }

        // GET api/<TypeFieldsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> Get(long id)
        {
            var course = await _context.Courses.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // POST api/<TypeFieldsController>
        [HttpPost]
        public async Task<ActionResult<Course>> Post(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = course.Id }, course);
        }

        // PUT api/<TypeFieldsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Course>> Put(long id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }


            List<Class> classes = await _context.Classes.Where(x => x.CurriculumCourse.Courses.Select(c => c.Id).Contains(id) || x.Course.Id == id).ToListAsync();
            if (classes.Count > 0)
                return Problem("Not possible to update ongoing course", null, 403, "Not allowed");


            _context.Entry(course).State = EntityState.Modified;

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
        public async Task<ActionResult<Course>> Delete(long id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            List<Class> classes = await _context.Classes.Where(x => x.CurriculumCourse.Courses.Select(c => c.Id).Contains(id) || x.Course.Id == id).ToListAsync();
            if (classes.Count > 0)
                return Problem("Not possible to delete ongoing course", null, 403, "Not allowed");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return course;
        }
    }
}
