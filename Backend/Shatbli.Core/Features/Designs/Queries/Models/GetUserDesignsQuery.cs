using MediatR;
using Shatbli.Core.Bases;
using Shatbli.Data.DTOs.DesignDtos;

namespace Shatbli.Core.Features.Designs.Queries.Models
{
    public class GetUserDesignsQuery : IRequest<Response<List<DesignListDto>>>
    {
        public int UserId { get; set; }
    }
}