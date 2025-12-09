using Microsoft.Extensions.DependencyInjection;
using Shatbli.Service.Implementations;
using Shatbli.Service.Interfaces;

namespace Shatbli.Service
{
    public static class ModuleServiceDependencies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IDesignService, DesignService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAdminSeedService, AdminSeedService>();

            return services;
        }
    }
}

