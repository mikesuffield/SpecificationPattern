using System.Collections.Generic;

namespace SpecificationPattern.Application.ViewModels
{
    public class CreateMenuItemViewModel
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public string MealType { get; set; }

        public IEnumerable<string> Allergens { get; set; }
    }
}
