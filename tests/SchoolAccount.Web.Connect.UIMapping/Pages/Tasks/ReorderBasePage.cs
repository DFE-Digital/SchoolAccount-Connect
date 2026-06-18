using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.Kernel.Utils;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model allowing items to be reordered.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public abstract class ReorderBasePage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    private ILocator AlphabeticalRadioButton => PageContext.InputLocatorById("alphabetical");
    private ILocator SpecificOrderRadioButton => PageContext.InputLocatorById("specific");
    private ILocator ContinueButton => PageContext.ButtonLocatorByText("Continue");
    private ILocator BackButton => PageContext.Locator("a.govuk-back-link");
    private ILocator CancelButton => PageContext.Locator("a").Filter(new() { HasText = "Cancel" });

    public async Task ClickContinueAsync()
    {
        await ClickAsync(ContinueButton);
    }

    public async Task ClickBackAsync()
    {
        await ClickAsync(BackButton);
    }

    public async Task ClickCancelAsync()
    {
        await ClickAsync(CancelButton);
    }

    public async Task ClickAlphabeticalAsync()
    {
        await AlphabeticalRadioButton.ClickAsync();
    }

    public async Task ClickSpecificAsync()
    {
        await SpecificOrderRadioButton.ClickAsync();
    }
}
