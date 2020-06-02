using FluentAssertions;
using Moq;
using SpecificationPattern.Core.Specifications;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SpecificationPattern.Core.Tests.Specifications
{
    public class NotSpecificationTests
    {
        [Fact]
        public void IsSatisfiedBy_ReturnsCorrectValue()
        {
            var mockSpecification = new Mock<ISpecification<object>>();
            mockSpecification.Setup(x => x.IsSatisfiedBy(It.IsAny<object>())).Returns(false);

            var notSpecification = mockSpecification.Object.Not();
            var result = notSpecification.IsSatisfiedBy(It.IsAny<object>());

            result.Should().BeTrue();
        }

        [Fact]
        public void IsSatisfiedBy_AppendsToBuilder()
        {
            var builder = new StringBuilder();
            var parameters = new Dictionary<string, object>();

            var mockSpecification = new Mock<ISpecification<object>>();
            mockSpecification.Setup(x => x.IsSatisfiedBy(builder, parameters))
                .Callback(() =>
                {
                    builder.Append("[MealType] = @mealType");
                    parameters.Add("mealType", "Starter");
                });

            var notSpecification = mockSpecification.Object.Not();

            notSpecification.IsSatisfiedBy(builder, parameters);

            builder.ToString().Should().Be("NOT [MealType] = @mealType");
            parameters["mealType"].Should().Be("Starter");
        }
    }
}
