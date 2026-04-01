using SchoolAccount.IntegrationTests.Fixtures;
using SchoolAccount.IntegrationTests.Helpers;
using Xunit;

namespace SchoolAccount.IntegrationTests.Pages;

[Collection("Database Collection")]
public class HomeControllerTests(DatabaseFixture databaseFixture)
{
    [Fact]
    [Trait("Support", "HomeController")]
    public async Task SupportPageShouldReturn404WhenNotAuthenticated()
    {
        // Arrange
        //todo disable some rules for test project
        using var request = BuildRequestMessage(HttpMethod.Get, "/Support", false);

        // Act
        var response = await databaseFixture.Client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(Skip = "Authentication changes break these tests")]
    [Trait("Support", "HomeController")]
    public async Task SupportPageShouldReturnView()
    {
        // Arrange
        //todo disable some rules for test project
        using var request = BuildRequestMessage(HttpMethod.Get, "/Support");

        // Act
        var response = await databaseFixture.Client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var supportHtml = await HtmlHelpers.GetDocumentAsync(response);

        var supportSection = supportHtml.QuerySelector("#support-section");

        var guidanceSection = supportHtml.QuerySelector("#data-and-insights-section");

        Assert.NotNull(supportSection);
        Assert.NotNull(guidanceSection);
    }

    [Fact(Skip = "Authentication changes break these tests")]
    [Trait("Index", "HomeController")]
    public async Task ShouldRedirectToLoginWhenNotAuthenticated()
    {
        // Arrange
        using var request = BuildRequestMessage(HttpMethod.Get, "/", false);

        // Act
        var response = await databaseFixture.Client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);

        var loginHtml = await HtmlHelpers.GetDocumentAsync(response);

        var loginButton = loginHtml.QuerySelector("#login-button");

        Assert.NotNull(loginButton);
    }

    [Fact(Skip = "Authentication changes break these tests")]
    [Trait("Index", "HomeController")]
    public async Task ShouldRedirectToDashboardWhenAuthenticated()
    {
        // Arrange
        using var request = BuildRequestMessage(HttpMethod.Get, "/");

        // Act
        var response = await databaseFixture.Client.SendAsync(request, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

        var landingHtml = await HtmlHelpers.GetDocumentAsync(response);

        var welcomeTextElement = landingHtml.QuerySelector("h1");

        Assert.NotNull(welcomeTextElement);
        Assert.Equal("Welcome to DfE Connect", welcomeTextElement.InnerHtml);
    }

    private static HttpRequestMessage BuildRequestMessage(HttpMethod method, string url, bool addAuthentication = true)
    {
        var request = new HttpRequestMessage(method, url);

        if (addAuthentication)
        {
            request.Headers.Add("X-User", "test.user");
        }

        return request;
    }
}
