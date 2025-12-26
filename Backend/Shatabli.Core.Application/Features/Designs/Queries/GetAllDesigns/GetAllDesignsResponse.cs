using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Designs.Queries.GetAllDesigns
{
    public class GetAllDesignsResponse
    {
        public List<GetAllDesignDTO> designsList { get; set; }

    }
    public class GetAllDesignDTO
    {
        public string designId { get; set; }
        public string generatedImageUrl { get; set; }
        public DateTime? completedAt { get; set; }

    }
}
