using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Application.Features.Designs.Commands.AddOrignalImage
{
    public class AddDesignWithOurCeramicImageValidator : AbstractValidator<AddDesignWithOurCeramicImageCommand>
    {
        public AddDesignWithOurCeramicImageValidator()
        {

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".jfif" };

            RuleFor(x => x.roomimageName)
                .Must(fileName =>
                {
                    var extension = Path.GetExtension(fileName).ToLower();
                    return allowedExtensions.Contains(extension);
                })
                .WithMessage("Image file must be with extention of (jpg, jpeg, png, webp, jfif)");

            //RuleFor(x => x.ceramicImageName)
            //    .Must(fileName =>
            //    {
            //        var extension = Path.GetExtension(fileName).ToLower();
            //        return allowedExtensions.Contains(extension);
            //    })
            //    .WithMessage("Image file must be with extention of (jpg, jpeg, png, webp, jfif)");

            RuleFor(x => x.roomBytes)
                .NotEmpty().WithMessage("Room image is required for us.")
                .Must(x => x.Length > 0).WithMessage("Room image file is empty.");

            RuleFor(x => x.roomBytes)
                .Must(x => x.Length <= 10 * 1024 * 1024)
                .WithMessage("Room image must be less than 10MB.");

            RuleFor(x => x.ceramicId)
                .NotEmpty().WithMessage("ceramicId code is required");

            RuleFor(x => x.designType)
                .IsInEnum().WithMessage("Invalid Design Type selection.");

            RuleFor(x => x.roomimageName).NotEmpty().MaximumLength(200);









            // Check Not Null
            //RuleFor(c => c.stream)
            //    .NotNull()
            //    .WithMessage("You Should Select Image To Upload");


            // Check Not Empty
            //RuleFor(c => c.stream)
                //.Must(StreamMustHaveData)
                //.When(c => c.stream != null)
                //.WithMessage("The Image File Is Empty ");

            // Check On File Name Exists Or Not
            //RuleFor(c => c.imageName)
            //    .NotEmpty()
            //    .WithMessage("Image Should Have Name");

            RuleFor(c => c.ceramicId)
                .NotEmpty()
                .WithMessage("You Should Select Ceramic Image");
            RuleFor(c => c.designType)
                .IsInEnum()
                .WithMessage("Must Be in The Enum");
        }
    }
}

