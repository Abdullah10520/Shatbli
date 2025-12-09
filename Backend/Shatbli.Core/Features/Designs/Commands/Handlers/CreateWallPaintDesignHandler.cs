using AutoMapper;
using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Core.Features.Designs.Commands.Models;
using Shatbli.Data.DTOs.DesignDtos;
using Shatbli.Service.Interfaces;

namespace Shatbli.Core.Features.Designs.Commands.Handlers
{
    public class CreateWallPaintDesignHandler : ResponseHandler, IRequestHandler<CreateWallPaintDesignCommand, Response<DesignResponseDto>>
    {
        private readonly IDesignService _designService;
        private readonly IMapper _mapper;

        public CreateWallPaintDesignHandler(IDesignService designService, IMapper mapper)
        {
            _designService = designService;
            _mapper = mapper;
        }

        public async Task<Response<DesignResponseDto>> Handle(CreateWallPaintDesignCommand request, CancellationToken cancellationToken)
        {
            // Map Command to DTO
            var dto = _mapper.Map<CreateWallPaintDesignDto>(request);

            // Create design
            var design = await _designService.CreateWallPaintDesignAsync(request.UserId, dto);

            // Map Entity to Response DTO
            var response = _mapper.Map<DesignResponseDto>(design);

            return Success(response);
        }
    }
}