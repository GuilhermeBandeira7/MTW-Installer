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
    public class ServersController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public ServersController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/Servers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Server>>> GetServers()
        {
            List<Server> list = await _context.Servers.Include(x => x.ServerEquipments).Include(x => x.ServerGroups).AsNoTracking().ToListAsync();
            return list;
        }

        // GET: api/Servers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Server>> GetServer(long id)
        {
            var server = await _context.Servers.Where(x => x.Id == id).Include(x => x.ServerGroups).Include(x => x.ServerEquipments).AsNoTracking().FirstOrDefaultAsync();

            if (server == null)
            {
                return NotFound();
            }

            return server;
        }

        // PUT: api/Servers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServer(long id, Server server)
        {
            if (id != server.Id)
            {
                return BadRequest();
            }

            _context.Database.OpenConnection();
            try
            {
                _context.Database.ExecuteSqlRaw("UPDATE [dbo].[Server] SET " +
                    "[TelemetryServer] = " + Convert.ToInt32(server.TelemetryServer) + "," +
                    "[LprServer] = " + Convert.ToInt32(server.LprServer) + "," +
                    "[MasterEyeServer] = " + Convert.ToInt32(server.MasterEyeServer) + "," +
                    "[DigifortServer] = " + Convert.ToInt32(server.DigifortServer) + "," +
                    "[RecordServer] = " + Convert.ToInt32(server.RecordServer) + "," +
                    "[SessionServer] = " + Convert.ToInt32(server.SessionServer) + "," +
                    "[RtspServer] = " + Convert.ToInt32(server.RtspServer) + " " +
                    "WHERE Id = " + Convert.ToInt32(server.Id));

                _context.Database.ExecuteSqlRaw("DELETE FROM [dbo].[GroupServer] WHERE ServersId = " + id);
                _context.Database.ExecuteSqlRaw("DELETE FROM [dbo].[EquipmentServer] WHERE ServersId = " + id);

                if (server.ServerGroups.Count() > 0)
                {
                    foreach (Group group in server.ServerGroups)
                    {
                        _context.Database.ExecuteSqlRaw("INSERT INTO [dbo].[GroupServer]([ServerGroupsId],[ServersId]) VALUES(" + group.Id + "," + server.Id + ")");
                    }
                }

                if (server.ServerEquipments.Count() > 0)
                {
                    foreach (Equipment equipment in server.ServerEquipments)
                    {
                        _context.Database.ExecuteSqlRaw("INSERT INTO [dbo].[EquipmentServer]([ServerEquipmentsId],[ServersId]) VALUES(" + equipment.Id + "," + server.Id + ")");
                    }
                }
            }
            finally
            {
                _context.Database.CloseConnection();
            }

            return CreatedAtAction("GetServer", new { id = server.Id }, server);
        }

        // POST: api/Servers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Server>> PostServer(Server server)
        {
            _context.Database.OpenConnection();
            try
            {
                _context.Database.ExecuteSqlRaw("INSERT INTO [dbo].[Server] ([Id],[TelemetryServer],[LprServer],[MasterEyeServer],[DigifortServer],[RecordServer],[SessionServer],[RtspServer]) " +
                    "VALUES(" + Convert.ToInt32(server.Id) + "," + Convert.ToInt32(server.TelemetryServer) + "," + Convert.ToInt32(server.LprServer) + "," + Convert.ToInt32(server.MasterEyeServer) + "," +
                    "" + Convert.ToInt32(server.DigifortServer) + "," + Convert.ToInt32(server.RecordServer) + "," + Convert.ToInt32(server.SessionServer) + "," + Convert.ToInt32(server.RtspServer) + ")");

                if (server.ServerGroups.Count() > 0)
                {
                    foreach (Group group in server.ServerGroups)
                    {
                        _context.Database.ExecuteSqlRaw("INSERT INTO [dbo].[GroupServer]([ServerGroupsId],[ServersId]) VALUES(" + group.Id + "," + server.Id + ")");
                    }
                }

                if (server.ServerEquipments.Count() > 0)
                {
                    foreach (Equipment equipment in server.ServerEquipments)
                    {
                        _context.Database.ExecuteSqlRaw("INSERT INTO [dbo].[EquipmentServer]([ServerEquipmentsId],[ServersId]) VALUES(" + equipment.Id + "," + server.Id + ")");
                    }
                }
            }
            finally
            {
                _context.Database.CloseConnection();
            }

            return CreatedAtAction("GetServer", new { id = server.Id }, server);
        }

        // DELETE: api/Servers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServer(long id)
        {
            var server = await _context.Servers.FindAsync(id);
            if (server == null)
            {
                return NotFound();
            }
            _context.Database.OpenConnection();
            try
            {
                _context.Database.ExecuteSqlRaw("DELETE FROM [dbo].[Server] WHERE Id = " + id);
            }
            catch
            {

            }

            _context.Servers.Remove(server);
            return NoContent();
        }

        private bool ServerExists(long id)
        {
            return _context.Servers.Any(e => e.Id == id);
        }
    }
}
