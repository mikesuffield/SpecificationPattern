using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpecificationPattern.Application.ViewModels;
using SpecificationPattern.Application.ViewModelServices;
using System;
using System.Threading.Tasks;

namespace SpecificationPattern.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MenuItemsController : ControllerBase
    {
        private readonly IShowMenuItemViewModelService _showMenuItemsViewModelService;
        private readonly IMenuItemViewModelService _menuItemViewModelService;
        private readonly ILogger<MenuItemsController> _logger;

        public MenuItemsController(
            IShowMenuItemViewModelService showMenuItemsViewModelService,
            IMenuItemViewModelService menuItemViewModelService,
            ILogger<MenuItemsController> logger)
        {
            _showMenuItemsViewModelService = showMenuItemsViewModelService;
            _menuItemViewModelService = menuItemViewModelService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // TODO: Add query string parameters to this endpoint to filter based on meal type and allergens
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Called: /MenuItems/Get");

            var menuItems = await _showMenuItemsViewModelService.GetMenuItems();
            return Ok(menuItems);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            _logger.LogInformation($"Called: /MenuItems/Get/{id}");

            var menuItem = await _showMenuItemsViewModelService.GetById(id);
            return Ok(menuItem);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] CreateMenuItemViewModel menuItemViewModel)
        {
            _logger.LogInformation($"Called: /MenuItems/Create");

            var showMenuItemViewModel = await _menuItemViewModelService.Create(menuItemViewModel);
            return new CreatedResult($"/MenuItems/{showMenuItemViewModel.Id}", showMenuItemViewModel);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Called: /MenuItems/Delete/{id}");

            await _menuItemViewModelService.Delete(id);
            return new NoContentResult();
        }
    }
}
