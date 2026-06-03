using System.Collections;
using System.Collections.ObjectModel;
using SchoolAccount.Application.Abstractions;
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

public class QueryAggregator(
    FilterableFieldRegistry filterRegistry,
    IEnumerable<IFilterableFactory<IQueryRow>> filterFactories
) : IQueryAggregator
{
    public async Task<Result<QueryPagedResult<TRow>>> Query<TRow>(
        IEnumerable<IQueryFactory<TRow>> factories,
        GenericQueryCriteria<TRow> criteria,
        CancellationToken cancellationToken = default
    )
        where TRow: IQueryRow
    {
        var validator = new QueryAggregatorValidator<TRow>();
        var validation = await validator.ValidateAsync(criteria, cancellationToken);

        if (!validation.IsValid)
        {
            return validation.ToResult<QueryPagedResult<TRow>>();
        }

        ConsolidateFilters(criteria);

        var query = factories
            .Select(f => f.Query(criteria.Filter, filterRegistry.All))
            .Aggregate((current, next) => current.Union(next))
            .Where(QueryRowSpecifications.IsWithinDateRange<TRow>(criteria.Range));

        var result = await query
            .WithSorting(criteria.CustomOrderByFunction)
            .PaginateAsync(criteria.PageSize, criteria.PageNumber, cancellationToken);

        return Result.Success(
            new QueryPagedResult<TRow>(
                criteria, 
                result, 
                await ProduceAndCorrelateFilter(criteria, query)));
    }
    
    private void ConsolidateFilters<TRow>(GenericQueryCriteria<TRow> criteria)
        where TRow: IQueryRow
    {
        foreach (
            var registrar in filterRegistry
                .Registrars.OfType<IFilterableAndConsolidateRegistrar>()
        )
        {
            registrar.ConsolidateFilters(criteria.Filter);
        }
    }

    private async Task<Collection<Filterable>> ProduceAndCorrelateFilter<TRow>(GenericQueryCriteria<TRow> criteria,
        IQueryable<TRow> query)
        where TRow: IQueryRow
    {
        if (criteria.PopulateFilterOptions)
        {
            return [];
        }
        
        var filters = await FilterFields.GetAvailableFiltersAsync<TRow>(
            (IList<IFilterableFactory<TRow>>)filterFactories,
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

        return filters.ToCollection();
    }
}
