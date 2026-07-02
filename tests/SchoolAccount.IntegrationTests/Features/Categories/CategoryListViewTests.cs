using AngleSharp.Dom;
using AwesomeAssertions;
using SchoolAccount.Application.Features.Categories.GetParentCategories;
using SchoolAccount.IntegrationTests.Features.Categories.Handlers;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fixtures;
using Xunit;

namespace SchoolAccount.IntegrationTests.Features.Categories;

public class CategoryListViewTests : IClassFixture<TestServerFixture>
{
    private readonly HttpClient _client;
    private readonly TestGetParentCategoriesQueryHandler _handler = new();

    public CategoryListViewTests(TestServerFixture fixture, ITestOutputHelper outputHelper)
    {
        fixture.SetOutputHelper(outputHelper);
        fixture.HandlerRegistry.Clear();
        fixture.HandlerRegistry.Register(_handler);
        _handler.Clear();
        _client = fixture.CreateAnonymousClient();
    }

    [Fact]
    public async Task Endpoint_returns_success_page()
    {
        // Act
        var page = await _client.GetAsync("/categories", TestContext.Current.CancellationToken).ReadAsPageAsync();

        // Assert
        page.Should().NotBeNull();
        page.QuerySelector("body")
            .Should()
            .NotBeNull()
            .And.Match<IElement>(e => !e.TextContent.Contains("Sorry, there is a problem with the service"));
    }

    [Fact]
    public async Task Endpoint_returns_expected_page_heading()
    {
        // Act
        var page = await _client.GetAsync("/categories", TestContext.Current.CancellationToken).ReadAsPageAsync();

        // Assert
        page.QuerySelector(".govuk-heading-l").Should().NotBeNull().And.HaveTextContent("Explore categories");
    }

    [Fact]
    public async Task Shows_no_results_message_when_no_categories()
    {
        // Act
        var page = await _client.GetAsync("/categories", TestContext.Current.CancellationToken).ReadAsPageAsync();

        // Assert
        page.QuerySelectorAll(".govuk-task-list__item")
            .Should()
            .Contain(e => e.TextContent.Contains("No tasks found for the category."));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task Shows_correct_number_of_categories(int count)
    {
        // Arrange
        for (var i = 1; i <= count; i++)
        {
            _handler.AddCategory(new GetParentCategoriesResponseCategory { Id = i, DisplayName = $"Category {i}" });
        }

        // Act
        var page = await _client.GetAsync("/categories", TestContext.Current.CancellationToken).ReadAsPageAsync();

        // Assert
        // The list prepends an "All tasks" link ahead of the categories
        page.QuerySelectorAll(".govuk-task-list__item--with-link").Should().HaveCount(count + 1);
        page.QuerySelectorAll(".govuk-task-list__item--with-link")
            .Should()
            .Contain(e => e.TextContent.Contains("All tasks"));
    }
}
