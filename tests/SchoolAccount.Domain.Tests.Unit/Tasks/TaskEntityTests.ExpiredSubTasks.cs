using AwesomeAssertions;
using SchoolAccount.Tests.Common.Builders;
using Xunit;
using static SchoolAccount.Domain.Common.WorkflowState;
using static SchoolAccount.Tests.Common.Builders.SubTaskBuilder;

// ReSharper disable ClassNeverInstantiated.Global

namespace SchoolAccount.Domain.Tests.Unit.Tasks;

public sealed partial class TaskEntityTests
{
    public sealed class ExpiredSubTasks
    {
        [Fact]
        public void Task_with_no_subtasks_has_no_expired_subtasks()
        {
            // Arrange
            var task = TaskBuilder.ATask().Build();

            // Act & Assert
            task.ExpiredSubTasks.Should().BeEmpty();
        }

        [Fact]
        public void Expired_subtask_without_start_and_due_date_is_not_included()
        {
            // Arrange
            var task = TaskBuilder.ATask().WithSubTask(ASubTask().InState(Expired)).Build();

            // Act & Assert
            task.ExpiredSubTasks.Should().BeEmpty();
        }

        [Fact]
        public void Only_expired_subtasks_with_start_and_due_date_are_included()
        {
            // Arrange
            var expiredWithDates = ASubTask().InState(Expired).WithStartDate(2024, 1, 1).WithDueDate(2024, 12, 31);

            var task = TaskBuilder
                .ATask()
                .WithSubTasks(
                    expiredWithDates,
                    ASubTask().InState(Expired),
                    ASubTask().InState(Published).WithStartDate(2024, 1, 1).WithDueDate(2024, 12, 31),
                    ASubTask().InState(Draft).WithStartDate(2024, 1, 1).WithDueDate(2024, 12, 31)
                )
                .Build();

            // Act & Assert
            task.ExpiredSubTasks.Should().ContainSingle().Which.WorkflowState.Should().Be(Expired);
        }
    }
}
