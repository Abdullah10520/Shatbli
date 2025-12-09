using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Data.DTOs.UserDtos;

namespace Shatbli.Core.Features.Users.Queries.Models
{
    public class GetUserProfileQuery : IRequest<Response<UserProfileDto>>
    {
        public int UserId { get; set; }
    }
}