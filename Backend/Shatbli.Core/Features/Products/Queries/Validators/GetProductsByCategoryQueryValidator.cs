using FluentValidation;
using Shatbli.Core.Features.Products.Queries.Models;

namespace Shatbli.Core.Features.Products.Queries.Validators
{
    public class GetProductsByCategoryQueryValidator : AbstractValidator<GetProductsByCategoryQuery>
    {
        public GetProductsByCategoryQueryValidator()
        {
            RuleFor(x => x.Category)
                .IsInEnum().WithMessage("Invalid product category");
        }
    }
}