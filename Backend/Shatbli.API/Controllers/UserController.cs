using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shatbli.Core.Bases;
using Shatbli.Core.Features.Users.Commands.Models;
using Shatbli.Core.Features.Users.Queries.Models;
using Shatbli.Data.DTOs.UserDtos;
using Shatbli.Data.MetaData;
using System.Security.Claims;

namespace Shatbli.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Router.UserRouting.Register)]
        public async Task<Response<UserProfileDto>> Register([FromBody] RegisterUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost(Router.UserRouting.Login)]
        public async Task<Response<AuthResponseDto>> Login([FromBody] LoginUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [Authorize]
        [HttpGet(Router.UserRouting.Profile)]
        public async Task<Response<UserProfileDto>> GetProfile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var query = new GetUserProfileQuery { UserId = userId };
            return await _mediator.Send(query);
        }
    }
}