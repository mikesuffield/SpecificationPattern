using SpecificationPattern.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace SpecificationPattern.Application.ApplicationServices
{
    public interface IMenuItemService
    {
        Task<MenuItemDto> CreateMenuItem(MenuItemDto menuItemDto);

        Task DeleteMenuItem(Guid id);
    }
}
