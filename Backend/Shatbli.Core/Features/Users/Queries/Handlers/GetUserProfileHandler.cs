using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Core.Features.Users.Queries.Models;
using Shatbli.Data.DTOs.UserDtos;
using Shatbli.Service.Interfaces;

namespace Shatbli.Core.Features.Users.Queries.Handlers
{
    public class GetUserProfileHandler : ResponseHandler, IRequestHandler<GetUserProfileQuery, Response<UserProfileDto>>
    {
        private readonly IUserService _userService;

        public GetUserProfileHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Response<UserProfileDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var profile = await _userService.GetUserProfileAsync(request.UserId);
            return Success(profile);
        }
    }
}