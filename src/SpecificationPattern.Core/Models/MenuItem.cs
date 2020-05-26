using SpecificationPattern.Shared.Enums;
using System.Collections.Generic;

namespace SpecificationPattern.Core.Models
{
    public class MenuItem : EntityBase
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public MealType MealType { get; set; }

        public IEnumerable<Allergen> Allergens { get; set; }
    }
}
