using System;
using System.Collections.Generic;

namespace SpecificationPattern.Application.ViewModels
{
    public class ShowMenuItemViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }

        public string MealType { get; set; }

        public IEnumerable<string> Allergens { get; set; }
    }
}
