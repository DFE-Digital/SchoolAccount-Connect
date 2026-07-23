using AngleSharp.Dom;
using AwesomeAssertions;
using SchoolAccount.Application.Features.Tasks.GetAll;
using SchoolAccount.IntegrationTests.Features.Tasks.Handlers;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fixtures;
using Xunit;

namespace SchoolAccount.IntegrationTests.Features.Tasks;

public class AllTasksViewTests : IClassFixture<TestServerFixture>
{
    private readonly HttpClient _client;
    private readonly TestGetAllTasksQueryHandler _handler = new();

    public AllTasksViewTests(TestServerFixture fixture, ITestOutputHelper outputHelper)
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
            .GetAsync("/tasks", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

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
            .GetAsync("/tasks", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelector(".govuk-heading-l").Should().NotBeNull().And.HaveTextContent("All tasks");
    }

    [Fact]
    public async Task Shows_no_results_message_when_no_tasks()
    {
        // Act
        var page = await _client
            .GetAsync("/tasks", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelectorAll(".govuk-task-list__item")
            .Should()
            .Contain(e => e.TextContent.Contains("No tasks found."));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(10)]
    public async Task Shows_correct_number_of_tasks(int count)
    {
        // Arrange
        for (var i = 1; i <= count; i++)
        {
            _handler.AddTask(new GetAllTasksResponseTask { Id = i, Name = $"Task {i}" });
        }

        // Act
        var page = await _client
            .GetAsync("/tasks", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelectorAll(".govuk-task-list__item--with-link").Should().HaveCount(count);
    }

    [Fact]
    public async Task Task_links_point_to_task_details()
    {
        // Arrange
        _handler.AddTask(new GetAllTasksResponseTask { Id = 42, Name = "School attendance" });

        // Act
        var page = await _client
            .GetAsync("/tasks", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelectorAll("a[href='/tasks/42']")
            .Should()
            .Contain(e => e.TextContent.Contains("School attendance"));
    }
}
