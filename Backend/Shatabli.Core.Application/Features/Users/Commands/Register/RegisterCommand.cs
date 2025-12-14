using MediatR;

namespace Shatabli.Core.Application.Features.Users.Commands.Register
{
    public class RegisterCommand : IRequest<int>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}