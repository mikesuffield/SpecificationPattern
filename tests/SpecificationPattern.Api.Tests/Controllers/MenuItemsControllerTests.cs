using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SpecificationPattern.Application.ViewModels;
using SpecificationPattern.Application.ViewModelServices;
using SpecificationPattern.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SpecificationPattern.Api.Tests
{
    public class MenuItemsControllerTests
    {
        private static readonly ShowMenuItemViewModel MenuItem = new ShowMenuItemViewModel
        {
            Id = Guid.NewGuid(),
            Name = "Test menu item",
            Price = "£5.99",
            MealType = "Starter",
            Allergens = new List<string>
            {
                "Soya",
            },
        };

        private static readonly IEnumerable<ShowMenuItemViewModel> MenuItems = new List<ShowMenuItemViewModel>
        {
            MenuItem,
        };

        [Fact]
        public async Task Get_ReturnsMenuItems()
        {
            var SUT = Setup();

            var result = await SUT.Get(null, null);
            var okResult = result as ObjectResult;

            okResult.Should().NotBeNull();
            okResult.Should().BeOfType(typeof(OkObjectResult));
            okResult.Value.Should().BeOfType(typeof(List<ShowMenuItemViewModel>));
            okResult.Value.Should().Be(MenuItems);
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetById_ReturnsMenuItem()
        {
            var SUT = Setup();

            var result = await SUT.GetById(Guid.NewGuid());
            var okResult = result as ObjectResult;

            okResult.Should().NotBeNull();
            okResult.Should().BeOfType(typeof(OkObjectResult));
            okResult.Value.Should().BeOfType(typeof(ShowMenuItemViewModel));
            okResult.Value.Should().Be(MenuItem);
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Post_ReturnsCreatedResult()
        {
            var SUT = Setup();
            var createMenuItemViewModel = new CreateMenuItemViewModel
            {
                Name = "Test input meal",
                Price = 3.50f,
                MealType = "Main",
            };

            var expectedResult = new CreatedResult($"/MenuItems/{MenuItem.Id}", MenuItem).Value;
            var result = await SUT.Post(createMenuItemViewModel);
            var createdResult = result as ObjectResult;

            createdResult.Should().NotBeNull();
            createdResult.Should().BeOfType(typeof(CreatedResult));
            createdResult.Value.Should().Be(expectedResult);
            createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        [Fact]
        public async Task Delete_ReturnsNoContentResult()
        {
            var SUT = Setup();

            var result = await SUT.Delete(Guid.NewGuid());
            var noContentResult = result as StatusCodeResult;

            noContentResult.Should().NotBeNull();
            noContentResult.Should().BeOfType(typeof(NoContentResult));
            noContentResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        private MenuItemsController Setup()
        {
            var mockShowMenuItemViewModelService = new Mock<IShowMenuItemViewModelService>();
            mockShowMenuItemViewModelService.Setup(x => x.GetMenuItems(null, null)).Returns(Task.FromResult(MenuItems));
            mockShowMenuItemViewModelService.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(MenuItem));

            var mockMenuItemViewModelService = new Mock<IMenuItemViewModelService>();
            mockMenuItemViewModelService.Setup(x => x.Create(It.IsAny<CreateMenuItemViewModel>())).Returns(Task.FromResult(MenuItem));
            mockMenuItemViewModelService.Setup(x => x.Delete(It.IsAny<Guid>()));

            var mockLogger = new Mock<ILogger<MenuItemsController>>();

            var SUT = new MenuItemsController(mockShowMenuItemViewModelService.Object, 
                mockMenuItemViewModelService.Object, 
                mockLogger.Object);

            return SUT;
        }
    }
}
