using AutoMapper;
using SpecificationPattern.Application.DTOs;
using SpecificationPattern.Core.Interfaces;
using SpecificationPattern.Core.Models;
using System;
using System.Threading.Tasks;

namespace SpecificationPattern.Application.ApplicationServices
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IMapper _mapper;

        public MenuItemService(IMenuItemRepository menuItemRepository, IMapper mapper)
        {
            _menuItemRepository = menuItemRepository;
            _mapper = mapper;
        }

        public async Task<MenuItemDto> CreateMenuItem(MenuItemDto menuItemDto)
        {
            var menuItem = _mapper.Map<MenuItem>(menuItemDto);

            var createdMenuItem = await _menuItemRepository.Add(menuItem);

            var createdMenuItemDto = _mapper.Map<MenuItemDto>(createdMenuItem);
            return createdMenuItemDto;
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
