using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;
using Shatabli.Core.Domain.Entities;

namespace Shatabli.Core.Application.Features.Subscriptions.Commands.Subscribe
{
    public class SubscribeCommandHandler : IRequestHandler<SubscribeCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IClaimsService _claimsService;

        public SubscribeCommandHandler(IApplicationDbContext context, IClaimsService claimsService)
        {
            _context = context;
            _claimsService = claimsService;
        }

        public async Task<Result> Handle(SubscribeCommand request, CancellationToken cancellationToken)
        {
            var userId = _claimsService.GetCurrentUserId();

            // Validate plan exists
            var plan = await _context.SubscriptionPlans.FindAsync(new object[] { request.PlanId }, cancellationToken);
            if (plan == null)
            {
                return Result.NotFound($"Subscription plan with ID '{request.PlanId}' not found");
            }

            // Deactivate existing subscriptions
            var existingSubscriptions = await _context.UserSubscriptions
                .AsTracking()
                .Where(s => s.UserId == userId && s.IsActive)
                .ToListAsync(cancellationToken);

            foreach (var sub in existingSubscriptions)
            {
                sub.IsActive = false;
                sub.EndDate = DateTime.UtcNow;
            }

            // Create new subscription
            var newSubscription = new UserSubscription
            {
                UserId = userId,
                SubscriptionPlanId = request.PlanId,
                StartDate = DateTime.UtcNow,
                IsActive = true,
                ImagesGeneratedThisMonth = 0,
                ImagesGeneratedToday = 0,
                LastResetDate = DateTime.UtcNow
            };

            _context.UserSubscriptions.Add(newSubscription);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success($"Successfully subscribed to {plan.Name} plan");
        }
    }
}