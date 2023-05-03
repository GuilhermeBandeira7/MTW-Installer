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
using System.Reflection;

namespace MTWServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAlarmsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public UserAlarmsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/UserAlarms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAlarm>>> GetUserAlarms()
        {
            return await _context.UserAlarms.ToListAsync();
        }

        // GET: api/UserAlarms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAlarm>> GetUserAlarm(long id)
        {
            var userAlarm = await _context.UserAlarms.AsNoTracking().Include(x => x.Schedule).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (userAlarm == null)
            {
                return NotFound();
            }

            return userAlarm;
        }

        // PUT: api/UserAlarms/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAlarm(long id, UserAlarm userAlarm)
        {
            if (id != userAlarm.Id)
            {
                return BadRequest();
            }

            UserAlarm userAlarmToUpdate = await _context.UserAlarms.Include(x => x.Schedule).Where(x => x.Id == id).FirstOrDefaultAsync();
            List<PropertyInfo> propertyInfos = new List<PropertyInfo>(userAlarm.GetType().GetProperties());
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                propertyInfo.SetValue(userAlarmToUpdate, propertyInfo.GetValue(userAlarm, null), null);
            }

            userAlarmToUpdate.Schedule = await _context.Schedules.Where(x => x.Id == userAlarm.Schedule.Id).FirstOrDefaultAsync();

         

            _context.Entry(userAlarmToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAlarmExists(id))
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

        // POST: api/UserAlarms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserAlarm>> PostUserAlarm(UserAlarm userAlarm)
        {
            userAlarm.Schedule = await _context.Schedules.Where(x => x.Id == userAlarm.Schedule.Id).FirstOrDefaultAsync();
            _context.UserAlarms.Add(userAlarm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserAlarm", new { id = userAlarm.Id }, userAlarm);
        }

        // DELETE: api/UserAlarms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAlarm(long id)
        {
            var userAlarm = await _context.UserAlarms.FindAsync(id);
            if (userAlarm == null)
            {
                return NotFound();
            }

            _context.UserAlarms.Remove(userAlarm);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserAlarmExists(long id)
        {
            return _context.UserAlarms.Any(e => e.Id == id);
        }
    }
}
