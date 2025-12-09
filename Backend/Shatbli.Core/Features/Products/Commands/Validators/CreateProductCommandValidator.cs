using FluentValidation;
using Shatbli.Core.Features.Products.Commands.Models;

namespace Shatbli.Core.Features.Products.Commands.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters");

            RuleFor(x => x.Image)
                .NotNull().WithMessage("Product image is required")
                .Must(file => file != null && file.Length > 0)
                .WithMessage("Product image is required")
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
                .WithMessage("Product image size cannot exceed 5 MB")
                .Must(file => file == null || IsValidImageExtension(file.FileName))
                .WithMessage("Product image must be .jpg, .jpeg, .png, or .webp");

            RuleFor(x => x.Vendor)
                .NotEmpty().WithMessage("Vendor name is required")
                .MaximumLength(100).WithMessage("Vendor name cannot exceed 100 characters");

            RuleFor(x => x.Category)
                .IsInEnum().WithMessage("Invalid product category");

            When(x => !string.IsNullOrEmpty(x.Texture), () =>
            {
                RuleFor(x => x.Texture)
                    .MaximumLength(100).WithMessage("Texture cannot exceed 100 characters");
            });

            When(x => !string.IsNullOrEmpty(x.Size), () =>
            {
                RuleFor(x => x.Size)
                    .MaximumLength(50).WithMessage("Size cannot exceed 50 characters");
            });

            When(x => x.PricePerUnit.HasValue, () =>
            {
                RuleFor(x => x.PricePerUnit)
                    .GreaterThan(0).WithMessage("Price must be greater than zero");
            });

            When(x => !string.IsNullOrEmpty(x.ColorCode), () =>
            {
                RuleFor(x => x.ColorCode)
                    .MaximumLength(20).WithMessage("Color code cannot exceed 20 characters")
                    .Matches(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
                    .WithMessage("Color code must be a valid hex color (e.g., #FFFFFF)");
            });

            When(x => !string.IsNullOrEmpty(x.Brand), () =>
            {
                RuleFor(x => x.Brand)
                    .MaximumLength(100).WithMessage("Brand cannot exceed 100 characters");
            });
        }

        private bool IsValidImageExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".webp";
        }
    }
}