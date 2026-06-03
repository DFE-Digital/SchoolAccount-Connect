using System.Collections;
using System.Collections.ObjectModel;
using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Application.Features.Shared.Filtering.Interfaces;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Application.Specifications;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Aggregators;

public class GenericQueryAggregator(
    FilterableFieldRegistry filterRegistry
) : IQueryAggregator
{
    public async Task<Result<GenericQueryPagedResult<TRow>>> Query<TEntity, TRow>(
        IList<IQueryFactory<TEntity, TRow>> queryFactories,
        IList<IFilterableFactory> filterableFactories,
        GenericQueryCriteria<TRow> criteria,
        CancellationToken cancellationToken = default
    )
        where TEntity: IEntity
        where TRow: IQueryRow
    {
        var validator = new QueryAggregatorValidator<TRow>();
        var validation = await validator.ValidateAsync(criteria, cancellationToken);

        if (!validation.IsValid)
        {
            return validation.ToResult<GenericQueryPagedResult<TRow>>();
        }

        ConsolidateFilters(queryFactories, criteria);

        var query = queryFactories
            .Select(f => f.Query(criteria.Filter, filterRegistry.All))
            .Aggregate((current, next) => current.Union(next))
            .Where(QueryRowSpecifications.IsWithinDateRange<TRow>(criteria.Range));

        var result = await query
            .WithSorting(criteria.CustomOrderByFunction)
            .PaginateAsync(criteria.PageSize, criteria.PageNumber, cancellationToken);

        return Result.Success(
            new GenericQueryPagedResult<TRow>(
                criteria, 
                result, 
                await ProduceAndCorrelateFilter(filterableFactories, criteria, query)));
    }
    
    private void ConsolidateFilters<TEntity, TRow>(IEnumerable<IQueryFactory<TEntity, TRow>> factories, GenericQueryCriteria<TRow> criteria)
        where TEntity: IEntity
        where TRow: IQueryRow
    {
        var consumableRegistrars = filterRegistry.Consolidates
            .Where(x => factories.Any(f => f.TypeBeingRegistered == x.TypeBeingRegistered));
        
        foreach (var registrar in consumableRegistrars)
        {
            registrar.ConsolidateFilters(criteria.Filter);
        }
    }

    private static async Task<Collection<Filterable>> ProduceAndCorrelateFilter<TRow>(
        IEnumerable<IFilterableFactory> filterFactories,
        GenericQueryCriteria<TRow> criteria,
        IQueryable<TRow> query)
        where TRow: IQueryRow
    {
        if (!criteria.PopulateFilterOptions)
        {
            return [];
        }
        
        var filters = await FilterFields.GetAvailableFiltersAsync(filterFactories.ToCollection(), query);

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

        return filters.ToCollection();
    }
}
