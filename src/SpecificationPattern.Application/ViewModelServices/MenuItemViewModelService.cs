using AutoMapper;
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
        private readonly IMapper _mapper;

        public MenuItemViewModelService(
            IMenuItemService menuItemService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _menuItemService = menuItemService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ShowMenuItemViewModel> Create(CreateMenuItemViewModel createMenuItemViewModel)
        {
            var createMenuItemDto = _mapper.Map<MenuItemDto>(createMenuItemViewModel);

            var createdDto = await _menuItemService.CreateMenuItem(createMenuItemDto);
            _unitOfWork.Complete();

            var showMenuItemViewModel = _mapper.Map<ShowMenuItemViewModel>(createdDto);
            return showMenuItemViewModel;
        }

        public async Task Delete(Guid id)
        {
            await _menuItemService.DeleteMenuItem(id);
            _unitOfWork.Complete();
        }
    }
}
