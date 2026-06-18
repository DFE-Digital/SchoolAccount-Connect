using Microsoft.Playwright;
using PlaywrightTests.Kernel.Pages;
using PlaywrightTests.Kernel.TestFixtures;
using PlaywrightTests.Kernel.Utils;

namespace PlaywrightTests.DfE.UIMapping.Pages.Tasks;

/// <summary>
/// Page model allowing the task to be published.
/// </summary>
/// <param name="page">The current Playwright page context.</param>
/// <param name="runContext">The current internal test run context.</param>
public class PublishTaskToDfEConnectPage(IPage page, IRunContext runContext) : BasePage(page, runContext)
{
    public ILocator PublishNowRadioButton => PageContext.InputLocatorById("now-date");
    private ILocator ContinueButton => PageContext.ButtonLocatorByType("submit");

    public async Task<PublishTaskDetailsConfirmPage> ClickContinueAsync()
    {
        await ContinueButton.ClickAsync();

        return new PublishTaskDetailsConfirmPage(PageContext, RunContext);
    }
}