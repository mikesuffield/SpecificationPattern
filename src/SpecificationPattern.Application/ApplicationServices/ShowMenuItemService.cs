using SpecificationPattern.Application.DTOs;
using SpecificationPattern.Core.Interfaces;
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

            return new MenuItemDto(menuItem);
        }
    }
}
