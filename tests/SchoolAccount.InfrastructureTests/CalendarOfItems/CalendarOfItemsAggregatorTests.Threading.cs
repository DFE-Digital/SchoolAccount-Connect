using AwesomeAssertions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;

namespace SchoolAccount.InfrastructureTests.CalendarOfItems;

public partial class QueryAggregatorTests
{
    [Fact]
    public async Task Ensure_that_if_the_thread_is_cancelled_the_application_stops()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var queryFactory = Make.Factory.Query(CalendarOfItemsQueryTypes.SubTask, []);
        var sut = Make.Aggregator([queryFactory]);
        var criteria = Make.Criteria();

        // Assert
        await sut.Invoking(s => s.Query(criteria, cts.Token)).Should().ThrowAsync<OperationCanceledException>();
    }
}
