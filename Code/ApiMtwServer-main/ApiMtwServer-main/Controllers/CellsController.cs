using EntityMtwServer;
using EntityMtwServer.Entities;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTWServerApi.Services;
using System.Reflection;

namespace MTWServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CellsController : ControllerBase
    {
        private readonly MasterServerContext _context;
        private readonly UserServices _userServices;

        public CellsController(MasterServerContext context, UserServices userServices)
        {
            _context = context;
            _userServices = userServices;
        }

        // GET: api/<CellsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cell>>> Get()
        {
            List<Group> allGroups = await _context.Groups.Include(x => x.Subgroups).Include(x => x.Users).AsNoTracking().ToListAsync();
            List<Group> cellsGroups = allGroups.Where(x => x.Subgroups.Count == 0 && x.Type == "Cell").ToList();
            List<Cell> cells = new List<Cell>();

            try
            {
                foreach (Group group in cellsGroups)
                {
                    Group gallery = allGroups.Where(x => x.Subgroups.Select(g => g.Id).Contains(group.Id)).FirstOrDefault();
                    Group block = allGroups.Where(x => x.Subgroups.Select(g => g.Id).Contains(gallery.Id)).FirstOrDefault();
                    Group penitentiary = allGroups.Where(x => x.Subgroups.Select(g => g.Id).Contains(block.Id)).FirstOrDefault();

                    Cell cell = new Cell();
                    cell.Name = group.Name;
                    cell.Id = group.Id;
                    cell.Gallery = await _context.Groups.Where(g => g.Id == gallery.Id).AsNoTracking().FirstOrDefaultAsync();
                    cell.Block = await _context.Groups.Where(g => g.Id == block.Id).AsNoTracking().FirstOrDefaultAsync();
                    cell.Penitentiary = await _context.Groups.Where(g => g.Id == penitentiary.Id).AsNoTracking().FirstOrDefaultAsync();
                    cell.Dvc = group.Equipments.Count > 0 ? _context.DVCs.Where(x => x.Id == group.Equipments.FirstOrDefault().Id).AsNoTracking().FirstOrDefault() : new DVC();
                    cell.Members = group.Users.ToList();
                    cells.Add(cell);
                }
            }
            catch
            {

            }

            return cells;
        }

        // GET api/<CellsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cell>> Get(long id)
        {
            List<Group> allGroups = await _context.Groups.Include(x => x.Subgroups).Include(x => x.Equipments).Include(x => x.Users).AsNoTracking().ToListAsync();
            Group? cellGroup = allGroups.Where(x => x.Id == id && x.Type == "Cell").FirstOrDefault();

            Group gallery = allGroups.Where(x => x.Subgroups.Select(g => g.Id).Contains(cellGroup.Id)).FirstOrDefault();
            Group block = allGroups.Where(x => x.Subgroups.Select(g => g.Id).Contains(gallery.Id)).FirstOrDefault();
            Group penitentiary = allGroups.Where(x => x.Subgroups.Select(g => g.Id).Contains(block.Id)).FirstOrDefault();

            List<User> groupUsers = await _context.Users.Where(x => x.Groups.Select(u => u.Id).Contains(cellGroup.Id)).AsNoTracking().ToListAsync();
            cellGroup.Users = groupUsers;

            Cell cell = new Cell();
            try
            {
                cell.Name = cellGroup.Name;
                cell.Id = cellGroup.Id;
                cell.Gallery = await _context.Groups.Where(g => g.Id == gallery.Id).AsNoTracking().FirstOrDefaultAsync();
                cell.Block = await _context.Groups.Where(g => g.Id == block.Id).AsNoTracking().FirstOrDefaultAsync();
                cell.Penitentiary = await _context.Groups.Where(g => g.Id == penitentiary.Id).AsNoTracking().FirstOrDefaultAsync();
                cell.Dvc = _context.DVCs.Where(x => x.Id == cellGroup.Equipments.FirstOrDefault().Id).FirstOrDefault();
                cell.Members = cellGroup.Users.ToList();
            }
            catch
            {

            }

            return cell;
        }

        // POST api/<CellsController>
        [HttpPost]
        public async Task<ActionResult<Cell>> Post([FromBody] Cell cell)
        {
            Group? penitentiary = await _context.Groups.Where(x => x.Id == cell.Penitentiary.Id).Include(x => x.Subgroups).FirstOrDefaultAsync();
            Group? block = await _context.Groups.Where(x => x.Id == cell.Block.Id).Include(x => x.Subgroups).FirstOrDefaultAsync();
            Group? gallery = await _context.Groups.Where(x => x.Id == cell.Gallery.Id).Include(x => x.Subgroups).FirstOrDefaultAsync();

            if (block != null)
            {
                if (!penitentiary.Subgroups.Contains(block))
                {
                    penitentiary.Subgroups.Add(block);
                    penitentiary.Type = "Penitentiary";
                    _context.Entry(penitentiary).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }

            if (gallery != null)
            {
                if (!block.Subgroups.Contains(gallery))
                {
                    block.Subgroups.Add(gallery);
                    block.Type = "Block";
                    _context.Entry(block).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }

            Group groupCell = new Group();
            groupCell.Name = cell.Name;
            groupCell.Type = "Cell";

            List<Equipment> l = _context.Equipments.Where(x => x.Id == cell.Dvc.Id).ToList();
            groupCell.Equipments = l;

            List<User> u = _context.Users.ToList() // list of all equipments in the database
                .Where(x => cell.Members //where each equipment in the list. 
                .Select(y => y.Id)
                .Contains(x.Id))
                .ToList();
            groupCell.Users = u;
            _context.Groups.Add(groupCell);
            await _context.SaveChangesAsync();

            if (groupCell != null)
            {
                if (!gallery.Subgroups.Contains(groupCell))
                {
                    gallery.Subgroups.Add(groupCell);
                    gallery.Type = "Gallery";
                    _context.Entry(gallery).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }

            cell.Id = groupCell.Id;
            return cell;
        }

        // PUT api/<CellsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Cell>> Put(long id, [FromBody] Cell cell)
        {
            Group? groupCell = await _context.Groups.Where(x => x.Id == id).Include(x => x.Users).Include(x => x.Equipments).FirstOrDefaultAsync();
            
            if (groupCell != null)
            {
                groupCell.Equipments.Clear();
                groupCell.Users.Clear();
                _context.Entry(groupCell).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                groupCell.Name = cell.Name;
                groupCell.Equipments.Add(await _context.Equipments.Where(x => x.Id == cell.Dvc.Id).FirstOrDefaultAsync());
                groupCell.Users = await _context.Users.Where(x => cell.Members.Select(y => y.Id).Contains(x.Id)).ToListAsync();
                

                await _context.SaveChangesAsync();
            }
               
            return cell;
        }

        // DELETE api/<CellsController>/5
        [HttpDelete("{id}")]
        public async Task<Result<Cell>> Delete(long id)
        {
            try
            {
                Group? groupCell = await _context.Groups.Where(x => x.Id == id).Include(x => x.Users).Include(x => x.Equipments).FirstOrDefaultAsync();
                groupCell.Equipments.Clear();
                groupCell.Users.Clear();
                _context.Entry(groupCell).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                Group? gallery = await _context.Groups.Where(x => x.Subgroups.Select(g => g.Id).Contains(id)).Include(x => x.Subgroups).FirstOrDefaultAsync();
                gallery.Subgroups.Remove(groupCell);
                _context.Entry(gallery).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                _context.Groups.Remove(groupCell);
                await _context.SaveChangesAsync();

                return Result.Ok();

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
    }
}
