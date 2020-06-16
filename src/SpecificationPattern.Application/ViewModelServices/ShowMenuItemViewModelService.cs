using AutoMapper;
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
        private readonly IMapper _mapper;

        public ShowMenuItemViewModelService(IShowMenuItemService showMenuItemService, IMapper mapper)
        {
            _showMenuItemService = showMenuItemService;
            _mapper = mapper;
        }

        public async Task<ShowMenuItemViewModel> GetById(Guid id)
        {
            var menuItemDto = await _showMenuItemService.GetMenuItemById(id);

            var showMenuItemVeiwModel = _mapper.Map<ShowMenuItemViewModel>(menuItemDto);
            return showMenuItemVeiwModel;
        }

        public async Task<IEnumerable<ShowMenuItemViewModel>> GetMenuItems(FilterViewModel filters)
        {
            IEnumerable<MenuItemDto> menuItemDtos;

            if ((filters.Allergens == null || !filters.Allergens.Any()) && string.IsNullOrEmpty(filters.MealType))
            {
                menuItemDtos = await _showMenuItemService.GetAllMenuItems();
            }
            else
            {
                var filterDto = new FilterDto
                {
                    MealType = string.IsNullOrEmpty(filters.MealType) ? MealType.Unknown : filters.MealType.ToEnum<MealType>(),
                    Allergens = filters.Allergens?.Select(x => x.ToEnum<AllergenType>()),
                };

                menuItemDtos = await _showMenuItemService.GetAllMenuItemsWithFilters(filterDto);
            }

            var showMenuItemViewModels = _mapper.Map<IEnumerable<ShowMenuItemViewModel>>(menuItemDtos);
            return showMenuItemViewModels;
        }
    }
}
