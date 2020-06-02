using SpecificationPattern.Shared.Enums;
using System.Collections.Generic;

namespace SpecificationPattern.Application.DTOs
{
    public class FilterDto
    {
        public MealType MealType { get; set; }

        public IEnumerable<AllergenType> Allergens { get; set; }
    }
}
