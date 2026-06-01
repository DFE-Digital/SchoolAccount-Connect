using AngleSharp.Dom;
using AwesomeAssertions;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.IntegrationTests.Features.CalendarOfItems.DataGeneration;
using SchoolAccount.IntegrationTests.Features.CalendarOfItems.Fixtures;
using SchoolAccount.Tests.Common.Extensions;
using Xunit;

namespace SchoolAccount.IntegrationTests.Features.CalendarOfItems;

public class CalendarOfItemsViewBuilderTests : IClassFixture<HttpServerFixture>
{
    private readonly HttpServerFixture _fixture;
    private readonly CalendarOfItemsDataGenerator _generator = new();

    public CalendarOfItemsViewBuilderTests(HttpServerFixture fixture, ITestOutputHelper outputHelper)
    {
        _fixture = fixture;
        _fixture.OutputHelper = outputHelper;
        _fixture.TestCalendarOfItemsDirectionalQueryHandler.Clear();
    }

    [Fact]
    public async Task Calendar_endpoint_returns_success_page()
    {
        // Act
        var request = await _fixture.RequestPageAsync("/calendar");

        // Assert
        request.Should().NotBeNull();
        request
            .QuerySelector("body")
            .Should()
            .NotBeNull()
            .And.Match<IElement>(e => !e.TextContent.Contains("Sorry, there is a problem with the service"));
    }

    [Fact]
    public async Task Calendar_endpoint_returns_expected_title()
    {
        // Act
        var request = await _fixture.RequestPageAsync("/calendar");

        // Assert
        request.Should().NotBeNull();
        request.QuerySelector(".govuk-heading-l").Should().NotBeNull().And.HaveTextContent("Calendar of tasks");
    }

    [Fact]
    public async Task Calendar_contains_upcoming_and_previous_task_tabs()
    {
        // Act
        var request = await _fixture.RequestPageAsync("/calendar");

        // Assert
        request.QuerySelectorAll(".govuk-tabs__tab").Should().HaveCount(2);
        request.QuerySelectorAll(".govuk-tabs__tab")[0].Should().HaveTextContent("Upcoming tasks");
        request.QuerySelectorAll(".govuk-tabs__tab")[1].Should().HaveTextContent("Previous tasks");
    }

    [Theory]
    [InlineData(CalendarOfItemsViewModes.Forward, "Upcoming tasks")]
    [InlineData(CalendarOfItemsViewModes.Backward, "Previous tasks")]
    public async Task Correct_tab_selected_based_on_view_mode(CalendarOfItemsViewModes mode, string expectedTitle)
    {
        // Act
        var request = await _fixture.RequestPageAsync($"/calendar?ViewModes={mode}");

        // Assert
        request
            .QuerySelector(".govuk-tabs__list-item--selected .govuk-tabs__tab")
            .Should()
            .HaveTextContent(expectedTitle);
    }

    [Theory]
    [InlineData(CalendarOfItemsViewModes.Forward, "Upcoming tasks")]
    [InlineData(CalendarOfItemsViewModes.Backward, "Previous tasks")]
    public async Task Page_heading_matches_selected_view_mode(CalendarOfItemsViewModes mode, string expectedTitle)
    {
        // Act
        var request = await _fixture.RequestPageAsync($"/calendar?ViewModes={mode}");

        // Assert
        request.QuerySelector(".dfe-tabs__panel .govuk-heading-m").Should().HaveTextContent(expectedTitle);
    }

    [Fact]
    public async Task Pagination_visible_when_multiple_pages_of_tasks()
    {
        // Arrange
        var filter = new CalendarOfItemsCriteria { ViewModes = CalendarOfItemsViewModes.Forward, PageSize = 10 };
        var rows = _generator.GenerateCalendarOfItemsRows(filter, 40);

        _fixture.TestCalendarOfItemsDirectionalQueryHandler.AddRows(rows).SetPageSize(10);

        // Act
        var request = await _fixture.RequestPageAsync($"/calendar?ViewModes={CalendarOfItemsViewModes.Forward}");

        // Assert
        request.QuerySelector(".govuk-pagination").Should().BePaginationWithLabels("1", "2", "3", "4", "Next page");
    }

    [Theory]
    [InlineData(20, 100, new[] { "1", "2", "3", "4", "5", "Next page" })]
    [InlineData(40, 80, new[] { "1", "2", "Next page" })]
    [InlineData(65, 66, new[] { "1", "2", "Next page" })]
    public async Task Pagination_changes_according_to_page_size(
        int pageSize,
        int entriesToGenerate,
        string[] expectedLabels
    )
    {
        // Arrange
        var filter = new CalendarOfItemsCriteria { ViewModes = CalendarOfItemsViewModes.Forward, PageSize = pageSize };
        var rows = _generator.GenerateCalendarOfItemsRows(filter, entriesToGenerate);

        _fixture.TestCalendarOfItemsDirectionalQueryHandler.AddRows(rows).SetPageSize(pageSize);

        // Act
        var request = await _fixture.RequestPageAsync($"/calendar?ViewModes={CalendarOfItemsViewModes.Forward}");

        // Assert
        request.QuerySelector(".govuk-pagination").Should().BePaginationWithLabels(expectedLabels);
    }

    [Fact]
    public async Task Pagination_hidden_when_tasks_fit_on_single_page()
    {
        // Arrange
        var filter = new CalendarOfItemsCriteria { ViewModes = CalendarOfItemsViewModes.Forward, PageSize = 10 };
        var rows = _generator.GenerateCalendarOfItemsRows(filter, 5);

        _fixture.TestCalendarOfItemsDirectionalQueryHandler.AddRows(rows).SetPageSize(10);

        // Act
        var request = await _fixture.RequestPageAsync($"/calendar?ViewModes={CalendarOfItemsViewModes.Forward}");

        // Assert
        request.QuerySelector(".govuk-pagination").Should().BeNull();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(11)]
    public async Task Calendar_endpoint_returns_calendar_of_items_containing_correct_number_of_rows(
        int numberToGenerate
    )
    {
        // Arrange.
        var filter = new CalendarOfItemsCriteria { ViewModes = CalendarOfItemsViewModes.Forward, PageSize = 10 };

        const int numberOfNoItemsMessageRows = 0;
        const int numberOfCallToActionRows = 0;

        var rows = _generator.GenerateCalendarOfItemsRows(filter, numberToGenerate);
        _fixture.TestCalendarOfItemsDirectionalQueryHandler.AddRows(rows).SetPageSize(10);
        int numberOfItemRows = rows.Count;
        int expectedRows = numberOfItemRows + numberOfNoItemsMessageRows + numberOfCallToActionRows;

        // Act.
        var request = await _fixture.RequestPageAsync("/calendar");

        // Assert.
        request.Should().NotBeNull();
        request.QuerySelectorAll(".govuk-task-list__item").Length.Should().Be(expectedRows);
    }

    [Fact]
    public async Task Calendar_endpoint_returns_calendar_of_items_containing_correct_number_of_rows_when_no_items()
    {
        // Arrange.
        const int numberOfNoItemsMessageRows = 1;
        const int numberOfCallToActionRows = 0;

        const int expectedRows = numberOfNoItemsMessageRows + numberOfCallToActionRows;

        // Act.
        var request = await _fixture.RequestPageAsync("/calendar");

        // Assert.
        request.Should().NotBeNull();
        var taskListItems = request.QuerySelectorAll(".govuk-task-list__item");
        taskListItems.Should().HaveCount(expectedRows).And.Contain(e => e.TextContent.Contains("No results found"));
    }
}
