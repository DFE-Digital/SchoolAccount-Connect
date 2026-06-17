using AwesomeAssertions;
using SchoolAccount.Web.Connect.Features.Tasks.Search;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.Tasks.Search.SearchTasksResponseBuilder;
using static SchoolAccount.Tests.Common.Builders.Tasks.Search.SearchTasksResponseTaskBuilder;

namespace SchoolAccount.Web.Connect.UnitTests.Features.Tasks.Search;

public static class SearchTasksViewModelBuilderTests
{
    public class SearchResults
    {
        [Fact]
        public void Returns_empty_collection_when_response_has_no_search_results()
        {
            // Arrange
            var response = AResponse();

            // Act
            var viewModel = SearchTasksViewModelBuilder.Build(response, string.Empty);

            // Assert
            viewModel.Tasks.PaginatedItems.Should().BeEmpty();
        }

        [Fact]
        public void Returns_search_results_mapped_to_list_item_view_models()
        {
            // Arrange
            var task1 = AResponseTask()
                .WithId(1)
                .WithName("Statutory Accounts")
                .WithDescription("File statutory accounts");

            var task2 = AResponseTask()
                .WithId(2)
                .WithName("Corporation Tax")
                .WithDescription("File corporation tax return");

            var response = AResponse().WithTasks(task1, task2);

            // Act
            var viewModel = SearchTasksViewModelBuilder.Build(response, string.Empty);

            // Assert
            viewModel
                .Tasks.PaginatedItems.Should()
                .BeEquivalentTo([
                    new
                    {
                        Name = "Statutory Accounts",
                        Url = "/tasks/1",
                        Description = "File statutory accounts",
                        OpenInNewTab = false,
                    },
                    new
                    {
                        Name = "Corporation Tax",
                        Url = "/tasks/2",
                        Description = "File corporation tax return",
                        OpenInNewTab = false,
                    },
                ]);
        }

        [Fact]
        public void Returns_single_search_result_when_response_has_one_task()
        {
            // Arrange
            var task = AResponseTask().WithId(1).WithName("Statutory Accounts");
            var response = AResponse().WithTasks(task);

            // Act
            var viewModel = SearchTasksViewModelBuilder.Build(response, string.Empty);

            // Act
            viewModel.Tasks.PaginatedItems.Should().ContainSingle().Which.Name.Should().Be("Statutory Accounts");
        }

        [Fact]
        public void Description_defaults_to_empty_string_when_task_has_no_description()
        {
            // Arrange
            var task = AResponseTask().WithId(1).WithName("Statutory Accounts");
            var response = AResponse().WithTasks(task);

            // Act
            var viewModel = SearchTasksViewModelBuilder.Build(response, string.Empty);

            // Act & Assert
            viewModel.Tasks.PaginatedItems.Should().ContainSingle().Which.Description.Should().Be(null);
        }
    }

    public class NoItemsFoundMessage
    {
        [Fact]
        public void Returns_expected_message()
        {
            // Act
            var viewModel = SearchTasksViewModelBuilder.Build(AResponse(), string.Empty);

            // Act & Assert
            viewModel.Tasks.NoResultsMessage.Should().Be("No tasks found.");
        }
    }

    public class Heading
    {
        [Fact]
        public void Returns_search_results()
        {
            // Act
            var viewModel = SearchTasksViewModelBuilder.Build(AResponse(), string.Empty);

            // Act & Assert
            viewModel.Heading.Should().Be("Search results");
        }
    }

    public class Description
    {
        [Fact]
        public void Returns_generic_message_when_term_is_null()
        {
            // Act
            var viewModel = SearchTasksViewModelBuilder.Build(AResponse(), string.Empty);

            // Act & Assert
            viewModel.Description.Should().Be("Showing matching tasks.");
        }

        [Fact]
        public void Returns_generic_message_when_term_is_whitespace()
        {
            // Act
            var viewModel = SearchTasksViewModelBuilder.Build(AResponse(), "    ");

            // Act & Assert
            viewModel.Description.Should().Be("Showing matching tasks.");
        }

        [Fact]
        public void Returns_message_containing_term_when_term_is_provided()
        {
            // Act
            var viewModel = SearchTasksViewModelBuilder.Build(AResponse(), "statutory accounts");

            // Act & Assert
            viewModel.Description.Should().Be("Showing results for \u201cstatutory accounts\u201d.");
        }
    }

    public class SubHeading
    {
        [Fact]
        public void Returns_no_items_found_message_when_total_item_count_is_zero()
        {
            // Act
            var viewModel = SearchTasksViewModelBuilder.Build(AResponse(), string.Empty);

            // Act & Assert
            viewModel.SubHeading.Should().Be("No tasks found.");
        }

        [Fact]
        public void Returns_singular_task_found_when_total_item_count_is_one()
        {
            // Arrange
            var task = AResponseTask().WithId(1).WithName("Statutory Accounts").Build();
            var response = AResponse().WithTasks(task);

            // Act
            var viewModel = SearchTasksViewModelBuilder.Build(response, string.Empty);

            // Act & Assert
            viewModel.SubHeading.Should().Be("1 task found.");
        }

        [Fact]
        public void Returns_plural_tasks_found_when_total_item_count_is_greater_than_one()
        {
            // Arrange
            var tasks = Enumerable
                .Range(1, 10)
                .Select(i => AResponseTask().WithId(i).WithName($"Category {i}").Build())
                .ToArray();

            var response = AResponse().WithTasks(tasks);

            // Act
            var viewModel = SearchTasksViewModelBuilder.Build(response, string.Empty);

            // Act & Assert
            viewModel.SubHeading.Should().Be("10 tasks found.");
        }
    }
}
