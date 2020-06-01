using SpecificationPattern.Application.DTOs;
using SpecificationPattern.Core.Interfaces;
using SpecificationPattern.Core.Models;
using SpecificationPattern.Core.Specifications;
using SpecificationPattern.Shared.Enums;
using SpecificationPattern.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecificationPattern.Application.ApplicationServices
{
    public class ShowMenuItemService : IShowMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public ShowMenuItemService(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public async Task<IEnumerable<MenuItemDto>> GetAllMenuItems()
        {
            var menuItems = await _menuItemRepository.All();

            return menuItems.Select(menuItem => new MenuItemDto(menuItem));
        }

        public async Task<MenuItemDto> GetMenuItemById(Guid id)
        {
            var menuItem = await _menuItemRepository.Find(id);

            if (menuItem == null)
            {
                throw new Exception($"MenuItem with ID {id} could not be found");
            }

            return new MenuItemDto(menuItem);
        }

        public async Task<IEnumerable<MenuItemDto>> FilterByMealTypeAndExcludeAllergens(string mealType, IEnumerable<AllergenType> allergens)
        {
            ISpecification<MenuItem> specification;

            if (!string.IsNullOrEmpty(mealType) && allergens != null && allergens.Any())
            {
                var mealTypeEnum = mealType.ToEnum<MealType>();
                var mealTypeSpec = new MenuItemForMealTypeSpecification(mealTypeEnum);
                var excludeAllergensSpec = new MenuItemForAllergensSpecification(allergens).Not();

                specification = mealTypeSpec.And(excludeAllergensSpec);
            }
            else if (allergens != null && allergens.Any())
            {
                specification = new MenuItemForAllergensSpecification(allergens).Not();
            }
            else
            {
                var mealTypeEnum = mealType.ToEnum<MealType>();
                specification = new MenuItemForMealTypeSpecification(mealTypeEnum);
            }

            var menuItems = await _menuItemRepository.All(specification);

            return menuItems.Select(menuItem => new MenuItemDto(menuItem));
        }
    }
}
