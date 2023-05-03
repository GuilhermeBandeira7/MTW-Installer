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
    public class LprsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public LprsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/Lprs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lprs>>> GetLprs()
        {
            return await _context.Lprs.AsNoTracking().Include(x => x.LprCamera).ToListAsync();
        }

        // GET: api/Lprs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lprs>> GetLpr(long id)
        {
            var lpr = await _context.Lprs.AsNoTracking()
                .Include(x => x.LprCamera)
                .Include(x => x.Origin)
                .Include(x => x.Controller)
                .Include(x => x.Acess)
                .Include(x => x.LprContext)
                .Include(x => x.Context1)
                .Include(x => x.Context2)
                .Include(x => x.Context3)
                .Include(x => x.Context4)
                .Include(x => x.UserAlarm)
                .ThenInclude(x => x.Schedule)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (lpr == null)
            {
                return NotFound();
            }

            return lpr;
        }

        // PUT: api/Lprs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLpr(long id, Lprs lpr)
        {
            var lprModel = await _context.Lprs
               .Include(x => x.LprCamera)
               .Include(x => x.Origin)
               .Include(x => x.Controller)
               .Include(x => x.Acess)
               .Include(x => x.Context1)
               .Include(x => x.Context2)
               .Include(x => x.Context3)
               .Include(x => x.Context4)
               .Where(x => x.Id == id).FirstOrDefaultAsync();


            lprModel.LprName = lpr.LprName;
            lprModel.CountryCode = lpr.CountryCode;
            lprModel.FalseTime = lpr.FalseTime;
            lprModel.RefreshTime = lpr.RefreshTime;
            lprModel.DatabaseTime = lpr.DatabaseTime;
            lprModel.Threads = lpr.Threads;
            lprModel.Fps = lpr.Fps;
            lprModel.ResultConfirmation = lpr.ResultConfirmation;
            lprModel.MinCharHeight = lpr.MinCharHeight;
            lprModel.MaxCharHeight = lpr.MaxCharHeight;
            lprModel.ZoneX = lpr.ZoneX;
            lprModel.ZoneY = lpr.ZoneY;

            lprModel.Origin = await _context.Origins.Where(x => x.Id == lpr.Origin.Id).FirstOrDefaultAsync();
            lprModel.LprCamera = await _context.Equipments.Where(x => x.Id == lpr.LprCamera.Id).FirstOrDefaultAsync();
            lprModel.Controller = await _context.Equipments.Where(x => x.Id == lpr.Controller.Id).FirstOrDefaultAsync();
            lprModel.Acess = await _context.Equipments.Where(x => x.Id == lpr.Acess.Id).FirstOrDefaultAsync();
            lprModel.UserAlarm = lpr.UserAlarm != null ? await _context.UserAlarms.Where(x => x.Id == lpr.UserAlarm.Id).FirstOrDefaultAsync() : null;
            lprModel.Context1 = lpr.Context1 != null ? await _context.Equipments.Where(x => x.Id == lpr.Context1.Id).FirstOrDefaultAsync() : null;
            lprModel.Context2 = lpr.Context2 != null ? await _context.Equipments.Where(x => x.Id == lpr.Context2.Id).FirstOrDefaultAsync() : null;
            lprModel.Context3 = lpr.Context3 != null ? await _context.Equipments.Where(x => x.Id == lpr.Context3.Id).FirstOrDefaultAsync() : null;
            lprModel.Context4 = lpr.Context4 != null ? await _context.Equipments.Where(x => x.Id == lpr.Context4.Id).FirstOrDefaultAsync() : null;

            if (id != lpr.Id)
            {
                return BadRequest();
            }

            _context.Entry(lprModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LprExists(id))
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

        // POST: api/Lprs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lprs>> PostLpr(Lprs lpr)
        {
            lpr.Origin = await _context.Origins.Where(x => x.Id == lpr.Origin.Id).FirstOrDefaultAsync();
            lpr.LprCamera = await _context.Equipments.Where(x => x.Id == lpr.LprCamera.Id).FirstOrDefaultAsync();
            lpr.Controller = await _context.Equipments.Where(x => x.Id == lpr.Controller.Id).FirstOrDefaultAsync();
            lpr.Acess = await _context.Equipments.Where(x => x.Id == lpr.Acess.Id).FirstOrDefaultAsync();
            lpr.UserAlarm = lpr.UserAlarm != null ? await _context.UserAlarms.Where(x => x.Id == lpr.UserAlarm.Id).FirstOrDefaultAsync() : null;
            lpr.Context1 = lpr.Context1 != null ? await _context.Equipments.Where(x => x.Id == lpr.Context1.Id).FirstOrDefaultAsync() : null;
            lpr.Context2 = lpr.Context2 != null ? await _context.Equipments.Where(x => x.Id == lpr.Context2.Id).FirstOrDefaultAsync() : null;
            lpr.Context3 = lpr.Context3 != null ? await _context.Equipments.Where(x => x.Id == lpr.Context3.Id).FirstOrDefaultAsync() : null;
            lpr.Context4 = lpr.Context4 != null ? await _context.Equipments.Where(x => x.Id == lpr.Context4.Id).FirstOrDefaultAsync() : null;


            _context.Lprs.Add(lpr);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLpr", new { id = lpr.Id }, lpr);
        }

        // DELETE: api/Lprs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLpr(long id)
        {
            var lpr = await _context.Lprs.FindAsync(id);
            if (lpr == null)
            {
                return NotFound();
            }

            _context.Lprs.Remove(lpr);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LprExists(long id)
        {
            return _context.Lprs.Any(e => e.Id == id);
        }
    }
}
