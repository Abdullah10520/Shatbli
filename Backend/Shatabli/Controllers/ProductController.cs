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
            var result = await _mediator.Send(new GetAllCeramicsQuery());

            // 1. فحص الفشل
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            return Ok(ApiResponse<GetAllCeramicsResponse>.SuccessResponse(result.Data, result.Message));

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCeramicById(string ceramicId)
        {

            if (string.IsNullOrWhiteSpace(ceramicId))
            {
                return BadRequest(ApiResponse<object>.FailureResponse("Ceramic ID is required",null));
            }

            var result = await _mediator.Send(new GetCeramicByIdQuery { ceramicId = ceramicId });

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }
            return Ok(ApiResponse<GetCeramicByIdResponse>.SuccessResponse(result.Data, result.Message));
        }
    }
}
