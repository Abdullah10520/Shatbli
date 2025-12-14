using MediatR;

namespace Shatabli.Core.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}