using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shatabli.Core.Application.Features.Products.Commands.AddProductsFromExel;
using Shatabli.Core.Application.Features.Products.Queries.GetAllCeramics;
using Shatabli.Core.Application.Features.Products.Queries.GetCeramicById;

namespace Shatabli.API.Controllers
{
    [Route("[controller]/[action]")]
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

        [HttpGet]
        public async Task<IActionResult> GetAllCeramics()
        {
            GetAllCeramicsQuery request = new();
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetCeramicById(string ceramicId)
        {
            GetCeramicByIdQuery request = new();
            request.ceramicId = ceramicId;
            var response = await _mediator.Send(request);
            return Ok(response);
        }


    }
}
