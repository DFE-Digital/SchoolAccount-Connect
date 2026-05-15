using System.Diagnostics.CodeAnalysis;
using System.Net;
using AwesomeAssertions;
using SchoolAccount.Domain.Providers;
using SchoolAccount.IntegrationTests.Extensions;
using SchoolAccount.IntegrationTests.Features.Authentication.Collections;
using SchoolAccount.IntegrationTests.Features.Authentication.Fixtures;
using SchoolAccount.Kernel;
using SchoolAccount.Tests.Common.Builders;
using SchoolAccount.Web.Connect;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.OrganisationClaimBuilder;

namespace SchoolAccount.IntegrationTests.Pages;

[Collection(SessionTests.CollectionName)]
[SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings")]
public class FallbackProviderTests : IClassFixture<SessionFixture>
{
    private readonly SessionFixture _fixture;
    private const string EmptyUkPrn = "000001";

    public FallbackProviderTests(SessionFixture fixture)
    {
        _fixture = fixture;

        _fixture.FallbackProviderResolver.ClearProviders();
        _fixture.FallbackProviderResolver.AddProvider(EmptyUkPrn,
            new ProviderOverrideEntity
            {
                HasAccess = true,
                Id = 1,
                SchoolName = "Test",
                SchoolType = SchoolType.Academy,
                UkPrn = EmptyUkPrn
            });
    }
    
    [Fact]
    public async Task Ensure_that_passing_a_fallback_organisation_resolver_authenticates()
    {
        var client = _fixture.CreateAuthenticatedClient(organisation: AOrganisationClaim().WithUkprn(EmptyUkPrn));

        var response = await client.GetAsync(RouteConstants.Support, TestContext.Current.CancellationToken);

        response.IsSuccessStatusCode.Should().BeTrue();
        
        var page = await response.GetPage();
        
        page.Should().NotBeNull();
        page.Title.Should().StartWith("Support");
    }
    
    [Fact]
    public async Task Ensure_that_if_no_fallback_organisation_is_found_it_will_fail_authentication()
    {
        var client = _fixture.CreateAuthenticatedClient(organisation: AOrganisationClaim().WithUkprn("000002"));

        var response = await client.GetAsync(RouteConstants.Support, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
        var page = await response.GetPage();
        
        page.Should().NotBeNull();
        page.Title.Should().StartWith("Service Inaccessible");
    }
}