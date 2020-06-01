using FluentAssertions;
using Moq;
using SpecificationPattern.Application.ApplicationServices;
using SpecificationPattern.Application.DTOs;
using SpecificationPattern.Core.Interfaces;
using SpecificationPattern.Core.Models;
using SpecificationPattern.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SpecificationPattern.Application.Tests
{
    public class MenuItemServiceTests
    {
        private static readonly Mock<IMenuItemRepository> MockRepository = new Mock<IMenuItemRepository>();

        private static readonly MenuItemDto MenuItemDto = new MenuItemDto
        {
            Id = Guid.NewGuid(),
            Name = "Test MenuItemDto",
            Price = 5.99f,
            MealType = MealType.Main,
            Allergens = new List<AllergenDto>
            {
                new AllergenDto
                {
                    Id = Guid.NewGuid(),
                    Name = AllergenType.Soya,
                },
            },
        };

        private static readonly MenuItem MenuItem = new MenuItem
        {
            Id = MenuItemDto.Id,
            Name = MenuItemDto.Name,
            Price = MenuItemDto.Price,
            MealType = MenuItemDto.MealType,
            Allergens = MenuItemDto.Allergens.Select(allergenDto => new Allergen
            {
                Id = allergenDto.Id,
                Name = allergenDto.Name,
            }),
        };

        [Fact]
        public async Task CreateMenuItem_ReturnsDTO()
        {
            var SUT = Setup();

            var result = await SUT.CreateMenuItem(MenuItemDto);

            result.Should().BeEquivalentTo(MenuItemDto);
        }

        [Fact]
        public async Task DeleteMenuItem_CallsRepository()
        {
            var SUT = Setup();

            await SUT.DeleteMenuItem(MenuItem.Id);

            MockRepository.Verify(x => x.Find(MenuItem.Id), Times.Once);
            MockRepository.Verify(x => x.Remove(MenuItem), Times.Once);
        }

        private MenuItemService Setup()
        {
            MockRepository.Setup(x => x.Add(It.IsAny<MenuItem>())).Returns(Task.FromResult(MenuItem));
            MockRepository.Setup(x => x.Find(MenuItem.Id)).Returns(Task.FromResult(MenuItem));
            MockRepository.Setup(x => x.Remove(MenuItem));

            var SUT = new MenuItemService(MockRepository.Object);

            return SUT;
        }
    }
}
