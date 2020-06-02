using SpecificationPattern.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpecificationPattern.Application.ApplicationServices
{
    public interface IShowMenuItemService
    {
        Task<IEnumerable<MenuItemDto>> GetAllMenuItems();

        Task<MenuItemDto> GetMenuItemById(Guid id);

        Task<IEnumerable<MenuItemDto>> GetAllMenuItemsWithFilters(FilterDto filters);
    }
}
