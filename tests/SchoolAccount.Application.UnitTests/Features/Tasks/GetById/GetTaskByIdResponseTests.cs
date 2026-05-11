using AwesomeAssertions;
using SchoolAccount.Application.Features.Tasks.GetById;
using Xunit;

namespace SchoolAccount.Application.UnitTests.Features.Tasks.GetById;

public class GetTaskByIdResponseTests
{
    [Fact]
    public void Current_subtasks_are_upcoming_subtasks_when_view_mode_is_upcoming()
    {
        // Arrange
        var sut = new GetTaskByIdResponse
        {
            ViewMode = TaskViewMode.UpcomingTasks,
            UpcomingSubTasks = [new GetTaskByIdResponseSubTask { Id = 1, Name = "SubTask 1" }],
            PreviousSubTasks = [new GetTaskByIdResponseSubTask { Id = 2, Name = "SubTask 2" }],
        };

        // Act & Assert
        sut.CurrentSubTasks.Should().BeEquivalentTo(sut.UpcomingSubTasks);
    }

    [Fact]
    public void Current_subtasks_are_previous_subtasks_when_view_mode_is_previous()
    {
        // Arrange
        var sut = new GetTaskByIdResponse
        {
            ViewMode = TaskViewMode.PreviousTasks,
            UpcomingSubTasks = [new GetTaskByIdResponseSubTask { Id = 1, Name = "SubTask 1" }],
            PreviousSubTasks = [new GetTaskByIdResponseSubTask { Id = 2, Name = "SubTask 2" }],
        };

        // Act & Assert
        sut.CurrentSubTasks.Should().BeEquivalentTo(sut.PreviousSubTasks);
    }

    [Fact]
    public void Heading_text_is_upcoming_tasks_when_view_mode_is_upcoming()
    {
        // Arrange
        var sut = new GetTaskByIdResponse { ViewMode = TaskViewMode.UpcomingTasks };

        // Act & Assert
        sut.HeadingText.Should().Be("Upcoming Tasks");
    }

    [Fact]
    public void Heading_text_is_previous_12_months_when_view_mode_is_previous()
    {
        // Arrange
        var sut = new GetTaskByIdResponse { ViewMode = TaskViewMode.PreviousTasks };

        // Act & Assert
        sut.HeadingText.Should().Be("Previous 12 months");
    }

    [Fact]
    public void No_tasks_found_message_is_upcoming_when_view_mode_is_upcoming()
    {
        var sut = new GetTaskByIdResponse { ViewMode = TaskViewMode.UpcomingTasks };

        sut.NoTasksFoundMessage.Should().Be("There are no upcoming tasks");
    }

    [Fact]
    public void No_tasks_found_message_is_previous_when_view_mode_is_previous()
    {
        var sut = new GetTaskByIdResponse { ViewMode = TaskViewMode.PreviousTasks };

        sut.NoTasksFoundMessage.Should().Be("There are no previous tasks");
    }

    [Fact]
    public void Is_upcoming_tasks_view_is_true_when_view_mode_is_upcoming()
    {
        // Arrange
        var sut = new GetTaskByIdResponse { ViewMode = TaskViewMode.UpcomingTasks };

        // Act & Assert
        sut.IsUpcomingTasksView.Should().BeTrue();
    }

    [Fact]
    public void Is_upcoming_tasks_view_is_false_when_view_mode_is_previous()
    {
        // Arrange
        var sut = new GetTaskByIdResponse { ViewMode = TaskViewMode.PreviousTasks };

        // Act & Assert
        sut.IsUpcomingTasksView.Should().BeFalse();
    }

    [Fact]
    public void Is_previous_tasks_view_is_true_when_view_mode_is_previous()
    {
        // Arrange
        var sut = new GetTaskByIdResponse { ViewMode = TaskViewMode.PreviousTasks };

        // Act & Assert
        sut.IsPreviousTasksView.Should().BeTrue();
    }

    [Fact]
    public void Is_previous_tasks_view_is_false_when_view_mode_is_upcoming()
    {
        // Arrange
        var sut = new GetTaskByIdResponse { ViewMode = TaskViewMode.UpcomingTasks };

        // Act & Assert
        sut.IsPreviousTasksView.Should().BeFalse();
    }
}
