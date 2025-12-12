using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage;

namespace Shatabli.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DesignController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> saveImage(IFormFile imageFile)
        {
            var imageStream = imageFile.OpenReadStream();
            AddOrignalImageCommand request = new AddOrignalImageCommand();
            request.stream = imageStream;
            request.ImageName = imageFile.FileName;

            var result = await _mediator.Send(request);

            return Ok(result);

        }

    }
}
