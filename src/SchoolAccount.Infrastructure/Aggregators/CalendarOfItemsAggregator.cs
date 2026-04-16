using System.Collections;
using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Infrastructure.Extensions;
using SchoolAccount.Infrastructure.Helpers.Filtering;
using SchoolAccount.Infrastructure.Helpers.Filtering.Interfaces;
using SchoolAccount.Infrastructure.Resolvers;
using SchoolAccount.Infrastructure.Specifications;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure.Aggregators;

public class CalendarOfItemsAggregator(
    CalendarOfItemsQueryFactoryResolver queryFactoryResolver,
    FilterableFieldRegistry filterRegistry,
    IEnumerable<IFilterableFactory> filterFactories
) : ICalendarOfItemsAggregator
{
    private readonly CalendarOfItemsAggregatorValidator _validator = new(queryFactoryResolver);

    private void ConsolidateFilters(CalendarOfItemsFilter filter)
    {
        foreach (var registrar in filterRegistry.Registrars.OfType<IFilterableRegistrar<CalendarOfItemsFilter>>())
        {
            registrar.ConsolidateFilters(filter);
        }
    }

    private async Task<List<Filterable>> ProduceAndCorrelateFilter(
        CalendarOfItemsCriteria criteria,
        IQueryable<CalendarOfItemsRow> query
    )
    {
        var filters = await FilterFields.GetAvailableFiltersAsync(FilterableEntities.SubTask, filterFactories, query);

        if (filters.Count == 0)
        {
            return [];
        }

        foreach (var items in filters)
        {
            var requested = criteria.Filter.FirstOrDefault(x => x.Field == items.Field);

            if (requested?.Value is not IList list)
            {
                continue;
            }

            var casted = list.Cast<object>().ToList();

            foreach (var value in items.Values)
            {
                value.IsSelected = casted.Select(b => b.ToString()).Contains(value.Value);
            }
        }

        return filters;
    }

    public async Task<Result<CalendarOfItemsPagedResult>> Query(
        CalendarOfItemsCriteria criteria,
        CancellationToken cancellationToken = default
    )
    {
        var validation = await _validator.ValidateAsync(criteria, cancellationToken);

        if (!validation.IsValid)
        {
            return validation.ToResult<CalendarOfItemsPagedResult>();
        }

        ConsolidateFilters(criteria.Filter);

        var factories = queryFactoryResolver.GetFactoriesByType(criteria.ToQuery);

        var query = factories
            .Select(f => f.Query(criteria.Filter, filterRegistry.All))
            .Aggregate((current, next) => current.Union(next))
            .Where(CalendarOfItemsRowSpecifications.IsWithinDateRange(criteria.Range));

        var result = await query
            .WithSorting(criteria.ViewModes, criteria.SortMode)
            .PaginateAsync(criteria.PageSize, criteria.PageNumber, cancellationToken);

        var filters = criteria.IncludeFilterOptions
            ? (await ProduceAndCorrelateFilter(criteria, query)).ToCollection()
            : [];

        return Result.Success(new CalendarOfItemsPagedResult(criteria, result, filters));
    }
}
