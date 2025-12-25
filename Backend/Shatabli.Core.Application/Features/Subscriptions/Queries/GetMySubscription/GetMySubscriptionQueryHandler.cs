using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Core.Application.Features.Subscriptions.Queries.GetMySubscription
{
    public class GetMySubscriptionQueryHandler : IRequestHandler<GetMySubscriptionQuery, Result<GetMySubscriptionQueryResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;

        public GetMySubscriptionQueryHandler(IApplicationDbContext context, IMapper mapper, IClaimsService claimsService)
        {
            _context = context;
            _mapper = mapper;
            _claimsService = claimsService;
        }

        public async Task<Result<GetMySubscriptionQueryResponse>> Handle(GetMySubscriptionQuery request, CancellationToken cancellationToken)
        {
            var userId = _claimsService.GetCurrentUserId();

            var subscription = await _context.UserSubscriptions
                .Include(s => s.SubscriptionPlan)
                .Where(s => s.UserId == userId && s.IsActive)
                .ProjectTo<GetMySubscriptionQueryResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (subscription == null)
            {
                return Result<GetMySubscriptionQueryResponse>.NotFound("No active subscription found");
            }

            return Result<GetMySubscriptionQueryResponse>.Success(subscription, "Subscription retrieved successfully");
        }
    }
}