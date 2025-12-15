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
            // Check Not Null
            RuleFor(c => c.stream)
                .NotNull()
                .WithMessage("You Should Select Image To Upload");


            // Check Not Empty
            //RuleFor(c => c.stream)
                //.Must(StreamMustHaveData)
                //.When(c => c.stream != null)
                //.WithMessage("The Image File Is Empty ");

            // Check On File Name Exists Or Not
            RuleFor(c => c.imageName)
                .NotEmpty()
                .WithMessage("Image Should Have Name");

            RuleFor(c => c.ceramicId)
                .NotEmpty()
                .WithMessage("You Should Select Ceramic Image");
            RuleFor(c => c.designType)
                .IsInEnum()
                .WithMessage("Must Be in The Enum");
        }

        private bool StreamMustHaveData(Stream stream)
        {
            if (stream == null)
            {
                return false;
            }

            //Seekable => Searchable (Can Move Right And Left On The data)
            if (stream.CanSeek)
            {
                return stream.Length > 0;
            }

            return true;
        }
    }
}

