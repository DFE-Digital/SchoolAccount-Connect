using AngleSharp.Dom;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SchoolAccount.IntegrationTests.Extensions;

internal static class WebApplicationFactoryExtensions
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
            throw new HttpRequestException(
                $"Request to {uri} failed with {response.StatusCode}.\nResponse Content:\n{errorHtml}"
            );
        }

        return (await response.GetPage())!;
    }
}
