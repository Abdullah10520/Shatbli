using Shatbli.Data.Models.Enums;

namespace Shatbli.Data.Models
{
    public class ScrapingJob : BaseEntity
    {
        public string VendorName { get; set; } = string.Empty;
        public string TargetUrl { get; set; } = string.Empty;
        public ProductCategory Category { get; set; }
        public ScrapingJobStatus Status { get; set; } = ScrapingJobStatus.Pending;
        
        // Results
        public int ProductsScraped { get; set; }
        public int ProductsAdded { get; set; }
        public int ProductsUpdated { get; set; }
        public int ErrorCount { get; set; }
        public string? ErrorDetails { get; set; }
        
        // Timing
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int DurationSeconds { get; set; }
        
        // Retry logic
        public int RetryCount { get; set; } = 0;
        public int MaxRetries { get; set; } = 3;
    }
}