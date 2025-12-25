using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shatabli.Core.Application.Features.Subscriptions.Commands.CancelSubscription;
using Shatabli.Core.Application.Features.Subscriptions.Commands.Subscribe;
using Shatabli.Core.Application.Features.Subscriptions.Queries.CheckCanGenerate;
using Shatabli.Core.Application.Features.Subscriptions.Queries.GetAllPlans;
using Shatabli.Core.Application.Features.Subscriptions.Queries.GetMySubscription;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SubscriptionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubscriptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPlans()
        {
            var query = new GetAllPlansQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            var response = ApiResponse<object>.SuccessResponse(result.Data, result.Message);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetMySubscription()
        {
            var query = new GetMySubscriptionQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            var response = ApiResponse<object>.SuccessResponse(result.Data, result.Message);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            var response = ApiResponse<object>.SuccessResponse(null, result.Message);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CancelSubscription()
        {
            var command = new CancelSubscriptionCommand();
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            var response = ApiResponse<object>.SuccessResponse(null, result.Message);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> CanGenerateImage()
        {
            var query = new CheckCanGenerateQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            var response = ApiResponse<object>.SuccessResponse(new { canGenerate = true }, result.Message);
            return Ok(response);
        }
    }
}
