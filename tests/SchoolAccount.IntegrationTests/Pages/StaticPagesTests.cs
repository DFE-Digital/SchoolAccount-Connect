using AngleSharp.Dom;
using AwesomeAssertions;
using SchoolAccount.Tests.Common.Extensions;
using SchoolAccount.Tests.Common.Fixtures;
using Xunit;

namespace SchoolAccount.IntegrationTests.Pages;

public class StaticPagesTests : IClassFixture<TestServerFixture>
{
    private readonly HttpClient _client;

    public StaticPagesTests(TestServerFixture fixture, ITestOutputHelper outputHelper)
    {
        fixture.SetOutputHelper(outputHelper);
        fixture.HandlerRegistry.Clear();
        _client = fixture.CreateAnonymousClient();
    }

    [Theory]
    [InlineData("/support", "Support")]
    [InlineData("/about", "About this application")]
    [InlineData("/cookies", "Cookies")]
    public async Task Static_page_returns_success_with_expected_heading(string url, string expectedHeading)
    {
        // Act
        var response = await _client.GetAsync(url, TestContext.Current.CancellationToken);
        var page = await Task.FromResult(response).ReadAsPageAsync(TestContext.Current.CancellationToken);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue($"the {url} page should render successfully");
        page.QuerySelector(".govuk-heading-l").Should().NotBeNull().And.HaveTextContent(expectedHeading);
        page.QuerySelector("body")
            .Should()
            .Match<IElement>(e => !e.TextContent.Contains("Sorry, there is a problem with the service"));
    }
}
