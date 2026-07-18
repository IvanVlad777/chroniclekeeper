using ChronicleKeeper.Core.Repositories;
using ChronicleKeeper.Infrastructure.Data;
using ChronicleKeeper.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChronicleKeeper.API.ServiceExtensions
{
    public static class DbSetup
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlServerConnection = configuration.GetSection("DatabaseSettings").GetValue<string>("SqlServerConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(sqlServerConnection));

            // Register repositories
            services.AddScoped<ICharacterRepository, CharacterRepository>();
            services.AddScoped<IWorldRepository, WorldRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ISpeciesRepository, SpeciesRepository>();
            services.AddScoped<IFactionRepository, FactionRepository>();
            services.AddScoped<ITimelineRepository, TimelineRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<ISocialClassRepository, SocialClassRepository>();
            services.AddScoped<INationRepository, NationRepository>();
            services.AddScoped<IReligionRepository, ReligionRepository>();
            services.AddScoped<ICultureRepository, CultureRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IPoliticalIdeologyRepository, PoliticalIdeologyRepository>();
            services.AddScoped<IGovernmentSystemRepository, GovernmentSystemRepository>();
            services.AddScoped<IPoliticalPartyRepository, PoliticalPartyRepository>();
            services.AddScoped<ILegalSystemRepository, LegalSystemRepository>();
            services.AddScoped<IDiplomaticAgreementRepository, DiplomaticAgreementRepository>();
            services.AddScoped<IProfessionRepository, ProfessionRepository>();
            services.AddScoped<IJobRankRepository, JobRankRepository>();
            services.AddScoped<IApprenticeshipRepository, ApprenticeshipRepository>();
            services.AddScoped<ISpecialisationRepository, SpecialisationRepository>();
            services.AddScoped<IEducationSystemRepository, EducationSystemRepository>();
            services.AddScoped<ISchoolRepository, SchoolRepository>();
            services.AddScoped<ITradeSchoolRepository, TradeSchoolRepository>();
            services.AddScoped<ISchoolSubjectRepository, SchoolSubjectRepository>();
            services.AddScoped<IUniversityRepository, UniversityRepository>();
            services.AddScoped<IUniversityMajorRepository, UniversityMajorRepository>();
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<IEducationRecordRepository, EducationRecordRepository>();
            services.AddScoped<IReligiousEducationRepository, ReligiousEducationRepository>();
            services.AddScoped<IAbilityRepository, AbilityRepository>();
            services.AddScoped<IAbilityLevelRepository, AbilityLevelRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IOwnershipHistoryRepository, OwnershipHistoryRepository>();
            services.AddScoped<IHistoryRepository, HistoryRepository>();
            services.AddScoped<IContentRepository, ContentRepository>();
            services.AddScoped<IChapterRepository, ChapterRepository>();
            services.AddScoped<IEpisodeRepository, EpisodeRepository>();
            services.AddScoped<IReferenceRepository, ReferenceRepository>();
            services.AddScoped<IClimateZoneRepository, ClimateZoneRepository>();
            services.AddScoped<IClimateDetailRepository, ClimateDetailRepository>();
            services.AddScoped<ISeasonRepository, SeasonRepository>();
            services.AddScoped<IWeatherPatternRepository, WeatherPatternRepository>();
            services.AddScoped<ICreatureRepository, CreatureRepository>();
            services.AddScoped<IEconomicSystemRepository, EconomicSystemRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<IBankingSystemRepository, BankingSystemRepository>();
            services.AddScoped<ITaxationSystemRepository, TaxationSystemRepository>();
            services.AddScoped<ITradeRouteRepository, TradeRouteRepository>();
            services.AddScoped<INaturalResourceRepository, NaturalResourceRepository>();
            services.AddScoped<IExtractionMethodRepository, ExtractionMethodRepository>();
            services.AddScoped<IIndustryRepository, IndustryRepository>();
            services.AddScoped<IGuildRepository, GuildRepository>();
            services.AddScoped<IGuildRankRepository, GuildRankRepository>();
            services.AddScoped<ICorporationRepository, CorporationRepository>();
            services.AddScoped<ICorporateLeadershipRepository, CorporateLeadershipRepository>();

            // Military
            services.AddScoped<IMilitaryDoctrineRepository, MilitaryDoctrineRepository>();
            services.AddScoped<IMilitaryOrganizationRepository, MilitaryOrganizationRepository>();
            services.AddScoped<IArmyRepository, ArmyRepository>();
            services.AddScoped<IMilitaryUnitRepository, MilitaryUnitRepository>();
            services.AddScoped<IMilitaryRankRepository, MilitaryRankRepository>();
            services.AddScoped<IBattleRepository, BattleRepository>();
            services.AddScoped<IMilitaryEquipmentRepository, MilitaryEquipmentRepository>();

            // Culture details
            services.AddScoped<ICustomRepository, CustomRepository>();
            services.AddScoped<IArtFormRepository, ArtFormRepository>();
            services.AddScoped<ICuisineRepository, CuisineRepository>();
            services.AddScoped<IClothingRepository, ClothingRepository>();
            services.AddScoped<ITraditionRepository, TraditionRepository>();
            services.AddScoped<IArchitectureStyleRepository, ArchitectureStyleRepository>();
            services.AddScoped<IFolkloreRepository, FolkloreRepository>();
            services.AddScoped<IMythRepository, MythRepository>();
            services.AddScoped<ICulturalFestivalRepository, CulturalFestivalRepository>();
            services.AddScoped<ICulturalInstitutionRepository, CulturalInstitutionRepository>();

            return services;
        }
    }
}
