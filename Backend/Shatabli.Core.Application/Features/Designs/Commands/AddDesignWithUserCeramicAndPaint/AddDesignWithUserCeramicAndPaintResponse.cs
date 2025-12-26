using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicAndPaint
{
    public class AddDesignWithUserCeramicAndPaintResponse
    {
        //public Stream GeneratedImage { get; set; }
        public string generatedImagePath { get; set; }
        public string designId  { get; set; } 
    }
}
