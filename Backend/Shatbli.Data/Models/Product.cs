using Shatbli.Data.Models.Enums;

namespace Shatbli.Data.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;  // For local storage
        public string Vendor { get; set; } = string.Empty;
        public ProductCategory Category { get; set; }
        public bool IsActive { get; set; } = true;

        // Additional properties for ceramics
        public string? Texture { get; set; }
        public string? Size { get; set; }
        public decimal? PricePerUnit { get; set; }

        // Additional properties for paints
        public string? ColorCode { get; set; }
        public string? Brand { get; set; }

        // Scraping metadata
        public string? SourceUrl { get; set; }
        public DateTime? LastScrapedAt { get; set; }
    }
}