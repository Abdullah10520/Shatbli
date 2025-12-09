using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shatbli.Core.Behaviors;
using System.Reflection;

namespace Shatbli.Core
{
    public static class ModuleCoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            // Register MediatR handlers from the current assembly
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ModuleCoreDependencies).Assembly));
            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ModuleCoreDependencies).Assembly));

            // Get Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Add Validation Pipeline Behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));

            return services;
        }
    }
}
