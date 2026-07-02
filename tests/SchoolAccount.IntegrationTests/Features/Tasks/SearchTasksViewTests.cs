using AngleSharp.Dom;
using AwesomeAssertions;
using SchoolAccount.Application.Features.Tasks.Search;
using SchoolAccount.IntegrationTests.Features.Tasks.Handlers;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fixtures;
using Xunit;

namespace SchoolAccount.IntegrationTests.Features.Tasks;

public class SearchTasksViewTests : IClassFixture<TestServerFixture>
{
    private readonly HttpClient _client;
    private readonly TestSearchTasksQueryHandler _handler = new();

    public SearchTasksViewTests(TestServerFixture fixture, ITestOutputHelper outputHelper)
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
        var page = await _client
            .GetAsync("/search?Term=policy", TestContext.Current.CancellationToken)
            .ReadAsPageAsync();

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
        var page = await _client
            .GetAsync("/search?Term=policy", TestContext.Current.CancellationToken)
            .ReadAsPageAsync();

        // Assert
        page.QuerySelector(".govuk-heading-l").Should().NotBeNull().And.HaveTextContent("Search results");
    }

    [Fact]
    public async Task Description_shows_search_term()
    {
        // Act
        var page = await _client
            .GetAsync("/search?Term=policy", TestContext.Current.CancellationToken)
            .ReadAsPageAsync();

        // Assert
        page.QuerySelector("body")
            .Should()
            .NotBeNull()
            .And.Match<IElement>(e => e.TextContent.Contains("Showing results for “policy”."));
    }

    [Fact]
    public async Task Shows_no_results_message_when_no_matches()
    {
        // Act
        var page = await _client
            .GetAsync("/search?Term=policy", TestContext.Current.CancellationToken)
            .ReadAsPageAsync();

        // Assert
        page.QuerySelectorAll(".govuk-task-list__item")
            .Should()
            .Contain(e => e.TextContent.Contains("No tasks found."));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(10)]
    public async Task Shows_correct_number_of_results(int count)
    {
        // Arrange
        for (var i = 1; i <= count; i++)
        {
            _handler.AddTask(new SearchTasksResponseTask { Id = i, Name = $"Task {i}" });
        }

        // Act
        var page = await _client
            .GetAsync("/search?Term=policy", TestContext.Current.CancellationToken)
            .ReadAsPageAsync();

        // Assert
        page.QuerySelectorAll(".govuk-task-list__item--with-link").Should().HaveCount(count);
    }
}
