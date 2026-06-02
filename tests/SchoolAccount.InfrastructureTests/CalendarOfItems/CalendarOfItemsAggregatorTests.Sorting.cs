using AwesomeAssertions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.InfrastructureTests.Extensions;

namespace SchoolAccount.InfrastructureTests.CalendarOfItems;

public partial class QueryAggregatorTests
{
    [Theory]
    [InlineData(CalendarOfItemsViewModes.Forward, true)]
    [InlineData(CalendarOfItemsViewModes.Backward, false)]
    public async Task When_doing_a_directional_query_sorting_is_correctly_determined(
        CalendarOfItemsViewModes viewModes,
        bool isAscending
    )
    {
        // Assert
        var queryFactory = Make.Factory.Query(
            CalendarOfItemsQueryTypes.SubTask,
            CalendarOfItemsRowExtensions.Collection(DefaultRange).Populate(10).Build()
        );
        var sut = Make.Aggregator([queryFactory]);
        var criteria = Make.Criteria(viewModes: viewModes);

        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        result
            .Value.Payload.Should()
            .BeInOrder(
                x => x.SortDate,
                isAscending,
                because: "Should be sorted ascending if looking forward or descending if backward"
            );
    }

    [Fact]
    public async Task Ensure_when_a_custom_ordering_method_is_passed_that_it_is_correctly_ordering()
    {
        // Assert
        var pool = CalendarOfItemsRowExtensions.Collection(DefaultRange).Populate(10).Build();
        var queryFactory = Make.Factory.Query(CalendarOfItemsQueryTypes.SubTask, pool);
        var sut = Make.Aggregator([queryFactory]);
        var criteria = Make.Criteria(
            viewModes: CalendarOfItemsViewModes.Custom,
            customOrderByFunction: x => x.OrderBy(r => r.Name)
        );

        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        result
            .Value.Payload.Should()
            .BeInOrder(x => x.Name, true, because: "We asked for all the rows to be in alphabetical order");
    }
}
