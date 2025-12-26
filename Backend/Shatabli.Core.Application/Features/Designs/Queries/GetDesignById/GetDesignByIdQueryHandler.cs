using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Features.Designs.Queries.GetAllDesigns;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Designs.Queries.GetDesignById
{
    public class GetDesignByIdQueryHandler : IRequestHandler<GetDesignByIdQuery, Result<GetDesignByIdResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;

        public GetDesignByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IClaimsService claimsService) 
        {
            _context = context;
            _mapper = mapper;
            _claimsService = claimsService;
        }
        async Task<Result<GetDesignByIdResponse>> IRequestHandler<GetDesignByIdQuery, Result<GetDesignByIdResponse>>.Handle(GetDesignByIdQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var currentUserId = _claimsService.GetCurrentUserId();
                if (string.IsNullOrEmpty(currentUserId))
                {
                    return Result<GetDesignByIdResponse>.Unauthorized("User session expired.");
                }

                var result = await _context.Designs
                    .Where(d => d.Id == request.designId && d.UserId == currentUserId)
                    .ProjectTo<GetDesignByIdDTO>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync(cancellationToken);

                if (result == null)
                {
                    return Result<GetDesignByIdResponse>.NotFound($"Design with ID {request.designId} was not found.");
                }

                var response = new GetDesignByIdResponse
                {
                    getDesignByIdDTO = result
                };

                return Result<GetDesignByIdResponse>.Success(response, "Design retrieved successfully.");
            }
            catch (Exception ex)
            {
                return Result<GetDesignByIdResponse>.Failure(
                    message: "An unexpected error occurred during getting design.",
                    statusCode: 500,
                    errors: new List<string> { ex.Message }
                    );
            }

            

        }
    }
}
