using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.Infrastructure.Context
{
    public class ApplictionDbContext : DbContext,IApplicationDbContext
    {
        public ApplictionDbContext(DbContextOptions<ApplictionDbContext> options) : base(options) { }

        // Catalog Management
        public DbSet<Product> Products { get; set; }

        // Design Studio
        public DbSet<Design> Designs { get; set; }
        public DbSet<AIProcessingLog> AIProcessingLogs { get; set; }

        // User Management
        public DbSet<User> Users { get; set; }

        public override int SaveChanges()
        {
            //For Auidit
            UpdateTimestamps();
            return base.SaveChanges();
        }

        //Because Any one Can Write With Async and At That Time Auidit Has No Effict 
        //So We Ovverride SaveChangesAsync 
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
                    entry.Entity.DelatedAt = DateTime.UtcNow;
                    entry.Entity.IsDeleted = true;

                }
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product entity configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Vendor).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Category).IsRequired();
                entity.Property(e => e.ColorCode).HasMaxLength(20);
                entity.Property(e => e.Brand).HasMaxLength(100);
                entity.Property(e => e.Texture).HasMaxLength(100);
                entity.Property(e => e.Size).HasMaxLength(50);
                entity.Property(e => e.SourceUrl).HasMaxLength(500);
                entity.Property(e => e.PricePerUnit).HasColumnType("decimal(18,2)");

                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.Vendor);
                entity.HasIndex(e => e.IsActive);
            });

            // User entity configuration
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

            // Design entity configuration
            modelBuilder.Entity<Design>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OriginalImagePath).IsRequired().HasMaxLength(500);
                entity.Property(e => e.OriginalImageUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.GeneratedImagePath).HasMaxLength(500);
                entity.Property(e => e.GeneratedImageUrl).HasMaxLength(500);
                entity.Property(e => e.SelectedWallColor).HasMaxLength(20);
                entity.Property(e => e.Prompt).HasMaxLength(2000);
                entity.Property(e => e.ErrorMessage).HasMaxLength(1000);
                entity.Property(e => e.Status).IsRequired();

                entity.HasOne(e => e.User)
                    .WithMany(u => u.Designs)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.SelectedCeramicProduct)
                    .WithMany()
                    .HasForeignKey(e => e.SelectedCeramicProductId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CreatedAt);
            });

            // ScrapingJob entity configuration
            //modelBuilder.Entity<ScrapingJob>(entity =>
            //{
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.VendorName).IsRequired().HasMaxLength(100);
            //    entity.Property(e => e.TargetUrl).IsRequired().HasMaxLength(500);
            //    entity.Property(e => e.Category).IsRequired();
            //    entity.Property(e => e.Status).IsRequired();
            //    entity.Property(e => e.ErrorDetails).HasMaxLength(2000);

            //    entity.HasIndex(e => e.Status);
            //    entity.HasIndex(e => e.CreatedAt);
            //});

            // AIProcessingLog entity configuration
            modelBuilder.Entity<AIProcessingLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Prompt).IsRequired().HasMaxLength(2000);
                entity.Property(e => e.RequestPayload).HasColumnType("nvarchar(max)");
                entity.Property(e => e.ResponsePayload).HasColumnType("nvarchar(max)");
                entity.Property(e => e.ErrorMessage).HasMaxLength(1000);
                entity.Property(e => e.ErrorStackTrace).HasColumnType("nvarchar(max)");
                entity.Property(e => e.ApiEndpoint).IsRequired().HasMaxLength(500);
                entity.Property(e => e.ApiVersion).HasMaxLength(50);
                entity.Property(e => e.ApiCost).HasColumnType("decimal(18,4)");

                entity.HasOne(e => e.Design)
                    .WithMany()
                    .HasForeignKey(e => e.DesignId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.DesignId);
                entity.HasIndex(e => e.RequestedAt);
            });


        }
    }
}
