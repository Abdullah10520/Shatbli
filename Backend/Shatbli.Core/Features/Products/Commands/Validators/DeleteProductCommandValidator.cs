using FluentValidation;
using Shatbli.Core.Features.Products.Commands.Models;

namespace Shatbli.Core.Features.Products.Commands.Validators
{
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Invalid Product ID");
        }
    }
}