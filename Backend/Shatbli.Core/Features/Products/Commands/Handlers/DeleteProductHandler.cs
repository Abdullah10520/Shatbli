using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Core.Features.Products.Commands.Models;
using Shatbli.Service.Interfaces;

namespace Shatbli.Core.Features.Products.Commands.Handlers
{
    public class DeleteProductHandler : ResponseHandler, IRequestHandler<DeleteProductCommand, Response<bool>>
    {
        private readonly IProductService _productService;

        public DeleteProductHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<Response<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var result = await _productService.DeleteProductAsync(request.ProductId);

            return result
                ? Success(true, "Product deleted successfully")
                : NotFound<bool>("Product not found");
        }
    }
}