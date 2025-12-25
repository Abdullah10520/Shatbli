using MediatR;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Core.Application.Features.Subscriptions.Commands.CancelSubscription
{
    public class CancelSubscriptionCommand : IRequest<Result>
    {
    }
}