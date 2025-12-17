using System.Threading.Tasks;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicImage;
using Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage;
using Shatabli.Core.Application.Features.Designs.Commands.SoftDeleteDesign;
using Shatabli.Core.Application.Features.Designs.Queries.GetAllDesigns;
using Shatabli.Core.Application.Features.Designs.Queries.GetDesignById;
using Shatabli.Core.Application.Features.Products.Queries.GetProductImageById;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Entities;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class DesignController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IStorageService _storageService;

        public DesignController(IMediator mediator , IStorageService storageService)
        {
            _mediator = mediator;
            _storageService = storageService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateDesign(IFormFile imageFile , string ceramicId ,DesignType designType)
        {
            var imageStream = imageFile.OpenReadStream();

            MemoryStream roomImageMemoryStream = new MemoryStream();

            await imageStream.CopyToAsync(roomImageMemoryStream);

            AddDesignWithOurCeramicImageCommand request = new AddDesignWithOurCeramicImageCommand();
            request.stream = roomImageMemoryStream.ToArray();
            request.imageName = imageFile.FileName;
            request.ceramicId = ceramicId;
            request.designType = designType; 

            var result = await _mediator.Send(request);



            return File(result.GeneratedImage , "image/png");

        }

        [HttpPost]
        public async Task<IActionResult> GenerateDesignWithUserCeramicImage(IFormFile roomImageFile ,IFormFile ceramicOrPaintImageFile ,DesignType designType)
        {
            var roomStream = roomImageFile.OpenReadStream();
            var ceramicOrPaintStream = ceramicOrPaintImageFile.OpenReadStream();

            MemoryStream roomMemoryStream = new MemoryStream();
            MemoryStream ceramicMemoryStream = new MemoryStream();

            await roomStream.CopyToAsync(roomMemoryStream);
            await ceramicOrPaintStream.CopyToAsync(ceramicMemoryStream);

            var roomBytes = roomMemoryStream.ToArray();
            var ceramicOrPaintBytes = ceramicMemoryStream.ToArray();



            AddDesignWithUserCeramicImageCommand request = new AddDesignWithUserCeramicImageCommand();
            request.roomBytes = roomBytes;
            request.ceramicOrPaintBytes = ceramicOrPaintBytes;
            request.roomimageName = roomImageFile.FileName;
            request.ceramicOrPaintimageName = ceramicOrPaintImageFile.FileName;
            request.designType = designType;

            var result = await _mediator.Send(request);

            return File(result.GeneratedImage, "image/png");


            //return Ok();

        }
        [HttpGet]
        public async Task<IActionResult> GetAllDesigns()
        {
            GetAllDesignsQuery request = new GetAllDesignsQuery();

            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDesignById(string designId)
        {
            GetDesignByIdQuery request = new();
            request.designId = designId;

            var response = await _mediator.Send(request);

            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> SoftDeleteDesign(string designId)
        {
            DesignSoftDeleteCommand request = new DesignSoftDeleteCommand();
            request.designId = designId;

            var response = await _mediator.Send(request);

            return Ok(response);
        }


    }
}
