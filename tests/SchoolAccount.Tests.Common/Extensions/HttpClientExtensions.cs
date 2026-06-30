using System.Text.Json;
using AngleSharp;
using AngleSharp.Dom;
using SchoolAccount.Tests.Common.Builders;
using static SchoolAccount.Tests.Common.Fakes.SessionAuthenticationHandler;

namespace SchoolAccount.Tests.Common.Extensions;

public static class HttpClientExtensions
{
    public static HttpClient WithAuthentication(
        this HttpClient client,
        string? userId = null,
        OrganisationClaimBuilder? organisation = null
    )
    {
        var organisationClaim = (organisation ?? OrganisationClaimBuilder.Default).Build();
        var serialisedClaim = JsonSerializer.Serialize(organisationClaim, JsonSerializerOptions.Web);

        client.DefaultRequestHeaders.Add(UserIdHeader, userId ?? DefaultUserId);
        client.DefaultRequestHeaders.Add(OrganisationHeader, serialisedClaim);

        return client;
    }

    public static async Task<IDocument> ReadAsPageAsync(this Task<HttpResponseMessage> responseTask)
    {
        var response = await responseTask;
        var html = await response.Content.ReadAsStringAsync();
        var context = BrowsingContext.New(Configuration.Default);
        return (await context.OpenAsync(req => req.Content(html)))!;
    }
}
