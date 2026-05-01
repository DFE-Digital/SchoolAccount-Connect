using AwesomeAssertions;
using SchoolAccount.Tests.Common.Builders;
using Xunit;
using static SchoolAccount.Domain.Common.WorkflowState;
using static SchoolAccount.Tests.Common.Builders.SubTaskBuilder;

// ReSharper disable once ClassNeverInstantiated.Global

namespace SchoolAccount.Domain.Tests.Unit.Tasks;

public sealed partial class TaskEntityTests
{
    public sealed class PublishedSubTasks
    {
        [Fact]
        public void Task_with_no_subtasks_has_no_published_subtasks()
        {
            // Arrange
            var task = TaskBuilder.ATask().Build();

            // Act & Assert
            task.PublishedSubTasks.Should().BeEmpty();
        }

        [Fact]
        public void Only_published_subtasks_are_included()
        {
            // Arrange
            var task = TaskBuilder
                .ATask()
                .WithSubTasks(
                    ASubTask().InState(Published).WithStartDate(2024, 1, 1).WithDueDate(2024, 12, 31),
                    ASubTask().InState(Published),
                    ASubTask().InState(Expired).WithStartDate(2024, 1, 1).WithDueDate(2024, 12, 31),
                    ASubTask().InState(Draft).WithStartDate(2024, 1, 1).WithDueDate(2024, 12, 31)
                )
                .Build();

            // Act & Assert
            task.PublishedSubTasks.Should().HaveCount(2).And.AllSatisfy(st => st.WorkflowState.Should().Be(Published));
        }
    }
}
