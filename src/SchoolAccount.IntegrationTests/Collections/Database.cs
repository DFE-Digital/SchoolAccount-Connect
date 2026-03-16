using SchoolAccount.IntegrationTests.Fixtures;
using Xunit;

namespace SchoolAccount.IntegrationTests.Collections;

[CollectionDefinition("Database Collection")]
public class Database : ICollectionFixture<DatabaseFixture> { }
