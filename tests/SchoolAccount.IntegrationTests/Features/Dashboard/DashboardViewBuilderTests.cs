using AwesomeAssertions;
using SchoolAccount.Application.Features.Dashboard;
using SchoolAccount.IntegrationTests.Features.Dashboard.Handlers;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fixtures;
using Xunit;

namespace SchoolAccount.IntegrationTests.Features.Dashboard;

public class DashboardViewBuilderTests : IClassFixture<TestServerFixture>
{
    private readonly HttpClient _client;
    private readonly TestGetDashboardQueryHandler _handler = new();

    public DashboardViewBuilderTests(TestServerFixture fixture, ITestOutputHelper outputHelper)
    {
        fixture.SetOutputHelper(outputHelper);
        fixture.HandlerRegistry.Clear();
        fixture.HandlerRegistry.Register(_handler);
        _handler.Clear();
        _client = fixture.CreateAnonymousClient();
    }

    [Fact]
    public async Task Dashboard_endpoint_returns_success_page()
    {
        // Act
        var page = await _client
            .GetAsync("/", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.Should().NotBeNull();
        page.QuerySelector("body")
            .Should()
            .NotBeNull()
            .And.Match<AngleSharp.Dom.IElement>(e =>
                !e.TextContent.Contains("Sorry, there is a problem with the service")
            );
    }

    [Fact]
    public async Task Dashboard_endpoint_returns_expected_page_heading()
    {
        // Act
        var page = await _client
            .GetAsync("/", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelector(".govuk-heading-xl").Should().NotBeNull().And.HaveTextContent("Welcome to DfE Connect");
    }

    [Fact]
    public async Task Dashboard_shows_upcoming_tasks_section_heading()
    {
        // Act
        var page = await _client
            .GetAsync("/", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelectorAll(".govuk-heading-m").Should().Contain(e => e.TextContent.Contains("Upcoming tasks"));
    }

    [Fact]
    public async Task Dashboard_shows_no_results_message_when_no_calendar_items()
    {
        // Act
        var page = await _client
            .GetAsync("/", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelectorAll(".govuk-task-list__item")
            .Should()
            .Contain(e => e.TextContent.Contains("No results found"));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(10)]
    public async Task Dashboard_shows_correct_number_of_calendar_items(int count)
    {
        // Arrange
        for (var i = 1; i <= count; i++)
        {
            _handler.AddCalendarItem(new GetDashboardResponseCalendarItem { Id = i, Name = $"Task {i}" });
        }

        // Act
        var page = await _client
            .GetAsync("/", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelectorAll(".govuk-task-list__item--with-link").Should().HaveCount(count);
    }

    [Fact]
    public async Task Dashboard_shows_explore_categories_section_heading()
    {
        // Act
        var page = await _client
            .GetAsync("/", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelectorAll(".govuk-heading-m").Should().Contain(e => e.TextContent.Contains("Explore categories"));
    }

    [Fact]
    public async Task Dashboard_shows_no_categories_message_when_no_categories()
    {
        // Act
        var page = await _client
            .GetAsync("/", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelector("body")
            .Should()
            .NotBeNull()
            .And.Match<AngleSharp.Dom.IElement>(e => e.TextContent.Contains("No tasks found for the category."));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task Dashboard_shows_correct_number_of_categories(int count)
    {
        // Arrange
        for (var i = 1; i <= count; i++)
        {
            _handler.AddCategory(new GetDashboardResponseCategoryItem { Id = i, DisplayName = $"Category {i}" });
        }

        // Act
        var page = await _client
            .GetAsync("/", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        var categoryListItems = page.QuerySelectorAll(".govuk-task-list__item");
        categoryListItems.Length.Should().BeGreaterThanOrEqualTo(count);
    }

    [Fact]
    public async Task Dashboard_does_not_show_see_full_list_link_when_ten_or_fewer_categories()
    {
        // Arrange
        for (var i = 1; i <= 10; i++)
        {
            _handler.AddCategory(new GetDashboardResponseCategoryItem { Id = i, DisplayName = $"Category {i}" });
        }

        // Act
        var page = await _client
            .GetAsync("/", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelectorAll("a").Should().NotContain(e => e.TextContent.Contains("See the full list of categories"));
    }

    [Fact]
    public async Task Dashboard_shows_see_full_list_link_when_more_than_ten_categories()
    {
        // Arrange
        for (var i = 1; i <= 11; i++)
        {
            _handler.AddCategory(new GetDashboardResponseCategoryItem { Id = i, DisplayName = $"Category {i}" });
        }

        // Act
        var page = await _client
            .GetAsync("/", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelectorAll("a").Should().Contain(e => e.TextContent.Contains("See the full list of categories"));
    }
}
