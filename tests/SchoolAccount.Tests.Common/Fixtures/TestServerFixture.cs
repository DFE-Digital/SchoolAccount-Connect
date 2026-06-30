using System.Diagnostics.CodeAnalysis;
using static SchoolAccount.Tests.Common.Factories.SchoolAccountWebApplicationFactory;

namespace SchoolAccount.Tests.Common.Fixtures;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public class TestServerFixture : WebApplicationFixture
{
    public TestServerFixture()
        : base() { }

    protected TestServerFixture(
        Func<Builder, Builder>? configureAnonymous,
        Func<Builder, Builder>? configureAuthenticated
    )
        : base(configureAnonymous, configureAuthenticated) { }
}
