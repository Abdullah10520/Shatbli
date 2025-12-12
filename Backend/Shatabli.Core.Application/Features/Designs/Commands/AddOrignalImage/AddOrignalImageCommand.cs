using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage
{
    public class AddOrignalImageCommand : IRequest<AddOrignalImageCommandResponse>
    {
        public Stream stream {  get; set; }
        public string ImageName {  get; set; }
    }
}
