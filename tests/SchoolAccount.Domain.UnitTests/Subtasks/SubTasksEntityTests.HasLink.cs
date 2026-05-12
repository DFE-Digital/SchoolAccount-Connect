using AwesomeAssertions;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.SubTaskBuilder;

// ReSharper disable ClassNeverInstantiated.Global

namespace SchoolAccount.Domain.UnitTests.Subtasks;

public sealed partial class SubTaskEntityTests
{
    public sealed class HasLink
    {
        [Fact]
        public void Returns_true_when_digital_link_is_provided()
        {
            // Arrange
            var subTask = ASubTask().WithDigitalLink("https://example.com").Build();

            // Act
            var result = subTask.HasLink;

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Returns_false_when_digital_link_is_missing_or_whitespace(string? link)
        {
            // Arrange
            var subTask = ASubTask().WithDigitalLink(link).Build();

            // Act
            var result = subTask.HasLink;

            // Assert
            result.Should().BeFalse();
        }
    }
}
