using Microsoft.AspNetCore.Mvc;

namespace SpecificationPattern.Application.ViewModels
{
    public class FilterViewModel
    {
        [FromQuery(Name = "mealType")]
        public string MealType { get; set; }

        [FromQuery(Name = "allergens")]
        public string Allergens { get; set; }
    }
}
