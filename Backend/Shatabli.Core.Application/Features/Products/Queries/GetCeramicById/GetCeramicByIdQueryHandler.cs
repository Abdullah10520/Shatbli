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

namespace Shatabli.Core.Application.Features.Products.Queries.GetCeramicById
{
    public class GetCeramicByIdQueryHandler : IRequestHandler<GetCeramicByIdQuery, Result<GetCeramicByIdResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCeramicByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        async Task<Result<GetCeramicByIdResponse>> IRequestHandler<GetCeramicByIdQuery, Result<GetCeramicByIdResponse>>.Handle(GetCeramicByIdQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var ceramic = await _context.Products
                    .Where(p => p.Id == request.ceramicId && p.Category == ProductCategory.FlooringCeramics)
                    .ProjectTo<CeramicDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                if (ceramic == null)
                {
                    return Result<GetCeramicByIdResponse>.NotFound($"Ceramic product with ID {request.ceramicId} not found.");
                }

                var response = new GetCeramicByIdResponse
                {
                    ceramicProduct = ceramic
                };

                return Result<GetCeramicByIdResponse>.Success(response, "Product retrieved successfully.");
            }
            catch (Exception ex)
            {
                return Result<GetCeramicByIdResponse>.Failure(
                    message: "An unexpected error occurred during getting product.",
                    statusCode: 500,
                    errors: new List<string> { ex.Message }
                    );
            }
        }
    }
}
