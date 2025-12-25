using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage;
using Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicImage;
using Shatabli.Core.Application.Features.Designs.Commands.SoftDeleteDesign;
using Shatabli.Core.Application.Features.Designs.Queries.GetAllDesigns;
using Shatabli.Core.Application.Features.Designs.Queries.GetDesignById;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DesignController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IClaimsService _claimsService;

        public DesignController(
            IMediator mediator,
            IClaimsService claimsService)
        {
            _mediator = mediator;
            _claimsService = claimsService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateDesign(IFormFile imageFile, string ceramicId, DesignType designType)
        {
            var userId = _claimsService.GetCurrentUserId();

            // Check subscription - will throw exception if limit exceeded

            using var imageStream = imageFile.OpenReadStream();
            using var roomImageMemoryStream = new MemoryStream();
            await imageStream.CopyToAsync(roomImageMemoryStream);

            var request = new AddDesignWithOurCeramicImageCommand
            {
                stream = roomImageMemoryStream.ToArray(),
                imageName = imageFile.FileName,
                ceramicId = ceramicId,
                designType = designType
            };

            var result = await _mediator.Send(request);

            // Increment counter after successful generation

            var response = ApiResponse<object>.SuccessResponse(
                new
                {
                    generatedImageUrl = result.GeneratedImageUrl,
                    designId = result.designId
                },
                "Design generated successfully");

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateDesignWithUserCeramicImage(
            IFormFile roomImageFile, 
            IFormFile ceramicOrPaintImageFile, 
            DesignType designType)
        {
            var userId = _claimsService.GetCurrentUserId();

            // Check subscription - will throw exception if limit exceeded

            using var roomStream = roomImageFile.OpenReadStream();
            using var ceramicOrPaintStream = ceramicOrPaintImageFile.OpenReadStream();
            using var roomMemoryStream = new MemoryStream();
            using var ceramicMemoryStream = new MemoryStream();

            await roomStream.CopyToAsync(roomMemoryStream);
            await ceramicOrPaintStream.CopyToAsync(ceramicMemoryStream);

            var request = new AddDesignWithUserCeramicImageCommand
            {
                roomBytes = roomMemoryStream.ToArray(),
                ceramicOrPaintBytes = ceramicMemoryStream.ToArray(),
                roomimageName = roomImageFile.FileName,
                ceramicOrPaintimageName = ceramicOrPaintImageFile.FileName,
                designType = designType
            };

            var result = await _mediator.Send(request);

            // Increment counter after successful generation

            var response = ApiResponse<object>.SuccessResponse(
                new
                {
                    generatedImageUrl = result.GeneratedImageUrl,
                    designId = result.designId
                },
                "Design generated successfully");

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDesigns()
        {
            var request = new GetAllDesignsQuery();
            var result = await _mediator.Send(request);

            var response = ApiResponse<object>.SuccessResponse(result, "Designs retrieved successfully");
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetDesignById(string designId)
        {
            var request = new GetDesignByIdQuery { designId = designId };
            var result = await _mediator.Send(request);

            var response = ApiResponse<object>.SuccessResponse(result, "Design retrieved successfully");
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> SoftDeleteDesign(string designId)
        {
            var request = new DesignSoftDeleteCommand { designId = designId };
            var result = await _mediator.Send(request);

            var response = ApiResponse<object>.SuccessResponse(result, "Design deleted successfully");
            return Ok(response);
        }
    }
}
