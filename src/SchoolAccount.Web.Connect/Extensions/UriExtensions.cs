using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace SchoolAccount.Web.Connect.Extensions;

public static class UriExtensions
{
    private static string ToQueryString(IHostEnvironment env, Uri uri, List<KeyValuePair<string, string?>> query)
    {
        var constructedUri = env.IsDevelopment()
            ? $"{uri.Scheme}://{uri.Host}:{uri.Port}"
            : $"{uri.Scheme}://{uri.Host}";

        return QueryHelpers.AddQueryString($"{constructedUri}{uri.AbsolutePath}", query);
    }

    private static List<KeyValuePair<string, string?>> DetermineQueryString(
        this Uri uri,
        IHostEnvironment env,
        Func<KeyValuePair<string, StringValues>, bool> predicate
    )
    {
        return QueryHelpers
            .ParseQuery(uri.Query)
            .Where(predicate)
            .SelectMany(q => q.Value.Select(v => new KeyValuePair<string, string?>(q.Key, v)))
            .ToList();
    }

    public static string RemoveByValueQuery(
        string url,
        IHostEnvironment env,
        params (string Key, object Value)[] queries
    )
    {
        var uri = new Uri(url);
        var query = uri.DetermineQueryString(env, q => true);

        query = query
            .Where(q =>
                !queries.Any(x =>
                    string.Equals(q.Key, x.Key, StringComparison.OrdinalIgnoreCase)
                    && q.Value?.ToString() == x.Value.ToString()
                )
            )
            .ToList();

        return ToQueryString(env, uri, query);
    }

    public static string RemoveByKeyQuery(string url, IHostEnvironment env, params string[] keys)
    {
        var uri = new Uri(url);
        var query = uri.DetermineQueryString(
            env,
            q => !keys.Any(x => q.Key.StartsWith(x, StringComparison.OrdinalIgnoreCase))
        );

        return ToQueryString(env, uri, query);
    }

    public static string AddOrUpdateQuery(
        string url,
        IHostEnvironment env,
        params (string Key, object? Value)[] queryParams
    )
    {
        var uri = new Uri(url);
        var query = uri.DetermineQueryString(
            env,
            q => queryParams.All(x => !string.Equals(q.Key, x.Key, StringComparison.OrdinalIgnoreCase))
        );

        foreach (var item in queryParams)
        {
            if (item.Value != null)
            {
                query.Add(new KeyValuePair<string, string>(item.Key, item.Value.ToString()!)!);
            }
            else
            {
                query.RemoveAt(query.FindIndex(x => x.Key == item.Key && x.Value == (string?)item.Value));
            }
        }

        return ToQueryString(env, uri, query);
    }
}
