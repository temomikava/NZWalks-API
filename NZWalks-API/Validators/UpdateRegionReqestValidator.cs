using FluentValidation;
using NZWalks_API.Models.DTOs;

namespace NZWalks_API.Validators
{
    public class UpdateRegionReqestValidator:AbstractValidator<UpdateRegionRequest>
    {
        public UpdateRegionReqestValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Area).GreaterThan(0);
            RuleFor(x => x.Population).GreaterThanOrEqualTo(0);
        }
    }
}
