using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage
{
    public class AddDesignWithOurCeramicImageResponse
    {
        //public Stream GeneratedImage {  get; set; }
        public string GeneratedImageUrl { get; set; }
        public string designId { get; set; }
    }
}
