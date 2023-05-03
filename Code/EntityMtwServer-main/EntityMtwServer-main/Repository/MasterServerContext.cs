using EntityMtwServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityMtwServer
{
    public class MasterServerContext : DbContext
    {
        public DbSet<Entities.Action> Actions { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Permanent> Permanents { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<RestrictedPlate> RestrictedPlates { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CameraControlAlarm> CameraControlAlarms { get; set; }
        public DbSet<TelemetryAlarm> TelemetryAlarms { get; set; }
        public DbSet<UserAlarm> UserAlarms { get; set; }
        public DbSet<CameraControl> CameraControls { get; set; }
        public DbSet<DVC> DVCs { get; set; }
        public DbSet<Gateway> Gateways { get; set; }
        public DbSet<Lprs> Lprs { get; set; }
        public DbSet<LprRecord> LprRecords { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Telemetry> Telemetries { get; set; }
        public DbSet<TelemetryMessage> TelemetryMessages { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CurriculumCourse> CurriculumCourses { get; set; }
        public DbSet<TypeField> TypeFields { get; set; }
        public DbSet<VehicleModel> VehiclesModels { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Log> Logs { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=10.1.1.31;Database=MasterServerLpr;User id=sa;Password=Senha@mtw;Trusted_Connection=False", builder =>
            //optionsBuilder.UseSqlServer(@"Server=172.16.2.218;Database=MasterServerSiris;User id=sa;Password=Senha@mtw;Trusted_Connection=False", builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
       
        }

    }
}
