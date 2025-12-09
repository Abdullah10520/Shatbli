using FluentValidation;
using Shatbli.Core.Features.Designs.Commands.Models;

namespace Shatbli.Core.Features.Designs.Commands.Validators
{
    public class CreateWallPaintDesignCommandValidator : AbstractValidator<CreateWallPaintDesignCommand>
    {
        public CreateWallPaintDesignCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Invalid User ID");

            RuleFor(x => x.RoomImage)
                .NotNull().WithMessage("Room image is required")
                .Must(file => file != null && file.Length > 0)
                .WithMessage("Room image is required")
                .Must(file => file == null || file.Length <= 10 * 1024 * 1024)
                .WithMessage("Room image size cannot exceed 10 MB")
                .Must(file => file == null || IsValidImageExtension(file.FileName))
                .WithMessage("Room image must be .jpg, .jpeg, .png, or .webp");

            RuleFor(x => x.WallColor)
                .NotEmpty().WithMessage("Wall color is required")
                .MaximumLength(20).WithMessage("Wall color cannot exceed 20 characters")
                .Matches(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
                .WithMessage("Wall color must be a valid hex color (e.g., #FFFFFF)");

            When(x => !string.IsNullOrEmpty(x.CustomPrompt), () =>
            {
                RuleFor(x => x.CustomPrompt)
                    .MaximumLength(2000).WithMessage("Custom prompt cannot exceed 2000 characters");
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