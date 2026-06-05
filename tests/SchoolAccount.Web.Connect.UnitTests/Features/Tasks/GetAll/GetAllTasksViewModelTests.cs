using AwesomeAssertions;
using SchoolAccount.Domain.Common;
using SchoolAccount.Web.Connect.Features.Tasks.GetAll;
using SchoolAccount.Web.Connect.Models.Shared;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.Tasks.GetAllTasksResponseBuilder;
using static SchoolAccount.Tests.Common.Builders.Tasks.GetAllTasksResponseTasksBuilder;

namespace SchoolAccount.Web.Connect.UnitTests.Features.Tasks.GetAll;

public static class GetAllTasksViewModelTests
{
    private static PaginationViewModel EmptyPagination() => new(showPagination: false);

    public class Tasks
    {
        [Fact]
        public void Returns_empty_collection_when_response_has_no_tasks()
        {
            // Arrange
            var sut = new GetAllTasksViewModel(AResponse().Build(), EmptyPagination());

            // Act & Assert
            sut.Tasks.Should().BeEmpty();
        }

        [Fact]
        public void Returns_tasks_mapped_to_list_item_view_models()
        {
            // Arrange
            var task1 = AResponseTask().WithId(1).WithName("Task One").Build();
            var task2 = AResponseTask().WithId(2).WithName("Task Two").Build();
            var sut = new GetAllTasksViewModel(AResponse().WithTasks(task1, task2).Build(), EmptyPagination());

            // Act & Assert
            sut.Tasks.Should().HaveCount(2);
            sut.Tasks.Should().ContainEquivalentOf(new { Name = "Task One", Url = "/task/1" });
            sut.Tasks.Should().ContainEquivalentOf(new { Name = "Task Two", Url = "/task/2" });
        }

        [Fact]
        public void Returns_single_task_when_response_has_one_task()
        {
            // Arrange
            var task = AResponseTask().WithId(42).WithName("Statutory Accounts").Build();
            var sut = new GetAllTasksViewModel(AResponse().WithTasks(task).Build(), EmptyPagination());

            // Act & Assert
            sut.Tasks.Should().ContainSingle().Which.Name.Should().Be("Statutory Accounts");
        }

        [Fact]
        public void Maps_requirement_to_description()
        {
            // Arrange
            var task = AResponseTask().WithId(1).WithName("Task One").WithRequirement(Requirement.Mandatory).Build();
            var sut = new GetAllTasksViewModel(AResponse().WithTasks(task).Build(), EmptyPagination());

            // Act & Assert
            sut.Tasks.Should().ContainSingle().Which.Description.Should().Be("Mandatory");
        }
    }

    public class NoTasksFound
    {
        [Fact]
        public void Returns_true_when_tasks_collection_is_empty()
        {
            // Arrange
            var sut = new GetAllTasksViewModel(AResponse().Build(), EmptyPagination());

            // Act & Assert
            sut.NoTasksFound.Should().BeTrue();
        }

        [Fact]
        public void Returns_false_when_tasks_collection_has_items()
        {
            // Arrange
            var task = AResponseTask().WithId(1).WithName("Task One").Build();
            var sut = new GetAllTasksViewModel(AResponse().WithTasks(task).Build(), EmptyPagination());

            // Act & Assert
            sut.NoTasksFound.Should().BeFalse();
        }
    }

    public class NoTasksFoundMessage
    {
        [Fact]
        public void Returns_message_containing_category_name()
        {
            // Arrange
            var sut = new GetAllTasksViewModel(AResponse().Build(), EmptyPagination());

            // Act & Assert
            sut.NoTasksFoundMessage.Should().Be("No tasks found.");
        }
    }

    public class Pagination
    {
        [Fact]
        public void Exposes_pagination_passed_in_constructor()
        {
            // Arrange
            var pagination = new PaginationViewModel(showPagination: true);
            var sut = new GetAllTasksViewModel(AResponse().Build(), pagination);

            // Act & Assert
            sut.Pagination.Should().BeSameAs(pagination);
        }

        [Fact]
        public void Show_pagination_is_false_when_constructed_with_false()
        {
            // Arrange
            var pagination = new PaginationViewModel(showPagination: false);
            var sut = new GetAllTasksViewModel(AResponse().Build(), pagination);

            // Act & Assert
            sut.Pagination.ShowPagination.Should().BeFalse();
        }

        [Fact]
        public void Show_pagination_is_true_when_constructed_with_true()
        {
            // Arrange
            var pagination = new PaginationViewModel(showPagination: true);
            var sut = new GetAllTasksViewModel(AResponse().Build(), pagination);

            // Act & Assert
            sut.Pagination.ShowPagination.Should().BeTrue();
        }
    }
}
