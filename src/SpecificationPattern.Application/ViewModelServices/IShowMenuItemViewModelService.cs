using SpecificationPattern.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpecificationPattern.Application.ViewModelServices
{
    public interface IShowMenuItemViewModelService
    {
        Task<IEnumerable<ShowMenuItemViewModel>> GetMenuItems(string type, string allergens);

        Task<ShowMenuItemViewModel> GetById(Guid id);
    }
}
