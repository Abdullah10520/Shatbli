using FluentValidation;

namespace Shatabli.Core.Application.Features.Subscriptions.Commands.Subscribe
{
    public class SubscribeCommandValidator : AbstractValidator<SubscribeCommand>
    {
        public SubscribeCommandValidator()
        {
            RuleFor(x => x.PlanId)
                .NotEmpty().WithMessage("Plan ID is required")
                .NotNull().WithMessage("Plan ID cannot be null");
        }
    }
}