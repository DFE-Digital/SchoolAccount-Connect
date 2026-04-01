using SchoolAccount.IntegrationTests.Features.Database.Fixtures;
using Xunit;

namespace SchoolAccount.IntegrationTests.Features.Database.Collections;

[CollectionDefinition("Database Collection")]
public class Database : ICollectionFixture<DatabaseFixture> { }
