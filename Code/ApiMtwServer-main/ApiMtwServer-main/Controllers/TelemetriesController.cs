#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EntityMtwServer;
using EntityMtwServer.Entities;
using System.Reflection;
using MTWServerApi.Services;


namespace MTWServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelemetriesController : ControllerBase
    {
        private readonly TelemetryServices _telemetryService;

        public TelemetriesController(TelemetryServices telemetryServices)
        {
            _telemetryService = telemetryServices;
        }

        // GET: api/Telemetries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Telemetry>>> GetTelemetries()
        {
            return await _telemetryService.ReadTelemetries();
        }

        // GET: api/Telemetries/5
        [HttpGet("{id}")]
        public async Task<Telemetry> GetTelemetry(long id)
        {
            return await _telemetryService.GetTelemetryById(id);
        }

        // PUT: api/Telemetries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTelemetry(long id, Telemetry telemetry)
        {
            if(telemetry == null)
            {
                return BadRequest();
            }
            await _telemetryService.UpdateTelemetry(id, telemetry);
            return Ok();    
        }

        // POST: api/Telemetries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Telemetry>> PostTelemetry(Telemetry telemetry)
        {
            if (telemetry == null)
            {
                return BadRequest();
            }
            await _telemetryService.AddTelemetry(telemetry);
            return CreatedAtAction("GetTelemetry", new { id = telemetry.Id }, telemetry);

        }

        // DELETE: api/Telemetries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTelemetry(long id)
        {
            await _telemetryService.DeleteTelemtry(id);
            return Ok();
        }

        
    }
}
