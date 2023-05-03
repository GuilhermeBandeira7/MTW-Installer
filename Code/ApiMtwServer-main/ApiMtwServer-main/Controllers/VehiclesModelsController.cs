#nullable disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityMtwServer;
using EntityMtwServer.Entities;

namespace MTWServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesModelsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public VehiclesModelsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/Actions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleModel>>> Get()
        {
            return await _context.VehiclesModels.AsNoTracking().OrderBy(x => x.Model).ToListAsync();
        }

        [HttpGet("subModels")]
        public async Task<ActionResult<IEnumerable<string>>> GetSubModels()
        {
            List<string> subModels = await _context.VehiclesModels.AsNoTracking().Select(x => x.SubModel).Distinct().Where(x => x != string.Empty).ToListAsync();
            List<string> filteredSubModels = new List<string>();
            foreach(string subModel in subModels)
                filteredSubModels.Add(subModel.Split('?').Last());
            return filteredSubModels.OrderBy(x => x).Distinct().ToList();
        }

        [HttpGet("{brand}/subModels")]
        public async Task<ActionResult<IEnumerable<string>>> GetSubModelsByBrand(string brand)
        {
            List<string> subModels = await _context.VehiclesModels.AsNoTracking().Where(x => x.Brand == brand).Select(x => x.SubModel).Distinct().Where(x => x != string.Empty).ToListAsync();
            List<string> filteredSubModels = new List<string>();
            foreach (string subModel in subModels)
                filteredSubModels.Add(subModel.Split('?').Last());
            return filteredSubModels.OrderBy(x => x).Distinct().ToList();
        }


        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<string>>> GetBrands()
        {
            
            List<string> brands = await _context.VehiclesModels.AsNoTracking().OrderBy(x => x.Brand).Select(x => x.Brand).Where(x => x != string.Empty).ToListAsync();
            return brands.Where(x => !x.Contains('?') && !x.Contains('/') && !x.Contains(' ')).Distinct().ToList();
        }

        // GET: api/Actions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleModel>> Get(long id)
        {
            var vehicleModel = await _context.VehiclesModels.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
            vehicleModel.SubModel = vehicleModel.SubModel.Split('?').Last();

            if (vehicleModel == null)
            {
                return NotFound();
            }

            return vehicleModel;
        }

        // GET: api/Actions/5
        [HttpGet("{brand}/{subModel}")]
        public async Task<ActionResult<VehicleModel>> GetByBrandSubModel(string brand, string subModel)
        {
            var action = await _context.VehiclesModels.AsNoTracking().Where(x => x.Brand.Contains(brand) && x.SubModel.Contains(subModel)).FirstOrDefaultAsync();

            if (action == null)
            {
                return NotFound();
            }

            return action;
        }



        // PUT: api/Actions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, VehicleModel vehicleModel)
        {
            if (id != vehicleModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(vehicleModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleModelExists(id))
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
        public async Task<ActionResult<VehicleModel>> Post(VehicleModel vehicleModel)
        {
            _context.VehiclesModels.Add(vehicleModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = vehicleModel.Id }, vehicleModel);
        }

        // DELETE: api/Actions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var action = await _context.VehiclesModels.FindAsync(id);
            if (action == null)
            {
                return NotFound();
            }

            _context.VehiclesModels.Remove(action);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleModelExists(long id)
        {
            return _context.VehiclesModels.Any(e => e.Id == id);
        }
    }
}
