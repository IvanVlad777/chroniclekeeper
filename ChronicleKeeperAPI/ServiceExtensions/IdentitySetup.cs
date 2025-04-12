using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ChronicleKeeper.Infrastructure.Data;

namespace ChronicleKeeper.API.ServiceExtensions
{
    public static class IdentitySetup
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
