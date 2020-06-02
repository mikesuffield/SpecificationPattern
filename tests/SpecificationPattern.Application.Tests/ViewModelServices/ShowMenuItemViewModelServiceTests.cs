using FluentAssertions;
using Moq;
using SpecificationPattern.Application.ApplicationServices;
using SpecificationPattern.Application.DTOs;
using SpecificationPattern.Application.ViewModels;
using SpecificationPattern.Application.ViewModelServices;
using SpecificationPattern.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SpecificationPattern.Application.Tests
{
    public class ShowMenuItemViewModelServiceTests
    {
        private static readonly MenuItemDto MenuItemDto = new MenuItemDto
        {
            Id = Guid.NewGuid(),
            Name = "Test MenuItemDto",
            Price = 7.99f,
            MealType = MealType.Main,
            Allergens = new List<AllergenDto>
            {
                new AllergenDto
                {
                    Id = Guid.NewGuid(),
                    AllergenType = AllergenType.Soya,
                },
            },
        };

        private static readonly IEnumerable<MenuItemDto> MenuItemDtos = new List<MenuItemDto>
        {
            MenuItemDto,
        };

        [Fact]
        public async Task GetMenuItems_ReturnsViewModels()
        {
            var SUT = Setup();

            var expectedResult = MenuItemDtos.Select(x => new ShowMenuItemViewModel(x));
            var filterViewModel = new FilterViewModel();
            var result = await SUT.GetMenuItems(filterViewModel);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetById_ReturnsSpecificViewModel()
        {
            var SUT = Setup();

            var expectedResult = new ShowMenuItemViewModel(MenuItemDto);
            var result = await SUT.GetById(MenuItemDto.Id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        private ShowMenuItemViewModelService Setup()
        {
            var mockService = new Mock<IShowMenuItemService>();
            mockService.Setup(x => x.GetMenuItemById(MenuItemDto.Id))
                .Returns(Task.FromResult(MenuItemDto));
            mockService.Setup(x => x.GetAllMenuItems())
                .Returns(Task.FromResult(MenuItemDtos));

            var SUT = new ShowMenuItemViewModelService(mockService.Object);

            return SUT;
        }
    }
}
