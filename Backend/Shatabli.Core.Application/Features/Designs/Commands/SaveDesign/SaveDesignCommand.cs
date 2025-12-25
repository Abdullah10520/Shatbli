using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatabli.Core.Application.Features.Designs.Commands.SaveDesign
{
    public class SaveDesignCommand : IRequest<SaveDesignResponse>
    {
        //public string designImagePath { get; set; }
        public string designId { get; set; }
    }
}
