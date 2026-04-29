using AwesomeAssertions;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.SubTaskBuilder;

// ReSharper disable ClassNeverInstantiated.Global

namespace SchoolAccount.Domain.Tests.Unit.Subtasks;

public sealed partial class SubTaskEntityTests
{
    public sealed class SortingDate
    {
        [Fact]
        public void Prioritises_due_date_when_available()
        {
            // Arrange
            var subTask = ASubTask().WithStartDate(2026, 3, 1).WithDueDate(2026, 3, 15).Build();

            // Act
            var result = subTask.SortingDate;

            // Assert
            result.Should().Be(new DateOnly(2026, 3, 15));
        }

        [Fact]
        public void Falls_back_to_start_date_when_due_date_unavailable()
        {
            // Arrange
            var subTask = ASubTask().WithStartDate(2026, 3, 1).Build();

            // Act
            var result = subTask.SortingDate;

            // Assert
            result.Should().Be(new DateOnly(2026, 3, 1));
        }

        [Fact]
        public void Returns_null_when_no_dates_exist()
        {
            // Arrange
            var subTask = ASubTask().Build();
            subTask.StartDate = null;
            subTask.DueDate = null;

            // Act
            var result = subTask.SortingDate;

            // Assert
            result.Should().BeNull();
        }
    }
}
