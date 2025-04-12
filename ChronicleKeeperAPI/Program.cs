using ChronicleKeeper.Infrastructure.Data;
using ChronicleKeeper.API.ServiceExtensions;
using Serilog;

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
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

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
app.UseAuthorization();
app.MapControllers();

//  Seed Database (Roles & SuperAdmin)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbSeeder.SeedRolesAndAdmin(services);
}

app.Run();
