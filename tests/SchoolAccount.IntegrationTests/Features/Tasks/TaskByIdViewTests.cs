using AngleSharp.Dom;
using AwesomeAssertions;
using SchoolAccount.Application.Features.Tasks.GetById;
using SchoolAccount.Domain.Common;
using SchoolAccount.IntegrationTests.Features.Tasks.Handlers;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fixtures;
using Xunit;

namespace SchoolAccount.IntegrationTests.Features.Tasks;

public class TaskByIdViewTests : IClassFixture<TestServerFixture>
{
    private readonly HttpClient _client;
    private readonly TestGetTaskByIdQueryHandler _handler = new();

    public TaskByIdViewTests(TestServerFixture fixture, ITestOutputHelper outputHelper)
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
            .GetAsync("/tasks/1", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.Should().NotBeNull();
        page.QuerySelector("body")
            .Should()
            .NotBeNull()
            .And.Match<IElement>(e => !e.TextContent.Contains("Sorry, there is a problem with the service"));
    }

    [Fact]
    public async Task Shows_task_name_as_heading()
    {
        // Act
        var page = await _client
            .GetAsync("/tasks/1", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelector(".govuk-heading-l").Should().NotBeNull().And.HaveTextContent("School attendance");
    }

    [Fact]
    public async Task Shows_no_upcoming_tasks_message_when_no_subtasks()
    {
        // Act
        var page = await _client
            .GetAsync("/tasks/1", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelector("body")
            .Should()
            .NotBeNull()
            .And.Match<IElement>(e => e.TextContent.Contains("There are no upcoming tasks."));
    }

    [Fact]
    public async Task Shows_published_subtasks()
    {
        // Arrange
        _handler.Response = TestGetTaskByIdQueryHandler.DefaultResponse() with
        {
            SubTasks =
            [
                new GetTaskByIdResponseSubtask
                {
                    Id = 1,
                    Name = "Submit the school census",
                    WorkflowState = WorkflowState.Published,
                },
                new GetTaskByIdResponseSubtask
                {
                    Id = 2,
                    Name = "Review the attendance policy",
                    WorkflowState = WorkflowState.Published,
                },
            ],
        };

        // Act
        var page = await _client
            .GetAsync("/tasks/1", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        var body = page.QuerySelector("body");
        body.Should().NotBeNull().And.Match<IElement>(e => e.TextContent.Contains("Submit the school census"));
        body.Should().Match<IElement>(e => e.TextContent.Contains("Review the attendance policy"));
    }

    [Fact]
    public async Task Shows_related_tasks_when_present()
    {
        // Arrange
        _handler.Response = TestGetTaskByIdQueryHandler.DefaultResponse() with
        {
            RelatedTasks = [new GetTaskByIdResponseRelatedTask { Id = 7, Name = "Safeguarding" }],
        };

        // Act
        var page = await _client
            .GetAsync("/tasks/1", TestContext.Current.CancellationToken)
            .ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        page.QuerySelector("body")
            .Should()
            .NotBeNull()
            .And.Match<IElement>(e => e.TextContent.Contains("Safeguarding"));
    }
}
