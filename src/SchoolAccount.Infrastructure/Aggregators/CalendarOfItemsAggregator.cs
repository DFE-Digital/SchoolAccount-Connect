using System.Collections;
using SchoolAccount.Application.Abstractions;
using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Domain.Tasks;
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

    private void ConsolidateFilters<TFilter>(CalendarOfItemsCriteria criteria)
        where TFilter : IFilter
    {
        var types = MapTypeToEntity((FilterableEntities)criteria.ToQuery);
        foreach (
            var registrar in filterRegistry
                .Registrars.Where(x => types.Contains(x.TypeBeingRegistered))
                .OfType<IFilterableRegistrar<TFilter>>()
        )
        {
            registrar.ConsolidateFilters(criteria.Filter);
        }
    }

    private static List<Type> MapTypeToEntity(FilterableEntities entity)
    {
        var types = new List<Type>();

        if (entity.HasFlag(FilterableEntities.SubTask))
        {
            types.Add(typeof(SubTaskEntity));
        }

        if (entity.HasFlag(FilterableEntities.Task))
        {
            types.Add(typeof(TaskEntity));
        }

        return types;
    }

    private async Task<List<Filterable>> ProduceAndCorrelateFilter(
        CalendarOfItemsCriteria criteria,
        IQueryable<CalendarOfItemsRow> query
    )
    {
        var filters = await FilterFields.GetAvailableFiltersAsync(
            (FilterableEntities)criteria.ToQuery,
            filterFactories,
            query
        );

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

    public async Task<Result<CalendarOfItemsPagedResult>> Query<TFilter>(
        CalendarOfItemsCriteria criteria,
        CancellationToken cancellationToken = default
    )
        where TFilter : IFilter
    {
        var validation = await _validator.ValidateAsync(criteria, cancellationToken);

        if (!validation.IsValid)
        {
            return validation.ToResult<CalendarOfItemsPagedResult>();
        }

        ConsolidateFilters<TFilter>(criteria);

        var factories = queryFactoryResolver.GetFactoriesByType(criteria.ToQuery);

        var query = factories
            .Select(f => f.Query(criteria.Filter, filterRegistry.All))
            .Aggregate((current, next) => current.Union(next))
            .Where(CalendarOfItemsRowSpecifications.IsWithinDateRange(criteria.Range));

        var result = await query
            .WithSorting(criteria.ViewModes, criteria.SortMode, criteria.CustomOrderByFunction)
            .PaginateAsync(criteria.PageSize, criteria.PageNumber, cancellationToken);

        var filters = criteria.PopulateFilterOptions
            ? (await ProduceAndCorrelateFilter(criteria, query)).ToCollection()
            : [];

        return Result.Success(new CalendarOfItemsPagedResult(criteria, result, filters));
    }
}
