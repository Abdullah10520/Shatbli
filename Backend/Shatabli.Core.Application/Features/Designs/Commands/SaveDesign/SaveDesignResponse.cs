using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Designs.Commands.SaveDesign
{
    public class SaveDesignResponse
    {
        public string designId { get; set; }
        public string generatedImageUrl {  get; set; }
    }
}
