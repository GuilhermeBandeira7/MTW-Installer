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
    public class CameraControlsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public CameraControlsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/CameraControls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CameraControl>>> GetCameraControls()
        {
            return await _context.CameraControls.ToListAsync();
        }

        // GET: api/CameraControls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CameraControl>> GetCameraControl(long id)
        {
            var cameraControl = await _context.CameraControls.FindAsync(id);

            if (cameraControl == null)
            {
                return NotFound();
            }

            return cameraControl;
        }

        // PUT: api/CameraControls/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCameraControl(long id, CameraControl cameraControl)
        {
            if (id != cameraControl.Id)
            {
                return BadRequest();
            }

            _context.Entry(cameraControl).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CameraControlExists(id))
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

        // POST: api/CameraControls
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CameraControl>> PostCameraControl(CameraControl cameraControl)
        {
            _context.CameraControls.Add(cameraControl);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCameraControl", new { id = cameraControl.Id }, cameraControl);
        }

        // DELETE: api/CameraControls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCameraControl(long id)
        {
            var cameraControl = await _context.CameraControls.FindAsync(id);
            if (cameraControl == null)
            {
                return NotFound();
            }

            _context.CameraControls.Remove(cameraControl);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CameraControlExists(long id)
        {
            return _context.CameraControls.Any(e => e.Id == id);
        }
    }
}
