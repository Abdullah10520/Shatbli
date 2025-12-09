using Microsoft.Extensions.DependencyInjection;
using Shatbli.Infrustructure.Abstracts;
using Shatbli.Infrustructure.Repositories;

namespace Shatbli.Infrustructure
{
    public static class ModuleInfrustructureDependencies
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Repositories (optional - already included in UnitOfWork)
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IDesignRepository, DesignRepository>();
            services.AddScoped<IAIProcessingLogRepository, AIProcessingLogRepository>();

            return services;
        }
    }
}
