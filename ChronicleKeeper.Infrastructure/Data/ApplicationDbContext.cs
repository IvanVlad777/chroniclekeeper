using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ChronicleKeeper.Core.Entities.Characters.Abilities;
using ChronicleKeeper.Core.Entities.Characters.CharacterInfo;
using ChronicleKeeper.Core.Entities.Characters.Equipment;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Geography.Climate;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Animals;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Fungi;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Plants;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.Geography.Creatures;
using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Miscellaneous;
using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Entities.Social.Military;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Entities.Social.Politics;
using ChronicleKeeper.Core.Entities.Social.Religions;
using ChronicleKeeper.Core.Entities.Social.Structure;
using ChronicleKeeper.Core.Entities.Social;
using System.Reflection;
using ChronicleKeeper.Core.Entities.Content;
using ChronicleKeeper.Core.Entities.Content.Movie;
using ChronicleKeeper.Core.Entities.Content.Book;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ChronicleKeeper.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Base
        //public DbSet<LoreEntity> LoreEntities { get; set; }

        // Characters
        public DbSet<Character> Characters { get; set; }
        public DbSet<CharacterRelationship> CharacterRelationships { get; set; }

        // Abilities
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<AbilityLevel> AbilityLevels { get; set; }

        // Character Info
        public DbSet<Hobby> Hobbies { get; set; }

        // Equipment
        public DbSet<Item> Items { get; set; }
        public DbSet<OwnershipHistory> OwnershipHistories { get; set; }

        // Geography
        public DbSet<City> Cities { get; set; }
        public DbSet<Continent> Continents { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Landmark> Landmarks { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Region> Regions { get; set; }

        // Ecosystems
        public DbSet<Ecosystem> Ecosystems { get; set; }
        public DbSet<CaveEcosystem> CaveEcosystems { get; set; }
        public DbSet<DesertEcosystem> DesertEcosystems { get; set; }
        public DbSet<ForestEcosystem> ForestEcosystems { get; set; }
        public DbSet<GrasslandEcosystem> GrasslandEcosystems { get; set; }
        public DbSet<LakeEcosystem> LakeEcosystems { get; set; }
        public DbSet<MountainEcosystem> MountainEcosystems { get; set; }
        public DbSet<MountainRange> MountainRanges { get; set; }
        public DbSet<OceanEcosystem> OceanEcosystems { get; set; }
        public DbSet<RiverEcosystem> RiverEcosystems { get; set; }
        public DbSet<SeaEcosystem> SeaEcosystems { get; set; }
        public DbSet<SwampEcosystem> SwampEcosystems { get; set; }
        public DbSet<WaterEcosystem> WaterEcosystems { get; set; }

        // Climate
        public DbSet<ClimateDetail> ClimateDetails { get; set; }
        public DbSet<ClimateZone> ClimateZones { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<WeatherPattern> WeatherPatterns { get; set; }

        // Creatures
        public DbSet<Creature> Creatures { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Fungus> Fungi { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<Tree> Trees { get; set; }

        // Sapient Creatures
        public DbSet<Deity> Deities { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<SapientSpecies> SapientSpecies { get; set; }

        // History & Timelines
        public DbSet<History> Histories { get; set; }
        public DbSet<Timeline> Timelines { get; set; }
        public DbSet<TimelineEvent> TimelineEvents { get; set; }

        // Miscellaneous
        public DbSet<Mutation> Mutations { get; set; }

        // Professions
        public DbSet<Apprenticeship> Apprenticeships { get; set; }
        public DbSet<JobRank> JobRanks { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<TradeSchool> TradeSchools { get; set; }
        public DbSet<Specialisation> Specialisations { get; set; }

        // Social - Culture
        public DbSet<ArchitectureStyle> ArchitectureStyles { get; set; }
        public DbSet<ArtForm> ArtForms { get; set; }
        public DbSet<Cuisine> Cuisines { get; set; }
        public DbSet<Clothing> CulturalClothings { get; set; }
        public DbSet<CulturalFestival> CulturalFestivals { get; set; }
        public DbSet<CulturalInstitution> CulturalInstitutions { get; set; }
        public DbSet<Culture> Cultures { get; set; }
        public DbSet<Custom> Customs { get; set; }
        public DbSet<Folklore> Folklores { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Myth> Myths { get; set; }
        public DbSet<Tradition> Traditions { get; set; }

        // Social - Economy
        public DbSet<BankingSystem> BankingSystems { get; set; }
        public DbSet<Corporation> Corporations { get; set; }
        public DbSet<CorporateLeadership> CorporateLeaderships { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<EconomicSystem> EconomicSystems { get; set; }
        public DbSet<ExtractionMethod> ExtractionMethods { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildRank> GuildRanks { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<NaturalResource> NaturalResources { get; set; }
        public DbSet<TaxationSystem> TaxationSystems { get; set; }
        public DbSet<TradeRoute> TradeRoutes { get; set; }

        // Social - Education
        public DbSet<EducationSystem> EducationSystems { get; set; }
        public DbSet<EducationRecord> EducationRecords { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<ReligiousEducation> ReligiousEducations { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<SchoolSubject> SchoolSubjects { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<UniversityMajor> UniversityMajors { get; set; }

        // Social - Military
        public DbSet<Army> Armies { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<MilitaryDoctrine> MilitaryDoctrines { get; set; }
        public DbSet<MilitaryEquipment> MilitaryEquipment { get; set; }
        public DbSet<MilitaryOrganization> MilitaryOrganizations { get; set; }
        public DbSet<MilitaryRank> MilitaryRanks { get; set; }
        public DbSet<MilitaryUnit> MilitaryUnits { get; set; }

        // Social - Nationality
        public DbSet<Nation> Nations { get; set; }

        // Social - Politics
        //public DbSet<DiplomaticAgreement> DiplomaticAgreements { get; set; }
        public DbSet<Faction> Factions { get; set; }
        public DbSet<GovernmentSystem> GovernmentSystems { get; set; }
        public DbSet<LegalSystem> LegalSystems { get; set; }
        public DbSet<PoliticalIdeology> PoliticalIdeologies { get; set; }
        public DbSet<PoliticalParty> PoliticalParties { get; set; }

        // Social - Religions
        public DbSet<HolySite> HolySites { get; set; }
        public DbSet<Religion> Religions { get; set; }
        public DbSet<ReligiousFestival> ReligiousFestivals { get; set; }
        public DbSet<ReligiousOrder> ReligiousOrders { get; set; }
        public DbSet<ReligiousText> ReligiousTexts { get; set; }

        // Social - Structure
        public DbSet<PrivilegeLaw> PrivilegeLaws { get; set; }
        public DbSet<SocialClass> SocialClasses { get; set; }
        public DbSet<SocialHierarchy> SocialHierarchies { get; set; }

        // --------------------------------------------
        // Content
        public DbSet<Content> Content { get; set; }
        public DbSet<Reference> Reference { get; set; }
        public DbSet<Episode> Episode { get; set; }
        public DbSet<Chapter> Chapter { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    foreach (var foreignKey in entityType.GetForeignKeys())
            //    {
            //        foreignKey.DeleteBehavior = DeleteBehavior.Restrict; // Disable cascade delete
            //    }
            //}

            // Postavljanje veza - Configurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }


    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // ✅ Locate API project folder dynamically
            var currentDirectory = Directory.GetCurrentDirectory();
            var solutionDirectory = Directory.GetParent(currentDirectory)?.FullName;

            //var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../ChronicleKeeper.API"));
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "ChronicleKeeperAPI");
            basePath = Path.GetFullPath(basePath);

            var appSettingsPath = Path.Combine(basePath, "appsettings.json");
            Console.WriteLine($"📌 Using appsettings.json from: {appSettingsPath}");

            if (!File.Exists(appSettingsPath))
            {
                throw new FileNotFoundException($"Could not find 'appsettings.json' at '{appSettingsPath}'");
            }
            // ✅ Load configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // ✅ Read connection string from DatabaseSettings(not GetConnectionString)
            var usePostgreSQL = configuration.GetValue<bool>("DatabaseSettings:UsePostgreSQL");
            string connectionString = configuration.GetSection("DatabaseSettings").GetValue<string>("SqlServerConnection");

            //var connectionString = configuration.GetConnectionString("SqlServerConnection");

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
