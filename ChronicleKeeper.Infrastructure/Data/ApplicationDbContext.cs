using ChronicleKeeper.Core.Entities.Base;
using ChronicleKeeper.Core.Entities.Characters;
using ChronicleKeeper.Core.Entities.Characters.Abilities;
using ChronicleKeeper.Core.Entities.Characters.CharacterInfo;
using ChronicleKeeper.Core.Entities.Characters.Equipment;
using ChronicleKeeper.Core.Entities.Content;
using ChronicleKeeper.Core.Entities.Content.Article;
using ChronicleKeeper.Core.Entities.Content.Book;
using ChronicleKeeper.Core.Entities.Content.Movie;
using ChronicleKeeper.Core.Entities.Geography;
using ChronicleKeeper.Core.Entities.Geography.Climate;
using ChronicleKeeper.Core.Entities.Geography.Creatures;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Animals;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Fungi;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Plants;
using ChronicleKeeper.Core.Entities.Geography.Creatures.Sapient;
using ChronicleKeeper.Core.Entities.Geography.Ecosystems;
using ChronicleKeeper.Core.Entities.HistoryTimelines;
using ChronicleKeeper.Core.Entities.Notes;
using ChronicleKeeper.Core.Entities.Professions;
using ChronicleKeeper.Core.Entities.Social;
using ChronicleKeeper.Core.Entities.Social.Cultures;
using ChronicleKeeper.Core.Entities.Social.Economy;
using ChronicleKeeper.Core.Entities.Social.Education;
using ChronicleKeeper.Core.Entities.Social.Military;
using ChronicleKeeper.Core.Entities.Social.Nationality;
using ChronicleKeeper.Core.Entities.Social.Politics;
using ChronicleKeeper.Core.Entities.Social.Religions;
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
        public DbSet<Continent> Continents { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<RegionSapientSpecies> RegionSapientSpecies { get; set; }

        // Ecosystems (TPH subtypes of Location)
        public DbSet<Ecosystem> Ecosystems { get; set; }
        public DbSet<LakeEcosystem> LakeEcosystems { get; set; }
        public DbSet<SeaEcosystem> SeaEcosystems { get; set; }
        public DbSet<OceanEcosystem> OceanEcosystems { get; set; }
        public DbSet<RiverEcosystem> RiverEcosystems { get; set; }
        public DbSet<MountainEcosystem> MountainEcosystems { get; set; }
        public DbSet<MountainRange> MountainRanges { get; set; }
        public DbSet<SwampEcosystem> SwampEcosystems { get; set; }
        public DbSet<DesertEcosystem> DesertEcosystems { get; set; }
        public DbSet<ForestEcosystem> ForestEcosystems { get; set; }
        public DbSet<CaveEcosystem> CaveEcosystems { get; set; }
        public DbSet<GrasslandEcosystem> GrasslandEcosystems { get; set; }

        // Climate
        public DbSet<ClimateZone> ClimateZones { get; set; }
        public DbSet<ClimateDetail> ClimateDetails { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<WeatherPattern> WeatherPatterns { get; set; }
        public DbSet<ClimateZoneDetail> ClimateZoneDetails { get; set; }
        public DbSet<ClimateZoneSeason> ClimateZoneSeasons { get; set; }
        public DbSet<LocationClimateZone> LocationClimateZones { get; set; }

        // Species
        public DbSet<SapientSpecies> SapientSpecies { get; set; }
        public DbSet<Race> Races { get; set; }

        // Creatures
        public DbSet<Creature> Creatures { get; set; }
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Tree> Trees { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<Fungus> Fungi { get; set; }
        public DbSet<CreatureCity> CreatureCities { get; set; }
        public DbSet<CreatureEcosystem> CreatureEcosystems { get; set; }

        // Factions
        public DbSet<Faction> Factions { get; set; }
        public DbSet<FactionMember> FactionMembers { get; set; }

        // Timelines
        public DbSet<Timeline> Timelines { get; set; }
        public DbSet<TimelineEvent> TimelineEvents { get; set; }
        public DbSet<TimelineEventCharacter> TimelineEventCharacters { get; set; }

        // History
        public DbSet<History> Histories { get; set; }

        // Content
        public DbSet<Content> Contents { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Comic> Comics { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<Reference> References { get; set; }

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
        public DbSet<Religion> Religions { get; set; }
        public DbSet<Culture> Cultures { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<PoliticalIdeology> PoliticalIdeologies { get; set; }
        public DbSet<GovernmentSystem> GovernmentSystems { get; set; }
        public DbSet<PoliticalParty> PoliticalParties { get; set; }
        public DbSet<LegalSystem> LegalSystems { get; set; }
        public DbSet<DiplomaticAgreement> DiplomaticAgreements { get; set; }
        public DbSet<CultureNation> CultureNations { get; set; }
        public DbSet<CultureSapientSpecies> CultureSapientSpecies { get; set; }
        public DbSet<CultureSocialClass> CultureSocialClasses { get; set; }
        public DbSet<LanguageNation> LanguageNations { get; set; }
        public DbSet<PoliticalPartyFaction> PoliticalPartyFactions { get; set; }
        public DbSet<PoliticalPartyNation> PoliticalPartyNations { get; set; }

        // Professions
        public DbSet<Profession> Professions { get; set; }
        public DbSet<JobRank> JobRanks { get; set; }
        public DbSet<Specialisation> Specialisations { get; set; }
        public DbSet<Apprenticeship> Apprenticeships { get; set; }
        public DbSet<ProfessionSapientSpecies> ProfessionSapientSpecies { get; set; }
        public DbSet<ProfessionSocialClass> ProfessionSocialClasses { get; set; }
        public DbSet<ProfessionTradeSchool> ProfessionTradeSchools { get; set; }

        // Education
        public DbSet<EducationSystem> EducationSystems { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<TradeSchool> TradeSchools { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<SchoolSubject> SchoolSubjects { get; set; }
        public DbSet<EducationRecord> EducationRecords { get; set; }
        public DbSet<UniversityMajor> UniversityMajors { get; set; }
        public DbSet<ReligiousEducation> ReligiousEducations { get; set; }

        // Abilities
        public DbSet<Ability> Abilities { get; set; }
        public DbSet<AbilityLevel> AbilityLevels { get; set; }
        public DbSet<CharacterAbility> CharacterAbilities { get; set; }

        // Equipment
        public DbSet<Item> Items { get; set; }
        public DbSet<OwnershipHistory> OwnershipHistories { get; set; }

        // Economy
        public DbSet<EconomicSystem> EconomicSystems { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<BankingSystem> BankingSystems { get; set; }
        public DbSet<TaxationSystem> TaxationSystems { get; set; }
        public DbSet<TradeRoute> TradeRoutes { get; set; }
        public DbSet<NaturalResource> NaturalResources { get; set; }
        public DbSet<ExtractionMethod> ExtractionMethods { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildRank> GuildRanks { get; set; }
        public DbSet<Corporation> Corporations { get; set; }
        public DbSet<CorporateLeadership> CorporateLeaderships { get; set; }
        public DbSet<GuildFaction> GuildFactions { get; set; }
        public DbSet<GuildProfession> GuildProfessions { get; set; }
        public DbSet<GuildSocialClass> GuildSocialClasses { get; set; }
        public DbSet<CorporationFaction> CorporationFactions { get; set; }
        public DbSet<CorporationProfession> CorporationProfessions { get; set; }
        public DbSet<TradeRouteLocation> TradeRouteLocations { get; set; }
        public DbSet<TradeRouteResource> TradeRouteResources { get; set; }
        public DbSet<NaturalResourceLocation> NaturalResourceLocations { get; set; }

        // Military
        public DbSet<MilitaryDoctrine> MilitaryDoctrines { get; set; }
        public DbSet<MilitaryOrganization> MilitaryOrganizations { get; set; }
        public DbSet<Army> Armies { get; set; }
        public DbSet<MilitaryUnit> MilitaryUnits { get; set; }
        public DbSet<MilitaryRank> MilitaryRanks { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<MilitaryEquipment> MilitaryEquipments { get; set; }
        public DbSet<ArmyBattle> ArmyBattles { get; set; }
        public DbSet<MilitaryUnitEquipment> MilitaryUnitEquipments { get; set; }
        public DbSet<MilitaryOrganizationCountry> MilitaryOrganizationCountries { get; set; }
        public DbSet<MilitaryOrganizationFaction> MilitaryOrganizationFactions { get; set; }

        // Culture details
        public DbSet<Custom> Customs { get; set; }
        public DbSet<Clothing> Clothing { get; set; }
        public DbSet<ArtForm> ArtForms { get; set; }
        public DbSet<Cuisine> Cuisines { get; set; }
        public DbSet<Tradition> Traditions { get; set; }
        public DbSet<ArchitectureStyle> ArchitectureStyles { get; set; }
        public DbSet<Folklore> Folktales { get; set; }
        public DbSet<Myth> Myths { get; set; }
        public DbSet<CulturalInstitution> CulturalInstitutions { get; set; }
        public DbSet<CulturalFestival> CulturalFestivals { get; set; }
        public DbSet<ArchitectureStyleLocation> ArchitectureStyleLocations { get; set; }
        public DbSet<FolkloreTimelineEvent> FolkloreTimelineEvents { get; set; }
        public DbSet<FolkloreSapientSpecies> FolkloreSapientSpecies { get; set; }

        // Mythology
        public DbSet<Deity> Deities { get; set; }
        public DbSet<HolySite> HolySites { get; set; }
        public DbSet<ReligiousText> ReligiousTexts { get; set; }
        public DbSet<ReligiousOrder> ReligiousOrders { get; set; }
        public DbSet<ReligiousFestival> ReligiousFestivals { get; set; }
        public DbSet<DeityReligiousOrder> DeityReligiousOrders { get; set; }
        public DbSet<ReligiousOrderFaction> ReligiousOrderFactions { get; set; }
        public DbSet<DeityAlliance> DeityAlliances { get; set; }
        public DbSet<DeityRivalry> DeityRivalries { get; set; }

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
