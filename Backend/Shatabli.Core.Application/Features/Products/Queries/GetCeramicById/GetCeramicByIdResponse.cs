using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Products.Queries.GetCeramicById
{
    public class GetCeramicByIdResponse
    {
        public CeramicDTO ceramicProduct { get; set; }
    }
    public class CeramicDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
