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
    public class TelemetryMessagesController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public TelemetryMessagesController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/TelemetryMessages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TelemetryMessage>>> GetTelemetryMessages()
        {
            return await _context.TelemetryMessages.ToListAsync();
        }

        // GET: api/TelemetryMessages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TelemetryMessage>> GetTelemetryMessage(long id)
        {
            var telemetryMessage = await _context.TelemetryMessages.FindAsync(id);

            if (telemetryMessage == null)
            {
                return NotFound();
            }

            return telemetryMessage;
        }

        // PUT: api/TelemetryMessages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTelemetryMessage(long id, TelemetryMessage telemetryMessage)
        {
            if (id != telemetryMessage.Id)
            {
                return BadRequest();
            }

            _context.Entry(telemetryMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TelemetryMessageExists(id))
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

        // POST: api/TelemetryMessages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TelemetryMessage>> PostTelemetryMessage(TelemetryMessage telemetryMessage)
        {
            _context.TelemetryMessages.Add(telemetryMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTelemetryMessage", new { id = telemetryMessage.Id }, telemetryMessage);
        }

        // DELETE: api/TelemetryMessages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTelemetryMessage(long id)
        {
            var telemetryMessage = await _context.TelemetryMessages.FindAsync(id);
            if (telemetryMessage == null)
            {
                return NotFound();
            }

            _context.TelemetryMessages.Remove(telemetryMessage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TelemetryMessageExists(long id)
        {
            return _context.TelemetryMessages.Any(e => e.Id == id);
        }
    }
}
