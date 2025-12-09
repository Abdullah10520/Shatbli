using AutoMapper;
using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Core.Features.Products.Commands.Models;
using Shatbli.Data.DTOs.ProductDtos;
using Shatbli.Service.Interfaces;

namespace Shatbli.Core.Features.Products.Commands.Handlers
{
    public class CreateProductHandler : ResponseHandler, IRequestHandler<CreateProductCommand, Response<ProductListDto>>
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public CreateProductHandler(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<Response<ProductListDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Map Command to DTO
            var dto = _mapper.Map<CreateProductDto>(request);

            // Create product
            var product = await _productService.CreateProductAsync(dto);

            // Map Entity to Response DTO
            var response = _mapper.Map<ProductListDto>(product);

            return Created(response);
        }
    }
}