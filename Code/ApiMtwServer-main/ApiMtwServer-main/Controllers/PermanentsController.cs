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
    public class PermanentsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public PermanentsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/Permanents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permanent>>> GetPermanents()
        {
            return await _context.Permanents.AsNoTracking().ToListAsync();
        }

        // GET: api/Permanents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Permanent>> GetPermanent(long id)
        {
            var permanent = await _context.Permanents.AsNoTracking().Include(x => x.Schedule).Include(x => x.VehicleModel).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (permanent == null)
            {
                return NotFound();
            }

            return permanent;
        }

        // PUT: api/Permanents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermanent(long id, Permanent permanent)
        {
            if (id != permanent.Id)
            {
                return BadRequest();
            }

            Permanent permanentToUpdate = await _context.Permanents.Include(x => x.VehicleModel).Include(x => x.Schedule).Where(x => x.Id == id).FirstOrDefaultAsync();
            permanentToUpdate.Plate =  permanent.Plate;
            permanentToUpdate.Title = permanent.Title;
            permanentToUpdate.JobTitle = permanent.JobTitle;
            permanentToUpdate.Registration = permanent.Registration;
            permanentToUpdate.StartValidDate = permanent.StartValidDate;
            permanentToUpdate.EndValidDate = permanent.EndValidDate;

            if (permanent.VehicleModel != null)
                permanentToUpdate.VehicleModel = await _context.VehiclesModels.Where(x => x.Id == permanent.VehicleModel.Id).FirstOrDefaultAsync();
            else
                permanentToUpdate.VehicleModel = null;

            if (permanent.Schedule != null)
                permanentToUpdate.Schedule = await _context.Schedules.Where(x => x.Id == permanent.Schedule.Id).FirstOrDefaultAsync();

            _context.Entry(permanentToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermanentExists(id))
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

        // POST: api/Permanents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Permanent>> PostPermanent(Permanent permanent)
        {
            if (permanent.VehicleModel != null)
                permanent.VehicleModel = await _context.VehiclesModels.Where(x => x.Id == permanent.VehicleModel.Id).FirstOrDefaultAsync();

            if (permanent.Schedule != null)
                permanent.Schedule = await _context.Schedules.Where(x =>x.Id == permanent.Schedule.Id).FirstOrDefaultAsync();

            _context.Permanents.Add(permanent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPermanent", new { id = permanent.Id }, permanent);
        }

        // DELETE: api/Permanents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermanent(long id)
        {
            var permanent = await _context.Permanents.FindAsync(id);
            if (permanent == null)
            {
                return NotFound();
            }

            _context.Permanents.Remove(permanent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PermanentExists(long id)
        {
            return _context.Permanents.Any(e => e.Id == id);
        }
    }
}
