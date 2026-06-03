using AwesomeAssertions;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.InfrastructureTests.Extensions;

namespace SchoolAccount.InfrastructureTests.CalendarOfItems;

public partial class QueryAggregatorTests
{
    [Fact]
    public async Task Ensure_if_multiple_factories_are_configured_that_they_are_both_queried()
    {
        // Arrange
        List<CalendarOfItemsRow> factoryThatIsExpectedARows =
        [
            CalendarOfItemsRowExtensions.Create(1, "Included", Today),
            CalendarOfItemsRowExtensions.Create(2, "Included", Today.AddDays(-2)),
            CalendarOfItemsRowExtensions.Create(3, "Included", Today.AddDays(3)),
            CalendarOfItemsRowExtensions.Create(4, "Excluded", DefaultRange.Start.AddDays(-1)),
        ];
        var factoryThatIsExpectedA = Make.Factory.Query(CalendarOfItemsQueryTypes.SubTask, factoryThatIsExpectedARows);

        List<CalendarOfItemsRow> factoryThatIsExpectedBRows =
        [
            CalendarOfItemsRowExtensions.Create(5, "Included", Today.AddDays(-7)),
            CalendarOfItemsRowExtensions.Create(6, "Included", Today.AddDays(-5)),
        ];
        var factoryThatIsExpectedB = Make.Factory.Query(CalendarOfItemsQueryTypes.SubTask, factoryThatIsExpectedBRows);

        List<CalendarOfItemsRow> factoryThatIsNotExpectedARows =
        [
            CalendarOfItemsRowExtensions.Create(7, "Excluded", DefaultRange.End),
        ];
        var factoryThatIsNotExpected = Make.Factory.Query(
            CalendarOfItemsQueryTypes.Task,
            factoryThatIsNotExpectedARows
        );

        var sut = Make.Aggregator([factoryThatIsExpectedA, factoryThatIsExpectedB, factoryThatIsNotExpected]);
        var criteria = Make.Criteria();

        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        var rowsInScope = factoryThatIsExpectedARows.Union(factoryThatIsExpectedBRows).Where(x => x.Name == "Included");
        result
            .Value.Payload.Should()
            .BeEquivalentTo(
                rowsInScope,
                because: "There should be none of the rows that are outside of the range or not apart of the selected query type"
            );
    }

    [Fact]
    public async Task Query_UnionDeduplicatesSameRows()
    {
        // Arrange
        CalendarOfItemsRow row = CalendarOfItemsRowExtensions.Create(1, Today);
        var factoryA = Make.Factory.Query(CalendarOfItemsQueryTypes.SubTask, [row]);
        var factoryB = Make.Factory.Query(CalendarOfItemsQueryTypes.SubTask, [row]);
        var sut = Make.Aggregator([factoryA, factoryB]);
        var criteria = Make.Criteria();

        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        result
            .Value.Payload.Should()
            .BeEquivalentTo([row], because: "the id's are the same so once unioned it will only return the one row.");
    }
}
