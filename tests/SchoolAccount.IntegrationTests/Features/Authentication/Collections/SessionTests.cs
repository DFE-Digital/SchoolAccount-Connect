using SchoolAccount.Tests.Common.Fixtures;
using Xunit;

namespace SchoolAccount.IntegrationTests.Features.Authentication.Collections;

[CollectionDefinition(CollectionName)]
public class SessionTests : ICollectionFixture<TestServerFixture>
{
    public const string CollectionName = "Session Collection";
}
