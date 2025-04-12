using Microsoft.Extensions.DependencyInjection;

namespace ChronicleKeeper.API.ServiceExtensions
{
    public static class AuthorizationSetup
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorizationBuilder()
                .AddPolicy("SuperAdminPolicy", policy => policy.RequireRole("SuperAdmin"))
                .AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"))
                .AddPolicy("EditorPolicy", policy => policy.RequireRole("Admin", "Editor"))
                .AddPolicy("ModeratorPolicy", policy => policy.RequireRole("Admin", "Editor", "Moderator"))
                .AddPolicy("ReaderPolicy", policy => policy.RequireRole("Admin", "Editor", "Moderator", "Reader"));

            return services;
        }
    }
}
