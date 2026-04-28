using SchoolAccount.InfrastructureTests.Builders;

namespace SchoolAccount.InfrastructureTests.Extensions;

public static class CalendarOfItemsExtensionNodeExtensions
{
    public static CalendarOfItemsExtensionNodeBuilder Create(long id)
    {
        return new CalendarOfItemsExtensionNodeBuilder(id);
    }
}