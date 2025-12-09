using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Data.DTOs.UserDtos;

namespace Shatbli.Core.Features.Users.Commands.Models
{
    public class RegisterUserCommand : IRequest<Response<UserProfileDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}