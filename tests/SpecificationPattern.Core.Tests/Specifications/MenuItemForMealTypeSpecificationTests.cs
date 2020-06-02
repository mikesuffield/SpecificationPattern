using FluentAssertions;
using SpecificationPattern.Core.Models;
using SpecificationPattern.Core.Specifications;
using SpecificationPattern.Shared.Enums;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SpecificationPattern.Core.Tests
{
    public class MenuItemForMealTypeSpecificationTests
    {
        private static readonly MealType MealType = MealType.Main;

        [Fact]
        public void IsSatisfiedBy_ReturnsTrue()
        {
            var spec = new MenuItemForMealTypeSpecification(MealType);

            var menuItem = new MenuItem
            {
                MealType = MealType.Main,
            };

            var result = spec.IsSatisfiedBy(menuItem);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_ReturnsFalse()
        {
            var spec = new MenuItemForMealTypeSpecification(MealType);

            var menuItem = new MenuItem
            {
                MealType = MealType.Starter,
            };

            var result = spec.IsSatisfiedBy(menuItem);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsSatisfiedBy_AppendsToBuilderAndAddsParameters()
        {
            var spec = new MenuItemForMealTypeSpecification(MealType);

            var builder = new StringBuilder();
            var parameters = new Dictionary<string, object>();

            spec.IsSatisfiedBy(builder, parameters);

            builder.ToString().Should().Be("[MealType] = @mealType");
            parameters["mealType"].Should().Be(MealType.ToString());
        }
    }
}
