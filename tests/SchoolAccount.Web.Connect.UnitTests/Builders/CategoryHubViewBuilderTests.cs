using System.Collections.ObjectModel;
using AwesomeAssertions;
using NSubstitute;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Category.Models;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.AuthenticationTests.Helpers;
using SchoolAccount.InfrastructureTests.Extensions;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders.CalendarOfItems;
using SchoolAccount.Web.Connect.Builders.Categories;
using X.PagedList;
using Xunit;

namespace SchoolAccount.Web.Connect.UnitTests.Builders;

public class CategoryHubViewBuilderTests
{
    [Fact]
    public void Returns_an_empty_items_collection_when_there_are_no_calendar_items()
    {
        // Arrange
        var organisationContext = Substitute.For<IOrganisationContext>();
        var emptyPagedList = new StaticPagedList<CalendarOfItemsRow>(new List<CalendarOfItemsRow>(), 1, 10, 0);
        var filters = new Collection<Filterable>();
        var calendarViewBuilder = new CalendarOfItemsViewBuilder(organisationContext);

        var categoryHubViewBuilder = new CategoryHubViewBuilder(calendarViewBuilder);
        var currentUri = new Uri("https://localhost:7033/categories/all-tasks");

        var items = new QueryPagedResult(new GenericQueryCriteria(), emptyPagedList, filters);

        // Act
        var viewModel = categoryHubViewBuilder.Build(items, currentUri);

        // Assert
        viewModel.Items.Should().BeEmpty();
    }

    [Fact]
    public void Displays_all_tasks_headings_and_text_when_no_category_is_provided()
    {
        // Arrange
        var schoolName = "Test School";
        var organisationContext = OrganisationContextHelper.CreateSimpleOrganisationContext(schoolName);

        var emptyPagedList = new StaticPagedList<CalendarOfItemsRow>(new List<CalendarOfItemsRow>(), 1, 10, 0);
        var filters = new Collection<Filterable>();
        var calendarViewBuilder = new CalendarOfItemsViewBuilder(organisationContext);

        var categoryHubViewBuilder = new CategoryHubViewBuilder(calendarViewBuilder);
        var currentUri = new Uri("https://localhost:7033/categories/all-tasks");

        var items = new QueryPagedResult(new GenericQueryCriteria(), emptyPagedList, filters);

        // Act
        var viewModel = categoryHubViewBuilder.Build(items, currentUri);

        // Assert
        viewModel.ViewModes.Should().Be(CalendarOfItemsViewModes.Custom);
        viewModel.Caption.Should().Be("Test School");
        viewModel.Heading.Should().Be("All tasks");
        viewModel.SubHeading.Should().Be("See all your tasks, returns and policies from DfE.");
        viewModel.Description.Should().Be("Explore all tasks and support");
        viewModel.NoResultsMessage.Should().Be("No results found");
    }

    [Fact]
    public void Displays_category_specific_headings_and_text_when_category_is_provided()
    {
        // Arrange
        var schoolName = "Test School";
        var organisationContext = OrganisationContextHelper.CreateSimpleOrganisationContext(schoolName);

        var emptyPagedList = new StaticPagedList<CalendarOfItemsRow>(new List<CalendarOfItemsRow>(), 1, 10, 0);
        var filters = new Collection<Filterable>();
        var calendarViewBuilder = new CalendarOfItemsViewBuilder(organisationContext);

        var categoryHubViewBuilder = new CategoryHubViewBuilder(calendarViewBuilder);
        var currentUri = new Uri("https://localhost:7033/categories/1");

        var items = new QueryPagedResult(new GenericQueryCriteria(), emptyPagedList, filters);

        var category = new CategoryType
        {
            Id = 1,
            Name = "NewCategory",
            DisplayName = "New Category",
            Description = "Description",
            HubViewDescription = "HubViewDescription",
        };

        // Act
        var viewModel = categoryHubViewBuilder.Build(items, currentUri, category);

        // Assert
        viewModel.ViewModes.Should().Be(CalendarOfItemsViewModes.Custom);
        viewModel.Caption.Should().Be("Test School");
        viewModel.Heading.Should().Be(category.DisplayName);
        viewModel.SubHeading.Should().Be(category.HubViewDescription);
        viewModel.Description.Should().Be("Explore all tasks and support");
        viewModel.NoResultsMessage.Should().Be("No results found");
    }

    [Fact]
    public void Pagination_shows_zero_items_when_the_list_is_empty()
    {
        // Arrange
        var organisationContext = Substitute.For<IOrganisationContext>();
        var filters = new Collection<Filterable>();
        var calendarViewBuilder = new CalendarOfItemsViewBuilder(organisationContext);
        var categoryHubViewBuilder = new CategoryHubViewBuilder(calendarViewBuilder);
        var currentUri = new Uri("https://localhost:7033/categories/1");

        var emptyPagedList = new StaticPagedList<CalendarOfItemsRow>(new List<CalendarOfItemsRow>(), 1, 10, 0);

        var items = new QueryPagedResult(new GenericQueryCriteria(), emptyPagedList, filters);

        // Act
        var viewModel = categoryHubViewBuilder.Build(items, currentUri);

        // Assert
        viewModel.ViewModes.Should().Be(CalendarOfItemsViewModes.Custom);
        viewModel.ViewModes.Should().NotHaveFlag(CalendarOfItemsViewModes.Standalone);
        viewModel.Pagination.Should().NotBeNull();
        viewModel.Pagination.TotalItemCount.Should().Be(0);
    }

    [Fact]
    public void Pagination_shows_the_correct_total_count_when_there_are_multiple_items()
    {
        // Arrange
        var organisationContext = Substitute.For<IOrganisationContext>();
        var filters = new Collection<Filterable>();
        var calendarViewBuilder = new CalendarOfItemsViewBuilder(organisationContext);
        var categoryHubViewBuilder = new CategoryHubViewBuilder(calendarViewBuilder);
        var currentUri = new Uri("https://localhost:7033/categories/1");

        var tasks = new StaticPagedList<CalendarOfItemsRow>(
            new List<CalendarOfItemsRow>()
            {
                CalendarOfItemsRowExtensions.Create(1, "Task 1", null, CalendarOfItemsRowType.Task),
                CalendarOfItemsRowExtensions.Create(2, "Task 2", null, CalendarOfItemsRowType.Task),
                CalendarOfItemsRowExtensions.Create(3, "Task 3", null, CalendarOfItemsRowType.Task),
            },
            1,
            10,
            3
        );

        var items = new QueryPagedResult(new GenericQueryCriteria(), tasks, filters);

        // Act
        var viewModel = categoryHubViewBuilder.Build(items, currentUri);

        // Assert
        viewModel.ViewModes.Should().Be(CalendarOfItemsViewModes.Custom);
        viewModel.ViewModes.Should().NotHaveFlag(CalendarOfItemsViewModes.Standalone);
        viewModel.Pagination.Should().NotBeNull();
        viewModel.Pagination.TotalItemCount.Should().Be(3);
    }
}
