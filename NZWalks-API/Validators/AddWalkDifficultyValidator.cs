using FluentValidation;
using NZWalks_API.Models.DTOs;

namespace NZWalks_API.Validators
{
    public class AddWalkDifficultyValidator:AbstractValidator<AddWalkdiffucultyRequest>
    {
        public AddWalkDifficultyValidator()
        {
            RuleFor(x=>x).NotNull();
            RuleFor(x=>x.Code).NotEmpty();
        }
    }
}
