using AutoMapper;
using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Core.Features.Products.Queries.Models;
using Shatbli.Data.DTOs.ProductDtos;
using Shatbli.Service.Interfaces;

namespace Shatbli.Core.Features.Products.Queries.Handlers
{
    public class GetProductsByCategoryHandler : ResponseHandler, IRequestHandler<GetProductsByCategoryQuery, Response<List<ProductListDto>>>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public GetProductsByCategoryHandler(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<Response<List<ProductListDto>>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await _productService.GetProductsByCategoryAsync(request.Category);

            var response = _mapper.Map<List<ProductListDto>>(products);

            return Success(response);
        }
    }
}