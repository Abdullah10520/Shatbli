using MediatR;
using Shatabli.Core.Domain.Common;
using Shatabli.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicImage
{
    public class AddDesignWithUserCeramicImageCommand : IRequest<Result<AddDesignWithUserCeramicImageResponse>>
    {
        public byte[] roomBytes { get; set; }
        public byte[] ceramicBytes { get; set; }
        public string roomimageName { get; set; }
        public string ceramicImageName { get; set; }
        public DesignType designType { get; set; }
    }
}
