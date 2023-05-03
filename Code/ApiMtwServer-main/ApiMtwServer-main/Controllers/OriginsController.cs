#nullable disable
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
    public class OriginsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public OriginsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/Origins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Origin>>> GetOrigins()
        {
            return await _context.Origins.AsNoTracking().ToListAsync();
        }

        // GET: api/Origins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Origin>> GetOrigin(long id)
        {
            var origin = await _context.Origins.AsNoTracking().Include(x => x.Telemetry).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (origin == null)
            {
                return NotFound();
            }

            return origin;
        }

        // PUT: api/Origins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrigin(long id, Origin origin)
        {
            Origin originModel = await _context.Origins.Include(x => x.Telemetry).Where(x => x.Id == id).FirstOrDefaultAsync();
            if(origin.Telemetry != null)
                originModel.Telemetry = _context.Telemetries.Where(x => x.Id == origin.Telemetry.Id).FirstOrDefault();
            else 
                originModel.Telemetry  = null;

            originModel.OriginCode = origin.OriginCode;
            originModel.Title = origin.Title;
            originModel.Type = origin.Type;

            if (id != origin.Id)
            {
                return BadRequest();
            }

            _context.Entry(originModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OriginExists(id))
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

        // POST: api/Origins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Origin>> PostOrigin(Origin origin)
        {
            if(origin.Telemetry != null)
            {
                Telemetry telemetry = await _context.Telemetries.Where(x => x.Id == origin.Telemetry.Id).FirstOrDefaultAsync();
                origin.Telemetry = telemetry;
            }
            else
                origin.Telemetry = null;
            _context.Origins.Add(origin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrigin", new { id = origin.Id }, origin);
        }

        // DELETE: api/Origins/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrigin(long id)
        {
            var origin = await _context.Origins.FindAsync(id);
            if (origin == null)
            {
                return NotFound();
            }

            _context.Origins.Remove(origin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OriginExists(long id)
        {
            return _context.Origins.Any(e => e.Id == id);
        }
    }
}
