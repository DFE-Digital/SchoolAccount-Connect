using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Models;

public class CalendarOfItemsExtensionNode : ExtensionNode<long>;

public enum CalendarOfItemsExtensionNodeType
{
    NotSpecified = 0,
    Tag = 1,
    Type = 2,
}