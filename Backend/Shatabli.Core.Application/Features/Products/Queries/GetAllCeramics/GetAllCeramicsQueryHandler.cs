using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Application.Features.Products.Queries.GetAllCeramics
{
    public class GetAllCeramicsQueryHandler : IRequestHandler<GetAllCeramicsQuery, GetAllCeramicsResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllCeramicsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        async Task<GetAllCeramicsResponse> IRequestHandler<GetAllCeramicsQuery, GetAllCeramicsResponse>.Handle(GetAllCeramicsQuery request, CancellationToken cancellationToken)
        {

            var result = await _context.Products
                .Where(p => p.Category == ProductCategory.FlooringCeramics)
                .ProjectTo<CeramicDTO>(_mapper.ConfigurationProvider).ToListAsync();

            GetAllCeramicsResponse response = new();
            response.ceramicList = result;

            return response;
        }
    }
}
