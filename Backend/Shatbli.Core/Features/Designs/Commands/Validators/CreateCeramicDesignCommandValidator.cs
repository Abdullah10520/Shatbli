using FluentValidation;
using Shatbli.Core.Features.Designs.Commands.Models;

namespace Shatbli.Core.Features.Designs.Commands.Validators
{
    public class CreateCeramicDesignCommandValidator : AbstractValidator<CreateCeramicDesignCommand>
    {
        public CreateCeramicDesignCommandValidator()
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

            RuleFor(x => x)
                .Must(cmd => cmd.CeramicImage != null || cmd.CeramicProductId.HasValue)
                .WithMessage("Either upload a ceramic image or select a ceramic product");

            When(x => x.CeramicImage != null, () =>
            {
                RuleFor(x => x.CeramicImage)
                    .Must(file => file!.Length > 0)
                    .WithMessage("Ceramic image is required")
                    .Must(file => file == null || file.Length <= 10 * 1024 * 1024)
                    .WithMessage("Ceramic image size cannot exceed 10 MB")
                    .Must(file => file == null || IsValidImageExtension(file.FileName))
                    .WithMessage("Ceramic image must be .jpg, .jpeg, .png, or .webp");
            });

            When(x => x.CeramicProductId.HasValue, () =>
            {
                RuleFor(x => x.CeramicProductId)
                    .GreaterThan(0).WithMessage("Invalid Ceramic Product ID");
            });

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