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

    public static async Task<IDocument> ReadAsPageAsync(
        this Task<HttpResponseMessage> responseTask,
        CancellationToken cancellationToken = default
    )
    {
        var response = await responseTask;
        return await response.ReadAsPageAsync(cancellationToken);
    }

    public static async Task<IDocument> ReadAsPageAsync(
        this HttpResponseMessage response,
        CancellationToken cancellationToken = default
    )
    {
        var html = await response.Content.ReadAsStringAsync(cancellationToken);
        var context = BrowsingContext.New(Configuration.Default);
        return (await context.OpenAsync(req => req.Content(html), cancellationToken))!;
    }
}
