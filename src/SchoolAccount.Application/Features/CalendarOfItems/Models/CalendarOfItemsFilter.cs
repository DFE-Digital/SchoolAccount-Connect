using System.Collections.ObjectModel;
using SchoolAccount.Application.Abstractions;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Models;

public class CalendarOfItemsFilter(Collection<FilterRequest> items) : Collection<FilterRequest>(items), IFilter
{
    public CalendarOfItemsFilter(IEnumerable<FilterRequest> items)
        : this(items.ToCollection()) { }
}
