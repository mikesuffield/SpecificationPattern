using FluentAssertions;
using SpecificationPattern.Core.Models;
using SpecificationPattern.Core.Specifications;
using SpecificationPattern.Shared.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SpecificationPattern.Core.Tests
{
    public class MenuItemForAllergensSpecificationTests
    {
        private static readonly IEnumerable<AllergenType> Allergens = new List<AllergenType>
        {
            AllergenType.Soya,
            AllergenType.Milk,
        };

        [Fact]
        public void IsSatisfiedBy_ReturnsTrue()
        {
            var spec = new MenuItemForAllergensSpecification(Allergens);

            var menuItem = new MenuItem
            {
                Allergens = new List<Allergen>
                {
                    new Allergen
                    {
                        AllergenType = AllergenType.Soya,
                    },
                    new Allergen
                    {
                        AllergenType = AllergenType.Celery,
                    },
                },
            };

            var result = spec.IsSatisfiedBy(menuItem);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_ReturnsFalse()
        {
            var spec = new MenuItemForAllergensSpecification(Allergens);

            var menuItem = new MenuItem
            {
                Allergens = new List<Allergen>
                {
                    new Allergen
                    {
                        AllergenType = AllergenType.Crustaceans,
                    },
                },
            };

            var result = spec.IsSatisfiedBy(menuItem);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsSatisfiedBy_AppendsToBuilderAndAddsParameters()
        {
            var spec = new MenuItemForAllergensSpecification(Allergens);

            var builder = new StringBuilder();
            var parameters = new Dictionary<string, object>();

            spec.IsSatisfiedBy(builder, parameters);

            builder.ToString().Should().Be("[Id] IN (SELECT MenuItemId FROM Allergens WHERE AllergenType IN @allergens)");
            parameters["allergens"].Should().BeEquivalentTo(Allergens.Select(x => x.ToString()));
        }
    }
}
