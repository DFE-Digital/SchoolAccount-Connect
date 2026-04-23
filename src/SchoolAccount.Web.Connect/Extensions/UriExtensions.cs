using System.Globalization;
using Microsoft.AspNetCore.WebUtilities;

namespace SchoolAccount.Web.Connect.Extensions;

public static class UriExtensions
{
    public static Uri SetQueryParam(this Uri uri, string key, string value)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentException.ThrowIfNullOrEmpty(key);

        var queryParameters = QueryHelpers.ParseQuery(uri.Query);

        queryParameters[key] = value;

        var builder = new UriBuilder(uri);

        builder.Query = QueryHelpers.AddQueryString(string.Empty, queryParameters);

        return builder.Uri;
    }

    public static Uri SetQueryParam(this Uri uri, string key, int value)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentException.ThrowIfNullOrEmpty(key);

        var valueAsString = value.ToString(CultureInfo.InvariantCulture);
        return uri.SetQueryParam(key, valueAsString);
    }

    public static Uri RemoveQueryParam(this Uri uri, string key, string value)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentException.ThrowIfNullOrEmpty(key);

        var queryParameters = QueryHelpers.ParseQuery(uri.Query);

        if (queryParameters.TryGetValue(key, out var existing))
        {
            var remainingValues = existing
                .Where(v => !string.Equals(v, value, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (remainingValues.Length > 0)
            {
                queryParameters[key] = remainingValues;
            }
            else
            {
                queryParameters.Remove(key);
            }
        }

        var builder = new UriBuilder(uri);
        builder.Query = QueryHelpers.AddQueryString(string.Empty, queryParameters);

        return builder.Uri;
    }

    public static Uri RemoveQueryParam(this Uri uri, string key)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentException.ThrowIfNullOrEmpty(key);

        var queryParameters = QueryHelpers.ParseQuery(uri.Query);

        queryParameters.Remove(key);

        var builder = new UriBuilder(uri);
        builder.Query = QueryHelpers.AddQueryString(string.Empty, queryParameters);

        return builder.Uri;
    }

    public static Uri RemoveQueryParamsStartingWith(this Uri uri, string prefix)
    {
        ArgumentNullException.ThrowIfNull(uri);
        ArgumentException.ThrowIfNullOrEmpty(prefix);

        var queryParameters = QueryHelpers.ParseQuery(uri.Query);

        var keysToRemove = queryParameters
            .Keys.Where(k => k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            .ToList();

        foreach (var k in keysToRemove)
            queryParameters.Remove(k);

        var builder = new UriBuilder(uri);
        builder.Query = QueryHelpers.AddQueryString(string.Empty, queryParameters);

        return builder.Uri;
    }
}
