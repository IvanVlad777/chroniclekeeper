using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ChronicleKeeper.Core.Entities.Characters;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ChronicleKeeper.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Characters
        public DbSet<Character> Characters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Using configurations from the executing assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Locate API project folder dynamically
            var currentDirectory = Directory.GetCurrentDirectory();
            var solutionDirectory = Directory.GetParent(currentDirectory)?.FullName;

            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "ChronicleKeeperAPI");
            basePath = Path.GetFullPath(basePath);

            var appSettingsPath = Path.Combine(basePath, "appsettings.json");

            if (!File.Exists(appSettingsPath))
            {
                throw new FileNotFoundException($"Could not find 'appsettings.json' at '{appSettingsPath}'");
            }

            // Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Read connection string from DatabaseSettings
            var usePostgreSQL = configuration.GetValue<bool>("DatabaseSettings:UsePostgreSQL");
            string connectionString = configuration.GetSection("DatabaseSettings").GetValue<string>("SqlServerConnection");

            Console.WriteLine($"📌 Found connection string: {connectionString}");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is missing in appsettings.json.");
            }
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}