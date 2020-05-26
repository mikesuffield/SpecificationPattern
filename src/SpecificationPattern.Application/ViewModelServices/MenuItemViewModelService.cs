using SpecificationPattern.Application.ApplicationServices;
using SpecificationPattern.Application.DTOs;
using SpecificationPattern.Application.ViewModels;
using SpecificationPattern.Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace SpecificationPattern.Application.ViewModelServices
{
    public class MenuItemViewModelService : IMenuItemViewModelService
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IUnitOfWork _unitOfWork;

        public MenuItemViewModelService(
            IMenuItemService menuItemService,
            IUnitOfWork unitOfWork)
        {
            _menuItemService = menuItemService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ShowMenuItemViewModel> Create(CreateMenuItemViewModel createMenuItemViewModel)
        {
            var createMenuItemDto = new MenuItemDto(createMenuItemViewModel);

            var createdDto = await _menuItemService.CreateMenuItem(createMenuItemDto);
            _unitOfWork.Complete();

            return new ShowMenuItemViewModel(createdDto);
        }

        public async Task Delete(Guid id)
        {
            await _menuItemService.DeleteMenuItem(id);
            _unitOfWork.Complete();
        }
    }
}
