using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Shatabli.Core.Application.Features.Designs.Commands.SoftDeleteDesign
{
    public class DesignSoftDeleteCommand : IRequest<DesignSoftDeleteResponse>
    {
        public string designId { get; set; }
    }
}
