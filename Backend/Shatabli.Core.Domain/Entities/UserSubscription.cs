using System;

namespace Shatabli.Core.Domain.Entities
{
    public class UserSubscription : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string SubscriptionPlanId { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public int ImagesGeneratedThisMonth { get; set; }
        public int ImagesGeneratedToday { get; set; }
        public DateTime LastResetDate { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual SubscriptionPlan SubscriptionPlan { get; set; } = null!;
    }
}