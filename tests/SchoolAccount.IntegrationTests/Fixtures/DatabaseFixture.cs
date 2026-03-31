using SchoolAccount.IntegrationTests.Factory;
using Xunit;

namespace SchoolAccount.IntegrationTests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
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

    async ValueTask IAsyncLifetime.InitializeAsync()
    {
        await Task.Delay(1);
    }

    ValueTask IAsyncDisposable.DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return _factory.DisposeAsync();
    }
}
