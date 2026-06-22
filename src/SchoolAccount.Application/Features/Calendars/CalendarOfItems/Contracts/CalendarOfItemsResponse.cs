using System.Collections.ObjectModel;
using SchoolAccount.Application.Common;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Calendars.CalendarOfItems.Contracts;

public class CalendarOfItemsResponse(
    CalendarOfItemsCriteria criteria,
    PagedResult<CalendarOfItemsRow> payload,
    Collection<Filterable> filter
)
{
    public CalendarOfItemsViewModes ViewModes { get; } = criteria.ViewModes;

    public DateTime GeneratedDate { get; } = DateTime.UtcNow;

    public DateOnlyRange QueryRange { get; } = criteria.Range;

    public Collection<Filterable> Filter { get; } = filter;

    public PagedResult<CalendarOfItemsRow> Payload { get; } = payload;
}
