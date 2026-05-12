using AwesomeAssertions;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.GetTaskByIdResponseBuilder;
using static SchoolAccount.Tests.Common.Builders.GetTaskByIdResponseSubtaskBuilder;

namespace SchoolAccount.Application.UnitTests.Features.Tasks.GetById;

public static class GetTaskByIdResponseTests
{
    public class SubtaskLastUpdated
    {
        [Fact]
        public void Task_with_no_subtasks_has_no_last_updated_date()
        {
            // Arrange
            var sut = AResponse().Build();

            // Act & Assert
            sut.SubTaskLastUpdated.Should().BeNull();
        }

        [Fact]
        public void Task_with_one_subtask_returns_that_subtasks_updated_date()
        {
            // Arrange
            var updated = new DateTime(2024, 6, 1, 12, 0, 0, DateTimeKind.Utc);

            var sut = AResponse().WithSubtasks(ASubtask().WithDateUpdated(updated).Build()).Build();

            // Act & Assert
            sut.SubTaskLastUpdated.Should().Be(updated);
        }

        [Fact]
        public void Task_with_multiple_subtasks_returns_the_most_recently_updated_date()
        {
            // Arrange
            var oldest = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            var newest = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);

            var sut = AResponse()
                .WithSubtasks(ASubtask().WithDateUpdated(oldest).Build(), ASubtask().WithDateUpdated(newest).Build())
                .Build();

            // Act & Assert
            sut.SubTaskLastUpdated.Should().Be(newest);
        }
    }
}
