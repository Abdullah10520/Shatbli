using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string Vendor { get; set; } = string.Empty;
        public ProductCategory Category { get; set; }
        public bool IsActive { get; set; } = true;

        public string? Texture { get; set; }
        public string? Size { get; set; }
        public decimal? PricePerUnit { get; set; }

        public string? ColorCode { get; set; }
        public string? Brand { get; set; }

        public string? SourceUrl { get; set; }
        public DateTime? LastScrapedAt { get; set; }
    }
}
