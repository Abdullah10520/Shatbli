
using Shatabli.Core.Application.Interfaces;
using Shatabli.Infrastructure;
using Shatabli.Core.Application;
using Shatabli.Infrastructure.Context;
using FluentValidation;
using Shatabli.Infrastructure.Services;
namespace Shatabli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddCoreApplicationService();

            //builder.Services.AddScoped<IApplicationDbContext, ApplictionDbContext>();

            //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ModuleCoreDependencies).Assembly));
            //builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(ModuleCoreDependencies).Assembly));
            //builder.Services.AddMediatR()
            //builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
            //var applicationAssembly = typeof(Shatabli.Core.Application.AssemblyMarker).Assembly; 

            //builder.Services.AddHttpClient<IStorageService, CloudinaryService>();


            builder.Services.AddHttpClient<IStorageService, CloudinaryService>(client =>
            {
                client.Timeout = TimeSpan.FromMinutes(5);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
