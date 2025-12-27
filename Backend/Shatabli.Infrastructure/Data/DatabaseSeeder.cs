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

            // ✅ Seed plans first (needed for admin subscription)
            await SeedSubscriptionPlansAsync(context, configuration);

            // ✅ Then seed admin (and subscribe to premium)
            await SeedAdminUserAsync(context, configuration);
        }

        private static async Task SeedAdminUserAsync(ApplictionDbContext context, IConfiguration configuration)
        {
            var adminConfig = configuration.GetSection("DefaultAdmin");
            var adminEmail = adminConfig["Email"] ?? "admin@shatabli.com";

            // Check if admin exists
            var adminUser = await context.Users.AsTracking().FirstOrDefaultAsync(u => u.Role == UserRole.Admin);

            if (adminUser == null)
            {
                // ✅ Create new admin
                adminUser = new User
                {
                    Email = adminEmail,
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

                // ✅ Auto-subscribe admin to Premium plan
                await SubscribeAdminToPremiumAsync(context, adminUser.Id);
            }
            else
            {
                // ✅ Update existing admin if config changed
                var configPassword = adminConfig["Password"] ?? "Admin@123";
                var configFullName = adminConfig["FullName"] ?? "System Administrator";
                var configPhoneNumber = adminConfig["PhoneNumber"];

                bool hasChanges = false;

                if (adminUser.Email != adminEmail)
                {
                    adminUser.Email = adminEmail;
                    hasChanges = true;
                }

                if (adminUser.FullName != configFullName)
                {
                    adminUser.FullName = configFullName;
                    hasChanges = true;
                }

                if (adminUser.PhoneNumber != configPhoneNumber)
                {
                    adminUser.PhoneNumber = configPhoneNumber;
                    hasChanges = true;
                }

                if (adminConfig.GetValue<bool>("ForcePasswordUpdate", false))
                {
                    adminUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(configPassword);
                    hasChanges = true;
                    Console.WriteLine("🔑 Admin password updated from configuration");
                }

                if (hasChanges)
                {
                    adminUser.UpdatedAt = DateTime.UtcNow;
                    context.Users.Update(adminUser);
                    await context.SaveChangesAsync();
                    Console.WriteLine("♻️ Admin user updated successfully!");
                }
                else
                {
                    Console.WriteLine("ℹ️ Admin user already exists and is up-to-date.");
                }

                // ✅ Check if admin has premium subscription
                var hasActiveSubscription = await context.UserSubscriptions
                    .Include(s => s.SubscriptionPlan)
                    .AnyAsync(s => s.UserId == adminUser.Id && s.IsActive && s.SubscriptionPlan.Type == PlanType.Premium);

                if (!hasActiveSubscription)
                {
                    await SubscribeAdminToPremiumAsync(context, adminUser.Id);
                }
            }
        }

        private static async Task SubscribeAdminToPremiumAsync(ApplictionDbContext context, string adminUserId)
        {
            var premiumPlan = await context.SubscriptionPlans
                .FirstOrDefaultAsync(p => p.Type == PlanType.Premium);

            if (premiumPlan != null)
            {
                // Deactivate any existing subscriptions
                var existingSubscriptions = await context.UserSubscriptions
                    .Where(s => s.UserId == adminUserId && s.IsActive)
                    .ToListAsync();

                foreach (var sub in existingSubscriptions)
                {
                    sub.IsActive = false;
                    sub.EndDate = DateTime.UtcNow;
                }

                // Create premium subscription
                var premiumSubscription = new UserSubscription
                {
                    UserId = adminUserId,
                    SubscriptionPlanId = premiumPlan.Id,
                    StartDate = DateTime.UtcNow,
                    IsActive = true,
                    ImagesGeneratedThisMonth = 0,
                    ImagesGeneratedToday = 0,
                    LastResetDate = DateTime.UtcNow
                };

                await context.UserSubscriptions.AddAsync(premiumSubscription);
                await context.SaveChangesAsync();

                Console.WriteLine("🌟 Admin subscribed to Premium plan successfully!");
            }
        }

        private static async Task SeedSubscriptionPlansAsync(ApplictionDbContext context, IConfiguration configuration)
        {
            var seedPlans = new List<SubscriptionPlan>
            {
                new SubscriptionPlan
                {
                    Name = "Free",
                    Type = PlanType.Free,
                    Price = 0,
                    MaxImagesPerMonth = 10,
                    MaxImagesPerDay = 3,
                    HasWatermark = true,
                    HasPriorityGeneration = false,
                    Description = "Free plan - 10 images per month"
                },
                new SubscriptionPlan
                {
                    Name = "Basic",
                    Type = PlanType.Basic,
                    Price = 30m,
                    MaxImagesPerMonth = 200,
                    MaxImagesPerDay = 20,
                    HasWatermark = false,
                    HasPriorityGeneration = false,
                    Description = "Basic plan - 200 images per month"
                },
                new SubscriptionPlan
                {
                    Name = "Premium",
                    Type = PlanType.Premium,
                    Price = 100m,
                    MaxImagesPerMonth = -1,
                    MaxImagesPerDay = -1,
                    HasWatermark = false,
                    HasPriorityGeneration = true,
                    Description = "Premium plan - unlimited images"
                }
            };

            foreach (var seedPlan in seedPlans)
            {
                var existingPlan = await context.SubscriptionPlans
                    .FirstOrDefaultAsync(p => p.Type == seedPlan.Type);

                if (existingPlan == null)
                {
                    seedPlan.Id = Guid.NewGuid().ToString();
                    seedPlan.CreatedAt = DateTime.UtcNow;
                    seedPlan.IsDeleted = false;

                    await context.SubscriptionPlans.AddAsync(seedPlan);
                    Console.WriteLine($"✅ Created subscription plan: {seedPlan.Name}");
                }
                else
                {
                    bool hasChanges = false;

                    if (existingPlan.Name != seedPlan.Name)
                    {
                        existingPlan.Name = seedPlan.Name;
                        hasChanges = true;
                    }

                    if (existingPlan.Price != seedPlan.Price)
                    {
                        existingPlan.Price = seedPlan.Price;
                        hasChanges = true;
                    }

                    if (existingPlan.MaxImagesPerMonth != seedPlan.MaxImagesPerMonth)
                    {
                        existingPlan.MaxImagesPerMonth = seedPlan.MaxImagesPerMonth;
                        hasChanges = true;
                    }

                    if (existingPlan.MaxImagesPerDay != seedPlan.MaxImagesPerDay)
                    {
                        existingPlan.MaxImagesPerDay = seedPlan.MaxImagesPerDay;
                        hasChanges = true;
                    }

                    if (existingPlan.HasWatermark != seedPlan.HasWatermark)
                    {
                        existingPlan.HasWatermark = seedPlan.HasWatermark;
                        hasChanges = true;
                    }

                    if (existingPlan.HasPriorityGeneration != seedPlan.HasPriorityGeneration)
                    {
                        existingPlan.HasPriorityGeneration = seedPlan.HasPriorityGeneration;
                        hasChanges = true;
                    }

                    if (existingPlan.Description != seedPlan.Description)
                    {
                        existingPlan.Description = seedPlan.Description;
                        hasChanges = true;
                    }

                    if (hasChanges)
                    {
                        existingPlan.UpdatedAt = DateTime.UtcNow;
                        context.SubscriptionPlans.Update(existingPlan);
                        Console.WriteLine($"♻️ Updated subscription plan: {existingPlan.Name}");
                    }
                    else
                    {
                        Console.WriteLine($"ℹ️ Subscription plan '{existingPlan.Name}' is up-to-date");
                    }
                }
            }

            await context.SaveChangesAsync();
            Console.WriteLine("✅ Subscription plans seeding completed");
        }
    }
}