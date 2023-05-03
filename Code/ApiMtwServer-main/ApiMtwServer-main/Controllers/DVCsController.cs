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
    public class DVCsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public DVCsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/DVCs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DVC>>> GetDVCs()
        {
            return await _context.DVCs.ToListAsync();
        }


        [HttpGet("availableDvc")]
        public async Task<ActionResult<IEnumerable<DVC>>> GetAvailableDVCs()
        {
            List<Equipment> cellsEquipments = await _context.Groups.Where(g => g.Type == "Cell").SelectMany(g => g.Equipments).ToListAsync();

            return await _context.DVCs.AsNoTracking()
                .Where(x => !cellsEquipments.Select(x => x.Id).Contains(x.Id))
                .Where(x => x.Function == "student")
                .ToListAsync();
        }

        // GET: api/DVCs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DVC>> GetDVC(long id)
        {
            var dVC = await _context.DVCs.FindAsync(id);

            if (dVC == null)
            {
                return NotFound();
            }

            return dVC;
        }


        // PUT: api/DVCs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDVC(long id, DVC dVC)
        {
            if (id != dVC.Id)
            {
                return BadRequest();
            }

            _context.Entry(dVC).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DVCExists(id))
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

        // POST: api/DVCs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DVC>> PostDVC(DVC dVC)
        {
            _context.Database.OpenConnection();
            try
            {
                string telemetryId = string.Empty;
                if (dVC.Telemetry != null)
                    telemetryId = dVC.Telemetry.Id > 0 ? dVC.Telemetry.Id.ToString() : "null";
                else
                    telemetryId = "null";

                string query = "INSERT INTO [dbo].[DVC] ([Id],[SerialNumber],[TelemetryId],[Function],[ServerId],[OperationalSystem],[VideoEnable],[AudioEnable],[PermanentStream],[StatusDateTime]) " +
                    "VALUES(" + dVC.Id + ",'" + dVC.SerialNumber + "'," + telemetryId + ",'" + dVC.Function + "'," + dVC.Server.Id + ",'" + dVC.OperationalSystem + "','" + dVC.VideoEnable + "','" + dVC.AudioEnable + "','" + dVC.PermanentStream + "','" + dVC.StatusDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                _context.Database.ExecuteSqlRaw(query);
            }
            finally
            {
                _context.Database.CloseConnection();
            }

            return CreatedAtAction("GetDVC", new { id = dVC.Id }, dVC);
        }

        // DELETE: api/DVCs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDVC(long id)
        {
            var dVC = await _context.DVCs.FindAsync(id);
            if (dVC == null)
            {
                return NotFound();
            }

            _context.DVCs.Remove(dVC);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DVCExists(long id)
        {
            return _context.DVCs.Any(e => e.Id == id);
        }
    }
}
