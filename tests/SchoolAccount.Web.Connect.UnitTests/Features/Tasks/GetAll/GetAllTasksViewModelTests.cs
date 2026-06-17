using AwesomeAssertions;
using SchoolAccount.Domain.Common;
using SchoolAccount.Web.Connect.Features.Tasks.GetAll;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.Tasks.GetAll.GetAllTasksResponseBuilder;
using static SchoolAccount.Tests.Common.Builders.Tasks.GetAll.GetAllTasksResponseTasksBuilder;

namespace SchoolAccount.Web.Connect.UnitTests.Features.Tasks.GetAll;

public static class GetAllTasksViewModelTests
{
    public class Tasks
    {
        [Fact]
        public void Returns_empty_collection_when_response_has_no_tasks()
        {
            // Arrange
            var sut = GetAllTasksViewModelBuilder.Build(AResponse().Build());

            // Act & Assert
            sut.Tasks.PaginatedItems.Should().BeEmpty();
        }

        [Fact]
        public void Returns_tasks_mapped_to_list_item_view_models()
        {
            // Arrange
            var task1 = AResponseTask().WithId(1).WithName("Task One").Build();
            var task2 = AResponseTask().WithId(2).WithName("Task Two").Build();
            var sut = GetAllTasksViewModelBuilder.Build(AResponse().WithTasks(task1, task2).Build());

            // Act & Assert
            sut.Tasks.PaginatedItems.Should().HaveCount(2);
            sut.Tasks.PaginatedItems.Should().ContainEquivalentOf(new { Name = "Task One", Url = "/tasks/1" });
            sut.Tasks.PaginatedItems.Should().ContainEquivalentOf(new { Name = "Task Two", Url = "/tasks/2" });
        }

        [Fact]
        public void Returns_single_task_when_response_has_one_task()
        {
            // Arrange
            var task = AResponseTask().WithId(42).WithName("Statutory Accounts").Build();
            var sut = GetAllTasksViewModelBuilder.Build(AResponse().WithTasks(task).Build());

            // Act & Assert
            sut.Tasks.PaginatedItems.Should().ContainSingle().Which.Name.Should().Be("Statutory Accounts");
        }

        [Fact]
        public void Link_items_mapped_to_tasks_use_the_correct_description_or_requirement_when_available()
        {
            // Arrange
            var task1 = AResponseTask()
                .WithId(1)
                .WithName("Task that has both a description and a requirement")
                .WithDescription("New task description")
                .WithRequirement(Requirement.Mandatory)
                .Build();

            var task2 = AResponseTask()
                .WithId(2)
                .WithName("Task that has a description")
                .WithDescription("Another new task description")
                .Build();

            var task3 = AResponseTask()
                .WithId(3)
                .WithName("Task that has a requirement")
                .WithRequirement(Requirement.Optional)
                .Build();

            var task4 = AResponseTask()
                .WithId(4)
                .WithName("Task that has neither a description nor a requirement")
                .Build();

            var sut = GetAllTasksViewModelBuilder.Build(AResponse().WithTasks(task1, task2, task3, task4).Build());

            // Act & Assert
            sut.Tasks.PaginatedItems.Should().HaveCount(4);
            sut.Tasks.PaginatedItems.Should()
                .ContainEquivalentOf(
                    new
                    {
                        Name = "Task that has both a description and a requirement",
                        Url = "/tasks/1",
                        Description = "New task description",
                        OpenInNewTab = false,
                    }
                );
            sut.Tasks.PaginatedItems.Should()
                .ContainEquivalentOf(
                    new
                    {
                        Name = "Task that has a description",
                        Url = "/tasks/2",
                        Description = "Another new task description",
                        OpenInNewTab = false,
                    }
                );
            sut.Tasks.PaginatedItems.Should()
                .ContainEquivalentOf(
                    new
                    {
                        Name = "Task that has a requirement",
                        Url = "/tasks/3",
                        Description = "Optional task",
                        OpenInNewTab = false,
                    }
                );
            sut.Tasks.PaginatedItems.Should()
                .ContainEquivalentOf(
                    new
                    {
                        Name = "Task that has neither a description nor a requirement",
                        Url = "/tasks/4",
                        OpenInNewTab = false,
                    }
                );
        }
    }

    public class NoTasksFoundMessage
    {
        [Fact]
        public void Returns_message_containing_category_name()
        {
            // Arrange
            var sut = GetAllTasksViewModelBuilder.Build(AResponse().Build());

            // Act & Assert
            sut.Tasks.NoResultsMessage.Should().Be("No tasks found.");
        }
    }
}
