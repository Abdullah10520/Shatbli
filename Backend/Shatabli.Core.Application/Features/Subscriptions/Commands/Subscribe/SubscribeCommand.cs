using MediatR;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Core.Application.Features.Subscriptions.Commands.Subscribe
{
    public class SubscribeCommand : IRequest<Result>
    {
        public string PlanId { get; set; } = string.Empty;
    }
}