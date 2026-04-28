using AwesomeAssertions;
using NSubstitute;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Infrastructure.Helpers.Filtering;
using SchoolAccount.InfrastructureTests.Extensions;

namespace SchoolAccount.InfrastructureTests.CalendarOfItems;

public partial class CalendarOfItemsAggregatorTests
{
    [Fact]
    public async Task Ensure_a_QueryType_identifier_is_assigned_to_the_query()
    {
        // Assert
        var queryFactory = Make.Factory.Query(CalendarOfItemsQueryTypes.SubTask, []);
        var sut = Make.Aggregator([queryFactory]);
        var criteria = Make.Criteria(toQuery: CalendarOfItemsQueryTypes.None);
        
        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        result.IsSuccess
            .Should()
            .BeFalse(because: "There was no provided ToQuery property within the criteria which is required");

        queryFactory.ShouldHave()
            .NotReceived(
                x => x.Query(Arg.Any<CalendarOfItemsFilter>(), Arg.Any<FieldSelectorMapping>()),
                because: "No factory should of been called to commence a query");
        
        result.Error.Should().NotBeNull(because: "A error response should of been returned");
    }
}