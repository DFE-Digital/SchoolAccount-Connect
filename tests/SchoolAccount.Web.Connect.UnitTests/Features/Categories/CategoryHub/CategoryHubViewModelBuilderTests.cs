using AwesomeAssertions;
using SchoolAccount.Domain.Common;
using SchoolAccount.Web.Connect.Features.Categories.CategoryHub;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.Categories.CategoryHub.GetCategoryHubResponseBuilder;
using static SchoolAccount.Tests.Common.Builders.Categories.CategoryHub.GetCategoryHubResponseTaskBuilder;

namespace SchoolAccount.Web.Connect.UnitTests.Features.Categories.CategoryHub;

public static class CategoryHubViewModelBuilderTests
{
    public class Tasks
    {
        [Fact]
        public void Returns_empty_collection_when_response_has_no_tasks()
        {
            // Arrange
            var response = AResponse();

            // Act
            var viewModel = CategoryHubViewModelBuilder.Build(response);

            // Assert
            viewModel.Tasks.PaginatedItems.Should().BeEmpty();
        }

        [Fact]
        public void Returns_single_task_when_response_has_one_task()
        {
            // Arrange
            var task = AResponseTask().WithId(42).WithName("Statutory Accounts");
            var response = AResponse().WithTasks(task);

            // Act
            var viewModel = CategoryHubViewModelBuilder.Build(response);

            // Assert
            viewModel.Tasks.PaginatedItems.Should().ContainSingle().Which.Name.Should().Be("Statutory Accounts");
        }

        [Fact]
        public void Returns_tasks_mapped_to_list_item_view_models()
        {
            // Arrange
            var task1 = AResponseTask()
                .WithId(1)
                .WithName("Task One")
                .WithDescription("New task description")
                .WithRequirement(Requirement.Mandatory);

            var task2 = AResponseTask()
                .WithId(2)
                .WithName("Task Two")
                .WithDescription("Another task description")
                .WithRequirement(Requirement.Optional);

            var response = AResponse().WithTasks(task1, task2);

            // Act
            var viewModel = CategoryHubViewModelBuilder.Build(response);

            // Assert
            viewModel
                .Tasks.PaginatedItems.Should()
                .BeEquivalentTo([
                    new
                    {
                        Name = "Task One",
                        Url = "/tasks/1",
                        Description = "New task description",
                        OpenInNewTab = false,
                    },
                    new
                    {
                        Name = "Task Two",
                        Url = "/tasks/2",
                        Description = "Another task description",
                        OpenInNewTab = false,
                    },
                ]);
        }

        [Fact]
        public void Uses_correct_description_or_requirement_when_available()
        {
            // Arrange
            var task1 = AResponseTask()
                .WithId(1)
                .WithName("Task that has both a description and a requirement")
                .WithDescription("New task description")
                .WithRequirement(Requirement.Mandatory);

            var task2 = AResponseTask()
                .WithId(2)
                .WithName("Task that has a description")
                .WithDescription("Another new task description");

            var task3 = AResponseTask()
                .WithId(3)
                .WithName("Task that has a requirement")
                .WithRequirement(Requirement.Optional);

            var task4 = AResponseTask().WithId(4).WithName("Task that has neither a description nor a requirement");

            var response = AResponse().WithTasks(task1, task2, task3, task4);

            // Act
            var viewModel = CategoryHubViewModelBuilder.Build(response);

            // Assert
            viewModel.Tasks.PaginatedItems.TotalItemCount.Should().Be(4);
            viewModel
                .Tasks.PaginatedItems.Should()
                .BeEquivalentTo([
                    new
                    {
                        Name = "Task that has both a description and a requirement",
                        Url = "/tasks/1",
                        Description = "New task description",
                        OpenInNewTab = false,
                    },
                    new
                    {
                        Name = "Task that has a description",
                        Url = "/tasks/2",
                        Description = "Another new task description",
                        OpenInNewTab = false,
                    },
                    new
                    {
                        Name = "Task that has a requirement",
                        Url = "/tasks/3",
                        Description = "Optional task",
                        OpenInNewTab = false,
                    },
                    new
                    {
                        Name = "Task that has neither a description nor a requirement",
                        Url = "/tasks/4",
                        Description = "",
                        OpenInNewTab = false,
                    },
                ]);
        }

        [Fact]
        public void Handles_task_with_null_name_gracefully()
        {
            // Arrange
            var task = AResponseTask().WithId(1).WithName(null!);
            var response = AResponse().WithTasks(task);

            // Act
            var viewModel = CategoryHubViewModelBuilder.Build(response);

            // Assert
            viewModel.Tasks.PaginatedItems.Should().ContainSingle();
        }

        [Fact]
        public void Handles_task_with_empty_description()
        {
            // Arrange
            var task = AResponseTask().WithId(1).WithName("Task").WithDescription(string.Empty);
            var response = AResponse().WithTasks(task);

            // Act
            var viewModel = CategoryHubViewModelBuilder.Build(response);

            // Assert
            viewModel.Tasks.PaginatedItems.Should().ContainSingle().Which.Description.Should().BeEmpty();
        }
    }

    public class IsAcademyTrustHandbookCategory
    {
        [Fact]
        public void Returns_true_when_category_id_is_1()
        {
            // Arrange
            var response = AResponse().WithId(1);

            // Act
            var viewModel = CategoryHubViewModelBuilder.Build(response);

            // Assert
            viewModel.IsAcademyTrustHandbookCategory.Should().BeTrue();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(10)]
        [InlineData(999)]
        public void Returns_false_when_category_id_is_not_1(int categoryId)
        {
            // Arrange
            var response = AResponse().WithId(categoryId);

            // Act
            var viewModel = CategoryHubViewModelBuilder.Build(response);

            // Assert
            viewModel.IsAcademyTrustHandbookCategory.Should().BeFalse();
        }
    }

    public class Mapping
    {
        [Fact]
        public void Maps_category_id_correctly()
        {
            // Arrange
            var response = AResponse().WithId(42);

            // Act
            var viewModel = CategoryHubViewModelBuilder.Build(response);

            // Assert
            viewModel.CategoryId.Should().Be(42);
        }

        [Fact]
        public void Maps_category_display_name_correctly()
        {
            // Arrange
            var response = AResponse().WithDisplayName("Financial Management");

            // Act
            var viewModel = CategoryHubViewModelBuilder.Build(response);

            // Assert
            viewModel.Name.Should().Be("Financial Management");
        }

        [Fact]
        public void Maps_hub_view_description_when_provided()
        {
            // Arrange
            var response = AResponse().WithHubViewDescription("Important category description");

            // Act
            var viewModel = CategoryHubViewModelBuilder.Build(response);

            // Assert
            viewModel.HubViewDescription.Should().Be("Important category description");
        }

        [Fact]
        public void Hub_view_description_is_null_when_not_provided()
        {
            // Arrange
            var response = AResponse();

            // Act
            var viewModel = CategoryHubViewModelBuilder.Build(response);

            // Assert
            viewModel.HubViewDescription.Should().BeNull();
        }
    }
}
