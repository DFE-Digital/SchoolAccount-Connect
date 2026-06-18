using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests.DfE.UIMapping.Pages.Services;

/// <summary>
/// Page model for the Services page.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class ServicesPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    // Page elements
    public ILocator AddServiceButton => PageContext.Locator("button").Filter(new() { HasText = "Add a service" });
    public ILocator SearchEntry => PageContext.Locator("input[id='searchInput']");
    public ILocator SearchSubmit => PageContext.Locator("input[id='searchsubmit']");
    public ILocator ShowFiltersLink => PageContext.Locator("a").Filter(new() { HasText = "Show filters" });
    public ILocator ServicesList => PageContext.Locator("div[id='services-list']");
    public ILocator ServicesTable => ServicesList.Locator("table");
    public ILocator ServicesTableRows => ServicesTable.Locator("tbody tr");

    // Actions
    public async Task ClickAddServiceAsync()
    {
        await AddServiceButton.ClickAsync();
    }

    public async Task ClickSearchSubmitAsync()
    {
        await SearchSubmit.ClickAsync();
    }

    public async Task ClickShowFiltersAsync()
    {
        await ShowFiltersLink.ClickAsync();
    }

    public async Task<ILocator> FindServiceRowByNameAsync(string serviceName)
    {
        // Wait for at least one row to be present in the table
        await PageContext.WaitForSelectorAsync("div[id='services-list'] table tbody tr");
        var rows = ServicesTableRows;
        var rowCount = await rows.CountAsync();
        
        for (int i = 0; i < rowCount; i++)
        {
            var row = rows.Nth(i);
            var firstCell = row.Locator("td").First;
            var cellText = await firstCell.TextContentAsync();
            
            if (cellText != null && cellText.Contains(serviceName, StringComparison.OrdinalIgnoreCase))
            {
                return firstCell.Locator("a");
            }
        }
        
        throw new InvalidOperationException($"Service link with name containing '{serviceName}' not found");
    }

    public async Task ClickServiceByNameAsync(string serviceName)
    {
        var serviceLink = await FindServiceRowByNameAsync(serviceName);
        await serviceLink.ClickAsync();
    }

    public async Task AssertServiceRowByNameAsync(string serviceName)
    {
        var serviceLink = await FindServiceRowByNameAsync(serviceName);
        await Expect(serviceLink).ToBeVisibleAsync();
    }
}