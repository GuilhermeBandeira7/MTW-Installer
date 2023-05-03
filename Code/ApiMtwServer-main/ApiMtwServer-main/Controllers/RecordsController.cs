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
using System.Diagnostics;

namespace MTWServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public RecordsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/Records
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Record>>> GetRecords()
        {
            return await _context.Records.ToListAsync();
        }

        // GET: api/Records/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Record>> GetRecord(long id)
        {
            var @record = await _context.Records.FindAsync(id);

            if (@record == null)
            {
                return NotFound();
            }

            return @record;
        }


        // PUT: api/Records/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecord(long id, Record @record)
        {
            if (id != @record.Id)
            {
                return BadRequest();
            }

            _context.Entry(@record).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecordExists(id))
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

        // POST: api/Records
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Record>> PostRecord(Record @record)
        {   
            _context.Database.OpenConnection();
            try
            {
                string query = "INSERT INTO [dbo].[Record] " +
                    "([Id],[RecordPath],[Duration],[RemoteUser],[RemotePassword]) " +
                    "VALUES(" + Convert.ToInt32(@record.Id) + ",'" + @record.RecordPath + "'," + record.Duration + ",'" + record.RemoteUser + "', '" + record.RemotePassword + "')";
                _context.Database.ExecuteSqlRaw(query);
            }
            finally
            {
                _context.Database.CloseConnection();
            }

            return CreatedAtAction("GetRecord", new { id = @record.Id }, @record);
        }

        // DELETE: api/Records/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecord(long id)
        {
            var @record = await _context.Records.FindAsync(id);
            if (@record == null)
            {
                return NotFound();
            }

            _context.Records.Remove(@record);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecordExists(long id)
        {
            return _context.Records.Any(e => e.Id == id);
        }

        [HttpPost("StartRecord/{id}/{recordName}")]
        public async Task<ActionResult<Record>> StartRecord(long id, string recordName)
        {
            Record record = await _context.Records.Where(r => r.Id == id).AsNoTracking().FirstOrDefaultAsync();

            string path = record.RecordPath + "/" + recordName;


            Process process = new Process();
            process.StartInfo.FileName = "ffmpeg.exe";
            process.StartInfo.Arguments = $" -i {record.PrimaryStreamingRtsp} -vcodec copy -t 00:00:15 {path}.mkv";
            process.Start();

            process.WaitForExit();

            return record;
        }
    }
}
