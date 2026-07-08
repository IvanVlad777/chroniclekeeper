using ChronicleKeeper.Infrastructure.Data;
using ChronicleKeeper.API.ServiceExtensions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ChronicleKeeperAPI.Mapping;
using ChronicleKeeperAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

var logFilePath = builder.Configuration["Logging:LogFilePath"] ?? "C:/logs/app.log";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();
Log.Information("Logger initialized successfully!");

//  Modular Service Configuration
builder.Services.AddDatabaseConfiguration(builder.Configuration);
var debugConnString = builder.Configuration.GetSection("DatabaseSettings:SqlServerConnection").Value;
Console.WriteLine("➡️ Koristi se connection string: " + debugConnString);

builder.Services.AddIdentityConfiguration();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerConfiguration();
builder.Services.AddCustomAuthorization();
builder.Services.AddMediatRConfiguration();
builder.Services.AddMappingProfiles();
// DTOs su bez ciklusa pa nema potrebe za ReferenceHandler.Preserve ($id/$values u JSON-u)
builder.Services.AddControllers();

var app = builder.Build();

//  Middleware
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ChronicleKeeper API V1");
        options.RoutePrefix = string.Empty; // Makes Swagger UI available at `/`
    });
}

app.UseAuthentication();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();
app.MapControllers();

//  Apply migrations & Seed Database (Roles, SuperAdmin, Demo World)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();

    await DbSeeder.SeedRolesAndAdmin(services);
    await DbSeeder.SeedDemoWorld(services);
}

app.Run();
