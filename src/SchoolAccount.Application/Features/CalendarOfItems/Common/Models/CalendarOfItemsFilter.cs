using System.Collections.ObjectModel;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Shared.Filtering.Interfaces;
using SchoolAccount.Application.Features.Shared.Filtering.Models;

namespace SchoolAccount.Application.Features.CalendarOfItems.Common.Models;

public class CalendarOfItemsFilter(Collection<FilterRequest> items) : Collection<FilterRequest>(items), IFilter
{
    public CalendarOfItemsFilter(IEnumerable<FilterRequest> items)
        : this(items.ToCollection()) { }
}
