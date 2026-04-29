using AwesomeAssertions;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.SubTaskBuilder;

// ReSharper disable ClassNeverInstantiated.Global

namespace SchoolAccount.Domain.Tests.Unit.Subtasks;

public sealed partial class SubTaskEntityTests
{
    public sealed class GenerateDueDateLabel
    {
        [Fact]
        public void Due_date_label_is_empty_when_due_date_is_missing()
        {
            // Arrange
            var subTask = ASubTask().Build();
            subTask.DueDate = null;
            subTask.DueDateIsExact = true;

            // Act
            var result = subTask.GenerateDueDateLabel();

            // Assert
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void Due_date_label_is_empty_when_exact_flag_is_missing()
        {
            // Arrange
            var subTask = ASubTask().WithDueDate(2026, 3, 15).Build();
            subTask.DueDateIsExact = null;

            // Act
            var result = subTask.GenerateDueDateLabel();

            // Assert
            result.Should().Be(string.Empty);
        }

        [Theory]
        [InlineData(true, "Due 15 Mar 2026.")]
        [InlineData(false, "Due Mar 2026.")]
        public void Due_date_label_shows_full_date_when_exact_or_month_when_approximate(
            bool isExact,
            string expectedLabel
        )
        {
            // Arrange
            var subTask = ASubTask().WithDueDate(2026, 3, 15).Build();
            subTask.DueDateIsExact = isExact;

            // Act
            var result = subTask.GenerateDueDateLabel();

            // Assert
            result.Should().Be(expectedLabel);
        }
    }
}
