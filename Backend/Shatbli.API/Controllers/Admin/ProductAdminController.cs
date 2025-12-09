using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shatbli.Core.Features.Products.Commands.Models;
using Shatbli.Data.MetaData;

namespace Shatbli.API.Controllers.Admin
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductAdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductAdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Router.ProductAdminRouting.Create)]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete(Router.ProductAdminRouting.Delete)]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var command = new DeleteProductCommand { ProductId = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}