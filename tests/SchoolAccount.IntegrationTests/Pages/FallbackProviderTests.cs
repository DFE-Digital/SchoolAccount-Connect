using System.Diagnostics.CodeAnalysis;
using AwesomeAssertions;
using SchoolAccount.Domain.Providers;
using SchoolAccount.IntegrationTests.Features.Authentication.Collections;
using SchoolAccount.Kernel;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fixtures;
using SchoolAccount.Web.Connect;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.OrganisationClaimBuilder;

namespace SchoolAccount.IntegrationTests.Pages;

[Collection(SessionTests.CollectionName)]
[SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings")]
public class FallbackProviderTests
{
    private readonly TestServerFixture _fixture;
    private const string EmptyUkPrn = "000001";

    public FallbackProviderTests(TestServerFixture fixture)
    {
        _fixture = fixture;

        _fixture.FallbackProviderResolver.ClearProviders();
        _fixture.FallbackProviderResolver.AddProvider(
            EmptyUkPrn,
            new ProviderOverrideEntity
            {
                HasAccess = true,
                Id = 1,
                SchoolName = "Test",
                SchoolType = SchoolType.Academy,
                UkPrn = EmptyUkPrn,
            }
        );
    }

    [Fact]
    public async Task Ensure_that_passing_a_fallback_organisation_resolver_authenticates()
    {
        var client = _fixture.CreateAuthenticatedClient(organisation: AOrganisationClaim().WithUkprn(EmptyUkPrn));

        var page = await client
            .GetAsync(RouteConstants.Support, TestContext.Current.CancellationToken)
            .ReadAsPageAsync();

        page.Should().NotBeNull();
        page.Title.Should().StartWith("Support");
    }

    [Fact]
    public async Task Ensure_that_if_no_fallback_organisation_is_found_it_will_fail_authentication()
    {
        var client = _fixture.CreateAuthenticatedClient(organisation: AOrganisationClaim().WithUkprn("000002"));

        var page = await client
            .GetAsync(RouteConstants.Support, TestContext.Current.CancellationToken)
            .ReadAsPageAsync();

        page.Should().NotBeNull();
        page.Title.Should().StartWith("Service Inaccessible");
    }
}
