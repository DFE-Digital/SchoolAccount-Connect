using System.Net;
using AwesomeAssertions;
using SchoolAccount.IntegrationTests.Data;
using SchoolAccount.IntegrationTests.Features.Authentication.Collections;
using SchoolAccount.Tests.Common.Builders;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fixtures;
using Xunit;

namespace SchoolAccount.IntegrationTests.Pages;

[Collection(SessionTests.CollectionName)]
public class IsLocalEndpointArgumentCheckTests(TestServerFixture fixture)
{
    const string ExternalEvilUrl = "https://evil-malicious-site.com";

    [Theory]
    [MemberData(nameof(EndpointEmulatedData.GetEndpointsWithUrlParameters), MemberType = typeof(EndpointEmulatedData))]
    public async Task Ensure_any_returnUrl_style_endpoints_only_allow_localised_urls(
        string routePattern,
        string paramName
    )
    {
        // Arrange
        var client = fixture.CreateAuthenticatedClient(
            organisation: OrganisationClaimBuilder.Academy,
            configure: options =>
            {
                options.AllowAutoRedirect = false;
            }
        );

        var requestUri = $"{routePattern}?{paramName}={Uri.EscapeDataString(ExternalEvilUrl)}";

        // Act
        var response = await client.GetAsync(requestUri, TestContext.Current.CancellationToken);

        // Assert
        if (response.Headers.Location is not null)
        {
            var location = response.Headers.Location.ToString();
            location
                .Should()
                .NotStartWith(ExternalEvilUrl, because: $"Endpoint '{routePattern}' should prevent open redirects");
        }
        else
        {
            var content = await response.ReadAsPageAsync(TestContext.Current.CancellationToken);
            var exception = content.GetExceptionMessage();
            response.StatusCode.Should().NotBe(HttpStatusCode.InternalServerError);
        }
    }
}
