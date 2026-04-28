using AwesomeAssertions;
using NSubstitute;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Infrastructure.Helpers.Filtering.Interfaces;
using SchoolAccount.InfrastructureTests.Extensions;

namespace SchoolAccount.InfrastructureTests.CalendarOfItems;

public partial class CalendarOfItemsAggregatorTests
{
    [Fact]
    public async Task Only_include_filterable_options_when_are_requested_for()
    {
        // Arrange
        var queryFactory = Make.Factory.Query(
            CalendarOfItemsQueryTypes.SubTask,
            [CalendarOfItemsRowExtensions.Create(Today)]
        );
        var sut = Make.Aggregator([queryFactory]);
        var criteria = Make.Criteria(includeFilterOptions: false);

        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue(because: "Operation should still successfully complete");
        result.Value.Filter.Should().BeEmpty(because: "Only returns a filterable items list when requested for");
    }

    [Fact]
    public async Task Only_call_GetAvailableFiltersAsync_if_there_is_any_factories_configured()
    {
        // Arrange
        var emulationOfNoFilterableFactories = Substitute.For<IFilterableFactory<CalendarOfItemsRow>>();
        var queryFactory = Make.Factory.Query(
            CalendarOfItemsQueryTypes.SubTask,
            [CalendarOfItemsRowExtensions.Create(Today)]
        );
        var sut = Make.Aggregator([queryFactory], filterFactories: [emulationOfNoFilterableFactories]);
        var criteria = Make.Criteria(includeFilterOptions: true);

        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue(because: "The query should still complete successfully");
        result.Value.Filter.Should().BeEmpty(because: "There are now filterable items configured");
        await emulationOfNoFilterableFactories
            .DidNotReceive()
            .GetAvailableFiltersAsync(Arg.Any<IQueryable<CalendarOfItemsRow>>());
    }

    [Fact]
    public async Task Ensure_filterable_items_within_catalog_are_present_when_requested()
    {
        // Arrange
        Filterable filterableOptions = FilterableExtensions
            .Create("Status")
            .WithValues(FilterableItemExtensions.Create("Active").UnSelected());
        var filterFactory = Make.Factory.Filterable(filterableOptions);
        var queryFactory = Make.Factory.Query(
            CalendarOfItemsQueryTypes.SubTask,
            [CalendarOfItemsRowExtensions.Create(Today)]
        );
        var sut = Make.Aggregator([queryFactory], filterFactories: [filterFactory]);
        var criteria = Make.Criteria(includeFilterOptions: true);

        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue(because: "Operation should still successfully complete");
        result
            .Value.Filter.Should()
            .ContainSingle()
            .Which.Field.Should()
            .Be(
                filterableOptions.Field,
                because: "We have only included one item from custom filterable factory presented to the engine"
            );
    }

    [Fact]
    public async Task If_a_filter_query_doesnt_match_item_in_filter_catelog_nothing_is_selected()
    {
        // Arrange
        Filterable filterable = FilterableExtensions
            .Create("Status")
            .WithValues(FilterableItemExtensions.Create("Archived"));
        var filterFactory = Make.Factory.Filterable(filterable);
        var queryFactory = Make.Factory.Query(
            CalendarOfItemsQueryTypes.SubTask,
            [CalendarOfItemsRowExtensions.Create(Today)]
        );
        var sut = Make.Aggregator([queryFactory], filterFactories: [filterFactory]);
        var criteria = Make.Criteria(
            includeFilterOptions: true,
            filter: CalendarOfItemsFilterExtensions.Create(
                FilterRequestExtensions.Create("Status").WithValues("Active")
            )
        );

        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue(because: "Operation should still successfully complete");
        result
            .Value.Filter.Should()
            .BeEquivalentTo(
                [filterable],
                because: "There should only be one item within the list as only one was registered and nothing should "
                    + "be selected"
            );
        result
            .Value.Filter.Should()
            .ContainSingle(f => f.Field == "Status")
            .Which.Values.Should()
            .ContainSingle(v => v.Value == "Archived")
            .Which.IsSelected.Should()
            .BeFalse();
    }

    [Fact]
    public async Task String_query_filters_are_not_supported_ensure_that_query_items_are_given_as_enumerable()
    {
        // Arrange
        var filterFactory = Make.Factory.Filterable(
            FilterableExtensions.Create("Status").WithValues(FilterableItemExtensions.Create("Active"))
        );
        var factoryFactory = Make.Factory.Query(
            CalendarOfItemsQueryTypes.SubTask,
            [CalendarOfItemsRowExtensions.Create(Today)]
        );
        var sut = Make.Aggregator([factoryFactory], filterFactories: [filterFactory]);
        var criteria = Make.Criteria(
            includeFilterOptions: true,
            filter: CalendarOfItemsFilterExtensions.Create(FilterRequestExtensions.Create("Status").WithValue("Active"))
        );

        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue(because: "Operation should still successfully complete");
        result
            .Value.Filter.Should()
            .ContainSingle(f => f.Field == "Status")
            .Which.Values.Should()
            .ContainSingle(v => v.Value == "Active")
            .Which.IsSelected.Should()
            .BeFalse();
    }

    [Fact]
    public async Task Ensure_even_if_complex_filter_catelog_that_if_a_user_has_not_queried_against_valid_item_nothing_is_selected()
    {
        // Arrange
        List<Filterable> filterables =
        [
            FilterableExtensions
                .Create("Status")
                .WithValues(
                    FilterableItemExtensions.Create("Active"),
                    FilterableItemExtensions.Create("Inactive"),
                    FilterableItemExtensions.Create("Urgent"),
                    FilterableItemExtensions.Create("Dismissed")
                ),
            FilterableExtensions
                .Create("Colours")
                .WithValues(
                    FilterableItemExtensions.Create("Red"),
                    FilterableItemExtensions.Create("Blue"),
                    FilterableItemExtensions.Create("Green"),
                    FilterableItemExtensions.Create("Pink"),
                    FilterableItemExtensions.Create("Orange"),
                    FilterableItemExtensions.Create("Purple"),
                    FilterableItemExtensions.Create("Black"),
                    FilterableItemExtensions.Create("White")
                ),
            FilterableExtensions
                .Create("SchoolType")
                .WithDisplayName("School Type")
                .WithValues(
                    FilterableItemExtensions.Create("Primary"),
                    FilterableItemExtensions.Create("Secondary"),
                    FilterableItemExtensions.Create("College"),
                    FilterableItemExtensions.Create("University")
                ),
        ];
        var filterFactory = Make.Factory.Filterable(filterables.ToArray());
        var factoryFactory = Make.Factory.Query(
            CalendarOfItemsQueryTypes.SubTask,
            [CalendarOfItemsRowExtensions.Create(Today)]
        );
        var sut = Make.Aggregator([factoryFactory], filterFactories: [filterFactory]);
        var criteria = Make.Criteria(
            includeFilterOptions: true,
            filter: CalendarOfItemsFilterExtensions.Create(
                FilterRequestExtensions.Create("Status").WithValues("Active")
            )
        );

        // Act
        var result = await sut.Query(criteria, CancellationToken.None);

        // Assert
        result
            .Value.Filter.Should()
            .BeEquivalentTo(
                filterables,
                because: "There should only be one item within the list as only one was registered and nothing should "
                    + "be selected"
            );
        result
            .Value.Filter.Should()
            .Contain(f => f.Field == "SchoolType")
            .Which.Values.Should()
            .Contain(v => v.Value == "Primary")
            .Which.IsSelected.Should()
            .BeFalse();
    }
}
