using AwesomeAssertions;
using Microsoft.AspNetCore.Authorization;
using NSubstitute;
using SchoolAccount.AuthenticationTests.Helpers;
using SchoolAccount.Integration.DfESignIn.Authentication;
using SchoolAccount.Integration.DfESignIn.Exceptions;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Integration.DfESignIn.Requirements;
using SchoolAccount.Kernel;
using Xunit;

namespace SchoolAccount.AuthenticationTests;

public class ProviderAuthorisationHandlerTests
{
    private static (ProviderAuthorisationHandler, AuthorizationHandlerContext) CreateContext(IOrganisationContext organisationContext)
    {
        var handler = new ProviderAuthorisationHandler(organisationContext);
        var requirement = new ProviderRequirement();
        var user = ClaimsPrincipalHelper.CreateUser();
        var context = new AuthorizationHandlerContext([requirement], user, null);
        
        return (handler, context);
    }
    
    [Fact]
    public async Task ShouldFailWhenNoProvider()
    {
        // Arrange
        var organisationContext = Substitute.For<IOrganisationContext>();
        organisationContext.Provider.Returns(NullProvider.Default);
        var (handler, context) = CreateContext(organisationContext);

        // Act
        var act = async () => await handler.HandleAsync(context);

        // Assert
        await act.Should().ThrowAsync<NoProviderException>();
    }

    [Fact]
    public async Task ShouldSucceedWhenProviderExistsAndCanAccess()
    {
        // Arrange
        var organisationContext = OrganisationContextHelper.CreateContext(true, SchoolType.Unknown, out _);
        var (handler, context) = CreateContext(organisationContext);

        // Act
        await handler.HandleAsync(context);

        // Assert
        context.HasSucceeded.Should().BeTrue();
    }
}