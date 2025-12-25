using MediatR;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Core.Application.Features.Subscriptions.Queries.GetMySubscription
{
    public class GetMySubscriptionQuery : IRequest<Result<GetMySubscriptionQueryResponse>>
    {
    }
}