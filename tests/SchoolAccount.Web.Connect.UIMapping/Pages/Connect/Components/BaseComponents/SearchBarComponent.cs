using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Connect.Components.Base;

public class SearchBarComponent(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    private ILocator SearchToggleButton => PageContext.Locator("button[id='dfe-connect-search-toggle']");
    private ILocator SearchInputField => PageContext.Locator("input[id='dfe-connect-search-field']");
    private ILocator SearchSubmitButton => PageContext.Locator("button[type='submit']");

    public async Task PerformSearchAsync(string query)
    {
        await SearchToggleButton.ClickAsync();
        await SearchInputField.FillAsync(query);
        await SearchSubmitButton.ClickAsync();
    }
}