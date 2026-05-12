using AngleSharp;
using AngleSharp.Dom;

namespace SchoolAccount.IntegrationTests.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<IDocument?> GetPage(this HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        var html = await response.Content.ReadAsStringAsync();
        var context = BrowsingContext.New(Configuration.Default);
        return await context.OpenAsync(req => req.Content(html));
    }
}