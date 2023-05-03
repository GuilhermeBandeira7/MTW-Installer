

using EntityMtwServer;
using EntityMtwServer.Entities;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Reflection;

namespace MTWServerApi.Services
{
    public class EquipmentsService
    {
        public MasterServerContext _context;

        public EquipmentsService(MasterServerContext context)
        {
            _context = context;
        }

        // If there is a equipment inside a group return true, else false
        public bool EquipmentsInsideGroups(Equipment equipment)
        {
            if (equipment == null)
            {
                throw new ArgumentNullException(nameof(equipment));
            }
            if (_context.Groups.Where(x => x.Equipments.Contains(equipment)).Count() > 0)
            {
                return true;
            }
            if (equipment.Groups.Count() > 0)
            {
                return true;
            }
            return false;
        }

        //Get Equipments by id
        public async Task<Equipment> GetEquipment(long id, long userId)
        {
            if(id != null)
            {
                if (userId == 1)
                {
                    Equipment? equipment = await _context.Equipments.Where(x => x.Id == id).AsNoTracking().Include(x => x.Groups).FirstOrDefaultAsync();
                    return equipment;
                }

                User? user = _context.Users.Include(x => x.Equipments).FirstOrDefault(x => x.Id == userId);
                if (user.Equipments.Select(x => x.Id).ToList().Contains(id))
                    return user.Equipments.Where(x => x.Id == id).FirstOrDefault();
            }

            return new Equipment();

        }
        //Get Equipments 
        public async Task<ActionResult<IEnumerable<Equipment>>> ReadEquipments([FromBody]long id)
        {
            if (id == 1)
            {
                if (_context.Equipments != null)
                {
                    return await _context.Equipments.ToListAsync();
                }
            }
            User? user = _context.Users.FirstOrDefault(x => x.Id == id);
            return user.Equipments.ToList();
        }

        //Put (Edit) Equipments
        public async Task<Result<Equipment>> PutEquipment(long id, Equipment equipment)
        {
            if (equipment == null)
            {
                return Result.Fail("Equipment cannot be null");
            }

            Equipment? eqp = _context.Equipments.Where(x => x.Id == id).Include(x => x.Groups).FirstOrDefault(); // select an equipment by it's id and include all equipment's groups in the query
            eqp.Groups.Clear();
            _context.Entry(eqp).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            System.Type myType = equipment.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                prop.SetValue(eqp, prop.GetValue(equipment, null), null);
            }

            List<Group> l = _context.Groups.ToList()
             .Where(x => equipment.Groups
             .Select(y => y.Id)
             .Contains(x.Id))
             .ToList();
            eqp.Groups = l;


            _context.Entry(eqp).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            //await _context.SaveChangesAsync();



            /* for(int counter = 0; counter < eqp.Groups.Count; counter++) // iterating through all equipment's groups.
             {
                 Group group = eqp.Groups.ElementAt(counter); // group receive a group at counter position
                 if(!equipment.Groups.ToList().Contains(group)) // if the equipment received as argument contains the group variable at the counter position remove that group
                 {
                     eqp.Groups.Remove(group);
                     counter--; // counter -1
                 }
             }*/

            /* _context.Entry(eqp).State = EntityState.Detached;

             _context.Equipments.Attach(equipment);
             _context.Equipments.Update(equipment);
             _context.Entry(equipment).State = EntityState.Modified;*/


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentExists(id))
                {
                    return Result.Fail("The equipment does not exists");
                }
                else
                {
                    throw;
                }
            }

            return Result.Ok();


        }

        //Post Equipment
        public async Task<Result<Equipment>> PostEquipment(Equipment equipment)
        {
            List<Group> l = _context.Groups.ToList() //geral 
               .Where(x => equipment.Groups // x é um dos equips 
               .Select(y => y.Id) // making equip list of id's
               .Contains(x.Id))  //verificando se o equip esta contido na lista gerada acima
               .ToList();
            equipment.Groups = l;

            _context.Equipments.Add(equipment);
            await _context.SaveChangesAsync();
            return Result.Ok();
        }


        public async Task<Result<Equipment>> DeleteEquipments(long id, long userId)
        {
            Equipment? equipment = _context.Equipments.Where(x => x.Id == id).Include(x => x.Groups).FirstOrDefault(); // select an equipment by it's id and include all equipment's groups in the query
            equipment.Groups.Clear();
            _context.Entry(equipment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            User? user = _context.Users.Include(x => x.Equipments).Include(x => x.Groups).Include(x => x.Profile).FirstOrDefault(user => user.Id == userId);
            if (userId == 1)
            {
                if (equipment != null)
                {
                    _context.Equipments.Remove(equipment);
                    await _context.SaveChangesAsync();
                    return Result.Ok();
                }
                throw new NullReferenceException(nameof(equipment));
            }
            return Result.Fail("The user does not has permission do delete an equipment.");
        }

        private bool EquipmentExists(long id)
        {
            return _context.Equipments.Any(e => e.Id == id);
        }


    }
}
