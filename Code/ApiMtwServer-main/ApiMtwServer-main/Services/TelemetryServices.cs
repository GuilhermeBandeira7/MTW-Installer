using EntityMtwServer;
using EntityMtwServer.Entities;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MTWServerApi.Services
{
    public class TelemetryServices
    {
        public MasterServerContext _context;

        public TelemetryServices(MasterServerContext context)
        {
            _context = context;
        }

        // GET ALL TELEMETRIES
        public async Task<ActionResult<IEnumerable<Telemetry>>> ReadTelemetries()
        {
            return await _context.Telemetries.Include(x => x.Gateways).ToListAsync();
        }
        
        // GET TELEMETRY BY ID
        public async Task<Telemetry> GetTelemetryById(long id)
        {            
            return await _context.Telemetries.Include(x => x.Gateways).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        // ADD TELEMETRY
        public async Task<Result<Telemetry>> AddTelemetry(Telemetry telemetry)
        {
            for (int counter = 0; counter < telemetry.Gateways.Count; counter++)
                if (telemetry.Gateways.ElementAt(counter).Id <= 0)
                    telemetry.Gateways.Remove(telemetry.Gateways.ElementAt(counter));

            _context.Database.OpenConnection();
            try
            {
                string query = "INSERT INTO [dbo].[Telemetry] ([Id],[SerialNumber],[VirtualInputs]) " +
                    "VALUES(" + Convert.ToInt32(telemetry.Id) + ",'" + telemetry.SerialNumber + "', '" + telemetry.VirtualInputs + "')";
                _context.Database.ExecuteSqlRaw(query);
            }
            finally
            {
                _context.Database.CloseConnection();
            }

            Telemetry? t = _context.Telemetries.Where(x => x.Id == telemetry.Id).Include(x => x.Gateways).FirstOrDefault();

            foreach (Gateway gateway in telemetry.Gateways)
                t.Gateways.Add(gateway);

            _context.Entry(t).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Result.Ok(telemetry);
        }

        // PUT TELEMETRY
        public async Task<Result<Telemetry>> UpdateTelemetry(long telemtryId, Telemetry telemetry)
        {
            Telemetry? t = _context.Telemetries.Where(x => x.Id == telemetry.Id).Include(x => x.Gateways).FirstOrDefault();
            t.Gateways.Clear();

            foreach (Gateway gateway in telemetry.Gateways)
            {
                t.Gateways.Add(gateway);
            }
                
            System.Type myType = telemetry.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                prop.SetValue(t, prop.GetValue(telemetry, null), null);
            }

            t.Gateways.Remove(t.Gateways.Last());

            _context.Entry(t).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TelemetryExists(telemtryId))
                {
                    return Result.Fail("Failed to update telemetry");
                }
                else
                {
                    throw;
                }
            }

            return Result.Ok();
        }

        // DELETE TELEMETRY
        public async Task<Result<Telemetry>> DeleteTelemtry(long telemtryId)
        {
            var telemetries = _context.Telemetries.Where(x => x.Id == telemtryId).FirstOrDefault();
            bool result = (telemetries != null ? true : false);
            if(result == true)
            {
                _context.Telemetries.Remove(telemetries);
                _context.Entry(telemetries).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            return Result.Fail("Failed to remove Telemetry.");
        }

        private bool TelemetryExists(long id)
        {
            return _context.Telemetries.Any(e => e.Id == id);
        }
    }
}
