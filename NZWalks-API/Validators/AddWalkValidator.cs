using FluentValidation;
using NZWalks_API.Models.DTOs;

namespace NZWalks_API.Validators
{
    public class AddWalkValidator:AbstractValidator<AddWalkRequest>
    {
        public AddWalkValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThanOrEqualTo(0);
        }
    }
}
