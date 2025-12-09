using FluentValidation;
using Shatbli.Core.Features.Users.Queries.Models;

namespace Shatbli.Core.Features.Users.Queries.Validators
{
    public class GetUserProfileQueryValidator : AbstractValidator<GetUserProfileQuery>
    {
        public GetUserProfileQueryValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Invalid User ID");
        }
    }
}