using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage
{
    public class AddDesignWithOurCeramicImageCommand : IRequest<AddDesignWithOurCeramicImageResponse>
    {
        public byte[] stream { get; set; } = null!;
        public string imageName { get; set; } = string.Empty;
        public string ceramicId { get; set; } = string.Empty;
        public DesignType designType { get; set; }
    }
}
