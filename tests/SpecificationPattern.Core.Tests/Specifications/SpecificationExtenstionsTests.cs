using FluentAssertions;
using Moq;
using SpecificationPattern.Core.Specifications;
using Xunit;

namespace SpecificationPattern.Core.Tests.Specifications
{
    public class SpecificationExtenstionsTests
    {
        [Fact]
        public void And_ReturnsAndSpecification()
        {
            var left = new Mock<ISpecification<object>>();
            var right = new Mock<ISpecification<object>>();

            var result = left.Object.And(right.Object);

            result.Should().BeOfType(typeof(AndSpecification<object>));
        }

        [Fact]
        public void Or_ReturnsOrSpecification()
        {
            var left = new Mock<ISpecification<object>>();
            var right = new Mock<ISpecification<object>>();

            var result = left.Object.Or(right.Object);

            result.Should().BeOfType(typeof(OrSpecification<object>));
        }

        [Fact]
        public void Not_ReturnsNotSpecification()
        {
            var SUT = new Mock<ISpecification<object>>();

            var result = SUT.Object.Not();

            result.Should().BeOfType(typeof(NotSpecification<object>));
        }
    }
}
