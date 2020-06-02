using FluentAssertions;
using Moq;
using SpecificationPattern.Application.ApplicationServices;
using SpecificationPattern.Application.DTOs;
using SpecificationPattern.Core.Interfaces;
using SpecificationPattern.Core.Models;
using SpecificationPattern.Core.Specifications;
using SpecificationPattern.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SpecificationPattern.Application.Tests
{
    public class ShowMenuItemServiceTests
    {
        private static readonly MenuItem MenuItem = new MenuItem
        {
            Id = Guid.NewGuid(),
            Name = "Test menu item",
            Price = 5.99f, 
            MealType = MealType.Main,
            Allergens = new List<Allergen>
            {
                new Allergen
                {
                    Id = Guid.NewGuid(),
                    AllergenType = AllergenType.Soya,
                },
            },
        };

        private static readonly IEnumerable<MenuItem> MenuItems = new List<MenuItem>
        {
            MenuItem,
        };

        [Fact]
        public async Task GetAllMenuItems_ReturnsMenuItemDtos()
        {
            var SUT = Setup();

            var expectedResult = MenuItems.Select(x => new MenuItemDto(x));
            var result = await SUT.GetAllMenuItems();

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetMenuItemById_ReturnsSpecificationMenuItem()
        {
            var SUT = Setup();

            var expectedResult = new MenuItemDto(MenuItem);
            var result = await SUT.GetMenuItemById(MenuItem.Id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetAllMenuItemsWithFilters_ReturnsMenuItemDtos()
        {
            var SUT = Setup();

            var expectedResult =  MenuItems.Select(x => new MenuItemDto(x));

            var filterDto = new FilterDto
            {
                MealType = MealType.Starter,
                Allergens = new List<AllergenType>
                {
                    AllergenType.Soya,
                },
            };
            var result = await SUT.GetAllMenuItemsWithFilters(filterDto);

            result.Should().BeEquivalentTo(expectedResult);
        }

        private ShowMenuItemService Setup()
        {
            var mockRepository = new Mock<IMenuItemRepository>();
            mockRepository.Setup(x => x.All())
                .Returns(Task.FromResult(MenuItems));
            mockRepository.Setup(x => x.Find(MenuItem.Id))
                .Returns(Task.FromResult(MenuItem));
            mockRepository.Setup(x => x.All(It.IsAny<ISpecification<MenuItem>>()))
                .Returns(Task.FromResult(MenuItems));

            var SUT = new ShowMenuItemService(mockRepository.Object);

            return SUT;
        }
    }
}
