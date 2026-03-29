using AuthenticationApi.Domain.Interfaces;
using AuthenticationApi.Infrastructure.Persistence;
using AuthenticationApi.Infrastructure.Repositories;
using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationApi.Infrastructure
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<UserDbContext>(services, config, config["MySerilog:Filename"]!);

            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
