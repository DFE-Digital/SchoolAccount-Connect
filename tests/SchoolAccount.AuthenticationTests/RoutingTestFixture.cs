using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Tests.Common.Fixtures;

namespace SchoolAccount.AuthenticationTests;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public class RoutingTestFixture()
    : TestServerFixture(
        configureAnonymous: b => b.WithInMemoryDatabase(),
        configureAuthenticated: b => b.WithInMemoryDatabase()
    );
