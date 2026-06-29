using System.Collections;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Logging;
using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Abstractions.Pipelines;
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
    ILogger<GenericQueryAggregator> logger,
    FilterableFieldRegistry filterRegistry
) : IQueryAggregator
{
    public async Task<Result<GenericQueryPagedResult<TRow>>> Query<TRow>(
        IQueryFactoryPipeline<TRow> factoryPipeline,
        IFilterablePipeline filterPipeline,
        GenericQueryCriteria<TRow> criteria,
        CancellationToken cancellationToken = default
    )
        where TRow : IQueryRow
    {
        var validator = new QueryAggregatorValidator<TRow>();
        var validation = await validator.ValidateAsync(criteria, cancellationToken);

        if (!validation.IsValid)
        {
            return validation.ToResult<GenericQueryPagedResult<TRow>>();
        }

        ConsolidateFilters(factoryPipeline.Factories, criteria);

        List<IEnumerable<TRow>> runs = [];
        var total = 0;

        foreach (var factory in factoryPipeline.Factories)
        {
            try
            {
                var outcome = await factory.Query(criteria, filterRegistry.All, cancellationToken);
                var localised = outcome.Payload
                    .Where(QueryRowSpecifications.IsWithinDateRange<TRow>(criteria.Range))
                    .ToList();

                runs.Add(localised);
                total += localised.Count;
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "Failed to query {FactoryType}", factory.TypeBeingRegistered);
            }
        }

        var query = runs
            .Aggregate((current, next) => current.Union(next))
            .ToList();

        var result = query
            .WithSorting(criteria.CustomOrderByFunction)
            .Paginate(criteria.PageSize, criteria.PageNumber, total);

        return Result.Success(
            new GenericQueryPagedResult<TRow>(
                criteria,
                result,
                await ProduceAndCorrelateFilter(filterPipeline.Factories, criteria)
            )
        );
    }

    private void ConsolidateFilters<TRow>(
        IEnumerable<IQueryFactory<TRow>> factories,
        GenericQueryCriteria<TRow> criteria
    )
        where TRow : IQueryRow
    {
        var consumableRegistrars = filterRegistry.Consolidates.Where(x =>
            factories.Any(f => f.TypeBeingRegistered == x.TypeBeingRegistered)
        );

        foreach (var registrar in consumableRegistrars)
        {
            registrar.ConsolidateFilters(criteria.Filter);
        }
    }

    private static async Task<Collection<Filterable>> ProduceAndCorrelateFilter<TRow>(
        IEnumerable<IFilterableFactory> filterFactories,
        GenericQueryCriteria<TRow> criteria,
        IQueryable<TRow>? query = null
    )
        where TRow : IQueryRow
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
