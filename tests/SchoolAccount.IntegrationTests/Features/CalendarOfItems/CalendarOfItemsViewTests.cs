using AngleSharp.Dom;
using AwesomeAssertions;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.IntegrationTests.Features.CalendarOfItems.DataGeneration;
using SchoolAccount.IntegrationTests.Features.CalendarOfItems.Handlers;
using SchoolAccount.Tests.Common.Extensions;
using Xunit;

namespace SchoolAccount.IntegrationTests.Features.CalendarOfItems;

public class CalendarOfItemsViewTests : IClassFixture<SchoolAccountWebApplicationFactory>
{
    private readonly SchoolAccountWebApplicationFactory _factory;
    private readonly TestCalendarOfItemsDirectionalQueryHandler _handler = new();
    private readonly CalendarOfItemsDataGenerator _generator = new();

    public CalendarOfItemsViewTests(SchoolAccountWebApplicationFactory factory, ITestOutputHelper outputHelper)
    {
        _factory = factory;
        _factory.OutputHelper = outputHelper;
        _factory.HandlerRegistry.Clear();
        _factory.HandlerRegistry.Register(_handler);
        _handler.Clear();
    }

    [Fact]
    public async Task Calendar_endpoint_returns_success_page()
    {
        // Act
        var request = await _factory.RequestPageAsync("/calendar");

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
        var request = await _factory.RequestPageAsync("/calendar");

        // Assert
        request.Should().NotBeNull();
        request.QuerySelector(".govuk-heading-l").Should().NotBeNull().And.HaveTextContent("Calendar of tasks");
    }

    [Fact]
    public async Task Calendar_contains_upcoming_and_previous_task_tabs()
    {
        // Act
        var request = await _factory.RequestPageAsync("/calendar");

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
        var request = await _factory.RequestPageAsync($"/calendar?ViewModes={mode}");

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
        var request = await _factory.RequestPageAsync($"/calendar?ViewModes={mode}");

        // Assert
        request.QuerySelector(".dfe-tabs__panel .govuk-heading-m").Should().HaveTextContent(expectedTitle);
    }

    [Fact]
    public async Task Pagination_visible_when_multiple_pages_of_tasks()
    {
        // Arrange
        var filter = new CalendarOfItemsCriteria { ViewModes = CalendarOfItemsViewModes.Forward, PageSize = 10 };
        var rows = _generator.GenerateCalendarOfItemsRows(filter, 40);

        _handler.AddRows(rows).SetPageSize(10);

        // Act
        var request = await _factory.RequestPageAsync($"/calendar?ViewModes={CalendarOfItemsViewModes.Forward}");

        // Assert
        request.QuerySelector(".govuk-pagination").Should().BePaginationWithLabels("1", "2", "3", "4", "Next page");
    }

    [Theory]
    [InlineData(10, 200, 1, new[] { "1", "2", "3", "⋯", "20", "Next page" })]
    [InlineData(10, 200, 2, new[] { "Previous page", "1", "2", "3", "⋯", "20", "Next page" })]
    [InlineData(10, 200, 3, new[] { "Previous page", "1", "2", "3", "4", "⋯", "20", "Next page" })]
    [InlineData(10, 200, 4, new[] { "Previous page", "1", "2", "3", "4", "5", "⋯", "20", "Next page" })]
    [InlineData(10, 200, 5, new[] { "Previous page", "1", "⋯", "4", "5", "6", "⋯", "20", "Next page" })]
    [InlineData(10, 200, 17, new[] { "Previous page", "1", "⋯", "16", "17", "18", "19", "20", "Next page" })]
    [InlineData(10, 200, 18, new[] { "Previous page", "1", "⋯", "17", "18", "19", "20", "Next page" })]
    [InlineData(10, 200, 19, new[] { "Previous page", "1", "⋯", "18", "19", "20", "Next page" })]
    [InlineData(10, 200, 20, new[] { "Previous page", "1", "⋯", "18", "19", "20" })]
    public async Task Pagination_changes_according_to_page_size(
        int pageSize,
        int entriesToGenerate,
        int pageNumber,
        string[] expectedLabels
    )
    {
        // Arrange
        var filter = new CalendarOfItemsCriteria { ViewModes = CalendarOfItemsViewModes.Forward, PageSize = pageSize };
        var rows = _generator.GenerateCalendarOfItemsRows(filter, entriesToGenerate);

        _handler.AddRows(rows).SetPageSize(pageSize);

        // Act
        var request = await _factory.RequestPageAsync(
            $"/calendar?ViewModes={CalendarOfItemsViewModes.Forward}&pageNumber={pageNumber}"
        );

        // Assert
        request.QuerySelector(".govuk-pagination").Should().BePaginationWithLabels(expectedLabels);
    }

    [Fact]
    public async Task Pagination_hidden_when_tasks_fit_on_single_page()
    {
        // Arrange
        var filter = new CalendarOfItemsCriteria { ViewModes = CalendarOfItemsViewModes.Forward, PageSize = 10 };
        var rows = _generator.GenerateCalendarOfItemsRows(filter, 5);

        _handler.AddRows(rows).SetPageSize(10);

        // Act
        var request = await _factory.RequestPageAsync($"/calendar?ViewModes={CalendarOfItemsViewModes.Forward}");

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

        const int pageSize = 10;
        var rows = _generator.GenerateCalendarOfItemsRows(filter, numberToGenerate);
        var numberOfItemRows = Math.Min(rows.Count, pageSize);
        var expectedRows = numberOfItemRows + numberOfNoItemsMessageRows + numberOfCallToActionRows;

        _handler.AddRows(rows).SetPageSize(pageSize);

        // Act.
        var request = await _factory.RequestPageAsync("/calendar");

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
        var request = await _factory.RequestPageAsync("/calendar");

        // Assert.
        request.Should().NotBeNull();
        var taskListItems = request.QuerySelectorAll(".govuk-task-list__item");
        taskListItems.Should().HaveCount(expectedRows).And.Contain(e => e.TextContent.Contains("No results found"));
    }
}
