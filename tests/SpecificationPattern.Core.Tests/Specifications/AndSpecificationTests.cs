using FluentAssertions;
using Moq;
using SpecificationPattern.Core.Specifications;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SpecificationPattern.Core.Tests.Specifications
{
    public class AndSpecificationTests
    {

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, true, false)]
        [InlineData(true, false, false)]
        [InlineData(true, true, true)]
        public void IsSatisfiedBy_ReturnsCorrectValue(bool leftBool, bool rightBool, bool expectedResult)
        {
            var mockLeftSpecification = new Mock<ISpecification<object>>();
            mockLeftSpecification.Setup(x => x.IsSatisfiedBy(It.IsAny<object>())).Returns(leftBool);

            var mockRightSpecification = new Mock<ISpecification<object>>();
            mockRightSpecification.Setup(x => x.IsSatisfiedBy(It.IsAny<object>())).Returns(rightBool);

            var andSpecification = mockLeftSpecification.Object.And(mockRightSpecification.Object);
            var result = andSpecification.IsSatisfiedBy(It.IsAny<object>());

            result.Should().Be(expectedResult);
        }

        [Fact]
        public void IsSatisfiedBy_AppendsToBuilderAndAddsParameters()
        {
            var builder = new StringBuilder();
            var parameters = new Dictionary<string, object>();

            var mockLeftSpecification = new Mock<ISpecification<object>>();
            mockLeftSpecification.Setup(x => x.IsSatisfiedBy(builder, parameters))
                .Callback(() =>
                {
                    builder.Append("[MealType] = @mealType");
                    parameters.Add("mealType", "Starter");
                });

            var mockRightSpecification = new Mock<ISpecification<object>>();
            mockRightSpecification.Setup(x => x.IsSatisfiedBy(builder, parameters))
                .Callback(() =>
                {
                    builder.Append("[Id] = @id");
                    parameters.Add("id", 117);
                });

            var andSpecification = mockLeftSpecification.Object.And(mockRightSpecification.Object);
            andSpecification.IsSatisfiedBy(builder, parameters);

            builder.ToString().Should().Be("([MealType] = @mealType) AND ([Id] = @id)");
            parameters["mealType"].Should().Be("Starter");
            parameters["id"].Should().Be(117);
        }
    }
}
