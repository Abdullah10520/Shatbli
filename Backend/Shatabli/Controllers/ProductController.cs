using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shatabli.Core.Application.Features.Products.Commands.AddProductsFromExel;

namespace Shatabli.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> SaveProductsFromExcel(IFormFile excelFile)
        {
            var excelStream = excelFile.OpenReadStream();

            AddProductsFromExelCommand request = new AddProductsFromExelCommand();
            request.stream = excelStream ;

            var response = await _mediator.Send(request);

            return Ok();
        }


    }
}
