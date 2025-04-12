using ChronicleKeeper.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChronicleKeeper.API.ServiceExtensions
{
    public static class DbSetup
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = configuration.GetSection("DatabaseSettings");
            bool usePostgreSQL = databaseSettings.GetValue<bool>("UsePostgreSQL");

            var sqlServerConnection = databaseSettings.GetValue<string>("SqlServerConnection");
            var postgreSqlConnection = databaseSettings.GetValue<string>("PostgreSqlConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                if (usePostgreSQL)
                {
                    options.UseNpgsql(postgreSqlConnection);
                }
                else
                {
                    options.UseSqlServer(sqlServerConnection);
                }
            });

            return services;
        }
    }
}
