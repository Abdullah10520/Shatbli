using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Data.DTOs.DesignDtos;

namespace Shatbli.Core.Features.Designs.Queries.Models
{
    public class GetDesignByIdQuery : IRequest<Response<DesignResponseDto>>
    {
        public int DesignId { get; set; }
        public int UserId { get; set; }
    }
}