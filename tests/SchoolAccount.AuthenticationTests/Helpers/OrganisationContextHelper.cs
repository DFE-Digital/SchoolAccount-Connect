using NSubstitute;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.AuthenticationTests.Helpers;

public static class OrganisationContextHelper
{
    public static IOrganisationContext CreateContext(bool canAccess, SchoolType schoolType, out IProvider provider)
    {
        provider = Substitute.For<IProvider>();
        provider.CanAccess(null).Returns(canAccess);

        var context = Substitute.For<IOrganisationContext>();
        context.Type.Returns(new Dictionary<string, SchoolType> { { string.Empty, schoolType } });
        context.Provider.Returns(provider);

        return context;
    }

    public static IOrganisationContext CreateContext(bool canAccess, SchoolType schoolType, IOrganisation organisation)
    {
        var provider = Substitute.For<IProvider>();
        provider.CanAccess(null).Returns(canAccess);

        var context = Substitute.For<IOrganisationContext>();
        context.Type.Returns(new Dictionary<string, SchoolType> { { organisation.Ukrpn, schoolType } });
        context.Provider.Returns(provider);
        context.Organisation.Returns(organisation);

        return context;
    }

    public static IOrganisationContext CreateSimpleOrganisationContext(
        string name = "Test School",
        bool isAuthenticated = true
    )
    {
        var context = Substitute.For<IOrganisationContext>();
        var organisation = Substitute.For<IOrganisation>();

        organisation.Name.Returns(name);
        context.Organisation.Returns(organisation);
        context.IsAuthorised().Returns(isAuthenticated);
        context.IsValid().Returns(true);
        context.IsDsiDetermined.Returns(true);

        return context;
    }
}
