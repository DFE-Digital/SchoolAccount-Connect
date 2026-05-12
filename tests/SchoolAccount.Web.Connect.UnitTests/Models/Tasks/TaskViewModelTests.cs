using AwesomeAssertions;
using SchoolAccount.Application.Features.Tasks.GetById;
using SchoolAccount.Web.Connect.Models.Tasks;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.GetTaskByIdResponseBuilder;
using static SchoolAccount.Tests.Common.Builders.GetTaskByIdResponseSubtaskBuilder;
using static SchoolAccount.Web.Connect.Models.Tasks.TaskViewMode;

// ReSharper disable RedundantArgumentDefaultValue

namespace SchoolAccount.Web.Connect.UnitTests.Models.Tasks;

public static class TaskViewModelTests
{
    public class HeadingText
    {
        [Fact]
        public void Returns_upcoming_tasks_when_view_mode_is_upcoming()
        {
            // Arrange
            var taskResponse = new GetTaskByIdResponse();
            var sut = new TaskViewModel(taskResponse, UpcomingTasks);

            // Act & Assert
            sut.HeadingText.Should().Be("Upcoming Tasks");
        }

        [Fact]
        public void Returns_previous_12_months_when_view_mode_is_previous()
        {
            // Arrange
            var taskResponse = new GetTaskByIdResponse();
            var sut = new TaskViewModel(taskResponse, PreviousTasks);

            // Act & Assert
            sut.HeadingText.Should().Be("Previous 12 months");
        }

        [Fact]
        public void Returns_upcoming_tasks_when_view_mode_is_not_supplied()
        {
            // Arrange
            var taskResponse = new GetTaskByIdResponse();
            var sut = new TaskViewModel(taskResponse);

            // Act & Assert
            sut.HeadingText.Should().Be("Upcoming Tasks");
        }
    }

    public class NoTasksFoundMessage
    {
        [Fact]
        public void Returns_no_upcoming_tasks_message_when_view_mode_is_upcoming()
        {
            // Arrange
            var taskResponse = new GetTaskByIdResponse();
            var sut = new TaskViewModel(taskResponse);

            // Act & Assert
            sut.NoTasksFoundMessage.Should().Be("There are no upcoming tasks");
        }

        [Fact]
        public void Returns_no_previous_tasks_message_when_view_mode_is_previous()
        {
            // Arrange
            var taskResponse = new GetTaskByIdResponse();
            var sut = new TaskViewModel(taskResponse, PreviousTasks);

            // Act & Assert
            sut.NoTasksFoundMessage.Should().Be("There are no previous tasks");
        }
    }

    public class Subtasks
    {
        [Fact]
        public void Returns_published_subtasks_when_view_mode_is_upcoming()
        {
            // Arrange
            var published = ASubtask().WithId(1).WithName("SubTask 1").Published().Build();
            var expired = ASubtask().WithId(2).WithName("SubTask 2").Expired().Build();
            var getTaskByIdResponse = AResponse().WithSubtasks(published, expired).Build();

            var sut = new TaskViewModel(getTaskByIdResponse, UpcomingTasks);

            // Act & Assert
            sut.SubTasks.Should().BeEquivalentTo([published]);
        }

        [Fact]
        public void Returns_expired_subtasks_when_view_mode_is_previous()
        {
            // Arrange
            var published = ASubtask().WithId(1).WithName("SubTask 1").Published().Build();
            var expired = ASubtask().WithId(2).WithName("SubTask 2").Expired().Build();
            var getTaskByIdResponse = AResponse().WithSubtasks(published, expired).Build();

            var sut = new TaskViewModel(getTaskByIdResponse, PreviousTasks);

            // Act & Assert
            sut.SubTasks.Should().BeEquivalentTo([expired]);
        }

        [Fact]
        public void Returns_published_subtasks_sorted_by_sorting_date_when_view_mode_is_upcoming()
        {
            // Arrange
            var laterSubtask = ASubtask().WithId(1).Published().WithDueDate(new DateOnly(2026, 6, 15)).Build();
            var earlierSubtask = ASubtask().WithId(2).Published().WithDueDate(new DateOnly(2026, 6, 1)).Build();
            var expiredSubtask = ASubtask().WithId(3).Expired().WithDueDate(new DateOnly(2026, 5, 1)).Build();
            var getTaskByIdResponse = AResponse().WithSubtasks(laterSubtask, earlierSubtask, expiredSubtask).Build();

            var sut = new TaskViewModel(getTaskByIdResponse, UpcomingTasks);

            // Act
            var result = sut.SubTasks;

            // Assert
            result.Should().HaveCount(2);
            result.ElementAt(0).Id.Should().Be(earlierSubtask.Id);
            result.ElementAt(1).Id.Should().Be(laterSubtask.Id);
        }

