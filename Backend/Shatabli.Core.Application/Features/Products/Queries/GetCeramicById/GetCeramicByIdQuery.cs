using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Shatabli.Core.Application.Features.Products.Queries.GetCeramicById
{
    public class GetCeramicByIdQuery : IRequest<GetCeramicByIdResponse>
    {
        public string ceramicId { get; set; }
    }
}
