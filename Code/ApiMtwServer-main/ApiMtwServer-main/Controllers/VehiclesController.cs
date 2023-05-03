#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityMtwServer;
using EntityMtwServer.Entities;

namespace MTWServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public VehiclesController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/Actions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> Get()
        {
            return await _context.Vehicles.AsNoTracking().Include(x => x.VehicleModel).ToListAsync();
        }

        // GET: api/Actions/5
        [HttpGet("{plate}")]
        public async Task<ActionResult<Vehicle>> Get(string plate)
        {
            var action = await _context.Vehicles.AsNoTracking().Include(x => x.VehicleModel).Where(x => x.Plate == plate).FirstOrDefaultAsync();

            if (action == null)
            {
                return NotFound();
            }

            return action;
        }

        // PUT: api/Actions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{plate}")]
        public async Task<IActionResult> Put(string plate, Vehicle vehicle)
        {
            if (plate != vehicle.Plate)
            {
                return BadRequest();
            }

            Vehicle vehicleToUpdate = await _context.Vehicles.Include(x => x.VehicleModel).Where(x => x.Plate == plate).FirstOrDefaultAsync();
            vehicleToUpdate.Plate = plate;
            if (vehicle.VehicleModel != null)
                vehicleToUpdate.VehicleModel = await _context.VehiclesModels.Where(x => x.Id == vehicle.VehicleModel.Id).FirstOrDefaultAsync();
            else
                vehicleToUpdate.VehicleModel = null;

            _context.Entry(vehicleToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(plate))
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

        // POST: api/Actions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Vehicle>> Post(Vehicle vehicle)
        {
            if (vehicle.VehicleModel != null)
                vehicle.VehicleModel = await _context.VehiclesModels.Where(x => x.Id == vehicle.VehicleModel.Id).FirstOrDefaultAsync();
            else
                vehicle.VehicleModel = null;

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { plate = vehicle.Plate }, vehicle);
        }

        // DELETE: api/Actions/5
        [HttpDelete("{plate}")]
        public async Task<IActionResult> Delete(string plate)
        {
            var vehicle = await _context.Vehicles.FindAsync(plate);
            if (vehicle == null)
            {
                return NotFound();
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleExists(string plate)
        {
            return _context.Vehicles.Any(e => e.Plate == plate);
        }
    }
}
