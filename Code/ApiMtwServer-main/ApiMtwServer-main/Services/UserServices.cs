using EntityMtwServer;
using EntityMtwServer.Entities;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace MTWServerApi.Services
{
    public class UserServices
    {
        private readonly MasterServerContext _context;

        public UserServices(MasterServerContext context)
        {
            _context = context;
        }


        //get method to get all users
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _context.Users.AsNoTracking().Include(x => x.Profile).Include(x => x.Groups).Include(x => x.Equipments).ToListAsync();       
        }

        //get method to get user by it's id
        public async Task<User> GetUserById([FromQuery] long id)
        {
            User? user = await _context.Users.Where(x => x.Id == id).Include(x => x.Profile).Include(x => x.Groups).Include(x => x.Equipments).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return user;
        }

        //get method to get user by it's id
        public async Task<User> GetUserByUsernamePassword(string username, string password)
        {
            User? user = await _context.Users.Where(x => (x.Login == username && x.Password == password)).Include(x => x.Profile).Include(x => x.Groups).Include(x => x.Equipments).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return user;
        }


        //put method to update a user
        public async Task<Result<User>> UpdateUser(long id, User user)
        {
            if (id != user.Id)
            {
                return Result.Fail("Id argument does not correspond to a existing user Id");
            }

            User? usr = await _context.Users.Where(x => x.Id == id).Include(x => x.Groups).Include(x => x.Equipments).FirstOrDefaultAsync(); // select an equipment by it's id and include all equipment's groups in the query
            usr.Groups.Clear();
            usr.Equipments.Clear();
            _context.Entry(usr).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            IList<PropertyInfo> props = new List<PropertyInfo>(user.GetType().GetProperties());

            foreach (PropertyInfo prop in props)
            {
                prop.SetValue(usr, prop.GetValue(user, null), null);
            }

            List<Group> lg = _context.Groups.ToList()
             .Where(x => usr.Groups
             .Select(y => y.Id)
             .Contains(x.Id))
             .ToList();
            usr.Groups = lg;

            List<Equipment> le = _context.Equipments.ToList()
            .Where(x => usr.Equipments
            .Select(y => y.Id)
            .Contains(x.Id))
            .ToList();
            usr.Equipments = le;


            _context.Entry(usr).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Result.Ok();
        }

        //post method to create a new user
        public async Task<Result<User>> AddUser(User user)
        {
            if(user == null)
            {
                return Result.Fail("Failed to create new user because the user reference is null.");
            }

            List<Equipment> equipmentList = _context.Equipments.ToList()
               .Where(x => user.Equipments
               .Select(y => y.Id)
               .Contains(x.Id))
               .ToList();
            user.Equipments = equipmentList; 

            List<Group> groupList = _context.Groups.ToList()
                .Where(x => user.Groups
                .Select(y => y.Id)
                .Contains(x.Id))
                .ToList();
            user.Groups = groupList;

            user.Profile = await _context.Profiles.Where(x => x.Id == user.Profile.Id).FirstOrDefaultAsync();
 
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Result.Ok(user);
        }

        //delete method to delete a user

        public async Task<Result<User>> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return Result.Fail("User not found");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Result.Ok();
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
