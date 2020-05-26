using SpecificationPattern.Application.ViewModels;
using System;
using System.Threading.Tasks;

namespace SpecificationPattern.Application.ViewModelServices
{
    public interface IMenuItemViewModelService
    {
        Task Delete(Guid id);

        Task<ShowMenuItemViewModel> Create(CreateMenuItemViewModel menuItemViewModel);
    }
}
