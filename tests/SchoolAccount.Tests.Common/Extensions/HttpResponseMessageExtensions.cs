using AngleSharp;
using AngleSharp.Dom;

namespace SchoolAccount.Tests.Common.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<IDocument?> GetPage(this HttpResponseMessage response, CancellationToken ct = default)
    {
        var html = await response.Content.ReadAsStringAsync(ct);
        var context = BrowsingContext.New(Configuration.Default);
        return await context.OpenAsync(req => req.Content(html), ct);
    }
}
