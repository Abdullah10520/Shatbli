using Microsoft.AspNetCore.Http;
using Shatbli.Data.Models.Enums;

namespace Shatbli.Data.DTOs.ProductDtos
{
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public IFormFile Image { get; set; } = null!;
        public string Vendor { get; set; } = string.Empty;
        public ProductCategory Category { get; set; }
        public string? Texture { get; set; }
        public string? Size { get; set; }
        public decimal? PricePerUnit { get; set; }
        public string? ColorCode { get; set; }
        public string? Brand { get; set; }
    }
}