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
    public class TelemetryAlarmsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public TelemetryAlarmsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/TelemetryAlarms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TelemetryAlarm>>> GetTelemetryAlarms()
        {
            return await _context.TelemetryAlarms.ToListAsync();
        }

        // GET: api/TelemetryAlarms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TelemetryAlarm>> GetTelemetryAlarm(long id)
        {
            var telemetryAlarm = await _context.TelemetryAlarms.FindAsync(id);

            if (telemetryAlarm == null)
            {
                return NotFound();
            }

            return telemetryAlarm;
        }

        // PUT: api/TelemetryAlarms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTelemetryAlarm(long id, TelemetryAlarm telemetryAlarm)
        {
            if (id != telemetryAlarm.Id)
            {
                return BadRequest();
            }

            _context.Entry(telemetryAlarm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TelemetryAlarmExists(id))
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

        // POST: api/TelemetryAlarms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TelemetryAlarm>> PostTelemetryAlarm(TelemetryAlarm telemetryAlarm)
        {
            _context.TelemetryAlarms.Add(telemetryAlarm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTelemetryAlarm", new { id = telemetryAlarm.Id }, telemetryAlarm);
        }

        // DELETE: api/TelemetryAlarms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTelemetryAlarm(long id)
        {
            var telemetryAlarm = await _context.TelemetryAlarms.FindAsync(id);
            if (telemetryAlarm == null)
            {
                return NotFound();
            }

            _context.TelemetryAlarms.Remove(telemetryAlarm);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TelemetryAlarmExists(long id)
        {
            return _context.TelemetryAlarms.Any(e => e.Id == id);
        }
    }
}
