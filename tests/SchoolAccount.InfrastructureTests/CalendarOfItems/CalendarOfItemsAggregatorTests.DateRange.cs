using AwesomeAssertions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.InfrastructureTests.Extensions;

namespace SchoolAccount.InfrastructureTests.CalendarOfItems;

public partial class CalendarOfItemsAggregatorTests
{
    [Fact]
    public async Task Includes_rows_within_range_boundaries_and_excludes_rows_outside()
    {
        // Arrange
        var criteria = Make.Criteria();

        CalendarOfItemsRow rowOnStartBoundary = CalendarOfItemsRowExtensions.Create(1, DefaultRange.Start);
        CalendarOfItemsRow rowOnEndBoundary = CalendarOfItemsRowExtensions.Create(2, DefaultRange.End);
        CalendarOfItemsRow rowOneDayBeforeStart = CalendarOfItemsRowExtensions.Create(3, DefaultRange.Start.AddDays(-1));
        CalendarOfItemsRow rowOneDayAfterEnd = CalendarOfItemsRowExtensions.Create(4, DefaultRange.End.AddDays(1));

        var factory = Make.Factory.Query(
            CalendarOfItemsQueryTypes.SubTask,
            [rowOnStartBoundary, rowOnEndBoundary, rowOneDayBeforeStart, rowOneDayAfterEnd]
        );

        var sut = Make.Aggregator([factory]);

        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        result
            .Value.Payload.Should()
            .BeEquivalentTo(
                [rowOnStartBoundary, rowOnEndBoundary],
                because: "rows within range boundaries should be included, rows outside should not"
            );
    }

    [Fact]
    public async Task Ensures_rows_with_no_dates_are_not_present()
    {
        // Arrange
        var criteria = Make.Criteria();
        
        CalendarOfItemsRow rowThatIsEmpty = CalendarOfItemsRowExtensions.Create();
        CalendarOfItemsRow rowWithDate = CalendarOfItemsRowExtensions.Create(Today);
        
        var factory = Make.Factory.Query(CalendarOfItemsQueryTypes.SubTask, [rowThatIsEmpty, rowWithDate]);
        var sut = Make.Aggregator([factory]);
        
        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        result
            .Value.Payload.Should()
            .BeEquivalentTo(
                [rowWithDate],
                because:"Has only one as the other row is not in scope as it has a missing date"
            );
    }
}