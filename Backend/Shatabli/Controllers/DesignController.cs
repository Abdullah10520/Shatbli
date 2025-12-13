using System.Threading.Tasks;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage;
using Shatabli.Core.Application.Features.Products.Queries.GetProductImageById;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
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
        public async Task<IActionResult> GenerateDesign(IFormFile imageFile , string ceramicId)
        {
            var imageStream = imageFile.OpenReadStream();
            AddOrignalImageCommand request = new AddOrignalImageCommand();
            request.stream = imageStream;
            request.ImageName = imageFile.FileName;
            request.CeramicId = ceramicId;

            var result = await _mediator.Send(request);



            return File(result.GeneratedImage , "image/png");

        }

        [HttpPost]
        public async Task<IActionResult> GenerateDesignWithUserCeramicImage(IFormFile roomImageFile, IFormFile ceramicImageFile)
        {

            return Ok();

        }

    }
}
