using AwesomeAssertions;
using NSubstitute;
using SchoolAccount.Application.Features.Categories.Contracts;
using SchoolAccount.Application.Features.Categories.Enums;
using SchoolAccount.Application.Features.Categories.Models;
using SchoolAccount.AuthenticationTests.Helpers;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders.Categories;
using X.PagedList;
using Xunit;

namespace SchoolAccount.Web.Connect.UnitTests.Builders;

public class CategoryListViewBuilderTests
{
    [Fact]
    public void BuildForPage_successfully_handles_an_empty_list_of_items()
    {
        // Arrange
        var organisationContext = Substitute.For<IOrganisationContext>();
        var emptyPagedList = new StaticPagedList<CategoryRow>(new List<CategoryRow>(), 1, 10, 0);
        var viewBuilder = new CategoryListViewBuilder(organisationContext);
        var currentUri = new Uri("https://localhost:7033/categories");

        var items = new CategoryPagedResult(emptyPagedList);

        // Act
        var viewModel = viewBuilder.BuildForPage(items, CategoryListViewModes.Standalone, currentUri);

        // Assert
        viewModel.Categories.Should().BeEmpty();
        viewModel.NoResultsMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void BuildForPage_successfully_sets_the_correct_text_for_the_Page_view()
    {
        // Arrange
        var organisationContext = Substitute.For<IOrganisationContext>();
        var emptyPagedList = new StaticPagedList<CategoryRow>(new List<CategoryRow>(), 1, 10, 0);
        var viewBuilder = new CategoryListViewBuilder(organisationContext);
        var currentUri = new Uri("https://localhost:7033/categories");

        var items = new CategoryPagedResult(emptyPagedList);

        // Act
        var viewModel = viewBuilder.BuildForPage(items, CategoryListViewModes.Standalone, currentUri);

        // Assert
        viewModel.Heading.Should().Be("Explore categories");
        viewModel.SubHeading.Should().Be("View required tasks and optional guidance by category.");
        viewModel.Description.Should().BeNullOrEmpty();
        viewModel.NoResultsMessage.Should().Be("No results found");
        viewModel.ShowNavigator.Should().BeFalse();
        viewModel.IsStandalone.Should().BeTrue();
    }

    [Fact]
    public void BuildForPage_successfully_sets_the_correct_caption_for_the_Page_view()
    {
        // Arrange
        var schoolName = "Test School";
        var organisationContext = OrganisationContextHelper.CreateSimpleOrganisationContext(schoolName);

        var emptyPagedList = new StaticPagedList<CategoryRow>(new List<CategoryRow>(), 1, 10, 0);
        var viewBuilder = new CategoryListViewBuilder(organisationContext);
        var currentUri = new Uri("https://localhost:7033/categories");

        var items = new CategoryPagedResult(emptyPagedList);

        // Act
        var viewModel = viewBuilder.BuildForPage(items, CategoryListViewModes.None, currentUri);

        // Assert
        organisationContext.Organisation.Name.Should().Be(schoolName);
        viewModel.Caption.Should().Be(schoolName);
    }

    [Fact]
    public void BuildForDashboard_successfully_handles_an_empty_list_of_items()
    {
        // Arrange
        var organisationContext = Substitute.For<IOrganisationContext>();
        var emptyPagedList = new StaticPagedList<CategoryRow>(new List<CategoryRow>(), 1, 10, 0);
        var viewBuilder = new CategoryListViewBuilder(organisationContext);
        var currentUri = new Uri("https://localhost:7033/categories");

        var items = new CategoryPagedResult(emptyPagedList);

        // Act
        var viewModel = viewBuilder.BuildForDashboard(items, CategoryListViewModes.Dashboard, currentUri);

        // Assert
        viewModel.Categories.Should().BeEmpty();
        viewModel.NoResultsMessage.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void BuildForDashboard_successfully_sets_the_correct_text_for_the_Dashboard_view()
    {
        // Arrange
        var organisationContext = Substitute.For<IOrganisationContext>();
        var emptyPagedList = new StaticPagedList<CategoryRow>(new List<CategoryRow>(), 1, 10, 0);
        var viewBuilder = new CategoryListViewBuilder(organisationContext);
        var currentUri = new Uri("https://localhost:7033/categories");

        var items = new CategoryPagedResult(emptyPagedList);

        // Act
        var viewModel = viewBuilder.BuildForDashboard(items, CategoryListViewModes.Dashboard, currentUri);

        // Assert
        viewModel.Title.Should().Be("Explore categories");
        viewModel.Description.Should().Be("View required tasks and optional guidance by category.");
        viewModel.Heading.Should().BeNullOrEmpty();
        viewModel.SubHeading.Should().BeNullOrEmpty();
        viewModel.NoResultsMessage.Should().Be("No results found");
        viewModel.ShowNavigator.Should().BeFalse();
        viewModel.IsStandalone.Should().BeFalse();
    }
}
