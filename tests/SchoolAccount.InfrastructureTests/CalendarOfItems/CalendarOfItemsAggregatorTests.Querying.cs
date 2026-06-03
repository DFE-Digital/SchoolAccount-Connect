using AwesomeAssertions;
using NSubstitute;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.InfrastructureTests.Extensions;

namespace SchoolAccount.InfrastructureTests.CalendarOfItems;

public partial class GenericQueryAggregatorTests
{
    [Theory]
    [InlineData(100, 10, 1, CalendarOfItemsViewModes.Forward)]
    [InlineData(0, 10, 1, CalendarOfItemsViewModes.Forward)]
    [InlineData(100, 10, 1, CalendarOfItemsViewModes.Backward)]
    public async Task Running_a_standard_query_it_will_always_succeed(
        int poolSize,
        int pageSize,
        int pageNumber,
        CalendarOfItemsViewModes mode
    )
    {
        // Arrange
        var queryFactory = Make.Factory.Query(
            CalendarOfItemsQueryTypes.SubTask,
            CalendarOfItemsRowExtensions.Collection(DefaultRange).Populate(poolSize).Build()
        );
        var sut = Make.Aggregator([queryFactory]);
        var criteria = Make.Criteria(
            toQuery: CalendarOfItemsQueryTypes.SubTask,
            range: DefaultRange,
            pageSize: pageSize,
            pageNumber: pageNumber,
            viewModes: mode,
            sortMode: CalendarOfItemsSortMode.NotSpecified
        );

        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue(because: "Should always succeed regardless of query state");
        result.Value.Should().NotBeNull(because: "Should always have a object regardless of query state");
        result
            .Value.Payload.Count.Should()
            .Be(Math.Min(poolSize, pageSize), because: "Should always match the query limit");
        result.Value.PageSize.Should().Be(pageSize, because: "Should always match the query limit");
        result.Value.PageNumber.Should().Be(pageNumber, because: "Should always match the query request");
    }

    [Fact]
    public async Task When_filtering_a_query_with_assignable_properties_the_query_factory_is_correctly_called()
    {
        // Arrange
        var inboundFilter = CalendarOfItemsFilterExtensions.Create(
            FilterRequestExtensions.Create("Name").WithValue("Test")
        );
        var queryFactory = Make.Factory.Query(
            CalendarOfItemsQueryTypes.SubTask,
            CalendarOfItemsRowExtensions
                .Collection(DefaultRange)
                .Populate(10, configure: x => x.Name = "Test", where: x => x is 5 or 7)
                .Build()
        );
        var sut = Make.Aggregator([queryFactory]);
        var criteria = Make.Criteria(filter: inboundFilter);

        // Act
        await sut.Query(criteria, CancellationToken.None);

        // Assert
        queryFactory
            .Received(1)
            .Query(Arg.Is<CalendarOfItemsFilter>(f => f.All(x => x.Field == "Name")), Arg.Any<FieldSelectorMapping>());
    }
}
