using AutoMapper;
using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Core.Features.Designs.Queries.Models;
using Shatbli.Data.DTOs.DesignDtos;
using Shatbli.Service.Interfaces;

namespace Shatbli.Core.Features.Designs.Queries.Handlers
{
    public class GetUserDesignsHandler : ResponseHandler, IRequestHandler<GetUserDesignsQuery, Response<List<DesignListDto>>>
    {
        private readonly IDesignService _designService;
        private readonly IMapper _mapper;

        public GetUserDesignsHandler(IDesignService designService, IMapper mapper)
        {
            _designService = designService;
            _mapper = mapper;
        }

        public async Task<Response<List<DesignListDto>>> Handle(GetUserDesignsQuery request, CancellationToken cancellationToken)
        {
            var designs = await _designService.GetUserDesignsAsync(request.UserId);

            var response = _mapper.Map<List<DesignListDto>>(designs);

            return Success(response);
        }
    }
}