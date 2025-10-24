using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ChronicleKeeper.API.ServiceExtensions
{
    public static class CQRSMediatRSetup
    {
        public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
        {
            // Register MediatR handlers from both API and Core assemblies
            services.AddMediatR(cfg => 
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); // API project
                cfg.RegisterServicesFromAssembly(typeof(ChronicleKeeper.Core.CQRS.Characters.Commands.CreateCharacterCommand).Assembly); // Core project
            });
            return services;
        }
    }
}
