using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shatabli.Core.Application.Features.Products.Commands.AddProductsFromExel;
using Shatabli.Core.Application.Features.Products.Queries.GetAllCeramics;
using Shatabli.Core.Application.Features.Products.Queries.GetCeramicById;
using Shatabli.Core.Domain.Common;

namespace Shatabli.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SaveProductsFromExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
                throw new ArgumentException("Excel file is required");

            using var excelStream = excelFile.OpenReadStream();

            var request = new AddProductsFromExelCommand
            {
                stream = excelStream
            };

            var result = await _mediator.Send(request);

            var response = ApiResponse<object>.SuccessResponse(
                result,
                "Products imported successfully from Excel");

            return Ok(response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCeramics()
        {
            var request = new GetAllCeramicsQuery();
            var result = await _mediator.Send(request);

            var response = ApiResponse<object>.SuccessResponse(
                result,
                "Ceramics retrieved successfully");

            return Ok(response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCeramicById(string ceramicId)
        {
            if (string.IsNullOrWhiteSpace(ceramicId))
                throw new ArgumentException("Ceramic ID is required");

            var request = new GetCeramicByIdQuery
            {
                ceramicId = ceramicId
            };

            var result = await _mediator.Send(request);

            var response = ApiResponse<object>.SuccessResponse(
                result,
                "Ceramic retrieved successfully");

            return Ok(response);
        }
    }
}
