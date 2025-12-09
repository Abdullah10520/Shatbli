using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shatbli.Core.Features.Products.Queries.Models;
using Shatbli.Data.MetaData;
using Shatbli.Data.Models.Enums;

namespace Shatbli.API.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Router.ProductRouting.GetCeramics)]
        public async Task<IActionResult> GetCeramics()
        {
            var query = new GetProductsByCategoryQuery
            {
                Category = ProductCategory.FlooringCeramics
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet(Router.ProductRouting.GetPaints)]
        public async Task<IActionResult> GetPaints()
        {
            var query = new GetProductsByCategoryQuery
            {
                Category = ProductCategory.WallPaints
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}