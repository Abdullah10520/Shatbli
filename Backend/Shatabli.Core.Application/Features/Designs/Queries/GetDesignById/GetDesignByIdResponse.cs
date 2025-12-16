using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Designs.Queries.GetDesignById
{
    public class GetDesignByIdResponse
    {
        public GetDesignByIdDTO getDesignByIdDTO { get; set; }
    }

    public class GetDesignByIdDTO
    {
        public string Id { get; set; }
        public string OriginalImageUrl { get; set; }
        public string? GeneratedImageUrl { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
