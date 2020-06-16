using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace SpecificationPattern.Application.ViewModels
{
    public class FilterViewModel
    {
        [FromQuery(Name = "mealType")]
        public string MealType { get; set; }

        [FromQuery(Name = "allergens")]
        public IEnumerable<string> Allergens { get; set; }
    }
}
