using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Products.Queries.GetAllPaints
{

        public class GetAllPaintsResponse
        {
            public List<PaintsDTO> paintsList { get; set; }
        }
        public class PaintsDTO
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string ImageUrl { get; set; }
            public bool IsActive { get; set; }
        }
    }

