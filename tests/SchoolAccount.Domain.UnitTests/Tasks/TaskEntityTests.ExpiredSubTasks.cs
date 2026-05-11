using AwesomeAssertions;
using SchoolAccount.Tests.Common.Builders;
using Xunit;
using static SchoolAccount.Domain.Common.WorkflowState;
using static SchoolAccount.Tests.Common.Builders.SubTaskBuilder;

// ReSharper disable ClassNeverInstantiated.Global

namespace SchoolAccount.Domain.UnitTests.Tasks;

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
        public void Only_expired_subtasks_are_included()
        {
            // Arrange
            var task = TaskBuilder
                .ATask()
                .WithSubTasks(
                    ASubTask().InState(Expired).WithStartDate(2024, 1, 1).WithDueDate(2024, 12, 31),
                    ASubTask().InState(Expired),
                    ASubTask().InState(Published).WithStartDate(2024, 1, 1).WithDueDate(2024, 12, 31),
                    ASubTask().InState(Draft).WithStartDate(2024, 1, 1).WithDueDate(2024, 12, 31)
                )
                .Build();

            // Act & Assert
            task.ExpiredSubTasks.Should().HaveCount(2).And.AllSatisfy(st => st.WorkflowState.Should().Be(Expired));
        }
    }
}
