#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityMtwServer;
using EntityMtwServer.Entities;

namespace MTWServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly MasterServerContext _context;

        public SessionsController(MasterServerContext context)
        {
            _context = context;
        }

        // GET: api/Sessions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Session>>> Get()
        {
            List<Session> sessions = await _context.Sessions.AsNoTracking().ToListAsync();
            return sessions;
        }

        // GET: api/Sessions/5
        [HttpGet("{startDate}/{endDate}")]
        public async Task<ActionResult<IEnumerable<Session>>> Get(DateTime startDate, DateTime endDate)
        {
            return await _context.Sessions.AsNoTracking().Where(x => x.StartDateTime >= startDate && x.EndDateTime <= endDate).Include(x => x.Students).Include(x => x.InstructorDevice).Include(x => x.Cells).Include(x => x.Instructor).Include(x => x.Equipments).Include(x => x.Class).ToListAsync();
        }

        [HttpGet("activeId/{state}")]
        public async Task<ActionResult<IEnumerable<Session>>> GetByState(bool state)
        {
            return await _context.Sessions.AsNoTracking().Where(x => x.Class.State == state).ToListAsync();
        }

        [HttpGet("instructorId/{id}")]
        public async Task<ActionResult<IEnumerable<Session>>> GetByInstructorId(long id)
        {
            return await _context.Sessions.AsNoTracking()
                .Where(x => x.Instructor.Id == id)
                .Where(x => x.Class.State)
                .Include(x => x.Students)
                .ThenInclude(x => x.User)
                .ToListAsync();
        }

        [HttpGet("deviceId/{id}")]
        public async Task<ActionResult<IEnumerable<Session>>> GetByDeviceId(long id)
        {
            return await _context.Sessions.AsNoTracking()
                .Where(x => x.Equipments.Select(x => x.Id).Contains(id))
                .Where(x => x.Class.State)
                .Include(x => x.Students)
                .ToListAsync();
        }

        [HttpGet("studentId/{id}")]
        public async Task<ActionResult<IEnumerable<Session>>> GetByStudentId(long id)
        {
            return await _context.Sessions.AsNoTracking()
                .Where(x => x.Students.Select(x => x.User.Id).Contains(id))
                .Where(x => x.Class.State)
                .Include(x => x.Students)
                .ToListAsync();
        }

        [HttpGet("activeSession/{id}")]
        public async Task<string> GetActiveString(long id)
        {
            Session session = await _context.Sessions.AsNoTracking()
              .Where(s => s.StartDateTime < DateTime.Now && s.EndDateTime > DateTime.Now)
              .Where(s => s.Class.State)
              .Where(s => s.Equipments.Select(x => x.Id).Contains(id))
              .FirstOrDefaultAsync();

            if (session != null)
                if (session.Id > 0)
                    return session.Id.ToString() + "_" + session.MainRtsp;

            return string.Empty;
        }

        [HttpGet("activeSubSession/{id}")]
        public async Task<string> GetActiveSubString(long id)
        {
            Session session = await _context.Sessions.AsNoTracking()
                .Where(s => s.StartDateTime < DateTime.Now && s.EndDateTime > DateTime.Now)
                .Where(s => s.Class.State)
                .Where(s => s.Students.Select(x => x.User.Id).Contains(id))
                .FirstOrDefaultAsync();

            if (session != null)
                if (session.Id > 0)
                    return session.Id.ToString() + "_" + session.SubRtsp;

            return string.Empty;

        }


        [HttpGet("requisition/{sessionId}/{id}")]
        public async Task<bool> GetRequisition(long sessionId, long id)
        {
            Session session = await _context.Sessions.AsNoTracking()
                .Where(s => s.Id == sessionId)
                .FirstOrDefaultAsync();

            if (session != null)
            {
                if (session.Id > 0)
                {
                    List<string> ids =  session.Requisition.Split(';').ToList();   
                    foreach(string idString in ids)
                    {
                        if (idString == id.ToString())
                            return true;
                    }
                }
            }

            return false;

        }

        // GET: api/Sessions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Session>> Get(long id)
        {
            var session = await _context.Sessions.AsNoTracking().Where(x => x.Id == id).Include(x => x.Students).Include(x => x.Instructor).Include(x => x.InstructorDevice).Include(x => x.Cells).Include(x => x.Equipments).Include(x => x.Class).FirstOrDefaultAsync();
            session.Instructor = await _context.Users.AsNoTracking().Where(x => x.Id == session.Instructor.Id).Include(x => x.Equipments).FirstOrDefaultAsync();

            if (session == null)
            {
                return NotFound();
            }

            return session;
        }

        // PUT: api/Sessions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Session>> Put(long id, Session session)
        {
            try
            {
                if (id != session.Id)
                {
                    return BadRequest();
                }


                if (session.StartDateTime < DateTime.Now)
                {
                    return ValidationProblem();
                }

                if (!await CheckForConflict(session))
                {
                    Session dbSession = await _context.Sessions.Where(x => x.Id == id).Include(x => x.Students).Include(x => x.Instructor).Include(x => x.InstructorDevice).Include(x => x.Cells).Include(x => x.Equipments).Include(x => x.Class).FirstOrDefaultAsync();
                    dbSession.Name = session.Name;
                    dbSession.Description = session.Description;
                    dbSession.Color = session.Color;
                    dbSession.StartDateTime = session.StartDateTime;
                    dbSession.EndDateTime = session.EndDateTime;
                    dbSession.MainRtsp = session.MainRtsp;
                    dbSession.SubRtsp = session.SubRtsp;

                    if (dbSession.Class != null)
                    {
                        if (dbSession.Class.Id > 0)
                        {
                            _context.Entry(dbSession).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                            return await Get(session.Id);
                        }
                    }

                    foreach (Student std in session.Students)
                        std.User = await _context.Users.Include(x => x.Equipments).Where(x => x.Id == std.User.Id).FirstOrDefaultAsync();

                    List<Group> cellsGroups = new List<Group>();
                    foreach (User u in _context.Users.Where(x => session.Students.Select(s => s.User.Id).Contains(x.Id)).Include(x => x.Groups).ToList())
                        foreach (Group g in u.Groups)
                            if (!cellsGroups.Select(x => x.Id).Contains(g.Id))
                                cellsGroups.Add(g);


                    dbSession.Cells = cellsGroups;
                    dbSession.Equipments = await _context.Equipments.Where(x => session.Students.Select(x => x.User.Equipments.First()).ToList().Select(e => e.Id).Contains(x.Id)).ToListAsync();

                    dbSession.Class = null;
                    dbSession.Students = await _context.Students.Where(x => session.Students.Select(y => y.Id).Contains(x.Id)).ToListAsync();

                    dbSession.Instructor = await _context.Users.Where(x => x.Id == session.Instructor.Id).FirstOrDefaultAsync();
                    dbSession.InstructorDevice = await _context.DVCs.Where(x => x.Id == session.InstructorDevice.Id).FirstOrDefaultAsync();

                    _context.Entry(dbSession).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return await Get(session.Id);
                }
                else
                {
                    return ValidationProblem();
                }
            }
            catch
            {
                return ValidationProblem();
            }
        }

        [HttpPut("requisition/{sessionId}/{id}")]
        public async Task<bool> Put(long sessionId, long id)
        {
            try
            {
                Session session = await _context.Sessions.Where(x => x.Id == sessionId).FirstOrDefaultAsync();
                List<string> ids = session.Requisition.Split(';').ToList();
                List<long> longIds = new List<long>();
                foreach (string idString in ids)
                {
                    long longId;
                    long.TryParse(idString, out longId);
                    if (!longIds.Contains(longId))
                        if (longId > 0)
                            longIds.Add(longId);
                }

                if (!longIds.Contains(id))
                    longIds.Add(id);

                session.Requisition = string.Join(";", longIds);
                _context.Entry(session).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }

        // POST: api/Sessions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Session>> Post(Session session)
        {
            try
            {
                if (!await CheckForConflict(session))
                {
                    foreach (Student std in session.Students)
                        std.User = await _context.Users.Include(x => x.Equipments).Where(x => x.Id == std.User.Id).FirstOrDefaultAsync();

                    List<Group> cellsGroups = _context.Users.Where(x => session.Students.Select(s => s.Id).Contains(x.Id)).Select(x => x.Groups.FirstOrDefault()).ToList();
                    foreach (User u in _context.Users.Where(x => session.Students.Select(s => s.User.Id).Contains(x.Id)).Include(x => x.Groups).ToList())
                        foreach (Group g in u.Groups)
                            if (!cellsGroups.Select(x => x.Id).Contains(g.Id))
                                cellsGroups.Add(g);
                    session.Cells = cellsGroups;
                    session.Equipments = session.Students.Select(x => x.User.Equipments.First()).ToList();
                    session.Class = null;

                    session.Instructor = await _context.Users.Where(x => x.Id == session.Instructor.Id).FirstOrDefaultAsync();
                    session.InstructorDevice = await _context.DVCs.Where(x => x.Id == session.InstructorDevice.Id).FirstOrDefaultAsync();

                    _context.Sessions.Add(session);
                    await _context.SaveChangesAsync();
                    return await Get(session.Id);
                }
                else
                {
                    return ValidationProblem();
                }
            }
            catch
            {
                return ValidationProblem();
            }


        }

        // DELETE: api/Sessions/5
        [HttpDelete("requisition/{sessionId}/{id}")]
        public async Task<bool> Delete(long sessionId, long id)
        {
            try
            {
                Session session = await _context.Sessions.Where(x => x.Id == sessionId).FirstOrDefaultAsync();
                List<string> ids = session.Requisition.Split(';').ToList();
                List<long> longIds = new List<long>();
                foreach (string idString in ids)
                {
                    long longId;
                    long.TryParse(idString, out longId);
                    if (!longIds.Contains(longId))
                        if (longId > 0 && longId != id)
                            longIds.Add(longId);
                }

             

                session.Requisition = string.Join(";", longIds);
                _context.Entry(session).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // DELETE: api/Sessions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            Session session = await _context.Sessions.Where(x => x.Id == id).Include(x => x.Students).Include(x => x.Instructor).Include(x => x.InstructorDevice).Include(x => x.Cells).Include(x => x.Equipments).Include(x => x.Class).FirstOrDefaultAsync();
            if (session == null)
            {
                return NotFound();
            }

            if (session.StartDateTime < DateTime.Now)
            {
                return ValidationProblem();
            }

            if (session.Class != null)
                if (session.Class.Id > 0)
                    return ValidationProblem();

            session.Equipments.Clear();
            session.Cells.Clear();
            session.Students.Clear();
            _context.Students.RemoveRange(session.Students);
            _context.Entry(session).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SessionExists(long id)
        {
            return _context.Sessions.Any(e => e.Id == id);
        }

        private async Task<bool> CheckForConflict(Session session)
        {
            List<User> users = await _context.Users.AsNoTracking().Include(x => x.Equipments).Where(x => session.Students.Select(u => u.User.Id).Contains(x.Id)).ToListAsync();
            List<DVC> dvcs = await _context.DVCs.Where(x => users.Select(u => u.Equipments.First().Id).Contains(x.Id)).ToListAsync();
            List<Session> sessions = await _context.Sessions
                .Include(x => x.Instructor)
                .Include(x => x.InstructorDevice)
                .Include(x => x.Equipments)
                .Where(x => x.Class.State)
                .ToListAsync();


            sessions = sessions.Where(x => x.Equipments.Where(m => dvcs.Select(d => d.Id).Contains(m.Id)).Count() > 0 || x.Instructor.Id == session.Instructor.Id || x.InstructorDevice.Id == session.InstructorDevice.Id).ToList();
            sessions = sessions.Where(x => !(x.EndDateTime < session.StartDateTime || x.StartDateTime > session.EndDateTime)).ToList();
            sessions = sessions.Where(x => x.Id != session.Id).ToList();


            return sessions.Count > 0;
        }


    }
}
