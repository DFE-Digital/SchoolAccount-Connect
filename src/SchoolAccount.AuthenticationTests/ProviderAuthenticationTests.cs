using AwesomeAssertions;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.AuthenticationTests.Helpers;
using SchoolAccount.Integration.DfESignIn.Filters;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Kernel;
using Xunit;

namespace SchoolAccount.AuthenticationTests;

public class ProviderAuthorizationFilterTests
{
    [Fact]
    public async Task ShouldForbidWhenProviderNotAllowed()
    {
        // Arrange
        var authorisationContext = AuthorizationFilterContextHelper.CreateContext(true);
        var organisationContext = OrganisationContextHelper.CreateContext(false, SchoolType.Unknown, out _);
        var filter = new ProviderAuthorisationFilter(organisationContext, [typeof(FreeSchoolProvider)]);

        // Act
        await filter.OnAuthorizationAsync(authorisationContext);

        // Assert
        authorisationContext.Result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task ShouldForbidWhenCanAccessIsFalse()
    {
        // Arrange
        var authorisationContext = AuthorizationFilterContextHelper.CreateContext(true);
        var organisationContext = OrganisationContextHelper.CreateContext(false, SchoolType.Unknown, out var provider);
        var filter = new ProviderAuthorisationFilter(organisationContext, [provider.GetType()]);

        // Act
        await filter.OnAuthorizationAsync(authorisationContext);

        // Assert
        authorisationContext.Result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task ShouldSucceedWhenProviderAllowedAndCanAccessTrue()
    {
        // Arrange
        var authorisationContext = AuthorizationFilterContextHelper.CreateContext(true);
        var organisationContext = OrganisationContextHelper.CreateContext(true, SchoolType.Academy, out var provider);
        var filter = new ProviderAuthorisationFilter(organisationContext, [provider.GetType()]);

        // Act
        await filter.OnAuthorizationAsync(authorisationContext);

        // Assert
        authorisationContext.Result.Should().BeNull();
    }
}
