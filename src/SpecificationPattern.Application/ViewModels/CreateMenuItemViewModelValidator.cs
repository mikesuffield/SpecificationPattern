using FluentValidation;
using SpecificationPattern.Shared.Enums;

namespace SpecificationPattern.Application.ViewModels
{
    public class CreateMenuItemViewModelValidator : AbstractValidator<CreateMenuItemViewModel>
    {
        public CreateMenuItemViewModelValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Price)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.MealType)
                .NotNull()
                .NotEmpty()
                .IsEnumName(typeof(MealType)).WithMessage("Invalid MealType");

            RuleForEach(x => x.Allergens)
                .MinimumLength(1)
                .IsEnumName(typeof(AllergenType)).WithMessage("Invalid Allergen");
        }
    }
}
