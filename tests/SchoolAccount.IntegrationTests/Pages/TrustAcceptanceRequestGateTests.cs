using System.Diagnostics.CodeAnalysis;
using AwesomeAssertions;
using SchoolAccount.IntegrationTests.Features.Authentication.Collections;
using SchoolAccount.Tests.Common.Builders;
using SchoolAccount.Tests.Common.Fixtures;
using SchoolAccount.Web.Connect;
using Xunit;

namespace SchoolAccount.IntegrationTests.Pages;

[Collection(SessionTests.CollectionName)]
public class TrustAcceptanceRequestGateTests(TestServerFixture fixture)
{
    [Fact]
    public async Task Ensure_if_trust_is_logged_in_that_if_navigate_to_authorised_page_they_need_to_accept_before_continuing()
    {
        var client = fixture.CreateAuthenticatedClient(
            organisation: OrganisationClaimBuilder.Trust,
            configure: options =>
            {
                options.AllowAutoRedirect = true;
            }
        );

        var response = await client.GetAsync(
            RouteConstants.Calendar.CalendarOfItems,
            TestContext.Current.CancellationToken
        );

        response.RequestMessage.Should().NotBeNull();
        response.RequestMessage.RequestUri.Should().NotBeNull();
        response.RequestMessage.RequestUri.AbsolutePath.Should().Be(RouteConstants.Start.MatAcceptance);
    }

    [Fact]
    public async Task Ensure_once_accepted_that_they_are_allowed_to_continue_their_journey()
    {
        var client = fixture.CreateAuthenticatedClient(
            organisation: OrganisationClaimBuilder.Trust,
            configure: options =>
            {
                options.AllowAutoRedirect = true;
            }
        );

        using var content = new StringContent(string.Empty);
        await client.PostAsync(RouteConstants.Start.MatAcceptance, content, TestContext.Current.CancellationToken);

        var response = await client.GetAsync(
            RouteConstants.Calendar.CalendarOfItems,
            TestContext.Current.CancellationToken
        );

        response.RequestMessage.Should().NotBeNull();
        response.RequestMessage.RequestUri.Should().NotBeNull();
        response.RequestMessage.RequestUri.AbsolutePath.Should().Be(RouteConstants.Calendar.CalendarOfItems);
    }

    [Fact]
    public async Task Ensure_once_logged_out_that_session_state_has_been_cleared()
    {
        var client = fixture.CreateAuthenticatedClient(
            organisation: OrganisationClaimBuilder.Trust,
            configure: options =>
            {
                options.AllowAutoRedirect = true;
            }
        );

        using var content = new StringContent(string.Empty);
        await client.PostAsync(RouteConstants.Start.MatAcceptance, content, TestContext.Current.CancellationToken);
        await client.GetAsync("/Account/SignOut", TestContext.Current.CancellationToken);

        // After sign out, should redirect to terms again
        var response = await client.GetAsync(
            RouteConstants.Calendar.CalendarOfItems,
            TestContext.Current.CancellationToken
        );

        response.RequestMessage.Should().NotBeNull();
        response.RequestMessage.RequestUri.Should().NotBeNull();
        response.RequestMessage.RequestUri.AbsolutePath.Should().Be(RouteConstants.Start.MatAcceptance);
    }

    [Fact]
    public async Task Ensure_if_not_trust_that_they_bypass_the_trust_acceptance_screen()
    {
        var client = fixture.CreateAuthenticatedClient(
            organisation: OrganisationClaimBuilder.Academy,
            configure: options =>
            {
                options.AllowAutoRedirect = true;
            }
        );

        var response = await client.GetAsync(
            RouteConstants.Calendar.CalendarOfItems,
            TestContext.Current.CancellationToken
        );

        response.RequestMessage.Should().NotBeNull();
        response.RequestMessage.RequestUri.Should().NotBeNull();
        response.RequestMessage.RequestUri.AbsolutePath.Should().Be(RouteConstants.Calendar.CalendarOfItems);
    }
}
