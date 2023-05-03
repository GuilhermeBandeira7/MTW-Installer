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
    public class RestrictedPlatesController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public RestrictedPlatesController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/RestrictedPlates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestrictedPlate>>> GetRestrictedPlates()
        {
            return await _context.RestrictedPlates.AsNoTracking().Include(x => x.VehicleModel).ToListAsync();
        }

        // GET: api/RestrictedPlates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RestrictedPlate>> GetRestrictedPlate(long id)
        {
            var restrictedPlate = await _context.RestrictedPlates.AsNoTracking().Include(x => x.VehicleModel).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (restrictedPlate == null)
            {
                return NotFound();
            }

            return restrictedPlate;
        }

        // PUT: api/RestrictedPlates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRestrictedPlate(long id, RestrictedPlate restrictedPlate)
        {
            if (id != restrictedPlate.Id)
            {
                return BadRequest();
            }

            RestrictedPlate restrictedPlateToUpdate = await _context.RestrictedPlates.Include(x => x.VehicleModel).Where(x => x.Id == id).FirstOrDefaultAsync();
            restrictedPlateToUpdate.Plate = restrictedPlate.Plate;
            restrictedPlateToUpdate.Description = restrictedPlate.Description;
            restrictedPlateToUpdate.EndValidDate = restrictedPlate.EndValidDate;
            restrictedPlateToUpdate.StartValidDate = restrictedPlate.StartValidDate;

            if (restrictedPlate.VehicleModel != null)
                restrictedPlateToUpdate.VehicleModel = await _context.VehiclesModels.Where(x => x.Id == restrictedPlate.VehicleModel.Id).FirstOrDefaultAsync();
            else
                restrictedPlateToUpdate.VehicleModel = null;

            _context.Entry(restrictedPlateToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestrictedPlateExists(id))
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

        // POST: api/RestrictedPlates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RestrictedPlate>> PostRestrictedPlate(RestrictedPlate restrictedPlate)
        {


            if (restrictedPlate.VehicleModel != null)
                restrictedPlate.VehicleModel = await _context.VehiclesModels.Where(x => x.Id == restrictedPlate.VehicleModel.Id).FirstOrDefaultAsync();



            _context.RestrictedPlates.Add(restrictedPlate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRestrictedPlate", new { id = restrictedPlate.Id }, restrictedPlate);
        }

        // DELETE: api/RestrictedPlates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestrictedPlate(long id)
        {
            var restrictedPlate = await _context.RestrictedPlates.FindAsync(id);
            if (restrictedPlate == null)
            {
                return NotFound();
            }

            _context.RestrictedPlates.Remove(restrictedPlate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RestrictedPlateExists(long id)
        {
            return _context.RestrictedPlates.Any(e => e.Id == id);
        }
    }
}
