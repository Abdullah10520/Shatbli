using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;

namespace Shatabli.Core.Application.Features.Subscriptions.Queries.GetAllPlans
{
    public class GetAllPlansQueryHandler : IRequestHandler<GetAllPlansQuery, Result<List<GetAllPlansQueryResponse>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public const string CacheKey = "AllPlans";
        
        public GetAllPlansQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllPlansQueryResponse>>> Handle(GetAllPlansQuery request, CancellationToken cancellationToken)
        {
            if (_cache.TryGetValue(CacheKey, out List<GetAllPlansQueryResponse> cachedPlans))
            {
                return Result<List<GetAllPlansQueryResponse>>.Success(cachedPlans, "Plans retrieved from cache successfully");
            }

            var plans = await _context.SubscriptionPlans
                .AsNoTracking()
                .ProjectTo<GetAllPlansQueryResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.NeverRemove
            };
            
            _cache.Set(CacheKey, plans, cacheEntryOptions);
            return Result<List<GetAllPlansQueryResponse>>.Success(plans, "Plans retrieved successfully");
        }
    }
}