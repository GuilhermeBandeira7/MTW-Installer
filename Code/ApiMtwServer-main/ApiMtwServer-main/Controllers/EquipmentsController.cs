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
using MTWServerApi.Services;
using Microsoft.AspNetCore.Cors;
using FluentResults;

namespace MTWServerApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EquipmentsController : ControllerBase
    {
        private readonly EquipmentsService _equipementsService;

        public EquipmentsController(EquipmentsService services)
        {
            _equipementsService = services;
        }

        // GET: api/Equipments
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Equipment>>> GetEquipments(long userId) 
        {
            return await _equipementsService.ReadEquipments(userId);
        }

        // GET: api/Equipments/5
        [HttpGet("{id}/{userId}")]
        public async Task<Equipment> GetEquipment(long id, long userId) 
        {
            return await _equipementsService.GetEquipment(id, userId);
        }

        // PUT: api/Equipments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipment(long id,Equipment equipment) 
        {
            await _equipementsService.PutEquipment(id, equipment);
            return NoContent();
        }

        // POST: api/Equipments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Equipment>> PostEquipment(Equipment equipment)
        {
            if (_equipementsService.EquipmentsInsideGroups(equipment) == true)
            {
                await _equipementsService.PostEquipment(equipment);
                return equipment;
            };
            return StatusCode(415);
        }

        // DELETE: api/Equipments/5
        [HttpDelete("{id}/{userId}")]
        public async Task<IActionResult> DeleteEquipment(long id, long userId)
        {
            await _equipementsService.DeleteEquipments(id, userId);
            return NoContent();
        }

    }
}
