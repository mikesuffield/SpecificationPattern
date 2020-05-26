using SpecificationPattern.Application.ApplicationServices;
using SpecificationPattern.Application.ViewModels;
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

        public async Task<IEnumerable<ShowMenuItemViewModel>> GetMenuItems()
        {
            var menuItemDtos = await _showMenuItemService.GetAllMenuItems();

            return menuItemDtos.Select(menuItemDto => new ShowMenuItemViewModel(menuItemDto));
        }
    }
}
