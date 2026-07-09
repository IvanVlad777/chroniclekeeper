using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Characters.CharacterInfo;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Notes;
using ChronicleKeeper.Core.Entities.Social;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Entities.Social.Structure;
using ChronicleKeeper.Core.Entities.Tags;
using ChronicleKeeper.Core.Entities.Worlds;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ChronicleKeeper.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Worlds (aggregate root)
        public DbSet<World> Worlds { get; set; }

        // Characters
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterRelationship> CharacterRelationships { get; set; }

        // Geography
        public DbSet<Location> Locations { get; set; }

        // Species
        public DbSet<SapientSpecies> SapientSpecies { get; set; }
        public DbSet<Race> Races { get; set; }

        // Factions
        public DbSet<Faction> Factions { get; set; }
        public DbSet<FactionMember> FactionMembers { get; set; }

        // Timelines
        public DbSet<Timeline> Timelines { get; set; }
        public DbSet<TimelineEvent> TimelineEvents { get; set; }

        // Tags
        public DbSet<Tag> Tags { get; set; }
        public DbSet<CharacterTag> CharacterTags { get; set; }
        public DbSet<LocationTag> LocationTags { get; set; }
        public DbSet<FactionTag> FactionTags { get; set; }

        // Notes
        public DbSet<Note> Notes { get; set; }

        // Social
        public DbSet<SocialClass> SocialClasses { get; set; }
        public DbSet<Nation> Nations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Using configurations from this assembly (Infrastructure)
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        // Overridamo bool overloade: kraći SaveChanges()/SaveChangesAsync(ct) interno
        // delegiraju u njih, pa su ovime pokriveni SVI putevi spremanja.
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            StampTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            StampTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void StampTimestamps()
        {
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is LoreEntity lore)
                {
                    if (entry.State == EntityState.Added) lore.CreatedAt = now;
                    if (entry.State is EntityState.Added or EntityState.Modified) lore.UpdatedAt = now;
                }
                else if (entry.Entity is World world)
                {
                    if (entry.State == EntityState.Added) world.CreatedAt = now;
                    if (entry.State is EntityState.Added or EntityState.Modified) world.UpdatedAt = now;
                }
            }
        }
    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Locate API project folder dynamically
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "ChronicleKeeperAPI");
            basePath = Path.GetFullPath(basePath);

            var appSettingsPath = Path.Combine(basePath, "appsettings.json");
            if (!File.Exists(appSettingsPath))
            {
                throw new FileNotFoundException($"Could not find 'appsettings.json' at '{appSettingsPath}'");
            }

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetSection("DatabaseSettings").GetValue<string>("SqlServerConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is missing in appsettings.json.");
            }

            optionsBuilder.UseSqlServer(connectionString);
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
