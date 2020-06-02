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

        public async Task<IEnumerable<ShowMenuItemViewModel>> GetMenuItems(FilterViewModel filters)
        {
            IEnumerable<MenuItemDto> menuItemDtos;

            if (string.IsNullOrEmpty(filters.Allergens) && string.IsNullOrEmpty(filters.MealType))
            {
                menuItemDtos = await _showMenuItemService.GetAllMenuItems();
            }
            else
            {
                var filterDto = new FilterDto
                {
                    MealType = string.IsNullOrEmpty(filters.MealType) ? MealType.Unknown : filters.MealType.ToEnum<MealType>(),
                    Allergens = filters.Allergens?.Split(",").Select(x => x.ToEnum<AllergenType>()),
                };

                menuItemDtos = await _showMenuItemService.GetAllMenuItemsWithFilters(filterDto);
            }

            return menuItemDtos.Select(menuItemDto => new ShowMenuItemViewModel(menuItemDto));
        }
    }
}
