using SchoolAccount.Application.Features.Shared.Filtering;

namespace SchoolAccount.Web.Connect.Extensions;

public static class FilterableExtension
{
    public static string? DetermineTemplate(this Filterable filterable)
    {
        return filterable.Type switch
        {
            FilterableItemType.Checkbox => "DisplayComponents/Filter/Checkbox",
            _ => null
        };
    }

    public static bool HasTemplate(this Filterable filterable)
    {
        return !string.IsNullOrEmpty(filterable.DetermineTemplate());
    }
}