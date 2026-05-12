using AwesomeAssertions;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.SubTaskBuilder;

// ReSharper disable ClassNeverInstantiated.Global

namespace SchoolAccount.Domain.UnitTests.Subtasks;

public sealed partial class SubTaskEntityTests
{
    public sealed class HasStartAndDueDate
    {
        [Fact]
        public void Returns_true_when_both_dates_exist()
        {
            // Arrange
            var subTask = ASubTask().WithStartDate(2026, 3, 1).WithDueDate(2026, 3, 15).Build();

            // Act
            var result = subTask.HasStartAndDueDate();

            // Assert
            result.Should().BeTrue();
        }

        [Theory]
        [InlineData(true, false)] // has start date, missing due date
        [InlineData(false, true)] // missing start date, has due date
        [InlineData(false, false)] // both dates missing
        public void Returns_false_when_any_date_is_missing(bool hasStartDate, bool hasDueDate)
        {
            // Arrange
            var subTask = ASubTask().Build();
            subTask.StartDate = hasStartDate ? new DateOnly(2026, 3, 1) : null;
            subTask.DueDate = hasDueDate ? new DateOnly(2026, 3, 15) : null;

            // Act
            var result = subTask.HasStartAndDueDate();

            // Assert
            result.Should().BeFalse();
        }
    }
}
