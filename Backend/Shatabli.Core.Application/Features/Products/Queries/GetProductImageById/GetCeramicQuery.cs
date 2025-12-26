using MediatR;
using Shatabli.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Products.Queries.GetProductImageById
{
    public class GetCeramicQuery : IRequest<Result<GetCeramicResponse>>
    {
        public string CeramicId { get; set; }
    }
}
