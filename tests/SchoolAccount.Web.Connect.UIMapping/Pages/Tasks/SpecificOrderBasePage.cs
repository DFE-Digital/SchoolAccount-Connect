using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.Kernel.Utils;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model allowing entities to be explicitly ordered.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public abstract class SpecificOrderBasePage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    private ILocator ConfirmOrderButton => PageContext.ButtonLocatorByText("Confirm order");
    private ILocator BackButton => PageContext.Locator("a.govuk-back-link");
    private ILocator CancelButton => PageContext.Locator("a").Filter(new() { HasText = "Cancel" });
    private ILocator MoveDownButton => PageContext.Locator("a").Filter(new() { HasText = "Move Down" });

    public async Task ClickConfirmOrderAsync()
    {
        await ClickAsync(ConfirmOrderButton);
    }

    public async Task ClickBackAsync()
    {
        await ClickAsync(BackButton);
    }

    public async Task ClickCancelAsync()
    {
        await ClickAsync(CancelButton);
    }

    public async Task ClickMoveDownAsync()
    {
        await ClickAsync(MoveDownButton);
    }
}
