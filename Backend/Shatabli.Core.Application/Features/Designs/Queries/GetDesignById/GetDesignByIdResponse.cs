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
        public string designId { get; set; }
        public string? generatedImageUrl { get; set; }
        public DateTime? completedAt { get; set; }
    }
}
