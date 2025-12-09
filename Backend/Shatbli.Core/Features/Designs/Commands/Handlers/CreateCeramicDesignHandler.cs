using AutoMapper;
using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Core.Features.Designs.Commands.Models;
using Shatbli.Data.DTOs.DesignDtos;
using Shatbli.Service.Interfaces;

namespace Shatbli.Core.Features.Designs.Commands.Handlers
{
    public class CreateCeramicDesignHandler : ResponseHandler, IRequestHandler<CreateCeramicDesignCommand, Response<DesignResponseDto>>
    {
        private readonly IDesignService _designService;
        private readonly IMapper _mapper;

        public CreateCeramicDesignHandler(IDesignService designService, IMapper mapper)
        {
            _designService = designService;
            _mapper = mapper;
        }

        public async Task<Response<DesignResponseDto>> Handle(CreateCeramicDesignCommand request, CancellationToken cancellationToken)
        {
            // Map Command to DTO
            var dto = _mapper.Map<CreateCeramicDesignDto>(request);

            // Create design
            var design = await _designService.CreateCeramicDesignAsync(request.UserId, dto);

            // Map Entity to Response DTO
            var response = _mapper.Map<DesignResponseDto>(design);

            return Success(response);
        }
    }
}