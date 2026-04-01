using AngleSharp.Dom;
using AwesomeAssertions;
using AwesomeAssertions.Primitives;

namespace SchoolAccount.IntegrationTests.Extensions;

public static class AngleSharpExtensions
{
    public static void HaveTextContent(this ObjectAssertions assertions, string expectedText)
    {
        var element = assertions.Subject as IElement;
        element.Should().NotBeNull("the element was expected to exist");
        element.TextContent.Trim().Should().Contain(expectedText);
    }

    public static void BePaginationWithLabels(this ObjectAssertions assertions, params string[] expectedLabels)
    {
        var paginationContainer = assertions.Subject as IElement;
        paginationContainer.Should().NotBeNull("the pagination container was not found");

        var labels = paginationContainer.QuerySelectorAll("a")
            .Select(a => a.TextContent.Trim());

        labels.Should().Equal(expectedLabels);
    }

    public static void HaveTabs(this ObjectAssertions assertions, params string[] expectedTabLabels)
    {
        var container = assertions.Subject as IElement;
        container.Should().NotBeNull("the tabs container was not found");

        var tabs = container.QuerySelectorAll(".moj-sub-navigation__link")
            .Select(t => t.TextContent.Trim());

        tabs.Should().BeEquivalentTo(expectedTabLabels, options => options.WithStrictOrdering(), "the tabs should match the expected labels in order");
    }

    public static void HaveSelectedTab(this ObjectAssertions assertions, string expectedLabel)
    {
        var container = assertions.Subject as IElement;
        container.Should().NotBeNull();

        var selectedTab = container.QuerySelector(".moj-sub-navigation__link[aria-current=page]");

        selectedTab.Should().NotBeNull("a tab was expected to be selected");
        selectedTab.TextContent.Trim().Should().Be(expectedLabel);
    }
}