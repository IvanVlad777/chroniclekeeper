using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ChronicleKeeper.API.ServiceExtensions
{
    public static class CQRSMediatRSetup
    {
        public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}
