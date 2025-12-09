using AutoMapper;
using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Core.Features.Designs.Queries.Models;
using Shatbli.Data.DTOs.DesignDtos;
using Shatbli.Service.Interfaces;

namespace Shatbli.Core.Features.Designs.Queries.Handlers
{
    public class GetDesignByIdHandler : ResponseHandler, IRequestHandler<GetDesignByIdQuery, Response<DesignResponseDto>>
    {
        private readonly IDesignService _designService;
        private readonly IMapper _mapper;

        public GetDesignByIdHandler(IDesignService designService, IMapper mapper)
        {
            _designService = designService;
            _mapper = mapper;
        }

        public async Task<Response<DesignResponseDto>> Handle(GetDesignByIdQuery request, CancellationToken cancellationToken)
        {
            var design = await _designService.GetDesignByIdAsync(request.DesignId, request.UserId);

            if (design == null)
                return NotFound<DesignResponseDto>("Design not found");

            var response = _mapper.Map<DesignResponseDto>(design);

            return Success(response);
        }
    }
}