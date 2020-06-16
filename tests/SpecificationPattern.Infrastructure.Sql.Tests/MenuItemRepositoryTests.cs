using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SpecificationPattern.Core.Models;
using SpecificationPattern.Shared.Enums;
using SpecificationPattern.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SpecificationPattern.Infrastructure.Sql.Tests
{
    public class MenuItemRepositoryTests
    {
        private static readonly Guid MenuItemId = Guid.NewGuid();
        private static readonly Guid AllergenId = Guid.NewGuid();
        private static readonly MenuItem MenuItem = new MenuItem
        {
            Id = MenuItemId,
            Name = "Test",
            Price = 5.99f,
            MealType = MealType.Main,
            Allergens = new List<Allergen>
            {
                new Allergen
                {
                    Id = AllergenId,
                    MenuItemId = MenuItemId,
                    AllergenType = AllergenType.Soya,
                },
            },
        };

        [Fact]
        public async Task Add_ReturnsMenuItem()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var SUT = Setup(mockUnitOfWork);

            var result = await SUT.Add(MenuItem);

            result.Should().BeEquivalentTo(MenuItem);

            var insertMenuItemQuery = "INSERT INTO MenuItems ([Id], [Name], [Price], [MealType], [CreatedAt]) OUTPUT Inserted.RowRevision VALUES (@id, @name, @price, @mealType, @createdAt)";
            mockUnitOfWork.Verify(x => x.ExecuteScalar<byte[]>(insertMenuItemQuery, It.IsAny<Dictionary<string, object>>()), Times.Once);

            var insertAllergensQuery = "INSERT INTO Allergens ([Id], [MenuItemId], [AllergenType], [CreatedAt]) OUTPUT Inserted.RowRevision VALUES (@id, @menuItemId, @allergenType, @createdAt)";
            mockUnitOfWork.Verify(x => x.ExecuteScalar<byte[]>(insertAllergensQuery, It.IsAny<Dictionary<string, object>>()), Times.Once);
        }

        [Fact]
        public async Task All_ReturnsMenuItems()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var menuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Id = MenuItemId,
                    Name = "Test",
                    Price = 5.99f,
                    MealType = MealType.Main,
                },
            }.AsEnumerable();
            var getAllMenuItemsQuery = "SELECT * FROM MenuItems";
            mockUnitOfWork
                .Setup(x => x.Query<MenuItem>(getAllMenuItemsQuery, It.IsAny<Dictionary<string, object>>()))
                .Returns(Task.FromResult(menuItems));

            var allergens = new List<Allergen>
            {
                new Allergen
                {
                    Id = AllergenId,
                    MenuItemId = MenuItemId,
                    AllergenType = AllergenType.Soya,
                }
            }.AsEnumerable();
            var getAllAllergensQuery = "SELECT * FROM Allergens";
            mockUnitOfWork
                .Setup(x => x.Query<Allergen>(getAllAllergensQuery, It.IsAny<Dictionary<string, object>>()))
                .Returns(Task.FromResult(allergens));

            var SUT = Setup(mockUnitOfWork);

            var expectedResult = new List<MenuItem>
            {
                new MenuItem
                {
                    Id = MenuItemId,
                    Name = "Test",
                    Price = 5.99f,
                    MealType = MealType.Main,
                    Allergens = allergens,
                },
            }.AsEnumerable();
            var result = await SUT.All();

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task Find_ReturnsSpecificMenuItem()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var getMenuItemsByIdQuery = "SELECT * FROM MenuItems WHERE Id = @id";
            var menuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Id = MenuItemId,
                    Name = "Test",
                    Price = 5.99f,
                    MealType = MealType.Main,
                },
            }.AsEnumerable();
            mockUnitOfWork
                .Setup(x => x.Query<MenuItem>(getMenuItemsByIdQuery, It.IsAny<Dictionary<string, object>>()))
                .Returns(Task.FromResult(menuItems));

            var getAllergensByMenuItemIdQuery = "SELECT * FROM Allergens WHERE MenuItemId = @id";
            var allergens = new List<Allergen>
            {
                new Allergen
                {
                    Id = AllergenId,
                    MenuItemId = MenuItemId,
                    AllergenType = AllergenType.Soya,
                },
            }.AsEnumerable();
            mockUnitOfWork
                .Setup(x => x.Query<Allergen>(getAllergensByMenuItemIdQuery, It.IsAny<Dictionary<string, object>>()))
                .Returns(Task.FromResult(allergens));

            var SUT = Setup(mockUnitOfWork);

            var expectedResult = new MenuItem
            {
                Id = MenuItemId,
                Name = "Test",
                Price = 5.99f,
                MealType = MealType.Main,
                Allergens = allergens,
            };
            var result = await SUT.Find(MenuItemId);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task Remove_CallsUnitOfWork()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            
            var deleteAllergenQuery = "DELETE FROM Allergens WHERE Id = @id AND RowRevision = @rowRevision";
            mockUnitOfWork
                .Setup(x => x.Execute(deleteAllergenQuery, It.IsAny<Dictionary<string, object>>()))
                .Returns(Task.FromResult(1));

            var deleteMenuItemQuery = "DELETE FROM MenuItems WHERE Id = @id AND RowRevision = @rowRevision";
            mockUnitOfWork
                .Setup(x => x.Execute(deleteMenuItemQuery, It.IsAny<Dictionary<string, object>>()))
                .Returns(Task.FromResult(1));

            var SUT = Setup(mockUnitOfWork);

            await SUT.Remove(MenuItem);

            mockUnitOfWork.Verify(x => x.Execute(deleteAllergenQuery, It.IsAny<Dictionary<string, object>>()), Times.Once); 
            mockUnitOfWork.Verify(x => x.Execute(deleteMenuItemQuery, It.IsAny<Dictionary<string, object>>()), Times.Once);
        }

        private MenuItemRepository Setup(Mock<IUnitOfWork> mockUnitOfWork)
        {
            var mockLogger = new Mock<ILogger<MenuItemRepository>>();

            var SUT = new MenuItemRepository(mockUnitOfWork.Object, mockLogger.Object);

            return SUT;
        }
    }
}
