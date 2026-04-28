using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.InfrastructureTests.Builders;

namespace SchoolAccount.InfrastructureTests.Extensions;

public static class CalendarOfItemsFilterExtensions
{
    public static CalendarOfItemsFilter Create(IEnumerable<FilterRequestBuilder> filters)
    {
        return new CalendarOfItemsFilter(filters.Select(x => x.Build()));
    }

    public static CalendarOfItemsFilter Create(params FilterRequestBuilder[] filters)
    {
        return new CalendarOfItemsFilter(filters.Select(x => x.Build()));
    }
}
