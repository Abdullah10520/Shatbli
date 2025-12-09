using Shatbli.Data.Models.Enums;

namespace Shatbli.Data.DTOs.ProductDtos
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Vendor { get; set; } = string.Empty;
        public ProductCategory Category { get; set; }
        public string? Texture { get; set; }
        public string? Size { get; set; }
        public decimal? PricePerUnit { get; set; }
        public string? ColorCode { get; set; }
        public string? Brand { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}