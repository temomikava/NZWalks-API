using FluentValidation;
using NZWalks_API.Models.DTOs;

namespace NZWalks_API.Validators
{
    public class UpdateWalkDifficultyValidator:AbstractValidator<UpdateWalkDiffucultyRequest>
    {
        public UpdateWalkDifficultyValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
