using AutoMapper;
using FluentAssertions;
using Moq;
using SpecificationPattern.Application.ApplicationServices;
using SpecificationPattern.Application.DTOs;
using SpecificationPattern.Application.Profiles;
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

            var expectedResult = MenuItems.Select(x => new MenuItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                MealType = x.MealType,
                Allergens = x.Allergens.Select(y => new AllergenDto
                {
                    Id = y.Id,
                    AllergenType = y.AllergenType,
                }),
            });

            var result = await SUT.GetAllMenuItems();

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetMenuItemById_ReturnsSpecificationMenuItem()
        {
            var SUT = Setup();

            var expectedResult = new MenuItemDto
            {
                Id = MenuItem.Id,
                Name = MenuItem.Name,
                Price = MenuItem.Price,
                MealType = MenuItem.MealType,
                Allergens = MenuItem.Allergens.Select(y => new AllergenDto
                {
                    Id = y.Id,
                    AllergenType = y.AllergenType,
                }),
            };

            var result = await SUT.GetMenuItemById(MenuItem.Id);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task GetAllMenuItemsWithFilters_ReturnsMenuItemDtos()
        {
            var SUT = Setup();

            var expectedResult =  MenuItems.Select(x => new MenuItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                MealType = x.MealType,
                Allergens = x.Allergens.Select(y => new AllergenDto
                {
                    Id = y.Id,
                    AllergenType = y.AllergenType,
                }),
            });

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
            mockRepository
                .Setup(x => x.All())
                .Returns(Task.FromResult(MenuItems));
            mockRepository
                .Setup(x => x.Find(MenuItem.Id))
                .Returns(Task.FromResult(MenuItem));
            mockRepository
                .Setup(x => x.All(It.IsAny<ISpecification<MenuItem>>()))
                .Returns(Task.FromResult(MenuItems));

            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MenuItemProfile>();
            });
            var mapper = mapperConfig.CreateMapper();

            var SUT = new ShowMenuItemService(mockRepository.Object, mapper);

            return SUT;
        }
    }
}
