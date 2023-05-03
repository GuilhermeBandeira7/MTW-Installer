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
    public class VisitorsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public VisitorsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/Visitors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Visitor>>> GetVisitors()
        {
            return await _context.Visitors.AsNoTracking().ToListAsync();
        }

        // GET: api/Visitors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Visitor>> GetVisitor(long id)
        {
            var visitor = await _context.Visitors.AsNoTracking().Include(x => x.Period).Include(x => x.VehicleModel).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (visitor == null)
            {
                return NotFound();
            }

            return visitor;
        }

        // PUT: api/Visitors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVisitor(long id, Visitor visitor)
        {
            if (id != visitor.Id)
            {
                return BadRequest();
            }

            Visitor visitorToUpdate = await _context.Visitors.Include(x => x.VehicleModel).Include(x => x.Period).Where(y => y.Id == id).FirstOrDefaultAsync();
            visitorToUpdate.Plate = visitor.Plate;
            visitorToUpdate.Title = visitor.Title;
            visitorToUpdate.AuthorizationDate = visitor.AuthorizationDate;
            visitorToUpdate.Name = visitor.Name;

            if(visitor.VehicleModel != null)
                visitorToUpdate.VehicleModel = await _context.VehiclesModels.Where(x => x.Id == visitor.VehicleModel.Id).FirstOrDefaultAsync();
            else
                visitorToUpdate.VehicleModel = null;

            if(visitor.Period != null)
                visitorToUpdate.Period = await _context.Periods.Where(x => x.Id == visitor.Period.Id).FirstOrDefaultAsync();
            else
                visitorToUpdate.Period = null;


            _context.Entry(visitorToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitorExists(id))
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

        // POST: api/Visitors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Visitor>> PostVisitor(Visitor visitor)
        {
            if (visitor.Period != null)
                visitor.Period = await _context.Periods.Where(x => x.Id == visitor.Period.Id).FirstOrDefaultAsync();

            if (visitor.VehicleModel != null)
                visitor.VehicleModel = await _context.VehiclesModels.Where(x => x.Id == visitor.VehicleModel.Id).FirstOrDefaultAsync();


            _context.Visitors.Add(visitor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVisitor", new { id = visitor.Id }, visitor);
        }

        // DELETE: api/Visitors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisitor(long id)
        {
            var visitor = await _context.Visitors.FindAsync(id);
            if (visitor == null)
            {
                return NotFound();
            }

            _context.Visitors.Remove(visitor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VisitorExists(long id)
        {
            return _context.Visitors.Any(e => e.Id == id);
        }
    }
}
