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
        //public IFormFile imageFile { get; set; }
        public Stream stream {  get; set; }
        public string imageName {  get; set; }

        public string ceramicId { get; set; }
        public DesignType designType { get; set; }
    }
}
