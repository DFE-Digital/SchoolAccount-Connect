using AwesomeAssertions;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.SubTaskBuilder;

// ReSharper disable ClassNeverInstantiated.Global

namespace SchoolAccount.Domain.Tests.Unit.Subtasks;

public sealed partial class SubTaskEntityTests
{
    public sealed class HasDescription
    {
        [Fact]
        public void Returns_true_when_description_is_provided()
        {
            // Arrange
            var subTask = ASubTask().WithDescription("This is a description").Build();

            // Act
            var result = subTask.HasDescription;

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Returns_false_when_description_is_missing_or_whitespace(string? description)
        {
            // Arrange
            var subTask = ASubTask().WithDescription(description).Build();

            // Act
            var result = subTask.HasDescription;

            // Assert
            result.Should().BeFalse();
        }
    }
}
