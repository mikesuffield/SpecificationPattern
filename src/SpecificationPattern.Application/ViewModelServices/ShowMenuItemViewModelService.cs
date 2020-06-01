using SpecificationPattern.Application.ApplicationServices;
using SpecificationPattern.Application.DTOs;
using SpecificationPattern.Application.ViewModels;
using SpecificationPattern.Shared.Enums;
using SpecificationPattern.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecificationPattern.Application.ViewModelServices
{
    public class ShowMenuItemViewModelService : IShowMenuItemViewModelService
    {
        private readonly IShowMenuItemService _showMenuItemService;

        public ShowMenuItemViewModelService(IShowMenuItemService showMenuItemService)
        {
            _showMenuItemService = showMenuItemService;
        }

        public async Task<ShowMenuItemViewModel> GetById(Guid id)
        {
            var menuItemDto = await _showMenuItemService.GetMenuItemById(id);

            return new ShowMenuItemViewModel(menuItemDto);
        }

        public async Task<IEnumerable<ShowMenuItemViewModel>> GetMenuItems(string mealType, string allergens)
        {
            IEnumerable<MenuItemDto> menuItemDtos;

            if (string.IsNullOrEmpty(mealType) && string.IsNullOrEmpty(allergens))
            {
                menuItemDtos = await _showMenuItemService.GetAllMenuItems();
            }
            else
            {
                var allergenEnums = allergens?.Split(",").Select(x => x.ToEnum<AllergenType>());
                menuItemDtos = await _showMenuItemService.FilterByMealTypeAndExcludeAllergens(mealType, allergenEnums);
            }

            return menuItemDtos.Select(menuItemDto => new ShowMenuItemViewModel(menuItemDto));
        }
    }
}
