using System.Net;
using AwesomeAssertions;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fixtures;
using Xunit;

namespace SchoolAccount.IntegrationTests.Pages;

public class ErrorPageTests : IClassFixture<TestServerFixture>
{
    private readonly HttpClient _client;

    public ErrorPageTests(TestServerFixture fixture, ITestOutputHelper outputHelper)
    {
        fixture.SetOutputHelper(outputHelper);
        fixture.HandlerRegistry.Clear();
        _client = fixture.CreateAnonymousClient();
    }

    [Fact]
    public async Task Unknown_route_returns_not_found_page()
    {
        // Act
        var response = await _client.GetAsync("/this-page-does-not-exist", TestContext.Current.CancellationToken);
        var page = await Task.FromResult(response).ReadAsPageAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        page.QuerySelector("body").Should().NotBeNull().And.HaveTextContent("Page not found");
    }
}
