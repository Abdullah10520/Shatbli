using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shatabli.Core.Domain.Entities;
using Shatabli.Core.Domain.Enums;
using Shatabli.Infrastructure.Context;

namespace Shatabli.Infrastructure.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplictionDbContext context, IConfiguration configuration)
        {
            await context.Database.MigrateAsync();

            await SeedAdminUserAsync(context, configuration);
        }

        private static async Task SeedAdminUserAsync(ApplictionDbContext context, IConfiguration configuration)
        {
            var adminExists = await context.Users.AnyAsync(u => u.Role == UserRole.Admin);

            if (!adminExists)
            {
                var adminConfig = configuration.GetSection("DefaultAdmin");

                var adminUser = new User
                {
                    Email = adminConfig["Email"] ?? "admin@shatabli.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminConfig["Password"] ?? "Admin@123"),
                    FullName = adminConfig["FullName"] ?? "System Administrator",
                    PhoneNumber = adminConfig["PhoneNumber"],
                    Role = UserRole.Admin,
                    IsActive = true,
                    IsEmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await context.Users.AddAsync(adminUser);
                await context.SaveChangesAsync();

                Console.WriteLine("✅ Admin user created successfully!");
                Console.WriteLine($"📧 Email: {adminUser.Email}");
            }
            else
            {
                Console.WriteLine("ℹ️ Admin user already exists.");
            }
        }
    }
}