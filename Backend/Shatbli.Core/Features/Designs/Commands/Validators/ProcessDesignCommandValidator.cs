using FluentValidation;
using Shatbli.Core.Features.Designs.Commands.Models;

namespace Shatbli.Core.Features.Designs.Commands.Validators
{
    public class ProcessDesignCommandValidator : AbstractValidator<ProcessDesignCommand>
    {
        public ProcessDesignCommandValidator()
        {
            RuleFor(x => x.DesignId)
                .GreaterThan(0).WithMessage("Invalid Design ID");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Invalid User ID");
        }
    }
}