using EntityMtwServer;
using Microsoft.EntityFrameworkCore;
using MTWServerApi.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<MasterServerContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<EquipmentsService>();
builder.Services.AddScoped<GroupServices>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions
    .ReferenceHandler = ReferenceHandler.IgnoreCycles); //handling cycles


var app = builder.Build();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin());


if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
