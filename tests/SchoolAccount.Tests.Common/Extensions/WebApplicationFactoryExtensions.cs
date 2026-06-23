using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SchoolAccount.Tests.Common.Extensions;

public static class WebApplicationFactoryExtensions
{
    public static async Task<IDocument> RequestPageAsync<T>(
        this WebApplicationFactory<T> factory,
        string uri,
        WebApplicationFactoryClientOptions? options = null
    )
        where T : class
    {
        var client = factory.CreateClient(options ?? factory.ClientOptions);
        var requestUri = new Uri(uri, UriKind.Relative);
        var response = await client.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
        {
            var errorHtml = await response.Content.ReadAsStringAsync();
            var context = BrowsingContext.New(Configuration.Default);
            var doc = await context.OpenAsync(req => req.Content(errorHtml));
            var formatted = doc.ToHtml(new PrettyMarkupFormatter());
            throw new HttpRequestException(
                $"Request to {uri} failed with {response.StatusCode}.\nResponse Content:\n{formatted}"
            );
        }

        return (await response.GetPage())!;
    }
}
