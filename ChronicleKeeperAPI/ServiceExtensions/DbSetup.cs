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

            return services;
        }
    }
}
