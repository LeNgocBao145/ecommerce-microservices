using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Domain.Interfaces;
using OrderApi.Infrastructure.Persistence;
using OrderApi.Infrastructure.Repositories;

namespace OrderApi.Infrastructure
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<OrderDbContext>(services, config, config["MySerilog:Filename"]);

            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
