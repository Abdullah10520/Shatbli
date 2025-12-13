using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage
{
    public class AddOrignalImageCommand : IRequest<AddOrignalImageCommandResponse>
    {
        //public IFormFile imageFile { get; set; }
        public Stream stream {  get; set; }
        public string ImageName {  get; set; }

        public string CeramicId { get; set; }
    }
}
