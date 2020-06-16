using SpecificationPattern.Shared.Enums;
using System;
using System.Collections.Generic;

namespace SpecificationPattern.Application.DTOs
{
    public class MenuItemDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public MealType MealType { get; set; }

        public IEnumerable<AllergenDto> Allergens { get; set; }
    }
}
