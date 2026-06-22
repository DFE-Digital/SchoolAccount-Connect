using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using AwesomeAssertions;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Models;
using SchoolAccount.InfrastructureTests.Extensions;

namespace SchoolAccount.InfrastructureTests.CalendarOfItems;

[SuppressMessage("Performance", "CA1826:Do not use Enumerable methods on indexable collections")]
public partial class CalendarOfItemsAggregatorTests
{
    [Theory]
    [InlineData(5, 1, 20)]
    [InlineData(5, 2, 20)]
    [InlineData(10, 1, 2000)]
    [InlineData(10, 2, 9000)]
    [InlineData(20, 40, 10000)]
    public async Task Ensure_that_criteria_returns_requested_pagination(int pageSize, int pageNumber, int numberOfRows)
    {
        // Arrange
        Collection<CalendarOfItemsRow> rows = CalendarOfItemsRowExtensions
            .Collection(DefaultRange)
            .Populate(numberOfRows);
        var queryFactory = Make.Factory.Query(CalendarOfItemsQueryTypes.SubTask, rows);
        var sut = Make.Aggregator([queryFactory]);

        var criteria = Make.Criteria(
            pageSize: pageSize,
            pageNumber: pageNumber,
            range: DefaultRange,
            customOrderByFunction: x => x.OrderBy(r => r.Id)
        );

        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        var firstId = (pageNumber - 1) * pageSize + 1;
        var lastId = Math.Min(pageNumber * pageSize, numberOfRows);

        result.IsSuccess.Should().BeTrue(because: "Should always succeed");
        result.Value.Payload.Items.Should().HaveCount(pageSize, because: "The requested page size");
        result
            .Value.Payload.Items.First()
            .Id.Should()
            .Be(firstId, because: "This should be the min ID from range of possible data");
        result
            .Value.Payload.Items.Last()
            .Id.Should()
            .Be(lastId, because: "This should be the max ID from range of possible data");
        result.Value.Payload.PageSize.Should().Be(pageSize, because: "The requested page size");
        result.Value.Payload.PageNumber.Should().Be(pageNumber, because: "The requested page");
        result
            .Value.Payload.TotalCount.Should()
            .Be(numberOfRows, because: "The pool of items that should be returning");
    }

    [Fact]
    public async Task Ensure_the_request_still_completes_if_pagination_is_out_of_range()
    {
        // Arrange
        var queryFactory = Make.Factory.Query(
            CalendarOfItemsQueryTypes.SubTask,
            [CalendarOfItemsRowExtensions.Create(Today)]
        );
        var sut = Make.Aggregator([queryFactory]);
        var criteria = Make.Criteria(pageNumber: 99);

        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue(because: "Should always succeed");
        result
            .Value.Payload.Items.Should()
            .BeEmpty(because: "We are outside of the one row pool so nothing should be returned");
    }
}
