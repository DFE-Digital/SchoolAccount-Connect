using System.Collections.ObjectModel;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Shared.Filtering;

namespace SchoolAccount.Application.Features.CalendarOfItems.Models;

public class CalendarOfItemsFilter(Collection<FilterRequest> items) : Collection<FilterRequest>(items)
{
    public CalendarOfItemsFilter(IEnumerable<FilterRequest> items) : this(items.ToCollection())
    {
    }
}