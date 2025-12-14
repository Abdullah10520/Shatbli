using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Infrastructure.Context;

namespace Shatabli.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplictionDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                //.LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging(true);
            });

            // ⚡ الحل: استخدم AddScoped بدل AddTransient
            services.AddScoped<IApplicationDbContext>(provider =>
                provider.GetRequiredService<ApplictionDbContext>());

            return services;
        }
    }
}
