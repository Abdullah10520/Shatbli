using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Core.Features.Users.Commands.Models;
using Shatbli.Data.DTOs.UserDtos;
using Shatbli.Service.Interfaces;

namespace Shatbli.Core.Features.Users.Commands.Handlers
{
    public class RegisterUserHandler : ResponseHandler, IRequestHandler<RegisterUserCommand, Response<UserProfileDto>>
    {
        private readonly IUserService _userService;

        public RegisterUserHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Response<UserProfileDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userService.RegisterUserAsync(new RegisterUserDto
            {
                Email = request.Email,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber
            });

            var profileDto = new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                IsEmailVerified = user.IsEmailVerified,
                TotalDesigns = 0
            };

            return Created(profileDto);
        }
    }
}