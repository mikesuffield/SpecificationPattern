using FluentAssertions;
using SpecificationPattern.Shared.Extensions;
using System;
using Xunit;

namespace SpecificationPattern.Shared.Tests
{
    public class StringExtenstionsTests
    {
        [Theory]
        [InlineData("Test1", ExampleEnum.Test1)]
        [InlineData("Test2", ExampleEnum.Test2)]
        public void ToEnum_ReturnsEnum(string input, ExampleEnum expectedResult)
        {
            var result = input.ToEnum<ExampleEnum>();

            result.Should().Be(expectedResult);
        }

        [Fact]
        public void ToEnum_ThrowsException()
        {
            Action act = () => "unknown".ToEnum<ExampleEnum>();

            act.Should().Throw<Exception>().WithMessage($"Unable to convert string 'unknown' to ExampleEnum Enum");
        }

        public enum ExampleEnum
        {
            Test1,
            Test2,
        }
    }
}
