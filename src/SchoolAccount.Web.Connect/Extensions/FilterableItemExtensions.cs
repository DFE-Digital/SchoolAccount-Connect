using SchoolAccount.Application.Features.Shared.Filtering;

namespace SchoolAccount.Web.Connect.Extensions;

public static class FilterableItemExtensions
{
    public static string GetUriWithoutPropertyValue(
        this FilterableItem item,
        string baseUri,
        IHostEnvironment environment,
        string key
    )
    {
        return UriExtensions.RemoveByValueQuery(baseUri, environment, ($"filters[{key}]", item.Value));
    }
}
