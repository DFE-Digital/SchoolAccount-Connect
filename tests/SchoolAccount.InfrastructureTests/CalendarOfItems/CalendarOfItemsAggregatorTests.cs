using NSubstitute;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Infrastructure.Abstraction;
using SchoolAccount.Infrastructure.Aggregators;
using SchoolAccount.Infrastructure.Helpers.Filtering;
using SchoolAccount.Infrastructure.Helpers.Filtering.Interfaces;
using SchoolAccount.Infrastructure.Resolvers;
using SchoolAccount.InfrastructureTests.Extensions;
using SchoolAccount.Kernel;

namespace SchoolAccount.InfrastructureTests.CalendarOfItems;

public partial class CalendarOfItemsAggregatorTests
{
    private static readonly DateOnly Today = DateOnlyExtensions.Today;
    private static readonly DateOnlyRange DefaultRange = new(Today.AddDays(-30), Today.AddDays(30));

    private static class Make
    {
        public static CalendarOfItemsCriteria Criteria(
            CalendarOfItemsQueryTypes toQuery = CalendarOfItemsQueryTypes.SubTask,
            DateOnlyRange? range = null,
            int pageSize = 10,
            int pageNumber = 1,
            CalendarOfItemsViewModes viewModes = CalendarOfItemsViewModes.Forward,
            CalendarOfItemsSortMode sortMode = CalendarOfItemsSortMode.NotSpecified,
            CalendarOfItemsFilter? filter = null,
            bool includeFilterOptions = false,
            CalendarOfItemsOrderFunction? customOrderByFunction = null
        )
        {
            return new CalendarOfItemsCriteria
            {
                ToQuery = toQuery,
                Range = range ?? DefaultRange,
                PageSize = pageSize,
                PageNumber = pageNumber,
                ViewModes = viewModes,
                SortMode = sortMode,
                Filter = filter ?? new CalendarOfItemsFilter([]),
                IncludeFilterOptions = includeFilterOptions,
                CustomOrderByFunction = customOrderByFunction,
            };
        }

        public static class Factory
        {
            public static ICalendarOfItemsQueryFactory Query(
                CalendarOfItemsQueryTypes type,
                IEnumerable<CalendarOfItemsRow> rows
            )
            {
                var factory = Substitute.For<ICalendarOfItemsQueryFactory>();
                factory.IsQueryableFor(type).Returns(true);
                factory
                    .Query(Arg.Any<CalendarOfItemsFilter>(), Arg.Any<FieldSelectorMapping>())
                    .Returns(rows.AsTestAsyncQueryable());
                return factory;
            }

            public static IFilterableFactory Filterable(params Filterable[] filterables)
            {
                var filterFactory = Substitute.For<IFilterableFactory<CalendarOfItemsRow>>();
                filterFactory.IsCreatorFor(Arg.Any<FilterableEntities>()).Returns(true);
                filterFactory
                    .GetAvailableFiltersAsync(Arg.Any<IQueryable<CalendarOfItemsRow>>())
                    .Returns(filterables.ToList());
                return filterFactory;
            }
        }

        public static CalendarOfItemsAggregator Aggregator(
            IEnumerable<ICalendarOfItemsQueryFactory> factories,
            FilterableFieldRegistry? registry = null,
            IEnumerable<IFilterableFactory>? filterFactories = null
        )
        {
            var resolver = new CalendarOfItemsQueryFactoryResolver(factories);
            var reg = registry ?? new FilterableFieldRegistry([]);
            var ff = filterFactories ?? [];
            return new CalendarOfItemsAggregator(resolver, reg, ff);
        }
    }
}
