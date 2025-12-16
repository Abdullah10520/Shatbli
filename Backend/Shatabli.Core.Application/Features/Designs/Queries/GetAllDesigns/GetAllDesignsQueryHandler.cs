using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public GetAllDesignsQueryHandler(IApplicationDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        async Task<GetAllDesignsResponse> IRequestHandler<GetAllDesignsQuery, GetAllDesignsResponse>.Handle(GetAllDesignsQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Designs
            .Where(d => d.UserId == "4")
            .ProjectTo<GetAllDesignDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();

            GetAllDesignsResponse response = new GetAllDesignsResponse();
            response.designsList = result;


            return response;
        }
    }
}
