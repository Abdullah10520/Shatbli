using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Shatabli.Core.Application.Features.Designs.Queries.GetDesignById
{
    public class GetDesignByIdQuery : IRequest<GetDesignByIdResponse>
    {
        public string designId { get; set; }
    }
}
