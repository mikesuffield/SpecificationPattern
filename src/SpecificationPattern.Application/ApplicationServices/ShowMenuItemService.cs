using AutoMapper;
using SpecificationPattern.Application.DTOs;
using SpecificationPattern.Core.Interfaces;
using SpecificationPattern.Core.Models;
using SpecificationPattern.Core.Specifications;
using SpecificationPattern.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpecificationPattern.Application.ApplicationServices
{
    public class ShowMenuItemService : IShowMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IMapper _mapper;

        public ShowMenuItemService(IMenuItemRepository menuItemRepository, IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MenuItemDto>> GetAllMenuItems()
        {
            var menuItems = await _menuItemRepository.All();

            var menuItemDtos = _mapper.Map<IEnumerable<MenuItemDto>>(menuItems);
            return menuItemDtos;
        }

        public async Task<MenuItemDto> GetMenuItemById(Guid id)
        {
            var menuItem = await _menuItemRepository.Find(id);

            if (menuItem == null)
            {
                throw new Exception($"MenuItem with ID {id} could not be found");
            }

            var menuItemDto = _mapper.Map<MenuItemDto>(menuItem);
            return menuItemDto;
        }

        public async Task<IEnumerable<MenuItemDto>> GetAllMenuItemsWithFilters(FilterDto filters)
        {
            ISpecification<MenuItem> specification;

            var mealType = filters.MealType;
            var allergens = filters.Allergens;

            if (!(mealType == MealType.Unknown) && allergens != null && allergens.Any())
            {
                var mealTypeSpec = new MenuItemForMealTypeSpecification(mealType);
                var excludeAllergensSpec = new MenuItemForAllergensSpecification(allergens).Not();

                specification = mealTypeSpec.And(excludeAllergensSpec);
            }
            else if (allergens != null && allergens.Any())
            {
                specification = new MenuItemForAllergensSpecification(allergens).Not();
            }
            else
            {
                specification = new MenuItemForMealTypeSpecification(mealType);
            }

            var menuItems = await _menuItemRepository.All(specification);

            var menuItemDtos = _mapper.Map<IEnumerable<MenuItemDto>>(menuItems);
            return menuItemDtos;
        }
    }
}
