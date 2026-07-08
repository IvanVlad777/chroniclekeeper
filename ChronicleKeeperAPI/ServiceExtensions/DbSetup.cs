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

            return services;
        }
    }
}
