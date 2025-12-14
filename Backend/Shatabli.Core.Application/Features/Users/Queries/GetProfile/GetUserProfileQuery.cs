using MediatR;

namespace Shatabli.Core.Application.Features.Users.Queries.GetProfile
{
    public class GetUserProfileQuery : IRequest<GetUserProfileQueryResponse>
    {
        public int UserId { get; set; }
    }
}