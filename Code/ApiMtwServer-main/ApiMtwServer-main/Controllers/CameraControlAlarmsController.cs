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
    public class CameraControlAlarmsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public CameraControlAlarmsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/CameraControlAlarms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CameraControlAlarm>>> GetCameraControlAlarms()
        {
            return await _context.CameraControlAlarms.ToListAsync();
        }

        // GET: api/CameraControlAlarms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CameraControlAlarm>> GetCameraControlAlarm(long id)
        {
            var cameraControlAlarm = await _context.CameraControlAlarms.FindAsync(id);

            if (cameraControlAlarm == null)
            {
                return NotFound();
            }

            return cameraControlAlarm;
        }

        // PUT: api/CameraControlAlarms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCameraControlAlarm(long id, CameraControlAlarm cameraControlAlarm)
        {
            if (id != cameraControlAlarm.Id)
            {
                return BadRequest();
            }

            _context.Entry(cameraControlAlarm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CameraControlAlarmExists(id))
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

        // POST: api/CameraControlAlarms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CameraControlAlarm>> PostCameraControlAlarm(CameraControlAlarm cameraControlAlarm)
        {
            _context.CameraControlAlarms.Add(cameraControlAlarm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCameraControlAlarm", new { id = cameraControlAlarm.Id }, cameraControlAlarm);
        }

        // DELETE: api/CameraControlAlarms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCameraControlAlarm(long id)
        {
            var cameraControlAlarm = await _context.CameraControlAlarms.FindAsync(id);
            if (cameraControlAlarm == null)
            {
                return NotFound();
            }

            _context.CameraControlAlarms.Remove(cameraControlAlarm);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CameraControlAlarmExists(long id)
        {
            return _context.CameraControlAlarms.Any(e => e.Id == id);
        }
    }
}
