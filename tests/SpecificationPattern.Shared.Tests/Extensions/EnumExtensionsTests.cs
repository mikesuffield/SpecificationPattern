using FluentAssertions;
using SpecificationPattern.Shared.Extensions;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace SpecificationPattern.Shared.Tests.Extensions
{
    public class EnumExtensionsTests
    {
        [Fact]
        public void DisplayName_ReturnsEnumNameWhenNoDescriptionExists()
        {
            var result = ExampleEnum.Test1.DisplayName();

            result.Should().Be("Test1");
        }

        [Fact]
        public void DisplayName_ReturnsDescription()
        {
            var result = ExampleEnum.Test2.DisplayName();

            result.Should().Be("Test 2 display name");
        }

        public enum ExampleEnum
        {
            Test1,
            [Display(Name = "Test 2 display name")]
            Test2,
        };
    }
}
