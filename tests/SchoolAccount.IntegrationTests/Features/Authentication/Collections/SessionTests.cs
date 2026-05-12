using SchoolAccount.IntegrationTests.Features.Authentication.Fixtures;
using Xunit;

namespace SchoolAccount.IntegrationTests.Features.Authentication.Collections;

[CollectionDefinition(CollectionName)]
public class SessionTests : ICollectionFixture<SessionFixture>
{
    public const string CollectionName = "Session Collection";
}