using SchoolAccount.Application.Features.CalendarOfItems.Models;

namespace SchoolAccount.Infrastructure.Helpers.Filtering.Interfaces;

public interface IFilterableRegistrar
{
    Type TypeBeingRegistered { get; }
    FieldSelector FieldSelectorsBeingRegistered { get; }
    void ConsolidateFilters(CalendarOfItemsFilter filter);
}
