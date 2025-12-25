using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shatabli.Core.Application.Features.Users.Commands.ChangePassword;
using Shatabli.Core.Application.Features.Users.Commands.Login;
using Shatabli.Core.Application.Features.Users.Commands.Register;
using Shatabli.Core.Application.Features.Users.Commands.UpdateProfile;
using Shatabli.Core.Application.Features.Users.Queries.GetProfile;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;

namespace Shatabli.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator, IClaimsService claimsService)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, 
                    ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            var response = ApiResponse<object>.SuccessResponse(null, result.Message);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, 
                    ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            var response = ApiResponse<object>.SuccessResponse(result.Data, result.Message);
            return Ok(response);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var query = new GetUserProfileQuery();
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            var response = ApiResponse<object>.SuccessResponse(result.Data, result.Message);
            return Ok(response);
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
        {
           
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            var response = ApiResponse<object>.SuccessResponse(null, result.Message);
            return Ok(response);
        }

        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResponse(result.Message, result.Errors));
            }

            var response = ApiResponse<object>.SuccessResponse(null, result.Message);
            return Ok(response);
        }
    }
}