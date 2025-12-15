using MediatR;

namespace Shatabli.Core.Application.Features.Users.Queries.GetProfile
{
    public class GetUserProfileQuery : IRequest<GetUserProfileQueryResponse>
    {
        public string UserId { get; set; }
    }
}