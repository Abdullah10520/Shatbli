using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Entities;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Infrastructure.Context
{
    public class ApplictionDbContext : DbContext, IApplicationDbContext
    {
        public ApplictionDbContext(DbContextOptions<ApplictionDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Design> Designs { get; set; }
        public DbSet<AIProcessingLog> AIProcessingLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entities = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entities)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    entry.Entity.IsDeleted = true;
                    entry.State = EntityState.Modified;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Design>()
                .HasQueryFilter(d => !d.IsDeleted);

            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDeleted);

            modelBuilder.Entity<Product>()
                .HasQueryFilter(c => !c.IsDeleted);

            modelBuilder.Entity<SubscriptionPlan>()
                .HasQueryFilter(sp => !sp.IsDeleted);

            modelBuilder.Entity<UserSubscription>()
                .HasQueryFilter(us => !us.IsDeleted);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Category).IsRequired();
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.IsActive);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(256);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Role).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Role);
            });

            modelBuilder.Entity<Design>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OriginalImagePath).IsRequired().HasMaxLength(500);
                entity.Property(e => e.OriginalImageUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.GeneratedImagePath).HasMaxLength(500);
                entity.Property(e => e.GeneratedImageUrl).HasMaxLength(500);
                entity.Property(e => e.ErrorMessage).HasMaxLength(1000);
                entity.Property(e => e.Status).IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Designs)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CreatedAt);
            });

            modelBuilder.Entity<AIProcessingLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ErrorMessage).HasMaxLength(1000);

                entity.HasOne(e => e.Design)
                    .WithMany()
                    .HasForeignKey(e => e.DesignId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.DesignId);
                entity.HasIndex(e => e.RequestedAt);
            });

            // ✅ Subscription Plan Configuration
            modelBuilder.Entity<SubscriptionPlan>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasIndex(e => e.Type);
            });

            // ✅ User Subscription Configuration
            modelBuilder.Entity<UserSubscription>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Subscriptions)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.SubscriptionPlan)
                    .WithMany(sp => sp.UserSubscriptions)
                    .HasForeignKey(e => e.SubscriptionPlanId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.IsActive);
            });

            // ❌ REMOVED - Seed data moved to SubscriptionPlanSeeder
        }
    }
}
