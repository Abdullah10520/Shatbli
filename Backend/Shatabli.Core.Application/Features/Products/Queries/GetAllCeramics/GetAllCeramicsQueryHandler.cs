using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shatabli.Core.Application.Features.Designs.Queries.GetAllDesigns;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;
using Shatabli.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Products.Queries.GetAllCeramics
{
    public class GetAllCeramicsQueryHandler : IRequestHandler<GetAllCeramicsQuery, Result<GetAllCeramicsResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllCeramicsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        async Task<Result<GetAllCeramicsResponse>> IRequestHandler<GetAllCeramicsQuery, Result<GetAllCeramicsResponse>>.Handle(GetAllCeramicsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _context.Products
                    .Where(p => p.Category == ProductCategory.FlooringCeramics)
                    .ProjectTo<CeramicDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                var response = new GetAllCeramicsResponse
                {
                    ceramicList = result ?? new List<CeramicDTO>()
                };

                return Result<GetAllCeramicsResponse>.Success(response, "Ceramics retrieved successfully.");
            }
            catch (Exception ex)
            {
                return Result<GetAllCeramicsResponse>.Failure(
                    message: "An error occurred while fetching ceramics.",
                    statusCode: 500,
                    errors: new List<string> { ex.Message }
                    );
            }
        }
    }
}
