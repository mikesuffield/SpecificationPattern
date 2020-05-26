using SpecificationPattern.Application.ViewModels;
using SpecificationPattern.Core.Models;
using SpecificationPattern.Shared.Enums;
using SpecificationPattern.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecificationPattern.Application.DTOs
{
    public class MenuItemDto
    {
        public MenuItemDto(MenuItem menuItem)
        {
            Id = menuItem.Id;
            Name = menuItem.Name;
            Price = menuItem.Price;
            MealType = menuItem.MealType;
            Allergens = menuItem.Allergens.Select(allergen => new AllergenDto
            {
                Id = allergen.Id,
                Name = allergen.Name,
            });
        }

        public MenuItemDto(CreateMenuItemViewModel createMenuItemViewModel)
        {
            Id = Guid.NewGuid();
            Name = createMenuItemViewModel.Name;
            Price = createMenuItemViewModel.Price;
            MealType = createMenuItemViewModel.MealType.ToEnum<MealType>();
            Allergens = createMenuItemViewModel.Allergens.Select(allergen => new AllergenDto
            {
                Id = Guid.NewGuid(),
                Name = allergen,
            });
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public MealType MealType { get; set; }

        public IEnumerable<AllergenDto> Allergens { get; set; }
    }
}
