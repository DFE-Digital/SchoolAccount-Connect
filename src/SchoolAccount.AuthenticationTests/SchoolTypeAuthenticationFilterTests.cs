using AwesomeAssertions;
using Microsoft.AspNetCore.Mvc;
using SchoolAccount.AuthenticationTests.Helpers;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Authentication.Filters;
using Xunit;

namespace SchoolAccount.AuthenticationTests;

public class SchoolTypeAuthenticationFilterTests
{
    [Fact]
    public async Task ShouldForbidWhenSchoolTypeNotAllowed()
    {
        // Arrange
        var context = AuthorizationFilterContextHelper.CreateContext(true);
        var organisationContext = OrganisationContextHelper.CreateContext(true, SchoolType.AcademySpecial, out _);
        var filter = new SchoolTypeAuthorisationFilter(organisationContext, [SchoolType.Academy]);
        
        // Act
        await filter.OnAuthorizationAsync(context);

        // Assert
        context.Result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task ShouldSucceedWhenSchoolTypeAllowed()
    {
        // Arrange
        var context = AuthorizationFilterContextHelper.CreateContext(true);
        var organisationContext = OrganisationContextHelper.CreateContext(true, SchoolType.Academy, out _);
        var filter = new SchoolTypeAuthorisationFilter(organisationContext, [SchoolType.Academy]);

        // Act
        await filter.OnAuthorizationAsync(context);

        // Assert
        context.Result.Should().BeNull();
    }
}