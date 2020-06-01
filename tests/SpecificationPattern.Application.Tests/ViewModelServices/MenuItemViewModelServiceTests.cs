using FluentAssertions;
using Moq;
using SpecificationPattern.Application.ApplicationServices;
using SpecificationPattern.Application.DTOs;
using SpecificationPattern.Application.ViewModels;
using SpecificationPattern.Application.ViewModelServices;
using SpecificationPattern.Shared.Enums;
using SpecificationPattern.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SpecificationPattern.Application.Tests
{
    public class MenuItemViewModelServiceTests
    {
        private static readonly Mock<IMenuItemService> MockService = new Mock<IMenuItemService>();

        private static readonly MenuItemDto CreatedDto = new MenuItemDto
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Price = 1.99f,
            MealType = MealType.Starter,
            Allergens = new List<AllergenDto>
            {
                new AllergenDto
                {
                    Id = Guid.NewGuid(),
                    Name = AllergenType.Soya,
                },
            },
        };

        [Fact]
        public async Task Create_ReturnsViewModel()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var SUT = Setup(mockUnitOfWork);
            var createMenuItemViewModel = new CreateMenuItemViewModel
            {
                Name = "Test",
                Price = 1.99f,
                MealType = "Starter",
                Allergens = new List<string>
                {
                    "Soya",
                }
            };

            var expectedResult = new ShowMenuItemViewModel(CreatedDto);
            var result = await SUT.Create(createMenuItemViewModel);

            result.Should().BeEquivalentTo(expectedResult);
            mockUnitOfWork.Verify(x => x.Complete(), Times.Once);
        }

        [Fact]
        public async Task Delete_CallsServices()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var SUT = Setup(mockUnitOfWork);

            var menuItemId = Guid.NewGuid();
            await SUT.Delete(menuItemId);

            MockService.Verify(x => x.DeleteMenuItem(menuItemId), Times.Once);
            mockUnitOfWork.Verify(x => x.Complete(), Times.Once);
        }

        private MenuItemViewModelService Setup(Mock<IUnitOfWork> mockUnitOfWork)
        {
            MockService.Setup(x => x.CreateMenuItem(It.IsAny<MenuItemDto>())).Returns(Task.FromResult(CreatedDto));
            MockService.Setup(x => x.DeleteMenuItem(It.IsAny<Guid>()));

            var SUT = new MenuItemViewModelService(MockService.Object, mockUnitOfWork.Object);

            return SUT;
        }
    }
}
