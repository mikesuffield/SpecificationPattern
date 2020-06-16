using FluentValidation;
using SpecificationPattern.Shared.Enums;

namespace SpecificationPattern.Application.ViewModels
{
    public class FilterViewModelValidator : AbstractValidator<FilterViewModel>
    {
        public FilterViewModelValidator()
        {
            RuleFor(x => x.MealType)
                .MinimumLength(1)
                .IsEnumName(typeof(MealType)).WithMessage("Invalid MealType");

            RuleForEach(x => x.Allergens)
                .MinimumLength(1)
                .IsEnumName(typeof(AllergenType)).WithMessage("Invalid Allergen");
        }
    }
}
