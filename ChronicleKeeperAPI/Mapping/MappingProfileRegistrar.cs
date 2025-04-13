using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ChronicleKeeperAPI.Mapping
{
    public static class MappingProfileRegistrar
    {
        public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
        {
            return services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}