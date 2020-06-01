using SpecificationPattern.Application.DTOs;
using SpecificationPattern.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpecificationPattern.Application.ApplicationServices
{
    public interface IShowMenuItemService
    {
        Task<IEnumerable<MenuItemDto>> GetAllMenuItems();

        Task<MenuItemDto> GetMenuItemById(Guid id);

        Task<IEnumerable<MenuItemDto>> FilterByMealTypeAndExcludeAllergens(string mealType, IEnumerable<AllergenType> allergens);
    }
}
