using FluentValidation;
using NZWalks_API.Models.DTOs;

namespace NZWalks_API.Validators
{
    public class UpdateWalkRequestValidator:AbstractValidator<UpdateWalkRequest>
    {

        public UpdateWalkRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThanOrEqualTo(0);
        }

    }
}
