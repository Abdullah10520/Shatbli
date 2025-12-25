using MediatR;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Core.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<Result>
    {
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}