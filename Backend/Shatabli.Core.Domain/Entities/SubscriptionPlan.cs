using System;
using System.Collections.Generic;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Domain.Entities
{
    public class SubscriptionPlan : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public PlanType Type { get; set; }
        public decimal Price { get; set; }
        public int MaxImagesPerMonth { get; set; }
        public int MaxImagesPerDay { get; set; }
        public bool HasWatermark { get; set; }
        public bool HasPriorityGeneration { get; set; }
        public string Description { get; set; } = string.Empty;
        
        // Navigation
        public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
    }
}