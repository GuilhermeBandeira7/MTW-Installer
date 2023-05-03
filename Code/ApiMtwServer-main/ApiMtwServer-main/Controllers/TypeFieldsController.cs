using EntityMtwServer;
using EntityMtwServer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MTWServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeFieldsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public TypeFieldsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/<TypeFieldsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeField>>> Get()
        {
            return await _context.TypeFields.ToListAsync();
        }

        // GET api/<TypeFieldsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeField>> Get(long id)
        {
            var typeField = await _context.TypeFields.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (typeField == null)
            {
                return NotFound();
            }

            return typeField;
        }

        // POST api/<TypeFieldsController>
        [HttpPost]
        public async Task<ActionResult<TypeField>> Post(TypeField typeField)
        {
            _context.TypeFields.Add(typeField);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = typeField.Id }, typeField);
        }

        // PUT api/<TypeFieldsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TypeField>> Put(long id, TypeField typeField)
        {
            if (id != typeField.Id)
            {
                return BadRequest();
            }

            _context.Entry(typeField).State = EntityState.Modified;

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
        public async Task<ActionResult<TypeField>> Delete(long id)
        {
            var typeField = await _context.TypeFields.FindAsync(id);
            if (typeField == null)
            {
                return NotFound();
            }

            _context.TypeFields.Remove(typeField);
            await _context.SaveChangesAsync();

            return typeField;
        }
    }
}
