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
        public string productId { get; set; }
        public string productName { get; set; }
        public string productImageUrl { get; set; }
    }
}
