
using EntityMtwServer;
using EntityMtwServer.Entities;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MTWServerApi.Services
{
    public class GroupServices
    {
        private readonly MasterServerContext _context;

        public GroupServices(MasterServerContext context)
        {
            _context = context;

        }

        //get method to get all groups
        public async Task<ActionResult<IEnumerable<Group>>> GetAllGroups(long groupId)
        {
            if (_context.Groups != null)
            {
                if(groupId == 1)
                {    
                    return await _context.Groups
                    .AsNoTracking() // this method is solving the cycling problem of returning groups inside equipments(that already belongs to the same group it's being returned)
                    .Include(x => x.Equipments)
                    .Include(x => x.Subgroups)
                    .ToListAsync(); //including the equipments inside the groups                          
                }
            }

            User? user = _context.Users.FirstOrDefault(x => x.Id == groupId);
            return user.Groups.ToList();

        }

        //get method to get groups by id
        public async Task<Group> GetGroupByID(long id, long userId)
        {
            if (id != null)
            {
                if (userId == 1)
                {
                    Group? group = await _context.Groups.Where(x => x.Id == id).AsNoTracking().Include(x => x.Equipments).Include(x => x.Subgroups).FirstOrDefaultAsync();
                    return group;
                }

                User? user = _context.Users.Include(x => x.Groups).FirstOrDefault(x => x.Id == userId);
                if (user.Groups.Select(x => x.Id).ToList().Contains(id))
                    return user.Groups.Where(x => x.Id == id).FirstOrDefault();
            }

            return new Group();
        }

        //Put method to update a group
        public async Task<Result> UpdateGroup(long id, Group group)
        {
          
            if (group == null)
            {
                return Result.Fail("Failed to add group");
            }

            Group? grp = _context.Groups.Where(x => x.Id == id).Include(x => x.Equipments).FirstOrDefault(); // select a group by it's id and include all group's equipments in the query
            grp.Equipments.Clear(); // deleting all equipments inside the group
            _context.Entry(grp).State = EntityState.Modified;
            await _context.SaveChangesAsync();


            System.Type myType = group.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties()); // mapping all properties of group parameter

            foreach (PropertyInfo prop in props)
            {
                prop.SetValue(grp, prop.GetValue(group, null), null); // mapping group with all grp properties
            }

            _context.Groups.Update(grp);
            _context.Entry(grp).State = EntityState.Modified;

            List<Equipment> l = _context.Equipments.ToList() // List of all equipments
             .Where(x => group.Equipments // where the equipment and it's groups
             .Select(y => y.Id) // select to create a new list of equipments id of the group parameter
             .Contains(x.Id)) //if the list of id's contains the id of equipment x 
             .ToList(); // to create a new list
            grp.Equipments = l;

            _context.Entry(grp).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        //Post method to add a Group
        public async Task<Result<Group>> AddGroup(Group group)
        {
            if (group == null)
            {
                return Result.Fail("Request failed, group cannot be null");
            }

            //logic to load the add group in the database
            if(group.Equipments.Count > 0)
            {
                List<Equipment> l = _context.Equipments.ToList() // list of all equipments in the database
                  .Where(x => group.Equipments //where each equipment in the list. 
                  .Select(y => y.Id)
                  .Contains(x.Id))
                  .ToList();
                group.Equipments = l;
            }
  

                                                                         
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return Result.Ok();

        }

        //Delete method to delete a group
        public async Task<Result<Group>> DeleteGroup(long id)
        {
            try
            {
                if (GroupExists(id) == true)
                {
                    Group? group = _context.Groups.Include(x => x.Equipments).FirstOrDefault(x => x.Id == id);
                    group.Equipments = null;
                    _context.Entry(group).State = EntityState.Modified;
                    _context.Groups.Remove(group);
                    await _context.SaveChangesAsync();

                    return Result.Ok();
                }
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return Result.Fail("Failed to remove group.");
        }

        private bool GroupExists(long id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }

    }
}
