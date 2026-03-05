using SchoolAccount.IntegrationTests.Factory;
using SchoolAccount.Web.Connect;
using Xunit;

namespace SchoolAccount.IntegrationTests.Fixtures;

#pragma warning disable CA1001
public class DatabaseFixture : IAsyncLifetime
#pragma warning restore CA1001
{
    private readonly SchoolAccountWebApplicationFactory<Program> _factory;
    public HttpClient Client { get; }
    
    public DatabaseFixture()
    {
        _factory = new SchoolAccountWebApplicationFactory<Program>();
        Client = _factory.CreateClient();
    }
    
    public async Task InitializeAsync()
    {
        await _factory.ResetDatabaseAsync();
    }
    
    public async Task ResetDatabaseAsync()
    {
        await _factory.ResetDatabaseAsync();
    }

    public async Task DisposeAsync()
    {
        await _factory.GetDbContext().Database.EnsureDeletedAsync();
    }
}