using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shatabli.Core.Application.Behaviors;
using Shatabli.Core.Application.Interfaces;
using System.Reflection;


namespace Shatabli.Core.Application
{
    public static class CoreApplicationServiceRegisteration
    {
        public static IServiceCollection AddCoreApplicationService(this IServiceCollection services)
        {
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CoreApplicationServiceRegisteration).Assembly));
            //services.AddAutoMapper(cfg => cfg.AddMaps(typeof(CoreApplicationServiceRegisteration).Assembly));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg =>
            {
                /// This is the new configuration method in MediatR v13
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddMemoryCache();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        


            return services;
        }
    }
}
