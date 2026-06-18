using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;

namespace PlaywrightTests.DfE.UIMapping.Pages.Resources;

/// <summary>
/// Page model for the final confirmation page before adding a resource.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class AddResourceConfirmationPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    private ILocator SubmitButton => PageContext.Locator("button[type='submit']");

    public async Task ClickSubmitButtonAsync()
    {
        await SubmitButton.ClickAsync();
    }
}
