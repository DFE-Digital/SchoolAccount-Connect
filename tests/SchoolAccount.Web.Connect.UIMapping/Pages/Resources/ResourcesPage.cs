using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests.DfE.UIMapping.Pages.Resources;

/// <summary>
/// Page model for the Resources page.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class ResourcesPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    // Page elements
    private ILocator AddResourceButton => PageContext.Locator("button").Filter(new() { HasText = "Add resource" });
    private ILocator ResourcesTable => PageContext.Locator("table[id='sortTable']");
    private ILocator ResourcesTableRows => ResourcesTable.Locator("tbody tr");

    // Actions
    public async Task ClickAddResourceAsync()
    {
        await AddResourceButton.ClickAsync();
    }

    public async Task AssertResourceRowByNameAsync(string resourceName)
    {
        var resourceRow = ResourcesTableRows.Filter(new() { HasText = resourceName });
        await Expect(resourceRow).ToBeVisibleAsync();
    }

    public async Task ClickResourceLinkByNameAsync(string resourceName)
    {
        var resourceRow = ResourcesTableRows.Filter(new() { HasText = resourceName });
        var resourceLink = resourceRow.Locator("td").First.Locator("a");
        await resourceLink.ClickAsync();
    }
}