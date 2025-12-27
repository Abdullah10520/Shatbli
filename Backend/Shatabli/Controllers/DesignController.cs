using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithOurCeramicAndPaint;
using Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithPaintOnly;
using Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicAndPaint;
using Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicImage;
using Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage;
using Shatabli.Core.Application.Features.Designs.Commands.SaveDesign;
using Shatabli.Core.Application.Features.Designs.Commands.SoftDeleteDesign;
using Shatabli.Core.Application.Features.Designs.Queries.GetAllDesigns;
using Shatabli.Core.Application.Features.Designs.Queries.GetDesignById;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;
using Shatabli.Core.Domain.Enums;
using System.Threading.Tasks;

namespace Shatabli.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DesignController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStorageService _storageService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DesignController(IMediator mediator, IStorageService storageService, IWebHostEnvironment webHostEnvironment)
        {
            _mediator = mediator;
            _storageService = storageService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateDesignWIthOurCeramic(IFormFile imageFile,string ceramicId)
        {
            using var roomImageMemoryStream = new MemoryStream();
            await imageFile.CopyToAsync(roomImageMemoryStream);

            var request = new AddDesignWithOurCeramicImageCommand
            {
                roomBytes = roomImageMemoryStream.ToArray(),
                roomimageName = imageFile.FileName,
                ceramicId = ceramicId,
                designType = DesignType.CeramicFloor
            };

            var result = await _mediator.Send(request);

             if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var responseData = result.Data;
            responseData.GeneratedImagePath = $"{baseUrl}{responseData.GeneratedImagePath}";

            return Ok(ApiResponse<AddDesignWithOurCeramicImageResponse>.SuccessResponse(responseData, result.Message));
        }

        [HttpPost]
        public async Task<IActionResult> GenerateDesignWithUserCeramicImage(IFormFile roomImageFile, IFormFile ceramicImageFile)
        {

            using var roomMemoryStream = new MemoryStream();
            using var ceramicMemoryStream = new MemoryStream();

            await roomImageFile.CopyToAsync(roomMemoryStream);
            await ceramicImageFile.CopyToAsync(ceramicMemoryStream);

            var request = new AddDesignWithUserCeramicImageCommand
            {
                roomBytes = roomMemoryStream.ToArray(),
                ceramicBytes = ceramicMemoryStream.ToArray(),
                roomimageName = roomImageFile.FileName,
                ceramicImageName = ceramicImageFile.FileName,
                designType = DesignType.CeramicFloor
            };

            var result = await _mediator.Send(request);


            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var responseData = result.Data;
            responseData.generatedImagePath = $"{baseUrl}{responseData.generatedImagePath}";

            return Ok(ApiResponse<AddDesignWithUserCeramicImageResponse>.SuccessResponse(responseData, result.Message));
        }

        [HttpPost]
        public async Task<IActionResult> GenerateDesignWithOurCeramicAndPaint(IFormFile roomImageFile, string ceramicId, string colorCode)
        {

            using var roomMemoryStream = new MemoryStream();
            await roomImageFile.CopyToAsync(roomMemoryStream);

            var request = new AddDesignWithOurCeramicAndPaintCommand
            {
                roomBytes = roomMemoryStream.ToArray(),
                ceramicId = ceramicId,
                roomimageName = roomImageFile.FileName,
                colorCode = colorCode
            };

            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var responseData = result.Data;
            responseData.generatedImagePath = $"{baseUrl}{responseData.generatedImagePath}";

            return Ok(ApiResponse<AddDesignWithOurCeramicAndPaintResponse>.SuccessResponse(responseData, result.Message));
        }

        [HttpPost]
        public async Task<IActionResult> GenerateDesignWithUserCeramicAndPaint(IFormFile roomImageFile, IFormFile ceramicImageFile, string colorCode)
        { 
            using var roomMemoryStream = new MemoryStream();
            using var ceramicMemoryStream = new MemoryStream();

            await roomImageFile.CopyToAsync(roomMemoryStream);
            await ceramicImageFile.CopyToAsync(ceramicMemoryStream);

            var request = new AddDesignWithUserCeramicAndPaintCommand
            {
                roomBytes = roomMemoryStream.ToArray(),
                ceramicBytes = ceramicMemoryStream.ToArray(),
                roomimageName = roomImageFile.FileName,
                ceramicImageName = ceramicImageFile.FileName,
                colorCode = colorCode
            };

            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var responseData = result.Data;
            responseData.generatedImagePath = $"{baseUrl}{responseData.generatedImagePath}";

            return Ok(ApiResponse<AddDesignWithUserCeramicAndPaintResponse>.SuccessResponse(responseData, result.Message));
        }

        [HttpPost]
        public async Task<IActionResult> GenerateDesignWithPaintOnly(IFormFile roomImage, string colorCode)
        {
            using var roomImageStream = new MemoryStream();

            await roomImage.CopyToAsync(roomImageStream);

            AddDesignWithPaintCommand request = new AddDesignWithPaintCommand
            {
                roomBytes = roomImageStream.ToArray(),
                roomimageName = roomImage.FileName,
                colorCode = colorCode
            };
            var result = await _mediator.Send(request);

            if(!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message,result.Errors));
            }
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var responseData = result.Data;
            responseData.generatedImagePath = $"{baseUrl}{responseData.generatedImagePath}";

            return Ok(ApiResponse<AddDesignWithPaintResponse>.SuccessResponse(responseData, result.Message));
        }

        [HttpPost]
        public async Task<IActionResult> SaveDesign(SaveDesignCommand request)
        {
            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }
            return Ok(ApiResponse<SaveDesignResponse>.SuccessResponse(result.Data, result.Message));
        }


        [HttpGet]
        public async Task<IActionResult> GetAllDesigns()
        {
            var result = await _mediator.Send(new GetAllDesignsQuery());

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            return Ok(ApiResponse<GetAllDesignsResponse>.SuccessResponse(result.Data, result.Message));

        }

        [HttpGet]
        public async Task<IActionResult> GetDesignById(string designId)
        {

            var query = new GetDesignByIdQuery { designId = designId };
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            return Ok(ApiResponse<GetDesignByIdResponse>.SuccessResponse(result.Data));
        }
        [HttpDelete]
        public async Task<IActionResult> SoftDeleteDesign(string designId)
        {
            var request = new DesignSoftDeleteCommand { designId = designId };
            var result = await _mediator.Send(request);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            return Ok(ApiResponse<DesignSoftDeleteResponse>.SuccessResponse(result.Data, result.Message));
        }
    }
}