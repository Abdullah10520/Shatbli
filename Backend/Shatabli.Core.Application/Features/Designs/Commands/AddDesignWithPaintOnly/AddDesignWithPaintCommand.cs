using MediatR;
using Shatabli.Core.Domain.Common;
using Shatabli.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithPaintOnly
{
    public class AddDesignWithPaintCommand : IRequest<Result<AddDesignWithPaintResponse>>
    {
        public byte[] roomBytes { get; set; }
        public string roomimageName { get; set; }
        public string colorCode { get; set; }
    }
}
