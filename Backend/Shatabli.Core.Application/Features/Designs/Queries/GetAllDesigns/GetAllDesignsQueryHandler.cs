using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicAndPaint;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Core.Application.Features.Designs.Queries.GetAllDesigns
{
    public class GetAllDesignsQueryHandler : IRequestHandler<GetAllDesignsQuery, Result<GetAllDesignsResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public readonly IClaimsService _claimsService;

        public GetAllDesignsQueryHandler(IApplicationDbContext context, IClaimsService claimsService, IMapper mapper)
        {
            _context = context;
            _claimsService = claimsService;
            _mapper = mapper;
        }


        async Task<Result<GetAllDesignsResponse>> IRequestHandler<GetAllDesignsQuery, Result<GetAllDesignsResponse>>.Handle(GetAllDesignsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = _claimsService.GetCurrentUserId();

                if (string.IsNullOrEmpty(userId))
                {
                    return Result<GetAllDesignsResponse>.Unauthorized("User session is invalid or expired.");
                }

                var designs = await _context.Designs
                    .Where(d => d.UserId == userId&&d.GeneratedImageUrl!=null)
                    .OrderByDescending(d => d.CreatedAt) 
                    .ProjectTo<GetAllDesignDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                var response = new GetAllDesignsResponse
                {
                    designsList = designs ?? new List<GetAllDesignDTO>()
                };

                return Result<GetAllDesignsResponse>.Success(response, "Designs retrieved successfully.");
            }
            catch (Exception ex)
            {
                return Result<GetAllDesignsResponse>.Failure(
                    message: "An unexpected error occurred during Getting all designes.",
                    statusCode: 500,
                    errors: new List<string> { ex.Message }
                );
            }
        }
    }
}
