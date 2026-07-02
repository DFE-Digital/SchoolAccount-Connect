using AngleSharp.Dom;
using AwesomeAssertions;
using SchoolAccount.Application.Features.Categories.GetCategoryHub;
using SchoolAccount.IntegrationTests.Features.Categories.Handlers;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fixtures;
using Xunit;

namespace SchoolAccount.IntegrationTests.Features.Categories;

public class CategoryHubViewTests : IClassFixture<TestServerFixture>
{
    private readonly HttpClient _client;
    private readonly TestGetCategoryHubQueryHandler _handler = new();

    public CategoryHubViewTests(TestServerFixture fixture, ITestOutputHelper outputHelper)
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
        var page = await _client.GetAsync("/categories/2", TestContext.Current.CancellationToken).ReadAsPageAsync();

        // Assert
        page.Should().NotBeNull();
        page.QuerySelector("body")
            .Should()
            .NotBeNull()
            .And.Match<IElement>(e => !e.TextContent.Contains("Sorry, there is a problem with the service"));
    }

    [Fact]
    public async Task Shows_category_display_name_as_heading()
    {
        // Arrange
        _handler.WithDisplayName("Finance");

        // Act
        var page = await _client.GetAsync("/categories/2", TestContext.Current.CancellationToken).ReadAsPageAsync();

        // Assert
        page.QuerySelector(".govuk-heading-l").Should().NotBeNull().And.HaveTextContent("Finance");
    }

    [Fact]
    public async Task Shows_no_results_message_when_no_tasks()
    {
        // Arrange
        _handler.WithDisplayName("Finance");

        // Act
        var page = await _client.GetAsync("/categories/2", TestContext.Current.CancellationToken).ReadAsPageAsync();

        // Assert
        page.QuerySelectorAll(".govuk-task-list__item")
            .Should()
            .Contain(e => e.TextContent.Contains("No tasks found for the category Finance."));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    public async Task Shows_correct_number_of_tasks(int count)
    {
        // Arrange
        for (var i = 1; i <= count; i++)
        {
            _handler.AddTask(new GetCategoryHubResponseTasks { Id = i, Name = $"Task {i}" });
        }

        // Act
        var page = await _client.GetAsync("/categories/2", TestContext.Current.CancellationToken).ReadAsPageAsync();

        // Assert
        page.QuerySelectorAll(".govuk-task-list__item--with-link").Should().HaveCount(count);
    }

    [Fact]
    public async Task Shows_academy_trust_handbook_link_for_handbook_category()
    {
        // Arrange
        _handler.WithId(1);

        // Act
        var page = await _client.GetAsync("/categories/1", TestContext.Current.CancellationToken).ReadAsPageAsync();

        // Assert
        page.QuerySelectorAll("a").Should().Contain(e => e.TextContent.Contains("Academy trust handbook"));
    }

    [Fact]
    public async Task Does_not_show_academy_trust_handbook_link_for_other_categories()
    {
        // Act
        var page = await _client.GetAsync("/categories/2", TestContext.Current.CancellationToken).ReadAsPageAsync();

        // Assert
        page.QuerySelectorAll("a").Should().NotContain(e => e.TextContent.Contains("Academy trust handbook"));
    }
}
