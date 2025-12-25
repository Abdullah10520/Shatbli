using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Core.Application.Features.Users.Queries.GetProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Result<GetUserProfileQueryResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;

        public GetUserProfileQueryHandler(IApplicationDbContext context, IMapper mapper, IClaimsService claimsService)
        {
            _context = context;
            _mapper = mapper;
            _claimsService = claimsService;
        }

        public async Task<Result<GetUserProfileQueryResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _claimsService.GetCurrentUserId();
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .ProjectTo<GetUserProfileQueryResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            // ✅ Business validation - NO exception
            if (user == null)
            {
                return Result<GetUserProfileQueryResponse>.NotFound($"User with ID '{userId}' was not found");
            }

            return Result<GetUserProfileQueryResponse>.Success(user, "Profile retrieved successfully");
        }
    }
}