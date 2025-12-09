using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Data.DTOs.ProductDtos;
using Shatbli.Data.Models.Enums;

namespace Shatbli.Core.Features.Products.Queries.Models
{
    public class GetProductsByCategoryQuery : IRequest<Response<List<ProductListDto>>>
    {
        public ProductCategory Category { get; set; }
    }
}