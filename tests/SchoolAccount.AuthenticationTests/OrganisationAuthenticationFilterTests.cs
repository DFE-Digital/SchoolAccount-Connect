using AwesomeAssertions;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.AuthenticationTests.Helpers;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;
using SchoolAccount.Web.Connect.Authentication.Filters;
using Xunit;

namespace SchoolAccount.AuthenticationTests;

public class OrganisationAuthenticationFilterTests
{
    [Fact]
    public async Task Should_forbid_when_organisation_not_allowed()
    {
        // Arrange
        var context = AuthorizationFilterContextHelper.CreateContext(true);
        var organisation = new AcademyOrganisation("0000000", "Test Academy");
        var organisationContext = OrganisationContextHelper.CreateContext(true, SchoolType.AcademySpecial, organisation);
        var filter = new OrganisationTypeAuthorisationFilter(organisationContext, [typeof(TrustOrganisation)]);

        // Act
        await filter.OnAuthorizationAsync(context);

        // Assert
        context.Result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task Should_succeed_when_organisation_allowed()
    {
        // Arrange
        var context = AuthorizationFilterContextHelper.CreateContext(true);
        var organisation = new AcademyOrganisation("0000000", "Test Academy");
        var organisationContext = OrganisationContextHelper.CreateContext(true, SchoolType.Academy, organisation);
        var filter = new OrganisationTypeAuthorisationFilter(organisationContext, [typeof(AcademyOrganisation)]);

        // Act
        await filter.OnAuthorizationAsync(context);

        // Assert
        context.Result.Should().BeNull();
    }

    [Fact]
    public async Task Should_cause_exception_if_wrong_type_is_provided()
    {
        // Arrange
        var context = AuthorizationFilterContextHelper.CreateContext(true);
        var organisation = new AcademyOrganisation("0000000", "Test Academy");
        var organisationContext = OrganisationContextHelper.CreateContext(true, SchoolType.Academy, organisation);
        
        // Act 
        var filter = () => new OrganisationTypeAuthorisationFilter(organisationContext, [typeof(PreSixteenProvider)]);

        // Assert
        filter.Should().Throw<ArgumentException>()
            .WithParameterName("allowedOrganisations");
    }
}
