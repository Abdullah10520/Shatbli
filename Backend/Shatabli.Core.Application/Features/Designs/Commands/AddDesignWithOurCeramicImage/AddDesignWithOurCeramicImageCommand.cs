using MediatR;
using Microsoft.AspNetCore.Http;
using Shatabli.Core.Domain.Common;
using Shatabli.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage
{
    public class AddDesignWithOurCeramicImageCommand : IRequest<Result<AddDesignWithOurCeramicImageResponse>>
    {
        public byte[] roomBytes { get; set; } = null!;
        public string roomimageName { get; set; } = string.Empty;
        public string ceramicId { get; set; } = string.Empty;
        public DesignType designType { get; set; }
    }
}
