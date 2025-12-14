using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shatabli.Core.Application;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Infrastructure;
using Shatabli.Infrastructure.Context;
using Shatabli.Infrastructure.Data;
using Shatabli.Infrastructure.Services;
using System.Text;
using FluentValidation;
namespace Shatabli
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddCoreApplicationService();
            builder.Services.AddScoped<ITokenService, TokenService>();

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var secretKey = jwtSettings["SecretKey"] ?? "YourSecretKeyHere_MustBe32CharactersOrMore!";

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"] ?? "ShatabliAPI",
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"] ?? "ShatabliClient",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization();
            //builder.Services.AddHttpClient<IStorageService, CloudinaryService>();


            builder.Services.AddHttpClient<IStorageService, CloudinaryService>(client =>
            {
                client.Timeout = TimeSpan.FromMinutes(5);
            });

            var app = builder.Build();

            // ⚡ Database Seeding
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplictionDbContext>();
                    await DatabaseSeeder.SeedAsync(context, builder.Configuration);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "❌ An error occurred while seeding the database.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
