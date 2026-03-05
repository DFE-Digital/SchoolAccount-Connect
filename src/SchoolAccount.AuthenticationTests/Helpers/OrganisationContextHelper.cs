using NSubstitute;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.AuthenticationTests.Helpers;

public static class OrganisationContextHelper
{
    public static IOrganisationContext CreateContext(bool canAccess, SchoolType schoolType, out IProvider provider)
    {
        provider = Substitute.For<IProvider>();
        provider.CanAccess().Returns(canAccess);
        
        var context = Substitute.For<IOrganisationContext>();
        context.Type.Returns(schoolType);
        context.Provider.Returns(provider);

        return context;
    }
}