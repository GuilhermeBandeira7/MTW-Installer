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
using System.Text.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using UtilsCore;
using uPLibrary.Networking.M2Mqtt;
using System.Net;
using uPLibrary.Networking.M2Mqtt.Messages;
using ApiMtwServer.Services;

namespace MTWServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LprRecordsController : ControllerBase
    {
        private readonly MasterServerContext _context;
        private readonly MqttService _mqttService;


        public LprRecordsController(MasterServerContext context, MqttService mqttService)
        {
            _context = context;
            _mqttService = mqttService;
        }

        // GET: api/LprRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LprRecord>>> GetLprRecords()
        {
            return await _context.LprRecords
                .AsNoTracking()
                .Include(x => x.Origin)
                .ToListAsync();
        }

        [HttpGet("filter/{records}")]
        public async Task<ActionResult<IEnumerable<LprRecord>>> GetLprRecords(long records)
        {
            return await _context.LprRecords
                .AsNoTracking()
                .Include(x => x.Origin)
                .OrderByDescending(x => x.Id).Take(Convert.ToInt32(records)).ToListAsync();
        }

        [HttpGet("filter/plate/{plate}")]
        public async Task<ActionResult<IEnumerable<LprRecord>>> GetLprRecords(string plate)
        {
            return await _context.LprRecords
                .AsNoTracking()
                .Include(x => x.Origin)
                .OrderByDescending(x => x.Id).Where(x => x.Plate.Contains(plate)).ToListAsync();
        }


        [HttpGet("filter/{startDateTime}/{endDateTime}/{originId}")]
        public async Task<ActionResult<IEnumerable<LprRecord>>> GetLprRecords(DateTime startDateTime, DateTime endDateTime, long originId)
        {
            if (startDateTime < endDateTime)
            {
                List<LprRecord> lprRecords = new List<LprRecord>();
                if (originId <= 0)
                    lprRecords = await _context.LprRecords.AsNoTracking()
                    .Include(x => x.Origin)
                    .Where(x => x.DateTime >= startDateTime && x.DateTime <= endDateTime)
                    .ToListAsync();
                else
                    lprRecords = await _context.LprRecords.AsNoTracking()
                    .Include(x => x.Origin)
                    .Where(x => x.DateTime >= startDateTime && x.DateTime <= endDateTime && x.Origin.Id == originId)
                    .ToListAsync();

                return lprRecords;

            }

            return new List<LprRecord>();
        }

        [HttpGet("filter/{startDateTime}/{endDateTime}/{originCode}/{originTitle}/{authorization}")]
        public async Task<ActionResult<IEnumerable<LprRecord>>> GetLprRecords(DateTime startDateTime, DateTime endDateTime, string originCode , string originTitle, string authorization)
        {
            if (startDateTime < endDateTime)
            {
                List<LprRecord> lprRecords = new List<LprRecord>();
                lprRecords = await _context.LprRecords.AsNoTracking()
                    .Include(x => x.Origin)
                    .Where(x => x.DateTime >= startDateTime && x.DateTime <= endDateTime)
                    .ToListAsync();

                if (originCode != "empty")
                    lprRecords = lprRecords.Where(x => x.Origin.OriginCode == originCode).ToList();

                if(originTitle != "empty")
                    lprRecords = lprRecords.Where(x => x.Origin.Title == originTitle).ToList();

                if(authorization != "empty")
                    lprRecords = lprRecords.Where(x => x.Authorization == authorization).ToList();


                return lprRecords;

            }

            return new List<LprRecord>();
        }


        // GET: api/LprRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LprRecord>> GetLprRecord(long id)
        {
            List<string> files = new List<string>();
            var lprRecord = await _context.LprRecords
                .AsNoTracking()
                .Include(x => x.Origin)
                .Include(x => x.Permanent)
                .Include(x => x.Visitor)
                .Include(x => x.Lpr)

                .Include(x => x.Lpr.Acess)
                .Include(x => x.Lpr.Context1)
                .Include(x => x.Lpr.Context2)
                .Include(x => x.Lpr.Context3)
                .Include(x => x.Lpr.Context4)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (lprRecord != null)
            {
                if (lprRecord.Lpr.Context1 != null)
                    files.Add(CreateContextFile(lprRecord, lprRecord.Lpr.Context1.Id.ToString()));

                if (lprRecord.Lpr.Context2 != null)
                    files.Add(CreateContextFile(lprRecord, lprRecord.Lpr.Context2.Id.ToString()));

                if (lprRecord.Lpr.Context3 != null)
                    files.Add(CreateContextFile(lprRecord, lprRecord.Lpr.Context3.Id.ToString()));

                if (lprRecord.Lpr.Context4 != null)
                    files.Add(CreateContextFile(lprRecord, lprRecord.Lpr.Context4.Id.ToString()));

                lprRecord.Files = files;
            }

            if (lprRecord == null)
            {
                return NotFound();
            }

            return lprRecord;
        }


        [HttpGet("{startDateTime}/{endDateTime}")]
        public async Task<ActionResult<IEnumerable<LprRecord>>> GetLprRecordByDateTime(DateTime startDateTime, DateTime endDateTime)
        {
            var lprRecords = await _context.LprRecords
              .AsNoTracking()
              .Include(x => x.Lpr.Context1)
              .Include(x => x.Lpr.Context2)
              .Include(x => x.Lpr.Context3)
              .Include(x => x.Lpr.Context4)
              .Where(x => x.DateTime >= startDateTime && x.DateTime <= endDateTime).ToListAsync();

            foreach(LprRecord lprRecord in lprRecords)
            {
                if (lprRecord.Lpr.Context1 != null)
                    CreateContextFile(lprRecord, lprRecord.Lpr.Context1.Id.ToString(), 100);

                if (lprRecord.Lpr.Context2 != null)
                    CreateContextFile(lprRecord, lprRecord.Lpr.Context2.Id.ToString(), 100);

                if (lprRecord.Lpr.Context3 != null)
                    CreateContextFile(lprRecord, lprRecord.Lpr.Context3.Id.ToString(), 100);

                if (lprRecord.Lpr.Context4 != null)
                    CreateContextFile(lprRecord, lprRecord.Lpr.Context4.Id.ToString(), 100);
            }

            return new List<LprRecord>();
        }
        // PUT: api/LprRecords/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<LprRecord>> PutLprRecord(long id, LprRecord lprRecord)
        {
            if (id != lprRecord.Id)
            {
                return BadRequest();
            }

            LprRecord lprRecordToEdit = _context.LprRecords
                .Where(x => x.Id == id).FirstOrDefault();

            lprRecordToEdit.Plate = lprRecord.Plate;
            VehicleModel recordVehicleModel = await GetVehicleModel(lprRecord);
            lprRecordToEdit.Authorization = await GetPlateAuthorization(lprRecord, recordVehicleModel);


            _context.Entry(lprRecordToEdit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LprRecordExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLprRecord", new { id = lprRecord.Id }, lprRecord);
        }

        // POST: api/LprRecords
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LprRecord>> PostLprRecord(LprRecord lprRecord)
        {
            string json = JsonSerializer.Serialize(lprRecord);
            if (lprRecord != null)
                lprRecord.Lpr = await _context.Lprs.Where(x => x.Id == lprRecord.Lpr.Id).FirstOrDefaultAsync();

            if (lprRecord.Visitor != null)
                lprRecord.Visitor = await _context.Visitors.Where(x => x.Plate == lprRecord.Plate).FirstOrDefaultAsync();

            if (lprRecord.Permanent != null)
                lprRecord.Permanent = await _context.Permanents.Where(x => x.Plate == lprRecord.Plate).FirstOrDefaultAsync();

            VehicleModel recordVehicleModel = await GetVehicleModel(lprRecord);
            lprRecord.Authorization = await GetPlateAuthorization(lprRecord, recordVehicleModel);

            if (lprRecord.Authorization == "BL")
            {
                Origin origin = await _context.Origins.AsNoTracking().Include(x => x.Telemetry).Where(x => x.Id == lprRecord.Origin.Id).FirstOrDefaultAsync();
                Task.Run(() => _mqttService.SendTelemetryAlarm(origin));
            }

            lprRecord.Origin = await _context.Origins.Where(x => x.Id == lprRecord.Origin.Id).FirstOrDefaultAsync();
            _context.LprRecords.Add(lprRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLprRecord", new { id = lprRecord.Id }, lprRecord);

        }

        private async Task<string> GetPlateAuthorization(LprRecord lprRecord, VehicleModel recordVehicleModel)
        {
            RestrictedPlate restrictedPlate = null;
            bool alarm = false;
            string authorization = "NA";
            int charCounter = 5;
            int.TryParse(System.IO.File.ReadAllText("charCounter.txt"), out charCounter);
            if (!string.IsNullOrEmpty(recordVehicleModel.SubModel))
            {
                restrictedPlate = await _context.RestrictedPlates.Where(x => x.VehicleModel.SubModel.Contains(recordVehicleModel.SubModel)).FirstOrDefaultAsync();
            }

            if (restrictedPlate == null)
            {
                
                List<RestrictedPlate> restrictedPlates = await _context.RestrictedPlates.ToListAsync();
                foreach (RestrictedPlate restricted in restrictedPlates)
                {
                    int alarmCounter = 0;
                    for (int counter = 0; counter < restricted.Plate.Length; counter++)
                    {
                        if (lprRecord.Plate[counter] == restricted.Plate[counter])
                            alarmCounter++;
                    }

                    if (alarmCounter >= charCounter)
                    {
                        restrictedPlate = restricted;
                        alarm = true;
                        break;
                    }

                }

            }


            if (restrictedPlate != null)
                authorization = "BL";
            else if (lprRecord.Visitor != null || lprRecord.Permanent != null)
                authorization = "AU";
            else
                authorization = "NA";

            return authorization;

        }


        // DELETE: api/LprRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLprRecord(long id)
        {
            var lprRecord = await _context.LprRecords.FindAsync(id);
            if (lprRecord == null)
            {
                return NotFound();
            }

            _context.LprRecords.Remove(lprRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LprRecordExists(long id)
        {
            return _context.LprRecords.Any(e => e.Id == id);
        }


        private async Task<VehicleModel> GetVehicleModel(LprRecord lprRecord)
        {

            VehicleModel recordVehicleModel = new VehicleModel();

            try
            {
                if (_context.Vehicles.Any(x => x.Plate == lprRecord.Plate))
                {
                    var vehicle = await _context.Vehicles.Include(x => x.VehicleModel).Where(x => x.Plate == lprRecord.Plate).FirstOrDefaultAsync();
                    recordVehicleModel = vehicle.VehicleModel;
                }
                else
                {

                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
                    client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
                    string req = "https://wdapi.com.br/placas/" + lprRecord.Plate + "/5a87c8e9f266f2666ca6ac8ec89851d0";

                    var msg = await client.GetStringAsync(req);
                    VehicleModelApi vehicleModelApi = JsonSerializer.Deserialize<VehicleModelApi>(msg);
                    VehicleModel vehicleModel = await _context.VehiclesModels
                        .Where(x => x.Brand == vehicleModelApi.MARCA &&
                            x.Model == vehicleModelApi.MODELO &&
                            x.SubModel == vehicleModelApi.SUBMODELO &&
                            x.Year.ToString() == vehicleModelApi.ano).FirstOrDefaultAsync();

                    if (vehicleModelApi.MARCA != null && vehicleModelApi.MODELO != string.Empty)
                    {
                        if (vehicleModel != null)
                        {
                            if (vehicleModel.Id <= 0)
                            {
                                vehicleModel = new VehicleModel();
                                vehicleModel.Brand = vehicleModelApi.MARCA;
                                vehicleModel.Model = vehicleModelApi.MODELO;
                                vehicleModel.SubModel = vehicleModelApi.SUBMODELO;
                                int year = 0;
                                int.TryParse(vehicleModelApi.ano, out year);
                                vehicleModel.Year = year;
                                vehicleModel.Version = "";
                                vehicleModel.Color = "";
                                _context.VehiclesModels.Add(vehicleModel);
                                recordVehicleModel = vehicleModel;
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            vehicleModel = new VehicleModel();
                            vehicleModel.Brand = vehicleModelApi.MARCA;
                            vehicleModel.Model = vehicleModelApi.MODELO;
                            vehicleModel.SubModel = vehicleModelApi.SUBMODELO;
                            int year = 0;
                            int.TryParse(vehicleModelApi.ano, out year);
                            vehicleModel.Year = year;
                            vehicleModel.Version = "";
                            vehicleModel.Color = "";
                            _context.VehiclesModels.Add(vehicleModel);
                            recordVehicleModel = vehicleModel;
                            await _context.SaveChangesAsync();
                        }


                        Vehicle vehicle = new Vehicle();
                        vehicle.Plate = lprRecord.Plate;
                        vehicle.VehicleModel = await _context.VehiclesModels.Where(x => x.Id == vehicleModel.Id).FirstOrDefaultAsync();
                        recordVehicleModel = vehicle.VehicleModel;
                        _context.Vehicles.Add(vehicle);
                        await _context.SaveChangesAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(lprRecord.Plate);
            }

            return recordVehicleModel;
        }



        void ExecuteCmd(string cmd)
        {
            Process processReload = new Process();
            processReload.StartInfo.FileName = "/bin/bash";
            processReload.StartInfo.Verb = "runas";
            processReload.StartInfo.Arguments = $"-c \"" + cmd + "\"";
            processReload.Start();
        }

        string CreateContextFile(LprRecord record, string recordId)
        {
            string endFile = string.Empty;
            string startFile = string.Empty;
            string name = recordId + "_" + record.DateTime.ToString("yyyyMMddHHmmss") + record.Plate;

            if (!Directory.Exists("/mnt/sdb/" + name))
            {
                if(Directory.Exists("/mnt/sdb/" + recordId + "_Record"))
                {
                    DateTime startDateTime = record.DateTime.AddSeconds(-30);

                    while (startDateTime < record.DateTime.AddSeconds(30))
                    {
                        string fileName = "/mnt/sdb/" + recordId + "_Record/" + startDateTime.ToString("yyyyMMddHHmmss") + ".mkv";

                        if (startDateTime < record.DateTime)
                        {
                            if (System.IO.File.Exists(fileName))
                                startFile = fileName;
                        }
                        else if (startDateTime >= record.DateTime)
                        {
                            if (endFile == string.Empty)
                                if (System.IO.File.Exists(fileName))
                                    endFile = fileName;
                        }

                        startDateTime = startDateTime.AddSeconds(1);
                        //Thread.Sleep(100);
                    }

                    ExecuteCmd("echo 'Senha@mtw' | sudo -S mkdir /mnt/sdb/" + name);
                    string cmd = "echo 'Senha@mtw' | sudo -S ffmpeg -i " + startFile + " -i " + endFile + " -filter_complex \"[0:v:0][1:v:0]concat=n=2:v=1:a=0[outv]\" -map \"[outv]\" \"/mnt/sdb/" + name + "/" + name + ".mkv\"";
                    ExecuteCmd(cmd);
                    Thread.Sleep(5000);
                }
       
            }

            //-------------------------------------------------------


            if (!Directory.Exists("/mnt/sdc/" + name))
            {
                if(Directory.Exists("/mnt/sdc/" + recordId + "_Record"))
                {
                    DateTime startDateTime = record.DateTime.AddSeconds(-30);

                    while (startDateTime < record.DateTime.AddSeconds(30))
                    {
                        string fileName = "/mnt/sdc/" + recordId + "_Record/" + startDateTime.ToString("yyyyMMddHHmmss") + ".mkv";

                        if (startDateTime < record.DateTime)
                        {
                            if (System.IO.File.Exists(fileName))
                                startFile = fileName;
                        }
                        else if (startDateTime >= record.DateTime)
                        {
                            if (endFile == string.Empty)
                                if (System.IO.File.Exists(fileName))
                                    endFile = fileName;
                        }

                        startDateTime = startDateTime.AddSeconds(1);
                        Thread.Sleep(100);
                    }


                    ExecuteCmd("echo 'Senha@mtw' | sudo -S mkdir /mnt/sdc/" + name);

                    string cmd = "echo 'Senha@mtw' | sudo -S ffmpeg -i " + startFile + " -i " + endFile + " -filter_complex \"[0:v:0][1:v:0]concat=n=2:v=1:a=0[outv]\" -map \"[outv]\" \"/mnt/sdc/" + name + "/" + name + ".mkv\"";
                    ExecuteCmd(cmd);
                    Thread.Sleep(5000);
                }
          
            }

            //-------------------------------------------------------

            if (!Directory.Exists("/mnt/sdd/" + name))
            {
                if(Directory.Exists("/mnt/sdd/" + recordId + "_Record"))
                {
                    DateTime startDateTime = record.DateTime.AddSeconds(-30);

                    while (startDateTime < record.DateTime.AddSeconds(30))
                    {
                        string fileName = "/mnt/sdd/" + recordId + "_Record/" + startDateTime.ToString("yyyyMMddHHmmss") + ".mkv";

                        if (startDateTime < record.DateTime)
                        {
                            if (System.IO.File.Exists(fileName))
                                startFile = fileName;
                        }
                        else if (startDateTime >= record.DateTime)
                        {
                            if (endFile == string.Empty)
                                if (System.IO.File.Exists(fileName))
                                    endFile = fileName;
                        }


                        startDateTime = startDateTime.AddSeconds(1);
                        Thread.Sleep(100);
                    }


                    ExecuteCmd("echo 'Senha@mtw' | sudo -S mkdir /mnt/sdd/" + name);

                    string cmd = "echo 'Senha@mtw' | sudo -S ffmpeg -i " + startFile + " -i " + endFile + " -filter_complex \"[0:v:0][1:v:0]concat=n=2:v=1:a=0[outv]\" -map \"[outv]\" \"/mnt/sdd/" + name + "/" + name + ".mkv\"";
                    ExecuteCmd(cmd);
                    Thread.Sleep(5000);
                }
           
            }





            return name + "/" + name + ".mkv";

        }

        string CreateContextFile(LprRecord record, string recordId, int sleepTime)
        {
            string endFile = string.Empty;
            string startFile = string.Empty;
            string name = recordId + "_" + record.DateTime.ToString("yyyyMMddHHmmss") + record.Plate;

            if (!Directory.Exists("/mnt/sdb/" + name))
            {
                if (Directory.Exists("/mnt/sdb/" + recordId + "_Record"))
                {
                    DateTime startDateTime = record.DateTime.AddSeconds(-30);

                    while (startDateTime < record.DateTime.AddSeconds(30))
                    {
                        string fileName = "/mnt/sdb/" + recordId + "_Record/" + startDateTime.ToString("yyyyMMddHHmmss") + ".mkv";

                        if (startDateTime < record.DateTime)
                        {
                            if (System.IO.File.Exists(fileName))
                                startFile = fileName;
                        }
                        else if (startDateTime >= record.DateTime)
                        {
                            if (endFile == string.Empty)
                                if (System.IO.File.Exists(fileName))
                                    endFile = fileName;
                        }

                        startDateTime = startDateTime.AddSeconds(1);
                    }

                    ExecuteCmd("echo 'Senha@mtw' | sudo -S mkdir /mnt/sdb/" + name);
                    string cmd = "echo 'Senha@mtw' | sudo -S ffmpeg -i " + startFile + " -i " + endFile + " -filter_complex \"[0:v:0][1:v:0]concat=n=2:v=1:a=0[outv]\" -map \"[outv]\" \"/mnt/sdb/" + name + "/" + name + ".mkv\"";
                    ExecuteCmd(cmd);
                    Thread.Sleep(sleepTime);
                }

            }

            //-------------------------------------------------------


            if (!Directory.Exists("/mnt/sdc/" + name))
            {
                if (Directory.Exists("/mnt/sdc/" + recordId + "_Record"))
                {
                    DateTime startDateTime = record.DateTime.AddSeconds(-30);

                    while (startDateTime < record.DateTime.AddSeconds(30))
                    {
                        string fileName = "/mnt/sdc/" + recordId + "_Record/" + startDateTime.ToString("yyyyMMddHHmmss") + ".mkv";

                        if (startDateTime < record.DateTime)
                        {
                            if (System.IO.File.Exists(fileName))
                                startFile = fileName;
                        }
                        else if (startDateTime >= record.DateTime)
                        {
                            if (endFile == string.Empty)
                                if (System.IO.File.Exists(fileName))
                                    endFile = fileName;
                        }

                        startDateTime = startDateTime.AddSeconds(1);
                    }


                    ExecuteCmd("echo 'Senha@mtw' | sudo -S mkdir /mnt/sdc/" + name);

                    string cmd = "echo 'Senha@mtw' | sudo -S ffmpeg -i " + startFile + " -i " + endFile + " -filter_complex \"[0:v:0][1:v:0]concat=n=2:v=1:a=0[outv]\" -map \"[outv]\" \"/mnt/sdc/" + name + "/" + name + ".mkv\"";
                    ExecuteCmd(cmd);
                    Thread.Sleep(sleepTime);
                }

            }

            //-------------------------------------------------------

            if (!Directory.Exists("/mnt/sdd/" + name))
            {
                if (Directory.Exists("/mnt/sdd/" + recordId + "_Record"))
                {
                    DateTime startDateTime = record.DateTime.AddSeconds(-30);

                    while (startDateTime < record.DateTime.AddSeconds(30))
                    {
                        string fileName = "/mnt/sdd/" + recordId + "_Record/" + startDateTime.ToString("yyyyMMddHHmmss") + ".mkv";

                        if (startDateTime < record.DateTime)
                        {
                            if (System.IO.File.Exists(fileName))
                                startFile = fileName;
                        }
                        else if (startDateTime >= record.DateTime)
                        {
                            if (endFile == string.Empty)
                                if (System.IO.File.Exists(fileName))
                                    endFile = fileName;
                        }


                        startDateTime = startDateTime.AddSeconds(1);
                    }


                    ExecuteCmd("echo 'Senha@mtw' | sudo -S mkdir /mnt/sdd/" + name);

                    string cmd = "echo 'Senha@mtw' | sudo -S ffmpeg -i " + startFile + " -i " + endFile + " -filter_complex \"[0:v:0][1:v:0]concat=n=2:v=1:a=0[outv]\" -map \"[outv]\" \"/mnt/sdd/" + name + "/" + name + ".mkv\"";
                    ExecuteCmd(cmd);
                    Thread.Sleep(sleepTime);
                }

            }





            return name + "/" + name + ".mkv";

        }



    }
}
