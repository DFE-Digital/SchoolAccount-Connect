using System.Diagnostics.CodeAnalysis;
using static SchoolAccount.Tests.Common.Fixtures.WebApplicationAccessMode;

namespace SchoolAccount.Tests.Common.Fixtures;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public class TestServerFixture : WebApplicationFixture
{
    protected override WebApplicationAccessMode DefaultAccessMode => Unauthenticated;
}