        [Fact]
        public void Returns_expired_subtasks_sorted_by_sorting_date_when_view_mode_is_previous()
        {
            // Arrange
            var earlierSubtask = ASubtask().WithId(1).Expired().WithDueDate(new DateOnly(2026, 3, 1)).Build();
            var laterSubtask = ASubtask().WithId(2).Expired().WithDueDate(new DateOnly(2026, 3, 15)).Build();
            var publishedSubtask = ASubtask().WithId(3).Published().WithDueDate(new DateOnly(2026, 6, 1)).Build();
            var getTaskByIdResponse = AResponse().WithSubtasks(laterSubtask, earlierSubtask, publishedSubtask).Build();

            var sut = new TaskViewModel(getTaskByIdResponse, PreviousTasks);

            // Act
            var result = sut.SubTasks;

            // Assert
            result.Should().HaveCount(2);
            result.ElementAt(0).Id.Should().Be(earlierSubtask.Id);
            result.ElementAt(1).Id.Should().Be(laterSubtask.Id);
        }

        [Fact]
        public void Sorts_by_start_date_when_due_date_is_null()
        {
            // Arrange
            var earlierSubtask = ASubtask().WithId(1).Published().WithStartDate(new DateOnly(2026, 6, 1)).Build();
            var laterSubtask = ASubtask().WithId(2).Published().WithStartDate(new DateOnly(2026, 6, 15)).Build();
            var getTaskByIdResponse = AResponse().WithSubtasks(earlierSubtask, laterSubtask).Build();

            var sut = new TaskViewModel(getTaskByIdResponse, UpcomingTasks);

            // Act
            var result = sut.SubTasks;

            // Assert
            result.ElementAt(0).Id.Should().Be(earlierSubtask.Id);
            result.ElementAt(1).Id.Should().Be(laterSubtask.Id);
        }

        [Fact]
        public void Places_subtasks_with_no_sorting_date_at_end()
        {
            // Arrange
            var withDate = ASubtask().WithId(1).Published().WithDueDate(new DateOnly(2026, 6, 15)).Build();
            var withoutDate = ASubtask().WithId(2).Published().Build();
            var getTaskByIdResponse = AResponse().WithSubtasks(withoutDate, withDate).Build();

            var sut = new TaskViewModel(getTaskByIdResponse, UpcomingTasks);

            // Act
            var result = sut.SubTasks;

            // Assert
            result.ElementAt(0).Id.Should().Be(withDate.Id);
            result.ElementAt(1).Id.Should().Be(withoutDate.Id);
        }

        [Fact]
        public void Returns_empty_array_when_no_subtasks_match_view_mode()
        {
            // Arrange
            var getTaskByIdResponse = AResponse()
                .WithSubtasks(ASubtask().Expired().Build(), ASubtask().Expired().Build())
                .Build();

            var sut = new TaskViewModel(getTaskByIdResponse, UpcomingTasks);

            // Act & Assert
            sut.SubTasks.Should().BeEmpty();
        }
    }

    public class IsUpcomingTasksView
    {
        [Fact]
        public void Returns_true_when_view_mode_is_upcoming()
        {
            // Arrange
            var getTaskByIdResponse = AResponse().Build();
            var sut = new TaskViewModel(getTaskByIdResponse, UpcomingTasks);

            // Act & Assert
            sut.IsUpcomingTasksView.Should().BeTrue();
        }

        [Fact]
        public void Returns_false_when_view_mode_is_previous()
        {
            // Arrange
            var getTaskByIdResponse = AResponse().Build();
            var sut = new TaskViewModel(getTaskByIdResponse, PreviousTasks);

            // Act & Assert
            sut.IsUpcomingTasksView.Should().BeFalse();
        }
    }

    public class IsPreviousTasksView
    {
        [Fact]
        public void Returns_true_when_view_mode_is_previous()
        {
            // Arrange
            var getTaskByIdResponse = AResponse().Build();
            var sut = new TaskViewModel(getTaskByIdResponse, PreviousTasks);

            // Act & Assert
            sut.IsPreviousTasksView.Should().BeTrue();
        }

        [Fact]
        public void Returns_false_when_view_mode_is_upcoming()
        {
            // Arrange
            var getTaskByIdResponse = AResponse().Build();
            var sut = new TaskViewModel(getTaskByIdResponse, UpcomingTasks);

            // Act & Assert
            sut.IsPreviousTasksView.Should().BeFalse();
        }
    }
}
