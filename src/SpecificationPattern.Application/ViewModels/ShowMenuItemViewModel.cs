using SpecificationPattern.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecificationPattern.Application.ViewModels
{
    public class ShowMenuItemViewModel
    {
        public ShowMenuItemViewModel(MenuItemDto menuItemDto)
        {
            Id = menuItemDto.Id;
            Name = menuItemDto.Name;
            Price = "£" + menuItemDto.Price.ToString("F");
            MealType = menuItemDto.MealType.ToString();
            Allergens = menuItemDto.Allergens.Select(allergen => allergen.Name);
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Price { get; set; }

        public string MealType { get; set; }

        public IEnumerable<string> Allergens { get; set; }
    }
}
