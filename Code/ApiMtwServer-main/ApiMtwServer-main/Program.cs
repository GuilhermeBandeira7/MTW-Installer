using ApiMtwServer.Services;
using EntityMtwServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MTWServerApi.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<MasterServerContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<EquipmentsService>();
builder.Services.AddScoped<GroupServices>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<TelemetryServices>();
builder.Services.AddSingleton<MqttService>();
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions 
    .ReferenceHandler = ReferenceHandler.IgnoreCycles); 
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


builder.Services.AddCors(option =>
{
    option.AddPolicy(name: MyAllowSpecificOrigins,
    policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();

    });
});

builder.Services.AddControllers(); 

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseHttpsRedirection(); 
app.UseStaticFiles();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
