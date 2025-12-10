using System.Reflection;
using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shatabli.Core.Application.Behaviors;


namespace Shatabli.Core.Application
{
    public static class CoreApplicationServiceRegisteration
    {
        public static IServiceCollection AddCoreApplicationService(this IServiceCollection services )
        {
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CoreApplicationServiceRegisteration).Assembly));
            //services.AddAutoMapper(cfg => cfg.AddMaps(typeof(CoreApplicationServiceRegisteration).Assembly));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(cfg =>
            {
                /// This is the new configuration method in MediatR v13
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
