using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;
using Shatabli.Core.Domain.Entities;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Application.Features.Subscriptions.Queries.CheckCanGenerate
{
    public class CheckCanGenerateQueryHandler : IRequestHandler<CheckCanGenerateQuery, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IClaimsService _claimsService;

        public CheckCanGenerateQueryHandler(IApplicationDbContext context, IClaimsService claimsService)
        {
            _context = context;
            _claimsService = claimsService;
        }

        public async Task<Result> Handle(CheckCanGenerateQuery request, CancellationToken cancellationToken)
        {
            var userId = _claimsService.GetCurrentUserId();

            var subscription = await _context.UserSubscriptions
                .Include(s => s.SubscriptionPlan)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.IsActive, cancellationToken);

            // Auto-assign free plan if no subscription
            if (subscription == null)
            {
                var freePlan = await _context.SubscriptionPlans
                    .FirstOrDefaultAsync(p => p.Type == PlanType.Free, cancellationToken);

                if (freePlan == null)
                {
                    throw new InvalidOperationException("Free plan is not available in the system");
                }

                // Create free subscription
                subscription = new UserSubscription
                {
                    UserId = userId,
                    SubscriptionPlanId = freePlan.Id,
                    StartDate = DateTime.UtcNow,
                    IsActive = true,
                    ImagesGeneratedThisMonth = 0,
                    ImagesGeneratedToday = 0,
                    LastResetDate = DateTime.UtcNow
                };

                _context.UserSubscriptions.Add(subscription);
                await _context.SaveChangesAsync(cancellationToken);

                subscription.SubscriptionPlan = freePlan;
            }

            // Reset counters if needed
            await ResetCountersIfNeededAsync(subscription, cancellationToken);

            var plan = subscription.SubscriptionPlan;

            // Unlimited plan
            if (plan.MaxImagesPerDay == -1 && plan.MaxImagesPerMonth == -1)
                return Result.Success("You can generate images");

            // Check daily limit
            if (plan.MaxImagesPerDay > 0 && subscription.ImagesGeneratedToday >= plan.MaxImagesPerDay)
            {
                return (Result)Result.Forbidden(
                    $"You have reached your daily limit of {plan.MaxImagesPerDay} images. Please upgrade or wait until tomorrow");
            }

            // Check monthly limit
            if (plan.MaxImagesPerMonth > 0 && subscription.ImagesGeneratedThisMonth >= plan.MaxImagesPerMonth)
            {
                return (Result)Result.Forbidden(
                    $"You have reached your monthly limit of {plan.MaxImagesPerMonth} images. Please upgrade to a higher plan");
            }

            return Result.Success("You can generate images");
        }

        private async Task ResetCountersIfNeededAsync(UserSubscription subscription, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            bool needsUpdate = false;

            if (subscription.LastResetDate.Date < now.Date)
            {
                subscription.ImagesGeneratedToday = 0;
                needsUpdate = true;
            }

            if (subscription.LastResetDate.Month != now.Month || subscription.LastResetDate.Year != now.Year)
            {
                subscription.ImagesGeneratedThisMonth = 0;
                needsUpdate = true;
            }

            if (needsUpdate)
            {
                subscription.LastResetDate = now;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}