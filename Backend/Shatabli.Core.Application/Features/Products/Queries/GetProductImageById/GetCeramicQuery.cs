using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Shatabli.Core.Application.Features.Products.Queries.GetProductImageById
{
    public class GetCeramicQuery : IRequest<GetCeramicResponse>
    {
        public string CeramicId { get; set; }
    }
}
