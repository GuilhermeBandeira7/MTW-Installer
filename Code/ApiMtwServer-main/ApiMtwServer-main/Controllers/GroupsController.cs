#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EntityMtwServer.Entities;
using MTWServerApi.Services;
using Microsoft.AspNetCore.Cors;

namespace MTWServerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly GroupServices _services;

        public GroupsController(GroupServices services)
        {
            _services = services;
        }

        // GET: api/Groups
        [HttpGet("{groupId}")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups(long groupId)
        {
            return await _services.GetAllGroups(groupId);
        }



        // GET: api/Groups/5
        [HttpGet("{id}/{userId}")]
        public async Task<Group> GetGroup(long id, long userId)
        {
            return await _services.GetGroupByID(id, userId);
        }

        // PUT: api/Groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(long id, Group group)
        {
            await _services.UpdateGroup(id, group);
            return NoContent();
        }

        // POST: api/Groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Group>> PostGroup(Group group)
        {
            await _services.AddGroup(group);
            return CreatedAtAction("GetGroup", new { id = group.Id, userId = 1 }, group);

        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(long id)
        {
            await _services.DeleteGroup(id);
            return NoContent();

        }


    }
}
