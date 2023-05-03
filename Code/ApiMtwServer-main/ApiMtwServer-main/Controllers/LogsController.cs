using EntityMtwServer;
using EntityMtwServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MTWServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public LogsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/<TypeFieldsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Log>>> Get()
        {
            return await _context.Logs.AsNoTracking().Include(x => x.User).ToListAsync();
        }

        // GET api/<TypeFieldsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Log>> Get(long id)
        {
            var log = await _context.Logs.AsNoTracking().Where(x => x.Id == id).Include(x => x.User).FirstOrDefaultAsync();

            if (log == null)
            {
                return NotFound();
            }

            return log;
        }

        // POST api/<TypeFieldsController>
        [HttpPost]
        public async Task<ActionResult<Log>> Post(Log log)
        {
            log.User = await _context.Users.Where(x => x.Id == log.User.Id).FirstOrDefaultAsync();
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = log.Id }, log);
        }

        // PUT api/<TypeFieldsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Log>> Put(long id, Log log)
        {
            if (id != log.Id)
            {
                return BadRequest();
            }

            _context.Entry(log).State = EntityState.Modified;

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
        public async Task<ActionResult<Log>> Delete(long id)
        {
            var log = await _context.Logs.FindAsync(id);
            if (log == null)
            {
                return NotFound();
            }

            _context.Logs.Remove(log);
            await _context.SaveChangesAsync();

            return log;
        }
    }
}





