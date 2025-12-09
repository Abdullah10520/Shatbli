using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Data.DTOs.UserDtos;

namespace Shatbli.Core.Features.Users.Commands.Models
{
    public class LoginUserCommand : IRequest<Response<AuthResponseDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}