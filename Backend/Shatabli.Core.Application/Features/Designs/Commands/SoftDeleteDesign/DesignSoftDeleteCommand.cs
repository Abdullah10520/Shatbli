using MediatR;
using Shatabli.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Designs.Commands.SoftDeleteDesign
{
    public class DesignSoftDeleteCommand : IRequest<Result<DesignSoftDeleteResponse>>
    {
        public string designId { get; set; }
    }
}
