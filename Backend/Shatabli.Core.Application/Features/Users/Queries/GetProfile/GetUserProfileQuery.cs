using MediatR;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Core.Application.Features.Users.Queries.GetProfile
{
    public class GetUserProfileQuery : IRequest<Result<GetUserProfileQueryResponse>>
    {
    }
}