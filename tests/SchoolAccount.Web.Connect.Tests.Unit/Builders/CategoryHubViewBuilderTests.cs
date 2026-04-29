using System.Collections.ObjectModel;
using AwesomeAssertions;
using NSubstitute;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Category.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.AuthenticationTests.Helpers;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders.CalendarOfItems;
using SchoolAccount.Web.Connect.Builders.Categories;
using X.PagedList;
using Xunit;

namespace SchoolAccount.Web.Connect.Tests.Unit.Builders;

public class CategoryHubViewBuilderTests
{
    [Fact]
    public void Successfully_handles_an_empty_list_of_items()
    {
        // Arrange
        var organisationContext = Substitute.For<IOrganisationContext>();
        var emptyPagedList = new StaticPagedList<CalendarOfItemsRow>(new List<CalendarOfItemsRow>(), 1, 10, 0);
        var filters = new Collection<Filterable>();
        var calendarViewBuilder = new CalendarOfItemsViewBuilder(organisationContext);

        var categoryHubViewBuilder = new CategoryHubViewBuilder(calendarViewBuilder);
        var currentUri = new Uri("https://localhost:7033/categories/all-tasks");

        var items = new CalendarOfItemsPagedResult(new CalendarOfItemsCriteria(), emptyPagedList, filters);

        // Act
        var viewModel = categoryHubViewBuilder.Build(items, currentUri);

        // Assert
        viewModel.Items.Should().BeEmpty();
    }

    [Fact]
    public void Successfully_sets_the_correct_text_for_AllTasks_view_when_category_is_null()
    {
        // Arrange
        var schoolName = "Test School";
        var organisationContext = OrganisationContextHelper.CreateSimpleOrganisationContext(schoolName);
        
        var emptyPagedList = new StaticPagedList<CalendarOfItemsRow>(new List<CalendarOfItemsRow>(), 1, 10, 0);
        var filters = new Collection<Filterable>();
        var calendarViewBuilder = new CalendarOfItemsViewBuilder(organisationContext);

        var categoryHubViewBuilder = new CategoryHubViewBuilder(calendarViewBuilder);
        var currentUri = new Uri("https://localhost:7033/categories/all-tasks");

        var items = new CalendarOfItemsPagedResult(new CalendarOfItemsCriteria(), emptyPagedList, filters);

        // Act
        var viewModel = categoryHubViewBuilder.Build(items, currentUri);

        // Assert
        viewModel.Caption.Should().Be("Test School");
        viewModel.Heading.Should().Be("All tasks");
        viewModel.SubHeading.Should().Be("See all your tasks, returns and policies from DfE.");
        viewModel.Description.Should().Be("Explore all tasks and support");
        viewModel.NoResultsMessage.Should().Be("No results found");
    }

    [Fact]
    public void Successfully_sets_the_correct_text_for_Category_when_category_is_set()
    {
        // Arrange
        var schoolName = "Test School";
        var organisationContext = OrganisationContextHelper.CreateSimpleOrganisationContext(schoolName);
        
        var emptyPagedList = new StaticPagedList<CalendarOfItemsRow>(new List<CalendarOfItemsRow>(), 1, 10, 0);
        var filters = new Collection<Filterable>();
        var calendarViewBuilder = new CalendarOfItemsViewBuilder(organisationContext);

        var categoryHubViewBuilder = new CategoryHubViewBuilder(calendarViewBuilder);
        var currentUri = new Uri("https://localhost:7033/categories/1");

        var items = new CalendarOfItemsPagedResult(new CalendarOfItemsCriteria(), emptyPagedList, filters);

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
        viewModel.Caption.Should().Be("Test School");
        viewModel.Heading.Should().Be(category.DisplayName);
        viewModel.SubHeading.Should().Be(category.HubViewDescription);
        viewModel.Description.Should().Be("Explore all tasks and support");
        viewModel.NoResultsMessage.Should().Be("No results found");
    }
}
