using System.Data.SqlTypes;
using SchoolAccount.Application.Abstractions.Infrastructure;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Infrastructure.Extensions;
using SchoolAccount.Infrastructure.Resolvers;
using SchoolAccount.Infrastructure.Specifications;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.CalendarOfItems;

namespace SchoolAccount.Infrastructure.Aggregators;

public class CalendarOfItemsAggregator(CalendarOfItemsQueryFactoryResolver queryFactoryResolver)
    : ICalendarOfItemsAggregator
{
    private readonly CalendarOfItemsAggregatorValidator _validator = new(queryFactoryResolver);

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

        var factories = queryFactoryResolver.GetFactoriesByType(criteria.ToQuery);

        var query = factories
            .Select(f => f.Query())
            .Aggregate((current, next) => current.Union(next))
            .Where(CalendarOfItemsRowSpecifications.IsWithinDateRange(criteria.Range));

        var result = await query
            .WithSorting(criteria.ViewMode, criteria.SortMode)
            .PaginateAsync(criteria.PageSize, criteria.PageNumber, cancellationToken);

        return Result.Success(new CalendarOfItemsPagedResult(criteria, result));
    }
}
