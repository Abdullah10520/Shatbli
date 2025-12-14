using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicImage
{
    public class AddDesignWithUserCeramicImageCommand : IRequest<AddDesignWithUserCeramicImageResponse>
    {
        public byte[] roomBytes { get; set; }
        //public Stream roomstream { get; set; }
        public byte[] ceramicOrPaintBytes { get; set; }
        //public Stream ceramicOrPaintStream { get; set; }
        public string roomimageName { get; set; }
        public string ceramicOrPaintimageName { get; set; }
        public DesignType designType { get; set; }
    }
}
