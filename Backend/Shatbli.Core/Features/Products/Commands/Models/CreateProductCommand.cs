using MediatR;
using Microsoft.AspNetCore.Http;
using Shatbli.Core.Bases;
using Shatbli.Data.DTOs.ProductDtos;
using Shatbli.Data.Models.Enums;

namespace Shatbli.Core.Features.Products.Commands.Models
{
    public class CreateProductCommand : IRequest<Response<ProductListDto>>
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