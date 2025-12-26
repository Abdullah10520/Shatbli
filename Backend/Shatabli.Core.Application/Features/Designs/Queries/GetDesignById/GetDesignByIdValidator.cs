using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Shatabli.Core.Application.Features.Designs.Queries.GetDesignById
{
    public class GetDesignByIdValidator : AbstractValidator<GetDesignByIdQuery>
    {
        public GetDesignByIdValidator() 
        {
            RuleFor(q=>q.designId)
                .NotEmpty()
                .WithMessage("Error Design Id is required");
        }
    }
}
