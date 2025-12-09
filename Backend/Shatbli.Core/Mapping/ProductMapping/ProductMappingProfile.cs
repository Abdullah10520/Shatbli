using AutoMapper;
using Shatbli.Core.Features.Products.Commands.Models;
using Shatbli.Data.DTOs.ProductDtos;
using Shatbli.Data.Models;

namespace Shatbli.Core.Mapping.ProductMapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            // Command to DTO
            CreateMap<CreateProductCommand, CreateProductDto>();

            // Entity to DTO
            CreateMap<Product, ProductListDto>();
        }
    }
}