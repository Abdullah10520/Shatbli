using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Core.Features.Designs.Commands.Models;
using Shatbli.Service.Interfaces;

namespace Shatbli.Core.Features.Designs.Commands.Handlers
{
    public class ProcessDesignHandler : ResponseHandler, IRequestHandler<ProcessDesignCommand, Response<bool>>
    {
        private readonly IDesignService _designService;

        public ProcessDesignHandler(IDesignService designService)
        {
            _designService = designService;
        }

        public async Task<Response<bool>> Handle(ProcessDesignCommand request, CancellationToken cancellationToken)
        {
            var design = await _designService.GetDesignByIdAsync(request.DesignId, request.UserId);
            if (design == null)
                return NotFound<bool>("Design not found");

            var success = await _designService.ProcessDesignWithAIAsync(request.DesignId);

            return success
                ? Success(true, "Design processed successfully")
                : BadRequest<bool>("Failed to process design");
        }
    }
}