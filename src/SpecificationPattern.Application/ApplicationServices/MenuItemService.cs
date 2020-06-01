using SpecificationPattern.Application.DTOs;
using SpecificationPattern.Core.Interfaces;
using SpecificationPattern.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SpecificationPattern.Application.ApplicationServices
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuItemService(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public async Task<MenuItemDto> CreateMenuItem(MenuItemDto menuItemDto)
        {
            var menuItem = new MenuItem
            {
                Id = menuItemDto.Id,
                Name = menuItemDto.Name,
                Price = menuItemDto.Price,
                MealType = menuItemDto.MealType,
                Allergens = menuItemDto.Allergens.Select(allergenDto => new Allergen
                {
                    Id = allergenDto.Id,
                    Name = allergenDto.Name,
                }),
            };

            var createdMenuItem = await _menuItemRepository.Add(menuItem);
            return new MenuItemDto(createdMenuItem);
        }

        public async Task DeleteMenuItem(Guid id)
        {
            var menuItem = await _menuItemRepository.Find(id);
            if (menuItem == null)
            {
                throw new Exception($"MenuItem with ID {id} could not be found");
            }
            await _menuItemRepository.Remove(menuItem);
        }
    }
}
