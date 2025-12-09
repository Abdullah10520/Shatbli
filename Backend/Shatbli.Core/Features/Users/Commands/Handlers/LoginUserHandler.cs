using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Core.Features.Users.Commands.Models;
using Shatbli.Data.DTOs.UserDtos;
using Shatbli.Service.Interfaces;

namespace Shatbli.Core.Features.Users.Commands.Handlers
{
    public class LoginUserHandler : ResponseHandler, IRequestHandler<LoginUserCommand, Response<AuthResponseDto>>
    {
        private readonly IUserService _userService;

        public LoginUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Response<AuthResponseDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _userService.LoginUserAsync(new LoginUserDto
            {
                Email = request.Email,
                Password = request.Password
            });

            return Success(result);
        }
    }
}