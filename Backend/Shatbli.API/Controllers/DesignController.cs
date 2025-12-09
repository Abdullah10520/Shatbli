using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shatbli.Core.Features.Designs.Commands.Models;
using Shatbli.Core.Features.Designs.Queries.Models;
using Shatbli.Data.MetaData;
using Shatbli.Service.Interfaces;
using System.Security.Claims;

namespace Shatbli.API.Controllers
{
    [ApiController]
    [Authorize]
    public class DesignController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDesignService _designService;

        public DesignController(IMediator mediator, IDesignService designService)
        {
            _mediator = mediator;
            _designService = designService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        [HttpPost(Router.DesignRouting.CreateCeramicDesign)]
        public async Task<IActionResult> CreateCeramicDesign([FromForm] CreateCeramicDesignCommand command)
        {
            command.UserId = GetUserId();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost(Router.DesignRouting.CreateWallPaintDesign)]
        public async Task<IActionResult> CreateWallPaintDesign([FromForm] CreateWallPaintDesignCommand command)
        {
            command.UserId = GetUserId();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost(Router.DesignRouting.ProcessDesign)]
        public async Task<IActionResult> ProcessDesign([FromRoute] int designId)
        {
            var command = new ProcessDesignCommand
            {
                DesignId = designId,
                UserId = GetUserId()
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet(Router.DesignRouting.GetDesignById)]
        public async Task<IActionResult> GetDesignById([FromRoute] int id)
        {
            var query = new GetDesignByIdQuery
            {
                DesignId = id,
                UserId = GetUserId()
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet(Router.DesignRouting.GetUserDesigns)]
        public async Task<IActionResult> GetUserDesigns()
        {
            var query = new GetUserDesignsQuery
            {
                UserId = GetUserId()
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet(Router.DesignRouting.DownloadImage)]
        public async Task<IActionResult> DownloadImage([FromRoute] int designId, [FromQuery] bool generated = true)
        {
            var imageBytes = await _designService.GetImageForDownloadAsync(designId, GetUserId(), generated);

            if (imageBytes == null)
                return NotFound("Image not found");

            await _designService.MarkAsDownloadedAsync(designId);

            var fileName = generated ? $"generated_design_{designId}.jpg" : $"original_design_{designId}.jpg";
            return File(imageBytes, "image/jpeg", fileName);
        }
    }
}