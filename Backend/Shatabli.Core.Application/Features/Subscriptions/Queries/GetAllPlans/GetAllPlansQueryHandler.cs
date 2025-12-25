using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Core.Application.Features.Subscriptions.Queries.GetAllPlans
{
    public class GetAllPlansQueryHandler : IRequestHandler<GetAllPlansQuery, Result<List<GetAllPlansQueryResponse>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllPlansQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllPlansQueryResponse>>> Handle(GetAllPlansQuery request, CancellationToken cancellationToken)
        {
            var plans = await _context.SubscriptionPlans
                .AsNoTracking()
                .ProjectTo<GetAllPlansQueryResponse>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return Result<List<GetAllPlansQueryResponse>>.Success(plans, "Plans retrieved successfully");
        }
    }
}