using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;

namespace Shatabli.Core.Application.Features.Designs.Queries.GetAllDesigns
{
    public class GetAllDesignsQueryHandler : IRequestHandler<GetAllDesignsQuery, GetAllDesignsResponse>
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


        async Task<GetAllDesignsResponse> IRequestHandler<GetAllDesignsQuery, GetAllDesignsResponse>.Handle(GetAllDesignsQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Designs
            .Where(d => d.UserId == _claimsService.GetCurrentUserId())
            .ProjectTo<GetAllDesignDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

            GetAllDesignsResponse response = new GetAllDesignsResponse();
            response.designsList = result;


            return response;
        }
    }
}
