using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;
using Shatabli.Core.Domain.Entities;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Application.Features.Subscriptions.Commands.CancelSubscription
{
    public class CancelSubscriptionCommandHandler : IRequestHandler<CancelSubscriptionCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IClaimsService _claimsService;

        public CancelSubscriptionCommandHandler(IApplicationDbContext context, IClaimsService claimsService)
        {
            _context = context;
            _claimsService = claimsService;
        }

        public async Task<Result> Handle(CancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var userId = _claimsService.GetCurrentUserId();

            var subscription = await _context.UserSubscriptions
                .AsTracking()
                .Include(s => s.SubscriptionPlan)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive, cancellationToken);

            if (subscription == null)
            {
                return Result.NotFound("No active subscription found");
            }

            // ✅ Deactivate current subscription
            subscription.IsActive = false;
            subscription.EndDate = DateTime.UtcNow;

            // ✅ Revert to free plan
            var freePlan = await _context.SubscriptionPlans
                .FirstOrDefaultAsync(p => p.Type == PlanType.Free, cancellationToken);

            if (freePlan != null)
            {
                var freeSubscription = new UserSubscription
                {
                    UserId = userId,
                    SubscriptionPlanId = freePlan.Id,
                    StartDate = DateTime.UtcNow,
                    IsActive = true,
                    ImagesGeneratedThisMonth = 0,
                    ImagesGeneratedToday = 0,
                    LastResetDate = DateTime.UtcNow
                };

                _context.UserSubscriptions.Add(freeSubscription);
            }

            // ✅ Save both changes together
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("Subscription cancelled and reverted to free plan");
        }
    }
}