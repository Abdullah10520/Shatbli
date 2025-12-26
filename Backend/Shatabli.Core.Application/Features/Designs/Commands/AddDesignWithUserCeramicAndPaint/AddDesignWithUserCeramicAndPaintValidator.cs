using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddDesignWithUserCeramicAndPaint
{
    public class AddDesignWithUserCeramicAndPaintValidator : AbstractValidator<AddDesignWithUserCeramicAndPaintCommand>
    {
        public AddDesignWithUserCeramicAndPaintValidator()
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".jfif" };

            RuleFor(x => x.roomimageName)
                .Must(fileName =>
                {
                    var extension = Path.GetExtension(fileName).ToLower();
                    return allowedExtensions.Contains(extension);
                })
                .WithMessage("Image file must be with extention of (jpg, jpeg, png, webp, jfif)");

            RuleFor(x => x.ceramicImageName)
                .Must(fileName =>
                {
                    var extension = Path.GetExtension(fileName).ToLower();
                    return allowedExtensions.Contains(extension);
                })
                .WithMessage("Image file must be with extention of (jpg, jpeg, png, webp, jfif)");

            RuleFor(x => x.roomBytes)
                .NotEmpty().WithMessage("Room image is required for us.")
                .Must(x => x.Length > 0).WithMessage("Room image file is empty.");

            RuleFor(x => x.ceramicBytes)
                .NotEmpty().WithMessage("Ceramic image is required.")
                .Must(x => x.Length > 0).WithMessage("Ceramic image file is empty.");

            RuleFor(x => x.roomBytes)
                .Must(x => x.Length <= 10 * 1024 * 1024)
                .WithMessage("Room image must be less than 10MB.");

            RuleFor(x => x.colorCode)
                .NotEmpty().WithMessage("Color code is required")
                .Must(x=>x.Length <= 6).WithMessage("Color code in hex and 6 or less char");

            RuleFor(x => x.roomimageName).NotEmpty().MaximumLength(200);
        }
    }
}
