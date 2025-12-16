using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Shatabli.Core.Application.Interfaces;

namespace Shatabli.Core.Application.Features.Designs.Queries.GetDesignById
{
    public class GetDesignByIdQueryHandler : IRequestHandler<GetDesignByIdQuery, GetDesignByIdResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDesignByIdQueryHandler(IApplicationDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        async Task<GetDesignByIdResponse> IRequestHandler<GetDesignByIdQuery, GetDesignByIdResponse>.Handle(GetDesignByIdQuery request, CancellationToken cancellationToken)
        {
            var result = _context.Designs
                .Where(d => d.Id == request.designId && d.UserId == "4")
                .ProjectTo<GetDesignByIdDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefault();

            GetDesignByIdResponse response = new();
            response.getDesignByIdDTO = result;


            return response;

        }
    }
}
