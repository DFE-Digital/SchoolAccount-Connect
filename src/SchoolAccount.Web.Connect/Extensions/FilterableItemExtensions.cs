using SchoolAccount.Application.Features.Shared.Filtering.Models;

namespace SchoolAccount.Web.Connect.Extensions;

public static class FilterableItemExtensions
{
    public static Uri GetUriWithoutPropertyValue(this FilterableItem item, Uri baseUri, string key)
    {
        var filterKey = $"filters[{key}]";

        return baseUri.RemoveQueryParam(filterKey, item.Value);
    }
}
