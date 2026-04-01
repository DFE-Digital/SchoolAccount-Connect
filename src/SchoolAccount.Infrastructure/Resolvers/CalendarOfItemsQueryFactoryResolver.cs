using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Infrastructure.Abstraction;

namespace SchoolAccount.Infrastructure.Resolvers;

public class CalendarOfItemsQueryFactoryResolver(IEnumerable<ICalendarOfItemsQueryFactory> factories)
{
    private readonly IReadOnlyList<ICalendarOfItemsQueryFactory> _factories = factories.ToList();

    public bool IsThereADefinedFactory(CalendarOfItemsQueryTypes type)
    {
        return _factories.Any(x => x.IsQueryableFor(type));
    }

    public IEnumerable<ICalendarOfItemsQueryFactory> GetFactoriesByType(CalendarOfItemsQueryTypes type)
    {
        return _factories.Where(x => x.IsQueryableFor(type));
    }
}
