using AwesomeAssertions;
using SchoolAccount.Tests.Common.Builders;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.SubTaskBuilder;

// ReSharper disable ClassNeverInstantiated.Global

namespace SchoolAccount.Domain.UnitTests.Tasks;

public sealed partial class TaskEntityTests
{
    public sealed class SubTaskLastUpdated
    {
        [Fact]
        public void Task_with_no_subtasks_has_no_last_updated_date()
        {
            // Arrange
            var task = TaskBuilder.ATask().Build();

            // Act & Assert
            task.SubTaskLastUpdated.Should().BeNull();
        }

        [Fact]
        public void Task_with_one_subtask_returns_that_subtasks_updated_date()
        {
            // Arrange
            var updated = new DateTime(2024, 6, 1, 12, 0, 0, DateTimeKind.Utc);
            var task = TaskBuilder.ATask().WithSubTask(ASubTask().UpdatedAt(updated)).Build();

            // Act & Assert
            task.SubTaskLastUpdated.Should().Be(updated);
        }

        [Fact]
        public void Task_with_multiple_subtasks_returns_the_most_recently_updated_date()
        {
            // Arrange
            var oldest = ASubTask().UpdatedAt(2024, 1, 1);
            var newest = ASubTask().UpdatedAt(2024, 12, 1);
            var task = TaskBuilder.ATask().WithSubTasks(oldest, newest).Build();

            // Act & Assert
            task.SubTaskLastUpdated.Should().Be(newest.Build().DateUpdated);
        }
    }
}
