using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.Core.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        // Catalog Management
        public DbSet<Product> Products { get; set; }

        // Design Studio
        public DbSet<Design> Designs { get; set; }
        public DbSet<AIProcessingLog> AIProcessingLogs { get; set; }

        // User Management
        public DbSet<User> Users { get; set; }

        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }

        public DbSet<UserSubscription> UserSubscriptions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);


    }
}
