using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Shatabli.Core.Application.Features.Designs.Commands.SoftDeleteDesign
{
    public class DesignSoftDeleteValidator : AbstractValidator<DesignSoftDeleteCommand>
    {
    }
}
