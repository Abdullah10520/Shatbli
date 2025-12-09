using FluentValidation;
using Shatbli.Core.Features.Designs.Queries.Models;

namespace Shatbli.Core.Features.Designs.Queries.Validators
{
    public class GetDesignByIdQueryValidator : AbstractValidator<GetDesignByIdQuery>
    {
        public GetDesignByIdQueryValidator()
        {
            RuleFor(x => x.DesignId)
                .GreaterThan(0).WithMessage("Invalid Design ID");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Invalid User ID");
        }
    }
}