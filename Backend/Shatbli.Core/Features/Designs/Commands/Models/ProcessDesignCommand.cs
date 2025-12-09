using MediatR;
using Shatbli.Core.Bases;

namespace Shatbli.Core.Features.Designs.Commands.Models
{
    public class ProcessDesignCommand : IRequest<Response<bool>>
    {
        public int DesignId { get; set; }
        public int UserId { get; set; }
    }
}